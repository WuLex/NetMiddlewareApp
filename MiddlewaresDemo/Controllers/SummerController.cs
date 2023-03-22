using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NetCoreDemo.Controllers
{
    public class SummerController : Controller
    {
        private readonly DbContext _dbContext;

        public SummerController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> IndexTwo()
        //{
        //    var products = await _dbContext.Products.ToListAsync();
        //    return View(products);
        //}

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var product = await _dbContext.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _dbContext.Update(product);
        //        await _dbContext.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(product);
        //}

    }
}