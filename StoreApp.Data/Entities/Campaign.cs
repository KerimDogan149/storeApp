using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Data.Entities
{
    public class Campaign
    {
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? SubTitle { get; set; }

    public string Description { get; set; } = string.Empty;

    public string? Link { get; set; }

    public string Url { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;


    public string? Image { get; set; }

    }
}