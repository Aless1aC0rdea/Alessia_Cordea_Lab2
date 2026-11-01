using Alessia_Cordea_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Alessia_Cordea_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Alessia_Cordea_Lab2Context _context;

    public IndexModel(Alessia_Cordea_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = default!;
    public BookData BookD { get; set; } = default!;
    public int BookID { get; set; }
    public int CategoryID { get; set; }

    //sortare
    public string TitleSort { get; set; }
    public string AuthorSort { get; set; }
    public string CurrentFilter { get; set; }

    // filtrul pe autor din Lab 2
    [BindProperty(SupportsGet = true)]
    public int? AuthorID { get; set; }

    public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
    {
        ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "FullName");

        BookD = new BookData();

        // urmatorul click pe coloana inverseaza ordinea
        TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        AuthorSort = sortOrder == "author" ? "author_desc" : "author";

        // pasul 12 - retinem textul cautat, ca sa ramana in casuta de cautare
        CurrentFilter = searchString;

        var books = _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
            .AsNoTracking();

        if (AuthorID != null)
        {
            books = books.Where(b => b.AuthorID == AuthorID);
        }

        // pasul 12 - cautare dupa titlu sau dupa numele autorului
        if (!String.IsNullOrEmpty(searchString))
        {
            books = books.Where(s => s.Author.FirstName.Contains(searchString)
                                  || s.Author.LastName.Contains(searchString)
                                  || s.Title.Contains(searchString));
        }

        BookD.Books = await books
            .OrderBy(b => b.Title)
            .ToListAsync();

        if (id != null)
        {
            BookID = id.Value;
            Book book = BookD.Books
                .Where(i => i.ID == id.Value).Single();
            BookD.Categories = book.BookCategories.Select(s => s.Category);
        }

        //sortarea efectiva
        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Title);
                break;
            case "author_desc":
                BookD.Books = BookD.Books.OrderByDescending(s => s.Author?.FullName);
                break;
            case "author":
                BookD.Books = BookD.Books.OrderBy(s => s.Author?.FullName);
                break;
            default:
                BookD.Books = BookD.Books.OrderBy(s => s.Title);
                break;
        }

        // pastram si lista simpla, folosita pentru numele coloanelor din tabel
        Book = BookD.Books.ToList();
    }
}