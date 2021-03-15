using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using SupermarktItemInfo;
using Util;

namespace Controllers
{
    public class AlbertHeijnController : ControllerBase
    {
        [HttpPost]
        [Route("api/ah/detail")]
        public ItemDetail GetItemDetail([FromBody] string url)
        {
            decimal price = 0;
            decimal.TryParse(HtmlHelper.ParseHtml(url, Constants.PRICE_CLASS_NAME), out price);
            return new ItemDetail
            {
                Name = UrlHelper.ExtractItemName(url),
                Price = price
            };
        }
        [HttpGet]
        [Route("api/ah/search/{keyword}")]
        public List<ItemDetail> search(string keyword)
        {
            return HtmlHelper.SearchResultList(keyword);
        }
    }
}
