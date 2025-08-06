using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using StoreApp.Data.Entities;
using StoreApp.Data.Concrete;



namespace StoreApp.Web.Areas.Admin.Models
{
    public class ProductApprovalFilterViewModel
    {
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public List<SelectListItem> Categories { get; set; }
    public List<Product> Products { get; set; }
    }
}