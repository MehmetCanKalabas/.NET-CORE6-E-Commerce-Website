using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proje.Models.MVVM
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required]
        public string NameSurname { get; set; }

        [StringLength(100)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [StringLength(100)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Telephone { get; set; }
        public string InvoıcesAddres { get; set; }
        public bool IsAdmin { get; set; }
        public bool Active { get; set; }
    }
}
