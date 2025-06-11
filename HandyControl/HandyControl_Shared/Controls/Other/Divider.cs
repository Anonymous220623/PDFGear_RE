// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Divider
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[System.Windows.Markup.ContentProperty("Content")]
public class Divider : Control
{
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (Divider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (Divider), new PropertyMetadata((object) Orientation.Horizontal));
  public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof (ContentTemplate), typeof (DataTemplate), typeof (Divider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(nameof (ContentStringFormat), typeof (string), typeof (Divider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(nameof (ContentTemplateSelector), typeof (DataTemplateSelector), typeof (Divider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(nameof (LineStroke), typeof (Brush), typeof (Divider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty LineStrokeThicknessProperty = DependencyProperty.Register(nameof (LineStrokeThickness), typeof (double), typeof (Divider), new PropertyMetadata(ValueBoxes.Double1Box));
  public static readonly DependencyProperty LineStrokeDashArrayProperty = DependencyProperty.Register(nameof (LineStrokeDashArray), typeof (DoubleCollection), typeof (Divider), new PropertyMetadata((object) new DoubleCollection()));

  public object Content
  {
    get => this.GetValue(Divider.ContentProperty);
    set => this.SetValue(Divider.ContentProperty, value);
  }

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(Divider.OrientationProperty);
    set => this.SetValue(Divider.OrientationProperty, (object) value);
  }

  public DataTemplate ContentTemplate
  {
    get => (DataTemplate) this.GetValue(Divider.ContentTemplateProperty);
    set => this.SetValue(Divider.ContentTemplateProperty, (object) value);
  }

  public string ContentStringFormat
  {
    get => (string) this.GetValue(Divider.ContentStringFormatProperty);
    set => this.SetValue(Divider.ContentStringFormatProperty, (object) value);
  }

  public DataTemplateSelector ContentTemplateSelector
  {
    get => (DataTemplateSelector) this.GetValue(Divider.ContentTemplateSelectorProperty);
    set => this.SetValue(Divider.ContentTemplateSelectorProperty, (object) value);
  }

  public Brush LineStroke
  {
    get => (Brush) this.GetValue(Divider.LineStrokeProperty);
    set => this.SetValue(Divider.LineStrokeProperty, (object) value);
  }

  public double LineStrokeThickness
  {
    get => (double) this.GetValue(Divider.LineStrokeThicknessProperty);
    set => this.SetValue(Divider.LineStrokeThicknessProperty, (object) value);
  }

  public DoubleCollection LineStrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(Divider.LineStrokeDashArrayProperty);
    set => this.SetValue(Divider.LineStrokeDashArrayProperty, (object) value);
  }
}
