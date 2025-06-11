// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ContextMenuToggleButton
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class ContextMenuToggleButton : ToggleButton
{
  public ContextMenu Menu { get; set; }

  protected override void OnClick()
  {
    base.OnClick();
    if (this.Menu == null)
      return;
    bool? isChecked = this.IsChecked;
    bool flag = true;
    if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
    {
      this.Menu.PlacementTarget = (UIElement) this;
      this.Menu.IsOpen = true;
    }
    else
      this.Menu.IsOpen = false;
  }
}
