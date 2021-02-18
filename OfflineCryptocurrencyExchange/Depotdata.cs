using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OfflineCryptocurrencyExchange
{
    public class Depotdata
    {
        public string Name;
        public List<Asset> Values;


        public Depotdata()
        {
           
        }   

        public static Depotdata GetDefaultDepot()
        {
            var depot = new Depotdata()
            {
                Name = "unnnamed",
                Values = new List<Asset> { new Asset() {
                    Name = "USD",
                    Code = "USD",
                    Ammount = 25000,
                    Point = new List<Point>()
                }}
            };
            return depot;
        }
    }
}
