using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomWebAPI.Model.DTO
{
    public class StudyDto
    {
        [Required]
        public string StudyInstanceUID { get; set; }

        public string StudyDescription { get; set; }

        public string StudyID { get; set; }

        public DateTime? StudyDateTime { get; set; }

        public int PatientId { get; set; }
       // public ICollection<Series>? Serieses { get; set; }

    }
}
