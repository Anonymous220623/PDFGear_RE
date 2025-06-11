// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CompareSlider
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class CompareSlider : Slider
{
  public static readonly DependencyProperty TargetContentProperty = DependencyProperty.Register(nameof (TargetContent), typeof (object), typeof (CompareSlider), new PropertyMetadata((object) null));
  public static readonly DependencyProperty SourceContentProperty = DependencyProperty.Register(nameof (SourceContent), typeof (object), typeof (CompareSlider), new PropertyMetadata((object) null));

  public object TargetContent
  {
    get => this.GetValue(CompareSlider.TargetContentProperty);
    set => this.SetValue(CompareSlider.TargetContentProperty, value);
  }

  public object SourceContent
  {
    get => this.GetValue(CompareSlider.SourceContentProperty);
    set => this.SetValue(CompareSlider.SourceContentProperty, value);
  }
}
