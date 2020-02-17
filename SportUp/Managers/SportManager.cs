using Microsoft.EntityFrameworkCore;
using SportUp.Data;
using SportUp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportUp.Managers
{
    public class SportManager
    {
        private readonly ApplicationDbContext _context;

        public SportManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sport>> GetSportsAsync() => 
            await _context.Sports.ToListAsync();

        public async Task<List<Sport>> GetSportsAsync(List<int> ids)
        {
            var sports = new List<Sport>();
            foreach (var id in ids)
            {
                sports.Add(await _context.Sports.SingleOrDefaultAsync(s => s.Id == id));
            }
            return sports;
        }

        public async Task<Sport> GetSportAsync(int id) =>
            await _context.Sports.SingleOrDefaultAsync(s => s.Id == id);


    }
}
