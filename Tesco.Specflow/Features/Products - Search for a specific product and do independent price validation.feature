Feature: 03: Products - Search for a specific product and do independent price validation

# In this demo, the database being referenced holds product data including price.
# In a real world scenario, I would have a process in place where we ensure our independent DB is kept up to date.
# Alternatively we could use the actual DB the application supports, but then we would need a process in place to validate that the product/price get updated correctly in that DB.
# If we have confidence in the prices in the DB, then we simple just need to do a UI vs. DB check, which we do with this specflow/selenium scenario.
#FYI the independent price could have been written directly into this feature file. However I find using a database is more flexible and easier to query/model.
@Products
Scenario Outline: Search for a specific product and do independent price validation
	Given navigate to the tesco homepage
	When search by product type:<ProductType>
	And get the price for product:<ProductName>
	Then independently verify the price is correct

	Examples:
		| ProductType | ProductName                        |
		| bread       | Hovis Soft White Medium Bread 800G |
		| pasta       | Tesco Fusilli Pasta 500G           |
		| fish        | Tesco Smoked Salmon 120G           |