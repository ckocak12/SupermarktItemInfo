namespace Util
{
    public static class UrlHelper
    {
        public static string ExtractItemName(string url)
        {
            string dashedName = url.Split("/")[6];
            return dashedName.Replace("-", " ");
        }
    }
}
