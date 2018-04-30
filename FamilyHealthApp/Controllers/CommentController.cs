using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyHealthApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FamilyHealthApp.Controllers
{
    public class CommentController : Controller
    {
        // Db acess
        private readonly ProfileContext _context;

        public CommentController(ProfileContext context)
        {
            _context = context;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,CarId")] Comment comment)
        {
            comment.Car = await _context.Cars.SingleOrDefaultAsync(c => c.CarId == comment.CarId); // Add car to the comment (does not work)

            try
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(comment);
                    await _context.SaveChangesAsync();
                            
                    return Redirect("/LoggedIn/Details/" + comment.CarId);
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View("/LoggedIn/Details/" + comment.CarId);
        }

    }
}
