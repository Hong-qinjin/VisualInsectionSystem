# VisualInsectionSystem

使用VM4.4+VS2022封装的视觉软件
<<<<<<< HEAD
解决方法：首先查看工程中时候存在Resources.resx相关的文件， 找到它在工程中的位置（不如说一般都是在：工程名.Properties 命名空间下）， 最后更改配置为new System.Resources.ResourceManager("工程名.Properties.Resources", typeof(Resources).Assembly);

启动程序，打开调试窗口，观察输出窗口的调试日志：
确认 DebugForm_Load: 句柄创建状态 - True 表示句柄正常创建
若事件在句柄创建前意外触发，会输出 StartEvent: Handle未创建 等日志，便于追踪
模拟连续运行 / 停止：
点击 buttonContiRun 启动连续运行，触发 OnProcessStatusStartEvent
再次点击停止，触发 OnProcessStatusStopEvent
检查是否有异常抛出，UI 是否正常更新
=======


1. VmGlobaltoolbar加载，环境配置信息
2. 界面方案的选择，加载，保存
3. 相机的拍照，参数界面调试
4. 外部通信的设置，PLC--
5. 用户登录信息，权限设置
6. 日志tab格式化输出
7. 
>>>>>>> refs/remotes/origin/master
