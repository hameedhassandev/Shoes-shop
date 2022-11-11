using Shoes_shop.Models;
using System.ComponentModel.DataAnnotations;

namespace Shoes_shop.ViewModels
{
    public class OrderAndDetailsVM
    {
        public int Id { get; set; }
        public DateTime dateTime { get; set; }
        public double TotalPrice { get; set; }
        public string FullNam { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;

        public bool IsConfirmed { get; set; }
        public bool IsShippedAndPay { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Shoes>? shoes { get; set; }


    }
}
