using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyHealthApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHealthApp.Controllers
{
    [Route("api/[controller]")]
    public class CarApiController : Controller
    {
        private readonly ProfileContext _context;

        public CarApiController(ProfileContext context)
        {
            _context = context;
        }    

        // *** Get all *** //
        // Http: GET
        [HttpGet]
        public IEnumerable<Car> GetAll()
        {
            return _context.Cars.ToList(); // Send all cars in DB
        }

        // *** Get by ID *** //
        // Http: GET
        [HttpGet("{id}", Name = "GetCar")]
        public IActionResult GetById(int id)
        {
            var car = _context.Cars.FirstOrDefault(t => t.CarId == id); // Find car
            if (car == null)
            {
                return NotFound();
            }
            return new ObjectResult(car);
        }

        // *** Create *** //
        // Http: POST
        [HttpPost]
        public IActionResult Create([FromBody] Car car)
        {
            if (car == null)
            {
                return BadRequest();
            }

            _context.Cars.Add(car);
            _context.SaveChanges();

            return CreatedAtRoute("GetCar", new { id = car.CarId }, car); // Go to GetCar which is GetById and send the new added car id 
        }

        // *** Update *** //
        // Http: PUT
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Car car)
        {
            if (car == null || car.CarId != id)
            {
                return BadRequest();
            }

            var updateCar = _context.Cars.FirstOrDefault(t => t.CarId == id); // Find car to update
            if (car == null)
            {
                return NotFound();
            }

            updateCar.Make = car.Make; // Override with new data
            updateCar.Model = car.Model;

            _context.Cars.Update(updateCar); // The update DB
            _context.SaveChanges();
            return new NoContentResult();
        }

        // *** Delete *** //
        // Http: DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var car = _context.Cars.FirstOrDefault(t => t.CarId == id); // Find car to delete
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car); // Remove from DB
            _context.SaveChanges();
            return new NoContentResult();
        }
    }

}
