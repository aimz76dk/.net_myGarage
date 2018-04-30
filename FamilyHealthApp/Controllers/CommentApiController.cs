using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyHealthApp.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FamilyHealthApp.Controllers
{
    [Route("api/[controller]")]
    public class CommentApiController : Controller
    {
        private readonly ProfileContext _context;

        public CommentApiController(ProfileContext context)
        {
            _context = context;
        }    

        // *** Get all *** //
        // Http: GET
        [HttpGet]
        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.ToList(); // Send all comments in DB
        }

        // *** Get by ID *** //
        // Http: GET
        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult GetById(int id)
        {
            var comment = _context.Comments.FirstOrDefault(t => t.CommentId == id); // Find comment by id
            if (comment == null)
            {
                return NotFound();
            }
            return new ObjectResult(comment);
        }


        // *** Create *** //
        // Http: POST
        [HttpPost]
        public IActionResult Create([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            _context.Comments.Add(comment); // Add comment to DB

            // ** Adding comment to car by id — Does not work proberly ** //
            var car = _context.Cars.Single(x => x.CarId == comment.CarId);
            car.Comments.Add(comment);
            _context.Cars.Update(car);

            _context.SaveChanges();

            return CreatedAtRoute("GetComment", new { id = comment.CommentId }, comment); // Go to GetComment which is GetById 
        }
    }
}
