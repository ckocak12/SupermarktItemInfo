
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using Models;
using SupermarktItemInfo;

namespace Util
{
    public static class HtmlHelper
    {
        private static readonly HttpClient client = new HttpClient();

        public static string GetHtml(string Url)
        {
            string responseString = client.GetStringAsync(Url).Result;
            return responseString;
        }

        public static string ParseHtml(string Url, string ClassToGet)
        {
            string value = string.Empty;
            try
            {
                var document = new HtmlDocument();
                document.LoadHtml(GetHtml(Url));
                foreach (HtmlNode node in document.DocumentNode.SelectNodes("//div[@class='" + ClassToGet + "']"))
                {
                    value = node.InnerText;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return value;
        }
        public static List<ItemDetail> SearchResultList(string keyword) //devam sayfaları için de ekle
        {
            List<ItemDetail> value = new List<ItemDetail>();
            var document = new HtmlDocument();
            document.LoadHtml(GetHtml(string.Format(Constants.SEARCH_BASE_URL,keyword.Replace(" ","%20"))));
            foreach (HtmlNode node in document.DocumentNode.SelectNodes("//article[@class='" + Constants.SEARCH_RESULTS_CLASS_NAME + "']"))
            {
                try
                {
                    string priceString = string.Empty;
                    string oldPriceString = string.Empty;
                    decimal price = 0;
                    decimal oldPrice = 0;
                    bool isDiscounted = false;
                    string name = node.SelectSingleNode(".//span[@class='" + Constants.SEARCH_RESULT_ITEMNAME_CLASS_NAME + "']").InnerText;
                    try
                    {
                        priceString = node.SelectSingleNode(".//div[@class='" + Constants.SEARCH_RESULT_PRICE_CLASS_NAME + "']").InnerText;
                        oldPriceString = priceString;
                    }
                    catch
                    {
                        try
                        {
                            priceString = node.SelectSingleNode(".//div[@class='" + Constants.SEARCH_RESULT_PRICE_CLASS_NAME_KORTING + "']").InnerText;
                            isDiscounted = true;
                            oldPriceString = node.SelectSingleNode(".//div[@class='" + Constants.SEARCH_RESULT_BEFORE_DISCOUNT_CLASS_NAME + "']").InnerText;
                        }
                        catch
                        {
                            Console.WriteLine(node.InnerHtml);
                        }

                    }
                    string imageUrl = node.SelectSingleNode(".//img[@class='" + Constants.SEARCH_RESULT_IMAGE_CLASS_NAME + "']").GetAttributeValue("src", "");

                    decimal.TryParse(priceString, out price);
                    decimal.TryParse(oldPriceString, out oldPrice);
                    value.Add(new ItemDetail()
                    {
                        Name = name,
                        ImageUrl = imageUrl,
                        Price = price,
                        isDiscounted = isDiscounted,
                        PriceBeforeDiscount = oldPrice
                    });
                }
                catch
                {
                    Console.WriteLine(node.InnerHtml);
                }
            }
            return value;
        }
    }
}