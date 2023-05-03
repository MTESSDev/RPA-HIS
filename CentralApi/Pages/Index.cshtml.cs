using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.HostIntegration.SNA.Session;
using System.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace CentralApi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public NavigationAction nav;

        [BindProperty(SupportsGet = true)]
        public string Soumis { get; set; } = String.Empty;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            SessionDisplay sessionDisplay = default!;
            string connectionString = "TRANSPORT=TN3270;IMPLEMENTATION=MANAGEDTN3270;NUMERICOVERRIDEBEHAVIOR=ENABLED;INVALIDCHARACTERSUPPORT=ALLOW;TN3270SERVER=cics-prod-sw.mes.reseau.intra;TN3270PORT=23;HOSTCODEPAGE=37;DEVICETYPE=IBM-3279-4";
            NavigationPlanScreen lastPlanScreen = default!;
            NavigationProgressHandler progressHandler;
            int timeout = 5000;
            progressHandler = new NavigationProgressHandler(new EcouteurDeNavigation());

            var fileConnect = System.IO.File.ReadAllText(@"C:\700\Central\ConnectDisconnect\ConnectDisconnect.hidx");
            var file = System.IO.File.ReadAllText(@"C:\Users\cotda05\OneDrive - Gouv Qc\Documents\FIN\FIN.hidx");
            var hidxConnect = Microsoft.HostIntegration.TI.LibraryReader.GetTypelessReader("ConnectDisconnect.hidx", fileConnect);
            var hidxTraitement = Microsoft.HostIntegration.TI.LibraryReader.GetTypelessReader("FIN.hidx", file);

            nav = hidxTraitement.NavigationActions.FirstOrDefault(e => e.Key == "Retour").Value;

            if(Soumis.Equals("oui"))
            {
                var login = hidxConnect.NavigationActions.FirstOrDefault(e => e.Key == "Login").Value;

                object[] parameters = new object[login.ParameterDefinitions.Count];
                parameters[0] = new JValue("caraXYZ");
                parameters[1] = new JValue("---Password---");
                parameters[2] = new JValue("u");
                parameters[3] = new JValue("L011D04");
                parameters[4] = new JValue("");


                login.Run(connectionString,
                     ref sessionDisplay, ref lastPlanScreen, parameters, timeout, progressHandler);


                login.Plan.PlanScreens[5] = nav.Plan.PlanScreens[1];
                //nav.Plan.PlanScreens[3] = login.Plan.PlanScreens[2];

                object[] parameters2 = new object[nav.ParameterDefinitions.Count];
                parameters2[0] = new JValue("COTR01518094"); //CP12
                parameters2[1] = new JValue("2"); //beneficiaire
                parameters2[2] = new JValue(""); //CP12 enfant
                parameters2[3] = new JValue("22"); //annee
                parameters2[4] = new JValue("11"); //mois
                parameters2[5] = new JValue("29"); //jour
                parameters2[6] = new JValue("1"); //noFacture
                parameters2[7] = new JValue("99,21"); //montantFacture

                nav.Run(connectionString,
                     ref sessionDisplay, ref lastPlanScreen, parameters2, timeout, progressHandler);

                var fermeture = hidxConnect.NavigationActions.FirstOrDefault(e => e.Key == "Fermer").Value;

                object[] parametersfermeture = new object[fermeture.ParameterDefinitions.Count];

                fermeture.Run(connectionString,
                     ref sessionDisplay, ref lastPlanScreen, parametersfermeture, timeout, progressHandler);
               
            
            }

        }
    }


    internal class EcouteurDeNavigation : INavigationListener
    {
        public static void GetRowsColumns(int size, out int rows, out int columns)
        {
            rows = 27;
            columns = 132;
            if (size != 3564)
            {
                columns = 80;
                rows = size / 80;
            }
        }

        private static Image BitmapFromStringBuilder(string text)
        {
            Image image = new Image<Rgb24>(580, 380); // Create any way you like.
            FontCollection collection = new();
            collection.AddSystemFonts();
            FontFamily family = collection.Get("Consolas");
            Font font = family.CreateFont(12, FontStyle.Regular);

            image.Mutate(x => x.DrawText(text, font, Color.White, new PointF(10, 10)));

            return image;
        }

        public static string ScreenToText(ScreenData data)
        {
            var convertedCharacters = new HostConverter().ConvertEbcdicToUnicode(data.Data);
            GetRowsColumns(data.Data.Length, out var rows, out var columns);
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder(data.Data.Length + Environment.NewLine.Length * rows);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    char value = (((data.ExtendedAttributes[num] & 0x80u) != 0) ? ' ' : convertedCharacters[num]);
                    stringBuilder.Append(value);
                    num++;
                }
                stringBuilder.Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        public void DoNavigationEnd()
        {
        }

        public void DoNavigationProgress(string text, ScreenData data)
        {

            if (data is null) return;

            var txt = ScreenToText(data);
            //BitmapFromStringBuilder(txt);

            Console.Clear();
            Console.Write(txt);
            //Console.ReadKey();

            if (text.Contains("none of the expected next screens"))
                return;
            //Thread.Sleep(500);
        }

        public void DoNavigationStart()
        {
        }
    }
}