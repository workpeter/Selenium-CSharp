using System;
using TechTalk.SpecFlow;
using WebDriverWrapper.Utils;

namespace Tesco.Specflow.Base
{
    public class ScenarioBase
    {
        public ScenarioBase(ScenarioContext scenarioContext)
        {
            //Tests are driven by Scenarios Feature files. The test steps must be located in a binded class for SpecFlow to recognise them.
            //A test scenario may comprise of a number of binded classes. In order to share common data between these classes, the test data is stored within a dictionary object called ScenarioContext.
            //When a test scenario executes, it creates instances of these binded classes and injects ScenarioContext into the class constructor allowing it access to the relevent scenario test data.

            //The mechanism { get => (xxxx)ScenarioContext[nameof(zzz)]; set => ScenarioContext[nameof(zzz)] = value; } means that when an object is created, it stored within the ScenarioContext dictionary
            //as a key. That object is now accessible throughout the test execution across various classes to have access to ScenarioContext.
            ScenarioContext = scenarioContext ?? throw new ArgumentNullException("scenarioContext");
        }

        //The ScenarioContext is created for each individual scenario execution and it is disposed when the scenario execution has been finished
        public ScenarioContext ScenarioContext { get; }

        //Keep track of the WebDriver object and provides extended functionality to manipulate the browser
        public WebDriverUtils Utils { get => (WebDriverUtils)ScenarioContext[nameof(Utils)]; set => ScenarioContext[nameof(Utils)] = value; }

        //Keeps track of all the dynamic data created and modified throughout test execution.
        public Scenario Scenario { get => (Scenario)ScenarioContext[nameof(Scenario)]; set => ScenarioContext[nameof(Scenario)] = value; }

        //public TestResult TestResult { get => (TestResult)ScenarioContext[nameof(TestResult)]; set => ScenarioContext[nameof(TestResult)] = value; }

        //public DatabaseConnect DatabaseConnect { get => (DatabaseConnect)ScenarioContext[nameof(DatabaseConnect)]; set => ScenarioContext[nameof(DatabaseConnect)] = value; }
    }
}