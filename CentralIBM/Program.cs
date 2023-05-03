using Microsoft.HostIntegration.SNA.Session;
using System.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Newtonsoft.Json.Linq;
using Microsoft.HostIntegration.RecordAndPlan.SessionControl;
using System.Collections;

SessionDisplay sessionDisplay = default!;
string connectionString = "TRANSPORT=TN3270;IMPLEMENTATION=MANAGEDTN3270;NUMERICOVERRIDEBEHAVIOR=ENABLED;INVALIDCHARACTERSUPPORT=ALLOW;TN3270SERVER=cics-prod-sw.mes.reseau.intra;TN3270PORT=23;HOSTCODEPAGE=37;DEVICETYPE=IBM-3279-4";
NavigationPlanScreen lastPlanScreen = default!;
NavigationProgressHandler progressHandler;
int timeout = 5000;
progressHandler = new NavigationProgressHandler(new EcouteurDeNavigation());

#region test
/*var reco = new RecognitionDefinitionCollection();
reco.Add(new RecognitionDefinitionInField("CA TPX Gestionnaire de Sessions", 1, 26));

var navScreen = new NavigationScreen("Login", reco);
var empty = new NavigationScreen("Empty", RecognitionDefinitionCollection.EmptyScreen);

var emptyS = new NavigationPlanScreen(empty);
var navSS = new NavigationPlanScreen(navScreen, SessionDisplayKeys.Enter);

navSS.NextScreens.Add(emptyS);

var listScreen = new List<NavigationPlanScreen>() { emptyS, navSS };

var plan = new NavigationPlan(NavigationPlanType.Process, "Login", listScreen);

var param = new List<NavigationParameter>() {
    new NavigationParameter("username",  NavigationParameterDirection.In, NavigationParameterDataType.String,
     new NavigationAtomicValueField(listScreen[1], new NavigationScreenField(1,1,10), NavigationParameterDataType.String), 0)
};
var navMaison = new NavigationAction2("Login", "", plan, param);

object[] parametersn = new object[navMaison.ParameterDefinitions.Count];
parametersn[0] = new JValue("cara694");

navMaison.Run(connectionString,
     ref sessionDisplay, ref lastPlanScreen, parametersn, timeout, progressHandler);*/
#endregion

var file = File.ReadAllText(@"C:\Users\cotda05\OneDrive - Gouv Qc\Documents\FIN\FIN.hidx");
var hidx = Microsoft.HostIntegration.TI.LibraryReader.GetTypelessReader("FIN.hidx", file);
var hidx2 = Microsoft.HostIntegration.TI.LibraryReader.GetTypelessReader("FIN.hidx", file);

var login = hidx.NavigationActions.FirstOrDefault(e => e.Key == "Login").Value;

object[] parameters = new object[login.ParameterDefinitions.Count];
parameters[0] = new JValue("caraXYZ");
parameters[1] = new JValue("--Password--");
parameters[2] = new JValue("u");
parameters[3] = new JValue("L011D04");
parameters[4] = new JValue("");

Console.WriteLine($"Pret?");
Console.ReadKey();

login.Run(connectionString,
     ref sessionDisplay, ref lastPlanScreen, parameters, timeout, progressHandler);

var retour = hidx2.NavigationActions.FirstOrDefault(e => e.Key == "Retour").Value;

object[] parameters2 = new object[retour.ParameterDefinitions.Count];
parameters2[0] = new JValue("COTR01518094"); //CP12
parameters2[1] = new JValue("2"); //beneficiaire
parameters2[2] = new JValue(""); //CP12 enfant
parameters2[3] = new JValue("22"); //annee
parameters2[4] = new JValue("12"); //mois
parameters2[5] = new JValue("05"); //jour
parameters2[6] = new JValue("1"); //noFacture
parameters2[7] = new JValue("123,45"); //montantFacture

retour.Run(connectionString,
     ref sessionDisplay, ref lastPlanScreen, parameters2, timeout, progressHandler);

var fermeture = hidx.NavigationActions.FirstOrDefault(e => e.Key == "Fermer").Value;

object[] parametersfermeture = new object[fermeture.ParameterDefinitions.Count];

fermeture.Run(connectionString,
     ref sessionDisplay, ref lastPlanScreen, parametersfermeture, timeout, progressHandler);

Console.Clear();
Console.WriteLine($"Reponse du central: ");
Console.WriteLine($"code:\t{parameters2[8]}");
Console.WriteLine($"message:\t{parameters2[9]}");
Console.ReadKey();


public class Navigat : NavigationFixedText
{
    //static List<NavigationTextAction> textActions = new List<NavigationTextAction>();
    static Navigat()
    {
    }

    public Navigat(List<NavigationTextAction>  textActions) : base(textActions) { }

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
        Console.ReadKey();

        if (text.Contains("none of the expected next screens"))
            return;
        //Thread.Sleep(500);
    }

    public void DoNavigationStart()
    {
    }
}