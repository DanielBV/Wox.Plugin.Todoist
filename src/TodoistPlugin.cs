using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows.Controls;
using Wox.Infrastructure.Storage;

namespace Wox.Plugin.Todoist
{
    public class TodoistPlugin : IPlugin, ISettingProvider
    {

      
     
      
        private static readonly HttpClient client = new HttpClient();
        private PluginJsonStorage<Settings> storage = new PluginJsonStorage<Settings>();

        public TodoistPlugin()
        {
            storage = new PluginJsonStorage<Settings>();
            var model = storage.Load();
            if (model.api_key is null)
            {
                model.api_key = "";
                storage.Save();
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
           

        }
        public Control CreateSettingPanel()
        {
            var control = new UserControl1(storage);
            return control;
        }

        //TODO test si puedo quitar Init o el constru ctor
        public void Init(PluginInitContext context)
        {
           /* var storage = new PluginJsonStorage<Settings>();
            var model = storage.Load();

            storage.Save();*/
        }

        public List<Result> Query(Query query)
        {
            String task = query.Search;
            String api_key = storage.Load().api_key;

            if (api_key.Trim().Length != 0)
                return new List<Result>
                {

                new Result
                    {

                        Title = $"Add the task: '{task}'.",
                        IcoPath = "icon.png",
                        Action = _ =>
                        {

                         var values = new Dictionary<string, string>
                            {
                               { "token", api_key},
                               { "text", task }
                            };
                            var content = new FormUrlEncodedContent(values);
                           var response = client.PostAsync("https://api.todoist.com/sync/v8/quick/add", content);
                            return true;
                        }
                    },


                };
           else
            return new List<Result>
                {

                new Result
                    {

                        Title =  "You must set your todoist API token in the plugin settings before adding tasks",
                        IcoPath = "icon.png",
                        Action = _ =>
                        {return true; }
                    },


                };
        }

      
    }
}
