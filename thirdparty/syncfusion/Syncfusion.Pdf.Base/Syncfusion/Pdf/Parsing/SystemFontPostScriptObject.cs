// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptObject
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontPostScriptObject
{
  private readonly Dictionary<string, ISystemFontProperty> properties;

  public SystemFontPostScriptObject()
  {
    this.properties = new Dictionary<string, ISystemFontProperty>();
  }

  public void Load(SystemFontPostScriptDictionary fromDict)
  {
    foreach (KeyValuePair<string, object> keyValuePair in fromDict)
    {
      ISystemFontProperty systemFontProperty;
      if (this.properties.TryGetValue(keyValuePair.Key, out systemFontProperty))
        systemFontProperty.SetValue(keyValuePair.Value);
    }
  }

  protected SystemFontProperty<T> CreateProperty<T>(SystemFontPropertyDescriptor descriptor)
  {
    SystemFontProperty<T> property = new SystemFontProperty<T>(descriptor);
    this.RegisterProperty((ISystemFontProperty) property);
    return property;
  }

  protected SystemFontProperty<T> CreateProperty<T>(
    SystemFontPropertyDescriptor descriptor,
    ISystemFontConverter converter)
  {
    SystemFontProperty<T> property = new SystemFontProperty<T>(descriptor, converter);
    this.RegisterProperty((ISystemFontProperty) property);
    return property;
  }

  protected SystemFontProperty<T> CreateProperty<T>(
    SystemFontPropertyDescriptor descriptor,
    T defaultValue)
  {
    SystemFontProperty<T> property = new SystemFontProperty<T>(descriptor, defaultValue);
    this.RegisterProperty((ISystemFontProperty) property);
    return property;
  }

  protected SystemFontProperty<T> CreateProperty<T>(
    SystemFontPropertyDescriptor descriptor,
    ISystemFontConverter converter,
    T defaultValue)
  {
    SystemFontProperty<T> property = new SystemFontProperty<T>(descriptor, converter, defaultValue);
    this.RegisterProperty((ISystemFontProperty) property);
    return property;
  }

  private void RegisterProperty(
    SystemFontPropertyDescriptor descriptor,
    ISystemFontProperty property)
  {
    if (descriptor.Name == null)
      return;
    this.properties[descriptor.Name] = property;
  }

  private void RegisterProperty(ISystemFontProperty property)
  {
    this.RegisterProperty(property.Descriptor, property);
  }
}
