// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.LabelButton
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace PDFLauncher.CustomControl;

public class LabelButton : Button
{
  public static readonly DependencyProperty ImgMarginProperty = DependencyProperty.Register(nameof (ImgMargin), typeof (Thickness), typeof (LabelButton), new PropertyMetadata((object) new Thickness(5.0, 5.0, 5.0, 5.0)));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (LabelButton), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (LabelButton), new PropertyMetadata((object) "Button"));
  public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof (IconWidth), typeof (double), typeof (LabelButton), new PropertyMetadata((object) 32.0));
  public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof (IconHeight), typeof (double), typeof (LabelButton), new PropertyMetadata((object) 32.0));
  public static readonly DependencyProperty HasChildrenProperty = DependencyProperty.Register(nameof (HasChildren), typeof (bool), typeof (LabelButton), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(nameof (ShowIcon), typeof (bool), typeof (LabelButton), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ArrowVisibilityProperty = DependencyProperty.Register(nameof (ArrowVisibility), typeof (Visibility), typeof (LabelButton), new PropertyMetadata((object) Visibility.Visible));

  static LabelButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (LabelButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (LabelButton)));
  }

  public Thickness ImgMargin
  {
    get => (Thickness) this.GetValue(LabelButton.ImgMarginProperty);
    set => this.SetValue(LabelButton.ImgMarginProperty, (object) value);
  }

  public ImageSource Icon
  {
    get => (ImageSource) this.GetValue(LabelButton.IconProperty);
    set => this.SetValue(LabelButton.IconProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(LabelButton.TextProperty);
    set => this.SetValue(LabelButton.TextProperty, (object) value);
  }

  public double IconWidth
  {
    get => (double) this.GetValue(LabelButton.IconWidthProperty);
    set => this.SetValue(LabelButton.IconWidthProperty, (object) value);
  }

  public double IconHeight
  {
    get => (double) this.GetValue(LabelButton.IconHeightProperty);
    set => this.SetValue(LabelButton.IconHeightProperty, (object) value);
  }

  public bool HasChildren
  {
    get => (bool) this.GetValue(LabelButton.HasChildrenProperty);
    set => this.SetValue(LabelButton.HasChildrenProperty, (object) value);
  }

  public bool ShowIcon
  {
    get => (bool) this.GetValue(LabelButton.ShowIconProperty);
    set => this.SetValue(LabelButton.ShowIconProperty, (object) value);
  }

  public Visibility ArrowVisibility
  {
    get => (Visibility) this.GetValue(LabelButton.ArrowVisibilityProperty);
    set => this.SetValue(LabelButton.ArrowVisibilityProperty, (object) value);
  }
}
