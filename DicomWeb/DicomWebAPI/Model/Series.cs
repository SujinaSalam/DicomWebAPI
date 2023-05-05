using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DicomWebAPI.Model
{
    public class Series
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string SeriesInstanceUID { get; set; }

        public string Modality { get; set; }

        public string BodyPart { get; set;}

        [ForeignKey("Study")]
        public string StudyInstanceUID { get; set; }

        public Study Study { get; set; }

        public ICollection<Image>? Images { get; set; }
    }
}
