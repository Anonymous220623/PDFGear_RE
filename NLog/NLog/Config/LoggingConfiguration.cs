// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfiguration
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;

#nullable disable
namespace NLog.Config;

public class LoggingConfiguration
{
  private readonly IDictionary<string, Target> _targets = (IDictionary<string, Target>) new Dictionary<string, Target>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private List<object> _configItems = new List<object>();
  private readonly ThreadSafeDictionary<string, SimpleLayout> _variables = new ThreadSafeDictionary<string, SimpleLayout>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private static readonly IEqualityComparer<Target> TargetNameComparer = (IEqualityComparer<Target>) new LoggingConfiguration.TargetNameEqualityComparer();

  public LogFactory LogFactory { get; }

  public LoggingConfiguration()
    : this(LogManager.LogFactory)
  {
  }

  public LoggingConfiguration(LogFactory logFactory)
  {
    this.LogFactory = logFactory ?? LogManager.LogFactory;
    this.LoggingRules = (IList<LoggingRule>) new List<LoggingRule>();
  }

  [Obsolete("This option will be removed in NLog 5. Marked obsolete on NLog 4.1")]
  public bool ExceptionLoggingOldStyle { get; set; }

  public IDictionary<string, SimpleLayout> Variables
  {
    get => (IDictionary<string, SimpleLayout>) this._variables;
  }

  public ReadOnlyCollection<Target> ConfiguredNamedTargets
  {
    get => this.GetAllTargetsThreadSafe().AsReadOnly();
  }

  public virtual IEnumerable<string> FileNamesToWatch
  {
    get => (IEnumerable<string>) ArrayHelper.Empty<string>();
  }

  public IList<LoggingRule> LoggingRules { get; private set; }

  internal List<LoggingRule> GetLoggingRulesThreadSafe()
  {
    lock (this.LoggingRules)
      return this.LoggingRules.ToList<LoggingRule>();
  }

  private void AddLoggingRulesThreadSafe(LoggingRule rule)
  {
    lock (this.LoggingRules)
      this.LoggingRules.Add(rule);
  }

  private bool TryGetTargetThreadSafe(string name, out Target target)
  {
    lock (this._targets)
      return this._targets.TryGetValue(name, out target);
  }

  private List<Target> GetAllTargetsThreadSafe()
  {
    lock (this._targets)
      return this._targets.Values.ToList<Target>();
  }

  private Target RemoveTargetThreadSafe(string name)
  {
    Target target;
    lock (this._targets)
    {
      if (this._targets.TryGetValue(name, out target))
        this._targets.Remove(name);
    }
    if (target != null)
      InternalLogger.Debug<string, string>("Unregistered target {0}: {1}", name, target.GetType().FullName);
    return target;
  }

  private void AddTargetThreadSafe(string name, Target target, bool forceOverwrite)
  {
    if (string.IsNullOrEmpty(name) && !forceOverwrite)
      return;
    lock (this._targets)
    {
      if (!forceOverwrite && this._targets.ContainsKey(name))
        return;
      this._targets[name] = target;
    }
    if (!string.IsNullOrEmpty(target.Name) && target.Name != name)
      InternalLogger.Info<string, string, string>("Registered target {0}: {1} (Target created with different name: {2})", name, target.GetType().FullName, target.Name);
    else
      InternalLogger.Debug<string, string>("Registered target {0}: {1}", name, target.GetType().FullName);
  }

  [CanBeNull]
  public CultureInfo DefaultCultureInfo { get; set; }

  public ReadOnlyCollection<Target> AllTargets
  {
    get
    {
      return this._configItems.OfType<Target>().Concat<Target>((IEnumerable<Target>) this.GetAllTargetsThreadSafe()).Distinct<Target>(LoggingConfiguration.TargetNameComparer).ToList<Target>().AsReadOnly();
    }
  }

  public void AddTarget([NotNull] Target target)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    if (target.Name == null)
      throw new ArgumentNullException("target.Name cannot be null.");
    this.AddTargetThreadSafe(target.Name, target, true);
  }

  public void AddTarget(string name, Target target)
  {
    if (name == null)
      throw new ArgumentException("Target name cannot be null", nameof (name));
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    this.AddTargetThreadSafe(name, target, true);
  }

  public Target FindTargetByName(string name)
  {
    Target target;
    return !this.TryGetTargetThreadSafe(name, out target) ? (Target) null : target;
  }

  public TTarget FindTargetByName<TTarget>(string name) where TTarget : Target
  {
    return this.FindTargetByName(name) as TTarget;
  }

  public void AddRule(
    NLog.LogLevel minLevel,
    NLog.LogLevel maxLevel,
    string targetName,
    string loggerNamePattern = "*")
  {
    this.AddRule(minLevel, maxLevel, this.FindTargetByName(targetName) ?? throw new NLogRuntimeException("Target '{0}' not found", new object[1]
    {
      (object) targetName
    }), loggerNamePattern, false);
  }

  public void AddRule(
    NLog.LogLevel minLevel,
    NLog.LogLevel maxLevel,
    Target target,
    string loggerNamePattern = "*")
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    this.AddRule(minLevel, maxLevel, target, loggerNamePattern, false);
  }

  public void AddRule(
    NLog.LogLevel minLevel,
    NLog.LogLevel maxLevel,
    Target target,
    string loggerNamePattern,
    bool final)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    this.AddLoggingRulesThreadSafe(new LoggingRule(loggerNamePattern, minLevel, maxLevel, target)
    {
      Final = final
    });
    this.AddTargetThreadSafe(target.Name, target, false);
  }

  public void AddRuleForOneLevel(NLog.LogLevel level, string targetName, string loggerNamePattern = "*")
  {
    this.AddRuleForOneLevel(level, this.FindTargetByName(targetName) ?? throw new NLogConfigurationException("Target '{0}' not found", new object[1]
    {
      (object) targetName
    }), loggerNamePattern, false);
  }

  public void AddRuleForOneLevel(NLog.LogLevel level, Target target, string loggerNamePattern = "*")
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    this.AddRuleForOneLevel(level, target, loggerNamePattern, false);
  }

  public void AddRuleForOneLevel(
    NLog.LogLevel level,
    Target target,
    string loggerNamePattern,
    bool final)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    LoggingRule rule = new LoggingRule(loggerNamePattern, target)
    {
      Final = final
    };
    rule.EnableLoggingForLevel(level);
    this.AddLoggingRulesThreadSafe(rule);
    this.AddTargetThreadSafe(target.Name, target, false);
  }

  public void AddRuleForAllLevels(string targetName, string loggerNamePattern = "*")
  {
    this.AddRuleForAllLevels(this.FindTargetByName(targetName) ?? throw new NLogRuntimeException("Target '{0}' not found", new object[1]
    {
      (object) targetName
    }), loggerNamePattern, false);
  }

  public void AddRuleForAllLevels(Target target, string loggerNamePattern = "*")
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    this.AddRuleForAllLevels(target, loggerNamePattern, false);
  }

  public void AddRuleForAllLevels(Target target, string loggerNamePattern, bool final)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    LoggingRule rule = new LoggingRule(loggerNamePattern, target)
    {
      Final = final
    };
    rule.EnableLoggingForLevels(NLog.LogLevel.MinLevel, NLog.LogLevel.MaxLevel);
    this.AddLoggingRulesThreadSafe(rule);
    this.AddTargetThreadSafe(target.Name, target, false);
  }

  public LoggingRule FindRuleByName(string ruleName)
  {
    if (ruleName == null)
      return (LoggingRule) null;
    List<LoggingRule> loggingRulesThreadSafe = this.GetLoggingRulesThreadSafe();
    for (int index = loggingRulesThreadSafe.Count - 1; index >= 0; --index)
    {
      if (string.Equals(loggingRulesThreadSafe[index].RuleName, ruleName, StringComparison.OrdinalIgnoreCase))
        return loggingRulesThreadSafe[index];
    }
    return (LoggingRule) null;
  }

  public bool RemoveRuleByName(string ruleName)
  {
    if (ruleName == null)
      return false;
    HashSet<LoggingRule> loggingRuleSet = new HashSet<LoggingRule>();
    foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
    {
      if (string.Equals(loggingRule.RuleName, ruleName, StringComparison.OrdinalIgnoreCase))
        loggingRuleSet.Add(loggingRule);
    }
    if (loggingRuleSet.Count > 0)
    {
      lock (this.LoggingRules)
      {
        for (int index = this.LoggingRules.Count - 1; index >= 0; --index)
        {
          if (loggingRuleSet.Contains(this.LoggingRules[index]))
            this.LoggingRules.RemoveAt(index);
        }
      }
    }
    return loggingRuleSet.Count > 0;
  }

  public virtual LoggingConfiguration Reload() => this;

  internal LoggingConfiguration ReloadNewConfig()
  {
    LoggingConfiguration loggingConfiguration1 = this.Reload();
    if (loggingConfiguration1 != null)
    {
      if (loggingConfiguration1 is XmlLoggingConfiguration loggingConfiguration2)
      {
        bool? initializeSucceeded = loggingConfiguration2.InitializeSucceeded;
        bool flag = true;
        if (!(initializeSucceeded.GetValueOrDefault() == flag & initializeSucceeded.HasValue))
        {
          InternalLogger.Warn("NLog Config Reload() failed. Invalid XML?");
          return (LoggingConfiguration) null;
        }
      }
      if (this.LogFactory.KeepVariablesOnReload)
      {
        LoggingConfiguration loggingConfiguration3 = this.LogFactory._config ?? this;
        if (loggingConfiguration1 != loggingConfiguration3)
          loggingConfiguration1.CopyVariables(loggingConfiguration3.Variables);
      }
    }
    return loggingConfiguration1;
  }

  public void RemoveTarget(string name)
  {
    HashSet<Target> removedTargets = new HashSet<Target>();
    Target removedTarget = this.RemoveTargetThreadSafe(name);
    if (removedTarget != null)
      removedTargets.Add(removedTarget);
    if (!string.IsNullOrEmpty(name) || removedTarget != null)
      this.CleanupRulesForRemovedTarget(name, removedTarget, removedTargets);
    if (removedTargets.Count <= 0)
      return;
    this.ValidateConfig();
    this.LogFactory.ReconfigExistingLoggers();
    ManualResetEvent flushCompleted = new ManualResetEvent(false);
    foreach (Target target in removedTargets)
    {
      flushCompleted.Reset();
      target.Flush((AsyncContinuation) (ex => flushCompleted.Set()));
      flushCompleted.WaitOne(TimeSpan.FromSeconds(15.0));
      target.Close();
    }
  }

  private void CleanupRulesForRemovedTarget(
    string name,
    Target removedTarget,
    HashSet<Target> removedTargets)
  {
    foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
    {
      foreach (Target target in loggingRule.GetTargetsThreadSafe())
      {
        if (removedTarget == target || !string.IsNullOrEmpty(name) && target.Name == name)
        {
          removedTargets.Add(target);
          loggingRule.RemoveTargetThreadSafe(target);
        }
      }
    }
  }

  public void Install(InstallationContext installationContext)
  {
    if (installationContext == null)
      throw new ArgumentNullException(nameof (installationContext));
    this.InitializeAll();
    foreach (IInstallable installableItem in this.GetInstallableItems())
    {
      installationContext.Info("Installing '{0}'", (object) installableItem);
      try
      {
        installableItem.Install(installationContext);
        installationContext.Info("Finished installing '{0}'.", (object) installableItem);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Install of '{0}' failed.", (object) installableItem);
        if (ex.MustBeRethrownImmediately() || installationContext.ThrowExceptions)
          throw;
        installationContext.Error("Install of '{0}' failed: {1}.", (object) installableItem, (object) ex);
      }
    }
  }

  public void Uninstall(InstallationContext installationContext)
  {
    if (installationContext == null)
      throw new ArgumentNullException(nameof (installationContext));
    this.InitializeAll();
    foreach (IInstallable installableItem in this.GetInstallableItems())
    {
      installationContext.Info("Uninstalling '{0}'", (object) installableItem);
      try
      {
        installableItem.Uninstall(installationContext);
        installationContext.Info("Finished uninstalling '{0}'.", (object) installableItem);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Uninstall of '{0}' failed.", (object) installableItem);
        if (ex.MustBeRethrownImmediately())
          throw;
        installationContext.Error("Uninstall of '{0}' failed: {1}.", (object) installableItem, (object) ex);
      }
    }
  }

  internal void Close()
  {
    InternalLogger.Debug("Closing logging configuration...");
    foreach (ISupportsInitialize supportsInitializ in this.GetSupportsInitializes())
    {
      InternalLogger.Trace<ISupportsInitialize>("Closing {0}", supportsInitializ);
      try
      {
        supportsInitializ.Close();
      }
      catch (Exception ex)
      {
        InternalLogger.Warn(ex, "Exception while closing.");
        if (ex.MustBeRethrown())
          throw;
      }
    }
    InternalLogger.Debug("Finished closing logging configuration.");
  }

  internal void Dump()
  {
    if (!InternalLogger.IsDebugEnabled)
      return;
    InternalLogger.Debug("--- NLog configuration dump ---");
    InternalLogger.Debug("Targets:");
    foreach (Target target in this.GetAllTargetsThreadSafe())
      InternalLogger.Debug<Target>("{0}", target);
    InternalLogger.Debug("Rules:");
    foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
      InternalLogger.Debug<LoggingRule>("{0}", loggingRule);
    InternalLogger.Debug("--- End of NLog configuration dump ---");
  }

  internal List<Target> GetAllTargetsToFlush()
  {
    List<Target> allTargetsToFlush = new List<Target>();
    foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
    {
      foreach (Target target in loggingRule.GetTargetsThreadSafe())
      {
        if (!allTargetsToFlush.Contains(target))
          allTargetsToFlush.Add(target);
      }
    }
    return allTargetsToFlush;
  }

  internal void ValidateConfig()
  {
    List<object> objectList = new List<object>();
    foreach (LoggingRule loggingRule in this.GetLoggingRulesThreadSafe())
      objectList.Add((object) loggingRule);
    foreach (Target target in this.GetAllTargetsThreadSafe())
      objectList.Add((object) target);
    this._configItems = ObjectGraphScanner.FindReachableObjects<object>(true, objectList.ToArray());
    InternalLogger.Info<LoggingConfiguration>("Validating config: {0}", this);
    foreach (object configItem in this._configItems)
    {
      try
      {
        if (!(configItem is ISupportsInitialize))
          PropertyHelper.CheckRequiredParameters(configItem);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown())
          throw;
      }
    }
  }

  internal void InitializeAll()
  {
    int num = this._configItems.Count == 0 ? 1 : 0;
    if (num != 0 && (this.LogFactory.ThrowExceptions || LogManager.ThrowExceptions))
      InternalLogger.Info("LogManager.ThrowExceptions = true can crash the application! Use only for unit-testing and last resort troubleshooting.");
    this.ValidateConfig();
    if (num != 0 && this._targets.Count > 0)
      this.CheckUnusedTargets();
    foreach (ISupportsInitialize supportsInitializ in this.GetSupportsInitializes(true))
    {
      InternalLogger.Trace<ISupportsInitialize>("Initializing {0}", supportsInitializ);
      try
      {
        supportsInitializ.Initialize(this);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrown(supportsInitializ as IInternalLoggerContext))
          throw;
        if (LogManager.ThrowExceptions)
          throw new NLogConfigurationException($"Error during initialization of {supportsInitializ}", ex);
      }
    }
  }

  private List<IInstallable> GetInstallableItems()
  {
    return this._configItems.OfType<IInstallable>().ToList<IInstallable>();
  }

  private List<ISupportsInitialize> GetSupportsInitializes(bool reverse = false)
  {
    IEnumerable<ISupportsInitialize> source = this._configItems.OfType<ISupportsInitialize>();
    if (reverse)
      source = source.Reverse<ISupportsInitialize>();
    return source.ToList<ISupportsInitialize>();
  }

  internal void CopyVariables(IDictionary<string, SimpleLayout> masterVariables)
  {
    this._variables.CopyFrom(masterVariables);
  }

  [NotNull]
  internal string ExpandSimpleVariables(string input)
  {
    string str = input;
    if (this.Variables.Count > 0 && str != null && str.IndexOf('$') >= 0)
    {
      foreach (KeyValuePair<string, SimpleLayout> keyValuePair in this.Variables.ToList<KeyValuePair<string, SimpleLayout>>())
      {
        SimpleLayout simpleLayout = keyValuePair.Value;
        if (simpleLayout != null)
          str = str.Replace($"${{{keyValuePair.Key}}}", simpleLayout.OriginalText);
      }
    }
    return str ?? string.Empty;
  }

  internal void CheckUnusedTargets()
  {
    if (!InternalLogger.IsWarnEnabled)
      return;
    List<Target> targetsThreadSafe = this.GetAllTargetsThreadSafe();
    InternalLogger.Debug<int, int>("Unused target checking is started... Rule Count: {0}, Target Count: {1}", this.LoggingRules.Count, targetsThreadSafe.Count);
    HashSet<string> targetNamesAtRules = new HashSet<string>(this.GetLoggingRulesThreadSafe().SelectMany<LoggingRule, Target>((Func<LoggingRule, IEnumerable<Target>>) (r => (IEnumerable<Target>) r.Targets)).Select<Target, string>((Func<Target, string>) (t => t.Name)));
    ReadOnlyCollection<Target> allTargets = this.AllTargets;
    ILookup<Target, WrapperTargetBase> wrappedTargets = allTargets.OfType<WrapperTargetBase>().ToLookup<WrapperTargetBase, Target, WrapperTargetBase>((Func<WrapperTargetBase, Target>) (wt => wt.WrappedTarget), (Func<WrapperTargetBase, WrapperTargetBase>) (wt => wt));
    ILookup<Target, Target> compoundTargets = allTargets.OfType<CompoundTargetBase>().SelectMany<CompoundTargetBase, KeyValuePair<Target, Target>>((Func<CompoundTargetBase, IEnumerable<KeyValuePair<Target, Target>>>) (wt => wt.Targets.Select<Target, KeyValuePair<Target, Target>>((Func<Target, KeyValuePair<Target, Target>>) (t => new KeyValuePair<Target, Target>(t, (Target) wt))))).ToLookup<KeyValuePair<Target, Target>, Target, Target>((Func<KeyValuePair<Target, Target>, Target>) (p => p.Key), (Func<KeyValuePair<Target, Target>, Target>) (p => p.Value));
    int num = targetsThreadSafe.Count<Target>((Func<Target, bool>) (target =>
    {
      if (targetNamesAtRules.Contains(target.Name) || !IsUnusedInList<WrapperTargetBase>(target, wrappedTargets) || !IsUnusedInList<Target>(target, compoundTargets))
        return false;
      InternalLogger.Warn<string>("Unused target detected. Add a rule for this target to the configuration. TargetName: {0}", target.Name);
      return true;
    }));
    InternalLogger.Debug<int, int, int>("Unused target checking is completed. Total Rule Count: {0}, Total Target Count: {1}, Unused Target Count: {2}", this.LoggingRules.Count, targetsThreadSafe.Count, num);

    bool IsUnusedInList<T>(Target target1, ILookup<Target, T> targets) where T : Target
    {
      if (targets.Contains(target1))
      {
        foreach (T key in targets[target1])
        {
          if (targetNamesAtRules.Contains(key.Name) || wrappedTargets.Contains((Target) key) || compoundTargets.Contains((Target) key))
            return false;
        }
      }
      return true;
    }
  }

  public override string ToString()
  {
    List<Target> source = this.GetAllTargetsToFlush();
    if (source.Count == 0)
      source = this.GetAllTargetsThreadSafe();
    if (source.Count == 0)
      source = this.AllTargets.ToList<Target>();
    return source.Count > 0 && source.Count < 5 ? $"TargetNames={string.Join(", ", source.Select<Target, string>((Func<Target, string>) (t => t.Name)).Where<string>((Func<string, bool>) (n => !string.IsNullOrEmpty(n))).ToArray<string>())}, ConfigItems={this._configItems.Count}" : $"Targets={source.Count}, ConfigItems={this._configItems.Count}";
  }

  private class TargetNameEqualityComparer : IEqualityComparer<Target>
  {
    public bool Equals(Target x, Target y) => string.Equals(x.Name, y.Name);

    public int GetHashCode(Target obj) => obj.Name == null ? 0 : obj.Name.GetHashCode();
  }
}
