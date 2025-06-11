// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontKerningSubTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontKerningSubTable : SystemFontTableBase
{
  public ushort Coverage { get; private set; }

  public bool IsHorizontal => this.GetBit((byte) 0);

  public bool HasMinimumValues => this.GetBit((byte) 1);

  public bool IsCrossStream => this.GetBit((byte) 2);

  public bool Override => this.GetBit((byte) 3);

  internal static SystemFontKerningSubTable ReadSubTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader)
  {
    long position = reader.Position;
    int num1 = (int) reader.ReadUShort();
    ushort num2 = reader.ReadUShort();
    ushort num3 = reader.ReadUShort();
    if (BitConverter.GetBytes(num3)[1] == (byte) 0)
    {
      SystemFontKerningSubTable fontKerningSubTable = (SystemFontKerningSubTable) new SystemFontFormat0KerningSubTable(fontSource);
      fontKerningSubTable.Coverage = num3;
      fontKerningSubTable.Read(reader);
      return fontKerningSubTable;
    }
    reader.Seek(position + (long) num2, SeekOrigin.Begin);
    return (SystemFontKerningSubTable) null;
  }

  internal static SystemFontKerningSubTable ImportSubTable(
    SystemFontOpenTypeFontSourceBase fontSource,
    SystemFontOpenTypeFontReader reader)
  {
    ushort num = reader.ReadUShort();
    if (BitConverter.GetBytes(num)[1] != (byte) 0)
      return (SystemFontKerningSubTable) null;
    SystemFontKerningSubTable fontKerningSubTable = (SystemFontKerningSubTable) new SystemFontFormat0KerningSubTable(fontSource);
    fontKerningSubTable.Coverage = num;
    fontKerningSubTable.Import(reader);
    return fontKerningSubTable;
  }

  public SystemFontKerningSubTable(SystemFontOpenTypeFontSourceBase fontSource)
    : base(fontSource)
  {
  }

  private bool GetBit(byte bit) => SystemFontBitsHelper.GetBit((int) this.Coverage, bit);

  public abstract short GetValue(ushort leftGlyphIndex, ushort rightGlyphIndex);

  internal override void Write(SystemFontFontWriter writer) => writer.WriteUShort(this.Coverage);
}
