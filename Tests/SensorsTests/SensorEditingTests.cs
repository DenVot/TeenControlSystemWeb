using System;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.SensorsTests;

public class SensorEditingTests
{
    [Fact]
    public async Task EditSensorName_Must_Edit()
    {
        var sensor = new Sensor()
        {
            Id = 0,
            Name = "Test"
        };
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync(sensor);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await sensorsProvider.EditSensorAsync(0L, "test", null);
        
        Assert.Equal("test", sensor.Name);
    }
    
    [Fact]
    public async Task EditSensorName_Must_Throw_Sensor_Not_Found()
    {
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.FindAsync(0L)).ReturnsAsync((Sensor?)null);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);
        
        await Assert.ThrowsAsync<SensorNotFoundException>(() => sensorsProvider.EditSensorAsync(0L, "test", null));
    }
}