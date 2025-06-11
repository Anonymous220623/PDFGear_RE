// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.ITrueTypeFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal interface ITrueTypeFont
{
  Font Font { get; }

  float Size { get; }

  PdfFontMetrics Metrics { get; }

  IPdfPrimitive GetInternals();

  bool EqualsToFont(PdfFont font);

  void CreateInternals();

  int GetCharWidth(char charCode);

  int GetLineWidth(string line);

  void Close();
}
