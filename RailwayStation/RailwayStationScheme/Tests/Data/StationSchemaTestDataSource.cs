using RailwayStationSchema.Data;

namespace RailwayStationSchema.Tests.Data
{
    public class TestDataSource : IRailwayStationDataSource
    {
        readonly List<(double X, double Y, string Name)> _pointsStorage = new List<(double, double, string)>();
        readonly List<(int Start, int End, string Name)> _segmentsStorage = new List<(int, int, string)>();
        readonly List<(IEnumerable<int> Segments, string Name)> _pathsStorage = new List<(IEnumerable<int>, string)>();
        readonly List<(IEnumerable<int> Paths, string Name)> _parksStorage = new List<(IEnumerable<int>, string)>();
        readonly List<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> _schemaStorage =
            new List<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)>();

        public IEnumerable<(double X, double Y, string Name)> PointsStorage => _pointsStorage;

        public IEnumerable<(int Start, int End, string Name)> SegmentsStorage => _segmentsStorage;

        public IEnumerable<(IEnumerable<int> Segments, string Name)> PathsStorage => _pathsStorage;

        public IEnumerable<(IEnumerable<int> Paths, string Name)> ParksStorage => _parksStorage;

        public IEnumerable<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> SchemaStorage => _schemaStorage;


        public void AddPoint(double x, double y, string name) {
            _pointsStorage.Add((x, y, name));
        }
        public void AddSegment(int start, int end, string name) {
            _segmentsStorage.Add((start, end, name));
        }
        public void AddPath(IEnumerable<int> segments, string name) {
            _pathsStorage.Add((segments, name));
        }
        public void AddPark(IEnumerable<int> paths, string name) {
            _parksStorage.Add((paths, name));
        }
        public void AddStation(IEnumerable<int> points, IEnumerable<int> segments, IEnumerable<int> parks, string name) {
            _schemaStorage.Add((points, segments, parks, name));
        }
    }
}
