using S7.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using VisualInsectionSystem.SubForms;

public enum PLCDataType
{
    DWord = 0,
    Bit = 1,
    Word = 2,
    Byte = 3,
    Real = 4,
    String = 5
}

public class PLCAddress
{
    public string Name { get; set; }
    public PLCDataType DataType { get; set; }
    public int DBNumber { get; set; }
    public int Start { get; set; }
    public int Index { get; set; }
}

public class PLCCommunicator
{

    private Plc _plc;              // 通信实例
    private  CpuType _cpuType;     //= CpuType.S71200; // PLC的CPU类型
    private string _ipAddress;     // PLC的IP地址
    private  int rack;            // PLC的机架号
    private  int _slot;            // PLC的插槽号
    private List<PLCAddress> addressList;
    
    // 原私有字段（假设原本是 private 或 protected）
    private bool isConnected;

    // 新增公开属性，供外部访问连接状态
    public bool IsConnected
    {
        get { return isConnected; } // 只暴露读取权限，不允许外部修改
    }

    // 连接状态变更事件（供UI层订阅）
    public event Action<bool> ConnectionStatusChanged;

    // 数据读写异常事件（仅传递硬件相关错误）
    public event Action<string> HardwareErrorOccurred;

    // 与HKcamera交互的事件
    public event Action<bool> CameraStatusChanged;

    public string PlcIPAddress { get; private set; }

    private int _rack;

    public PLCCommunicator(CpuType cpuType,string ipAddress,int rack=0,int slot=1 )
    {
        // 参数校验（避免非硬件错误）
        if (!IPAddress.TryParse(ipAddress, out _))
            throw new ArgumentException("无效的IP地址格式", nameof(ipAddress));
        if (rack < 0)
            throw new ArgumentException("机架号不能为负数", nameof(rack));
        if (slot < 0)
            throw new ArgumentException("插槽号不能为负数", nameof(slot));

        _cpuType = cpuType;
        _ipAddress = ipAddress;
        _rack = rack;
        _slot = slot;
        _plc = new Plc(_cpuType, _ipAddress, (short)_rack, (short)_slot);
        InitializePLC();               // 初始化PLC连接
        //BuildDefaultAddresses();        // 构建默认地址列表
    }
    

    // 从app.config读取IP
    private void LoadConfiguration()
    {
        try
        {
            // 从app.config读取配置
            PlcIPAddress = ConfigurationManager.AppSettings["PLCIPAddress"];
            _rack = int.TryParse(ConfigurationManager.AppSettings["PLCRack"], out int r) ? r : 0;
            _slot = int.TryParse(ConfigurationManager.AppSettings["PLCSlot"], out int s) ? s : 1;

            _ipAddress = string.IsNullOrWhiteSpace(PlcIPAddress)
                ? "172.52.51.2"  // 默认IP地址
                : PlcIPAddress;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"配置加载错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // 更新连接信息
    public void UpdateConnection(string ip, int rack = 0, int slot = 1)
    {
        try
        {
            DisconnectPLC();

            _ipAddress = ip;
            _rack = rack;
            _slot = slot;
            this.PlcIPAddress = ip;

            InitializePLC();
            ConnectPLC();

            // 主窗口连接状态更新
            //MainForm.Instance?.UpdatePLCConnectionStatus(isConnected);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"连接更新错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void InitializePLC()
    {
        if(_plc == null)
        {
            _plc = new Plc(_cpuType, _ipAddress, (short)_rack, (short)_slot);
        }
        
        _plc.ReadTimeout = 5000;
        _plc.WriteTimeout = 5000;
    }

    // 连接PLC
    public bool ConnectPLC()
    {
        try
        {
            if (isConnected) return true;  // 已连接则直接返回
            if (_plc == null) InitializePLC();
            
            // 执行连接（S7NetPlus的Open方法返回连接状态）
            _plc.Open();
            isConnected = _plc.IsConnected;
            if (isConnected)
            {
                ConnectionStatusChanged?.Invoke(true); // 通知连接成功
                return true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"连接异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            isConnected = false;
            return false;
        }
        return false; // 保证所有路径都有返回值
    }

    public void DisconnectPLC()
    {
        try
        {
            if (!isConnected || _plc == null) return;
            _plc.Close();
            isConnected = false;
            CameraStatusChanged?.Invoke(false);     // 断开连接时通知相机状态为假

        }
        catch (Exception ex)
        {
            MessageBox.Show($"断开连接异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    // 检查相机状态
    private void CheckCameraStatus()
    {
        if (!isConnected) return;
        if (Read("CameraAlive", out object cameraAlive))
        {
            bool isAlive = (bool)cameraAlive;
            CameraStatusChanged?.Invoke(isAlive);
            //HKCamera.Instance?.SetCameraStatus(isAlive);
        }
    }

    // 构建默认地址列表
    private void BuildDefaultAddresses()
    {
        AddAddress("CameraAlive", PLCDataType.Bit, 45, 0, 3);  
        AddAddress("Cameracomplete", PLCDataType.Bit, 45, 0, 2);
        AddAddress("DataOK", PLCDataType.Bit, 45, 0, 1);
        AddAddress("DataNG", PLCDataType.Bit, 45, 0, 0);

    }
    // 添加地址
    public void AddAddress(string name, PLCDataType dataType, int dbNumber, int start, int index = 0)
    {
        addressList.Add(new PLCAddress
        {
            Name = name,
            DataType = dataType,
            DBNumber = dbNumber,
            Start = start,
            Index = index
        });
    }

    // 获取地址
    public PLCAddress GetAddress(string name)
    {
        return addressList.Find(addr => addr.Name == name);
    }

    // 
    public PLCAddress GetAddress(int index)
    {
        if (index >= 0 && index < addressList.Count)
            return addressList[index];
        return null;
    }

    #region Read Methods读取方法

    public bool Read(string name, out object value)
    {
        value = null;
        var address = GetAddress(name);
        if (address == null || !isConnected) return false;

        return ReadAddress(address, out value);
    }

    public bool Read(int index, out object value)
    {
        value = null;
        var address = GetAddress(index);
        if (address == null || !isConnected) return false;

        return ReadAddress(address, out value);
    }

    private bool ReadAddress(PLCAddress address, out object value)
    {
        value = null;
        try
        {
            switch (address.DataType)
            {
                case PLCDataType.DWord:
                    var dwordResult = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.DWord, 1);
                    if (dwordResult != null)
                    {
                        value = Convert.ToUInt32(dwordResult);
                        return true;
                    }
                    break;

                case PLCDataType.Bit:
                    var bitResult = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.Bit, 1);
                    if (bitResult != null)
                    {
                        // 对于位操作，使用Index作为位索引
                        byte byteValue = Convert.ToByte(bitResult);
                        value = (byteValue & (1 << address.Index)) != 0;
                        return true;
                    }
                    break;

                case PLCDataType.Word:
                    var wordResult = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.Word, 1);
                    if (wordResult != null)
                    {
                        value = Convert.ToUInt16(wordResult);
                        return true;
                    }
                    break;

                case PLCDataType.Byte:
                    var byteResult = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.Byte, 1);
                    if (byteResult != null)
                    {
                        value = Convert.ToByte(byteResult);
                        return true;
                    }
                    break;

                case PLCDataType.Real:
                    var realResult = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.Real, 1);
                    if (realResult != null)
                    {
                        value = Convert.ToSingle(realResult);
                        return true;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"读取数据错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
    }

    public bool ReadString(string name, out string value)
    {
        value = string.Empty;
        var address = GetAddress(name);
        if (address == null || !isConnected) return false;

        return ReadStringAddress(address, out value);
    }

    public bool ReadString(int index, out string value)
    {
        value = string.Empty;
        var address = GetAddress(index);
        if (address == null || !isConnected) return false;

        return ReadStringAddress(address, out value);
    }

    private bool ReadStringAddress(PLCAddress address, out string value)
    {
        value = string.Empty;
        try
        {
            // 字符串读取 - Index作为字符串长度
            var result = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.String, address.Index + 2); // +2 for header
            if (result != null)
            {
                value = result.ToString();
                return true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"读取字符串错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
    }

    #endregion

    #region Write Methods 写入方法

    public bool Write(string name, object value)
    {
        var address = GetAddress(name);
        if (address == null || !isConnected) return false;

        return WriteAddress(address, value);
    }

    public bool Write(int index, object value)
    {
        var address = GetAddress(index);
        if (address == null || !isConnected) return false;

        return WriteAddress(address, value);
    }

    private bool WriteAddress(PLCAddress address, object value)
    {
        try
        {
            switch (address.DataType)
            {
                case PLCDataType.DWord:
                    // 先调用Write（void），再返回true表示成功
                    _plc.Write(DataType.DataBlock, address.DBNumber, address.Start, Convert.ToUInt32(value));
                    return true;

                case PLCDataType.Bit:
                    // 对于位操作，需要先读取字节，修改特定位，然后写回
                    var currentByte = _plc.Read(DataType.DataBlock, address.DBNumber, address.Start, VarType.Byte, 1);
                    if (currentByte != null)
                    {
                        byte byteValue = Convert.ToByte(currentByte);
                        bool bitValue = Convert.ToBoolean(value);

                        if (bitValue)
                            byteValue |= (byte)(1 << address.Index);
                        else
                            byteValue &= (byte)~(1 << address.Index);

                        _plc.Write(DataType.DataBlock, address.DBNumber, address.Start, byteValue);
                        return true;
                    }
                    break;

                case PLCDataType.Word:
                    _plc.Write(DataType.DataBlock, address.DBNumber, address.Start, Convert.ToUInt16(value));
                    return true;

                case PLCDataType.Byte:
                    _plc.Write(DataType.DataBlock, address.DBNumber, address.Start, Convert.ToByte(value));
                    return true;

                case PLCDataType.Real:
                    _plc.Write(DataType.DataBlock, address.DBNumber, address.Start, Convert.ToSingle(value));
                    return true;

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"写入数据错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
    }

    public bool WriteString(string name, string value)
    {
        var address = GetAddress(name);
        if (address == null || !isConnected) return false;

        return WriteStringAddress(address, value);
    }

    public bool WriteString(int index, string value)
    {
        var address = GetAddress(index);
        if (address == null || !isConnected) return false;

        return WriteStringAddress(address, value);
    }

    private bool WriteStringAddress(PLCAddress address, string value)
    {
        try
        {
            // 确保字符串不超过最大长度
            if (value.Length > address.Index)
                value = value.Substring(0, address.Index);
            _plc.Write(DataType.DataBlock, address.DBNumber, address.Start, value);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"写入字符串错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
    }

    #endregion

    public void Dispose()
    {
        DisconnectPLC();
        // 先转换为IDisposable接口，再调用Dispose（空条件运算符确保安全）
        (_plc as IDisposable)?.Dispose();
    }
}