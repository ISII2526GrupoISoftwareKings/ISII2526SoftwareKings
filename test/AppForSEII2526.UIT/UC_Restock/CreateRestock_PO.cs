using System;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class CreateRestock_PO : PageObject
    {
        private By _inputTitle = By.Id("RestockTitle");
        private By _inputDeliveryAddress = By.Id("DeliveryAddress");
        private By _inputDescription = By.Id("Description");
        private By _inputExpectedDate = By.Id("ExpectedDate");
        private By _buttonSubmit = By.Id("Submit");
        private By _buttonModify = By.Id("ModifyItems");

        private By _buttonDialogOK = By.Id("Button_DialogOK");

        private By _errorShownBy = By.Id("ErrorsShown");
        private By _validationSummary = By.CssSelector(".alert.alert-danger");

        public CreateRestock_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillRestockForm(string title, string address, string description, DateTime? expectedDate)
        {
            WaitForBeingClickable(_inputTitle);

            _driver.FindElement(_inputTitle).Clear();
            _driver.FindElement(_inputTitle).SendKeys(title);

            _driver.FindElement(_inputDeliveryAddress).Clear();
            _driver.FindElement(_inputDeliveryAddress).SendKeys(address);

            _driver.FindElement(_inputDescription).Clear();
            if (!string.IsNullOrEmpty(description))
            {
                _driver.FindElement(_inputDescription).SendKeys(description);
            }

            if (expectedDate.HasValue)
            {
                InputDateInDatePicker(_inputExpectedDate, expectedDate.Value);
            }
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
            try
            {
                WaitForBeingVisible(_validationSummary);
                IWebElement actualSummary = _driver.FindElement(_validationSummary);
                _output.WriteLine($"Actual validation summary: {actualSummary.Text}");
                return actualSummary.Text.Contains(errorMessage);
            }
            catch { return false; }
        }
    }
}