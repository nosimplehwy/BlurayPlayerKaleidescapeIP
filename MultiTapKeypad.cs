using System;
using System.Timers;
using Crestron.RAD.Common.Logging;

namespace BlurayPlayer_Kaleidescape_IP
{
    public class MultiTapKeypad
    {
        private int _taps;
        private uint _currentKey = 11;
        private const bool Logging = false;

        private static Timer _timer;

        public event EventHandler<string> KeyPressed;
        
      /*
       private readonly string[,] _values = new string[,]
        {
            {"0", "0", "0", "0", "0"},
            {"1", "1", "1", "1", "1"},
            {"2", "a", "b", "c", "2"},
            {"3", "d", "e", "f", "3"},
            {"4", "g", "h", "i", "4"},
            {"5", "j", "k", "l", "5"},
            {"6", "m", "n", "o", "6"},
            {"7", "p", "q", "r", "s"},
            {"8", "t", "u", "v", "8"},
            {"9", "w", "x", "y", "z"}
        };
        */
        
        private readonly string[,] _values = new string[,]
        {
            {"0", "0", "0", "0", "0"},
            {"1", "1", "1", "1", "1"},
            {"a", "b", "c", "2", "2"},
            {"d", "e", "f", "3", "3"},
            {"g", "h", "i", "4", "4"},
            {"j", "k", "l", "5", "5"},
            {"m", "n", "o", "6", "6"},
            {"p", "q", "r", "s", "7"},
            {"t", "u", "v", "8", "8"},
            {"w", "x", "y", "z", "9"}
        };

        public MultiTapKeypad()
        {
            _timer = new Timer(500);
            _timer.Elapsed += TimerElapsed;
            _timer.AutoReset = false;
            _timer.Enabled = true;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            DriverLog.Log(Logging, LoggingLevel.Debug, "TimerElapsed", _currentKey.ToString());
            //if greater than 11 then a character doesn't need to be sent
            if(_currentKey < 11)
              OnKeyPressed(GetChar());
        }

        public void KeyPress(uint key)
        {
            //if less than 11 then a key was just pressed and timer is running
            if (_currentKey < 11)
            {
                _timer.Stop();
                
                //changed keys, send last key immediately
                if (_currentKey != key)
                {
                    OnKeyPressed(GetChar());
                    _currentKey = key;
                    _taps = 0;
                }
                //additional tap
                else
                {
                    _taps++;
                }
            }
            //first tap
            else
            {
                _currentKey = key;
                _taps = 0;
            }
            _timer.Start();
            DriverLog.Log(Logging, LoggingLevel.Debug, "KeyPress", $"Key={_currentKey}, NumTaps={_taps}");
        }

        private string GetChar()
        {
            return _taps <= 4 ? _values[_currentKey, _taps] : _values[_currentKey, 0];
        }

        private void OnKeyPressed(string key)
        {
            DriverLog.Log(Logging, LoggingLevel.Debug, "OnKeyPressed", $"Key={key}");
            //set out of range 
            _currentKey = 11;
            KeyPressed?.Invoke(this, key);
        }

    }
}