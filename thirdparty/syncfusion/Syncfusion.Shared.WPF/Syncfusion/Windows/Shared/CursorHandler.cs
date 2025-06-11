// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CursorHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class CursorHandler
{
  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool GetCursorPos(out CursorHandler.POINT lpPoint);

  [DllImport("User32.dll")]
  public static extern bool SetCursorPos(int x, int y);

  public struct POINT(int x, int y)
  {
    public int X = x;
    public int Y = y;
  }
}
