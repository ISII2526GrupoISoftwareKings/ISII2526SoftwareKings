using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class SelectItemsForPurchase_PO : PageObject {

        By inputItemName = By.Id("inputItemName");
        By inputBrandName = By.Id("inputBrandName");
        By buttonSearchItems = By.Id("searchItems");
        By tableOfItemsBy = By.Id("TableOfItems");

        public SelectItemsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void SearchItems(string itemName, string itemBrand) {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputItemName);
            _driver.FindElement(inputItemName).SendKeys(itemName);

            if (itemBrand != "")
                _driver.FindElement(inputBrandName).SendKeys(itemBrand);

            _driver.FindElement(buttonSearchItems).Click();
        }

        public bool CheckListOfItems(List<string[]> expectedItems) {
            return CheckBodyTable(expectedItems, tableOfItemsBy);
        }
    }
}
