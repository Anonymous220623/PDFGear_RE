// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfIndexedColorSpace
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfIndexedColorSpace : PdfColorSpaces
{
  private PdfColorSpaces m_basecolorspace = (PdfColorSpaces) new PdfDeviceColorSpace(PdfColorSpace.RGB);
  private int m_maxColorIndex;
  private byte[] m_indexedColorTable;
  private PdfStream m_stream = new PdfStream();

  public PdfIndexedColorSpace()
  {
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
    this.Initialize();
  }

  public PdfColorSpaces BaseColorSpace
  {
    get => this.m_basecolorspace;
    set
    {
      this.m_basecolorspace = value;
      this.Initialize();
    }
  }

  public int MaxColorIndex
  {
    get => this.m_maxColorIndex;
    set
    {
      this.m_maxColorIndex = value;
      this.Initialize();
    }
  }

  public byte[] IndexedColorTable
  {
    get => this.m_indexedColorTable;
    set
    {
      this.m_indexedColorTable = value;
      this.Initialize();
    }
  }

  public byte[] GetProfileData()
  {
    byte[] numArray = new byte[1000];
    return this.m_indexedColorTable;
  }

  protected void Save()
  {
    byte[] buffer = this.m_indexedColorTable != null ? this.m_indexedColorTable : this.GetProfileData();
    this.m_stream.Clear();
    this.m_stream.InternalStream.Write(buffer, 0, buffer.Length);
  }

  private void Initialize()
  {
    lock (PdfColorSpaces.s_syncObject)
    {
      IPdfCache pdfCache = PdfDocument.Cache.Search((IPdfCache) this);
      ((IPdfCache) this).SetInternals(pdfCache != null ? pdfCache.GetInternals() : (IPdfPrimitive) this.CreateInternals());
    }
  }

  private PdfArray CreateInternals()
  {
    PdfArray internals = new PdfArray();
    if (internals != null)
    {
      PdfName element1 = new PdfName("Indexed");
      internals.Add((IPdfPrimitive) element1);
      PdfReferenceHolder element2 = new PdfReferenceHolder((IPdfPrimitive) this.m_stream);
      if (this.m_basecolorspace != null)
      {
        if (this.m_basecolorspace is PdfCalGrayColorSpace)
        {
          PdfReferenceHolder element3 = new PdfReferenceHolder((IPdfWrapper) this.m_basecolorspace);
          internals.Add((IPdfPrimitive) element3);
        }
        else if (this.m_basecolorspace is PdfCalRGBColorSpace)
        {
          PdfReferenceHolder element4 = new PdfReferenceHolder((IPdfWrapper) this.m_basecolorspace);
          internals.Add((IPdfPrimitive) element4);
        }
        else if (this.m_basecolorspace is PdfLabColorSpace)
        {
          PdfReferenceHolder element5 = new PdfReferenceHolder((IPdfWrapper) this.m_basecolorspace);
          internals.Add((IPdfPrimitive) element5);
        }
        else if (this.m_basecolorspace is PdfDeviceColorSpace)
        {
          switch ((this.m_basecolorspace as PdfDeviceColorSpace).DeviceColorSpaceType.ToString())
          {
            case "RGB":
              PdfName element6 = new PdfName("DeviceRGB");
              internals.Add((IPdfPrimitive) element6);
              break;
            case "CMYK":
              PdfName element7 = new PdfName("DeviceCMYK");
              internals.Add((IPdfPrimitive) element7);
              break;
            case "GrayScale":
              PdfName element8 = new PdfName("DeviceGray");
              internals.Add((IPdfPrimitive) element8);
              break;
          }
        }
        internals.Add((IPdfPrimitive) new PdfNumber(this.m_maxColorIndex));
      }
      internals.Add((IPdfPrimitive) element2);
    }
    return internals;
  }

  private void Stream_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();
}
