using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SerialPortTestConsole.Tests
{
    public class MotorInterfaceTests
    {        
        [Fact]
        public void ValidateArgs_ShouldSucceed()
        {            
            Assert.True(MotorInterface.ValidateArgs(new string[] { "port:com1", "baud:9600", "command:ls" }));            
        }

        [Fact]
        public void ValidateArgs_ShouldFail()
        {                        
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1", "command:ls"  }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1", "baud:9600", "command:ls", "fourth arg" }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "portX:com1", "baud:9600", "command:ls" }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1", "baudX:9600", "command:ls" }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1", "baud:9600", "commandX:ls" }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1!", "baud:9600", "command:ls" }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1", "baud:9601", "command:ls" }));
            Assert.False(MotorInterface.ValidateArgs(new string[] { "port:com1", "baud:9600", "command:xs" }));
        }

        [Fact]
        public void WaitForResponse_ReturnImmediatelyOnResponse()
        {
            bool actual = false;

            using (var motorInterface = new MotorInterface())
            {
                motorInterface.DebugTest_SetResponse();
                actual = motorInterface.DebugTest_WaitForResponse();                
            }

            Assert.True(actual);
        }

        [Fact]
        public void WaitForResponse_TimeoutIfNoResponse()
        {
            int expectedMillis = 3500; 
            bool actualResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (var motorInterface = new MotorInterface())
            {
                actualResult = motorInterface.DebugTest_WaitForResponse();
            }

            sw.Stop();
            int actualMillis = (int)sw.ElapsedMilliseconds;

            //timeout is set to 3s
            Assert.True(actualMillis < expectedMillis);
            Assert.False(actualResult);
        }
    }
}
