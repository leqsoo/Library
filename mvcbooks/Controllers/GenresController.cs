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
    public class GenresController : BaseController
    {
        private LibraryEntities db = new LibraryEntities();
        
        // GET: Genres
        public ActionResult Index()
        {
            return View(db.Genres.OrderBy(x => x.Genre_Name).ToList());
        }
        

        // GET: Genres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Genre_Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                if(db.Genres.Any(g => g.Genre_Name.Equals(genre.Genre_Name)))
                {
                    ViewBag.Message = "This Genre already exists";
                }
                else { 
                    db.Genres.Add(genre);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                var similarGenre = (from myGenre in db.Genres
                                   where myGenre.Genre_Name == genre.Genre_Name
                                   select myGenre.Id).FirstOrDefault();

                if (similarGenre != 0 && similarGenre != genre.Id)
                {
                    ModelState.AddModelError("", "Such Genre already exists in the DataBase!");
                }
                else
                {
                    db.Entry(genre).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(genre);
        }

        public ActionResult Delete(int id)
        {

            Genre genre = db.Genres.Find(id);

            int counter = 0;
            foreach (var item in db.Book_Genre.Where(x => x.Genre_Id == id))
            {

                if (item.Book.Book_Genre.Count() == 1)
                {
                    counter++;
                }
            }

            if (counter == 0)
            {
                foreach (var item in db.Book_Genre.Where(x => x.Genre_Id == id))
                {
                    db.Book_Genre.Remove(item);
                }
                db.Genres.Remove(genre);
                db.SaveChanges();                
            }
            else
            {
                TempData["ErrorMessage"] = "You can't delete the Genre '" + genre.Genre_Name + "'";
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
