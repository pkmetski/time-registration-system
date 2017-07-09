Time Registration System v1.0

This project implements WebApi RESTful ednpoints for registering and retrieving
project time registrations data.

**Features**

-   create new time registration

-   retrieve time registration(s)

 

**Technologies**

-   C\# WebApi REST services

-   Data layer using NHibernate

-   SQL database for production code

-   In-memory SQLite database for testing

 

**Prerequisites**

An SQL server installation with database named *TimeRegistrationSystem*.

 

**Usage**

-   Create new registration (POST)

The request body is a JSON representation of a Registration instance:

{

"Date": "2034-12-31",

"Hours": 62,

"Project": "project1",

"Customer": "customer3"

}

In order to create a new registration ,a POST request is made against the
following endpoint:

http://localhost:65046/api/registrations

If the request succeeds, HTTP code 201 is returned, together with the ID of the
newly created record.

 

-   Retrieve registration by ID (GET)

In order to retrieve a specific record, whose ID is known, a GET request is
issued against the following endpoint:

http://localhost:65046/api/registrations/id/1

This will return a JSON representation of the entity with ID 1, or 404 in case
such entity doesn’t exist.

 

-   Retrieve registrations matching search criteria (GET)

It is possible to issue a more complex queries against the API:

http://localhost:65046/api/registrations?fromDate=2000-01-31&toDate=2007-01-01&project=project1&customer=customer3

This query will match all registration records with date between 31st January
2000 and 1st January 2007, with project “project1”, and customer “customer3”.
All the fields need to match in order for a record to be returned. It is
possible to omit any (or all) parameters. Issuing the query with no parameters
will apply no filtering to the data set and all records will be returned.
