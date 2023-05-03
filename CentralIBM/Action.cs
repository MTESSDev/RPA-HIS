using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.HostIntegration.SNA.Session;

namespace ScreenScraping3270
{

    #region Menus
    #endregion

    #region Plans
    public class PlanGenerique : NavigationPlan
    {
        //static readonly Lazy<PlanGenerique> _instance = new Lazy<PlanGenerique>(() => new PlanGenerique());

        static string name = "asas,amb";
        static List<NavigationPlanScreen> planScreens = new List<NavigationPlanScreen>();
        static PlanGenerique()
        {
            var f = new RecognitionDefinitionCollection();
            f.Add(new RecognitionDefinitionInField("CA TPX Gestionnaire de Sessions", 1, 26));
            var screen = new NavigationScreen("a1", f);
            var screen2 = new NavigationScreen("a1");
            // all plan screen definitionss
            NavigationPlanScreen nps_1 = new(screen, SessionDisplayKeys.Enter, "test");
            NavigationPlanScreen nps_2 = new(screen2, SessionDisplayKeys.Enter, "test");
            /*NavigationPlanScreen nps_3 = AsasAmbPlan_HomeScreen_1.Instance;
            NavigationPlanScreen nps_4 = AsasAmbPlan_EmptyScreen_2.Instance;
            NavigationPlanScreen nps_5 = AsasAmbPlan_SIGNScreen_1.Instance;
            NavigationPlanScreen nps_6 = AsasAmbPlan_CICSLOGONScreen_1.Instance;
            NavigationPlanScreen nps_7 = AsasAmbPlan_TSSScreen_1.Instance;*/

            // flows from screen to screen
            /*nps_2.NextScreens.Add(nps_1);
            nps_1.NextScreens.Add(nps_2);*/
            /*nps_1.NextScreens.Add(nps_4);
            nps_3.NextScreens.Add(nps_1);
            nps_4.NextScreens.Add(nps_6);
            nps_5.NextScreens.Add(nps_6);
            nps_6.NextScreens.Add(nps_7);
            nps_7.NextScreens.Add(nps_2);*/

            // collect them
            planScreens.Add(nps_1);
            planScreens.Add(nps_2);
            /*planScreens.Add(nps_2);
            planScreens.Add(nps_3);
            planScreens.Add(nps_4);
            planScreens.Add(nps_5);
            planScreens.Add(nps_6);
            planScreens.Add(nps_7);*/
        }

        public PlanGenerique() : base(NavigationPlanType.Process, name, planScreens) { }

        //public static NavigationPlan Instance { get { return _instance.Value; } }
    }
    #endregion

    #region Structures
    #endregion

    #region Actions
    public class ActionGenerique : NavigationAction
    {
        //static readonly Lazy<LoginAction> _instance = new Lazy<LoginAction>(() => new LoginAction());

        static string name = "login";
        static string description = "";
        static List<NavigationParameter> parameters = new List<NavigationParameter>();
        static NavigationPlan plan = new PlanGenerique();

        static ActionGenerique()
        {
            parameters.Add(
                new NavigationParameter("utilisateur", NavigationParameterDirection.In, NavigationParameterDataType.String,
                new NavigationAtomicValueField(AsasAmbPlan_HomeScreen_1.Instance, new NavigationScreenField(19, 25, 7), NavigationParameterDataType.String),
                0)
                );
            parameters.Add(
                new NavigationParameter("motdepasse", NavigationParameterDirection.In, NavigationParameterDataType.String,
                new NavigationAtomicValueField(AsasAmbPlan_HomeScreen_1.Instance, new NavigationScreenField(20, 25, 8), NavigationParameterDataType.String),
                1)
                );
            parameters.Add(
                new NavigationParameter("environnement", NavigationParameterDirection.In, NavigationParameterDataType.String,
                new NavigationAtomicValueField(AsasAmbPlan_TerminalScreen_1.Instance, new NavigationScreenField(43, 12, 23), NavigationParameterDataType.String),
                2)
                );
            parameters.Add(
                new NavigationParameter("motdepasse2", NavigationParameterDirection.In, NavigationParameterDataType.String,
                new NavigationAtomicValueField(AsasAmbPlan_CICSLOGONScreen_1.Instance, new NavigationScreenField(4, 11, 20), NavigationParameterDataType.String),
                3)
                );
            parameters.Add(
                new NavigationParameter("motdepasse3", NavigationParameterDirection.In, NavigationParameterDataType.String,
                new NavigationAtomicValueField(AsasAmbPlan_CICSLOGONScreen_1.Instance, new NavigationScreenField(6, 16, 8), NavigationParameterDataType.String),
                4)
                );
        }

        public ActionGenerique() : base(name, description, plan, parameters) { }

       // public static NavigationAction Instance { get { return _instance.Value; } }
    }
    #endregion
}
