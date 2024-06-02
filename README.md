# PauseThePi

This is an app I made so that I (or other people in my home) can use to easily pause PiHole ad blocking without me needing to do it every time. The backend gets the list of PiHole devices from configuration and attempts to pause/unpause ad blocking and check the status for all devices.

There's currently not very much effective error handling.


## Backend

For the backend, I use a very simple C# 8 minimal API to interact with each PiHole so API keys and other secret values aren't contained in the frontend. We also use it to deal with any issues with CORS (it's a huge pain and you have to do it on each PiHole if you want to talk to the API from a browser).


## Frontend

I use a ReactJS frontend for this project. It has a very simple (and probably a little ugly) interface that shows the status of each PiHole and allows you to pause/unpause the PiHoles in a single click.