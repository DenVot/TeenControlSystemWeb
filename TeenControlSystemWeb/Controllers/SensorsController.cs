using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Providers;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/api/sensors")]
public class SensorsController : ControllerBase
{
    private readonly SensorsProvider _sensorsProvider;

    public SensorsController(SensorsProvider sensorsProvider)
    {
        _sensorsProvider = sensorsProvider;
    }

    [HttpPost("add-sensor")]
    public async Task<ActionResult> AddSensor([FromBody] AddSensorType addSensorType)
    {
        try
        {
            var sensor =
                await _sensorsProvider.AddSensorAsync(addSensorType.Mac, addSensorType.Name, addSensorType.Order);

            return Ok(sensor.ConvertToApiType(false));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
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
            await _sensorsProvider.EditSensorAsync(editSensorNameType.SensorId, editSensorNameType.NewName, null);
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("edit-sensor-order")]
    public async Task<ActionResult> EditSensorOrder([FromBody] EditSensorOrderType editSensorOrderType)
    {
        try
        {
            await _sensorsProvider.EditSensorAsync(editSensorOrderType.SensorId, null, editSensorOrderType.Order);
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("remove-sensor")]
    public async Task<ActionResult> RemoveSensor([FromQuery] long id)
    {
        try
        {
            await _sensorsProvider.DeleteSensorAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}