using SerialPortTestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SerialPortTestConsole.Tests
{
    public class ResponseManagerTests
    {
        [Theory]
        [InlineData(1, "{ \"Result\":7,\"ErrorMessage\":\"Przekroczono limit czasu zapisu.\"}")]
        [InlineData(2, "{ \"Result\":7,\"ErrorMessage\":\"Przekroczono limit czasu zapisu.\"}\r\n{ \"Result\":5,\"ErrorMessage\":\"Command timed out\"}\r\n")]
        public void GetErrors_ShouldSucceed(int expected, string json)
        {            
            var actual = ResponseManager.GetErrors(json);

            Assert.Equal(expected, actual.Count);
        }

        [Theory]        
        [InlineData(1, "xxx")]
        [InlineData(2, "{ \"Result\":7,\"ErrorMessage\":\"Przekroczono limit czasu zapisu.\"}")]
        [InlineData(2, "{ \"ResultX\":7,\"ErrorMessageX\":\"Przekroczono limit czasu zapisu.\"}\r\n{ \"Result\":5,\"ErrorMessage\":\"Command timed out\"}\r\n")]
        [InlineData(1, "{ \"Command\":1,\"Firmware\":\"123456\"}")]        
        public void GetErrors_ShouldFail(int expected, string json)
        {            
            var actual = ResponseManager.GetErrors(json);

            Assert.NotEqual(expected, actual.Count);
        }
    }
}
