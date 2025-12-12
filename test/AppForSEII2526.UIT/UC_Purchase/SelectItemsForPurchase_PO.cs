using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class SelectItemsForPurchase_PO : PageObject {
        
        By inputItemName = By.Id("inputItemName");
        By inputBrandName = By.Id("inputBrandName");
        By buttonSearchItems = By.Id("searchItems");
        By tableOfItemsBy = By.Id("TableOfItems");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonPurchaseItems = By.Id("purchaseItemsButton");
        By noItemsMessageBy = By.XPath("//div[contains(@class, 'alert-warning')]//p[contains(text(), 'There are no items available')]");

        public SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void SearchItems(string itemName, string itemBrand) {
            // Wait for the item name input to be clickable
            WaitForBeingClickable(inputItemName);
            _driver.FindElement(inputItemName).SendKeys(itemName);
            if (itemBrand != "")
                _driver.FindElement(inputBrandName).SendKeys(itemBrand);
            _driver.FindElement(buttonSearchItems).Click();
        }

        public bool CheckListOfItems(List<string[]> expectedItems) {
            return CheckBodyTable(expectedItems, tableOfItemsBy);
        }

        public bool CheckMessageError(string errorMessage) {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Actual error message shown: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddItemToPurchaseCart(string itemName) {
            string safeItemName = itemName.Replace(" ", "_");
            WaitForBeingClickable(By.Id("itemToPurchase_" + safeItemName));
            _driver.FindElement(By.Id("itemToPurchase_" + safeItemName)).Click();
        }

        public void RemoveItemFromPurchaseCart(string itemName) {
            string safeItemName = itemName.Replace(" ", "_");
            WaitForBeingClickable(By.Id("removeItem_" + safeItemName));
            _driver.FindElement(By.Id("removeItem_" + safeItemName)).Click();
        }

        public bool PurchaseNotAvailable() {
            // The button should be hidden when there are no items in the cart.
            return _driver.FindElement(buttonPurchaseItems).Displayed == false;
        }

        public bool PurchaseButtonAvailable() {
            // The button should be visible when there are items in the cart.
            return _driver.FindElement(buttonPurchaseItems).Displayed == true;
        }

        public bool CheckNoItemsMessage() {
            // Check if the warning message is displayed when no items match filters
            var elements = _driver.FindElements(noItemsMessageBy);
            return elements.Count > 0 && elements[0].Displayed;
        }
    }
}
