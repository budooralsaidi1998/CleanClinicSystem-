using System.ComponentModel.DataAnnotations;

namespace CleanCllinicSystem.models
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string spe { get; set; }
        [Required]
        public int num_of_slots { get; set; }

        public virtual ICollection<booking> bookings { get; set; }
    }
}
