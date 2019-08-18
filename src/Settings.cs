using System;
using System.Collections.Generic;
using System.Net;

namespace Wox.Plugin.Todoist
{
    public class Settings
    {



        public HttpStatusCode LastFailedStatusCode { get; set; }

        public List<String> FailedRequests { get; set; }

        public Settings()
        {
            api_key = "";
            FailedRequests = new List<string>();
            LastFailedStatusCode = HttpStatusCode.OK;
        }
        public String api_key { get; set; }
      
    }
}
