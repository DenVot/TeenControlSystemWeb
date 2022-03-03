using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Helpers;

namespace TeenControlSystemWeb.Providers;

public class AuthorizationProvider
{
    private readonly IDataProvider _dataProvider;
    private readonly PasswordComparator _passwordComparator;

    public AuthorizationProvider(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
        _passwordComparator = new PasswordComparator(dataProvider);
    }
}