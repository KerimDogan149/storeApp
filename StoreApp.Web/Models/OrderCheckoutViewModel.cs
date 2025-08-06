using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreApp.Data.Concrete;
using StoreApp.Data.Entities;
using StoreApp.Web.Models;

namespace StoreApp.Web.Models
{
    public class OrderCheckoutViewModel
    {
        public Cart Cart { get; set; }
        public List<Address> Addresses { get; set; }
        public int SelectedAddressId { get; set; }

    }
}