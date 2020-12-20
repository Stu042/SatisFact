using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatisFact.Data;
using SatisFact.Models;
using SatisFact.Utilities;


namespace SatisFact.Controllers
{
	public class BuildingsController : Controller
    {
        private readonly BuildingContext _context;
        private readonly string[] AllowedContentTypes = { "image/png", "image/jpeg", "image/jpg" };
        private readonly string ServerPath = "wwwroot/imgs/Buildings/";


        public BuildingsController(BuildingContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Building.ToListAsync());
        }

        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var building = await _context.Building.FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }
            return View(building);
        }



        // GET: Buildings/Create
        public IActionResult Create()
        {
            return View();
        }



        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Image")] IFormFile image, [Bind("Id,Title,ImageName")] Building building)
        {
            if (ModelState.IsValid)
            {
                FileUpload fu = new FileUpload(image, ServerPath, AllowedContentTypes);
                if (fu.AbleToUpload())
                {
                    fu.Upload();
                    building.ImageName = fu.ValidFilename;
                    _context.Add(building);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }



        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var building = await _context.Building.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            return View(building);
        }



        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Image")] IFormFile image, [Bind("Id,Title,ImageName")] Building building)
        {
            if (id != building.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                FileUpload fu = new FileUpload(image, ServerPath, AllowedContentTypes);
                if (fu.AbleToUpload())
                {
                    if (fu.Upload())
                    {
                        System.IO.File.Delete(ServerPath + building.ImageName);
                        building.ImageName = fu.ValidFilename;
                        try
                        {
                            _context.Update(building);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!BuildingExists(building.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }



        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var building = await _context.Building.FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }
            return View(building);
        }



        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var building = await _context.Building.FindAsync(id);
            System.IO.File.Delete(ServerPath + building.ImageName);
            _context.Building.Remove(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool BuildingExists(int id)
        {
            return _context.Building.Any(e => e.Id == id);
        }
    }
}


