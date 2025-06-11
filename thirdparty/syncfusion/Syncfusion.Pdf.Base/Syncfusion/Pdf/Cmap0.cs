// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Cmap0
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class Cmap0 : CmapTables
{
  private ushort m_firstcode;
  public byte[] glyphIdArray;

  public override ushort FirstCode => this.m_firstcode;

  public override ushort GetGlyphId(ushort charCode)
  {
    return charCode >= (ushort) 0 && (int) charCode < this.glyphIdArray.Length ? (ushort) this.glyphIdArray[(int) charCode] : (ushort) 0;
  }

  public override void Read(ReadFontArray reader)
  {
    int num1 = (int) reader.getnextUshort();
    int num2 = (int) reader.getnextUshort();
    this.glyphIdArray = new byte[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      this.glyphIdArray[index] = reader.getnextbyte();
  }
}
