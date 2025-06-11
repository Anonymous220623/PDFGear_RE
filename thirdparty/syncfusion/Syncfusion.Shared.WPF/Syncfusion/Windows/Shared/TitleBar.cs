// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TitleBar
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class TitleBar : ContentControl
{
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (TitleBar), new PropertyMetadata((PropertyChangedCallback) null));

  static TitleBar()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TitleBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TitleBar)));
  }

  public override void OnApplyTemplate() => base.OnApplyTemplate();

  public ImageSource Icon
  {
    get => (ImageSource) this.GetValue(TitleBar.IconProperty);
    set => this.SetValue(TitleBar.IconProperty, (object) value);
  }

  internal Window MainWindow => this.TemplatedParent as Window;
}
