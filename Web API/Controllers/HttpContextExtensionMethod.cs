using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Web_API.Controllers
{
    public static class HttpContextExtensionMethod
    {
        public static IEnumerable<HttpPostedFile> Collection(this HttpFileCollection files)
        {
            var data = new List<HttpPostedFile>();
            for (var i = 0; i < files.Count; i++)
            {
                data.Add(files[i]);
            }
            return data;
        }
    }
}
