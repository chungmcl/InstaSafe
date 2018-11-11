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
using System.Reflection;

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
            this.suspects = new List<Account>();
        }

        private void ButtonGenerateData_Click(object sender, RoutedEventArgs e)
        {
            string[] usernames = this.TextBoxUsernames.Text.Split(',');
            string currentFolderPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            StreamWriter streamWriter = new StreamWriter(currentFolderPath + "\\usernames.txt");
            for (int i = 0; i < usernames.Length; i++)
            {
                usernames[i] = usernames[i].Trim();
                streamWriter.WriteLine(usernames[i]);
            }
            streamWriter.Close();
            // Have python generate ImageData.txt and CaptionData.txt from usernames.txt
            this.LoadAccounts($"{currentFolderPath}\\ImageData.txt", $"{currentFolderPath}\\CaptionData.txt");
            this.suspects.Sort();
            foreach (Account account in this.suspects)
            {
                this.DataGrid.Items.Add(account);
            }
        }

        private void LoadAccounts(string imageTextFile, string captionTextFile)
        {
            List<Account> accounts = new List<Account>();
            StreamReader readerImage = new StreamReader(imageTextFile);
            StreamReader readerCap = new StreamReader(captionTextFile);
            string username = readerImage.ReadLine();
            readerCap.ReadLine();
            List<Post> posts = new List<Post>();

            while (!readerImage.EndOfStream && !readerCap.EndOfStream)
            {
                string current = readerImage.ReadLine();
                if (!current.Contains(' '))
                {
                    this.suspects.Add(new Account(posts, username));
                    username = current;
                    posts = new List<Post>();
                }
                else
                {
                    string[] dataImage = current.Split(' ');
                    string dataCap = readerCap.ReadLine().Trim();
                    posts.Add(new Post((dataCap == "1"), Convert.ToDouble(dataImage[1]), Convert.ToDateTime(dataImage[0])));
                    
                }
            }
            this.suspects.Add(new Account(posts, username));
        }
    }
}
