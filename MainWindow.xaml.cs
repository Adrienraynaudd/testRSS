using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Windows;
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
            List<SyndicationItem> items = rssReader.ReadRss("https://www.courrierinternational.com/feed/rubrique/france/rss.xml");

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
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    using (Stream stream = client.OpenRead(url))
                    {
                        // Use StreamReader to detect the encoding
                        using (StreamReader streamReader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true))
                        {
                            using (XmlReader reader = XmlReader.Create(streamReader))
                            {
                                SyndicationFeed feed = SyndicationFeed.Load(reader);

                                if (feed != null)
                                {
                                    foreach (SyndicationItem item in feed.Items)
                                    {
                                        items.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return items;
        }


    }
}