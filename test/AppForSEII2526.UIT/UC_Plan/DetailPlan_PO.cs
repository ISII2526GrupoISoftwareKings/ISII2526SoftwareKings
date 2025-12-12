using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class DetailPlan_PO : PageObject
    {
        // Locators for plan details
        By nameSurname = By.Id("NameSurname");
        By planDate = By.Id("PlanDate");
        By totalPrice = By.Id("TotalPrice");
        By planName = By.Id("PlanName");
        By description = By.Id("Description");
        By numberOfWeeks = By.Id("NumberOfWeeks");
        By healthIssues = By.Id("HealthIssues");
        By enrolledClassesTable = By.Id("EnrolledClasses");

        public DetailPlan_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckPlanDetails(string expectedNameSurname, string expectedPlanName, 
            string expectedDescription, string expectedWeeks, string expectedHealthIssues, 
            string expectedTotalPrice, string todayDate)
        {
            try
            {
                // Wait for navigation to complete
                Thread.Sleep(2000);
                WaitForBeingVisible(nameSurname);
                
                var actualNameSurname = _driver.FindElement(nameSurname).Text;
                var actualPlanDate = _driver.FindElement(planDate).Text;
                var actualTotalPrice = _driver.FindElement(totalPrice).Text;
                var actualPlanName = _driver.FindElement(planName).Text;
                var actualDescription = _driver.FindElement(description).Text;
                var actualWeeks = _driver.FindElement(numberOfWeeks).Text;
                var actualHealthIssues = _driver.FindElement(healthIssues).Text;

                _output.WriteLine($"Expected Name/Surname: {expectedNameSurname}, Actual: {actualNameSurname}");
                _output.WriteLine($"Expected Plan Name: {expectedPlanName}, Actual: {actualPlanName}");
                _output.WriteLine($"Expected Description: {expectedDescription}, Actual: {actualDescription}");
                _output.WriteLine($"Expected Weeks: {expectedWeeks}, Actual: {actualWeeks}");
                _output.WriteLine($"Expected Health Issues: {expectedHealthIssues}, Actual: {actualHealthIssues}");
                _output.WriteLine($"Expected Total Price: {expectedTotalPrice}, Actual: {actualTotalPrice}");
                _output.WriteLine($"Expected Date (contains): {todayDate}, Actual: {actualPlanDate}");

                return actualNameSurname == expectedNameSurname &&
                       actualPlanName == expectedPlanName &&
                       actualDescription == expectedDescription &&
                       actualWeeks == expectedWeeks &&
                       actualHealthIssues == expectedHealthIssues &&
                       actualTotalPrice == expectedTotalPrice &&
                       actualPlanDate.Contains(todayDate);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error checking plan details: {ex.Message}");
                return false;
            }
        }

        public bool CheckEnrolledClass(string className, string typeItems, string price, 
            string date, string time, string goal = "")
        {
            try
            {
                WaitForBeingVisible(enrolledClassesTable);
                
                var table = _driver.FindElement(enrolledClassesTable);
                var rows = table.FindElements(By.TagName("tr"));

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));
                    if (cells.Count >= 6)
                    {
                        var actualName = cells[0].Text;
                        var actualType = cells[1].Text;
                        var actualPrice = cells[2].Text;
                        var actualDate = cells[3].Text;
                        var actualTime = cells[4].Text;
                        var actualGoal = cells[5].Text;

                        _output.WriteLine($"Row - Name: {actualName}, Type: {actualType}, Price: {actualPrice}, Date: {actualDate}, Time: {actualTime}, Goal: {actualGoal}");

                        if (actualName == className &&
                            actualType == typeItems &&
                            actualPrice == price &&
                            actualDate == date &&
                            actualTime == time &&
                            actualGoal == goal)
                        {
                            _output.WriteLine($"Found matching class: {className}");
                            return true;
                        }
                    }
                }

                _output.WriteLine($"Class {className} not found in enrolled classes table");
                return false;
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error checking enrolled class: {ex.Message}");
                return false;
            }
        }
    }
}
