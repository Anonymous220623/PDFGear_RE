// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PostScriptObj
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class PostScriptObj
{
  private readonly Dictionary<string, IProperty> properties;

  public PostScriptObj() => this.properties = new Dictionary<string, IProperty>();

  public void Load(PostScriptDict fromDict)
  {
    foreach (KeyValuePair<string, object> keyValuePair in fromDict)
    {
      IProperty property;
      if (this.properties.TryGetValue(keyValuePair.Key, out property))
        property.SetValue(keyValuePair.Value);
    }
  }

  protected KeyProperty<T> CreateProperty<T>(KeyPropertyDescriptor descriptor)
  {
    KeyProperty<T> property = new KeyProperty<T>(descriptor);
    this.RegisterProperty((IProperty) property);
    return property;
  }

  protected KeyProperty<T> CreateProperty<T>(KeyPropertyDescriptor descriptor, IConverter converter)
  {
    KeyProperty<T> property = new KeyProperty<T>(descriptor, converter);
    this.RegisterProperty((IProperty) property);
    return property;
  }

  protected KeyProperty<T> CreateProperty<T>(KeyPropertyDescriptor descriptor, T defaultValue)
  {
    KeyProperty<T> property = new KeyProperty<T>(descriptor, defaultValue);
    this.RegisterProperty((IProperty) property);
    return property;
  }

  protected KeyProperty<T> CreateProperty<T>(
    KeyPropertyDescriptor descriptor,
    IConverter converter,
    T defaultValue)
  {
    KeyProperty<T> property = new KeyProperty<T>(descriptor, converter, defaultValue);
    this.RegisterProperty((IProperty) property);
    return property;
  }

  private void RegisterProperty(KeyPropertyDescriptor descriptor, IProperty property)
  {
    if (descriptor.Name == null)
      return;
    this.properties[descriptor.Name] = property;
  }

  private void RegisterProperty(IProperty property)
  {
    this.RegisterProperty(property.Descriptor, property);
  }
}
