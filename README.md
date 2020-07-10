# FleckTest
A little chat console program to test Fleck websockets library and IoC with Microsoft Dependency Injection library, developed in C# using .NET Core 3.1.

* Fleck: https://github.com/statianzo/Fleck
* Microsoft.Extensions.DependencyInjection: https://github.com/aspnet/DependencyInjection

![FleckTest.jpg](https://github.com/VisualStudioEX3/FleckTest/blob/master/FleckTest.jpg)

## Issues/bugs:
* Sometimes, the prompt/input line is not deleted by the last user message printed in console.
* Sometimes, the messages not uses the right color assigned (seems to be a tasks synchronization issue with the console settings changes).
* Multiline messages not delete properly the last prompt/input printed line.

## TODO
* Implementing ping-pong task or similar in client to check state of the server.
* Basic encryption on socket messages? (this would be easy to implement in the ServerMessage model implementation).
* Aditional commands like "ping", to send beep sound to all users or an especific user maybe, for example (this might need to extend the IUserInputCommandProcessor interface).
* Create unit tests.
