// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocument
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Licensing;
using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xmp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocument : PdfDocumentBase, IDisposable
{
  internal const float DefaultMargin = 40f;
  private static PdfFont s_defaultFont = (PdfFont) null;
  [ThreadStatic]
  private static PdfCacheCollection s_cache;
  private static object s_cacheLock = new object();
  private PdfDocumentTemplate m_pageTemplate;
  private PdfAttachmentCollection m_attachments;
  private PdfDocumentPageCollection m_pages;
  private PdfNamedDestinationCollection m_namedDestinations;
  private bool m_isPdfViewerDocumentDisable = true;
  private PdfSectionCollection m_sections;
  private PdfPageSettings m_settings;
  private PdfBookmarkBase m_outlines;
  private bool m_bPageLabels;
  private bool m_bWasEncrypted;
  private PdfDocumentActions m_actions;
  private PdfColorSpace m_colorSpace;
  [ThreadStatic]
  internal static PdfConformanceLevel ConformanceLevel;
  private static bool m_enableCache = true;
  [ThreadStatic]
  internal static bool m_resourceNaming = false;
  [ThreadStatic]
  private static bool m_enableUniqueResourceNaming = true;
  private static bool m_enableThreadSafe = false;
  private bool m_isDisposed;
  private bool m_isTaggedPdf;
  private PdfStructTreeRoot m_treeRoot;
  private bool m_isMergeDocHasSection;
  private ZugferdConformanceLevel m_zugferdConformanceLevel;
  private ZugferdVersion m_zugferdConformanceVersion;
  internal Dictionary<string, PdfImage> m_imageCollection;
  internal static PrivateFontCollection m_privateFonts;
  private Dictionary<string, PdfTrueTypeFont> m_fontCollection;
  internal Dictionary<string, PdfDictionary> m_parnetTagDicitionaryCollection = new Dictionary<string, PdfDictionary>();
  private bool m_split = true;
  internal bool m_WordtoPDFTagged;

  static PdfDocument()
  {
    PdfDocument.s_cache = new PdfCacheCollection();
    PdfDocument.m_privateFonts = new PrivateFontCollection();
  }

  public PdfDocument()
    : this(false)
  {
    PdfDocument.ConformanceLevel = PdfConformanceLevel.None;
  }

  internal bool SeparateTable
  {
    get => this.m_split;
    set => this.m_split = value;
  }

  internal PdfDocument(bool isMerging)
  {
    PdfMainObjectCollection moc = new PdfMainObjectCollection();
    this.SetMainObjectCollection(moc);
    this.SetCrossTable(new PdfCrossTable()
    {
      Document = (PdfDocumentBase) this
    });
    PdfCatalog pdfCatalog = new PdfCatalog();
    this.SetCatalog(pdfCatalog);
    moc.Add((IPdfPrimitive) pdfCatalog);
    if (!isMerging)
      pdfCatalog.Position = -1;
    this.m_sections = new PdfSectionCollection(this);
    this.m_pages = new PdfDocumentPageCollection(this);
    pdfCatalog.Pages = this.m_sections;
  }

  public PdfDocument(PdfConformanceLevel conformance)
    : this()
  {
    PdfDocument.ConformanceLevel = conformance;
    if (this.Conformance == PdfConformanceLevel.Pdf_A1B || this.Conformance == PdfConformanceLevel.Pdf_A1A || this.Conformance == PdfConformanceLevel.Pdf_A3B || this.Conformance == PdfConformanceLevel.Pdf_A2B || this.Conformance == PdfConformanceLevel.Pdf_A2A || this.Conformance == PdfConformanceLevel.Pdf_A2U || this.Conformance == PdfConformanceLevel.Pdf_A3A || this.Conformance == PdfConformanceLevel.Pdf_A3U)
    {
      this.FileStructure.CrossReferenceType = PdfCrossReferenceType.CrossReferenceTable;
      this.FileStructure.Version = PdfVersion.Version1_4;
      if (conformance == PdfConformanceLevel.Pdf_A1A || conformance == PdfConformanceLevel.Pdf_A2A || conformance == PdfConformanceLevel.Pdf_A3A)
        this.AutoTag = true;
      this.SetDocumentColorProfile();
    }
    else
    {
      if (conformance != PdfConformanceLevel.Pdf_X1A2001)
        return;
      this.FileStructure.Version = PdfVersion.Version1_3;
      this.FileStructure.CrossReferenceType = PdfCrossReferenceType.CrossReferenceTable;
      this.DocumentInformation.XmpMetadata.ToString();
      this.DocumentInformation.ApplyPdfXConformance();
      this.Catalog.ApplyPdfXConformance();
    }
  }

  public ZugferdConformanceLevel ZugferdConformanceLevel
  {
    get => this.m_zugferdConformanceLevel;
    set
    {
      this.Catalog.Pages.Document.DocumentInformation.ZugferdConformanceLevel = value;
      this.m_zugferdConformanceLevel = value;
    }
  }

  public ZugferdVersion ZugferdVersion
  {
    get => this.m_zugferdConformanceVersion;
    set
    {
      this.Catalog.Pages.Document.DocumentInformation.ZugferdVersion = value;
      this.m_zugferdConformanceVersion = value;
    }
  }

  public PdfDocumentTemplate Template
  {
    get
    {
      if (this.m_pageTemplate == null)
        this.m_pageTemplate = new PdfDocumentTemplate();
      return this.m_pageTemplate;
    }
    set => this.m_pageTemplate = value;
  }

  internal override bool IsPdfViewerDocumentDisable
  {
    get => this.m_isPdfViewerDocumentDisable;
    set => this.m_isPdfViewerDocumentDisable = value;
  }

  public PdfDocumentActions Actions
  {
    get
    {
      if (this.m_actions == null)
      {
        this.m_actions = new PdfDocumentActions(this.Catalog);
        this.Catalog["AA"] = ((IPdfWrapper) this.m_actions).Element;
      }
      return this.m_actions;
    }
  }

  public bool AutoTag
  {
    get => this.m_isTaggedPdf;
    set
    {
      this.m_isTaggedPdf = value;
      this.FileStructure.TaggedPdf = this.m_isTaggedPdf;
      this.DocumentInformation.m_autoTag = value;
    }
  }

  public PdfDocumentPageCollection Pages => this.m_pages;

  public PdfNamedDestinationCollection NamedDestinationCollection
  {
    get
    {
      if (this.m_namedDestinations == null)
      {
        this.m_namedDestinations = new PdfNamedDestinationCollection();
        if (this.Catalog.ContainsKey("Names"))
        {
          PdfReferenceHolder pdfReferenceHolder = this.Catalog["Names"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary pdfDictionary)
            pdfDictionary["Dests"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_namedDestinations);
        }
        else
        {
          PdfCatalogNames names = this.Catalog.Names;
          if (this.Catalog.ContainsKey("Names"))
          {
            PdfReferenceHolder pdfReferenceHolder = this.Catalog["Names"] as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary pdfDictionary)
              pdfDictionary["Dests"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_namedDestinations);
          }
        }
      }
      return this.m_namedDestinations;
    }
  }

  public PdfSectionCollection Sections => this.m_sections;

  internal bool IsMergeDocHasSections
  {
    get => this.m_isMergeDocHasSection;
    set => this.m_isMergeDocHasSection = value;
  }

  public PdfPageSettings PageSettings
  {
    get
    {
      if (this.m_settings == null)
        this.m_settings = new PdfPageSettings(40f);
      return this.m_settings;
    }
    set
    {
      this.m_settings = value != null ? value : throw new ArgumentNullException(nameof (PageSettings));
    }
  }

  public override PdfBookmarkBase Bookmarks
  {
    get
    {
      if (this.m_outlines == null)
      {
        this.m_outlines = new PdfBookmarkBase();
        this.Catalog["Outlines"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_outlines);
      }
      return this.m_outlines;
    }
  }

  public PdfAttachmentCollection Attachments
  {
    get
    {
      if (this.m_attachments == null)
      {
        this.m_attachments = new PdfAttachmentCollection();
        this.Catalog.Names.EmbeddedFiles = this.m_attachments;
      }
      return this.m_attachments;
    }
  }

  public PdfForm Form
  {
    get
    {
      if (this.Catalog.Form == null)
        this.Catalog.Form = new PdfForm();
      return this.Catalog.Form;
    }
  }

  public PdfColorSpace ColorSpace
  {
    get
    {
      return this.m_colorSpace == PdfColorSpace.RGB || this.m_colorSpace == PdfColorSpace.CMYK || this.m_colorSpace == PdfColorSpace.GrayScale ? this.m_colorSpace : PdfColorSpace.RGB;
    }
    set
    {
      if (value == PdfColorSpace.RGB || value == PdfColorSpace.CMYK || value == PdfColorSpace.GrayScale)
        this.m_colorSpace = value;
      else
        this.m_colorSpace = PdfColorSpace.RGB;
    }
  }

  internal static PdfCacheCollection Cache
  {
    get
    {
      lock (PdfDocument.s_cacheLock)
      {
        if (PdfDocument.s_cache == null)
          PdfDocument.s_cache = new PdfCacheCollection();
        return PdfDocument.s_cache;
      }
    }
    set => PdfDocument.s_cache = value;
  }

  internal static PrivateFontCollection PrivateFonts
  {
    get
    {
      if (PdfDocument.m_privateFonts == null)
        PdfDocument.m_privateFonts = new PrivateFontCollection();
      return PdfDocument.m_privateFonts;
    }
  }

  internal static PdfFont DefaultFont
  {
    get
    {
      lock (PdfDocument.s_cacheLock)
      {
        if (PdfDocument.s_defaultFont == null)
          PdfDocument.s_defaultFont = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 8f);
      }
      return PdfDocument.s_defaultFont;
    }
  }

  internal override bool WasEncrypted => this.m_bWasEncrypted;

  internal Dictionary<string, PdfImage> ImageCollection
  {
    get
    {
      if (this.m_imageCollection == null)
        this.m_imageCollection = new Dictionary<string, PdfImage>();
      return this.m_imageCollection;
    }
  }

  internal Dictionary<string, PdfTrueTypeFont> FontCollection
  {
    get
    {
      if (this.m_fontCollection == null)
        this.m_fontCollection = new Dictionary<string, PdfTrueTypeFont>();
      return this.m_fontCollection;
    }
  }

  internal override int PageCount => this.Pages.Count;

  public PdfConformanceLevel Conformance => PdfDocument.ConformanceLevel;

  private string CheckLicense()
  {
    string empty = string.Empty;
    return FusionLicenseProvider.GetLicenseType(Platform.FileFormats);
  }

  public override void Save(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (!string.IsNullOrEmpty(this.CheckLicense()))
    {
      lock (PdfDocumentBase.s_licenseLock)
        this.AddWaterMark();
    }
    if (this.Form != null && this.Form.Xfa != null)
      this.Form.Xfa.Save(this);
    this.CheckPagesPresence();
    if (this.Conformance == PdfConformanceLevel.Pdf_A1B || this.Conformance == PdfConformanceLevel.Pdf_A1A || this.Conformance == PdfConformanceLevel.Pdf_A3B || this.Conformance == PdfConformanceLevel.Pdf_A2B || this.Conformance == PdfConformanceLevel.Pdf_A2A || this.Conformance == PdfConformanceLevel.Pdf_A2U || this.Conformance == PdfConformanceLevel.Pdf_A3A || this.Conformance == PdfConformanceLevel.Pdf_A3U)
    {
      this.AutoTagRequiredElements();
      if ((this.Conformance == PdfConformanceLevel.Pdf_A3B || this.Conformance == PdfConformanceLevel.Pdf_A3A || this.Conformance == PdfConformanceLevel.Pdf_A3U) && this.Catalog != null && this.Catalog.Names != null && this.Attachments != null && this.Attachments.Count > 0)
      {
        PdfName key = new PdfName("AFRelationship");
        PdfArray pdfArray = new PdfArray();
        for (int index = 0; index < this.Attachments.Count; ++index)
        {
          if (this.ZugferdConformanceLevel == ZugferdConformanceLevel.None)
          {
            pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.Attachments[index].Dictionary));
          }
          else
          {
            if (!this.Attachments[index].Dictionary.Items.ContainsKey(key))
              this.Attachments[index].Dictionary.Items[key] = (IPdfPrimitive) new PdfName((Enum) PdfAttachmentRelationship.Alternative);
            pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.Attachments[index].Dictionary));
          }
        }
        this.Catalog.Items[new PdfName("AF")] = (IPdfPrimitive) pdfArray;
      }
    }
    PdfWriter writer = new PdfWriter(stream);
    writer.Document = (PdfDocumentBase) this;
    if (this.m_outlines != null && this.m_outlines.Count < 1)
      this.Catalog.Remove("Outlines");
    if (PdfCatalog.StructTreeRoot != null)
    {
      this.DocumentInformation.m_autoTag = true;
      this.AutoTagRequiredElements();
      if (this.DocumentInformation.Language != null)
        this.Catalog["Lang"] = (IPdfPrimitive) new PdfString(this.DocumentInformation.Language);
      else
        this.Catalog["Lang"] = (IPdfPrimitive) new PdfString("en");
      if (this.DocumentInformation.Title == string.Empty)
        this.DocumentInformation.Title = "Tagged Pdf";
      if (!this.Catalog.ContainsKey("StructTreeRoot"))
        this.Catalog["StructTreeRoot"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) PdfCatalog.StructTreeRoot);
      if (!this.Catalog.ContainsKey("MarkInfo"))
        this.Catalog["MarkInfo"] = (IPdfPrimitive) new PdfDictionary();
      this.ViewerPreferences.DisplayTitle = true;
      (this.Catalog["MarkInfo"] as PdfDictionary)["Marked"] = (IPdfPrimitive) new PdfBoolean(true);
      this.m_treeRoot = PdfCrossTable.Dereference(this.Catalog["StructTreeRoot"]) as PdfStructTreeRoot;
      if (this.m_treeRoot != null && this.m_treeRoot.HasOrder && this.m_treeRoot.ContainsKey("K") && PdfCrossTable.Dereference(this.m_treeRoot["K"]) is PdfArray pdfArray)
      {
        if (this.m_WordtoPDFTagged)
          this.m_treeRoot.m_WordtoPDFTaggedObject = this.m_WordtoPDFTagged;
        if (this.m_treeRoot.OrderList.Count == pdfArray.Count)
          this.m_treeRoot.ReArrange(pdfArray, this.m_treeRoot.OrderList);
        else
          this.m_treeRoot.GetChildElements(pdfArray);
      }
    }
    this.ProcessPageLabels();
    if (this.Security.EncryptOnlyAttachment && this.Security.UserPassword == string.Empty)
      throw new PdfException("User password cannot be empty for encrypt only attachment.");
    this.CrossTable.Save(writer);
    if (this.progressDelegate != null)
    {
      int count = this.Pages.Count;
      this.OnSaveProgress(new ProgressEventArgs(count, count));
    }
    this.OnDocumentSaved(new DocumentSavedEventArgs(writer));
    writer.Close();
  }

  private void AddWaterMark()
  {
    if (this.Pages == null || this.Pages.Count <= 0 || this.Pages[0] == null)
      return;
    PdfGraphics graphics = this.Pages[0].Graphics;
    bool uniqueResourceNaming = PdfDocument.EnableUniqueResourceNaming;
    PdfDocument.EnableUniqueResourceNaming = true;
    PdfPageRotateAngle rotate = this.PageSettings.Rotate;
    PdfTemplate pdfTemplate = (PdfTemplate) null;
    switch (rotate)
    {
      case PdfPageRotateAngle.RotateAngle0:
      case PdfPageRotateAngle.RotateAngle90:
      case PdfPageRotateAngle.RotateAngle270:
        pdfTemplate = new PdfTemplate(this.Pages[0].Size.Width, this.Pages[0].Size.Height);
        break;
      case PdfPageRotateAngle.RotateAngle180:
        pdfTemplate = new PdfTemplate(this.Pages[0].Size.Width, 33.75f);
        break;
    }
    pdfTemplate.Graphics.Save();
    pdfTemplate.Graphics.SetTransparency(0.2f);
    if (rotate == PdfPageRotateAngle.RotateAngle0 || rotate == PdfPageRotateAngle.RotateAngle180)
      pdfTemplate.Graphics.DrawRectangle(PdfBrushes.White, new RectangleF(0.0f, 0.0f, this.Pages[0].Size.Width, 33.75f));
    else if (rotate == PdfPageRotateAngle.RotateAngle90)
      pdfTemplate.Graphics.DrawRectangle(PdfBrushes.White, new RectangleF(0.0f, 0.0f, 33.75f, this.Pages[0].Size.Height));
    else
      pdfTemplate.Graphics.DrawRectangle(PdfBrushes.White, new RectangleF(this.Pages[0].Size.Width - 33.75f, 0.0f, 33.75f, this.Pages[0].Size.Height));
    pdfTemplate.Graphics.Restore();
    pdfTemplate.Graphics.Save();
    pdfTemplate.Graphics.SetTransparency(0.5f);
    PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12f, PdfFontStyle.Regular, true);
    SizeF sizeF = new SizeF(220.962f, 16.184f);
    PdfStringFormat format = new PdfStringFormat();
    format.LineAlignment = PdfVerticalAlignment.Middle;
    format.Alignment = PdfTextAlignment.Center;
    switch (rotate)
    {
      case PdfPageRotateAngle.RotateAngle90:
        pdfTemplate.Graphics.TranslateTransform(0.0f, this.Pages[0].Size.Height);
        pdfTemplate.Graphics.RotateTransform(-90f);
        pdfTemplate.Graphics.DrawString("Created with a trial version of Syncfusion Essential PDF", (PdfFont) font, PdfBrushes.Black, new RectangleF(0.0f, 0.0f, this.Pages[0].Size.Height, 33.75f), format);
        break;
      case PdfPageRotateAngle.RotateAngle180:
        pdfTemplate.Graphics.TranslateTransform(this.Pages[0].Size.Width, 0.0f);
        pdfTemplate.Graphics.RotateTransform(180f);
        pdfTemplate.Graphics.DrawString("Created with a trial version of Syncfusion Essential PDF", (PdfFont) font, PdfBrushes.Black, new PointF((float) ((double) this.Pages[0].Size.Width / 2.0 - (double) sizeF.Width / 2.0), -22f));
        break;
      case PdfPageRotateAngle.RotateAngle270:
        pdfTemplate.Graphics.TranslateTransform(this.Pages[0].Size.Width - 33.75f, 0.0f);
        pdfTemplate.Graphics.RotateTransform(90f);
        pdfTemplate.Graphics.DrawString("Created with a trial version of Syncfusion Essential PDF", (PdfFont) font, PdfBrushes.Black, new PointF((float) ((double) this.Pages[0].Size.Height / 2.0 - (double) sizeF.Width / 2.0), -22f));
        break;
      default:
        pdfTemplate.Graphics.DrawString("Created with a trial version of Syncfusion Essential PDF", (PdfFont) font, PdfBrushes.Black, new RectangleF(0.0f, 0.0f, this.Pages[0].Size.Width, 33.75f), format);
        break;
    }
    pdfTemplate.Graphics.Restore();
    this.SetWaterMarkResources(pdfTemplate.m_resources, this.Pages[0].GetResources());
    PdfDictionary content = (PdfDictionary) pdfTemplate.m_content;
    content.Items.Clear();
    this.Pages[0].Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (content as PdfStream)));
    PdfDocument.EnableUniqueResourceNaming = uniqueResourceNaming;
  }

  public override void Close(bool completely)
  {
    if (this.m_isDisposed)
      return;
    this.m_isDisposed = true;
    if (completely && this.Form != null && this.EnableMemoryOptimization)
      this.Form.Clear();
    if (completely && this.EnableMemoryOptimization)
    {
      this.m_off = (PdfArray) null;
      this.m_on = (PdfArray) null;
      this.m_order = (PdfArray) null;
      if (this.m_outlines != null)
        this.m_outlines.Dispose();
      this.progressDelegate = (PdfDocumentBase.ProgressEventHandler) null;
      this.m_sublayer = (PdfArray) null;
      if (this.m_pages != null)
        this.m_pages.Clear();
      if (this.m_sections != null)
        this.m_sections.Clear();
      PdfDocument.s_defaultFont = (PdfFont) null;
    }
    base.Close(completely);
    PdfDocument.ConformanceLevel = PdfConformanceLevel.None;
    this.m_pageTemplate = (PdfDocumentTemplate) null;
    this.m_attachments = (PdfAttachmentCollection) null;
    this.m_pages = (PdfDocumentPageCollection) null;
    this.m_sections = (PdfSectionCollection) null;
    this.m_settings = (PdfPageSettings) null;
    this.m_outlines = (PdfBookmarkBase) null;
    this.m_bPageLabels = false;
    this.m_bWasEncrypted = false;
    this.m_actions = (PdfDocumentActions) null;
    this.m_parnetTagDicitionaryCollection.Clear();
    PdfCatalog.m_structTreeRoot = (PdfStructTreeRoot) null;
    if (this.m_treeRoot != null)
      this.m_treeRoot.Dispose();
    if (this.m_imageCollection != null)
    {
      foreach (KeyValuePair<string, PdfImage> image1 in this.m_imageCollection)
      {
        if (image1.Value is PdfBitmap image2)
        {
          if (image2.Mask != null && image2.Mask is PdfImageMask mask1)
          {
            PdfBitmap mask = mask1.Mask;
            if (mask != null)
              this.DisposeImageStreams(mask);
          }
          this.DisposeImageStreams(image2);
        }
      }
      this.m_imageCollection.Clear();
    }
    PdfDocument.m_privateFonts = (PrivateFontCollection) null;
    if (this.m_fontCollection == null)
      return;
    foreach (KeyValuePair<string, PdfTrueTypeFont> font in this.m_fontCollection)
      font.Value.Dispose();
    this.m_fontCollection.Clear();
  }

  private void DisposeImageStreams(PdfBitmap image)
  {
    if (image.Stream == null || image.Stream.InternalStream == null)
      return;
    image.Stream.Clear();
    image.Stream.InternalStream.Dispose();
    image.Stream.InternalStream = (MemoryStream) null;
  }

  public void Dispose()
  {
    if (this.EnableMemoryOptimization)
      this.Close(true);
    else
      this.Close(false);
    GC.SuppressFinalize((object) this);
  }

  public object Clone()
  {
    if (this.CrossTable.EncryptorDictionary != null)
      throw new ArgumentException("Can't clone the Encrypted document");
    return this.MemberwiseClone();
  }

  public static void ClearFontCache()
  {
    Dictionary<string, PdfFont> fontCollection = PdfDocument.Cache.FontCollection;
    if (fontCollection == null || fontCollection.Count <= 0)
      return;
    foreach (KeyValuePair<string, PdfFont> keyValuePair in fontCollection)
    {
      if (keyValuePair.Value != null && keyValuePair.Value is PdfTrueTypeFont pdfTrueTypeFont && pdfTrueTypeFont.m_fontInternal != null)
        pdfTrueTypeFont.m_fontInternal.Close();
    }
    fontCollection.Clear();
  }

  private void AutoTagRequiredElements()
  {
    if (this.DocumentInformation.Language != null)
      this.Catalog["Lang"] = (IPdfPrimitive) new PdfString(this.DocumentInformation.Language);
    else
      this.DocumentInformation.Language = "en";
    if (this.DocumentInformation.Title == string.Empty)
      this.DocumentInformation.Title = "Tagged Pdf";
    XmpMetadata xmpMetadata = this.DocumentInformation.XmpMetadata;
  }

  internal static void ValidateLicense()
  {
    try
    {
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(PdfBaseAssembly.AssemblyResolver);
    }
    finally
    {
      GC.Collect();
      AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(PdfBaseAssembly.AssemblyResolver);
    }
  }

  internal void PageLabelsSet() => this.m_bPageLabels = true;

  private void CheckPagesPresence()
  {
    if (this.Pages.Count != 0)
      return;
    this.Pages.Add();
  }

  public static bool EnableCache
  {
    get => PdfDocument.m_enableCache;
    set => PdfDocument.m_enableCache = value;
  }

  public static bool EnableUniqueResourceNaming
  {
    get => PdfDocument.m_enableUniqueResourceNaming;
    set
    {
      PdfDocument.m_enableUniqueResourceNaming = value;
      if (value)
        PdfDocument.m_resourceNaming = false;
      else
        PdfDocument.m_resourceNaming = true;
    }
  }

  public static bool EnableThreadSafe
  {
    get => PdfDocument.m_enableThreadSafe;
    set
    {
      PdfDocument.m_enableThreadSafe = value;
      if (value)
        PdfDocument.m_enableCache = false;
      else
        PdfDocument.m_enableCache = true;
    }
  }

  private void ProcessPageLabels()
  {
    if (!this.m_bPageLabels)
      return;
    if (!(this.Catalog["PageLabels"] is PdfDictionary pdfDictionary))
    {
      pdfDictionary = new PdfDictionary();
      this.Catalog["PageLabels"] = (IPdfPrimitive) pdfDictionary;
    }
    PdfArray pdfArray = new PdfArray();
    pdfDictionary["Nums"] = (IPdfPrimitive) pdfArray;
    int num = 0;
    foreach (PdfSection section in this.Sections)
    {
      PdfPageLabel pdfPageLabel = section.PageLabel ?? new PdfPageLabel();
      pdfArray.Add((IPdfPrimitive) new PdfNumber(num));
      pdfArray.Add(((IPdfWrapper) pdfPageLabel).Element);
      num += section.Count;
    }
  }

  private void SetDocumentColorProfile()
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary["Info"] = (IPdfPrimitive) new PdfString("sRGB IEC61966-2.1");
    pdfDictionary["S"] = (IPdfPrimitive) new PdfName("GTS_PDFA1");
    pdfDictionary["OutputConditionIdentifier"] = (IPdfPrimitive) new PdfString("custom");
    pdfDictionary["Type"] = (IPdfPrimitive) new PdfName("OutputIntent");
    pdfDictionary["OutputCondition"] = (IPdfPrimitive) new PdfString("");
    pdfDictionary["RegistryName"] = (IPdfPrimitive) new PdfString("");
    PdfICCColorProfile wrapper = new PdfICCColorProfile();
    pdfDictionary["DestOutputProfile"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper);
    this.Catalog["OutputIntents"] = (IPdfPrimitive) new PdfArray()
    {
      (IPdfPrimitive) pdfDictionary
    };
  }

  internal string GetImageHash(Image img)
  {
    ImageConverter imageConverter = new ImageConverter();
    byte[] numArray = new byte[1];
    return this.GetImageHash((byte[]) imageConverter.ConvertTo((object) img, numArray.GetType()));
  }

  internal string GetImageHash(byte[] imageData)
  {
    string hashFromStream = this.CreateHashFromStream(imageData);
    imageData = (byte[]) null;
    return hashFromStream;
  }

  internal override PdfForm ObtainForm() => this.Form;

  internal override void AddFields(
    PdfLoadedDocument ldDoc,
    PdfPageBase newPage,
    List<PdfField> fields)
  {
    List<PdfDictionary> pdfDictionaryList = (List<PdfDictionary>) null;
    PdfArray pdfArray1 = (PdfArray) null;
    if (ldDoc.Catalog.ContainsKey("AcroForm"))
    {
      this.CloneAcroFormFontResources(ldDoc);
      if (PdfCrossTable.Dereference(ldDoc.Catalog["AcroForm"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("CO") && PdfCrossTable.Dereference(pdfDictionary1["CO"]) is PdfArray pdfArray2)
      {
        pdfDictionaryList = new List<PdfDictionary>();
        pdfArray1 = this.Form.Dictionary.ContainsKey("CO") ? PdfCrossTable.Dereference(this.Form.Dictionary["CO"]) as PdfArray : new PdfArray();
        for (int index = 0; index < pdfArray2.Count; ++index)
        {
          if (PdfCrossTable.Dereference(pdfArray2[index]) is PdfDictionary pdfDictionary && !pdfDictionaryList.Contains(pdfDictionary))
            pdfDictionaryList.Add(pdfDictionary);
        }
      }
    }
    int index1 = 0;
    for (int count = fields.Count; index1 < count; ++index1)
    {
      if (!this.EnableMemoryOptimization && fields[index1].Dictionary.ContainsKey("P"))
        fields[index1].Dictionary.Remove("P");
      int index2 = this.Form.Fields.Add(fields[index1], newPage);
      if (pdfDictionaryList != null && pdfArray1 != null && pdfDictionaryList.Contains(fields[index1].Dictionary) && !pdfArray1.Contains((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Form.Fields[index2])))
        pdfArray1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Form.Fields[index2]));
    }
    if (this.EnableMemoryOptimization && this.Form != null && this.Form.Fields.Count > 0 && ldDoc.Form != null)
    {
      PdfReferenceHolder pdfReferenceHolder = ldDoc.Catalog["AcroForm"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null && !ldDoc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) pdfReferenceHolder.Reference))
      {
        PdfReference reference = this.CrossTable.GetReference((IPdfPrimitive) this.Form.Dictionary);
        ldDoc.CrossTable.PageCorrespondance.Add((IPdfPrimitive) pdfReferenceHolder.Reference, (object) reference);
      }
    }
    if (pdfArray1 == null || pdfArray1.Count <= 0)
      return;
    this.Form.Dictionary["CO"] = (IPdfPrimitive) pdfArray1;
    pdfDictionaryList?.Clear();
  }

  internal override PdfPageBase ClonePage(
    PdfLoadedDocument ldDoc,
    PdfPageBase page,
    List<PdfArray> destinations)
  {
    return this.Pages.Add(ldDoc, page, destinations);
  }

  private void CloneAcroFormFontResources(PdfLoadedDocument ldDoc)
  {
    if (!(PdfCrossTable.Dereference(ldDoc.Catalog["AcroForm"]) is PdfDictionary pdfDictionary1))
      return;
    if (PdfCrossTable.Dereference(pdfDictionary1["DR"]) is PdfDictionary pdfDictionary9 && pdfDictionary9.ContainsKey("Font"))
    {
      PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary9["Font"]) as PdfDictionary;
      if (this.Form.Dictionary != null && pdfDictionary2 != null)
      {
        if (this.Form.Dictionary.ContainsKey("DR"))
        {
          PdfDictionary baseDictionary = PdfCrossTable.Dereference(this.Form.Dictionary["DR"]) as PdfDictionary;
          PdfDictionary pdfDictionary3 = PdfCrossTable.Dereference(pdfDictionary9["Font"]) as PdfDictionary;
          PdfDictionary pdfDictionary4 = this.EnableMemoryOptimization ? pdfDictionary2.Clone(this.CrossTable) as PdfDictionary : pdfDictionary2;
          if (pdfDictionary4 != null && baseDictionary != null)
          {
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary4.Items)
            {
              if (pdfDictionary3 != null && pdfDictionary3.Items != null && !pdfDictionary3.Items.ContainsKey(keyValuePair.Key))
                pdfDictionary3.Items.Add(keyValuePair.Key, keyValuePair.Value);
              if (PdfCrossTable.Dereference(baseDictionary.Items[new PdfName("Font")]) is PdfDictionary pdfDictionary8 && !pdfDictionary8.Items.ContainsKey(keyValuePair.Key))
              {
                if (PdfCrossTable.Dereference(baseDictionary.Items[new PdfName("Font")]) is PdfDictionary pdfDictionary5)
                  pdfDictionary5.Items.Add(keyValuePair.Key, keyValuePair.Value);
              }
              else if (pdfDictionary8 != null)
              {
                PdfDictionary pdfDictionary6 = PdfCrossTable.Dereference(pdfDictionary8.Items[keyValuePair.Key]) as PdfDictionary;
                PdfName key = new PdfName("Encoding");
                if (pdfDictionary6 != null && pdfDictionary3 != null && pdfDictionary3.Items != null && pdfDictionary6.Items.ContainsKey(key) && pdfDictionary3.Items.ContainsKey(keyValuePair.Key))
                {
                  PdfName pdfName1 = PdfCrossTable.Dereference(pdfDictionary6.Items[key]) as PdfName;
                  if (PdfCrossTable.Dereference(pdfDictionary3.Items[keyValuePair.Key]) is PdfDictionary pdfDictionary7 && pdfDictionary7.ContainsKey(key))
                  {
                    PdfName pdfName2 = PdfCrossTable.Dereference(pdfDictionary7.Items[key]) as PdfName;
                    if (pdfName1 != (PdfName) null && pdfName2 != (PdfName) null && pdfName1.Equals((object) pdfName2))
                    {
                      pdfDictionary6.Items.Remove(new PdfName("Encoding"));
                      pdfDictionary6.Items.Add(new PdfName("Encoding"), (IPdfPrimitive) pdfName2);
                      PdfResources primitive = new PdfResources(baseDictionary);
                      this.Form.Resources = primitive;
                      this.Form.Dictionary.SetProperty("DR", (IPdfPrimitive) primitive);
                      this.Form.Dictionary.Modify();
                    }
                  }
                }
              }
            }
          }
          else if (pdfDictionary4 != null)
          {
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary4.Items)
            {
              if (pdfDictionary3 != null && pdfDictionary3.Items != null && !pdfDictionary3.Items.ContainsKey(keyValuePair.Key))
                pdfDictionary3.Items.Add(keyValuePair.Key, keyValuePair.Value);
            }
          }
          pdfDictionary3?.Modify();
        }
        else
        {
          PdfResources primitive = new PdfResources(!this.EnableMemoryOptimization ? pdfDictionary9 : pdfDictionary9.Clone(this.CrossTable) as PdfDictionary);
          this.Form.Resources = primitive;
          this.Form.Dictionary.SetProperty("DR", (IPdfPrimitive) primitive);
          this.Form.Dictionary.Modify();
        }
      }
    }
    this.Form.SetAppearanceDictionary = ldDoc.Form.SetAppearanceDictionary;
    this.Form.NeedAppearances = ldDoc.Form.NeedAppearances;
  }
}
