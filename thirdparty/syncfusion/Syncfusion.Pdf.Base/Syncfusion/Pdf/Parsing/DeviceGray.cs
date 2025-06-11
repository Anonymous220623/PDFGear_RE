// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.DeviceGray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class DeviceGray : Colorspace
{
  internal override int Components => 1;

  internal override Color GetColor(string[] pars) => Colorspace.GetGrayColor(pars);

  internal override Color GetColor(byte[] bytes, int offset)
  {
    return Colorspace.GetGrayColor(bytes, offset);
  }

  internal override Brush GetBrush(string[] pars, PdfPageResources resource)
  {
    return new Pen(this.GetColor(pars)).Brush;
  }
}
