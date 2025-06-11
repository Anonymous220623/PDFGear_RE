// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Cmap4
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class Cmap4 : CmapTables
{
  private Segments[] segments;
  private ushort m_firstCode;

  public override ushort FirstCode => this.m_firstCode;

  public override ushort GetGlyphId(ushort charCode)
  {
    foreach (Segments segment in this.segments)
    {
      if (segment.IsContain(charCode))
        return segment.GetGlyphId(charCode);
    }
    return 0;
  }

  public override void Read(ReadFontArray reader)
  {
    int num1 = (int) reader.getnextUshort();
    int num2 = (int) reader.getnextUshort();
    ushort length = (ushort) ((uint) reader.getnextUshort() / 2U);
    int num3 = (int) reader.getnextUshort();
    int num4 = (int) reader.getnextUshort();
    int num5 = (int) reader.getnextUshort();
    ushort[] numArray1 = new ushort[(int) length];
    ushort[] numArray2 = new ushort[(int) length];
    short[] numArray3 = new short[(int) length];
    ushort[] numArray4 = new ushort[(int) length];
    this.segments = new Segments[(int) length];
    this.m_firstCode = ushort.MaxValue;
    for (int index = 0; index < (int) length; ++index)
      numArray1[index] = reader.getnextUshort();
    int num6 = (int) reader.getnextUshort();
    for (int index = 0; index < (int) length; ++index)
    {
      numArray2[index] = reader.getnextUshort();
      if ((int) this.m_firstCode > (int) numArray2[index])
        this.m_firstCode = numArray2[index];
    }
    for (int index = 0; index < (int) length; ++index)
      numArray3[index] = reader.getnextshort();
    for (int index1 = 0; index1 < (int) length; ++index1)
    {
      long pointer1 = (long) reader.Pointer;
      numArray4[index1] = reader.getnextUshort();
      if (numArray4[index1] <= (ushort) 0)
      {
        this.segments[index1] = new Segments(numArray2[index1], numArray1[index1], numArray3[index1]);
      }
      else
      {
        int pointer2 = reader.Pointer;
        long num7 = pointer1 + (long) numArray4[index1];
        ushort[] mapval = new ushort[(int) numArray1[index1] - (int) numArray2[index1] + 1];
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        reader.Pointer = (int) num7;
        for (int index2 = 0; index2 < mapval.Length; ++index2)
          mapval[index2] = reader.getnextUshort();
        this.segments[index1] = new Segments(numArray2[index1], numArray1[index1], numArray3[index1], mapval);
        reader.Pointer = pointer2;
      }
    }
  }
}
