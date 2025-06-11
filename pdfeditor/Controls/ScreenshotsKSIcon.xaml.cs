// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.KSIcon
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class KSIcon : Control
{
  public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(nameof (IconBrush), typeof (Brush), typeof (KSIcon), new PropertyMetadata((object) new SolidColorBrush(Colors.Gray)));
  public static readonly DependencyProperty IconGeometryProperty = DependencyProperty.Register(nameof (IconGeometry), typeof (Geometry), typeof (KSIcon), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IconPenProperty = DependencyProperty.Register(nameof (IconPen), typeof (Pen), typeof (KSIcon), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (KSIcon), new PropertyMetadata((object) new CornerRadius(0.0)));

  static KSIcon()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (KSIcon), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (KSIcon)));
  }

  public Brush IconBrush
  {
    get => (Brush) this.GetValue(KSIcon.IconBrushProperty);
    set => this.SetValue(KSIcon.IconBrushProperty, (object) value);
  }

  public Geometry IconGeometry
  {
    get => (Geometry) this.GetValue(KSIcon.IconGeometryProperty);
    set => this.SetValue(KSIcon.IconGeometryProperty, (object) value);
  }

  public Pen IconPen
  {
    get => (Pen) this.GetValue(KSIcon.IconPenProperty);
    set => this.SetValue(KSIcon.IconPenProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(KSIcon.CornerRadiusProperty);
    set => this.SetValue(KSIcon.CornerRadiusProperty, (object) value);
  }
}
