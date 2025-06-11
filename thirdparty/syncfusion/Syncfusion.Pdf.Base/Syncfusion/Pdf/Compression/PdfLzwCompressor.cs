// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.PdfLzwCompressor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class PdfLzwCompressor : IPdfCompressor
{
  private const int c_eod = 257;
  private const int c_clearTable = 256 /*0x0100*/;
  private const int c_startCode = 258;
  private const int c_10BitsCode = 511 /*0x01FF*/;
  private const int c_11BitsCode = 1023 /*0x03FF*/;
  private const int c_12BitsCode = 2047 /*0x07FF*/;
  private byte[][] m_codeTable;
  private byte[] m_inputData;
  private Stream m_outputData;
  private int m_tableIndex;
  private int m_bitsToGet;
  private int m_byteRead;
  private int m_nextData;
  private int m_nextBits;
  private int[] m_sizeTable;
  private bool m_isEarlyChanged = true;

  public PdfLzwCompressor()
  {
    this.m_sizeTable = new int[4]
    {
      511 /*0x01FF*/,
      1023 /*0x03FF*/,
      2047 /*0x07FF*/,
      4095 /*0x0FFF*/
    };
  }

  public CompressionType Type => CompressionType.LZW;

  public void Decompress(byte[] inputData, Stream outputData)
  {
    if (outputData == null)
      throw new ArgumentNullException(nameof (outputData));
    if (inputData == null)
      throw new ArgumentNullException(nameof (inputData));
    this.InitializeDataTable();
    this.m_inputData = inputData;
    this.m_outputData = outputData;
    this.m_byteRead = 0;
    this.m_nextData = 0;
    this.m_nextBits = 0;
    this.m_bitsToGet = 9;
    int index1 = 0;
    int index2;
    while ((index2 = this.NewCode()) != 257)
    {
      switch (index2)
      {
        case -1:
          return;
        case 256 /*0x0100*/:
          this.InitializeDataTable();
          int index3 = this.NewCode();
          switch (index3)
          {
            case -1:
              return;
            case 257:
              return;
            default:
              if (this.m_codeTable[index3] != null)
                this.WriteCode(this.m_codeTable[index3]);
              index1 = index3;
              continue;
          }
        default:
          if (index2 < this.m_tableIndex)
          {
            byte[] code = this.m_codeTable[index2];
            if (code != null && this.m_codeTable[index1] != null)
            {
              this.WriteCode(code);
              this.AddCodeToTable(this.m_codeTable[index1], code[0]);
              index1 = index2;
              continue;
            }
            continue;
          }
          byte[] oldData = this.m_codeTable[index1];
          if (oldData != null)
          {
            byte[] numArray = this.UniteBytes(oldData, oldData[0]);
            this.WriteCode(numArray);
            this.AddCodeToTable(numArray);
            index1 = index2;
            continue;
          }
          continue;
      }
    }
  }

  public string Name => (string) null;

  public byte[] Compress(byte[] data) => (byte[]) null;

  public byte[] Compress(string data) => (byte[]) null;

  public Stream Compress(Stream inputStream) => (Stream) null;

  public byte[] Decompress(string value)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(value);
    MemoryStream outputData = new MemoryStream();
    this.Decompress(bytes, (Stream) outputData);
    byte[] buffer = new byte[outputData.Length];
    outputData.Position = 0L;
    outputData.Read(buffer, 0, (int) outputData.Length - 1);
    return buffer;
  }

  public byte[] Decompress(byte[] value, bool isearlychange)
  {
    this.m_isEarlyChanged = isearlychange;
    return this.Decompress(value);
  }

  public byte[] Decompress(byte[] value)
  {
    MemoryStream outputData = new MemoryStream();
    this.Decompress(value, (Stream) outputData);
    byte[] buffer = new byte[outputData.Length];
    outputData.Position = 0L;
    if (outputData.Length > 0L)
      outputData.Read(buffer, 0, (int) outputData.Length);
    return buffer;
  }

  public Stream Decompress(Stream inputStream)
  {
    MemoryStream outputData = new MemoryStream();
    byte[] numArray = new byte[inputStream.Length];
    inputStream.Position = 0L;
    inputStream.Read(numArray, 0, (int) inputStream.Length - 1);
    this.Decompress(numArray, (Stream) outputData);
    return (Stream) outputData;
  }

  private void InitializeDataTable()
  {
    this.m_codeTable = new byte[8192 /*0x2000*/][];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
    {
      this.m_codeTable[index] = new byte[1];
      this.m_codeTable[index][0] = (byte) index;
    }
    this.m_tableIndex = 258;
    this.m_bitsToGet = 9;
  }

  private void WriteCode(byte[] code) => this.m_outputData.Write(code, 0, code.Length);

  private void AddCodeToTable(byte[] oldBytes, byte newByte)
  {
    int length = oldBytes.Length;
    byte[] numArray = new byte[length + 1];
    Array.Copy((Array) oldBytes, 0, (Array) numArray, 0, length);
    numArray[length] = newByte;
    this.AddCodeToTable(numArray);
  }

  private void AddCodeToTable(byte[] data)
  {
    if (this.m_isEarlyChanged)
      this.m_codeTable[this.m_tableIndex++] = data;
    if (this.m_tableIndex == 511 /*0x01FF*/)
      this.m_bitsToGet = 10;
    else if (this.m_tableIndex == 1023 /*0x03FF*/)
      this.m_bitsToGet = 11;
    else if (this.m_tableIndex == 2047 /*0x07FF*/)
      this.m_bitsToGet = 12;
    if (this.m_isEarlyChanged || this.m_tableIndex > this.m_codeTable.Length)
      return;
    this.m_codeTable[this.m_tableIndex++] = data;
  }

  private byte[] UniteBytes(byte[] oldData, byte newData)
  {
    int length = oldData.Length;
    byte[] destinationArray = new byte[length + 1];
    Array.Copy((Array) oldData, 0, (Array) destinationArray, 0, length);
    destinationArray[length] = newData;
    return destinationArray;
  }

  private int NewCode()
  {
    if (this.m_byteRead >= this.m_inputData.Length)
      return -1;
    this.m_nextData = this.m_nextData << 8 | (int) this.m_inputData[this.m_byteRead++];
    this.m_nextBits += 8;
    if (this.m_nextBits < this.m_bitsToGet && this.m_byteRead < this.m_inputData.Length)
    {
      this.m_nextData = this.m_nextData << 8 | (int) this.m_inputData[this.m_byteRead++];
      this.m_nextBits += 8;
    }
    int num = this.m_nextData >> this.m_nextBits - this.m_bitsToGet & this.m_sizeTable[this.m_bitsToGet - 9];
    this.m_nextBits -= this.m_bitsToGet;
    return num;
  }
}
