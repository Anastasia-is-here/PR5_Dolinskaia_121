using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR5_Dolinskaia_121;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void AuthTest()
        {
            var page = new AuthPage();
            Assert.IsTrue(page.Auth("Anisa@gmai.com", "Wh4OYm"));
        }
    }
}
