using AuthorManagement_CRUD.Models;
using AuthorManagement_CRUD.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuthorManagement_CRUD.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly AuthorDbContext _db;
        private readonly IWebHostEnvironment _env;
        public AuthorsController(AuthorDbContext db, IWebHostEnvironment env)
        {
            this._db = db;
            _env = env;
        }
        public IActionResult Index()
        {
            var authors = _db.Authors.Include(a => a.AuthorBooks).ThenInclude(b => b.Book).OrderByDescending(x => x.AuthorId).ToList();
            return View(authors);
        }
        public IActionResult AddNewBook(int? id)
        {
            ViewBag.books = new SelectList(_db.Books.ToList(), "BookId", "BookName", (id != null) ? id.ToString() : "");
            return PartialView("_AddNewBook");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel avm, int[] bookId)
        {
            if (ModelState.IsValid)
            {
                Author author = new Author()
                {
                    AuthorName = avm.AuthorName,
                    DateOfBirth = avm.DateOfBirth,
                    Age = avm.Age,
                    MaritalStatus = avm.MaritalStatus
                };

                // Image
                var file = avm.ImageFile;
                string webroot = _env.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(avm.ImageFile.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);

                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        avm.ImageFile.CopyTo(stream);
                        author.Image = "/" + folder + "/" + imgFileName;
                    }
                }
                foreach (var item in bookId)
                {
                    AuthorBook ab = new AuthorBook()
                    {
                        Author = author,
                        AuthorId = author.AuthorId,
                        BookId = item
                    };
                    _db.AuthorBooks.Add(ab);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var author = await _db.Authors.FirstOrDefaultAsync(x => x.AuthorId == id);

            AuthorViewModel avm = new AuthorViewModel()
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                DateOfBirth = author.DateOfBirth,
                Age = author.Age,
                Image = author.Image,
                MaritalStatus = author.MaritalStatus
            };

            var existSkill = _db.AuthorBooks.Where(x => x.AuthorId == id).ToList();
            foreach (var item in existSkill)
            {
                avm.BookList.Add((int)item.BookId);
            }
            return View(avm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AuthorViewModel avm, int[] bookId)
        {
            if (ModelState.IsValid)
            {
                Author author = new Author()
                {
                    AuthorId = avm.AuthorId,
                    AuthorName = avm.AuthorName,
                    DateOfBirth = avm.DateOfBirth,
                    Age = avm.Age,
                    MaritalStatus = avm.MaritalStatus,
                    Image = avm.Image
                };
                var file = avm.ImageFile;
                if (file != null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Picture";
                    string imgFileName = Path.GetFileName(avm.ImageFile.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        avm.ImageFile.CopyTo(stream);
                        author.Image = "/" + folder + "/" + imgFileName;
                    }
                }
                else
                {

                }
                var existSkill = _db.AuthorBooks.Where(x => x.AuthorId == author.AuthorId).ToList();
                foreach (var item in existSkill)
                {
                    _db.AuthorBooks.Remove(item);
                }
                foreach (var item in bookId)
                {
                    AuthorBook ab = new AuthorBook()
                    {
                        AuthorId = author.AuthorId,
                        BookId = item
                    };
                    _db.AuthorBooks.Add(ab);
                }
                _db.Update(author);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _db.Authors
                .Include(a => a.AuthorBooks)
                .ThenInclude(ab => ab.Book)
                .FirstOrDefault(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _db.Authors.Find(id);

            if (author == null)
            {
                return NotFound();
            }

            _db.Authors.Remove(author);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
