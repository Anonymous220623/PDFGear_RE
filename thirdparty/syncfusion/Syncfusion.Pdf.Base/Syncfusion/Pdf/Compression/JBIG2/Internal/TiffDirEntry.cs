// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.TiffDirEntry
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class TiffDirEntry
{
  public const int SizeInBytes = 12;
  public Syncfusion.Pdf.Compression.JBIG2.TiffTag tdir_tag;
  public Syncfusion.Pdf.Compression.JBIG2.TiffType tdir_type;
  public int tdir_count;
  public uint tdir_offset;

  public new string ToString()
  {
    return $"{this.tdir_tag.ToString()}, {this.tdir_type.ToString()} {this.tdir_offset.ToString((IFormatProvider) CultureInfo.InvariantCulture)}";
  }
}
