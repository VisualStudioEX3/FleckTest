# FleckTest
A little chat console program to test __Fleck__ websockets library and __IoC__ with Microsoft Dependency Injection library, developed in __C#__ using __.NET Core 3.1__.

* Fleck: https://github.com/statianzo/Fleck
* Microsoft.Extensions.DependencyInjection: https://github.com/aspnet/DependencyInjection

![FleckTest.jpg](https://github.com/VisualStudioEX3/FleckTest/blob/master/FleckTest.jpg)

## Issues/bugs
* Sometimes, the prompt/input line is not deleted by the last user message printed in console.
* Sometimes, the messages not uses the right color assigned (seems to be a task synchronization issue when change the console settings).
* Multiline messages not delete properly the last prompt/input printed line.

## TODO
* Implementing ping-pong task or similar in client to check state of the server (to end session in client when the server connection is lost).
* Basic encryption on socket messages (this would be easy to implement in the ServerMessage model implementation).
* Aditional commands like "ping", to send beep sound to all users or an especific user maybe, for example (this might need to extend the IUserInputCommandProcessor service interface).
* Create unit tests.
