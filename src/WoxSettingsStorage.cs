
using System.Collections.Generic;
using Wox.Infrastructure.Storage;


namespace Wox.Plugin.Todoist
{
    public class WoxSettingsStorage
    {

        private PluginJsonStorage<Settings> settings;

        public WoxSettingsStorage()
        {
           
            settings = new PluginJsonStorage<Settings>();
            settings.Save();
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
                return settings.Load().failedRequests;
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
       

      
    }
}
