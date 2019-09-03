
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Wox.Plugin.Todoist
{
    
    public partial class SettingsControl : UserControl
    {
        private WoxSettingsStorage storage;
        private TodoistPlugin plugin;

        public SettingsControl(WoxSettingsStorage storage, TodoistPlugin plugin)
        {
            plugin.controlDispatcher = Application.Current.Dispatcher;
            this.storage = storage;
            this.plugin = plugin;
          
            InitializeComponent();
            apiKeyTextBox.Text = storage.Api_key;

            ConfigureFailedTasks(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            storage.Api_key = apiKeyTextBox.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            plugin.ResendFailedTasks();
        }

        public void ConfigureFailedTasks()
        {
            
            int failedTasks = storage.FailedRequests.Count;
            lblFailedTasks.Content = $"Failed to sync {failedTasks} tasks.";
        
            if (failedTasks == 0)
            {
                lblFailedTasks.Visibility = Visibility.Hidden;
                btnResendRequests.Visibility = Visibility.Hidden;
                lblFailedStatusCode.Visibility = Visibility.Hidden;
            }
            else
            {
                SetFailedStatusCodeMessage();
                lblFailedTasks.Visibility = Visibility.Visible;
                btnResendRequests.Visibility = Visibility.Visible;
                lblFailedStatusCode.Visibility = Visibility.Visible;
            }
            

        }

    public void SetFailedStatusCodeMessage()
        {
            HttpStatusCode status = storage.GetLastFailedHttpCode();
            string message;

            switch (status)
            {
                case HttpStatusCode.OK:
                    message = "The last failed request returned OK but somehow it still failed. Try again or contact the developer.";
                    break;
                case HttpStatusCode.Forbidden:
                    message = "The last failed request returned HTTP 403 Forbidden.\nPlease verify that your API Token is correct and try again.";
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    message = "Can't access 'api.todoist.com'. Please ensure you are connected to the internet and try again. ";
                    break;
                default:
                    message = $"The last failed request returned {status}.";
                    break;

            }

            lblFailedStatusCode.Content = message;

        }
    }
}
