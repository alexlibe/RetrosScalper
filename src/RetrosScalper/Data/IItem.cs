namespace RetrosScalper.Data
{
    public interface IItem
    {
        string Name { get; set; }
        float? Price { get; set; }
        bool InStock { get; set; }
        string URL { get; set; }
    }
}
