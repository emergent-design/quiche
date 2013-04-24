Quiche.Provider
===============

QuicheProvider is a singleton which provides access to its internal instance of IQuicheProvider. As it stands, only one class implementing IQuicheProvider exists (SqliteBackedPerceptionProx) which uses a Sqlite database and a PerceptionProx prox device. In most cases, this should be all that you need. If you wish to integrate with different databases or prox devices, you'll have to roll your own provider. SqliteBackedPerceptionProx should provide a reasonable reference implementation.

##Using Quiche.Provider##
**QuiRing** is an example application built using QuicheProvider which is probably the best point of reference. The absolute basics are, however:

```csharp
public MyQuicheApp()
{
    //Initialise the QuicheProvider
    QuicheProvider.Initialise<SqliteBackedPerceptionProx>();
    
    
    //Hook in your event handlers for proxcard events...
    
    //Your OnCardProgress event handler should report back to the 
    //user that progress is being made writing the card.        
    QuicheProvider.Instance.CardProgress += this.OnCardProgress;
    
    //Use the OnCardOperation event handler to respond to card reads
    //and writes starting and ending
    QuicheProvider.Instance.CardOperation += this.OnCardOperation;
    
    //Your OnCardRead event handler will receive the card information
    //retrieved from the proxcard when a read has successfully completed
    QuicheProvider.Instance.CardRead += this.OnCardRead;
    
    //ProxConnectionChanged will fire when the proxcard is connected or
    //disconnected
    QuicheProvider.Instance.ProxConnectionChanged += this.OnProxConnection;
    
    //Client.ConnectionChanged will fire when the network connection to Qui
    //is either made or lost/disconnected
    QuicheProvider.Instance.Client.ConnectionChanged += this.OnQuiConnection;
    
    //Add any extra log handling you want in your OnLog handler
    QuicheProvider.Instance.Client.LogReceived += this.OnLog;
    
    //Though you don't need to worry about logging because 
    //the provider will log to text file fo you
    QuicheProvider.LogPath = "location_for_logging";
    
    //Initialise and begin Quiche...
    QuicheProvider.Instance.Initialise("myQuicheApp");
    
    //Do whatever other initialisation you need to do...
}
    
```