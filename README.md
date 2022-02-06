# #SudoItApi-一款轻量级远程控制解决方案
## SudoItApi : A lightweight remote control solution
SudoItApi（又称$udo!T-Api）是一款基于Microsoft .NET技术的控制框架，使用ASP .NET MVC进行开发，从而实现了通过Http接口远程获取计算机状态以及控制计算机的功能。

SudoItApi (also known as $udo!T-Api) is a Microsoft .NET technology based control framework , using ASP .NET MVC for development , so as to achieve the Http interface to remotely obtain the status of the computer and control the computer function .

## #跨平台
### Cross- Platform
SudoItApi尽量使用跨平台的运行库进行开发，因此在理论上实现了大部分功能的跨平台运行，你可以在任何支持ASP .NET框架的操作系统上（包括MacOS，Linux及其衍生操作系统，Windows等）编译并运行。（当然并非所有功能实现了跨平台，目前已知文件系统的压缩文件模块与状态获取模块的部分参数仍无法完全实现跨平台。）

SudoItApi is developed using cross-platform libraries as much as possible, so in theory most of the features are cross-platform, and you can compile and run it on any operating system that supports the ASP .NET (Of course, not all features are cross-platform, and some of the parameters of the compressed file module and state acquisition module of the known file system are still not fully cross-platform.)

## #功能
### Features

截止Release I（内部版本v.1.0.1.0）为止，SudoItApi已经实现了以下功能：
-文件系统 包括大部分对于文件与目录的操作，如移动文件，复制文件，删除文件，创建文件，读取文件，获取文件基本信息等。
-进程控制 对于系统进程的操作，如启动进程，结束进程，Pid与进程的相互查询等
-状态获取 查询系统各硬件的使用率与服务端的在线状态
-命令系统 调用系统命令提示符执行命令
-插件系统 将在稍后着重论述

As of Release I (internal version v.1.0.1.0), SudoItApi has implemented the following features.
-File system Includes most operations on files and directories, such as moving files, copying files, deleting files, creating files, reading files, getting basic information about files, etc.
-Process control Operations on system processes, such as starting processes, ending processes, mutual query of Pid and processes, etc.
-Status acquisition Query the utilization rate of system hardware and the online status of the server
-Command system Call the system command prompt to execute commands
-Plug-in system will be discussed later

## #关于插件
### About Plugins

SudoItApi已全面启用插件系统，鉴于操作系统架构的不同以及用户的需求不同，我们为SudoItApi配备了插件系统。
插件系统运行后，将在目录下创建Plugins文件夹，其中包括四个部分：GET插件（GET-Methods），POST插件（POST-Methods），修改器插件（InsideProcessor），命令行插件（CommandProcessor），四个目录下将会放置不同的txt文件，这些文件名称就是方法名称，其内容指向一个可执行文件，即插件本身，当POST和GET插件被调用时，SudoItApi会启动一个该可执行文件的进程并将参数设置为其调用方法与传入参数。修改器插件会在用户执行一定的方法并返回正常结果后自动逐一执行，对结果进行改动处理，命令行插件则会在用户在终端窗口输入相应指令后被调用并输出结果。

SudoItApi is fully enabled with a Plugins system. In view of the different operating system architectures and the different needs of users, we have equipped SudoItApi with a Plugins system.
When the plugins system is running, a Plugins folder will be created in the directory, which includes four parts: GET plugins (GET-Methods), POST plugins (POST-Methods), modifier plugins (InsideProcessor), and command line plugins (CommandProcessor), and the four directories will When the POST and GET plugins are called, SudoItApi will start a process for that executable and set the parameters to its calling method and incoming parameters. The modifier plug-in is automatically executed one by one after the user executes certain methods and returns normal results, and the result is processed for changes, while the command line plug-in is invoked after the user enters the corresponding command in the terminal window and outputs the result.

安装插件时，您需要将以SudoItApi插件方式编辑的可执行文件放入Plugins文件夹中。这些可执行文件会在下一次SudoItApi服务端启动时被逐一执行（附带初始化参数），然后，插件需要在Plugins文件夹下的四个目录内注入自己的方法。随后，在插件被要求执行时，SudoItApi会通过这些注册的方法找到相应的可执行文件，并将方法与调用触发器作为参数执行这个可执行文件，执行完毕并得到结果后，结果的第一行将返回给用户（Http请求方），第一行与第二行都会作为日志保存并打印在终端窗口中。

When installing the plugin, you need to put the executables edited in the SudoItApi plugin way into the Plugins folder. These executables will be executed one by one the next time the SudoItApi server is started (with initialization parameters), and then the plugin will need to inject its own methods in four directories under the Plugins folder. Subsequently, when the plugin is requested to execute, SudoItApi will find the corresponding executable by these registered methods and execute this executable with the method and call trigger as parameters. After the execution is completed and the result is obtained, the first line of the result will be returned to the user (Http requesting party) and both the first and second lines will be saved as logs and printed in the terminal window.

## #关于开源许可
### About The OpenSource License

开发方使用AGPL-3.0开源协议对SudoItApi进行开源。这意味着如果您要使用、修改以及发布本项目，您必须遵循AGPL-3.0开源协议，将本项目的修改版本也进行AGPL-3.0进行开源。需要开源的代码包括但不限于软件与网络接口等的源代码。当然，如果您为SudoItApi开发插件，我们允许您对插件部分进行闭源。关于AGPL-3.0开源协议的具体条款，你可以查看LICENSE文件。
如果您肆意修改、发布、仿冒甚至售卖SudoItApi软件，或是以任何形式诋毁开发团队与本产品的声誉，我们就会使用一切手段进行维权，包括但不限于对您发起诉讼。
若您对本项目进行下载和使用，即代表您已认真阅读并同意以上声明，并且愿意承担违法以上声明造成的一切后果。共同营造好的开源环境与保护开源产品的合法权利是每个开发者乃至每个人的义务，在此，SudoItApi开发团队感谢您的下载和使用，希望您使用愉快！

The developer has open sourced SudoItApi using the AGPL-3.0 open source protocol. This means that if you want to use, modify and distribute this project, you must follow the AGPL-3.0 open source protocol and open source the modified version of this project with AGPL-3.0 as well. The code to be open sourced includes, but is not limited to, the source code of the software and web interface, etc. Of course, if you develop plug-ins for SudoItApi, we allow you to close source the plug-in part. For the specific terms of the AGPL-3.0 open source agreement, you can check the LICENSE file.
If you recklessly modify, distribute, counterfeit or even sell SudoItApi software, or in any way discredit the development team and this product, we will use all means to defend our rights, including but not limited to initiating a lawsuit against you.
By downloading and using this project, you agree that you have read and agreed to the above statement, and are willing to bear all the consequences of violating the above statement. It is the obligation of every developer and every person to create a good open source environment and protect the legal rights of open source products.

## #联系我们
### Contact us

SudoItApi开发团队非常高兴能听取各方意见和建议，如果您对我们的代码有任何疑问，或者有任何的改进建议，欢迎您通过以下联系方式与我们共同探讨！我们不一定24小时在线，但是我们一定会在看到消息后第一时间考虑，在考虑周全后尽快回复。
微信：EachFengye1003
QQ：3072554288
Telegram：@Fengye1003

The SudoItApi development team is always happy to listen to comments and suggestions. If you have any questions about our code or any suggestions for improvement, you are welcome to discuss them with us by using the contact form below! We may not be online 24 hours a day, but we will definitely consider the message as soon as we see it and reply as soon as possible after thorough consideration.
WeChat：EachFengye1003
QQ：3072554288
Telegram：@Fengye1003