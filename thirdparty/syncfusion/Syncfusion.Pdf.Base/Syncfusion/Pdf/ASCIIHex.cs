// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ASCIIHex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class ASCIIHex
{
  public byte[] Decode(byte[] data)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    MemoryStream memoryStream1 = new MemoryStream(data);
    StreamReader streamReader = new StreamReader((Stream) memoryStream1);
    if (streamReader != null)
    {
      while (true)
      {
        string str = streamReader.ReadLine();
        if (str != null)
          stringBuilder2.Append(str);
        else
          break;
      }
    }
    if (streamReader != null)
    {
      streamReader.Close();
      memoryStream1.Close();
    }
    int length = stringBuilder2.Length;
    int index = 0;
    int num = 0;
    MemoryStream memoryStream2 = new MemoryStream();
    do
    {
      char ch = stringBuilder2[index];
      if (ch >= '0' && ch <= '9' || ch >= 'a' && ch <= 'f' || ch >= 'A' && ch <= 'F')
      {
        stringBuilder1.Append(ch);
        if (num == 1)
        {
          int int32 = Convert.ToInt32(stringBuilder1.ToString(), 16 /*0x10*/);
          byte[] buffer = new byte[4]
          {
            (byte) int32,
            (byte) (int32 >> 8),
            (byte) (int32 >> 16 /*0x10*/),
            (byte) (int32 >> 24)
          };
          memoryStream2.Write(buffer, 0, 4);
          num = 0;
          stringBuilder1 = new StringBuilder();
        }
        else
          ++num;
      }
      if (ch != '>')
        ++index;
      else
        break;
    }
    while (index != length);
    if (num == 1)
    {
      stringBuilder1.Append('0');
      int int32 = Convert.ToInt32(stringBuilder1.ToString(), 16 /*0x10*/);
      byte[] buffer = new byte[4]
      {
        (byte) int32,
        (byte) (int32 >> 8),
        (byte) (int32 >> 16 /*0x10*/),
        (byte) (int32 >> 24)
      };
      memoryStream2.Write(buffer, 0, 4);
    }
    memoryStream2.Close();
    return memoryStream2.GetBuffer();
  }
}
