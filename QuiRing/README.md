QuiRing
=======

QuiRing is an example application built using Quiche and winforms. It should work on both .net and mono. On Windows you should just be able to run the .exe (but make sure the appropriate sqlite dll is copied into the build folder alongside the .exe - not an issue in Linux). On linux, you'll need mono installed, then just run `mono QuiRing.exe`. If you wish to wipe the current cache, pass in a wipe argument at startup thus: `mono QuiRing.exe wipe`

QuiRing provides the following features:

* Card creation
* Logging
* Local caching of Qui data
* Terminal status monitoring
* User management
* Data export and import

#Connecting to a prox reader#

You'll need a compatible prox reader ( [Perception-SI Ltd](http://www.psi-ltd.com) supply prox readers; you may be able to find other suppliers ). To connect, click on the Card Reader dropdown in the status bar at the bottom of the QuiRing window. Select the serial port that the prox reader is connected to. QuiRing will automatically try to reconnect upon startup if you close it down whilst connected.

#Connecting to Qui#

Connect to the Qui server through the Logs tab. Set the address in the Qui Watchman Address field (`192.168.0.16` for example) and click the Connect button. QuiRing will attempt to maintain this connection until you click the Connect button again (It'll be labelled "Disconnect" if a connection has been established or "Cancel" if QuiRing is still trying to connect)

#Logging#

Set the location that QuiRing logs to by clicking the "Set Log Path" button on the Logs tab. Logs are written to plain text files, one file per day, at the path specified. Filenames are automatically generated and contain the date of creation:- "24-Mar-2013.log" for example.

#Cache Export and Import#

The entire current cache can be backed up to an XML text file. Click "Export Database" on the Logs tab. An exported cache can be re-imported using the "Import Database" button on the same tab. Note that importing will replace, not augment, whatever is currently cached.


#User management#

Users can be managed from the Users tab. Here you can save usernames to cache, to make users easier to identify in QuiRing, issue cards, write access permissions to cards for use at enrolment or access modification, and issue replacement cards for lost or stolen cards. Best practise is to record a users details in QuiRing *as you issue them their first card*. This means that should they lose their card - or refuse to hand it over when the time comes to revoke their access rights - you can simply generate a proxy card as a replacement.

If a prox reader is connected, you can read a card to retrieve a User; just click the Read Card button. If you don't have a card, or a prox reader is not connected, you can also find a previously created User by clicking on "Find User". Use the name box at the top of the Find User form to enter a string which is used to filter the results by name.

#Control card creation#

Control cards - cards used to put a Psiloc terminal into one of the special administrative modes such as Enrol mode - can be written from the Control Cards tab.

#Reading and writing cards#

When reading or writing cards, begin the operation in QuiRing *first* with the card absent from the reader device, then introduce the device. The prox reader code uses some key up/key down logic to make sure that operations aren't accidentally repeated when a card is left in place. As a result, it's possible that the reader will simply ignore the card if you leave it on the device before starting an operation.

#Terminal status#

The status of every terminal that QuiRing has seen on the network can be found in the Units tab. Each terminal is listed along with its current connection status and the serial number of the processing hub it is connected to.