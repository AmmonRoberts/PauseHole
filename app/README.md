# PauseThePi App

For the frontend, I use a ReactJS app along with [Bun](https://bun.sh/). You should be able to just use NPM or Yarn based on your preference.

Before running, you'll need a .env file in the `app` directory. Create it like so:

```
REACT_APP_BACKEND_ADDRESS=<Backend Address>
```

To start the app, run the following commands:

```bash
cd app
bun install
bun start
```