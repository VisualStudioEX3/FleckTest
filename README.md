# FleckTest
A little chat console program to test __Fleck__ websockets library and __IoC__ with Microsoft Dependency Injection library, developed in __C#__ using __.NET Core 3.1__.

* Fleck: https://github.com/statianzo/Fleck
* Microsoft.Extensions.DependencyInjection: https://github.com/aspnet/DependencyInjection

![FleckTest.jpg](https://github.com/VisualStudioEX3/FleckTest/blob/master/FleckTest.jpg)

## Usage
Type `FleckTest.exe %PORT_NUMBER`

* The program has two behaviours on the start process:
  * First tries to connect as client to server address (in fact, for testing pourposes, currently the address always is localhost) and a port passed as argument.
  * If the connection failed, assumed to the server is not running. Then starts the creation server proccess.

## Overview and feature list
* The client currently only implements a basic command to terminate chat session and close the program, `exit`.
* The server supports, in theory (not tested it), an unlimited user sessions in the unique chat room.
* Each user has able to register an unique username, any value and length except empty string or spaces.
* The server assign an one color scheme to each user (12 variations).
* User messages can be any string value and length except empty string or spaces.
* The server can detect any client lost connection (the client crash or exit using the close window command instead of `exit` command) to avoid active ghost/abandoned sessions. The clients, at now, only catch the error when trying to send a message to a server that was crashed o not closed properly.

FYI: The Visual Studio project has defined a port number in debug profile (Project Properties -> Debug tab). You can run multiple instances of the program to run server and multiple clients (using [Debug->Start New Instance](https://docs.microsoft.com/en-us/visualstudio/debugger/debug-multiple-processes?view=vs-2019) command from context menu on project item in __Solution Explorer__ window).

## Issues/bugs
* Sometimes, the prompt/input line is not deleted by the last user message printed in console.
* Sometimes, the messages not uses the right color assigned.
* Multiline messages not delete properly the last prompt/input printed line.
* The above issues seems to be because a bad tasks synchronization when changed the console settings and cursor position corrections (necessary to the right prompt behaviour, for example).

## TODO list
* Implementing ping-pong task or similar in client to check state of the server (to end session in client when the server connection is lost).
* Basic encryption on socket messages (this would be easy to implement in the [ServerMessage](https://github.com/VisualStudioEX3/FleckTest/blob/master/FleckTest/Models/ServerMessage.cs) model implementation).
* Additional commands like "ping", to send beep sound to all users or an especific user maybe, for example (this might need to extend the [IUserInputCommandProcessor](https://github.com/VisualStudioEX3/FleckTest/blob/master/FleckTest/Interfaces/IUserInputCommandProcessor.cs) service interface).
* Create unit tests.
* Create [.editorconfig](https://docs.microsoft.com/es-es/visualstudio/ide/create-portable-custom-editor-options?view=vs-2019) file (suggested by [AzazelN28](https://github.com/AzazelN28)).

## Disclaimer
This is a side project developed as result of a technical test for a job interview and a way to learn new technologies and common practices. Not intended to be a serious project or final product in the future.
