// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.GlyphOutlinesCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class GlyphOutlinesCollection : List<PathFigure>
{
  public GlyphOutlinesCollection Clone()
  {
    GlyphOutlinesCollection outlinesCollection = new GlyphOutlinesCollection();
    foreach (PathFigure pathFigure in (List<PathFigure>) this)
      outlinesCollection.Add(pathFigure.Clone());
    return outlinesCollection;
  }

  public void Transform(Matrix transformMatrix)
  {
    foreach (PathFigure pathFigure in (List<PathFigure>) this)
      pathFigure.Transform(transformMatrix);
  }
}
