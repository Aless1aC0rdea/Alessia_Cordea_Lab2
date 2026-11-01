using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Alessia_Cordea_Lab2.Models;
using Alessia_Cordea_Lab2.Models.ViewModels;

namespace Alessia_Cordea_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Alessia_Cordea_Lab2Context _context;

    public IndexModel(Alessia_Cordea_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;

    public CategoryIndexData CategoryData { get; set; } = default!;
    public int CategoryID { get; set; }

    public async Task OnGetAsync(int? id)
    {
        CategoryData = new CategoryIndexData();
        CategoryData.Categories = await _context.Category
            .Include(c => c.BookCategories)
                .ThenInclude(bc => bc.Book)
                    .ThenInclude(b => b.Author)
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

        // pastram si lista simpla, folosita pentru numele coloanelor din tabel
        Category = CategoryData.Categories.ToList();

        if (id != null)
        {
            CategoryID = id.Value;
            Category category = CategoryData.Categories
                .Where(c => c.ID == id.Value).Single();
            CategoryData.BookCategories = category.BookCategories;
        }
    }
}