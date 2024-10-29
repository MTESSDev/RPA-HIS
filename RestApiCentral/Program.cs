using Microsoft.HostIntegration.SNA.Session;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
app.UseSwagger();
app.UseSwaggerUI();
/*}*/

app.MapPost("/ajouterFactureAmbulance", (string cp12,
                                        string beneficiaire,
                                        string cp12enfant,
                                        string annee,
                                        string mois,
                                        string jour,
                                        string noFacture,
                                        string montantFacture) =>
{
    try
    {
        SessionDisplay sessionDisplay = default!;
        string connectionString = "TRANSPORT=TN3270;IMPLEMENTATION=MANAGEDTN3270;NUMERICOVERRIDEBEHAVIOR=ENABLED;INVALIDCHARACTERSUPPORT=ALLOW;TN3270SERVER=cics-**-sw.mes.reseau.intra;TN3270PORT=23;HOSTCODEPAGE=37;DEVICETYPE=IBM-3279-4";
        NavigationPlanScreen lastPlanScreen = default!;
        NavigationProgressHandler progressHandler;
        int timeout = 5000;
        progressHandler = new NavigationProgressHandler(new EcouteurDeNavigation());

        var file = File.ReadAllText(@"C:\700\FIN.hidx");
        var hidx = Microsoft.HostIntegration.TI.LibraryReader.GetTypelessReader("FIN.hidx", file);
        var hidx2 = Microsoft.HostIntegration.TI.LibraryReader.GetTypelessReader("FIN.hidx", file);

        var login = hidx.NavigationActions.FirstOrDefault(e => e.Key == "Login").Value;

        object[] parameters = new object[login.ParameterDefinitions.Count];
        parameters[0] = new JValue("caraXYZ");
        parameters[1] = new JValue("---Password---");
        parameters[2] = new JValue("u");
        parameters[3] = new JValue("L011D04");
        parameters[4] = new JValue("");

        login.Run(connectionString,
             ref sessionDisplay, ref lastPlanScreen, parameters, timeout, progressHandler);

        var retour = hidx2.NavigationActions.FirstOrDefault(e => e.Key == "Retour").Value;

        object[] parameters2 = new object[retour.ParameterDefinitions.Count];
        parameters2[0] = new JValue(cp12); //CP12 COTR01518094
        parameters2[1] = new JValue(beneficiaire); //beneficiaire
        parameters2[2] = new JValue(cp12enfant); //CP12 enfant
        parameters2[3] = new JValue(annee); //annee
        parameters2[4] = new JValue(mois); //mois
        parameters2[5] = new JValue(jour); //jour
        parameters2[6] = new JValue(noFacture); //noFacture
        parameters2[7] = new JValue(montantFacture); //montantFacture

        retour.Run(connectionString,
            ref sessionDisplay, ref lastPlanScreen, parameters2, timeout, progressHandler);


        var fermeture = hidx.NavigationActions.FirstOrDefault(e => e.Key == "Fermer").Value;

        object[] parametersfermeture = new object[fermeture.ParameterDefinitions.Count];

        fermeture.Run(connectionString,
             ref sessionDisplay, ref lastPlanScreen, parametersfermeture, timeout, progressHandler);

        return new Reponse() { Code = parameters2[8]?.ToString() ?? string.Empty, Message = parameters2[9]?.ToString() ?? string.Empty };

    }
    catch (Exception ex)
    {
        return new Reponse() { Code = "500", Message = ex.Message };
    }
})
.WithName("PostAjouterFactureAmbulance");

app.MapPost("/obtenirCp12DepuisNam", (string nam) =>
{
    switch (nam)
    {
        case "POTS21101517":
            return "COTR01518094";
        case "AUDE21101517":
            return "AUDE02528090";
        case "BENS21101517":
            return "BIDD21101517";
        default:
            return null;
    }
})
.WithName("PostObtenirCp12DepuisNam");

app.Run();


public class Reponse
{

    public string? Code { get; set; }
    public string? Message { get; set; }
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

    /*private static Image BitmapFromStringBuilder(string text)
    {
        Image image = new Image<Rgb24>(580, 380); // Create any way you like.
        FontCollection collection = new();
        collection.AddSystemFonts();
        FontFamily family = collection.Get("Consolas");
        Font font = family.CreateFont(12, FontStyle.Regular);

        image.Mutate(x => x.DrawText(text, font, Color.White, new PointF(10, 10)));

        return image;
    }*/

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

        //var txt = ScreenToText(data);
        //BitmapFromStringBuilder(txt);

        //Console.Clear();
        //Console.Write(txt);
        //Console.ReadKey();

        if (text.Contains("none of the expected next screens"))
            return;
        //Thread.Sleep(500);
    }

    public void DoNavigationStart()
    {
    }
}
