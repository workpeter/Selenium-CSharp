# Selenium-CSharp

## Author

* **Peter Anderson (peter.x4000@gmail.com)** 

## Built With

* [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) - Programming language
* [Selenium (inc. Selenium Grid)](https://en.wikipedia.org/wiki/Selenium_(software)) - Automate web applications
* [SpecFlow](https://specflow.org/) - BDD framework. Allows tests to be driven by feature files. Readable user flows which bind to code behind the scenes.

## Introduction

I wrote this solution to demonstrate a clear and robust approach to UI selenium automation testing, both in terms of code organization and functional capability. 

The code solution has a modular framework, whereby there are supporting class libraries files that can be used to support any selenium project for rapid and robust delivery.
I have also provided a working demonstration of a SpecFlow project designed to test the live https://www.tesco.com/ website. This can be used as a template design for setting up new real-world projects. 

This solution can be executed via the visual studios test explorer, which will then invoke various Specflow scenarios in parallel. 


## Test approach

Its common practice now to use C# selenium with a BDD library solution such as SpecFlow. 
SpecFlow provides the means to write business readable test flows in feature files, as well as organise those tests into scenarios and groups, and provide an easy mechanism for data-driven and parallel execution capabilities. 
In my example project, I demonstrate how this is achieved. 

![alt text](https://i.ibb.co/8sFkcqX/feature.png)

## Code organisation

This framework demonstrates how to write code in a structured way to promote robustness and maintainability. 

![alt text](https://i.ibb.co/0cMQ7hJ/org.png)


Below are some concepts I have used:

 * **Object modelling** 
the Tesco project is organised into two projects:

***Tesco.Framework:***

This principally models the Tesco website. For example, I have created classes Account, products, users, etc. I have leveraged entity framework so that I can create, read, update and delete these test objects to/from a SQL database. 
By organising my test inputs and outputs into meaningful objects, it adds structure to the testing and these objects can be passed around the testing solution to be used in conjunction with various user actions and assertions. 

Another form of object modelling I have used is page-object-modelling. This is where you create a class file for each webpage being interacted with and that class file is principally responsible for the interactions with that page. 
Typically you find the 'By' references and methods for invoking DOM elements for that UI page in said class file. This promotes code reusability, maintainability and encapsulation. 

***Tesco.Specflow:***

Here is where I keep the feature files (that contain the tests), the binded classes/methods that the tests bind to, any enhanced business logic and the Specflow hooks designed to invoke at the start/end of a given process.
I have setup a structured approach for setting up new test coverage, which uses inheritance to solve 3 things:
1) Binded classes inherent from FeatureBase, which automatically creates objects for all page and business logic interactions. This provides maximum flexible to any test without worrying about object creation. 
2) Specflow injects a ScenarioContext object which contains all the running data of the current test. I have this object passed down into the base class and I also create a Scenario an Utils object stored within ScenarioContext keys. This enables me to provide,  share and manipulate test information and objects for the entire life cycle of the running test.
3) The Specflow project sits at the top having access to lower dependency projects such as on the tesco.framework (access to complex Tesco data objects) and supporting class libraries given it maximum exposure and capability when carrying out tests. 


 * **Supporting web driver wrapper class library**  
Can be used/referenced by any selenium project. This wrapper class provides features such as:
    * Ability to launch different browsers (I have used chrome more often in the past, hence why the logic is more extended in that area)
    * Ability to launch local or remote drivers (selenium grid)
    * is non-static so can support parallel test execution (the number of parallel users is dictated by an assembly file read by NUnit -> AssemblyInfo_parallelism.cs)
    * Provides wrapper methods around common selenium actions (click, select, etc) to make them more reliable and versatile. This reduces the number of reported failures due to script failures and improves script capability.
    
 * **Supporting reporting class library**  
Can be used/referenced by any selenium project. By hooking into this library a selenium project can:
    * Capture results into a SQL database, Excel and file system (logs). 
    * The SQL database approach is particularly versatile because it allows you to overwrite results into an existing results table. useful if you had an existing script failure, which you have since fixed. 
    * The excel option is useful for those who do not wish to set up a database dependency and simply want the result written into an excel report template (provided). 
    * The log file system approach I find is a useful backup of results. The solution will also take screenshots on-error and put them in these logs folders. 
  
 
## Data-Driven
 
Specflow offers data-driven capability through its feature files and I have demonstrated some examples in this project. 

Beyond that, I have also demonstrated how to leverage data from a database to be used to drive tests. To do this I have used entity-framework. An example of how this works is I have a registration script, which creates a new account object during the registration process, then saves this account to my test database upon successful registration. 
Then I have a login script design to pick a random account for login. To pick a random account, it uses entity-framework to hook into the test database and find any account previously created by the registration script. it then pulls this account object into the solution and uses its username and password properties to login. 

There are ways to make tests data-driven without using SpecFlow which I haven't demonstrated in this project. However, one approach which comes to mind is using excel interop, which provides an interface between C# and excel. 
Then it's simply a matter of the soltuion pointing to the workbook and reading in data rows. Ideally pushing those rows into C# test case object first so that the data is more organised. 

* **Example test data created by the registration test** 

![alt text](https://i.ibb.co/4FcP5S7/DB-test-data.png)


## Test configuration

A settings file can be found within the solution, which can hold test setup variables before executing the test. 

![alt text](https://i.ibb.co/4gYnCyj/run-settings.png)

## Example Results screenshots

* **Visual studios Test Explorer** 

![alt text](https://i.ibb.co/BZS2cZJ/test-explorer.png)


* **Results pushed to SQL database** 

![alt text](https://i.ibb.co/4Sb6KsJ/db-results.png)


* **Results pushed to Excel** 

![alt text](https://i.ibb.co/p4bR7Tk/excel-results.png)
