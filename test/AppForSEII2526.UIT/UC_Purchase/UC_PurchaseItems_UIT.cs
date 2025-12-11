using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class UC_PurchaseItems_UIT : UC_UIT {

        private SelectItemsForPurchase_PO selectItemsForPurchase_PO;

        // Test data constants for Item 1: iPhone 15
        private const int itemId1 = 1;
        private const string itemName1 = "iPhone 15";
        private const string itemBrand1 = "Apple";
        private const string itemDescription1 = "Smartphone 6.1\"";
        private const string itemPrice1 = "999.00";
        private const string itemQuantity1 = "5";

        // Test data constants for Item 2: XPS 13
        private const int itemId2 = 2;
        private const string itemName2 = "XPS 13";
        private const string itemBrand2 = "Dell";
        private const string itemDescription2 = "Ultrabook 13\"";
        private const string itemPrice2 = "1399.00";
        private const string itemQuantity2 = "10";

        public UC_PurchaseItems_UIT(ITestOutputHelper output) : base(output) {
            selectItemsForPurchase_PO = new SelectItemsForPurchase_PO(_driver, _output);
        }

        private void Precondition_perform_login() {
            Perform_login("hugo@uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchaseItems() {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectItemsForPurchase_PO.WaitForBeingVisible(By.LinkText("Purchase"));
            //we click on the menu
            _driver.FindElement(By.LinkText("Purchase")).Click();
        }

        // Basic Flow: System shows a list of items available to purchase
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_BF_2_ShowListOfItems() {
            //Arrange
            InitialStepsForPurchaseItems();
            var expectedItems = new List<string[]> {
                new string[] { itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantity1 },
                new string[] { itemName2, itemBrand2, itemDescription2, itemPrice2, itemQuantity2 }
            };

            //Act - no filters, just view all items
            selectItemsForPurchase_PO.SearchItems("", "");

            //Assert
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        // Basic Flow: User selects desired items and clicks on Buy
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_BF_3_SelectItemAndBuy() {
            //Arrange
            InitialStepsForPurchaseItems();

            //Act
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName1);

            //Assert - Buy button should be available
            Assert.True(selectItemsForPurchase_PO.PurchaseButtonAvailable());
        }

        // Basic Flow: User selects multiple items
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_BF_3_SelectMultipleItems() {
            //Arrange
            InitialStepsForPurchaseItems();

            //Act
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName1);
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName2);

            //Assert - Buy button should be available
            Assert.True(selectItemsForPurchase_PO.PurchaseButtonAvailable());
        }

        // AF1 (No items available): System shows error message (filtered to no results)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_AF1_NoItemsAvailable() {
            //Arrange
            InitialStepsForPurchaseItems();

            //Act - search for non-existent item
            selectItemsForPurchase_PO.SearchItems("NonExistentItem", "NonExistentBrand");

            //Assert - should show no items message
            Assert.True(selectItemsForPurchase_PO.CheckNoItemsMessage());
        }

        // AF2 (Filter items by name and brand): User filters by name
        [Theory]
        [InlineData(itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantity1, "iPhone", "")]
        [InlineData(itemName2, itemBrand2, itemDescription2, itemPrice2, itemQuantity2, "XPS", "")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_AF2_FilterByName(string itemName, string itemBrand, string itemDescription,
            string itemPrice, string itemQuantity, string filterName, string filterBrand) {
            //Arrange
            InitialStepsForPurchaseItems();
            var expectedItems = new List<string[]> {
                new string[] { itemName, itemBrand, itemDescription, itemPrice, itemQuantity }
            };

            //Act
            selectItemsForPurchase_PO.SearchItems(filterName, filterBrand);

            //Assert
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        // AF2 (Filter items by name and brand): User filters by brand
        [Theory]
        [InlineData(itemName1, itemBrand1, itemDescription1, itemPrice1, itemQuantity1, "", "Apple")]
        [InlineData(itemName2, itemBrand2, itemDescription2, itemPrice2, itemQuantity2, "", "Dell")]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_AF2_FilterByBrand(string itemName, string itemBrand, string itemDescription,
            string itemPrice, string itemQuantity, string filterName, string filterBrand) {
            //Arrange
            InitialStepsForPurchaseItems();
            var expectedItems = new List<string[]> {
                new string[] { itemName, itemBrand, itemDescription, itemPrice, itemQuantity }
            };

            //Act
            selectItemsForPurchase_PO.SearchItems(filterName, filterBrand);

            //Assert
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        // AF3 (Modify cart): User adds item, then removes it (modifies cart)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_AF3_ModifyCartRemoveItem() {
            //Arrange
            InitialStepsForPurchaseItems();

            //Act
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName1);
            selectItemsForPurchase_PO.RemoveItemFromPurchaseCart(itemName1);

            //Assert - cart should be empty, Buy button not available
            Assert.True(selectItemsForPurchase_PO.PurchaseNotAvailable());
        }
    }
}
