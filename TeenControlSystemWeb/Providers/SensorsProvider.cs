using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<Sensor> AddSensorAsync(string mac, string sensorName, int? order)
    {
        var allSensors = _dataProvider.SensorsRepository.GetAll().ToList();

        async Task<Sensor> ExecuteAddSensorAsync()
        {
            var regex = new Regex(
                "[\\dA-F][\\dA-F]((:[\\dA-F][\\dA-F]){5})");

            if (!regex.IsMatch(mac) || regex.Match(mac).Value != mac) throw new InvalidMacException();

            if (order == null) //Присвоить номер автоматически
            {
                if (allSensors.Count == 0) order = 1; //Нет маячков в бд
                else order = allSensors.Select(x => x.Order).Max() + 1; //Иначе присваиваем макс номер + 1
            }
            
            var sensor = new Sensor()
            {
                Mac = mac,
                Name = sensorName,
                Order = order.Value
            };

            await _dataProvider.SensorsRepository.AddAsync(sensor);
            await _dataProvider.SaveChangesAsync();

            return sensor;
        }

        if (!allSensors.Any())
        {
            return await ExecuteAddSensorAsync();
        }

        if (allSensors.Any(x => x.Mac == mac)) throw new SensorAlreadyExistsException();
        if (allSensors.Any(x => x.Order == order)) throw new InvalidOrderException();
        
        return await ExecuteAddSensorAsync();
    }
    
    public async Task EditSensorAsync(long id, string? name, int? order)
    {
        var sensor = await _dataProvider.SensorsRepository.FindAsync(id);

        if (sensor == null) throw new SensorNotFoundException();
        if(name != null) sensor.Name = name;
        if (order != null && _dataProvider.SensorsRepository.GetAll().All(x => x.Order != order)) sensor.Order = order.Value;

        await _dataProvider.SaveChangesAsync();
    }

    public async Task DeleteSensorAsync(long id)
    {
        _dataProvider.SensorsRepository.Remove(new Sensor()
        {
            Id = id
        });

        await _dataProvider.SaveChangesAsync();
    }

    public IEnumerable<Sensor> GetAllSensors() => _dataProvider.SensorsRepository.GetAll().ToArray();
}