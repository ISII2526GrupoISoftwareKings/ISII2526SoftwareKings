using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class CreatePurchase_PO : PageObject {

        By streetBy = By.Id("Street");
        By cityBy = By.Id("City");
        By countryBy = By.Id("Country");
        By descriptionBy = By.Id("Description");
        By paymentMethodBy = By.Id("PaymentMethod");
        By submitButtonBy = By.Id("Submit");
        By modifyItemsButtonBy = By.Id("ModifyItems");
        By errorShownBy = By.Id("ErrorsShown");
        By tableOfPurchaseItemsBy = By.Id("TableOfPurchaseItems");
        By dialogOkButtonBy = By.Id("Button_DialogOK");

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void FillPurchaseForm(string street, string city, string country, string description) {
            WaitForBeingClickable(streetBy);
            _driver.FindElement(streetBy).Clear();
            _driver.FindElement(streetBy).SendKeys(street);
            
            _driver.FindElement(cityBy).Clear();
            _driver.FindElement(cityBy).SendKeys(city);
            
            _driver.FindElement(countryBy).Clear();
            _driver.FindElement(countryBy).SendKeys(country);
            
            if (!string.IsNullOrEmpty(description)) {
                _driver.FindElement(descriptionBy).Clear();
                _driver.FindElement(descriptionBy).SendKeys(description);
            }
        }

        public void SelectPaymentMethod(string paymentMethodValue) {
            WaitForBeingClickable(paymentMethodBy);
            var selectElement = new SelectElement(_driver.FindElement(paymentMethodBy));
            selectElement.SelectByValue(paymentMethodValue);
        }

        public void SetItemQuantity(int itemId, int quantity) {
            By quantityInputBy = By.Id($"quantity_{itemId}");
            WaitForBeingClickable(quantityInputBy);
            var input = _driver.FindElement(quantityInputBy);
            input.Clear();
            input.SendKeys(quantity.ToString());
        }

        public void ClickContinue() {
            WaitForBeingClickable(submitButtonBy);
            _driver.FindElement(submitButtonBy).Click();
        }

        public void ConfirmPurchaseDialog() {
            WaitForBeingClickable(dialogOkButtonBy);
            _driver.FindElement(dialogOkButtonBy).Click();
        }

        public void ClickModifyItems() {
            WaitForBeingClickable(modifyItemsButtonBy);
            _driver.FindElement(modifyItemsButtonBy).Click();
        }

        public bool CheckMessageError(string expectedError) {
            WaitForBeingVisible(errorShownBy);
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Actual error message shown: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(expectedError);
        }

        public bool IsOnCreatePurchasePage() {
            try {
                WaitForBeingVisible(submitButtonBy);
                return _driver.FindElement(submitButtonBy).Displayed;
            } catch {
                return false;
            }
        }
    }
}
