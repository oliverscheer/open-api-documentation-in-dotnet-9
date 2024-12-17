# Open API Documentation and Testing in .NET 9.0

## **Introduction to OpenAPI Support in ASP.NET Core 9**

With the release of **.NET 9**, the ASP.NET Core team has removed built-in support for **Swagger** via Swashbuckle. This decision stems from several factors, including:

- **Maintenance Challenges**: The Swashbuckle project is no longer actively maintained, with unresolved issues and no official release for .NET 8.
- **Built-in Metadata Support**: ASP.NET Core has evolved to include native support for the metadata required to describe APIs, minimizing the need for external tools.
- **Focus on OpenAPI**: Microsoft is prioritizing OpenAPI as a first-class feature in ASP.NET Core, enhancing its capabilities with `Microsoft.AspNetCore.OpenApi` for OpenAPI document generation.
- **New Alternatives**: Visual Studio now includes tools like **.http file support** and the **Endpoints Explorer**, enabling developers to explore, test, and debug APIs more effectively without additional packages.
- **Encouraging Community Innovation**: By removing the default Swashbuckle dependency, the team hopes to foster innovation in community-driven OpenAPI tools.

### **About This Project**

This project demonstrates how to adapt to the new direction of **ASP.NET Core** by leveraging **built-in OpenAPI capabilities** and alternative tooling. It serves as an example for developers looking for modern, lightweight approaches to generate, explore, and document their APIs without relying on Swashbuckle or other external dependencies.

### Samples Included in This Project

To help developers adapt to the changes in .NET 9, this project includes three samples:

1. Using .http-Files in Visual Studio Code and Visual Studio, which is slightly better in Visual Studio Code.
2. Re-adding **Swagger** Support: A sample demonstrating how to integrate Swagger (Swashbuckle) back into your project, for those who still prefer using it despite its removal.
3. Introducing **Scalar**: An alternative OpenAPI tool that offers powerful features and a modern experience for API exploration, testing, and documentation. Scalar is a great option for developers looking for a feature-rich and well-maintained replacement for Swagger.

## Using .http - Files

**`.http` files** are plain text files used to define and execute **HTTP requests** directly from an editor like **Visual Studio Code** with the **REST Client** extension. They allow developers to write, test, and debug API calls in a simple, human-readable format.

It also works in Visual Studio, with some limitations that I will explan later.

### **Key Features:**

- Write HTTP methods like `GET`, `POST`, `PUT`, etc., with headers and body.  
- Execute requests directly from the file.  
- Supports variables, environment configurations, and response extraction.

---

**Example of a .http file:**
```http
### GET Request
GET https://api.example.com/users
Accept: application/json

### POST Request with JSON Body
POST https://api.example.com/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com"
}
```

`.http` files simplify API testing and are a lightweight alternative to tools like Postman.

To test my API I added to files. One for Visual Studio which is not able to work with response variables (which are great), and one for Visual Studio Code.

```text
@hostaddress = https://localhost:5555/calculation
@value1 = 20
@value2 = 22

### Randomvalue
GET {{hostaddress}}/randomvalue
Accept: application/json

### Randomvalue with range
POST {{hostaddress}}/randomvalueinrange
Content-Type: application/json

{
  "min": 1000,
  "max": 2000
}

### Add
# @name add
POST {{hostaddress}}/add
Content-Type: application/json

{
  "value1": {{value1}},
  "value2": {{value2}}
}


### Add 2
POST {{hostaddress}}/add
Content-Type: application/json

{
  "value1": {{value1}},
  "value2": {{value2}}
}

### Subtract
POST {{hostaddress}}/subtract
Content-Type: application/json

{
  "value1": {{value1}},
  "value2": {{value2}}
}

### Multiply
POST {{hostaddress}}/multiply
Content-Type: application/json

{
  "value1": {{value1}},
  "value2": {{value2}}
}

### Divide
POST {{hostaddress}}/divide
Content-Type: application/json

{
  "value1": {{value1}},
  "value2": {{value2}}
}

### Divide by zero
POST {{hostaddress}}/divide
Content-Type: application/json

{
  "value1": 42,
  "value2": 0
}
```

Opening this file in Visual Studio enables you to see some more information.

![HTTP-File in Visual Studio](images/http-files-in-visual-studio.png)
You can instantly run the request or start debugging.

One big difference of Visual Studio and Visual Studio Code is ... that the Rest Client Extension in Code is leveraged to work with http-files. 
And this enables you to work with response variables. 

The following code is an example for this, it is exact the same code as above with two differences:

```text
### Add
# @name add
POST {{hostaddress}}/add
Content-Type: application/json

{
  "value1": {{value1}},
  "value2": {{value2}}
}

### Add 2
@addresult = {{add.response.body.result}}

POST {{hostaddress}}/add
Content-Type: application/json

{
  "value1": {{addresult}},
  "value2": {{addresult}}
}
```

By adding the `# @name somename` to the request, you define a variable that contains the reponse of the request.

In the next request you can create a new variable and query parts of the response and use it in another call. Which is excellent if you work with dynamic generated objects and get random guid back. Here I just use the `@addresult` as new input to the same api request.

I don't know when the support of this is coming to Visual Studio, but I hope soon.

## Swagger

Swagger is a good old friend for API developer. It enables you ... if the api and routes are configured correctly ... to "see" and "test" the api.

Swagger is an open-source framework for designing, documenting, and testing RESTful APIs. It uses the OpenAPI Specification to provide clear, interactive, and machine-readable API documentation. Tools like Swagger UI allow you to explore and test endpoints directly in the browser, improving developer productivity and collaboration.

Learn more at <swagger.io>.

It is very easy to explore the existing API definitions.

![Swagger UI](images/swagger-1.png)

You also get a very good example input and UI for your methods. If you like you can also see the `curl` statement you used for a call.

![Swagger UI](images/swagger-2.png)


### Manuall Steps to add Swagger back to your project

1. Add required Nuget-Packages for OpenAPI Documentation

   - Microsoft.AspNetCore.OpenApi
   - Swashbuckle.AspNetCore

    Installation with PowerShell

    ```powershell
    Install-Package Microsoft.AspNetCore.OpenApi
    Install-Package Swashbuckle.AspNetCore
    ```

    Installation with .NET CLI

    ```bash
    dotnet add package Microsoft.AspNetCore.OpenApi
    dotnet add package Swashbuckle.AspNetCore
    ```

1. Update the `program.cs`

    ```csharp
    using Microsoft.OpenApi.Models;

    namespace OliverScheer.OpenApiSample;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add Swagger
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Oliver Scheer Sample API",
                    Version = "v1",
                    Description = "API to to demonstrate some features."
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapRazorPages()
            .WithStaticAssets();

            app.MapGroup("/api/v1").MapGet("/helloworld", () => "Hello Hello!");
            app.MapGroup("/api/v1").MapGet("/time", () =>
            {
                return Results.Ok(
                    new
                    {
                        Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    });
            });

            app.MapControllers();

            app.Run();
        }
    }
    ```

## Scalar

Scalar is an open-source API platform designed to enhance the developer experience by providing modern tools for API interaction and documentation.

**Key Features:**

- **Modern REST API Client:** Scalar offers an intuitive interface for testing and interacting with APIs, streamlining the development process.

- **Beautiful API References:** It generates aesthetically pleasing and user-friendly API documentation, making it easier for developers to understand and utilize APIs.

- **First-Class OpenAPI/Swagger Support:** With robust support for OpenAPI specifications, Scalar ensures seamless integration and accurate API representations.

Additionally, Scalar integrates with various API frameworks, including .NET, allowing for easy incorporation into existing projects.

For more information and to explore Scalar's capabilities, visit their GitHub repository.  

The UI it generates is a little modern and brings also "dark mode" for real engineers.

![scalar 1](images/scalar-1.png)

![scalar 2](images/scalar-2.png)

One (of many) highlights is the posibility to create over 25 different languages or frameworks samples from your code.

For example in C#:

```csharp
using System.Net.Http.Headers;
var client = new HttpClient();
var request = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("https://localhost:5555/api/v1/time"),
};
using (var response = await client.SendAsync(request))
{
    response.EnsureSuccessStatusCode();
    var body = await response.Content.ReadAsStringAsync();
    Console.WriteLine(body);
}
```

Or JavaScript/jQuery:

```javascript
const settings = {
  async: true,
  crossDomain: true,
  url: 'https://localhost:5555/api/v1/time',
  method: 'GET',
  headers: {}
};

$.ajax(settings).done(function (response) {
  console.log(response);
});
```

### Manual Steps to add Scalar to your Web API Project

With PowerShell

```powershell 
Install-Package  package Scalar.AspNetCore
```

With .NET cli

```bash
dotnet add package Scalar.AspNetCore
```

### Update Your `program.cs`

```csharp
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace OliverScheer.OpenApiSample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.MapGroup("/api/v1").MapGet("/helloworld", () => "Hello Hello!");
        app.MapGroup("/api/v1").MapGet("/time", () =>
        {
            return Results.Ok(
                new
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
        });

        app.MapControllers();

        app.Run();
    }
}
```

## Summary

It is very simple to re-activate Swagger in your API-Projects. But with .http-Files and Scalar you have two very good and modern options to test you API's to. 

You can find my sourecode for this project here: 