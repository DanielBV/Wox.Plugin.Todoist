
using System.Windows;
using System.Windows.Controls;
using Wox.Infrastructure.Storage;

namespace Wox.Plugin.Todoist
{
    
    public partial class SettingsControl : UserControl
    {
        private PluginJsonStorage<Settings> storage;
        private TodoistPlugin plugin;

        public SettingsControl(PluginJsonStorage<Settings> storage, TodoistPlugin plugin)
        {
            this.storage = storage;
            this.plugin = plugin;
          
            InitializeComponent();
            Settings settings = storage.Load();
            apiKeyTextBox.Text = settings.api_key;

            ConfigureFailedTasks();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           
            Settings settings = storage.Load();
            settings.api_key = apiKeyTextBox.Text;
            storage.Save();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            plugin.ResendFailedTasks();

            ConfigureFailedTasks();
        }

        private void ConfigureFailedTasks()
        {
            Settings settings = storage.Load();
            int failedTasks = settings.failedRequests.Count;
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
