Quiche.Proxcard
===============

Quiche.Proxcard allows you to read from and write to proxcards. At present, the only type of prox card reader supported is PerceptionProx, a prox reader supplied by [Perception-SI Ltd](http://www.perception-si.com). If you wish to interface with a different hardware device, you will need to implement your own IProx device to replace PerceptionProx.

#Using ProxInput#

ProxInput is a class which wraps an IProx device, and provides a polling thread so you don't need to worry about threading requests for yourself. It's fairly simple to use; in general you simply tell ProxInput what you want it to do and then at some point later an event will fire to let you know that it's either done what you asked or an error has prevented it from doing what you asked. SqliteBackedPerceptionProx in the Providers project is a good reference for use of ProxInput, but the basics are:


```csharp
public MyProxReaderThing : IDisposable
{
    protected ProxInput cardReader = new ProxInput();

    public MyProxReaderThing(string connection)
    {
                //Signal card operation progress
    				this.cardReader.CardProgress += (error, progress, read) => {
					Console.WriteLine("{0}, {1} percent complete. {2}", 
					   read ? "Read": "Write", progress, error ? "ERROR": "Working...");
				};
				
				//Handle card read completion
				this.cardReader.CardRead += (tokenType, pin, units, zones) => {
				    Console.WriteLine("Card Read: {0} card with pin {1}. {2} units and {3} zones are whitelisted", 
				        tokenType, pin, units.Count, zones.Count);
					this.cardReader.Pause();
				};
				
				//Handle card write completion
				this.cardReader.CardWritten += (success) => {
					Console.WriteLine("Write {0}", success ? "Completed: "Aborted");
					this.cardReader.Pause();
				};
				
				//Handle aborted operations...
				this.cardReader.CancelCardOperation += (writeAborted) => {
					Console.WriteLine("{0} operation aborted", writeAborted ? "Write": "Read");
					this.cardReader.Pause();
				};
				
				//Connect to the specified port
				this.Connect(connection);
    }

    //Connecting to a prox reader
    public void Connect(string commPort)
    {
        //Make sure we're not connected to anything
        //(and dispose any IProx we are connected to)
        this.cardReader.Disconnect();
        
        //Create a new prox reader
        var device = new PerceptionProx();
        
        //Initialise it with the port it's connecting to, and test 
        //that it seems to be a working connection
        if (device.Initialise(commPort) && device.TestConnection()) 
        {
            //cardReader is going to take posession of device
            //so we don't need to worry about disposing it
            //once we've connected
            this.cardReader.Connect(device);
        }
        else
        {
            //But if we're not connecting to it, we should dispose it
            device.Dispose();
        }
    }
    
    //Call this to read a card
    public void Read()
    {
        //read a card
        this.cardReader.Read();
    }
    
    //Call this to write a card
    public void Write(TokenType tokenType, uint pin = 0, List<ushort> zones = null, List<ushort> terminals = null)
    {
        //write a card with the specified tokentype, pin number and zone/terminal whitelists
        this.cardReader.WriteCard(tokenType, pin, zones, terminals);
    }
    
        
    public void Dispose()
    {
        //Disposing cardReader disposes
        //any connected IProx
        this.cardReader.Dispose();
    }
}
    
```