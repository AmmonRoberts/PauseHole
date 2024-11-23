# PauseHole Backend

This backend is a C# 8 minimal API and very easy to set up.

To run it, all you need is to add some config. Here's an example of how that might look:

```json
{
	"PiHoleSettings": {
		"PiHoles": {
			"PiHole0": {
				"URL": "<PiHole0 Address>",
				"APIKey": "<PiHole0 API key>"
			},
			"PiHole1": {
				"URL": "<PiHole1 Address>",
				"APIKey": "<PiHole1 API key>"
			}
		}
	}
}
```

Once you have that, if you'd like the run the API by itself and are using Visual Studio, you can just run it the usual way by hitting F5 after selecting API as the startup project. If you're using a CLI, run the following commands:

```bash
cd API
dotnet build
dotnet run
```
