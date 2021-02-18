using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;

namespace OfflineCryptocurrencyExchange
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainController.Instance.ShowDepot(lb_OwnedCurrency);
            MainController.Instance.CheckCryptoCurrencies(lb_CurrenciesCurrentValue);
            MainController.Instance.FillComboBoxTotal(cb_Total);
            MainController.Instance.FillComboBoxTotal(cb_Stoinks);        
            MainController.Instance.FillComboBox(cb_From, cb_To);
            



            cb_From.SelectedIndex = 1;
            cb_To.SelectedIndex = 1;



            //REFRESH

            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        int thirtyseconds = 30;

        private void timer_Tick(object sender, EventArgs e)
        {
            if(thirtyseconds == 0)
            {
                lb_CurrenciesCurrentValue.Items.Clear();
                MainController.Instance.CheckCryptoCurrencies(lb_CurrenciesCurrentValue);
                MainController.Instance.AddNewPoints(canvans1);
                MainController.Instance.SaveAssets();
                MainController.Instance.Stoinks(canvans1, cb_Stoinks);
                MainController.Instance.ShowProfit(tb_Profit, cb_Stoinks);
                thirtyseconds = 30;
            }
            lb_timerTest.Content = thirtyseconds;
            thirtyseconds--;
        }

        private void Button_Click(object sender, RoutedEventArgs e) //BUY BUTTON
        {

            string selectedValue_From = (string)cb_From.SelectedValue;
            string selectedValue_To = (string)cb_To.SelectedValue;

            decimal enteredValue_From = Convert.ToDecimal(tb_From.Text);
            decimal enteredValue_To = Convert.ToDecimal(tb_To.Text);

            try
            {
                MainController.Instance.CalculateTheExchange(selectedValue_From, selectedValue_To, enteredValue_From, enteredValue_To);
                lb_OwnedCurrency.Items.Clear();
                MainController.Instance.ShowDepot(lb_OwnedCurrency);
                cb_Total.Items.Clear();
                MainController.Instance.FillComboBoxTotal(cb_Total);
                MainController.Instance.FillComboBoxTotal(cb_Stoinks);
            }
            catch (Exception ex)
            {
                throw (ex);
            }                

        }

        // Calculation for the two TextBox prices: 

        bool toActive = false;

        private void Tb_To_GotFocus(object sender, RoutedEventArgs e)
        {
            fromActive = false;
            toActive = true; 
        }

        bool fromActive = false;

        private void Tb_From_GotFocus(object sender, RoutedEventArgs e)
        {
            toActive = false;
            fromActive = true;
        }

        private void Tb_From_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(fromActive == true)
            {
                if (tb_From.Text == "")
                {
                    tb_From.Text = "0";
                }
                try
                {
                    string selectedValue_From = (string)cb_From.SelectedValue;
                    string selectedValue_To = (string)cb_To.SelectedValue;

                    decimal enteredValue_From = Convert.ToDecimal(tb_From.Text);
                    decimal tempValue;

                    tempValue = MainController.Instance.FastCalculation(selectedValue_From, selectedValue_To, enteredValue_From);
                    tb_To.Text = String.Format("{0:0.####}", tempValue);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void Tb_To_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(toActive == true)
            {
                if (tb_To.Text == "")
                {
                    tb_To.Text = "0";
                }
                try
                {
                    string selectedValue_From = (string)cb_From.SelectedValue;
                    string selectedValue_To = (string)cb_To.SelectedValue;

                    decimal enteredValue_To = Convert.ToDecimal(tb_To.Text);
                    decimal tempValue;

                    tempValue = MainController.Instance.FastCalculation(selectedValue_To, selectedValue_From, enteredValue_To);
                    tb_From.Text = String.Format("{0:0.####}", tempValue);               
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void Cb_From_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_From.Text = null;
            tb_To.Text = null;
        }

        private void Cb_To_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_From.Text = null;
            tb_To.Text = null;
        }

        // End of calculation section

        private void Cb_Total_SelectionChanged(object sender, SelectionChangedEventArgs e) // Calculates the total owned crypto in $
        {
            string selectedValue_Total = (string)cb_Total.SelectedValue;
            decimal tempvalue = MainController.Instance.CalculateTheTotal(selectedValue_Total);

            lb_Total.Content = String.Format("{0:0.##}", tempvalue) + "$";
        }

        private void Cb_Stoinks_SelectionChanged(object sender, SelectionChangedEventArgs e) // PROFIT IN THE TEXTBOX
        {
            MainController.Instance.Stoinks(canvans1, cb_Stoinks);
            MainController.Instance.ShowProfit(tb_Profit, cb_Stoinks);
        }

    }
}
