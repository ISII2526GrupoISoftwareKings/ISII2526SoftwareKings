using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class UC_PurchaseItems_UIT : UC_UIT {

        private SelectItemsForPurchase_PO selectItemsForPurchase_PO;

        public UC_PurchaseItems_UIT(ITestOutputHelper output) : base(output) {
        }

        private void Precondition_perform_login() {
            Perform_login("wedonthaveone@gmail.com", "wedonthaveone");
        }

        private void InitialStepsForPurchaseItems() {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectItemsForPurchase_PO.WaitForBeingVisible(By.LinkText("Purchase"));
            //we click on the menu
            _driver.FindElement(By.LinkText("Purchase")).Click();
        }
    }
}
