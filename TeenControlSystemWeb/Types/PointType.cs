namespace TeenControlSystemWeb.Types;

public class PointType
{
    public PointType(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    public double Longitude { get; }
    public double Latitude { get; }
}