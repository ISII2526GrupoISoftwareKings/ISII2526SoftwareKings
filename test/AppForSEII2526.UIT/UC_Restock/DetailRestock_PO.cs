using System;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class DetailRestock_PO : PageObject
    {
        private By _restockId = By.Id("RestockId");
        private By _adminName = By.Id("AdminName");
        private By _title = By.Id("Title");
        private By _restockDate = By.Id("RestockDate");
        private By _totalPrice = By.Id("TotalPrice");
        private By _description = By.Id("Description");
        private By _deliveryAddress = By.Id("DeliveryAddress");
        private By _restockedItemsTable = By.Id("RestockedItems");

        public DetailRestock_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckRestockDetails(string expectedAdminName, string expectedTitle,
            string expectedDescription, string expectedAddress, string expectedTotalPrice,
            string todayDate)
        {
            try
            {
                Thread.Sleep(2000);
                WaitForBeingVisible(_restockId);

                var actualAdminName = _driver.FindElement(_adminName).Text;
                var actualTitle = _driver.FindElement(_title).Text;
                var actualDescription = _driver.FindElement(_description).Text;
                var actualAddress = _driver.FindElement(_deliveryAddress).Text;
                var actualTotalPrice = _driver.FindElement(_totalPrice).Text;
                var actualRestockDate = _driver.FindElement(_restockDate).Text;

                _output.WriteLine($"Expected Title: {expectedTitle}, Actual: {actualTitle}");
                _output.WriteLine($"Expected Price: {expectedTotalPrice}, Actual: {actualTotalPrice}");

                return actualAdminName.Contains(expectedAdminName) &&
                       actualTitle == expectedTitle &&
                       actualDescription == expectedDescription &&
                       actualAddress == expectedAddress &&
                       actualTotalPrice == expectedTotalPrice &&
                       actualRestockDate.Contains(todayDate);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error checking restock details: {ex.Message}");
                return false;
            }
        }

        public bool CheckRestockedItem(string itemName, string brand, string unitPrice, string quantity, string subtotal)
        {
            try
            {
                WaitForBeingVisible(_restockedItemsTable);
                var table = _driver.FindElement(_restockedItemsTable);
                var rows = table.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 5)
                    {
                        var actualName = cells[0].Text;
                        var actualBrand = cells[1].Text;
                        var actualPrice = cells[3].Text;
                        var actualQty = cells[2].Text;
                        var actualSubtotal = cells[4].Text;

                        if (actualName == itemName &&
                            actualBrand == brand &&
                            actualPrice == unitPrice &&
                            actualQty == quantity &&
                            actualSubtotal == subtotal)
                        {
                            return true;
                        }
                    }
                }
                _output.WriteLine($"Item {itemName} not found in detail table");
                return false;
            }
            catch { return false; }
        }
    }
}