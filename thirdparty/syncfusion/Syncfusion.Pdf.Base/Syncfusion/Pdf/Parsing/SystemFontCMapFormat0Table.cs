// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCMapFormat0Table
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCMapFormat0Table : SystemFontCMapTable
{
  private const int GLYPH_IDS = 256 /*0x0100*/;
  private byte[] glyphIdArray;

  public override ushort FirstCode => throw new NotSupportedException();

  public override ushort GetGlyphId(ushort charCode)
  {
    return charCode >= (ushort) 0 && (int) charCode < this.glyphIdArray.Length ? (ushort) this.glyphIdArray[(int) charCode] : (ushort) 0;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num1 = (int) reader.ReadUShort();
    int num2 = (int) reader.ReadUShort();
    this.glyphIdArray = new byte[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      this.glyphIdArray[index] = reader.Read();
  }

  public override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) 0);
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      writer.Write(this.glyphIdArray[index]);
  }

  public override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.glyphIdArray = new byte[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
      this.glyphIdArray[index] = reader.Read();
  }
}
