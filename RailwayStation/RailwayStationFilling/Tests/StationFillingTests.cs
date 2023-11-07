using Microsoft.VisualStudio.TestTools.UnitTesting;
using RailwayStationFilling.StationProcessing;
using RailwayStationSchema.Data;
using RailwayStationSchema.Tests.Data;
using Unity;

namespace RailwayStationFilling.Tests
{
    [TestClass]
    public class StationFillingTests
    {
        TestDataSource _ds;
        IUnityContainer _uc;

        [TestInitialize]
        public void InitTest() {
            _ds = new TestDataSource();
            _uc = new UnityContainer();
            _uc.RegisterInstance<IRailwayStationDataSource>(_ds);
            _uc.RegisterType<IParkDataProvider, ParkDataProvider>();
            _uc.RegisterType<IStationProcessor, StationParkFilling>();
        }

        [TestMethod]
        public async Task SinglePath() {
            _ds.AddPoint(10, 10, "Точка 1");
            _ds.AddPoint(20, 10, "Точка 2");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddPath(new[] { 0 }, "Путь 1");
            _ds.AddPark(new[] { 0 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1 },
                new[] { 0 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            var station = stations.First();
            Assert.AreEqual(1, station.Parks.Count());

            IStationProcessor stationProcessor = _uc.Resolve<IStationProcessor>();
            var parkPoints = stationProcessor.GetParkFillPoints(station.Parks.First()).ToList();

            Assert.IsNotNull(parkPoints);
            Assert.AreEqual(2, parkPoints.Count);

            Assert.AreEqual("Точка 2", parkPoints[0].Name);
            Assert.AreEqual("Точка 1", parkPoints[1].Name);
        }

        [TestMethod]
        public async Task TwoPaths() {
            _ds.AddPoint(10, 30, "Точка 1");
            _ds.AddPoint(40, 30, "Точка 2");
            _ds.AddPoint(10, 10, "Точка 3");
            _ds.AddPoint(40, 10, "Точка 4");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(2, 3, "Участок 2");
            _ds.AddPath(new[] { 0 }, "Путь 1");
            _ds.AddPath(new[] { 1 }, "Путь 2");
            _ds.AddPark(new[] { 0, 1 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1, 2, 3 },
                new[] { 0, 1 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            var station = stations.First();
            Assert.AreEqual(1, station.Parks.Count());

            IStationProcessor stationProcessor = _uc.Resolve<IStationProcessor>();
            var parkPoints = stationProcessor.GetParkFillPoints(station.Parks.First()).ToList();

            Assert.IsNotNull(parkPoints);
            Assert.AreEqual(4, parkPoints.Count);

            Assert.AreEqual("Точка 4", parkPoints[0].Name);
            Assert.AreEqual("Точка 2", parkPoints[1].Name);
            Assert.AreEqual("Точка 1", parkPoints[2].Name);
            Assert.AreEqual("Точка 3", parkPoints[3].Name);
        }

        [TestMethod]
        public async Task InnerPath_Exclude() {
            _ds.AddPoint(10, 30, "Точка 1");
            _ds.AddPoint(40, 30, "Точка 2");
            _ds.AddPoint(10, 10, "Точка 3");
            _ds.AddPoint(40, 10, "Точка 4");
            _ds.AddPoint(20, 20, "Точка 5");
            _ds.AddPoint(30, 20, "Точка 6");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(2, 3, "Участок 2");
            _ds.AddSegment(4, 5, "Участок 3");
            _ds.AddPath(new[] { 0 }, "Путь 1");
            _ds.AddPath(new[] { 1 }, "Путь 2");
            _ds.AddPath(new[] { 2 }, "Путь 3");
            _ds.AddPark(new[] { 0, 1, 2 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1, 2, 3, 4, 5 },
                new[] { 0, 1, 2 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            var station = stations.First();
            Assert.AreEqual(1, station.Parks.Count());

            IStationProcessor stationProcessor = _uc.Resolve<IStationProcessor>();
            var parkPoints = stationProcessor.GetParkFillPoints(station.Parks.First()).ToList();

            Assert.IsNotNull(parkPoints);
            Assert.AreEqual(4, parkPoints.Count);

            Assert.AreEqual("Точка 4", parkPoints[0].Name);
            Assert.AreEqual("Точка 2", parkPoints[1].Name);
            Assert.AreEqual("Точка 1", parkPoints[2].Name);
            Assert.AreEqual("Точка 3", parkPoints[3].Name);
        }

        [TestMethod]
        public async Task InnerPath_Include() {
            _ds.AddPoint(10, 30, "Точка 1");
            _ds.AddPoint(40, 30, "Точка 2");
            _ds.AddPoint(10, 10, "Точка 3");
            _ds.AddPoint(40, 10, "Точка 4");
            _ds.AddPoint(5, 20, "Точка 5");
            _ds.AddPoint(45, 20, "Точка 6");
            _ds.AddSegment(0, 1, "Участок 1");
            _ds.AddSegment(2, 3, "Участок 2");
            _ds.AddSegment(4, 5, "Участок 3");
            _ds.AddPath(new[] { 0 }, "Путь 1");
            _ds.AddPath(new[] { 1 }, "Путь 2");
            _ds.AddPath(new[] { 2 }, "Путь 3");
            _ds.AddPark(new[] { 0, 1, 2 }, "Парк 1");
            _ds.AddStation(
                new[] { 0, 1, 2, 3, 4, 5 },
                new[] { 0, 1, 2 },
                new[] { 0 },
                "Станция 1");

            var dp = _uc.Resolve<IParkDataProvider>();
            Assert.IsNotNull(dp);

            var stations = await dp.LoadStations();
            Assert.IsNotNull(stations);
            var station = stations.First();
            Assert.AreEqual(1, station.Parks.Count());

            IStationProcessor stationProcessor = _uc.Resolve<IStationProcessor>();
            var parkPoints = stationProcessor.GetParkFillPoints(station.Parks.First()).ToList();

            Assert.IsNotNull(parkPoints);
            Assert.AreEqual(6, parkPoints.Count);

            Assert.AreEqual("Точка 4", parkPoints[0].Name);
            Assert.AreEqual("Точка 6", parkPoints[1].Name);
            Assert.AreEqual("Точка 2", parkPoints[2].Name);
            Assert.AreEqual("Точка 1", parkPoints[3].Name);
            Assert.AreEqual("Точка 5", parkPoints[4].Name);
            Assert.AreEqual("Точка 3", parkPoints[5].Name);
        }
    }
}
