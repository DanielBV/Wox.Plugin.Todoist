using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wox.Infrastructure.Storage;

namespace Wox.Plugin.Todoist
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl

    
{
        private PluginJsonStorage<Settings> storage;
      

        public UserControl1(PluginJsonStorage<Settings> storage)
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
