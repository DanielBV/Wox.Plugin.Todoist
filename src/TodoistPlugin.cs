using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Wox.Plugin.Todoist
{
    public class TodoistPlugin : IPlugin, ISettingProvider
    {

        private static readonly TodoistAPI client = new TodoistAPI();
        private static readonly WoxSettingsStorage configuration = new WoxSettingsStorage();

        private SettingsControl settingsControl;
        public Dispatcher controlDispatcher;

        private static readonly int FAILED_TASK_RESULT_SCORE = 1;
        private static readonly int VALID_RESULT_SCORE = 1000000;

       

        public TodoistPlugin()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public Control CreateSettingPanel()
        {
           settingsControl = new SettingsControl(configuration, this);
            return settingsControl;
        }

      
        public void Init(PluginInitContext context)
        {}

        public List<Result> Query(Query query)
        {
            String task = query.Search;
            String api_key = configuration.Api_key; //TODO abstract storage

            if (api_key.Trim().Length != 0)
            {
                List<Result> results = new List<Result>
                {
                    GetResultValid(api_key,task)
                     
            };
                if (configuration.FailedRequests.Count != 0)
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
                            configuration.AddFailedRequest(task);
                            UpdateSettingsControl();
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
                Score = FAILED_TASK_RESULT_SCORE,
                Action = _ =>
                { return true; }
            };
        }
        public void ResendFailedTasks()
        {
            List<string> tasks = configuration.FailedRequests;
            string api_key = configuration.Api_key;

            foreach (string task in tasks)
            {
                var request = client.CreateQuickTask(api_key, task);

                request.ContinueWith((taskRequest) =>
                {
                    bool worked = taskRequest.Result;
                    if (worked) 
                        configuration.RemoveFailedRequest(task);

                    

                });


            }
            UpdateSettingsControl();

        }


        public void UpdateSettingsControl()
        {

         
             if (settingsControl!=null && controlDispatcher!= null)
             {
                
                controlDispatcher.BeginInvoke(
                new ThreadStart(() => { settingsControl.ConfigureFailedTasks(); }));

            }
             
              
        }

    }
 
}
