using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR5_Dolinskaia_121;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest3
    {
        static BaseEntities data = new BaseEntities();
        AuthPage page = new AuthPage();


        [TestMethod]
        public void AuthTestSuccess()
        {
            List<string> logins = data.User.Where(t => t.Role != "Удален").Select(t => t.Login).ToList();   // успешный заход
            List<string> passes = data.User.Where(t => t.Role != "Удален").Select(t => t.Password).ToList();
            for (int i = 0; i < logins.Count; i++)
            {
                Assert.IsTrue(page.Auth(logins[i], passes[i]));
            }
            Assert.IsTrue(page.Auth("vSESLAVA@GmAI.cOm", "fhDSBf")); // зависимость от регистра логина
        }
        [TestMethod]
        public void AuthTestFailure()
        {
            List<string> logins = data.User.Where(t => t.Role == "Удален").Select(t => t.Login).ToList(); // удаленные пользователи
            List<string> passes = data.User.Where(t => t.Role == "Удален").Select(t => t.Password).ToList();
            for (int i = 0; i < logins.Count; i++)
            {
                Assert.IsFalse(page.Auth(logins[i], passes[i]));
            }

            Assert.IsFalse(page.Auth("Anisa@gmai.com", "wH4oyM")); // зависимость от регистра пароля
            Assert.IsFalse(page.Auth("WrongData@list.ru", "wrongpass"));
            Assert.IsFalse(page.Auth("", ""));
            Assert.IsFalse(page.Auth("", "Wh4OYm"));
            Assert.IsFalse(page.Auth("Galina@gmai.com", ""));
        }

        [TestMethod]
        public void CaptchaTest_Failure()
        {
            page.Auth("WrongData@list.ru", "wrongpass");
            page.Auth("WrongData@list.ru", "wrongpass");
            page.Auth("WrongData@list.ru", "wrongpass");
            Assert.IsFalse(page.Auth("Vseslava@gmai.com", "fhDSBf"));
        }

        [TestMethod]
        public void CaptchaTest_Success()
        {
            page.Auth("Vseslava@gmai.com", "no");
            page.Auth("Vseslava@gmai.com", "no");
            page.Auth("Vseslava@gmai.com", "no");
            Assert.IsTrue(page.Auth("Vseslava@gmai.com", "fhDSBf", page.get_captcha()));
        }
    }
}
