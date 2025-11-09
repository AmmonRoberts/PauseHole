# PauseHole App

For the frontend, I use a ReactJS app along with [Bun](https://bun.sh/). You should be able to just use NPM or Yarn based on your preference.

In the 'app/public' directory, you'll find a 'config.sample.json' file. You'll need to update this file with your own PauseHole backend address like so:

```json
{
	"PauseHoleBackendAddress": "192.168.1.46:8080"

```

To start the app by itself, run the following commands in the CLI:

```bash
cd app
bun install
bun start
```