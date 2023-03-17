using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proje.Models.MVVM
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }

        [StringLength(100)]
        [Required]
        public string StatusName { get; set; }

        public bool Active { get; set; }

    }
}
