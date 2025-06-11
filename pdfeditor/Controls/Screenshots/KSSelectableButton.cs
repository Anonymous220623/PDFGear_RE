// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.KSSelectableButton
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class KSSelectableButton : KSButton
{
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (KSButton), new PropertyMetadata((object) false));

  public bool IsSelected
  {
    get => (bool) this.GetValue(KSSelectableButton.IsSelectedProperty);
    set => this.SetValue(KSSelectableButton.IsSelectedProperty, (object) value);
  }
}
