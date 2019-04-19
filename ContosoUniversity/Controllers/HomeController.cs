using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;
using System.Data.Common;

namespace ContosoUniversity.Controllers
{
	public class HomeController : Controller
	{
		private readonly SchoolContext _context;

		public HomeController(SchoolContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> About()
		{
			IQueryable<EnrollmentDateGroup> data =
				from student in _context.Students
				group student by student.EnrollmentDate into dateGroup
				select new EnrollmentDateGroup()
				{
					EnrollmentDate = dateGroup.Key,
					StudentCount = dateGroup.Count()
				};
			return View(await data.AsNoTracking().ToListAsync());
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		/*
		The LINQ statement below groups the student entities by enrollment date,
		calculates the number of entities in each group, and stores the results i
		a collection of EnrollmentDateGroup view model objects.
		
		In the 1.0 version of Entity Framework Core, the entire result set is
		returned to the client, and grouping is done on the client. In some
		scenarios this could create performance problems. Be sure to test
		performance with production volumes of data, and if necessary use raw SQL
		to do the grouping on the server.

		*/
	}
}
