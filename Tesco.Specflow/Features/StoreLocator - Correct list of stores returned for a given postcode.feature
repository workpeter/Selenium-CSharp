Feature: 04: StoreLocator - Correct list of stores returned for a given postcode

# Tesco has a service that enables their application to send a postcode and get back a list of stores arranged by distance.
# Ideally the best way to test this would be two-fold
#
# 1) Validate their service via a unit/api test whereby 1000's postcodes are sent to the service and then we verify the response for each returns the correct stores.
# To achieve that we would need to know all of tesco store locations by long/lat and have a list of UK postcodes inc. long/lat.
# Then recreate the business logic for determining what stores were closest to a given postcode.
# That then provides independent validation on their service. i.e. do a method call against our independent logic vs. the API under test and verify they both returned the same list of stores.
#
# 2) Assuming the first testing solution was in place, you could do a cut down UI test, i.e. using specflow/selenium, where you take x number of postcodes, feed them into the UI, capture
# the response then assert the response (list of stores) would be the same compared to if we fed the same postcode into our aforementioned independent logic.
#
# As this is just a simple demo geared towards showcasing specflow/selenium, I will use 5 fixed postcodes and hardcode the expected responses in the feature file, which can be compared
# with the response from tesco. If this was a real project, I would insist on building my own independent dynamic logic, which would take a postcode as a parameters, then from that postcode,
# use a DB to determine long/lat, then compare that with a DB of store locations, then feedback a dynamic response, which could be asserted against the response from tesco.
#
@StoreLocator
Scenario Outline: Correct list of stores returned for a given postcode
	Given navigate to the tesco homepage
	When search for store using postcode:<MyPostcode>
	Then verify the top 2 stores are Store1Name:<Store1Name> Store1Postcode:<Store1Postcode> Store1Distance:<Store1Distance> Store2Name:<Store2Name> Store2Postcode:<Store2Postcode> Store2Distance:<Store2Distance>

	Examples:
		| MyPostcode | Store1Name                   | Store1Postcode | Store1Distance | Store2Name                  | Store2Postcode | Store2Distance |
		| CT12 6QA   | Broadstairs Extra            | CT10 2QJ       | 0.70           | Ramsgate Manston Superstore | CT12 6NT       | 0.74           |
		| BS20 8PY   | Portishead W Hill Express    | BS20 6LR       | 1.39           | Nailsea Superstore          | BS48 1AQ       | 2.92           |
		| CO2 0EJ    | Colchester Crouch St Express | CO3 3HH        | 3.39           | Colchester Magdalen Express | CO1 2LA        | 3.79           |
		| BT35 7LZ   | Newry Extra                  | BT34 1EE       | 2.69           | na                          | na             | na             |
		| CF44 6EN   | Aberdare Superstore          | CF44 8DL       | 1.62           | Merthyr Tydfil Extra        | CF47 0AL       | 3.98           |