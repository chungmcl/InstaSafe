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
using System.IO;

namespace InstaSafe
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Account> suspects;
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void ButtonGenerateData_Click(object sender, RoutedEventArgs e)
        {
            //string[] usernames = this.TextBoxUsernames.Text.Split(',');
            //foreach (string username in usernames)
            //{
            //    username.Trim();
            //    LoadAccounts(username);
            //}
            // Pass all usernames to python algorithm
            // Save all usernames to text file
            LoadAccounts("C:\\Users\\chung\\Desktop\\ImageTestFile.txt", "C:\\Users\\chung\\Desktop\\CaptionTestFile.txt");
        }

        private void LoadAccounts(string imageTextFile, string captionTextFile)
        {
            List<Account> accounts = new List<Account>();
            StreamReader readerImage = new StreamReader(imageTextFile);
            StreamReader readerCap = new StreamReader(captionTextFile);
            string username;
            while (!readerImage.EndOfStream && !readerCap.EndOfStream)
            {
                List<Post> posts = new List<Post>();
                string current = readerImage.ReadLine();
                if (!current.Contains(','))
                {
                    username = current;
                }
                string[] dataImage = readerImage.ReadLine().Split(',');
                string dataCap = readerCap.ReadLine().Trim();
                posts.Add(new Post((dataCap == "1"), Convert.ToDouble(dataImage[1]), Convert.ToDateTime(dataImage[0])));
                this.suspects.Add(new Account(posts));
            }
        }
    }
}
