using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mvcbooks;
using mvcbooks.Data;
using mvcbooks.Models;

namespace mvcbooks.Controllers
{
    public class AuthorsController : BaseController
    {
        private LibraryEntities db = new LibraryEntities();
        

        // GET: Authors
        public ActionResult Index()
        {

            return View(db.Authors.OrderBy(x => x.Name).ToList());
        }
        
        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        

        // POST: Authors/Delete/5
        public ActionResult Delete(int id)
        {
            Author author = db.Authors.Find(id);

            //Counter is used to check if the Author - to - be - deleted is the last Author for any Book.
            //      If it is last one, we can't leave the Book without any Author
            //      If the Book has other Authors too, then we let this Author to be deleted.
     
            int counter = 0;
            foreach (var item in db.Author_Book.Where(x => x.Author_Id == id))
            {
                if (item.Book.Author_Book.Count() == 1)
                {
                    counter++;
                }
            }

            if (counter == 0)
            {
                foreach (var item in db.Author_Book.Where(x => x.Author_Id == id))
                {
                    db.Author_Book.Remove(item);
                }
                db.Authors.Remove(author);
                db.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "You can't delete the Author '" + author.Name + "'";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
