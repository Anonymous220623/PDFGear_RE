// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.ColorExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Collections.Generic;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class ColorExtension
{
  public static int ToInt32(this Color color)
  {
    return (int) color.R << 16 /*0x10*/ | (int) color.G << 8 | (int) color.B;
  }

  public static int ToInt32Reverse(this Color color)
  {
    return (int) color.R | (int) color.G << 8 | (int) color.B << 18;
  }

  internal static List<byte> ToList(this Color color)
  {
    return new List<byte>() { color.R, color.G, color.B };
  }
}
