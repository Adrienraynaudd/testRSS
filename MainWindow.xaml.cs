using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
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
using System.Xml;

namespace testRSS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadRss();
        }

        private void LoadRss()
        {
            RssReader rssReader = new RssReader();
            List<SyndicationItem> items = rssReader.ReadRss("https://www.lemonde.fr/rss/une.xml");

            rssListBox.ItemsSource = items;
        }
    }


    public class RssReader
    {
        public List<SyndicationItem> ReadRss(string url)
        {
            List<SyndicationItem> items = new List<SyndicationItem>();

            try
            {
                XmlReader reader = XmlReader.Create(url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                if (feed != null)
                {
                    foreach (SyndicationItem item in feed.Items)
                    {
                        items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return items;
        }
    }

}

