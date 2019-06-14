Prolix: A footprint for a Clean Architecture
===================

What is Prolix?
------------

According [Orford Dictionary](https://en.oxforddictionaries.com/definition/prolix), Prolix means "using or containing too many words; tediously lengthy".

Many developers have the same problems everyday, and most of then try to recreate the solutions instead , causing repetitive, boring and tedious code.

Prolix is a starting point for building modern RESTful applications. It is intended to help developers to write clean, maintenable and less code.

### Architecture

Prolix is a application framework built on top of __.NET Framework__, __.NET Standard__ , __Xamarin__ and __ASP .NET Web API__, using modern practices (SOLID, IoC, etc.) and known tools for developers. It  implements a very simple (but powerful) layered architecture (Domain, Data, Logic, App) allowing developers focus on the solution itself.

### Technologies

- Xamarin Forms
- .NET Standard
- .NET Framework 4.6.1
- ASP .NET Web API
- Entity Framework
- Autofac
- Unity
- TypeScript
- AngularJS
- Bootstrap

### Structure

|Component|Description|
|:-:|-|
|__Prolix.Core__|Common components and interfaces| 
|__Prolix.AspNet__|Api layer components implemeted on top of ASP .NET Web API|
|__Prolix.Http__|Http client components for consuming Prolix-powered RESTful apps|
|__Prolix.Data.EF__|Data layer implementation using Entity Framework|
|__Prolix.Identity.AspNet__|Authentication layer implenentation using ASP .NET Identity|
|__Prolix.Ioc.Autofac__|Dependency injection implementation using Autofac|
|__Prolix.Ioc.Unity__|Dependency injection implementation using Microsoft Unity|
|__Prolix.Angular__|TypeScript infrastructure code for consuming Prolix-powered RESTful apps|
|__Prolix.Xam__|Core components for Xamarin Forms|

Links
-----

* Twitter: http://twitter.com/wwdenis
* LinkedIn: http://linkedin.com/in/denis81

Roadmap
-----
- .NET Standard (In Progress)
- ASP .NET Core
- Angular CLI Support
- More data access components (nHibernate, Dapper, etc.)
- Xamarin Forms sample project (In Progress)
- More languages (Ruby, Node, Python, etc.)

Sample Project
-----
The Marketplace project demonstrate how Prolix can be used in building RESTful applications and consuming them.

### Structure

|Component|Description|
|:-:|-|
|__Marketplace.Domain__|Domain objects (POCO classes)|
|__Marketplace.Data__|Data layer (Data context and mappings logic)|
|__Marketplace.Logic__|Business layer (Services)|
|__Marketplace.Api__|App layer (Controllers, Models)|
|__Marketplace.Web__|Single page application using AngularJS, TypeScript and Bootstrap|
|__Marketplace.Tests__|Unit test project|
|__Marketplace.Xam__|Xamarin Forms PCL project|
|__Marketplace.Client__|Client and Models shared between API and mobile (.NET Standard)|
|__Marketplace.Sql__|Application database schema (SQL Server Database Project)|

### Running the Sample Project

Open the solution Marketplace.sln in the [samples] folder in Visual Studio 2015 (or greater).

The first time you run the project, Visual Studio will try restore the NPM packages from the Marketplace.Web project.

If you see missing dependency errors (e.g. np.Controller), enter the following command in the Command-Line:

```
C:\Dev\Git\prolix\samples\Marketplace\Marketplace.Web> npm i
```