// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSimpleGlyph
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSimpleGlyph(SystemFontOpenTypeFontSourceBase fontFile, ushort glyphIndex) : 
  SystemFontGlyphData(fontFile, glyphIndex)
{
  private List<SystemFontOutlinePoint[]> contours;

  internal override IEnumerable<SystemFontOutlinePoint[]> Contours
  {
    get => (IEnumerable<SystemFontOutlinePoint[]>) this.contours;
  }

  private static bool XIsByte(byte[] flags, int index)
  {
    return SystemFontBitsHelper.GetBit((int) flags[index], (byte) 1);
  }

  private static bool YIsByte(byte[] flags, int index)
  {
    return SystemFontBitsHelper.GetBit((int) flags[index], (byte) 2);
  }

  private static bool Repeat(byte[] flags, int index)
  {
    return SystemFontBitsHelper.GetBit((int) flags[index], (byte) 3);
  }

  private static bool XIsSame(byte[] flags, int index)
  {
    return SystemFontBitsHelper.GetBit((int) flags[index], (byte) 4);
  }

  private static bool YIsSame(byte[] flags, int index)
  {
    return SystemFontBitsHelper.GetBit((int) flags[index], (byte) 5);
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort[] numArray1 = new ushort[(int) this.NumberOfContours];
    for (int index = 0; index < (int) this.NumberOfContours; ++index)
      numArray1[index] = reader.ReadUShort();
    int length1 = (int) numArray1[(int) this.NumberOfContours - 1] + 1;
    ushort length2 = reader.ReadUShort();
    byte[] numArray2 = new byte[(int) length2];
    for (int index = 0; index < (int) length2; ++index)
      numArray2[index] = reader.Read();
    byte[] flags = new byte[length1];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      flags[index1] = reader.Read();
      if (SystemFontSimpleGlyph.Repeat(flags, index1))
      {
        byte num1 = flags[index1];
        byte num2 = reader.Read();
        for (int index2 = 0; index2 < (int) num2; ++index2)
          flags[++index1] = num1;
      }
    }
    int[] numArray3 = new int[length1];
    for (int index = 0; index < length1; ++index)
    {
      if (index > 0)
        numArray3[index] = numArray3[index - 1];
      if (SystemFontSimpleGlyph.XIsByte(flags, index))
      {
        int num = (int) reader.Read();
        if (!SystemFontSimpleGlyph.XIsSame(flags, index))
          num = -num;
        numArray3[index] += num;
      }
      else if (!SystemFontSimpleGlyph.XIsSame(flags, index))
        numArray3[index] += (int) reader.ReadShort();
    }
    int[] numArray4 = new int[length1];
    for (int index = 0; index < length1; ++index)
    {
      if (index > 0)
        numArray4[index] = numArray4[index - 1];
      if (SystemFontSimpleGlyph.YIsByte(flags, index))
      {
        int num = (int) reader.Read();
        if (!SystemFontSimpleGlyph.YIsSame(flags, index))
          num = -num;
        numArray4[index] += num;
      }
      else if (!SystemFontSimpleGlyph.YIsSame(flags, index))
        numArray4[index] += (int) reader.ReadShort();
    }
    this.contours = new List<SystemFontOutlinePoint[]>();
    int index3 = 0;
    int num3 = 0;
    SystemFontOutlinePoint[] fontOutlinePointArray = new SystemFontOutlinePoint[(int) numArray1[0] + 1];
    this.contours.Add(fontOutlinePointArray);
    for (int index4 = 0; index4 < length1; ++index4)
    {
      fontOutlinePointArray[num3++] = new SystemFontOutlinePoint((double) numArray3[index4], (double) numArray4[index4], flags[index4]);
      if (index4 == (int) numArray1[index3])
      {
        if (index3 == numArray1.Length - 1)
          break;
        ++index3;
        fontOutlinePointArray = new SystemFontOutlinePoint[(int) numArray1[index3] - (int) numArray1[index3 - 1]];
        this.contours.Add(fontOutlinePointArray);
        num3 = 0;
      }
    }
  }
}
