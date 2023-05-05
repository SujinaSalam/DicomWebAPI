using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DicomWebAPI.Model
{
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ImageInstanceUID { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set;}

        [ForeignKey("Series")]
        public string SeriesInstanceUID { get; set; }

        public Series Series { get; set; }
    }
}
