using AutoMapper;
using Azure;
using DicomWebAPI.Model;
using DicomWebAPI.Model.DTO;
using DicomWebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DicomWebAPI.Controllers
{
    [Route("api/Patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        //private readonly IStudyRepository _studyRepository;
        //private readonly ISeriesRepository _seriesRepository;
        //private readonly ISeriesRepository _seriesRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        public PatientController(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllPatientAsync()
        {
            try
            {
                var patientList = await _patientRepository.GetAll();
                List<PatientDto> patientDtoList = _mapper.Map<List<PatientDto>>(patientList);
                _response.Result = patientDtoList;
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


        [HttpGet("GetPatientDetails/{patientId:int}")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllPatientDetailsAsync(int patientId )
        {
            try
            {
                var patient = await _patientRepository.GetAllPatientDetails(p=>p.PatientId == patientId);
                PatientDto patientDto = _mapper.Map<PatientDto>(patient);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(patient, options);
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

        [HttpGet("{id:int}",Name = "GetPatient")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetPatientAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.Get(item=>item.PatientId == id );
                var patientDto = _mapper.Map<PatientDto>(patient);
                _response.Result = patientDto;
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
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> CreatePatientAsync([FromBody] PatientDto patientDto )
        {
            try
            {
                if (patientDto != null)
                {
                    Patient patient = _mapper.Map<Patient>(patientDto);
                    await _patientRepository.Create(patient);
                    _response.Result = _mapper.Map<PatientDto>(patient);
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    return CreatedAtRoute("GetPatient", new { id = patient.PatientId }, _response);
                }

                return BadRequest(patientDto);
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
        ////[Authorize(Roles = "custom")]
        public async Task<ActionResult<APIResponse>> DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.Get(item=>item.PatientId == id);
                if (patient != null)
                {
                     await _patientRepository.Delete(patient);
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
        public async Task<ActionResult<APIResponse>> UpdatePatientAsync([FromBody] PatientDto patientDto)
        {
            try
            {
                if( patientDto != null)
                {
                    var patient = _mapper.Map<Patient>(patientDto);
                    await _patientRepository.Update(patient);
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
