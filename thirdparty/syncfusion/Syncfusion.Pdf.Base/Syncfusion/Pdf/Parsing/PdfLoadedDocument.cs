// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedDocument
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Licensing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Redaction;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf.Xmp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedDocument : PdfDocumentBase, IDisposable
{
  internal string m_password;
  internal Stream m_stream;
  private bool m_bWasEncrypted;
  private bool m_isPdfViewerDocumentDisable = true;
  private PdfLoadedForm m_form;
  private PdfLoadedPageCollection m_pages;
  private PdfBookmarkBase m_bookmark;
  private PdfNamedDestinationCollection m_namedDestinations;
  private bool m_bCloseStream;
  private bool m_isDisposed;
  internal PdfDocumentInformation m_documentInfo;
  private MemoryStream m_internalStream = new MemoryStream();
  private PdfColorSpace m_colorSpace;
  private PdfAttachmentCollection m_attachments;
  private string password;
  private PdfPageLabel m_pageLabel;
  private PdfLoadedPageLabelCollection m_pageLabelCollection;
  private bool isPageLabel;
  private bool m_isXFAForm;
  private bool m_isExtendedFeatureEnabled;
  internal string m_fileName;
  private PdfConformanceLevel m_conformance;
  private bool isLinearized;
  private bool isPortfolio;
  internal bool m_isXfaDocument;
  private PdfPortfolioInformation m_portfolio;
  private static Stream m_openStream;
  private bool isDispose;
  private bool isParsedForm;
  internal bool isOpenAndRepair;
  private int acroformFieldscount;
  private bool m_isOpenAndRepair;
  internal bool m_isNamedDestinationCall = true;
  internal bool IsSkipSaving;
  private bool isAttachmentOnlyEncryption;
  internal bool isCompressPdf;
  private PdfPageTemplateCollection m_pageTemplatesCollection;
  internal Dictionary<string, string> currentFont = new Dictionary<string, string>();
  internal Dictionary<long, PdfFont> font = new Dictionary<long, PdfFont>();
  private DublinCoreSchema m_dublinschema;
  private List<PdfUsedFont> m_usedFonts;
  private PdfCompressionOptions m_compressionOptions;
  private static bool m_isPathStream = false;
  private Dictionary<PdfPageBase, object> m_bookmarkHashtable;
  internal bool IsOcredDocument;
  internal bool ConformanceEnabled;
  internal PdfConformanceLevel m_previousConformance;
  internal List<PdfLoadedPage> m_redactionPages = new List<PdfLoadedPage>();
  internal List<PdfException> pdfException = new List<PdfException>();
  internal bool validateSyntax;
  private bool m_isOptimizeIdentical;
  private PdfDocumentBase m_destinationDocument;
  private bool m_isExtendMargin;
  private bool isRedacted;
  private bool isConformanceApplied;
  private PdfFeatures feature;
  private ImageExportSettings m_imageExportSetting;
  private PdfDocumentActions m_actions;

  internal bool IsXFAForm
  {
    get => this.m_isXFAForm;
    set => this.m_isXFAForm = value;
  }

  public bool IsExtendedFeatureEnabled => this.m_isExtendedFeatureEnabled;

  public PdfCompressionOptions CompressionOptions
  {
    get => this.m_compressionOptions;
    set => this.m_compressionOptions = value;
  }

  internal DublinCoreSchema DublinSchema
  {
    get => this.m_dublinschema;
    set => this.m_dublinschema = value;
  }

  public PdfPageLabel LoadedPageLabel
  {
    get => this.m_pageLabel;
    set
    {
      if (this.m_pageLabelCollection == null)
        this.m_pageLabelCollection = new PdfLoadedPageLabelCollection();
      this.isPageLabel = true;
      this.m_pageLabelCollection.Add(value);
    }
  }

  internal string Password
  {
    get => this.password;
    set => this.password = value;
  }

  public PdfDocumentActions Actions
  {
    get
    {
      if (this.m_actions == null)
        this.m_actions = new PdfDocumentActions(this.Catalog);
      if (this.Catalog["AA"] == null)
        this.Catalog["AA"] = ((IPdfWrapper) this.m_actions).Element;
      return this.m_actions;
    }
  }

  public PdfAttachmentCollection Attachments
  {
    get
    {
      if (this.RaiseUserPassword && this.m_password == string.Empty)
      {
        OnPdfPasswordEventArgs args = new OnPdfPasswordEventArgs();
        this.OnPdfPassword((object) this, args);
        this.m_password = args.UserPassword;
      }
      if (this.isAttachmentOnlyEncryption)
        this.CheckEncryption(this.isAttachmentOnlyEncryption);
      if (this.m_attachments == null)
      {
        PdfDictionary attachmentDictionary = this.GetAttachmentDictionary();
        if (attachmentDictionary != null && attachmentDictionary.Count > 0)
        {
          if (attachmentDictionary.ContainsKey("EmbeddedFiles"))
          {
            this.m_attachments = new PdfAttachmentCollection(attachmentDictionary, this.CrossTable);
            if (this.m_attachments != null)
              this.Catalog.Attachments = this.m_attachments;
          }
          else
          {
            this.m_attachments = new PdfAttachmentCollection();
            if (this.Catalog.Names != null)
              this.Catalog.Names.EmbeddedFiles = this.m_attachments;
          }
        }
      }
      return this.m_attachments;
    }
  }

  public new PdfPortfolioInformation PortfolioInformation
  {
    set => this.Catalog.PdfPortfolio = value;
    get
    {
      if (this.m_portfolio == null)
      {
        PdfDictionary portfolioDictionary = this.GetPortfolioDictionary();
        if (portfolioDictionary != null)
        {
          this.m_portfolio = new PdfPortfolioInformation(portfolioDictionary);
          if (portfolioDictionary["D"] is PdfString pdfString && !string.IsNullOrEmpty(pdfString.Value))
          {
            foreach (PdfAttachment attachment in (PdfCollection) this.Attachments)
            {
              if (pdfString.Value == attachment.FileName)
              {
                this.m_portfolio.StartupDocument = attachment;
                break;
              }
            }
          }
        }
      }
      return this.m_portfolio;
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

  public PdfLoadedForm Form
  {
    get
    {
      if (this.m_form == null)
      {
        PdfDictionary formDictionary = this.GetFormDictionary();
        if (formDictionary != null)
        {
          this.m_form = new PdfLoadedForm(formDictionary, this.CrossTable);
          if (this.m_form != null && this.m_form.Fields.Count != 0)
          {
            this.Catalog.LoadedForm = this.m_form;
            this.acroformFieldscount = this.m_form.Fields.Count;
            List<long> widgetReferences = (List<long>) null;
            if (!this.isParsedForm)
            {
              for (int index = 0; index < (this.m_form.CrossTable.Document as PdfLoadedDocument).Pages.Count; ++index)
              {
                PdfLoadedPage page = (this.m_form.CrossTable.Document as PdfLoadedDocument).Pages[index] as PdfLoadedPage;
                if (widgetReferences == null && page != null)
                  widgetReferences = page.GetWidgetReferences();
                if (page != null && widgetReferences != null && widgetReferences.Count > 0)
                  page.CreateAnnotations(widgetReferences);
              }
              widgetReferences?.Clear();
              if (!this.m_form.IsFormContainsKids)
                this.m_form.Fields.CreateFormFieldsFromWidgets(this.acroformFieldscount);
              this.isParsedForm = true;
            }
          }
        }
      }
      return this.m_form;
    }
  }

  public PdfLoadedPageCollection Pages
  {
    get
    {
      if (this.m_pages == null)
        this.m_pages = new PdfLoadedPageCollection((PdfDocumentBase) this, this.CrossTable);
      return this.m_pages;
    }
  }

  public override PdfBookmarkBase Bookmarks
  {
    get
    {
      if (this.m_bookmark == null)
      {
        if (this.Catalog.ContainsKey("Outlines"))
        {
          if (PdfCrossTable.Dereference(this.Catalog["Outlines"]) is PdfDictionary dictionary)
          {
            this.m_bookmark = new PdfBookmarkBase(dictionary, this.CrossTable);
            if (dictionary.ContainsKey("First"))
            {
              PdfReferenceHolder pdfReferenceHolder = dictionary["First"] as PdfReferenceHolder;
              if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Reference != (PdfReference) null)
                this.m_bookmark.m_bookmarkReference.Add(pdfReferenceHolder.Reference.ObjNum);
            }
            this.m_bookmark.ReproduceTree();
          }
          else
            this.m_bookmark = this.CreateBookmarkRoot();
        }
        else
          this.m_bookmark = this.CreateBookmarkRoot();
      }
      return this.m_bookmark;
    }
  }

  public PdfNamedDestinationCollection NamedDestinationCollection
  {
    get
    {
      if (this.m_namedDestinations == null)
      {
        if (this.Catalog.ContainsKey("Names") && this.m_namedDestinations == null)
          this.m_namedDestinations = new PdfNamedDestinationCollection(PdfCrossTable.Dereference(this.Catalog["Names"]) as PdfDictionary, this.CrossTable);
        else if (this.m_namedDestinations == null && this.m_isNamedDestinationCall)
          this.m_namedDestinations = this.CreateNamedDestinations();
      }
      return this.m_namedDestinations;
    }
  }

  public PdfPageTemplateCollection PdfPageTemplates
  {
    get
    {
      if (this.m_pageTemplatesCollection == null)
      {
        if (this.Catalog.ContainsKey("Names") && this.m_pageTemplatesCollection == null)
          this.m_pageTemplatesCollection = !(PdfCrossTable.Dereference(this.Catalog["Names"]) is PdfDictionary dictionary) ? this.CreatePageTemplates() : new PdfPageTemplateCollection(dictionary, this.CrossTable);
        else if (this.m_pageTemplatesCollection == null)
          this.m_pageTemplatesCollection = this.CreatePageTemplates();
      }
      return this.m_pageTemplatesCollection;
    }
  }

  internal override int PageCount => this.Pages.Count;

  public PdfConformanceLevel Conformance
  {
    get
    {
      if (this.m_conformance == PdfConformanceLevel.None)
        this.m_conformance = this.GetDocumentConformance(this.m_conformance);
      return this.m_conformance;
    }
    set
    {
      this.m_conformance = value == PdfConformanceLevel.Pdf_A1B || value == PdfConformanceLevel.Pdf_A2B || value == PdfConformanceLevel.Pdf_A3B || value == PdfConformanceLevel.None ? value : throw new PdfException($"Pdf conformance level {value.ToString()} is not currently supported.");
      if (value != PdfConformanceLevel.Pdf_A1B && value != PdfConformanceLevel.Pdf_A2B && value == PdfConformanceLevel.Pdf_A2B)
        return;
      this.ConformanceEnabled = true;
      this.m_previousConformance = PdfDocument.ConformanceLevel;
      PdfDocument.ConformanceLevel = value;
    }
  }

  public PdfUsedFont[] UsedFonts
  {
    get
    {
      if (this.m_usedFonts == null)
        this.m_usedFonts = this.ExtractFonts();
      return this.m_usedFonts.ToArray();
    }
  }

  public bool IsLinearized
  {
    get
    {
      this.isLinearized = this.CheckLinearization();
      return this.isLinearized;
    }
  }

  public bool IsPortfolio
  {
    get
    {
      if (this.GetPortfolioDictionary() != null)
      {
        this.isPortfolio = true;
        return this.isPortfolio;
      }
      this.isPortfolio = false;
      return this.isPortfolio;
    }
  }

  internal bool IsOptimizeIdentical
  {
    get => this.m_isOptimizeIdentical;
    set => this.m_isOptimizeIdentical = value;
  }

  internal PdfDocumentBase DestinationDocument
  {
    get => this.m_destinationDocument;
    set => this.m_destinationDocument = value;
  }

  internal bool IsExtendMargin
  {
    get => this.m_isExtendMargin;
    set => this.m_isExtendMargin = value;
  }

  public PdfLoadedDocument(string filename)
    : this(PdfLoadedDocument.CreateStream(filename))
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
    this.m_fileName = filename;
  }

  public PdfLoadedDocument(string filename, bool openAndRepair)
    : this(PdfLoadedDocument.CreateStream(filename), openAndRepair)
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
    this.m_fileName = filename;
  }

  internal PdfLoadedDocument(string filename, bool openAndRepair, bool isXfaDocument)
    : this(PdfLoadedDocument.CreateStream(filename), openAndRepair, isXfaDocument)
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
    this.m_fileName = filename;
  }

  public PdfLoadedDocument(string filename, string password)
    : this(PdfLoadedDocument.CreateStream(filename), password)
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
    this.Password = password;
    this.m_fileName = filename;
  }

  public PdfLoadedDocument(string filename, string password, bool openAndRepair)
    : this(PdfLoadedDocument.CreateStream(filename), password, openAndRepair)
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
    this.Password = password;
    this.m_fileName = filename;
  }

  internal PdfLoadedDocument(
    string filename,
    string password,
    bool openAndRepair,
    bool isXfaDocument)
    : this(PdfLoadedDocument.CreateStream(filename), password, openAndRepair, isXfaDocument)
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
    this.Password = password;
    this.m_fileName = filename;
  }

  public PdfLoadedDocument(byte[] file)
    : this(PdfLoadedDocument.CreateStream(file))
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
  }

  public PdfLoadedDocument(byte[] file, bool openAndRepair)
    : this(PdfLoadedDocument.CreateStream(file), openAndRepair)
  {
    this.isDispose = true;
    this.m_bCloseStream = true;
  }

  public PdfLoadedDocument(byte[] file, string password)
    : this(PdfLoadedDocument.CreateStream(file), password)
  {
    this.isDispose = true;
    this.Password = password;
    this.m_bCloseStream = true;
  }

  public PdfLoadedDocument(byte[] file, string password, bool openAndRepair)
    : this(PdfLoadedDocument.CreateStream(file), password, openAndRepair)
  {
    this.isDispose = true;
    this.Password = password;
    this.m_bCloseStream = true;
  }

  public PdfLoadedDocument(Stream file)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (file.Length == 0L)
      throw new PdfException("Contents of file stream is empty");
    if (file.Position != 0L)
      file.Position = 0L;
    this.LoadDocument(file);
  }

  public PdfLoadedDocument(Stream file, bool openAndRepair)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    Stream file1 = file.Length != 0L ? this.CheckIfValid(file) : throw new PdfException("Contents of file stream is empty");
    this.isOpenAndRepair = openAndRepair;
    if (file1.Position != 0L)
      file1.Position = 0L;
    this.LoadDocument(file1);
  }

  internal PdfLoadedDocument(Stream file, bool openAndRepair, bool isXfaDocument)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    Stream file1 = file.Length != 0L ? this.CheckIfValid(file) : throw new PdfException("Contents of file stream is empty");
    this.isOpenAndRepair = openAndRepair;
    this.m_isXfaDocument = isXfaDocument;
    if (file1.Position != 0L)
      file1.Position = 0L;
    this.LoadDocument(file1);
  }

  public PdfLoadedDocument(Stream file, string password)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (file.Length == 0L)
      throw new PdfException("Contents of file stream is empty");
    this.m_password = password != null ? password : throw new ArgumentNullException(nameof (password));
    this.LoadDocument(file);
  }

  public PdfLoadedDocument(Stream file, string password, bool openAndRepair)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (file.Length == 0L)
      throw new PdfException("Contents of file stream is empty");
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    this.isOpenAndRepair = openAndRepair;
    this.m_password = password;
    this.LoadDocument(file);
  }

  internal PdfLoadedDocument(Stream file, string password, bool openAndRepair, bool isXfaDocument)
  {
    if (file == null)
      throw new ArgumentNullException(nameof (file));
    if (file.Length == 0L)
      throw new PdfException("Contents of file stream is empty");
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    this.isOpenAndRepair = openAndRepair;
    this.m_isXfaDocument = isXfaDocument;
    this.m_password = password;
    this.LoadDocument(file);
  }

  ~PdfLoadedDocument() => this.Dispose(false);

  internal PdfLoadedDocument(Stream file, string password, out List<PdfException> exceptions)
  {
    try
    {
      this.validateSyntax = true;
      if (file == null)
        throw new ArgumentNullException(nameof (file));
      if (file.Length == 0L)
        throw new PdfException("Contents of file stream is empty");
      this.m_password = password;
      Stream file1 = this.CheckIfValid(file);
      if (file1.Position != 0L)
        file1.Position = 0L;
      this.LoadDocument(file1);
    }
    catch (Exception ex)
    {
      this.pdfException.Add(new PdfException(ex.Message));
    }
    exceptions = this.pdfException;
  }

  public event PdfLoadedDocument.OnPdfPasswordEventHandler OnPdfPassword;

  internal bool RaiseUserPassword => this.OnPdfPassword != null;

  internal void PdfUserPassword(OnPdfPasswordEventArgs args)
  {
    this.OnPdfPassword((object) this, args);
  }

  public event PdfLoadedDocument.RedactionProgressEventHandler RedactionProgress;

  internal bool RaiseTrackRedactionProgress => this.RedactionProgress != null;

  internal void OnTrackProgress(RedactionProgressEventArgs arguments)
  {
    this.RedactionProgress((object) this, arguments);
  }

  public event PdfLoadedDocument.PdfAConversionProgressEventHandler PdfAConversionProgress;

  internal bool RaiseTrackPdfAConversionProgress => this.PdfAConversionProgress != null;

  internal void OnPdfAConversionTrackProgress(PdfAConversionProgressEventArgs arguments)
  {
    this.PdfAConversionProgress((object) this, arguments);
  }

  private static Stream CreateStream(string filename)
  {
    if (filename == null)
      throw new ArgumentNullException(nameof (filename));
    if (!File.Exists(filename))
      throw new ArgumentException("File doesn't exist", nameof (filename));
    PdfLoadedDocument.m_isPathStream = true;
    FileInfo fileInfo = new FileInfo(filename);
    Stream stream;
    if ((fileInfo.Attributes & FileAttributes.ReadOnly) != (FileAttributes) 0)
    {
      stream = (Stream) fileInfo.OpenRead();
    }
    else
    {
      try
      {
        stream = (Stream) fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
      }
      catch (IOException ex)
      {
        stream = (Stream) fileInfo.OpenRead();
      }
      catch (SystemException ex)
      {
        stream = (Stream) fileInfo.OpenRead();
      }
    }
    stream.Position = 0L;
    return stream;
  }

  private static Stream CreateStream(byte[] file)
  {
    return file != null ? (Stream) new MemoryStream(file) : throw new ArgumentNullException(nameof (file));
  }

  public void Save()
  {
    this.m_stream.Position = 0L;
    if (this.Security.Enabled == this.m_bWasEncrypted && (!this.m_bWasEncrypted || !this.Security.Encryptor.Changed) && this.FileStructure.IncrementalUpdate && !this.isOpenAndRepair && this.m_stream.CanWrite)
    {
      MemoryStream memoryStream = new MemoryStream();
      this.Save((Stream) memoryStream);
      this.m_stream.Position = 0L;
      memoryStream.Position = 0L;
      byte[] buffer = new byte[8190];
      int count;
      while ((count = memoryStream.Read(buffer, 0, buffer.Length)) != 0)
        this.m_stream.Write(buffer, 0, count);
      memoryStream.Dispose();
    }
    else
    {
      this.m_internalStream = new MemoryStream();
      this.Save((Stream) this.m_internalStream);
      if (string.IsNullOrEmpty(this.m_fileName))
        return;
      using (FileStream fileStream = new FileStream(this.m_fileName, FileMode.Create, FileAccess.Write))
      {
        this.m_internalStream.Position = 0L;
        byte[] buffer = new byte[8190];
        int count;
        while ((count = this.m_internalStream.Read(buffer, 0, buffer.Length)) != 0)
          fileStream.Write(buffer, 0, count);
      }
    }
  }

  public ImageExportSettings ImageExportSettings
  {
    get
    {
      if (this.m_imageExportSetting == null)
        this.m_imageExportSetting = new ImageExportSettings();
      return this.m_imageExportSetting;
    }
  }

  public Bitmap ExportAsImage(int pageIndex)
  {
    this.feature = new PdfFeatures();
    return this.feature.ExportAsImage(this.Pages[pageIndex], this);
  }

  internal Metafile ExportAsMetafile(int pageIndex)
  {
    this.feature = new PdfFeatures();
    return this.feature.ExportAsMetafile(this.Pages[pageIndex]);
  }

  public Bitmap ExportAsImage(int pageIndex, float dpiX, float dpiY)
  {
    this.feature = new PdfFeatures();
    return this.feature.ExportAsImage(this.Pages[pageIndex], dpiX, dpiY, this);
  }

  public Bitmap ExportAsImage(int pageIndex, SizeF customSize, bool keepAspectRatio)
  {
    this.feature = new PdfFeatures();
    return this.feature.ExportAsImage(this.Pages[pageIndex], customSize, keepAspectRatio, this);
  }

  public Bitmap ExportAsImage(
    int pageIndex,
    SizeF customSize,
    float dpiX,
    float dpiY,
    bool keepAspectRatio)
  {
    this.feature = new PdfFeatures();
    return this.feature.ExportAsImage(this.Pages[pageIndex], customSize, dpiX, dpiY, keepAspectRatio, this);
  }

  public Bitmap ExportAsImage(int pageIndex, ImageExportSettings exportSettings)
  {
    this.feature = new PdfFeatures();
    this.isSystemFontavailable(exportSettings);
    return (double) exportSettings.DpiX <= 0.0 || (double) exportSettings.DpiY <= 0.0 || (double) exportSettings.CustomSize.Height <= 0.0 || (double) exportSettings.CustomSize.Width <= 0.0 ? ((double) exportSettings.DpiX <= 0.0 || (double) exportSettings.DpiY <= 0.0 ? ((double) exportSettings.CustomSize.Height <= 0.0 || (double) exportSettings.CustomSize.Width <= 0.0 ? this.feature.ExportAsImage(this.Pages[pageIndex], this) : this.feature.ExportAsImage(this.Pages[pageIndex], exportSettings.CustomSize, exportSettings.KeepAspectRatio, this)) : this.feature.ExportAsImage(this.Pages[pageIndex], exportSettings.DpiX, exportSettings.DpiY, this)) : this.feature.ExportAsImage(this.Pages[pageIndex], exportSettings.CustomSize, exportSettings.DpiX, exportSettings.DpiY, exportSettings.KeepAspectRatio, this);
  }

  public Bitmap[] ExportAsImage(int startIndex, int endIndex)
  {
    this.feature = new PdfFeatures();
    if (startIndex < 0 || endIndex >= this.PageCount)
      throw new ArgumentOutOfRangeException("Starting Index should be greater than Zero and less than total number of pages; ending index should be less than total number of pages");
    Bitmap[] bitmapArray = startIndex <= endIndex ? new Bitmap[endIndex - startIndex + 1] : throw new ArgumentException("Starting index should be less than ending index");
    int index1 = startIndex;
    int index2 = 0;
    while (index1 <= endIndex)
    {
      bitmapArray[index2] = this.feature.ExportAsImage(this.Pages[index1], this);
      ++index1;
      ++index2;
    }
    return bitmapArray;
  }

  public Bitmap[] ExportAsImage(int startIndex, int endIndex, float dpiX, float dpiY)
  {
    this.feature = new PdfFeatures();
    if (startIndex < 0 || endIndex >= this.PageCount)
      throw new ArgumentOutOfRangeException("Starting Index should be greater than Zero and less than total number of pages; ending index should be less than total number of pages");
    Bitmap[] bitmapArray = startIndex <= endIndex ? new Bitmap[endIndex - startIndex + 1] : throw new ArgumentException("Starting index should be less than ending index");
    int index1 = startIndex;
    int index2 = 0;
    while (index1 <= endIndex)
    {
      bitmapArray[index2] = this.feature.ExportAsImage(this.Pages[index1], dpiX, dpiY, this);
      ++index1;
      ++index2;
    }
    return bitmapArray;
  }

  public Bitmap[] ExportAsImage(
    int startIndex,
    int endIndex,
    SizeF customSize,
    bool keepAspectRatio)
  {
    this.feature = new PdfFeatures();
    if (startIndex < 0 || endIndex >= this.PageCount)
      throw new ArgumentOutOfRangeException("Starting Index should be greater than Zero and less than total number of pages; ending index should be less than total number of pages");
    Bitmap[] bitmapArray = startIndex <= endIndex ? new Bitmap[endIndex - startIndex + 1] : throw new ArgumentException("Starting index should be less than ending index");
    int index1 = startIndex;
    int index2 = 0;
    while (index1 <= endIndex)
    {
      bitmapArray[index2] = this.feature.ExportAsImage(this.Pages[index1], customSize, keepAspectRatio, this);
      ++index1;
      ++index2;
    }
    return bitmapArray;
  }

  public Bitmap[] ExportAsImage(
    int startIndex,
    int endIndex,
    SizeF customSize,
    float dpiX,
    float dpiY,
    bool keepAspectRatio)
  {
    this.feature = new PdfFeatures();
    if (startIndex < 0 || endIndex >= this.PageCount)
      throw new ArgumentOutOfRangeException("Starting Index should be greater than Zero and less than total number of pages; ending index should be less than total number of pages");
    Bitmap[] bitmapArray = startIndex <= endIndex ? new Bitmap[endIndex - startIndex + 1] : throw new ArgumentException("Starting index should be less than ending index");
    int index1 = startIndex;
    int index2 = 0;
    while (index1 <= endIndex)
    {
      bitmapArray[index2] = this.feature.ExportAsImage(this.Pages[index1], customSize, dpiX, dpiY, keepAspectRatio, this);
      ++index1;
      ++index2;
    }
    return bitmapArray;
  }

  public Bitmap[] ExportAsImage(int startIndex, int endIndex, ImageExportSettings exportSettings)
  {
    this.feature = new PdfFeatures();
    if (startIndex < 0 || endIndex >= this.PageCount)
      throw new ArgumentOutOfRangeException("Starting Index should be greater than Zero and less than total number of pages; ending index should be less than total number of pages");
    Bitmap[] bitmapArray = startIndex <= endIndex ? new Bitmap[endIndex - startIndex + 1] : throw new ArgumentException("Starting index should be less than ending index");
    this.isSystemFontavailable(exportSettings);
    int index1 = startIndex;
    int index2 = 0;
    while (index1 <= endIndex)
    {
      bitmapArray[index2] = (double) exportSettings.DpiX <= 0.0 || (double) exportSettings.DpiY <= 0.0 || (double) exportSettings.CustomSize.Height <= 0.0 || (double) exportSettings.CustomSize.Width <= 0.0 ? ((double) exportSettings.DpiX <= 0.0 || (double) exportSettings.DpiY <= 0.0 ? ((double) exportSettings.CustomSize.Height <= 0.0 || (double) exportSettings.CustomSize.Width <= 0.0 ? this.feature.ExportAsImage(this.Pages[index1], this) : this.feature.ExportAsImage(this.Pages[index1], exportSettings.CustomSize, exportSettings.KeepAspectRatio, this)) : this.feature.ExportAsImage(this.Pages[index1], exportSettings.DpiX, exportSettings.DpiY, this)) : this.feature.ExportAsImage(this.Pages[index1], exportSettings.CustomSize, exportSettings.DpiX, exportSettings.DpiY, exportSettings.KeepAspectRatio, this);
      ++index1;
      ++index2;
    }
    return bitmapArray;
  }

  public bool FindText(string text, int index, out List<RectangleF> matchRect)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindTextMatches(index, this, text, out matchRect);
  }

  public bool FindText(string text, out Dictionary<int, List<RectangleF>> matchRect)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindTextMatches(this, text, out matchRect);
  }

  public bool FindText(List<string> searchItems, out TextSearchResultCollection searchResult)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, TextSearchOptions.None, out searchResult, true);
  }

  public bool FindText(
    List<string> searchItems,
    int pageIndex,
    out List<MatchedItem> searchResults)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, pageIndex, TextSearchOptions.None, out searchResults);
  }

  public bool FindText(
    List<TextSearchItem> searchItems,
    out TextSearchResultCollection searchResult)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, out searchResult, true);
  }

  public bool FindText(
    List<TextSearchItem> searchItems,
    int pageIndex,
    out List<MatchedItem> searchResults)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, pageIndex, out searchResults);
  }

  public bool FindText(
    List<string> searchItems,
    TextSearchOptions textSearchOption,
    out TextSearchResultCollection searchResult)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, textSearchOption, out searchResult, true);
  }

  public bool FindText(
    List<string> searchItems,
    int pageIndex,
    TextSearchOptions textSearchOption,
    out List<MatchedItem> searchResults)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, pageIndex, textSearchOption, out searchResults);
  }

  public bool FindText(
    List<string> searchItems,
    out TextSearchResultCollection searchResult,
    bool enableMultiThreading)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, TextSearchOptions.None, out searchResult, enableMultiThreading);
  }

  public bool FindText(
    List<TextSearchItem> searchItems,
    out TextSearchResultCollection searchResult,
    bool enableMultiThreading)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, out searchResult, enableMultiThreading);
  }

  public bool FindText(
    List<string> searchItems,
    TextSearchOptions textSearchOption,
    out TextSearchResultCollection searchResult,
    bool enableMultiThreading)
  {
    this.feature = new PdfFeatures();
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    return this.feature.FindText(this, searchItems, textSearchOption, out searchResult, enableMultiThreading);
  }

  public List<PdfRedactionResult> Redact()
  {
    List<PdfRedactionResult> pdfRedactionResultList = new List<PdfRedactionResult>();
    foreach (PdfLoadedPage page in this.Pages)
    {
      foreach (PdfAnnotation annotation in (PdfCollection) page.Annotations)
      {
        if (annotation is PdfLoadedRedactionAnnotation)
          (annotation as PdfLoadedRedactionAnnotation).Flatten = true;
      }
    }
    for (int index1 = 0; index1 < this.m_redactionPages.Count; ++index1)
    {
      this.FileStructure.IncrementalUpdate = false;
      PdfLoadedPage redactionPage = this.m_redactionPages[index1];
      PdfTextParser pdfTextParser = new PdfTextParser(redactionPage);
      if (this.m_redactionPages.Count == 1 && this.RaiseTrackRedactionProgress)
        pdfTextParser.redactionTrackProcess = true;
      pdfTextParser.Process();
      for (int index2 = 0; index2 < redactionPage.Redactions.Count; ++index2)
      {
        PdfRedaction redaction = redactionPage.Redactions[index2];
        pdfRedactionResultList.Add(new PdfRedactionResult(this.Pages.IndexOf((PdfPageBase) redactionPage) + 1, redaction.Success, redaction.Bounds));
      }
      this.isCompressPdf = true;
      if (this.RaiseTrackRedactionProgress && this.m_redactionPages.Count > 1)
        this.OnTrackProgress(new RedactionProgressEventArgs()
        {
          m_progress = (float) (100 / this.m_redactionPages.Count * (index1 + 1))
        });
    }
    if (this.RaiseTrackRedactionProgress && this.m_redactionPages.Count != 0)
    {
      bool flag = 100 % this.m_redactionPages.Count == 0;
      if (this.m_redactionPages.Count != 0 && !flag)
        this.OnTrackProgress(new RedactionProgressEventArgs()
        {
          m_progress = 100f
        });
    }
    this.isRedacted = true;
    return pdfRedactionResultList;
  }

  public void ConvertToPDFA(PdfConformanceLevel conformanceLevel)
  {
    this.Conformance = conformanceLevel;
    if (!this.ConformanceEnabled)
      return;
    if (this.m_documentInfo == null)
      this.ReadDocumentInfo();
    PdfDocument.ConformanceLevel = this.m_previousConformance;
    new PdfA1BConverter() { PdfALevel = conformanceLevel }.Convert(this);
    this.isConformanceApplied = true;
  }

  private void isSystemFontavailable(ImageExportSettings exportSettings)
  {
    FontFamily[] families = new InstalledFontCollection().Families;
    int length1 = families.Length;
    List<string> stringList = new List<string>();
    for (int index = 0; index < length1; ++index)
    {
      string name = families[index].Name;
      stringList.Add(name);
    }
    bool flag1 = false;
    for (int index1 = 0; index1 < this.UsedFonts.Length; ++index1)
    {
      bool flag2 = false;
      string usedFont = this.UsedFonts[index1].Name;
      if (this.UsedFonts[index1].Style.ToString() != "Regular")
        usedFont = $"{usedFont} {(object) this.UsedFonts[index1].Style}";
      if (stringList.Contains(usedFont))
      {
        flag1 = true;
        break;
      }
      if (!flag2)
      {
        FontNotFoundEventArgs e = new FontNotFoundEventArgs(usedFont);
        exportSettings.OnFontNotFounded(e);
        string str1 = e.UsedFont.Replace(" MT", "").Replace("MT", "");
        if (str1.Contains("#20"))
          str1 = str1.Replace("#20", " ");
        string[] sourceArray = new string[1]{ "" };
        int length2 = 0;
        for (int startIndex = 0; startIndex < str1.Length; ++startIndex)
        {
          string str2 = str1.Substring(startIndex, 1);
          if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str2) && startIndex > 0 && !"ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".Contains(str1[startIndex - 1].ToString()))
          {
            ++length2;
            string[] destinationArray = new string[length2 + 1];
            System.Array.Copy((System.Array) sourceArray, 0, (System.Array) destinationArray, 0, length2);
            sourceArray = destinationArray;
          }
          string[] strArray;
          IntPtr index2;
          (strArray = sourceArray)[(int) (index2 = (IntPtr) length2)] = strArray[index2] + str2;
        }
        string str3 = string.Empty;
        foreach (string str4 in sourceArray)
        {
          string str5 = str4.Trim();
          str3 = $"{str3}{str5} ";
        }
        if (str3.Contains("Zapf"))
          str3 = "MS Gothic";
        else if (str3.Contains("Times"))
          str3 = "Times New Roman";
        else if (str3 == "Bookshelf Symbol Seven")
          str3 = "Bookshelf Symbol 7";
        else if (str3.Contains("Courier"))
          str3 = "Courier New";
        else if (str3.Contains("Free") && str3.Contains("9"))
          str3 = "Free 3 of 9";
        string key = str3.Trim();
        if (!this.currentFont.ContainsKey(key))
          this.currentFont.Add(key, e.AlternateFont);
      }
    }
  }

  public void Split(string destFilePattern) => this.Split(destFilePattern, 0);

  public void Split(string destFilePattern, int startNumber)
  {
    if (destFilePattern == null)
      throw new ArgumentNullException("destFileName");
    if (!new Regex("\\w*\\{0.*\\}\\w*", RegexOptions.None).Match(destFilePattern).Success)
    {
      int num = destFilePattern.LastIndexOf('.');
      destFilePattern = num >= 0 ? $"{destFilePattern.Substring(0, num)}{"{0}"}{destFilePattern.Substring(num, destFilePattern.Length - num)}" : $"{destFilePattern}{"{0}.pdf"}";
    }
    int pageIndex = 0;
    for (int count = this.Pages.Count; pageIndex < count; ++pageIndex)
    {
      PdfDocument pdfDocument = new PdfDocument();
      pdfDocument.EnableMemoryOptimization = true;
      pdfDocument.ImportPage(this, pageIndex);
      pdfDocument.Save(string.Format(destFilePattern, (object) (pageIndex + startNumber)));
      pdfDocument.Close(true);
    }
  }

  public void CreateForm()
  {
    if (this.m_form != null)
      return;
    this.m_form = new PdfLoadedForm(this.CrossTable);
    this.Catalog.SetProperty("AcroForm", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_form));
    this.Catalog.LoadedForm = this.m_form;
  }

  public PdfAttachmentCollection CreateAttachment()
  {
    this.m_attachments = new PdfAttachmentCollection();
    this.Catalog.CreateNamesIfNone();
    this.Catalog.Names.EmbeddedFiles = this.m_attachments;
    return this.m_attachments;
  }

  public PdfBookmarkBase CreateBookmarkRoot()
  {
    this.m_bookmark = new PdfBookmarkBase();
    this.Catalog.SetProperty("Outlines", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_bookmark));
    return this.m_bookmark;
  }

  private PdfNamedDestinationCollection CreateNamedDestinations()
  {
    this.m_namedDestinations = new PdfNamedDestinationCollection();
    if (this.Catalog.Names == null)
      this.Catalog.CreateNamesIfNone();
    PdfReferenceHolder pdfReferenceHolder = this.Catalog["Names"] as PdfReferenceHolder;
    if (pdfReferenceHolder != (PdfReferenceHolder) null)
    {
      if (pdfReferenceHolder.Object is PdfDictionary pdfDictionary)
        pdfDictionary.SetProperty("Dests", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_namedDestinations));
    }
    else
      this.Catalog.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_namedDestinations));
    this.Catalog.Modify();
    return this.m_namedDestinations;
  }

  private PdfPageTemplateCollection CreatePageTemplates()
  {
    this.m_pageTemplatesCollection = new PdfPageTemplateCollection();
    if (this.Catalog.Names == null)
      this.Catalog.CreateNamesIfNone();
    this.Catalog.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_pageTemplatesCollection));
    this.Catalog.Modify();
    return this.m_pageTemplatesCollection;
  }

  private string CheckLicense()
  {
    string empty = string.Empty;
    return FusionLicenseProvider.GetLicenseType(Platform.FileFormats);
  }

  public override void Save(Stream stream)
  {
    if (stream.CanSeek && stream.CanWrite && this.m_stream.CanWrite && this.m_stream.CanRead && stream == this.m_stream && stream.Length == this.m_stream.Length && PdfDocument.ConformanceLevel == PdfConformanceLevel.None)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.Save((Stream) memoryStream);
        stream.Position = 0L;
        memoryStream.Position = 0L;
        byte[] buffer = new byte[8190];
        int count;
        while ((count = memoryStream.Read(buffer, 0, buffer.Length)) != 0)
          stream.Write(buffer, 0, count);
      }
    }
    else
    {
      if (this.ConformanceEnabled && !this.isConformanceApplied)
      {
        if (this.m_documentInfo == null)
          this.ReadDocumentInfo();
        PdfDocument.ConformanceLevel = this.m_previousConformance;
        new PdfA1BConverter() { PdfALevel = this.Conformance }.Convert(this);
      }
      using (PdfWriter writer = new PdfWriter(stream))
      {
        writer.isCompress = this.isCompressPdf;
        if (this.m_isXfaDocument || this.IsXFAForm && this.Form.EnableXfaFormFill && this.Form != null && this.Form.LoadedXfa != null && this.Form.LoadedXfa.Fields != null)
          this.Form.LoadedXfa.Save(this);
        if (this.CompressionOptions != null && (this.CompressionOptions.CompressImages || this.CompressionOptions.OptimizeFont || this.CompressionOptions.OptimizePageContents || this.CompressionOptions.RemoveMetadata))
        {
          writer.isCompress = true;
          new PdfOptimizer(this).Close();
        }
        if (!string.IsNullOrEmpty(this.CheckLicense()))
        {
          lock (PdfDocumentBase.s_licenseLock)
            this.AddWaterMark();
        }
        if (!this.isRedacted)
        {
          for (int index = 0; index < this.m_redactionPages.Count; ++index)
          {
            this.FileStructure.IncrementalUpdate = false;
            PdfTextParser pdfTextParser = new PdfTextParser(this.m_redactionPages[index]);
            if (this.m_redactionPages.Count == 1 && this.RaiseTrackRedactionProgress)
              pdfTextParser.redactionTrackProcess = true;
            pdfTextParser.Process();
            writer.isCompress = true;
            if (this.RaiseTrackRedactionProgress && this.m_redactionPages.Count > 1)
              this.OnTrackProgress(new RedactionProgressEventArgs()
              {
                m_progress = (float) (100 / this.m_redactionPages.Count * (index + 1))
              });
          }
          if (this.RaiseTrackRedactionProgress && this.m_redactionPages.Count != 0)
          {
            bool flag = 100 % this.m_redactionPages.Count == 0;
            if (this.m_redactionPages.Count != 0 && !flag)
              this.OnTrackProgress(new RedactionProgressEventArgs()
              {
                m_progress = 100f
              });
          }
        }
        if (this.isRedacted)
          this.FileStructure.IncrementalUpdate = false;
        if (this.Security.EncryptOnlyAttachment && !this.isAttachmentOnlyEncryption && this.Security.UserPassword == string.Empty)
          throw new PdfException("User password cannot be empty for encrypt only attachment.");
        if (this.Security.m_modifiedSecurity && this.isAttachmentOnlyEncryption)
        {
          PdfEncryptor encryptor = this.Security.Encryptor;
          bool encryptOnlyAttachment = this.Security.EncryptOnlyAttachment;
          PdfEncryptionOptions encryptionOptions = this.Security.EncryptionOptions;
          string userPassword = this.Security.UserPassword;
          PdfAttachmentCollection attachments = this.Attachments;
          foreach (PdfLoadedPage page in this.Pages)
          {
            PdfLoadedAnnotationCollection annotations = page.Annotations;
          }
          if (this.Security.Encryptor.EncryptOnlyAttachment)
            this.FileStructure.IncrementalUpdate = false;
          this.Security.UserPassword = userPassword;
          this.Security.EncryptOnlyAttachment = encryptOnlyAttachment;
          this.Security.Encryptor = encryptor;
          this.Security.EncryptionOptions = encryptionOptions;
          if (this.Security.Encryptor.EncryptOnlyAttachment && this.Security.Encryptor.UserPassword.Length == 0)
            this.Security.Encryptor.EncryptOnlyAttachment = false;
        }
        bool flag1 = false;
        if (this.CrossTable.DocumentCatalog != null && this.CrossTable.DocumentCatalog.ContainsKey("Perms"))
        {
          PdfDictionary pdfDictionary = PdfCrossTable.Dereference(this.CrossTable.DocumentCatalog["Perms"]) as PdfDictionary;
          flag1 = pdfDictionary.ContainsKey("UR3");
          if (flag1 && this.Form != null && flag1 && (this.Form.Flatten || !this.Form.NeedAppearances && this.Form.IsModified && (this.Form.Fields != null && (this.Form.Fields.Count != 0 || !this.IsXFAForm) || this.Security.Enabled) && (this.Form.SignatureFlags == SignatureFlags.None || this.Form.SignatureFlags == SignatureFlags.AppendOnly) && this.Form.SignatureFlags != SignatureFlags.SignaturesExists))
          {
            pdfDictionary.Remove("UR3");
            flag1 = false;
          }
        }
        if (!this.Security.Enabled && this.CrossTable.Trailer.ContainsKey("Encrypt") && !this.Security.m_encryptOnlyAttachment)
          this.FileStructure.IncrementalUpdate = false;
        if (this.m_pageTemplatesCollection != null && this.m_pageTemplatesCollection.Count > 0)
        {
          Dictionary<PdfPageBase, PdfPageTemplate> dictionary = new Dictionary<PdfPageBase, PdfPageTemplate>();
          foreach (PdfPageTemplate pageTemplates in (IEnumerable) this.m_pageTemplatesCollection)
            dictionary[pageTemplates.PdfPageBase] = pageTemplates;
          this.m_pageTemplatesCollection.Clear();
          foreach (KeyValuePair<PdfPageBase, PdfPageTemplate> keyValuePair in dictionary)
            this.m_pageTemplatesCollection.Add(keyValuePair.Value);
          for (int index = 0; index < this.m_pageTemplatesCollection.Count; ++index)
          {
            PdfPageTemplate pageTemplates = this.m_pageTemplatesCollection[index];
            if (pageTemplates != null && this.PageCount > 1 && !pageTemplates.IsVisible)
              this.Pages.Remove(pageTemplates.PdfPageBase);
            else if (pageTemplates.IsVisible && this.Pages.GetIndex(pageTemplates.PdfPageBase) < 0 && !pageTemplates.PdfPageBase.m_removedPage)
              pageTemplates.PdfPageBase = this.Pages.Add(this, pageTemplates.PdfPageBase);
          }
          dictionary.Clear();
        }
        if (this.Security.Enabled == this.m_bWasEncrypted && this.Security.OwnerPassword.Length == 0 && this.Security.UserPassword.Length == 0 && this.FileStructure.IncrementalUpdate && !this.CrossTable.isOpenAndRepair && !this.Security.m_modifiedSecurity)
        {
          if (writer.Length > 0L)
          {
            if (!this.m_isImported)
              this.m_isStreamCopied = true;
            writer.Position = writer.Length;
            this.AppendDocument(writer);
          }
          else
          {
            this.CopyOldStream(writer);
            if (!this.m_isImported)
              this.m_isStreamCopied = true;
            if (this.Catalog.Changed)
              this.ReadDocumentInfo();
            if (!this.IsSkipSaving)
              this.AppendDocument(writer);
          }
        }
        else if (flag1 && !this.isCompressed)
        {
          this.CopyOldStream(writer);
          if (!this.m_isImported)
            this.m_isStreamCopied = true;
          if (!this.IsSkipSaving)
            this.AppendDocument(writer);
        }
        else
        {
          bool flag2 = false;
          if (this.FileStructure.Version <= PdfVersion.Version1_2)
          {
            this.FileStructure.Version = PdfVersion.Version1_4;
            flag2 = true;
          }
          if (this.Security.Enabled || !this.FileStructure.IncrementalUpdate)
            this.ReadDocumentInfo();
          PdfCrossTable crossTable = this.CrossTable;
          if (this.CrossTable.isOpenAndRepair)
            this.SetCrossTable(new PdfCrossTable(crossTable.Count, crossTable.EncryptorDictionary, crossTable.DocumentCatalog, crossTable.CrossTable));
          else if (!flag2 && this.FileStructure.IncrementalUpdate && !this.Security.m_modifiedSecurity)
          {
            this.CopyOldStream(writer);
            if (!this.m_isImported)
              this.m_isStreamCopied = true;
          }
          else
            this.SetCrossTable(new PdfCrossTable(crossTable.Count, crossTable.EncryptorDictionary, crossTable.DocumentCatalog));
          this.CrossTable.Document = (PdfDocumentBase) this;
          if (this.DocumentInformation != null)
            this.CrossTable.Trailer["Info"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.DocumentInformation);
          this.AppendDocument(writer);
          if (this.FileStructure.Version <= PdfVersion.Version1_3)
          {
            if (!this.Security.Enabled)
            {
              if (this.Form == null)
              {
                if (this.IsOcredDocument)
                {
                  if (!this.isCompressed)
                  {
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument.ImportPageRange(this, 0, this.Pages.Count - 1);
                    if (this.Catalog.Changed)
                      pdfDocument.Save(stream);
                    pdfDocument.Close(true);
                  }
                }
              }
            }
          }
        }
      }
      if (this.ConformanceEnabled)
        PdfDocument.ConformanceLevel = PdfConformanceLevel.None;
      if (this.m_redactionPages == null || this.m_redactionPages.Count <= 0)
        return;
      for (int index = 0; index < this.m_redactionPages.Count; ++index)
        this.m_redactionPages[index].Redactions.Clear();
    }
  }

  private void AddWaterMark()
  {
    if (this.Pages == null || this.Pages.Count <= 0 || this.Pages[0] == null)
      return;
    PdfGraphics graphics = this.Pages[0].Graphics;
    graphics.Save();
    graphics.SetTransparency(0.2f);
    graphics.DrawRectangle(PdfBrushes.White, new RectangleF(0.0f, 0.0f, this.Pages[0].Size.Width, 33.75f));
    graphics.Restore();
    graphics.Save();
    graphics.SetTransparency(0.5f);
    PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12f, PdfFontStyle.Regular, true);
    graphics.DrawString("Created with a trial version of Syncfusion Essential PDF", (PdfFont) font, PdfBrushes.Black, new RectangleF(0.0f, 0.0f, this.Pages[0].Size.Width, 33.75f), new PdfStringFormat()
    {
      LineAlignment = PdfVerticalAlignment.Middle,
      Alignment = PdfTextAlignment.Center
    });
    graphics.Restore();
  }

  private List<PdfUsedFont> ExtractFonts()
  {
    List<PdfUsedFont> fonts = new List<PdfUsedFont>();
    PdfLoadedPageCollection pages = this.Pages;
    ArrayList arrayList = new ArrayList();
    foreach (PdfLoadedPage page in pages)
    {
      foreach (PdfFont font in page.ExtractFonts())
        fonts.Add(new PdfUsedFont(font, page));
    }
    PdfFont[] pdfFontArray = new PdfFont[arrayList.Count];
    int num = 0;
    foreach (PdfFont pdfFont in arrayList)
      pdfFontArray[num++] = pdfFont;
    return fonts;
  }

  internal override void AddFields(
    PdfLoadedDocument ldDoc,
    PdfPageBase newPage,
    List<PdfField> fields)
  {
    if (fields.Count > 0 && this.Form == null)
      this.CreateForm();
    this.Form.Fields.m_isImported = true;
    int index = 0;
    for (int count = fields.Count; index < count; ++index)
      this.Form.Fields.Add(fields[index], newPage);
  }

  internal override PdfPageBase ClonePage(
    PdfLoadedDocument ldDoc,
    PdfPageBase page,
    List<PdfArray> destinations)
  {
    return this.Pages.Add(ldDoc, page, destinations);
  }

  public override PdfDocumentInformation DocumentInformation
  {
    get
    {
      if (this.m_documentInfo == null)
      {
        this.m_documentInfo = !(PdfCrossTable.Dereference(this.CrossTable.Trailer["Info"]) is PdfDictionary dictionary) ? base.DocumentInformation : new PdfDocumentInformation(dictionary, this.Catalog);
        this.ReadDocumentInfo();
      }
      return this.m_documentInfo;
    }
  }

  internal override PdfForm ObtainForm()
  {
    if (this.Form == null)
      this.CreateForm();
    return (PdfForm) this.Form;
  }

  public void Dispose()
  {
    if (this.EnableMemoryOptimization)
      this.Close(true);
    else
      this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private void Dispose(bool dispose)
  {
    if (this.m_isDisposed)
      return;
    this.m_isDisposed = true;
    if (dispose && this.EnableMemoryOptimization)
    {
      if (this.m_bookmark != null)
        this.m_bookmark.Dispose();
      if (this.m_bookmarkHashtable != null)
        this.m_bookmarkHashtable.Clear();
      this.m_documentInfo = (PdfDocumentInformation) null;
      this.m_form = (PdfLoadedForm) null;
      this.m_internalStream = (MemoryStream) null;
      PdfLoadedDocument.m_openStream = (Stream) null;
      this.m_pageLabel = (PdfPageLabel) null;
      this.m_pageLabelCollection = (PdfLoadedPageLabelCollection) null;
      this.m_redactionPages.Clear();
      if (this.m_stream != null && this.isDispose)
      {
        this.m_stream.Dispose();
        this.m_stream.Close();
      }
      if (this.m_internalStream != null)
        this.m_internalStream.Dispose();
      this.m_dublinschema = (DublinCoreSchema) null;
      if (this.m_usedFonts != null)
        this.m_usedFonts.Clear();
    }
    if (this.m_stream != null && this.isDispose)
    {
      this.m_stream.Dispose();
      this.m_stream.Close();
    }
    if (this.m_internalStream != null)
      this.m_internalStream.Dispose();
    this.m_stream = (Stream) null;
    this.m_pages = (PdfLoadedPageCollection) null;
    this.m_form = (PdfLoadedForm) null;
    this.m_bookmark = (PdfBookmarkBase) null;
  }

  public override void Close(bool completely)
  {
    if (completely && !PdfDocument.EnableCache)
      this.isCompressed = true;
    if (completely && this.m_pages != null && this.EnableMemoryOptimization)
      this.m_pages.Clear(this.isCompressed);
    else if (this.isCompressed && this.m_pages != null)
      this.m_pages.Clear(this.isCompressed);
    if (this.m_pages != null && this.m_pages.m_pageIndexCollection != null && this.m_pages.m_pageIndexCollection.Count > 0)
      this.m_pages.m_pageIndexCollection.Clear();
    if (this.m_pageTemplatesCollection != null)
      this.m_pageTemplatesCollection.Clear();
    base.Close(completely);
    if (this.EnableMemoryOptimization || this.isCompressed)
      this.Dispose(completely);
    else
      this.Dispose();
    this.ImageCollection.Clear();
  }

  public object Clone() => this.MemberwiseClone();

  internal PdfConformanceLevel GetDocumentConformance(PdfConformanceLevel m_conformance)
  {
    if (PdfCrossTable.Dereference(this.Catalog["OutputIntents"]) is PdfArray pdfArray)
    {
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary)
        {
          PdfName pdfName = PdfCrossTable.Dereference(pdfDictionary["S"]) as PdfName;
          if (pdfName.Value == "GTS_PDFA1")
          {
            m_conformance = PdfConformanceLevel.Pdf_A1B;
            break;
          }
          if (pdfName.Value == "GTS_PDFX" && this.DocumentInformation.Dictionary.ContainsKey("GTS_PDFXConformance") && (PdfCrossTable.Dereference(this.DocumentInformation.Dictionary["GTS_PDFXConformance"]) as PdfString).Value == "PDF/X-1a:2001")
          {
            m_conformance = PdfConformanceLevel.Pdf_X1A2001;
            break;
          }
        }
      }
    }
    if (pdfArray == null | m_conformance == PdfConformanceLevel.Pdf_A1B)
    {
      string name1 = "pdfaid:part";
      string name2 = "pdfaid:conformance";
      this.DocumentInformation.isConformanceCheck = true;
      XmlElement xmlElement = (XmlElement) null;
      if (this.DocumentInformation.XmpMetadata.XmlData.DocumentElement.Name == "x:xmpmeta")
        xmlElement = this.DocumentInformation.XmpMetadata.Xmpmeta;
      this.DocumentInformation.isConformanceCheck = false;
      bool found = false;
      if (xmlElement != null)
      {
        foreach (XmlNode childNode1 in xmlElement.ChildNodes)
        {
          foreach (XmlNode childNode2 in childNode1.ChildNodes)
          {
            XmlAttribute attribute1 = childNode2.Attributes[name1];
            XmlAttribute attribute2 = childNode2.Attributes[name2];
            if (attribute1 != null && attribute2 != null)
              m_conformance = this.GetConformanceLevel(attribute1.Value + attribute2.Value, out found);
            if (childNode2.InnerXml.Contains("pdfaid") && childNode2[name1] != null && childNode2[name1].InnerText != null && childNode2[name2] != null && childNode2[name2].InnerText != null)
            {
              m_conformance = this.GetConformanceLevel(childNode2[name1].InnerText + childNode2[name2].InnerText, out found);
              break;
            }
          }
          if (found)
            break;
        }
      }
      if (!found)
        m_conformance = PdfConformanceLevel.None;
    }
    return m_conformance;
  }

  private PdfConformanceLevel GetConformanceLevel(string conformanceValue, out bool found)
  {
    PdfConformanceLevel conformanceLevel = PdfConformanceLevel.None;
    found = false;
    switch (conformanceValue)
    {
      case "1A":
        conformanceLevel = PdfConformanceLevel.Pdf_A1A;
        found = true;
        break;
      case "1B":
        conformanceLevel = PdfConformanceLevel.Pdf_A1B;
        found = true;
        break;
      case "2A":
        conformanceLevel = PdfConformanceLevel.Pdf_A2A;
        found = true;
        break;
      case "2B":
        conformanceLevel = PdfConformanceLevel.Pdf_A2B;
        found = true;
        break;
      case "2U":
        conformanceLevel = PdfConformanceLevel.Pdf_A2U;
        found = true;
        break;
      case "3A":
        conformanceLevel = PdfConformanceLevel.Pdf_A3A;
        found = true;
        break;
      case "3B":
        conformanceLevel = PdfConformanceLevel.Pdf_A3B;
        found = true;
        break;
      case "3U":
        conformanceLevel = PdfConformanceLevel.Pdf_A3U;
        found = true;
        break;
    }
    return conformanceLevel;
  }

  internal void PageLabel()
  {
    if (!(this.Catalog["PageLabels"] is PdfDictionary pdfDictionary1))
    {
      pdfDictionary1 = new PdfDictionary();
      this.Catalog["PageLabels"] = (IPdfPrimitive) pdfDictionary1;
    }
    PdfArray pdfArray1 = new PdfArray();
    pdfDictionary1["Nums"] = (IPdfPrimitive) pdfArray1;
    PdfArray pdfArray2 = (this.CrossTable.GetObject(this.Catalog["Pages"]) as PdfDictionary)["Kids"] as PdfArray;
    Dictionary<int, PdfPageLabel> dictionary = new Dictionary<int, PdfPageLabel>();
    List<int> intList = new List<int>();
    for (int index = 0; index < this.m_pageLabelCollection.Count; ++index)
    {
      PdfPageLabel pageLabel = this.m_pageLabelCollection[index];
      if (pageLabel.StartPageIndex != -1 && !dictionary.ContainsKey(pageLabel.StartPageIndex))
      {
        intList.Add(pageLabel.StartPageIndex);
        dictionary.Add(pageLabel.StartPageIndex, pageLabel);
      }
    }
    if (intList.Count > 0)
    {
      intList.Sort();
      if (!intList.Contains(0))
      {
        PdfPageLabel pdfPageLabel = new PdfPageLabel();
        pdfPageLabel.StartNumber = 1;
        pdfPageLabel.StartPageIndex = 0;
        pdfArray1.Add((IPdfPrimitive) new PdfNumber(0));
        pdfArray1.Add(((IPdfWrapper) pdfPageLabel).Element);
      }
      for (int index = 0; index < dictionary.Count; ++index)
      {
        PdfPageLabel pdfPageLabel = dictionary[intList[index]];
        pdfArray1.Add((IPdfPrimitive) new PdfNumber(pdfPageLabel.StartPageIndex));
        pdfArray1.Add(((IPdfWrapper) pdfPageLabel).Element);
      }
      intList.Clear();
      dictionary.Clear();
    }
    else
    {
      int num = 0;
      for (int index = 0; index < pdfArray2.Count; ++index)
      {
        if (index < this.m_pageLabelCollection.Count)
        {
          PdfPageLabel pdfPageLabel = this.m_pageLabelCollection[index] ?? new PdfPageLabel();
          pdfArray1.Add((IPdfPrimitive) new PdfNumber(num));
          if (PdfCrossTable.Dereference(pdfArray2[index]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Kids") && PdfCrossTable.Dereference(pdfDictionary2["Kids"]) is PdfArray pdfArray3)
            num += pdfArray3.Count;
          pdfArray1.Add(((IPdfWrapper) pdfPageLabel).Element);
        }
      }
    }
  }

  internal override bool IsPdfViewerDocumentDisable
  {
    get => this.m_isPdfViewerDocumentDisable;
    set => this.m_isPdfViewerDocumentDisable = value;
  }

  internal override bool WasEncrypted => this.m_bWasEncrypted;

  public bool IsEncrypted => this.m_bWasEncrypted;

  internal Dictionary<PdfPageBase, object> CreateBookmarkDestinationDictionary()
  {
    PdfBookmarkBase bookmarks = this.Bookmarks;
    if (this.m_bookmarkHashtable == null && bookmarks != null)
    {
      this.m_bookmarkHashtable = new Dictionary<PdfPageBase, object>();
      Stack<PdfLoadedDocument.CurrentNodeInfo> currentNodeInfoStack = new Stack<PdfLoadedDocument.CurrentNodeInfo>();
      PdfLoadedDocument.CurrentNodeInfo currentNodeInfo = new PdfLoadedDocument.CurrentNodeInfo(bookmarks.List);
      do
      {
        while (currentNodeInfo.Index < currentNodeInfo.Kids.Count)
        {
          PdfBookmarkBase kid = currentNodeInfo.Kids[currentNodeInfo.Index];
          PdfNamedDestination namedDestination = (kid as PdfBookmark).NamedDestination;
          if (namedDestination != null)
          {
            if (namedDestination.Destination != null)
            {
              PdfPageBase page = namedDestination.Destination.Page;
              List<object> objectList = this.m_bookmarkHashtable.ContainsKey(page) ? this.m_bookmarkHashtable[page] as List<object> : (List<object>) null;
              if (objectList == null)
              {
                objectList = new List<object>();
                this.m_bookmarkHashtable[page] = (object) objectList;
              }
              objectList.Add((object) kid);
            }
          }
          else
          {
            PdfDestination destination = (kid as PdfBookmark).Destination;
            if (destination != null)
            {
              PdfPageBase page = destination.Page;
              List<object> objectList = this.m_bookmarkHashtable.ContainsKey(page) ? this.m_bookmarkHashtable[page] as List<object> : (List<object>) null;
              if (objectList == null)
              {
                objectList = new List<object>();
                this.m_bookmarkHashtable[page] = (object) objectList;
              }
              objectList.Add((object) kid);
            }
          }
          ++currentNodeInfo.Index;
          if (kid.Count > 0)
          {
            currentNodeInfoStack.Push(currentNodeInfo);
            currentNodeInfo = new PdfLoadedDocument.CurrentNodeInfo(kid.List);
          }
        }
        if (currentNodeInfoStack.Count > 0)
        {
          currentNodeInfo = currentNodeInfoStack.Pop();
          while (currentNodeInfo.Index == currentNodeInfo.Kids.Count && currentNodeInfoStack.Count > 0)
            currentNodeInfo = currentNodeInfoStack.Pop();
        }
      }
      while (currentNodeInfo.Index < currentNodeInfo.Kids.Count);
    }
    return this.m_bookmarkHashtable;
  }

  internal PdfArray GetNamedDestination(PdfName name)
  {
    PdfDictionary destinations = this.Catalog.Destinations;
    PdfArray namedDestination = (PdfArray) null;
    if (destinations != null)
      namedDestination = PdfLoadedDocument.ExtractDestination(destinations[name]);
    return namedDestination;
  }

  internal PdfArray GetNamedDestination(PdfString name)
  {
    PdfCatalogNames names = this.Catalog.Names;
    PdfArray namedDestination = (PdfArray) null;
    if (name != null && this.Catalog.Names != null)
    {
      PdfDictionary destinations = names.Destinations;
      namedDestination = PdfLoadedDocument.ExtractDestination(names.GetNamedObjectFromTree(destinations, name));
    }
    return namedDestination;
  }

  private static PdfArray ExtractDestination(IPdfPrimitive obj)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (obj is PdfDictionary)
      pdfDictionary = obj as PdfDictionary;
    else if ((object) (obj as PdfReferenceHolder) != null)
    {
      PdfReferenceHolder pdfReferenceHolder = obj as PdfReferenceHolder;
      if (pdfReferenceHolder.Object is PdfDictionary)
        pdfDictionary = pdfReferenceHolder.Object as PdfDictionary;
      else if (pdfReferenceHolder.Object is PdfArray)
        obj = (IPdfPrimitive) (pdfReferenceHolder.Object as PdfArray);
    }
    PdfArray destination = obj as PdfArray;
    if (pdfDictionary != null)
    {
      obj = PdfCrossTable.Dereference(pdfDictionary["D"]);
      destination = obj as PdfArray;
    }
    return destination;
  }

  private void LoadDocument(Stream file)
  {
    if (!file.CanRead || !file.CanSeek)
      throw new ArgumentException("Can't use the specified stream.", nameof (file));
    this.m_stream = file;
    this.SetMainObjectCollection(new PdfMainObjectCollection());
    try
    {
      PdfCrossTable cTable = !this.validateSyntax ? (!this.m_isOpenAndRepair ? (this.m_isOpenAndRepair || !this.isOpenAndRepair ? new PdfCrossTable(file) : new PdfCrossTable(file, this.m_isOpenAndRepair, true)) : new PdfCrossTable(file, this.isOpenAndRepair)) : new PdfCrossTable(file, this);
      cTable.Document = (PdfDocumentBase) this;
      if (cTable.StructureAltered)
        cTable.Document.FileStructure.IncrementalUpdate = false;
      this.SetCrossTable(cTable);
      this.m_bWasEncrypted = this.CheckEncryption(false);
      PdfCatalog catalogValue = this.GetCatalogValue();
      if (catalogValue != null && catalogValue.ContainsKey("Pages") && !catalogValue.ContainsKey("Type"))
        catalogValue.Items.Add(new PdfName("Type"), (IPdfPrimitive) new PdfName("Catalog"));
      if (!catalogValue.ContainsKey("Type"))
        throw new PdfException("Cannot find the Pdf catalog information");
      if (!(catalogValue["Type"] as PdfName).Value.Contains("Catalog"))
        catalogValue["Type"] = (IPdfPrimitive) new PdfName("Catalog");
      if (!(catalogValue["Type"] as PdfName).Value.Contains("Catalog"))
        throw new PdfException("Cannot find the Pdf catalog information");
      if (catalogValue.ContainsKey("Perms"))
      {
        this.m_isExtendedFeatureEnabled = true;
        if (catalogValue["Perms"] is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("UR3"))
        {
          if (!(pdfDictionary1["UR3"] is PdfDictionary pdfDictionary) && (object) (pdfDictionary1["UR3"] as PdfReferenceHolder) != null)
            pdfDictionary = (pdfDictionary1["UR3"] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary != null && pdfDictionary.ContainsKey("Contents") && !(pdfDictionary["Contents"] as PdfString).m_isHexString)
            throw new PdfException("Cannot load the pdf file unexpected hex string");
        }
      }
      this.SetCatalog(catalogValue);
      bool flag = false;
      if (catalogValue.ContainsKey("Version"))
      {
        PdfName pdfName = catalogValue["Version"] as PdfName;
        if (pdfName != (PdfName) null)
        {
          this.SetFileVersion("PDF-" + pdfName.Value);
          flag = true;
        }
      }
      if (!flag)
        this.ReadFileVersion();
      this.CheckIfTagged();
    }
    catch (Exception ex)
    {
      if (this.isOpenAndRepair && !this.m_isOpenAndRepair)
      {
        this.m_isOpenAndRepair = true;
        this.LoadDocument(file);
      }
      else
      {
        if (this.m_stream != null && PdfLoadedDocument.m_isPathStream)
        {
          PdfLoadedDocument.m_isPathStream = false;
          this.m_stream.Close();
        }
        if (ex is PdfInvalidPasswordException)
          throw new PdfInvalidPasswordException(ex.Message);
        if (ex is PdfDocumentException)
          throw new PdfDocumentException(ex.Message);
        if (!this.validateSyntax)
          throw new PdfException(ex.Message);
        this.pdfException.Add(new PdfException(ex.Message));
      }
    }
  }

  private void CheckIfTagged()
  {
    if (!(this.CrossTable.DocumentCatalog["MarkInfo"] is PdfDictionary pdfDictionary) || !pdfDictionary.ContainsKey("Marked"))
      return;
    this.FileStructure.TaggedPdf = (pdfDictionary["Marked"] as PdfBoolean).Value;
  }

  private void ReadFileVersion()
  {
    PdfReader pdfReader = new PdfReader(this.m_stream);
    pdfReader.Position = 0L;
    if (!pdfReader.GetNextToken().StartsWith("%"))
      return;
    string nextToken = pdfReader.GetNextToken();
    if (nextToken == null)
      return;
    this.SetFileVersion(nextToken);
  }

  private void SetFileVersion(string token)
  {
    switch (token)
    {
      case "PDF-1.4":
        this.FileStructure.Version = PdfVersion.Version1_4;
        break;
      case "PDF-1.0":
        this.FileStructure.Version = PdfVersion.Version1_0;
        this.FileStructure.IncrementalUpdate = false;
        break;
      case "PDF-1.1":
        this.FileStructure.Version = PdfVersion.Version1_1;
        this.FileStructure.IncrementalUpdate = false;
        break;
      case "PDF-1.2":
        this.FileStructure.Version = PdfVersion.Version1_2;
        this.FileStructure.IncrementalUpdate = false;
        break;
      case "PDF-1.3":
        this.FileStructure.Version = PdfVersion.Version1_3;
        this.FileStructure.IncrementalUpdate = false;
        break;
      case "PDF-1.5":
        this.FileStructure.Version = PdfVersion.Version1_5;
        break;
      case "PDF-1.6":
        this.FileStructure.Version = PdfVersion.Version1_6;
        break;
      case "PDF-1.7":
        this.FileStructure.Version = PdfVersion.Version1_7;
        break;
      case "PDF-2.0":
        this.FileStructure.Version = PdfVersion.Version2_0;
        break;
    }
  }

  private PdfCatalog GetCatalogValue()
  {
    PdfCatalog newObj = new PdfCatalog(this, this.CrossTable.DocumentCatalog);
    this.PdfObjects.ReregisterReference((IPdfPrimitive) this.CrossTable.DocumentCatalog, (IPdfPrimitive) newObj);
    if (!this.CrossTable.IsMerging)
      newObj.Position = -1;
    PdfDictionary dictionary = (PdfDictionary) newObj;
    if (dictionary != null)
      this.CheckNeedAppearence(dictionary);
    return newObj;
  }

  private void CheckNeedAppearence(PdfDictionary dictionary)
  {
    if (!dictionary.ContainsKey("AcroForm"))
      return;
    if ((object) (dictionary["AcroForm"] as PdfReferenceHolder) != null)
    {
      if ((dictionary["AcroForm"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("XFA"))
      {
        this.IsXFAForm = true;
      }
      else
      {
        if (pdfDictionary1 == null || !pdfDictionary1.ContainsKey("NeedAppearances"))
          return;
        this.IsXFAForm = false;
      }
    }
    else
    {
      if (!(dictionary["AcroForm"] is PdfDictionary))
        return;
      PdfDictionary pdfDictionary2 = dictionary["AcroForm"] as PdfDictionary;
      if (pdfDictionary2.ContainsKey("XFA"))
      {
        this.IsXFAForm = true;
      }
      else
      {
        if (!pdfDictionary2.ContainsKey("NeedAppearances"))
          return;
        this.IsXFAForm = false;
      }
    }
  }

  internal void ReadDocumentInfo()
  {
    if (PdfCrossTable.Dereference(this.CrossTable.Trailer["Info"]) is PdfDictionary pdfDictionary && this.m_documentInfo == null)
      this.m_documentInfo = new PdfDocumentInformation(pdfDictionary, this.Catalog);
    PdfReference infoReference = (PdfReference) null;
    int conformance = (int) this.Conformance;
    if (pdfDictionary != null && this.Catalog.Metadata != null)
    {
      bool changed = pdfDictionary.Changed;
      bool flag = false;
      XmpMetadata metadata = this.Catalog.Metadata;
      if (this.m_bWasEncrypted)
      {
        if ((object) (this.CrossTable.Trailer["Info"] as PdfReferenceHolder) != null)
          infoReference = (this.CrossTable.Trailer["Info"] as PdfReferenceHolder).Reference;
        if (metadata.BasicSchema != null && !string.IsNullOrEmpty(metadata.BasicSchema.Label))
          this.m_documentInfo.Label = metadata.BasicSchema.Label;
        if (metadata.m_hasAttributes)
        {
          this.DecryptDocumentInfo(pdfDictionary, infoReference);
          if (pdfDictionary != null)
          {
            if (pdfDictionary.ContainsKey("Producer"))
              this.m_documentInfo.Producer = (PdfCrossTable.Dereference(pdfDictionary["Producer"]) as PdfString).Value;
            if (pdfDictionary.ContainsKey("Subject"))
              this.m_documentInfo.Subject = (PdfCrossTable.Dereference(pdfDictionary["Subject"]) as PdfString).Value;
            if (pdfDictionary.ContainsKey("Title"))
              this.m_documentInfo.Title = (PdfCrossTable.Dereference(pdfDictionary["Title"]) as PdfString).Value;
            if (pdfDictionary.ContainsKey("Keywords"))
              this.m_documentInfo.Keywords = (PdfCrossTable.Dereference(pdfDictionary["Keywords"]) as PdfString).Value;
            if (pdfDictionary.ContainsKey("Author"))
              this.m_documentInfo.Author = (PdfCrossTable.Dereference(pdfDictionary["Author"]) as PdfString).Value;
          }
        }
        else
        {
          this.DecryptDocumentInfo(pdfDictionary, infoReference);
          if (pdfDictionary.ContainsKey("Producer") && metadata.PDFSchema != null && metadata.PDFSchema.Producer != string.Empty && metadata.PDFSchema.Producer != (PdfCrossTable.Dereference(pdfDictionary["Producer"]) as PdfString).Value)
            pdfDictionary["Producer"] = (IPdfPrimitive) new PdfString(metadata.PDFSchema.Producer);
          if (pdfDictionary.ContainsKey("Author") && PdfCrossTable.Dereference(pdfDictionary["Author"]) is PdfString pdfString1 && this.DublinSchema != null && metadata.DublinCoreSchema != null && this.DublinSchema.Creator.Items != null && this.DublinSchema.Creator.Items.Length > 0 && this.DublinSchema.Creator.Items[0] != string.Empty && metadata.DublinCoreSchema.Creator.Items[0] != pdfString1.Value)
            pdfDictionary["Author"] = (IPdfPrimitive) new PdfString(this.DublinSchema.Creator.Items[0]);
          if (metadata.XmlData.InnerText.Contains("Title") && pdfDictionary.ContainsKey("Title") && PdfCrossTable.Dereference(pdfDictionary["Title"]) is PdfString pdfString2 && this.DublinSchema != null && metadata.DublinCoreSchema != null && this.DublinSchema.Title.DefaultText != string.Empty && metadata.DublinCoreSchema.Title.DefaultText != pdfString2.Value)
            pdfDictionary["Title"] = (IPdfPrimitive) new PdfString(this.DublinSchema.Title.DefaultText);
        }
      }
      if (pdfDictionary.ContainsKey("Creator"))
      {
        if (this.m_bWasEncrypted || this.Catalog.Changed)
        {
          if (metadata.m_hasAttributes)
          {
            PdfReferenceHolder pdfReferenceHolder = pdfDictionary["Creator"] as PdfReferenceHolder;
            this.m_documentInfo.Creator = !(pdfReferenceHolder != (PdfReferenceHolder) null) ? (pdfDictionary["Creator"] as PdfString).Value : (pdfReferenceHolder.Object as PdfString).Value;
          }
          else if (!changed)
          {
            string str = (string) null;
            PdfReferenceHolder pdfReferenceHolder = pdfDictionary["Creator"] as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
            {
              if (pdfReferenceHolder.Object is PdfString pdfString3)
                str = pdfString3.Value;
            }
            else if (pdfDictionary["Creator"] is PdfString pdfString4)
              str = pdfString4.Value;
            if (metadata.BasicSchema != null && metadata.BasicSchema.CreatorTool != string.Empty && metadata.BasicSchema.CreatorTool != str)
              pdfDictionary["Creator"] = (IPdfPrimitive) new PdfString(metadata.BasicSchema.CreatorTool);
          }
        }
        else
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary["Creator"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null)
          {
            if (pdfReferenceHolder.Object is PdfString pdfString5 && !string.IsNullOrEmpty(pdfString5.Value) && metadata.BasicSchema != null)
              metadata.BasicSchema.CreatorTool = pdfString5.Value;
          }
          else if (pdfDictionary["Creator"] is PdfString pdfString6 && !string.IsNullOrEmpty(pdfString6.Value) && metadata.BasicSchema != null)
            metadata.BasicSchema.CreatorTool = pdfString6.Value;
        }
      }
      if (pdfDictionary.ContainsKey("CreationDate") && metadata.BasicSchema != null)
      {
        if (this.m_bWasEncrypted || this.Catalog.Changed)
        {
          if (!changed)
          {
            XmpMetadata xmpMetadata = metadata;
            if (this.m_bWasEncrypted && infoReference != (PdfReference) null && pdfDictionary.ContainsKey("CreationDate"))
              (PdfCrossTable.Dereference(pdfDictionary["CreationDate"]) as PdfString).Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
            if (PdfCrossTable.Dereference(pdfDictionary["CreationDate"]) is PdfString dateTimeStringValue)
            {
              string s = dateTimeStringValue.Value;
              DateTime result = DateTime.Now;
              string str1 = "yyyyMMddHHmmss";
              if (s.Length > str1.Length)
                result = new PdfDictionary().GetDateTime(dateTimeStringValue);
              else
                DateTime.TryParseExact(s, "yyyyMMddHHmmss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite, out result);
              string str2 = xmpMetadata.BasicSchema.CreateDate.ToString("yyyyMMddHHmmsstt");
              if (metadata.BasicSchema.m_externalCreationDate && str2 != result.ToString("yyyyMMddHHmmsstt"))
                pdfDictionary["CreationDate"] = (IPdfPrimitive) new PdfString(metadata.BasicSchema.CreateDate.ToString("yyyyMMddHHmmss"));
              else if (metadata.BasicSchema != null && !this.ConformanceEnabled)
                metadata.BasicSchema.CreateDate = result;
            }
          }
        }
        else
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary["CreationDate"] as PdfReferenceHolder;
          string s = "";
          if (pdfReferenceHolder != (PdfReferenceHolder) null)
          {
            if (pdfReferenceHolder.Object is PdfString dateTimeStringValue2)
              s = dateTimeStringValue2.Value;
          }
          else if (pdfDictionary["CreationDate"] is PdfString dateTimeStringValue2)
            s = dateTimeStringValue2.Value;
          DateTime result = DateTime.Now;
          string str = "yyyyMMddHHmmss";
          if (s.Length > str.Length)
            result = new PdfDictionary().GetDateTime(dateTimeStringValue2);
          else
            DateTime.TryParseExact(s, "yyyyMMddHHmmss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite, out result);
          if (metadata.BasicSchema != null && !this.ConformanceEnabled)
            metadata.BasicSchema.CreateDate = result;
        }
      }
      if (pdfDictionary.ContainsKey("ModDate") && metadata.BasicSchema != null)
      {
        if (this.m_bWasEncrypted || this.Catalog.Changed)
        {
          if (!changed)
          {
            XmpMetadata xmpMetadata = metadata;
            if (this.m_bWasEncrypted && infoReference != (PdfReference) null && pdfDictionary.ContainsKey("ModDate"))
              (PdfCrossTable.Dereference(pdfDictionary["ModDate"]) as PdfString).Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
            if (PdfCrossTable.Dereference(pdfDictionary["ModDate"]) is PdfString dateTimeStringValue)
            {
              string s = dateTimeStringValue.Value;
              DateTime result = DateTime.Now;
              string str3 = "yyyyMMddHHmmss";
              if (s.Length > str3.Length)
                result = new PdfDictionary().GetDateTime(dateTimeStringValue);
              else
                DateTime.TryParseExact(s, "yyyyMMddHHmmss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite, out result);
              string str4 = xmpMetadata.BasicSchema.ModifyDate.ToString("yyyyMMddHHmmsstt");
              if (metadata.BasicSchema.m_externalModifyDate && str4 != result.ToString("yyyyMMddHHmmsstt"))
                pdfDictionary["ModDate"] = (IPdfPrimitive) new PdfString(metadata.BasicSchema.ModifyDate.ToString("yyyyMMddHHmmss"));
              else if (metadata.BasicSchema != null && !this.ConformanceEnabled)
                metadata.BasicSchema.ModifyDate = result;
            }
          }
        }
        else
        {
          IPdfPrimitive pdfPrimitive1 = (IPdfPrimitive) (pdfDictionary["ModDate"] as PdfReferenceHolder);
          PdfString dateTimeStringValue;
          string s;
          if (pdfPrimitive1 != null)
          {
            IPdfPrimitive pdfPrimitive2 = (pdfPrimitive1 as PdfReferenceHolder).Object;
            dateTimeStringValue = pdfPrimitive2 as PdfString;
            s = (pdfPrimitive2 as PdfString).Value;
          }
          else
          {
            s = (pdfDictionary["ModDate"] as PdfString).Value;
            dateTimeStringValue = pdfDictionary["ModDate"] as PdfString;
          }
          DateTime result = DateTime.Now;
          string str = "yyyyMMddHHmmss";
          if (s.Length > str.Length)
            result = new PdfDictionary().GetDateTime(dateTimeStringValue);
          else
            DateTime.TryParseExact(s, "yyyyMMddHHmmss", (IFormatProvider) DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite, out result);
          if (metadata.BasicSchema != null && !this.ConformanceEnabled)
            metadata.BasicSchema.ModifyDate = result;
        }
      }
      if (changed && flag)
        this.Catalog.SetProperty("Metadata", (IPdfWrapper) metadata);
    }
    if (pdfDictionary != null && !pdfDictionary.Changed && (object) (this.CrossTable.Trailer["Info"] as PdfReferenceHolder) != null)
    {
      this.m_documentInfo = new PdfDocumentInformation(pdfDictionary, this.Catalog);
      if (this.m_bWasEncrypted)
      {
        if ((object) (this.CrossTable.Trailer["Info"] as PdfReferenceHolder) != null)
          infoReference = (this.CrossTable.Trailer["Info"] as PdfReferenceHolder).Reference;
        this.DecryptDocumentInfo(pdfDictionary, infoReference);
      }
      if (this.PdfObjects.IndexOf(((IPdfWrapper) this.m_documentInfo).Element) > -1)
      {
        this.PdfObjects.ReregisterReference((IPdfPrimitive) pdfDictionary, ((IPdfWrapper) this.m_documentInfo).Element);
        ((IPdfWrapper) this.m_documentInfo).Element.Position = -1;
      }
    }
    if (this.Catalog.Metadata == null)
      return;
    this.Catalog.Metadata.m_hasAttributes = false;
  }

  internal bool CheckEncryption(bool isAttachEncryption)
  {
    bool flag1 = false;
    PdfDictionary trailer = this.CrossTable.Trailer;
    PdfDictionary encryptorDictionary = this.CrossTable.EncryptorDictionary;
    bool flag2 = true;
    if (encryptorDictionary != null && flag2)
    {
      if (this.m_password == null)
        this.m_password = string.Empty;
      IPdfPrimitive pdfPrimitive = trailer["ID"];
      if (pdfPrimitive == null)
      {
        pdfPrimitive = (IPdfPrimitive) new PdfArray();
        (pdfPrimitive as PdfArray).Add((IPdfPrimitive) new PdfString(new byte[0]));
      }
      PdfString key = pdfPrimitive != null ? (pdfPrimitive as PdfArray)[0] as PdfString : throw new PdfDocumentException("Unable to decrypt document without ID.");
      flag1 = true;
      PdfEncryptor pdfEncryptor = new PdfEncryptor();
      if (encryptorDictionary != null && encryptorDictionary.ContainsKey("EncryptMetadata"))
        pdfEncryptor.EncryptMetaData = (encryptorDictionary["EncryptMetadata"] as PdfBoolean).Value;
      pdfEncryptor.ReadFromDictionary(encryptorDictionary);
      bool attachEncryption = true;
      if (!isAttachEncryption && pdfEncryptor.EncryptOnlyAttachment)
        attachEncryption = false;
      if (!pdfEncryptor.CheckPassword(this.m_password, key, attachEncryption))
      {
        if (this.isOpenAndRepair)
          this.isOpenAndRepair = false;
        throw new PdfInvalidPasswordException("Can't open an encrypted document. The password is invalid.");
      }
      encryptorDictionary.Encrypt = false;
      PdfSecurity security = new PdfSecurity();
      security.m_encryptOnlyAttachment = pdfEncryptor.EncryptOnlyAttachment;
      this.isAttachmentOnlyEncryption = pdfEncryptor.EncryptOnlyAttachment;
      if (this.isAttachmentOnlyEncryption)
        security.m_encryptionOption = PdfEncryptionOptions.EncryptOnlyAttachments;
      else if (!pdfEncryptor.EncryptMetaData)
        security.m_encryptionOption = PdfEncryptionOptions.EncryptAllContentsExceptMetadata;
      security.Encryptor = pdfEncryptor;
      this.SetSecurity(security);
      this.CrossTable.Encryptor = pdfEncryptor;
    }
    return flag1;
  }

  private PdfDictionary GetFormDictionary()
  {
    return PdfCrossTable.Dereference(this.Catalog["AcroForm"]) as PdfDictionary;
  }

  private PdfDictionary GetAttachmentDictionary()
  {
    return PdfCrossTable.Dereference(this.Catalog["Names"]) as PdfDictionary;
  }

  private PdfDictionary GetPortfolioDictionary()
  {
    return PdfCrossTable.Dereference(this.Catalog["Collection"]) as PdfDictionary;
  }

  private void AppendDocument(PdfWriter writer)
  {
    writer.Document = (PdfDocumentBase) this;
    if (this.isPageLabel)
      this.PageLabel();
    if (this.FileStructure.IncrementalUpdate)
    {
      foreach (PdfPageBase page in this.Pages)
      {
        if (page != null)
        {
          PdfDictionary dictionary = page.Dictionary;
          if (dictionary != null && dictionary.Changed)
            ++this.m_changedPages;
        }
      }
    }
    this.CrossTable.Save(writer);
    if (this.progressDelegate != null)
    {
      int count = this.Pages.Count;
      this.OnSaveProgress(new ProgressEventArgs(count, count));
    }
    this.Security.Enabled = true;
    this.OnDocumentSaved(new DocumentSavedEventArgs(writer));
  }

  private void CopyOldStream(PdfWriter writer)
  {
    this.m_stream.Position = 0L;
    byte[] numArray = new byte[8190];
    int end;
    while ((end = this.m_stream.Read(numArray, 0, numArray.Length)) != 0)
      writer.Write(numArray, end);
  }

  private Stream CheckIfValid(Stream file)
  {
    file.Position = file.Length - 1L;
    if (file.ReadByte() == 0)
    {
      byte[] numArray1 = new byte[file.Length];
      file.Position = 0L;
      file.Read(numArray1, 0, numArray1.Length);
      int index = numArray1.Length - 1;
      while (numArray1[index] == (byte) 0)
        --index;
      byte[] numArray2 = new byte[index + 1];
      System.Array.Copy((System.Array) numArray1, (System.Array) numArray2, index + 1);
      MemoryStream memoryStream = new MemoryStream();
      memoryStream.Write(numArray2, 0, numArray2.Length);
      file.Dispose();
      return (Stream) memoryStream;
    }
    file.Position = 0L;
    return file;
  }

  private bool CheckLinearization()
  {
    bool flag = false;
    PdfReader pdfReader = new PdfReader(this.m_stream);
    if (this.CrossTable != null && this.CrossTable.CrossTable != null && this.CrossTable.CrossTable.Parser != null)
    {
      PdfParser parser = this.CrossTable.CrossTable.Parser;
      Dictionary<long, ObjectInformation> newObjects = new Dictionary<long, ObjectInformation>();
      foreach (KeyValuePair<long, ObjectInformation> keyValuePair in parser.FindFirstObject(newObjects, this.CrossTable.CrossTable))
      {
        ObjectInformation objectInformation = keyValuePair.Value;
        if (objectInformation != null && objectInformation.Type == ObjectType.Normal)
        {
          IPdfPrimitive pdfPrimitive = parser.Parse(objectInformation.Offset);
          if (pdfPrimitive != null && PdfCrossTable.Dereference(pdfPrimitive) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Linearized") && pdfDictionary.ContainsKey("L"))
          {
            long length = this.m_stream.Length;
            if (PdfCrossTable.Dereference(pdfDictionary["L"]) is PdfNumber pdfNumber)
            {
              flag = pdfNumber.LongValue == length;
              break;
            }
          }
        }
      }
    }
    return flag;
  }

  public bool ExportAnnotations(string fileName, AnnotationDataFormat format)
  {
    return this.ExportAnnotations(fileName, format, string.Empty);
  }

  public bool ExportAnnotations(
    string fileName,
    AnnotationDataFormat format,
    string targetFilePath)
  {
    Stream fileStream = this.GetFileStream(fileName);
    bool flag = true;
    if (this.ExportAnnotations(fileStream, format, targetFilePath))
    {
      fileStream.Close();
    }
    else
    {
      fileStream.Close();
      fileStream.Dispose();
      File.Delete(fileName);
      flag = false;
    }
    return flag;
  }

  public bool ExportAnnotations(
    string fileName,
    AnnotationDataFormat format,
    PdfExportAnnotationCollection collection)
  {
    return this.ExportAnnotations(fileName, format, string.Empty, collection);
  }

  public bool ExportAnnotations(
    string fileName,
    AnnotationDataFormat format,
    string targetFilePath,
    PdfExportAnnotationCollection collection)
  {
    bool flag = true;
    Stream fileStream = this.GetFileStream(fileName);
    if (this.ExportAnnotations(fileStream, format, targetFilePath, collection))
    {
      fileStream.Close();
    }
    else
    {
      fileStream.Close();
      fileStream.Dispose();
      File.Delete(fileName);
      flag = false;
    }
    return flag;
  }

  public bool ExportAnnotations(Stream stream, AnnotationDataFormat format)
  {
    return this.ExportAnnotations(stream, format, string.Empty);
  }

  public bool ExportAnnotations(Stream stream, AnnotationDataFormat format, string targetFilePath)
  {
    bool flag = true;
    PdfWriter writer = new PdfWriter(stream);
    switch (format)
    {
      case AnnotationDataFormat.Fdf:
        if (!this.ExportAnnotationsFDF(writer, targetFilePath, (PdfExportAnnotationCollection) null))
        {
          flag = true;
          break;
        }
        break;
      case AnnotationDataFormat.Json:
        this.ExportAnnotationsJSON(stream, targetFilePath);
        break;
      default:
        this.ExportAnnotationsXFDF(stream, targetFilePath);
        break;
    }
    return flag;
  }

  public bool ExportAnnotations(
    Stream stream,
    AnnotationDataFormat format,
    PdfExportAnnotationCollection collection)
  {
    return this.ExportAnnotations(stream, format, string.Empty, collection);
  }

  public bool ExportAnnotations(
    Stream stream,
    AnnotationDataFormat format,
    string targetFilePath,
    PdfExportAnnotationCollection collection)
  {
    bool flag = true;
    if (collection == null)
      throw new ArgumentNullException("Collection");
    if (collection.Count <= 0)
      throw new ArgumentException("Empty collection");
    PdfWriter writer = new PdfWriter(stream);
    switch (format)
    {
      case AnnotationDataFormat.Fdf:
        if (!this.ExportAnnotationsFDF(writer, targetFilePath, collection))
        {
          flag = true;
          break;
        }
        break;
      case AnnotationDataFormat.Json:
        this.ExportAnnotationsJSON(stream, targetFilePath, collection);
        break;
      default:
        this.ExportAnnotationsXFDF(stream, targetFilePath, collection);
        break;
    }
    return flag;
  }

  private void ExportAnnotationsXFDF(Stream stream, string fileName)
  {
    XFdfDocument xfdfDocument = new XFdfDocument(fileName);
    xfdfDocument.IsExportAnnotations = true;
    xfdfDocument.ExportAnnotations(stream, this);
    xfdfDocument.IsExportAnnotations = false;
  }

  private void ExportAnnotationsJSON(Stream stream, string fileName)
  {
    new JsonDocument(fileName).ExportAnnotations(stream, this);
  }

  private void ExportAnnotationsJSON(
    Stream stream,
    string fileName,
    PdfExportAnnotationCollection collection)
  {
    new JsonDocument(fileName)
    {
      AnnotationCollection = collection
    }.ExportAnnotations(stream, this);
  }

  private void ExportAnnotationsXFDF(
    Stream stream,
    string fileName,
    PdfExportAnnotationCollection collection)
  {
    XFdfDocument xfdfDocument = new XFdfDocument(fileName);
    xfdfDocument.IsExportAnnotations = true;
    xfdfDocument.AnnotationCollection = collection;
    xfdfDocument.ExportAnnotations(stream, this);
    xfdfDocument.IsExportAnnotations = false;
  }

  private Stream GetFileStream(string fileName)
  {
    try
    {
      return (Stream) new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
    }
    catch (Exception ex)
    {
      throw ex;
    }
  }

  private bool ExportAnnotationsFDF(
    PdfWriter writer,
    string fileName,
    PdfExportAnnotationCollection collection)
  {
    string str1 = " 0 ";
    string str2 = "<</";
    writer.Write("%FDF-1.2" + Environment.NewLine);
    int currentID = 2;
    List<string> annotID = new List<string>();
    if (collection == null)
    {
      for (int index = 0; index < this.PageCount; ++index)
      {
        foreach (PdfAnnotation annotation in (PdfCollection) (this.Pages[index] as PdfLoadedPage).Annotations)
        {
          if (annotation is PdfLoadedAnnotation && annotation is PdfLoadedAnnotation loadedAnnotation)
          {
            switch (loadedAnnotation)
            {
              case PdfLoadedFileLinkAnnotation _:
              case PdfLoadedTextWebLinkAnnotation _:
              case PdfLoadedDocumentLinkAnnotation _:
              case PdfLoadedUriAnnotation _:
                continue;
              case PdfLoadedRubberStampAnnotation _:
                loadedAnnotation.ExportAnnotation(ref writer, ref currentID, annotID, index, true);
                continue;
              default:
                loadedAnnotation.ExportAnnotation(ref writer, ref currentID, annotID, index, false);
                continue;
            }
          }
        }
      }
    }
    else
    {
      foreach (PdfLoadedAnnotation loadedAnnotation in (PdfCollection) collection)
      {
        switch (loadedAnnotation)
        {
          case null:
          case PdfLoadedFileLinkAnnotation _:
          case PdfLoadedTextWebLinkAnnotation _:
          case PdfLoadedDocumentLinkAnnotation _:
          case PdfLoadedUriAnnotation _:
            continue;
          default:
            int pageIndex = this.Pages.IndexOf((PdfPageBase) loadedAnnotation.Page);
            if (pageIndex >= 0)
            {
              if (loadedAnnotation is PdfLoadedRubberStampAnnotation)
              {
                loadedAnnotation.ExportAnnotation(ref writer, ref currentID, annotID, pageIndex, true);
                continue;
              }
              loadedAnnotation.ExportAnnotation(ref writer, ref currentID, annotID, pageIndex, false);
              continue;
            }
            continue;
        }
      }
    }
    if (currentID == 2)
      return false;
    string str3 = "1" + str1;
    writer.Write($"{str3}obj\r\n{str2}FDF{str2}Annots[");
    for (int index = 0; index < annotID.Count - 1; ++index)
      writer.Write($"{annotID[index]}{str1}R ");
    writer.Write($"{annotID[annotID.Count - 1]}{str1}R]/F({fileName})/UF({fileName})>>/Type/Catalog>>\r\nendobj\r\n");
    writer.Write($"trailer\r\n{str2}Root {str3}R>>\r\n%%EOF\r\n");
    return true;
  }

  public void ImportAnnotations(string fileName, AnnotationDataFormat format)
  {
    FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
    this.ImportAnnotations((Stream) fileStream, format);
    fileStream?.Close();
  }

  public void ImportAnnotations(Stream stream, AnnotationDataFormat format)
  {
    if (stream == null)
      throw new ArgumentNullException("Annotation Data Stream");
    if (!stream.CanSeek || !stream.CanRead)
      throw new Exception("Ivalid stream");
    if (format == AnnotationDataFormat.Fdf)
    {
      FdfParser fdfParser = new FdfParser(stream);
      fdfParser.ParseAnnotationData();
      fdfParser.ImportAnnotations(this);
      fdfParser.Dispose();
    }
    else if (format == AnnotationDataFormat.Json)
      new JsonParser(stream, this).ImportAnnotationData(stream);
    else
      new XfdfParser(stream, this).ParseAndImportAnnotationData();
  }

  private void DecryptDocumentInfo(PdfDictionary info, PdfReference infoReference)
  {
    if (info == null || !(infoReference != (PdfReference) null))
      return;
    if (info.ContainsKey("Producer") && PdfCrossTable.Dereference(info["Producer"]) is PdfString pdfString1)
      pdfString1.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (info.ContainsKey("Creator") && PdfCrossTable.Dereference(info["Creator"]) is PdfString pdfString2)
      pdfString2.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (info.ContainsKey("Author") && PdfCrossTable.Dereference(info["Author"]) is PdfString pdfString3)
      pdfString3.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (info.ContainsKey("Title") && PdfCrossTable.Dereference(info["Title"]) is PdfString pdfString4)
      pdfString4.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (info.ContainsKey("Subject") && PdfCrossTable.Dereference(info["Subject"]) is PdfString pdfString5)
      pdfString5.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (info.ContainsKey("CreationDate") && PdfCrossTable.Dereference(info["CreationDate"]) is PdfString pdfString6)
      pdfString6.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (info.ContainsKey("ModDate") && PdfCrossTable.Dereference(info["ModDate"]) is PdfString pdfString7)
      pdfString7.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
    if (!info.ContainsKey("Keywords") || !(PdfCrossTable.Dereference(info["Keywords"]) is PdfString pdfString8))
      return;
    pdfString8.Decrypt(this.CrossTable.Encryptor, infoReference.ObjNum);
  }

  public delegate void OnPdfPasswordEventHandler(object sender, OnPdfPasswordEventArgs args);

  public delegate void RedactionProgressEventHandler(
    object sender,
    RedactionProgressEventArgs arguments);

  public delegate void PdfAConversionProgressEventHandler(
    object sender,
    PdfAConversionProgressEventArgs arguments);

  private class CurrentNodeInfo
  {
    public List<PdfBookmarkBase> Kids;
    public int Index;

    public CurrentNodeInfo(List<PdfBookmarkBase> kids)
    {
      this.Kids = kids;
      this.Index = 0;
    }

    public CurrentNodeInfo(List<PdfBookmarkBase> kids, int index)
      : this(kids)
    {
      this.Index = index;
    }
  }
}
