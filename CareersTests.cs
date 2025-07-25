using OpenQA.Selenium;

namespace _Task__Selenium_WebDriver
{
    public class CareersTests : BaseTest
    {      
        [TestCase("Java", "All Locations")]
        [TestCase("C#", "All Locations")]
        [TestCase("Python", "All Locations")]
        public void ValidateJobSearchByCriteria(string language, string locationName)
        {
            NavigateToCareers();
            EnterKeyword(language);
            SelectLocation(locationName);
            ToggleRemoteOption();
            SubmitSearch();
            OpenLastSearchResult();
            AssertLanguageIsPresent(language);
        }

        private void NavigateToCareers()
        {
            driver.FindElement(By.LinkText("Careers")).Click();
        }

        private void EnterKeyword(string language)
        {
            var keywordField = driver.FindElements(By.CssSelector(".recruiting-search__keyword"))[1];
            keywordField.SendKeys(language);
        }

        private void SelectLocation(string locationName)
        {
            var locationDropdown = wait.Until(d => d.FindElement(By.CssSelector(".recruiting-search__location")));
            locationDropdown.Click();

            var optionToClick = wait.Until(d => d.FindElement(By.XPath($"//li[contains(text(), '{locationName}')]")));
            optionToClick.Click();
        }

        private void ToggleRemoteOption()
        {
            var remoteElement = driver.FindElement(By.XPath("//input[@name='remote']/.."));
            remoteElement.Click();
        }

        private void SubmitSearch()
        {
            driver.FindElement(By.CssSelector("#jobSearchFilterForm > button")).Click();
        }

        private void OpenLastSearchResult()
        {
            string lastResultXPath = "//ul[@class='search-result__list']/li[last()]/descendant::a[text()='View and apply']";
            wait.Until(d => d.FindElements(By.XPath(lastResultXPath)).Count > 0);
            var link = driver.FindElement(By.XPath(lastResultXPath));
            link.Click();
        }

        private void AssertLanguageIsPresent(string language)
        {
            bool isPresent = driver.FindElements(By.XPath($"//*[contains(text(), '{language}')]")).Count > 0;
            Assert.That(isPresent, Is.True, $"Expected programming language '{language}' is not found on the page.");
        }
    }
}
