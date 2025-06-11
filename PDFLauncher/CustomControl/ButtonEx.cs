// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.ButtonEx
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace PDFLauncher.CustomControl;

public class ButtonEx : Button
{
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (ButtonEx), new PropertyMetadata((object) new CornerRadius(0.0)));
  public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(nameof (DisabledBackground), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(nameof (DisabledForeground), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty DisabledBorderbrushProperty = DependencyProperty.Register(nameof (DisabledBorderbrush), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(nameof (MouseOverBackground), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(nameof (MouseOverForeground), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MouseOverBorderbrushProperty = DependencyProperty.Register(nameof (MouseOverBorderbrush), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MousePressedBackgroundProperty = DependencyProperty.Register(nameof (MousePressedBackground), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MousePressedForegroundProperty = DependencyProperty.Register(nameof (MousePressedForeground), typeof (Brush), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty DisabledContentProperty = DependencyProperty.Register(nameof (DisabledContent), typeof (object), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MouseOverContentProperty = DependencyProperty.Register(nameof (MouseOverContent), typeof (object), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty MousePressedContentProperty = DependencyProperty.Register(nameof (MousePressedContent), typeof (object), typeof (ButtonEx), new PropertyMetadata((PropertyChangedCallback) null));

  static ButtonEx()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ButtonEx), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ButtonEx)));
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(ButtonEx.CornerRadiusProperty);
    set => this.SetValue(ButtonEx.CornerRadiusProperty, (object) value);
  }

  public Brush DisabledBackground
  {
    get => (Brush) this.GetValue(ButtonEx.DisabledBackgroundProperty);
    set => this.SetValue(ButtonEx.DisabledBackgroundProperty, (object) value);
  }

  public Brush DisabledForeground
  {
    get => (Brush) this.GetValue(ButtonEx.DisabledForegroundProperty);
    set => this.SetValue(ButtonEx.DisabledForegroundProperty, (object) value);
  }

  public Brush DisabledBorderbrush
  {
    get => (Brush) this.GetValue(ButtonEx.DisabledBorderbrushProperty);
    set => this.SetValue(ButtonEx.DisabledBorderbrushProperty, (object) value);
  }

  public Brush MouseOverBackground
  {
    get => (Brush) this.GetValue(ButtonEx.MouseOverBackgroundProperty);
    set => this.SetValue(ButtonEx.MouseOverBackgroundProperty, (object) value);
  }

  public Brush MouseOverForeground
  {
    get => (Brush) this.GetValue(ButtonEx.MouseOverForegroundProperty);
    set => this.SetValue(ButtonEx.MouseOverForegroundProperty, (object) value);
  }

  public Brush MouseOverBorderbrush
  {
    get => (Brush) this.GetValue(ButtonEx.MouseOverBorderbrushProperty);
    set => this.SetValue(ButtonEx.MouseOverBorderbrushProperty, (object) value);
  }

  public Brush MousePressedBackground
  {
    get => (Brush) this.GetValue(ButtonEx.MousePressedBackgroundProperty);
    set => this.SetValue(ButtonEx.MousePressedBackgroundProperty, (object) value);
  }

  public Brush MousePressedForeground
  {
    get => (Brush) this.GetValue(ButtonEx.MousePressedForegroundProperty);
    set => this.SetValue(ButtonEx.MousePressedForegroundProperty, (object) value);
  }

  public object DisabledContent
  {
    get => this.GetValue(ButtonEx.DisabledContentProperty);
    set => this.SetValue(ButtonEx.DisabledContentProperty, value);
  }

  public object MouseOverContent
  {
    get => this.GetValue(ButtonEx.MouseOverContentProperty);
    set => this.SetValue(ButtonEx.MouseOverContentProperty, value);
  }

  public object MousePressedContent
  {
    get => this.GetValue(ButtonEx.MousePressedContentProperty);
    set => this.SetValue(ButtonEx.MousePressedContentProperty, value);
  }
}
