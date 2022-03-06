Feature: 01: AccountPortal - Register new Tesco customer account

# After the registration has been verified, I write the full account details to my own test database via entity framework, so that the account getails can automatically be picked
# up by my login script when searching for random login details that meet a broad criteria. For example, I may wish to pull out a random account which has MarketingCommunications=store

#FYI - commented out last few steps to avoid creating lots of accounts on tesco's live website

@AccountPortal
Scenario Outline: Register new Tesco customer account
	Given navigate to the tesco homepage
	And click the register link
	And generate random account details
	And complete the registration page using random details - using clubcard status:<ClubcardStatus> Marketing communications:<MarketingCommunications>
	When click to submit registration details and create an account
	Then the account has been created succesfully
	And Account data has been written to the data database

	Examples:
		| ClubcardStatus | MarketingCommunications |
		| JoinToday      | Store                   |
		| JoinToday      | Bank                    |
		| JoinToday      | Mobile                  |
		| JoinToday      | Store_Bank_Mobile       |
		| NoJoin         | Store                   |
		| NoJoin         | Bank                    |
		| NoJoin         | Mobile                  |
		| NoJoin         | Store_Bank_Mobile       |