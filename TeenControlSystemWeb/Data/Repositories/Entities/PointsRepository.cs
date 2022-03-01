namespace TeenControlSystemWeb.Data.Repositories.Entities;

public class PointsRepository : Repository<Point>
{
    public PointsRepository(TcsDatabaseContext context) : base(context)
    {
    }
}