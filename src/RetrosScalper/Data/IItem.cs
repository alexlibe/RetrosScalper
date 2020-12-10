namespace RetrosScalper.Data
{
    public interface IItem
    {
        public string Name { get; set; }
        public float? Price { get; set; }
        public bool InStock { get; set; }
        public string URL { get; set; }
    }
}
