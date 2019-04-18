﻿using ContosoUniversity.Models;
using System;
using System.Linq;

/* 
Initialize DB with test data

The Entity Framework will create an empty database for you. In this section, you write a method that's called after the database
is created in order to populate it with test data.

Here you'll use the EnsureCreated method to automatically create the database. In a later tutorial you'll see how to handle
model changes by using Code First Migrations to change the database schema instead of dropping and re-creating the database.

In the Data folder, create a new class file named DbInitializer.cs and replace the template code with the following code, which
causes a database to be created when needed and loads test data into the new database.

The code checks if there are any students in the database, and if not, it assumes the database is new and needs to be seeded with
test data. It loads test data into arrays rather than List<T> collections to optimize performance.
 */


namespace ContosoUniversity.Data
{
	public static class DbInitializer
	{
		public static void Initialize(SchoolContext context)
		{
			context.Database.EnsureCreated();

			// Look for any students.
			if (context.Students.Any())
			{
				return;   // DB has been seeded
			}

			var students = new Student[]
			{
			new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
			new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
			new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
			new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
			new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
			new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
			new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
			new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
			};
			foreach (Student s in students)
			{
				context.Students.Add(s);
			}
			context.SaveChanges();

			var courses = new Course[]
			{
			new Course{CourseID=1050,Title="Chemistry",Credits=3},
			new Course{CourseID=4022,Title="Microeconomics",Credits=3},
			new Course{CourseID=4041,Title="Macroeconomics",Credits=3},
			new Course{CourseID=1045,Title="Calculus",Credits=4},
			new Course{CourseID=3141,Title="Trigonometry",Credits=4},
			new Course{CourseID=2021,Title="Composition",Credits=3},
			new Course{CourseID=2042,Title="Literature",Credits=4}
			};
			foreach (Course c in courses)
			{
				context.Courses.Add(c);
			}
			context.SaveChanges();

			var enrollments = new Enrollment[]
			{
			new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
			new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
			new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
			new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
			new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
			new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
			new Enrollment{StudentID=3,CourseID=1050},
			new Enrollment{StudentID=4,CourseID=1050},
			new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
			new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
			new Enrollment{StudentID=6,CourseID=1045},
			new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
			};
			foreach (Enrollment e in enrollments)
			{
				context.Enrollments.Add(e);
			}
			context.SaveChanges();
		}
	}
}
