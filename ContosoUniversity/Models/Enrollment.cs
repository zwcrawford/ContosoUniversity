using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models
{
	/* 
	The EnrollmentID property will be the primary key; this entity uses the classnameID pattern instead of ID by 
    itself as you saw in the Student entity.
	
	Entity Framework interprets a property as a foreign key property if it's named <navigation property name><primary
	key property name> (for example, StudentID for the Student navigation property since the Student entity's primary
	key is ID). Foreign key properties can also be named simply <primary key property name> (for example, CourseID
	since the Course entity's primary key is CourseID).
	*/
	public class Enrollment
	{
		public int EnrollmentID { get; set; }
		/*
		The CourseID property is a foreign key, and the corresponding navigation property is Course. An Enrollment
		entity is associated with one Course entity.
		*/
		public int CourseID { get; set; }
		/*
		The StudentID property is a foreign key, and the corresponding navigation property is Student. An Enrollment
		entity is associated with one Student entity, so the property can only hold a single Student entity (unlike
		the Student.Enrollments navigation property you saw earlier, which can hold multiple Enrollment entities).
		*/
		public int StudentID { get; set; }
		/*
		The Grade property is an enum. The question mark after the Grade type declaration indicates that the Grade
		property is nullable. A grade that's null is different from a zero grade -- null means a grade isn't known or
		hasn't been assigned yet.
		*/
		public Grade? Grade { get; set; }

		public Course Course { get; set; }
		public Student Student { get; set; }
	}
}
