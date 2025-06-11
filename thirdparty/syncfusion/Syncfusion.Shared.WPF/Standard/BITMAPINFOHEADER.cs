// Decompiled with JetBrains decompiler
// Type: Standard.BITMAPINFOHEADER
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct BITMAPINFOHEADER
{
  public int biSize;
  public int biWidth;
  public int biHeight;
  public short biPlanes;
  public short biBitCount;
  public BI biCompression;
  public int biSizeImage;
  public int biXPelsPerMeter;
  public int biYPelsPerMeter;
  public int biClrUsed;
  public int biClrImportant;
}
