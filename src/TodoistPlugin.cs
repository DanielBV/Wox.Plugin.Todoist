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




        private static readonly TodoistAPI client = new TodoistAPI();
        private PluginJsonStorage<Settings> storage = new PluginJsonStorage<Settings>();
        private static readonly int FAILED_TASK_RESULT_SCORE = -1;
        private static readonly int VALID_RESULT_SCORE = 10;

        public TodoistPlugin()
        {
            storage = new PluginJsonStorage<Settings>();
            var model = storage.Load();
            if (model.api_key is null)
            { //TODO this shouldn't be necessary
                model.api_key = "";
                storage.Save();
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
           

        }
        public Control CreateSettingPanel()
        {
           return new SettingsControl(storage);
        }

      
        public void Init(PluginInitContext context)
        {}

        public List<Result> Query(Query query)
        {
            String task = query.Search;
            String api_key = storage.Load().api_key;

            if (api_key.Trim().Length != 0)
            {
                List<Result> results = new List<Result>
                {
                    GetResultValid(api_key,task)
                };
                if (storage.Load().failedRequests.Count != 0)
                    results.Add(getResultTasksFailed());

                return results;
            }   
            else
                return new List<Result>
                {
                    getResultMissingAPI()         
                };
        }


        public Result GetResultValid(string api_key, string task)
        {
            return new Result
            {

                Title = $"Add the task: '{task}'.",
                IcoPath = "icon.png",
                Score = VALID_RESULT_SCORE,
                Action = _ =>
                {
                    var request = client.CreateQuickTask(api_key, task);

                    request.ContinueWith((taskRequest) =>
                    {
                        bool worked = taskRequest.Result;
                        if (!worked)
                        {
                            storage.Load().failedRequests.Add(task);
                            storage.Save();
                        }
                    });

                    return true;
                },
               
            };
        }


        public Result getResultMissingAPI()
        {
            return new Result
            {

                Title = "You must set your todoist API token in the plugin settings before adding tasks",
                IcoPath = "icon.png",
                Score = FAILED_TASK_RESULT_SCORE,
                Action = _ =>
                { return true; },
                
            };
        }

        public Result getResultTasksFailed()
        {
            return new Result
            {

                Title = "The creation of some tasks failed, go to the plugin settings to resend them.",
                IcoPath = "icon.png",
                Action = _ =>
                { return true; }
            };
        }

    }

      
   
}
