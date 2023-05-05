using AutoMapper;
using Azure;
using DicomWebAPI.Model;
using DicomWebAPI.Model.DTO;
using DicomWebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DicomWebAPI.Controllers
{
    [Route("api/Study")]
    [ApiController]
    public class StudyController : ControllerBase
    {
        private readonly IStudyRepository _studyRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        public StudyController(IStudyRepository studyRepository, IMapper mapper)
        {
            _studyRepository = studyRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllStudiesAsync()
        {
            try
            {
                var studyList = await _studyRepository.GetAll();
                List<StudyDto> studyDtoList = _mapper.Map<List<StudyDto>>(studyList);
                _response.Result = studyDtoList;
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

        [HttpGet("{studyInstanceUID}", Name = "GetStudy")]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> GetStudyAsync(string studyInstanceUID)
        {
            try
            {
                var study = await _studyRepository.Get(item => item.StudyInstanceUID == studyInstanceUID);
                var studyDto = _mapper.Map<StudyDto>(study);
                _response.Result = studyDto;
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

        [HttpGet("GetAllStudyWithPatientID/{patientID:int}")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllStudyWithPatientID(int patientID)
        {
            try
            {
                var studies = await _studyRepository.GetAllStudyUnderPatient(item => item.PatientId == patientID);
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(studies, options);
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

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> CreateStudyAsync([FromBody] StudyDto studyDto )
        {
            try
            {
                if (studyDto != null)
                {
                    Study study = _mapper.Map<Study>(studyDto);
                    await _studyRepository.Create(study);
                    _response.Result = _mapper.Map<StudyDto>(study);
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    return CreatedAtRoute("GetStudy", new { studyInstanceUID = study.StudyInstanceUID }, _response);
                }

                return BadRequest(studyDto);
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
        public async Task<ActionResult<APIResponse>> DeleteStudyAsync(string studyInstanceUID)
        {
            try
            {
                var study = await _studyRepository.Get(item=>item.StudyInstanceUID == studyInstanceUID);
                if (study != null)
                {
                     await _studyRepository.Delete(study);
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
        public async Task<ActionResult<APIResponse>> UpdateStudyAsync([FromBody] StudyDto studyDto)
        {
            try
            {
                if(studyDto != null)
                {
                    var study = _mapper.Map<Study>(studyDto);
                    await _studyRepository.Update(study);
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
    }
}
