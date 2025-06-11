// Decompiled with JetBrains decompiler
// Type: NLog.Targets.TargetWithContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Targets;

public abstract class TargetWithContext : TargetWithLayout, IIncludeContext
{
  private TargetWithContext.TargetWithContextLayout _contextLayout;
  private IPropertyTypeConverter _propertyTypeConverter;

  public sealed override Layout Layout
  {
    get => (Layout) this._contextLayout;
    set
    {
      if (this._contextLayout != null)
        this._contextLayout.TargetLayout = value;
      else
        this._contextLayout = new TargetWithContext.TargetWithContextLayout(this, value);
    }
  }

  bool IIncludeContext.IncludeAllProperties
  {
    get => this.IncludeEventProperties;
    set => this.IncludeEventProperties = value;
  }

  public bool IncludeEventProperties
  {
    get => this._contextLayout.IncludeAllProperties;
    set => this._contextLayout.IncludeAllProperties = value;
  }

  public bool IncludeMdc
  {
    get => this._contextLayout.IncludeMdc;
    set => this._contextLayout.IncludeMdc = value;
  }

  public bool IncludeNdc
  {
    get => this._contextLayout.IncludeNdc;
    set => this._contextLayout.IncludeNdc = value;
  }

  public bool IncludeMdlc
  {
    get => this._contextLayout.IncludeMdlc;
    set => this._contextLayout.IncludeMdlc = value;
  }

  public bool IncludeNdlc
  {
    get => this._contextLayout.IncludeNdlc;
    set => this._contextLayout.IncludeNdlc = value;
  }

  public bool IncludeGdc { get; set; }

  public bool IncludeCallSite
  {
    get => this._contextLayout.IncludeCallSite;
    set => this._contextLayout.IncludeCallSite = value;
  }

  public bool IncludeCallSiteStackTrace
  {
    get => this._contextLayout.IncludeCallSiteStackTrace;
    set => this._contextLayout.IncludeCallSiteStackTrace = value;
  }

  [ArrayParameter(typeof (TargetPropertyWithContext), "contextproperty")]
  public virtual IList<TargetPropertyWithContext> ContextProperties { get; } = (IList<TargetPropertyWithContext>) new List<TargetPropertyWithContext>();

  private IPropertyTypeConverter PropertyTypeConverter
  {
    get
    {
      return this._propertyTypeConverter ?? (this._propertyTypeConverter = ConfigurationItemFactory.Default.PropertyTypeConverter);
    }
    set => this._propertyTypeConverter = value;
  }

  protected TargetWithContext()
  {
    this._contextLayout = this._contextLayout ?? new TargetWithContext.TargetWithContextLayout(this, base.Layout);
    this.OptimizeBufferReuse = true;
  }

  protected override void CloseTarget()
  {
    this.PropertyTypeConverter = (IPropertyTypeConverter) null;
    base.CloseTarget();
  }

  protected bool ShouldIncludeProperties(LogEventInfo logEvent)
  {
    if (this.IncludeGdc || this.IncludeMdc || this.IncludeMdlc)
      return true;
    return this.IncludeEventProperties && logEvent != null && logEvent.HasProperties;
  }

  protected IDictionary<string, object> GetContextProperties(LogEventInfo logEvent)
  {
    return this.GetContextProperties(logEvent, (IDictionary<string, object>) null);
  }

  protected IDictionary<string, object> GetContextProperties(
    LogEventInfo logEvent,
    IDictionary<string, object> combinedProperties)
  {
    IList<TargetPropertyWithContext> contextProperties = this.ContextProperties;
    if ((contextProperties != null ? (contextProperties.Count > 0 ? 1 : 0) : 0) != 0)
      combinedProperties = this.CaptureContextProperties(logEvent, combinedProperties);
    if (this.IncludeMdlc && !this.CombineProperties(logEvent, (Layout) this._contextLayout.MdlcLayout, ref combinedProperties))
      combinedProperties = this.CaptureContextMdlc(logEvent, combinedProperties);
    if (this.IncludeMdc && !this.CombineProperties(logEvent, (Layout) this._contextLayout.MdcLayout, ref combinedProperties))
      combinedProperties = this.CaptureContextMdc(logEvent, combinedProperties);
    if (this.IncludeGdc)
      combinedProperties = this.CaptureContextGdc(logEvent, combinedProperties);
    return combinedProperties;
  }

  protected IDictionary<string, object> GetAllProperties(LogEventInfo logEvent)
  {
    return this.GetAllProperties(logEvent, (IDictionary<string, object>) null);
  }

  protected IDictionary<string, object> GetAllProperties(
    LogEventInfo logEvent,
    IDictionary<string, object> combinedProperties)
  {
    if (this.IncludeEventProperties && logEvent.HasProperties)
    {
      IDictionary<string, object> dictionary = combinedProperties;
      if (dictionary == null)
      {
        int count1 = logEvent.Properties.Count;
        IList<TargetPropertyWithContext> contextProperties = this.ContextProperties;
        int count2 = contextProperties != null ? contextProperties.Count : 0;
        dictionary = TargetWithContext.CreateNewDictionary(count1 + count2);
      }
      combinedProperties = dictionary;
      bool checkForDuplicates = combinedProperties.Count > 0;
      foreach (KeyValuePair<object, object> property in (IEnumerable<KeyValuePair<object, object>>) logEvent.Properties)
      {
        string itemName = property.Key.ToString();
        if (!string.IsNullOrEmpty(itemName))
          this.AddContextProperty(logEvent, itemName, property.Value, checkForDuplicates, combinedProperties);
      }
    }
    combinedProperties = this.GetContextProperties(logEvent, combinedProperties);
    return combinedProperties ?? (IDictionary<string, object>) new Dictionary<string, object>();
  }

  private static IDictionary<string, object> CreateNewDictionary(int initialCapacity)
  {
    return (IDictionary<string, object>) new Dictionary<string, object>(Math.Max(initialCapacity, 3));
  }

  protected virtual string GenerateUniqueItemName(
    LogEventInfo logEvent,
    string itemName,
    object itemValue,
    IDictionary<string, object> combinedProperties)
  {
    itemName = itemName ?? string.Empty;
    int num = 1;
    string key = itemName + "_1";
    while (combinedProperties.ContainsKey(key))
      key = $"{itemName}_{(++num).ToString()}";
    return key;
  }

  private bool CombineProperties(
    LogEventInfo logEvent,
    Layout contextLayout,
    ref IDictionary<string, object> combinedProperties)
  {
    object obj;
    if (!logEvent.TryGetCachedLayoutValue(contextLayout, out obj))
      return false;
    if (obj is IDictionary<string, object> dictionary)
    {
      if (combinedProperties != null)
      {
        bool checkForDuplicates = combinedProperties.Count > 0;
        foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) dictionary)
          this.AddContextProperty(logEvent, keyValuePair.Key, keyValuePair.Value, checkForDuplicates, combinedProperties);
      }
      else
        combinedProperties = dictionary;
    }
    return true;
  }

  private void AddContextProperty(
    LogEventInfo logEvent,
    string itemName,
    object itemValue,
    bool checkForDuplicates,
    IDictionary<string, object> combinedProperties)
  {
    if (checkForDuplicates && combinedProperties.ContainsKey(itemName))
    {
      itemName = this.GenerateUniqueItemName(logEvent, itemName, itemValue, combinedProperties);
      if (itemName == null)
        return;
    }
    combinedProperties[itemName] = itemValue;
  }

  protected IDictionary<string, object> GetContextMdc(LogEventInfo logEvent)
  {
    object obj;
    return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.MdcLayout, out obj) ? obj as IDictionary<string, object> : this.CaptureContextMdc(logEvent, (IDictionary<string, object>) null);
  }

  protected IDictionary<string, object> GetContextMdlc(LogEventInfo logEvent)
  {
    object obj;
    return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.MdlcLayout, out obj) ? obj as IDictionary<string, object> : this.CaptureContextMdlc(logEvent, (IDictionary<string, object>) null);
  }

  protected IList<object> GetContextNdc(LogEventInfo logEvent)
  {
    object obj;
    return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.NdcLayout, out obj) ? obj as IList<object> : this.CaptureContextNdc(logEvent);
  }

  protected IList<object> GetContextNdlc(LogEventInfo logEvent)
  {
    object obj;
    return logEvent.TryGetCachedLayoutValue((Layout) this._contextLayout.NdlcLayout, out obj) ? obj as IList<object> : this.CaptureContextNdlc(logEvent);
  }

  private IDictionary<string, object> CaptureContextProperties(
    LogEventInfo logEvent,
    IDictionary<string, object> combinedProperties)
  {
    combinedProperties = combinedProperties ?? TargetWithContext.CreateNewDictionary(this.ContextProperties.Count);
    for (int index = 0; index < this.ContextProperties.Count; ++index)
    {
      TargetPropertyWithContext contextProperty = this.ContextProperties[index];
      if (!string.IsNullOrEmpty(contextProperty?.Name))
      {
        if (contextProperty.Layout != null)
        {
          try
          {
            object propertyValue;
            if (this.TryGetContextPropertyValue(logEvent, contextProperty, out propertyValue))
              combinedProperties[contextProperty.Name] = propertyValue;
          }
          catch (Exception ex)
          {
            if (ex.MustBeRethrownImmediately())
              throw;
            object[] objArray = new object[3]
            {
              (object) this.GetType(),
              (object) this.Name,
              (object) contextProperty.Name
            };
            InternalLogger.Warn(ex, "{0}(Name={1}): Failed to add context property {2}", objArray);
          }
        }
      }
    }
    return combinedProperties;
  }

  private bool TryGetContextPropertyValue(
    LogEventInfo logEvent,
    TargetPropertyWithContext contextProperty,
    out object propertyValue)
  {
    Type type1 = contextProperty.PropertyType;
    if ((object) type1 == null)
      type1 = typeof (string);
    Type type2 = type1;
    bool flag = type2 == typeof (string);
    object rawValue;
    if (!flag && contextProperty.Layout.TryGetRawValue(logEvent, out rawValue))
    {
      if (type2 == typeof (object))
      {
        propertyValue = rawValue;
        return contextProperty.IncludeEmptyValue || propertyValue != null;
      }
      if (rawValue?.GetType() == type2)
      {
        propertyValue = rawValue;
        return true;
      }
    }
    string propertyValue1 = this.RenderLogEvent(contextProperty.Layout, logEvent) ?? string.Empty;
    if (!contextProperty.IncludeEmptyValue && string.IsNullOrEmpty(propertyValue1))
    {
      propertyValue = (object) null;
      return false;
    }
    if (flag)
    {
      propertyValue = (object) propertyValue1;
      return true;
    }
    if (string.IsNullOrEmpty(propertyValue1) && type2.IsValueType())
    {
      propertyValue = Activator.CreateInstance(type2);
      return true;
    }
    propertyValue = this.PropertyTypeConverter.Convert((object) propertyValue1, type2, (string) null, (IFormatProvider) CultureInfo.InvariantCulture);
    return true;
  }

  protected virtual IDictionary<string, object> CaptureContextGdc(
    LogEventInfo logEvent,
    IDictionary<string, object> contextProperties)
  {
    ICollection<string> names = GlobalDiagnosticsContext.GetNames();
    if (names.Count == 0)
      return contextProperties;
    contextProperties = contextProperties ?? TargetWithContext.CreateNewDictionary(names.Count);
    bool checkForDuplicates = contextProperties.Count > 0;
    foreach (string str in (IEnumerable<string>) names)
    {
      object serializedValue = GlobalDiagnosticsContext.GetObject(str);
      if (this.SerializeItemValue(logEvent, str, serializedValue, out serializedValue))
        this.AddContextProperty(logEvent, str, serializedValue, checkForDuplicates, contextProperties);
    }
    return contextProperties;
  }

  protected virtual IDictionary<string, object> CaptureContextMdc(
    LogEventInfo logEvent,
    IDictionary<string, object> contextProperties)
  {
    ICollection<string> names = MappedDiagnosticsContext.GetNames();
    if (names.Count == 0)
      return contextProperties;
    contextProperties = contextProperties ?? TargetWithContext.CreateNewDictionary(names.Count);
    bool checkForDuplicates = contextProperties.Count > 0;
    foreach (string str in (IEnumerable<string>) names)
    {
      object obj = MappedDiagnosticsContext.GetObject(str);
      object serializedValue;
      if (this.SerializeMdcItem(logEvent, str, obj, out serializedValue))
        this.AddContextProperty(logEvent, str, serializedValue, checkForDuplicates, contextProperties);
    }
    return contextProperties;
  }

  protected virtual bool SerializeMdcItem(
    LogEventInfo logEvent,
    string name,
    object value,
    out object serializedValue)
  {
    if (!string.IsNullOrEmpty(name))
      return this.SerializeItemValue(logEvent, name, value, out serializedValue);
    serializedValue = (object) null;
    return false;
  }

  protected virtual IDictionary<string, object> CaptureContextMdlc(
    LogEventInfo logEvent,
    IDictionary<string, object> contextProperties)
  {
    ICollection<string> names = MappedDiagnosticsLogicalContext.GetNames();
    if (names.Count == 0)
      return contextProperties;
    contextProperties = contextProperties ?? TargetWithContext.CreateNewDictionary(names.Count);
    bool checkForDuplicates = contextProperties.Count > 0;
    foreach (string str in (IEnumerable<string>) names)
    {
      object obj = MappedDiagnosticsLogicalContext.GetObject(str);
      object serializedValue;
      if (this.SerializeMdlcItem(logEvent, str, obj, out serializedValue))
        this.AddContextProperty(logEvent, str, serializedValue, checkForDuplicates, contextProperties);
    }
    return contextProperties;
  }

  protected virtual bool SerializeMdlcItem(
    LogEventInfo logEvent,
    string name,
    object value,
    out object serializedValue)
  {
    if (!string.IsNullOrEmpty(name))
      return this.SerializeItemValue(logEvent, name, value, out serializedValue);
    serializedValue = (object) null;
    return false;
  }

  protected virtual IList<object> CaptureContextNdc(LogEventInfo logEvent)
  {
    object[] allObjects = NestedDiagnosticsContext.GetAllObjects();
    if (allObjects.Length == 0)
      return (IList<object>) allObjects;
    IList<object> objectList = (IList<object>) null;
    for (int index1 = 0; index1 < allObjects.Length; ++index1)
    {
      object obj = allObjects[index1];
      object serializedValue;
      if (this.SerializeNdcItem(logEvent, obj, out serializedValue))
      {
        if (objectList != null)
          objectList.Add(serializedValue);
        else
          allObjects[index1] = serializedValue;
      }
      else if (objectList == null)
      {
        objectList = (IList<object>) new List<object>(allObjects.Length);
        for (int index2 = 0; index2 < index1; ++index2)
          objectList.Add(allObjects[index2]);
      }
    }
    return objectList ?? (IList<object>) allObjects;
  }

  protected virtual bool SerializeNdcItem(
    LogEventInfo logEvent,
    object value,
    out object serializedValue)
  {
    return this.SerializeItemValue(logEvent, (string) null, value, out serializedValue);
  }

  protected virtual IList<object> CaptureContextNdlc(LogEventInfo logEvent)
  {
    object[] allObjects = NestedDiagnosticsLogicalContext.GetAllObjects();
    if (allObjects.Length == 0)
      return (IList<object>) allObjects;
    IList<object> objectList = (IList<object>) null;
    for (int index1 = 0; index1 < allObjects.Length; ++index1)
    {
      object obj = allObjects[index1];
      object serializedValue;
      if (this.SerializeNdlcItem(logEvent, obj, out serializedValue))
      {
        if (objectList != null)
          objectList.Add(serializedValue);
        else
          allObjects[index1] = serializedValue;
      }
      else if (objectList == null)
      {
        objectList = (IList<object>) new List<object>(allObjects.Length);
        for (int index2 = 0; index2 < index1; ++index2)
          objectList.Add(allObjects[index2]);
      }
    }
    return objectList ?? (IList<object>) allObjects;
  }

  protected virtual bool SerializeNdlcItem(
    LogEventInfo logEvent,
    object value,
    out object serializedValue)
  {
    return this.SerializeItemValue(logEvent, (string) null, value, out serializedValue);
  }

  protected virtual bool SerializeItemValue(
    LogEventInfo logEvent,
    string name,
    object value,
    out object serializedValue)
  {
    if (value == null)
    {
      serializedValue = (object) null;
      return true;
    }
    if (!(value is string) && Convert.GetTypeCode(value) == TypeCode.Object)
    {
      switch (value)
      {
        case Guid _:
        case TimeSpan _:
        case DateTimeOffset _:
          break;
        default:
          ref object local = ref serializedValue;
          object obj = value;
          IFormatProvider provider = logEvent.FormatProvider;
          if (provider == null)
          {
            LoggingConfiguration loggingConfiguration = this.LoggingConfiguration;
            provider = loggingConfiguration != null ? (IFormatProvider) loggingConfiguration.DefaultCultureInfo : (IFormatProvider) null;
          }
          string str = Convert.ToString(obj, provider);
          local = (object) str;
          return true;
      }
    }
    serializedValue = value;
    return true;
  }

  [ThreadSafe]
  [ThreadAgnostic]
  private class TargetWithContextLayout : Layout, IIncludeContext, IUsesStackTrace
  {
    private Layout _targetLayout;

    public Layout TargetLayout
    {
      get => this._targetLayout;
      set => this._targetLayout = this == value ? this._targetLayout : value;
    }

    internal TargetWithContext.TargetWithContextLayout.LayoutContextMdc MdcLayout { get; }

    internal TargetWithContext.TargetWithContextLayout.LayoutContextNdc NdcLayout { get; }

    internal TargetWithContext.TargetWithContextLayout.LayoutContextMdlc MdlcLayout { get; }

    internal TargetWithContext.TargetWithContextLayout.LayoutContextNdlc NdlcLayout { get; }

    public bool IncludeAllProperties { get; set; }

    public bool IncludeCallSite { get; set; }

    public bool IncludeCallSiteStackTrace { get; set; }

    public bool IncludeMdc
    {
      get => this.MdcLayout.IsActive;
      set => this.MdcLayout.IsActive = value;
    }

    public bool IncludeNdc
    {
      get => this.NdcLayout.IsActive;
      set => this.NdcLayout.IsActive = value;
    }

    public bool IncludeMdlc
    {
      get => this.MdlcLayout.IsActive;
      set => this.MdlcLayout.IsActive = value;
    }

    public bool IncludeNdlc
    {
      get => this.NdlcLayout.IsActive;
      set => this.NdlcLayout.IsActive = value;
    }

    StackTraceUsage IUsesStackTrace.StackTraceUsage
    {
      get
      {
        if (this.IncludeCallSiteStackTrace)
          return StackTraceUsage.WithSource;
        return this.IncludeCallSite ? StackTraceUsage.WithoutSource : StackTraceUsage.None;
      }
    }

    public TargetWithContextLayout(TargetWithContext owner, Layout targetLayout)
    {
      this.TargetLayout = targetLayout;
      this.MdcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextMdc(owner);
      this.NdcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextNdc(owner);
      this.MdlcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextMdlc(owner);
      this.NdlcLayout = new TargetWithContext.TargetWithContextLayout.LayoutContextNdlc(owner);
    }

    protected override void InitializeLayout()
    {
      base.InitializeLayout();
      if (this.IncludeMdc || this.IncludeNdc)
        this.ThreadAgnostic = false;
      if (this.IncludeMdlc || this.IncludeNdlc)
        this.ThreadAgnostic = false;
      if (!this.IncludeAllProperties)
        return;
      this.MutableUnsafe = true;
    }

    public override string ToString() => this.TargetLayout?.ToString() ?? base.ToString();

    public override void Precalculate(LogEventInfo logEvent)
    {
      Layout targetLayout1 = this.TargetLayout;
      if ((targetLayout1 != null ? (targetLayout1.ThreadAgnostic ? 1 : 0) : 1) != 0)
      {
        Layout targetLayout2 = this.TargetLayout;
        if ((targetLayout2 != null ? (targetLayout2.MutableUnsafe ? 1 : 0) : 0) == 0)
          goto label_4;
      }
      this.TargetLayout.Precalculate(logEvent);
      object obj;
      if (logEvent.TryGetCachedLayoutValue(this.TargetLayout, out obj))
        logEvent.AddCachedLayoutValue((Layout) this, obj);
label_4:
      this.PrecalculateContext(logEvent);
    }

    internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
    {
      Layout targetLayout1 = this.TargetLayout;
      if ((targetLayout1 != null ? (targetLayout1.ThreadAgnostic ? 1 : 0) : 1) != 0)
      {
        Layout targetLayout2 = this.TargetLayout;
        if ((targetLayout2 != null ? (targetLayout2.MutableUnsafe ? 1 : 0) : 0) == 0)
          goto label_4;
      }
      this.TargetLayout.PrecalculateBuilder(logEvent, target);
      object obj;
      if (logEvent.TryGetCachedLayoutValue(this.TargetLayout, out obj))
        logEvent.AddCachedLayoutValue((Layout) this, obj);
label_4:
      this.PrecalculateContext(logEvent);
    }

    private void PrecalculateContext(LogEventInfo logEvent)
    {
      if (this.IncludeMdc)
        this.MdcLayout.Precalculate(logEvent);
      if (this.IncludeNdc)
        this.NdcLayout.Precalculate(logEvent);
      if (this.IncludeMdlc)
        this.MdlcLayout.Precalculate(logEvent);
      if (!this.IncludeNdlc)
        return;
      this.NdlcLayout.Precalculate(logEvent);
    }

    protected override string GetFormattedMessage(LogEventInfo logEvent)
    {
      return this.TargetLayout?.Render(logEvent) ?? string.Empty;
    }

    protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
    {
      this.TargetLayout?.RenderAppendBuilder(logEvent, target);
    }

    [ThreadSafe]
    public class LayoutContextMdc : Layout
    {
      private readonly TargetWithContext _owner;

      public bool IsActive { get; set; }

      public LayoutContextMdc(TargetWithContext owner) => this._owner = owner;

      protected override string GetFormattedMessage(LogEventInfo logEvent)
      {
        this.CaptureContext(logEvent);
        return string.Empty;
      }

      public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

      private void CaptureContext(LogEventInfo logEvent)
      {
        if (!this.IsActive)
          return;
        IDictionary<string, object> dictionary = this._owner.CaptureContextMdc(logEvent, (IDictionary<string, object>) null);
        logEvent.AddCachedLayoutValue((Layout) this, (object) dictionary);
      }
    }

    [ThreadSafe]
    public class LayoutContextMdlc : Layout
    {
      private readonly TargetWithContext _owner;

      public bool IsActive { get; set; }

      public LayoutContextMdlc(TargetWithContext owner) => this._owner = owner;

      protected override string GetFormattedMessage(LogEventInfo logEvent)
      {
        this.CaptureContext(logEvent);
        return string.Empty;
      }

      public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

      private void CaptureContext(LogEventInfo logEvent)
      {
        if (!this.IsActive)
          return;
        IDictionary<string, object> dictionary = this._owner.CaptureContextMdlc(logEvent, (IDictionary<string, object>) null);
        logEvent.AddCachedLayoutValue((Layout) this, (object) dictionary);
      }
    }

    [ThreadSafe]
    public class LayoutContextNdc : Layout
    {
      private readonly TargetWithContext _owner;

      public bool IsActive { get; set; }

      public LayoutContextNdc(TargetWithContext owner) => this._owner = owner;

      protected override string GetFormattedMessage(LogEventInfo logEvent)
      {
        this.CaptureContext(logEvent);
        return string.Empty;
      }

      public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

      private void CaptureContext(LogEventInfo logEvent)
      {
        if (!this.IsActive)
          return;
        IList<object> objectList = this._owner.CaptureContextNdc(logEvent);
        logEvent.AddCachedLayoutValue((Layout) this, (object) objectList);
      }
    }

    [ThreadSafe]
    public class LayoutContextNdlc : Layout
    {
      private readonly TargetWithContext _owner;

      public bool IsActive { get; set; }

      public LayoutContextNdlc(TargetWithContext owner) => this._owner = owner;

      protected override string GetFormattedMessage(LogEventInfo logEvent)
      {
        this.CaptureContext(logEvent);
        return string.Empty;
      }

      public override void Precalculate(LogEventInfo logEvent) => this.CaptureContext(logEvent);

      private void CaptureContext(LogEventInfo logEvent)
      {
        if (!this.IsActive)
          return;
        IList<object> objectList = this._owner.CaptureContextNdlc(logEvent);
        logEvent.AddCachedLayoutValue((Layout) this, (object) objectList);
      }
    }
  }
}
