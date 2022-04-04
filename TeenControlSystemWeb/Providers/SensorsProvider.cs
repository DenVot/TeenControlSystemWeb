using System.Text.RegularExpressions;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;

namespace TeenControlSystemWeb.Providers;

public class SensorsProvider
{
    private readonly IDataProvider _dataProvider;

    public SensorsProvider(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public async Task AddSensorAsync(string mac, string sensorName)
    {
        var allSensors = _dataProvider.SensorsRepository.GetAll().ToList();

        async Task ExecuteAddSensorAsync()
        {
            var regex = new Regex(
                "[\\d\\w][\\d\\w]((:[\\d\\w][\\d\\w]){5})");

            if (!regex.IsMatch(mac) || regex.Match(mac).Value != mac) throw new InvalidMacException();
            
            var sensor = new Sensor()
            {
                Mac = mac,
                Name = sensorName
            };

            await _dataProvider.SensorsRepository.AddAsync(sensor);
            await _dataProvider.SaveChangesAsync();
        }

        if (!allSensors.Any())
        {
            await ExecuteAddSensorAsync();
            
            return;
        }

        if (allSensors.Any(x => x.Mac == mac))
        {
            throw new SensorAlreadyExistsException();
        }

        await ExecuteAddSensorAsync();
    }

    public async Task EditSensorAsync(long id, string name)
    {
        var sensor = await _dataProvider.SensorsRepository.FindAsync(id);

        if (sensor == null) throw new SensorNotFoundException();

        sensor.Name = name;

        await _dataProvider.SaveChangesAsync();
    }

    public IEnumerable<Sensor> GetAllSensors() => _dataProvider.SensorsRepository.GetAll().ToArray();
}