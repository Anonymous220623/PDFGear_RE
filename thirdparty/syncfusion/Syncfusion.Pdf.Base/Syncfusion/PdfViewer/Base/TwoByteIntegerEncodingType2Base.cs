// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.TwoByteIntegerEncodingType2Base
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class TwoByteIntegerEncodingType2Base : ByteEncodingBase
{
  public TwoByteIntegerEncodingType2Base()
    : base((byte) 251, (byte) 254)
  {
  }

  public override object Read(EncodedDataParser reader)
  {
    return (object) (short) (-((int) reader.Read() - 251) * 256 /*0x0100*/ - (int) reader.Read() - 108);
  }
}
