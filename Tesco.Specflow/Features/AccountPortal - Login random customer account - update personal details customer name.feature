Feature: 02: AccountPortal - Login random customer account - update personal details customer name


@AccountPortal
Scenario Outline: Login random customer account - update personal details customer name
	Given log into a random account
	When view my account portal
	And view my personal details page
	Then verify the customer name is correct
	When update the customer name to a random name
	Then verify the customer name is correct


