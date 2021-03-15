using System;
namespace Models
{
    public class ItemDetail
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public string ImageUrl { get; set; }
        public bool isDiscounted { get; set; }

    }
}
