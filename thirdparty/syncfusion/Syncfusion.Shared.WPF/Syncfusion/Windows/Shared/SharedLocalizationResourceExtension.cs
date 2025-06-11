// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SharedLocalizationResourceExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class SharedLocalizationResourceExtension : MarkupExtension
{
  public string ResourceName { get; set; }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    return (object) SharedLocalizationResourceAccessor.Instance.GetString(this.ResourceName);
  }
}
