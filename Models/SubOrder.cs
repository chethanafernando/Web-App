using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class SubOrder
    {
        public string[] Data { get; set; }
        public int OrderNo { get; set; }
        public string ProductCode { get; set; }
        public decimal ItemTotal { get; set; }
        public int Qty { get; set; }
        [NotMapped]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public int CustomerID { get; set; }
        [NotMapped]
        public decimal NetTotal { get; set; }
        [NotMapped]
        public decimal SubTotal { get; set; }
        [NotMapped]
        public decimal DiscountPrice { get; set; }
    }
}