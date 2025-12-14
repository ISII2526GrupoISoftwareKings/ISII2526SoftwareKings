using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UCPurchase_UIT : UC_UIT
    {
        private SelectItemsForPurchase_PO selectItemsForPurchase_PO;
        private CreatePurchase_PO createPurchase_PO;
        private DetailPurchase_PO detailPurchase_PO;

        // Item 1: iPhone 15
        private const int itemIdIphone = 1;
        private const string itemNameIphone = "iPhone 15";
        private const string itemBrandIphone = "Apple";
        private const string itemDescIphone = "Smartphone 6.1\"";
        private const string itemPriceIphone = "999.00";
        private const string itemQuantityIphone = "5";

        // Item 2: XPS 13
        private const int itemIdXps = 2;
        private const string itemNameXps = "XPS 13";
        private const string itemBrandXps = "Dell";
        private const string itemDescXps = "Ultrabook 13\"";
        private const string itemPriceXps = "1399.00";
        private const string itemQuantityXps = "10";

        public UCPurchase_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForPurchase_PO = new SelectItemsForPurchase_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
            detailPurchase_PO = new DetailPurchase_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("hugo@uclm.es", "Pass123$");
        }

        private void InitialStepsForPurchase()
        {
            Precondition_perform_login();

            selectItemsForPurchase_PO.WaitForBeingVisible(By.CssSelector("a[href='/purchase/selectitemsforpurchase']"));
            Thread.Sleep(500);
            selectItemsForPurchase_PO.WaitForBeingClickable(By.CssSelector("a[href='/purchase/selectitemsforpurchase']"));
            _driver.FindElement(By.CssSelector("a[href='/purchase/selectitemsforpurchase']")).Click();
        }

        private void InitialStepsForCreatePurchase(string itemName)
        {
            InitialStepsForPurchase();

            selectItemsForPurchase_PO.SearchItems(itemName, "");
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemName);

            selectItemsForPurchase_PO.ClickPurchase();
        }

        // UC3-Esc2-1: No items available with (AF1)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc2_1_NoItemsAvailable()
        {
            //Arrange
            InitialStepsForPurchase();

            //Act
            selectItemsForPurchase_PO.SearchItems("NonExistentItem", "");

            //Assert
            Assert.True(selectItemsForPurchase_PO.CheckNoItemsMessage());
        }

        // UC3-Esc3-1: Filter by Name (AF2)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc3_1_FilterByName()
        {
            //Arrange
            InitialStepsForPurchase();
            var expectedItems = new List<string[]> {
                new string[] { itemNameIphone, itemBrandIphone, itemDescIphone, itemPriceIphone, itemQuantityIphone }
            };

            //Act
            selectItemsForPurchase_PO.SearchItems("iPhone", "");

            //Assert
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        // UC3-Esc3-2: Filter by Brand (AF2)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc3_2_FilterByBrand()
        {
            //Arrange
            InitialStepsForPurchase();
            var expectedItems = new List<string[]> {
                new string[] { itemNameXps, itemBrandXps, itemDescXps, itemPriceXps, itemQuantityXps }
            };

            //Act
            selectItemsForPurchase_PO.SearchItems("", "Dell");

            //Assert
            Assert.True(selectItemsForPurchase_PO.CheckListOfItems(expectedItems));
        }

        // UC3-Esc4-1: Modify purchase cart (AF3)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc4_1_ModifyPurchaseCart()
        {
            //Arrange
            InitialStepsForPurchase();

            //Act
            selectItemsForPurchase_PO.SearchItems("", "");
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemNameIphone);
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemNameXps);

            selectItemsForPurchase_PO.RemoveItemFromPurchaseCart(itemNameIphone);

            //Assert
            Assert.False(selectItemsForPurchase_PO.PurchaseNotAvailable());
        }

        // UC3-Esc4-2: Purchase not available (empty cart)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc4_2_PurchaseNotAvailable()
        {
            //Arrange
            InitialStepsForPurchase();

            //Act
            selectItemsForPurchase_PO.SearchItems(itemNameIphone, "");
            selectItemsForPurchase_PO.AddItemToPurchaseCart(itemNameIphone);
            selectItemsForPurchase_PO.RemoveItemFromPurchaseCart(itemNameIphone);

            //Assert
            Assert.True(selectItemsForPurchase_PO.PurchaseNotAvailable());
        }

        // UC3-Esc5-1: Modify Items (return to selection)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc5_1_ModifyItems()
        {
            //Arrange
            InitialStepsForCreatePurchase(itemNameIphone);

            //Act
            createPurchase_PO.FillPurchaseForm("Calle Test", "Madrid", "Spain", "Test purchase");
            createPurchase_PO.ClickModify();
            Thread.Sleep(1000);

            //Assert
            Assert.EndsWith("/purchase/selectitemsforpurchase", _driver.Url);
        }

        // UC3-Esc6-1: Mandatory Fields Error (AF4)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc6_1_ErrorMandatoryFields()
        {
            //Arrange
            InitialStepsForCreatePurchase(itemNameIphone);

            //Act
            createPurchase_PO.FillPurchaseForm("", "", "", "");
            createPurchase_PO.ClickSubmit();
            createPurchase_PO.ConfirmDialog();

            //Assert
            Assert.True(createPurchase_PO.CheckMessageError("Street, City and Country are mandatory"));
        }

        // UC3-Esc6-2: Error Quantity Exceeds Available (AF5)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc6_2_ErrorQuantityExceedsAvailable()
        {
            //Arrange
            InitialStepsForCreatePurchase(itemNameIphone);

            //Act
            createPurchase_PO.FillPurchaseForm("Calle Test", "Madrid", "Spain", "Test purchase");
            createPurchase_PO.SelectPaymentMethod("1");
            createPurchase_PO.SetItemQuantity(itemIdIphone, "100"); // iPhone only has 5 available
            createPurchase_PO.ClickSubmit();
            createPurchase_PO.ConfirmDialog();

            //Assert
            Assert.True(createPurchase_PO.CheckMessageError("exceeds"));
        }

        // UC3-Esc1-1: Complete Successful Flow (Main Flow)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC3_Esc1_1_CreatePurchaseSuccess()
        {
            //Arrange
            InitialStepsForCreatePurchase(itemNameXps);

            string street = "Calle Falsa 123";
            string city = "Madrid";
            string country = "Spain";
            string desc = "Test purchase for XPS";
            string quantity = "2";
            string expectedTotal = "2798.00 €";
            string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

            //Act
            createPurchase_PO.FillPurchaseForm(street, city, country, desc);
            createPurchase_PO.SelectPaymentMethod("1");
            createPurchase_PO.SetItemQuantity(itemIdXps, quantity);

            createPurchase_PO.ClickSubmit();
            createPurchase_PO.ConfirmDialog();

            Thread.Sleep(3000);

            //Assert
            Assert.True(detailPurchase_PO.CheckPurchaseDetails(
                street,
                expectedTotal,
                desc,
                "CreditCard",
                todayDate
            ));

            Assert.True(detailPurchase_PO.CheckPurchasedItem(
                itemNameXps,
                itemBrandXps,
                quantity,
                itemPriceXps + " €"
            ));
        }
    }
}
