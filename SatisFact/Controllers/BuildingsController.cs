using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatisFact.Data;
using SatisFact.Models;
using SatisFact.Utilities;


namespace SatisFact.Controllers {
	public class BuildingsController : Controller {
		private readonly SatisFactContext _context;
		private readonly string[] _allowedContentTypes = { "image/png", "image/jpeg", "image/jpg" };
		private readonly string _serverPath = "/imgs/Buildings/";


		public BuildingsController(SatisFactContext context) {
			_context = context;
		}


		// GET: Buildings
		public async Task<IActionResult> Index() {
			return View(await _context.Building.ToListAsync());
		}


		// GET: Buildings/Details/5
		public async Task<IActionResult> Details(int? id) {
			if (id == null) {
				return NotFound();
			}
			var building = await _context.Building.FirstOrDefaultAsync(m => m.Id == id);
			if (building == null) {
				return NotFound();
			}
			return View(building);
		}


		// GET: Buildings/Create
		public IActionResult Create() {
			return View();
		}


		// POST: Buildings/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Image")] IFormFile image, [Bind("Id,Title,ImageName")] Building building) {
			if (ModelState.IsValid) {
				FileUpload fu = new FileUpload(image, _serverPath, _allowedContentTypes);
				if (fu.Upload()) {
					building.ImageName = fu.ValidFilename;
					_context.Add(building);
					await _context.SaveChangesAsync();
				}
			}
			return View(building);
		}


		// GET: Buildings/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null) {
				return NotFound();
			}
			var building = await _context.Building.FindAsync(id);
			if (building == null) {
				return NotFound();
			}
			return View(building);
		}


		// POST: Buildings/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Image")] IFormFile image, [Bind("Id,Title,ImageName")] Building building) {
			if (id != building.Id) {
				return NotFound();
			}
			if (ModelState.IsValid) {
				FileUpload fu = new FileUpload(image, _serverPath, _allowedContentTypes);
				if (fu.Upload()) {
					System.IO.File.Delete(_serverPath + building.ImageName);
					building.ImageName = fu.ValidFilename;
					try {
						_context.Update(building);
						await _context.SaveChangesAsync();
					} catch (DbUpdateConcurrencyException) {
						if (!BuildingExists(building.Id)) {
							return NotFound();
						} else {
							throw;
						}
					}
				}
			}
			return View(building);
		}


		// GET: Buildings/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null) {
				return NotFound();
			}
			var building = await _context.Building.FirstOrDefaultAsync(m => m.Id == id);
			if (building == null) {
				return NotFound();
			}
			return View(building);
		}


		// POST: Buildings/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			var building = await _context.Building.FindAsync(id);
			try { 
				System.IO.File.Delete(_serverPath + building.ImageName);
			} catch(System.Exception) {
				// log file for building image doesnt exist
			}
			_context.Building.Remove(building);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


		private bool BuildingExists(int id) {
			return _context.Building.Any(e => e.Id == id);
		}
	}
}


