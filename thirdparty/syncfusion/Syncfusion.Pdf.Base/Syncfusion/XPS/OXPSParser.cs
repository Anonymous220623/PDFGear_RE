// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.XPS;

internal class OXPSParser : IDisposable
{
  private OXPSFixedPage m_page;
  private OXPSRenderer m_renderer;
  private OXPSParser.EnumerateXPSFileProc m_enumerateHandler;
  private PdfUnitConvertor m_unitConvertor;

  internal OXPSRenderer Renderer => this.m_renderer;

  internal OXPSParser(OXPSFixedPage page, OXPSRenderer renderer)
  {
    this.m_page = page;
    this.m_renderer = renderer;
    this.m_unitConvertor = new PdfUnitConvertor(96f);
  }

  internal void Enumerate()
  {
    if (this.m_enumerateHandler == null)
      this.m_enumerateHandler = new OXPSParser.EnumerateXPSFileProc(this.EnumerateXPSPage);
    this.m_enumerateHandler();
  }

  internal void ReadCanvas(OXPSCanvas canvas)
  {
    PdfGraphicsState state = this.Renderer.Graphics.Save();
    this.Renderer.DrawCanvas(canvas);
    this.Renderer.Graphics.Restore(state);
  }

  internal void ReadGlyphs(OXPSGlyphs glyphs) => this.Renderer.DrawGlyphs(glyphs);

  private void ReadPath(OXPSPath path) => this.Renderer.DrawPath(path);

  internal void EnumerateXPSPage()
  {
    if (this.m_page.Items == null)
      return;
    foreach (object obj in this.m_page.Items)
    {
      switch (obj)
      {
        case OXPSCanvas _:
          this.ReadCanvas((OXPSCanvas) obj);
          break;
        case OXPSGlyphs _:
          this.ReadGlyphs((OXPSGlyphs) obj);
          break;
        case OXPSPath _:
          this.ReadPath((OXPSPath) obj);
          break;
      }
    }
  }

  private float PixelsToPoints(double value)
  {
    return this.m_unitConvertor.ConvertFromPixels((float) value, PdfGraphicsUnit.Point);
  }

  public void Dispose()
  {
  }

  private delegate void EnumerateXPSFileProc();
}
