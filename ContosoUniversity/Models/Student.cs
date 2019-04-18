using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models
{
	// The ID property will be the primary key  of the Student class. 
	// By default, the Entity Framework interprets a property that's named ID or classnameID as the primary key.
	public class Student
	{
		public int Id { get; set; }
		public string LastName { get; set; }
		public string FirstMidName { get; set; }
		public DateTime EnrollmentDate { get; set; }
		/* 
		The Enrollments property is a navigation property. Navigation properties hold other entities that are 
        related to this entity. In this case, the Enrollments property of a Student entity will hold all of the 
        Enrollment entities that are related to that Student entity.
		*/
		/*
		If a navigation property can hold multiple entities (as in many-to-many or one-to-many relationships), its
		type must be a list in which entries can be added, deleted, and updated, such as ICollection<T>. You can
		specify ICollection<T> or a type such as List<T> or HashSet<T>.
		
		If you specify ICollection<T>, EF creates a HashSet<T> collection by default.
		*/
		public ICollection<Enrollment> Enrollments { get; set; }
	}
}
