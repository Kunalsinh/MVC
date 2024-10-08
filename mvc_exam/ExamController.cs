using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exam.Models;

namespace Exam.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        public ActionResult Index()
        {
            CIAEntities1 db = new CIAEntities1();
            List<Register> reg = db.Registers.ToList();
            return View(reg);
        }

        public ActionResult Login() { 
            return View();
        }

        public ActionResult Create() {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Register formRecord) {
            CIAEntities1 db = new CIAEntities1();
            Register record = new Register();
            record = db.Registers.Where(a => a.username == formRecord.username).FirstOrDefault();
            if (record == null)
            {
                db.Registers.Add(formRecord);
                db.SaveChanges();
                return RedirectToAction("Login");
                
            }
            else {
                TempData["insert_error"] = "Record not inserted!";
                return View();    
            }
        }

        [HttpPost]
        public ActionResult Login(Register formRecord) {
            CIAEntities1 db = new CIAEntities1();
            Register record = new Register();
            record = db.Registers.Where(a => a.username == formRecord.username && a.password == formRecord.password).FirstOrDefault();
            if (record != null)
            {
                return RedirectToAction("Index");
            }
            else {
                TempData["login_error"] = "Error Occured while login!";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Delete(int id) {
            CIAEntities1 db = new CIAEntities1();
            Register record = db.Registers.Find(id);
            if (record != null) { 
                db.Registers.Remove(record);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteSelected(int[] selectedIds) {
            if (selectedIds != null && selectedIds.Length > 0) {
                CIAEntities1 db = new CIAEntities1();

                foreach (var id in selectedIds) {
                    Register record = db.Registers.Find(id);
                    if (record!= null) {
                        db.Registers.Remove(record);
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(string searchQuery, string searchField) {
            CIAEntities1 db = new CIAEntities1 ();
            List<Register> result = new List<Register> ();

            if (searchQuery != null) {
                result = db.Registers.Where(a => a.name.StartsWith(searchQuery)).ToList();
            }

            if (searchQuery != null) {
                switch (searchField) {
                    case "name":
                        result = db.Registers.Where(a => a.name.StartsWith(searchQuery)).ToList ();
                        break;

                    case "gender":
                        result = db.Registers.Where(a => a.gender.StartsWith(searchQuery)).ToList();
                        break;
                    default:
                        result = db.Registers.ToList(); 
                        break;
                }
            }
            
            return View("Index", result);
        }
    }
}