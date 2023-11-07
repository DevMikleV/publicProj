using RailwayStationSchema.Data;

namespace RailwayStationPathSearch.Data
{
    internal class PathSearchDataSource : IRailwayStationDataSource
    {
        readonly List<(double X, double Y, string Name)> _pointsStorage = new List<(double, double, string)>()
        {
            (10, 10, "Точка 0"),
            (20, 10, "Точка 1"),

            (20, 20, "Точка 2"),
            (10, 20, "Точка 3"),
            (10, 30, "Точка 4"),
            (40, 30, "Точка 5"),
            (40, 20, "Точка 6"),
            (30, 20, "Точка 7"),

            (30, 10, "Точка 8"),
            (40, 10, "Точка 9"),

            (35, 15, "Точка 10"),
            (45, 15, "Точка 11"),

            (20, 5, "Точка 12"),
            (40, 5, "Точка 13"),
        };
        readonly List<(int Start, int End, string Name)> _segmentsStorage = new List<(int, int, string)>()
        {
            (0, 1, "Участок 1"),
            (1, 2, "Участок 2"),
            (2, 3, "Участок 3"),
            (3, 4, "Участок 4"),
            (4, 5, "Участок 5"),
            (5, 6, "Участок 6"),
            (6, 7, "Участок 7"),
            (7, 8, "Участок 8"),
            (8, 9, "Участок 9"),
            (8, 10, "Участок 10"),
            (10, 11, "Участок 11"),
            (1, 12, "Участок 12"),
            (12, 13, "Участок 13"),
            (13, 9, "Участок 14"),
        };
        readonly List<(IEnumerable<int> Segments, string Name)> _pathsStorage =
            new List<(IEnumerable<int>, string)>()
            {
                (new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13  }, "Путь 1")
            };
        readonly List<(IEnumerable<int> Paths, string Name)> _parksStorage =
            new List<(IEnumerable<int>, string)>()
            {
                (new[] { 0 }, "Парк 1")
            };
        readonly List<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> _schemaStorage =
            new List<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)>()
            {
                (new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 ,9, 10, 11, 12, 13 },
                new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
                new[] { 0 },
                "Станция 1")
            };

        public IEnumerable<(double X, double Y, string Name)> PointsStorage => _pointsStorage;

        public IEnumerable<(int Start, int End, string Name)> SegmentsStorage => _segmentsStorage;

        public IEnumerable<(IEnumerable<int> Segments, string Name)> PathsStorage => _pathsStorage;

        public IEnumerable<(IEnumerable<int> Paths, string Name)> ParksStorage => _parksStorage;

        public IEnumerable<(IEnumerable<int> Points, IEnumerable<int> Segments, IEnumerable<int> Parks, string Name)> SchemaStorage => _schemaStorage;
    }
}
