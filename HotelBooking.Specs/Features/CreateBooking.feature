Feature: CreateBooking
		In order to create a correct booking
		As a customer
		I want to be able to know if the dates I used are correct

Background: The occupied date is: 2023-12-02 to 2023-12-07

@CreateBooking_IncorrectDates_ExpectFalse
Scenario: Incorrect start and enddate
	Given I create a booking with startdate <startDate>
	And The following EndDate <endDate>
	When I create the booking
	Then The result should be <result>

	Examples: 
	| startDate    | endDate      | result |
	| '2023-12-01' | '2023-12-05' | False  |
	| '2023-11-03' | '2023-12-04' | False  |
	| '2023-12-01' | '2023-12-09' | False  |
	| '2023-12-03' | '2024-01-01' | False  |
	| '2023-12-03' | '2023-12-05' | False  |
	| '2023-12-02' | '2023-12-06' | False  |

@CreateBooking_CorrectDates_ExpectTrue
Scenario: Correct start and enddate
	Given The following StartDate <startDate>
	And The EndDate <endDate>
	When I create a booking
	Then The result will be <result>

	Examples: 
	| startDate    | endDate      | result |
	| '2023-12-08' | '2023-12-09' | True   |
	| '2023-11-04' | '2023-11-07' | True   |






@CreateBooking_StartDateBeforeOccupied_EndDateAfterOccupied
Scenario: Startdate before occupied and Enddate after occupied
	Given I create a booking with startdate 2023-12-01
	And The following End date 2023-12-09
	When I create booking
	Then The result is False