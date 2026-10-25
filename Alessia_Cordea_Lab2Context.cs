using Microsoft.EntityFrameworkCore;

public class Alessia_Cordea_Lab2Context(DbContextOptions<Alessia_Cordea_Lab2Context> options) : DbContext(options)
{
    public DbSet<Alessia_Cordea_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Alessia_Cordea_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Alessia_Cordea_Lab2.Models.Author> Author { get; set; } = default!;
    public DbSet<Alessia_Cordea_Lab2.Models.Category> Category { get; set; } = default!;
}
