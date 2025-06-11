// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageTransition
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageTransition : IPdfWrapper, ICloneable
{
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfTransitionStyle m_style = PdfTransitionStyle.Replace;
  private float m_duration = 1f;
  private PdfTransitionDimension m_dimension;
  private PdfTransitionMotion m_motion;
  private PdfTransitionDirection m_direction;
  private float m_scale = 1f;
  private float m_pageDuration;

  public PdfTransitionStyle Style
  {
    get => this.m_style;
    set
    {
      this.m_style = value;
      this.m_dictionary.SetProperty("S", (IPdfPrimitive) new PdfName(this.StyleToString(this.m_style)));
    }
  }

  public float Duration
  {
    get => this.m_duration;
    set
    {
      this.m_duration = value;
      this.m_dictionary.SetProperty("D", (IPdfPrimitive) new PdfNumber(this.m_duration));
    }
  }

  public PdfTransitionDimension Dimension
  {
    get => this.m_dimension;
    set
    {
      this.m_dimension = value;
      this.m_dictionary.SetProperty("Dm", (IPdfPrimitive) new PdfName(this.DimensionToString(this.m_dimension)));
    }
  }

  public PdfTransitionMotion Motion
  {
    get => this.m_motion;
    set
    {
      this.m_motion = value;
      this.m_dictionary.SetProperty("M", (IPdfPrimitive) new PdfName(this.MotionToString(this.m_motion)));
    }
  }

  public PdfTransitionDirection Direction
  {
    get => this.m_direction;
    set
    {
      this.m_direction = value;
      this.m_dictionary.SetProperty("Di", (IPdfPrimitive) new PdfNumber((int) this.m_direction));
    }
  }

  public float Scale
  {
    get => this.m_scale;
    set
    {
      this.m_scale = value;
      this.m_dictionary.SetProperty("SS", (IPdfPrimitive) new PdfNumber(this.m_scale));
    }
  }

  public float PageDuration
  {
    get => this.m_pageDuration;
    set => this.m_pageDuration = value;
  }

  public PdfPageTransition()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Trans"));
  }

  private string MotionToString(PdfTransitionMotion motion)
  {
    switch (motion)
    {
      case PdfTransitionMotion.Outward:
        return "O";
      default:
        return "I";
    }
  }

  private string DimensionToString(PdfTransitionDimension dimension)
  {
    switch (dimension)
    {
      case PdfTransitionDimension.Vertical:
        return "V";
      default:
        return "H";
    }
  }

  private string StyleToString(PdfTransitionStyle style)
  {
    return style == PdfTransitionStyle.Replace ? "R" : style.ToString();
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;

  public object Clone() => this.MemberwiseClone();
}
