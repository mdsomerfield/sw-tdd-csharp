namespace Wordle.Web.Models;

public class ApiStatusModel
{
    public bool IsHealthy { get; set; }
    public string Status => IsHealthy ? "OK" : "Error";
    public DateTime LastChecked { get; set; }
}