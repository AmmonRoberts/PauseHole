# PauseHole App

For the frontend, I use a ReactJS app along with [Bun](https://bun.sh/). You should be able to just use NPM or Yarn based on your preference.

Before running, you'll need a .env file in the `app` directory. Create it like so:

```
REACT_APP_BACKEND_ADDRESS=<Backend Address Or Domain>:8080
```

To start the app by itself, run the following commands in the CLI:

```bash
cd app
bun install
bun start
```