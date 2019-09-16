# AceBlog
AceBlog 项目使用 ASP.NET CORE 开发(src)，安装.NET CORE2.1环境即可打开运行。

数据库使用 SQLite，启动运行前请在配置文件中（src/AceBlog.Web/configs/appsettings.json）配置好 SQLite 的db文件（db文件位于src/AceBlog.Web/aceblog.db），连接字符串建议使用绝对路径。将AceBlog.Web设置为启动项即可启动。

默认测试帐号：admin 密码：111111

项目已实现：
* 注册和登录
* 发表博客、编辑博客，编辑器使用markdown语法
* 首页展示博客列表
* 个人主页、个人中心、博客管理