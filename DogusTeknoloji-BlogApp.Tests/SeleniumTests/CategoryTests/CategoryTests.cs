using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using DogusTeknoloji_BlogApp.Tests.SeleniumTests.Common;

namespace DogusTeknoloji_BlogApp.Tests.SeleniumTests.CategoryTests
{
    [TestClass]
    [TestCategory("Selenium")]
    [TestCategory("Category")]
    public class CategoryTests : BaseSeleniumTest
    {
        [TestMethod]
        public void CreateCategory_AuthenticatedUser()
        {
            // Giriş yap
            Login();
            
            // Kategori oluşturma sayfasına git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Create");
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Name")));
            
            // Benzersiz bir kategori adı oluştur
            string uniqueCategoryName = "TestKategori" + DateTime.Now.Ticks.ToString().Substring(0, 6);
            
            // Formu doldur
            var nameInput = _driver.FindElement(By.Id("Name"));
            nameInput.Clear();
            nameInput.SendKeys(uniqueCategoryName);
            
            // Formu gönder
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            
            // Kategori listesine git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Index");
            
            // Sayfanın tamamen yüklenmesini bekle
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            Thread.Sleep(1000);
            
            // Sayfayı yenile
            _driver.Navigate().Refresh();
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            
            // Tablodaki kategorileri kontrol et
            var rows = _driver.FindElements(By.CssSelector("table tbody tr"));
            bool categoryFound = false;
            
            // Her satırı kontrol et
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count >= 2 && cells[1].Text.Trim() == uniqueCategoryName)
                {
                    categoryFound = true;
                    break;
                }
            }
            
            // Kategori bulunamadıysa, sayfa kaynağında ara
            if (!categoryFound && _driver.PageSource.Contains(uniqueCategoryName))
            {
                categoryFound = true;
            }
            
            Assert.IsTrue(categoryFound, $"'{uniqueCategoryName}' kategorisi oluşturuldu ama sayfada bulunamadı.");
        }
        
        [TestMethod]
        public void UpdateCategory_AuthenticatedUser()
        {
            // Giriş yap
            Login();
            
            // Önce yeni bir kategori oluştur
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Create");
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Name")));
            
            string originalCategoryName = "UpdateTest" + DateTime.Now.Ticks.ToString().Substring(0, 6);
            
            _driver.FindElement(By.Id("Name")).SendKeys(originalCategoryName);
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            
            // Kategori listesine git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Index");
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            
            // Oluşturulan kategoriyi bul ve düzenle butonuna tıkla
            bool foundCategory = false;
            int categoryId = 0;
            
            // Sayfada kaydırma yaparak tüm kategorileri kontrol et
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            int scrollAttempts = 0;
            int maxScrollAttempts = 5;
            
            while (!foundCategory && scrollAttempts < maxScrollAttempts)
            {
                var rows = _driver.FindElements(By.CssSelector("table tbody tr"));
                
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2 && cells[1].Text.Trim() == originalCategoryName)
                    {
                        foundCategory = true;
                        categoryId = int.Parse(cells[0].Text.Trim());
                        
                        // Düzenle butonunu bul ve tıkla
                        var editButton = row.FindElement(By.LinkText("Düzenle"));
                        js.ExecuteScript("arguments[0].scrollIntoView(true);", editButton);
                        Thread.Sleep(500);
                        editButton.Click();
                        break;
                    }
                }
                
                if (!foundCategory)
                {
                    // Sayfayı aşağı kaydır
                    js.ExecuteScript("window.scrollBy(0, 500);");
                    Thread.Sleep(500);
                    scrollAttempts++;
                }
            }
            
            Assert.IsTrue(foundCategory, $"Güncellenecek kategori '{originalCategoryName}' bulunamadı.");
            
            // Kategori düzenleme formunun yüklenmesini bekle
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Name")));
            
            // Kategori adını güncelle
            string updatedCategoryName = "Updated" + DateTime.Now.Ticks.ToString().Substring(0, 6);
            var nameInput = _driver.FindElement(By.Id("Name"));
            nameInput.Clear();
            nameInput.SendKeys(updatedCategoryName);
            
            // Formu gönder
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            
            // Kategori listesine yönlendirildiğini kontrol et
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Index");
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            
            // Güncellenen kategoriyi kontrol et
            bool updatedCategoryFound = false;
            scrollAttempts = 0;
            
            while (!updatedCategoryFound && scrollAttempts < maxScrollAttempts)
            {
                var rows = _driver.FindElements(By.CssSelector("table tbody tr"));
                
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2 && cells[0].Text.Trim() == categoryId.ToString() && cells[1].Text.Trim() == updatedCategoryName)
                    {
                        updatedCategoryFound = true;
                        break;
                    }
                }
                
                if (!updatedCategoryFound)
                {
                    // Sayfayı aşağı kaydır
                    js.ExecuteScript("window.scrollBy(0, 500);");
                    Thread.Sleep(500);
                    scrollAttempts++;
                }
            }
            
            Assert.IsTrue(updatedCategoryFound, $"Güncellenen kategori '{updatedCategoryName}' bulunamadı.");
        }
        
        [TestMethod]
        public void DeleteCategory_AuthenticatedUser()
        {
            // Giriş yap
            Login();
            
            // Önce yeni bir kategori oluştur
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Create");
            _wait.Until(ExpectedConditions.ElementExists(By.Id("Name")));
            
            string categoryToDelete = "DeleteTest" + DateTime.Now.Ticks.ToString().Substring(0, 6);
            
            _driver.FindElement(By.Id("Name")).SendKeys(categoryToDelete);
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(1000);
            
            // Kategori listesine git
            _driver.Navigate().GoToUrl($"{BaseUrl}/Category/Index");
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            
            // Oluşturulan kategoriyi bul ve sil butonuna tıkla
            bool foundCategory = false;
            
            // Sayfada kaydırma yaparak tüm kategorileri kontrol et
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            int scrollAttempts = 0;
            int maxScrollAttempts = 5;
            
            while (!foundCategory && scrollAttempts < maxScrollAttempts)
            {
                var rows = _driver.FindElements(By.CssSelector("table tbody tr"));
                
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2 && cells[1].Text.Trim() == categoryToDelete)
                    {
                        foundCategory = true;
                        
                        // Sil butonunu bul ve tıkla
                        var deleteButton = row.FindElement(By.CssSelector(".delete-category"));
                        js.ExecuteScript("arguments[0].scrollIntoView(true);", deleteButton);
                        Thread.Sleep(500);
                        deleteButton.Click();
                        break;
                    }
                }
                
                if (!foundCategory)
                {
                    // Sayfayı aşağı kaydır
                    js.ExecuteScript("window.scrollBy(0, 500);");
                    Thread.Sleep(500);
                    scrollAttempts++;
                }
            }
            
            Assert.IsTrue(foundCategory, $"Silinecek kategori '{categoryToDelete}' bulunamadı.");
            
            // Silme onay modalinin görüntülenmesini bekle
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("deleteConfirmModal")));
            
            // Onay butonuna tıkla
            _driver.FindElement(By.Id("confirmDeleteBtn")).Click();
            Thread.Sleep(2000); // Ajax işleminin tamamlanmasını bekle
            
            // Sayfayı yenile
            _driver.Navigate().Refresh();
            _wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table")));
            
            // Silinen kategorinin artık listede olmadığını kontrol et
            bool categoryStillExists = false;
            scrollAttempts = 0;
            
            while (scrollAttempts < maxScrollAttempts)
            {
                var rows = _driver.FindElements(By.CssSelector("table tbody tr"));
                
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 2 && cells[1].Text.Trim() == categoryToDelete)
                    {
                        categoryStillExists = true;
                        break;
                    }
                }
                
                if (categoryStillExists)
                {
                    break; // Kategori bulundu, döngüyü sonlandır
                }
                
                // Sayfayı aşağı kaydır
                js.ExecuteScript("window.scrollBy(0, 500);");
                Thread.Sleep(500);
                scrollAttempts++;
            }
            
            Assert.IsFalse(categoryStillExists, $"Silinen kategori '{categoryToDelete}' hala listede görünüyor.");
        }

        [TestMethod]
        public void FilterPostsByCategory()
        {
            // Arrange
            _driver.Navigate().GoToUrl(BaseUrl);

            // Make sure the page is fully loaded
            _wait.Until(d => d.FindElement(By.Name("categoryId")).Displayed);

            // Act - select a category from dropdown
            var categorySelect = new SelectElement(_driver.FindElement(By.Name("categoryId")));

            // Make sure we have categories to select
            Assert.IsTrue(categorySelect.Options.Count > 1, "Need at least two category options to test filtering");

            // Store the selected category name before changing it
            string categoryName = categorySelect.Options[1].Text;
            Console.WriteLine($"Selecting category: {categoryName}");

            // Select first non-empty option
            categorySelect.SelectByIndex(1);

            // Wait for page to reload with filtered results
            Thread.Sleep(2000);

            // Need to find elements again after page refresh/update
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".badge.bg-primary")));

            // Assert - posts should be filtered
            var categoryBadges = _driver.FindElements(By.CssSelector(".badge.bg-primary"));
            Assert.IsTrue(categoryBadges.Count > 0, "Expected at least one post with category badge");

            // Verify category name appears in posts
            bool categoryMatches = false;
            foreach (var badge in categoryBadges)
            {
                Console.WriteLine($"Badge text: {badge.Text}");
                if (badge.Text.Contains(categoryName))
                {
                    categoryMatches = true;
                    break;
                }
            }

            Assert.IsTrue(categoryMatches, $"No posts found with the selected category '{categoryName}'");
        }
    }
}
