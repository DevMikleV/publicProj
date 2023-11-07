using Microsoft.VisualStudio.TestTools.UnitTesting;
using RailwayStationSchema.Data;
using RailwayStationSchema.Tests.Data;
using Unity;

namespace RailwayStationSchema.Tests
{
    [TestClass]
    public class StationSchemeTests
    {
        TestDataSource _ds;
        IUnityContainer _uc;

        [TestInitialize]
        public void InitTest() {
            _ds = new TestDataSource();
            _uc = new UnityContainer();
            _uc.RegisterInstance<IRailwayStationDataSource>(_ds);
            _uc.RegisterType<IParkDataProvider, ParkDataProvider>();
        }

        [TestMethod]
        public async Task LoadPointsTest() {
            _ds.AddPoint(10, 10, "Точка 1");
            _ds.AddPoint(20, 20, "Точка 2");
            _ds.AddStation(
                new[] { 0, 1 },
                new int[0],
                new int[0],
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());

            var station = stations.First();
            Assert.IsNotNull(station);
            Assert.IsNotNull(station.Points);
            var stationPoints = station.Points.ToList();
            Assert.AreEqual(2, stationPoints.Count);

            Assert.AreEqual(10, stationPoints[0].Coordinates.X);
            Assert.AreEqual(10, stationPoints[0].Coordinates.Y);
            Assert.AreEqual("Точка 1", stationPoints[0].Name);

            Assert.AreEqual(20, stationPoints[1].Coordinates.X);
            Assert.AreEqual(20, stationPoints[1].Coordinates.Y);
            Assert.AreEqual("Точка 2", stationPoints[1].Name);
        }

        [TestMethod]
        public async Task LoadSegmentsTest() {
            _ds.AddPoint(10, 10, "Точка 1");
            _ds.AddPoint(20, 10, "Точка 2");
            _ds.AddPoint(30, 10, "Точка 3");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(1, 2, "Участок 2");
            _ds.AddStation(
                new[] { 0, 1, 2 },
                new[] { 0, 1 },
                new int[0],
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());

            var station = stations.First();
            Assert.IsNotNull(station);
            var stationPoints = station.Points.ToList();
            var stationSegments = station.Segments.ToList();
            Assert.AreEqual(2, stationSegments.Count);

            Assert.AreEqual(stationPoints[0], stationSegments[0].Start);
            Assert.AreEqual(stationPoints[1], stationSegments[0].End);
            Assert.AreEqual("Участок 1", stationSegments[0].Name);

            Assert.AreEqual(stationPoints[1], stationSegments[1].Start);
            Assert.AreEqual(stationPoints[2], stationSegments[1].End);
            Assert.AreEqual("Участок 2", stationSegments[1].Name);
        }

        [TestMethod]
        public async Task LoadSegments_InvalidSegment_Test() {
            _ds.AddPoint(10, 10, "Точка 1");
            _ds.AddPoint(20, 10, "Точка 2");
            _ds.AddPoint(30, 10, "Точка 3");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(1, 2, "Участок 2");
            _ds.AddStation(
                new[] { 0, 1 },
                new[] { 0, 1 },
                new int[0],
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());

            var station = stations.First();
            Assert.IsNotNull(station);
            var stationPoints = station.Points.ToList();
            var stationSegments = station.Segments.ToList();
            Assert.AreEqual(1, stationSegments.Count);

            Assert.AreEqual(stationPoints[0], stationSegments[0].Start);
            Assert.AreEqual(stationPoints[1], stationSegments[0].End);
            Assert.AreEqual("Участок 1", stationSegments[0].Name);
        }

        [TestMethod]
        public async Task LoadParksTest() {
            _ds.AddPoint(10, 10, "Точка 1");
            _ds.AddPoint(20, 10, "Точка 2");
            _ds.AddPoint(30, 10, "Точка 3");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(1, 2, "Участок 2");
            _ds.AddPath(new[] { 0 }, "Путь 1");
            _ds.AddPath(new[] { 1 }, "Путь 2");
            _ds.AddPark(new[] { 0 }, "Парк 1");
            _ds.AddPark(new[] { 1 }, "Парк 2");
            _ds.AddStation(
                new[] { 0, 1, 2 },
                new[] { 0, 1 },
                new[] { 0, 1 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());

            var station = stations.First();
            Assert.IsNotNull(station);
            var stationSegments = station.Segments.ToList();
            var stationParks = station.Parks.ToList();
            Assert.AreEqual(2, stationParks.Count);

            var park1 = stationParks[0];
            Assert.AreEqual(park1.Name, "Парк 1");
            var park1Paths = park1.Paths.ToList();
            Assert.AreEqual(1, park1Paths.Count);
            Assert.AreEqual(1, park1Paths[0].Segments.Count());
            Assert.AreEqual(stationSegments[0], park1Paths[0].Segments.First());

            var park2 = stationParks[1];
            Assert.AreEqual(park2.Name, "Парк 2");
            var park2Paths = park2.Paths.ToList();
            Assert.AreEqual(1, park2Paths.Count);
            Assert.AreEqual(1, park2Paths[0].Segments.Count());
            Assert.AreEqual(stationSegments[1], park2Paths[0].Segments.First());
        }

        [TestMethod]
        public async Task LoadParks_InvalidPath_Test() {
            _ds.AddPoint(10, 10, "Точка 1");
            _ds.AddPoint(20, 10, "Точка 2");
            _ds.AddPoint(30, 10, "Точка 3");
            _ds.AddPoint(40, 10, "Точка 4");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(2, 3, "Участок 2");
            _ds.AddPath(new[] { 0, 1 }, "Путь 1");
            _ds.AddPark(new[] { 0 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1, 2, 3 },
                new[] { 0, 1 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            Assert.AreEqual(1, stations.Count());

            var station = stations.First();
            Assert.IsNotNull(station);
            var stationSegments = station.Segments.ToList();
            var stationParks = station.Parks.ToList();
            Assert.AreEqual(1, stationParks.Count);

            var park1Paths = stationParks[0].Paths.ToList();
            Assert.AreEqual(1, park1Paths.Count);
            Assert.AreEqual(1, park1Paths[0].Segments.Count());
            Assert.AreEqual(stationSegments[0], park1Paths[0].Segments.First());
        }
    }
}
