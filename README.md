Word Wide Architecture
===================

What is WWA?
------------

WWA is a starting point for building modern RESTful applications. It is intended to help .NET developers on REST architecture, facilitating migration from ASP .NET Web Forms and MVC to Web API.

### Architecture

WWA is a application framework built on top of __.NET Framework__, __.NET Standard__ , __Xamarin__ and __ASP .NET Web API__, using modern practices (SOLID, IoC, etc.) and known tools for developers. It  implements a very simple (but powerful) layered architecture (Domain, Data, Logic, App) allowing developers focus on the solution itself.

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

- __Wwa.Core__: Common components and interfaces 
- __Wwa.Api__: Api layer components implemeted on top of
- __Wwa.Http__: Http client components for consuming Wwa-powered RESTful apps
- __Wwa.Data.EF__: Data layer implementation using Entity Framework
- __Wwa.Identity.AspNet__: Authentication layer implenentation using ASP .NET Identity
- __Wwa.Ioc.Autofac__: Dependency injection implementation using Autofac
- __Wwa.Ioc.Unity__: Dependency injection implementation using Microsoft Unity
- __Wwa.Angular__: TypeScript infrastructure code for consuming Wwa-powered RESTful apps
- __Wwa.Xam__: Core components for Xamarin Forms

Links
-----

* Twitter: http://twitter.com/wwdenis
* LinkedIn: http://linkedin.com/in/denis81

Future
-----
- .NET Standard (In Progress)
- ASP .NET Core
- More data access components (nHibernate, Dapper, etc.)
- Xamarin Forms sample project (In Progress)
- More languages (Ruby, Node, Python, etc.)

Sample Project
-----
The Marketplace project demonstrate how WWA can be used in building RESTful applications and consuming them.

### Structure

- __Marketplace.Domain__: Domaain objects (POCO classes) 
- __Marketplace.Data__: Data layer (Data context and mappings logic)
- __Marketplace.Logic__: Business layer
- __Marketplace.Api__: App layer (Controllers, Models)
- __Marketplace.Web__: Single page application using Angular 1, TypeScript and Bootstrap
- __Marketplace.Tests__: Unit test project
- __Marketplace.Xam__: Xamarin Forms PCL project
- __Marketplace.Models__: Models shared between API and mobile (.NET Standart)
- __Marketplace.Db.Schema__: Application database schema (SQL Server Database Project)
- __Marketplace.Db.Identity__: Authetication entine schema (Microsoft Identity, SQL Server Database Project)

### Running the Sample Project

Open the solution Marketplace.sln in the [samples] folder in Visual Studio 2015 (or greater).

The first time you ron the project, Visual Studio will warn about mising files on the Marketplace.Web project. This happens because all nuget script files must be copied to the project folder.

When you see this warning, enter the following command in the package Manager Console:

```
Update-Package -Reinstall -ProjectName Marketplace.Web
```