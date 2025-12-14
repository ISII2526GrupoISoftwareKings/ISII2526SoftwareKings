using System;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class CreatePurchase_PO : PageObject
    {
        private By _inputStreet = By.Id("Street");
        private By _inputCity = By.Id("City");
        private By _inputCountry = By.Id("Country");
        private By _inputDescription = By.Id("Description");
        private By _selectPaymentMethod = By.Id("PaymentMethod");
        private By _buttonSubmit = By.Id("Submit");
        private By _buttonModify = By.Id("ModifyItems");

        private By _buttonDialogOK = By.Id("Button_DialogOK");

        private By _errorShownBy = By.Id("ErrorsShown");
        private By _validationSummary = By.CssSelector(".alert.alert-danger");

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPurchaseForm(string street, string city, string country, string description)
        {
            WaitForBeingClickable(_inputStreet);

            _driver.FindElement(_inputStreet).Clear();
            _driver.FindElement(_inputStreet).SendKeys(street);

            _driver.FindElement(_inputCity).Clear();
            _driver.FindElement(_inputCity).SendKeys(city);

            _driver.FindElement(_inputCountry).Clear();
            _driver.FindElement(_inputCountry).SendKeys(country);

            _driver.FindElement(_inputDescription).Clear();
            if (!string.IsNullOrEmpty(description))
            {
                _driver.FindElement(_inputDescription).SendKeys(description);
            }
        }

        public void SelectPaymentMethod(string paymentMethodValue)
        {
            WaitForBeingClickable(_selectPaymentMethod);
            var selectElement = new SelectElement(_driver.FindElement(_selectPaymentMethod));
            selectElement.SelectByValue(paymentMethodValue);
        }

        public void SetItemQuantity(int itemId, string quantity)
        {
            By qtyInput = By.Id($"quantity_{itemId}");
            WaitForBeingVisible(qtyInput);
            _driver.FindElement(qtyInput).Clear();
            _driver.FindElement(qtyInput).SendKeys(quantity);
        }

        public void ClickSubmit()
        {
            WaitForBeingClickable(_buttonSubmit);
            _driver.FindElement(_buttonSubmit).Click();
        }

        public void ClickModify()
        {
            WaitForBeingClickable(_buttonModify);
            _driver.FindElement(_buttonModify).Click();
        }

        public void ConfirmDialog()
        {
            Thread.Sleep(500);
            try
            {
                WaitForBeingClickable(_buttonDialogOK);
                _driver.FindElement(_buttonDialogOK).Click();
            }
            catch (NoSuchElementException)
            {
                _driver.FindElement(By.XPath("//button[contains(text(), 'Yes') or contains(text(), 'Confirm')]")).Click();
            }
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(_errorShownBy);
                IWebElement actualErrorShown = _driver.FindElement(_errorShownBy);
                _output.WriteLine($"Actual error shown: {actualErrorShown.Text}");
                return actualErrorShown.Text.Contains(errorMessage);
            }
            catch { return false; }
        }

        public bool CheckValidationSummaryError(string errorMessage)
        {
            Thread.Sleep(500); // Wait for validation to render
            try
            {
                // Blazor ValidationSummary renders as ul.validation-errors inside an alert
                By validationErrors = By.CssSelector(".validation-errors, .alert.alert-danger");
                WaitForBeingVisible(validationErrors);
                IWebElement actualSummary = _driver.FindElement(validationErrors);
                _output.WriteLine($"Actual validation summary: {actualSummary.Text}");
                return actualSummary.Text.Contains(errorMessage);
            }
            catch { return false; }
        }
    }
}
