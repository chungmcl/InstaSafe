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

namespace InstaSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string[]> suspects;
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void ButtonGenerateData_Click(object sender, RoutedEventArgs e)
        {
            string[] usernames = this.TextBoxUsernames.Text.Split(',');
            foreach (string username in usernames)
            {
                username.Trim();
            }
            

            // Pass all usernames to python algorithm
            // Save all usernames to text file
        }

        private void LoadData()
        {

        }

        private void CalculateSeverity()
        {
            for (int i = 0; i < suspects.Count; i++)
            {
                suspects[i][i]
            }
        }

        private void PostData()
        {

        }
    }
}
