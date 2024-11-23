# PauseHole

This is an app I made so that I (or other people in my home) can use to easily pause PiHole ad blocking without me needing to do it every time. The backend gets the list of PiHole devices from configuration and attempts to pause/unpause ad blocking and check the status for all devices.

There's currently not very much useful error handling.


## Backend

For the backend, I use a very simple C# 8 minimal API to interact with each PiHole so API keys and other secret values aren't contained in the frontend. We also use it to deal with any issues with CORS (it's a huge pain and you have to do it on each PiHole if you want to talk to the API from a browser).


## Frontend

I use a ReactJS frontend for this project. It has a very simple (and probably a little ugly) interface that shows the status of each PiHole and allows you to pause/unpause the PiHoles in a single click.


## .NET Aspire

This project uses .NET Aspire, which makes running the app very simple. Before running the project, you'll need to set up your configuration for both the frontend and backend.

For frontend setup, see the [app README](./app/README.md).

For backend setup, see the [API README](./API/README.md).

Once you have the back- and frontend set up, to build and run them together using .NET Aspire, select PauseHole.AppHost as your startup project in Visual Studio, or execute the following commands in the CLI:


```bash
cd PauseHole.AppHost
dotnet build
dotnet run
```

<!--
### Backend

```bash
docker build --target backend-runtime -t pausehole-backend .
docker run -p 8080:8080 pausehole-backend
```


### Frontend

```bash
docker build --target app-runtime -t pausehole .
docker run -p 80:80 pausehole
```
-->

## Credit

Thanks to [@ericpattison](https://github.com/ericpattison) for helping me come up with the most perfect name possible.