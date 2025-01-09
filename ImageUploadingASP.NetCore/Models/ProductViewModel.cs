namespace ImageUploadingASP.NetCore.Models
{
    public class ProductViewModel
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public int Price { get; set; }
        
        public IFormFile Photo { get; set; } = null!;
    }
}
