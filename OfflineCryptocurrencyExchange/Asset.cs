using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OfflineCryptocurrencyExchange
{
    public class Asset
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Ammount { get; set; }
        public int graphXCount { get; set; }
        public decimal BoughtPrice { get; set; }
        public List<Point> Point;

    }
}
