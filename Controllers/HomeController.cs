using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using iTunesDemo.Models;

namespace iTunesDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IEnumerable<Song>? Songs;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index( string searchString )
    {
        if(string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(ViewBag.SearchString))
            return View();

        if(!string.IsNullOrEmpty(searchString))
            ViewBag.SearchString = searchString;

        Songs = Song.GetSongsByName(searchString);
        var svm = new SongViewModel{
            SearchString = searchString,
            Songs = Songs.ToList()
        };

        return View(svm);
    }

    // public ActionResult SortResults() {

    //     //to do sort via ajax and deliver results
    //     Console.Out.WriteLine(Request.Form.Keys);
        
    //     SongViewModel svm = System.Text.Json.JsonSerializer.Deserialize<SongViewModel>(x, new System.Text.Json.JsonSerializerOptions{IncludeFields=true});
    //     //Console.Out.WriteLine(svm.SearchString);
    //     svm.Songs.Sort(new SongNameComparer());
    //     return PartialView("_SortResults", svm);
    //     //return svm.Songs.Count.ToString();
    // }

    public ActionResult GetMoreInfo(string ce) {
        Task.Run( () => ClickCounter.handle_click(ce));
        ClickElement o;
    
        o = System.Text.Json.JsonSerializer.Deserialize<ClickElement>(ce,new System.Text.Json.JsonSerializerOptions{ IncludeFields=true});
  
        return Redirect(o.Url);
    }

    public IActionResult ClickView() {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
