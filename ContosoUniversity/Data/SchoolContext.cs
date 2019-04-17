using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
	/*
	This code creates a DbSet property for each entity set. In Entity Framework
	terminology, an entity set typically corresponds to a database table, and an entity
	corresponds to a row in the table.

	We could've omitted the DbSet<Enrollment> and DbSet<Course> statements and it would
	work the same. The Entity Framework would include them implicitly because the Student
	entity references the Enrollment entity and the Enrollment entity references the
	Course entity.

	Basically, lists are created for each base table...Courses, Students, etc.
	Note that the join tables also need to be defined here as well which is different
	from ADO.NET.
	*/
	public class SchoolContext : DbContext
	{
		public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
		{
		}

		public DbSet<Course> Courses { get; set; }
		public DbSet<Enrollment> Enrollments { get; set; }
		public DbSet<Student> Students { get; set; }

		/*
		When the database is created, EF creates tables that have names the same as the
		DbSet property names. Property names for collections are typically plural
		(Students rather than Student), but developers disagree about whether table names
		should be pluralized or not. For this tutorials you'll override the default
		behavior below by specifying singular table names in the DbContext. To do that,
		add the following highlighted code.
		*/
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Course>().ToTable("Course");
			modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
			modelBuilder.Entity<Student>().ToTable("Student");
		}
	}
}
