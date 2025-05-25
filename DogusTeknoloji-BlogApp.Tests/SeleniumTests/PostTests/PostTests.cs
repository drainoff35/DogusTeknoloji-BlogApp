using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using DogusTeknoloji_BlogApp.Tests.SeleniumTests.Common;

namespace DogusTeknoloji_BlogApp.Tests.SeleniumTests.PostTests
{
    [TestClass]
    [TestCategory("Selenium")]
    [TestCategory("Post")]
    public class PostTests : BaseSeleniumTest
    {
        [TestMethod]
        public void HomePage_LoadsSuccessfully()
        {
            // Arrange & Act
            _driver.Navigate().GoToUrl(BaseUrl);

            // Assert
            Assert.IsTrue(_driver.Title.Contains("Blog Anasayfa"));
            Assert.IsTrue(_driver.FindElement(By.TagName("h1")).Text.Contains("Doğuş Teknoloji Blog"));
        }

        [TestMethod]
        public void Navigation_MenuItemsExist()
        {
            // Arrange & Act
            _driver.Navigate().GoToUrl(BaseUrl);

            // Assert
            Assert.IsTrue(_driver.FindElement(By.LinkText("Ana Sayfa")).Displayed);
            Assert.IsTrue(_driver.FindElement(By.LinkText("Yazılar")).Displayed);
            Assert.IsTrue(_driver.FindElement(By.LinkText("Kategoriler")).Displayed);
        }

        [TestMethod]
        public void Posts_ViewListOfPosts()
        {
            // Arrange & Act
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");

            // Assert
            Assert.IsTrue(_driver.Title.Contains("Yazılar") || _driver.FindElement(By.TagName("h1")).Text.Contains("Yazılar"));
        }

        [TestMethod]
        public void PostDetails_DisplaysCorrectly()
        {
            // Arrange
            // First go to posts page
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");

            // Make sure the page is fully loaded
            _wait.Until(d => d.FindElements(By.LinkText("Detaylar")).Count > 0);

            // Act
            // Click on the first post's details link
            var detailsLink = _driver.FindElement(By.LinkText("Detaylar"));
            detailsLink.Click();

            // Wait for post details page to load
            _wait.Until(d => d.Url.Contains("/Post/Details/"));

            // Give the page time to fully render
            Thread.Sleep(1000);

            // Assert - check for post content using more flexible selectors
            // Try different possible selectors for the title and content
            bool titleFound = false;
            bool contentFound = false;

            try
            {
                // Check for post title - try multiple possible selectors
                titleFound = _driver.FindElements(By.CssSelector(".card-title")).Count > 0 ||
                            _driver.FindElements(By.TagName("h1")).Count > 0 ||
                            _driver.FindElements(By.TagName("h2")).Count > 0;

                // Check for post content - try multiple possible selectors
                contentFound = _driver.FindElements(By.CssSelector(".card-text")).Count > 0 ||
                              _driver.FindElements(By.CssSelector(".post-content")).Count > 0 ||
                              _driver.PageSource.Contains("</p>");

                Console.WriteLine($"Title found: {titleFound}, Content found: {contentFound}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking page elements: {ex.Message}");
            }

            // Assert content is found using flexible approach
            Assert.IsTrue(titleFound, "Could not find post title element on the page");
            Assert.IsTrue(contentFound, "Could not find post content on the page");
        }

        [TestMethod]
        public void CreatePost_RequiresAuthentication()
        {
            // Arrange & Act
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Create");

            // Assert - should redirect to login page
            Assert.IsTrue(_driver.Url.Contains("/User/Login"));
        }

        [TestMethod]
        public void CreatePost_AuthenticatedUser()
        {
            // Login first
            Login();

            // Navigate to create post page
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Create");

            // Generate a unique title for testing
            string testTitle = "Test Post " + DateTime.Now.Ticks;
            string testContent = "This is a test post created by automated test.";

            // Fill in the post form
            _driver.FindElement(By.Id("Title")).SendKeys(testTitle);
            _driver.FindElement(By.Id("Content")).SendKeys(testContent);

            // Select first category
            var categoryDropdown = new SelectElement(_driver.FindElement(By.Id("CategoryId")));
            categoryDropdown.SelectByIndex(1);

            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(1000);

            // Submit the form
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait for redirect
            _wait.Until(d => !d.Url.Contains("/Post/Create"));

            // Yazılar sayfasına git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");

            // Sayfanın yüklenmesini bekle
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));
            Thread.Sleep(2000); // Sayfanın tam yüklenmesi için ek bekleme

            // Oluşturduğumuz postun başlığını sayfada ara
            bool postFound = _driver.PageSource.Contains(testTitle);         

            Assert.IsTrue(postFound, $"Oluşturulan post başlığı '{testTitle}' sayfa listesinde bulunamadı");
        }

        [TestMethod]
        public void UpdatePost_AuthenticatedUser()
        {
            // 1. Login ol ve post sayfasına git
            Login();

            _driver.Navigate().GoToUrl($"{BaseUrl}/Post");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));

            // 2. Yeni post oluştur
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Create");
            string originalTitle = "Test Post " + DateTime.Now.Ticks;
            string originalContent = "This is a test post created for update test.";
            _driver.FindElement(By.Id("Title")).SendKeys(originalTitle);
            _driver.FindElement(By.Id("Content")).SendKeys(originalContent);

            // Kategori seç
            var categoryDropdown = new SelectElement(_driver.FindElement(By.Id("CategoryId")));
            categoryDropdown.SelectByIndex(1);

            // Scroll en alta ve oluştur butonuna bas
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Post sayfasına yönlendirilmesini bekle
            _wait.Until(d => !d.Url.Contains("/Post/Create"));

            // 3. Post sayfasına git ve oluşturduğumuz postu bul
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));
            Thread.Sleep(2000); // Sayfanın tamamen yüklenmesini bekle

            // Postu bul (sayfada birden fazla post olabileceği için tüm sayfayı tara)
            bool foundPost = false;
            IWebElement editButton = null;

            // Önce sayfanın en üstüne kaydır
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(500);

            // 5 adımda sayfayı aşağı kaydırarak tüm postları tara
            for (int i = 0; i < 5 && !foundPost; i++)
            {
                // Her adımda sayfayı kademeli olarak aşağı kaydır
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, " + (i + 1) * 500 + ");");
                Thread.Sleep(1000);

                var cardElements = _driver.FindElements(By.CssSelector(".card-title"));
                Console.WriteLine($"Adım {i + 1}: Aranan başlık: {originalTitle}");
                Console.WriteLine($"Adım {i + 1}: Bulunan kart sayısı: {cardElements.Count}");

                foreach (var card in cardElements)
                {
                    Console.WriteLine($"Kart başlığı: {card.Text.Trim()}");
                    if (card.Text.Contains(originalTitle))
                    {
                        foundPost = true;
                        Console.WriteLine($"Post bulundu: {card.Text}");

                        // Düzenle butonunu bul
                        var parentCard = card.FindElement(By.XPath("./ancestor::div[contains(@class, 'card')]"));
                        editButton = parentCard.FindElement(By.LinkText("Düzenle"));

                        // Düzenle butonunun görünür olması için o alana kaydır
                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", editButton);
                        Thread.Sleep(1000);
                        break;
                    }
                }
            }

            // Son bir deneme olarak sayfanın en altına kaydır
            if (!foundPost)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                Thread.Sleep(1500);

                var cardElements = _driver.FindElements(By.CssSelector(".card-title"));
                foreach (var card in cardElements)
                {
                    if (card.Text.Contains(originalTitle))
                    {
                        foundPost = true;
                        Console.WriteLine($"Post sayfanın en altında bulundu: {card.Text}");

                        var parentCard = card.FindElement(By.XPath("./ancestor::div[contains(@class, 'card')]"));
                        editButton = parentCard.FindElement(By.LinkText("Düzenle"));

                        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", editButton);
                        Thread.Sleep(1000);
                        break;
                    }
                }
            }

            Assert.IsTrue(foundPost, $"Oluşturulan post başlığı '{originalTitle}' düzenlemek için bulunamadı");

            // Düzenle butonuna tıkla
            editButton.Click();

            // 4. Güncelleme formunu doldur ve kaydet
            _wait.Until(d => d.Url.Contains("/Post/Edit/"));
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Title")));
            string updatedTitle = "Updated Post " + DateTime.Now.Ticks;
            string updatedContent = "This post content has been updated for testing.";

            var titleInput = _driver.FindElement(By.Id("Title"));
            titleInput.Clear();
            titleInput.SendKeys(updatedTitle);

            var contentInput = _driver.FindElement(By.Id("Content"));
            contentInput.Clear();
            contentInput.SendKeys(updatedContent);

            // Düzenleme formundaki kaydet butonuna kaydır ve tıkla
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 5. Post detay sayfasına yönlendirilmesini bekle
            _wait.Until(d => d.Url.Contains("/Post"));
            Thread.Sleep(3000); // Daha uzun bekleme süresi

            // Sayfa başlığını konsola yazdır
            Console.WriteLine("Sayfa başlığı: " + _driver.Title);
            Console.WriteLine("Mevcut URL: " + _driver.Url);

            // Önce sayfanın en üstüne kaydır
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0);");
            Thread.Sleep(1000);

            // Sayfanın ortasına kaydır (başlık genelde burada olur)
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, Math.max(document.documentElement.scrollHeight, document.body.scrollHeight) / 2);");
            Thread.Sleep(1000);

            // Son olarak sayfanın en altına kaydır
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            Thread.Sleep(1000);

            // HTML kaynağında güncellenen başlığı ara
            bool titleInSource = _driver.PageSource.Contains(updatedTitle);
            Console.WriteLine($"Başlık HTML kaynağında bulundu mu: {titleInSource}");

            // Tüm olası başlık elementlerini kontrol et
            var allTitleElements = _driver.FindElements(By.CssSelector("h1, h2, h3, .card-title, .post-title"));
            Console.WriteLine($"Sayfada bulunan başlık elementi sayısı: {allTitleElements.Count}");

            bool titleFound = false;
            foreach (var element in allTitleElements)
            {
                Console.WriteLine($"Başlık elementi içeriği: {element.Text}");
                if (element.Text.Contains(updatedTitle))
                {
                    titleFound = true;
                    break;
                }
            }

            // Başlık bulunamadıysa, HTML kaynağında olup olmadığını kontrol et
            if (!titleFound)
            {
                titleFound = titleInSource;
            }

            Assert.IsTrue(titleFound, $"Güncellenen post başlığı '{updatedTitle}' detay sayfasında bulunamadı");
        }

        [TestMethod]
        public void DeletePost_AuthenticatedUser()
        {
            // Login first
            Login();

            // Create a new post first
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Create");

            // Generate a unique title for testing
            string testTitle = "Delete Test Post " + DateTime.Now.Ticks;
            string testContent = "This is a test post that will be deleted by automated test.";

            // Fill in the post form
            _driver.FindElement(By.Id("Title")).SendKeys(testTitle);
            _driver.FindElement(By.Id("Content")).SendKeys(testContent);

            // Select first category
            var categoryDropdown = new SelectElement(_driver.FindElement(By.Id("CategoryId")));
            categoryDropdown.SelectByIndex(1);

            // Scroll to the bottom to ensure the submit button is visible
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            Thread.Sleep(1000);

            // Submit the form
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait for redirect
            _wait.Until(d => !d.Url.Contains("/Post/Create"));

            // Go to posts page to find our newly created post
            _driver.Navigate().GoToUrl($"{BaseUrl}/Post/Index");
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));
            Thread.Sleep(2000);

            // Find the post by title and click delete
            bool foundPost = false;
            var cardElements = _driver.FindElements(By.CssSelector(".card-title"));
            foreach (var card in cardElements)
            {
                if (card.Text.Contains(testTitle))
                {
                    foundPost = true;
                    // Find the parent card and click delete button
                    var parentCard = card.FindElement(By.XPath("./ancestor::div[contains(@class, 'card')]"));
                    parentCard.FindElement(By.CssSelector(".delete-post")).Click();
                    break;
                }
            }

            Assert.IsTrue(foundPost, $"Silinecek post başlığı '{testTitle}' bulunamadı");

            // Wait for delete confirmation modal
            _wait.Until(ExpectedConditions.ElementExists(By.Id("deleteConfirmModal")));

            // Confirm deletion
            _driver.FindElement(By.Id("confirmDeleteBtn")).Click();

            // Wait for AJAX operation to complete
            Thread.Sleep(3000);

            // Refresh the page to verify the post is gone
            _driver.Navigate().Refresh();
            _wait.Until(ExpectedConditions.ElementExists(By.TagName("body")));

            // Verify post was deleted
            bool postStillExists = false;
            cardElements = _driver.FindElements(By.CssSelector(".card-title"));
            foreach (var card in cardElements)
            {
                if (card.Text.Contains(testTitle))
                {
                    postStillExists = true;
                    break;
                }
            }

            Assert.IsFalse(postStillExists, $"Silinen post başlığı '{testTitle}' hala sayfa listesinde görünüyor");
        }
    }
}
