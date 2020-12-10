namespace RetrosScalper.Data
{
    class BestBuyItem : IItem
    {
        public string Name { get; set; }
        public float? Price { get; set; } = null;
        public bool InStock { get; set; }
        public string URL { get; set; }
    }
}
