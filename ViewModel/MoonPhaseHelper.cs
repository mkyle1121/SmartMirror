using AngleSharp.Html.Dom;
using AngleSharp;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace SmartMirror.ViewModel
{
    public class MoonPhaseHelper
    {
        public static async Task<IHtmlImageElement?> GetMoonPhaseAsync()
        {
            try
            {
                using var client = new HttpClient();
                var moonPhaseRespone = await client.GetStringAsync("https://phasesmoon.com/");
                var config = Configuration.Default;
                using var context = BrowsingContext.New(config);
                using var doc = await context.OpenAsync(req => req.Content(moonPhaseRespone));
                var docImages = doc.QuerySelectorAll("img");
                var docImage = (IHtmlImageElement)docImages.FirstOrDefault(docImage => docImage.OuterHtml.Contains("moonpng"));

                return docImage;
            }
            catch (Exception e)
            {
                var logOutputLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "smartmirror.log");
                var file = new StreamWriter(logOutputLocation, true);
                file.WriteLine(string.Concat(DateTime.Now, e.Message));
                file.Close();
                return null;                
            }
        }
    }
}
