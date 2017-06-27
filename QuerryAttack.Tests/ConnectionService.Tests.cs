using NUnit.Framework;
using QueryAttack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuerryAttack.Tests
{
    [TestFixture]
    public class ConnectionServcieTest
    {
        [Test]
        public void Connection_WrongConnectionProperties()
        {
            ConnectionService service = new ConnectionService();
            Assert.False(service.Connect(String.Empty, String.Empty, String.Empty, String.Empty));
        }
        [Test]
        public void Connection_IsNotConnectedWhenConnectionIsNull()
        {
            ConnectionService service = new ConnectionService();
            Assert.False(service.IsConnected());
        }
    }
}
