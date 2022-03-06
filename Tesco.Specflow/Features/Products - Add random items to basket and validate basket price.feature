Feature: 03: Products - Add random items to basket and validate basket price

# Please note: The other scenario 'Products - Search for a specific product and do independent price validation' is an example of doing independent price validation, if the database being hooked into was
# independently managed by the test ream in terms of product/price list.
# This scenario is more geared towards making sure the product+price translates correctly from the prouct overview into the basket view. 
@Products
Scenario Outline: Add random items to basket and validate basket price
	Given log into a random account
	When search by product type:<ProductType>
	And clear any existing items in the basket
	And add to basket <NumItemsToBasket> random products from the search results
	Then verify the items were added to the basket with the expected price


	Examples:
		| ProductType | NumItemsToBasket |
		| bread       | 1                |
		| beans       | 2                |
		| chicken     | 3                |