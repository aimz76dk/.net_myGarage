using System;
using System.Collections.Generic;
using System.Linq;
using FamilyHealthApp.Models;

namespace FamilyHealthApp.Repository
{
    // Easy implementation of usefull method for getting car list
    public class ProfileRepository
    {
        private readonly ProfileContext _context;

        public ProfileRepository(ProfileContext context)
        {
            _context = context;
        }

        public List<Car> GetCarList(string id)
        {
            return _context.Cars.Where(x => x.AspNetUserId == id).ToList();
        }
    }
}
