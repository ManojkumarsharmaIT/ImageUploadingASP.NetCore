using ImageUploadingASP.NetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageUploadingASP.NetCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ImageDbContext context;
        IWebHostEnvironment env;
        public ProductController(ImageDbContext context,IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }
        public IActionResult Index()
        {
            return View(context.Products.ToList());
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductViewModel data)
        {
            string fileName = "";
            if (data.Photo != null)
            {   
                var ext = Path.GetExtension(data.Photo.FileName);
                var size =data.Photo.Length;
                if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                {
                    if (size < 1000000)
                    {
                        //Get rootfile of wwwroot folder from root
                        string folder = Path.Combine(env.WebRootPath, "images");

                        fileName = Guid.NewGuid().ToString() + "_" + data.Photo.FileName;
                        string filepath = Path.Combine(folder, fileName);

                        // Add image in image folder in wwwroot folder 
                        data.Photo.CopyTo(new FileStream(filepath, FileMode.Create));

                        Product p = new Product()
                        {
                            Name = data.Name,
                            Price = data.Price,
                            ImagePath = fileName
                        };
                        context.Products.Add(p);
                        context.SaveChanges();
                        TempData["Success"] = "Product Added...";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Size_Error"] = "Image Size should be less than 1MB";
                    }
                }
                else
                {
                    TempData["Error_img"] = "only PNG, JPG ,JPEG allowed";
                }
            }
            return View();
        }
    }
}
