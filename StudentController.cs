using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PracticeDatabase.Models;

namespace PracticeDatabase.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            // Initialize the database context
            using (StudentDBEntities1 db = new StudentDBEntities1())
            {
                // Retrieve all students from the database
                List<stud> students = db.studs.ToList();
                return View(students);
            }
        }

        // GET: AddRecordForm
        public ActionResult AddRecord()
        {
            // Return the view for adding a new record
            return View();
        }

        // POST: AddRecord
        [HttpPost]
        public ActionResult AddRecord(stud st)
        {
            // Validate the incoming model data
            if (ModelState.IsValid)
            {
                using (StudentDBEntities1 db = new StudentDBEntities1())
                {
                    // Add the student record to the database
                    db.studs.Add(st);
                    db.SaveChanges();
                }

                // Redirect to the Index action to display the updated list
                return RedirectToAction("Index");
            }

            // If the model is invalid, return the form with validation messages
            return View(st);
        }

        // GET: Delete a student record
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (StudentDBEntities1 db = new StudentDBEntities1())
            {
                // Find the student by ID and remove if found
                stud student = db.studs.Find(id);
                if (student != null)
                {
                    db.studs.Remove(student);
                    db.SaveChanges();
                }
            }

            // Redirect to the Index action after deletion
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] selectedIds)
        {
            if (selectedIds != null && selectedIds.Length > 0)
            {
                using (StudentDBEntities1 db = new StudentDBEntities1())
                {
                    // Find and remove all the selected records
                    foreach (var id in selectedIds)
                    {
                        var student = db.studs.Find(id);
                        if (student != null)
                        {
                            db.studs.Remove(student);
                        }
                    }

                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Search by fname or lname
        [HttpGet]
        public ActionResult Search(string searchQuery, string lnameSearchQuery, string searchField, string searchQ)
        {
            using (StudentDBEntities1 db = new StudentDBEntities1())
            {
                List<stud> result = new List<stud>();

                // If both searchQuery and lnameSearchQuery are provided, give precedence to lnameSearchQuery
                if (!string.IsNullOrEmpty(lnameSearchQuery))
                {
                    result = db.studs
                        .Where(s => s.lname.StartsWith(lnameSearchQuery))
                        .ToList();
                }
                else if (!string.IsNullOrEmpty(searchQuery))
                {
                    result = db.studs
                        .Where(s => s.fname.StartsWith(searchQuery) || s.lname.StartsWith(searchQuery) || s.city.StartsWith(searchQuery))
                        .ToList();
                }
                else
                {
                    result = db.studs.ToList(); // Return all records if no query
                }

                if (!string.IsNullOrEmpty(searchQ))
                {
                    switch (searchField)
                    {
                        case "fname":
                            result = db.studs.Where(s => s.fname.StartsWith(searchQ)).ToList();
                            break;
                        case "lname":
                            result = db.studs.Where(s => s.lname.StartsWith(searchQ)).ToList();
                            break;
                        case "city":
                            result = db.studs.Where(s => s.city.StartsWith(searchQ)).ToList();
                            break;
                        default:
                            result = db.studs.ToList();
                            break;
                    }
                }
                else
                {
                    result = db.studs.ToList(); // Return all records if no query
                }

                return View("Index", result); // Reuse the Index view to display search results
            }
        }
    }
}
