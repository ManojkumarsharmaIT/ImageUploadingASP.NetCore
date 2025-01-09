using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageUploadingASP.NetCore.Models
{
    public partial class Product
    {   [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public int Price { get; set; }
        [Required]
        public string ImagePath { get; set; } = null!;
    }
}
