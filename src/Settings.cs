using System;
using System.Collections.Generic;

namespace Wox.Plugin.Todoist
{
    public class Settings
    {

   
        public List<String> failedRequests { get; set; }
        public Settings()
        {
            api_key = "";
            failedRequests = new List<string>();
        }
        public String api_key { get; set; }
       
    }
}
