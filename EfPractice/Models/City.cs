using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public partial class City
    {
        [Key]
        public int? Id { get; set; }
        public string? CityName { get; set; }
    }
}
