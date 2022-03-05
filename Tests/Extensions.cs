using Moq;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Helpers;
using TeenControlSystemWeb.Providers;
using TeenControlSystemWeb.Services;

namespace Tests;

public static class Extensions
{
    public static SessionProvider ConfigureSessionProvider(this Mock<IDataProvider> mock) => new(mock.Object);
    public static PasswordComparator ConfigurePasswordComparator(this Mock<IDataProvider> mock) => new(mock.Object);
    public static AuthorizationService ConfigureAuthorizationService(this Mock<IDataProvider> mock) => new(mock.Object);
}