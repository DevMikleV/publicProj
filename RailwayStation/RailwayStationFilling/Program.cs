using RailwayStationFilling.Data;
using RailwayStationFilling.StationProcessing;
using RailwayStationSchema.Data;
using System.Text;
using Unity;

IUnityContainer uc = new UnityContainer();
uc.RegisterType<IRailwayStationDataSource, StationFillingDataSource>();
uc.RegisterType<IParkDataProvider, ParkDataProvider>();
uc.RegisterType<IStationProcessor, StationParkFilling>();

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Заливка станции");
Console.WriteLine();

var allStations = await LoadParkData();
FillStations(allStations);

async Task<IEnumerable<StationSchemaData>> LoadParkData() {
    if (uc.IsRegistered<IParkDataProvider>()) {
        IParkDataProvider dp = uc.Resolve<IParkDataProvider>();
        return await dp.LoadStations();
    }
    return null;
}

void FillStations(IEnumerable<StationSchemaData> stations) {
    if (stations != null && uc.IsRegistered<IStationProcessor>()) {
        Console.WriteLine("Результат заливки:");
        IStationProcessor stationProcessor = uc.Resolve<IStationProcessor>();
        foreach (var station in stations) {
            foreach (var park in station.Parks) {
                var parkPoints = stationProcessor.GetParkFillPoints(park);
                Console.WriteLine($"Парк {park.Name}: {string.Join(", ", parkPoints.Select(p => p.Name))}");
            }
        }
    }
}
