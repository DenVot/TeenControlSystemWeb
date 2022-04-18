using System;
using System.Collections.Generic;
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
        const int testOrder = 1;
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(ArraySegment<Sensor>.Empty);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await sensorsProvider.AddSensorAsync(testMac, testName, testOrder);
    }

    
    [Fact]
    public async Task AddSensor_Set_Default_Order_When_Empty()
    {
        const string testMac = "50:46:5D:6E:8C:20";
        const string testName = "Денис";
        int? testOrder = null;
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(ArraySegment<Sensor>.Empty);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        var sensor = await sensorsProvider.AddSensorAsync(testMac, testName, testOrder);
        
        Assert.Equal(1, sensor.Order);
    }

    [Fact]
    public async Task AddSensor_Set_Default_Order_When_Some_Sensor_Exists_In_Repository()
    {
        const string testMac = "50:46:5D:6E:8C:20";
        const string testName = "Денис";
        int? testOrder = null;
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(new []
        {
            new Sensor()
            {
                Id = 0,
                Order = 5,
                Mac = "50:46:5D:6E:8C:21"
            }
        });

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        var sensor = await sensorsProvider.AddSensorAsync(testMac, testName, testOrder);
        
        Assert.Equal(6, sensor.Order);
    }
    
    [Theory]
    [InlineData("50:46:5D:6E:8:20")] //Invalid byte
    [InlineData("50:46:5D:6E:8C:20:20")] //Invalid length
    [InlineData("50:46:5D:6E:8CC:20:20")] //Invalid length
    [InlineData("")] //No mac
    public async Task AddSensor_Incorrect_Mac(string mac)
    {
        const string testName = "Денис";
        const int testOrder = 1;
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(ArraySegment<Sensor>.Empty);

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await Assert.ThrowsAsync<InvalidMacException>(() => sensorsProvider.AddSensorAsync(mac, testName, testOrder));
    }
    
    [Fact]
    public async Task AddSensor_Already_Exists()
    {
        const string testMac = "50:46:5D:6E:8C:20";
        const string testName = "Денис";
        const int testOrder = 1;
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(new []
        {
            new Sensor()
            {
                Mac = testMac
            }
        });

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await Assert.ThrowsAsync<SensorAlreadyExistsException>(() => sensorsProvider.AddSensorAsync(testMac, testName, testOrder));
    }
    
    [Fact]
    public async Task AddSensor_Must_Throw_Invalid_Order_Exception()
    {
        const string testMac = "50:46:5D:6E:8C:20";
        const string testName = "Денис";
        const int testOrder = 1;
        
        var dataProviderMock = new Mock<IDataProvider>();

        dataProviderMock.Setup(x => x.SensorsRepository.GetAll()).Returns(new []
        {
            new Sensor()
            {
                Mac = "50:46:5D:6E:8C:21",
                Order = testOrder
            }
        });

        var sensorsProvider = new SensorsProvider(dataProviderMock.Object);

        await Assert.ThrowsAsync<InvalidOrderException>(() => sensorsProvider.AddSensorAsync(testMac, testName, testOrder));
    }
}