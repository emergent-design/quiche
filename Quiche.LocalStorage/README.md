Quiche.LocalStorage
===================

#Cacher#

Quiche.LocalStorage.Cacher is an example of an ICache suitable for use by Quiche.Client. It uses OrmLite under the hood and so can connect to any underlying datastore that OrmLite supports; simply instantiate the appropriate type of OrmLiteConnectionFactory and hand it to the Cacher at initialisation.

##Usage##

At the most basic level, you can simply instantiate something that implements the ICache interface and hand it to a Client at initialisation. The client will then use the cache to store settings and build a local store of Users, Terminals and Zones present on the Qui network it connects to. This allows the Client object to provide this data to calling code when disconnected from the network. For example:


```csharp
using (Quiche.Client client = new Quiche.Client())
using (Quiche.Client cachedClient = new Quiche.Client())
{
    //tell the client where it's connecting to
    client.Address = "qui.server.address";
    cachedClient.Address = "qui.server.address";
    
    //we're not providing a cache
    //so initialise will just set the 
    //connection thread in motion
    client.Initialise();
 
 
    //Create a sqlite (or whatever) OrmLiteConnectionFactory that Cacher is
    //going to use under the bonnet to provide storage
    string path  = "path_to_db";
    var storage = new OrmLiteConnectionFactory (path, false, SqliteDialect.Provider );
    
    //Hand the cacher the storage object it should use. Cacher provides
    //logging (ICache doesn't; this is extra functionality). We don't want
    //the cache overflowing with logs, though, so Cacher expects to be
    //told how long to keep logs for before pruning them. We're not hooking
    //logging up, though, so it'll be ignored.
    Cacher cache = new Cacher (storage, new TimeSpan(1) );
      
    //cachedClient IS getting a cache 
    cachedClient.Initialise(cache);
    
    //Tell the client to connect
    client.Connect = true;
    cachedClient.Connect = true
    
    if (client.Connected)
    {
        //Get all the terminals. client will get all
        //terminals currently on the network. cachedClient
        //will ALSO get any terminals that have been seen
        //previously on the network
        var liveTerminals = client.GetAll<Terminal>();
        var allTerminals = cachedClient.GetAll<Terminal>();
    }
    else
    {
        //in this case, the client isn't connected
        //for some reason. As a result liveTerminals
        //will be an empty list
        var liveTerminals = client.GetAll<Terminal>();
        
        //allTerminals, on the other hand, will still provide
        //a list of all the terminals it's ever seen
        var allTerminals = cachedClient.GetAll<Terminal>();
    }
}
```

Generally speaking, it's still best to get things through the Client rather than the Cache directly as the Client will add the data from the Cache on to any live data it retrieves.

You can also use the Cacher to store logs for a limited amount of time, keep track of which terminals seem to be live and store application-specific settings for you:

```csharp
{
    //Create the client that this cache instance is going to be working with
    Quiche.Client client = new Quiche.Client();
    
    //Create a sqlite (or whatever) OrmLiteConnectionFactory that Cacher is
    //going to use under the bonnet to provide storage
    string path  = "path_to_db";
    var storage = new OrmLiteConnectionFactory (path, false, SqliteDialect.Provider );
    
    //Hand the cacher the storage object it should use. Cacher provides
    //logging (ICache doesn't; this is extra functionality). We don't want
    //the cache overflowing with logs, though, so Cacher expects to be
    //told how long to keep logs for before pruning them.
    Cacher cache = new Cacher (storage, new TimeSpan( 7, 0 ,0 ,0 ) );
    
    //Logging isn't a core function of an ICache, though Cacher does 
    //implement it. As a result, you need to manually hook the Cacher
    //LogReceived event handler in to client.LogReceived manually
	client.LogReceived+=cache.Log;
	
	//The same is true of StatusPing events
	this.client.StatusPinged+= (node, terminal) => cache.TerminalStatusPing(node, terminal);
	
	//Initialise the client with the cache. The client will hook the cache
	//into itself such that as it receives data, the cache is updated without
	//intervention by your code.
	client.Initialise(cache);
	
	foreach (Setting setting in cache.Settings)
	{
	   //Apply any application-specific settings you've stashed in the cache
	   if (setting.Id=="my.setting") 
	   {
	       this.Apply(setting.Value);
	   }
	}
	
	//Now continue with initialisation of the Client...
}
```

Storing settings is simple:

```csharp
{
    cache.SaveSetting("setting.id", "setting.value");
}
```

As is saving local User information (such as username):

```csharp
{
    User user = new User(){ Id = id, Name = "Bruce Wayne" };
    cache.Store(user);
}
```
If a user with id already exists, created by a Client recieving it from Qui, then it will simply gain this name, otherwise a new User will be stored. In either case, permissions connected to the User with this id will be stored into the user alongside the name as they are retrieved over time by the Client.

#Logger#

Logger is a very simple example of a logging class. It hooks in to the LogReceived event of a Client and simply spews out the received logs to an appropriately named text file.
