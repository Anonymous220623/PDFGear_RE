// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.DrawingContext
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class DrawingContext
{
  private PIGraphicsSurface _gfxSurface;
  private bool _initalized;
  private Stack<PIGraphicsState> _gfxStates;

  public DrawingContext() => this._gfxStates = new Stack<PIGraphicsState>();

  public PIGraphicsSurface Graphics => this._gfxSurface;

  public void InitializeGraphics(float width, float height)
  {
    this._gfxSurface = (PIGraphicsSurface) new WindowsSurface();
    this._gfxSurface.Initialize(width, height);
    this._initalized = true;
  }

  public void DrawElements(List<string[]> contentElements, PdfPageResources resources)
  {
    if (!this._initalized)
      throw new ArgumentNullException("Drawing surface is not initialized.");
  }
}
