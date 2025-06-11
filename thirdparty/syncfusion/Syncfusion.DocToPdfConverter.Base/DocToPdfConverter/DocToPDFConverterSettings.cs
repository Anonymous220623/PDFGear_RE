// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocToPDFConverter.DocToPDFConverterSettings
// Assembly: Syncfusion.DocToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 84EFC094-D348-494C-A410-44F5807BB0D3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocToPdfConverter.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf;

#nullable disable
namespace Syncfusion.DocToPDFConverter;

public class DocToPDFConverterSettings
{
  private bool m_autoDetectComplexScript;
  private bool m_enableAlternateChunks = true;
  private bool m_enableFastRendering;
  private bool m_updateDocumentFields;
  private int m_imageQuality = 100;
  private bool m_preserveFormFields;
  internal int m_imageResolution;
  private byte m_bFlags = 1;
  private byte m_bFlags1 = 2;
  private IWarning m_warning;
  private PdfConformanceLevel m_pdfConformanceLevel;
  private bool m_preserveOleEquationAsBitmap;
  private ExportBookmarkType m_exportBookmarkType;

  public bool AutoDetectComplexScript
  {
    get => this.m_autoDetectComplexScript;
    set => this.m_autoDetectComplexScript = value;
  }

  public bool RecreateNestedMetafile
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public bool EnableAlternateChunks
  {
    get => this.m_enableAlternateChunks;
    set => this.m_enableAlternateChunks = value;
  }

  public bool EnableFastRendering
  {
    get => this.m_enableFastRendering;
    set => this.m_enableFastRendering = value;
  }

  public bool PreserveFormFields
  {
    get => this.m_preserveFormFields;
    set => this.m_preserveFormFields = value;
  }

  public bool UpdateDocumentFields
  {
    get => this.m_updateDocumentFields;
    set => this.m_updateDocumentFields = value;
  }

  public int ImageQuality
  {
    get => this.m_imageQuality;
    set
    {
      this.m_imageQuality = value <= 100 ? value : throw new PdfException("The value should be between 0 and 100");
    }
  }

  public int ImageResolution
  {
    set
    {
      this.m_imageResolution = value > 0 ? value : throw new PdfException("The value should be valid DPI");
    }
  }

  public bool OptimizeIdenticalImages
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public IWarning Warning
  {
    get => this.m_warning;
    set => this.m_warning = value;
  }

  public PdfConformanceLevel PdfConformanceLevel
  {
    get => this.m_pdfConformanceLevel;
    set => this.m_pdfConformanceLevel = value;
  }

  public bool EmbedFonts
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public bool PreserveOleEquationAsBitmap
  {
    get => this.m_preserveOleEquationAsBitmap;
    set => this.m_preserveOleEquationAsBitmap = value;
  }

  public bool EmbedCompleteFonts
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public bool AutoTag
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set
    {
      this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 254 | (value ? 1 : 0));
      if (this.m_enableFastRendering || !value)
        return;
      this.m_enableFastRendering = true;
      this.m_exportBookmarkType = ExportBookmarkType.Headings;
    }
  }

  public ExportBookmarkType ExportBookmarks
  {
    get => this.m_exportBookmarkType;
    set
    {
      if (this.m_exportBookmarkType == ExportBookmarkType.Headings)
        return;
      this.m_exportBookmarkType = value;
    }
  }
}
