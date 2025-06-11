// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontEncoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontEncoding : SystemFontCFFTable, ISystemFontEncoding
{
  private readonly SystemFontCharset charset;
  private List<ushort> gids;

  public SystemFontSupplementalEncoding SupplementalEncoding { get; private set; }

  public SystemFontEncoding(SystemFontCFFFontFile file, SystemFontCharset charset, long offset)
    : base(file, offset)
  {
    this.charset = charset;
  }

  private void ReadFormat0(SystemFontCFFFontReader reader)
  {
    byte capacity = reader.ReadCard8();
    this.gids = new List<ushort>((int) capacity);
    for (int index = 0; index < (int) capacity; ++index)
      this.gids.Add((ushort) reader.ReadCard8());
  }

  private void ReadFormat1(SystemFontCFFFontReader reader)
  {
    byte num1 = reader.ReadCard8();
    this.gids = new List<ushort>();
    for (int index1 = 0; index1 < (int) num1; ++index1)
    {
      byte num2 = reader.ReadCard8();
      byte num3 = reader.ReadCard8();
      this.gids.Add((ushort) num2);
      for (int index2 = 0; index2 < (int) num3; ++index2)
        this.gids.Add((ushort) (byte) ((int) num2 + index2 + 1));
    }
  }

  public override void Read(SystemFontCFFFontReader reader)
  {
    byte n = reader.ReadCard8();
    if (SystemFontBitsHelper.GetBit((int) n, (byte) 0))
      this.ReadFormat1(reader);
    else
      this.ReadFormat0(reader);
    if (!SystemFontBitsHelper.GetBit((int) n, (byte) 7))
      return;
    this.SupplementalEncoding = new SystemFontSupplementalEncoding();
    this.SupplementalEncoding.Read(reader);
  }

  public string GetGlyphName(SystemFontCFFFontFile fontFile, ushort index)
  {
    int index1 = this.gids.IndexOf(index);
    return index1 < 0 ? ".notdef" : this.charset[(ushort) index1];
  }
}
