using System;
using System.Threading.Tasks;
using Moq;
using TeenControlSystemWeb.Data.Models;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.Sensor;
using TeenControlSystemWeb.Providers;
using Xunit;

namespace Tests.SensorsTests;

public class SensorsAddingTests
{
    [Fact]
    public async Task AddSensor_Must_Add()
    {
        const string testMac = "50:46:5D:6E:8C:20";
        const string testName = "Денис";
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(ArraySegment<Sensor>.Empty);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await sensorsProvider.AddSensorAsync(testMac, testName);
    }

    [Theory]
    [InlineData("50:46:5D:6E:8:20")] //Invalid byte
    [InlineData("50:46:5D:6E:8C:20:20")] //Invalid length
    [InlineData("")] //No mac
    public async Task AddSensor_Incorrect_Mac(string mac)
    {
        const string testName = "Денис";
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(ArraySegment<Sensor>.Empty);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await Assert.ThrowsAsync<InvalidMacException>(() => sensorsProvider.AddSensorAsync(mac, testName));
    }
    
    [Fact]
    public async Task AddSensor_Already_Exists()
    {
        const string testMac = "50:46:5D:6E:8C:20";
        const string testName = "Денис";
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(new []
        {
            new Sensor()
            {
                Mac = testMac
            }
        });

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await Assert.ThrowsAsync<SensorAlreadyExistsException>(() => sensorsProvider.AddSensorAsync(testMac, testName));
    }
}