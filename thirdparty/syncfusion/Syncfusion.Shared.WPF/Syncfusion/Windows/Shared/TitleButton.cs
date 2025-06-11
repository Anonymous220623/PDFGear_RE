// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TitleButton
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class TitleButton : Button
{
  public static readonly DependencyProperty IsCloseButtonProperty = DependencyProperty.Register(nameof (IsCloseButton), typeof (bool), typeof (TitleButton), (PropertyMetadata) new UIPropertyMetadata((object) false));

  static TitleButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TitleButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TitleButton)));
  }

  public bool IsCloseButton
  {
    get => (bool) this.GetValue(TitleButton.IsCloseButtonProperty);
    internal set => this.SetValue(TitleButton.IsCloseButtonProperty, (object) value);
  }
}
