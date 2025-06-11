// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ColorSpace.PdfSeparationColorSpace
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Functions;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.ColorSpace;

public class PdfSeparationColorSpace : PdfColorSpaces, IPdfWrapper
{
  private string m_colorant;
  private PdfFunction m_function;
  private PdfStream m_stream = new PdfStream();
  private PdfColorSpaces m_alterantecolorspaces = (PdfColorSpaces) new PdfDeviceColorSpace(PdfColorSpace.CMYK);

  public PdfSeparationColorSpace()
  {
    this.m_stream.Compress = true;
    this.m_stream.SetProperty("Filter", (IPdfPrimitive) new PdfName("FlateDecode"));
    this.m_stream.BeginSave += new SavePdfPrimitiveEventHandler(this.Stream_BeginSave);
    this.Initialize();
  }

  public PdfColorSpaces AlternateColorSpaces
  {
    get => this.m_alterantecolorspaces;
    set
    {
      this.m_alterantecolorspaces = value;
      this.Initialize();
    }
  }

  public string Colorant
  {
    get => this.m_colorant;
    set
    {
      this.m_colorant = value;
      this.Initialize();
    }
  }

  public PdfFunction TintTransform
  {
    get => this.m_function;
    set
    {
      this.m_function = value;
      this.Initialize();
    }
  }

  public byte[] GetProfileData() => new byte[0];

  protected void Save()
  {
    byte[] profileData = this.GetProfileData();
    this.m_stream.Clear();
    this.m_stream.InternalStream.Write(profileData, 0, profileData.Length);
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
      PdfName element1 = new PdfName("Separation");
      internals.Add((IPdfPrimitive) element1);
      if (this.m_colorant != null)
      {
        PdfName element2 = new PdfName(this.m_colorant);
        internals.Add((IPdfPrimitive) element2);
      }
      else
      {
        PdfName element3 = new PdfName("All");
        internals.Add((IPdfPrimitive) element3);
      }
      if (this.m_alterantecolorspaces != null)
      {
        if (this.m_alterantecolorspaces is PdfCalGrayColorSpace)
        {
          PdfName pdfName = new PdfName("CalGray");
          PdfReferenceHolder element4 = new PdfReferenceHolder((IPdfWrapper) this.m_alterantecolorspaces);
          internals.Add((IPdfPrimitive) element4);
        }
        else if (this.m_alterantecolorspaces is PdfCalRGBColorSpace)
        {
          PdfName pdfName = new PdfName("CalRGB");
          PdfReferenceHolder element5 = new PdfReferenceHolder((IPdfWrapper) this.m_alterantecolorspaces);
          internals.Add((IPdfPrimitive) element5);
        }
        else if (this.m_alterantecolorspaces is PdfLabColorSpace)
        {
          PdfName pdfName = new PdfName("Lab");
          PdfReferenceHolder element6 = new PdfReferenceHolder((IPdfWrapper) this.m_alterantecolorspaces);
          internals.Add((IPdfPrimitive) element6);
        }
        else if (this.m_alterantecolorspaces is PdfDeviceColorSpace)
        {
          switch ((this.m_alterantecolorspaces as PdfDeviceColorSpace).DeviceColorSpaceType.ToString())
          {
            case "RGB":
              PdfName element7 = new PdfName("DeviceRGB");
              internals.Add((IPdfPrimitive) element7);
              break;
            case "CMYK":
              PdfName element8 = new PdfName("DeviceCMYK");
              internals.Add((IPdfPrimitive) element8);
              break;
            case "GrayScale":
              PdfName element9 = new PdfName("DeviceGray");
              internals.Add((IPdfPrimitive) element9);
              break;
          }
        }
      }
      else
      {
        PdfName element10 = new PdfName("DeviceCMYK");
        internals.Add((IPdfPrimitive) element10);
      }
      if (this.m_function != null)
      {
        if (this.m_alterantecolorspaces is PdfCalGrayColorSpace)
        {
          PdfExponentialInterpolationFunction function = this.m_function as PdfExponentialInterpolationFunction;
          function.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(2));
          function.Dictionary.SetProperty("Domain", (IPdfPrimitive) new PdfArray(new double[2]
          {
            0.0,
            1.0
          }));
          function.Dictionary.SetProperty("Range", (IPdfPrimitive) new PdfArray(new double[2]
          {
            0.0,
            1.0
          }));
          function.Dictionary.SetProperty("C0", (IPdfPrimitive) new PdfArray(new double[1]));
          if (function.C1.Length != 1)
            throw new ArgumentOutOfRangeException();
          function.Dictionary.SetProperty("C1", (IPdfPrimitive) new PdfArray(function.C1));
          function.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfNumber(1));
          PdfReferenceHolder element11 = new PdfReferenceHolder((IPdfWrapper) function);
          internals.Add((IPdfPrimitive) element11);
        }
        else if (this.m_alterantecolorspaces is PdfCalRGBColorSpace)
        {
          PdfExponentialInterpolationFunction function = this.m_function as PdfExponentialInterpolationFunction;
          function.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(2));
          function.Dictionary.SetProperty("Domain", (IPdfPrimitive) new PdfArray(new double[2]
          {
            0.0,
            1.0
          }));
          function.Dictionary.SetProperty("Range", (IPdfPrimitive) new PdfArray(new double[6]
          {
            0.0,
            1.0,
            0.0,
            1.0,
            0.0,
            1.0
          }));
          function.Dictionary.SetProperty("C0", (IPdfPrimitive) new PdfArray(new double[3]));
          if (function.C1.Length != 3)
            throw new ArgumentOutOfRangeException();
          function.Dictionary.SetProperty("C1", (IPdfPrimitive) new PdfArray(function.C1));
          function.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfNumber(1));
          PdfReferenceHolder element12 = new PdfReferenceHolder((IPdfWrapper) function);
          internals.Add((IPdfPrimitive) element12);
        }
        else if (this.m_alterantecolorspaces is PdfLabColorSpace)
        {
          PdfExponentialInterpolationFunction function = this.m_function as PdfExponentialInterpolationFunction;
          function.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(2));
          function.Dictionary.SetProperty("Domain", (IPdfPrimitive) new PdfArray(new double[2]
          {
            0.0,
            1.0
          }));
          function.Dictionary.SetProperty("Range", (IPdfPrimitive) new PdfArray(new double[6]
          {
            0.0,
            100.0,
            0.0,
            100.0,
            0.0,
            100.0
          }));
          function.Dictionary.SetProperty("C0", (IPdfPrimitive) new PdfArray(new double[3]));
          if (function.C1.Length != 3)
            throw new ArgumentOutOfRangeException();
          function.Dictionary.SetProperty("C1", (IPdfPrimitive) new PdfArray(function.C1));
          function.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfNumber(1));
          PdfReferenceHolder element13 = new PdfReferenceHolder((IPdfWrapper) function);
          internals.Add((IPdfPrimitive) element13);
        }
        else if (this.m_alterantecolorspaces is PdfDeviceColorSpace)
        {
          switch ((this.m_alterantecolorspaces as PdfDeviceColorSpace).DeviceColorSpaceType.ToString())
          {
            case "RGB":
              PdfExponentialInterpolationFunction function1 = this.m_function as PdfExponentialInterpolationFunction;
              function1.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(2));
              function1.Dictionary.SetProperty("Domain", (IPdfPrimitive) new PdfArray(new double[2]
              {
                0.0,
                1.0
              }));
              function1.Dictionary.SetProperty("Range", (IPdfPrimitive) new PdfArray(new double[6]
              {
                0.0,
                1.0,
                0.0,
                1.0,
                0.0,
                1.0
              }));
              function1.Dictionary.SetProperty("C0", (IPdfPrimitive) new PdfArray(new double[3]));
              if (function1.C1.Length != 3)
                throw new ArgumentOutOfRangeException();
              function1.Dictionary.SetProperty("C1", (IPdfPrimitive) new PdfArray(function1.C1));
              function1.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfNumber(1));
              PdfReferenceHolder element14 = new PdfReferenceHolder((IPdfWrapper) function1);
              internals.Add((IPdfPrimitive) element14);
              break;
            case "CMYK":
              PdfExponentialInterpolationFunction function2 = this.m_function as PdfExponentialInterpolationFunction;
              function2.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(2));
              function2.Dictionary.SetProperty("C0", (IPdfPrimitive) new PdfArray(function2.C0));
              if (function2.C1.Length != 4)
                throw new ArgumentOutOfRangeException();
              function2.Dictionary.SetProperty("C1", (IPdfPrimitive) new PdfArray(function2.C1));
              function2.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfNumber(1));
              PdfReferenceHolder element15 = new PdfReferenceHolder((IPdfWrapper) function2);
              internals.Add((IPdfPrimitive) element15);
              break;
            case "GrayScale":
              PdfExponentialInterpolationFunction function3 = this.m_function as PdfExponentialInterpolationFunction;
              function3.Dictionary.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(2));
              function3.Dictionary.SetProperty("Domain", (IPdfPrimitive) new PdfArray(new double[2]
              {
                0.0,
                1.0
              }));
              function3.Dictionary.SetProperty("Range", (IPdfPrimitive) new PdfArray(new double[2]
              {
                0.0,
                1.0
              }));
              function3.Dictionary.SetProperty("C0", (IPdfPrimitive) new PdfArray(new double[1]));
              if (function3.C1.Length != 1)
                throw new ArgumentOutOfRangeException();
              function3.Dictionary.SetProperty("C1", (IPdfPrimitive) new PdfArray(function3.C1));
              function3.Dictionary.SetProperty("N", (IPdfPrimitive) new PdfNumber(1));
              PdfReferenceHolder element16 = new PdfReferenceHolder((IPdfWrapper) function3);
              internals.Add((IPdfPrimitive) element16);
              break;
          }
        }
      }
      else
      {
        float[] array1 = new float[2]{ 0.0f, 1f };
        float[] array2 = new float[8]
        {
          0.0f,
          1f,
          0.0f,
          1f,
          0.0f,
          1f,
          0.0f,
          1f
        };
        this.m_stream.SetProperty("FunctionType", (IPdfPrimitive) new PdfNumber(4));
        this.m_stream.SetProperty("Domain", (IPdfPrimitive) new PdfArray(array1));
        this.m_stream.SetProperty("Range", (IPdfPrimitive) new PdfArray(array2));
      }
    }
    return internals;
  }

  private void Stream_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();
}
