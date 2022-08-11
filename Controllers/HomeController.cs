using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class HomeController : Controller
{
    private ProductsCategoriesContext _context;

    public HomeController(ProductsCategoriesContext context)
    {
        _context = context;
    }

    [HttpGet("/")]
    [HttpGet("/products")]
    public IActionResult AllProducts()
    {
        ViewBag.allProducts = _context.Products.ToList();

        return View("AllProducts");
    }

    [HttpPost("/product/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("AllProducts");
        }
        return AllProducts();
    }

    [HttpGet("/products/{productId}")]
    public IActionResult ShowProduct(int productId)
    {
        ViewBag.categoryWithoutProducts = _context.Categories.Include(c => c.Affiliations).ThenInclude(affil => affil.Product).Where(c => !c.Affiliations.Any(p => p.ProductId == productId));

        Product? prod = _context.Products.Include(p => p.Associations).ThenInclude(assoc => assoc.Category). FirstOrDefault(p => p.ProductId == productId);

        if(prod == null)
        {
            return RedirectToAction("AllProducts");
        }
        return View("ShowProduct", prod);
    }

    [HttpGet("/categories")]
    public IActionResult AllCategories()
    {
        ViewBag.allCategories = _context.Categories.ToList();

        return View("AllCategories");
    }

    [HttpPost("/category/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("AllCategories");
        }
        return AllCategories();
    }

    [HttpGet("/categories/{categoryId}")]
    public IActionResult ShowCategory(int categoryId)
    {
        ViewBag.productWithoutCategories = _context.Products.Include(p => p.Associations).ThenInclude(assoc => assoc.Category).Where(p => !p.Associations.Any(c => c.CategoryId == categoryId));

        Category? cat = _context.Categories.Include(c => c.Affiliations).ThenInclude(affil => affil.Product).FirstOrDefault(p => p.CategoryId == categoryId);

        if(cat == null)
        {
            return RedirectToAction("AllCategories");
        }
        return View("ShowCategory", cat);
    }

    [HttpPost("/products/addcategory")]
    public IActionResult AddCategoryToProduct(Association newAssociation)
    {
        _context.Add(newAssociation);
        _context.SaveChanges();
        return RedirectToAction("ShowProduct", new {productId = newAssociation.ProductId});
    }

    [HttpPost("/categories/addproduct")]
    public IActionResult AddProductToCategory(Association newAssociation)
    {
        _context.Add(newAssociation);
        _context.SaveChanges();
        return RedirectToAction("ShowCategory", new {categoryId = newAssociation.CategoryId});
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}