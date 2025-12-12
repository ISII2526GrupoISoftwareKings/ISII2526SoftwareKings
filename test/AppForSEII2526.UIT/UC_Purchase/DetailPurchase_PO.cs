using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class DetailPurchase_PO : PageObject {

        By deliveryAddressBy = By.Id("DeliveryAddress");
        By totalPriceBy = By.Id("TotalPrice");
        By descriptionBy = By.Id("Description");
        By paymentMethodBy = By.Id("PaymentMethod");
        By purchaseDateBy = By.Id("PurchaseDate");
        By purchasedItemsTableBy = By.Id("PurchasedItems");
        By totalPriceFooterBy = By.Id("TotalPriceFooter");

        public DetailPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void GoToDetailPurchase(string baseUri, int purchaseId) {
            // Navigate directly to the detail purchase page with the purchase ID
            _driver.Navigate().GoToUrl($"{baseUri}purchase/detailpurchase?PurchaseID={purchaseId}");
            // Wait for the page to load
            WaitForBeingVisible(deliveryAddressBy);
        }

        public string GetDeliveryAddress() {
            WaitForBeingVisible(deliveryAddressBy);
            return _driver.FindElement(deliveryAddressBy).Text;
        }

        public string GetTotalPrice() {
            WaitForBeingVisible(totalPriceBy);
            return _driver.FindElement(totalPriceBy).Text;
        }

        public string GetDescription() {
            WaitForBeingVisible(descriptionBy);
            return _driver.FindElement(descriptionBy).Text;
        }

        public string GetPaymentMethod() {
            WaitForBeingVisible(paymentMethodBy);
            return _driver.FindElement(paymentMethodBy).Text;
        }

        public string GetPurchaseDate() {
            WaitForBeingVisible(purchaseDateBy);
            return _driver.FindElement(purchaseDateBy).Text;
        }

        public string GetTotalPriceFooter() {
            WaitForBeingVisible(totalPriceFooterBy);
            return _driver.FindElement(totalPriceFooterBy).Text;
        }

        public bool CheckPurchasedItemsTable(List<string[]> expectedItems) {
            return CheckBodyTable(expectedItems, purchasedItemsTableBy);
        }
    }
}
