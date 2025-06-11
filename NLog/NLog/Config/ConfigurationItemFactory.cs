// Decompiled with JetBrains decompiler
// Type: NLog.Config.ConfigurationItemFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Filters;
using NLog.Internal;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using NLog.Time;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

#nullable disable
namespace NLog.Config;

public class ConfigurationItemFactory
{
  private static ConfigurationItemFactory _defaultInstance;
  private readonly IFactory[] _allFactories;
  private readonly Factory<Target, TargetAttribute> _targets;
  private readonly Factory<Filter, FilterAttribute> _filters;
  private readonly LayoutRendererFactory _layoutRenderers;
  private readonly Factory<Layout, LayoutAttribute> _layouts;
  private readonly MethodFactory _conditionMethods;
  private readonly Factory<LayoutRenderer, AmbientPropertyAttribute> _ambientProperties;
  private readonly Factory<TimeSource, TimeSourceAttribute> _timeSources;
  private IJsonConverter _jsonSerializer = (IJsonConverter) DefaultJsonSerializer.Instance;
  private IObjectTypeTransformer _objectTypeTransformer = ObjectReflectionCache.Instance;

  public static event EventHandler<AssemblyLoadingEventArgs> AssemblyLoading;

  public ConfigurationItemFactory(params Assembly[] assemblies)
  {
    this.CreateInstance = new ConfigurationItemCreator(FactoryHelper.CreateInstance);
    this._targets = new Factory<Target, TargetAttribute>(this);
    this._filters = new Factory<Filter, FilterAttribute>(this);
    this._layoutRenderers = new LayoutRendererFactory(this);
    this._layouts = new Factory<Layout, LayoutAttribute>(this);
    this._conditionMethods = new MethodFactory((Func<Type, IList<KeyValuePair<string, MethodInfo>>>) (classType => MethodFactory.ExtractClassMethods<ConditionMethodsAttribute, ConditionMethodAttribute>(classType)));
    this._ambientProperties = new Factory<LayoutRenderer, AmbientPropertyAttribute>(this);
    this._timeSources = new Factory<TimeSource, TimeSourceAttribute>(this);
    this._allFactories = new IFactory[7]
    {
      (IFactory) this._targets,
      (IFactory) this._filters,
      (IFactory) this._layoutRenderers,
      (IFactory) this._layouts,
      (IFactory) this._conditionMethods,
      (IFactory) this._ambientProperties,
      (IFactory) this._timeSources
    };
    foreach (Assembly assembly in assemblies)
      this.RegisterItemsFromAssembly(assembly);
  }

  public static ConfigurationItemFactory Default
  {
    get
    {
      return ConfigurationItemFactory._defaultInstance ?? (ConfigurationItemFactory._defaultInstance = ConfigurationItemFactory.BuildDefaultFactory());
    }
    set => ConfigurationItemFactory._defaultInstance = value;
  }

  public ConfigurationItemCreator CreateInstance { get; set; }

  public INamedItemFactory<Target, Type> Targets => (INamedItemFactory<Target, Type>) this._targets;

  public INamedItemFactory<Filter, Type> Filters => (INamedItemFactory<Filter, Type>) this._filters;

  internal LayoutRendererFactory GetLayoutRenderers() => this._layoutRenderers;

  public INamedItemFactory<LayoutRenderer, Type> LayoutRenderers
  {
    get => (INamedItemFactory<LayoutRenderer, Type>) this._layoutRenderers;
  }

  public INamedItemFactory<Layout, Type> Layouts => (INamedItemFactory<Layout, Type>) this._layouts;

  public INamedItemFactory<LayoutRenderer, Type> AmbientProperties
  {
    get => (INamedItemFactory<LayoutRenderer, Type>) this._ambientProperties;
  }

  [Obsolete("Use JsonConverter property instead. Marked obsolete on NLog 4.5")]
  public IJsonSerializer JsonSerializer
  {
    get => this._jsonSerializer as IJsonSerializer;
    set
    {
      this._jsonSerializer = value != null ? (IJsonConverter) new JsonConverterLegacy(value) : (IJsonConverter) DefaultJsonSerializer.Instance;
    }
  }

  public IJsonConverter JsonConverter
  {
    get => this._jsonSerializer;
    set => this._jsonSerializer = value ?? (IJsonConverter) DefaultJsonSerializer.Instance;
  }

  public IValueFormatter ValueFormatter
  {
    get => NLog.MessageTemplates.ValueFormatter.Instance;
    set => NLog.MessageTemplates.ValueFormatter.Instance = value;
  }

  internal IObjectTypeTransformer ObjectTypeTransformer
  {
    get => this._objectTypeTransformer;
    set => this._objectTypeTransformer = value ?? ObjectReflectionCache.Instance;
  }

  public IPropertyTypeConverter PropertyTypeConverter { get; set; } = (IPropertyTypeConverter) new NLog.Config.PropertyTypeConverter();

  public bool? ParseMessageTemplates
  {
    get
    {
      if (LogEventInfo.DefaultMessageFormatter == LogEventInfo.StringFormatMessageFormatter)
        return new bool?(false);
      return LogEventInfo.DefaultMessageFormatter == LogMessageTemplateFormatter.Default.MessageFormatter ? new bool?(true) : new bool?();
    }
    set => LogEventInfo.SetDefaultMessageFormatter(value);
  }

  public INamedItemFactory<TimeSource, Type> TimeSources
  {
    get => (INamedItemFactory<TimeSource, Type>) this._timeSources;
  }

  public INamedItemFactory<MethodInfo, MethodInfo> ConditionMethods
  {
    get => (INamedItemFactory<MethodInfo, MethodInfo>) this._conditionMethods;
  }

  internal MethodFactory ConditionMethodDelegates => this._conditionMethods;

  public void RegisterItemsFromAssembly(Assembly assembly)
  {
    this.RegisterItemsFromAssembly(assembly, string.Empty);
  }

  public void RegisterItemsFromAssembly(Assembly assembly, string itemNamePrefix)
  {
    if (ConfigurationItemFactory.AssemblyLoading != null)
    {
      AssemblyLoadingEventArgs e = new AssemblyLoadingEventArgs(assembly);
      ConfigurationItemFactory.AssemblyLoading((object) null, e);
      if (e.Cancel)
      {
        InternalLogger.Info<string>("Loading assembly '{0}' is canceled", assembly.FullName);
        return;
      }
    }
    InternalLogger.Debug<string>("ScanAssembly('{0}')", assembly.FullName);
    Type[] types = assembly.SafeGetTypes();
    this.PreloadAssembly(types);
    foreach (IFactory allFactory in this._allFactories)
      allFactory.ScanTypes(types, itemNamePrefix);
  }

  public void PreloadAssembly(Type[] typesToScan)
  {
    foreach (Type type in ((IEnumerable<Type>) typesToScan).Where<Type>((Func<Type, bool>) (t => t.Name.Equals("NLogPackageLoader", StringComparison.OrdinalIgnoreCase))))
      this.CallPreload(type);
  }

  private void CallPreload(Type type)
  {
    if (type == (Type) null)
      return;
    InternalLogger.Debug<string>("Found for preload'{0}'", type.FullName);
    MethodInfo method = type.GetMethod("Preload");
    if (method != (MethodInfo) null)
    {
      if (method.IsStatic)
      {
        InternalLogger.Debug("NLogPackageLoader contains Preload method");
        try
        {
          object[] preloadParameters = ConfigurationItemFactory.CreatePreloadParameters(method, this);
          method.Invoke((object) null, preloadParameters);
          InternalLogger.Debug<string>("Preload successfully invoked for '{0}'", type.FullName);
        }
        catch (Exception ex)
        {
          object[] objArray = new object[1]
          {
            (object) type.FullName
          };
          InternalLogger.Warn(ex, "Invoking Preload for '{0}' failed", objArray);
        }
      }
      else
        InternalLogger.Debug("NLogPackageLoader contains a preload method, but isn't static");
    }
    else
      InternalLogger.Debug<string>("{0} doesn't contain Preload method", type.FullName);
  }

  private static object[] CreatePreloadParameters(
    MethodInfo preloadMethod,
    ConfigurationItemFactory configurationItemFactory)
  {
    ParameterInfo parameterInfo = ((IEnumerable<ParameterInfo>) preloadMethod.GetParameters()).FirstOrDefault<ParameterInfo>();
    object[] preloadParameters = (object[]) null;
    if (parameterInfo?.ParameterType == typeof (ConfigurationItemFactory))
      preloadParameters = new object[1]
      {
        (object) configurationItemFactory
      };
    return preloadParameters;
  }

  public void Clear()
  {
    foreach (IFactory allFactory in this._allFactories)
      allFactory.Clear();
  }

  public void RegisterType(Type type, string itemNamePrefix)
  {
    foreach (IFactory allFactory in this._allFactories)
      allFactory.RegisterType(type, itemNamePrefix);
  }

  private static ConfigurationItemFactory BuildDefaultFactory()
  {
    Assembly assembly = typeof (ILogger).GetAssembly();
    ConfigurationItemFactory factory = new ConfigurationItemFactory(new Assembly[1]
    {
      assembly
    });
    factory.RegisterExtendedItems();
    try
    {
      string str = string.Empty;
      string[] extensionDlls = ArrayHelper.Empty<string>();
      foreach (KeyValuePair<string, Assembly> loadingFileLocation in ConfigurationItemFactory.GetAutoLoadingFileLocations())
      {
        if (!string.IsNullOrEmpty(loadingFileLocation.Key))
        {
          if (string.IsNullOrEmpty(str))
            str = loadingFileLocation.Key;
          extensionDlls = ConfigurationItemFactory.GetNLogExtensionFiles(loadingFileLocation.Key);
          if (extensionDlls.Length != 0)
          {
            str = loadingFileLocation.Key;
            break;
          }
        }
      }
      InternalLogger.Debug<string>("Start auto loading, location: {0}", str);
      ConfigurationItemFactory.LoadNLogExtensionAssemblies(factory, assembly, extensionDlls);
    }
    catch (SecurityException ex)
    {
      InternalLogger.Warn((Exception) ex, "Seems that we do not have permission");
      if (ex.MustBeRethrown())
        throw;
    }
    catch (UnauthorizedAccessException ex)
    {
      InternalLogger.Warn((Exception) ex, "Seems that we do not have permission");
      if (ex.MustBeRethrown())
        throw;
    }
    InternalLogger.Debug("Auto loading done");
    return factory;
  }

  private static void LoadNLogExtensionAssemblies(
    ConfigurationItemFactory factory,
    Assembly nlogAssembly,
    string[] extensionDlls)
  {
    HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      nlogAssembly.FullName
    };
    foreach (string extensionDll in extensionDlls)
    {
      InternalLogger.Info<string>("Auto loading assembly file: {0}", extensionDll);
      bool flag = false;
      try
      {
        Assembly assembly = AssemblyHelpers.LoadFromPath(extensionDll);
        InternalLogger.LogAssemblyVersion(assembly);
        factory.RegisterItemsFromAssembly(assembly);
        stringSet.Add(assembly.FullName);
        flag = true;
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        object[] objArray = new object[1]
        {
          (object) extensionDll
        };
        InternalLogger.Warn(ex, "Auto loading assembly file: {0} failed! Skipping this file.", objArray);
      }
      if (flag)
        InternalLogger.Info<string>("Auto loading assembly file: {0} succeeded!", extensionDll);
    }
    foreach (Assembly assembly in LogFactory.CurrentAppDomain.GetAssemblies())
    {
      if (assembly.FullName.StartsWith("NLog.", StringComparison.OrdinalIgnoreCase) && !stringSet.Contains(assembly.FullName))
        factory.RegisterItemsFromAssembly(assembly);
      if (assembly.FullName.StartsWith("NLog.Extensions.Logging,", StringComparison.OrdinalIgnoreCase) || assembly.FullName.StartsWith("NLog.Web,", StringComparison.OrdinalIgnoreCase) || assembly.FullName.StartsWith("NLog.Web.AspNetCore,", StringComparison.OrdinalIgnoreCase) || assembly.FullName.StartsWith("Microsoft.Extensions.Logging,", StringComparison.OrdinalIgnoreCase) || assembly.FullName.StartsWith("Microsoft.Extensions.Logging.Abstractions,", StringComparison.OrdinalIgnoreCase) || assembly.FullName.StartsWith("Microsoft.Extensions.Logging.Filter,", StringComparison.OrdinalIgnoreCase) || assembly.FullName.StartsWith("Microsoft.Logging,", StringComparison.OrdinalIgnoreCase))
        LogManager.AddHiddenAssembly(assembly);
    }
  }

  internal static IEnumerable<KeyValuePair<string, Assembly>> GetAutoLoadingFileLocations()
  {
    Assembly assembly = typeof (ILogger).GetAssembly();
    string assemblyLocation = PathHelpers.TrimDirectorySeparators(AssemblyHelpers.GetAssemblyFileLocation(assembly));
    InternalLogger.Debug<string>("Auto loading based on NLog-Assembly found location: {0}", assemblyLocation);
    if (!string.IsNullOrEmpty(assemblyLocation))
      yield return new KeyValuePair<string, Assembly>(assemblyLocation, assembly);
    Assembly entryAssembly = Assembly.GetEntryAssembly();
    string str1 = PathHelpers.TrimDirectorySeparators(AssemblyHelpers.GetAssemblyFileLocation(entryAssembly));
    InternalLogger.Debug<string>("Auto loading based on GetEntryAssembly-Assembly found location: {0}", str1);
    if (!string.IsNullOrEmpty(str1) && !string.Equals(str1, assemblyLocation, StringComparison.OrdinalIgnoreCase))
      yield return new KeyValuePair<string, Assembly>(str1, entryAssembly);
    string str2 = PathHelpers.TrimDirectorySeparators(LogFactory.CurrentAppDomain.BaseDirectory);
    InternalLogger.Debug<string>("Auto loading based on AppDomain-BaseDirectory found location: {0}", str2);
    if (!string.IsNullOrEmpty(str2) && !string.Equals(str2, assemblyLocation, StringComparison.OrdinalIgnoreCase))
      yield return new KeyValuePair<string, Assembly>(str2, (Assembly) null);
  }

  private static string[] GetNLogExtensionFiles(string assemblyLocation)
  {
    try
    {
      InternalLogger.Debug<string>("Search for auto loading files in location: {0}", assemblyLocation);
      return string.IsNullOrEmpty(assemblyLocation) ? ArrayHelper.Empty<string>() : ((IEnumerable<string>) Directory.GetFiles(assemblyLocation, "NLog*.dll")).Select<string, string>(new Func<string, string>(Path.GetFileName)).Where<string>((Func<string, bool>) (x => !x.Equals("NLog.dll", StringComparison.OrdinalIgnoreCase))).Where<string>((Func<string, bool>) (x => !x.Equals("NLog.UnitTests.dll", StringComparison.OrdinalIgnoreCase))).Where<string>((Func<string, bool>) (x => !x.Equals("NLog.Extended.dll", StringComparison.OrdinalIgnoreCase))).Select<string, string>((Func<string, string>) (x => Path.Combine(assemblyLocation, x))).ToArray<string>();
    }
    catch (DirectoryNotFoundException ex)
    {
      InternalLogger.Warn((Exception) ex, "Skipping auto loading location because assembly directory does not exist: {0}", (object) assemblyLocation);
      if (!ex.MustBeRethrown())
        return ArrayHelper.Empty<string>();
      throw;
    }
    catch (SecurityException ex)
    {
      InternalLogger.Warn((Exception) ex, "Skipping auto loading location because access not allowed to assembly directory: {0}", (object) assemblyLocation);
      if (!ex.MustBeRethrown())
        return ArrayHelper.Empty<string>();
      throw;
    }
    catch (UnauthorizedAccessException ex)
    {
      InternalLogger.Warn((Exception) ex, "Skipping auto loading location because access not allowed to assembly directory: {0}", (object) assemblyLocation);
      if (!ex.MustBeRethrown())
        return ArrayHelper.Empty<string>();
      throw;
    }
  }

  private void RegisterExtendedItems()
  {
    string assemblyQualifiedName = typeof (ILogger).AssemblyQualifiedName;
    string str = "NLog,";
    int? nullable1 = assemblyQualifiedName?.IndexOf(str, StringComparison.OrdinalIgnoreCase);
    int? nullable2 = nullable1;
    int num = 0;
    if (nullable2.GetValueOrDefault() >= num & nullable2.HasValue)
      this._targets.RegisterNamedType("MSMQ", $"{typeof (DebugTarget).Namespace}.MessageQueueTarget{", NLog.Extended," + assemblyQualifiedName.Substring(nullable1.Value + str.Length)}");
    this._layoutRenderers.RegisterNamedType("configsetting", "NLog.Extensions.Logging.ConfigSettingLayoutRenderer, NLog.Extensions.Logging");
  }
}
