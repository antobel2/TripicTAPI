using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_API.Controllers;
using Web_API.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http.ModelBinding;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;

namespace WebAPI.Tests
{
    /// <summary>
    /// Description résumée pour PostControllerTest
    /// </summary>
    [TestClass]
    public class PostsControllerTest
    {
        PostsController _controller;
        public PostsControllerTest()
        {
            _controller = new PostsController();
        }

        private TestContext testContextInstance;

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
        public void CreatePost_GoodValues_OK()
        {
            var testCreatePostDTO = new CreatePostDTO()
            {
                UserId = 1,
                ActivityId = 1,
                PicCount = 4,
                Text = "TestTestTest"
            };
            
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Post/CreatePost");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Posts" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            var response = _controller.CreatePost(testCreatePostDTO);

            Assert.AreEqual(request.CreateResponse(HttpStatusCode.OK).StatusCode, response.StatusCode);
        }

        [TestMethod]
        public void CreatePost_TooManyPics_KO()
        {
            var testCreatePostDTO = new CreatePostDTO()
            {
                UserId = 1,
                ActivityId = 1,
                PicCount = 88,
                Text = "TestTestTest"
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Post/CreatePost");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Posts" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            var response = _controller.CreatePost(testCreatePostDTO);

            Assert.AreEqual(request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le champ PictureNumber doit être compris entre 0 et 25.").ReasonPhrase, response.ReasonPhrase);
        }

        [TestMethod]
        public void CreatePost_TextTooLong_KO()
        {
            var testCreatePostDTO = new CreatePostDTO()
            {
                UserId = 1,
                ActivityId = 1,
                PicCount = 4,
                Text = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest"
            };

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Post/CreatePost");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Posts" } });

            _controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            _controller.Request = request;
            _controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            var response = _controller.CreatePost(testCreatePostDTO);

            Assert.Fail(response.ReasonPhrase);
        }
    }
}
