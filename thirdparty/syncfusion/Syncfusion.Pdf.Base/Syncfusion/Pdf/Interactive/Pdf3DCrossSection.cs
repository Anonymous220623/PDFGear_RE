// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DCrossSection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DCrossSection : IPdfWrapper
{
  private float[] m_center;
  private PdfColor m_color;
  private PdfColor m_intersectionColor;
  private bool m_intersectionIsVisible;
  private object[] m_orientation;
  private float m_opacity;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public Pdf3DCrossSection() => this.Initialize();

  public float[] Center
  {
    get => this.m_center;
    set
    {
      this.m_center = value;
      if (this.m_center == null || this.m_center.Length < 3)
        throw new ArgumentOutOfRangeException("Center.Length", "Center Array must have atleast 3 elements.");
    }
  }

  public PdfColor Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public PdfColor IntersectionColor
  {
    get => this.m_intersectionColor;
    set => this.m_intersectionColor = value;
  }

  public bool IntersectionIsVisible
  {
    get => this.m_intersectionIsVisible;
    set => this.m_intersectionIsVisible = value;
  }

  public float Opacity
  {
    get => this.m_opacity;
    set => this.m_opacity = value;
  }

  public object[] Orientation
  {
    get => this.m_orientation;
    set
    {
      this.m_orientation = value;
      if (this.m_orientation == null || this.m_orientation.Length < 3)
        throw new ArgumentOutOfRangeException("Orientation.Length", "Orientation Array must have atleast 3 elements.");
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("3DCrossSection"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    if (this.m_center != null)
      this.Dictionary.SetProperty("C", (IPdfPrimitive) new PdfArray(this.m_center));
    if (this.m_orientation != null)
    {
      PdfArray array = new PdfArray();
      if (this.m_orientation[0] == null)
        array.Insert(0, (IPdfPrimitive) new PdfName("null"));
      else
        array.Insert(0, (IPdfPrimitive) new PdfName((string) this.m_orientation[0]));
      if (this.m_orientation[1] == null)
        array.Insert(1, (IPdfPrimitive) new PdfName("null"));
      else
        array.Insert(1, (IPdfPrimitive) new PdfName((string) this.m_orientation[1]));
      if (this.m_orientation[2] == null)
        array.Insert(2, (IPdfPrimitive) new PdfName("null"));
      else
        array.Insert(2, (IPdfPrimitive) new PdfName((string) this.m_orientation[2]));
      this.Dictionary["O"] = (IPdfPrimitive) new PdfArray(array);
    }
    this.Dictionary.SetProperty("PO", (IPdfPrimitive) new PdfNumber(this.m_opacity));
    float num1 = (float) this.m_color.R / (float) byte.MaxValue;
    float num2 = (float) this.m_color.G / (float) byte.MaxValue;
    float num3 = (float) this.m_color.B / (float) byte.MaxValue;
    PdfArray array1 = new PdfArray();
    array1.Insert(0, (IPdfPrimitive) new PdfName("DeviceRGB"));
    array1.Insert(1, (IPdfPrimitive) new PdfNumber(num1));
    array1.Insert(2, (IPdfPrimitive) new PdfNumber(num2));
    array1.Insert(3, (IPdfPrimitive) new PdfNumber(num3));
    this.Dictionary["PC"] = (IPdfPrimitive) new PdfArray(array1);
    float num4 = (float) this.m_intersectionColor.R / (float) byte.MaxValue;
    float num5 = (float) this.m_intersectionColor.G / (float) byte.MaxValue;
    float num6 = (float) this.m_intersectionColor.B / (float) byte.MaxValue;
    PdfArray array2 = new PdfArray();
    array2.Insert(0, (IPdfPrimitive) new PdfName("DeviceRGB"));
    array2.Insert(1, (IPdfPrimitive) new PdfNumber(num4));
    array2.Insert(2, (IPdfPrimitive) new PdfNumber(num5));
    array2.Insert(3, (IPdfPrimitive) new PdfNumber(num6));
    this.Dictionary["IC"] = (IPdfPrimitive) new PdfArray(array2);
    this.Dictionary.SetProperty("IV", (IPdfPrimitive) new PdfBoolean(this.m_intersectionIsVisible));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
