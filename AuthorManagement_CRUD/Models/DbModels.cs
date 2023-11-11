using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthorManagement_CRUD.Models
{
    public class Book
    {
        public Book()
        {
            this.AuthorBooks = new List<AuthorBook>();
        }
        public int BookId { get; set; }
        [Required, StringLength(30), Display(Name = "Book Name")]
        public string BookName { get; set; } = default!;
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; }
    }
    public class Author
    {
        public Author()
        {
            this.AuthorBooks = new List<AuthorBook>();
        }
        public int AuthorId { get; set; }
        [Required, StringLength(40), Display(Name = "Candidate Name")]
        public string AuthorName { get; set; } = default!;
        [Required, Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Marital Status")]
        public bool MaritalStatus { get; set; }
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; }
    }
    public class AuthorBook
    {
        public int AuthorBookId { get; set; }
        [ForeignKey("Book")]
        public int? BookId { get; set; }
        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        public virtual Book Book { get; set; } = default!;
        public virtual Author Author { get; set; } = default!;
    }
    public class AuthorDbContext : DbContext
    {
        public AuthorDbContext(DbContextOptions<AuthorDbContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
    }
}
