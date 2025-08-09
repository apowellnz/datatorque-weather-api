# datatorque-weather-api
A basic .net 8 API that consumers 3rd part weather services and returns it to the user with suggestion of what to wear 

## Original Breif
Wellington weather is famously unpredictable. As a developer heading into the office, it helps to know what you’re in for - and what to bring with you.

Your task is to build a small weather-checking API using .NET 8. The API should return a basic forecast along with a helpful recommendation for what to wear.

The endpoint should accept a GET request to /weather, with latitude and longitude as query parameters. When called, it should return a 200 OK response with the following:
- The current temperature in Celsius
- The wind speed in kilometers per hour
- A simple weather condition (Sunny, Windy, Rainy, or Snowing)
- A recommendation phrase suggesting what to wear based on the weather

You’ll need to fetch real data using OpenWeatherMap. The free tier is fine for this exercise.

The recommendation should be based on the forecast and should adhere to the following rules:
On a sunny day you should return "Don't forget to bring a hat".
If it’s over 25°C you should return "It’s a great day for a swim".
If it’s less than 15°C and either raining or snowing you should return "Don't forget to bring a coat".
If it’s raining you should return "Don’t forget the umbrella".
To simulate occasional upstream failures, every fifth request to your endpoint should return a 503 Service Unavailable.

Include tests that cover the different kinds of responses - both success and error cases.

Bonus Challenge
You've been using this API via cURL or Postman, but it's time for a proper interface.

As an optional bonus, build a small React app that calls your API and displays the weather and recommendation in a simple, user-friendly way.

Extra marks will be given based on design decisions and adherence to clean architecture.

## Project Structure

This solution is organized using a domain-driven approach, with a focus on modularity, testability, and clean architecture. The main components are:

### Architecture Principles
- Unit tests are written using the Arrange-Act-Assert (AAA) pattern
- City GeoLocation functionality is mocked for an example but was not required for by the brief. (extra)
- Shared interfaces, records, and DTOs promote reusability and separation of concerns
- Follows SOLID and DRY principles
- Uses dependency injection throughout
- Service layer architecture for clean separation of concerns


### DataTorque.Shared
Contains shared interfaces, records, DTOs, and configuration options:


### DataTorque.Services
Implements the core weather logic and integrations:


### WebApplication1 (DataTorque.WeatherApi)
The ASP.NET Core Web API project:


### Weather Suggestion Logic
The `WeatherService` applies the following rules to generate recommendations:


### API Response Example
The `/weather` endpoint returns structured JSON:
```json
{
  "temperature": 18.5,
  "windSpeed": 12.3,
  "condition": "Windy",
  "recommendation": "Don't forget the umbrella.(Not good advise in Windy Welly)"
}
```


