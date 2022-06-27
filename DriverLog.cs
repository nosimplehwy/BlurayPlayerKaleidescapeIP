using System;
using Crestron.RAD.Common.Logging;
using Crestron.SimplSharp;

namespace BlurayPlayer_Kaleidescape_IP
{
    public class DriverLog
    {

        public static void Log(bool logEnabled, LoggingLevel logLevel, string methodName, string message)
        {
            var logMessage = $"{logLevel}: {methodName} - {message}";

            if (!logEnabled) return;
            switch (logLevel)
            {
                case LoggingLevel.Error:
                    ErrorLog.Error(logMessage);
                    break;

                case LoggingLevel.Warning:
                    ErrorLog.Warn(logMessage);
                    break;

                case LoggingLevel.Debug:
                    ErrorLog.Notice(logMessage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }

        }
    }
}