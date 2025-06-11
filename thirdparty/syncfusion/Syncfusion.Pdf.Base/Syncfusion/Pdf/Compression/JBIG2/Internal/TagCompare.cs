// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Internal.TagCompare
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2.Internal;

internal class TagCompare : IComparer
{
  int IComparer.Compare(object x, object y)
  {
    TiffFieldInfo tiffFieldInfo1 = x as TiffFieldInfo;
    TiffFieldInfo tiffFieldInfo2 = y as TiffFieldInfo;
    if (tiffFieldInfo1.Tag != tiffFieldInfo2.Tag)
      return tiffFieldInfo1.Tag - tiffFieldInfo2.Tag;
    return tiffFieldInfo1.Type != Syncfusion.Pdf.Compression.JBIG2.TiffType.NOTYPE ? (int) (tiffFieldInfo2.Type - tiffFieldInfo1.Type) : 0;
  }
}
