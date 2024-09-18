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
using System.Windows.Shapes;

namespace PacMan
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : Window
    {
        public PauseMenu()
        {
            InitializeComponent();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
            //Application.Current.Shutdown();
        }

        private void PauseMenuWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) 
            { 
                ResumeButton_Click(sender, e);
            }
        }
    }
}