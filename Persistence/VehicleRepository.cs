using System;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Core;
using Vega.Core.Models;
using Vega.Extensions;

namespace Vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;
        public VehicleRepository(VegaDbContext context)
        {
            _context = context;
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public async Task<Vehicle> GetVehicle(int id, bool IncludeRelated = true)
        {

            if (!IncludeRelated)
                await _context.Vehicles.Include(v => v.Features)
                        .SingleOrDefaultAsync(v => v.Id == id);

            return await _context.Vehicles
                        .Include(v => v.Features)
                            .ThenInclude(vr => vr.Feature)
                        .Include(v => v.Model)
                            .ThenInclude(m => m.Make)
                        .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {

            var result = new QueryResult<Vehicle>();

            var query = _context.Vehicles
                        .Include(v => v.Model)
                            .ThenInclude(m => m.Make)
                            .Include(v => v.Features)
                            .ThenInclude(vf => vf.Feature)
                            .AsQueryable();
            // Filtering
            if (queryObj.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);

            if (queryObj.ModelId.HasValue)
                query = query.Where(v => v.ModelId == queryObj.ModelId.Value);

            // Sorting
            var columnMap = new Dictionary<string, Expression<Func<Vehicle, object>>>
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Make.Name,
                ["contactName"] = v => v.ContactName,           
            };

            query = query.ApplyOrdering(queryObj, columnMap);

            result.TotalItems = await query.CountAsync();
            
            query = query.ApplyPaging(queryObj);
            
             result.Items = await query.ToListAsync();

             return result;
        }



        public void Remove(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

    }
}