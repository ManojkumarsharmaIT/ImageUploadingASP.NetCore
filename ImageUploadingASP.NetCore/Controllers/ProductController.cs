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

                        using (var fileStream = new FileStream(filepath, FileMode.Create))
                        {
                            data.Photo.CopyTo(fileStream);
                        }

                        // Add image in image folder in wwwroot folder 

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

        [HttpGet]
        public IActionResult Details(int id)
        {
            var prod = context.Products.FirstOrDefault(x => x.Id == id);
            return View(prod);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var prod = context.Products.FirstOrDefault(x => x.Id == id);
            if (prod == null)
            {
                return NotFound();
            }

            // Map Product to ProductViewModel
            var viewModel = new ProductViewModel
            {
                Id = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                ImagePath = prod.ImagePath
            };


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors
                var product = context.Products.FirstOrDefault(p => p.Id == model.Id);
                if (product == null)
                {
                    return NotFound();
                }

                // Update product properties
                product.Name = model.Name;
                product.Price = model.Price;

                if (model.Photo != null)
                {
                    // Delete the existing image if it exists
                    string imagePath = product.ImagePath;
                    string folder = Path.Combine(env.WebRootPath, "images");
                    string filepath = Path.Combine(folder, imagePath);
                    if (System.IO.File.Exists(filepath))
                    {

                        System.IO.File.Delete(filepath);
                        // The file is now empty, and you can write new data to it if needed.
                    }
                    else
                    {
                        Console.WriteLine("File not found.");
                    }
                    // Save the new file
                    var ext = Path.GetExtension(model.Photo.FileName);
                    var size = model.Photo.Length;
                    if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                    {
                        if (size < 1000000)
                        {
                            //Get rootfile of wwwroot folder from root
                            string folderr = Path.Combine(env.WebRootPath, "images");

                            String fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                            string filepathh = Path.Combine(folderr, fileName);

                            // Add image in image folder in wwwroot folder 
                            using (var fileStream = new FileStream(filepathh, FileMode.Create))
                            {
                                model.Photo.CopyTo(fileStream);
                            }

                            product.ImagePath = fileName;


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
                    // Update the image path in the product

                }

                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {   
            var prod=context.Products.FirstOrDefault(x=>x.Id == id);
            return View(prod);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteItem(int id)
        {
            var prod = context.Products.FirstOrDefault(x => x.Id == id);
            string imagePath = prod.ImagePath; 
            string folder = Path.Combine(env.WebRootPath, "images");
            string filepath = Path.Combine(folder, imagePath);
            if (System.IO.File.Exists(filepath))
            {
                
                System.IO.File.Delete(filepath);
                // The file is now empty, and you can write new data to it if needed.
            }
            else
            {
                Console.WriteLine("File not found.");
            }
            context.Remove(prod);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
