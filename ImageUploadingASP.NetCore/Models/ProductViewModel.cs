using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ImageUploadingASP.NetCore.Models
{
    public class ProductViewModel
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public int Price { get; set; }

        public string ImagePath { get; set; } = null!;
        [RequiredForCreate(ErrorMessage = "Please upload an image for new products.")]
        public IFormFile? Photo { get; set; }
    }
}
