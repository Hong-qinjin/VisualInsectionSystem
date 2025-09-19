# VisualInsectionSystem

使用VM4.4+VS2022封装的视觉软件
解决方法：首先查看工程中时候存在Resources.resx相关的文件， 找到它在工程中的位置（不如说一般都是在：工程名.Properties 命名空间下）， 最后更改配置为new System.Resources.ResourceManager("工程名.Properties.Resources", typeof(Resources).Assembly);