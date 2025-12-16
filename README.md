# Demo API

## Quick Start with GitHub Codespaces

1. Click the "Code" button on the GitHub repository page.
2. Select "Open with Codespaces" and create a new codespace.
3. Once the codespace is ready, open a terminal and run:
   ```
   cd Demo-api
   dotnet run
   ```
4. The API will start on http://localhost:5203. In Codespaces, the port will be forwarded automatically.

## Local Development

If running locally:

1. Ensure you have .NET 8.0 SDK installed.
2. Clone the repository.
3. Navigate to the Demo-api folder.
4. Run `dotnet restore` to restore packages.
5. Run `dotnet run` to start the application.

## Testing the API

Once the API is running, you can test it in several ways:

### Using Swagger UI
- Open your browser and go to `http://localhost:5203/swagger` (or the forwarded URL in Codespaces).
- This provides an interactive interface to explore and test the API endpoints.

### Using REST Client Extension
- Install the "REST Client" extension in VS Code if not already installed.
- Open the `Demo-api.http` file.
- Click "Send Request" next to the GET request to test the `/weatherforecast` endpoint.

### Using curl
- In a terminal, run:
  ```
  curl http://localhost:5203/weatherforecast
  ```

## Project Structure

- `Demo-api/`: Main application folder.
- `Controllers/`: API controllers.
- `WeatherForecast.cs`: Sample model.
- `Program.cs`: Application entry point.
- `Demo-api.http`: HTTP requests for testing.