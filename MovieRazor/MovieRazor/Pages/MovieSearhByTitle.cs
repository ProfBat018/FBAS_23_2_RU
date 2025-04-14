namespace MovieRazor.Pages;

public class MovieSearhByTitle
{
    public int page { get; set; }
    public MovieSearchRes[] results { get; set; }
    public int total_pages { get; set; }
    public int total_results { get; set; }
}