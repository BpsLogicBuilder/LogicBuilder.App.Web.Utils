# LogicBuilder.App.Web.Utils

[![CI](https://github.com/BpsLogicBuilder/LogicBuilder.App.Web.Utils/actions/workflows/ci.yml/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Web.Utils/actions/workflows/ci.yml)
[![CodeQL](https://github.com/BpsLogicBuilder/LogicBuilder.App.Web.Utils/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Web.Utils/actions/workflows/github-code-scanning/codeql)
[![codecov](https://codecov.io/github/BpsLogicBuilder/LogicBuilder.App.Web.Utils/graph/badge.svg?token=ERODHGSGYY)](https://codecov.io/github/BpsLogicBuilder/LogicBuilder.App.Web.Utils)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=BpsLogicBuilder_LogicBuilder.App.Web.Utils&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=BpsLogicBuilder_LogicBuilder.App.Web.Utils)

A utility library providing commonly-used web functionality for business applications built with LogicBuilder.

## Overview

LogicBuilder.App.Web.Utils includes reusable classes and helper methods designed to perform routine HTTP operations that are commonly needed across business applications. This library targets both .NET Standard 2.0 (for broad compatibility) and .NET 10 (for modern .NET applications).

## Features

### HttpClientHelper

A simplified wrapper around `HttpClient` that provides strongly-typed HTTP operations with built-in JSON serialization/deserialization.

**Key Benefits:**
- Integrates with `IHttpClientFactory` for proper HttpClient lifecycle management
- Automatic JSON serialization and deserialization
- Strongly-typed request and response handling
- Supports custom `JsonSerializerOptions` for flexible JSON configuration
- Throws `InvalidOperationException` on deserialization failures for robust error handling

## Installation

Install via NuGet Package Manager:

Or via Package Manager Console:
Install-Package LogicBuilder.App.Web.Utils

## Usage

### Basic Setup

First, register the `HttpClientHelper` and `IHttpClientFactory` in your dependency injection container:

```c#
services.AddHttpClient(); 
services.AddTransient<IHttpClientHelper, HttpClientHelper>();
```

### GET Request

```c#
    public class MyService { private readonly IHttpClientHelper _httpClientHelper;

    public MyService(IHttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public async Task<MyModel> GetDataAsync()
    {
        var result = await _httpClientHelper.GetAsync<MyModel>(
            "https://api.example.com/data"
        );
        return result;
    }
```

### POST Request

```c#
public async Task<ResponseModel> CreateDataAsync(RequestModel model) 
{ 
    string jsonPayload = JsonSerializer.Serialize(model); 
    var result = await _httpClientHelper.PostAsync<ResponseModel>( "https://api.example.com/data", jsonPayload ); 
    return result; 
}
```

### PUT Request


```c#
public async Task<ResponseModel> UpdateDataAsync(RequestModel model) 
{ 
    string jsonPayload = JsonSerializer.Serialize(model); 
    var result = await _httpClientHelper.PutAsync<ResponseModel>( "https://api.example.com/data/123", jsonPayload ); 
    return result; 
}
```
### Using Custom JsonSerializerOptions
```c#
var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
var result = await _httpClientHelper.GetAsync<MyModel>( "https://api.example.com/data", options );
```

## API Reference

### IHttpClientHelper

#### Methods

- **`GetAsync<TResult>(string url, JsonSerializerOptions? options = null)`**
  - Performs an HTTP GET request and deserializes the response to the specified type
  - Parameters:
    - `url`: The endpoint URL
    - `options`: Optional JSON serializer settings
  - Returns: Deserialized object of type `TResult`
  - Throws: `InvalidOperationException` if deserialization fails

- **`PostAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null)`**
  - Performs an HTTP POST request with JSON content and deserializes the response
  - Parameters:
    - `url`: The endpoint URL
    - `jsonObject`: JSON string to send in the request body
    - `options`: Optional JSON serializer settings
  - Returns: Deserialized object of type `TResult`
  - Throws: `InvalidOperationException` if deserialization fails

- **`PutAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null)`**
  - Performs an HTTP PUT request with JSON content and deserializes the response
  - Parameters:
    - `url`: The endpoint URL
    - `jsonObject`: JSON string to send in the request body
    - `options`: Optional JSON serializer settings
  - Returns: Deserialized object of type `TResult`
  - Throws: `InvalidOperationException` if deserialization fails

## Target Frameworks

- .NET Standard 2.0
- .NET 10

## Dependencies

- `System.Text.Json` - For JSON serialization/deserialization
- `Microsoft.Extensions.Http` - For `IHttpClientFactory` support

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.


## Related Projects

- [LogicBuilder](https://github.com/BpsLogicBuilder) - The main LogicBuilder framework

