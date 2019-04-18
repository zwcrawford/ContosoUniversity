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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

		// GET: Students/Details/5

		/*
		The scaffolded code for the Students Index page left out the Enrollments property, because
		that property holds a collection. In the Details page, you'll display the contents of the
		collection in an HTML table.

		In Controllers/StudentsController.cs, the action method for the Details view uses the
		FirstOrDefaultAsync method to retrieve a single Student entity. Add code that calls
		Include, ThenInclude, and AsNoTracking methods, as shown in the following highlighted code.

		The Include and ThenInclude methods cause the context to load the Student.Enrollments
		navigation property, and within each enrollment the Enrollment.Course navigation property.

		The AsNoTracking method improves performance in scenarios where the entities returned won't
		be updated in the current context's lifetime. You'll learn more about AsNoTracking at the
		end of this tutorial.

		 */
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var student = await _context.Students
					.Include(s => s.Enrollments)
					.ThenInclude(e => e.Course)
					.AsNoTracking()
					.FirstOrDefaultAsync(m => m.Id == id);

			if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
