AltNetDI
===========

A set of examples using DI from a talk I gave, recording here - http://www.programgood.net/files/di.zip

One of the ways to make **maintainable code** is through **loose coupling**.
**DI enables loose coupling**


Use the bootstrapper to decide which example to run. 

Do the examples in the following order :-)  Have left this crazy numbering system so it's in sync with the video

-1. Dependency injection - Just a *Reader* and *Writer*

-7. Manager comes along wants you to write business logic that looks for [OldDbName] and
replace it with [NewDbName] in Sql script input file - Addition of a *Transformer* class

-2. Manager comes and says wants to log all the writes to a file - decorator pattern

-8. Using logging with a decorator with 7


-6. Manager wants to log file and database.  Adding in an *AggregateWriter*.. The *WriterLogger* decorates the *AggregateWriter*, which takes an array of *IWriters*

-5. Testing - want to make sure the datetime of the logger is correct.  Uses a Func<T>



All new changes - only affect compostiion root, and new code is added.
ie codebase is Open for Extension, Closed for Modification
  Open/Closed principle

The diagramming in VS2013 Professional is useful.  Slightly broken - when you press View Class diagram in file explorer it should go to the specific diagram.  However I tried to 
change things manually by editing the xml and haven't been able to fix it yet.  http://stackoverflow.com/questions/25604151/visual-studio-2013-class-diagram-strangeness
  

Many thanks to Mike Hadlow
