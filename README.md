<p align="center">
	<img alt="logo" src="https://oscimg.oschina.net/oscnet/up-dd77653d7c9f197dd9d93684f3c8dcfbab6.png">
</p>
<h1 align="center" style="margin: 30px 0 30px; font-weight: bold;">RuoYi.Net v2.0.0</h1>
<h4 align="center">基于.NET开发的快速开发框架</h4>

## 平台简介
若依(<a href="https://github.com/yangzongzhuan/RuoYi" target="_blank">github</a>, <a href="https://gitee.com/y_project/RuoYi" target="_blank">gitee</a>)是一款优秀的Java平台下的后台管理系统, 非常受欢迎. .NET平台下也有很多类似的开源项目, 但使用起来总感觉不够顺手, 因此便有了RuoYi.Net项目. RuoYi.Net目前仅实现了前后端分离版, 后端几乎一比一复刻了RuoYi后端功能, 前端仅对原ruoyi-ui(vue2版本)做了极小的改动以适应.net(如:服务监控页面的java相关信息改成了.net相关信息, 不在意此功能的可直接使用原ruoyi-ui).
	
## 技术框架及依赖
1. 后端		
   - 基础架构: .NET7
   - Excel处理: 基于国产优秀框架MiniExcel(<a href="https://github.com/mini-software/MiniExcel" target="_blank">github</a>, <a href="https://gitee.com/dotnetchina/MiniExcel" target="_blank">gitee</a>)
   - 数据库: MySql + 国产优秀框架SqlSugar(<a href="https://github.com/DotNetNext/SqlSugar" target="_blank">github</a>, <a href="https://gitee.com/dotnetchina/SqlSugar" target="_blank">gitee</a>), SqlSugar支持多数据库, 目前项目支持MySql/SqlServer, 其他数据库支持待完善.
   
2. 前端 
   VUE2 (element-ui)

## 使用
账号/密码: admin/admin123

## 内置功能(同若依)

1.  用户管理：用户是系统操作者，该功能主要完成系统用户配置。
2.  部门管理：配置系统组织机构（公司、部门、小组），树结构展现支持数据权限。
3.  岗位管理：配置系统用户所属担任职务。
4.  菜单管理：配置系统菜单，操作权限，按钮权限标识等。
5.  角色管理：角色菜单权限分配、设置角色按机构进行数据范围权限划分。
6.  字典管理：对系统中经常使用的一些较为固定的数据进行维护。
7.  参数管理：对系统动态配置常用参数。
8.  通知公告：系统通知公告信息发布维护。
9.  操作日志：系统正常操作日志记录和查询；系统异常信息日志记录和查询。
10. 登录日志：系统登录日志记录查询包含登录异常。
11. 在线用户：当前系统中活跃用户状态监控。
12. 定时任务：在线（添加、修改、删除)任务调度包含执行结果日志。
13. 代码生成：前后端代码的生成（.net、html、js、sql）支持CRUD下载 。
14. 系统接口：根据业务代码自动生成相关的api接口文档。
15. 服务监控：监视当前系统CPU、内存、磁盘、堆栈等相关信息。
16. 缓存监控：对系统的缓存查询，删除、清空等操作。
17. 在线构建器：拖动表单元素生成相应的HTML代码。
18. 连接池监视：监视当前系统数据库连接池状态，可进行分析SQL找出系统性能瓶颈。(暂无)
