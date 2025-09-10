namespace EfPractice.Models.CustomerModel
{
    public class ItemRegistrarion
    {
        public List<Cate> CatergoryList { get; set; } = new List<Cate>();
        public int Itcode { get; set; }
        public string? Itname { get; set; }
        public string? Unit { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Weight { get; set; }
        public string? Pack { get; set; }
        public string? Type { get; set; }
        public int? Acid { get; set; }
        public decimal? OpenAmt { get; set; }
        public decimal? Prate { get; set; }
        public string? Ic { get; set; }
        public string? IcName { get; set; }
    }
}
