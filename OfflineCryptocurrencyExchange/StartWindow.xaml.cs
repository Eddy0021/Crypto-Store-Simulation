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

namespace OfflineCryptocurrencyExchange
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            MainWindow mw = new MainWindow();

            mw.Show();
            mw.lb_OwnedCurrency.Items.Clear();
            MainController.Instance.LoadAssets(mw.lb_OwnedCurrency, mw.cb_Total, mw.cb_Stoinks);

            this.Close();
            
        }
    }
}
