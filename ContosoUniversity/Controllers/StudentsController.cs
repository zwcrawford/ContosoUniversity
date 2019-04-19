using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

/*
When you click Add, the Visual Studio scaffolding engine creates a StudentsController.cs file and
set of views (.cshtml files) that work with the controller.

(The scaffolding engine can also create the database context for you if you don't create it manually
first as you did earlier for this tutorial. You can specify a new context class in the Add
Controller box by clicking the plus sign to the right of Data context class. Visual Studio will then
create your DbContext class as well as the controller and views.)

You'll notice that the controller takes a SchoolContext as a constructor parameter.

ASP.NET Core dependency injection takes care of passing an instance of SchoolContext into the
controller. You configured that in the Startup.cs file earlier.

The controller contains an Index action method, which displays all students in the database. The
method gets a list of students from the Students entity set by reading the Students property of the
database context instance.

 */
namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

		// GET: Students
		/*
		This code receives a sortOrder parameter from the query string in the URL. The query string value is
		provided by ASP.NET Core MVC as a parameter to the action method. The parameter will be a string that'
		either "Name" or "Date", optionally followed by an underscore and the string "desc" to specify
		descending order. The default sort order is ascending.

		The first time the Index page is requested, there's no query string. The students are displayed in
		ascending order by last name, which is the default as established by the fall-through case in the
		switch statement. When the user clicks a column heading hyperlink, the appropriate sortOrder value is
		provided in the query string.

		The two ViewData elements (NameSortParm and DateSortParm) are used by the view to configure the column
		heading hyperlinks with the appropriate query string values.

		*/
		public async Task<IActionResult> Index(
		string sortOrder,
		string currentFilter,
		string searchString,
		int? pageNumber)
			{
				ViewData["CurrentSort"] = sortOrder;
				ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
				ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

				if (searchString != null)
				{
					pageNumber = 1;
				}
				else
				{
					searchString = currentFilter;
				}

				ViewData["CurrentFilter"] = searchString;

				var students = from s in _context.Students
							   select s;
				if (!String.IsNullOrEmpty(searchString))
				{
					students = students.Where(s => s.LastName.Contains(searchString)
										   || s.FirstMidName.Contains(searchString));
				}
				switch (sortOrder)
				{
					case "name_desc":
						students = students.OrderByDescending(s => s.LastName);
						break;
					case "Date":
						students = students.OrderBy(s => s.EnrollmentDate);
						break;
					case "date_desc":
						students = students.OrderByDescending(s => s.EnrollmentDate);
						break;
					default:
						students = students.OrderBy(s => s.LastName);
						break;
				}

				int pageSize = 3;
				return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
			}

		// POST: Students/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
			[Bind("EnrollmentDate,FirstMidName,LastName")] Student student)
		{
			/*
			This code adds the Student entity created by the ASP.NET Core MVC model binder to the Students entity
			set and then saves the changes to the database. (Model binder refers to the ASP.NET Core MVC
			functionality that makes it easier for you to work with data submitted by a form; a model binder
			converts posted form values to CLR types and passes them to the action method in parameters. In this
			case, the model binder instantiates a Student entity for you using property values from the Form
			collection.)

			You removed ID from the Bind attribute because ID is the primary key value which SQL Server will set
			automatically when the row is inserted. Input from the user doesn't set the ID value.

			Other than the Bind attribute, the try-catch block is the only change you've made to the scaffolded
			code. If an exception that derives from DbUpdateException is caught while the changes are being saved,
			a generic error message is displayed. DbUpdateException exceptions are sometimes caused by something
			external to the application rather than a programming error, so the user is advised to try again.
			Although not implemented in this sample, a production quality application would log the exception. For
			more information, see the Log for insight section in Monitoring and Telemetry (Building Real-World
			Cloud Apps with Azure).

			The ValidateAntiForgeryToken attribute helps prevent cross-site request forgery (CSRF) attacks. The
			token is automatically injected into the view by the FormTagHelper and is included when the form is
			submitted by the user. The token is validated by the ValidateAntiForgeryToken attribute. For more
			information about CSRF, see Anti-Request Forgery.
			*/
			try
			{
				/* 
				This is server-side validation that you get by default; in a later tutorial you'll see how to add
				attributes that will generate code for client-side validation also. The following code: 

				[if (ModelState.IsValid)]

				shows the model validation check in the Create method.
				*/
				if (ModelState.IsValid)
				{
					_context.Add(student);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}
			return View(student);
		}

		// GET: Students/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

		// POST: Students/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost, ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditPost(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
			if (await TryUpdateModelAsync<Student>(
				studentToUpdate,
				"",
				s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
			{
				try
				{
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateException /* ex */)
				{
					//Log the error (uncomment ex variable name and write a log.)
					ModelState.AddModelError("", "Unable to save changes. " +
						"Try again, and if the problem persists, " +
						"see your system administrator.");
				}
			}
			return View(studentToUpdate);
		}

		// GET: Students/Delete/5
		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
		{
			/*
			This code accepts an optional parameter that indicates whether the method was called after a failure t
			save changes. This parameter is false when the HttpGet Delete method is called without a previous
			failure. When it's called by the HttpPost Delete method in response to a database update error, the
			parameter is true and an error message is passed to the view.
			*/
			if (id == null)
			{
				return NotFound();
			}

			var student = await _context.Students
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);
			if (student == null)
			{
				return NotFound();
			}

			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return View(student);
		}

		// POST: Students/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var student = await _context.Students.FindAsync(id);
			if (student == null)
			{
				return RedirectToAction(nameof(Index));
			}

			try
			{
				_context.Students.Remove(student);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.)
				return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
			}
		}
	}
}
/*
Applies to:

	GET: Students/Edit/5 
	POST: Students/Edit/5
 
These changes implement a security best practice to prevent overposting. The scaffolder generated a Bind attribute and
added the entity created by the model binder to the entity set with a Modified flag. That code isn't recommended for
many scenarios because the Bind attribute clears out any pre-existing data in fields not listed in the Include
parameter.

The new code reads the existing entity and calls TryUpdateModel to update fields in the retrieved entity based on user
input in the posted form data. The Entity Framework's automatic change tracking sets the Modified flag on the fields
that are changed by form input. When the SaveChanges method is called, the Entity Framework creates SQL statements to
update the database row. Concurrency conflicts are ignored, and only the table columns that were updated by the user
are updated in the database. (A later tutorial shows how to handle concurrency conflicts.)

As a best practice to prevent overposting, the fields that you want to be updateable by the Edit page are whitelisted
in the TryUpdateModel parameters. (The empty string preceding the list of fields in the parameter list is for a prefix
to use with the form fields names.) Currently there are no extra fields that you're protecting, but listing the fields
that you want the model binder to bind ensures that if you add fields to the data model in the future, they're
automatically protected until you explicitly add them here.

As a result of these changes, the method signature of the HttpPost Edit method is the same as the HttpGet Edit method;
therefore you've renamed the method EditPost.

*/
