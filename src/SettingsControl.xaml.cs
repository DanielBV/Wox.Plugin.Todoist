using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wox.Infrastructure.Storage;

namespace Wox.Plugin.Todoist
{
    
    public partial class SettingsControl : UserControl
    {
        private PluginJsonStorage<Settings> storage;
      

        public SettingsControl(PluginJsonStorage<Settings> storage)
        {

            this.storage = storage;
          
            InitializeComponent();
            apiKeyTextBox.Text = storage.Load().api_key;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           
            Settings settings = storage.Load();
            settings.api_key = apiKeyTextBox.Text;
            storage.Save();
            
        }
    }
}
