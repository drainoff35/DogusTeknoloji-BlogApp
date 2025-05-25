using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;
using DogusTeknoloji_BlogApp.Tests.SeleniumTests.Common;

namespace DogusTeknoloji_BlogApp.Tests.SeleniumTests.CommentTests
{
    [TestClass]
    [TestCategory("Selenium")]
    [TestCategory("Comment")]
    public class CommentTests : BaseSeleniumTest
    {
        [TestMethod]
        public void AddComment_RequiresAuthentication()
        {
            // Önce giriş yap
            Login();

            // Şifre uyarı kontrolü - Chrome'un şifre uyarısı genellikle sayfa yüklendikten hemen sonra gelebilir
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    // Kısa süreli uyarı kontrolü
                    WebDriverWait shortWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
                    IAlert alert = shortWait.Until(ExpectedConditions.AlertIsPresent());
                    if (alert != null)
                    {
                        Console.WriteLine($"Login sonrası uyarı: {alert.Text}");
                        alert.Accept();
                        // Uyarı işlendikten sonra kısa bir bekleme
                        Thread.Sleep(500);
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Uyarı yoksa sorun değil, devam et
                }
            }

            // Giriş yapıldığını doğrula
            _wait.Until(d => d.FindElement(By.Id("userDropdown")));

            // Şimdi bir posta git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");
            var detailsLink = _driver.FindElement(By.LinkText("Detaylar"));
            detailsLink.Click();

            // Sayfa yüklendikten sonra da uyarı kontrolü
            try
            {
                WebDriverWait alertWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                IAlert alert = alertWait.Until(ExpectedConditions.AlertIsPresent());
                if (alert != null)
                {
                    Console.WriteLine($"Detay sayfasında uyarı: {alert.Text}");
                    alert.Accept();
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Uyarı yoksa sorun değil
            }

            // Yorum ekle
            string commentText = "Test comment from Selenium " + DateTime.Now.Ticks;

            try
            {
                // name ile textarea'yı bul
                _driver.FindElement(By.Name("Text")).SendKeys(commentText);
            }
            catch (NoSuchElementException)
            {
                try
                {
                    // CSS selector ile bul
                    _driver.FindElement(By.CssSelector("textarea[name='Text']")).SendKeys(commentText);
                }
                catch (NoSuchElementException)
                {
                    // JavaScript ile textarea'ya yazı yazabilir
                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("document.querySelector('textarea').value = arguments[0];", commentText);
                }
            }

            // Yorumu gönder
            _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

            // Yorum gönderimi sonrası da uyarı kontrolü
            try
            {
                WebDriverWait alertWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                IAlert alert = alertWait.Until(ExpectedConditions.AlertIsPresent());
                if (alert != null)
                {
                    Console.WriteLine($"Yorum gönderimi sonrası uyarı: {alert.Text}");
                    alert.Accept();
                }
            }
            catch (WebDriverTimeoutException)
            {
                // Uyarı yoksa sorun değil
            }

            // Sayfanın yeniden yüklenmesini bekle
            _wait.Until(d => d.FindElement(By.TagName("body")).Displayed);

            // Daha uzun bir bekleme süresi ekle
            Thread.Sleep(3000);

            // Yorumun eklendiğini doğrula
            Assert.IsTrue(_driver.PageSource.Contains(commentText),
                $"Comment text '{commentText}' not found in page source after submission");
        }

        [TestMethod]
        public void UpdateComment_AuthenticatedUser()
        {
            // 1. Login ol ve postlara git
            Login();

            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));

            // 2. Post detay sayfasına gir ve yorum oluştur
            _driver.FindElement(By.LinkText("Detaylar")).Click();
            _wait.Until(d => d.Url.Contains("/Post/Details/"));

            string originalComment = "Selenium Update Test " + DateTime.Now.Ticks;
            _driver.FindElement(By.Name("Text")).SendKeys(originalComment);
            _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

            // 1.5 saniye bekle, sayfayı yenile ve scroll'u en alta kaydır
            Thread.Sleep(1500);
            _driver.Navigate().Refresh();
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);

            // 3. Oluşturduğumuz yorumu bul (en sondan başa doğru arama)
            var commentCards = _driver.FindElements(By.CssSelector(".card-body"));
            IWebElement commentCard = null;
            for (int i = commentCards.Count - 1; i >= 0; i--)
            {
                if (commentCards[i].Text.Contains(originalComment))
                {
                    commentCard = commentCards[i];
                    break;
                }
            }
            Assert.IsNotNull(commentCard, "Eklenen yorum sayfanın en altında bulunamadı.");

            // Yorumun içindeki "Düzenle" butonunu bul ve tıkla
            var editBtn = commentCard.FindElements(By.LinkText("Düzenle"))
                .FirstOrDefault(btn => btn.Displayed && btn.Enabled);
            Assert.IsNotNull(editBtn, "Düzenle butonu bulunamadı veya tıklanabilir değil.");

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", editBtn);
            Thread.Sleep(500);
            editBtn.Click();

            // Güncelleme formunun yüklenmesini bekle
            _wait.Until(d => d.Url.Contains("/Comment/Edit/"));
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Text")));

            // Güncellenecek alanları doldur
            string updatedComment = "Updated comment " + DateTime.Now.Ticks;
            var commentInput = _driver.FindElement(By.Id("Text"));
            commentInput.Clear();
            commentInput.SendKeys(updatedComment);

            // Güncelle butonuna tıkla
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Post detay sayfasına dönmesini bekle
            _wait.Until(d => d.Url.Contains("/Post/Details/"));
            Thread.Sleep(1500);

            // Güncellenen yorumu doğrula
            Assert.IsTrue(_driver.PageSource.Contains(updatedComment), $"Güncellenen yorum '{updatedComment}' sayfada bulunamadı");
        }

        [TestMethod]
        public void DeleteComment_AuthenticatedUser()
        {
            // Giriş yap
            Login();

            // Post detayına git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");
            _driver.FindElement(By.LinkText("Detaylar")).Click();
            _wait.Until(d => d.Url.Contains("/Post/Details/"));

            // Yorum ekle
            string commentText = "Selenium Delete Test " + DateTime.Now.Ticks;
            _driver.FindElement(By.Name("Text")).SendKeys(commentText);
            _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();
            Thread.Sleep(2000);

            // Sayfayı yenile ve en alta kaydır
            _driver.Navigate().Refresh();
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);

            // Tüm yorum kartlarını bul ve sondan başa tara
            var commentCards = _driver.FindElements(By.CssSelector(".card-body"));
            IWebElement commentCard = null;
            for (int i = commentCards.Count - 1; i >= 0; i--)
            {
                if (commentCards[i].Text.Contains(commentText))
                {
                    commentCard = commentCards[i];
                    break;
                }
            }
            Assert.IsNotNull(commentCard, "Eklenen yorum sayfanın en altında bulunamadı.");

            // Sadece ilgili yorumun içindeki sil butonunu bul
            var deleteBtn = commentCard.FindElements(By.CssSelector("button.delete-Comment"))
                .FirstOrDefault(btn => btn.Displayed && btn.Enabled);
            Assert.IsNotNull(deleteBtn, "Sil butonu bulunamadı veya tıklanabilir değil.");

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", deleteBtn);
            Thread.Sleep(500);
            deleteBtn.Click();

            // Modal'ın açılmasını bekle
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".modal.show")));
            var modal = _driver.FindElement(By.CssSelector(".modal.show"));

            // "Sil" butonunu modal içinde butonun görünen metniyle bul
            var silButton = modal.FindElements(By.TagName("button"))
                .FirstOrDefault(btn => btn.Text.Trim().Equals("Sil", StringComparison.OrdinalIgnoreCase)
                                    || btn.Text.Trim().Equals("Delete", StringComparison.OrdinalIgnoreCase));
            Assert.IsNotNull(silButton, "Modal içinde 'Sil' butonu bulunamadı.");

            _wait.Until(driver => silButton.Displayed && silButton.Enabled);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", silButton);
            Thread.Sleep(2000);

            // Yorumun silindiğini doğrula
            Assert.IsFalse(_driver.PageSource.Contains(commentText), "Yorum silinemedi, sayfada hala görünüyor.");
        }
    }
}
