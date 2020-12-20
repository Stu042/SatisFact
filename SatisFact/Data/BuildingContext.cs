using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SatisFact.Models;


namespace SatisFact.Data
{
	public class BuildingContext : DbContext
	{
		public BuildingContext(DbContextOptions<BuildingContext> options) : base(options)
		{
		}

		public DbSet<Building> Building { get; set; }
	}
}
