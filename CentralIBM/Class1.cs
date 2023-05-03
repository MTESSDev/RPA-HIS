#region assembly Microsoft.HostIntegration.SNA.Session, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.SNA.Session.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Collections.Generic;

namespace Microsoft.HostIntegration.SNA.Session
{
    public class NavigationAction2
    {
        private string currencySymbol;

        private bool needsCallContext;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public NavigationPlan Plan { get; private set; }

        public List<NavigationParameter> ParameterDefinitions { get; private set; }

        public int NumberOfParametersRequired { get; private set; }

        public List<NavigationMenuUsage> MenuUsages { get; private set; }

        public NavigationAction2(string name, string description, NavigationPlan plan, List<NavigationParameter> parameters)
            : this(name, description, plan, parameters, null, menusCanBeNull: true)
        {
        }

        public NavigationAction2(string name, string description, NavigationPlan plan, List<NavigationParameter> parameters, List<NavigationMenuUsage> menuUsages)
            : this(name, description, plan, parameters, menuUsages, menusCanBeNull: false)
        {
        }

        private NavigationAction2(string name, string description, NavigationPlan plan, List<NavigationParameter> parameters, List<NavigationMenuUsage> menuUsages, bool menusCanBeNull)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (plan == null)
            {
                throw new ArgumentNullException("plan");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (!menusCanBeNull && (menuUsages == null || menuUsages.Count == 0))
            {
                throw new ArgumentNullException("menus");
            }

            int num = 0;
            int num2 = -1;
            HashSet<NavigationPlanScreen> hashSet = new HashSet<NavigationPlanScreen>();
            foreach (NavigationParameter parameter in parameters)
            {
                if (parameter.IsComparisonValue)
                {
                    if (parameter.ParameterNumber > num2)
                    {
                        num2 = parameter.ParameterNumber;
                    }

                    continue;
                }

                if (!plan.PlanScreens.Contains(parameter.PlanScreen))
                {
                    throw new ArgumentException("parameters");
                }

                if (parameter.ParameterContainsDecimal)
                {
                    if (currencySymbol == null)
                    {
                        currencySymbol = parameter.CurrencySymbol;
                    }

                    if (currencySymbol != parameter.CurrencySymbol)
                    {
                        throw new ArgumentException("parameters");
                    }
                }

                /*if (parameter.ParameterNumber == -1)
                {
                    parameter.ParameterNumber = num;
                }*/

                if (parameter.ParameterNumber > num2)
                {
                    num2 = parameter.ParameterNumber;
                }

                if (parameter.ArrayDefinition != null)
                {
                    needsCallContext = true;
                    if (!hashSet.Contains(parameter.PlanScreen))
                    {
                        hashSet.Add(parameter.PlanScreen);
                    }
                }

                num++;
            }

            if (num2 != -1)
            {
                bool[] array = new bool[num2 + 1];
                foreach (NavigationParameter parameter2 in parameters)
                {
                    array[parameter2.ParameterNumber] = true;
                }

                bool[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    if (!array2[i])
                    {
                        throw new ArgumentException("parameters");
                    }
                }
            }

            if (menuUsages != null)
            {
                List<NavigationMenu> list = new List<NavigationMenu>(menuUsages.Count);
                foreach (NavigationMenuUsage menuUsage in menuUsages)
                {
                    if (list.Contains(menuUsage.Menu))
                    {
                        throw new ArgumentException("menuUsages");
                    }

                    list.Add(menuUsage.Menu);
                    foreach (NavigationPlanScreen planScreen in menuUsage.PlanScreens)
                    {
                        if (!plan.PlanScreens.Contains(planScreen))
                        {
                            throw new ArgumentException("menuUsage");
                        }

                        if (hashSet.Contains(planScreen))
                        {
                            throw new ArgumentException("menuUsage");
                        }

                        foreach (NavigationMenuUsage menuUsage2 in menuUsages)
                        {
                            if (menuUsage2 == menuUsage)
                            {
                                continue;
                            }

                            foreach (NavigationPlanScreen planScreen2 in menuUsage2.PlanScreens)
                            {
                                if (planScreen2 == planScreen)
                                {
                                    throw new ArgumentException("menuUsage");
                                }
                            }
                        }
                    }

                    //List<int> menuParameterNumbersToActionParameterNumbers = menuUsage.MenuParameterNumbersToActionParameterNumbers;
                    //List<NavigationParameterDataType> parameterDataTypes = menuUsage.Menu.ParameterDataTypes;
                    /*if (menuParameterNumbersToActionParameterNumbers != null)
                    {
                        for (int j = 0; j < menuParameterNumbersToActionParameterNumbers.Count; j++)
                        {
                            int num3 = menuParameterNumbersToActionParameterNumbers[j];
                            if (num3 > parameters.Count)
                            {
                                throw new ArgumentOutOfRangeException("menuUsage");
                            }

                            NavigationParameterDataType navigationParameterDataType = parameterDataTypes[j];
                            if (navigationParameterDataType != NavigationParameterDataType.Unknown && parameters[num3].DataType != navigationParameterDataType)
                            {
                                throw new ArgumentException("menuUsage");
                            }
                        }
                    }*/

                    //string text = menuUsage.Menu.CurrencySymbol;
                    /*if (currencySymbol != null)
                    {
                        if (text != currencySymbol)
                        {
                            throw new ArgumentException("menuUsage");
                        }
                    }
                    else
                    {
                        currencySymbol = text;
                    }*/
                }
            }

            Name = name;
            Description = description;
            Plan = plan;
            ParameterDefinitions = parameters;
            NumberOfParametersRequired = num2 + 1;
            MenuUsages = menuUsages;
        }

        public virtual void Run(string connectionString, ref SessionDisplay sessionDisplay, ref NavigationPlanScreen lastPlanScreen, object[] parameters, int timeout, NavigationProgressHandler progressHandler)
        {
            if (parameters.Length != NumberOfParametersRequired)
            {
                throw new ArgumentOutOfRangeException("parameters");
            }

            NavigationCallContext callContext = (needsCallContext ? new NavigationCallContext(NumberOfParametersRequired, MenuUsages) : ((MenuUsages != null) ? new NavigationCallContext(MenuUsages) : null));
            Plan.Run(connectionString, currencySymbol, ref sessionDisplay, ref lastPlanScreen, ParameterDefinitions, parameters, MenuUsages, timeout, progressHandler, callContext);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
#if false // Journal de décompilation
'321' éléments dans le cache
------------------
Résoudre : 'System.Runtime, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.2.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.Tracing.Globals, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.Tracing.Globals, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.Tracing.Globals.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.Tracing.Containers, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.Tracing.Containers, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.Tracing.Containers.dll'
------------------
Résoudre : 'System.Runtime.Extensions, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Runtime.Extensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.2.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.Extensions.dll'
------------------
Résoudre : 'System.Xml.ReaderWriter, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Xml.ReaderWriter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.2.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.ReaderWriter.dll'
------------------
Résoudre : 'System.IO.Compression, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Un seul assembly trouvé : 'System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.2.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.dll'
------------------
Résoudre : 'System.Threading.Timer, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Threading.Timer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.1.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Timer.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.CounterTelemetry.Containers, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.CounterTelemetry.Containers, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.CounterTelemetry.Containers.dll'
------------------
Résoudre : 'System.IO.FileSystem, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.IO.FileSystem, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.1.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.dll'
------------------
Résoudre : 'System.Collections, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.1.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Collections.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.Nls, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.Nls, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.Nls.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.SiAutomatons, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.SiAutomatons, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.SiAutomatons.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.EventLogging.Containers, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.EventLogging.Containers, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.EventLogging.Containers.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.Common.BasePrimitiveConverter, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.Common.BasePrimitiveConverter, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.Common.BasePrimitiveConverter.dll'
------------------
Résoudre : 'System.Resources.ResourceManager, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Resources.ResourceManager, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.1.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Resources.ResourceManager.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.Tracing.Runtime, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.Tracing.Runtime, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.Tracing.Runtime.dll'
------------------
Résoudre : 'System.Threading.Thread, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Threading.Thread, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.1.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.EventLogging.Globals, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.EventLogging.Globals, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.EventLogging.Globals.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.AutomatonDriver, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.AutomatonDriver, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.AutomatonDriver.dll'
------------------
Résoudre : 'System.Threading, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
AVERTISSEMENT : Incompatibilité de version. Attendu : '4.1.2.0'. Reçu : '6.0.0.0'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.dll'
------------------
Résoudre : 'Microsoft.HostIntegration.Common.Globals, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Un seul assembly trouvé : 'Microsoft.HostIntegration.Common.Globals, Version=1.0.2069.19, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Charger à partir de : 'C:\Users\cotda05\.nuget\packages\microsoft.hostintegration.logicapps.si3270\1.0.2069.19\lib\netcoreapp3.1\Microsoft.HostIntegration.Common.Globals.dll'
------------------
Résoudre : 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Un seul assembly trouvé : 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Charger à partir de : 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.dll'
#endif
