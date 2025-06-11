// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Web;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfDocumentBase
{
  private PdfMainObjectCollection m_objects;
  internal PdfArray m_lock = new PdfArray();
  internal static object s_licenseLock = new object();
  private int m_progressPageIndex = -1;
  private List<PdfNamedDestination> m_namedDetinations = new List<PdfNamedDestination>();
  private PdfSecurity m_security;
  private PdfReference m_currentSavingObj;
  private PdfCatalog m_catalog;
  private PdfCrossTable m_crossTable;
  private PdfDocumentInformation m_documentInfo;
  private string m_password;
  private Dictionary<string, int> m_imageCollection = new Dictionary<string, int>();
  private PdfCompressionLevel m_compression = PdfCompressionLevel.Normal;
  private PdfFileStructure m_fileStructure;
  private List<IDisposable> m_disposeObjects;
  private bool m_enableMemoryOptimization;
  private PdfPortfolioInformation m_portfolio;
  internal PdfArray primitive = new PdfArray();
  internal int m_positon;
  internal int m_orderposition;
  internal int m_onpositon;
  internal int m_offpositon;
  internal PdfArray m_order = new PdfArray();
  internal PdfArray m_on = new PdfArray();
  internal PdfArray m_off = new PdfArray();
  internal PdfArray m_sublayer = new PdfArray();
  internal int m_sublayerposition;
  internal PdfArray m_printLayer = new PdfArray();
  internal bool m_isStreamCopied;
  internal bool m_isImported;
  private int m_annotCount;
  private bool m_isKidsPage;
  internal bool isCompressed;
  private bool m_isMerging;
  private PdfDocumentLayerCollection m_layers;
  internal PdfDocumentBase.ProgressEventHandler progressDelegate;
  private int m_pageProcessed = -1;
  private int m_pageCount;
  internal int m_changedPages;
  private Dictionary<string, PdfField> m_fieldKids = new Dictionary<string, PdfField>();
  private List<string> m_addedField = new List<string>();
  private Dictionary<string, IPdfPrimitive> m_resourceCollection;
  internal Dictionary<PdfReferenceHolder, PdfDictionary> documentLayerCollection;

  internal event PdfDocumentBase.DocumentSavedEventHandler DocumentSaved;

  public event PdfDocumentBase.ProgressEventHandler SaveProgress
  {
    add
    {
      this.progressDelegate = Delegate.Combine((Delegate) this.progressDelegate, (Delegate) value) as PdfDocumentBase.ProgressEventHandler;
      if (this.progressDelegate == null)
        return;
      this.SetProgress();
    }
    remove
    {
      this.progressDelegate = Delegate.Remove((Delegate) this.progressDelegate, (Delegate) value) as PdfDocumentBase.ProgressEventHandler;
      if (this.progressDelegate != null)
        return;
      this.ResetProgress();
    }
  }

  public PdfSecurity Security
  {
    get
    {
      if (this.m_security == null)
        this.m_security = new PdfSecurity();
      return this.m_security;
    }
  }

  internal Dictionary<string, int> ImageCollection
  {
    get
    {
      if (this.m_imageCollection == null)
        this.m_imageCollection = new Dictionary<string, int>();
      return this.m_imageCollection;
    }
  }

  internal static bool IsSecurityGranted
  {
    get
    {
      bool isSecurityGranted = false;
      SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
      try
      {
        securityPermission.Demand();
        isSecurityGranted = true;
      }
      catch (SecurityException ex)
      {
      }
      return isSecurityGranted;
    }
  }

  public virtual PdfDocumentInformation DocumentInformation
  {
    get
    {
      if (this.m_documentInfo == null)
      {
        this.m_documentInfo = new PdfDocumentInformation(this.Catalog);
        this.CrossTable.Trailer["Info"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_documentInfo);
      }
      return this.m_documentInfo;
    }
  }

  public PdfViewerPreferences ViewerPreferences
  {
    get
    {
      if (this.m_catalog.ViewerPreferences == null)
        this.m_catalog.ViewerPreferences = new PdfViewerPreferences(this.m_catalog);
      return this.m_catalog.ViewerPreferences;
    }
    set
    {
      this.m_catalog.ViewerPreferences = value != null ? value : throw new ArgumentNullException(nameof (ViewerPreferences));
    }
  }

  public PdfCompressionLevel Compression
  {
    get => this.m_compression;
    set => this.m_compression = value;
  }

  public PdfFileStructure FileStructure
  {
    get
    {
      if (this.m_fileStructure == null)
      {
        this.m_fileStructure = new PdfFileStructure();
        this.m_fileStructure.TaggedPdfChanged += new EventHandler(this.m_fileStructure_TaggedPdfChanged);
      }
      return this.m_fileStructure;
    }
    set => this.m_fileStructure = value;
  }

  public PdfPortfolioInformation PortfolioInformation
  {
    get => this.m_portfolio;
    set
    {
      this.m_portfolio = value;
      this.m_catalog.PdfPortfolio = this.m_portfolio;
    }
  }

  private void m_fileStructure_TaggedPdfChanged(object sender, EventArgs e)
  {
    if (!this.m_fileStructure.TaggedPdf)
      return;
    this.Catalog.InitializeStructTreeRoot();
  }

  public abstract PdfBookmarkBase Bookmarks { get; }

  internal abstract bool WasEncrypted { get; }

  internal abstract bool IsPdfViewerDocumentDisable { get; set; }

  internal PdfMainObjectCollection PdfObjects => this.m_objects;

  internal PdfReference CurrentSavingObj
  {
    get => this.m_currentSavingObj;
    set => this.m_currentSavingObj = value;
  }

  internal PdfCrossTable CrossTable => this.m_crossTable;

  internal PdfCatalog Catalog => this.m_catalog;

  internal List<IDisposable> DisposeObjects
  {
    get
    {
      if (this.m_disposeObjects == null)
        this.m_disposeObjects = new List<IDisposable>();
      return this.m_disposeObjects;
    }
  }

  internal abstract int PageCount { get; }

  public bool EnableMemoryOptimization
  {
    get => this.m_enableMemoryOptimization;
    set => this.m_enableMemoryOptimization = value;
  }

  public PdfDocumentLayerCollection Layers
  {
    get
    {
      if (this.m_layers == null)
        this.m_layers = new PdfDocumentLayerCollection(this);
      return this.m_layers;
    }
  }

  internal Dictionary<string, IPdfPrimitive> ResourceCollection
  {
    get
    {
      if (this.m_resourceCollection == null)
        this.m_resourceCollection = new Dictionary<string, IPdfPrimitive>();
      return this.m_resourceCollection;
    }
  }

  public static PdfDocumentBase Merge(
    PdfDocumentBase dest,
    PdfMergeOptions options,
    params object[] sourceDocuments)
  {
    if (dest == null)
      dest = (PdfDocumentBase) new PdfDocument(true);
    if (dest is PdfDocument && (dest as PdfDocument).Sections.Count > 0)
      (dest as PdfDocument).IsMergeDocHasSections = true;
    int index = 0;
    for (int length = sourceDocuments.Length; index < length; ++index)
    {
      object sourceDocument = sourceDocuments[index];
      string filename = sourceDocument as string;
      Stream file1 = sourceDocument as Stream;
      byte[] file2 = sourceDocument as byte[];
      PdfLoadedDocument ldDoc = sourceDocument as PdfLoadedDocument;
      bool flag = true;
      if (filename != null)
        ldDoc = new PdfLoadedDocument(filename);
      else if (file1 != null)
        ldDoc = new PdfLoadedDocument(file1);
      else if (file2 != null)
      {
        ldDoc = new PdfLoadedDocument(file2);
      }
      else
      {
        if (ldDoc == null)
          throw new ArgumentException("Unsupported argument type: " + (object) sourceDocument.GetType());
        flag = false;
      }
      ldDoc.IsOptimizeIdentical = options.OptimizeResources;
      ldDoc.DestinationDocument = dest;
      ldDoc.IsExtendMargin = options.ExtendMargin;
      dest.Append(ldDoc);
      if (dest is PdfDocument && (dest as PdfDocument).Form != null && ldDoc.Form != null)
      {
        if (ldDoc.Form.IsXFAForm)
        {
          (dest as PdfDocument).Form.NeedAppearances = true;
          (dest as PdfDocument).Form.IsXFA = true;
        }
        else if (ldDoc.IsXFAForm)
        {
          (dest as PdfDocument).Form.NeedAppearances = true;
          (dest as PdfDocument).Form.IsXFA = true;
        }
      }
      if (flag && dest.EnableMemoryOptimization)
        ldDoc.Close(true);
    }
    if (dest is PdfDocument)
      (dest as PdfDocument).IsMergeDocHasSections = true;
    return dest;
  }

  public static PdfDocumentBase Merge(PdfDocumentBase dest, params object[] sourceDocuments)
  {
    PdfMergeOptions options = new PdfMergeOptions();
    return PdfDocumentBase.Merge(dest, options, sourceDocuments);
  }

  public static PdfDocument Merge(string[] paths, PdfMergeOptions options)
  {
    if (paths == null)
      throw new ArgumentNullException(nameof (paths));
    PdfDocument pdfDocument = new PdfDocument(true);
    pdfDocument.EnableMemoryOptimization = true;
    if (pdfDocument != null && pdfDocument.Sections.Count > 0)
      pdfDocument.IsMergeDocHasSections = true;
    bool flag = false;
    foreach (string path in paths)
    {
      PdfLoadedDocument ldDoc = path != null ? new PdfLoadedDocument(path) : throw new ArgumentNullException("path");
      if (ldDoc.IsXFAForm)
      {
        flag = true;
        pdfDocument.Form.IsXFA = true;
      }
      else if (ldDoc.Form != null && ldDoc.Form.IsXFAForm)
      {
        flag = true;
        pdfDocument.Form.IsXFA = true;
      }
      ldDoc.IsOptimizeIdentical = options.OptimizeResources;
      ldDoc.DestinationDocument = (PdfDocumentBase) pdfDocument;
      pdfDocument.Append(ldDoc);
      ldDoc.Close(true);
    }
    if (pdfDocument != null && pdfDocument.Form != null && flag)
      pdfDocument.Form.NeedAppearances = true;
    if (pdfDocument != null)
      pdfDocument.IsMergeDocHasSections = true;
    return pdfDocument;
  }

  public static PdfDocument Merge(string[] paths)
  {
    PdfMergeOptions options = new PdfMergeOptions();
    return PdfDocumentBase.Merge(paths, options);
  }

  public static PdfDocumentBase Merge(PdfDocumentBase dest, PdfLoadedDocument src)
  {
    if (src == null)
      throw new ArgumentNullException(nameof (src));
    if (dest == null)
      dest = (PdfDocumentBase) new PdfDocument(true);
    dest.Append(src);
    if (dest is PdfDocument && (dest as PdfDocument).Sections.Count > 0)
      (dest as PdfDocument).IsMergeDocHasSections = true;
    if (dest is PdfDocument && (dest as PdfDocument).Form != null)
    {
      if (src.IsXFAForm)
      {
        (dest as PdfDocument).Form.NeedAppearances = true;
        (dest as PdfDocument).Form.IsXFA = true;
      }
      else if (src.Form != null && src.Form.IsXFAForm)
      {
        (dest as PdfDocument).Form.NeedAppearances = true;
        (dest as PdfDocument).Form.IsXFA = true;
      }
    }
    if (dest is PdfDocument)
      (dest as PdfDocument).IsMergeDocHasSections = true;
    return dest;
  }

  public void DisposeOnClose(IDisposable obj)
  {
    if (obj == null)
      return;
    this.DisposeObjects.Add(obj);
  }

  public void Save(string filename)
  {
    switch (filename)
    {
      case null:
        throw new ArgumentNullException("fileName");
      case "":
        throw new ArgumentException("fileName - string can not be empty");
      default:
        string fullPath = Path.GetFullPath(filename);
        string directoryName = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        if (File.Exists(fullPath) && (File.GetAttributes(fullPath) & FileAttributes.ReadOnly) != (FileAttributes) 0)
          throw new ArgumentException("File attributes set to Read-only state. File Name: " + fullPath);
        if (this is PdfLoadedDocument && (this as PdfLoadedDocument).m_fileName != null)
        {
          if (Path.GetFullPath((this as PdfLoadedDocument).m_fileName).Equals(fullPath))
          {
            (this as PdfLoadedDocument).Save();
            break;
          }
          using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
          {
            this.Save((Stream) fileStream);
            break;
          }
        }
        using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
        {
          this.Save((Stream) fileStream);
          break;
        }
    }
  }

  public void Save(string fileName, HttpResponse response, HttpReadType type)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    if (response == null)
      throw new ArgumentNullException(nameof (response));
    response.ClearContent();
    response.Expires = 0;
    response.Buffer = true;
    string name = "content-disposition";
    switch (type)
    {
      case HttpReadType.Open:
        response.AddHeader(name, "inline; filename=" + fileName);
        break;
      case HttpReadType.Save:
        response.AddHeader(name, $"attachment; filename=\"{fileName}\"");
        break;
    }
    response.AddHeader("Content-Type", "application/pdf");
    response.Clear();
    if (this.DocumentSaved != null)
    {
      MemoryStream memoryStream = new MemoryStream();
      this.Save((Stream) memoryStream);
      memoryStream.Position = 0L;
      for (int index = 0; (long) index < memoryStream.Length; ++index)
      {
        int num = memoryStream.ReadByte();
        if (num > -1)
          response.OutputStream.WriteByte((byte) num);
      }
      memoryStream.Close();
    }
    else
      this.Save(response.OutputStream);
    if (this is PdfDocument)
    {
      (this as PdfDocument).Close();
      (this as PdfDocument).Dispose();
    }
    else if (this is PdfLoadedDocument)
    {
      (this as PdfLoadedDocument).Close();
      (this as PdfLoadedDocument).Dispose();
    }
    if (PdfDocumentBase.IsSecurityGranted)
    {
      response.Flush();
      response.End();
    }
    else
      response.End();
  }

  public void Save(Stream stream, HttpContext response)
  {
    if (stream == null)
      throw new ArgumentNullException("fileName");
    if (response == null)
      throw new ArgumentNullException(nameof (response));
    this.Save(stream);
    stream.Position = 0L;
    response.Response.Clear();
    response.Response.ClearContent();
    response.Response.ClearHeaders();
    response.Response.ContentType = "application/pdf";
    for (int index = 0; (long) index < stream.Length; ++index)
    {
      int num = stream.ReadByte();
      if (num > -1)
        response.Response.OutputStream.WriteByte((byte) num);
    }
    response.Response.Flush();
    response.Response.Close();
    stream.Close();
  }

  public void Close() => this.Close(false);

  public virtual void Close(bool completely)
  {
    this.m_security = (PdfSecurity) null;
    PdfColor.Clear();
    this.m_objects = (PdfMainObjectCollection) null;
    this.m_currentSavingObj = (PdfReference) null;
    if (this.m_catalog != null && completely && this.EnableMemoryOptimization)
    {
      this.m_catalog.Clear();
      this.m_catalog = (PdfCatalog) null;
    }
    if (this.m_crossTable != null)
      this.m_crossTable.isDisposed = completely;
    if (this.EnableMemoryOptimization)
    {
      if (this.m_crossTable != null)
        this.m_crossTable.Close(true);
    }
    else if (completely && this.m_crossTable != null)
      this.m_crossTable.Dispose();
    this.m_crossTable = (PdfCrossTable) null;
    this.m_fieldKids.Clear();
    this.m_addedField.Clear();
    if (this.documentLayerCollection != null)
    {
      this.documentLayerCollection.Clear();
      this.documentLayerCollection = (Dictionary<PdfReferenceHolder, PdfDictionary>) null;
    }
    this.m_documentInfo = (PdfDocumentInformation) null;
    this.m_compression = PdfCompressionLevel.Normal;
    if (PdfCatalog.m_structTreeRoot != null)
      PdfCatalog.m_structTreeRoot = (PdfStructTreeRoot) null;
    if (this.m_disposeObjects != null)
    {
      int index = 0;
      for (int count = this.m_disposeObjects.Count; index < count; ++index)
        this.m_disposeObjects[index]?.Dispose();
      this.m_disposeObjects.Clear();
      this.m_disposeObjects = (List<IDisposable>) null;
    }
    if (this.m_resourceCollection != null)
      this.m_resourceCollection.Clear();
    if (this is PdfLoadedDocument pdfLoadedDocument && pdfLoadedDocument.ConformanceEnabled && PdfDocument.ConformanceLevel != PdfConformanceLevel.None || pdfLoadedDocument == null)
      PdfDocument.ConformanceLevel = PdfConformanceLevel.None;
    if (!PdfDocument.EnableCache)
      return;
    PdfDocument.Cache.Clear();
    PdfDocument.Cache = (PdfCacheCollection) null;
  }

  public abstract void Save(Stream stream);

  public PdfPageBase ImportPage(PdfLoadedDocument ldDoc, PdfPageBase page)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    int pageIndex = ldDoc.Pages.IndexOf(page);
    return this.ImportPage(ldDoc, pageIndex);
  }

  public PdfPageBase ImportPage(PdfLoadedDocument ldDoc, int pageIndex)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    if (pageIndex < 0 || pageIndex >= ldDoc.Pages.Count)
      throw new ArgumentOutOfRangeException(nameof (pageIndex));
    return this.ImportPageRange(ldDoc, pageIndex, pageIndex);
  }

  public PdfPageBase ImportPageRange(PdfLoadedDocument ldDoc, int startIndex, int endIndex)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    if (startIndex > endIndex)
      throw new ArgumentException("The start index is greater then the end index, which might indicate the error in the program.");
    return this.ImportPageRange(ldDoc, startIndex, endIndex, true);
  }

  public PdfPageBase ImportPageRange(
    PdfLoadedDocument ldDoc,
    int startIndex,
    int endIndex,
    bool importBookmarks)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    if (startIndex > endIndex)
      throw new ArgumentException("The start index is greater then the end index, which might indicate the error in the program.");
    this.m_isImported = true;
    PdfPageBase pdfPageBase1 = (PdfPageBase) null;
    PdfLoadedPageCollection pages = ldDoc.Pages;
    if (this is PdfLoadedDocument)
    {
      PdfLoadedDocument pdfLoadedDocument = this as PdfLoadedDocument;
      foreach (PdfPageBase page in pages)
      {
        if (pdfLoadedDocument.Pages.IndexOf(page) >= 0)
          return (PdfPageBase) null;
      }
    }
    if (ldDoc.CrossTable.DocumentCatalog.ContainsKey("Pages"))
    {
      PdfDictionary pdfDictionary = (ldDoc.CrossTable.DocumentCatalog["Pages"] as PdfReferenceHolder).Object as PdfDictionary;
      PdfReferenceHolder pdfReferenceHolder = pdfDictionary["Kids"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        IPdfPrimitive pdfPrimitive1 = pdfReferenceHolder.Object;
      }
      else
      {
        IPdfPrimitive pdfPrimitive2 = pdfDictionary["Kids"];
      }
    }
    if (endIndex >= pages.Count || startIndex >= pages.Count)
      throw new ArgumentException("Either or both indices are out of range", "endIndex, startIndex");
    List<PdfField> pdfFieldList = new List<PdfField>();
    List<PdfBookmarkBase> bookmarks = new List<PdfBookmarkBase>();
    List<PdfArray> destinations = new List<PdfArray>();
    Dictionary<IPdfPrimitive, object> pageCorrespondance = ldDoc.CrossTable.PageCorrespondance;
    Dictionary<int, PdfPageBase> dictionary = new Dictionary<int, PdfPageBase>();
    Dictionary<PdfPageBase, object> bookmarkshash = (Dictionary<PdfPageBase, object>) null;
    bool flag1 = false;
    if (importBookmarks)
    {
      bookmarkshash = ldDoc.CreateBookmarkDestinationDictionary();
      flag1 = bookmarkshash != null && bookmarkshash.Count > 0;
    }
    bool flag2 = false;
    ldDoc.m_isNamedDestinationCall = false;
    if (ldDoc.NamedDestinationCollection != null && ldDoc.NamedDestinationCollection.Count > 0)
      flag2 = true;
    ldDoc.m_isNamedDestinationCall = true;
    int pageCount = 0;
    for (int index = startIndex; index <= endIndex; ++index)
    {
      PdfPageBase pdfPageBase2 = pages[index];
      dictionary.Add(index, pdfPageBase2);
      PdfPageBase pdfPageBase3 = this.ClonePage(ldDoc, pdfPageBase2, destinations);
      pdfPageBase3.Imported = true;
      pdfPageBase3.Rotation = pdfPageBase2.Rotation;
      pageCorrespondance[((IPdfWrapper) pdfPageBase2).Element] = (object) pdfPageBase3;
      ++pageCount;
      if (flag1 && importBookmarks)
      {
        List<object> pageBookmarks = bookmarkshash.ContainsKey(pdfPageBase2) ? bookmarkshash[pdfPageBase2] as List<object> : (List<object>) null;
        if (pageBookmarks != null)
          this.MarkBookmarks(pageBookmarks, bookmarks);
      }
      if (pdfPageBase2.Dictionary.ContainsKey("Resources"))
        pdfPageBase1 = pdfPageBase3;
      else if (pdfPageBase2.Dictionary.ContainsKey("Parent"))
      {
        PdfDictionary pdfDictionary1 = (pdfPageBase2.Dictionary["Parent"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary1.ContainsKey("Resources"))
        {
          PdfResources pdfResources = (PdfResources) null;
          if ((object) (pdfDictionary1["Resources"] as PdfReferenceHolder) != null)
          {
            if ((pdfDictionary1["Resources"] as PdfReferenceHolder).Object is PdfDictionary)
              pdfResources = new PdfResources((pdfDictionary1["Resources"] as PdfReferenceHolder).Object as PdfDictionary);
          }
          else
            pdfResources = new PdfResources(pdfDictionary1["Resources"] as PdfDictionary);
          if (pdfResources != null && (pdfPageBase3 as PdfPage).Dictionary.ContainsKey("Resources"))
          {
            (pdfPageBase3 as PdfPage).Dictionary.Remove("Resources");
            PdfDictionary pdfDictionary2 = (object) (pdfDictionary1["Resources"] as PdfReferenceHolder) == null ? pdfDictionary1["Resources"] as PdfDictionary : (pdfDictionary1["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary2 != null)
            {
              PdfDictionary pdfDictionary3 = this.EnableMemoryOptimization ? pdfDictionary2.Clone(this.CrossTable) as PdfDictionary : pdfDictionary2;
              (pdfPageBase3 as PdfPage).Dictionary["Resources"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary3);
            }
            pdfPageBase3.Contents.Clear();
            foreach (IPdfPrimitive content in pdfPageBase2.Contents)
            {
              if (this.EnableMemoryOptimization)
                pdfPageBase3.Contents.Add(content.Clone(this.m_crossTable));
              else
                pdfPageBase3.Contents.Add(content);
            }
            (pdfPageBase3 as PdfPage).Dictionary.Modify();
          }
        }
      }
    }
    for (int key = startIndex; key <= endIndex; ++key)
    {
      List<PdfField> fields = new List<PdfField>();
      PdfPageBase page = dictionary[key];
      PdfPageBase pdfPageBase4 = pageCorrespondance[((IPdfWrapper) page).Element] as PdfPageBase;
      if (this.EnableMemoryOptimization && page.Dictionary.ContainsKey("Annots"))
      {
        (pdfPageBase4 as PdfPage).ImportAnnotations(ldDoc, page, destinations);
        this.m_annotCount = (pdfPageBase4 as PdfPage).FieldsCount;
      }
      if (page.Dictionary.ContainsKey("Annots"))
        this.CheckFields(ldDoc, page, fields, pdfPageBase4);
      if (fields.Count > 0)
      {
        this.AddFields(ldDoc, pdfPageBase4, fields);
        fields.Clear();
        PdfForm form = this.ObtainForm();
        if (form != null && !form.m_pageMap.ContainsKey(page.Dictionary))
          form.m_pageMap.Add(page.Dictionary, pdfPageBase4);
      }
      else if (this.m_isKidsPage)
      {
        PdfForm form = this.ObtainForm();
        if (form != null && !form.m_pageMap.ContainsKey(page.Dictionary))
          form.m_pageMap.Add(page.Dictionary, pdfPageBase4);
        this.m_isKidsPage = false;
      }
    }
    this.FixDestinations(pageCorrespondance, destinations);
    if (flag1 && importBookmarks)
    {
      this.m_namedDetinations.Clear();
      this.ExportBookmarks(ldDoc, bookmarks, pageCount, bookmarkshash);
      this.Bookmarks.CrossTable.Document = this;
      if (this.m_namedDetinations.Count > 0)
      {
        if (this.Bookmarks.CrossTable.Document is PdfLoadedDocument)
        {
          PdfNamedDestinationCollection destinationCollection = (this.Bookmarks.CrossTable.Document as PdfLoadedDocument).NamedDestinationCollection;
          foreach (PdfNamedDestination namedDetination in this.m_namedDetinations)
            destinationCollection.Add(namedDetination);
        }
        else
        {
          PdfNamedDestinationCollection destinationCollection = (this.Bookmarks.CrossTable.Document as PdfDocument).NamedDestinationCollection;
          foreach (PdfNamedDestination namedDetination in this.m_namedDetinations)
            destinationCollection.Add(namedDetination);
        }
      }
    }
    if (flag2)
    {
      bool flag3 = false;
      if (ldDoc.Bookmarks != null)
      {
        foreach (PdfBookmark bookmark in (IEnumerable) ldDoc.Bookmarks)
        {
          if (bookmark.List != null)
          {
            if (bookmark.List.Count > 0)
            {
              for (int index = 0; index < bookmark.List.Count; ++index)
              {
                PdfLoadedBookmark pdfLoadedBookmark = bookmark.List[index] as PdfLoadedBookmark;
                if (pdfLoadedBookmark.NamedDestination != null && pdfLoadedBookmark.NamedDestination != null)
                {
                  flag3 = true;
                  break;
                }
              }
            }
            else if (bookmark.NamedDestination != null && bookmark.NamedDestination != null)
            {
              flag3 = true;
              break;
            }
          }
        }
      }
      if (!flag3)
      {
        this.m_namedDetinations.Clear();
        this.ExportNamedDestination(ldDoc);
        if (this.CrossTable.Document is PdfLoadedDocument)
        {
          PdfNamedDestinationCollection destinationCollection = (this.CrossTable.Document as PdfLoadedDocument).NamedDestinationCollection;
          foreach (PdfNamedDestination namedDetination in this.m_namedDetinations)
            destinationCollection.Add(namedDetination);
        }
        else
        {
          PdfNamedDestinationCollection destinationCollection = (this.CrossTable.Document as PdfDocument).NamedDestinationCollection;
          foreach (PdfNamedDestination namedDetination in this.m_namedDetinations)
            destinationCollection.Add(namedDetination);
        }
      }
      if (this.CrossTable.Document is PdfDocument document && ldDoc != null && ldDoc.Bookmarks != null && this.m_isMerging && flag3 && ldDoc.NamedDestinationCollection != null && this.m_namedDetinations.Count < ldDoc.NamedDestinationCollection.Count)
      {
        PdfNamedDestinationCollection destinationCollection = document.NamedDestinationCollection;
        foreach (PdfNamedDestination namedDestination1 in (IEnumerable) ldDoc.NamedDestinationCollection)
        {
          if (!destinationCollection.Contains(namedDestination1))
          {
            PdfNamedDestination namedDestination2 = namedDestination1;
            if (namedDestination2.Destination != null && namedDestination2.Destination.Page != null && pageCorrespondance.ContainsKey((IPdfPrimitive) namedDestination2.Destination.Page.Dictionary))
            {
              PdfPageBase page = pageCorrespondance[(IPdfPrimitive) namedDestination2.Destination.Page.Dictionary] as PdfPageBase;
              namedDestination2 = this.GetNamedDestination(namedDestination2, page);
            }
            destinationCollection.Add(namedDestination2);
          }
        }
      }
    }
    if (!this.m_isMerging && ldDoc.CrossTable.DocumentCatalog.ContainsKey("OCProperties"))
    {
      int index1 = 0;
      PdfArray pdfArray1 = new PdfArray();
      PdfDictionary optimalContentViewDictionary = new PdfDictionary();
      PdfArray pdfArray2 = new PdfArray();
      PdfArray pdfArray3 = new PdfArray();
      PdfDictionary pdfDictionary4 = new PdfDictionary();
      pdfDictionary4.SetProperty("Event", (IPdfPrimitive) new PdfName("Print"));
      optimalContentViewDictionary["Name"] = (IPdfPrimitive) new PdfString("Layers");
      pdfDictionary4.SetProperty("Category", (IPdfPrimitive) new PdfArray()
      {
        (IPdfPrimitive) new PdfName("Print")
      });
      for (int key = startIndex; key <= endIndex; ++key)
      {
        PdfPageBase pdfPageBase5 = dictionary[key];
        if (pdfPageBase5.Dictionary.ContainsKey("Resources") && pdfPageBase5.Dictionary.Items[new PdfName("Resources")] is PdfDictionary pdfDictionary7 && pdfDictionary7.ContainsKey("Properties") && pdfDictionary7.Items[new PdfName("Properties")] is PdfDictionary pdfDictionary8)
        {
          PdfDictionary pdfDictionary5 = new PdfDictionary();
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary8.Items)
          {
            if ((object) (keyValuePair.Value as PdfReferenceHolder) != null && (keyValuePair.Value as PdfReferenceHolder).Object is PdfDictionary pdfDictionary6 && pdfDictionary6.ContainsKey("Type"))
            {
              switch ((pdfDictionary6.Items[new PdfName("Type")] as PdfName).Value)
              {
                case "OCG":
                  IPdfPrimitive element = keyValuePair.Value;
                  if (this.EnableMemoryOptimization)
                    element = keyValuePair.Value.Clone(this.CrossTable);
                  pdfDictionary5.Items.Add(keyValuePair.Key, element);
                  pdfArray1.Insert(index1, element);
                  PdfArray pdfArray4 = new PdfArray();
                  pdfArray2.Insert(index1, element);
                  pdfArray3.Insert(index1, element);
                  optimalContentViewDictionary["Order"] = (IPdfPrimitive) pdfArray2;
                  optimalContentViewDictionary["ON"] = (IPdfPrimitive) pdfArray3;
                  PdfArray pdfArray5 = new PdfArray();
                  optimalContentViewDictionary["OFF"] = (IPdfPrimitive) pdfArray5;
                  pdfDictionary4.SetProperty("OCGs", (IPdfPrimitive) pdfArray1);
                  pdfArray4.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary4));
                  optimalContentViewDictionary["AS"] = (IPdfPrimitive) pdfArray4;
                  ++index1;
                  continue;
                default:
                  continue;
              }
            }
          }
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary5.Items)
          {
            if (pdfDictionary8.Items.ContainsKey(keyValuePair.Key))
              pdfDictionary8.Items[keyValuePair.Key] = keyValuePair.Value;
          }
        }
        if (pdfPageBase5.Dictionary.ContainsKey("Annots") && PdfCrossTable.Dereference(pdfPageBase5.Dictionary.Items[new PdfName("Annots")]) is PdfArray pdfArray8)
        {
          for (int index2 = 0; index2 <= pdfArray8.Count - 1; ++index2)
          {
            if (PdfCrossTable.Dereference(pdfArray8[index2]) is PdfDictionary pdfDictionary10)
            {
              foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary10.Items)
              {
                if (keyValuePair.Key.Value == "OC")
                {
                  PdfDictionary pdfDictionary9 = (keyValuePair.Value as PdfReferenceHolder).Object as PdfDictionary;
                  using (Dictionary<PdfName, IPdfPrimitive>.Enumerator enumerator = pdfDictionary9.Items.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      KeyValuePair<PdfName, IPdfPrimitive> current = enumerator.Current;
                      if (pdfDictionary9 != null && current.Key.Value == "Type")
                      {
                        switch ((pdfDictionary9.Items[new PdfName("Type")] as PdfName).Value)
                        {
                          case "OCG":
                            IPdfPrimitive element = keyValuePair.Value;
                            if (this.EnableMemoryOptimization)
                              element = keyValuePair.Value.Clone(this.CrossTable);
                            pdfArray1.Insert(index1, element);
                            PdfArray pdfArray6 = new PdfArray();
                            pdfArray2.Insert(index1, element);
                            pdfArray3.Insert(index1, element);
                            optimalContentViewDictionary["Order"] = (IPdfPrimitive) pdfArray2;
                            optimalContentViewDictionary["ON"] = (IPdfPrimitive) pdfArray3;
                            PdfArray pdfArray7 = new PdfArray();
                            optimalContentViewDictionary["OFF"] = (IPdfPrimitive) pdfArray7;
                            pdfDictionary4.SetProperty("OCGs", (IPdfPrimitive) pdfArray1);
                            pdfArray6.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary4));
                            optimalContentViewDictionary["AS"] = (IPdfPrimitive) pdfArray6;
                            ++index1;
                            continue;
                          default:
                            continue;
                        }
                      }
                    }
                    break;
                  }
                }
              }
            }
          }
        }
      }
      PdfDictionary primitive = new PdfDictionary();
      primitive.Items.Add(new PdfName("OCGs"), (IPdfPrimitive) pdfArray1);
      primitive.Items.Add(new PdfName("D"), (IPdfPrimitive) optimalContentViewDictionary);
      if (this.CrossTable.Document is PdfDocument)
      {
        if (!(this.CrossTable.Document as PdfDocument).Catalog.ContainsKey("OCProperties"))
          (this.CrossTable.Document as PdfDocument).Catalog.SetProperty("OCProperties", (IPdfPrimitive) primitive);
        else
          this.ImportLayers((this.CrossTable.Document as PdfDocument).Catalog.Items[new PdfName("OCProperties")] as PdfDictionary, pdfArray1, optimalContentViewDictionary);
      }
      else if (!(this.CrossTable.Document as PdfLoadedDocument).Catalog.ContainsKey("OCProperties"))
        (this.CrossTable.Document as PdfLoadedDocument).Catalog.SetProperty("OCProperties", (IPdfPrimitive) primitive);
      else
        this.ImportLayers((this.CrossTable.Document as PdfLoadedDocument).Catalog.Items[new PdfName("OCProperties")] as PdfDictionary, pdfArray1, optimalContentViewDictionary);
    }
    bookmarks.Clear();
    destinations.Clear();
    dictionary.Clear();
    this.CrossTable.PrevReference = (List<PdfReference>) null;
    return pdfPageBase1;
  }

  private void ImportLayers(
    PdfDictionary ocPropertiesDictionary,
    PdfArray ocgArray,
    PdfDictionary optimalContentViewDictionary)
  {
    if (!ocPropertiesDictionary.ContainsKey("OCGs"))
      return;
    PdfArray pdfArray1 = ocPropertiesDictionary.Items[new PdfName("OCGs")] as PdfArray;
    if (!(ocPropertiesDictionary.Items[new PdfName("D")] is PdfDictionary pdfDictionary))
      return;
    if (pdfArray1 != null && ocgArray != null)
    {
      int count = pdfArray1.Count;
      for (int index = 0; index < ocgArray.Count; ++index)
      {
        pdfArray1.Insert(count, ocgArray[index]);
        ++count;
      }
    }
    PdfArray optimalContentView = optimalContentViewDictionary["AS"] as PdfArray;
    pdfDictionary["Order"] = (IPdfPrimitive) pdfArray1;
    pdfDictionary["ON"] = (IPdfPrimitive) pdfArray1;
    if (!pdfDictionary.ContainsKey("AS"))
      return;
    PdfArray pdfArray2 = pdfDictionary["AS"] as PdfArray;
    if (optimalContentView == null || pdfArray2 == null)
      return;
    for (int index = 0; index < optimalContentView.Count; ++index)
      pdfArray2.Insert(pdfArray2.Count, optimalContentView[index]);
  }

  private PdfDictionary ParsePdfLayers(PdfDictionary lDocLayers)
  {
    PdfDictionary pdfLayers = new PdfDictionary();
    PdfDictionary pdfDictionary = (object) (lDocLayers["OCProperties"] as PdfReferenceHolder) == null ? lDocLayers["OCProperties"] as PdfDictionary : (lDocLayers["OCProperties"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary != null)
      pdfLayers = !this.EnableMemoryOptimization ? pdfDictionary : pdfDictionary.Clone(this.CrossTable) as PdfDictionary;
    return pdfLayers;
  }

  public void Append(PdfLoadedDocument ldDoc)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    if (ldDoc.IsXFAForm && this is PdfDocument)
      ((PdfDocument) this).Form.IsXFA = true;
    if (!ldDoc.IsXFAForm && ldDoc.Form != null)
    {
      int num = ldDoc.Form.IsXFAForm ? 1 : 0;
    }
    int startIndex = 0;
    int endIndex = ldDoc.Pages.Count - 1;
    this.m_isMerging = true;
    this.ImportPageRange(ldDoc, startIndex, endIndex);
    this.MergeLayer(ldDoc);
    this.MergeAttachments(ldDoc);
  }

  private void MergeLayer(PdfLoadedDocument ldDoc)
  {
    if (!ldDoc.CrossTable.DocumentCatalog.ContainsKey("OCProperties"))
      return;
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    PdfDictionary pdfLayers = this.ParsePdfLayers(ldDoc.CrossTable.DocumentCatalog);
    if (this is PdfDocument)
    {
      PdfDocument pdfDocument = this as PdfDocument;
      if (!pdfDocument.Catalog.ContainsKey("OCProperties"))
        pdfDocument.Catalog.SetProperty("OCProperties", (IPdfPrimitive) pdfLayers);
      else
        pdfDictionary1 = PdfCrossTable.Dereference(pdfDocument.Catalog["OCProperties"]) as PdfDictionary;
    }
    else
    {
      PdfLoadedDocument pdfLoadedDocument = this as PdfLoadedDocument;
      if (!pdfLoadedDocument.Catalog.ContainsKey("OCProperties"))
        pdfLoadedDocument.Catalog.SetProperty("OCProperties", (IPdfPrimitive) pdfLayers);
      else
        pdfDictionary1 = PdfCrossTable.Dereference(pdfLoadedDocument.Catalog["OCProperties"]) as PdfDictionary;
      pdfDictionary1?.Modify();
    }
    if (pdfDictionary1 == null || pdfLayers == null)
      return;
    if (pdfDictionary1.ContainsKey("OCGs") && pdfLayers.ContainsKey("OCGs"))
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(pdfDictionary1["OCGs"]) as PdfArray;
      PdfArray primitive = PdfCrossTable.Dereference(pdfLayers["OCGs"]) as PdfArray;
      if (pdfArray != null && primitive != null)
        pdfArray.Elements.AddRange((IEnumerable<IPdfPrimitive>) primitive.Elements);
      else if (primitive != null)
        pdfDictionary1.SetProperty("OCGs", (IPdfPrimitive) primitive);
    }
    else
      pdfDictionary1.SetProperty("OCGs", (IPdfPrimitive) pdfLayers);
    if (!pdfDictionary1.ContainsKey("D") || !pdfLayers.ContainsKey("D"))
      return;
    PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary1["D"]) as PdfDictionary;
    PdfDictionary primitive1 = PdfCrossTable.Dereference(pdfLayers["D"]) as PdfDictionary;
    if (pdfDictionary2 != null && primitive1 != null)
    {
      if (pdfDictionary2.ContainsKey("Order") && primitive1.ContainsKey("Order"))
      {
        PdfArray pdfArray1 = PdfCrossTable.Dereference(pdfDictionary2["Order"]) as PdfArray;
        PdfArray pdfArray2 = PdfCrossTable.Dereference(primitive1["Order"]) as PdfArray;
        if (pdfArray1 != null && pdfArray2 != null)
          pdfArray1.Elements.AddRange((IEnumerable<IPdfPrimitive>) pdfArray2.Elements);
      }
      else if (primitive1.ContainsKey("Order"))
        pdfDictionary2.SetProperty("D", (IPdfPrimitive) (primitive1["Order"] as PdfDictionary));
      if (pdfDictionary2.ContainsKey("ON") && primitive1.ContainsKey("ON"))
      {
        PdfArray pdfArray3 = PdfCrossTable.Dereference(pdfDictionary2["ON"]) as PdfArray;
        PdfArray pdfArray4 = PdfCrossTable.Dereference(primitive1["ON"]) as PdfArray;
        if (pdfArray3 != null && pdfArray4 != null)
          pdfArray3.Elements.AddRange((IEnumerable<IPdfPrimitive>) pdfArray4.Elements);
      }
      else if (primitive1.ContainsKey("ON"))
        pdfDictionary2.SetProperty("ON", (IPdfPrimitive) (primitive1["ON"] as PdfDictionary));
      if (pdfDictionary2.ContainsKey("OFF") && primitive1.ContainsKey("OFF"))
      {
        PdfArray pdfArray5 = PdfCrossTable.Dereference(pdfDictionary2["OFF"]) as PdfArray;
        PdfArray pdfArray6 = PdfCrossTable.Dereference(primitive1["OFF"]) as PdfArray;
        if (pdfArray5 != null && pdfArray6 != null)
          pdfArray5.Elements.AddRange((IEnumerable<IPdfPrimitive>) pdfArray6.Elements);
      }
      else if (primitive1.ContainsKey("OFF"))
        pdfDictionary2.SetProperty("OFF", (IPdfPrimitive) (primitive1["OFF"] as PdfDictionary));
      if (pdfDictionary2.ContainsKey("AS") && primitive1.ContainsKey("AS"))
      {
        PdfArray pdfArray7 = PdfCrossTable.Dereference(pdfDictionary2["AS"]) as PdfArray;
        PdfArray pdfArray8 = PdfCrossTable.Dereference(primitive1["AS"]) as PdfArray;
        if (pdfArray7 == null || pdfArray8 == null || pdfArray8.Count <= 0 || pdfArray7.Count <= 0)
          return;
        PdfDictionary pdfDictionary3 = PdfCrossTable.Dereference(pdfArray8[0]) as PdfDictionary;
        PdfDictionary pdfDictionary4 = PdfCrossTable.Dereference(pdfArray7[0]) as PdfDictionary;
        if (pdfDictionary3 == null || pdfDictionary4 == null || !pdfDictionary3.ContainsKey("OCGs"))
          return;
        PdfArray pdfArray9 = PdfCrossTable.Dereference(pdfDictionary3["OCGs"]) as PdfArray;
        if (!(PdfCrossTable.Dereference(pdfDictionary4["OCGs"]) is PdfArray pdfArray10) || pdfArray9 == null)
          return;
        pdfArray10.Elements.AddRange((IEnumerable<IPdfPrimitive>) pdfArray9.Elements);
      }
      else
      {
        if (!primitive1.ContainsKey("AS"))
          return;
        pdfDictionary2.SetProperty("AS", (IPdfPrimitive) (primitive1["AS"] as PdfDictionary));
      }
    }
    else
    {
      if (primitive1 == null)
        return;
      pdfDictionary1.SetProperty("D", (IPdfPrimitive) primitive1);
    }
  }

  private bool CheckEncryption(PdfLoadedDocument ldoc)
  {
    bool flag1 = false;
    PdfDictionary trailer = ldoc.CrossTable.Trailer;
    PdfDictionary encryptorDictionary = ldoc.CrossTable.EncryptorDictionary;
    this.m_password = ldoc.Password;
    bool flag2 = true;
    if (encryptorDictionary != null && encryptorDictionary.ContainsKey("EncryptMetadata"))
      flag2 = (encryptorDictionary["EncryptMetadata"] as PdfBoolean).Value;
    if (encryptorDictionary != null && flag2)
    {
      if (this.m_password == null)
        this.m_password = string.Empty;
      PdfString key = ((trailer["ID"] ?? throw new PdfDocumentException("Unable to decrypt document without ID.")) as PdfArray)[0] as PdfString;
      PdfEncryptor pdfEncryptor = new PdfEncryptor();
      pdfEncryptor.ReadFromDictionary(encryptorDictionary);
      if (!pdfEncryptor.CheckPassword(this.m_password, key, true))
      {
        this.Close(true);
        throw new PdfDocumentException("Can't open an encrypted document. The password is invalid.");
      }
      encryptorDictionary.Encrypt = false;
      PdfSecurity security = new PdfSecurity();
      if (!this.Security.Encryptor.Encrypt)
      {
        security.Encryptor = pdfEncryptor;
        this.SetSecurity(security);
        flag1 = true;
        this.Security.Encryptor = pdfEncryptor;
      }
    }
    return flag1;
  }

  internal void OnPageSave(PdfPageBase page)
  {
    if (this.progressDelegate == null)
      return;
    switch (this)
    {
      case PdfDocument pdfDocument:
        int current = ++this.m_progressPageIndex;
        if (this.m_pageCount == 0)
          this.m_pageCount = pdfDocument.PageCount;
        this.OnSaveProgress(new ProgressEventArgs(current, this.m_pageCount));
        break;
      case PdfLoadedDocument pdfLoadedDocument:
        int index = pdfLoadedDocument.Pages.GetIndex(page);
        if (this.m_pageCount == 0)
          this.m_pageCount = pdfLoadedDocument.PageCount;
        if (index <= -1)
          break;
        this.OnSaveProgress(!pdfLoadedDocument.FileStructure.IncrementalUpdate || this.m_changedPages <= 0 ? new ProgressEventArgs(index, this.m_pageCount, ++this.m_pageProcessed) : new ProgressEventArgs(index, this.m_pageCount, ++this.m_pageProcessed, this.m_changedPages));
        break;
    }
  }

  internal void OnSaveProgress(ProgressEventArgs arguments)
  {
    if (this.progressDelegate == null)
      return;
    this.progressDelegate((object) this, arguments);
  }

  internal void SetWaterMarkResources(PdfResources templateResources, PdfResources pageResources)
  {
    PdfName key1 = new PdfName("ExtGState");
    PdfName key2 = new PdfName("Font");
    if (templateResources.ContainsKey(key1))
    {
      PdfDictionary pdfDictionary1 = templateResources.Items[key1] as PdfDictionary;
      if (pageResources.Items.ContainsKey(key1))
      {
        if (pageResources.Items[key1] is PdfDictionary pdfDictionary2)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary1.Items)
          {
            if (!pdfDictionary2.Items.ContainsKey(keyValuePair.Key))
            {
              pdfDictionary2.Items.Add(keyValuePair.Key, keyValuePair.Value);
            }
            else
            {
              PdfName key3 = new PdfName(Guid.NewGuid().ToString());
              pdfDictionary2.Items.Add(key3, keyValuePair.Value);
            }
          }
        }
      }
      else
        pageResources.Items.Add(key1, (IPdfPrimitive) pdfDictionary1);
    }
    if (!templateResources.ContainsKey(key2))
      return;
    PdfDictionary pdfDictionary3 = templateResources.Items[new PdfName("Font")] as PdfDictionary;
    if (pageResources.Items.ContainsKey(key2))
    {
      if (!(pageResources.Items[key2] is PdfDictionary pdfDictionary4))
        return;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary3.Items)
      {
        if (!pdfDictionary4.Items.ContainsKey(keyValuePair.Key))
        {
          pdfDictionary4.Items.Add(keyValuePair.Key, keyValuePair.Value);
        }
        else
        {
          PdfName key4 = new PdfName(Guid.NewGuid().ToString());
          pdfDictionary4.Items.Add(key4, keyValuePair.Value);
        }
      }
    }
    else
      pageResources.Items.Add(key2, (IPdfPrimitive) pdfDictionary3);
  }

  internal abstract PdfForm ObtainForm();

  internal void SetMainObjectCollection(PdfMainObjectCollection moc)
  {
    this.m_objects = moc != null ? moc : throw new ArgumentNullException(nameof (moc));
  }

  internal void SetSecurity(PdfSecurity security)
  {
    this.m_security = security != null ? security : throw new ArgumentNullException(nameof (security));
  }

  internal void SetCrossTable(PdfCrossTable cTable)
  {
    this.m_crossTable = cTable != null ? cTable : throw new ArgumentNullException(nameof (cTable));
  }

  internal void SetCatalog(PdfCatalog catalog)
  {
    this.m_catalog = catalog != null ? catalog : throw new ArgumentNullException(nameof (catalog));
    if (!this.m_catalog.ContainsKey("Outlines"))
      return;
    PdfReferenceHolder pdfReferenceHolder1 = this.m_catalog["Outlines"] as PdfReferenceHolder;
    PdfDictionary pdfDictionary = !(pdfReferenceHolder1 == (PdfReferenceHolder) null) ? pdfReferenceHolder1.Object as PdfDictionary : this.m_catalog["Outlines"] as PdfDictionary;
    if (pdfDictionary == null || !pdfDictionary.ContainsKey("First"))
      return;
    PdfReferenceHolder pdfReferenceHolder2 = pdfDictionary["First"] as PdfReferenceHolder;
    if (!(pdfReferenceHolder2 != (PdfReferenceHolder) null) || pdfReferenceHolder2.Object is PdfDictionary)
      return;
    pdfDictionary.Remove("First");
  }

  internal void OnDocumentSaved(DocumentSavedEventArgs args)
  {
    if (args == null)
      throw new ArgumentNullException(nameof (args));
    if (this.DocumentSaved == null)
      return;
    this.DocumentSaved((object) this, args);
  }

  internal abstract void AddFields(
    PdfLoadedDocument ldDoc,
    PdfPageBase newPage,
    List<PdfField> fields);

  internal abstract PdfPageBase ClonePage(
    PdfLoadedDocument ldDoc,
    PdfPageBase page,
    List<PdfArray> destinations);

  protected virtual void CheckFields(
    PdfLoadedDocument ldDoc,
    PdfPageBase page,
    List<PdfField> fields,
    PdfPageBase importedPage)
  {
    PdfArray annotations = page.ObtainAnnotations();
    PdfLoadedForm form = ldDoc.Form;
    PdfName key = new PdfName("Kids");
    if (annotations == null || form == null)
      return;
    int index = 0;
    for (int count = form.Fields.Count; index < count; ++index)
    {
      PdfField field = form.Fields[index];
      if (!field.Dictionary.ContainsKey("removed") || !(field.Dictionary["removed"] is PdfBoolean pdfBoolean) || !pdfBoolean.Value)
      {
        bool flag = false;
        if (field is PdfLoadedSignatureField)
          flag = true;
        PdfCollection pdfCollection = (PdfCollection) null;
        if (field.Dictionary.ContainsKey(key) && (field.Dictionary[key] as PdfArray).Count > 0 && !flag)
        {
          switch (field)
          {
            case PdfLoadedButtonField _ when (field as PdfLoadedButtonField).Items.Count > 0:
              pdfCollection = (PdfCollection) (field as PdfLoadedButtonField).Items;
              break;
            case PdfLoadedCheckBoxField _ when (field as PdfLoadedCheckBoxField).Items.Count > 0:
              pdfCollection = (PdfCollection) (field as PdfLoadedCheckBoxField).Items;
              break;
            case PdfLoadedComboBoxField _ when (field as PdfLoadedComboBoxField).Items.Count > 0:
              pdfCollection = (PdfCollection) (field as PdfLoadedComboBoxField).Items;
              break;
            case PdfLoadedListBoxField _ when (field as PdfLoadedListBoxField).Items.Count > 0:
              pdfCollection = (PdfCollection) (field as PdfLoadedListBoxField).Items;
              break;
            case PdfLoadedRadioButtonListField _ when (field as PdfLoadedRadioButtonListField).Items.Count > 0:
              pdfCollection = (PdfCollection) (field as PdfLoadedRadioButtonListField).Items;
              break;
            case PdfLoadedTextBoxField _ when (field as PdfLoadedTextBoxField).Items.Count > 0:
              pdfCollection = (PdfCollection) (field as PdfLoadedTextBoxField).Items;
              break;
          }
        }
        if (this.EnableMemoryOptimization)
        {
          if (pdfCollection != null)
          {
            foreach (PdfLoadedFieldItem pdfLoadedFieldItem in pdfCollection)
            {
              if (pdfLoadedFieldItem.Page != null && pdfLoadedFieldItem.Page == page)
              {
                fields.Add(field);
                break;
              }
            }
            if (this.m_annotCount != 0 && fields.Count == this.m_annotCount)
              break;
          }
          else if (field.Page == page)
          {
            fields.Add(field);
            if (this.m_annotCount != 0 && fields.Count == this.m_annotCount)
              break;
          }
        }
        else if (field.Page == page)
        {
          if (pdfCollection != null)
          {
            foreach (PdfLoadedFieldItem pdfLoadedFieldItem in pdfCollection)
            {
              if (pdfLoadedFieldItem.Page != null && pdfLoadedFieldItem.Page == page)
                pdfLoadedFieldItem.Page = importedPage;
            }
          }
          fields.Add(field);
          this.m_addedField.Add(field.Name);
          if (this.m_annotCount != 0 && fields.Count == this.m_annotCount)
            break;
        }
        else if (field is PdfLoadedTextBoxField)
        {
          PdfLoadedTextBoxField loadedTextBoxField = field as PdfLoadedTextBoxField;
          if (loadedTextBoxField.Kids != null)
          {
            foreach (PdfLoadedTexBoxItem loadedTexBoxItem in (PdfCollection) loadedTextBoxField.Items)
            {
              if (loadedTexBoxItem.Page != null && loadedTexBoxItem.Page == page)
              {
                loadedTexBoxItem.Page = importedPage;
                field.Page = importedPage;
                if (!this.m_fieldKids.ContainsKey(field.Name) && !this.m_addedField.Contains(field.Name))
                {
                  this.m_fieldKids.Add(field.Name, field);
                  fields.Add(field);
                  this.m_addedField.Add(field.Name);
                }
                this.m_isKidsPage = true;
              }
            }
          }
        }
      }
    }
  }

  private void MergeAttachments(PdfLoadedDocument ldDoc)
  {
    PdfCatalogNames names = ldDoc.Catalog.Names;
    if (names != null)
    {
      if (ldDoc.Security.EncryptOnlyAttachment)
      {
        PdfAttachmentCollection attachments = ldDoc.Attachments;
      }
      this.Catalog.CreateNamesIfNone();
      if (this.EnableMemoryOptimization)
        this.Catalog.Names.MergeEmbedded(names, this.m_crossTable);
      else
        this.Catalog.Names.MergeEmbedded(names, (PdfCrossTable) null);
    }
    this.m_crossTable.PrevReference = (List<PdfReference>) null;
  }

  private PdfNamedDestination GetNamedDestination(PdfNamedDestination nDest, PdfPageBase page)
  {
    return new PdfNamedDestination(nDest.Title)
    {
      Destination = this.GetDestination(page, nDest.Destination)
    };
  }

  private PdfDestination GetDestination(PdfPageBase page, PdfDestination dest)
  {
    return new PdfDestination(page, dest.Location)
    {
      Mode = dest.Mode,
      Zoom = dest.Zoom
    };
  }

  private void ExportBookmarks(
    PdfLoadedDocument ldDoc,
    List<PdfBookmarkBase> bookmarks,
    int pageCount,
    Dictionary<PdfPageBase, object> bookmarkshash)
  {
    PdfBookmarkBase bookmarkBase = this.Bookmarks;
    PdfBookmarkBase bookmarks1 = ldDoc.Bookmarks;
    List<string> stringList = (List<string>) null;
    if (bookmarks1 == null)
      return;
    if (bookmarkBase == null)
      bookmarkBase = (this as PdfLoadedDocument).CreateBookmarkRoot();
    Stack<PdfDocumentBase.NodeInfo> nodeInfoStack = new Stack<PdfDocumentBase.NodeInfo>();
    PdfDocumentBase.NodeInfo nodeInfo = new PdfDocumentBase.NodeInfo(bookmarkBase, bookmarks1.List);
    if (ldDoc.Pages.Count != pageCount)
    {
      nodeInfo = new PdfDocumentBase.NodeInfo(bookmarkBase, bookmarks);
      stringList = new List<string>();
    }
    do
    {
      while (nodeInfo.Index < nodeInfo.Kids.Count)
      {
        PdfBookmarkBase kid = nodeInfo.Kids[nodeInfo.Index];
        if (bookmarks.Contains(kid) && stringList != null && !stringList.Contains((kid as PdfBookmark).Title))
        {
          PdfBookmark pdfBookmark1 = kid as PdfBookmark;
          PdfBookmark pdfBookmark2 = bookmarkBase.Add(pdfBookmark1.Title);
          pdfBookmark2.TextStyle = pdfBookmark1.TextStyle;
          pdfBookmark2.Color = pdfBookmark1.Color;
          PdfDestination destination = pdfBookmark1.Destination;
          PdfNamedDestination namedDestination1 = pdfBookmark1.NamedDestination;
          if (namedDestination1 != null)
          {
            if (namedDestination1.Destination != null)
            {
              PdfPageBase page1 = namedDestination1.Destination.Page;
              if (ldDoc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) page1.Dictionary) && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page1.Dictionary] != null && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page1.Dictionary] is PdfPageBase page2)
              {
                PdfNamedDestination namedDestination2 = this.GetNamedDestination(namedDestination1, page2);
                pdfBookmark2.NamedDestination = namedDestination2;
                pdfBookmark2.Dictionary.Remove("C");
                this.m_namedDetinations.Add(namedDestination2);
              }
            }
          }
          else if (destination != null && this.EnableMemoryOptimization)
          {
            PdfPageBase page3 = destination.Page;
            if (ldDoc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) page3.Dictionary) && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page3.Dictionary] != null)
            {
              if (ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page3.Dictionary] is PdfPageBase page4)
                pdfBookmark2.Destination = new PdfDestination(page4, destination.Location)
                {
                  Mode = destination.Mode,
                  Bounds = destination.Bounds,
                  Zoom = destination.Zoom
                };
            }
            else
              pdfBookmark2.Dictionary.Remove("A");
          }
          else if (destination != null)
          {
            PdfPageBase page = destination.Page;
            pdfBookmark2.Destination = new PdfDestination(ldDoc.CrossTable.PageCorrespondance[((IPdfWrapper) page).Element] as PdfPageBase, destination.Location)
            {
              Mode = destination.Mode,
              Bounds = destination.Bounds,
              Zoom = destination.Zoom
            };
          }
          bookmarkBase = (PdfBookmarkBase) pdfBookmark2;
          stringList.Add(pdfBookmark2.Title);
        }
        else
        {
          PdfBookmark pdfBookmark3 = kid as PdfBookmark;
          PdfDestination destination = pdfBookmark3.Destination;
          PdfPageBase pdfPageBase = (PdfPageBase) null;
          PdfNamedDestination namedDestination3 = pdfBookmark3.NamedDestination;
          if (ldDoc.Pages.Count == pageCount)
          {
            PdfBookmark pdfBookmark4 = bookmarkBase.Add(pdfBookmark3.Title);
            if (!this.EnableMemoryOptimization && pdfBookmark3.Dictionary.ContainsKey("A"))
              pdfBookmark4.Dictionary.SetProperty("A", pdfBookmark3.Dictionary["A"]);
            pdfBookmark4.TextStyle = pdfBookmark3.TextStyle;
            pdfBookmark4.Color = pdfBookmark3.Color;
            if (namedDestination3 != null)
            {
              if (namedDestination3.Destination != null)
              {
                PdfPageBase page5 = namedDestination3.Destination.Page;
                if (ldDoc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) page5.Dictionary) && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page5.Dictionary] != null && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page5.Dictionary] is PdfPageBase page6)
                {
                  PdfNamedDestination namedDestination4 = this.GetNamedDestination(namedDestination3, page6);
                  this.m_namedDetinations.Add(namedDestination4);
                  pdfBookmark4.NamedDestination = namedDestination4;
                  pdfBookmark4.Dictionary.Remove("C");
                }
              }
            }
            else if (destination != null)
            {
              PdfPageBase page7 = destination.Page;
              if (ldDoc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) page7.Dictionary) && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page7.Dictionary] != null)
              {
                if (ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) page7.Dictionary] is PdfPageBase page8)
                  pdfBookmark4.Destination = new PdfDestination(page8, destination.Location)
                  {
                    Mode = destination.Mode,
                    Bounds = destination.Bounds,
                    Zoom = destination.Zoom
                  };
              }
              else
                pdfBookmark4.Dictionary.Remove("A");
            }
            bookmarkBase = (PdfBookmarkBase) pdfBookmark4;
          }
          else if (destination != null && destination.Page != null && ldDoc.Pages.IndexOf(destination.Page) < pageCount && ldDoc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) destination.Page.Dictionary) && ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) destination.Page.Dictionary] != null)
          {
            pdfPageBase = destination.Page;
            PdfPageBase page = ldDoc.CrossTable.PageCorrespondance[(IPdfPrimitive) destination.Page.Dictionary] as PdfPageBase;
            PdfBookmark pdfBookmark5 = bookmarkBase.Add(pdfBookmark3.Title);
            if (pdfBookmark3.Dictionary.ContainsKey("A"))
            {
              if (this.EnableMemoryOptimization)
              {
                IPdfPrimitive primitive = pdfBookmark3.Dictionary["A"].Clone(this.m_crossTable);
                pdfBookmark5.Dictionary.SetProperty("A", primitive);
              }
              else
                pdfBookmark5.Dictionary.SetProperty("A", pdfBookmark3.Dictionary["A"]);
            }
            if (page != null)
            {
              pdfBookmark5.TextStyle = pdfBookmark3.TextStyle;
              pdfBookmark5.Color = pdfBookmark3.Color;
              pdfBookmark5.Destination = new PdfDestination(page, destination.Location)
              {
                Mode = destination.Mode,
                Bounds = destination.Bounds,
                Zoom = destination.Zoom
              };
              bookmarkBase = (PdfBookmarkBase) pdfBookmark5;
            }
          }
        }
        ++nodeInfo.Index;
        if (kid.Count > 0)
        {
          nodeInfoStack.Push(nodeInfo);
          nodeInfo = new PdfDocumentBase.NodeInfo(bookmarkBase, kid.List);
        }
        else
          bookmarkBase = nodeInfo.Base;
      }
      if (nodeInfoStack.Count > 0)
      {
        nodeInfo = nodeInfoStack.Pop();
        while (nodeInfo.Index == nodeInfo.Kids.Count && nodeInfoStack.Count > 0)
          nodeInfo = nodeInfoStack.Pop();
        bookmarkBase = nodeInfo.Base;
      }
    }
    while (nodeInfo.Index < nodeInfo.Kids.Count);
    stringList?.Clear();
  }

  private void ExportNamedDestination(PdfLoadedDocument doc)
  {
    if (doc.NamedDestinationCollection != null)
    {
      foreach (object namedDestination in (IEnumerable) doc.NamedDestinationCollection)
      {
        if (namedDestination is PdfNamedDestination nDest && nDest.Destination != null)
        {
          PdfPageBase page1 = nDest.Destination.Page;
          if (doc.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) page1.Dictionary) && doc.CrossTable.PageCorrespondance[(IPdfPrimitive) page1.Dictionary] != null && doc.CrossTable.PageCorrespondance[((IPdfWrapper) page1).Element] != null && doc.CrossTable.PageCorrespondance[(IPdfPrimitive) page1.Dictionary] is PdfPageBase page2 && page2.Equals(doc.CrossTable.PageCorrespondance[((IPdfWrapper) page1).Element]))
            this.m_namedDetinations.Add(this.GetNamedDestination(nDest, page2));
        }
      }
    }
    doc.CrossTable.PageCorrespondance = (Dictionary<IPdfPrimitive, object>) null;
  }

  private void MarkBookmarks(List<object> pageBookmarks, List<PdfBookmarkBase> bookmarks)
  {
    if (pageBookmarks == null)
      return;
    foreach (object pageBookmark in pageBookmarks)
    {
      if (!(pageBookmark is PdfBookmarkBase))
        throw new Exception("Type not specified properly");
      bookmarks.Add(pageBookmark as PdfBookmarkBase);
    }
  }

  private void MarkBookmarks(PdfBookmarkBase bookmarkBase, List<PdfBookmarkBase> bookmarks)
  {
    bookmarks.Add(bookmarkBase);
  }

  private void FixDestinations(
    Dictionary<IPdfPrimitive, object> pageCorrespondance,
    List<PdfArray> destinations)
  {
    PdfNull element1 = new PdfNull();
    int index = 0;
    for (int count = destinations.Count; index < count; ++index)
    {
      PdfArray destination = destinations[index];
      if (destination != null)
      {
        PdfReferenceHolder pdfReferenceHolder = destination[0] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          if (pdfReferenceHolder.Object is PdfDictionary key && pageCorrespondance.ContainsKey((IPdfPrimitive) key) && pageCorrespondance[(IPdfPrimitive) key] != null)
          {
            PdfPageBase wrapper = pageCorrespondance[(IPdfPrimitive) key] as PdfPageBase;
            destination.RemoveAt(0);
            if (wrapper != null)
            {
              PdfReferenceHolder element2 = new PdfReferenceHolder((IPdfWrapper) wrapper);
              destination.Insert(0, (IPdfPrimitive) element2);
            }
            else
              destination.Insert(0, (IPdfPrimitive) element1);
          }
          else if (key != null && pageCorrespondance.ContainsKey((IPdfPrimitive) key) && pageCorrespondance[(IPdfPrimitive) key] == null)
          {
            destination.RemoveAt(0);
            destination.Insert(0, (IPdfPrimitive) element1);
          }
        }
      }
    }
  }

  private void ResetProgress()
  {
    if (!(this is PdfDocument))
      return;
    (this as PdfDocument).Sections.ResetProgress();
  }

  private void SetProgress()
  {
    if (!(this is PdfDocument))
      return;
    (this as PdfDocument).Sections.SetProgress();
  }

  internal string CreateHashFromStream(byte[] streamBytes)
  {
    HashAlgorithm hashAlgorithm = this.GetHashAlgorithm((Func<HashAlgorithm>) (() => (HashAlgorithm) new SHA256Managed()), (Func<HashAlgorithm>) (() => HashAlgorithm.Create("System.Security.Cryptography.SHA256CryptoServiceProvider")));
    byte[] hash = hashAlgorithm.ComputeHash(streamBytes);
    hashAlgorithm.Clear();
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < hash.Length; ++index)
      stringBuilder.Append(hash[index].ToString("x2"));
    return stringBuilder.ToString();
  }

  internal HashAlgorithm GetHashAlgorithm(
    Func<HashAlgorithm> createStandardHashAlgorithmCallback,
    Func<HashAlgorithm> createFipsHashAlgorithmCallback)
  {
    if (!CryptoConfig.AllowOnlyFipsAlgorithms)
      return createStandardHashAlgorithmCallback();
    try
    {
      return createFipsHashAlgorithmCallback();
    }
    catch (PlatformNotSupportedException ex)
    {
      throw new InvalidOperationException(ex.Message, (Exception) ex);
    }
  }

  internal delegate void DocumentSavedEventHandler(object sender, DocumentSavedEventArgs args);

  public delegate void ProgressEventHandler(object sender, ProgressEventArgs arguments);

  private class NodeInfo
  {
    public int Index;
    public PdfBookmarkBase Base;
    public List<PdfBookmarkBase> Kids;

    public NodeInfo(PdfBookmarkBase bookmarkBase, List<PdfBookmarkBase> kids)
    {
      if (bookmarkBase == null)
        throw new ArgumentNullException(nameof (bookmarkBase));
      if (kids == null)
        throw new ArgumentNullException(nameof (kids));
      this.Base = bookmarkBase;
      this.Kids = kids;
    }
  }
}
