// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Cmap6
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class Cmap6 : CmapTables
{
  private ushort firstCode;
  private ushort[] glyphIdArray;

  public override ushort FirstCode => this.firstCode;

  public override ushort GetGlyphId(ushort charCode)
  {
    return (int) this.firstCode <= (int) charCode && (int) charCode < (int) this.firstCode + this.glyphIdArray.Length ? this.glyphIdArray[(int) charCode - (int) this.firstCode] : (ushort) 0;
  }

  public override void Read(ReadFontArray reader)
  {
    int num1 = (int) reader.getnextUshort();
    int num2 = (int) reader.getnextUshort();
    this.firstCode = reader.getnextUshort();
    ushort length = reader.getnextUshort();
    this.glyphIdArray = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.glyphIdArray[index] = reader.getnextUshort();
  }
}
