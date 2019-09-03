
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Wox.Plugin.Todoist
{
    public class WoxSettingsStorage
    {

        private Settings settings;
        private string plugin_directory;
        private static readonly string SETTINGS_FILE_NAME = "todoist-settings.json";

        public WoxSettingsStorage(string plugin_directory)
        {
            this.plugin_directory = plugin_directory;
            settings = LoadSettingsFile(plugin_directory);
        }


        private Settings LoadSettingsFile(string plugin_directory)
        {
            var full_path = Path.Combine(plugin_directory, SETTINGS_FILE_NAME);

            if (File.Exists(full_path))
            {
                try {
                    string json = File.ReadAllText(full_path);
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
                catch (Exception)
                {
                    return new Settings();
                }
                
            }

            return new Settings();
        }

        public string Api_key
        {
            get
            {
                return settings.api_key;
            }

            set
            {
                settings.api_key = value;
                Save();
            }
        }

        public List<string> FailedRequests
        {
            get
            {
                return settings.FailedRequests;
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
            var full_path = Path.Combine(plugin_directory, SETTINGS_FILE_NAME);

            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(full_path, json);

        }
       
        public void SetLastFailedHttpCode(HttpStatusCode code)
        {
            settings.LastFailedStatusCode = code;
            Save();
        }

        public void EmptyLastFailedHttpCode()
        {
            settings.LastFailedStatusCode = HttpStatusCode.OK;
            Save();
        }

      
        public HttpStatusCode GetLastFailedHttpCode()
        {
            return settings.LastFailedStatusCode;
        }
      
    }
}
