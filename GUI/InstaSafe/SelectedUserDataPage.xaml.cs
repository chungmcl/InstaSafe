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

namespace InstaSafe
{
    /// <summary>
    /// Interaction logic for SelectedUserDataPage.xaml
    /// </summary>
    public partial class SelectedUserDataPage : Window
    {
        public SelectedUserDataPage(Account account)
        {
            this.InitializeComponent();
            for (int i = 0; i < account.Posts.Count(); i++)
            {
                this.DataGrid.Items.Add(account.Posts[i]);
                this.LabelUsername.Content = $"Post details for: {account.Username}";
            }
        }
    }
}
