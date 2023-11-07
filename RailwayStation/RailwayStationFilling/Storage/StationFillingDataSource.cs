using RailwayStationSchema.Data;

namespace RailwayStationFilling.Data
{
    internal class StationFillingDataSource : IRailwayStationDataSource
    {
        readonly List<(double X, double Y, string Name)> _pointsStorage = new List<(double, double, string)>()
        {
            (10, 10, "Точка 1"),
            (40, 10, "Точка 2"),
            (20, 20, "Точка 3"),
            (30, 20, "Точка 4"),
            (10, 30, "Точка 5"),
            (40, 30, "Точка 6"),
            (60, 10, "Точка 7"),
            (70, 10, "Точка 8"),
            (50, 20, "Точка 9"),
            (80, 20, "Точка 10"),
            (60, 30, "Точка 11"),
            (70, 30, "Точка 12"),
        };
        readonly List<(int Start, int End, string Name)> _segmentsStorage = new List<(int, int, string)>()
        {
            (0, 1, "Участок 1"),
            (2, 3, "Участок 2"),
            (4, 5, "Участок 3"),
            (6, 7, "Участок 4"),
            (8, 9, "Участок 5"),
            (10, 11, "Участок 6"),
        };
        readonly List<(IEnumerable<int> Segments, string Name)> _pathsStorage =
            new List<(IEnumerable<int>, string)>()
            {
                (new[] { 0 }, "Путь 1"),
                (new[] { 1 }, "Путь 2"),
                (new[] { 2 }, "Путь 3"),
                (new[] { 3 }, "Путь 4"),
                (new[] { 4 }, "Путь 5"),
                (new[] { 5 }, "Путь 6"),
            };
        readonly List<(IEnumerable<int> Paths, string Name)> _parksStorage =
            new List<(IEnumerable<int>, string)>()
            {
                (new[] { 0, 1, 2 }, "Парк 1"),
                (new[] { 3, 4, 5 }, "Парк 2"),
            };
        readonly List<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> _schemaStorage =
            new List<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)>()
            {
                (new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 },
                new[] { 0, 1, 2, 3, 4, 5 },
                new[] { 0, 1 },
                "Станция 1")
            };

        public IEnumerable<(double X, double Y, string Name)> PointsStorage => _pointsStorage;

        public IEnumerable<(int Start, int End, string Name)> SegmentsStorage => _segmentsStorage;

        public IEnumerable<(IEnumerable<int> Segments, string Name)> PathsStorage => _pathsStorage;

        public IEnumerable<(IEnumerable<int> Paths, string Name)> ParksStorage => _parksStorage;

        public IEnumerable<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> SchemaStorage => _schemaStorage;
    }
}
