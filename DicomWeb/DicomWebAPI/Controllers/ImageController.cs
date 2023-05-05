using AutoMapper;
using Azure;
using DicomWebAPI.Model;
using DicomWebAPI.Model.DTO;
using DicomWebAPI.Repository;
using DicomWebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DicomWebAPI.Controllers
{
    [Route("api/Image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        public ImageController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllImageAsync()
        {
            try
            {
                var imageList = await _imageRepository.GetAll();
                List<ImageDto> imageDtoList = _mapper.Map<List<ImageDto>>(imageList);
                _response.Result = imageDtoList;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }

        [HttpGet("{imageInstanceUID}",Name = "GetImage")]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> GetImageAsync(string imageInstanceUID)
        {
            try
            {
                var image = await _imageRepository.Get(item=>item.ImageInstanceUID == imageInstanceUID);
                var imageDto = _mapper.Map<ImageDto>(image);
                _response.Result = imageDto;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return _response;
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> CreateImageAsync([FromBody] ImageDto imageDto )
        {
            try
            {
                if (imageDto != null)
                {
                    Image image = _mapper.Map<Image>(imageDto);
                    await _imageRepository.Create(image);
                    _response.Result = _mapper.Map<ImageDto>(image);
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    return CreatedAtRoute("GetImage", new { imageInstanceUID = image.ImageInstanceUID }, _response);
                }

                return BadRequest(imageDto);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }

        [HttpDelete]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> DeleteImageAsync(string imageInstanceUID)
        {
            try
            {
                var image = await _imageRepository.Get(item=>item.ImageInstanceUID == imageInstanceUID);
                if (image != null)
                {
                     await _imageRepository.Delete(image);
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                    return Ok(_response);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }

        [HttpPut]
        public async Task<ActionResult<APIResponse>> UpdateImageAsync([FromBody] ImageDto imageDto)
        {
            try
            {
                if(imageDto != null)
                {
                    var image = _mapper.Map<Image>(imageDto);
                    await _imageRepository.Update(image);
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                    return Ok(_response);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }


        [HttpGet("GetAllImagesWithSeriesID/{seriesID}")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllImagesUnderSeriesID(string seriesID)
        {
            try
            {
                var items = await _imageRepository.GetAllImagesUnderSeries(item => item.SeriesInstanceUID.ToLower() == seriesID.ToLower());
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(items, options);
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
                _response.Result = jsonObject;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }

        [HttpGet("GetAllImagesWithStudyID/{studyID}")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllImagesWithStudyID(string studyID)
        {
            try
            {
                var items = await _imageRepository.GetAllImagesUnderSeries(item => item.Series.Study.StudyInstanceUID.ToLower() == studyID.ToLower());
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(items, options);
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
                _response.Result = jsonObject;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }


        [HttpGet("GetAllImagesWithPatientID/{patientID:int}")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllImagesWithPatientID(int patientID)
        {
            try
            {
                var items = await _imageRepository.GetAllImagesUnderSeries(item => item.Series.Study.Patient.PatientId == patientID);
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(items, options);
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
                _response.Result = jsonObject;
                //_response.Result = items;
                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return _response;
        }
    }
}
