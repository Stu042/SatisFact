using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SatisFact.Data;


namespace SatisFact.Models
{
    public class SeedBuildings
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
			using var context = new SatisFactContext(serviceProvider.GetRequiredService<DbContextOptions<SatisFactContext>>());
			// Look for any movies.
			if (context.Building.Any())
			{
				return;   // DB has been seeded
			}
			context.Building.AddRange(
				new Models.Building
				{
					Title = "Assembler",
					ImageName = "Assembler.png"
				},
				new Models.Building
				{
					Title = "Foundry",
					ImageName = "Foundry.png"
				},
				new Models.Building
				{
					Title = "Manufacturer",
					ImageName = "Manufacturer.png"
				},
				new Models.Building
				{
					Title = "Miner Mk.1",
					ImageName = "Miner Mk.1.png"
				},
				new Models.Building
				{
					Title = "Miner Mk.2",
					ImageName = "Miner Mk.2.png"
				},
				new Models.Building
				{
					Title = "Miner Mk.3",
					ImageName = "Miner Mk.3.png"
				},
				new Models.Building
				{
					Title = "Nuclear Power Plant",
					ImageName = "Nuclear Power Plant.png"
				},
				new Models.Building
				{
					Title = "Oil Extractor",
					ImageName = "Oil Extractor.png"
				},
				new Models.Building
				{
					Title = "Packager",
					ImageName = "Packager.png"
				},
				new Models.Building
				{
					Title = "Refinery",
					ImageName = "Refinery.png"
				},
				new Models.Building
				{
					Title = "Smelter",
					ImageName = "Smelter.png"
				}
			);
			context.SaveChanges();
		}


    }
}