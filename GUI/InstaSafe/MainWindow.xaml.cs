using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace InstaSafe
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double HateSpeechWeight = 2;
        private const double OffensiveLanguageWeight = .1;
        List<Account> suspects;
        public MainWindow()
        {
            this.InitializeComponent();
            this.suspects = new List<Account>();
        }

        private void ButtonGenerateData_Click(object sender, RoutedEventArgs e)
        {
            this.DataGrid.Items.Clear();
            this.suspects.Clear();
            //WebScraper
            string[] usernames = this.TextBoxUsernames.Text.Split(',');
            string currentFolderPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            StreamWriter streamWriter = new StreamWriter(currentFolderPath + "\\usernames.txt");
            for (int i = 0; i < usernames.Length; i++)
            {
                usernames[i] = usernames[i].Trim();
                streamWriter.WriteLine(usernames[i] + ";");
            }
            streamWriter.Close();
            this.LoadAccounts($"{currentFolderPath}\\ImageData.txt", $"{currentFolderPath}\\CaptionData.txt");
            //Task loadAccounts = Task.Run(() => this.LoadAccounts($"{currentFolderPath}\\ImageData.txt", $"{currentFolderPath}\\CaptionData.txt"));
            this.suspects.Sort();
            foreach (Account account in this.suspects)
            {
                this.DataGrid.Items.Add(account);
            }
        }

        private void LoadAccounts(string imageTextFile, string captionTextFile)
        {
            // Have python generate ImageData.txt and CaptionData.txt from usernames.txt
            string path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            System.Diagnostics.Process.Start(path + @"\InstagramScraper.py");
            if (File.Exists(imageTextFile) && File.Exists(captionTextFile))
            {
                File.Delete(imageTextFile);
                File.Delete(captionTextFile);
            }
            File.Delete(path + "\\complete.txt");
            while (true)
            {
                if (File.Exists(path + "\\complete.txt"))
                {
                    try
                    {
                        List<Account> accounts = new List<Account>();
                        StreamReader readerImage = new StreamReader(imageTextFile);
                        StreamReader readerCap = new StreamReader(captionTextFile);
                        string username = readerImage.ReadLine();
                        readerCap.ReadLine();
                        List<Post> posts = new List<Post>();

                        while (!readerImage.EndOfStream && !readerCap.EndOfStream)
                        {
                            string dataCap = readerCap.ReadLine().Trim();
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
                                string[] capData = dataCap.Split(' ');
                                double weightedScore = //0;
                                //if (Convert.ToDouble(capData[5]) < (Convert.ToDouble(capData[1]) + Convert.ToDouble(capData[3])) * 1.5)
                                    weightedScore = Convert.ToDouble(capData[1]) * HateSpeechWeight + Convert.ToDouble(capData[3]) * OffensiveLanguageWeight; //- Convert.ToDouble(capData[5]) * .2;
                                posts.Add(new Post(Convert.ToDouble(weightedScore), Convert.ToDouble(dataImage[1]), Convert.ToDateTime(dataImage[0])));

                            }
                        }
                        this.suspects.Add(new Account(posts, username));
                        break;
                    }
                    catch (Exception ex)
                    {
                        string test = ex.Message.ToString();
                    }
                }
            }
           
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Account> selectedAccounts = new List<Account>();
            // Load all the items that the user selects
            for (int i = 0; i < this.DataGrid.SelectedItems.Count; i++)
            {
                selectedAccounts.Add((Account)this.DataGrid.SelectedItems[i]);
                SelectedUserDataPage selectedUserDataPage = new SelectedUserDataPage(selectedAccounts[0]);
                selectedUserDataPage.Show();
            }
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://kylemumma.github.io/instasafe/instasafe.htm");
        }
    }
}
