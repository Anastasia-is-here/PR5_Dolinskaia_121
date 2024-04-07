using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR5_Dolinskaia_121;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest4
    {
        static BaseEntities data = new BaseEntities();
        RegPage page = new RegPage();

        [TestMethod]
        public void RegistrationTestSuccess()
        {
            Assert.IsTrue(page.registration("Имя Фамилия Отчество", "MyLogin@gmai.com", "CoolPass888", true, false, true, "+7(999)999-99-99", 
                "Менеджер С", "СсылкаНаФото"));
        }
    }
}
