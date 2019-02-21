using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Web_API.Controllers;
using Web_API.Models;

namespace WebAPI.Tests
{
    /// <summary>
    /// Description résumée pour ActivitiesControllerTest
    /// </summary>
    [TestClass]
    public class ActivitiesControllerTest
    {
        public ActivitiesControllerTest()
        {
            _controller = new ActivitiesController();
        }

        private TestContext testContextInstance;
        private ActivitiesController _controller;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void CreateActivity_GoodValues_OK()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                Name = "ActivityOK",
                TripId = 1
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.OK).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateActivity_NoUser_KO()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                Name = "ActivityOK",
                TripId = 1
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.Unauthorized).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateActivity_NoName_KO()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                TripId = 1
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.BadRequest).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateActivity_EmptyName_KO()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                Name = "",
                TripId = 1
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.BadRequest).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateActivity_LongName_KO()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                Name = "ASDASDASDLKJLKJLKJQPOWIEPQWOIEQWPOEILAKSJDLASKJDLSKAJZXMNCXZMVZXASDASDASDLKJLKJLKJQPOWIEPQWOIEQWPOEILAKSJDLASKJDLSKAJZXMNCXZMVZXMNB",
                TripId = 1
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.BadRequest).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateActivity_NoTrip_KO()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                Name = "Activity Test no trip"
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.BadRequest).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreateActivity_InvalidTripId_KO()
        {
            var testCreateActivityDTO = new CreateActivityDTO
            {
                Name = "Activity Test Wrong Trip",
                TripId = 3333
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Activities/CreateActivity");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Activities" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            var response = _controller.CreateActivity(testCreateActivityDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.BadRequest).StatusCode, response.StatusCode);
        }
    }
}
