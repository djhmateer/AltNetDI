AltNetDI
===========

A set of examples using DI

One of the ways to make maintainable code is through loose coupling.
DI enables loose coupling


Use the bootstrapper to decide which example to run

1. Dependency injection

2. Manager comes and says wants to log all the writes to a file - decorator pattern

3. Manager wants to read from DB

4. Manager wants to logs all write to db

5. Testing - want to make sure the datetime of the logger is correct

6. Manager wants to log file and database

**all new changes - only affect compostiion root, and new code is added.
ie codebase is Open for Extension, Closed for Modification
  Open/Closed principle

7. Manager comes along wants you to write business logic that looks for [OldDbName] and
replace it with [NewDbName] in Sql script input file


Many thanks to Mike Hadlow
