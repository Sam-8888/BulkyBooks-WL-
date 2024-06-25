using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Cover Type")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
