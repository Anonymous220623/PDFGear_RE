// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.SwitchButton
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace PDFLauncher.CustomControl;

public class SwitchButton : CheckBox
{
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (SwitchButton), new PropertyMetadata((PropertyChangedCallback) null));

  static SwitchButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SwitchButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SwitchButton)));
  }

  public ImageSource Icon
  {
    get => (ImageSource) this.GetValue(SwitchButton.IconProperty);
    set => this.SetValue(SwitchButton.IconProperty, (object) value);
  }
}
