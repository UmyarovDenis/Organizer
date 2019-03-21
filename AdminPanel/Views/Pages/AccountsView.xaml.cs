using AdminPanel.Views.Attributes;
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

namespace AdminPanel.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccountsView.xaml
    /// </summary>
    [PageDescriptor(ImageSource = @"pack://application:,,,/Resourses/users.png", Label = "Учетные записи")]
    public partial class AccountsView : Page
    {
        public AccountsView()
        {
            InitializeComponent();
        }
    }
}
