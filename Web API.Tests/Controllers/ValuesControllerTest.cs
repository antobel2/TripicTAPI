using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web_API;
using Web_API.Controllers;

namespace Web_API.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Réorganiser
            PostsController controller = new PostsController();

            // Agir
            IEnumerable<string> result = controller.GetPosts();

            // Déclarer
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Réorganiser
            PostsController controller = new PostsController();

            // Agir
            string result = controller.Get(5);

            // Déclarer
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Réorganiser
            PostsController controller = new PostsController();

            // Agir
            controller.Post("value");

            // Déclarer
        }

        [TestMethod]
        public void Put()
        {
            // Réorganiser
            PostsController controller = new PostsController();

            // Agir
            controller.Put(5, "value");

            // Déclarer
        }

        [TestMethod]
        public void Delete()
        {
            // Réorganiser
            PostsController controller = new PostsController();

            // Agir
            controller.Delete(5);

            // Déclarer
        }
    }
}
