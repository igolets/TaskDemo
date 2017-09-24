# TaskDemo
The goal of this project is to demonstrate some complex ideas and technologies on a simple task. You can use it when nearning C#/ASP.NET MVC or when teaching others.
Feel free to contact me if you have any questions/suggestions

## Requirements
  - Create a new application to manage tasks using ASP.NET MVC 5 with Bootstrap.
  - Create a very simple list view to show Tasks with Add, Delete actions
  - Tasks can have subtasks and so on.. (unlimited nesting)
  - Task name must be unique on a given nesting level.
  - Do server and client Validations.
  - Save the form to the database. 
  - Support only modern browsers
  - Use : ( Sql Server Express / Local db, EF,  NUnit or other test framework)

## How to run
  - Clone the repo
  - Open solution in Visual Studio and build (I've used VS2017, but no new C# features used, it should work on earlier versions of Visual Studio)
  - Create new empty DB on MSSQL server and update web.config file in TaskDemo.Web project (I've used Localdb, but more mature versions should work as well).
  - Run TaskDemo.Web project, it will create DB itself

## What you can find in the code
  - EntityFramework [code first](https://msdn.microsoft.com/en-us/library/jj193542(v=vs.113).aspx)
  - DB initialization with indexes and foreign keys
  - Repository design pattern usage (https://msdn.microsoft.com/en-us/library/ff649690.aspx)
  - Dependency injection pattern usage (https://msdn.microsoft.com/en-us/library/ff921087.aspx) using [Microsoft Unity](https://msdn.microsoft.com/en-us/library/dn507457(v=pandp.30).aspx)
  - Unit testing (using mstest, [Moq](https://github.com/moq/moq4) and [Shouldly](https://github.com/shouldly/shouldly)) that disconnect code from database
  - Unit testing of server validation based on [attributes](https://msdn.microsoft.com/library/ee256141(v=vs.100).aspx)
  - Client-side validation with [unobtrusive validation](https://github.com/aspnet/jquery-validation-unobtrusive)
  - Data consistency checks on MSSQL side and handling them in controllers (name uniqueness and delete restrictions)
  - All external libs are references with [nuget](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console)
  - C# coding style that follows Microsoft and other industry leader recommendations (https://stackoverflow.com/questions/4678178/style-guide-for-c)
  - Distribution of the code between libraries that follows industry leader recommendations
  - MVC pattern usage that is a bit more complex than in Microsoft examples, but more often used in industry (see [MVVM](https://msdn.microsoft.com/en-us/library/hh848246.aspx))
  - Usage of builtin [bundling and minification](https://docs.microsoft.com/en-us/aspnet/mvc/overview/performance/bundling-and-minification) for JS and CSS
  - Usage of [Bootstrap](http://getbootstrap.com/)
  - Collect all unhandled exceptions using [Elmah](http://elmah.github.io/) and display user-friendly pages for 500 and 404 errors (disabled in debug mode)
  - Ignore file setup for Visual Studio :)

## What else you may find after a deeper look
  - Project is ready for but not used (for such simple case) [EF migrations](https://msdn.microsoft.com/en-us/library/jj591621(v=vs.113).aspx)
  - Unity is configured in code in order to have compile-time consistency checks, but you can see in comments how to move settings to web.config.
  - I'm using generic repositories, but code is ready to define custom repos and add some logic to them
  - Elmah does not send data to SQL oo email, but this is very easy to set up
  - No authentication there, but it is possible to add this and unit test authentication logic. This is a bit more compex task, so it is out of the scope
  - WebAPI controllers require some specific efforts to use Dependency Injection. All code is there, if you add WebAPI it should work and you'll be able to unit test thise controllers too



## License

MIT

