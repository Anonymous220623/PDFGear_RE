// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.DrawSettingConstants
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class DrawSettingConstants
{
  public static readonly double DefaultFontSize = 12.0;
  public static readonly Color[] Colors = new Color[8]
  {
    Color.FromRgb((byte) 0, (byte) 0, (byte) 0),
    Color.FromRgb((byte) 251, (byte) 48 /*0x30*/, (byte) 47),
    Color.FromRgb((byte) 253, (byte) 153, (byte) 39),
    Color.FromRgb((byte) 254, (byte) 216, (byte) 49),
    Color.FromRgb((byte) 165, (byte) 222, (byte) 80 /*0x50*/),
    Color.FromRgb((byte) 67, (byte) 217, (byte) 239),
    Color.FromRgb((byte) 82, (byte) 170, (byte) 236),
    Color.FromRgb((byte) 149, (byte) 115, (byte) 228)
  };
  public static readonly double[] Thicknesses = new double[3]
  {
    1.0,
    3.0,
    5.0
  };
  public static readonly double[] ArrowHeight = new double[3]
  {
    12.0,
    18.0,
    26.0
  };
  public static readonly double[] ThicknessListBoxEllipseSize = new double[3]
  {
    6.0,
    10.0,
    14.0
  };
}
