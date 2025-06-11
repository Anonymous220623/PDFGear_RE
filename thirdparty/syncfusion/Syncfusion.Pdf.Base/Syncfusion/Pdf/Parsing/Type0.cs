// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Type0
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.IO;
using System.IO.Compression;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Type0 : Function
{
  private int m_bitsPerSample;
  private int m_order;
  private PdfArray m_size;
  private PdfArray m_encode;
  private PdfArray m_decode;
  private int[][] m_sampleValue;
  private int outputValuesCount;
  private string m_filter;

  internal int BitsPerSample
  {
    get => this.m_bitsPerSample;
    set => this.m_bitsPerSample = value;
  }

  internal string Filter
  {
    get => this.m_filter;
    set => this.m_filter = value;
  }

  internal int Order
  {
    get => this.m_order;
    set => this.m_order = value;
  }

  internal PdfArray Decode
  {
    get => this.m_decode == null ? this.Range : this.m_decode;
    set => this.m_decode = value;
  }

  internal PdfArray Encode
  {
    get
    {
      if (this.m_encode == null)
      {
        PdfArray pdfArray = new PdfArray();
        for (int index = 0; index < this.Size.Count; ++index)
        {
          int intValue = (this.Size[index] as PdfNumber).IntValue;
          pdfArray.Add((IPdfPrimitive) new PdfNumber(0));
          pdfArray.Add((IPdfPrimitive) new PdfNumber(intValue - 1));
        }
        this.Encode = pdfArray;
      }
      return this.m_encode;
    }
    set => this.m_encode = value;
  }

  internal PdfArray Size
  {
    get => this.m_size;
    set => this.m_size = value;
  }

  public Type0(PdfDictionary dictionary)
    : base(dictionary)
  {
    this.m_bitsPerSample = dictionary.Items.ContainsKey(new PdfName("BitPerSample")) ? (dictionary[new PdfName("BitPerSample")] as PdfNumber).IntValue : 8;
    this.m_decode = dictionary.Items.ContainsKey(new PdfName(nameof (Decode))) ? dictionary[new PdfName(nameof (Decode))] as PdfArray : (PdfArray) null;
    this.m_encode = dictionary.Items.ContainsKey(new PdfName(nameof (Encode))) ? dictionary[new PdfName(nameof (Encode))] as PdfArray : (PdfArray) null;
    this.m_size = dictionary.Items.ContainsKey(new PdfName(nameof (Size))) ? dictionary[new PdfName(nameof (Size))] as PdfArray : (PdfArray) null;
    this.m_filter = this.GetFilter(dictionary);
    this.Load(dictionary as PdfStream);
  }

  internal void Load(PdfStream stream)
  {
    DataReader dataReader = new DataReader(this.GetDecodedStream(stream), this.BitsPerSample);
    int length = 1;
    for (int index = 0; index < this.Size.Count; ++index)
    {
      int intValue = (this.Size[index] as PdfNumber).IntValue;
      length *= intValue;
    }
    this.outputValuesCount = this.Range.Count / 2;
    this.m_sampleValue = new int[length][];
    for (int index1 = 0; index1 < length; ++index1)
    {
      this.m_sampleValue[index1] = new int[this.outputValuesCount];
      for (int index2 = 0; index2 < this.outputValuesCount; ++index2)
        this.m_sampleValue[index1][index2] = dataReader.Read();
    }
  }

  protected override double[] PerformFunction(double[] inputData)
  {
    int[] encodedData = new int[inputData.Length];
    for (int index = 0; index < inputData.Length; ++index)
    {
      double floatValue1 = (double) (this.Domain[2 * index] as PdfNumber).FloatValue;
      double floatValue2 = (double) (this.Domain[2 * index + 1] as PdfNumber).FloatValue;
      double floatValue3 = (double) (this.Encode[2 * index] as PdfNumber).FloatValue;
      double floatValue4 = (double) (this.Encode[2 * index + 1] as PdfNumber).FloatValue;
      int intValue = (this.Size[index] as PdfNumber).IntValue;
      encodedData[index] = Type0.EncodeInputData(inputData[index], floatValue1, floatValue2, floatValue3, floatValue4, intValue);
    }
    int[] numArray1 = this.m_sampleValue[this.GetIndex(encodedData)];
    double[] numArray2 = new double[numArray1.Length];
    int maxN = (1 << this.BitsPerSample) - 1;
    for (int index = 0; index < numArray1.Length; ++index)
    {
      double floatValue5 = (double) (this.Decode[2 * index] as PdfNumber).FloatValue;
      double floatValue6 = (double) (this.Decode[2 * index + 1] as PdfNumber).FloatValue;
      numArray2[index] = Type0.DecodeOutputData((double) numArray1[index], (double) maxN, floatValue5, floatValue6);
    }
    return numArray2;
  }

  private static int EncodeInputData(
    double x,
    double minD,
    double maxD,
    double minE,
    double maxE,
    int size)
  {
    return (int) Math.Round(Function.ExtractData(Function.FindIntermediateData(x, minD, maxD, minE, maxE), 0.0, (double) (size - 1)));
  }

  private static double DecodeOutputData(double r, double maxN, double minD, double maxD)
  {
    return Function.FindIntermediateData(r, 0.0, maxN, minD, maxD);
  }

  private int GetIndex(int[] encodedData)
  {
    int index1 = encodedData[0];
    int num = 1;
    for (int index2 = 1; index2 < encodedData.Length; ++index2)
    {
      int intValue = (this.Size[index2 - 1] as PdfNumber).IntValue;
      num *= intValue;
      index1 += num * encodedData[index2];
    }
    return index1;
  }

  private byte[] GetDecodedStream(PdfStream stream)
  {
    return this.m_filter == "FlateDecode" ? this.DecodeFlateStream(stream.InternalStream).ToArray() : stream.Data;
  }

  private MemoryStream DecodeFlateStream(MemoryStream encodedStream)
  {
    encodedStream.Position = 0L;
    encodedStream.ReadByte();
    encodedStream.ReadByte();
    DeflateStream deflateStream = new DeflateStream((Stream) encodedStream, CompressionMode.Decompress, true);
    byte[] buffer = new byte[4096 /*0x1000*/];
    MemoryStream memoryStream = new MemoryStream();
    while (true)
    {
      int count = deflateStream.Read(buffer, 0, 4096 /*0x1000*/);
      if (count > 0)
        memoryStream.Write(buffer, 0, count);
      else
        break;
    }
    return memoryStream;
  }

  private string GetFilter(PdfDictionary dictionary)
  {
    if (dictionary.Items.ContainsKey(new PdfName("Filter")))
    {
      PdfName pdfName = dictionary.Items[new PdfName("Filter")] as PdfName;
      if (pdfName != (PdfName) null)
        return pdfName.Value;
      if (dictionary.Items[new PdfName("Filter")] is PdfArray pdfArray && pdfArray.Count == 1)
        return (pdfArray[0] as PdfName).Value;
    }
    return string.Empty;
  }
}
