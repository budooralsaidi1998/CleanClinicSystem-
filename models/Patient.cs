using System.ComponentModel.DataAnnotations;

namespace CleanCllinicSystem.models
{
    public enum GenderType
    {
        Male,
        Female
    }
    public class Patient
    {
        [Key]
        public int Pid { get; set; }
        [Required]
        public string pname { get; set; }
        [Required]
        public int age { get; set; }
        [Required]
        public GenderType RGender { get; set; }
        public virtual ICollection<booking> bookings { get; set; }

    }
}
