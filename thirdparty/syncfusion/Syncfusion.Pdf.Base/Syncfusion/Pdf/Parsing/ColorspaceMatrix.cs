// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.ColorspaceMatrix
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class ColorspaceMatrix
{
  private double xa;
  private double ya;
  private double za;
  private double xb;
  private double yb;
  private double zb;
  private double xc;
  private double yc;
  private double zc;

  internal double Xa => this.xa;

  internal double Ya => this.ya;

  internal double Za => this.za;

  internal double Xb => this.xb;

  internal double Yb => this.yb;

  internal double Zb => this.zb;

  internal double Xc => this.xc;

  internal double Yc => this.yc;

  internal double Zc => this.zc;

  internal bool IsIdentity
  {
    get
    {
      return this.xa == 1.0 && this.ya == 0.0 && this.za == 0.0 && this.xb == 0.0 && this.yb == 1.0 && this.zb == 0.0 && this.xc == 0.0 && this.yc == 0.0 && this.zc == 1.0;
    }
  }

  internal ColorspaceMatrix(PdfArray array)
  {
    this.xa = array.Count == 9 ? (double) (array[0] as PdfNumber).FloatValue : throw new InvalidOperationException();
    this.ya = (double) (array[1] as PdfNumber).FloatValue;
    this.za = (double) (array[2] as PdfNumber).FloatValue;
    this.xb = (double) (array[3] as PdfNumber).FloatValue;
    this.yb = (double) (array[4] as PdfNumber).FloatValue;
    this.zb = (double) (array[5] as PdfNumber).FloatValue;
    this.xc = (double) (array[6] as PdfNumber).FloatValue;
    this.yc = (double) (array[7] as PdfNumber).FloatValue;
    this.zc = (double) (array[8] as PdfNumber).FloatValue;
  }

  internal IList<object> ToArray()
  {
    return (IList<object>) new object[9]
    {
      (object) this.xa,
      (object) this.ya,
      (object) this.za,
      (object) this.xb,
      (object) this.yb,
      (object) this.zb,
      (object) this.xc,
      (object) this.yc,
      (object) this.zc
    };
  }

  internal double[] Multiply(double x, double y, double z)
  {
    return new double[3]
    {
      this.xa * x + this.ya * y + this.za * z,
      this.xb * x + this.yb * y + this.zb * z,
      this.xc * x + this.yc * y + this.zc * z
    };
  }
}
