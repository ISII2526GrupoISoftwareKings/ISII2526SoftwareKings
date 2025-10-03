using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataType = System.ComponentModel.DataAnnotations.DataType;


namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string Country { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Purchase Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Street cannot be longer than 100 characters.")]
        public string Street { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public List<PurchaseItem> PurchaseItems { get; set; }
    }
}
