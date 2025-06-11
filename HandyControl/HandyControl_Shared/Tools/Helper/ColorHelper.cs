// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ColorHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools;

public class ColorHelper
{
  private const int Win32RedShift = 0;
  private const int Win32GreenShift = 8;
  private const int Win32BlueShift = 16 /*0x10*/;

  public static int ToWin32(Color c) => (int) c.R | (int) c.G << 8 | (int) c.B << 16 /*0x10*/;

  public static Color ToColor(uint c)
  {
    byte[] bytes = BitConverter.GetBytes(c);
    return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
  }
}
