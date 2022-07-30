Understanding Asynchronous Programming in C# [2022 Workshop]
============================
This project contains slides and code samples for the "Understanding Asynchronous Programminc in C#" workshop with Jeremy Clark.  

Overview and Objectives
-----------------------
*Level: Introductory / Intermediate*  
Asynchronous code is everywhere. In our C# code, we "await" method calls to services and databases; and more and more packages that we use every day have asynchronous methods. But do you really understand what this does?

Understanding is critical. When done correctly, we can make our applications more responsive, faster, and reliable. When done incorrectly, we can block threads or even hang the application entirely.

In this half-day workshop, we'll start at the beginning to see how "await" relates to "Task”. We'll do this by calling an asynchronous method, getting a result, and handling errors that come up. We will create our own "awaitable" methods to see how Task and return types work together. With our own methods, we'll also better understand why we may (or may not) care about getting back to the original calling thread. We'll also cover some dangers, such as using "async void" or misusing ".Result". Finally, we'll use Task to run multiple operations in parallel to make things faster. With all of these skills, we can write more effective asynchronous code.

You will learn:  
* How to use "await" and "Task" to run asynchronous methods  
* About handling errors from asynchronous processes  
* About writing your own asynchronous methods
How to avoid pitfalls such as "async void" and ".Result"  
* About running multiple methods in parallel  

**Pre-Requisites**  
For this workshop, it is assumed that you have some experience with C#, but no specific asynchronous programming experience is needed. To run the sample code, you will need the **NET 6.0 SDK** installed. Jeremy will be using **Visual Studio 2022** (Community Edition), but the code samples will run using **Visual Studio Code** or the editor of your choice.

Links:
* .NET 6.0 SDK  
[https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
* Visual Studio 2022 (Community)  
[https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)
* Visual Studio Code  
[https://code.visualstudio.com/download](https://code.visualstudio.com/download)

Running the Samples
-------------------
The sample code uses .NET 6.0. The console and web samples will run on all Window, macOS, and Linux versions that support .NET 6.0. The desktop samples are Windows-only.

Samples have been tested with Visual Studio 2022 and Visual Studio Code.

All samples require the "Person.Service" web service be running. To start the service, navigate to the "Person.Service" folder from the command line and type "dotnet run".

Ex:
```
C:\understanding-async\People.Service> dotnet run
```  

The service can be found at [http://localhost:9874/people](http://localhost:9874/people)

Project Layout
--------------
The "DemoCode" folder contains the code samples used in the workshop.  
**Shared Projects**  
* *People.Service*  
A web service that supplies data for the sample projects.  
* *TaskAwait.Shared*  
A library with data types that are shared across projects (primarily the "Person" type).  
* *TaskAwait.Library*  
A library with asynchronous methods that access the web service. These methods are called in the various applications detailed below.  
Relevant file: **PersonReader.cs**

**Concurrent Samples**  
The Concurrent samples run asynchronous methods, get results, handle exceptions, and support cancellation (unless otherwise noted).
* *Concurrent.UI.Console*  
A console application  (Windows, macOS, Linux)  
Relevant file: **Program.cs**
* *Concurrent.UI.Desktop*  
A WPF desktop application (Windows only).  
Relevant file: **MainWindow.xaml.cs**  
* *Concurrent.UI. Web*  
A web application (Windows, macOS, Linux).  
Note: this application does not support cancellation.  
Relevant file: **Controllers/PeopleController.cs**  

**Parallel Samples**  
The Parallel samples use Task to run asynchronous methods in parallel - also get results, handle exceptions, and support cancellation (unless otherwise noted).
* *Parallel.Basic*  
A console application that does not support cancellation or error handling (Windows, macOS, Linux).  
Relevant file: **Program.cs**
* *Parallel.UI.Console*  
A console application (Windows, macOS, Linux).  
Relevant file: **Program.cs**
* *Parallel.UI.Desktop*  
A WPF desktop application (Windows only).  
Relevant file: **MainWindow.xaml.cs**  
* *Parallel.UI. Web*  
A web application (Windows, macOS, Linux).  
Note: this application does not support cancellation.  
Relevant file: **Controllers/PeopleController.cs**  

**Progress Reporting (Bonus Material)**  
The Progress Reporting samples show how to report progress from an asynchronous method - in this case, as a percentage complete. These also get results, handle exceptions, and support cancellation.
* *ProgressReport.UI.Console*  
A console application that reports percentage complete progress through text. Ex: "21% Complete". (Windows, macOS, Linux)  
Relevant file: **Program.cs**
* *Parallel.UI.Desktop*  
A WPF desktop application that  reports percentage complete progress through a graphical progress bar. (Windows only)  
Relevant file: **MainWindow.xaml.cs**  
* *TaskAwait.Library*  
This shared library contains a method that supports progress reporting.  
Relevant method:
```c#
public async Task<List<Person>> GetPeopleAsync(IProgress<int> progress,
    CancellationToken cancelToken = new CancellationToken()) {...}
```

Hands-On Labs (Bonus Material)  
--------------
The "Labs" folder contains hands-on labs. These labs are not directly covered in the workshop due to time constraints but are provided as additional resources that can be completed separately.  

* [Lab 01 - Recommended Practices and Continuations](https://github.com/jeremybytes/async-workshop-2022/tree/main/Labs/Lab01)
* [Lab 02 - Unit Testing Asynchronous Methods](https://github.com/jeremybytes/async-workshop-2022/tree/main/Labs/Lab02)
* [Lab 03 - Working with AggregateException](https://github.com/jeremybytes/async-workshop-2022/tree/main/Labs/Lab03)

Each lab consists of the following:

* **Labxx-Instructions** (Markdown)  
A markdown file containing the lab instructions. This includes the scenario, a set of goals, and step-by-step instructions.  
This can be viewed on GitHub or in Visual Studio Code (just click the "Open Preview to the Side" button in the upper right corner).

* **Starter** (Folder)  
This folder contains the starting code for the lab.

* **Completed** (Folder)  
This folder contains the completed solution. If at any time, you get stuck during the lab, you can check this folder for a solution.

Additional Resources
--------------------
**Related Articles (by Jeremy)**
* ["await.WhenAll" Shows 1 Exception - Here's How to See Them All](https://jeremybytes.blogspot.com/2020/09/await-taskwhenall-shows-one-exception.html)

**Video Series & Articles (by Jeremy)**  
Each of these has a lot of supporting links:  
* [I'll Get Back to You: Task, Await, and Asynchronous Programming in C#](http://www.jeremybytes.com/Demos.aspx#TaskAndAwait)  
* [Run Faster: Parallel Programming in C#](http://www.jeremybytes.com/Demos.aspx#ParallelProgramming)  
* [Learn to Love Lambdas in C# (and LINQ, ToO!)](http://www.jeremybytes.com/Demos.aspx#LLL)  
* [Get Func-y: Delegates in .NET](http://www.jeremybytes.com/Demos.aspx#GF)  
* [Better Parallel Code with C# Channels](https://github.com/jeremybytes/csharp-channels-presentation)

**Other Resources**  
Stephen Cleary has lots of great articles, books, and practical advice.
* [Don't Block on Async Code](https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html) - Stephen Cleary
* [Async/Await - Best Practices in Asynchronous Programming](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming) - Stephen Cleary

Stephen Toub has great articles, too (generally with advanced insights).
* [Do I Need to Dispose of Tasks?](https://devblogs.microsoft.com/pfxteam/do-i-need-to-dispose-of-tasks/) - Stephen Toub

**Articles / Videos Suggested by prior Workshop Attendees**  
* [ASP.NET Core SynchronizationContext](https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html) by Stephen Cleary  
* [ConfigureAwait FAQ](https://devblogs.microsoft.com/dotnet/configureawait-faq/) by Stephen Toub  
* [There Is No Thread](https://blog.stephencleary.com/2013/11/there-is-no-thread.html) by Stephen Cleary  
* [C# Async/Await/Task Explained (Deep Dive)](https://www.youtube.com/watch?v=il9gl8MH17s) Video by Raw Coding

For more information, visit [jeremybytes.com](http://www.jeremybytes.com).