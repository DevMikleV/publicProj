namespace RailwayStationSchema.Data
{
    public interface IRailwayStationDataSource
    {
        IEnumerable<(double X, double Y, string Name)> PointsStorage { get; }
        IEnumerable<(int Start, int End, string Name)> SegmentsStorage { get; }
        IEnumerable<(IEnumerable<int> Segments, string Name)> PathsStorage { get; }
        IEnumerable<(IEnumerable<int> Paths, string Name)> ParksStorage { get; }
        IEnumerable<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> SchemaStorage { get; }
    }
}