using System.ComponentModel.DataAnnotations;

namespace DicomWebAPI.Model.DTO
{
    public class PatientDto
    {
        [Required]
        public int PatientId { get; set; }
        public string? PatientName { get; set;}
        public DateTime? PatientDOB { get; set; }

        //public ICollection<Study>? Studies { get; set; }
    }
}
