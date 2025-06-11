// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.KeyProperty`1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class KeyProperty<T> : IProperty
{
  private readonly IConverter converter;
  private T value;

  public KeyPropertyDescriptor Descriptor { get; set; }

  public KeyProperty(KeyPropertyDescriptor descriptor) => this.Descriptor = descriptor;

  public KeyProperty(KeyPropertyDescriptor descriptor, IConverter converter)
    : this(descriptor)
  {
    this.converter = converter;
  }

  public KeyProperty(KeyPropertyDescriptor descriptor, T defaultValue)
    : this(descriptor)
  {
    this.value = defaultValue;
  }

  public KeyProperty(KeyPropertyDescriptor descriptor, IConverter converter, T defaultValue)
    : this(descriptor)
  {
    this.value = defaultValue;
    this.converter = converter;
  }

  public T GetValue() => this.value;

  public bool SetValue(object value)
  {
    if (value is T obj)
    {
      this.value = obj;
      return true;
    }
    if (this.converter == null)
      return false;
    this.value = (T) this.converter.Convert(typeof (T), value);
    return true;
  }
}
