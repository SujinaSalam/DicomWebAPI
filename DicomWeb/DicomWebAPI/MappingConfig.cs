using AutoMapper;
using DicomWebAPI.Model;
using DicomWebAPI.Model.DTO;

namespace DicomWebAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<LocalUser, RegistrationRequestDto>().ReverseMap();
            CreateMap<Study, StudyDto>().ReverseMap();
            CreateMap<Series, SeriesDto>().ReverseMap();
            CreateMap<Image, ImageDto>().ReverseMap();
        }
    }
}
