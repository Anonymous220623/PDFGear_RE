// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.LinkInfoStream
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;

internal class LinkInfoStream
{
  private const int DEF_STRUCT_SIZE = 0;
  private const int DEF_UNICODE_MARKER = -858997829;
  private const int DEF_UNICODE_MARKER_SIZE = 4;
  private byte[] m_filePathDataASCII;
  private byte[] m_filePathDataUNICOD;
  private string m_filePath;

  internal int Length
  {
    get
    {
      return this.m_filePathDataASCII != null && this.m_filePathDataUNICOD != null ? this.m_filePathDataASCII.Length + this.m_filePathDataUNICOD.Length + 4 + 8 : 0;
    }
  }

  internal LinkInfoStream(Stream stream) => this.Parse((stream as MemoryStream).ToArray(), 0);

  internal LinkInfoStream(string filePath)
  {
    this.m_filePath = filePath;
    Encoding encoding = (Encoding) new ASCIIEncoding();
    UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
    this.m_filePathDataASCII = encoding.GetBytes(this.m_filePath);
    this.m_filePathDataUNICOD = unicodeEncoding.GetBytes(this.m_filePath);
  }

  internal void Parse(byte[] arrData, int iOffset)
  {
    int iOffset1 = 0;
    int num = 0;
    for (int length = arrData.Length; num < length; ++num)
    {
      if (iOffset1 > 0)
        iOffset1 -= 3;
      if (ByteConverter.ReadInt32(arrData, ref iOffset1) == -858997829)
        break;
    }
    byte[] numArray = new byte[iOffset1 - 4];
    int iOffset2 = 0;
    this.m_filePathDataASCII = ByteConverter.ReadBytes(arrData, iOffset1 - 4, ref iOffset2);
    int iOffset3 = iOffset2 + 4;
    int length1 = arrData.Length - numArray.Length - 4;
    this.m_filePathDataUNICOD = ByteConverter.ReadBytes(arrData, length1, ref iOffset3);
  }

  internal int Save(byte[] arrData, int iOffset)
  {
    throw new NotImplementedException("Not implemented");
  }

  internal void SaveTo(Stream stream)
  {
    byte[] numArray = new byte[this.Length];
    int index = 0;
    numArray[index] = (byte) this.m_filePathDataASCII.Length;
    int iOffset1 = index + 2;
    ByteConverter.WriteBytes(numArray, ref iOffset1, this.m_filePathDataASCII);
    int iOffset2 = iOffset1 + 2;
    ByteConverter.WriteBytes(numArray, ref iOffset2, BitConverter.GetBytes(-858997829));
    numArray[iOffset2] = (byte) this.m_filePathDataASCII.Length;
    int iOffset3 = iOffset2 + 2;
    ByteConverter.WriteBytes(numArray, ref iOffset3, this.m_filePathDataUNICOD);
    stream.Write(numArray, 0, numArray.Length);
  }
}
