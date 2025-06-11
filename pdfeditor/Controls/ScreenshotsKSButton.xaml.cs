// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.KSButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class KSButton : Button
{
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (KSButton), new PropertyMetadata((object) new CornerRadius(0.0)));

  static KSButton()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (KSButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (KSButton)));
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(KSButton.CornerRadiusProperty);
    set => this.SetValue(KSButton.CornerRadiusProperty, (object) value);
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    this.Focusable = true;
    this.Focus();
    this.Focusable = false;
  }
}
