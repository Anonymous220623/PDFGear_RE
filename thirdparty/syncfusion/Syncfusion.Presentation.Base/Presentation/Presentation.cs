// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Presentation
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.OfficeChart;
using Syncfusion.Presentation.CommentImplementation;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Rendering;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;

#nullable disable
namespace Syncfusion.Presentation;

public sealed class Presentation : IPresentation, IDisposable
{
  private const string PresentationContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml";
  internal const string DEF_SUMMARY_INFO = "\u0005SummaryInformation";
  internal const string DEF_DOCUMENT_SUMMARY_INFO = "\u0005DocumentSummaryInformation";
  private Graphics _graphics;
  private Graphics _internalGraphics;
  private RendererBase _renderer;
  private bool _IsInternalGraphics;
  private IOfficeChartToImageConverter _chartToImageConverter;
  private bool _hasChartToImage;
  internal bool HasThumbnail;
  internal bool IsStyleChanged;
  internal string PresentationName;
  internal byte IsPdfConversion;
  private FileDataHolder _dataHolder;
  private Dictionary<string, string> _handoutList;
  private bool _showSpecialPlsOnTitleSld = true;
  private bool _created;
  private bool _sourcePresentation;
  private int _firstSlideNum = 1;
  private bool _isEmbedTrueTypeFont;
  private bool _rightToLeft;
  private Dictionary<string, string> _defaultContentType;
  private Dictionary<string, string> _overrideContentType;
  private RelationCollection _topRelation;
  private MasterSlides _masterCollection;
  private Dictionary<string, string> _masterList;
  private List<EmbeddedFont> _embeddedFontList;
  private Dictionary<string, string> _notesList;
  private Syncfusion.Presentation.NotesSize _notesSize;
  private Syncfusion.Presentation.SlideImplementation.Slides _slideCollection;
  private Dictionary<string, string> _slideList;
  private Dictionary<string, ISlide> _slidesFromInputFile;
  private Syncfusion.Presentation.SlideSize _slideSize;
  private Dictionary<string, int> _styleList;
  private HandoutMaster _handoutMaster;
  private int _imageCount;
  private Dictionary<string, LayoutSlide> _slideLayout;
  private bool _isDisposed;
  private Dictionary<string, TableStyle> _tableStyleList;
  private Syncfusion.Presentation.RichText.TextBody _defaultTextStyle;
  private Dictionary<string, Stream> _preservedElements;
  private Slide _exportingSilde;
  private Syncfusion.Presentation.BuiltInDocumentProperties _builtInDocumentProperties;
  private Syncfusion.Presentation.CustomDocumentProperties _customDocumentProperties;
  private List<int> _slideIdList;
  private int _smartArtCount;
  private string _password;
  private NotesMasterSlide _notesMaster;
  private int _notesSlideCount;
  internal uint _customId;
  private bool? _hasDocumentProperties;
  private NotesSlide _exportingNotesSilde;
  private bool _markAsFinal;
  private string _lastView;
  private int _oleObjectCount;
  private int _excelCount;
  private Stream _vbaProject;
  private FormatType _formatType;
  private bool _isEncrypted;
  private int _slideLayoutCount;
  private int _themeCount;
  private int _slideCount;
  private Syncfusion.Presentation.SlideImplementation.Sections _sections;
  private CommentAuthors _commentAuthors;
  private int _commentCount;
  private string _defaultStyleId;
  private string syncfusionLicense = string.Empty;
  private WriteProtection _writeProtection;
  private FontSettings m_fontSettings;
  private List<Dictionary<string, RectangleF>> _hyperlinks;

  internal Presentation()
  {
    this.syncfusionLicense = FusionLicenseProvider.GetLicenseType(Platform.FileFormats);
    this._masterCollection = new MasterSlides(this);
    this._slideCollection = new Syncfusion.Presentation.SlideImplementation.Slides(this);
    this._slideSize = new Syncfusion.Presentation.SlideSize();
    this._notesSize = new Syncfusion.Presentation.NotesSize();
    this._topRelation = new RelationCollection();
    this._defaultContentType = new Dictionary<string, string>();
    this._overrideContentType = new Dictionary<string, string>();
    this._builtInDocumentProperties = new Syncfusion.Presentation.BuiltInDocumentProperties(this);
    this._customDocumentProperties = new Syncfusion.Presentation.CustomDocumentProperties(this);
    this._customId = 2147483648U /*0x80000000*/;
  }

  internal WriteProtection WriteProtection
  {
    get => this._writeProtection ?? (this._writeProtection = new WriteProtection());
  }

  public ISections Sections
  {
    get => (ISections) this._sections ?? (ISections) (this._sections = new Syncfusion.Presentation.SlideImplementation.Sections(this));
  }

  public FontSettings FontSettings
  {
    get => this.m_fontSettings ?? (this.m_fontSettings = new FontSettings());
  }

  internal List<Dictionary<string, RectangleF>> DocumentLinkHyperlinks
  {
    get => this._hyperlinks ?? (this._hyperlinks = new List<Dictionary<string, RectangleF>>());
  }

  internal CommentAuthors CommentAuthors
  {
    get => this._commentAuthors ?? (this._commentAuthors = new CommentAuthors(this));
  }

  public IMasterSlides Masters => (IMasterSlides) this._masterCollection;

  private bool CheckCustomXml(RelationCollection topRelation)
  {
    foreach (Relation relation in topRelation.GetRelationList())
    {
      if (relation.Target.Contains("custom.xml"))
        return true;
    }
    return false;
  }

  public ISlides Slides => (ISlides) this._slideCollection;

  public bool Final
  {
    get => this._markAsFinal;
    set
    {
      this._markAsFinal = value;
      if (this._markAsFinal)
      {
        this._builtInDocumentProperties.ContentStatus = nameof (Final);
        RelationCollection relationCollection = new RelationCollection();
        RelationCollection topRelation = !this.Created ? this._dataHolder.TopRelations : this.TopRelation;
        string id = (string) null;
        if (this.CheckCustomXml(topRelation))
        {
          foreach (Relation relation in topRelation.GetRelationList())
          {
            if (relation.Target.Contains("custom.xml"))
              id = relation.Id;
          }
        }
        else
          id = Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(topRelation);
        Relation relation1 = new Relation(id, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties", "docProps/custom.xml", (string) null);
        topRelation.Add(relation1.Id, relation1);
        this.AddOverrideContentType("/docProps/custom.xml", "application/vnd.openxmlformats-officedocument.custom-properties+xml");
        if (this._customDocumentProperties.Contains("_MarkAsFinal"))
          return;
        this._customDocumentProperties.Add("_MarkAsFinal");
        this._customDocumentProperties[this._customDocumentProperties.Count - 1].Boolean = true;
      }
      else
      {
        RelationCollection topRelations = this._dataHolder.TopRelations;
        if (!this.CheckCustomXml(topRelations))
          return;
        foreach (Relation relation in topRelations.GetRelationList())
        {
          if (relation.Target.Contains("custom.xml") && this._customDocumentProperties.Contains("_MarkAsFinal"))
          {
            List<DocumentPropertyImpl> documentPropertyImplList = new List<DocumentPropertyImpl>((IEnumerable<DocumentPropertyImpl>) this._customDocumentProperties.GetDocumentPropertyList());
            foreach (DocumentPropertyImpl documentPropertyImpl in documentPropertyImplList)
            {
              if (documentPropertyImpl.Name.Contains("_MarkAsFinal"))
              {
                documentPropertyImpl.Boolean = false;
                break;
              }
            }
            documentPropertyImplList.Clear();
            break;
          }
        }
      }
    }
  }

  public IOfficeChartToImageConverter ChartToImageConverter
  {
    get => this._chartToImageConverter;
    set
    {
      this._chartToImageConverter = value;
      this._hasChartToImage = true;
    }
  }

  internal bool HasChartToImageConverter
  {
    get => this._hasChartToImage;
    set => this._hasChartToImage = value;
  }

  internal bool ShowSpecialPlsOnTitleSld
  {
    get => this._showSpecialPlsOnTitleSld;
    set => this._showSpecialPlsOnTitleSld = value;
  }

  internal Dictionary<string, TableStyle> TableStyleList
  {
    get => this._tableStyleList ?? (this._tableStyleList = new Dictionary<string, TableStyle>());
  }

  internal string LastView
  {
    get => this._lastView;
    set => this._lastView = value;
  }

  internal Syncfusion.Presentation.RichText.TextBody DefaultTextStyle
  {
    get => this._defaultTextStyle;
    set => this._defaultTextStyle = value;
  }

  internal string DefaultTableStyle
  {
    get => this._defaultStyleId;
    set => this._defaultStyleId = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal FileDataHolder DataHolder
  {
    get => this._dataHolder ?? (this._dataHolder = new FileDataHolder(this));
  }

  internal bool HasDocumentProperties
  {
    get => this._hasDocumentProperties.HasValue && this._hasDocumentProperties.Value;
    set
    {
      if (this._hasDocumentProperties.HasValue)
        return;
      this._hasDocumentProperties = new bool?(value);
    }
  }

  public IBuiltInDocumentProperties BuiltInDocumentProperties
  {
    get => (IBuiltInDocumentProperties) this._builtInDocumentProperties;
  }

  public ICustomDocumentProperties CustomDocumentProperties
  {
    get => (ICustomDocumentProperties) this._customDocumentProperties;
  }

  internal int FirstSlideNumber
  {
    get => this._firstSlideNum;
    set => this._firstSlideNum = value;
  }

  internal bool IsEmbedTrueTypeFont
  {
    get => this._isEmbedTrueTypeFont;
    set => this._isEmbedTrueTypeFont = value;
  }

  internal bool RightToLeftView
  {
    get => this._rightToLeft;
    set => this._rightToLeft = value;
  }

  internal Dictionary<string, string> DefaultContentType => this._defaultContentType;

  internal Dictionary<string, string> HandoutList
  {
    get => this._handoutList;
    set => this._handoutList = value;
  }

  internal Dictionary<string, string> MasterList
  {
    get => this._masterList;
    set => this._masterList = value;
  }

  internal List<EmbeddedFont> EmbeddedFontList
  {
    get => this._embeddedFontList ?? (this._embeddedFontList = new List<EmbeddedFont>());
    set => this._embeddedFontList = value;
  }

  internal Dictionary<string, string> NotesList
  {
    get => this._notesList;
    set => this._notesList = value;
  }

  internal INotesSize NotesSize => (INotesSize) this._notesSize;

  internal Dictionary<string, string> OverrideContentType => this._overrideContentType;

  internal bool Created => this._created;

  internal bool IsSourcePresentation
  {
    get => this._sourcePresentation;
    set => this._sourcePresentation = value;
  }

  internal int SlideCount
  {
    get => this._slideCount;
    set => this._slideCount = value;
  }

  internal List<int> SlideIdList
  {
    get
    {
      if (this._slideIdList == null)
        this._slideIdList = new List<int>();
      return this._slideIdList;
    }
  }

  internal int SlideLayoutCount
  {
    get => this._slideLayoutCount;
    set => this._slideLayoutCount = value;
  }

  internal Dictionary<string, string> SlideList
  {
    get => this._slideList ?? (this._slideList = new Dictionary<string, string>());
    set => this._slideList = value;
  }

  internal Dictionary<string, ISlide> SlidesFromInputFile
  {
    get
    {
      return this._slidesFromInputFile ?? (this._slidesFromInputFile = new Dictionary<string, ISlide>());
    }
    set => this._slidesFromInputFile = value;
  }

  public ISlideSize SlideSize => (ISlideSize) this._slideSize;

  internal Dictionary<string, int> StyleList
  {
    get => this._styleList ?? (this._styleList = new Dictionary<string, int>());
  }

  internal Theme Theme => ((MasterSlide) this._masterCollection[0]).Theme;

  internal int ThemeCount
  {
    get => this._themeCount;
    set => this._themeCount = value;
  }

  internal RelationCollection TopRelation
  {
    get => this._topRelation;
    set => this._topRelation = value;
  }

  internal Graphics Graphics
  {
    get => this._graphics;
    set => this._graphics = value;
  }

  internal RendererBase Renderer
  {
    get => this._renderer;
    set => this._renderer = value;
  }

  internal Graphics InternalGraphics
  {
    get => this._internalGraphics;
    set => this._internalGraphics = value;
  }

  internal bool IsInternalGraphics
  {
    get => this._IsInternalGraphics;
    set => this._IsInternalGraphics = value;
  }

  public static IPresentation Create()
  {
    Syncfusion.Presentation.Presentation presentation = new Syncfusion.Presentation.Presentation();
    presentation._created = true;
    presentation.IntializeDefaultCollections();
    presentation.DataHolder.AddDefaultContent();
    presentation.HasDocumentProperties = true;
    presentation.DataHolder.ParsePresentationRelation();
    presentation.DataHolder.ParsePresentation();
    presentation.DataHolder.ParseMasterSlides();
    presentation.DataHolder.ParseViewProperties();
    presentation.DataHolder.RemoveItemFromArchieve();
    return (IPresentation) presentation;
  }

  public static IPresentation Open(string fileName)
  {
    Stream source = (Stream) File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    Stream stream = (Stream) new MemoryStream();
    Syncfusion.Presentation.Presentation.CopyTo(source, stream);
    stream.Position = 0L;
    source.Dispose();
    return Syncfusion.Presentation.Presentation.OpenAsStream(stream, (string) null, Path.GetExtension(fileName).ToLower());
  }

  internal static void CopyTo(Stream source, Stream destination)
  {
    byte[] buffer = new byte[source.Length];
    source.Position = 0L;
    int count;
    while ((count = source.Read(buffer, 0, buffer.Length)) != 0)
      destination.Write(buffer, 0, count);
  }

  private void CheckFormatType(string fileExtension)
  {
    switch (fileExtension)
    {
      case ".pptm":
        this._formatType = FormatType.Pptm;
        break;
      case ".pptx":
        this._formatType = FormatType.Pptx;
        break;
      case ".potx":
        this._formatType = FormatType.Potx;
        break;
      case ".potm":
        this._formatType = FormatType.Potm;
        break;
      default:
        throw new Exception($"The {fileExtension} file format is not supported");
    }
  }

  public static IPresentation Open(Stream fileStream)
  {
    Stream stream = (Stream) new MemoryStream();
    Syncfusion.Presentation.Presentation.CopyTo(fileStream, stream);
    stream.Position = 0L;
    fileStream.Dispose();
    return Syncfusion.Presentation.Presentation.OpenAsStream(stream, (string) null, (string) null);
  }

  public void Save(string fileName)
  {
    Stream fileStream = (Stream) File.Create(fileName);
    this.SaveAsStream(fileStream);
    fileStream.Close();
  }

  public void Save(Stream stream) => this.SaveAsStream(stream);

  public IPresentation Clone()
  {
    Syncfusion.Presentation.Presentation presentation = (Syncfusion.Presentation.Presentation) this.MemberwiseClone();
    if (this._dataHolder != null)
    {
      presentation._dataHolder = this._dataHolder.Clone();
      presentation._dataHolder.SetParent(presentation);
    }
    if (this._defaultContentType != null)
      presentation._defaultContentType = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._defaultContentType);
    if (this._defaultTextStyle != null)
    {
      presentation._defaultTextStyle = this._defaultTextStyle.Clone();
      presentation._defaultTextStyle.SetParent(presentation);
    }
    if (this._exportingSilde != null)
      presentation._exportingSilde = (Slide) this._exportingSilde.Clone();
    presentation._graphics = (Graphics) null;
    presentation._internalGraphics = (Graphics) null;
    if (this._builtInDocumentProperties != null)
    {
      presentation._builtInDocumentProperties = (Syncfusion.Presentation.BuiltInDocumentProperties) this._builtInDocumentProperties.Clone();
      presentation._builtInDocumentProperties.SetParent(presentation);
    }
    if (this._customDocumentProperties != null)
    {
      presentation._customDocumentProperties = this._customDocumentProperties.Clone();
      presentation._customDocumentProperties.SetParent(presentation);
    }
    if (this._handoutList != null)
      presentation._handoutList = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._handoutList);
    if (this._handoutMaster != null)
    {
      presentation._handoutMaster = this._handoutMaster.Clone();
      presentation._handoutMaster.SetParent(presentation);
    }
    if (this._notesMaster != null)
    {
      presentation._notesMaster = this._notesMaster.Clone();
      presentation._notesMaster.SetParent(presentation);
    }
    presentation._masterCollection = this._masterCollection.Clone();
    presentation._masterCollection.SetParent(presentation);
    presentation._masterList = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._masterList);
    if (this._notesList != null)
      presentation._notesList = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._notesList);
    presentation._notesSize = this._notesSize.Clone();
    presentation._overrideContentType = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._overrideContentType);
    presentation._preservedElements = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._preservedElements);
    presentation._slideCollection = this._slideCollection.Clone();
    presentation._slideCollection.SetParent(presentation);
    presentation._slideLayout = this.CloneSlideLayout(presentation);
    presentation._slideList = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._slideList);
    presentation._slideSize = this._slideSize.Clone();
    if (this._styleList != null)
      presentation._styleList = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(this._styleList);
    if (this._tableStyleList != null)
      presentation._tableStyleList = this.CloneTableStyleList(presentation);
    presentation._topRelation = this._topRelation.Clone();
    if (this._vbaProject != null)
      presentation._vbaProject = (Stream) new MemoryStream(Syncfusion.Presentation.Drawing.Helper.CloneByteArray(((MemoryStream) this._vbaProject).ToArray()));
    if (this._sections != null)
    {
      presentation._sections = this._sections.Clone();
      presentation._sections.SetParent(presentation);
    }
    return (IPresentation) presentation;
  }

  private Dictionary<string, TableStyle> CloneTableStyleList(Syncfusion.Presentation.Presentation newParent)
  {
    Dictionary<string, TableStyle> dictionary = new Dictionary<string, TableStyle>();
    foreach (KeyValuePair<string, TableStyle> tableStyle1 in this._tableStyleList)
    {
      TableStyle tableStyle2 = tableStyle1.Value.Clone();
      tableStyle2.SetParent(newParent);
      dictionary.Add(tableStyle1.Key, tableStyle2);
    }
    return dictionary;
  }

  private Dictionary<string, LayoutSlide> CloneSlideLayout(Syncfusion.Presentation.Presentation newParent)
  {
    Dictionary<string, LayoutSlide> dictionary = new Dictionary<string, LayoutSlide>();
    foreach (KeyValuePair<string, LayoutSlide> keyValuePair in this._slideLayout)
    {
      LayoutSlide layoutSlide = keyValuePair.Value.Clone();
      layoutSlide.SetParent(newParent);
      layoutSlide.SetMaster(layoutSlide.GetMasterSlide(newParent));
      dictionary.Add(keyValuePair.Key, layoutSlide);
    }
    return dictionary;
  }

  public void Save(string fileName, FormatType formatType, HttpResponse response)
  {
    this._formatType = formatType;
    this.PrepareResponse(fileName, response);
    this.SaveAsStream(response.OutputStream);
    response.End();
  }

  public static IPresentation Open(string fileName, string password)
  {
    Stream source = (Stream) File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    Stream stream = (Stream) new MemoryStream();
    Syncfusion.Presentation.Presentation.CopyTo(source, stream);
    stream.Position = 0L;
    source.Dispose();
    return Syncfusion.Presentation.Presentation.OpenAsStream(stream, password, Path.GetExtension(fileName).ToLower());
  }

  public static IPresentation Open(Stream stream, string password)
  {
    Stream stream1 = (Stream) new MemoryStream();
    Syncfusion.Presentation.Presentation.CopyTo(stream, stream1);
    stream1.Position = 0L;
    stream.Dispose();
    return Syncfusion.Presentation.Presentation.OpenAsStream(stream1, password, (string) null);
  }

  public void Encrypt(string password)
  {
    this._password = !string.IsNullOrEmpty(password) ? password : throw new Exception("Password cannot be null or empty!");
    this.IsEncrypted = true;
  }

  public void RemoveEncryption()
  {
    this._password = (string) null;
    this.IsEncrypted = false;
  }

  internal int GetVmlDrawingCount(Slide slide)
  {
    int vmlDrawingCount = 0;
    foreach (ISlideItem shape in (IEnumerable<ISlideItem>) slide.Shapes)
    {
      if (shape.SlideItemType == SlideItemType.OleObject)
      {
        ++vmlDrawingCount;
        break;
      }
    }
    return vmlDrawingCount;
  }

  internal bool HasLicense() => string.IsNullOrEmpty(this.syncfusionLicense);

  internal void AddDefaultContentType(string partName, string contentType)
  {
    if (partName == null || this._defaultContentType.ContainsKey(partName))
      return;
    this._defaultContentType.Add(partName, contentType);
  }

  internal void OpenDocument(Stream fileStream) => this.ParseDocument(fileStream);

  internal void SetHandoutMaster(HandoutMaster handoutMaster)
  {
    this._handoutMaster = handoutMaster;
  }

  internal HandoutMaster GetHandoutMaster() => this._handoutMaster;

  internal Dictionary<string, LayoutSlide> GetSlideLayout()
  {
    return this._slideLayout ?? (this._slideLayout = new Dictionary<string, LayoutSlide>(1));
  }

  internal int ImageCount
  {
    get => this._imageCount;
    set => this._imageCount = value;
  }

  internal int CommentCount
  {
    get => this._commentCount;
    set => this._commentCount = value;
  }

  internal void AddWaterMark(IPresentation presentation)
  {
    foreach (ISlide slide in (IEnumerable<ISlide>) presentation.Slides)
      this.AddWaterMark(slide);
  }

  internal void AddWaterMark(ISlide slide)
  {
    double width = 430.0;
    double height = 30.0;
    double left = slide.SlideSize.Width / 2.0 - width / 2.0;
    double top = slide.SlideSize.Height - height * 2.0;
    IShape shape = slide.Shapes.AddTextBox(left, top, width, height);
    shape.ShapeName = "SyncfusionLicense";
    IParagraph paragraph = shape.TextBody.AddParagraph("Created with a trial version of Syncfusion Essential Presentation");
    paragraph.TextParts[0].Font.Color = ColorObject.Black;
    paragraph.Font.FontSize = 14f;
    paragraph.HorizontalAlignment = HorizontalAlignmentType.Center;
    shape.TextBody.VerticalAlignment = VerticalAlignmentType.Middle;
  }

  internal void SaveAsStream(Stream fileStream)
  {
    if (!this.HasLicense())
      this.AddWaterMark((IPresentation) this);
    if (this._dataHolder == null)
      this._dataHolder = new FileDataHolder(this);
    if (fileStream.CanSeek)
      fileStream.SetLength(0L);
    Stream stream = fileStream;
    if (stream is FileStream)
    {
      switch (Path.GetExtension(((FileStream) fileStream).Name).ToLower())
      {
        case ".pptm":
          this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.ms-powerpoint.presentation.macroEnabled.main+xml";
          break;
        case ".pptx":
          this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml";
          break;
        case ".potm":
          this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.ms-powerpoint.template.macroEnabled.main+xml";
          break;
        case ".potx":
          this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.openxmlformats-officedocument.presentationml.template.main+xml";
          break;
        default:
          throw new NotSupportedException("Cannot save in this file type");
      }
    }
    else
      this.SetFormatType();
    this.InitialiseDefaultRelation();
    this.SaveFromStream(stream);
  }

  private void SetFormatType()
  {
    switch (this._formatType)
    {
      case FormatType.Pptx:
        this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml";
        break;
      case FormatType.Pptm:
        this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.ms-powerpoint.presentation.macroEnabled.main+xml";
        break;
      case FormatType.Potx:
        this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.openxmlformats-officedocument.presentationml.template.main+xml";
        break;
      case FormatType.Potm:
        this._overrideContentType["/ppt/presentation.xml"] = "application/vnd.ms-powerpoint.template.macroEnabled.main+xml";
        break;
    }
  }

  internal void SetExportingSlide(ISlide slide) => this._exportingSilde = slide as Slide;

  internal void SetExportingSlide(NotesSlide notesSlide) => this._exportingNotesSilde = notesSlide;

  internal NotesSlide GetExportingNotesSlide() => this._exportingNotesSilde;

  internal Slide GetExportingSlide() => this._exportingSilde;

  internal MasterSlide GetMaster(string index)
  {
    foreach (MasterSlide master in this._masterCollection)
    {
      if (master.LayoutList.ContainsValue(index))
        return master;
    }
    return (MasterSlide) null;
  }

  public Stream[] RenderAsImages(ImageFormat imageFormat)
  {
    ISlides slides = this.Slides;
    List<Stream> streamList = new List<Stream>(slides.Count);
    foreach (ISlide slide in (IEnumerable<ISlide>) slides)
    {
      Stream image = slide.ConvertToImage(imageFormat);
      streamList.Add(image);
    }
    return streamList.ToArray();
  }

  public System.Drawing.Image[] RenderAsImages(Syncfusion.Drawing.ImageType imageType)
  {
    ISlides slides = this.Slides;
    List<System.Drawing.Image> imageList = new List<System.Drawing.Image>(slides.Count);
    foreach (ISlide slide in (IEnumerable<ISlide>) slides)
    {
      System.Drawing.Image image = slide.ConvertToImage(imageType);
      imageList.Add(image);
    }
    return imageList.ToArray();
  }

  private static IPresentation OpenAsStream(Stream stream, string password, string extension)
  {
    Syncfusion.Presentation.Presentation presentation = new Syncfusion.Presentation.Presentation();
    if (extension != null)
      presentation.CheckFormatType(extension);
    presentation._password = password;
    presentation.ParseDocument(stream);
    return (IPresentation) presentation;
  }

  private void PrepareResponse(string fileName, HttpResponse response)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException(nameof (fileName));
      case "":
        throw new ArgumentOutOfRangeException(nameof (fileName));
      default:
        if (response == null)
          throw new ArgumentNullException(nameof (response));
        fileName = Path.GetFileName(fileName);
        response.Clear();
        response.ContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml";
        response.AddHeader("Content-Disposition", $"{"attachment"}; filename={fileName};");
        break;
    }
  }

  private void AddDefaultContentToArchieve() => this._dataHolder.AddDefaultContent();

  internal void AddOverrideContentType(string partName, string contentType)
  {
    if (this._overrideContentType.ContainsKey(partName))
      return;
    this._overrideContentType.Add(partName, contentType);
  }

  internal void RemoveOverrideContentType(string partName)
  {
    if (!this._overrideContentType.ContainsKey(partName))
      return;
    this._overrideContentType.Remove(partName);
  }

  private void InitializeContentTypeRelation()
  {
    this.AddDefaultContentType("jpeg", "image/jpeg");
    this.AddDefaultContentType("rels", "application/vnd.openxmlformats-package.relationships+xml");
    this.AddDefaultContentType("xml", "application/xml");
    this.AddDefaultContentType("jpg", "image/jpg");
    this.AddOverrideContentType("/ppt/presentation.xml", "application/vnd.openxmlformats-officedocument.presentationml.presentation.main+xml");
    for (int index = 0; index < this._masterCollection.Count; ++index)
    {
      MasterSlide master = (MasterSlide) this._masterCollection[index];
      string partName = Syncfusion.Presentation.Drawing.Helper.GetSlidePath(this.MasterList, this.TopRelation, master.MasterId);
      if (!partName.StartsWith("/ppt/"))
        partName = "/ppt/" + partName;
      this.AddOverrideContentType(partName, "application/vnd.openxmlformats-officedocument.presentationml.slideMaster+xml");
      this.AddOverrideContentType("/" + Syncfusion.Presentation.Drawing.Helper.FormatPathForZipArchive(master.TopRelation.GetItemPathByKeyword("theme")), "application/vnd.openxmlformats-officedocument.theme+xml");
    }
    bool flag = false;
    for (int index = 0; index < this.Slides.Count; ++index)
    {
      Slide slide = (Slide) this.Slides[index];
      if (!flag)
        flag = slide.HasNotes;
      string partName = Syncfusion.Presentation.Drawing.Helper.GetSlidePath(slide.Presentation.SlideList, slide.Presentation.TopRelation, slide.SlideID.ToString());
      if (!partName.StartsWith("/ppt/"))
        partName = "/ppt/" + partName;
      this.AddOverrideContentType(partName, "application/vnd.openxmlformats-officedocument.presentationml.slide+xml");
    }
    if (flag)
      ((Slide) this.Slides[0]).InitializeNotesMasterSlide();
    this.AddOverrideContentType("/ppt/presProps.xml", "application/vnd.openxmlformats-officedocument.presentationml.presProps+xml");
    this.AddOverrideContentType("/ppt/viewProps.xml", "application/vnd.openxmlformats-officedocument.presentationml.viewProps+xml");
    this.AddOverrideContentType("/ppt/tableStyles.xml", "application/vnd.openxmlformats-officedocument.presentationml.tableStyles+xml");
    int num = 0;
    foreach (MasterSlide master in this._masterCollection)
    {
      if (master.LayoutList != null)
      {
        num += master.LayoutList.Count;
        if (this.Created)
        {
          for (int index = 0; index < num; ++index)
            this.AddOverrideContentType($"/ppt/slideLayouts/slideLayout{(object) (index + 1)}.xml", "application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml");
        }
        else
        {
          foreach (LayoutSlide layoutSlide in (IEnumerable<ILayoutSlide>) master.LayoutSlides)
          {
            string layoutIndex = this._dataHolder.GetLayoutIndex(layoutSlide);
            if (layoutIndex != null)
            {
              string itemPathByRelation = ((BaseSlide) layoutSlide.MasterSlide).TopRelation.GetItemPathByRelation(layoutIndex);
              string partName;
              if (!itemPathByRelation.StartsWith("/ppt"))
                partName = $"/ppt/slideLayouts/slideLayout{(object) Convert.ToInt32(itemPathByRelation.Replace("../slideLayouts/slideLayout", string.Empty).Split('.')[0])}.xml";
              else
                partName = itemPathByRelation;
              this.AddOverrideContentType(partName, "application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml");
            }
          }
        }
      }
    }
    if (!this.HasDocumentProperties)
      return;
    this.AddOverrideContentType("/docProps/core.xml", "application/vnd.openxmlformats-package.core-properties+xml");
    this.AddOverrideContentType("/docProps/app.xml", "application/vnd.openxmlformats-officedocument.extended-properties+xml");
  }

  private void InitialiseDefaultRelation()
  {
    this.InitializeRootRelation();
    this.InitializeHandoutMaster();
    this.InitializeContentTypeRelation();
    this.InitializeSlideListForSection();
  }

  private void InitializeSlideListForSection()
  {
    if (this._sections == null || this._sections.Count == 0)
      return;
    Dictionary<string, string> dictionary1 = new Dictionary<string, string>(this._sections.Presentation.SlideList.Count);
    Dictionary<string, string> dictionary2 = new Dictionary<string, string>(this._sections.Presentation.SlideList.Count);
    foreach (KeyValuePair<string, string> slide in this._sections.Presentation.SlideList)
      dictionary2.Add(slide.Value, slide.Key);
    foreach (Syncfusion.Presentation.SlideImplementation.Section section in this._sections)
    {
      foreach (string slideId in section.SlideIdList)
        dictionary1.Add(dictionary2[slideId], slideId);
    }
    if (this._slideList.Count == dictionary1.Count)
      this._slideList = Syncfusion.Presentation.Drawing.Helper.CloneDictionary(dictionary1);
    dictionary1.Clear();
    dictionary2.Clear();
  }

  private void InitializePresentation()
  {
    if (!this._created)
      return;
    this._slideList = new Dictionary<string, string>();
    for (int index = 0; index < this.Slides.Count; ++index)
      this._slideList.Add("rId" + (object) (index + 1 + 1), ((int) byte.MaxValue + (index + 1)).ToString((IFormatProvider) CultureInfo.InvariantCulture));
  }

  internal void InitializeHandoutMaster()
  {
    if (this._handoutMaster == null)
      return;
    this._handoutList = new Dictionary<string, string>();
    if (this._slideList == null || this._slideList.Count == 0)
      this._handoutList.Add("r:id2", (string) null);
    else
      this._handoutList.Add("r:id3", (string) null);
    if (this._handoutMaster.TopRelation.Count == 0)
    {
      this._handoutMaster.TopRelation = new RelationCollection();
      this._handoutMaster.TopRelation.Add("rId1", new Relation("rId1", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", "../theme/theme2.xml", (string) null));
      this.AddOverrideContentType("/ppt/handoutMasters/handoutMaster1.xml", "application/vnd.openxmlformats-officedocument.presentationml.handoutMaster+xml");
      this.AddOverrideContentType("/ppt/theme/theme2.xml", "application/vnd.openxmlformats-officedocument.theme+xml");
    }
    this.HasThumbnail = true;
    if (this._topRelation.GetItemPathByKeyword("handoutMaster") != null)
      return;
    int count = this._topRelation.Count;
    int num;
    Relation relation = new Relation("rId" + (object) (num = count + 1), "http://schemas.openxmlformats.org/officeDocument/2006/relationships/handoutMaster", "handoutMasters/handoutMaster1.xml", (string) null);
    this._topRelation.Add(relation.Id, relation);
  }

  private void InitializeRootRelation()
  {
    if (this._dataHolder.TopRelations.Count != 0)
      return;
    int num1 = 1;
    int num2 = num1 + 1;
    Relation relation1 = new Relation("rId" + (object) num1, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "ppt/presentation.xml", (string) null);
    this._dataHolder.TopRelations.Add(relation1.Id, relation1);
    if (this.HasThumbnail)
    {
      Relation relation2 = new Relation("rId" + (object) num2++, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail", "docProps/thumbnail.jpeg", (string) null);
      this._dataHolder.TopRelations.Add(relation2.Id, relation2);
    }
    int num3 = num2;
    int num4 = num3 + 1;
    Relation relation3 = new Relation("rId" + (object) num3, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties", "docProps/core.xml", (string) null);
    this._dataHolder.TopRelations.Add(relation3.Id, relation3);
    Relation relation4 = new Relation("rId" + (object) num4, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties", "docProps/app.xml", (string) null);
    this._dataHolder.TopRelations.Add(relation4.Id, relation4);
    if (this._customDocumentProperties.Count == 0)
      return;
    string relationId = Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(this._dataHolder.TopRelations);
    this._dataHolder.TopRelations.Add(relationId, new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties", "docProps/custom.xml", (string) null));
    this.AddOverrideContentType("/docProps/custom.xml", "application/vnd.openxmlformats-officedocument.custom-properties+xml");
  }

  private void IntializeDefaultCollections()
  {
    this._masterList = new Dictionary<string, string>();
    this._masterList.Add("rId1", this._customId.ToString());
  }

  private void ParseDocument(Stream fileStream)
  {
    Stream stream = fileStream;
    if (stream.Length == 0L)
      return;
    this.SetArchieveItems(stream);
    this._dataHolder.ParseDocument();
  }

  private void SaveFromStream(Stream stream) => this._dataHolder.WriteDocument(stream);

  private void SetArchieveItems(Stream stream)
  {
    this._dataHolder = new FileDataHolder(this);
    this._dataHolder.Open(stream);
  }

  public void Close()
  {
    if (this._isDisposed)
      return;
    this.ClearAll();
    this._isDisposed = true;
  }

  public void Dispose() => this.Close();

  private void ClearAll()
  {
    if (this._chartToImageConverter != null)
      this._chartToImageConverter = (IOfficeChartToImageConverter) null;
    if (this._dataHolder != null)
    {
      this._dataHolder.Close();
      this._dataHolder = (FileDataHolder) null;
    }
    if (this._handoutList != null)
    {
      this._handoutList.Clear();
      this._handoutList = (Dictionary<string, string>) null;
    }
    if (this._defaultContentType != null)
    {
      this._defaultContentType.Clear();
      this._defaultContentType = (Dictionary<string, string>) null;
    }
    if (this._overrideContentType != null)
    {
      this._overrideContentType.Clear();
      this._overrideContentType = (Dictionary<string, string>) null;
    }
    if (this._topRelation != null)
    {
      this._topRelation.Close();
      this._topRelation = (RelationCollection) null;
    }
    if (this._masterCollection != null)
    {
      this._masterCollection.Close();
      this._masterCollection = (MasterSlides) null;
    }
    if (this._masterList != null)
    {
      this._masterList.Clear();
      this._masterList = (Dictionary<string, string>) null;
    }
    if (this._notesList != null)
    {
      this._notesList.Clear();
      this._notesList = (Dictionary<string, string>) null;
    }
    if (this._notesSize != null)
      this._notesSize = (Syncfusion.Presentation.NotesSize) null;
    if (this._slideCollection != null)
    {
      this._slideCollection.Close();
      this._slideCollection = (Syncfusion.Presentation.SlideImplementation.Slides) null;
    }
    if (this._slideList != null)
    {
      this._slideList.Clear();
      this._slideList = (Dictionary<string, string>) null;
    }
    if (this._slideSize != null)
      this._slideSize = (Syncfusion.Presentation.SlideSize) null;
    if (this._styleList != null)
    {
      this._styleList.Clear();
      this._styleList = (Dictionary<string, int>) null;
    }
    if (this._handoutMaster != null)
    {
      this._handoutMaster.Close();
      this._handoutMaster = (HandoutMaster) null;
    }
    if (this._slideLayout != null)
    {
      foreach (KeyValuePair<string, LayoutSlide> keyValuePair in this._slideLayout)
        keyValuePair.Value.Close();
      this._slideLayout.Clear();
      this._slideLayout = (Dictionary<string, LayoutSlide>) null;
    }
    if (this._exportingSilde != null)
    {
      this._exportingSilde.Close();
      this._exportingSilde = (Slide) null;
    }
    if (this._defaultTextStyle != null)
    {
      this._defaultTextStyle.Close();
      this._defaultTextStyle = (Syncfusion.Presentation.RichText.TextBody) null;
    }
    if (this._tableStyleList != null)
    {
      foreach (TableStyle tableStyle in this._tableStyleList.Values)
        tableStyle.Close();
      this._tableStyleList.Clear();
      this._tableStyleList = (Dictionary<string, TableStyle>) null;
    }
    if (this._preservedElements != null)
    {
      foreach (KeyValuePair<string, Stream> preservedElement in this._preservedElements)
        preservedElement.Value.Dispose();
      this._preservedElements.Clear();
      this._preservedElements = (Dictionary<string, Stream>) null;
    }
    if (this._builtInDocumentProperties != null)
    {
      this._builtInDocumentProperties.Close();
      this._builtInDocumentProperties = (Syncfusion.Presentation.BuiltInDocumentProperties) null;
    }
    if (this._customDocumentProperties != null)
    {
      this._customDocumentProperties.Close();
      this._customDocumentProperties = (Syncfusion.Presentation.CustomDocumentProperties) null;
    }
    if (this._slideIdList != null)
    {
      this._slideIdList.Clear();
      this._slideIdList = (List<int>) null;
    }
    if (this._slidesFromInputFile != null)
    {
      this._slidesFromInputFile.Clear();
      this._slidesFromInputFile = (Dictionary<string, ISlide>) null;
    }
    if (this._graphics != null)
    {
      this._graphics.Dispose();
      this._graphics = (Graphics) null;
    }
    if (this._internalGraphics != null)
    {
      this._internalGraphics.Dispose();
      this._internalGraphics = (Graphics) null;
    }
    if (this._hyperlinks != null)
    {
      this._hyperlinks.Clear();
      this._hyperlinks = (List<Dictionary<string, RectangleF>>) null;
    }
    if (this.m_fontSettings != null)
    {
      this.m_fontSettings.Close();
      this.m_fontSettings = (FontSettings) null;
    }
    if (this._vbaProject != null)
    {
      this._vbaProject.Dispose();
      this._vbaProject = (Stream) null;
    }
    if (this._sections == null)
      return;
    this._sections.Close();
    this._sections = (Syncfusion.Presentation.SlideImplementation.Sections) null;
  }

  internal bool CheckForEncryption(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    bool flag = false;
    if (Syncfusion.CompoundFile.Presentation.Net.CompoundFile.CheckHeader(stream))
      flag = true;
    return flag;
  }

  internal string Password
  {
    get => this._password;
    set => this._password = value;
  }

  internal int SmartArtCount
  {
    get => this._smartArtCount;
    set => this._smartArtCount = value;
  }

  internal NotesMasterSlide NotesMaster
  {
    get => this._notesMaster;
    set => this._notesMaster = value;
  }

  internal int NotesSlideCount
  {
    get => this._notesSlideCount;
    set => this._notesSlideCount = value;
  }

  internal bool CheckHasNotesSlide
  {
    get
    {
      if (this._notesMaster != null)
        return true;
      return this._notesList != null && this._notesList.Count > 0;
    }
  }

  internal bool IsEncrypted
  {
    get => this._isEncrypted;
    set => this._isEncrypted = value;
  }

  internal int OleObjectCount
  {
    get => this._oleObjectCount;
    set => this._oleObjectCount = value;
  }

  internal int ExcelCount
  {
    get => this._excelCount;
    set => this._excelCount = value;
  }

  internal Stream VBAProject
  {
    get => this._vbaProject;
    set => this._vbaProject = value;
  }

  public void RemoveMacros()
  {
    this._topRelation.RemoveRelation(this._topRelation.GetRelationByContentType("http://schemas.microsoft.com/office/2006/relationships/vbaProject").Id);
    if (this._defaultContentType["bin"].Equals("application/vnd.ms-office.vbaProject"))
      this._defaultContentType.Remove("bin");
    if (this.VBAProject == null)
      return;
    this.VBAProject.Dispose();
    this.VBAProject = (Stream) null;
  }

  public bool HasMacros => this._vbaProject != null;

  public bool IsWriteProtected => this.WriteProtection.IsWriteProtected;

  public void SetWriteProtection(string password)
  {
    this.WriteProtection.SetWriteProtection(password);
  }

  public void RemoveWriteProtection() => this.WriteProtection.RemoveWriteProtection();

  internal void SetSlideListValue(Dictionary<string, string> newSlideIdList)
  {
    this._slideList = newSlideIdList;
  }
}
