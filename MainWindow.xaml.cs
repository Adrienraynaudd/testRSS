using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Ude;

namespace testRSS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            string rssUrl = "https://www.lemondeinformatique.fr/flux-rss/thematique/securite/rss.xml";

            
            Encoding encoding = EncodingDetectionHelper.DetectEncoding(rssUrl);

            
            XmlReaderSettings readerSettings = new XmlReaderSettings
            {
                Async = true,
                
            };

            
            XmlReader reader = XmlReader.Create(rssUrl, readerSettings);

            
            DisplayRssFeed(rssUrl, reader);
        }

        
        private async void DisplayRssFeed(string rssUrl, XmlReader xmlReader)
        {
            
            var rssItems = await RssReadFeed.CreateRssFeedReaderExample(rssUrl, xmlReader);

            
            rssListBox.ItemsSource = rssItems;
        }

        public class EncodingDetectionHelper
        {
            public static Encoding DetectEncoding(string filePath)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                       
                        HttpResponseMessage response = client.GetAsync(filePath).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Stream stream = response.Content.ReadAsStreamAsync().Result;

                            var detector = new Ude.CharsetDetector();
                            detector.Feed(stream);
                            detector.DataEnd();

                            if (detector.Charset != null)
                            {
                                return Encoding.GetEncoding("iso-8859-15");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"La requête a échoué avec le code : {response.StatusCode}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une exception s'est produite : {ex.Message}");
                    
                }

                
                return Encoding.UTF8;

            }
        }


    }
    class RssReadFeed
    {
        public class ItemClass
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
        }

        public static async Task<List<ItemClass>> CreateRssFeedReaderExample(string filePath, XmlReader xmlReader)
        {
            List<ItemClass> rssItems = new List<ItemClass>();

            var feedReader = new RssFeedReader(xmlReader);

            while (await feedReader.Read())
            {
                switch (feedReader.ElementType)
                {
                    case SyndicationElementType.Item:
                        ISyndicationItem item = await feedReader.ReadItem();
                        ItemClass rssItem = ConvertToYourRssItem(item);
                        rssItems.Add(rssItem);
                        break;

                }
            }

            return rssItems;
        }



        private static ItemClass ConvertToYourRssItem(ISyndicationItem syndicationItem)
        {
            return new ItemClass
            {
                Title = syndicationItem.Title,
                Description = syndicationItem.Description,
                
            };
        }

    }
}