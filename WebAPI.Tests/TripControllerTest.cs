using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_API.Controllers;
using Web_API.Models;

namespace WebAPI.Tests
{
    [TestClass]
    public class TripControllerTest
    {

        TripsController _controller;
        public TripControllerTest()
        {
            _controller = new TripsController();
        }
        [TestMethod]
        public void InviteUserToTrip_GoodValues_OK()
        {
            var testCreateTripDTO = new CreateTripDTO()
            {
                Name = "Test à cuba"
            };
            
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Trips/InviteUserToTrip");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Trips" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            var response = _controller.CreateTrip(testCreateTripDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.OK).StatusCode, response.StatusCode);
        }
    }
}
