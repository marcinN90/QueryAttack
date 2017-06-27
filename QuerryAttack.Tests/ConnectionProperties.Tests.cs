using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QueryAttack.Model;

namespace QuerryAttack.Tests
{
    [TestFixture]
    class ConnectionPropertiesTests
    {
		[Test]
		public void ConnectionProperties_IsResetPropertiesSuccesfuly()
        {
            ConnectionProperties connProperties = new ConnectionProperties();
            connProperties.ServerName = @"DESKTOP-SLEAS3V\SQL2014";
            connProperties.DatabaseName = "CS";
            connProperties.User = "sa";
            connProperties.Password = "maca2bra";
            connProperties.ResetProperties();
            Assert.AreEqual(true, (connProperties.ServerName == String.Empty
							&&	connProperties.DatabaseName == String.Empty
							&&	connProperties.User == String.Empty
                            &&	connProperties.Password == String.Empty));
        }

    }
}
