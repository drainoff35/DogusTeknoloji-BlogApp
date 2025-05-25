using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace DogusTeknoloji_BlogApp.Tests.SeleniumTests.Common
{
    [TestClass]
    public abstract class BaseSeleniumTest
    {
        protected IWebDriver _driver;
        protected WebDriverWait _wait;
        protected const string BaseUrl = "http://localhost:8080";
        protected const string TestEmail = "kutay123@hotmail.com";
        protected const string TestPassword = "Wh24preprvl!";
        protected const string TestUserDisplayName = "admin";

        [TestInitialize]
        public virtual void TestInitialize()
        {
            var options = new ChromeOptions();
            options.AddArgument("--password-store=basic");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--disable-features=PasswordLeakDetection");

            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            _driver?.Quit();
        }

        protected void Login()
        {
            _driver.Navigate().GoToUrl($"{BaseUrl}/User/Login");
            _driver.FindElement(By.Id("Email")).SendKeys(TestEmail);
            _driver.FindElement(By.Id("Password")).SendKeys(TestPassword);
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            _wait.Until(d => d.FindElement(By.Id("userDropdown")));
        }

        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            Thread.Sleep(500);
        }

        protected void ScrollToBottom()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(500);
        }

        protected void ScrollToTop()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(500);
        }
    }
}
