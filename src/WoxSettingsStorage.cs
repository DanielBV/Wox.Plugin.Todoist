
using System.Collections.Generic;
using System.Net;
using Wox.Infrastructure.Storage;


namespace Wox.Plugin.Todoist
{
    public class WoxSettingsStorage
    {

        private PluginJsonStorage<Settings> settings;

        public WoxSettingsStorage()
        {
            settings = new PluginJsonStorage<Settings>();
           
        }


        public string Api_key
        {
            get
            {
                return settings.Load().api_key;
            }

            set
            {
                settings.Load().api_key = value;
                settings.Save();
            }
        }

        public List<string> FailedRequests
        {
            get
            {
                return settings.Load().FailedRequests;
            }
        }

        public void AddFailedRequest(string value)
        {
            FailedRequests.Add(value);
            Save();
        }

        public void RemoveFailedRequest(string value)
        {
            FailedRequests.Remove(value);
            Save();
        }


        public void Save()
        {
            settings.Save();
        }
       
        public void SetLastFailedHttpCode(HttpStatusCode code)
        {
            settings.Load().LastFailedStatusCode = code;
            Save();
        }

        public void EmptyLastFailedHttpCode()
        {
            settings.Load().LastFailedStatusCode = HttpStatusCode.OK;
            Save();
        }

      
        public HttpStatusCode GetLastFailedHttpCode()
        {
            return settings.Load().LastFailedStatusCode;
        }
      
    }
}
