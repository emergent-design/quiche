Quiche.Client
=============
Quiche.Client provides a complete client capable of connecting to the both log and status websocket channels and the REST interface of a Qui server. In essence it provides access to everything that you could possibly want to watch on a Qui network.

##Using Quiche.Client##
In many cases you'll actually want to use a full IQuicheProvider, but should you wish to roll your own cache and logging solutions you can simply provide an ICache object and hook into the appropriate events that Quiche.Client fires. It's possible to run the client with no local cache if you want to for some reason though be aware that with no ICache present, the client will not even be able to persist its own connection settings (it normally stashes these as Settings inside the ICache and then automatically applies them at initialisation if the appropriate Settings exist in the provided cache)

The client runs in it's own thread, which is started at initialisation. From that point on, until you tell it to exit, the client will attempt to stay connected (or reconnect if the connection is lost) as long as you have set the Connect flag to true.

To create a client that has no cache is simple:

```csharp
using (Quiche.Client client = new Quiche.Client())
{
    //tell the client where it's connecting to
    client.Address = "qui.server.address";
    
    //we're not providing a cache
    //so initialise will just set the 
    //connection thread in motion
    client.Initialise();
    
    //Hook in our event handler for connection
    //events (so we can respond to disconnection
    //or connection with the server appropriately)
    client.ConnectionChanged += this.OnConnection;
    
    //Hook in the event handler for log events.
    //We probably want to write them to file.
    client.LogReceived += this.OnLog;
    
    //Hook in the event handler for status
    //ping events - these are for keeping
    //track of which terminals are alive
    client.StatusPinged += this.OnStatusPing;
    
    //Tell the client to connect
    client.Connect = true;
    
    while(!this.exit)
    {
     //do stuff...
    }
}
```

If you want to include a cache, just pass an ICache object in to the Initialise call. See Quiche.LocalStorage for an example.

Once you have a connected client as well as waiting for events to handle you can also request information about what's present in the Qui network you're now connected to:

```csharp
//Get all the terminals
var terminals = client.GetAll<Terminal>();

//Get all the users
var users = client.GetAll<User>();

//Get all the zones
var zones = client.GetAll<Zone>();
```

If you know which individual instance you want, just get it by ID:

```csharp
string id = "theidyouwant"
var terminal = client.Get<Terminal>(id);
```
For all of these calls, if you've provided an ICache to the Client at initialisation, it will augment the received data with any present in its cache. This means if you're disconnected at the moment, you should still have access to the last known data.