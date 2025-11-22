# MoviesArchive

MoviesArchive is a web-based movies data storage project, written with C# 13 (.NET 9.0). Although it follows MVC design pattern, it is intended to be used as a Windows service, but can also function as a standalone executable (.exe) application.

## Implemented technologies

* Model validation: FluentValidation;
* Logging: Serilog;
* Dependency Injection control: Autofac;
* Mapping: Mapster;
* Database: SQLite;
* Data access and object-relational mapping (ORM): Entity Framework;
* Testing framework: xUnit;
* Mocking framework: Moq.

## Used patterns

* Template Method.

## Other details

* Cookie-based authentication;
* Main page contains pagination;
* Main page has a draggable modal window, which can be created in 2 different ways:
   1. With JavaScript fetch API when "Add movies from file" button is clicked;
   2. By merging with another HTML file – in all other cases (movie creation, editing, and deletion).
* Main page allows movies case-insensitive search by name and/or genre;
* Movies can be sorted by title, genre, release year, and rating.

## Project setup guidelines (Engilsh)

1. Download project from GitHub;

2. Create a database called "moviesdb.db" in MoviesArchive.Web project and execute "DB script.txt" script inside it;

3. In "appsettings.json" specify an absolute path to the database in parameter "DefaultConnection", for example:

`"DefaultConnection": "Data source=C:\DATABASEPATH\moviesdb.db"`

4. Open project solution (.sln) file (using the last version of Visual Studio 2022 or Visual Studio 2026 is advised for this step);

5. Click "MoviesArchive.Web" project name using right mouse button (later on – RMB), then click "Publish";

6. In the opened window click "Add a publish profile" > Folder > Finish > Close;

7. Click "Show all settings" and choose the following values:
Configuration: Release
Target Framework: net9.0
Deployment Mode: Self-contained
Target Runtime: win-x64

8. In list "File Publish Options" **ONLY** these options must be checked:
- [x] Produce single file
- [x] Enable ReadyToRun compilation 
- [x] Delete all existing files prior to publish

9. Press "Publish" button;

10. When publish is complete press "Navigate";

11. Open the Command Prompt;

12. Write the following command (PROJECTPATH must be the path of folder that opens after you press "Navigate" button):

`sc create MoviesArchive binpath= "C:\PROJECTPATH\MoviesArchive.Web.exe"`

You should see:

`[SC] CreateService SUCCESS`

13. Open "Windows Services" application. It can be done by pressing Windows+R > "services.msc" > Enter;

14. Find "MoviesArchive" service and launch it (RMB > Start);

15. Now you can access MoviesArchive website by entering the following text in browser address bar:

http://localhost:5000/user/login

16. To delete the service you need to stop it and run this command in the Command Prompt:

`sc delete MoviesArchive`


## Руководство по установке проекта (Русский)

1. Скачать проект с GitHub;

2. Создать базу данных "moviesdb.db" в проекте MoviesArchive.Web и выполнить в ней скрипт из файла "DB script.txt";

3. В файле "appsettings.json" указать абсолютный путь к базе данных для значения "DefaultConnection", пример:

`"DefaultConnection": "Data source=C:\ПУТЬКБАЗЕДАННЫХ\moviesdb.db"`

4. Открыть solution проекта (.sln файл) с помощью Visual Studio 2022 или Visual Studio 2026 последней версии;

5. Нажать правой кнопкой мыши (далее – ПКМ) по названию проекта "MoviesArchive.Web" > Publish;

6. В открывшемся окне нажать "Add a publish profile" > Folder > Finish > Close;

7. Нажать "Show all settings" и убедиться, что выбраны следующие значения:
Configuration: Release
Target Framework: net9.0
Deployment Mode: Self-contained
Target Runtime: win-x64

8. В списке "File Publish Options" должны быть отмечены **ТОЛЬКО** пункты:
- [x] Produce single file
- [x] Enable ReadyToRun compilation 
- [x] Delete all existing files prior to publish

9. Нажать кнопку "Publish";

10. Когда появится окно "Publish succeeded on ..." нажать на "Navigate";

11. Нажать Windows+R > написать "cmd" > Enter;

12. В открывшейся консоли написать следующую команду (заменив путь к файлу MoviesArchive.exe на тот, который открылся ранее при нажатии на кнопку "Navigate"):

`sc create MoviesArchive binpath= "C:\ПУТЬКПРОЕКТУ\MoviesArchive.Web.exe"`

должно появиться сообщение:

`[SC] CreateService SUCCESS`

13. Открыть приложение Windows Services. Это можно сделать нажав Windows+R > написать "services.msc" > Enter;

14. Найти сервис "MoviesArchive" и запустить его (например, ПКМ > Start);

15. Теперь можно открыть страницу входа на сайт по адресу:

http://localhost:5000/user/login

16. Для удаления сервиса нужно сначала его остановить, а затем написать в командной строке:

`sc delete MoviesArchive`
