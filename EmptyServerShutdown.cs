#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.Configuration;

namespace EmptyServerShutdown
{
    [ApiVersion(major: 2, minor: 1)]
    [SuppressMessage(category: "ReSharper", checkId: "UnusedType.Global",
        Justification = "Used implicitly as TerrariaPlugin")]
    public sealed class EmptyServerShutdown : TerrariaPlugin
    {
        private EmptyServerShutdownSettings? Settings { get; set; }

        private ScheduledServerShutdown? ScheduledServerShutdown { get; set; }

        private const string SavePath = "empty-server-shutdown";

        public override string Author => "andreblanke";

        public override string Description => "Shuts down the server some time after the last player left the server";

        public override string Name => "Empty Server Shutdown";

        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public EmptyServerShutdown(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            static EmptyServerShutdownSettings LoadConfiguration()
            {
                string configPath = Path.Combine(SavePath, "config.json");

                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);

                var config = new ConfigFile<EmptyServerShutdownSettings>();
                config.Read(configPath, out bool incompleteSettings);

                if (incompleteSettings)
                    config.Write(configPath);

                return config.Settings;
            }
            Settings = LoadConfiguration();

            ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
            ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);

            /*
             * Treat the game initialization the same way as a player that is leaving to prevent the server from
             * running indefinitely if it is lazily activated (e.g. via terrariad) but then no player joins the
             * server.
             */
            ServerApi.Hooks.GamePostInitialize.Register(this, OnServerLeave);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);

                ServerApi.Hooks.GamePostInitialize.Deregister(this, OnServerLeave);

                ScheduledServerShutdown?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnServerJoin(EventArgs args)
        {
            void CancelServerShutdown()
            {
                ScheduledServerShutdown.Cancel();

                TShock.Log.ConsoleInfo(Settings!.ConsoleInfoShutdownCancelled,
                    ScheduledServerShutdown.ScheduledTime!.Value.ToString(Settings!.DateTimeFormat));

                ScheduledServerShutdown.Dispose();
                ScheduledServerShutdown = null;
            }

            if (ScheduledServerShutdown != null)
                CancelServerShutdown();
        }

        private void OnServerLeave(EventArgs args)
        {
            void ScheduleServerShutdown()
            {
                ScheduledServerShutdown = new ScheduledServerShutdown
                {
                    Save   = Settings!.Save,
                    Reason = Settings!.Reason
                };
                ScheduledServerShutdown.Initiate(Settings.ShutdownDelay);

                TShock.Log.ConsoleInfo(Settings!.ConsoleInfoShutdownScheduled,
                    ScheduledServerShutdown.ScheduledTime!.Value.ToString(Settings!.DateTimeFormat));
            }

            /*
             * TShock.Utils.GetActivePlayerCount() returns the active player count before the leave event,
             * i.e. 1 if there are no players left on the server after the leave event.
             */
            if (TShock.Utils.GetActivePlayerCount() <= 1)
                ScheduleServerShutdown();
        }
    }
}
