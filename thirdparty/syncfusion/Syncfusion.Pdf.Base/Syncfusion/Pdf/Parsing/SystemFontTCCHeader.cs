// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTCCHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTCCHeader
{
  private readonly SystemFontTrueTypeCollection collection;
  private uint[] offsetTable;
  private SystemFontOpenTypeFontSourceBase[] fonts;

  protected SystemFontOpenTypeFontReader Reader => this.collection.Reader;

  public SystemFontOpenTypeFontSourceBase[] Fonts
  {
    get
    {
      if (this.fonts == null)
      {
        this.fonts = new SystemFontOpenTypeFontSourceBase[this.offsetTable.Length];
        for (int index = 0; index < this.offsetTable.Length; ++index)
          this.fonts[index] = (SystemFontOpenTypeFontSourceBase) SystemFontTCCHeader.ReadTrueTypeFontFile(this.Reader, this.offsetTable[index]);
      }
      return this.fonts;
    }
  }

  private static SystemFontOpenTypeFontSource ReadTrueTypeFontFile(
    SystemFontOpenTypeFontReader reader,
    uint offset)
  {
    reader.BeginReadingBlock();
    reader.Seek((long) offset, SeekOrigin.Begin);
    SystemFontOpenTypeFontSource openTypeFontSource = new SystemFontOpenTypeFontSource(reader);
    reader.EndReadingBlock();
    return openTypeFontSource;
  }

  public SystemFontTCCHeader(SystemFontTrueTypeCollection collection)
  {
    this.collection = collection;
  }

  public void Read(SystemFontOpenTypeFontReader reader)
  {
    int num1 = (int) reader.ReadULong();
    double num2 = (double) reader.ReadFixed();
    uint length = reader.ReadULong();
    this.offsetTable = new uint[(IntPtr) length];
    for (int index = 0; (long) index < (long) length; ++index)
      this.offsetTable[index] = reader.ReadULong();
  }
}
