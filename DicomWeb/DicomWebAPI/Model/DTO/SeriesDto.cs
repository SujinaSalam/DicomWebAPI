using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomWebAPI.Model.DTO
{
    public class SeriesDto
    {
        [Required]
        public string SeriesInstanceUID { get; set; }

        public string Modality { get; set; }

        public string BodyPart { get; set; }

        public string StudyInstanceUID { get; set; }

        //public ICollection<Image>? Images { get; set; }
    }
}
