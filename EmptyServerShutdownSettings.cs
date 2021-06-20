using System;
using System.ComponentModel;

namespace EmptyServerShutdown
{
    public sealed class EmptyServerShutdownSettings
    {
        #region Server shutdown
        [Description("Whether the world should be saved on server shutdown.")]
        public bool Save { get; } = true;

        [Description("The shutdown reason which appears in the server logs and the command line.")]
        public string Reason { get; } = "Server shutting down!";

        [Description("Delay of the server shutdown after the last player has left the server.")]
        public TimeSpan ShutdownDelay { get; } = new TimeSpan(days: 0, hours: 0, minutes: 5, seconds: 0);
        #endregion

        #region Logging
        [Description("Log message that should be printed when a server shutdown is scheduled.")]
        public string ConsoleInfoShutdownScheduled { get; } = "Scheduled server shutdown at {0}.";

        [Description("Log message that should be printed when a server shutdown is cancelled.")]
        public string ConsoleInfoShutdownCancelled { get; } = "Cancelled server shutdown at {0}.";

        [Description("The date-time format that should be used when logging.")]
        public string DateTimeFormat { get; } = "yyyy-MM-dd HH:mm:ss";
        #endregion
    }
}
