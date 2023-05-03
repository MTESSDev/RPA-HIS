using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.HostIntegration.SNA.Session;

namespace ScreenScraping3270
{
    // this file contains all the singleton definitions for Screens, PlanScreens, Plans, Structures and Actions
    #region Screens
    public class CICSLOGONScreen : NavigationScreen
    {
        static readonly Lazy<CICSLOGONScreen> _instance = new Lazy<CICSLOGONScreen>(() => new CICSLOGONScreen());

        static string name = "CICSLOGON";
        static short screenSize = 3440;
        static RecognitionDefinitionCollection recognitionDefinitions = new RecognitionDefinitionCollection();
        static CICSLOGONScreen()
        {
            recognitionDefinitions.Add(new RecognitionDefinitionInField("CICS SIGN-ON", 1, 26));
        }

        public CICSLOGONScreen() : base(name, recognitionDefinitions, screenSize) { }

        public static NavigationScreen Instance { get { return _instance.Value; } }
    }
    public class EmptyScreen : NavigationScreen
    {
        static readonly Lazy<EmptyScreen> _instance = new Lazy<EmptyScreen>(() => new EmptyScreen());

        static string name = "Empty";
        static short screenSize = 1920;
        static RecognitionDefinitionCollection recognitionDefinitions = RecognitionDefinitionCollection.EmptyScreen;

        public EmptyScreen() : base(name, recognitionDefinitions, screenSize) { }

        public static NavigationScreen Instance { get { return _instance.Value; } }
    }
    public class HomeScreen : NavigationScreen
    {
        static readonly Lazy<HomeScreen> _instance = new Lazy<HomeScreen>(() => new HomeScreen());

        static string name = "Home";
        static short screenSize = 1920;
        static RecognitionDefinitionCollection recognitionDefinitions = new RecognitionDefinitionCollection();
        static HomeScreen()
        {
            recognitionDefinitions.Add(new RecognitionDefinitionInField("CA TPX Gestionnaire de Sessions", 1, 26));
        }

        public HomeScreen() : base(name, recognitionDefinitions, screenSize) { }

        public static NavigationScreen Instance { get { return _instance.Value; } }
    }
    public class SIGNScreen : NavigationScreen
    {
        static readonly Lazy<SIGNScreen> _instance = new Lazy<SIGNScreen>(() => new SIGNScreen());

        static string name = "SIGN";
        static short screenSize = 3440;
        static RecognitionDefinitionCollection recognitionDefinitions = new RecognitionDefinitionCollection();
        static SIGNScreen()
        {
            recognitionDefinitions.Add(new RecognitionDefinitionInField("SIGN", 1, 2));
            recognitionDefinitions.Add(new RecognitionDefinitionInField("UTD1162", 2, 74));
        }

        public SIGNScreen() : base(name, recognitionDefinitions, screenSize) { }

        public static NavigationScreen Instance { get { return _instance.Value; } }
    }
    public class TerminalScreen : NavigationScreen
    {
        static readonly Lazy<TerminalScreen> _instance = new Lazy<TerminalScreen>(() => new TerminalScreen());

        static string name = "Terminal";
        static short screenSize = 3440;
        static RecognitionDefinitionCollection recognitionDefinitions = new RecognitionDefinitionCollection();
        static TerminalScreen()
        {
            recognitionDefinitions.Add(new RecognitionDefinitionInField("Terminal:", 1, 2));
        }

        public TerminalScreen() : base(name, recognitionDefinitions, screenSize) { }

        public static NavigationScreen Instance { get { return _instance.Value; } }
    }
    public class TSSScreen : NavigationScreen
    {
        static readonly Lazy<TSSScreen> _instance = new Lazy<TSSScreen>(() => new TSSScreen());

        static string name = "TSS";
        static short screenSize = 3440;
        static RecognitionDefinitionCollection recognitionDefinitions = new RecognitionDefinitionCollection();
        static TSSScreen()
        {
            recognitionDefinitions.Add(new RecognitionDefinitionInField("SIG0045I  SIGN-ON EST COMPLETE", 43, 2));
        }

        public TSSScreen() : base(name, recognitionDefinitions, screenSize) { }

        public static NavigationScreen Instance { get { return _instance.Value; } }
    }
    #endregion

    #region FixedTexts
    public class FixedText_1 : NavigationFixedText
    {
        static readonly Lazy<FixedText_1> _instance = new Lazy<FixedText_1>(() => new FixedText_1());

        static List<NavigationTextAction> textActions = new List<NavigationTextAction>();
        static FixedText_1()
        {
            textActions.Add(new NavigationTextActionCursor(43, 12));
        }

        public FixedText_1() : base(textActions) { }

        public static NavigationFixedText Instance { get { return _instance.Value; } }
    }
    public class FixedText_2 : NavigationFixedText
    {
        static readonly Lazy<FixedText_2> _instance = new Lazy<FixedText_2>(() => new FixedText_2());

        static List<NavigationTextAction> textActions = new List<NavigationTextAction>();
        static FixedText_2()
        {
            textActions.Add(new NavigationTextActionCursor(1, 1));
            textActions.Add(new NavigationTextActionText("cesf logo"));
        }

        public FixedText_2() : base(textActions) { }

        public static NavigationFixedText Instance { get { return _instance.Value; } }
    }
    public class FixedText_3 : NavigationFixedText
    {
        static readonly Lazy<FixedText_3> _instance = new Lazy<FixedText_3>(() => new FixedText_3());

        static List<NavigationTextAction> textActions = new List<NavigationTextAction>();
        static FixedText_3()
        {
            textActions.Add(new NavigationTextActionCursor(21, 28));
        }

        public FixedText_3() : base(textActions) { }

        public static NavigationFixedText Instance { get { return _instance.Value; } }
    }
    public class FixedText_4 : NavigationFixedText
    {
        static readonly Lazy<FixedText_4> _instance = new Lazy<FixedText_4>(() => new FixedText_4());

        static List<NavigationTextAction> textActions = new List<NavigationTextAction>();
        static FixedText_4()
        {
            textActions.Add(new NavigationTextActionCursor(1, 1));
            textActions.Add(new NavigationTextActionText("sign"));
        }

        public FixedText_4() : base(textActions) { }

        public static NavigationFixedText Instance { get { return _instance.Value; } }
    }
    public class FixedText_5 : NavigationFixedText
    {
        static readonly Lazy<FixedText_5> _instance = new Lazy<FixedText_5>(() => new FixedText_5());

        static List<NavigationTextAction> textActions = new List<NavigationTextAction>();
        static FixedText_5()
        {
        }

        public FixedText_5() : base(textActions) { }

        public static NavigationFixedText Instance { get { return _instance.Value; } }
    }
    #endregion

    #region Menus
    #endregion

    #region Plan Screens
    public class AsasAmbPlan_CICSLOGONScreen_1 : NavigationPlanScreen
    {
        static NavigationScreen screen = CICSLOGONScreen.Instance;
        static SessionDisplayKeys aidKey = SessionDisplayKeys.Enter;
        static NavigationFixedText fixedText = FixedText_5.Instance;
        public AsasAmbPlan_CICSLOGONScreen_1() : base(screen, aidKey, fixedText) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_CICSLOGONScreen_1();
    }
    public class AsasAmbPlan_EmptyScreen_1 : NavigationPlanScreen
    {
        static NavigationScreen screen = EmptyScreen.Instance;
        public AsasAmbPlan_EmptyScreen_1() : base(screen) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_EmptyScreen_1();
    }
    public class AsasAmbPlan_EmptyScreen_2 : NavigationPlanScreen
    {
        static NavigationScreen screen = EmptyScreen.Instance;
        static SessionDisplayKeys aidKey = SessionDisplayKeys.Enter;
        static NavigationFixedText fixedText = FixedText_4.Instance;
        public AsasAmbPlan_EmptyScreen_2() : base(screen, aidKey, fixedText) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_EmptyScreen_2();
    }
    public class AsasAmbPlan_HomeScreen_1 : NavigationPlanScreen
    {
        static NavigationScreen screen = HomeScreen.Instance;
        static SessionDisplayKeys aidKey = SessionDisplayKeys.Enter;
        static NavigationFixedText fixedText = FixedText_3.Instance;
        public AsasAmbPlan_HomeScreen_1() : base(screen, aidKey, fixedText) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_HomeScreen_1();
    }
    public class AsasAmbPlan_SIGNScreen_1 : NavigationPlanScreen
    {
        static NavigationScreen screen = SIGNScreen.Instance;
        static SessionDisplayKeys aidKey = SessionDisplayKeys.Enter;
        static NavigationFixedText fixedText = FixedText_5.Instance;
        public AsasAmbPlan_SIGNScreen_1() : base(screen, aidKey, fixedText) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_SIGNScreen_1();
    }
    public class AsasAmbPlan_TerminalScreen_1 : NavigationPlanScreen
    {
        static NavigationScreen screen = TerminalScreen.Instance;
        static SessionDisplayKeys aidKey = SessionDisplayKeys.Enter;
        static NavigationFixedText fixedText = FixedText_1.Instance;
        public AsasAmbPlan_TerminalScreen_1() : base(screen, aidKey, fixedText) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_TerminalScreen_1();
    }
    public class AsasAmbPlan_TSSScreen_1 : NavigationPlanScreen
    {
        static NavigationScreen screen = TSSScreen.Instance;
        static SessionDisplayKeys aidKey = SessionDisplayKeys.Clear;
        static NavigationFixedText fixedText = FixedText_5.Instance;
        public AsasAmbPlan_TSSScreen_1() : base(screen, aidKey, fixedText) { }

        public static NavigationPlanScreen Instance = new AsasAmbPlan_TSSScreen_1();
    }
    #endregion

    #region Plans
    public class AsasAmbPlan : NavigationPlan
    {
        static readonly Lazy<AsasAmbPlan> _instance = new Lazy<AsasAmbPlan>(() => new AsasAmbPlan());

        static string name = "asas,amb";
        static List<NavigationPlanScreen> planScreens = new List<NavigationPlanScreen>();
        static AsasAmbPlan()
        {
            // all plan screen definitions
            NavigationPlanScreen nps_1 = AsasAmbPlan_TerminalScreen_1.Instance;
            NavigationPlanScreen nps_2 = AsasAmbPlan_EmptyScreen_1.Instance;
            NavigationPlanScreen nps_3 = AsasAmbPlan_HomeScreen_1.Instance;
            NavigationPlanScreen nps_4 = AsasAmbPlan_EmptyScreen_2.Instance;
            NavigationPlanScreen nps_5 = AsasAmbPlan_SIGNScreen_1.Instance;
            NavigationPlanScreen nps_6 = AsasAmbPlan_CICSLOGONScreen_1.Instance;
            NavigationPlanScreen nps_7 = AsasAmbPlan_TSSScreen_1.Instance;

            // flows from screen to screen
            nps_1.NextScreens.Add(nps_5);
            nps_1.NextScreens.Add(nps_4);
            nps_3.NextScreens.Add(nps_1);
            nps_4.NextScreens.Add(nps_6);
            nps_5.NextScreens.Add(nps_6);
            nps_6.NextScreens.Add(nps_7);
            nps_7.NextScreens.Add(nps_2);

            // collect them
            planScreens.Add(nps_1);
            planScreens.Add(nps_2);
            planScreens.Add(nps_3);
            planScreens.Add(nps_4);
            planScreens.Add(nps_5);
            planScreens.Add(nps_6);
            planScreens.Add(nps_7);
        }

        public AsasAmbPlan() : base(NavigationPlanType.Process, name, planScreens) { }

        public static NavigationPlan Instance { get { return _instance.Value; } }
    }
    #endregion

    #region Structures
    #endregion

    #region Actions
    public class LoginAction : NavigationAction
    {
        static readonly Lazy<LoginAction> _instance = new Lazy<LoginAction>(() => new LoginAction());

        static string name = "login";
        static string description = "";
        static List<NavigationParameter> parameters = new List<NavigationParameter>();
        static NavigationPlan plan = AsasAmbPlan.Instance;

        static LoginAction()
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

        public LoginAction() : base(name, description, plan, parameters) { }

        public static NavigationAction Instance { get { return _instance.Value; } }
    }
    #endregion
}
