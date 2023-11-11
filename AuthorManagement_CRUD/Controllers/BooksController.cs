using AuthorManagement_CRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthorManagement_CRUD.Controllers
{
    public class BooksController : Controller
    {
        private readonly AuthorDbContext _db;
        public BooksController(AuthorDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Books.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("BookId,BookName")] Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || _db.Books == null)
            {
                return NotFound();
            }
            var book = _db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        public IActionResult Edit([Bind("BookId,BookName")] Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || _db.Books == null)
            {
                return NotFound();
            }
            var book = _db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            if (_db.Books == null)
            {
                return NotFound();
            }
            var book = new Book
            {
                BookId = id
            };
            _db.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
