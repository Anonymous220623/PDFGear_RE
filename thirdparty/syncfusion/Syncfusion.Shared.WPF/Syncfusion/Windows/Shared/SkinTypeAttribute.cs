// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SkinTypeAttribute
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Windows.Shared;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public class SkinTypeAttribute : Attribute
{
  private Skin skinVisualStyle;
  private string xamlResource;
  private Type type;

  public Skin SkinVisualStyle
  {
    get => this.skinVisualStyle;
    set => this.skinVisualStyle = value;
  }

  public string XamlResource
  {
    get => this.xamlResource;
    set => this.xamlResource = value;
  }

  public Type Type
  {
    get => this.type;
    set => this.type = value;
  }
}
