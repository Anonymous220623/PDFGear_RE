// Decompiled with JetBrains decompiler
// Type: Syncfusion.PresentationToPdfConverter.PresentationToPdfConverterSettings
// Assembly: Syncfusion.PresentationToPdfConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 66FE5253-50B1-47E3-888F-DF2FAFB49C7E
// Assembly location: C:\Program Files\PDFgear\Syncfusion.PresentationToPdfConverter.Base.dll

using Syncfusion.Pdf;

#nullable disable
namespace Syncfusion.PresentationToPdfConverter;

public class PresentationToPdfConverterSettings
{
  private SlidesPerPage _slidesPerPage;
  private bool _showHiddenSlides;
  private bool _enablePortableRendering;
  private PublishOptions _publishOptions;
  private int _imageQuality = 100;
  internal int _imageResolution = 0;
  private PdfConformanceLevel _pdfConformanceLevel;
  private byte _bFlags = 1;

  public SlidesPerPage SlidesPerPage
  {
    get => this._slidesPerPage;
    set
    {
      this._slidesPerPage = value;
      this._publishOptions = PublishOptions.Handouts;
    }
  }

  public bool ShowHiddenSlides
  {
    get => this._showHiddenSlides;
    set => this._showHiddenSlides = value;
  }

  public bool EnablePortableRendering
  {
    get => this._enablePortableRendering;
    set => this._enablePortableRendering = value;
  }

  public PublishOptions PublishOptions
  {
    get => this._publishOptions;
    set => this._publishOptions = value;
  }

  public int ImageQuality
  {
    get => this._imageQuality;
    set
    {
      this._imageQuality = value <= 100 ? value : throw new PdfException("The value should be between 0 and 100");
    }
  }

  public int ImageResolution
  {
    set
    {
      this._imageResolution = value > 0 ? value : throw new PdfException("The value should be valid DPI");
    }
  }

  public PdfConformanceLevel PdfConformanceLevel
  {
    get => this._pdfConformanceLevel;
    set => this._pdfConformanceLevel = value;
  }

  public bool OptimizeIdenticalImages
  {
    get => ((int) this._bFlags & 1) != 0;
    set => this._bFlags = (byte) ((int) this._bFlags & 254 | (value ? 1 : 0));
  }

  public bool EmbedFonts
  {
    get => ((int) this._bFlags & 2) >> 1 != 0;
    set => this._bFlags = (byte) ((int) this._bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool EmbedCompleteFonts
  {
    get => ((int) this._bFlags & 4) >> 2 != 0;
    set => this._bFlags = (byte) ((int) this._bFlags & 251 | (value ? 1 : 0) << 2);
  }
}
