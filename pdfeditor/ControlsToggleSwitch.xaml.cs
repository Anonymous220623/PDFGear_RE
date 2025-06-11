// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ToggleSwitch
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace pdfeditor.Controls;

public partial class ToggleSwitch : ToggleButton
{
  private long loadedTick;

  static ToggleSwitch()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToggleSwitch), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToggleSwitch)));
  }

  public ToggleSwitch() => this.Loaded += new RoutedEventHandler(this.ToggleSwitch_Loaded);

  private void ToggleSwitch_Loaded(object sender, RoutedEventArgs e)
  {
    this.loadedTick = Stopwatch.GetTimestamp();
  }

  protected override void OnChecked(RoutedEventArgs e)
  {
    base.OnChecked(e);
    if (Stopwatch.GetTimestamp() - this.loadedTick >= 5000000L)
      return;
    VisualStateManager.GoToState((FrameworkElement) this, "Unchecked", false);
    VisualStateManager.GoToState((FrameworkElement) this, "Checked", false);
  }
}
