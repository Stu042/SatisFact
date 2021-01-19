using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using SatisFact.Data;


namespace SatisFact.Models
{
    public class SeedIngredients {
        public static void Initialize(IServiceProvider serviceProvider)
        {
			using var context = new SatisFactContext(serviceProvider.GetRequiredService<DbContextOptions<SatisFactContext>>());
			// Look for any movies.
			if (context.Ingredient.Any())
			{
				return;   // DB has been seeded
			}
			context.Ingredient.AddRange(
				new Ingredient {
					Title = "Bauxite",
					ImageName = "Bauxite.png"
				},
				new Ingredient {
					Title = "Coal",
					ImageName = "Coal.png"
				},
				new Ingredient {
					Title = "Copper Ore",
					ImageName = "Copper Ore.png"
				},
				new Ingredient {
					Title = "Iron Ore",
					ImageName = "Iron Ore.png"
				},
				new Ingredient {
					Title = "Limestone",
					ImageName = "Limestone.png"
				},
				new Ingredient {
					Title = "Raw Quartz",
					ImageName = "Raw Quartz.png"
				},
				new Ingredient {
					Title = "S.A.M. Ore",
					ImageName = "S.A.M. Ore.png"
				},
				new Ingredient {
					Title = "Sulfur",
					ImageName = "Sulfur.png"
				},
				new Ingredient {
					Title = "Uranium",
					ImageName = "Uranium.png"
				}
			);
			context.SaveChanges();
		}


    }
}