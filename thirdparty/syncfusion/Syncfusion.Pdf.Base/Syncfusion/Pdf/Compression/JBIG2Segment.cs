// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2Segment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal struct JBIG2Segment
{
  private uint m_number;
  private byte m_deferredNonRetain;
  private byte m_pageAssocSize;
  private byte m_sType;
  private byte m_segmentCount;
  private byte m_retainBits;

  internal uint Number
  {
    get => this.m_number;
    set => this.m_number = value;
  }

  internal byte DeferredNonRetain
  {
    get => this.m_deferredNonRetain;
    set => this.m_deferredNonRetain = value;
  }

  internal byte PageAssocSize
  {
    get => this.m_pageAssocSize;
    set => this.m_pageAssocSize = value;
  }

  internal byte SType
  {
    get => this.m_sType;
    set => this.m_sType = value;
  }

  internal byte SegmentCount
  {
    get => this.m_segmentCount;
    set => this.m_segmentCount = value;
  }

  internal byte RetainBits
  {
    get => this.m_retainBits;
    set => this.m_retainBits = value;
  }

  internal byte[] Serialize()
  {
    byte[] destinationArray = new byte[6];
    Array.Copy((Array) BitConverter.GetBytes(this.Number), (Array) destinationArray, 4);
    string str1 = Convert.ToString(this.DeferredNonRetain, 2);
    string str2 = Convert.ToString(this.PageAssocSize, 2);
    string str3;
    string[] strArray1;
    for (str3 = Convert.ToString(this.SType, 2); str3.Length < 6; str3 = string.Concat(strArray1))
      strArray1 = new string[1]{ "0" + str3 };
    string str4 = str1 + str2 + str3;
    int[] bits1 = new int[str4.Length];
    int index1 = 0;
    foreach (int num in str4.ToCharArray())
    {
      bits1[index1] = int.Parse(str4[index1].ToString());
      ++index1;
    }
    byte[] byteArray1 = this.ToByteArray(bits1);
    destinationArray[4] = byteArray1[0];
    string str5;
    string[] strArray2;
    for (str5 = Convert.ToString(this.SegmentCount, 2); str5.Length < 3; str5 = string.Concat(strArray2))
      strArray2 = new string[1]{ "0" + str5 };
    string str6;
    string[] strArray3;
    for (str6 = Convert.ToString(this.RetainBits, 2); str6.Length < 5; str6 = string.Concat(strArray3))
      strArray3 = new string[1]{ "0" + str6 };
    string str7 = str5 + str6;
    int[] bits2 = new int[str7.Length];
    int index2 = 0;
    foreach (int num in str7.ToCharArray())
    {
      bits2[index2] = int.Parse(str7[index2].ToString());
      ++index2;
    }
    byte[] byteArray2 = this.ToByteArray(bits2);
    destinationArray[5] = byteArray2[0];
    return destinationArray;
  }

  private byte[] ToByteArray(int[] bits)
  {
    int length = bits.Length / 8;
    if (bits.Length % 8 != 0)
      ++length;
    byte[] byteArray = new byte[length];
    int index1 = 0;
    int num = 0;
    for (int index2 = 0; index2 < bits.Length; ++index2)
    {
      if (bits[index2] != 0)
        byteArray[index1] |= (byte) (1 << 7 - num);
      ++num;
      if (num == 8)
      {
        num = 0;
        ++index1;
      }
    }
    return byteArray;
  }
}
