// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Indexed
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Indexed : Colorspace
{
  private Colorspace m_baseColorspace;
  private int m_hiVal;
  private LookupParameter m_lookup;
  private string m_filter;

  internal override int Components => 1;

  internal override Brush DefaultBrush => this.BaseColorspace.DefaultBrush;

  internal Colorspace BaseColorspace
  {
    get => this.m_baseColorspace;
    set => this.m_baseColorspace = value;
  }

  internal int HiVal
  {
    get => this.m_hiVal;
    set => this.m_hiVal = value;
  }

  internal LookupParameter Lookup
  {
    get => this.m_lookup;
    set => this.m_lookup = value;
  }

  private Color GetColor(int index)
  {
    if (this.BaseColorspace == null || this.Lookup == null)
      return Color.Transparent;
    if (index < 0)
      index = 0;
    if (index > this.HiVal)
      index = this.HiVal;
    Colorspace colorSpace = this.GetColorSpace();
    if (index * colorSpace.Components == this.Lookup.Data.Length)
      index = 0;
    return colorSpace.GetColor(this.Lookup.Data, index * colorSpace.Components);
  }

  internal void SetValue(PdfArray array)
  {
    this.m_baseColorspace = this.GetBaseColorspace(array);
    this.m_hiVal = (array[2] as PdfNumber).IntValue;
    this.m_lookup = new LookupParameter(this.GetLookupData(array));
  }

  internal override Color GetColor(string[] pars)
  {
    float result;
    float.TryParse(pars[0], out result);
    return this.GetColor((int) result);
  }

  internal override Color GetColor(byte[] bytes, int offset)
  {
    throw new NotSupportedException("This method is not supported.");
  }

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    return new Pen(this.GetColor(pars)).Brush;
  }

  private Colorspace GetBaseColorspace(PdfArray array)
  {
    if ((object) (array[1] as PdfName) != null)
      return Colorspace.CreateColorSpace((array[1] as PdfName).Value);
    if (array[1] is PdfArray)
    {
      PdfArray array1 = array[1] as PdfArray;
      if ((object) (array1[0] as PdfName) != null)
        return Colorspace.CreateColorSpace((array[0] as PdfName).Value, (IPdfPrimitive) array1);
    }
    return Colorspace.CreateColorSpace("DeviceRGB");
  }

  private byte[] GetLookupData(PdfArray array)
  {
    int count = array.Count;
    PdfReferenceHolder pdfReferenceHolder = array[3] as PdfReferenceHolder;
    return pdfReferenceHolder == (PdfReferenceHolder) null ? (array.Elements[3] as PdfString).Bytes : this.Load((PdfDictionary) (pdfReferenceHolder.Object as PdfStream));
  }

  private Colorspace GetColorSpace()
  {
    Colorspace colorSpace = this.BaseColorspace;
    if (colorSpace is ICCBased iccBased)
      colorSpace = iccBased.Profile.AlternateColorspace;
    return colorSpace;
  }

  private byte[] Load(PdfDictionary dictionary)
  {
    this.m_filter = dictionary.Items.ContainsKey(new PdfName("Filter")) ? (dictionary[new PdfName("Filter")] as PdfName).Value : "";
    return this.GetDecodedStream(dictionary as PdfStream);
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
}
