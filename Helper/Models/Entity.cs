using SQLite;
namespace Helper.Models;

public abstract class Entity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // I chose the format so that I can use strftime in my sqlite queries
    private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    [Ignore]
    public DateTime Date
    {
        get => DateTime.ParseExact(DateAsString, DATE_FORMAT, null);
        set
        {
            DateAsString = value.ToString(DATE_FORMAT);
        }
    }

    [Indexed]
    public string DateAsString { get; set; } = DateTime.Now.ToString(DATE_FORMAT);

    public bool IsDeleted { get; set; } = false;
}
