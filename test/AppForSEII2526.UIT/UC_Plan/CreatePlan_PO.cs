using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class CreatePlan_PO : PageObject
    {
        // Locators for form fields
        By inputUserName = By.Id("Name");
        By inputPlanName = By.Id("PlanName");
        By inputWeeks = By.Id("Weeks");
        By inputDescription = By.Id("Description");
        By inputHealthIssues = By.Id("HealthIssues");
        By selectPaymentMethod = By.Id("PaymentMethod");

        // Locators for buttons
        By buttonSubmit = By.Id("Submit");
        By buttonModifyClasses = By.Id("ModifyClasses");
        By buttonDialogOK = By.Id("Button_DialogOK");
        By buttonDialogCancel = By.Id("Button_DialogCancel");

        // Locators for error messages
        By errorShownBy = By.Id("ErrorsShown");
        By validationSummary = By.CssSelector(".validation-summary-errors");
        By modalDialog = By.Id("DialogOKSaveDelete");

        public CreatePlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPlanForm(string userName, string planName, string weeks, string paymentMethod)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputUserName);
            
            _driver.FindElement(inputUserName).Clear();
            _driver.FindElement(inputUserName).SendKeys(userName);

            _driver.FindElement(inputPlanName).Clear();
            _driver.FindElement(inputPlanName).SendKeys(planName);

            _driver.FindElement(inputWeeks).Clear();
            _driver.FindElement(inputWeeks).SendKeys(weeks);

            SelectElement selectElement = new SelectElement(_driver.FindElement(selectPaymentMethod));
            selectElement.SelectByText(paymentMethod);
        }

        public void ClickSubmit()
        {
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();
        }

        public void ConfirmDialog()
        {
            WaitForBeingClickable(buttonDialogOK);
            _driver.FindElement(buttonDialogOK).Click();
        }

        public bool IsDialogVisible()
        {
            try
            {
                WaitForBeingVisible(modalDialog);
                return _driver.FindElement(modalDialog).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(errorShownBy);
                IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
                _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
                return actualErrorShown.Text.Contains(errorMessage);
            }
            catch
            {
                return false;
            }
        }

        public bool CheckValidationSummaryError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(validationSummary);
                IWebElement actualValidationSummary = _driver.FindElement(validationSummary);
                _output.WriteLine($"actual Validation Summary:{actualValidationSummary.Text}");
                return actualValidationSummary.Text.Contains(errorMessage);
            }
            catch
            {
                return false;
            }
        }

        public void ClickModifyClasses()
        {
            WaitForBeingClickable(buttonModifyClasses);
            _driver.FindElement(buttonModifyClasses).Click();
        }
    }
}
