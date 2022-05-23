using Crestron.RAD.Common.BasicDriver;
using Crestron.RAD.Common.Enums;
using Crestron.RAD.Common.Logging;
using Crestron.RAD.Common.Transports;
using Crestron.RAD.DeviceTypes.BlurayPlayer;

namespace BlurayPlayer_Kaleidescape_IP
{
    public class KaleidescapeProtocol : ABlurayPlayerProtocol
    {
        private string _deviceId;
        private ArrowDirections _arrowKeyDirection;
        private readonly MultiTapKeypad _keypad;
        public KaleidescapeProtocol(ISerialTransport transportDriver, byte id)
            : base(transportDriver, id)
        {
            ResponseValidation = new KaleidescapeValidator(Id, ValidatedData);
            ValidatedData.PowerOnPollingSequence = new[]
            {
                StandardCommandsEnum.PowerPoll
            };

            _keypad = new MultiTapKeypad();
            _keypad.KeyPressed += KeypadOnKeyPressed;
        }

        public override void Dispose()
        {
            _keypad.Dispose();
        }

        protected override void ConnectionChanged(bool connection)
        {
            // IsConnected = connection;
            DriverLog.Log(EnableLogging, LoggingLevel.Debug, "ConnectionChanged", connection.ToString());

            base.ConnectionChanged(connection);
        }

        //TODO
        //add page up/dn to chan buttons, exit and last should do something
        //GetPowerState

        //Enter Standby

        //Leave Standby


        //SetPowerState
        public override void ReleaseArrowKey()
        {
            var cmd = "/1/DOWN_RELEASE:";
            var commandEnum = StandardCommandsEnum.DownArrow;
            switch (_arrowKeyDirection)
            {
                case ArrowDirections.Down:
                    break;
                case ArrowDirections.Left:
                    cmd = "/1/LEFT_RELEASE:";
                    commandEnum = StandardCommandsEnum.LeftArrow;
                    break;
                case ArrowDirections.Right:
                    cmd = "/1/RIGHT_RELEASE:";
                    commandEnum = StandardCommandsEnum.RightArrow;
                    break;
                case ArrowDirections.Up:
                    cmd = "/1/UP_RELEASE:";
                    commandEnum = StandardCommandsEnum.UpArrow;
                    break;
            }

            SendCommand(new CommandSet(
                "ArrowRelease",
                cmd,
                CommonCommandGroupType.Arrow,
                null,
                false,
                CommandPriority.Normal,
                commandEnum));
        }

        public override void PressArrowKey(ArrowDirections direction)
        {
            _arrowKeyDirection = direction;
            var cmd = "/1/DOWN_PRESS:";
            var commandEnum  = StandardCommandsEnum.DownArrow;
            switch (direction)
            {
                case ArrowDirections.Down:
                    break;
                case ArrowDirections.Left:
                    cmd = "/1/LEFT_PRESS:";
                    commandEnum = StandardCommandsEnum.LeftArrow;
                    break;
                case ArrowDirections.Right:
                    cmd = "/1/RIGHT_PRESS:";
                    commandEnum = StandardCommandsEnum.RightArrow;
                    break;
                case ArrowDirections.Up:
                    cmd = "/1/UP_PRESS:";
                    commandEnum = StandardCommandsEnum.UpArrow;
                    break;
            }

            SendCommand(new CommandSet(
                    "ArrowPress",
                    cmd,
                    CommonCommandGroupType.Arrow,
                    null,
                    false,
                    CommandPriority.Normal,
                    commandEnum));
        }

        public override void ArrowKey(ArrowDirections direction)
        {
            var cmd = "/1/DOWN:";
            var commandEnum = StandardCommandsEnum.DownArrow;
            switch (direction)
            {
                case ArrowDirections.Down:
                    break;
                case ArrowDirections.Left:
                    cmd = "/1/LEFT:";
                    commandEnum = StandardCommandsEnum.LeftArrow;
                    break;
                case ArrowDirections.Right:
                    cmd = "/1/RIGHT:";
                    commandEnum = StandardCommandsEnum.RightArrow;
                    break;
                case ArrowDirections.Up:
                    cmd = "/1/UP:";
                    commandEnum = StandardCommandsEnum.UpArrow;
                    break;
            }

            SendCommand(new CommandSet(
                "ArrowPressandRelease",
                cmd,
                CommonCommandGroupType.Arrow,
                null,
                false,
                CommandPriority.Normal,
                commandEnum));
        }

        public override void KeypadNumber(uint num)
        {
            DriverLog.Log(EnableLogging,LoggingLevel.Debug, "KeypadNumber", num.ToString());
            _keypad.KeyPress(num);
        }
        
        private void SendKeyCommand(string key)
        {
            DriverLog.Log(EnableLogging, LoggingLevel.Debug, "SendKeyCommand", key);

            SendCommand(new CommandSet(
                "KeyboardCharacter",
                $"/4/KEYBOARD_CHARACTER:{key}:",
                CommonCommandGroupType.Keypad,
                null,
                false,
                CommandPriority.Normal,
                StandardCommandsEnum.KeypadNumber));
        }
        
        private void KeypadOnKeyPressed(object sender, string e1)
        {
            DriverLog.Log(EnableLogging, LoggingLevel.Debug, "KeypadOnKeyPressed", e1);
            SendKeyCommand(e1);
        }

        //add cpid to command
        protected override bool PrepareStringThenSend(CommandSet commandSet)
        {
            commandSet.Command = $"{_deviceId}{commandSet.Command}\r\n";
            DriverLog.Log(EnableLogging,  LoggingLevel.Debug, "PrepareStringThenSend", commandSet.Command);
            return base.PrepareStringThenSend(commandSet);
        }

        public override void SetUserAttribute(string attributeId, string attributeValue)
        {
            if (attributeValue == null)
            {
                DriverLog.Log(EnableLogging,  LoggingLevel.Error, "SetUserAttribute", "Attribute value is null");
                return;
            }

            DriverLog.Log(EnableLogging,  LoggingLevel.Debug, "SetUserAttribute", attributeValue);
            if (attributeId != "ID") return;
            var valid = int.TryParse(attributeValue, out var idVal);
            if (valid)
            {
                if (idVal >= 01 && idVal <= 99)
                {
                    _deviceId = attributeValue;
                }
                else
                {
                    DriverLog.Log(EnableLogging,  LoggingLevel.Error, "SetUserAttribute",
                        "Attribute value is out of range.");
                }
            }
            else
            {
                DriverLog.Log(EnableLogging,  LoggingLevel.Error, "SetUserAttribute",
                    "Attribute value is not valid.");
            }
            
        }
        private class KaleidescapeValidator : ResponseValidation
        {
            public KaleidescapeValidator(
                byte id,
                DataValidation dataValidation)
                : base(id, dataValidation)
            {
                Id = id;
                DataValidation = dataValidation;
            }

            public override ValidatedRxData ValidateResponse(
                string response,
                CommonCommandGroupType commandGroup)
            {
                var validatedData = new ValidatedRxData(false, string.Empty)
                {
                    Data = response,
                    Ready = true
                };
                DriverLog.Log(true,  LoggingLevel.Debug, "ValidateResponse", response);

                return validatedData;
            }
        }
    }
}
