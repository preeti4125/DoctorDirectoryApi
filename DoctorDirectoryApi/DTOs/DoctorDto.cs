using System.ComponentModel.DataAnnotations;

namespace DoctorDirectoryApi.DTOs
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "FullName is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required.")]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "A valid email address is required.")]
        public string Email { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "YearsOfExperience must be 0 or greater.")]
        public int YearsOfExperience { get; set; }

        [Range(0.00, 1000000.00, ErrorMessage = "ConsultationFee must be non-negative.")]
        public decimal ConsultationFee { get; set; }
    }

    public class UpdateDoctorDto : CreateDoctorDto
    {
    }
}