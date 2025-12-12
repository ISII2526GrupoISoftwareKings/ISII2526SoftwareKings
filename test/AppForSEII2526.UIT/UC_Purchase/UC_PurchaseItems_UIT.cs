using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase {
    public class UC_PurchaseItems_UIT : UC_UIT {

        private SelectItemsForPurchase_PO selectItemsForPurchase_PO;
        private DetailPurchase_PO detailPurchase_PO;
        private CreatePurchase_PO createPurchase_PO;

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

        // Test data constants for seeded Purchase (Hugo's purchase from SeedData)
        private const int purchaseId = 1;
        private const string purchaseStreet = "Calle Mayor 25";
        private const string purchaseCity = "Ciudad Real";
        private const string purchaseCountry = "Spain";
        private const string purchaseDescription = "Compra inicial de equipamiento";
        private const string purchasePaymentMethod = "CreditCard";
        private const string purchaseTotalPrice = "1998.00 €";

        public UC_PurchaseItems_UIT(ITestOutputHelper output) : base(output) {
            selectItemsForPurchase_PO = new SelectItemsForPurchase_PO(_driver, _output);
            detailPurchase_PO = new DetailPurchase_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
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

        // Select Items Tests (Basic Flow and Alternative Flows 1-3)

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


        // Detail Purchase Tests (Basic Flow - Step 7)

        // Basic Flow Step 7: System shows purchase details (delivery_address, total_price, description, payment_method)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_BF_7_ShowsPurchaseDetails() {
            //Arrange
            Precondition_perform_login();

            //Act - Navigate to seeded purchase detail
            detailPurchase_PO.GoToDetailPurchase(_URI, purchaseId);

            //Assert - Verify all purchase details are correctly displayed
            string actualAddress = detailPurchase_PO.GetDeliveryAddress();
            Assert.Contains(purchaseStreet, actualAddress);
            Assert.Contains(purchaseCity, actualAddress);
            Assert.Contains(purchaseCountry, actualAddress);

            Assert.Equal(purchaseTotalPrice, detailPurchase_PO.GetTotalPrice());
            Assert.Equal(purchaseDescription, detailPurchase_PO.GetDescription());
            Assert.Equal(purchasePaymentMethod, detailPurchase_PO.GetPaymentMethod());
        }

        // Basic Flow Step 7: System shows purchased items with name, brand, quantity, price
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_BF_7_ShowsPurchasedItems() {
            //Arrange
            Precondition_perform_login();
            // Expected items: iPhone 15, Apple, 2 units, 999.00 € (from SeedData)
            var expectedItems = new List<string[]> {
                new string[] { itemName1, itemBrand1, "2", itemPrice1 + " €" }
            };

            //Act
            detailPurchase_PO.GoToDetailPurchase(_URI, purchaseId);

            //Assert - Verify items table and total price consistency
            Assert.True(detailPurchase_PO.CheckPurchasedItemsTable(expectedItems));
            Assert.Equal(detailPurchase_PO.GetTotalPrice(), detailPurchase_PO.GetTotalPriceFooter());
        }

        // Create Purchase Tests (Basic Flow Steps 4-6, Alternative Flows 4-5)

        // Basic Flow Steps 4-6: User fills purchase form with valid data and completes purchase
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_BF_4_6_CreatePurchaseWithValidData() {
            //Arrange
            InitialStepsForPurchaseItems();
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName2); // Add XPS 13

            //Act - Navigate to create purchase and fill form
            _driver.FindElement(By.Id("purchaseItemsButton")).Click();
            createPurchase_PO.FillPurchaseForm("Test Street 123", "Test City", "Test Country", "Test purchase description");
            createPurchase_PO.SelectPaymentMethod("1"); // CreditCard
            createPurchase_PO.SetItemQuantity(itemId2, 1);
            createPurchase_PO.ClickContinue();
            createPurchase_PO.ConfirmPurchaseDialog();

            //Assert - Should navigate to detail page (purchase completed successfully)
            // Wait for navigation to detail page
            detailPurchase_PO.WaitForBeingVisible(By.Id("DeliveryAddress"));
            string actualAddress = detailPurchase_PO.GetDeliveryAddress();
            Assert.Contains("Test Street 123", actualAddress);
            Assert.Contains("Test City", actualAddress);
            Assert.Contains("Test Country", actualAddress);
        }

        // Alternative Flow 4: System detects mandatory fields are not filled and shows error
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_AF4_MandatoryFieldsMissing() {
            //Arrange
            InitialStepsForPurchaseItems();
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName1);

            //Act - Navigate to create purchase but leave mandatory fields empty
            _driver.FindElement(By.Id("purchaseItemsButton")).Click();
            createPurchase_PO.FillPurchaseForm("", "", "", ""); // Empty mandatory fields
            createPurchase_PO.SetItemQuantity(itemId1, 1);
            createPurchase_PO.ClickContinue();
            createPurchase_PO.ConfirmPurchaseDialog();

            //Assert - Should show error about mandatory fields and stay on create page
            Assert.True(createPurchase_PO.CheckMessageError("mandatory"));
            Assert.True(createPurchase_PO.IsOnCreatePurchasePage());
        }

        // Alternative Flow 5: System detects quantity exceeds available and shows error
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_AF5_QuantityExceedsAvailable() {
            //Arrange
            InitialStepsForPurchaseItems();
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName1); // iPhone 15 has 5 available

            //Act - Navigate to create purchase and set quantity greater than available
            _driver.FindElement(By.Id("purchaseItemsButton")).Click();
            createPurchase_PO.FillPurchaseForm("Test Street", "Test City", "Test Country", "");
            createPurchase_PO.SelectPaymentMethod("1");
            createPurchase_PO.SetItemQuantity(itemId1, 100); // Request 100 but only 5 available
            createPurchase_PO.ClickContinue();
            createPurchase_PO.ConfirmPurchaseDialog();

            //Assert - Should show error about quantity exceeding available stock
            Assert.True(createPurchase_PO.CheckMessageError("exceeds"));
            Assert.True(createPurchase_PO.IsOnCreatePurchasePage());
        }
    }
}