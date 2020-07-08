# FleckTest
A little chat console program to test Fleck websockets library and IoC with Microsoft Dependency Injection library, developed in C# using .NET Core 3.1.

* Fleck: https://github.com/statianzo/Fleck
* Microsoft.Extensions.DependencyInjection: https://github.com/aspnet/DependencyInjection

## TODO
* Implementing check for duplicated user names in server session.
* Implementing ping-pong task in server to check state of each connection (to resolve lost connections and removed from session).
* Implementing ping-pong task in client to check state of the server (to resolve lost connection and ended the session).
* Improving client implementation trying to use Fleck SocketWrapper implementation.
* Improving UserInput behaviour (to fix the prompt and other issues).
* Improving most of the WebSocket exception catches.
