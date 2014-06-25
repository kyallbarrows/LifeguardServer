using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parichay.MVC.Controllers
{
    public class JsonResponse
    {
        public string errorMessage { get; set; }

        public bool isSuccessful { get; set; }

        public string responseText { get; set; }

        public string Id { get; set; }

        public JsonResponse()
        {

            errorMessage = "";
            isSuccessful = false;
        }

    }
}