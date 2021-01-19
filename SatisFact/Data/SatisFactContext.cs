using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SatisFact.Models;


namespace SatisFact.Data {
	public class SatisFactContext : DbContext {
		public SatisFactContext(DbContextOptions<SatisFactContext> options) : base(options) {
		}

		public DbSet<Building> Building { get; set; }
		public DbSet<Ingredient> Ingredient { get; set; }
	}
}
