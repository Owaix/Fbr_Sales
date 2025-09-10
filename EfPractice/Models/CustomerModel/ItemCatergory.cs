namespace EfPractice.Models.CustomerModel
{
    public class ItemCatergory
    {

        public List<Imh> ImhList { get; set; } = new List<Imh>();
        public int Cid { get; set; }
        public string? Name { get; set; }
        public string? Aid { get; set; }
        public string? Aname { get; set; }
        public int? Mid { get; set; }
        public string? Mn { get; set; }

    }
}
