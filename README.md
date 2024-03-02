# KYC360Assn API Documentation

## Introduction
This document provides an overview of the KYC360Assn API, including endpoints, functionality, and usage instructions.

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- SQL Server
- FluentValidation
- Serilog

## Installation
1. Clone the repository to your local machine.
2. Open the solution in Visual Studio or your preferred IDE.
3. Install any required NuGet packages if not already installed.
4. Configure the database connection string in `appsettings.json`.
5. Run the application.

## Endpoints

### 1. GET /api/Entity
- **Description:** Retrieves a list of entities.
- **Query Parameters:**
  - `search`: Search term to filter entities.
  - `gender`: Gender of the entity.
  - `startDate`: Start date for filtering by `Dates.Date` field.
  - `endDate`: End date for filtering by `Dates.Date` field.
  - `countries`: Comma-separated list of countries to filter by `Addresses.AddressCountry` field.
- **Example:** `GET /api/Entity?search=bob&gender=male&startDate=2023-01-01&endDate=2023-12-31&countries=USA,UK`

### 2. GET /api/Entity/{id}
- **Description:** Retrieves the entity with the specified ID.
- **Example:** `GET /api/Entity/1`

### 3. POST /api/Entity
- **Description:** Creates a new entity.
- **Request Body:** 
  ```json
  {
    "names": [
      {
        "firstName": "John",
        "middleName": "Doe",
        "surname": "Smith"
      }
    ],
    "addresses": [
      {
        "addressLine": "123 Main St",
        "city": "New York",
        "country": "USA"
      }
    ],
    "dates": [
      {
        "dateType": "Birth",
        "dateValue": "1990-01-01"
      }
    ],
    "deceased": false,
    "gender": "Male"
  }

### 4. PUT /api/Entity/{id}
- **Description:** Updates the entity with the specified ID.
- **Request Body:** Same as POST /api/Entity

### 5. DELETE /api/Entity/{id}
- **Description:** Deletes the entity with the specified ID.
- **Example:** DELETE /api/Entity/1

## Searching, Filtering, Pagination, Sorting
The /api/Entity endpoint supports searching, filtering, pagination, and sorting.
You can search for entities using the search parameter, filter by gender, start and end date, and countries.
Pagination and sorting are also supported, allowing you to retrieve a subset of results and specify the sorting order. You can create multiple searching, filtering, pagination and sorting combinations easily with SwaggerUI.

## SwaggerUI
Navigate to /swagger/index.html after running the application to explore and test the endpoints interactively.

## Retry and Backoff Mechanism
The API implements a retry and backoff mechanism for database write operations to enhance robustness against transient failures. For the retry mechanism, there is a specified number of maxAttempts defined. When the number of attempts is equal to maxAttempts, the retry mechanism stops. Also there is a timeDelay that starts from 3 secs and is multiplied by 2 every time the operation fails to implement the backoff mechanism.
