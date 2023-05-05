using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DicomWebAPI.Model
{
    public class Study
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string StudyInstanceUID { get; set; }

        public string StudyDescription { get; set; }

        public string StudyID { get; set;}

        public DateTime? StudyDateTime { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public ICollection<Series>? Series { get; set; }
    }
}
