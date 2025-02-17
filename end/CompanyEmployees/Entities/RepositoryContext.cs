﻿using Entities.Configuration;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
	public class RepositoryContext : DbContext
	{
		public RepositoryContext(DbContextOptions options)
		: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CompanyConfiguration());
		}

		public DbSet<Company> Companies { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Address> Addresses { get; set; }
	}
}
