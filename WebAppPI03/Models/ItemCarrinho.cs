using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppPI03.Models
{
    [NotMapped]
    public class ItemCarrinho
    {
        public virtual int productId { get; set; }
        public virtual int productQtd { get; set; }
        public virtual string productName { get; set; }
        public virtual decimal productPrice { get; set; }
        public virtual decimal productTotal { get; set; }
        public virtual string productImage { get; set; }
    }
}