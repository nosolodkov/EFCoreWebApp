using System.Text.Json;
using EFCoreDataAccessLibrary.DataAccess;
using EFCoreDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWeb.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly PeopleContext _db;

    public IndexModel(ILogger<IndexModel> logger, PeopleContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void OnGet()
    {
        LoadSampleData();

        var peope = _db.People
            .Include(a => a.Addresses)
            .Include(a => a.EmailAddresses)
            .ToList();
    }

    private void LoadSampleData()
    {
        if (_db.People.Any() is false) 
        {
            var file = System.IO.File.ReadAllText("generated.json");
            var people = JsonSerializer.Deserialize<List<Person>>(file);
            _db.AddRange(people);
            _db.SaveChanges();
        }
    }
}