using RailwayStationPathSearch.Data;
using RailwayStationPathSearch.PathSearch;
using RailwayStationSchema.Data;
using System.Text;
using Unity;

IUnityContainer uc = new UnityContainer();
uc.RegisterType<IRailwayStationDataSource, PathSearchDataSource>();
uc.RegisterType<IParkDataProvider, ParkDataProvider>();
uc.RegisterType<IStationPathFinder, ShortestPathFinder>();

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Поиск кратчайшего пути между участками станции");
Console.WriteLine();

var station = await LoadParkData();
if (station == null)
    return;
var pathStartEnd = GetPathStartEnd(station);
FindShortestPath(pathStartEnd.Start, pathStartEnd.End, station);

async Task<StationSchemaData> LoadParkData() {
    if (uc.IsRegistered<IParkDataProvider>()) {
        IParkDataProvider dp = uc.Resolve<IParkDataProvider>();
        return (await dp.LoadStations()).First();
    }
    return null;
}

(SegmentData Start, SegmentData End) GetPathStartEnd(StationSchemaData station) {
    List<SegmentData> allSegments = station.Segments.ToList();
    for (int i = 0; i < allSegments.Count; i++)
        Console.WriteLine($"{i + 1}.{allSegments[i].Name}");
    int startIndex = -1, endIndex = -1;
    do {
        if (startIndex < 0) {
            Console.Write($"Введите индекс начального участка (1-{allSegments.Count}): ");
            int.TryParse(Console.ReadLine(), out startIndex);
            if (startIndex <= 0 || startIndex > allSegments.Count) {
                startIndex = -1;
                continue;
            }
        }
        if (endIndex < 0) {
            Console.Write($"Введите индекс конечного участка (1-{allSegments.Count}): ");
            int.TryParse(Console.ReadLine(), out endIndex);
            if (endIndex <= 0 || endIndex > allSegments.Count)
                endIndex = -1;
        }
    } while (startIndex < 0 || endIndex < 0);
    return (allSegments[startIndex - 1], allSegments[endIndex - 1]);
}

void FindShortestPath(SegmentData start, SegmentData end, StationSchemaData station) {
    if (uc.IsRegistered<IStationPathFinder>()) {
        Console.WriteLine();
        IStationPathFinder pathFinder = uc.Resolve<IStationPathFinder>();
        var path = pathFinder.FingShortestPath(start, end, station);
        if (path != null) {
            Console.WriteLine($"Кратчайший путь между участками {start.Name} и {end.Name}:");
            Console.WriteLine(string.Join(", ", path.Select(p => p.Name)));
        }
        else {
            Console.WriteLine($"Пути между участками {start.Name} и {end.Name} не существует");
        }
    }
}
