// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCMapFormat4Table
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCMapFormat4Table : SystemFontCMapTable
{
  private SystemFontSegment[] segments;
  private ushort firstCode;

  public override ushort FirstCode => this.firstCode;

  public override ushort GetGlyphId(ushort charCode)
  {
    foreach (SystemFontSegment segment in this.segments)
    {
      if (segment.IsInside(charCode))
        return segment.GetGlyphId(charCode);
    }
    return 0;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num1 = (int) reader.ReadUShort();
    int num2 = (int) reader.ReadUShort();
    ushort length1 = (ushort) ((uint) reader.ReadUShort() / 2U);
    int num3 = (int) reader.ReadUShort();
    int num4 = (int) reader.ReadUShort();
    int num5 = (int) reader.ReadUShort();
    ushort[] numArray1 = new ushort[(int) length1];
    ushort[] numArray2 = new ushort[(int) length1];
    short[] numArray3 = new short[(int) length1];
    ushort[] numArray4 = new ushort[(int) length1];
    this.segments = new SystemFontSegment[(int) length1];
    this.firstCode = ushort.MaxValue;
    for (int index = 0; index < (int) length1; ++index)
      numArray1[index] = reader.ReadUShort();
    int num6 = (int) reader.ReadUShort();
    for (int index = 0; index < (int) length1; ++index)
    {
      numArray2[index] = reader.ReadUShort();
      if ((int) this.firstCode > (int) numArray2[index])
        this.firstCode = numArray2[index];
    }
    for (int index = 0; index < (int) length1; ++index)
      numArray3[index] = reader.ReadShort();
    for (int index1 = 0; index1 < (int) length1; ++index1)
    {
      long position = reader.Position;
      numArray4[index1] = reader.ReadUShort();
      if (numArray4[index1] <= (ushort) 0)
      {
        this.segments[index1] = new SystemFontSegment(numArray2[index1], numArray1[index1], numArray3[index1]);
      }
      else
      {
        long offset = position + (long) numArray4[index1];
        int length2 = (int) numArray1[index1] - (int) numArray2[index1] + 1;
        ushort[] map = new ushort[length2];
        reader.BeginReadingBlock();
        reader.Seek(offset, SeekOrigin.Begin);
        for (int index2 = 0; index2 < length2; ++index2)
          map[index2] = reader.ReadUShort();
        this.segments[index1] = new SystemFontSegment(numArray2[index1], numArray1[index1], numArray3[index1], map);
        reader.EndReadingBlock();
      }
    }
  }

  public override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) 4);
    writer.WriteUShort(this.firstCode);
    writer.WriteUShort((ushort) this.segments.Length);
    for (int index = 0; index < this.segments.Length; ++index)
      this.segments[index].Write(writer);
  }

  public override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.firstCode = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.segments = new SystemFontSegment[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      SystemFontSegment systemFontSegment = new SystemFontSegment();
      systemFontSegment.Import(reader);
      this.segments[index] = systemFontSegment;
    }
  }
}
