using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectItemsForPurchase_PO : PageObject
    {
        private By _inputItemName = By.Id("inputItemName");
        private By _inputBrandName = By.Id("inputBrandName");
        private By _buttonSearchItems = By.Id("searchItems");
        private By _tableOfItemsBy = By.Id("TableOfItems");
        private By _errorShownBy = By.Id("ErrorsShown");
        private By _buttonPurchaseItems = By.Id("purchaseItemsButton");
        private By _noItemsMessageBy = By.XPath("//div[contains(@class, 'alert-warning')]//p[contains(text(), 'There are no items available')]");

        public SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchItems(string itemName, string itemBrand)
        {
            WaitForBeingClickable(_inputItemName);

            _driver.FindElement(_inputItemName).Clear();
            _driver.FindElement(_inputItemName).SendKeys(itemName);

            _driver.FindElement(_inputBrandName).Clear();
            if (!string.IsNullOrEmpty(itemBrand))
            {
                _driver.FindElement(_inputBrandName).SendKeys(itemBrand);
            }

            _driver.FindElement(_buttonSearchItems).Click();
            Thread.Sleep(1000);
        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, _tableOfItemsBy);
        }

        public void AddItemToPurchaseCart(string itemName)
        {
            string safeItemName = itemName.Replace(" ", "_");
            By itemAddButton = By.Id($"itemToPurchase_{safeItemName}");

            WaitForBeingClickable(itemAddButton);
            _driver.FindElement(itemAddButton).Click();
        }

        public void RemoveItemFromPurchaseCart(string itemName)
        {
            string safeItemName = itemName.Replace(" ", "_");
            By removeBtn = By.Id($"removeItem_{safeItemName}");
            WaitForBeingClickable(removeBtn);
            _driver.FindElement(removeBtn).Click();
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(_errorShownBy);
                IWebElement actualErrorShown = _driver.FindElement(_errorShownBy);
                _output.WriteLine($"Actual Message shown: {actualErrorShown.Text}");
                return actualErrorShown.Text.Contains(errorMessage);
            }
            catch
            {
                return false;
            }
        }

        public bool CheckNoItemsMessage()
        {
            try
            {
                Thread.Sleep(500);
                var elements = _driver.FindElements(_noItemsMessageBy);
                return elements.Count > 0 && elements[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool PurchaseNotAvailable()
        {
            try
            {
                return !_driver.FindElement(_buttonPurchaseItems).Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public bool PurchaseButtonAvailable()
        {
            try
            {
                return _driver.FindElement(_buttonPurchaseItems).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void ClickPurchase()
        {
            WaitForBeingClickable(_buttonPurchaseItems);
            _driver.FindElement(_buttonPurchaseItems).Click();
        }
    }
}
