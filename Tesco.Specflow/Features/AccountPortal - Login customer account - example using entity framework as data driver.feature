Feature: 02: AccountPortal - Login customer account - example using entity framework as data driver

#This is example scenario using entity framework to pull login details from a database. 

#Entity framework data selection is very flexible, in that we can specify a single property/value for an account, then look it up in the DB to grab the
#the entire account object (including the key properties username/password to support the login step)

#Please note - i am using my own test database to support this. When I run the registration scenario, that scenario automatically writes the new accounts to the test database
#so they can be used to login. 
# In a real-world sitution, when testing in the dev environment, id be looking to hook directly into the dev database to pull account data, which is more guaranted given that the 
# application itself would be writing/reading to that DB. This would also mean database restores and rebaselining wouldnt be an issue, because my test solution would always be in sync
# with the application under a test from a data perspective.

@AccountPortal
Scenario Outline: Login with tesco customer account - example using entity framework as data driver
	Given navigate to the tesco homepage
	And click the login link
	And select account by:<SelectionType> and <SearchCriteria>
	When submit login details and igmore one time passcode setup:true
	Then account is logged in


	Examples:
		| SelectionType          | SearchCriteria                      |
		| random                 | n/a                                 |
		| email                  | Brain.Mitchell.07676682028@test.com |
		| email                  | Tyrel.Corkery.07552728778@test.com  |
		| MarketingCommunication | Bank                                |
		| ClubcardStatus         | NoJoin                              |