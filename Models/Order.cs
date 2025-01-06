using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Order
    {
        public int OrderNo { get; set; }
        public int CustomerID { get; set; }
        public decimal NetTotal { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountPrice { get; set; }
    }
}