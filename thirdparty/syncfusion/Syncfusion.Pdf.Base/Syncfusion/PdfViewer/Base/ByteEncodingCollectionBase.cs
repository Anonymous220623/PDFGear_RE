// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.ByteEncodingCollectionBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class ByteEncodingCollectionBase : List<ByteEncodingBase>
{
  public ByteEncodingBase FindEncoding(byte b0)
  {
    foreach (ByteEncodingBase encoding in (List<ByteEncodingBase>) this)
    {
      if (encoding.IsInRange(b0))
        return encoding;
    }
    return (ByteEncodingBase) null;
  }
}
