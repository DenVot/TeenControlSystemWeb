using Moq;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Providers;

namespace Tests;

public static class Extensions
{
    public static void InitializeMock(this Mock<IDataProvider> mock)
    {
        mock.Setup(x => x.PointsRepository);
        mock.Setup(x => x.SensorsRepository);
        mock.Setup(x => x.SessionsRepository);
        mock.Setup(x => x.UsersRepository);
        mock.Setup(x => x.UserAuthorizationTokensRepository);
    }
    
    public static SessionProvider ConfigureSessionProvider(this Mock<IDataProvider> mock) => new(mock.Object);
}