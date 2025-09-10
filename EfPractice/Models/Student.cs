using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPractice.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

        [Column("StudentGender",TypeName ="varchar(100)")]
        public string Gender { get; set; }
        [Column("StudentName", TypeName = "varchar(100)")]
     

        public string Age { get; set; }

        public string Title { get; set; }
    }
}
