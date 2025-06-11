// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.XPSParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;

#nullable disable
namespace Syncfusion.XPS;

internal class XPSParser : IDisposable
{
  private FixedPage m_page;
  private XPSRenderer m_renderer;
  private XPSParser.EnumerateXPSFileProc m_enumerateHandler;
  private PdfUnitConvertor m_unitConvertor;

  internal XPSRenderer Renderer => this.m_renderer;

  public XPSParser(FixedPage page, XPSRenderer renderer)
  {
    this.m_page = page;
    this.m_renderer = renderer;
    this.m_unitConvertor = new PdfUnitConvertor(96f);
  }

  public void Enumerate()
  {
    if (this.m_enumerateHandler == null)
      this.m_enumerateHandler = new XPSParser.EnumerateXPSFileProc(this.EnumerateXPSPage);
    this.m_enumerateHandler();
  }

  private void ReadCanvas(Canvas canvas)
  {
    PdfGraphicsState state = this.Renderer.Graphics.Save();
    this.Renderer.DrawCanvas(canvas, this.Renderer.Graphics);
    this.Renderer.Graphics.Restore(state);
  }

  private void ReadGlyphs(Glyphs glyphs)
  {
    this.Renderer.DrawGlyphs(glyphs, this.Renderer.Graphics);
  }

  private void ReadPath(Path path) => this.Renderer.DrawPath(path, this.Renderer.Graphics);

  internal void EnumerateXPSPage()
  {
    if (this.m_page.Items == null)
      return;
    foreach (object obj in this.m_page.Items)
    {
      switch (obj)
      {
        case Canvas _:
          this.ReadCanvas((Canvas) obj);
          break;
        case Glyphs _:
          this.ReadGlyphs((Glyphs) obj);
          break;
        case Path _:
          this.ReadPath((Path) obj);
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
