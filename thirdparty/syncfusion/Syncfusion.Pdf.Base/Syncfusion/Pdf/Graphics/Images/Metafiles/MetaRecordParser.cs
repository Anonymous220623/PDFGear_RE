// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.MetaRecordParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class MetaRecordParser : IDisposable
{
  private const byte PointNumber = 2;
  private const byte RectNumber = 4;
  private const int RegionFlag = 268435456 /*0x10000000*/;
  private const int ObjectFlag = 65280;
  private const int PathFillWinding = 24576 /*0x6000*/;
  private const int BrushTypeIndex = 4;
  private static readonly int IntSize = Marshal.SizeOf(typeof (int));
  private static readonly int ShortSize = Marshal.SizeOf(typeof (short));
  private static readonly int FloatSize = Marshal.SizeOf(typeof (float));
  private Metafile m_metaFile;
  private MetafileParser m_parser;
  private PdfEmfRenderer m_renderer;
  private bool m_bDisposed;
  private bool m_bImgWMF;

  public PdfEmfRenderer Renderer
  {
    get => this.Parser.Renderer == null ? this.m_renderer : this.Parser.Renderer;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Renderer));
      if (this.Parser != null)
        this.Parser.Renderer = value;
      else
        this.m_renderer = value;
    }
  }

  public Metafile MetaFile
  {
    get => this.m_metaFile;
    set => this.AssignMetaFile(value);
  }

  public object Context => this.Parser.Context;

  public object ImageContext => this.Parser.ImageContext;

  internal MetafileParser Parser => this.m_parser;

  private MetaRecordParser()
  {
  }

  public MetaRecordParser(PdfEmfRenderer renderer, Metafile metaFile)
    : this()
  {
    if (metaFile == null)
      throw new ArgumentNullException(nameof (metaFile));
    this.AssignMetaFile(metaFile);
    this.Renderer = renderer;
  }

  public void Dispose()
  {
    if (this.m_bDisposed)
      return;
    if (this.m_parser != null)
    {
      this.m_parser.Dispose();
      this.m_parser = (MetafileParser) null;
    }
    if (this.m_bImgWMF && this.m_metaFile != null)
      this.m_metaFile.Dispose();
    this.m_metaFile = (Metafile) null;
    this.m_bDisposed = true;
  }

  public bool Enumerate()
  {
    bool flag = true;
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, (float) this.MetaFile.Width, (float) this.MetaFile.Height);
    try
    {
      using (Bitmap bitmap = new Bitmap(1, 1))
      {
        using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap))
          graphics.EnumerateMetafile(this.MetaFile, rectangleF.Location, this.Parser.ParsingHandler);
      }
    }
    catch (Exception ex)
    {
      flag = false;
      this.Renderer.OnError(ex);
    }
    return flag;
  }

  private void AssignMetaFile(Metafile metaFile)
  {
    if (metaFile == null)
      throw new ArgumentNullException(nameof (metaFile));
    if (this.m_metaFile == metaFile)
      return;
    if (!metaFile.GetMetafileHeader().IsEmfOrEmfPlus())
      this.m_bImgWMF = true;
    this.m_metaFile = PdfMetafile.AdjustMetafile(metaFile);
    SizeF dpi = new SizeF(this.m_metaFile.HorizontalResolution, this.m_metaFile.VerticalResolution);
    this.RecognizeParser(this.m_metaFile.GetMetafileHeader(), dpi);
  }

  private void RecognizeParser(MetafileHeader header, SizeF dpi)
  {
    if (header == null)
      throw new ArgumentNullException(nameof (header));
    switch (header.Type)
    {
      case MetafileType.Wmf:
        this.m_parser = (MetafileParser) new EmfParser(MetafileType.Wmf, dpi);
        break;
      case MetafileType.Emf:
        this.m_parser = (MetafileParser) new EmfParser(MetafileType.Emf, dpi);
        break;
      case MetafileType.EmfPlusOnly:
        this.m_parser = (MetafileParser) new EmfPlusParser(MetafileType.EmfPlusOnly, dpi);
        break;
      case MetafileType.EmfPlusDual:
        this.m_parser = (MetafileParser) new EmfPlusParser(MetafileType.EmfPlusDual, dpi);
        break;
    }
    if (this.Renderer != null)
      this.Parser.Renderer = this.Renderer;
    this.Parser.Metafile = this.MetaFile;
  }
}
