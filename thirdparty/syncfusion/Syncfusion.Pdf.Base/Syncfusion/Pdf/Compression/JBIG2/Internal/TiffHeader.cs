// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.TiffHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal struct TiffHeader
{
  public const int TIFF_MAGIC_SIZE = 2;
  public const int TIFF_VERSION_SIZE = 2;
  public const int TIFF_DIROFFSET_SIZE = 4;
  public const int SizeInBytes = 8;
  public short tiff_magic;
  public short tiff_version;
  public uint tiff_diroff;
}
