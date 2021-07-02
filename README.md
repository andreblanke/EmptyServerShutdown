# Empty Server Shutdown

A [TShock](https://github.com/Pryaxis/TShock) plugin that schedules a server shutdown a
configurable amount of time after the last left the server.

## About

This plugin is intended to be used in conjunction with a daemon that automatically launches
certain programs – TShock in this case – when connection attempts are made on certain ports.
[systemd-socket-proxyd](https://www.freedesktop.org/software/systemd/man/systemd-socket-proxyd.html)
is one example of such a daemon.

It is targeted at systems running TShock in more memory-constrained environments, such as on
Raspberry Pis.

## Installation

Navigate to the [latest release](https://github.com/andreblanke/EmptyServerShutdown/releases/latest),
download `EmptyServerShutdown.dll`, and drop it into the `ServerPlugins` folder in your TShock root
directory.

## Configuration

The plugin configuration file `config.json` is located under the `empty-server-shutdown` folder of
your TShock installation and allows configuration of most aspects of this plugin. The table below
lists the currently available configuration options.

| Option                       | Description                                             | Default value                         |
|------------------------------|---------------------------------------------------------|---------------------------------------|
| Save                         | If the world should be saved on server stop             | `false`                               |
| Reason                       | Shutdown reason logged printed to console and log files | `"Server shutting down!"`             |
| ShutdownDelay                | Delay of shutdown after last player left server         | `"00:05:00"`                          |
| ConsoleInfoShutdownScheduled | Log message printed when server shutdown is scheduled   | `"Scheduled server shutdown at {0}."` |
| ConsoleInfoShutdownCancelled | Log message printed when server shutdown is cancelled   | `"Cancelled server shutdown at {0}."` |
| DateTimeFormat               | Date-time format that should be used when logging       | `"yyyy-MM-dd HH:mm:ss"`               |
