using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Providers;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/sensors")]
public class SensorsController : ControllerBase
{
    private readonly SensorsProvider _sensorsProvider;

    public SensorsController(SensorsProvider sensorsProvider)
    {
        _sensorsProvider = sensorsProvider;
    }

    [HttpGet("get-all-sensors")]
    public ActionResult GetAllSensors()
    {
        var allSensors = _sensorsProvider.GetAllSensors();

        return Ok(from sensor in allSensors
            select sensor.ConvertToApiType());
    }

    [HttpPut("edit-sensor-name")]
    public async Task<ActionResult> EditSensorName([FromBody] EditSensorNameType editSensorNameType)
    {
        try
        {
            await _sensorsProvider.EditSensorAsync(editSensorNameType.SensorId, editSensorNameType.NewName);
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}