using System;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class DetailPurchase_PO : PageObject
    {
        private By _deliveryAddress = By.Id("DeliveryAddress");
        private By _totalPrice = By.Id("TotalPrice");
        private By _description = By.Id("Description");
        private By _paymentMethod = By.Id("PaymentMethod");
        private By _purchaseDate = By.Id("PurchaseDate");
        private By _purchasedItemsTable = By.Id("PurchasedItems");

        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckPurchaseDetails(string expectedAddress, string expectedTotalPrice,
            string expectedDescription, string expectedPaymentMethod, string todayDate)
        {
            try
            {
                Thread.Sleep(2000);
                WaitForBeingVisible(_deliveryAddress);

                var actualAddress = _driver.FindElement(_deliveryAddress).Text;
                var actualTotalPrice = _driver.FindElement(_totalPrice).Text;
                var actualDescription = _driver.FindElement(_description).Text;
                var actualPaymentMethod = _driver.FindElement(_paymentMethod).Text;
                var actualPurchaseDate = _driver.FindElement(_purchaseDate).Text;

                _output.WriteLine($"Expected Address: {expectedAddress}, Actual: {actualAddress}");
                _output.WriteLine($"Expected Price: {expectedTotalPrice}, Actual: {actualTotalPrice}");
                _output.WriteLine($"Expected Payment: {expectedPaymentMethod}, Actual: {actualPaymentMethod}");

                return actualAddress.Contains(expectedAddress) &&
                       actualTotalPrice == expectedTotalPrice &&
                       actualDescription == expectedDescription &&
                       actualPaymentMethod == expectedPaymentMethod &&
                       actualPurchaseDate.Contains(todayDate);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error checking purchase details: {ex.Message}");
                return false;
            }
        }

        public bool CheckPurchasedItem(string itemName, string brand, string quantity, string unitPrice)
        {
            try
            {
                WaitForBeingVisible(_purchasedItemsTable);
                var table = _driver.FindElement(_purchasedItemsTable);
                var rows = table.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 4)
                    {
                        var actualName = cells[0].Text;
                        var actualBrand = cells[1].Text;
                        var actualQty = cells[2].Text;
                        var actualPrice = cells[3].Text;

                        if (actualName == itemName &&
                            actualBrand == brand &&
                            actualQty == quantity &&
                            actualPrice == unitPrice)
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
