namespace RetrosScalper.VS2017.Data
{
    class NeweggItem
    {
        public string Name { get; set; }
        public float? Price { get; set; } = null;
        public bool InStock { get; set; }
        public string SKU { get; set; }
        public string URL { get; set; }
    }
}
