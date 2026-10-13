using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorDirectoryApi.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required.")]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "A valid email address is required.")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "YearsOfExperience must be 0 or greater.")]
        public int YearsOfExperience { get; set; }

        [Range(0.00, 1000000.00, ErrorMessage = "ConsultationFee must be non-negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }
    }
}