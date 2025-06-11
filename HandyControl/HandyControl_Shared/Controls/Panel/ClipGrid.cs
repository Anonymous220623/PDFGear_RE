// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ClipGrid
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class ClipGrid : Grid
{
  public static readonly DependencyProperty IsClipEnabledProperty = DependencyProperty.Register(nameof (IsClipEnabled), typeof (bool), typeof (ClipGrid), new PropertyMetadata(ValueBoxes.TrueBox));

  public bool IsClipEnabled
  {
    get => (bool) this.GetValue(ClipGrid.IsClipEnabledProperty);
    set => this.SetValue(ClipGrid.IsClipEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  protected override Geometry GetLayoutClip(Size layoutSlotSize)
  {
    return this.IsClipEnabled ? base.GetLayoutClip(layoutSlotSize) : (Geometry) null;
  }
}
