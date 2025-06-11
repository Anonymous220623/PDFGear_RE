// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.FontFile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class FontFile
{
  private const int c1 = 52845;
  private const int c2 = 22719;
  private int m_skipBytes = 4;
  private Dictionary<string, byte[]> m_glyphs = new Dictionary<string, byte[]>();
  internal Dictionary<int, string> m_differenceEncoding = new Dictionary<int, string>();
  internal double[] m_fontMatrix = new double[6];
  internal bool m_hasFontMatrix;
  internal CffGlyphs m_cffGlyphs = new CffGlyphs();

  public void Type1()
  {
  }

  private void ParseDifferenceEncoding(StreamReader br)
  {
    bool flag = true;
label_12:
    string str1;
    while ((str1 = br.ReadLine()) != null)
    {
      string str2 = str1.Trim();
      if (str2.StartsWith("readonly"))
        break;
      int index1 = 0;
      if (str2.StartsWith("dup") && str2.Contains("/"))
      {
        string[] strArray = str2.Split(new string[2]
        {
          " ",
          "/"
        }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length >= 3)
        {
          string str3 = strArray[index1];
          int index2 = index1 + 1;
          string s = strArray[index2];
          int length1 = s.IndexOf('#');
          int key;
          if (length1 == -1)
          {
            key = int.Parse(s);
          }
          else
          {
            s.Substring(0, length1);
            key = int.Parse(s.Substring(length1 + 1, s.Length));
          }
          int index3 = index2 + 1;
          string str4 = strArray[index3];
          this.m_differenceEncoding.Add(key, str4);
          switch (str4[0])
          {
            case 'B':
            case 'C':
            case 'G':
            case 'c':
              int num = 1;
              int length2 = str4.Length;
              while (true)
              {
                if (!flag && num < length2)
                  flag = char.IsLetter(str4[num++]);
                else
                  goto label_12;
              }
            default:
              continue;
          }
        }
      }
    }
  }

  internal CffGlyphs ParseType1FontFile(byte[] content)
  {
    StreamReader br = new StreamReader((Stream) new MemoryStream(content));
label_1:
    string str1;
    do
    {
      str1 = br.ReadLine();
      if (str1 != null)
      {
        if (str1.StartsWith("/Encoding 256 array"))
          this.ParseDifferenceEncoding(br);
        else if (str1.StartsWith("/lenIV"))
          this.m_skipBytes = Convert.ToInt32(str1.Split(new string[1]
          {
            " "
          }, StringSplitOptions.RemoveEmptyEntries)[1]);
      }
      else
        goto label_16;
    }
    while (str1.IndexOf("/FontMatrix") == -1);
    string str2 = "";
    int num1 = str1.IndexOf('[');
    if (num1 != -1)
    {
      int num2 = str1.IndexOf(']');
      str2 = str1.Substring(num1 + 1, num2 - (num1 + 1));
    }
    else
    {
      int num3 = str1.IndexOf('{');
      if (num3 != -1)
      {
        int num4 = str1.IndexOf('}');
        str2 = str1.Substring(num3 + 1, num4 - (num3 + 1));
      }
    }
    string[] strArray = str2.Split(new string[1]{ " " }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 6)
      this.m_hasFontMatrix = true;
    for (int index = 0; index < 6; ++index)
      this.m_fontMatrix[index] = Convert.ToDouble(strArray[index]);
    goto label_1;
label_16:
    if (br != null)
    {
      try
      {
        br.Dispose();
      }
      catch (Exception ex)
      {
      }
    }
    this.m_cffGlyphs.Glyphs = this.ParseEncodedContent(content);
    this.m_cffGlyphs.FontMatrix = this.m_fontMatrix;
    this.m_cffGlyphs.DifferenceEncoding = this.m_differenceEncoding;
    int count = this.m_glyphs.Count;
    return this.m_cffGlyphs;
  }

  public Dictionary<string, byte[]> ParseEncodedContent(byte[] cont)
  {
    string rd = "rd";
    string nd = "nd";
    int length1 = cont.Length;
    int index1 = -1;
    int index2 = -1;
    for (int index3 = 4; index3 < length1; ++index3)
    {
      if (cont[index3 - 3] == (byte) 101 && cont[index3 - 2] == (byte) 120 && cont[index3 - 1] == (byte) 101 && cont[index3] == (byte) 99)
      {
        index1 = index3 + 1;
        while (cont[index1] == (byte) 10 || cont[index1] == (byte) 13)
          ++index1;
        index3 = length1;
      }
    }
    if (index1 != -1)
    {
      for (int index4 = index1; index4 < length1 - 10; ++index4)
      {
        if (cont[index4] == (byte) 99 && cont[index4 + 1] == (byte) 108 && cont[index4 + 2] == (byte) 101 && cont[index4 + 3] == (byte) 97 && cont[index4 + 4] == (byte) 114 && cont[index4 + 5] == (byte) 116 && cont[index4 + 6] == (byte) 111 && cont[index4 + 7] == (byte) 109 && cont[index4 + 8] == (byte) 97 && cont[index4 + 9] == (byte) 114 && cont[index4 + 10] == (byte) 107)
        {
          index2 = index4 - 1;
          while (cont[index2] == (byte) 10 || cont[index2] == (byte) 13)
            --index2;
          index4 = length1;
        }
      }
    }
    if (index2 == -1)
      index2 = length1;
    int num1 = 55665;
    int num2 = 4;
    for (int index5 = index1; index5 < index1 + num2 * 2; ++index5)
    {
      char ch = (char) cont[index5];
      if ((ch < '0' || ch > '9') && (ch < 'A' || ch > 'F') && (ch < 'a' || ch > 'f'))
        break;
    }
    MemoryStream memoryStream = new MemoryStream(index2 - index1);
    if (index1 != -1)
    {
      for (int index6 = index1; index6 < index2; ++index6)
      {
        int num3 = (int) cont[index6];
        int num4 = num3 ^ num1 >> 8;
        num1 = (num3 + num1) * 52845 + 22719;
        if (index6 > index1 + num2)
          memoryStream.WriteByte((byte) num4);
      }
      cont = memoryStream.ToArray();
      memoryStream.Position = 0L;
    }
    StreamReader streamReader = new StreamReader((Stream) memoryStream);
    while (true)
    {
      string str;
      do
      {
        str = streamReader.ReadLine();
        if (str == null)
          goto label_34;
      }
      while (!str.StartsWith("/lenIV"));
      this.m_skipBytes = Convert.ToInt32(str.Split(new string[1]
      {
        " "
      }, StringSplitOptions.RemoveEmptyEntries)[1]);
    }
label_34:
    streamReader.Dispose();
    int length2 = cont.Length;
    int index7 = 0;
    int start = -1;
    int num5 = -1;
    for (; index7 < length2 && index7 != length2; ++index7)
    {
      if (index7 + 11 < length2 && cont[index7] == (byte) 47 && cont[index7 + 1] == (byte) 67 && cont[index7 + 2] == (byte) 104 && cont[index7 + 3] == (byte) 97 && cont[index7 + 4] == (byte) 114 && cont[index7 + 5] == (byte) 83 && cont[index7 + 6] == (byte) 116 && cont[index7 + 7] == (byte) 114 && cont[index7 + 8] == (byte) 105 && cont[index7 + 9] == (byte) 110 && cont[index7 + 10] == (byte) 103 && cont[index7 + 11] == (byte) 115)
        num5 = index7 + 11;
      else if (index7 + 5 < length2 && cont[index7] == (byte) 47 && cont[index7 + 1] == (byte) 83 && cont[index7 + 2] == (byte) 117 && cont[index7 + 3] == (byte) 98 && cont[index7 + 4] == (byte) 114 && cont[index7 + 5] == (byte) 115)
        start = index7 + 6;
      if (start > -1 && num5 > -1)
        break;
    }
    Dictionary<string, byte[]> encodedContent = new Dictionary<string, byte[]>();
    if (num5 != -1)
    {
      encodedContent = this.ExtractFontData(this.m_skipBytes, cont, num5, rd, length2, nd);
      int count = encodedContent.Count;
    }
    if (start > -1)
      encodedContent = this.ExtractSubroutineData(this.m_skipBytes, cont, start, num5, rd, length2, nd);
    return encodedContent;
  }

  private Dictionary<string, byte[]> ExtractSubroutineData(
    int skipBytes,
    byte[] cont,
    int start,
    int charStart,
    string rd,
    int l,
    string nd)
  {
    while (cont[start] == (byte) 32 /*0x20*/ || cont[start] == (byte) 10 || cont[start] == (byte) 13)
      ++start;
    StringBuilder stringBuilder1 = new StringBuilder();
    while (true)
    {
      char ch = (char) cont[start];
      if (ch != ' ')
      {
        stringBuilder1.Append(ch);
        ++start;
      }
      else
        break;
    }
    int int32_1 = Convert.ToInt32(stringBuilder1.ToString());
    for (int index = 0; index < int32_1; ++index)
    {
      while (start < l && ((cont[start - 2] != (byte) 100 || cont[start - 1] != (byte) 117 ? 0 : (cont[start] == (byte) 112 /*0x70*/ ? 1 : 0)) | (start == charStart ? 1 : 0)) == 0)
        ++start;
      if (start == charStart)
      {
        index = int32_1;
      }
      else
      {
        while (cont[start + 1] == (byte) 32 /*0x20*/)
          ++start;
        StringBuilder stringBuilder2 = new StringBuilder("subrs");
        while (true)
        {
          ++start;
          char ch = (char) cont[start];
          if (ch != ' ')
            stringBuilder2.Append(ch);
          else
            break;
        }
        StringBuilder stringBuilder3 = new StringBuilder();
        while (true)
        {
          ++start;
          char ch = (char) cont[start];
          if (ch != ' ')
            stringBuilder3.Append(ch);
          else
            break;
        }
        int int32_2 = Convert.ToInt32(stringBuilder3.ToString());
        while (cont[start] == (byte) 32 /*0x20*/)
          ++start;
        start = start + rd.Length + 1;
        byte[] stream = this.GetStream(skipBytes, start, int32_2, cont);
        this.m_glyphs.Add(stringBuilder2.ToString(), stream);
        start = start + int32_2 + nd.Length;
      }
    }
    return this.m_glyphs;
  }

  private Dictionary<string, byte[]> ExtractFontData(
    int skipBytes,
    byte[] cont,
    int start,
    string rd,
    int l,
    string nd)
  {
    int length = cont.Length;
    int num = 0;
    while (start < length && cont[start] != (byte) 47)
      ++start;
    int index = start;
    while (start < l)
    {
      if (cont[index] == (byte) 47)
      {
        index += 2;
        while (index < length && (cont[index - 1] != (byte) 124 || cont[index] != (byte) 45 && cont[index] != (byte) 48 /*0x30*/ || cont[index + 1] != (byte) 10 && cont[index + 1] != (byte) 13) && (cont[index - 1] != (byte) 78 || cont[index] != (byte) 68))
          ++index;
      }
      if (length - index >= 3 && (cont[index - 1] == (byte) 47 || cont[index] != (byte) 101 || cont[index + 1] != (byte) 110 || cont[index + 2] != (byte) 100))
        ++index;
      else
        break;
    }
label_23:
    while (start <= index)
    {
      StringBuilder stringBuilder1 = new StringBuilder(20);
      while (true)
      {
        ++start;
        char ch = (char) cont[start];
        if (ch != ' ')
          stringBuilder1.Append(ch);
        else
          break;
      }
      ++start;
      StringBuilder stringBuilder2 = new StringBuilder();
      while (true)
      {
        char ch = (char) cont[start];
        if (ch != ' ')
        {
          stringBuilder2.Append(ch);
          ++start;
        }
        else
          break;
      }
      int int32 = Convert.ToInt32(stringBuilder2.ToString());
      while (cont[start] == (byte) 32 /*0x20*/)
        ++start;
      start = start + rd.Length + 1;
      byte[] stream = this.GetStream(skipBytes, start, int32, cont);
      this.m_glyphs.Add(stringBuilder1.ToString(), stream);
      ++num;
      start = start + int32 + nd.Length;
      while (true)
      {
        if (start <= index && cont[start] != (byte) 47)
          ++start;
        else
          goto label_23;
      }
    }
    return this.m_glyphs;
  }

  private byte[] GetStream(int skipBytes, int start, int byteCount, byte[] cont)
  {
    MemoryStream memoryStream = new MemoryStream();
    int num1 = 4330;
    for (int index = 0; index < byteCount; ++index)
    {
      int num2 = (int) cont[start + index];
      int num3 = num2 ^ num1 >> 8;
      num1 = (num2 + num1) * 52845 + 22719;
      if (index >= skipBytes)
        memoryStream.WriteByte((byte) num3);
    }
    return memoryStream.ToArray();
  }
}
