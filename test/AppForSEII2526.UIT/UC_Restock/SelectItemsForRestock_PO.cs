using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class SelectItemsForRestock_PO : PageObject
    {
        private By _inputItemName = By.Id("inputItemName");
        private By _inputQuantity = By.Id("inputQuantity");
        private By _buttonSearchItems = By.Id("searchItems");
        private By _tableOfItemsBy = By.Id("TableOfItems");
        private By _errorShownBy = By.Id("ErrorsShown");
        private By _buttonCreateRestock = By.Id("createRestockButton");

        public SelectItemsForRestock_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchItems(string itemName, string maxQuantity)
        {
            WaitForBeingClickable(_inputItemName);

            _driver.FindElement(_inputItemName).Clear();
            _driver.FindElement(_inputItemName).SendKeys(itemName);

            _driver.FindElement(_inputQuantity).Clear();
            if (!string.IsNullOrEmpty(maxQuantity))
            {
                _driver.FindElement(_inputQuantity).SendKeys(maxQuantity);
            }

            _driver.FindElement(_buttonSearchItems).Click();
            Thread.Sleep(1000);
        }

        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, _tableOfItemsBy);
        }

        public void AddItemToRestockCart(int itemId)
        {
            By itemAddButton = By.Id($"itemToRestock_{itemId}");

            WaitForBeingClickable(itemAddButton);
            _driver.FindElement(itemAddButton).Click();
        }

        public void RemoveItemFromRestockCart(int itemId)
        {
            By removeBtn = By.Id($"removeItem_{itemId}");
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

        public bool CreateRestockNotAvailable()
        {
            try
            {
                return !_driver.FindElement(_buttonCreateRestock).Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public void ClickCreateRestock()
        {
            WaitForBeingClickable(_buttonCreateRestock);
            _driver.FindElement(_buttonCreateRestock).Click();
        }
    }
}