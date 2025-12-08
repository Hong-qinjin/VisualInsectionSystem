# Systemdemo01
    海康演示程序
# VisualInsectionSystem
    使用VM4.4+VS2022封装的视觉软件，套用一些显示组件。
    1.启动程序，打开调试窗口，观察输出窗口的调试日志：确认 DebugForm_Load: 句柄创建状态 - True 表示句柄正常创建
若事件在句柄创建前意外触发，会输出 StartEvent: Handle未创建 等日志，便于追踪
    2.模拟连续运行 / 停止：点击 buttonContiRun 启动连续运行，触发 OnProcessStatusStartEvent
再次点击停止，触发 OnProcessStatusStopEvent，检查是否有异常抛出，UI 是否正常更新

## 功能介绍
1. VmGlobaltoolbar加载，环境配置信息
2. 界面方案的选择，加载，保存
3. 相机的拍照，参数界面调试
4. 外部通信的设置，PLC--
5. 用户登录信息，权限设置
6. 日志tab格式化输出
7. 


## 操作方式
#region  plc备注
/* 配置PLC通讯
 * 确认PC网络设置
 * 安装必要S7.Net库 
 * 创建连接实例
 * 对话通信管理：会话验证，心跳机制，超时处理，连接恢复
 * 
 * 链接策略：创建连接池实例，请求连接，连接复用，关闭·释放连接
 * 
 * 断线重连策略：定期检测连接状态，设置重连机制，日志记录，用户通知
 * 
 * 数据读写操作：读写基本数据类型，读取单个和多个，读取DB块数据的技术，
 * 写入单个和多个，功能块数据写入技术，
 * 
 * 错误处理异常分类：数据读取异常，连接异常，超时异常
 *                    通信异常，网络异常，PLC响应异常
 * 
*/
#endregion


解决方法：首先查看工程中时候存在Resources.resx相关的文件， 找到它在工程中的位置（不如说一般都是在：工程名.Properties 命名空间下）， 最后更改配置为new System.Resources.ResourceManager("工程名.Properties.Resources", typeof(Resources).Assembly);