AspMvc5Northwind
================

This is an example project demonstrating ASP.NET MVC5, using database-first
design method, using the Northwind database.

Most code is either copied from or heavily inspired by examples/tutorials
available at http://www.asp.net/mvc/tutorials/.

The database for this project was created locally using NorthwindDB.sqlserver.sql
in this directory.


Getting started
---------------

To build and run this project, do the following:

# TODO: I haven't actually tested this method.

- Check out / copy this project
- Open the project with Visual Studio (I used vs2013.2)
- Create the Northwind database using the included script
  (NorthwindDB.sqlserver.sql), or bring your own.
- Create a connection to the Northwind database within the project (?)


Rough steps to recreate this project
------------------------------------

If for some reason you want to create this project from scratch (rather than
using the included source files), do the following:

- Create a new ASP.NET MVC project
- Add a blank database
- Run NorthwindDB.sqlserver.sql to create the Northwind database
- Either copy the controllers, views etc. that are unique to this project,
  or create them using MVC templates/tools within Visual Studio. You'll
  then have to modify the generated code to match changes that I've made...


More info about this project
----------------------------

So far I've just been working through [this tutorial](http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc)
to develop the project. The tutorial uses code-first development, whereas I'm using
database-first.

ASP.NET / MVC / EF6 features used in this project:

- Enhancing a generated data model using data annotations attributes,
  enabling better front-end input data validation
- Performing basic CRUD operations
- Filtering, ordering, searching, paging
- Working with related data
- Implementing connection resiliency
- Using command interception

TODO
----

- Test the 'getting started' method above
- Put on github