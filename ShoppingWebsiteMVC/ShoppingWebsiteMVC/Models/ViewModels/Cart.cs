using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsiteMVC.Models.ViewModels
{
    public class Cart 
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}