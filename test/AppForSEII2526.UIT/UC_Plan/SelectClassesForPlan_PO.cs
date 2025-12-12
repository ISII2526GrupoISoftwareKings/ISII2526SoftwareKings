using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class SelectClassesForPlan_PO : PageObject
    {
        By inputClassName = By.Id("inputClassName");
        By inputClassDate = By.Id("classDate");
        By buttonSearchClasses = By.Id("searchClasses");
        By tableOfClassesBy = By.Id("TableOfClasses");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonCreatePlan = By.Id("createPlanButton");

        public SelectClassesForPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchClasses(string className, string classDate)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputClassName);
            _driver.FindElement(inputClassName).SendKeys(className);

            if (classDate != "")
                _driver.FindElement(inputClassDate).SendKeys(classDate);

            _driver.FindElement(buttonSearchClasses).Click();
        }

        public bool CheckListOfClasses(List<string[]> expectedClasses)
        {
            return CheckBodyTable(expectedClasses, tableOfClassesBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddClassToPlanCart(string className)
        {
            WaitForBeingClickable(By.Id("classToPlan_" + className));
            _driver.FindElement(By.Id("classToPlan_" + className)).Click();
        }

        public void RemoveClassFromPlanCart(int classId)
        {
            WaitForBeingClickable(By.Id("removeClass_" + classId));
            _driver.FindElement(By.Id("removeClass_" + classId)).Click();
        }

        public bool CreatePlanNotAvailable()
        {
            //the button is not Displayed=hidden
            return _driver.FindElement(buttonCreatePlan).Displayed == false;
        }

    }
}