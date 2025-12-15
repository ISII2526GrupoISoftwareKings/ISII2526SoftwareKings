using AppForSEII2526.UIT.Shared;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Restock
{
    public class UCRestock_UIT : UC_UIT
    {
        private SelectItemsForRestock_PO selectItemsForRestock_PO;
        private CreateRestock_PO createRestock_PO;
        private DetailRestock_PO detailRestock_PO;

        private const int itemIdIphone = 1;
        private const string itemNameIphone = "iPhone 15";
        private const string itemBrandIphone = "Apple";
        private const string itemStockIphone = "10";
        private const string itemPriceIphone = "870,00";

        private const int itemIdSamsung = 3;
        private const string itemNameSamsung = "Laptop Samsung";
        private const string itemStockSamsung = "20";

        public UCRestock_UIT(ITestOutputHelper output) : base(output)
        {
            selectItemsForRestock_PO = new SelectItemsForRestock_PO(_driver, _output);
            createRestock_PO = new CreateRestock_PO(_driver, _output);
            detailRestock_PO = new DetailRestock_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("alberto@uclm.es", "Pass123$");
        }

        private void InitialStepsForRestock()
        {
            Precondition_perform_login();

            selectItemsForRestock_PO.WaitForBeingVisible(By.CssSelector("a[href='/restock/selectitemsforrestock']"));
            _driver.FindElement(By.CssSelector("a[href='/restock/selectitemsforrestock']")).Click();
        }

        private void InitialStepsForCreateRestock(string itemName, int itemId)
        {
            InitialStepsForRestock();

            selectItemsForRestock_PO.SearchItems(itemName, "");
            selectItemsForRestock_PO.AddItemToRestockCart(itemId);

            selectItemsForRestock_PO.ClickCreateRestock();
        }

        // UC4-Esc2-1: No hay items disponibles con filtros imposibles
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc2_1_NoItemsAvailable()
        {
            //Arrange
            InitialStepsForRestock();

            //Act
            selectItemsForRestock_PO.SearchItems("NonExistentItem", "");

            //Assert
            Assert.True(selectItemsForRestock_PO.CheckMessageError("There are no items available for restock with these filters."));
        }

        // UC4-Esc3-1: Filtrar por Nombre (iPhone)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc3_1_FilterByName()
        {
            //Arrange
            InitialStepsForRestock();
            var expectedItems = new List<string[]> {
                new string[] { itemNameIphone, itemBrandIphone, itemStockIphone, itemPriceIphone }
            };

            //Act
            selectItemsForRestock_PO.SearchItems("iPhone", "");

            //Assert
            Assert.True(selectItemsForRestock_PO.CheckListOfItems(expectedItems));
        }

        // UC4-Esc3-2: Filtrar por Cantidad (Escenario Nuevo)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc3_2_FilterByQuantity()
        {
            //Arrange
            InitialStepsForRestock();
            var expectedItems = new List<string[]> {
                new string[] { itemNameIphone, itemBrandIphone, itemStockIphone, itemPriceIphone }
            };

            //Act
            selectItemsForRestock_PO.SearchItems("", "6");

            //Assert
            Assert.True(selectItemsForRestock_PO.CheckListOfItems(expectedItems));
            Assert.False(selectItemsForRestock_PO.CheckListOfItems(new List<string[]> {
                new string[] { itemNameSamsung }
            }));
        }

        // UC4-Esc4-1: Modificar carrito de restock
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc4_1_ModifyRestockCart()
        {
            //Arrange
            InitialStepsForRestock();

            //Act
            selectItemsForRestock_PO.SearchItems("", "");
            selectItemsForRestock_PO.AddItemToRestockCart(itemIdIphone);
            selectItemsForRestock_PO.AddItemToRestockCart(itemIdSamsung);

            selectItemsForRestock_PO.RemoveItemFromRestockCart(itemIdIphone);

            //Assert
            Assert.False(selectItemsForRestock_PO.CreateRestockNotAvailable());
        }

        // UC4-Esc4-2: Restock no disponible (carrito vacío)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc4_2_RestockNotAvailable()
        {
            //Arrange
            InitialStepsForRestock();

            //Act
            selectItemsForRestock_PO.SearchItems(itemNameIphone, "");
            selectItemsForRestock_PO.AddItemToRestockCart(itemIdIphone);
            selectItemsForRestock_PO.RemoveItemFromRestockCart(itemIdIphone);

            //Assert
            Assert.True(selectItemsForRestock_PO.CreateRestockNotAvailable());
        }

        // UC4-Esc6-1: Modify Items
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc5_1_ModifyItems()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);

            //Act
            createRestock_PO.FillRestockForm("My Restock", "Calle Test", "Restock for Gym", DateTime.Today.AddDays(1));
            createRestock_PO.ClickModify();

            //Assert
            Assert.EndsWith("/restock/selectitemsforrestock", _driver.Url);
        }

        // UC4-Esc7-1: Error Título Obligatorio
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc6_1_ErrorTitleRequired()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);

            //Act
            createRestock_PO.FillRestockForm("", "Calle Test", "Restock for Gym", DateTime.Today.AddDays(1));
            createRestock_PO.ClickSubmit();

            //Assert
            Assert.True(createRestock_PO.CheckValidationSummaryError("The Title field is required."));
        }

        // UC4-Esc7-2: Error Formato Descripción (Debe empezar por "Restock for")
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc6_2_ErrorDescriptionFormat()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);

            //Act
            createRestock_PO.FillRestockForm("My Restock", "Calle Test", "Bad Description", DateTime.Today.AddDays(1));
            createRestock_PO.ClickSubmit();
            createRestock_PO.ConfirmDialog();

            //Assert
            Assert.True(createRestock_PO.CheckMessageError("Error!, You must start the Description with Restock for"));
        }

        // UC4-Esc7-2: Error Formato Descripción (Debe empezar por "Restock for")
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc6_3_ErrorDeliveryAddress()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);

            //Act
            createRestock_PO.FillRestockForm("My Restock", "", "Restock for test", DateTime.Today.AddDays(1));
            createRestock_PO.ClickSubmit();

            //Assert
            Assert.True(createRestock_PO.CheckValidationSummaryError("The DeliveryAddress field is required."));
        }

        // UC4-Esc7-5: Error Fecha Pasada (Regla de negocio controlador)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc6_5_ErrorDateInPast()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);
            DateTime pastDate = DateTime.Today.AddDays(-1);

            //Act
            createRestock_PO.FillRestockForm("My Restock", "Calle Test", "Restock for Gym", pastDate);
            createRestock_PO.ClickSubmit();
            createRestock_PO.ConfirmDialog();

            //Assert
            Assert.True(createRestock_PO.CheckValidationSummaryError("Expected date cannot be in the past") ||
                        createRestock_PO.CheckMessageError("Expected date cannot be in the past"));
        }

        // UC4-Esc7-4: Error Cantidad (Debe ser > 0)
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc6_4_ErrorQuantity()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);

            //Act
            createRestock_PO.FillRestockForm("My Restock", "Calle Test", "Restock for Gym", DateTime.Today.AddDays(1));
            createRestock_PO.SetItemQuantity(itemIdIphone, "0");
            createRestock_PO.ClickSubmit();
            createRestock_PO.ConfirmDialog();

            //Assert
            Assert.True(createRestock_PO.CheckValidationSummaryError("Minimum amount bought is 1"));
        }

        // UC4-Esc1-1: Flujo Exitoso Completo
        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc1_1_CreateRestockSuccess()
        {
            //Arrange
            InitialStepsForCreateRestock(itemNameIphone, itemIdIphone);

            string title = "Full Restock";
            string address = "Calle Falsa 123";
            string desc = "Restock for Apple";
            string quantity = "10";
            string expectedTotal = "8700,00 €";
            string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

            //Act
            createRestock_PO.FillRestockForm(title, address, desc, DateTime.Today.AddDays(5));
            createRestock_PO.SetItemQuantity(itemIdIphone, quantity);

            createRestock_PO.ClickSubmit();
            createRestock_PO.ConfirmDialog();

            Thread.Sleep(3000);

            //Assert
            Assert.True(detailRestock_PO.CheckRestockDetails(
                "Alberto Bueno Baquero",
                title,
                desc,
                address,
                expectedTotal,
                todayDate
            ));

            Assert.True(detailRestock_PO.CheckRestockedItem(
                itemNameIphone,
                itemBrandIphone,
                itemPriceIphone + " €",
                quantity,
                expectedTotal
            ));
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC4_Esc1_2_BF_AF2_AF3()
        {
            //Arrange
            InitialStepsForRestock();

            string title = "Full Restock";
            string address = "Calle Falsa 123";
            string desc = "Restock for Apple";
            string quantity = "10";
            string expectedTotal = "8700,00 €";
            string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

            //Act
            selectItemsForRestock_PO.AddItemToRestockCart(itemIdSamsung);
            selectItemsForRestock_PO.SearchItems(itemNameIphone, "");
            selectItemsForRestock_PO.AddItemToRestockCart(itemIdIphone);
            selectItemsForRestock_PO.RemoveItemFromRestockCart(itemIdSamsung);

            selectItemsForRestock_PO.ClickCreateRestock();

            createRestock_PO.FillRestockForm(title, address, desc, DateTime.Today.AddDays(5));
            createRestock_PO.SetItemQuantity(itemIdIphone, quantity);

            createRestock_PO.ClickSubmit();
            createRestock_PO.ConfirmDialog();

            Thread.Sleep(3000);

            //Assert
            Assert.True(detailRestock_PO.CheckRestockDetails(
                "Alberto Bueno Baquero",
                title,
                desc,
                address,
                expectedTotal,
                todayDate
            ));

            Assert.True(detailRestock_PO.CheckRestockedItem(
                itemNameIphone,
                itemBrandIphone,
                itemPriceIphone + " €",
                quantity,
                expectedTotal
            ));
        }
    }
}