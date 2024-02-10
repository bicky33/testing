using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthwindWebMvc.Basic.Models;
using NorthwindWebMvc.Basic.Models.Dto;
using NorthwindWebMvc.Basic.Repository;
using NorthwindWebMvc.Basic.RepositoryContext;
using NorthwindWebMvc.Basic.Service;
using System.Net.Http.Headers;

namespace NorthwindWebMvc.Basic.Controllers
{
    public class ProductsController : Controller
    {
        private readonly RepositoryDbContext _context;

        private readonly IProductService<ProductDto> _productService;

        //replace RepositoryDbContext with IRepositoryBase
        private readonly IRepositoryBase<Product> _repositoryBase;
        private readonly ICategoryService<CategoryDto> _categoryService;



        public ProductsController(IProductService<ProductDto> productService, ICategoryService<CategoryDto> categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }


        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _productService.FindAll(true));

        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _productService.FindAll(true).Result.FirstOrDefault(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public async  Task<IActionResult> Create()
        {
            var categories = await _categoryService.FindAll(false);
            List<SelectListItem> items = new();
            foreach (var item in categories)
            {
                var list = new SelectListItem()
                {
                    Value = item.Id.ToString(), // Assuming Id is of type int
                    Text = item.CategoryName
                };
                items.Add(list);
            }

            ViewBag.Categories = items;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductDtoCreate ProductDtoCreate)
        {
            if (ModelState.IsValid)
            {
      
                try
                {
                    var file = ProductDtoCreate.Photo;
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        //collect data from dto dan filename
                        var productDto = new ProductDto
                        {
                            Price = ProductDtoCreate.Price,
                            ProductName = ProductDtoCreate.ProductName,
                            Photo = fileName,
                            Stock = ProductDtoCreate.Stock,
                            CategoryId = ProductDtoCreate.CategoryId
                        };
                        _productService.Create(productDto);
                        
                        return RedirectToAction(nameof(Index));

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ProductDtoCreate);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.FindById((int)id, true);
            var ProductDtoCreate = new ProductDtoCreate
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Stock = product.Stock,
                Price = product.Price,
                CategoryId = product.CategoryId
            };

            var categories = await _categoryService.FindAll(false);
            List<SelectListItem> items = new();
            foreach (var item in categories)
            {
                var list = new SelectListItem()
                {
                    Value = item.Id.ToString(), // Assuming Id is of type int
                    Text = item.CategoryName
                };
                items.Add(list);
            }
            ViewBag.Categories = items;

            if (product == null)
            {
                return NotFound();
            }
            return View(ProductDtoCreate);

        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] ProductDtoCreate ProductDtoCreate)
        {
            if (id != ProductDtoCreate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var file = ProductDtoCreate.Photo;
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        //collect data from dto dan filename
                        var categoryDto = new ProductDto
                        {
                            Id=ProductDtoCreate.Id,
                             Price= ProductDtoCreate.Price,
                            Stock = ProductDtoCreate.Stock,
                            Photo = fileName,
                            ProductName = ProductDtoCreate.ProductName,
                            CategoryId = ProductDtoCreate.CategoryId
                        };
                        _productService.Update(categoryDto);

                        return RedirectToAction(nameof(Index));

                    }
                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(ProductDtoCreate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ProductDtoCreate);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _productService.FindById((int)id,true);

            var product = _productService.FindAll(true).Result.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return Problem("Entity set 'RepositoryDbContext.Categories'  is null.");
            }
            //var category = await _productService.FindById((int)id,true);
            var product = _productService.FindAll(true).Result.FirstOrDefault(m => m.Id == id);
            if (product != null)
            {
                _productService.Delete(product);
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_productService.FindAll(true)?.Result.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
