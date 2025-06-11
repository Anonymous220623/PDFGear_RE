// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Sprite
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public sealed class Sprite : System.Windows.Window
{
  private Sprite()
  {
    this.WindowStyle = WindowStyle.None;
    this.AllowsTransparency = true;
  }

  public static Sprite Show(object content)
  {
    Sprite sprite1 = new Sprite();
    sprite1.Content = content;
    Sprite sprite2 = sprite1;
    sprite2.Show();
    Rect workArea = SystemParameters.WorkArea;
    sprite2.Left = workArea.Width - sprite2.ActualWidth - 50.0;
    sprite2.Top = 50.0 - sprite2.Padding.Top;
    return sprite2;
  }
}
