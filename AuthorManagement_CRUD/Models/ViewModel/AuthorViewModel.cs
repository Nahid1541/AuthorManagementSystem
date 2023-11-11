using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuthorManagement_CRUD.Models.ViewModel
{
    public class AuthorViewModel
    {
        public AuthorViewModel()
        {
            this.BookList = new List<int>();
        }
        public int AuthorId { get; set; }
        [Required, StringLength(40), Display(Name = "Author Name")]
        public string AuthorName { get; set; } = default!;
        [Required, Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
        public bool MaritalStatus { get; set; }
        public List<int> BookList { get; set; }
    }
}
