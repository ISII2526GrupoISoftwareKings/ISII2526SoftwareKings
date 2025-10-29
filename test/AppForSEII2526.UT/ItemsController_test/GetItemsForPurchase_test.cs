using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Data;          
using AppForSEII2526.API.Models;    
using AppForSEII2526.API.DTOs.ItemDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.UT.ItemsController_test
{
    public class GetItemsForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetItemsForPurchase_test()
        {
            var brands = new List<Brand>()
            {
                new Brand() { Id = 1, Name = "Elyte" },
                new Brand() { Id = 2, Name = "Domyos" }
            };
            var items = new List<Item>()
            {
                new Item() { Id = 1, Name = "Protein Bar", Brand = brands[0], Description = "Delicious protein bar", PurchasePrice = 2.50M, QuantityAvailableForPurchase = 10 },
                new Item() { Id = 2, Name = "Yoga Mat", Brand = brands[1], Description = "Comfortable yoga mat", PurchasePrice = 20.00M, QuantityAvailableForPurchase = 0 },
                new Item() { Id = 3, Name = "Dumbbells Set", Brand = brands[0], Description = "Set of dumbbells for strength training", PurchasePrice = 50.00M, QuantityAvailableForPurchase = 5 }
            };
            _context.AddRange(brands);
            _context.AddRange(items);
            _context.SaveChanges();


        }
    }
}
