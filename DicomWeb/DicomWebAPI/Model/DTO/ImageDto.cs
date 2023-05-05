using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicomWebAPI.Model.DTO
{
    public class ImageDto
    {
        [Required]
        public string ImageInstanceUID { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public string SeriesInstanceUID { get; set; }

    }
}
