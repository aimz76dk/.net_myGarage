using System;
using System.Linq;
using System.Threading.Tasks;
using FamilyHealthApp.Models;
using FamilyHealthApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyHealthApp.Controllers
{   
    [Authorize]
    public class LoggedInController : Controller
    {
        // Identity manager
        private readonly UserManager<ApplicationUser> _userManager;
        // Db acess
        private readonly ProfileContext _context;
        // Profile repository (for getting car list)
        public ProfileRepository _profileRepository;

        public LoggedInController(UserManager<ApplicationUser> userManager, ProfileContext context, ProfileRepository profileRepository)
        {
            _userManager = userManager;
            _context = context;
            _profileRepository = profileRepository;

        }

        // *** Index *** //

        public IActionResult Index()
        {
            // Get the user from identity using HttpContext (?)
            var userId = _userManager.GetUserId(HttpContext.User);

            // Get lists of entites of the user by using repository pattern
            var cars = _profileRepository.GetCarList(userId);

            // Send the model to the view
            return View(cars);
        }


        // *** About & Contact us *** //

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        // *** Create *** // 

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Make,Model")] Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var carToAdd = new Car
                    {
                        Make = car.Make,
                        Model = car.Model,
                        AspNetUserId = _userManager.GetUserId(HttpContext.User)
                    };

                    await _context.AddAsync(carToAdd);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(car);
        }


        // *** Edit *** // 

        // GET: LoggedIn/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.SingleOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: LoggedIn/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var carToUpdate = await _context.Cars.SingleOrDefaultAsync(c => c.CarId == id);
            if (await TryUpdateModelAsync<Car>(
                carToUpdate,
                "",
                p => p.Make, p => p.Model))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(carToUpdate);
        }
       

        // *** Details *** // 

        // GET: LoggedIn/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Car = await _context.Cars.SingleOrDefaultAsync(m => m.CarId == id);
            ViewBag.Comments =  _context.Comments.AsEnumerable();
         
            if (ViewBag.Car == null)
            {
                return NotFound();
            }

            return View();
        }


        // *** Delete *** // 

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = await _context.Cars.SingleOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.SingleOrDefaultAsync(m => m.CarId == id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}