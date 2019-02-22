using System;
using System.Collections.Generic;
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

        [TestInitialize]
        private void initializeTestMethod()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Trips/InviteUserToTrip");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Trips" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

        [TestMethod]
        public void InviteUserToTrip_GoodValues_OK()
        {
            InviteUserToTripDTO model = new InviteUserToTripDTO()
            {
                TripId = 1,
                UserIds = new List<string>()
                {
                    "04b6dea7-fd92-43ae-9912-f563fbff42d8"
                }
            };
            
            var response = _controller.InviteUserToTrip(model);
            Assert.AreEqual(_controller.Request.CreateResponse(HttpStatusCode.OK).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void InviteUserToTrip_NoTripId_KO()
        {
            InviteUserToTripDTO model = new InviteUserToTripDTO()
            {
                UserIds = new List<string>()
                {
                    "04b6dea7-fd92-43ae-9912-f563fbff42d8"
                }
            };
            
            var response = _controller.InviteUserToTrip(model);
            Assert.AreEqual(_controller.Request.CreateResponse(HttpStatusCode.BadRequest, "Le model InviteUserToTripDTO n'est pas valide").ReasonPhrase, response.ReasonPhrase);
        }

        [TestMethod]
        public void InviteUserToTrip_NoUserIds_KO()
        {
            InviteUserToTripDTO model = new InviteUserToTripDTO()
            {
                TripId = 1,
                UserIds = new List<string>()
                {
                    
                }
            };

            var response = _controller.InviteUserToTrip(model);
            Assert.AreEqual(_controller.Request.CreateResponse(HttpStatusCode.BadRequest, "Le model InviteUserToTripDTO n'est pas valide").ReasonPhrase, response.ReasonPhrase);
        }

        [TestMethod]
        public void InviteUserToTrip_TripNotFound_KO()
        {
            InviteUserToTripDTO model = new InviteUserToTripDTO()
            {
                TripId = int.MaxValue,
                UserIds = new List<string>()
                {
                    "04b6dea7-fd92-43ae-9912-f563fbff42d8"
                }
            };
            
            var response = _controller.InviteUserToTrip(model);
            Assert.AreEqual(_controller.Request.CreateResponse(HttpStatusCode.NotFound, "L'id du voyage n'a retourné aucun résultats").ReasonPhrase, response.ReasonPhrase);
        }

        [TestMethod]
        public void InviteUserToTrip_UserNotPermitted_KO()
        {
            InviteUserToTripDTO model = new InviteUserToTripDTO()
            {
                TripId = int.MaxValue,
                UserIds = new List<string>()
                {
                    "04b6dea7-fd92-43ae-9912-f563fbff42d8"
                }
            };
            
            var response = _controller.InviteUserToTrip(model);
            Assert.AreEqual(_controller.Request.CreateResponse(HttpStatusCode.Forbidden, "L'utilisateur actuel n'a pas les droits d'accès au voyage demandé").ReasonPhrase, response.ReasonPhrase);
        }

        [TestMethod]
        public void InviteUserToTrip_UserToAddNotFound_KO()
        {
            InviteUserToTripDTO model = new InviteUserToTripDTO()
            {
                TripId = int.MaxValue,
                UserIds = new List<string>()
                {
                    "notValidId"
                }
            };
            
            var response = _controller.InviteUserToTrip(model);
            Assert.AreEqual(_controller.Request.CreateResponse(HttpStatusCode.NotFound, "L'id de l'utilisateur à inviter n'a retourné aucun résultats").ReasonPhrase, response.ReasonPhrase);
        }
        
    }
}
