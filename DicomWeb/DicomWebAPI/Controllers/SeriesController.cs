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
    [Route("api/Series")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        public SeriesController(ISeriesRepository seriesRepository, IMapper mapper)
        {
            _seriesRepository = seriesRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllSeriesAsync()
        {
            try
            {
                var seriesList = await _seriesRepository.GetAll();
                List<SeriesDto> seriesDtoList = _mapper.Map<List<SeriesDto>>(seriesList);
                _response.Result = seriesDtoList;
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

        [HttpGet("GetAllSeriesWithStudyID/{studyID}")]
        ////[Authorize]
        public async Task<ActionResult<APIResponse>> GetAllSeriesUnderStudyID(string studyID)
        {
            try
            {
                var series = await _seriesRepository.GetAllSeriesUnderStudy(item => item.StudyInstanceUID.ToLower() == studyID.ToLower());
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                string json = JsonSerializer.Serialize(series, options);
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
                _response.Result = jsonObject;
               // _response.Result = series;
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


        [HttpGet("GetSeries/{seriesInstanceUID}", Name = "GetSeries")]
        //[Authorize]
        public async Task<ActionResult<APIResponse>> GetSeriesAsync(string seriesInstanceUID)
        {
            try
            {
                var series = await _seriesRepository.Get(item=>item.SeriesInstanceUID == seriesInstanceUID);
                var seriesDto = _mapper.Map<SeriesDto>(series);
                _response.Result = seriesDto;
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
        public async Task<ActionResult<APIResponse>> CreateSeriesAsync([FromBody] SeriesDto seriesDto )
        {
            try
            {
                if (seriesDto != null)
                {
                    Series series = _mapper.Map<Series>(seriesDto);
                    await _seriesRepository.Create(series);
                    _response.Result = _mapper.Map<SeriesDto>(series);
                    _response.IsSuccess = true;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    return CreatedAtRoute("GetSeries", new { seriesInstanceUID = series.SeriesInstanceUID }, _response);
                }

                return BadRequest(seriesDto);
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
        public async Task<ActionResult<APIResponse>> DeleteSeriesAsync(string seriesInstanceUID)
        {
            try
            {
                var series = await _seriesRepository.Get(item=>item.SeriesInstanceUID == seriesInstanceUID);
                if (series != null)
                {
                     await _seriesRepository.Delete(series);
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
        public async Task<ActionResult<APIResponse>> UpdateSeriesAsync([FromBody] SeriesDto seriesDto)
        {
            try
            {
                if(seriesDto != null)
                {
                    var series = _mapper.Map<Series>(seriesDto);
                    await _seriesRepository.Update(series);
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
