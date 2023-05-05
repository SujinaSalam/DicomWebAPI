using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomWebAPI.Model
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public DateTime? PatientDOB { get; set; }

        public ICollection<Study>? Studies { get; set; }
    }
}
