Quiche: The Qui Cache Provider
==============================

Psiloc is a networked biometric access control system provided by [Perception-SI Ltd](http://www.perception-si.com) which uses a combination of prox cards and face recognition. It's unusual in that rather than use an admin application, enrolment, permissions management, etc, are all performed simply by presenting prox cards to a Psiloc terminal on the network. This means that as well as access being secured by biometrics, even account management and setup is biometrically secured.

Psiloc also exposes some information about what Qui (the software which manages the Psiloc hardware) is doing for consumption by third-party data management so that the terminals can be integrated seamlessly into a complete system.

So why is Quiche called Quiche? Honestly, it's because... well... who doesn't like quiche?

**Quiche** provides a simple API for connecting to a Qui server, monitoring and caching the data produced by Qui, and writing proxcards for use with the system.

**QuiRing** is an example winforms application which demonstrates the capabilities of Quiche. For many applications, QuiRing is more than sufficient for all card issue and data logging. Should you wish to roll your own application, or integrate Quiche into a larger solution, it's worth taking a look at QuiRing. Whilst much of the code is standard winforms gui implementation, the sections dealing specifically with Quiche are likely to be helpful for reference.

If you wish to roll your own solution without using Quiche at all, the following information is probably useful:

# Qui Websocket Publication #

Whenever an interesting event occurs, Qui publishes a structured notification over a websocket running at the root address on port 8181. In addition to this log feed a status socket publishes over the /status/ location on the same port. This broadcasts a simple status message each second from every Psiloc terminal attached to the network so any failed terminals are readily apparent. 

Status pings are of the form `HubSerial, PsilocSerial` - i.e., the serial number of the processing hub that the Psiloc terminal is connected to and the serial number of the terminal itself. Several terminals may be connected to the same processing hub, so if you suddenly see every terminal on a hub go down then it's likely that something has happened to the hub (such as a power outage), whereas if you still see all of the other terminals on a hub responding correctly then it's an issue with the Psiloc terminal itself (a severed cable for example).

A less structured, human-readable info feed is also available over a websocket at the root of port 8282 though this is unlikely to be useful for integration into other software.

##Understanding the log information##

Log messages are of the form:

`Event, Source, User, Result`

* *Event* is string which denotes what type of event Qui is reporting.
* *Source* is the ID of the Psiloc terminal which caused the event.
* *User* is the ID of the user that is being affected by the event.
* *Result* is the outcome of the event.

If a notification has no useful information to present for a given field, the string will be empty (for example, Qui can timeout from an admin mode and switch back to its idle state without intervention. In this case, there will be no *User* information. There is no outcome from switching to the Idle state, so the *Result* will also be empty.

###Immediate events##

These events will be published as they happen

| Event         | Description                    | Source          | User       | Results                         |
|---------------|--------------------------------|-----------------|------------|---------------------------------|
| **Starting**  | A terminal is starting up      | Terminal ID     | Empty      | Empty                           |
| **Exiting**   | A terminal is closing down     | Terminal ID     | Empty      | Empty                           |
| **LockFault** | A fault has occurred on the secure lock attached to a terminal | Terminal ID | Empty | The type of fault reported (*Communications*, *Initialisation* or *Tamper*; see below) |
| **Mode**      | A terminal has switched mode   | Terminal ID     | The ID of the user who instigated the change (Empty if no user is responsible) | The new mode |
| **Verify**    | A User has attempted to verify | Terminal ID     | The ID of the user | *Invalid*, *Denied*, *Fail* or *Success* |
| **Unlock**    | A terminal with no camera has been accessed | Terminal ID | The ID of the user | *Invalid*, *Denied* or *Success* 

###Deferred events###
These events are published when they are authorised by an administrator rather than as they occur. Qui does not allow any modification of its internal data store without administrator authorisation; instead it caches these transactions until the administrator that authorised the current admin session re-presents their admin card to verify that they are happy to commit the changes. If they do not present their card, or present a control card to signal they are unhappy with the transactions, Qui drops the changes.

| Event         | Description                                | Source          | User       | Results             |
|---------------|--------------------------------------------|-----------------|------------|---------------------|
| **Enrol**     | A user has been enrolled                   | Terminal ID     | User ID    | *Success* or *Fail* |
| **Revoke**    | A user has been removed                    | Terminal ID     | User ID    | *Success* or *Fail* |
| **Access**    | User access rights have been modified      | Terminal ID     | User ID    | *Success* or *Fail* |
| **AdminGrant**| A User has been granted admin rights       | Terminal ID     | User ID    | *Success* or *Fail* | 
|**AdminRevoke**| Admin rights have been removed from a User | Terminal ID     | User ID    | *Success* or *Fail* | 

### Verification results ###
* *Invalid* - The card presented does not correspond to a user enrolled on the system. Psiloc will signal a failure to verify and not unlock any attached locking devices (without biometrically verifying who the bearer of the card is).
* *Denied* - The card belongs to a user without access rights to use this terminal. Psiloc will signal a failure to verify and not unlock any attached locking devices (without biometrically verifying who the bearer of the card is).
* *Fail* - The person presenting the card does not appear to be the authorised user. Psiloc will signal a failure to verify and not unlock any attached locking devices.
* *Success* - The card belongs to a valid user with rights to access this terminal and appears to have been presented by the the user in question. Psiloc will at this point unlock any attached locking devices.

### Unlock events ###
Unlock events are unique to versions of Psiloc that don't physically have a camera. The Qui network has to be specifically configured to allow this arrangement as for security these units are untrusted by default (see the documentation provided by Perception for details), but the result is that you have terminals that act like simple card-operated access points. Present a card and if it belongs to a user with permission to access that terminal then the unit will signal success and unlock any locks. It works exactly like **Verify**, but since there's no biometric verification of the bearer *fail* isn't an exit code that can occur.

### Modes of operation in Qui ###
Qui has six modes of operation:

* **Normal** - Normal operating mode (awaits a card. When a user presents their card, biometric verification will take place)
* **Initialise** -Initialisation mode; no administrators have been enrolled on the system. The first person to enrol will be granted admin rights.
* **Enrol** - The system is currently enrolling new users as cards are presented
* **Revoke** - The system is currently removing existing users as their cards are presented
* **Admin** - The system is currently toggling admin status for existing users as their cards are presented
* **Access** - The system is currently updating access rights for users when their cards are presented (see the proxcard section below).


### Types of lock fault ###
Psiloc optionally includes a "secure lock" - this is a physical actuator that is connected to a Psiloc terminal over an encrypted channel. "Lock faults" are fault signals emitted which specifically refer to this device.

A *Communications* fault is most likely a transient issue caused by environmental noise and can be ignored unless it recurs. An *Initialisation* fault signals that the Psiloc unit is unable to perform a secure handshake with its secure lock; most likely the lock has been initialised already. Resetting the lock to factory defaults should resolve this. *Tamper* signals that whilst Psiloc is configured to expect a secure lock to be present, it appears to be physically absent.


# Qui Watchman REST Interface #
In addition to the event-based data published over websockets a REST interface is exposed over port 80 at /watchman. This simply allows you to GET lists of all of a type of entity currently stored in Qui; *Users*, *Zones* or *Terminals*. User is self-explanatory, as is Terminal. A Zone is a group that Terminals can belong to. Granting permission for a User to access a Zone grants access to all terminals in the Zone.

```HTML 
GET Address.of.qui/watchman/Users
```

Would return something along the lines of:
```JSON
    {
        "Id": 2205639807,
        "Name": "Bruce Wayne",
        "ZoneWhitelist": [],
        "TerminalWhitelist": []
    },
    {
        "Id": 3768738943,
        "ZoneWhitelist": [],
        "TerminalWhitelist": []
    }
``` 
With similar responses from .../Zones and .../Terminals.

Watchman also provides a GET for Ping; this doesn't actually return anything but is useful for detecting a broken connection (as an error will occur if no response is forthcoming).


# Proxcard Data Format #

By default Psiloc simply reads the unique ID hardwired into each prox card. For anything other than standard User cards, further data must be written to the card at these locations:-

| Name      |  Page    | Length (bytes) | Description                                                                        |
|-----------|----------|----------------|------------------------------------------------------------------------------------|
| PIN       | 0x00     | 4              | The ID of the card. Cannot be overwritten. For **user** cards, this is also the PIN|
| Type      | 0x10     | 4              | Code specifying the type of card (see below)                                       |
| ProxyPIN  | 0x11     | 4              | PIN number; ignored for **user** cards                                             |
| Terminals | 0x14     | 16             | A list of Terminal reference IDs that the User should be granted access to. Each reference ID is one byte. Zero bytes are ignored. |
| Zones     | 0x14     | 16             | A list of Zone IDs that the User should be granted access to. Each ID is one byte. Zero bytes are ignored. |

Types that can be specified in the *Type* field are:-

| Type      |  Code      | Description                          |
|-----------|------------|--------------------------------------|
| User      | 0x00000000 | A standard User card. Default.       |
| Proxy     | 0x10101010 | Behaves exactly like a user card, but uses the ProxyPIN field for the PIN number. This allows replacement user cards to be created, provided the original PIN number has been recorded somewhere. |
| Enrol     | 0x09090909 | Switches the system into Enrol mode |
| Verify    | 0x03030303 | Switches the system into Verify mode (the normal operating mode) |
| Revoke    | 0x0A0A0A0A | Switches the system into Revoke mode (for removing users) |
| Admin     | 0x0F0F0F0F | Switches the system into Admin Toggle mode (grant or remove admin rights from users) |
| Access    | 0x1F1F1F1F | Switches the system into Access Permissions mode (for assigning new zone/terminal access rights to users) |
