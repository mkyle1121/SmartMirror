using AngleSharp.Html.Dom;
using AngleSharp;
using Newtonsoft.Json;
using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace SmartMirror.ViewModel
{
    internal class TransitPositionsHelper
    {
        private static string[] celestialBodies = new string[10] { "Sun", "Moon", "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto" };
        private static string transitPositionsEndpoint = "https://horoscopes.astro-seek.com/current-planets-astrology-transits-planetary-positions";
        private static string imagePath = "/View/Pictures/TransitPictures/Signs";
        private static List<TransitPosition> currentTransitPositions;
        private static TransitPositions transitPositions;

        public static async Task<TransitPositions?> GetTransitPositionsAsync()
        {
            try
            {
                using var client = new HttpClient();
                var respone = await client.GetStringAsync(transitPositionsEndpoint);
                var config = Configuration.Default;
                using var context = BrowsingContext.New(config);
                using var doc = await context.OpenAsync(req => req.Content(respone));

                var rightPanel = doc.All.Where(x => x.ClassName == "right-sedy-odsazeni").Select(x => x.Children).FirstOrDefault().ToList(); //Get Right Panel With Sign Data                              
                OragnizeHtmlDataIntoTransitPositions(rightPanel);
                OrganizeTransitPositionsIntoFlatTransitPostitions();
                return transitPositions;
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

        private static void OragnizeHtmlDataIntoTransitPositions(List<IElement> PanelOfHtmlData)
        {
            currentTransitPositions = new List<TransitPosition>();

            foreach (var celestialBody in celestialBodies)
            {
                var currentTransitPosition = new TransitPosition();

                currentTransitPosition.Name = celestialBody;
                var currentSign = PanelOfHtmlData.Where(x => x.TextContent == celestialBody).FirstOrDefault(); //Get Current Planet Element
                currentTransitPosition.CurrentSign = ((IHtmlImageElement)currentSign.NextElementSibling.Children.FirstOrDefault()).AlternativeText; //Get Next Element For Sign Name
                currentTransitPosition.CurrentDegrees = currentSign.NextElementSibling.NextElementSibling.TextContent; //Get Next Next Element For Degrees                

                currentTransitPositions.Add(currentTransitPosition);               
            }
        }

        private static void OrganizeTransitPositionsIntoFlatTransitPostitions()
        {
            transitPositions = new TransitPositions();

            foreach (var transitPosition in currentTransitPositions)
            {
                switch (transitPosition.Name)
                {
                    case "Sun":
                        transitPositions.CurrentSunSign = transitPosition.CurrentSign;
                        transitPositions.CurrentSunDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentSunImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Moon":
                        transitPositions.CurrentMoonSign = transitPosition.CurrentSign;
                        transitPositions.CurrentMoonDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentMoonImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";

                        break;
                    case "Mercury":
                        transitPositions.CurrentMercurySign = transitPosition.CurrentSign;
                        transitPositions.CurrentMercuryDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentMercuryImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Venus":
                        transitPositions.CurrentVenusSign = transitPosition.CurrentSign;
                        transitPositions.CurrentVenusDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentVenusImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Mars":
                        transitPositions.CurrentMarsSign = transitPosition.CurrentSign;
                        transitPositions.CurrentMarsDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentMarsImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Jupiter":
                        transitPositions.CurrentJupiterSign = transitPosition.CurrentSign;
                        transitPositions.CurrentJupiterDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentJupiterImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Saturn":
                        transitPositions.CurrentSaturnSign = transitPosition.CurrentSign;
                        transitPositions.CurrentSaturnDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentSaturnImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Uranus":
                        transitPositions.CurrentUranusSign = transitPosition.CurrentSign;
                        transitPositions.CurrentUranusDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentUranusImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Neptune":
                        transitPositions.CurrentNeptuneSign = transitPosition.CurrentSign;
                        transitPositions.CurrentNeptuneDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentNeptuneImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    case "Pluto":
                        transitPositions.CurrentPlutoSign = transitPosition.CurrentSign;
                        transitPositions.CurrentPlutoDegrees = transitPosition.CurrentDegrees;
                        transitPositions.CurrentPlutoImage = $"{imagePath}/{transitPosition.CurrentSign}.gif";
                        break;
                    default:                                              
                        break;
                }
            }
        }
    }
}

