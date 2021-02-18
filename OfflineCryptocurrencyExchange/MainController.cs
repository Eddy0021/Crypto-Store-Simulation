using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Path = System.Windows.Shapes.Path;

namespace OfflineCryptocurrencyExchange
{
    class MainController
    {
        private static MainController _instance = null;

        public static MainController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainController();
                }
                return _instance;
            }
        }

        Depotdata CurrentDepot = new Depotdata();

        private MainController()
        {
            CurrentDepot = Depotdata.GetDefaultDepot();
        }

        List<Asset> tempCurrency = new List<Asset>();


        decimal CyrptoPrice = 0;
        decimal tempConvertValue_to;
        decimal tempConvertValue_from;
        decimal tempValue_total;
        decimal tempValue_total2;
        bool NotExist = true;
        decimal BoughtPriceLast;

        public void ShowDepot(ListBox list) //FILLING THE OWN CURRENCIES
        {
            
            foreach (var item in CurrentDepot.Values)
            {
               list.Items.Add("(" + item.Code + ") " + item.Name + " : " + String.Format("{0:0.0000}", item.Ammount) );       
            }
        }

        public void CheckCryptoCurrencies(ListBox list) // NEED CHANGES
        {

            //string exmple = "{\"XMR\":{\"USD\":158.86},\"LTC\":{\"USD\":129.28}}";

            //JObject jobj = JObject.Parse(exmple);


            WebClient wc = new WebClient();

            //char[] spearator = { ':', '{', '"', '}' };

            string currencis = "BTC,ETH,USDT,XRP,LTC,BCH,DOT,ADA,BNB,LINK,USDC,XLM,WBTC,BSV,XMR,USD";
    
            try
            {
                string mess;
                mess = wc.DownloadString(@"https://min-api.cryptocompare.com/data/pricemulti?fsyms=" + currencis + "&tsyms=USD");
                //string[] WebsiteString = mess.Split(spearator);


                JObject jobj = JObject.Parse(mess);

                if (tempCurrency != null || tempCurrency.Count != 0)
                {
                    tempCurrency.Clear();
                }
                foreach (var item in jobj)
                {                               
                    list.Items.Add(item.Key + " : " + item.Value.Last.First);
                    tempCurrency.Add(new Asset { Code = item.Key, Ammount = (decimal)item.Value.Last.First });
                }

                //if (item.Code != "USD")
                //{
                //    //list.Items.Add(WebsiteString[2] + " : " + WebsiteString[8]);
                //}

                //tempCurrency.Add( new Asset { Code = WebsiteString[2], Ammount = Convert.ToDecimal(WebsiteString[8]) });
            }
            catch (Exception)
            {

                list.Items.Add("Error while trying to load the data!");
            } 

            
        }

        public void FillComboBox(ComboBox from, ComboBox to) //COMBOBOXES FILLING WITH DATA
        {
            foreach (var item in tempCurrency)
            {
                from.Items.Add(item.Code);
                to.Items.Add(item.Code);
            }
            
        }

        public void FillComboBoxTotal(ComboBox Total) //FILLING THE "TOTAL" COMBOBOX
        {
            Total.Items.Clear();
            foreach (var item in CurrentDepot.Values)
            {
                Total.Items.Add(item.Code);           
            }
        }

        public void CalculateTheExchange(string s_From,string s_To,decimal tb_From , decimal tb_To) // ADDING NEW ASSET OR JUST THE AMMOUNT & SAVE THE PRICE OF CRYPTO WHEN IT WAS BOUGHT
        {
            NotExist = true;
            foreach (var item in CurrentDepot.Values)
            {
                if(item.Code == s_To) // ADDING THE AMMOUNT THE ASSET IF IT EXIST ALREADY
                {
                    item.Ammount += tb_To;
                    item.BoughtPrice = BoughtPriceLast;
                    item.Point.Clear();
                    NotExist = false;
                }            

                
                //if(item.Code !=)
                //{
                //    throw new Exception("You have 0 Crypto!");
                //}

                if (item.Code == s_From)
                {
                    item.Ammount -= tb_From;
                    Console.WriteLine("USD: " + item.Ammount);
                }
                
            }

            if (NotExist == true) // CREATING NEW ASSET IF THERE'S NONE
            {
                CurrentDepot.Values.Add(new Asset() { Code = s_To, Name = s_To, Ammount = tb_To , Point = new List<Point>(), BoughtPrice = BoughtPriceLast });            
            }              

            SaveAssets();
        }

        public decimal FastCalculation(string s_From, string s_To, decimal tb_From) // CALCULATES THE EXCHANGE IN THE TEXTBOXES
        {
            foreach (var item in tempCurrency)
            {
                if(item.Code == s_From)
                {
                    tempConvertValue_from = item.Ammount;
                    if (item.Ammount != 1)
                    {
                        BoughtPriceLast = item.Ammount;
                    }
                }
                if (item.Code == s_To)
                {
                    tempConvertValue_to = item.Ammount;
                    if(item.Ammount != 1)
                    {
                        BoughtPriceLast = item.Ammount;
                    }                
                }
            }          

            CyrptoPrice = (tb_From * tempConvertValue_from) / tempConvertValue_to;

            return CyrptoPrice;
        }

        public decimal CalculateTheTotal(string total) // GIVES THE TOTAL AMMOUNT MONEY YOU OWN IN SELECTED ASSET
        {
            foreach (var item in tempCurrency)
            {
                if(item.Code == total)
                {
                    tempValue_total = item.Ammount;
                }
            }

            foreach (var item1 in CurrentDepot.Values)
            {
                if (item1.Code == total)
                {
                    tempValue_total2 = item1.Ammount;
                }
            }

            return tempValue_total * tempValue_total2;
        }


        public void SaveAssets() // SAVE ASSETS!
        {
            var json = JsonConvert.SerializeObject(CurrentDepot.Values, Formatting.Indented);
            //JObject json = new JObject();
            File.WriteAllText(@"Save.txt", json);


        }

        public void LoadAssets(ListBox list, ComboBox Total, ComboBox Stoinks) // LOAD THE SAVED ASSETS
        {

            CurrentDepot.Values.Clear();
      
            string file = File.ReadAllText(@"Save.txt");

             var json = JsonConvert.DeserializeObject<List<Asset>>(file);



            foreach (var item in json)
            {
                CurrentDepot.Values.Add(new Asset { Name = item.Name, Code = item.Code, Ammount = item.Ammount, graphXCount = item.graphXCount, Point = new List<Point>(item.Point), BoughtPrice = item.BoughtPrice });
            }


            FillComboBoxTotal(Total);
            FillComboBoxTotal(Stoinks);
            ShowDepot(list);
        }


        public void AddNewPoints(Canvas can) //CALCULATES THE NEW POINT ON THE GRAPH BY CHECKING THE BOUGHT PRICE AND THE CURRENT OWN PRICE OF THE CRYPTO
        {
            foreach (var itemInCD in CurrentDepot.Values)
            {
                foreach (var itemInTC in tempCurrency)
                {
                    if (itemInCD.Code == itemInTC.Code)
                    {
                        if(itemInCD.graphXCount != 270)
                        {
                            if (itemInCD.Point == null || itemInCD.Point.Count < 1)
                            {
                                itemInCD.Point.Add(new Point(10, 90));
                                itemInCD.graphXCount = 20;
                            }

                            decimal temp = itemInTC.Ammount - itemInCD.BoughtPrice;

                            temp /= 50;

                            if (temp > 0.5m)
                            {
                                double graphY = 90 - Convert.ToDouble(temp);

                                if(graphY < -0)
                                {
                                    graphY = 0;
                                }

                                itemInCD.Point.Add(new Point(itemInCD.graphXCount, graphY));
                                itemInCD.graphXCount += 10;
                            }
                            if (temp < -0.5m)
                            {
                                decimal endValue = 0 - (temp);
                                double graphY = 90 + Convert.ToDouble(endValue);

                                if(graphY > 140)
                                {
                                    graphY = 130;
                                }

                                itemInCD.Point.Add(new Point(itemInCD.graphXCount, graphY));
                                itemInCD.graphXCount += 10;
                            }
                        }
                        else
                        {
                            Point temp = itemInCD.Point.Last();                       
                            itemInCD.Point.Clear();
                            itemInCD.graphXCount = 10;
                            itemInCD.Point.Add(new Point(itemInCD.graphXCount, temp.Y));
                        }                   
                    }
                }
            }
        }

        public void ShowPointsOnGraph(PointCollection points, ComboBox Stoinks) // SHOWS THE SELECTED CRYPTO ON THE PARAGPRAH
        {
            string selectedValue_Stoinks = (string)Stoinks.SelectedValue;
            foreach (var item in CurrentDepot.Values)
            {
                if(item.Code == selectedValue_Stoinks)
                {
                    for (int i = 0; i < item.Point.Count; i++)
                    {
                        points.Add(item.Point[i]);
                    }
                }
                
            }
            
        }

        public void Stoinks(Canvas can, ComboBox Stoinks) // FILLIN THE GRAPHICAL STOINKS
        {
            can.Children.Clear();
            const double margin = 10;
            double xmin = margin;
            double xmax = can.Width - margin;
            //double ymin = margin;
            double ymax = can.Height - margin;
            //const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, 90), new Point(can.Width, 90)));
            //for (double x = xmin + step;
            //    x <= canv1.Width - step; x += step)
            //{
            //    xaxis_geom.Children.Add(new LineGeometry(
            //        new Point(x, ymax - margin / 2),
            //        new Point(x, ymax + margin / 2)));
            //}   

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 0.5;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            can.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, can.Height)));
            //for (double y = step; y <= canv1.Height - step; y += step)
            //{
            //    yaxis_geom.Children.Add(new LineGeometry(
            //        new Point(xmin - margin / 2, y),
            //        new Point(xmin + margin / 2, y)));
            //}     

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            can.Children.Add(yaxis_path);

            //List<Point> pts = new List<Point>();

            PointCollection points = new PointCollection();

            ShowPointsOnGraph(points, Stoinks);

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = Brushes.Black;
            polyline.Points = points;

            can.Children.Add(polyline);

            //pts.Add(new Point(10, 90));
            //pts.Add(new Point(20, 62));
            //pts.Add(new Point(30, 65));
            //pts.Add(new Point(40, 67));
            //pts.Add(new Point(50, 114));
            //pts.Add(new Point(70, 70));






        }

        public void ShowProfit(TextBox tb_Profit, ComboBox cb_Stoinks) // SHOW THE PROFIT IN THE SELECTED CURRENCY 
        {
            string selectedValue_Stoinks = (string)cb_Stoinks.SelectedValue;
            foreach (var itemCD in CurrentDepot.Values)
            {
                foreach (var itemTC in tempCurrency)
                {
                    if (itemCD.Code == selectedValue_Stoinks && itemTC.Code == selectedValue_Stoinks)
                    {
                        tb_Profit.Text = Convert.ToString(String.Format("{0:0.##}", itemTC.Ammount - itemCD.BoughtPrice));
                    }
                }
                
            }
        }
    }
}
