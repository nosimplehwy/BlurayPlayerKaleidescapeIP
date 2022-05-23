using Crestron.RAD.Common.Interfaces;
using Crestron.RAD.Common.Logging;
using Crestron.RAD.Common.Transports;
using Crestron.RAD.DeviceTypes.BlurayPlayer;
using Crestron.SimplSharp;

namespace BlurayPlayer_Kaleidescape_IP
{
    public class KaleidescapeTcp : ABasicBlurayPlayer, ITcp
    {
        #region ITcp Members

        public void Initialize(IPAddress ipAddress, int port)
        {
            EnableLogging = true;
            var tcpTransport = new TcpTransport()
            {
                EnableAutoReconnect = EnableAutoReconnect,
                EnableLogging = InternalEnableLogging,
                CustomLogger = InternalCustomLogger,
                EnableRxDebug = InternalEnableRxDebug,
                EnableTxDebug = InternalEnableTxDebug
            };

            tcpTransport.Initialize(ipAddress, port);
            ConnectionTransport = tcpTransport;

            BlurayPlayerProtocol = new KaleidescapeProtocol(ConnectionTransport, Id)
            {
                EnableLogging = InternalEnableLogging,
                CustomLogger = InternalCustomLogger
            };
            BlurayPlayerProtocol.StateChange += StateChange;
            BlurayPlayerProtocol.RxOut += SendRxOut;
            BlurayPlayerProtocol.Initialize(BlurayPlayerData);
            
            DriverLog.Log(EnableLogging,LoggingLevel.Debug, "Init", NumericKeypadLabels.ToString());
        }
        #endregion
        
    }
}
