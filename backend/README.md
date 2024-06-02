# PauseThePi Backend

This backend is a C# 8 minimal API and very easy to st up.

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

Once you have that if you're using Visual Studio, you can just run it the usual way by hitting F5. If you're using a CLI, run the following commands:

```bash
cd backend/API
dotnet build
dotnet run
```