Feature: CreateBooking
		?

@CreateBooking_StartDateBeforeOccupied_EndDateInOccupied
Scenario: Startdate before occupied and Enddate in occupied
	Given I create a booking with startdate <startDate>
	And The <endDate> between occupied Start date and End date
	When I create the booking
	Then The result should be <result>

	Examples: 
	| startDate  | endDate    | result |
	| 01-12-2023 | 05-12-2023 | False  |
	| 03-11-2023 | 04-12-2023 | False  |

@CreateBooking_StartDateBeforeOccupied_EndDateAfterOccupied
Scenario: Startdate before occupied and Enddate after occupied
	Given I create a booking with startdate 01-12-2023
	And The following End date 09-12-2023
	When I create booking
	Then The result is False