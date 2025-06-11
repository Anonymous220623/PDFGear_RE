// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.NavigateButtonBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class NavigateButtonBase : ButtonBase
{
  private CornerRadius mcornerRadius;

  static NavigateButtonBase()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (NavigateButtonBase), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (NavigateButtonBase)));
    Border.CornerRadiusProperty.AddOwner(typeof (NavigateButtonBase));
  }

  public CornerRadius CornerRadius
  {
    get => this.mcornerRadius;
    set => this.mcornerRadius = value;
  }
}
