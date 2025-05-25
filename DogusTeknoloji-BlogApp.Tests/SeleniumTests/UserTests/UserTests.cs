using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using DogusTeknoloji_BlogApp.Tests.SeleniumTests.Common;

namespace DogusTeknoloji_BlogApp.Tests.SeleniumTests.UserTests
{
    [TestClass]
    [TestCategory("Selenium")]
    [TestCategory("User")]
    public class UserTests : BaseSeleniumTest
    {
        [TestMethod]
        public void Login_SuccessfulLogin()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{BaseUrl}/User/Login");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys(TestEmail);
            _driver.FindElement(By.Id("Password")).SendKeys(TestPassword);
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait for redirect and check for authentication
            _wait.Until(d => d.FindElement(By.Id("userDropdown")));

            // Assert
            Assert.IsTrue(_driver.FindElement(By.Id("userDropdown")).Text.Contains(TestUserDisplayName));
        }

        [TestMethod]
        public void Login_InvalidCredentials()
        {
            // Arrange
            _driver.Navigate().GoToUrl($"{BaseUrl}/User/Login");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("invalid@hotmail.com");
            _driver.FindElement(By.Id("Password")).SendKeys("invalidpassword");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert - should stay on login page with error message
            Assert.IsTrue(_driver.Url.Contains("/User/Login"));
            Assert.IsTrue(_driver.PageSource.Contains("Kullanıcı bulunamadı."));
        }

        [TestMethod]
        public void Register_NewUser()
        {
            // Arrange
            string uniqueEmail = $"test{DateTime.Now.Ticks}@example.com";
            string uniqueUsername = $"testuser{DateTime.Now.Ticks}";
            string password = "Test1234";

            // Act
            _driver.Navigate().GoToUrl($"{BaseUrl}/User/Register");

            // Register formunu doldur - doğru ID'leri kullanarak
            _driver.FindElement(By.Id("UserName")).SendKeys(uniqueUsername);
            _driver.FindElement(By.Id("Email")).SendKeys(uniqueEmail);
            _driver.FindElement(By.Id("Password")).SendKeys(password);

            // Form submit butonuna tıkla
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Assert - Login sayfasına yönlendirildiğini doğrula
            _wait.Until(d => d.Url.Contains("/User/Login"));

            // Login sayfasında başarı mesajını kontrol et
            bool hasSuccessMessage = _driver.FindElements(By.CssSelector(".alert-success")).Count > 0;
            if (hasSuccessMessage)
            {
                Assert.IsTrue(true, "Kayıt işlemi başarılı oldu.");
            }
            else
            {
                // Başarı mesajı yoksa, oluşturulan hesapla giriş yapmaya çalış
                _driver.FindElement(By.Id("Email")).SendKeys(uniqueEmail);
                _driver.FindElement(By.Id("Password")).SendKeys(password);
                _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                try
                {
                    // Giriş başarılı olursa userDropdown görünür olmalı
                    _wait.Until(d => d.FindElement(By.Id("userDropdown")));
                    Assert.IsTrue(true, "Kayıt işlemi başarılı oldu, giriş yapılabildi.");
                }
                catch (WebDriverTimeoutException)
                {
                    Assert.Fail("Kayıt işlemi başarısız oldu veya oluşturulan hesapla giriş yapılamadı.");
                }
            }
        }
    }
}
