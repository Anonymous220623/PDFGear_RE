// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TiffHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal struct TiffHeader
{
  public const int ByteOrderSize = 2;
  public const int VersionSize = 2;
  public const int DirOffsetSize = 4;
  public const int SizeInBytes = 8;
  public short m_byteOrder;
  public short m_version;
  public uint m_dirOffset;
}
