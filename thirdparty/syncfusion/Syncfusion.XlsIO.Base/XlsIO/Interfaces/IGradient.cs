// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IGradient
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IGradient
{
  ColorObject BackColorObject { get; }

  Color BackColor { get; set; }

  ExcelKnownColors BackColorIndex { get; set; }

  ColorObject ForeColorObject { get; }

  Color ForeColor { get; set; }

  ExcelKnownColors ForeColorIndex { get; set; }

  ExcelGradientStyle GradientStyle { get; set; }

  ExcelGradientVariants GradientVariant { get; set; }

  int CompareTo(IGradient gradient);

  void TwoColorGradient();

  void TwoColorGradient(ExcelGradientStyle style, ExcelGradientVariants variant);
}
