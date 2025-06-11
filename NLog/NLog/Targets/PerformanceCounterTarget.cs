// Decompiled with JetBrains decompiler
// Type: NLog.Targets.PerformanceCounterTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace NLog.Targets;

[Target("PerfCounter")]
public class PerformanceCounterTarget : Target, IInstallable
{
  private PerformanceCounter perfCounter;
  private bool initialized;
  private bool created;

  public PerformanceCounterTarget()
  {
    this.CounterType = PerformanceCounterType.NumberOfItems32;
    this.IncrementValue = (Layout) new SimpleLayout("1");
    this.InstanceName = string.Empty;
    this.CounterHelp = string.Empty;
  }

  public PerformanceCounterTarget(string name)
    : this()
  {
    this.Name = name;
  }

  public bool AutoCreate { get; set; }

  [RequiredParameter]
  public string CategoryName { get; set; }

  [RequiredParameter]
  public string CounterName { get; set; }

  public string InstanceName { get; set; }

  public string CounterHelp { get; set; }

  [DefaultValue(PerformanceCounterType.NumberOfItems32)]
  public PerformanceCounterType CounterType { get; set; }

  [DefaultValue(1)]
  public Layout IncrementValue { get; set; }

  public void Install(InstallationContext installationContext)
  {
    Dictionary<string, List<PerformanceCounterTarget>> dictionary = this.LoggingConfiguration.AllTargets.OfType<PerformanceCounterTarget>().BucketSort<PerformanceCounterTarget, string>((SortHelpers.KeySelector<PerformanceCounterTarget, string>) (c => c.CategoryName));
    string categoryName = this.CategoryName;
    if (dictionary[categoryName].Any<PerformanceCounterTarget>((Func<PerformanceCounterTarget, bool>) (c => c.created)))
    {
      installationContext.Trace("Category '{0}' has already been installed.", (object) categoryName);
    }
    else
    {
      try
      {
        PerformanceCounterCategoryType categoryType;
        CounterCreationDataCollection creationDataCollection = PerformanceCounterTarget.GetCounterCreationDataCollection((IEnumerable<PerformanceCounterTarget>) dictionary[this.CategoryName], out categoryType);
        if (PerformanceCounterCategory.Exists(categoryName))
        {
          installationContext.Debug("Deleting category '{0}'", (object) categoryName);
          PerformanceCounterCategory.Delete(categoryName);
        }
        installationContext.Debug("Creating category '{0}' with {1} counter(s) (Type: {2})", (object) categoryName, (object) creationDataCollection.Count, (object) categoryType);
        foreach (CounterCreationData counterCreationData in (CollectionBase) creationDataCollection)
          installationContext.Trace("  Counter: '{0}' Type: ({1}) Help: {2}", (object) counterCreationData.CounterName, (object) counterCreationData.CounterType, (object) counterCreationData.CounterHelp);
        PerformanceCounterCategory.Create(categoryName, "Category created by NLog", categoryType, creationDataCollection);
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        if (installationContext.IgnoreFailures)
        {
          installationContext.Warning("Error creating category '{0}': {1}", (object) categoryName, (object) ex.Message);
          if (!this.ExceptionMustBeRethrown(ex))
            return;
          throw;
        }
        installationContext.Error("Error creating category '{0}': {1}", (object) categoryName, (object) ex.Message);
        throw;
      }
      finally
      {
        foreach (PerformanceCounterTarget performanceCounterTarget in dictionary[categoryName])
          performanceCounterTarget.created = true;
      }
    }
  }

  public void Uninstall(InstallationContext installationContext)
  {
    string categoryName = this.CategoryName;
    if (PerformanceCounterCategory.Exists(categoryName))
    {
      installationContext.Debug("Deleting category '{0}'", (object) categoryName);
      PerformanceCounterCategory.Delete(categoryName);
    }
    else
      installationContext.Debug("Category '{0}' does not exist.", (object) categoryName);
  }

  public bool? IsInstalled(InstallationContext installationContext)
  {
    return !PerformanceCounterCategory.Exists(this.CategoryName) ? new bool?(false) : new bool?(PerformanceCounterCategory.CounterExists(this.CounterName, this.CategoryName));
  }

  protected override void Write(LogEventInfo logEvent)
  {
    if (!this.EnsureInitialized())
      return;
    string s = this.IncrementValue.Render(logEvent);
    long result;
    if (long.TryParse(s, out result))
      this.perfCounter.IncrementBy(result);
    else
      InternalLogger.Error<string, string, string>("PerfCounterTarget(Name={0}): Error incrementing PerfCounter {1}. IncrementValue must be an integer but was <{2}>", this.Name, this.CounterName, s);
  }

  protected override void CloseTarget()
  {
    base.CloseTarget();
    if (this.perfCounter != null)
    {
      this.perfCounter.Close();
      this.perfCounter = (PerformanceCounter) null;
    }
    this.initialized = false;
  }

  private static CounterCreationDataCollection GetCounterCreationDataCollection(
    IEnumerable<PerformanceCounterTarget> countersInCategory,
    out PerformanceCounterCategoryType categoryType)
  {
    categoryType = PerformanceCounterCategoryType.SingleInstance;
    CounterCreationDataCollection creationDataCollection = new CounterCreationDataCollection();
    foreach (PerformanceCounterTarget performanceCounterTarget in countersInCategory)
    {
      if (!string.IsNullOrEmpty(performanceCounterTarget.InstanceName))
        categoryType = PerformanceCounterCategoryType.MultiInstance;
      creationDataCollection.Add(new CounterCreationData(performanceCounterTarget.CounterName, performanceCounterTarget.CounterHelp, performanceCounterTarget.CounterType));
    }
    return creationDataCollection;
  }

  private bool EnsureInitialized()
  {
    if (!this.initialized)
    {
      this.initialized = true;
      if (this.AutoCreate)
      {
        using (InstallationContext installationContext = new InstallationContext())
          this.Install(installationContext);
      }
      try
      {
        this.perfCounter = new PerformanceCounter(this.CategoryName, this.CounterName, this.InstanceName, false);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "PerfCounterTarget(Name={0}): Cannot open performance counter {1}/{2}/{3}.", (object) this.Name, (object) this.CategoryName, (object) this.CounterName, (object) this.InstanceName);
        if (this.ExceptionMustBeRethrown(ex))
          throw;
      }
    }
    return this.perfCounter != null;
  }
}
