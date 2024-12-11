using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanCllinicSystem.models
{
    public class Clinic
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string spe { get; set; }
        [Required]
        public int num_of_slots { get; set; }

        public virtual ICollection<booking> bookings { get; set; }
    }
}
