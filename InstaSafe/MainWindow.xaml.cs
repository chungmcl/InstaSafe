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
        List<Post> posts;
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

        private void SetAllSeverity()
        {
            for (int i = 0; i < this.posts.Count; i++)
            {
                this.posts[i].overallSeverity = (CompareCaptSeverity(this.posts[i].captionBad) + CompareImgSeverity(this.posts[i].imageSeverity));
            }
        }

        private int CompareCaptSeverity(bool capt)
        {
            if (capt)
                return 4;
            else
                return 0;
        }

        private int CompareImgSeverity(double img)
        {
            if (img >= 75)
                return 3;
            else if (img < 75 && img >= 50)
                return 2;
            else if (img < 50 && img >= 25)
                return 1;
            else
                return 0;
        }
    }

    class Post
    {
        /// <summary>
        /// Whether the caption is considered to have risk or not
        /// </summary>
        public bool captionBad { get; set; }

        /// <summary>
        /// The severity score of the image posted
        /// </summary>
        public double imageSeverity { get; set; }

        /// <summary>
        /// The date the post was made on.
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        /// The overall severity of the post;
        /// </summary>
        public int overallSeverity { get; set; }

        public Post(bool setCapt, double setImg, DateTime setDate)
        {
            this.captionBad = setCapt;
            this.imageSeverity = setImg;
            this.date = setDate;
        }
    }
}
