﻿using CityInfo.ASP.DbContexts;
using CityInfo.ASP.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.ASP.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                var cityA = await _context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
                return cityA;
            }
            var cityB = await _context.Cities
                .Where(c => c.Id == cityId)
                .FirstOrDefaultAsync();
            return cityB;
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .ToListAsync();
        }
    }
}
