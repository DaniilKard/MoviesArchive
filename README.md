# MoviesArchive

MoviesArchive is a web-based project for storing movie data. It's an ASP .NET Core MVC (.NET 9.0) application and should be used as a Windows service.

## Implemented technologies

* Validation: FluentValidation, DataAnnotations;
* Logging: Serilog;
* IoC: Autofac;
* Mapping: Mapster;
* Database: SQLite;
* ORM: Entity Framework;
* Testing: xUnit, Moq;
* Cookie-based authentication;
* Cookie-based authentication;
* Bundling & Minification.

## Design patterns

* Template Method;
* Strategy.

## Features

* Passwords are hashed;
* Admin can add, edit and delete genres;
* Admin can edit and delete movies created by any user;
* Main page contains pagination;
* Main page has a draggable modal window;
* Main page allows movies search by name and genre;
* Movies can be sorted by title, genre, release year, and rating.
