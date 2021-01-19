using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatisFact.Data;
using SatisFact.Models;
using SatisFact.Utilities;


namespace SatisFact.Controllers {
	public class IngredientsController : Controller {
		private readonly SatisFactContext _context;
		private readonly string[] _allowedContentTypes = { "image/png", "image/jpeg", "image/jpg" };
		private readonly string _serverPath = "/imgs/Ingredients/";


		public IngredientsController(SatisFactContext context) {
			_context = context;
		}


		// GET: Components
		public async Task<IActionResult> Index() {
			return View(await _context.Ingredient.ToListAsync());
		}


		// GET: Components/Details/5
		public async Task<IActionResult> Details(int? id) {
			if (id == null) {
				return NotFound();
			}
			var component = await _context.Ingredient.FirstOrDefaultAsync(m => m.Id == id);
			if (component == null) {
				return NotFound();
			}
			return View(component);
		}


		// GET: Components/Create
		public IActionResult Create() {
			return View();
		}


		// POST: Components/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Image")] IFormFile image, [Bind("Id,Title,ImageName")] Ingredient component) {
			if (ModelState.IsValid) {
				FileUpload fu = new FileUpload(image, _serverPath, _allowedContentTypes);
				if (fu.Upload()) {
					component.ImageName = fu.ValidFilename;
					_context.Add(component);
					await _context.SaveChangesAsync();
				}
			}
			return View(component);
		}


		// GET: Components/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null) {
				return NotFound();
			}
			var component = await _context.Ingredient.FindAsync(id);
			if (component == null) {
				return NotFound();
			}
			return View(component);
		}


		// POST: Components/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Image")] IFormFile image, [Bind("Id,Title,ImageName")] Ingredient ingredient) {
			if (id != ingredient.Id) {
				return NotFound();
			}
			if (ModelState.IsValid) {
				FileUpload fu = new FileUpload(image, _serverPath, _allowedContentTypes);
				if (fu.Upload()) {
					System.IO.File.Delete(_serverPath + ingredient.ImageName);
					ingredient.ImageName = fu.ValidFilename;
					try {
						_context.Update(ingredient);
						await _context.SaveChangesAsync();
					} catch (DbUpdateConcurrencyException) {
						if (!IngredientExists(ingredient.Id)) {
							return NotFound();
						} else {
							throw;
						}
					}
				}
			}
			return View(ingredient);
		}


		// GET: Components/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null) {
				return NotFound();
			}
			var component = await _context.Ingredient.FirstOrDefaultAsync(m => m.Id == id);
			if (component == null) {
				return NotFound();
			}
			return View(component);
		}


		// POST: Components/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			var ingredient = await _context.Ingredient.FindAsync(id);
			try {
				System.IO.File.Delete(_serverPath + ingredient.ImageName);
			} catch (System.Exception) {
				// log file for component image doesnt exist
			}
			_context.Ingredient.Remove(ingredient);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		private bool IngredientExists(int id) {
			return _context.Ingredient.Any(e => e.Id == id);
		}
	}
}
