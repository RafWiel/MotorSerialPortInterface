using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SerialPortTestConsole.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void ValidateArgs_ShouldSucceed()
        {
            Assert.True(Program.ValidateArgs(new string[] { "port:COM1", "baud:9600", "command:LS" }));
        }

        [Fact]
        public void ValidateArgs_ShouldFail()
        {
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1", "command:LS" }));
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1", "baud:9600", "command:LS", "fourth arg" }));
            Assert.False(Program.ValidateArgs(new string[] { "portX:COM1", "baud:9600", "command:LS" }));
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1", "baudX:9600", "command:LS" }));
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1", "baud:9600", "commandX:LS" }));
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1!", "baud:9600", "command:LS" }));
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1", "baud:9601", "command:LS" }));
            Assert.False(Program.ValidateArgs(new string[] { "port:COM1", "baud:9600", "command:xS" }));
        }
    }
}
