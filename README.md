# MIRA - A Broadcast Box Discord Bot

![Mira Icon](/assets/Mira.png)

Mira is a Discord bot for use with the fantastic [Broadcast Box](https://github.com/glimesh/broadcast-box). It allows you to specify which hosts to monitor and then subscribe to different stream keys.

## Prerequisites
Mira requires you to provide a Discord bot token. You can acquire one by visiting the [Discord Applications Portal](https://discord.com/developers/applications/) and creating a new application.

## Add to server
You can add Mira to your server with [this link](https://discord.com/oauth2/authorize?client_id=1250447229572087808).

> [!WARNING]  
> Mira is still in active development. Breaking changes and data wipes are likely. Use at your own risk! 

## Run this locally
Build and run `Mira` with Docker:
```sh
git clone https://github.com/richardtatum/mira.git mira
cd mira
docker build -t mira .
docker run -e DISCORD__TOKEN='valid-bot-token-here' mira:latest
```
Note that the environment variable `DISCORD__TOKEN` has a double underscore between the two words.

## Usage
Once added, Mira listens for slash commands in any channel. These commands should be automatically populated by Discord into the command pop-up.

### Commands
Mira provides the following slash commands out of the box:

#### /add-host
The `add-host` command allows a user to add a new Broadcast Box host and choose one of the selected polling intervals.

#### /subscribe
The `subscribe` command allows a user to subscribe to changes to a provided stream key on a selected host. When a stream starts on the given host/key, a notification will be sent to the channel the subscription was requested from.

#### /playing
The `playing` command allows a user to set a text value for the 'Currently Playing' section of any live stream. This can be set multiple times to reflect the change in content during a stream, with each new value overriding the last.

#### /unsubscribe
The `unsubscribe` command allows a user to choose a subscription to remove from a list of available subscriptions.

#### /list
The `list` command provides a list of all registered hosts and their subscribed keys.

#### /remove-host
The `remove-host` command allows a user to choose a host to remove, along with any relevant subscriptions.

## Todo
There are still many things to do before this can be considered complete, including but not limited to:
- ~~Add a hosted version that can be added to any server with a simple click~~
- ~~Cleanup the current response embeds~~
- ~~Improve the development experience~~
- ~~Various improvements to the layout of the code~~
- Add tests (in progress)
- Add support for restricting access to the add and remove commands
- Add support for retrieving the last sent frame and using it as the thumbnail for the stream
- Add support for integrating with the [IGDB](https://www.igdb.com/) additional 'Currently Playing' metadata
- Add regular backups

## References
- [Discord.NET](https://docs.discordnet.dev/index.html) 
- [Broadcast Box](https://github.com/glimesh/broadcast-box)
