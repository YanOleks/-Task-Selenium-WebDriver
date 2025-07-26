using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace _Task__Selenium_WebDriver
{
    public class GlobalSearchTests : BaseTest
    {
        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void GlobalSearch_ShouldReturnExpectedResults(string searchTerm)
        {
            var searchButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@class, 'header-search__button')]")));
            searchButton.Click();
     
            var searchInput = wait.Until(d => d.FindElement(By.TagName("input")));
            wait.Until(s => searchInput.Displayed && searchInput.Enabled);
            searchInput.SendKeys(searchTerm);
            driver.FindElement(By.XPath("//form[contains(@class, 'header-search__field')]//button")).Click();


            var links = wait.Until(d => {
                var elements = d.FindElements(By.XPath("//div[@class='search-results__items']/article/h3/a"));
                return elements.Count > 0 ? elements : null;
            });

            bool allContainKeyword = links
                .Select(link => link.Text)
                .All(text => text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(links, Is.Not.Empty, "No search results were found.");
                Assert.That(allContainKeyword, Is.True, "Not all links contain the expected keywords.");
            }
        }
    }
}
