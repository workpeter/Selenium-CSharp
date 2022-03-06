Feature: 02: AccountPortal - Login customer account - example using feature file as data driver

@AccountPortal
Scenario Outline: Login with tesco customer account - example using feature file as data driver
	Given navigate to the tesco homepage
	And click the login link
	When submit login details username:<Username> and password:<Password>
	Then account is logged in

	Examples:
		| Username                            | Password     |
		| Brain.Mitchell.07676682028@test.com | Pass@word456 |
		| Deontae.Borer.07654860509@test.com  | Pass@word456 |
		| Tyrel.Corkery.07552728778@test.com  | Pass@word456 |
