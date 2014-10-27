#ADO.NET DAL

An assembly designed to provide all the normal ADO.Net commands, but to make some of the activities that you usually have to worry about, a lot easier, such as connectivity to the database.

The API is designed around an interface to make it easy to use with dependency injection.

##Why?

This assembly is one of my "ToolBelt" pieces of code that I've had knocking around for about 5 years.
I decided to dust it off, update it and then put it on GitHub so I always have access to it and if other people find it useful they can use it as well.  

##Using SQL Express

If you are using SQL Express you'll need to do a couple of things
 - ensure SQL Express is in **mixed mode authentication**
 - alter the connection strings to point to your SQL Express instance rather than LocalHost


###Licence
This software is licensed under the MIT license which basically means you can do what you want
with the software (read the license.txt for full details).
