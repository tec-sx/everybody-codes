using EverybodyCodes.Application.Contracts;
using EverybodyCodes.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace EverybodyCodes.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly ICameraService _cameraService;
        private readonly ILogger<CameraController> _logger;

        public CameraController(ILogger<CameraController> logger, ICameraService cameraService)
        {
            _logger = logger;
            _cameraService = cameraService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CameraDto> cameraDtos = await _cameraService.GetAllCamerasAsync();

            return Ok(cameraDtos);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCameras([FromQuery] string searchTerm)
        {
            IEnumerable<CameraDto> cameraDtos = await _cameraService.SearchByNameAsync(searchTerm);

            return Ok(cameraDtos);
        }

        [HttpGet("number")]
        public async Task<IActionResult> GetCameraByNumber([FromQuery] int number)
        {
            CameraDto cameraDtos = await _cameraService.GetCameraByNumberAsync(number);

            return Ok(cameraDtos);
        }
    }
}
