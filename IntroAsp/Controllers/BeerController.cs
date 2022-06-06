using IntroAsp.Models;
using IntroAsp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IntroAsp.Controllers
{
    public class BeerController : Controller        
    {

        private readonly PubContext _context;

        public BeerController(PubContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var beers = _context.Beers.Include(b => b.Brand);
            return View(await beers.ToListAsync()); //30 min
        }
        public IActionResult Create()
        {
            //recibe 4 param 1 fuente de informacion(-context), 2 el id con el que se "maneja", 3 lo que se muestra es el nombre
            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId","Name"); 

            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //valida que la info provenga del form que esta en el mismo dominio de la pag
        public  async Task<IActionResult> Create(BeerViewModel model)
        {
            if (ModelState.IsValid) //el obj mod. tiene un atr isValid nos dice si el modelo pasó o no segun las validaciones que pongamos en beerviewModel
            {
                var beer = new Beer()
                {
                    Name = model.Name,
                    BrandId = model.BrandId,
                };
                _context.Add(beer); // aca estamos indicandolo a entity pero no se guarda en la bbdd todavia

                await _context.SaveChangesAsync(); // aca se realiza el guardado en la bbdd
                return RedirectToAction(nameof(Index)); //vuelve a la vista despues de guardar
            }

            ViewData["Brands"] = new SelectList(_context.Brands, "BrandId", "Name", model.BrandId);   // agrega ese otro param para volver amostrar los campos que estaban escritos

            return View(model);
        }
    }
}
