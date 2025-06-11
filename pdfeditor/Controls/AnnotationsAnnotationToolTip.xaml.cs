// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationToolTip
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class AnnotationToolTip : System.Windows.Controls.ToolTip
{
  public static readonly DependencyProperty HeaderProperty = HeaderedContentControl.HeaderProperty.AddOwner(typeof (AnnotationToolTip));
  public static readonly DependencyProperty HeaderTemplateProperty = HeaderedContentControl.HeaderTemplateProperty.AddOwner(typeof (AnnotationToolTip));

  static AnnotationToolTip()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (AnnotationToolTip), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (AnnotationToolTip)));
  }

  public object Header
  {
    get => this.GetValue(AnnotationToolTip.HeaderProperty);
    set => this.SetValue(AnnotationToolTip.HeaderProperty, value);
  }

  public DataTemplate HeaderTemplate
  {
    get => (DataTemplate) this.GetValue(AnnotationToolTip.HeaderTemplateProperty);
    set => this.SetValue(AnnotationToolTip.HeaderTemplateProperty, (object) value);
  }
}
