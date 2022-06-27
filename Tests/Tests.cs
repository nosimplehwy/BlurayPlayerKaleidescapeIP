using System;
using System.Threading;
using NUnit.Framework;
using BlurayPlayer_Kaleidescape_IP;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private MultiTapKeypad _keypad;
        private string _character;
        
        public Tests()
        {
            _keypad = new MultiTapKeypad();
            _keypad.KeyPressed += KeypadOnKeyPressed;     
        }

        private void KeypadOnKeyPressed(object sender, string e)
        {
            _character += e;
        }

        [Test]
        public void Test1()
        {
            _keypad.KeyPress(9);
            _character = String.Empty;
            do
            {
                Thread.Sleep(100);
            } while (_character == String.Empty);
            Assert.AreEqual("9",_character );
        }
        [Test]
        public void Test2()
        {
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            _character = String.Empty;
            do
            {
                Thread.Sleep(100);
            } while (_character == String.Empty);
            Assert.AreEqual("W",_character );
        }
        
        [Test]
        public void Test3()
        {
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            _character = String.Empty;
            do
            {
                Thread.Sleep(100);
            } while (_character == String.Empty);
            Assert.AreEqual("X",_character );
        }
        
        [Test]
        public void Test4()
        {
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            _character = String.Empty;
            do
            {
                Thread.Sleep(100);
            } while (_character == String.Empty);
            Assert.AreEqual("Y",_character );
        }
        
        [Test]
        public void Test5()
        {
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            _character = String.Empty;
            do
            {
                Thread.Sleep(100);
            } while (_character == String.Empty);
            Assert.AreEqual("Z",_character );
        }
        
        [Test]
        public void Test6()
        {
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(9);
            _character = String.Empty;
            do
            {
                Thread.Sleep(100);
            } while (_character == String.Empty);
            Assert.AreEqual("9",_character );
        }
        
        [Test]
        public void Test2Keys()
        {
            _character = null;
            _keypad.KeyPress(9);
            Thread.Sleep(100);
            _keypad.KeyPress(7);
            Thread.Sleep(50);
            _keypad.KeyPress(7);
            Thread.Sleep(50);
            _keypad.KeyPress(7);
            Thread.Sleep(50);
            _keypad.KeyPress(7);
           
                Thread.Sleep(300);
            
            Assert.AreEqual("09R",_character );
        }
        
    }
}