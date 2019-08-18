
using System.IO;
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
            }
            else
            {
                lblFailedTasks.Visibility = Visibility.Visible;
                btnResendRequests.Visibility = Visibility.Visible;
            }
            

           

           

        }
    }
}
