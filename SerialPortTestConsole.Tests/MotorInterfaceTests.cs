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
