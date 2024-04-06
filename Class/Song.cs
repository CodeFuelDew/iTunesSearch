
using System.Web;

public class Song {
    public string? Name;
    public string? Artist;
    public string? Type;
    public string? IconUrl {get;set;}
    public string? Url;

    
    public string Stringify () {
        try {
        var ce = new ClickElement {
            Name = HttpUtility.UrlEncode( this.Name.Replace("\"", "\\\"")),
            Artist = HttpUtility.UrlEncode(this.Artist),
            Url = HttpUtility.UrlEncode(this.Url)
        };
        var s = System.Text.Json.JsonSerializer.Serialize(ce,typeof(ClickElement), new System.Text.Json.JsonSerializerOptions{ IncludeFields=true});
        return s.ToString();
        }catch(Exception ex) {
            Console.Out.WriteLine(ex.Message +": "+this.Artist);
            return null;
        }
    }

    

    public static IEnumerable<Song> GetSongsByName( string name ) {
        try {
                var str_o = "";
                using (var client = new System.Net.Http.HttpClient()) {
                    System.Net.Http.HttpResponseMessage response = client.GetAsync($"http://itunes.apple.com/search?term={name}").Result;
                    if(response.IsSuccessStatusCode)
                    {
                        str_o = response.Content.ReadAsStringAsync().Result;

                    }
                }
                var list = new List<Song>();
                
                dynamic json_result = System.Text.Json.JsonSerializer.Deserialize<dynamic>(str_o);

                
                var collection = json_result.GetProperty("results").EnumerateArray();
                foreach(var el in collection) {
                    var song = new Song();
                    // song.Type = el["wrapperType"].toString();
                    song.Type = el.GetProperty("wrapperType").GetString();
                    try {
                        song.Name = el.GetProperty("collectionName").GetString();
                    }catch {
                        song.Name = "";
                    }
                    song.Artist = el.GetProperty("artistName").GetString();
                    try {
                        song.Url = el.GetProperty("collectionViewUrl").GetString();
                    }catch {
                        song.Url = "";
                    }
                    song.IconUrl = el.GetProperty("artworkUrl60").GetString();

                    if(song.Type == "track") {
                        // var n = el["trackName"].toString();
                        var n = el.GetProperty("trackName").GetString();

                        if(!string.IsNullOrEmpty(n) && !string.IsNullOrEmpty(song.Name))
                            song.Name = $"{song.Name} : {n}";
                        else if(!string.IsNullOrEmpty(n)) 
                            song.Name = n;
                        
                        // var t = el["trackViewUrl"].toString();
                        var t = el.GetProperty("trackViewUrl").GetString();
                        if(!string.IsNullOrEmpty(t))
                            song.Url = t;
                    }
                    song.Name.Replace("\"", "'");
                    list.Add(song);
                }
                return list;
        }catch(Exception ex) {
                        Console.Out.WriteLine($"{ex}");

            return new List<Song>();
        }
    }
}

public class ArtistComparer : IComparer<Song>
{
    public int Compare(Song? x, Song? y)
    {
        if(string.Compare(x.Artist,y.Artist,true) == 0 )
            return 0;

        if(string.Compare(x.Artist,y.Artist,true) < 0 )
            return -1;

        return 1;
    }
}

public class SongNameComparer : IComparer<Song>
{
    public int Compare(Song? x, Song? y)
    {
         if(string.Compare(x.Name,y.Name,true) == 0 )
            return 0;

        if(string.Compare(x.Name,y.Name,true) < 0 )
            return -1;

        return 1;
    }
}