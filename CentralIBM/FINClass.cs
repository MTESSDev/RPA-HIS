using System;

using Microsoft.HostIntegration.SNA.Session;

namespace ScreenScraping3270
{
    public class Actions
    {
        SessionDisplay sessionDisplay;
        string connectionString;
        NavigationPlanScreen lastPlanScreen;
        public NavigationProgressHandler ProgressHandler { get; set; }
        int timeout;

        public Actions(string connectionString, int timeout)
        {
            this.connectionString = connectionString;
            this.timeout = timeout;

            /*sessionDisplay = new SessionDisplay();
            lastPlanScreen = new AsasAmbPlan_EmptyScreen_1();*/
            
        }
        public Actions(string connectionString) :
            this(connectionString, 30000)
        {
        }
        public void Disconnect()
        {
            Disconnect(false);
        }
        public void Disconnect(bool force)
        {
            NavigationPlan.Disconnect(ref sessionDisplay, ref lastPlanScreen, force, timeout, ProgressHandler);
        }

        public void Login(string utilisateur, string motdepasse, string environnement, string motdepasse2, string motdepasse3)
        {
            object[] parameters = new object[5];
            parameters[0] = utilisateur;
            parameters[1] = motdepasse;
            parameters[2] = environnement;
            parameters[3] = motdepasse2;
            parameters[4] = motdepasse3;

            new ActionGenerique().Run(connectionString, ref sessionDisplay, ref lastPlanScreen, parameters, timeout, ProgressHandler);

        }
    }
}
