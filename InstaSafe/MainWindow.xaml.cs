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

        private int SeverityOfPost()
        {
            return 0;
        }
    }

    struct Post
    {
        /// <summary>
        /// Whether the caption is considered to have risk or not
        /// </summary>
        private bool captionBad;

        /// <summary>
        /// The severity score of the image posted
        /// </summary>
        private double imageSeverity;

        /// <summary>
        /// The date the post was made on.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Sets the caption severity as a true or false
        /// </summary>
        /// <param name="setTo">Whether the caption contains severity</param>
        public void SetCaption(bool setTo)
        {
            this.captionBad = setTo;
        }

        /// <summary>
        /// Sets the severity score of the image posted
        /// </summary>
        /// <param name="setTo">The severity score of the image</param>
        public void SetImage(double setTo)
        {
            this.imageSeverity = setTo;
        }

        /// <summary>
        /// Sets the date of the post
        /// </summary>
        /// <param name="setTo">The date the post was made</param>
        public void SetDate(DateTime setTo)
        {
            this.date = setTo;
        }

        /// <summary>
        /// Returns whether the caption was risky or not
        /// </summary>
        /// <returns>Whether the caption is risky</returns>
        public bool GetCaption()
        {
            return this.captionBad;
        }

        /// <summary>
        /// Returns the severity score of the image
        /// </summary>
        /// <returns>The severity score of the image</returns>
        public double GetImage()
        {
            return this.imageSeverity;
        }

        /// <summary>
        /// Returns the date of the post
        /// </summary>
        /// <returns>The date of the post</returns>
        public DateTime GetDate()
        {
            return this.date;
        }
    }
}
