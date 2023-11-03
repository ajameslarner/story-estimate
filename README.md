# Guesstimate - Story Point Estimator

This is a Blazor .NET Core Web App that uses SignalR to provide a small platform for users to connect to and vote on story point estimations for development cards.

## Features
- **Real-time communication**: Utilizes SignalR for real-time, bi-directional communication between server and client.
- **Story point voting**: Allows users to vote on story point estimations for development cards.
- **User connection**: Provides a platform for multiple users to connect and interact.

## Quick Setup Guide

### Prerequisites
- .NET Core SDK
- Visual Studio Code
- SignalR

### Steps

1. **Clone the repository**
    ```
    git clone https://github.com/ajameslarner/Guesstimate.git
    ```

2. **Navigate to the project directory**
    ```
    cd Guesstimate
    ```

3. **Restore the .NET packages**
    ```
    dotnet restore
    ```

4. **Build the project**
    ```
    dotnet build
    ```

5. **Run the project**
    ```
    dotnet run
    ```

### Hosting Sessions with VSCode Ports Tool

1. **Open the Ports view**
    - Open up the `Ports` panel in the terminal Panel group in vscode (typically at the bottom).
    - Select `Forward a Port`.

2. **Forward a port**
    - Assign the same port running the application on localhost.
    - Once exposed, change the visibility to `public` when hosting a session.
    - You can now share the `Forwarded Address` for public access.

Please note that exposing a port will make your application accessible to anyone with the link. Be sure to secure your application appropriately.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
