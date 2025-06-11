// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WordDocument
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.DocIO.DLS.Convertors;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ODTConvertion;
using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;
using Syncfusion.DocIO.ReaderWriter.Security;
using Syncfusion.Layouting;
using Syncfusion.Licensing;
using Syncfusion.Office;
using Syncfusion.OfficeChart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WordDocument : 
  WidgetContainer,
  IWordDocument,
  ICompositeEntity,
  IEntity,
  IDisposable,
  IXmlSerializable,
  IWidgetContainer,
  IWidget
{
  private const string DEF_NORMAL_STYLE = "Normal";
  internal const string DEF_BULLETS_STYLE = "Bulleted";
  internal const string DEF_NUMBERING_STYLE = "Numbered";
  private byte m_bFlags1 = 144 /*0x90*/;
  private byte m_bFlags;
  private byte m_bFlags2;
  private byte m_bFlags3;
  internal TextBodyItem m_prevClonedEntity;
  private FormatType m_actualFormatType;
  private ushort m_fibVersion;
  private string m_duplicateListStyleNames = string.Empty;
  private FontFamilyNameStringTable m_ffnStringTable;
  internal BuiltinDocumentProperties m_builtinProp = new BuiltinDocumentProperties();
  internal CustomDocumentProperties m_customProp = new CustomDocumentProperties();
  protected WSectionCollection m_sections;
  protected IStyleCollection m_styles;
  internal ListStyleCollection m_listStyles;
  internal Dictionary<string, int> m_sequenceFieldResults;
  private ListOverrideStyleCollection m_listOverrides;
  private BookmarkCollection m_bookmarks;
  private EditableRangeCollection m_editableRanges;
  private FieldCollection m_fields;
  private TextBoxCollection m_txbxItems;
  private RevisionCollection m_revisions;
  internal MetaProperties m_contentTypeProperties;
  private CommentsCollection m_Comments;
  private CommentsExCollection m_CommentsEx;
  private float m_defaultTabWidth = 36f;
  private MailMerge m_mailMerge;
  private ViewSetup m_viewSetup;
  private Watermark m_watermark;
  private Background m_background;
  private DOPDescriptor m_dop;
  private GrammarSpelling m_grammarSpellingData;
  private EscherClass m_escher;
  private string m_password;
  private byte[] m_macrosData;
  private byte[] m_escherDataContainers;
  private byte[] m_escherContainers;
  private byte[] m_macroCommands;
  private int m_defShapeId = 1;
  private string m_standardAsciiFont;
  private string m_standardFarEastFont;
  private string m_standardNonFarEastFont;
  private string m_standardBidiFont;
  private static readonly object m_threadLocker = new object();
  private XHTMLValidationType m_htmlValidationOption = XHTMLValidationType.Transitional;
  private XmlNode m_latentStyles;
  private MemoryStream m_latentStyles2010;
  private WCharacterFormat m_defCharFormat;
  internal WParagraphFormat m_defParaFormat;
  private Package m_docxPackage;
  private ImportOptions m_importOption = ImportOptions.UseDestinationStyles;
  private DocVariables m_variables;
  private DocProperties m_props;
  private ParagraphItem m_nextParaItem;
  private TextBodyItem m_prevBodyItem;
  private SaveOptions m_saveOptions;
  private RevisionOptions m_revisionOptions;
  private List<Stream> m_docxProps;
  private SttbfAssoc m_assocStrings;
  private Dictionary<string, string> m_styleNameIds;
  private int m_paraCount;
  private int m_wordCount;
  private int m_charCount;
  private Dictionary<string, string> m_fontSubstitutionTable;
  private string m_htmlBaseUrl = string.Empty;
  private Dictionary<WField, TableOfContent> m_tableOfContent;
  private List<Font> m_usedFonts;
  private Settings m_settings;
  private Themes m_themes;
  private Stream m_vbaProject;
  private Stream m_vbaProjectSignature;
  private Stream m_vbaProjectSignatureAgile;
  private PartContainer m_CustomUIPartContainer;
  private PartContainer m_UserCustomizationPartContainer;
  private PartContainer m_CustomXMLContainer;
  private List<MacroData> m_vbaData;
  private List<string> m_docEvents;
  private FormatType m_saveFormatType;
  private ushort m_wordVersion;
  private Stack<WField> m_clonedFields;
  private int m_altChunkCount;
  private ImageCollection m_imageCollection;
  private Footnote m_footnotes;
  private Endnote m_endnotes;
  private Dictionary<string, string> m_listStyleNames;
  private Dictionary<string, Storage> m_oleObjectCollection;
  private static bool m_EnablePartialTrustCode;
  private static bool m_disableDateTimeUpdating;
  private Dictionary<string, CustomXMLPart> m_customXMLParts;
  private PartContainer m_customXMLPartContainer;
  private HTMLImportSettings m_htmlImportSettings;
  private Dictionary<string, Dictionary<int, int>> m_lists;
  private HybridDictionary m_listNames;
  private Dictionary<string, int> m_previousListLevel;
  private List<string> m_previousListLevelOverrideStyle;
  internal int PageCount;
  private List<Shape> m_AutoShapeCollection = new List<Shape>();
  private byte[] m_sttbfRMark;
  private IOfficeChartToImageConverter m_chartToImageConverter;
  private FontSettings m_fontSettings;
  private Hyphenator m_hyphenator;
  private List<Entity> m_FloatingItems;
  internal int m_notSupportedElementFlag;
  internal int m_supportedElementFlag_1;
  internal int m_supportedElementFlag_2;
  internal WordDocument m_AltChunkOwner;
  private int m_balloonCount;
  internal string m_metaXmlItem;
  private string syncfusionLicense;
  private bool m_isWarnInserted;
  internal int m_tocBookmarkID;

  public IOfficeChartToImageConverter ChartToImageConverter
  {
    get => this.m_chartToImageConverter;
    set => this.m_chartToImageConverter = value;
  }

  public FontSettings FontSettings
  {
    get => this.m_fontSettings ?? (this.m_fontSettings = new FontSettings());
  }

  public Hyphenator Hyphenator => this.m_hyphenator ?? (this.m_hyphenator = new Hyphenator());

  public Footnote Footnotes
  {
    get
    {
      if (this.m_footnotes == null)
        this.m_footnotes = new Footnote(this);
      return this.m_footnotes;
    }
    set
    {
      this.m_footnotes = value;
      if (this.m_footnotes == null)
        return;
      this.m_footnotes.SetOwner(this);
    }
  }

  public Endnote Endnotes
  {
    get
    {
      if (this.m_endnotes == null)
        this.m_endnotes = new Endnote(this);
      return this.m_endnotes;
    }
    set
    {
      this.m_endnotes = value;
      if (this.m_endnotes == null)
        return;
      this.m_endnotes.SetOwner(this);
    }
  }

  public float DefaultTabWidth
  {
    get => this.m_defaultTabWidth;
    set => this.m_defaultTabWidth = value;
  }

  internal ushort FIBVersion
  {
    get => this.m_fibVersion;
    set => this.m_fibVersion = value;
  }

  public override EntityType EntityType => EntityType.WordDocument;

  public BuiltinDocumentProperties BuiltinDocumentProperties
  {
    get
    {
      if (this.m_builtinProp == null)
        this.m_builtinProp = new BuiltinDocumentProperties(this);
      return this.m_builtinProp;
    }
  }

  public Template AttachedTemplate => new Template(this.AssociatedStrings);

  public bool UpdateStylesOnOpen
  {
    get => this.DOP.LinkStyles;
    set => this.DOP.LinkStyles = value;
  }

  public CustomDocumentProperties CustomDocumentProperties
  {
    get
    {
      if (this.m_customProp == null)
        this.m_customProp = new CustomDocumentProperties();
      return this.m_customProp;
    }
  }

  public WSectionCollection Sections
  {
    get
    {
      if (this.m_sections == null)
        this.m_sections = new WSectionCollection(this);
      return this.m_sections;
    }
  }

  public IStyleCollection Styles
  {
    get
    {
      if (this.m_styles == null)
        this.m_styles = (IStyleCollection) new StyleCollection(this);
      return this.m_styles;
    }
  }

  public ListStyleCollection ListStyles
  {
    get
    {
      if (this.m_listStyles == null)
        this.m_listStyles = new ListStyleCollection(this);
      return this.m_listStyles;
    }
  }

  public ImportOptions ImportOptions
  {
    get => this.m_importOption;
    set
    {
      this.m_importOption = value;
      this.UpdateImportOption();
    }
  }

  public BookmarkCollection Bookmarks
  {
    get
    {
      if (this.m_bookmarks == null)
        this.m_bookmarks = new BookmarkCollection(this);
      return this.m_bookmarks;
    }
  }

  internal EditableRangeCollection EditableRanges
  {
    get
    {
      if (this.m_editableRanges == null)
        this.m_editableRanges = new EditableRangeCollection(this);
      return this.m_editableRanges;
    }
  }

  public CommentsCollection Comments
  {
    get
    {
      if (this.m_Comments == null)
        this.m_Comments = new CommentsCollection(this);
      return this.m_Comments;
    }
  }

  internal CommentsExCollection CommentsEx
  {
    get
    {
      if (this.m_CommentsEx == null)
        this.m_CommentsEx = new CommentsExCollection(this);
      return this.m_CommentsEx;
    }
  }

  public TextBoxCollection TextBoxes
  {
    get
    {
      if (this.m_txbxItems == null)
        this.m_txbxItems = new TextBoxCollection(this);
      return this.m_txbxItems;
    }
    set => this.m_txbxItems = value;
  }

  public RevisionCollection Revisions
  {
    get
    {
      if (this.m_revisions == null)
        this.m_revisions = new RevisionCollection(this);
      return this.m_revisions;
    }
  }

  public MetaProperties ContentTypeProperties
  {
    get
    {
      if (this.m_contentTypeProperties == null)
        this.m_contentTypeProperties = new MetaProperties();
      return this.m_contentTypeProperties;
    }
  }

  public WSection LastSection
  {
    get
    {
      int count = this.Sections.Count;
      return count > 0 ? this.Sections[count - 1] : (WSection) null;
    }
  }

  public WParagraph LastParagraph
  {
    get
    {
      WSection lastSection = this.LastSection;
      if (lastSection != null)
      {
        (lastSection.Body.Paragraphs as WParagraphCollection).ClearIndexes();
        int count = lastSection.Body.Paragraphs.Count;
        if (count > 0)
          return lastSection.Body.Paragraphs[count - 1];
      }
      return (WParagraph) null;
    }
  }

  public FootEndNoteNumberFormat EndnoteNumberFormat
  {
    get => (FootEndNoteNumberFormat) this.DOP.EndnoteNumberFormat;
    set => this.DOP.EndnoteNumberFormat = (byte) value;
  }

  public FootEndNoteNumberFormat FootnoteNumberFormat
  {
    get => (FootEndNoteNumberFormat) this.DOP.FootnoteNumberFormat;
    set
    {
      this.DOP.FootnoteNumberFormat = (byte) value;
      if (this.IsOpening)
        return;
      foreach (WSection section in (CollectionImpl) this.Sections)
        section.PageSetup.FootnoteNumberFormat = value;
    }
  }

  public EndnoteRestartIndex RestartIndexForEndnote
  {
    get => (EndnoteRestartIndex) this.DOP.RestartIndexForEndnote;
    set
    {
      this.DOP.RestartIndexForEndnote = (byte) value;
      if (this.IsOpening)
        return;
      foreach (WSection section in (CollectionImpl) this.Sections)
        section.PageSetup.RestartIndexForEndnote = value;
    }
  }

  public EndnotePosition EndnotePosition
  {
    get => (EndnotePosition) this.DOP.EndnotePosition;
    set => this.DOP.EndnotePosition = (byte) value;
  }

  public FootnoteRestartIndex RestartIndexForFootnotes
  {
    get => (FootnoteRestartIndex) this.DOP.RestartIndexForFootnotes;
    set
    {
      this.DOP.RestartIndexForFootnotes = (byte) value;
      if (this.IsOpening)
        return;
      foreach (WSection section in (CollectionImpl) this.Sections)
        section.PageSetup.RestartIndexForFootnotes = value;
    }
  }

  public FootnotePosition FootnotePosition
  {
    get => (FootnotePosition) this.DOP.FootnotePosition;
    set => this.DOP.FootnotePosition = (byte) value;
  }

  public Watermark Watermark
  {
    get
    {
      if (this.m_watermark == null)
        this.m_watermark = new Watermark(this, WatermarkType.NoWatermark);
      return this.m_watermark;
    }
    set
    {
      this.ResetWatermark();
      this.m_watermark = value;
      WordDocument document = this.m_watermark != null ? this.m_watermark.Document : (WordDocument) null;
      if (this.m_watermark != null)
      {
        if (!this.Document.IsOpening)
          this.UpdateHeaderWatermark(this.m_watermark);
        if (this.m_watermark is PictureWatermark)
          (this.m_watermark as PictureWatermark).UpdateImage();
        else
          (this.m_watermark as TextWatermark).SetDefaultSize();
      }
      if (!(this.m_watermark is PictureWatermark) || document == null || this.m_doc == document || document.ActualFormatType != FormatType.Doc || this.m_doc.Escher == null || this.m_doc.Escher.m_msofbtDggContainer == null || this.m_doc.Escher.m_msofbtDggContainer.BstoreContainer == null)
        return;
      int count = this.m_doc.Escher.m_msofbtDggContainer.BstoreContainer.Children.Count;
      document.CloneShapeEscher(this.m_doc, (IParagraphItem) (this.m_watermark as PictureWatermark).WordPicture);
      if (count == this.m_doc.Escher.m_msofbtDggContainer.BstoreContainer.Children.Count)
        return;
      (this.m_watermark as PictureWatermark).OriginalPib = this.m_doc.Escher.m_msofbtDggContainer.BstoreContainer.Children.Count;
    }
  }

  public Background Background
  {
    get
    {
      if (this.m_background == null)
        this.m_background = new Background(this, BackgroundType.NoBackground);
      return this.m_background;
    }
  }

  public MailMerge MailMerge
  {
    get
    {
      if (this.m_mailMerge == null)
        this.m_mailMerge = new MailMerge(this);
      return this.m_mailMerge;
    }
  }

  public ProtectionType ProtectionType
  {
    get => this.DOP.ProtectionType;
    set => this.DOP.ProtectionType = value;
  }

  public ViewSetup ViewSetup
  {
    get
    {
      if (this.m_viewSetup == null)
        this.m_viewSetup = new ViewSetup((IWordDocument) this);
      return this.m_viewSetup;
    }
  }

  public bool ThrowExceptionsForUnsupportedElements
  {
    get => ((int) this.m_bFlags1 & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 191 | (value ? 1 : 0) << 6);
  }

  public int InitialFootnoteNumber
  {
    get => this.DOP.InitialFootnoteNumber;
    set
    {
      this.DOP.InitialFootnoteNumber = value;
      if (this.IsOpening)
        return;
      foreach (WSection section in (CollectionImpl) this.Sections)
        section.PageSetup.InitialFootnoteNumber = value;
    }
  }

  public int InitialEndnoteNumber
  {
    get => this.DOP.InitialEndnoteNumber;
    set
    {
      this.DOP.InitialEndnoteNumber = value;
      if (this.IsOpening)
        return;
      foreach (WSection section in (CollectionImpl) this.Sections)
        section.PageSetup.InitialEndnoteNumber = value;
    }
  }

  public EntityCollection ChildEntities => (EntityCollection) this.m_sections;

  public XHTMLValidationType XHTMLValidateOption
  {
    get => this.m_htmlValidationOption;
    set => this.m_htmlValidationOption = value;
  }

  [Obsolete("This property has been deprecated. Use the Picture property of Background class to set the background image of the document")]
  public Image BackgroundImage
  {
    get => this.GetBackGndImage();
    set => this.SetBackgroundImageValue(value);
  }

  public DocVariables Variables
  {
    get
    {
      if (this.m_variables == null)
        this.m_variables = new DocVariables();
      return this.m_variables;
    }
  }

  public DocProperties Properties
  {
    get
    {
      if (this.m_props == null)
        this.m_props = new DocProperties((IWordDocument) this);
      return this.m_props;
    }
  }

  public bool HasChanges => this.HasTrackedChanges();

  public bool TrackChanges
  {
    get => this.DOP.RevMarking;
    set => this.DOP.RevMarking = value;
  }

  public bool ReplaceFirst
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public HTMLImportSettings HTMLImportSettings
  {
    get
    {
      if (this.m_htmlImportSettings == null)
        this.m_htmlImportSettings = new HTMLImportSettings();
      return this.m_htmlImportSettings;
    }
    set => this.m_htmlImportSettings = value;
  }

  public SaveOptions SaveOptions
  {
    get
    {
      if (this.m_saveOptions == null)
        this.m_saveOptions = new SaveOptions();
      return this.m_saveOptions;
    }
  }

  public RevisionOptions RevisionOptions
  {
    get
    {
      if (this.m_revisionOptions == null)
        this.m_revisionOptions = new RevisionOptions();
      return this.m_revisionOptions;
    }
  }

  [Obsolete("This property has been deprecated. Use the UpdateDocumentFields method of WordDocument class to update the fields in the document.")]
  public bool UpdateFields
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public FormatType ActualFormatType
  {
    get => this.m_actualFormatType;
    internal set => this.m_actualFormatType = value;
  }

  public Dictionary<string, string> FontSubstitutionTable
  {
    get
    {
      if (this.m_fontSubstitutionTable == null)
        this.m_fontSubstitutionTable = new Dictionary<string, string>();
      return this.m_fontSubstitutionTable;
    }
    set => this.m_fontSubstitutionTable = value;
  }

  public bool HasMacros
  {
    get
    {
      return this.VbaProject != null || this.VbaData.Count > 0 || this.DocEvents.Count > 0 || this.MacrosData != null;
    }
  }

  public static bool EnablePartialTrustCode
  {
    get => WordDocument.m_EnablePartialTrustCode;
    set => WordDocument.m_EnablePartialTrustCode = value;
  }

  internal static bool DisableDateTimeUpdating
  {
    get => WordDocument.m_disableDateTimeUpdating;
    set => WordDocument.m_disableDateTimeUpdating = value;
  }

  internal bool RestrictFormatting
  {
    get => ((int) this.m_bFlags3 & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags3 = (byte) ((int) this.m_bFlags3 & 223 | (value ? 1 : 0) << 5);
  }

  internal bool Enforcement
  {
    get => ((int) this.m_bFlags3 & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags3 = (byte) ((int) this.m_bFlags3 & 239 | (value ? 1 : 0) << 4);
  }

  internal MultiplePage MultiplePage
  {
    get
    {
      if (this.DOP.MirrorMargins)
        return MultiplePage.MirrorMargins;
      if (this.DOP.Dop97.DopTypography.Print2on1)
        return MultiplePage.TwoPagesPerSheet;
      if (this.DOP.Dop2002.ReverseFolio)
        return MultiplePage.ReverseBookFold;
      return this.DOP.Dop2002.FolioPrint ? MultiplePage.BookFold : MultiplePage.Normal;
    }
    set
    {
      switch (value)
      {
        case MultiplePage.MirrorMargins:
          this.DOP.MirrorMargins = true;
          break;
        case MultiplePage.TwoPagesPerSheet:
          this.DOP.Dop97.DopTypography.Print2on1 = true;
          break;
        case MultiplePage.BookFold:
          this.DOP.Dop2002.FolioPrint = true;
          break;
        case MultiplePage.ReverseBookFold:
          this.DOP.Dop2002.ReverseFolio = true;
          this.DOP.Dop2002.FolioPrint = true;
          break;
      }
    }
  }

  internal int SheetsPerBooklet
  {
    get => (int) this.DOP.Dop2002.IFolioPages;
    set
    {
      if (value == 0)
        return;
      this.DOP.Dop2002.IFolioPages = (ushort) value;
    }
  }

  internal FontFamilyNameStringTable FFNStringTable
  {
    get => this.m_ffnStringTable;
    set => this.m_ffnStringTable = value;
  }

  internal bool HasStyleSheets
  {
    get => ((int) this.m_bFlags3 & 2) >> 1 != 0;
    set => this.m_bFlags3 = (byte) ((int) this.m_bFlags3 & 253 | (value ? 1 : 0) << 1);
  }

  internal Dictionary<string, CustomXMLPart> CustomXmlParts
  {
    get
    {
      if (this.m_customXMLParts == null)
        this.m_customXMLParts = new Dictionary<string, CustomXMLPart>();
      return this.m_customXMLParts;
    }
  }

  internal PartContainer CustomXmlPartContainer
  {
    get
    {
      if (this.m_customXMLPartContainer == null)
        this.m_customXMLPartContainer = new PartContainer();
      return this.m_customXMLPartContainer;
    }
  }

  internal ImageCollection Images
  {
    get
    {
      if (this.m_imageCollection == null)
        this.m_imageCollection = new ImageCollection(this);
      return this.m_imageCollection;
    }
  }

  internal Dictionary<string, Storage> OleObjectCollection
  {
    get
    {
      if (this.m_oleObjectCollection == null)
        this.m_oleObjectCollection = new Dictionary<string, Storage>();
      return this.m_oleObjectCollection;
    }
  }

  internal Stack<WField> ClonedFields
  {
    get
    {
      if (this.m_clonedFields == null)
        this.m_clonedFields = new Stack<WField>();
      return this.m_clonedFields;
    }
  }

  internal ListOverrideStyleCollection ListOverrides
  {
    get
    {
      if (this.m_listOverrides == null)
        this.m_listOverrides = new ListOverrideStyleCollection(this);
      return this.m_listOverrides;
    }
  }

  internal GrammarSpelling GrammarSpellingData
  {
    get => this.m_grammarSpellingData;
    set => this.m_grammarSpellingData = value;
  }

  internal DOPDescriptor DOP
  {
    get
    {
      if (this.m_dop == null)
        this.m_dop = new DOPDescriptor();
      return this.m_dop;
    }
    set => this.m_dop = value;
  }

  internal EscherClass Escher
  {
    get => this.m_escher;
    set => this.m_escher = value;
  }

  internal FormatType SaveFormatType
  {
    get => this.m_saveFormatType;
    set => this.m_saveFormatType = value;
  }

  internal bool IsMacroEnabled
  {
    get
    {
      return this.SaveFormatType == FormatType.Word2007Docm || this.SaveFormatType == FormatType.Word2010Docm || this.SaveFormatType == FormatType.Word2007Dotm || this.SaveFormatType == FormatType.Word2010Dotm;
    }
  }

  internal Stream VbaProject
  {
    get => this.m_vbaProject;
    set => this.m_vbaProject = value;
  }

  internal Stream VbaProjectSignature
  {
    get => this.m_vbaProjectSignature;
    set => this.m_vbaProjectSignature = value;
  }

  internal Stream VbaProjectSignatureAgile
  {
    get => this.m_vbaProjectSignatureAgile;
    set => this.m_vbaProjectSignatureAgile = value;
  }

  internal PartContainer CustomUIPartContainer
  {
    get => this.m_CustomUIPartContainer;
    set => this.m_CustomUIPartContainer = value;
  }

  internal PartContainer UserCustomizationPartContainer
  {
    get => this.m_UserCustomizationPartContainer;
    set => this.m_UserCustomizationPartContainer = value;
  }

  internal PartContainer CustomXMLContainer
  {
    get => this.m_CustomXMLContainer;
    set => this.m_CustomXMLContainer = value;
  }

  internal List<MacroData> VbaData
  {
    get
    {
      if (this.m_vbaData == null)
        this.m_vbaData = new List<MacroData>();
      return this.m_vbaData;
    }
    set => this.m_vbaData = value;
  }

  internal List<string> DocEvents
  {
    get
    {
      if (this.m_docEvents == null)
        this.m_docEvents = new List<string>();
      return this.m_docEvents;
    }
    set => this.m_docEvents = value;
  }

  internal byte[] MacrosData
  {
    get => this.m_macrosData;
    set => this.m_macrosData = value;
  }

  internal byte[] MacroCommands
  {
    get => this.m_macroCommands;
    set => this.m_macroCommands = value;
  }

  internal string StandardAsciiFont
  {
    get => this.m_standardAsciiFont;
    set => this.m_standardAsciiFont = value;
  }

  internal string StandardFarEastFont
  {
    get => this.m_standardFarEastFont;
    set => this.m_standardFarEastFont = value;
  }

  internal string StandardNonFarEastFont
  {
    get => this.m_standardNonFarEastFont;
    set => this.m_standardNonFarEastFont = value;
  }

  internal string StandardBidiFont
  {
    get => this.m_standardBidiFont;
    set => this.m_standardBidiFont = value;
  }

  internal string Password
  {
    get => this.m_password;
    set => this.m_password = value;
  }

  internal MemoryStream LatentStyles2010
  {
    get => this.m_latentStyles2010;
    set => this.m_latentStyles2010 = value;
  }

  internal XmlNode LatentStyles
  {
    get => this.m_latentStyles;
    set => this.m_latentStyles = value;
  }

  internal Package DocxPackage
  {
    get => this.m_docxPackage;
    set => this.m_docxPackage = value;
  }

  public bool ImportStyles
  {
    get => ((int) this.m_bFlags1 & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  public bool ImportStylesOnTypeMismatch
  {
    get => ((int) this.m_bFlags3 & 1) != 0;
    set => this.m_bFlags3 = (byte) ((int) this.m_bFlags3 & 254 | (value ? 1 : 0));
  }

  internal WCharacterFormat DefCharFormat
  {
    get => this.m_defCharFormat;
    set => this.m_defCharFormat = value;
  }

  internal WParagraphFormat DefParaFormat
  {
    get
    {
      if (this.m_defParaFormat == null && !this.IsOpening)
        this.InitDefaultParagraphFormat();
      return this.m_defParaFormat;
    }
    set => this.m_defParaFormat = value;
  }

  internal SttbfAssoc AssociatedStrings
  {
    get
    {
      if (this.m_assocStrings == null)
        this.m_assocStrings = new SttbfAssoc();
      return this.m_assocStrings;
    }
  }

  internal bool IsEncrypted
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    private set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool HasPicture
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  internal bool WriteWarning
  {
    get => !string.IsNullOrEmpty(this.syncfusionLicense) && !this.m_isWarnInserted;
  }

  internal bool WriteProtected
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal bool UpdateAlternateChunk
  {
    get => ((int) this.m_bFlags2 & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsDeletingBookmarkContent
  {
    get => ((int) this.m_bFlags2 & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 223 | (value ? 1 : 0) << 5);
  }

  private HybridDictionary ListNames
  {
    get
    {
      if (this.m_listNames == null)
        this.m_listNames = new HybridDictionary();
      return this.m_listNames;
    }
  }

  private Dictionary<string, Dictionary<int, int>> Lists
  {
    get
    {
      if (this.m_lists == null)
        this.m_lists = new Dictionary<string, Dictionary<int, int>>();
      return this.m_lists;
    }
  }

  private Dictionary<string, int> PreviousListLevel
  {
    get
    {
      if (this.m_previousListLevel == null)
        this.m_previousListLevel = new Dictionary<string, int>();
      return this.m_previousListLevel;
    }
  }

  private List<string> PreviousListLevelOverrideStyle
  {
    get
    {
      if (this.m_previousListLevelOverrideStyle == null)
        this.m_previousListLevelOverrideStyle = new List<string>();
      return this.m_previousListLevelOverrideStyle;
    }
  }

  internal bool UseHangingIndentAsListTab
  {
    get
    {
      if (this.ActualFormatType == FormatType.Doc)
        return false;
      return this.ActualFormatType != FormatType.Docx && this.ActualFormatType != FormatType.Word2007 && this.ActualFormatType != FormatType.Word2010 && this.ActualFormatType != FormatType.Word2013 || !this.Settings.CompatibilityOptions[CompatibilityOption.DontUseIndentAsNumberingTabStop];
    }
  }

  internal bool UseHangingIndentAsTabPosition
  {
    get
    {
      return this.Settings.CompatibilityMode == CompatibilityMode.Word2013 || !this.Settings.CompatibilityOptions[CompatibilityOption.NoTabForInd];
    }
  }

  internal Themes Themes
  {
    get
    {
      if (this.m_themes == null)
        this.m_themes = new Themes((IWordDocument) this);
      return this.m_themes;
    }
  }

  public Settings Settings
  {
    get
    {
      if (this.m_settings == null)
        this.m_settings = new Settings(this);
      return this.m_settings;
    }
  }

  internal int AlternateChunkCount => ++this.m_altChunkCount;

  internal bool IsOpening
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 254 | (value ? 1 : 0));
  }

  internal bool IsMailMerge
  {
    get => ((int) this.m_bFlags1 & 2) >> 1 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsCloning
  {
    get => ((int) this.m_bFlags1 & 4) >> 2 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 251 | (value ? 1 : 0) << 2);
  }

  internal bool DocHasThemes
  {
    get => ((int) this.m_bFlags1 & 8) >> 3 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 247 | (value ? 1 : 0) << 3);
  }

  internal bool CreateBaseStyle
  {
    get => ((int) this.m_bFlags1 & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsNormalStyleDefined
  {
    get => ((int) this.m_bFlags1 & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsDefaultParagraphFontStyleDefined
  {
    get => ((int) this.m_bFlags2 & 8) >> 3 != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsHTMLImport
  {
    get => ((int) this.m_bFlags2 & 1) != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 254 | (value ? 1 : 0));
  }

  internal bool IsSkipFieldDetach
  {
    get => ((int) this.m_bFlags2 & 2) >> 1 != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsFieldRangeAdding
  {
    get => ((int) this.m_bFlags2 & 4) >> 2 != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsReadOnly
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool BordersSurroundHeader
  {
    get => this.m_doc.DOP.Dop97.IncludeHeader;
    set => this.m_doc.DOP.Dop97.IncludeHeader = value;
  }

  internal bool BordersSurroundFooter
  {
    get => this.m_doc.DOP.Dop97.IncludeFooter;
    set => this.m_doc.DOP.Dop97.IncludeFooter = value;
  }

  internal bool DifferentOddAndEvenPages
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  internal ushort WordVersion
  {
    get => this.m_wordVersion;
    set
    {
      this.m_wordVersion = value;
      this.m_fibVersion = value;
    }
  }

  internal List<Font> UsedFontNames
  {
    get
    {
      if (this.m_usedFonts == null)
        this.m_usedFonts = new List<Font>();
      return this.m_usedFonts;
    }
    set => this.m_usedFonts = value;
  }

  internal bool HasTOC => this.m_tableOfContent != null;

  internal Dictionary<WField, TableOfContent> TOC
  {
    get
    {
      return this.HasTOC ? this.m_tableOfContent : (this.m_tableOfContent = new Dictionary<WField, TableOfContent>());
    }
    set => this.m_tableOfContent = value;
  }

  internal string HtmlBaseUrl
  {
    get => this.m_htmlBaseUrl;
    set => this.m_htmlBaseUrl = value;
  }

  internal FieldCollection Fields
  {
    get
    {
      if (this.m_fields == null)
        this.m_fields = new FieldCollection(this);
      return this.m_fields;
    }
  }

  internal List<Shape> AutoShapeCollection
  {
    get
    {
      if (this.m_AutoShapeCollection == null)
        this.m_AutoShapeCollection = new List<Shape>();
      return this.m_AutoShapeCollection;
    }
    set => this.m_AutoShapeCollection = value;
  }

  internal void SetDefaultSectionFormatting(WSection destination)
  {
    destination.SectionFormat.PageSetup.PageSize = new SizeF(612f, 792f);
    destination.SectionFormat.PageSetup.HeaderDistance = 36f;
    destination.SectionFormat.PageSetup.FooterDistance = 36f;
    destination.SectionFormat.PageSetup.LinePitch = 18f;
    destination.SectionFormat.PageSetup.EqualColumnWidth = true;
    destination.SectionFormat.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
    destination.SectionFormat.PageSetup.RestartPageNumbering = false;
    destination.SectionFormat.PageSetup.Margins.Left = 72f;
    destination.SectionFormat.PageSetup.Margins.Right = 72f;
    destination.SectionFormat.PageSetup.Margins.Top = 72f;
    destination.SectionFormat.PageSetup.Margins.Bottom = 72f;
    destination.SectionFormat.PageSetup.VerticalAlignment = PageAlignment.Top;
    destination.SectionFormat.PageSetup.PageNumbers.ChapterPageSeparator = ChapterPageSeparatorType.Hyphen;
    destination.SectionFormat.PageSetup.PageNumbers.HeadingLevelForChapter = HeadingLevel.None;
    destination.SectionFormat.PageSetup.Margins = destination.SectionFormat.PageSetup.Margins;
    destination.SectionFormat.PageSetup.PageNumbers = destination.SectionFormat.PageSetup.PageNumbers;
    destination.SectionFormat.PageSetup.Borders = new Borders();
    destination.SectionFormat.PageSetup = destination.SectionFormat.PageSetup;
  }

  internal List<Entity> FloatingItems
  {
    get
    {
      if (this.m_FloatingItems == null)
        this.m_FloatingItems = new List<Entity>();
      return this.m_FloatingItems;
    }
    set => this.m_FloatingItems = value;
  }

  internal List<Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new List<Stream>();
      return this.m_docxProps;
    }
  }

  internal Dictionary<string, string> ListStyleNames
  {
    get
    {
      if (this.m_listStyleNames == null)
        this.m_listStyleNames = new Dictionary<string, string>();
      return this.m_listStyleNames;
    }
  }

  internal bool HasDocxProps => this.m_docxProps != null;

  internal bool IsClosing
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    private set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal Dictionary<string, string> StyleNameIds
  {
    get
    {
      if (this.m_styleNameIds == null)
        this.m_styleNameIds = new Dictionary<string, string>();
      return this.m_styleNameIds;
    }
  }

  internal bool HasCoverPage
  {
    get => ((int) this.m_bFlags2 & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags2 = (byte) ((int) this.m_bFlags2 & 191 | (value ? 1 : 0) << 6);
  }

  internal int TrackChangesBalloonCount
  {
    get => this.m_balloonCount;
    set => this.m_balloonCount = value;
  }

  internal Dictionary<string, int> SequenceFieldResults
  {
    get => this.m_sequenceFieldResults;
    set => this.m_sequenceFieldResults = value;
  }

  public WordDocument(string fileName)
    : this()
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.WordDocumentType(fileName, (string) null);
  }

  public WordDocument(string fileName, string password)
    : this()
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    if (password == null)
      throw new ArgumentNullException(nameof (Password));
    this.WordDocumentType(fileName, password);
  }

  public WordDocument(string fileName, FormatType type)
    : this()
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.OpenInternal(fileName, type, (string) null);
  }

  public WordDocument(string fileName, FormatType type, XHTMLValidationType validationType)
    : this()
  {
    if (type == FormatType.Automatic)
      type = this.GetFormatType(fileName, false);
    this.Open(fileName, type, validationType);
  }

  public WordDocument(string fileName, FormatType type, string password)
    : this()
  {
    if (type == FormatType.Automatic)
      type = this.GetFormatType(fileName, false);
    this.Open(fileName, type, password);
  }

  private void WordDocumentType(string fileName, string password)
  {
    fileName = fileName.ToLower();
    if (!File.Exists(fileName))
      throw new Exception("Cannot recognize current file path");
    if (!string.IsNullOrEmpty(Path.GetExtension(fileName)))
    {
      FormatType formatType = this.GetFormatType(fileName, false);
      this.OpenInternal(fileName, formatType, password);
    }
    else
    {
      try
      {
        this.OpenInternal(fileName, FormatType.Doc, password);
        return;
      }
      catch
      {
      }
      try
      {
        this.OpenInternal(fileName, FormatType.Docx, password);
      }
      catch
      {
        throw new Exception("Cannot recognize file format");
      }
    }
  }

  public WordDocument(Stream stream, FormatType type, XHTMLValidationType validationType)
    : this()
  {
    this.Open(stream, type, validationType);
  }

  public WordDocument()
    : base((WordDocument) null, (Entity) null)
  {
    this.syncfusionLicense = FusionLicenseProvider.GetLicenseType(Platform.FileFormats);
    this.m_doc = this;
    if (!this.IsOpening)
      this.m_actualFormatType = FormatType.Docx;
    this.Init();
  }

  public WordDocument(Stream stream)
    : this()
  {
    FormatType formatType = FormatType.Automatic;
    if (stream == null)
      throw new ArgumentNullException("Stream");
    this.OpenInternal(stream, formatType, (string) null);
  }

  public WordDocument(Stream stream, FormatType type)
    : this()
  {
    this.Open(stream, type);
  }

  public WordDocument(Stream stream, string password)
    : this()
  {
    FormatType formatType = FormatType.Automatic;
    this.Open(stream, formatType, password);
  }

  public WordDocument(Stream stream, FormatType type, string password)
    : this()
  {
    this.Open(stream, type, password);
  }

  protected WordDocument(WordDocument doc)
    : this()
  {
    this.m_standardAsciiFont = doc.StandardAsciiFont;
    this.m_standardFarEastFont = doc.StandardFarEastFont;
    this.m_standardNonFarEastFont = doc.StandardNonFarEastFont;
    this.m_standardBidiFont = doc.m_standardBidiFont;
    this.m_viewSetup = doc.ViewSetup.Clone(this);
    this.DefaultTabWidth = doc.DefaultTabWidth;
    this.ActualFormatType = doc.ActualFormatType;
    if (doc.BuiltinDocumentProperties != null)
      this.m_builtinProp = doc.BuiltinDocumentProperties.Clone();
    if (doc.CustomDocumentProperties != null)
      this.m_customProp = doc.CustomDocumentProperties.Clone();
    if (doc.Watermark != null && doc.Watermark.Type != WatermarkType.NoWatermark)
      this.Watermark = (Watermark) doc.Watermark.Clone();
    if (doc.Background.Type != BackgroundType.NoBackground)
    {
      this.m_background = doc.Background.Clone();
      this.m_background.SetOwner((OwnerHolder) this);
      this.m_background.UpdateImageRecord(this);
    }
    if (doc.DOP != null)
      this.m_dop = doc.DOP.Clone();
    if (doc.DefCharFormat != null)
    {
      this.m_defCharFormat = new WCharacterFormat((IWordDocument) this);
      this.m_defCharFormat.ImportContainer((FormatBase) doc.DefCharFormat);
    }
    if (doc.DefParaFormat != null)
    {
      this.m_defParaFormat = new WParagraphFormat((IWordDocument) this);
      this.m_defParaFormat.ImportContainer((FormatBase) doc.DefParaFormat);
    }
    foreach (KeyValuePair<string, string> keyValuePair in doc.FontSubstitutionTable)
    {
      if (!this.FontSubstitutionTable.ContainsKey(keyValuePair.Key))
        this.FontSubstitutionTable.Add(keyValuePair.Key, keyValuePair.Value);
      else
        this.FontSubstitutionTable[keyValuePair.Key] = keyValuePair.Value;
    }
    this.Footnotes = doc.Footnotes.Clone();
    this.Endnotes = doc.Endnotes.Clone();
    this.ImportContent((IWordDocument) doc);
  }

  private FormatType GetFormatType(string fileName, bool isStorageFile)
  {
    switch ((isStorageFile ? fileName : Path.GetExtension(fileName)).ToLower())
    {
      case ".doc":
      case ".dot":
        return FormatType.Doc;
      case ".docx":
        return this.m_actualFormatType == FormatType.StrictDocx ? FormatType.StrictDocx : FormatType.Docx;
      case ".odt":
        return FormatType.Odt;
      case ".dotx":
        return FormatType.Dotx;
      case ".docm":
        return FormatType.Docm;
      case ".dotm":
        return FormatType.Dotm;
      case ".txt":
        return FormatType.Txt;
      case ".htm":
      case ".html":
        return FormatType.Html;
      case ".rtf":
        return FormatType.Rtf;
      case ".xml":
        return FormatType.WordML;
      case ".epub":
        return FormatType.EPub;
      default:
        throw new Exception("Cannot recognize current file type");
    }
  }

  public IWParagraph CreateParagraph() => (IWParagraph) new WParagraph((IWordDocument) this);

  public void EnsureMinimal()
  {
    if (this.Sections.Count != 0)
      return;
    this.AddSection().Body.AddParagraph();
  }

  public IWSection AddSection()
  {
    WSection section = new WSection((IWordDocument) this.Document);
    if (this.m_sections != null && this.m_sections.Count > 0)
    {
      WPageSetup pageSetup1 = this.m_sections[this.m_sections.Count - 1].PageSetup;
      WPageSetup pageSetup2 = section.PageSetup;
      pageSetup2.Margins = pageSetup1.Margins.Clone();
      pageSetup2.PageSize = pageSetup1.ClonePageSize();
      pageSetup2.Orientation = pageSetup1.Orientation;
    }
    this.Sections.Add((IWSection) section);
    return (IWSection) section;
  }

  public IWParagraphStyle AddParagraphStyle(string styleName)
  {
    return this.AddStyle(StyleType.ParagraphStyle, styleName) as IWParagraphStyle;
  }

  public IWCharacterStyle AddCharacterStyle(string styleName)
  {
    return this.AddStyle(StyleType.CharacterStyle, styleName) as IWCharacterStyle;
  }

  public ListStyle AddListStyle(ListType listType, string styleName)
  {
    ListStyle style = new ListStyle((IWordDocument) this, listType);
    this.ListStyles.Add(style);
    style.Name = styleName;
    return style;
  }

  public string GetText() => new TextConverter().GetText(this);

  public WordDocument Clone() => (WordDocument) this.CloneImpl();

  public void ImportSection(IWSection section) => this.Sections.Add((IWSection) section.Clone());

  public void ImportContent(IWordDocument doc) => this.ImportContent(doc, true);

  public void ImportContent(IWordDocument doc, ImportOptions importOptions)
  {
    (doc as WordDocument).IsCloning = true;
    bool importStyles = this.ImportStyles;
    ImportOptions importOption = this.m_importOption;
    bool importedListCache = this.Settings.MaintainImportedListCache;
    this.ImportOptions = importOptions;
    this.ImportStyles = true;
    this.Settings.MaintainImportedListCache = false;
    if ((this.m_importOption & ImportOptions.UseDestinationStyles) != (ImportOptions) 0)
      this.ImportStyles = false;
    if ((this.m_importOption & ImportOptions.KeepTextOnly) != (ImportOptions) 0)
    {
      this.ImportDocumentText(doc);
    }
    else
    {
      doc.Sections.CloneTo((EntityCollection) this.Sections);
      if ((doc as WordDocument).m_revisions != null && (doc as WordDocument).m_revisions.Count > 0)
      {
        if (this.m_revisions == null)
          this.m_revisions = new RevisionCollection(this);
        foreach (Revision revision in (CollectionImpl) (doc as WordDocument).m_revisions)
          this.m_revisions.Add(revision);
      }
      this.CopyBinaryData((doc as WordDocument).MacrosData, ref this.m_macrosData);
      this.CopyBinaryData((doc as WordDocument).MacroCommands, ref this.m_macroCommands);
      if (this.m_docxPackage == null && (doc as WordDocument).DocxPackage != null)
        this.m_docxPackage = (doc as WordDocument).DocxPackage.Clone();
      this.m_docxProps = (doc as WordDocument).m_docxProps;
    }
    (doc as WordDocument).IsCloning = false;
    this.m_importOption = importOption;
    this.ImportStyles = importStyles;
    this.Settings.MaintainImportedListCache = importedListCache;
    this.Settings.DuplicateListStyleNames = string.Empty;
  }

  private void ImportDocumentText(IWordDocument doc)
  {
    string text1 = doc.Sections.GetText();
    this.m_prevClonedEntity = (TextBodyItem) null;
    IWSection wsection = this.AddSection();
    string str = text1;
    char[] chArray = new char[1]{ '\r' };
    foreach (string text2 in str.Split(chArray))
      wsection.AddParagraph().AppendText(text2);
  }

  public void ImportContent(IWordDocument doc, bool importStyles)
  {
    (doc as WordDocument).IsCloning = true;
    bool importStyles1 = this.ImportStyles;
    ImportOptions importOption = this.m_importOption;
    bool importedListCache = this.Settings.MaintainImportedListCache;
    this.ImportStyles = importStyles;
    this.m_importOption = ImportOptions.UseDestinationStyles;
    this.Settings.MaintainImportedListCache = false;
    doc.Sections.CloneTo((EntityCollection) this.Sections);
    foreach (KeyValuePair<string, string> styleNameId in (doc as WordDocument).StyleNameIds)
    {
      if (!this.StyleNameIds.ContainsKey(styleNameId.Key))
        this.StyleNameIds.Add(styleNameId.Key, styleNameId.Value);
    }
    int index1 = 0;
    for (int count = doc.Styles.Count; index1 < count; ++index1)
    {
      Style style = doc.Styles[index1] as Style;
      string name = style.Name;
      List<string> styelNames = new List<string>();
      bool isDiffTypeStyleFound = false;
      if (!((this.Styles as StyleCollection).FindByName(name, style.StyleType, ref styelNames, ref isDiffTypeStyleFound) is Style))
      {
        if (!isDiffTypeStyleFound)
          this.Styles.Add(style.Clone());
        else if (this.ImportStylesOnTypeMismatch)
        {
          style.SetStyleName(style.GetUniqueStyleName(name, styelNames));
          this.Styles.Add(style.Clone());
        }
      }
      styelNames.Clear();
    }
    int index2 = 0;
    for (int count = doc.ListStyles.Count; index2 < count; ++index2)
    {
      ListStyle listStyle = doc.ListStyles[index2];
      if (listStyle != null && !this.ListStyles.HasEquivalentStyle(listStyle))
        this.ListStyles.Add((ListStyle) listStyle.Clone());
    }
    foreach (KeyValuePair<string, string> keyValuePair in (doc as WordDocument).FontSubstitutionTable)
    {
      if (!this.FontSubstitutionTable.ContainsKey(keyValuePair.Key))
        this.FontSubstitutionTable.Add(keyValuePair.Key, keyValuePair.Value);
      else
        this.FontSubstitutionTable[keyValuePair.Key] = keyValuePair.Value;
    }
    int index3 = 0;
    for (int count = (doc as WordDocument).ListOverrides.Count; index3 < count; ++index3)
    {
      ListOverrideStyle listOverride = (doc as WordDocument).ListOverrides[index3];
      if (listOverride != null && !this.ListOverrides.HasEquivalentStyle(listOverride))
        this.ListOverrides.Add((ListOverrideStyle) listOverride.Clone());
    }
    this.CopyBinaryData((doc as WordDocument).MacrosData, ref this.m_macrosData);
    this.CopyBinaryData((doc as WordDocument).MacroCommands, ref this.m_macroCommands);
    if ((doc as WordDocument).DefCharFormat != null)
    {
      if (this.m_defCharFormat == null)
        this.m_defCharFormat = new WCharacterFormat((IWordDocument) this.m_doc);
      this.m_defCharFormat.ImportContainer((FormatBase) (doc as WordDocument).DefCharFormat);
    }
    if (this.m_defParaFormat == null)
    {
      this.m_defParaFormat = new WParagraphFormat((IWordDocument) this.m_doc);
      this.m_defParaFormat.ImportContainer((FormatBase) (doc as WordDocument).DefParaFormat);
    }
    if (!this.m_doc.DocHasThemes && (doc as WordDocument).DocHasThemes)
    {
      this.m_doc.Themes.FontScheme.FontSchemeName = (doc as WordDocument).Themes.FontScheme.FontSchemeName;
      this.CopyFontScheme((doc as WordDocument).Themes.FontScheme.MajorFontScheme, this.m_doc.Themes.FontScheme.MajorFontScheme);
      this.CopyFontScheme((doc as WordDocument).Themes.FontScheme.MinorFontScheme, this.m_doc.Themes.FontScheme.MinorFontScheme);
    }
    if (this.m_docxPackage == null && (doc as WordDocument).DocxPackage != null)
      this.m_docxPackage = (doc as WordDocument).DocxPackage.Clone();
    this.m_docxProps = (doc as WordDocument).m_docxProps;
    (doc as WordDocument).IsCloning = false;
    this.ImportStyles = importStyles1;
    this.m_importOption = importOption;
    this.Settings.MaintainImportedListCache = importedListCache;
  }

  private void CopyFontScheme(MajorMinorFontScheme src, MajorMinorFontScheme dest)
  {
    foreach (FontSchemeStruct fontScheme in src.FontSchemeList)
      dest.FontSchemeList.Add(new FontSchemeStruct()
      {
        Name = fontScheme.Name,
        Typeface = fontScheme.Typeface,
        Charset = fontScheme.Charset,
        Panose = fontScheme.Panose,
        PitchFamily = fontScheme.PitchFamily
      });
  }

  public IStyle AddStyle(BuiltinStyle builtinStyle)
  {
    this.CheckNormalStyle();
    string name = Style.BuiltInToName(builtinStyle);
    IStyle style = this.Document.Styles.FindByName(name);
    if (style == null)
    {
      if (this.IsBuiltInCharacterStyle(builtinStyle))
      {
        style = Style.CreateBuiltinCharacterStyle(builtinStyle, this.Document);
        this.Document.Styles.Add(style);
      }
      else
      {
        style = Style.CreateBuiltinStyle(builtinStyle, this.Document);
        this.Document.Styles.Add(style);
        if (builtinStyle != BuiltinStyle.MacroText && builtinStyle != BuiltinStyle.CommentSubject)
          (this.Document.Styles.FindByName(name) as Style).ApplyBaseStyle("Normal");
        this.UpdateNextStyle(style as Style);
      }
    }
    return style;
  }

  internal void UpdateNextStyle(Style style)
  {
    if (style.Name.Contains("List") || !(style.Name != "No Spacing"))
      return;
    style.NextStyle = "Normal";
  }

  public void AcceptChanges()
  {
    foreach (WSection section in (CollectionImpl) this.Sections)
      section.MakeChanges(true);
    this.Document.Revisions.AcceptAll();
  }

  public void RejectChanges() => this.Document.Revisions.RejectAll();

  public void Protect(ProtectionType type)
  {
    this.ResetProtectionTypesValues();
    this.Protect(type, (string) null);
  }

  public void Protect(ProtectionType type, string password)
  {
    this.ResetProtectionTypesValues();
    this.DOP.SetProtection(type, password);
  }

  private void ResetProtectionTypesValues()
  {
    this.Settings.CryptProviderTypeValue = "rsaFull";
    this.Settings.CryptAlgorithmTypeValue = "typeAny";
    this.Settings.CryptAlgorithmClassValue = "hash";
    this.Settings.CryptAlgorithmSidValue = 4.ToString();
    this.Settings.CryptSpinCountValue = 100000.ToString();
  }

  public void EncryptDocument(string password)
  {
    this.m_password = !string.IsNullOrEmpty(password) ? password : throw new Exception("Password cannot be null or empty!");
  }

  public void RemoveEncryption() => this.m_password = (string) null;

  internal IStyle AddStyle(StyleType styleType, string styleName)
  {
    if (styleType == StyleType.OtherStyle)
      throw new NotSupportedException();
    IStyle style = (IStyle) null;
    switch (styleType)
    {
      case StyleType.ParagraphStyle:
        style = (IStyle) new WParagraphStyle((IWordDocument) this.Document);
        break;
      case StyleType.CharacterStyle:
        style = (IStyle) new WCharacterStyle(this.Document);
        break;
      case StyleType.TableStyle:
        style = (IStyle) new WTableStyle((IWordDocument) this.Document);
        break;
    }
    if (style != null)
    {
      if (styleName != null && styleName.Length > 0)
        style.Name = styleName;
      this.Styles.Add(style);
    }
    return style;
  }

  private void CheckNormalStyle()
  {
    if ((IStyle) (this.Document.Styles.FindByName("Normal", StyleType.ParagraphStyle) as WParagraphStyle) == null)
      this.Document.Styles.Add(Style.CreateBuiltinStyle(BuiltinStyle.Normal, this.Document));
    if ((IStyle) (this.Document.Styles.FindByName("Default Paragraph Font", StyleType.CharacterStyle) as WCharacterStyle) != null)
      return;
    IStyle builtinCharacterStyle = Style.CreateBuiltinCharacterStyle(BuiltinStyle.DefaultParagraphFont, this.Document);
    (builtinCharacterStyle as Style).IsSemiHidden = true;
    (builtinCharacterStyle as Style).UnhideWhenUsed = true;
    this.Document.Styles.Add(builtinCharacterStyle);
  }

  public List<Entity> GetCrossReferenceItems(ReferenceType refernceType)
  {
    return refernceType == ReferenceType.Bookmark ? this.GetBookmarksValue() : new List<Entity>();
  }

  private List<Entity> GetBookmarksValue()
  {
    List<Entity> bookmarksValue = new List<Entity>();
    foreach (Bookmark bookmark in (CollectionImpl) this.Bookmarks)
    {
      if (bookmark.BookmarkStart != null && bookmark.BookmarkEnd != null && !bookmark.Name.StartsWithExt("_"))
        bookmarksValue.Add((Entity) bookmark.BookmarkStart);
    }
    return bookmarksValue;
  }

  private void OpenDocx(string fileName)
  {
    DocxParser docxParser = new DocxParser();
    this.IsOpening = true;
    docxParser.Read(fileName, this);
    this.AddEmptyParagraph();
    this.IsOpening = false;
  }

  private void OpenDocx(Stream stream)
  {
    DocxParser docxParser = new DocxParser();
    this.IsOpening = true;
    docxParser.Read(stream, this);
    this.AddEmptyParagraph();
    this.IsOpening = false;
  }

  private void OpenWordML(string fileName)
  {
    DocxParser docxParser = new DocxParser();
    this.IsOpening = true;
    docxParser.ReadWordML(fileName, this);
    this.IsOpening = false;
  }

  private void OpenWordML(Stream stream)
  {
    DocxParser docxParser = new DocxParser();
    this.IsOpening = true;
    docxParser.ReadWordML(stream, this);
    this.IsOpening = false;
  }

  internal void OpenXml(string fileName)
  {
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
      this.OpenXml((Stream) fileStream);
  }

  internal void OpenXml(Stream stream)
  {
    XmlReaderSettings settings = new XmlReaderSettings();
    settings.ValidationType = ValidationType.Schema;
    settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
    this.ReadXml(XmlReader.Create(stream, settings));
  }

  private static void ValidationCallBack(object sender, ValidationEventArgs args)
  {
    if (args.Severity != XmlSeverityType.Warning)
      throw new XDLSException("\tValidation error: " + args.Message);
  }

  internal void SaveXml(string fileName)
  {
    XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
    writer.Formatting = Formatting.Indented;
    this.WriteXml((XmlWriter) writer);
    writer.Close();
  }

  internal void SaveXml(Stream stream)
  {
    XmlTextWriter writer = new XmlTextWriter(stream, Encoding.Unicode);
    try
    {
      writer.Formatting = Formatting.Indented;
      this.WriteXml((XmlWriter) writer);
    }
    finally
    {
      writer.Flush();
    }
  }

  internal void SaveTxt(string fileName)
  {
    this.SaveTxt(fileName, (Encoding) new UTF8Encoding(false));
  }

  public void SaveTxt(string fileName, Encoding encoding)
  {
    StreamWriter writer = new StreamWriter(fileName, false, encoding);
    new TextConverter().Write(writer, (IWordDocument) this);
    writer.Close();
  }

  private void SaveEPub(string fileName, WPicture coverImage)
  {
    new EPubConverter()
    {
      FileName = Path.GetFileName(fileName).Replace(Path.GetExtension(fileName), string.Empty),
      CoverImage = coverImage
    }.ConvertToEPub(fileName, this);
  }

  private void SaveEPub(Stream stream, WPicture coverImage)
  {
    new EPubConverter() { CoverImage = coverImage }.ConvertToEPub(stream, this);
  }

  private void SaveDocx(string fileName)
  {
    this.SortByZIndex(false);
    new DocxSerializator().Serialize(fileName, this);
  }

  private void SaveWordML(string fileName)
  {
    this.SortByZIndex(false);
    new DocxSerializator().SerializeWordML(fileName, this);
  }

  private void SaveODT(string fileName) => new DocToODTConverter(this).ConvertToODF(fileName);

  private void SaveDocx(Stream stream)
  {
    this.SortByZIndex(false);
    new DocxSerializator().Serialize(stream, this);
  }

  private void SaveODT(Stream stream) => new DocToODTConverter(this.m_doc).ConvertToODF(stream);

  private void SaveWordML(Stream stream)
  {
    this.SortByZIndex(false);
    new DocxSerializator().SerializeWordML(stream, this);
  }

  private void SaveRtf(Stream stream) => new RtfWriter().Write(stream, (IWordDocument) this);

  internal string GetRtfText() => new RtfWriter().GetRtfText((IWordDocument) this);

  internal void SaveTxt(Stream stream) => this.SaveTxt(stream, (Encoding) new UTF8Encoding(false));

  public void SaveTxt(Stream stream, Encoding encoding)
  {
    StreamWriter writer = new StreamWriter(stream, encoding);
    try
    {
      new TextConverter().Write(writer, (IWordDocument) this);
    }
    finally
    {
      writer.Flush();
    }
  }

  internal void OpenTxt(Stream stream)
  {
    Encoding encoding = WordDocument.GetEncoding("Windows-1252");
    if (Utf8Checker.IsUtf8(stream))
      encoding = Encoding.UTF8;
    new TextConverter().Read(new StreamReader(stream, encoding), (IWordDocument) this);
  }

  internal void OpenText(string text) => new TextConverter().Read(text, (IWordDocument) this);

  internal void OpenHTML(Stream stream, XHTMLValidationType validationType)
  {
    string codePageName = "Windows-1252";
    if (Utf8Checker.IsUtf8(stream))
      codePageName = "utf-8";
    StreamReader streamReader = new StreamReader(stream, WordDocument.GetEncoding(codePageName));
    string end = streamReader.ReadToEnd();
    streamReader.Close();
    if (this.Sections.Count == 0)
      this.AddSection();
    this.XHTMLValidateOption = validationType;
    this.LastSection.PageSetup.Margins.All = 72f;
    this.IsHTMLImport = true;
    this.LastSection.Body.InsertXHTML(end, 0);
    this.DOP.Dop2000.Copts.SplitPgBreakAndParaMark = false;
    this.DOP.Dop2000.Copts.DontUseIndentAsNumberingTabStop = false;
    this.DOP.Dop2000.Copts.UseNormalStyleForList = false;
    this.DOP.Dop2000.Copts.FELineBreak11 = false;
    this.DOP.Dop2000.Copts.AllowSpaceOfSameStyleInTable = false;
    this.DOP.Dop2000.Copts.WW11IndentRules = false;
    this.DOP.Dop2000.Copts.DontAutofitConstrainedTables = false;
    this.DOP.Dop2000.Copts.AutofitLikeWW11 = false;
    this.DOP.Dop2000.Copts.HangulWidthLikeWW11 = false;
    this.DOP.Dop2000.Copts.DontVertAlignCellWithSp = false;
    this.DOP.Dop2000.Copts.DontBreakConstrainedForcedTables = false;
    this.DOP.Dop2000.Copts.DontVertAlignInTxbx = false;
    this.DOP.Dop2000.Copts.Word11KerningPairs = false;
    if (this.LastSection.Body.Items.Count != 0 && !(this.LastSection.Body.Items.LastItem is WTable))
      return;
    this.LastSection.Body.Items.Insert(this.LastSection.Body.Items.Count, (IEntity) new WParagraph((IWordDocument) this.Document));
  }

  private void OpenDoc(string fileName)
  {
    DocReaderAdapter docReaderAdapter = new DocReaderAdapter();
    this.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark] = true;
    using (WordReader reader = new WordReader(fileName))
      docReaderAdapter.Read(reader, this);
    this.AddEmptyParagraph();
  }

  public void Open(string fileName)
  {
    FormatType formatType = fileName != null ? this.GetFormatType(fileName, false) : throw new ArgumentNullException("FileName");
    this.Open(fileName, formatType);
  }

  public void Open(string fileName, FormatType formatType)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.OpenInternal(fileName, formatType, (string) null);
  }

  public void Open(
    string fileName,
    FormatType formatType,
    XHTMLValidationType validationType,
    string baseUrl)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.HtmlBaseUrl = baseUrl != null ? baseUrl : throw new ArgumentNullException("BaseUrl");
    this.OpenInternal(fileName, formatType, validationType);
  }

  public void Open(string fileName, FormatType formatType, XHTMLValidationType validationType)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.OpenInternal(fileName, formatType, validationType);
  }

  private void OpenInternal(
    string fileName,
    FormatType formatType,
    XHTMLValidationType validationType)
  {
    if (formatType == FormatType.Automatic)
      formatType = this.GetFormatType(fileName, false);
    fileName = this.CheckExtension(fileName, formatType);
    this.UpdateFormatType(fileName, ref formatType);
    this.ActualFormatType = formatType;
    if (FormatType.Html == formatType)
    {
      using (Stream stream = (Stream) new FileStream(fileName, FileMode.Open))
      {
        this.Init();
        this.HtmlBaseUrl = Path.GetDirectoryName(fileName).TrimEnd('\\');
        this.OpenHTML(stream, validationType);
      }
      this.RemoveTrailVersionWatermark(formatType);
    }
    else
      this.Open(fileName, formatType);
  }

  public void Open(string fileName, FormatType formatType, string password)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    if (password == null)
      throw new ArgumentNullException("Password");
    this.OpenInternal(fileName, formatType, password);
  }

  private void OpenInternal(string fileName, FormatType formatType, string password)
  {
    this.Init();
    this.CheckFileName(fileName);
    this.Password = password;
    if (formatType == FormatType.Automatic)
      formatType = this.GetFormatType(fileName, false);
    fileName = this.CheckExtension(fileName, formatType);
    this.UpdateFormatType(fileName, ref formatType);
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      this.UpdateFormatType((Stream) fileStream, ref formatType);
    this.ActualFormatType = formatType;
    switch (formatType)
    {
      case FormatType.Doc:
      case FormatType.Dot:
        this.OpenDoc(fileName);
        break;
      case FormatType.Docx:
      case FormatType.Word2007:
      case FormatType.Word2010:
      case FormatType.Word2013:
      case FormatType.Word2007Dotx:
      case FormatType.Word2010Dotx:
      case FormatType.Word2013Dotx:
      case FormatType.Dotx:
      case FormatType.Word2007Docm:
      case FormatType.Word2010Docm:
      case FormatType.Word2013Docm:
      case FormatType.Docm:
      case FormatType.Word2007Dotm:
      case FormatType.Word2010Dotm:
      case FormatType.Word2013Dotm:
      case FormatType.Dotm:
        this.OpenDocx(fileName);
        break;
      case FormatType.WordML:
        this.OpenWordML(fileName);
        break;
      case FormatType.Rtf:
        using (Stream stream = (Stream) new FileStream(fileName, FileMode.Open))
        {
          this.OpenRtf(stream);
          break;
        }
      case FormatType.Txt:
        using (Stream stream = (Stream) new FileStream(fileName, FileMode.Open))
        {
          this.OpenTxt(stream);
          break;
        }
      case FormatType.Xml:
        this.OpenXml(fileName);
        break;
      case FormatType.Html:
        using (Stream stream = (Stream) new FileStream(fileName, FileMode.Open))
        {
          this.HtmlBaseUrl = Path.GetDirectoryName(fileName);
          this.Open(stream, formatType, XHTMLValidationType.Transitional);
          break;
        }
      default:
        throw new NotSupportedException("DocIO does not support this file format");
    }
    if (formatType == FormatType.Html)
      return;
    this.RemoveTrailVersionWatermark(formatType);
  }

  private void UpdateFormatType(string fileName, ref FormatType formatType)
  {
    FileStream data = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    data.Position = 0L;
    if (this.CheckForEncryption((Stream) data))
    {
      using (ICompoundFile compoundFile = this.CreateCompoundFile((Stream) data))
      {
        SecurityHelper.EncrytionType encryptionType = new SecurityHelper().GetEncryptionType(compoundFile.RootStorage);
        data.Position = 0L;
        if (encryptionType != SecurityHelper.EncrytionType.None)
        {
          formatType = FormatType.Docx;
          data.Close();
          return;
        }
      }
    }
    data.Position = 0L;
    byte[] buffer = new byte[5];
    if (data.Read(buffer, 0, 5) == 5 && buffer[0] == (byte) 80 /*0x50*/ && buffer[1] == (byte) 75)
    {
      data.Position = 0L;
      formatType = FormatType.Docx;
    }
    else if (buffer[0] == (byte) 123 && buffer[1] == (byte) 92 && buffer[2] == (byte) 114 && buffer[3] == (byte) 116 && buffer[4] == (byte) 102)
    {
      data.Position = 0L;
      formatType = FormatType.Rtf;
    }
    else
    {
      data.Position = 0L;
      if (Syncfusion.CompoundFile.DocIO.Net.CompoundFile.CheckHeader((Stream) data))
      {
        data.Position = 0L;
        formatType = FormatType.Doc;
      }
      else
      {
        try
        {
          XmlReader reader = UtilityMethods.CreateReader((Stream) data);
          reader.MoveToElement();
          formatType = reader.LocalName == "wordDocument" || reader.LocalName == "package" ? FormatType.WordML : (!(reader.LocalName == "html") ? this.GetFormatType(fileName, false) : FormatType.Html);
        }
        catch
        {
          formatType = this.GetFormatType(fileName, false);
        }
      }
    }
    data.Close();
  }

  public void OpenReadOnly(string strFileName, FormatType formatType)
  {
    using (FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      this.IsReadOnly = true;
      this.Open((Stream) fileStream, formatType);
    }
  }

  public void OpenReadOnly(string fileName, FormatType formatType, string password)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    if (password == null)
      throw new ArgumentNullException("Password");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      this.IsReadOnly = true;
      this.Open((Stream) fileStream, formatType, password);
    }
  }

  public void OpenReadOnly(
    string fileName,
    FormatType formatType,
    XHTMLValidationType validationType)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      this.IsReadOnly = true;
      this.Open((Stream) fileStream, formatType, validationType);
    }
  }

  public void OpenReadOnly(
    string fileName,
    FormatType formatType,
    XHTMLValidationType validationType,
    string baseUrl)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    if (baseUrl == null)
      throw new ArgumentNullException("BaseUrl");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      this.IsReadOnly = true;
      this.HtmlBaseUrl = baseUrl;
      this.OpenInternal((Stream) fileStream, formatType, validationType);
    }
  }

  public void Save(string fileName) => this.Save(fileName, FormatType.Automatic);

  public void Save(string fileName, FormatType formatType)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    this.SaveInternal(fileName, formatType);
  }

  private void SaveInternal(string fileName, FormatType formatType)
  {
    if (this.UpdateFields)
      this.UpdateDocumentFields();
    if (formatType == FormatType.Automatic)
      formatType = this.GetFormatType(fileName, false);
    this.SaveFormatType = formatType;
    if (formatType != FormatType.Txt && formatType != FormatType.Html && formatType != FormatType.EPub)
    {
      if (formatType != FormatType.Odt)
        this.AddTrailVersionWatermark();
      else
        this.AddTrailVersionWatermarkForODT();
    }
    switch (formatType)
    {
      case FormatType.Doc:
        this.SaveDoc(fileName);
        break;
      case FormatType.Dot:
        this.SaveDot(fileName);
        break;
      case FormatType.Docx:
      case FormatType.StrictDocx:
      case FormatType.Word2007:
      case FormatType.Word2010:
      case FormatType.Word2013:
      case FormatType.Word2007Dotx:
      case FormatType.Word2010Dotx:
      case FormatType.Word2013Dotx:
      case FormatType.Dotx:
      case FormatType.Word2007Docm:
      case FormatType.Word2010Docm:
      case FormatType.Word2013Docm:
      case FormatType.Docm:
      case FormatType.Word2007Dotm:
      case FormatType.Word2010Dotm:
      case FormatType.Word2013Dotm:
      case FormatType.Dotm:
        this.SaveDocx(fileName);
        break;
      case FormatType.WordML:
        this.SaveWordML(fileName);
        break;
      case FormatType.Rtf:
        this.SaveRtf(fileName);
        break;
      case FormatType.Txt:
        this.SaveTxt(fileName);
        break;
      case FormatType.EPub:
        this.SaveEPub(fileName, (WPicture) null);
        break;
      case FormatType.Xml:
        this.SaveXml(fileName);
        break;
      case FormatType.Html:
        new HTMLExport().SaveAsXhtml(this, fileName);
        break;
      case FormatType.Odt:
        this.SaveODT(fileName);
        break;
    }
  }

  public void SaveAsEpub(string fileName, WPicture coverImage)
  {
    if (this.UpdateFields)
      this.UpdateDocumentFields();
    this.SaveEPub(fileName, coverImage);
  }

  private void SaveDoc(string fileName)
  {
    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
    {
      using (WordWriter writer = new WordWriter((Stream) fileStream))
        new DocWriterAdapter().Write(writer, this);
    }
  }

  public void Save(
    string fileName,
    FormatType formatType,
    HttpResponse response,
    HttpContentDisposition contentDisposition)
  {
    if (fileName == null)
      throw new ArgumentNullException("FileName");
    if (response == null)
      throw new ArgumentNullException("HttpResponse");
    this.SaveInternal(fileName, formatType, response, contentDisposition);
  }

  private void SaveInternal(
    string fileName,
    FormatType formatType,
    HttpResponse response,
    HttpContentDisposition contentDisposition)
  {
    if (this.UpdateFields)
      this.UpdateDocumentFields();
    fileName = Path.GetFileName(fileName);
    if (formatType == FormatType.Automatic)
      formatType = this.GetFormatType(fileName, false);
    this.SaveFormatType = formatType;
    response.Clear();
    string str1 = string.Empty;
    switch (formatType)
    {
      case FormatType.Doc:
      case FormatType.Dot:
        str1 = "application/msword";
        break;
      case FormatType.Docx:
      case FormatType.StrictDocx:
      case FormatType.Word2007:
      case FormatType.Word2007Dotx:
      case FormatType.Word2007Docm:
      case FormatType.Word2007Dotm:
        str1 = "application/vnd.ms-word.document.12";
        break;
      case FormatType.Word2010:
      case FormatType.Word2010Dotx:
      case FormatType.Word2010Docm:
      case FormatType.Word2010Dotm:
        str1 = "application/vnd.ms-word.document.14";
        break;
      case FormatType.Word2013:
      case FormatType.Word2013Dotx:
      case FormatType.Dotx:
      case FormatType.Word2013Docm:
      case FormatType.Docm:
      case FormatType.Word2013Dotm:
      case FormatType.Dotm:
        str1 = "application/vnd.ms-word.document.15";
        break;
      case FormatType.EPub:
        str1 = "application/epub+zip";
        break;
      case FormatType.Xml:
        str1 = "application/xml";
        break;
    }
    string str2 = contentDisposition == HttpContentDisposition.InBrowser ? "inline" : "attachment";
    response.AddHeader("Content-Type", str1);
    response.AddHeader("Content-Disposition", $"{str2};filename={$"\"{fileName}\""};");
    this.Save(response.OutputStream, formatType);
    response.End();
  }

  public void SaveAsEpub(
    string fileName,
    WPicture coverImage,
    HttpResponse response,
    HttpContentDisposition contentDisposition)
  {
    if (this.UpdateFields)
      this.UpdateDocumentFields();
    fileName = Path.GetFileName(fileName);
    this.SaveFormatType = FormatType.EPub;
    response.Clear();
    string str1 = "application/epub+zip";
    string str2 = contentDisposition == HttpContentDisposition.InBrowser ? "inline" : "attachment";
    response.AddHeader("Content-Type", str1);
    response.AddHeader("Content-Disposition", $"{str2};filename={$"\"{fileName}\""};");
    this.SaveEPub(response.OutputStream, coverImage);
    response.End();
  }

  private void SaveDot(string fileName)
  {
    DocWriterAdapter docWriterAdapter = new DocWriterAdapter();
    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
    {
      using (WordWriter writer = new WordWriter((Stream) fileStream))
      {
        writer.IsTemplate = true;
        docWriterAdapter.Write(writer, this);
      }
    }
  }

  private void SaveRtf(string fileName) => new RtfWriter().Write(fileName, (IWordDocument) this);

  public void Open(
    Stream stream,
    FormatType formatType,
    XHTMLValidationType validationType,
    string baseUrl)
  {
    if (stream == null)
      throw new ArgumentNullException("Stream");
    this.HtmlBaseUrl = baseUrl != null ? baseUrl : throw new ArgumentNullException("BaseUrl");
    this.OpenInternal(stream, formatType, validationType);
  }

  public void Open(Stream stream, FormatType formatType, XHTMLValidationType validationType)
  {
    if (stream == null)
      throw new ArgumentNullException("Stream");
    this.OpenInternal(stream, formatType, validationType);
  }

  private void OpenInternal(
    Stream stream,
    FormatType formatType,
    XHTMLValidationType validationType)
  {
    this.UpdateFormatType(stream, ref formatType);
    this.ActualFormatType = formatType;
    if (FormatType.Html == formatType)
    {
      this.Init();
      this.OpenHTML(stream, validationType);
      this.RemoveTrailVersionWatermark(formatType);
    }
    else
      this.Open(stream, formatType);
  }

  public void Open(Stream stream, FormatType formatType)
  {
    if (stream == null)
      throw new ArgumentNullException("Stream");
    this.OpenInternal(stream, formatType, (string) null);
  }

  public void Open(Stream stream, FormatType formatType, string password)
  {
    if (stream == null)
      throw new ArgumentNullException("Stream");
    if (password == null)
      throw new ArgumentNullException("Password");
    this.OpenInternal(stream, formatType, password);
  }

  private void OpenInternal(Stream stream, FormatType formatType, string password)
  {
    this.Init();
    this.Password = password;
    this.UpdateFormatType(stream, ref formatType);
    this.ActualFormatType = formatType;
    switch (formatType)
    {
      case FormatType.Doc:
      case FormatType.Dot:
        DocReaderAdapter docReaderAdapter = new DocReaderAdapter();
        this.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark] = true;
        using (WordReader reader = new WordReader(stream))
          docReaderAdapter.Read(reader, this);
        break;
      case FormatType.Docx:
      case FormatType.Word2007:
      case FormatType.Word2010:
      case FormatType.Word2013:
      case FormatType.Word2007Dotx:
      case FormatType.Word2010Dotx:
      case FormatType.Word2013Dotx:
      case FormatType.Dotx:
      case FormatType.Word2007Docm:
      case FormatType.Word2010Docm:
      case FormatType.Word2013Docm:
      case FormatType.Docm:
      case FormatType.Word2007Dotm:
      case FormatType.Word2010Dotm:
      case FormatType.Word2013Dotm:
      case FormatType.Dotm:
        this.OpenDocx(stream);
        break;
      case FormatType.WordML:
        this.OpenWordML(stream);
        break;
      case FormatType.Rtf:
        this.OpenRtf(stream);
        break;
      case FormatType.Txt:
        this.OpenTxt(stream);
        break;
      case FormatType.Xml:
        this.OpenXml(stream);
        break;
      case FormatType.Html:
        this.Open(stream, formatType, XHTMLValidationType.Transitional);
        break;
      default:
        throw new NotSupportedException("DocIO does not support this file format");
    }
    if (formatType == FormatType.Html)
      return;
    this.RemoveTrailVersionWatermark(formatType);
  }

  internal void OpenRtf(Stream stream)
  {
    this.IsOpening = true;
    this.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark] = true;
    new RtfParser(this, stream).ParseToken();
    this.IsOpening = false;
  }

  internal void OpenRtf(string rtfText)
  {
    this.IsOpening = true;
    MemoryStream memoryStream = new MemoryStream();
    StreamWriter streamWriter = new StreamWriter((Stream) memoryStream, WordDocument.GetEncoding("ASCII"));
    streamWriter.Write(rtfText);
    rtfText = "";
    streamWriter.Flush();
    memoryStream.Position = 0L;
    new RtfParser(this, (Stream) memoryStream).ParseToken();
    streamWriter.Dispose();
    this.IsOpening = false;
  }

  private void UpdateFormatType(Stream stream, ref FormatType formatType)
  {
    if (formatType == FormatType.Automatic)
      formatType = !(stream is FileStream) ? FormatType.Doc : this.GetFormatType((stream as FileStream).Name, false);
    stream.Position = 0L;
    if (this.CheckForEncryption(stream))
    {
      using (ICompoundFile compoundFile = this.CreateCompoundFile(stream))
      {
        SecurityHelper.EncrytionType encryptionType = new SecurityHelper().GetEncryptionType(compoundFile.RootStorage);
        stream.Position = 0L;
        if (encryptionType != SecurityHelper.EncrytionType.None)
        {
          formatType = FormatType.Docx;
          return;
        }
      }
    }
    stream.Position = 0L;
    byte[] buffer = new byte[5];
    if (stream.Read(buffer, 0, 5) == 5 && buffer[0] == (byte) 80 /*0x50*/ && buffer[1] == (byte) 75)
    {
      stream.Position = 0L;
      formatType = FormatType.Docx;
    }
    else if (buffer[0] == (byte) 123 && buffer[1] == (byte) 92 && buffer[2] == (byte) 114 && buffer[3] == (byte) 116 && buffer[4] == (byte) 102)
    {
      stream.Position = 0L;
      formatType = FormatType.Rtf;
    }
    else
    {
      stream.Position = 0L;
      if (Syncfusion.CompoundFile.DocIO.Net.CompoundFile.CheckHeader(stream))
      {
        stream.Position = 0L;
        formatType = FormatType.Doc;
      }
      else
      {
        try
        {
          XmlReader reader = UtilityMethods.CreateReader(stream);
          reader.MoveToElement();
          if (reader.LocalName == "wordDocument" || reader.LocalName == "package")
            formatType = FormatType.WordML;
          else if (reader.LocalName == "html")
            formatType = FormatType.Html;
        }
        catch
        {
        }
      }
    }
    stream.Position = 0L;
  }

  public void Save(Stream stream, FormatType formatType)
  {
    if (stream == null)
      throw new ArgumentNullException("Stream");
    this.SaveInternal(stream, formatType);
  }

  internal string GetAsRoman(int number)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.GenerateNumber(ref number, 1000, "M"));
    stringBuilder.Append(this.GenerateNumber(ref number, 900, "CM"));
    stringBuilder.Append(this.GenerateNumber(ref number, 500, "D"));
    stringBuilder.Append(this.GenerateNumber(ref number, 400, "CD"));
    stringBuilder.Append(this.GenerateNumber(ref number, 100, "C"));
    stringBuilder.Append(this.GenerateNumber(ref number, 90, "XC"));
    stringBuilder.Append(this.GenerateNumber(ref number, 50, "L"));
    stringBuilder.Append(this.GenerateNumber(ref number, 40, "XL"));
    stringBuilder.Append(this.GenerateNumber(ref number, 10, "X"));
    stringBuilder.Append(this.GenerateNumber(ref number, 9, "IX"));
    stringBuilder.Append(this.GenerateNumber(ref number, 5, "V"));
    stringBuilder.Append(this.GenerateNumber(ref number, 4, "IV"));
    stringBuilder.Append(this.GenerateNumber(ref number, 1, "I"));
    return stringBuilder.ToString();
  }

  private string GenerateNumber(ref int value, int magnitude, string letter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (value >= magnitude)
    {
      value -= magnitude;
      stringBuilder.Append(letter);
    }
    return stringBuilder.ToString();
  }

  private string GetChineseWithinTenThousand(
    int number,
    bool isAboveFiveDigit,
    ListPatternType patternType)
  {
    if (number == 0)
      return "○";
    string input1 = "";
    string[] strArray1 = new string[10]
    {
      "○",
      "一",
      "二",
      "三",
      "四",
      "五",
      "六",
      "七",
      "八",
      "九"
    };
    string[] strArray2 = new string[4]{ "", "十", "百", "千" };
    for (int index1 = 0; number > 0 && index1 < strArray2.Length; number /= 10)
    {
      int index2 = number % 10;
      if (patternType == ListPatternType.ChineseCountingThousand)
      {
        input1 = index2 == 0 ? strArray1[index2] + input1 : strArray1[index2] + strArray2[index1] + input1;
      }
      else
      {
        string str1;
        switch (index2)
        {
          case 0:
            goto label_10;
          case 1:
            if (index1 > 0)
            {
              str1 = "";
              break;
            }
            goto default;
          default:
            str1 = strArray1[index2];
            break;
        }
        string str2 = strArray2[index1];
        string str3 = input1;
        input1 = str1 + str2 + str3;
      }
label_10:
      ++index1;
    }
    string input2 = Regex.Replace(Regex.Replace(input1, "○*○", "○"), "○$", "");
    if (!isAboveFiveDigit && patternType == ListPatternType.ChineseCountingThousand)
      input2 = Regex.Replace(input2, "^一十", "十");
    return input2;
  }

  internal string GetChineseExpression(int number, ListPatternType patternType)
  {
    if (number == 0)
      return "○";
    if (number >= 1000000)
      return string.Empty;
    string chineseExpression = "";
    string[] strArray = new string[3]{ "", "万", "亿" };
    int index = 0;
    bool isAboveFiveDigit = number > 100000;
    for (; number > 0; number /= 10000)
    {
      int number1 = number <= 10000 || patternType != ListPatternType.ChineseCountingThousand ? number % 10000 : number % 100000;
      if (number1 != 0)
      {
        chineseExpression = this.GetChineseWithinTenThousand(number1, isAboveFiveDigit, patternType) + strArray[index] + chineseExpression;
        if (patternType == ListPatternType.ChineseCountingThousand)
        {
          if (number1 < 1000 && number1 != number || isAboveFiveDigit && number1 < 10000 && number1 != number)
            chineseExpression = "○" + chineseExpression;
        }
        else if (number1 / 1000 == 1 && number1 != number)
          chineseExpression = "一" + chineseExpression;
      }
      ++index;
    }
    return chineseExpression;
  }

  internal string GetAsLetter(int number)
  {
    if (number <= 0)
      return "";
    int num1 = number / 26;
    int num2 = number % 26;
    if (num2 == 0)
    {
      num2 = 26;
      --num1;
    }
    char ch = (char) (64 /*0x40*/ + num2);
    string asLetter = ch.ToString();
    for (; num1 > 0; --num1)
      asLetter += ch.ToString();
    return asLetter;
  }

  internal string GetSpanishCardinalTextString(bool cardinalString, string text)
  {
    if (cardinalString)
    {
      text = text.Trim();
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsLetter(text[index]))
        {
          cardinalString = false;
          break;
        }
      }
      if (cardinalString)
        text = this.NumberToSpanishWords(int.Parse(text), true);
    }
    return text;
  }

  internal string GetCardTextString(bool cardinalString, string text)
  {
    if (cardinalString)
    {
      text = text.Trim();
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsLetter(text[index]))
        {
          cardinalString = false;
          break;
        }
      }
      if (cardinalString)
        text = this.NumberToWords(int.Parse(text), true);
    }
    return text;
  }

  internal string GetOrdTextString(bool ordinalString, string text)
  {
    if (ordinalString)
    {
      text = text.Trim();
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsLetter(text[index]))
        {
          ordinalString = false;
          break;
        }
      }
      if (ordinalString)
        text = this.NumberToWords(int.Parse(text), false);
    }
    return text;
  }

  internal string GetSpanishOrdinalTextString(bool ordinalString, string text)
  {
    if (ordinalString)
    {
      text = text.Trim();
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsLetter(text[index]))
        {
          ordinalString = false;
          break;
        }
      }
      if (ordinalString)
        text = this.NumberToSpanishWords(int.Parse(text), false);
    }
    return text;
  }

  internal string NumberToSpanishWords(int number, bool isCardText)
  {
    if (number == 0 && isCardText)
      return "cero";
    StringBuilder stringBuilder = new StringBuilder();
    if (number / 1000 > 0 && number <= 10000)
    {
      string[] strArray1 = new string[11]
      {
        "",
        "mil",
        "dos mil",
        "tres mil",
        "cuatro mil",
        "cinco mil",
        "seis mil",
        "siete mil",
        "ocho mil",
        "nueve mil",
        "diez mil"
      };
      string[] strArray2 = new string[11]
      {
        "",
        "milésimo",
        "dosmilésimo",
        "tresmilésimo",
        "cuatromilésimo",
        "cincomilésimo",
        "seismilésimo",
        "sietemilésimo",
        "ochomilésimo",
        "nuevemilésimo",
        "diezmilésimo"
      };
      if (isCardText)
        stringBuilder.Append(strArray1[number / 1000]);
      else
        stringBuilder.Append(strArray2[number / 1000]);
      number %= 1000;
    }
    if (number / 100 > 0)
    {
      if (!string.IsNullOrEmpty(stringBuilder.ToString()))
        stringBuilder.Append(" ");
      string[] strArray3 = new string[10]
      {
        "",
        "ciento",
        "doscientos",
        "trescientos",
        "cuatrocientos",
        "quinientos",
        "seiscientos",
        "setecientos",
        "ochocientos",
        "novecientos"
      };
      string[] strArray4 = new string[10]
      {
        "",
        "centésimo",
        "ducentésimo",
        "tricentésimo",
        "cuadringentésimo",
        "quingentésimo",
        "sexcentésimo",
        "septingentésimo",
        "octingentésimo",
        "noningentésimo"
      };
      if (isCardText)
        stringBuilder.Append(strArray3[number / 100]);
      else
        stringBuilder.Append(strArray4[number / 100]);
      number %= 100;
    }
    if (number > 0 && number < 100)
    {
      if (!string.IsNullOrEmpty(stringBuilder.ToString()))
        stringBuilder.Append(" ");
      string[] strArray5;
      if (isCardText)
        strArray5 = new string[20]
        {
          "",
          "uno",
          "dos",
          "tres",
          "cuatro",
          "cinco",
          "seis",
          "siete",
          "ocho",
          "nueve",
          "diez",
          "once",
          "doce",
          "trece",
          "catorce",
          "quince",
          "dieciséis",
          "diecisiete",
          "dieciocho",
          "diecinueve"
        };
      else
        strArray5 = new string[20]
        {
          "",
          "primero",
          "segundo",
          "tercero",
          "cuarto",
          "quinto",
          "sexto",
          "séptimo",
          "octavo",
          "noveno",
          "décimo",
          "undécimo",
          "duodécimo",
          "decimotercero",
          "decimocuarto",
          "decimoquinto",
          "decimosexto",
          "decimoséptimo",
          "decimoctavo",
          "decimonoveno"
        };
      string[] strArray6 = new string[10]
      {
        "",
        "diez",
        "veinte",
        "treinta",
        "cuarenta",
        "cincuenta",
        "sesenta",
        "setenta",
        "ochenta",
        "noventa"
      };
      string[] strArray7 = new string[10]
      {
        "",
        "décimo",
        "vigésimo",
        "trigésimo",
        "cuadragésimo",
        "quincuagésimo",
        "sexagésimo",
        "septuagésimo",
        "octogésimo",
        "nonagésimo"
      };
      string[] strArray8 = new string[10]
      {
        "",
        "veintiuno",
        "veintidós",
        "veintitrés",
        "veinticuatro",
        "veinticinco",
        "veintiséis",
        "veintisiete",
        "veintiocho",
        "veintinueve"
      };
      if (number < 20)
        stringBuilder.Append(strArray5[number]);
      else if (number > 20 && number < 30 && isCardText)
      {
        stringBuilder.Append(strArray8[number % 10]);
      }
      else
      {
        if (isCardText && number % 10 > 0)
          stringBuilder.Append(strArray6[number / 10]);
        else if (isCardText && number % 10 == 0)
          stringBuilder.Append(strArray6[number / 10]);
        if (number % 10 > 0 && !isCardText)
          stringBuilder.Append(strArray7[number / 10]);
        if (number % 10 == 0 && !isCardText)
          stringBuilder.Append(strArray7[number / 10]);
        else if (number % 10 > 0)
        {
          if (isCardText)
            stringBuilder.Append(" y " + strArray5[number % 10]);
          else
            stringBuilder.Append(" " + strArray5[number % 10]);
        }
      }
    }
    return stringBuilder.ToString();
  }

  internal string NumberToWords(int number, bool isCardText)
  {
    if (number == 0)
      return "zero";
    StringBuilder stringBuilder = new StringBuilder();
    if (number / 1000000 > 0)
    {
      stringBuilder.Append(this.NumberToWords(number / 1000000, isCardText) + " million ");
      if (!isCardText && number % 10 == 0)
        stringBuilder.Append("th");
      number %= 1000000;
    }
    if (number / 1000 > 0)
    {
      stringBuilder.Append(this.NumberToWords(number / 1000, isCardText) + " thousand ");
      if (!isCardText && number % 10 == 0)
        stringBuilder.Append("th");
      number %= 1000;
    }
    if (number / 100 > 0)
    {
      stringBuilder.Append(this.NumberToWords(number / 100, isCardText) + " hundred ");
      if (!isCardText && number % 10 == 0)
        stringBuilder.Append("th");
      number %= 100;
    }
    if (number > 0)
    {
      if (!string.IsNullOrEmpty(stringBuilder.ToString()) && isCardText)
        stringBuilder.Append("and ");
      string[] strArray1;
      if (isCardText)
        strArray1 = new string[20]
        {
          "",
          "one",
          "two",
          "three",
          "four",
          "five",
          "six",
          "seven",
          "eight",
          "nine",
          "ten",
          "eleven",
          "twelve",
          "thirteen",
          "fourteen",
          "fifteen",
          "sixteen",
          "seventeen",
          "eighteen",
          "nineteen"
        };
      else
        strArray1 = new string[20]
        {
          "",
          "first",
          "second",
          "third",
          "fourth",
          "fifth",
          "sixth",
          "seventh",
          "eighth",
          "ninth",
          "tenth",
          "eleventh",
          "twelfth",
          "thirteenth",
          "fourteenth",
          "fifteenth",
          "sixteenth",
          "seventeenth",
          "eighteenth",
          "nineteenth"
        };
      string[] strArray2 = new string[10]
      {
        "",
        "ten",
        "twenty",
        "thirty",
        "forty",
        "fifty",
        "sixty",
        "seventy",
        "eighty",
        "ninety"
      };
      string[] strArray3 = new string[10]
      {
        "",
        "tenth",
        "twentieth",
        "thirtieth",
        "fortieth",
        "fiftieth",
        "sixtieth",
        "seventieth",
        "eightieth",
        "ninetieth"
      };
      if (number < 20)
      {
        stringBuilder.Append(strArray1[number]);
      }
      else
      {
        if (isCardText || number % 10 > 0)
          stringBuilder.Append(strArray2[number / 10]);
        if (number % 10 == 0 && !isCardText)
          stringBuilder.Append(strArray3[number / 10]);
        else if (number % 10 > 0)
          stringBuilder.Append("-" + strArray1[number % 10]);
      }
    }
    return stringBuilder.ToString();
  }

  private bool IsTrailParagraph(WParagraph paragraph, bool isText)
  {
    if (paragraph == null || paragraph.Items.Count != 1)
      return false;
    if (isText)
      return paragraph.Text == "Created with a trial version of Syncfusion Essential DocIO.";
    return paragraph.Items[0] is WTextBox && (paragraph.Items[0] as WTextBox).Name == "SyncfusionLicense";
  }

  internal void RemoveTrailVersionWatermark(FormatType formatType)
  {
    if (formatType == FormatType.Txt || formatType == FormatType.Html)
    {
      if (this.Sections.Count <= 0 || this.Sections[0].Body.Items.Count <= 0 || !this.IsTrailParagraph(this.Sections[0].Body.Items[0] as WParagraph, true))
        return;
      this.Sections[0].Body.Items.RemoveAt(0);
    }
    else
    {
      foreach (WSection section in (CollectionImpl) this.Sections)
      {
        if (section.HeadersFooters.OddHeader.ChildEntities.Count > 0 && this.IsTrailParagraph(section.HeadersFooters.OddHeader.ChildEntities[0] as WParagraph, false))
          section.HeadersFooters.OddHeader.ChildEntities.RemoveAt(0);
        if (this.m_saveFormatType != FormatType.Xml)
        {
          if (section.HeadersFooters.EvenHeader.ChildEntities.Count > 0 && this.IsTrailParagraph(section.HeadersFooters.EvenHeader.ChildEntities[0] as WParagraph, false))
            section.HeadersFooters.EvenHeader.ChildEntities.RemoveAt(0);
          if (section.HeadersFooters.FirstPageHeader.ChildEntities.Count > 0 && this.IsTrailParagraph(section.HeadersFooters.FirstPageHeader.ChildEntities[0] as WParagraph, false))
            section.HeadersFooters.FirstPageHeader.ChildEntities.RemoveAt(0);
        }
      }
    }
  }

  internal void AddTrailVersionWatermarkForODT()
  {
    if (!this.WriteWarning)
      return;
    try
    {
      WParagraph wparagraph = new WParagraph((IWordDocument) this);
      IWTextRange wtextRange = wparagraph.AppendText("Created with a trial version of Syncfusion Essential DocIO.");
      wtextRange.CharacterFormat.TextColor = Color.Black;
      wtextRange.CharacterFormat.Bold = true;
      wtextRange.CharacterFormat.FontName = "Calibri";
      wtextRange.CharacterFormat.FontSize = 11f;
      wparagraph.ParaStyle.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
      foreach (WSection section in (CollectionImpl) this.Sections)
      {
        section.PageSetup.HeaderDistance = 0.0f;
        section.HeadersFooters.OddHeader.ChildEntities.Insert(0, (IEntity) wparagraph.Clone());
        section.HeadersFooters.EvenHeader.ChildEntities.Insert(0, (IEntity) wparagraph.Clone());
        section.HeadersFooters.FirstPageHeader.ChildEntities.Insert(0, (IEntity) wparagraph);
      }
      this.m_isWarnInserted = true;
    }
    catch
    {
    }
  }

  internal void AddTrailVersionWatermark()
  {
    if (!this.WriteWarning)
      return;
    try
    {
      WParagraph wparagraph = new WParagraph((IWordDocument) this);
      IWTextBox wtextBox = wparagraph.AppendTextBox(281f, 25f);
      wtextBox.Name = "SyncfusionLicense";
      wtextBox.TextBoxFormat.VerticalPosition = 0.0f;
      wtextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.Page;
      wtextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Margin;
      wtextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Center;
      wtextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Behind;
      wtextBox.TextBoxFormat.NoLine = true;
      IWTextRange wtextRange = wtextBox.TextBoxBody.AddParagraph().AppendText("Created with a trial version of Syncfusion Essential DocIO.");
      wtextRange.CharacterFormat.FontName = "Calibri";
      wtextRange.CharacterFormat.FontSize = 11f;
      wtextRange.CharacterFormat.TextColor = Color.Black;
      wtextRange.CharacterFormat.Bold = true;
      foreach (WSection section in (CollectionImpl) this.Sections)
      {
        section.HeadersFooters.OddHeader.ChildEntities.Insert(0, (IEntity) wparagraph.Clone());
        if (this.m_saveFormatType != FormatType.Xml)
        {
          section.HeadersFooters.EvenHeader.ChildEntities.Insert(0, (IEntity) wparagraph.Clone());
          section.HeadersFooters.FirstPageHeader.ChildEntities.Insert(0, (IEntity) wparagraph.Clone());
        }
      }
      this.m_isWarnInserted = true;
    }
    catch
    {
    }
  }

  private void SaveInternal(Stream stream, FormatType formatType)
  {
    if (stream.CanSeek)
      stream.SetLength(0L);
    if (this.UpdateFields)
      this.UpdateDocumentFields();
    this.SaveFormatType = formatType;
    switch (formatType)
    {
      case FormatType.Txt:
      case FormatType.EPub:
      case FormatType.Html:
        switch (formatType)
        {
          case FormatType.Doc:
          case FormatType.Dot:
            DocWriterAdapter docWriterAdapter = new DocWriterAdapter();
            using (WordWriter writer = new WordWriter(stream))
            {
              if (formatType == FormatType.Dot)
                writer.IsTemplate = true;
              docWriterAdapter.Write(writer, this);
            }
            return;
          case FormatType.Docx:
          case FormatType.StrictDocx:
          case FormatType.Word2007:
          case FormatType.Word2010:
          case FormatType.Word2013:
          case FormatType.Word2007Dotx:
          case FormatType.Word2010Dotx:
          case FormatType.Word2013Dotx:
          case FormatType.Dotx:
          case FormatType.Word2007Docm:
          case FormatType.Word2010Docm:
          case FormatType.Word2013Docm:
          case FormatType.Docm:
          case FormatType.Word2007Dotm:
          case FormatType.Word2010Dotm:
          case FormatType.Word2013Dotm:
          case FormatType.Dotm:
            this.SaveDocx(stream);
            return;
          case FormatType.WordML:
            this.SaveWordML(stream);
            return;
          case FormatType.Rtf:
            this.SaveRtf(stream);
            return;
          case FormatType.Txt:
            this.SaveTxt(stream);
            return;
          case FormatType.EPub:
            this.SaveEPub(stream, (WPicture) null);
            return;
          case FormatType.Xml:
            this.SaveXml(stream);
            return;
          case FormatType.Automatic:
            throw new Exception("Please provide appropriate format type other than Automatic.");
          case FormatType.Html:
            new HTMLExport().SaveAsXhtml(this, stream);
            return;
          case FormatType.Odt:
            this.SaveODT(stream);
            return;
          default:
            return;
        }
      case FormatType.Odt:
        this.AddTrailVersionWatermarkForODT();
        goto case FormatType.Txt;
      default:
        this.AddTrailVersionWatermark();
        goto case FormatType.Txt;
    }
  }

  internal string GetOrdinal(int num, WCharacterFormat characterFormat)
  {
    switch (characterFormat.LocaleIdASCII)
    {
      case 1027:
        return this.GetOrdinalInCatalan(num);
      case 1029:
      case 1031:
      case 1035:
      case 1038:
      case 1044:
      case 1045:
      case 1050:
      case 1055:
      case 1060:
      case 1061:
      case 1069:
      case 2055:
      case 2068:
      case 2074:
      case 3079:
      case 4103:
      case 4122:
      case 5127:
      case 5146:
      case 6170:
      case 8218:
        return num.ToString() + ".";
      case 1030:
        return this.GetOrdinalInDanish(num);
      case 1032:
        return num.ToString() + "o";
      case 1034:
      case 1046:
      case 2058:
      case 2070:
      case 3082:
      case 4106:
      case 5130:
      case 6154:
      case 7178:
      case 8202:
      case 9226:
      case 10250:
      case 11274:
      case 12298:
      case 13322:
      case 14346:
      case 15370:
      case 16394:
      case 17418:
      case 18442:
      case 19466:
      case 20490:
      case 21514:
        return num.ToString() + (object) 'º';
      case 1036:
      case 2060:
      case 3084:
      case 4108:
      case 5132:
      case 6156:
      case 8204:
      case 9228:
      case 10252:
      case 11276:
      case 12300:
      case 13324:
      case 14348:
      case 15372:
        return num == 1 ? num.ToString() + "er" : num.ToString() + "e";
      case 1040:
      case 2064:
        return num.ToString() + (object) '°';
      case 1043:
      case 2067:
        return num.ToString() + "e";
      case 1049:
      case 2073:
        return $"{num.ToString()}-{(object) 'й'}";
      case 1053:
      case 2077:
        return this.GetOrdinalInSwedish(num);
      default:
        return this.GetOrdinalInEnglish(num);
    }
  }

  private string GetOrdinalInSwedish(int num)
  {
    return num == 11 || num == 12 || num % 10 != 1 && num % 10 != 2 ? num.ToString() + ":e" : num.ToString() + ":a";
  }

  private string GetOrdinalInCatalan(int num)
  {
    switch (num)
    {
      case 1:
        return num.ToString() + ".";
      case 2:
        return num.ToString() + "n";
      case 3:
        return num.ToString() + "r";
      case 4:
        return num.ToString() + "t";
      case 14:
        return $"{num.ToString()}{(object) 'è'}h";
      default:
        return num.ToString() + (object) 'è';
    }
  }

  private string GetOrdinalInDanish(int num)
  {
    if (num == 0)
      return num.ToString() + "te";
    switch (num % 100)
    {
      case 0:
        return num.ToString() + "ende";
      case 1:
        return num.ToString() + "ste";
      case 2:
        return num.ToString() + "nden";
      case 3:
        return num.ToString() + "dje";
      case 4:
        return num.ToString() + "rde";
      case 5:
      case 6:
      case 11:
      case 12:
      case 30:
        return num.ToString() + "te";
      default:
        return num.ToString() + "nde";
    }
  }

  private string GetOrdinalInEnglish(int num)
  {
    switch (num % 100)
    {
      case 11:
      case 12:
      case 13:
        return num.ToString() + "th";
      default:
        switch (num % 10)
        {
          case 1:
            return num.ToString() + "st";
          case 2:
            return num.ToString() + "nd";
          case 3:
            return num.ToString() + "rd";
          default:
            return num.ToString() + "th";
        }
    }
  }

  public new void Close()
  {
    this.IsClosing = true;
    this.CloseContent();
    GC.WaitForPendingFinalizers();
    this.ResetSingleLineReplace();
    this.m_doc = this;
    this.Init();
    this.IsClosing = false;
  }

  public void Dispose() => this.Close();

  internal static bool CompareArray(byte[] buffer1, byte[] buffer2)
  {
    bool flag = true;
    for (int index = 0; index < buffer1.Length; ++index)
    {
      if ((int) buffer1[index] != (int) buffer2[index])
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void CloseContent()
  {
    this.CloseSecContent();
    this.CloseStyles();
    if (this.m_builtinProp != null)
    {
      this.m_builtinProp.Close();
      this.m_builtinProp = (BuiltinDocumentProperties) null;
    }
    if (this.FFNStringTable != null)
    {
      this.FFNStringTable.Close();
      this.FFNStringTable = (FontFamilyNameStringTable) null;
    }
    if (this.m_customProp != null)
    {
      this.m_customProp.Close();
      this.m_customProp = (CustomDocumentProperties) null;
    }
    if (this.m_imageCollection != null)
    {
      this.m_imageCollection.Clear();
      this.m_imageCollection = (ImageCollection) null;
    }
    if (this.m_oleObjectCollection != null)
    {
      this.m_oleObjectCollection.Clear();
      this.m_oleObjectCollection = (Dictionary<string, Storage>) null;
    }
    if (this.m_escher != null)
    {
      this.m_escher.Close();
      this.m_escher = (EscherClass) null;
    }
    if (this.m_bookmarks != null)
    {
      this.m_bookmarks.Close();
      this.m_bookmarks = (BookmarkCollection) null;
    }
    if (this.m_editableRanges != null)
    {
      this.m_editableRanges.Close();
      this.m_editableRanges = (EditableRangeCollection) null;
    }
    if (this.m_revisions != null)
    {
      this.m_revisions.Close();
      this.m_revisions = (RevisionCollection) null;
    }
    if (this.m_fields != null)
    {
      if (this.m_fields.m_sortedAutoNumFields != null)
      {
        this.m_fields.m_sortedAutoNumFields.Clear();
        this.m_fields.m_sortedAutoNumFields = (List<WField>) null;
      }
      if (this.m_fields.m_sortedAutoNumFieldIndexes != null)
      {
        this.m_fields.m_sortedAutoNumFieldIndexes.Clear();
        this.m_fields.m_sortedAutoNumFieldIndexes = (List<string>) null;
      }
      this.m_fields.Close();
      this.m_fields = (FieldCollection) null;
    }
    if (this.m_txbxItems != null)
    {
      this.m_txbxItems.Close();
      this.m_txbxItems = (TextBoxCollection) null;
    }
    if (this.m_footnotes != null)
    {
      this.m_footnotes.Close();
      this.m_footnotes = (Footnote) null;
    }
    if (this.m_endnotes != null)
    {
      this.m_endnotes.Close();
      this.m_endnotes = (Endnote) null;
    }
    if (this.m_Comments != null)
    {
      this.m_Comments.Close();
      this.m_Comments = (CommentsCollection) null;
    }
    if (this.m_CommentsEx != null)
    {
      this.m_CommentsEx.Close();
      this.m_CommentsEx = (CommentsExCollection) null;
    }
    if (this.m_mailMerge != null)
    {
      this.m_mailMerge.Close();
      this.m_mailMerge = (MailMerge) null;
    }
    if (this.m_viewSetup != null)
    {
      this.m_viewSetup.Close();
      this.m_viewSetup = (ViewSetup) null;
    }
    if (this.m_watermark != null)
    {
      this.m_watermark.Close();
      this.m_watermark = (Watermark) null;
    }
    if (this.m_background != null)
    {
      this.m_background.Close();
      this.m_background = (Background) null;
    }
    this.m_dop = (DOPDescriptor) null;
    if (this.m_grammarSpellingData != null)
    {
      this.m_grammarSpellingData.Close();
      this.m_grammarSpellingData = (GrammarSpelling) null;
    }
    this.m_password = (string) null;
    this.m_macrosData = (byte[]) null;
    this.m_escherDataContainers = (byte[]) null;
    this.m_escherContainers = (byte[]) null;
    this.m_macroCommands = (byte[]) null;
    this.m_defShapeId = 1;
    this.m_standardAsciiFont = (string) null;
    this.m_standardFarEastFont = (string) null;
    this.m_standardNonFarEastFont = (string) null;
    this.ThrowExceptionsForUnsupportedElements = false;
    if (this.m_latentStyles2010 != null)
    {
      this.m_latentStyles2010.Close();
      this.m_latentStyles2010 = (MemoryStream) null;
    }
    if (this.m_defCharFormat != null)
    {
      this.m_defCharFormat.Close();
      this.m_defCharFormat = (WCharacterFormat) null;
    }
    if (this.m_defParaFormat != null)
    {
      this.m_defParaFormat.Close();
      this.m_defParaFormat = (WParagraphFormat) null;
    }
    if (this.m_docxPackage != null)
    {
      this.m_docxPackage.Close();
      this.m_docxPackage = (Package) null;
    }
    if (this.m_variables != null)
    {
      this.m_variables.Close();
      this.m_variables = (DocVariables) null;
    }
    this.m_fontSubstitutionTable = (Dictionary<string, string>) null;
    this.m_htmlValidationOption = XHTMLValidationType.Transitional;
    if (this.m_props != null)
    {
      this.m_props.Close();
      this.m_props = (DocProperties) null;
    }
    if (this.m_saveOptions != null)
    {
      this.m_saveOptions.Close();
      this.m_saveOptions = (SaveOptions) null;
    }
    if (this.m_docxProps != null)
    {
      foreach (Stream docxProp in this.m_docxProps)
        docxProp.Close();
      this.m_docxProps.Clear();
      this.m_docxProps = (List<Stream>) null;
    }
    this.ImportStyles = true;
    this.m_nextParaItem = (ParagraphItem) null;
    this.m_prevBodyItem = (TextBodyItem) null;
    if (this.m_settings != null)
    {
      this.m_settings.Close();
      this.m_settings = (Settings) null;
    }
    this.DocHasThemes = false;
    if (this.m_themes != null)
    {
      this.m_themes.Close();
      this.m_themes = (Themes) null;
    }
    if (this.m_styleNameIds != null)
    {
      this.m_styleNameIds.Clear();
      this.m_styleNameIds = (Dictionary<string, string>) null;
    }
    if (this.m_fontSubstitutionTable != null)
    {
      this.m_fontSubstitutionTable.Clear();
      this.m_fontSubstitutionTable = (Dictionary<string, string>) null;
    }
    this.m_tableOfContent = (Dictionary<WField, TableOfContent>) null;
    if (this.m_usedFonts != null)
    {
      this.m_usedFonts.Clear();
      this.m_usedFonts = (List<Font>) null;
    }
    if (this.m_vbaProject != null)
    {
      this.m_vbaProject.Close();
      this.m_vbaProject = (Stream) null;
    }
    if (this.m_vbaProjectSignature != null)
    {
      this.m_vbaProjectSignature.Close();
      this.m_vbaProjectSignature = (Stream) null;
    }
    if (this.m_vbaProjectSignatureAgile != null)
    {
      this.m_vbaProjectSignatureAgile.Close();
      this.m_vbaProjectSignatureAgile = (Stream) null;
    }
    if (this.m_CustomUIPartContainer != null)
    {
      this.m_CustomUIPartContainer.Close();
      this.m_CustomUIPartContainer = (PartContainer) null;
    }
    if (this.m_UserCustomizationPartContainer != null)
    {
      this.m_UserCustomizationPartContainer.Close();
      this.m_UserCustomizationPartContainer = (PartContainer) null;
    }
    if (this.m_CustomXMLContainer != null)
    {
      this.m_CustomXMLContainer.Close();
      this.m_CustomXMLContainer = (PartContainer) null;
    }
    if (this.m_vbaData != null)
    {
      this.m_vbaData.Clear();
      this.m_vbaData = (List<MacroData>) null;
    }
    if (this.m_docEvents != null)
    {
      this.m_docEvents.Clear();
      this.m_docEvents = (List<string>) null;
    }
    if (this.m_listStyleNames != null)
    {
      this.m_listStyleNames.Clear();
      this.m_listStyleNames = (Dictionary<string, string>) null;
    }
    if (this.m_lists != null)
    {
      this.m_lists.Clear();
      this.m_lists = (Dictionary<string, Dictionary<int, int>>) null;
    }
    this.m_listNames = (HybridDictionary) null;
    if (this.m_previousListLevel != null)
    {
      this.m_previousListLevel.Clear();
      this.m_previousListLevel = (Dictionary<string, int>) null;
    }
    if (this.m_previousListLevelOverrideStyle != null)
    {
      this.m_previousListLevelOverrideStyle.Clear();
      this.m_previousListLevelOverrideStyle = (List<string>) null;
    }
    if (this.m_AutoShapeCollection != null)
    {
      this.m_AutoShapeCollection.Clear();
      this.m_AutoShapeCollection = (List<Shape>) null;
    }
    AdapterListIDHolder.Instance.Close();
  }

  private void CloseSecContent()
  {
    if (this.m_sections != null && this.m_sections.Count > 0)
    {
      WSection wsection = (WSection) null;
      for (int index = 0; index < this.m_sections.Count; ++index)
      {
        this.m_sections[index].Close();
        wsection = (WSection) null;
      }
    }
    if (this.m_sections == null)
      return;
    this.m_sections.Close();
    this.m_sections = (WSectionCollection) null;
  }

  private void CloseStyles()
  {
    if (this.m_styles != null)
    {
      int count = this.m_styles.Count;
      Style style = (Style) null;
      for (int index = 0; index < count; ++index)
      {
        (this.m_styles[index] as Style).Close();
        style = (Style) null;
      }
      (this.m_styles as StyleCollection).InnerList.Clear();
      this.m_styles = (IStyleCollection) null;
    }
    if (this.m_listStyles != null)
    {
      int count = this.m_listStyles.Count;
      ListStyle listStyle = (ListStyle) null;
      for (int index = 0; index < count; ++index)
      {
        this.m_listStyles[index].Close();
        listStyle = (ListStyle) null;
      }
      this.m_listStyles.Close();
      this.m_listStyles = (ListStyleCollection) null;
    }
    if (this.m_listOverrides == null)
      return;
    int count1 = this.m_listOverrides.Count;
    for (int index = 0; index < count1; ++index)
      this.m_listOverrides[index].Close();
    this.m_listOverrides.Close();
    this.m_listOverrides = (ListOverrideStyleCollection) null;
  }

  public Image[] RenderAsImages(ImageType type)
  {
    this.AddTrailVersionWatermark();
    return new WordToImageConverter().ConvertToImage(this, type);
  }

  public Stream RenderAsImages(int pageIndex, ImageFormat imageFormat)
  {
    this.AddTrailVersionWatermark();
    return new WordToImageConverter().ConvertToImage(pageIndex, this, imageFormat);
  }

  public Image RenderAsImages(int pageIndex, ImageType type)
  {
    this.AddTrailVersionWatermark();
    Image[] image = new WordToImageConverter().ConvertToImage(pageIndex, 1, this, type);
    return image != null && image[pageIndex] != null ? image[pageIndex] : (Image) null;
  }

  public Image[] RenderAsImages(int pageIndex, int noOfPages, ImageType type)
  {
    this.AddTrailVersionWatermark();
    Image[] image = new WordToImageConverter().ConvertToImage(pageIndex, noOfPages, this, type);
    List<Image> imageList = new List<Image>();
    for (int index = 0; index < image.Length; ++index)
    {
      if (image[index] != null)
        imageList.Add(image[index]);
    }
    Image[] imageArray = new Image[imageList.Count];
    for (int index = 0; index < imageList.Count; ++index)
      imageArray[index] = imageList[index];
    return imageArray;
  }

  public TextSelection Find(Regex pattern)
  {
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
      {
        TextSelection textSelection = childEntity.Find(pattern);
        if (textSelection != null)
          return textSelection;
      }
    }
    return (TextSelection) null;
  }

  public TextSelection[] FindSingleLine(Regex pattern)
  {
    TextSelection[] singleLine = (TextSelection[]) null;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      singleLine = TextFinder.Instance.FindSingleLine(section.Body, pattern);
      if (singleLine != null)
        break;
    }
    return singleLine;
  }

  public TextSelection Find(string given, bool caseSensitive, bool wholeWord)
  {
    return this.Find(FindUtils.StringToRegex(given, caseSensitive, wholeWord));
  }

  public TextSelection[] FindSingleLine(string given, bool caseSensitive, bool wholeWord)
  {
    return this.FindSingleLine(FindUtils.StringToRegex(given, caseSensitive, wholeWord));
  }

  public TextSelection[] FindAll(Regex pattern)
  {
    TextSelectionList textSelectionList = (TextSelectionList) null;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
      {
        TextSelectionList all = childEntity.FindAll(pattern);
        if (all != null && all.Count > 0)
        {
          if (textSelectionList == null)
            textSelectionList = all;
          else
            textSelectionList.AddRange((IEnumerable<TextSelection>) all);
        }
      }
    }
    return textSelectionList?.ToArray();
  }

  public TextSelection[] FindAll(string given, bool caseSensitive, bool wholeWord)
  {
    return this.FindAll(FindUtils.StringToRegex(given, caseSensitive, wholeWord));
  }

  public int Replace(Regex pattern, string replace)
  {
    int num = 0;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
      {
        num += childEntity.Replace(pattern, replace);
        if (this.ReplaceFirst && num > 0)
          return num;
      }
    }
    return num;
  }

  public int Replace(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replace);
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord)
  {
    return this.Replace(given, textSelection, caseSensitive, wholeWord, false);
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord,
    bool saveFormatting)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), textSelection, saveFormatting);
  }

  public int Replace(Regex pattern, TextSelection textSelection)
  {
    return this.Replace(pattern, textSelection, false);
  }

  public int Replace(Regex pattern, TextSelection textSelection, bool saveFormatting)
  {
    textSelection.CacheRanges();
    int num = 0;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
      {
        num += childEntity.Replace(pattern, textSelection, saveFormatting);
        if (this.ReplaceFirst && num > 0)
          return num;
      }
    }
    return num;
  }

  public int Replace(string given, TextBodyPart bodyPart, bool caseSensitive, bool wholeWord)
  {
    return this.Replace(given, bodyPart, caseSensitive, wholeWord, false);
  }

  public int Replace(
    string given,
    TextBodyPart bodyPart,
    bool caseSensitive,
    bool wholeWord,
    bool saveFormatting)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), bodyPart, saveFormatting);
  }

  public int Replace(Regex pattern, TextBodyPart bodyPart)
  {
    return this.Replace(pattern, bodyPart, false);
  }

  public int Replace(Regex pattern, TextBodyPart bodyPart, bool saveFormatting)
  {
    int num = 0;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
      {
        num += childEntity.Replace(pattern, bodyPart, saveFormatting);
        if (this.ReplaceFirst && num > 0)
          return num;
      }
    }
    return num;
  }

  public int Replace(string given, IWordDocument replaceDoc, bool caseSensitive, bool wholeWord)
  {
    return this.Replace(given, replaceDoc, caseSensitive, wholeWord, false);
  }

  public int Replace(
    string given,
    IWordDocument replaceDoc,
    bool caseSensitive,
    bool wholeWord,
    bool saveFormatting)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replaceDoc, saveFormatting);
  }

  public int Replace(Regex pattern, IWordDocument replaceDoc, bool saveFormatting)
  {
    int num = 0;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
      {
        num += childEntity.Replace(pattern, replaceDoc, saveFormatting);
        if (this.ReplaceFirst && num > 0)
          return num;
      }
    }
    return num;
  }

  public void UpdateWordCount()
  {
    this.m_paraCount = this.m_wordCount = this.m_charCount = 0;
    foreach (WSection section in (CollectionImpl) this.Sections)
      this.CalculateForTextBody(section.Body.Items);
    this.BuiltinDocumentProperties.ParagraphCount = this.m_paraCount;
    this.BuiltinDocumentProperties.WordCount = this.m_wordCount;
    this.BuiltinDocumentProperties.CharCount = this.m_charCount;
  }

  public void UpdateWordCount(bool performlayout) => this.InternalUpdateWordCount(performlayout);

  internal void InternalUpdateWordCount(bool performlayout)
  {
    if (performlayout)
    {
      DocumentLayouter documentLayouter = new DocumentLayouter();
      documentLayouter.UpdatePageFields(this, false);
      this.BuiltinDocumentProperties.PageCount = this.PageCount = documentLayouter.Pages.Count;
      documentLayouter.InitLayoutInfo();
    }
    this.UpdateWordCount();
  }

  public void UpdateDocumentFields()
  {
    this.SequenceFieldResults = new Dictionary<string, int>();
    List<WField> wfieldList = new List<WField>();
    List<WField> refFields = new List<WField>();
    if (this.IsContainNumPagesField(wfieldList, refFields))
    {
      DocumentLayouter documentLayouter = new DocumentLayouter();
      documentLayouter.UpdatePageFields(this, false);
      this.PageCount = documentLayouter.Pages.Count;
      if (wfieldList.Count != 0)
        this.UpdatePageRefFields(wfieldList, documentLayouter.BookmarkStartPageNumbers);
      documentLayouter.InitLayoutInfo();
    }
    if (refFields.Count > 0)
      this.UpdateRefFields(refFields);
    if (this.m_fields != null)
    {
      for (int index = 0; index < this.m_fields.Count; ++index)
        this.m_fields[index].IsUpdated = false;
      List<WSeqField> bookmarkSeqFiled = new List<WSeqField>();
      for (int index = 0; index < this.m_fields.Count; ++index)
      {
        WField field = this.m_fields[index];
        if (!field.IsUpdated)
        {
          field.Update();
          if (field is WSeqField && !string.IsNullOrEmpty((field as WSeqField).BookmarkName))
            bookmarkSeqFiled.Add(field as WSeqField);
          int num = this.m_fields.InnerList.IndexOf((object) field);
          if (index != num)
            index = num;
        }
      }
      this.UpdateBookmarkSeqField(bookmarkSeqFiled);
    }
    this.SequenceFieldResults.Clear();
    this.SequenceFieldResults = (Dictionary<string, int>) null;
  }

  private void UpdateBookmarkSeqField(List<WSeqField> bookmarkSeqFiled)
  {
    foreach (WSeqField field in bookmarkSeqFiled)
      field.UpdateSequenceFieldResult(this.GetBookmarkSeqFiledResultNumber(field));
  }

  private string GetBookmarkSeqFiledResultNumber(WSeqField field)
  {
    Bookmark byName = this.Bookmarks.FindByName(field.BookmarkName);
    if (byName != null)
    {
      string str = byName.BookmarkEnd.NextSibling is WSeqField ? (byName.BookmarkEnd.NextSibling as Entity).GetHierarchicalIndex(string.Empty) : byName.BookmarkEnd.GetHierarchicalIndex(string.Empty);
      for (int index = this.m_fields.Count - 1; index >= 0; --index)
      {
        if (this.m_fields[index].FieldType == FieldType.FieldSequence && !((this.m_fields[index] as WSeqField).CaptionName != field.CaptionName))
        {
          string hierarchicalIndex = this.m_fields[index].GetHierarchicalIndex(string.Empty);
          if (hierarchicalIndex == str || field.CompareHierarchicalIndex(hierarchicalIndex, str))
            return this.m_fields[index].Text;
        }
      }
    }
    return 0.ToString();
  }

  internal DocumentLayouter UpdateDocumentFieldsInOptimalWay()
  {
    if (this.m_fields != null)
    {
      for (int index = 0; index < this.m_fields.Count; ++index)
        this.m_fields[index].IsUpdated = false;
      for (int index = 0; index < this.m_fields.Count; ++index)
      {
        if (!this.m_fields[index].IsUpdated)
          this.m_fields[index].Update();
      }
    }
    List<WField> wfieldList = new List<WField>();
    List<WField> refFields = new List<WField>();
    DocumentLayouter documentLayouter = (DocumentLayouter) null;
    if (this.IsContainNumPagesField(wfieldList, refFields))
    {
      documentLayouter = new DocumentLayouter();
      documentLayouter.UpdatePageFields(this, true);
      this.PageCount = documentLayouter.Pages.Count;
      if (wfieldList.Count != 0)
        this.UpdatePageRefFields(wfieldList, documentLayouter.BookmarkStartPageNumbers);
    }
    if (refFields.Count > 0)
      this.UpdateRefFields(refFields);
    return documentLayouter;
  }

  public void UpdateAlternateChunks()
  {
    this.UpdateAlternateChunk = true;
    for (int index = 0; index < this.Sections.Count; ++index)
    {
      WSection section = this.Sections[index];
      if (section != null)
      {
        while (section.Body.AlternateChunkCollection.Count > 0)
        {
          AlternateChunk alternateChunk = section.Body.AlternateChunkCollection[0];
          if (alternateChunk != null && alternateChunk.GetFormatType(alternateChunk.ContentExtension))
            alternateChunk.Update();
          section.Body.AlternateChunkCollection.Remove(alternateChunk);
        }
      }
    }
    this.UpdateAlternateChunk = false;
  }

  private void UpdatePageRefFields(
    List<WField> pagerefFields,
    Dictionary<Entity, int> bkStartPageNumbers)
  {
    for (int index = 0; index < pagerefFields.Count; ++index)
    {
      WField pagerefField = pagerefFields[index];
      bool isHiddenBookmark = false;
      BookmarkStart bookmarkOfCrossRefField = pagerefField.GetBookmarkOfCrossRefField(ref isHiddenBookmark);
      if (bookmarkOfCrossRefField != null && (bkStartPageNumbers.ContainsKey((Entity) bookmarkOfCrossRefField) || this.GetEntityOwnerTextBody(bookmarkOfCrossRefField.OwnerParagraph).Owner is WComment) && pagerefField.FieldType == FieldType.FieldPageRef)
      {
        int num = this.GetEntityOwnerTextBody(bookmarkOfCrossRefField.OwnerParagraph).Owner is WComment ? 1 : bkStartPageNumbers[(Entity) bookmarkOfCrossRefField];
        bool flag = pagerefField.InternalFieldCode.Contains("\\p");
        string empty = string.Empty;
        if (flag)
        {
          string text = !(pagerefField.FieldResult == num.ToString()) || !pagerefField.CompareOwnerTextBody((Entity) bookmarkOfCrossRefField) ? "on page " + num.ToString() : pagerefField.GetPositionValue(bookmarkOfCrossRefField);
          pagerefField.UpdateFieldResult(text);
        }
        else
          pagerefField.UpdateNumberFormatResult(num.ToString());
      }
    }
  }

  private void UpdateRefFields(List<WField> refFields)
  {
    Dictionary<Entity, string>[] listValueCollections = new Dictionary<Entity, string>[6];
    Dictionary<Entity, int>[] levelNumberCollections = new Dictionary<Entity, int>[6];
    Dictionary<Entity, string> dictionary1 = listValueCollections[0];
    Dictionary<Entity, int> dictionary2 = levelNumberCollections[0];
    for (int index1 = 0; index1 < refFields.Count; ++index1)
    {
      string[] strArray = refFields[index1].InternalFieldCode.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      bool isHiddenBookmark = false;
      BookmarkStart bookmarkOfCrossRefField = refFields[index1].GetBookmarkOfCrossRefField(ref isHiddenBookmark);
      if (bookmarkOfCrossRefField != null)
      {
        WParagraph ownerParagraph = bookmarkOfCrossRefField.OwnerParagraph;
        WTextBody entityOwnerTextBody = this.GetEntityOwnerTextBody(ownerParagraph);
        Dictionary<Entity, string> paragraphListValue;
        Dictionary<Entity, int> paragraphLevelNumbers;
        if (entityOwnerTextBody is HeaderFooter)
        {
          this.UpdateHeaderFooterListValues(listValueCollections, levelNumberCollections);
          paragraphListValue = listValueCollections[1];
          paragraphLevelNumbers = levelNumberCollections[1];
        }
        else if (entityOwnerTextBody.Owner is WTextBox || entityOwnerTextBody.Owner is Shape)
        {
          this.UpdateShapeListValues(listValueCollections, levelNumberCollections);
          paragraphListValue = listValueCollections[2];
          paragraphLevelNumbers = levelNumberCollections[2];
        }
        else if (entityOwnerTextBody.Owner is WComment)
        {
          this.UpdateCommentListValues(listValueCollections, levelNumberCollections);
          paragraphListValue = listValueCollections[3];
          paragraphLevelNumbers = levelNumberCollections[3];
        }
        else if (entityOwnerTextBody.Owner is WFootnote && (entityOwnerTextBody.Owner as WFootnote).FootnoteType == FootnoteType.Footnote)
        {
          this.UpdateFootNoteListValues(listValueCollections, levelNumberCollections);
          paragraphListValue = listValueCollections[4];
          paragraphLevelNumbers = levelNumberCollections[4];
        }
        else if (entityOwnerTextBody.Owner is WFootnote && (entityOwnerTextBody.Owner as WFootnote).FootnoteType == FootnoteType.Endnote)
        {
          this.UpdateEndNoteListValues(listValueCollections, levelNumberCollections);
          paragraphListValue = listValueCollections[5];
          paragraphLevelNumbers = levelNumberCollections[5];
        }
        else
        {
          this.UpdateSectionListValues(listValueCollections, levelNumberCollections);
          paragraphListValue = listValueCollections[0];
          paragraphLevelNumbers = levelNumberCollections[0];
        }
        bool flag = false;
        string empty = string.Empty;
        ReferenceKind referencekind = ReferenceKind.NumberFullContext;
        for (int index2 = 2; index2 < strArray.Length; ++index2)
        {
          switch (strArray[index2].ToLower())
          {
            case "\\h":
            case "\\d":
              continue;
            case "\\p":
              flag = true;
              continue;
            case "\\r":
              referencekind = ReferenceKind.NumberRelativeContext;
              continue;
            case "\\n":
              referencekind = ReferenceKind.NumberNoContext;
              continue;
            case "\\w":
              referencekind = ReferenceKind.NumberFullContext;
              continue;
            default:
              empty = strArray[index2];
              continue;
          }
        }
        WListFormat listFormatValue = ownerParagraph.GetListFormatValue();
        string str = "0";
        if (listFormatValue != null && listFormatValue.CurrentListStyle != null && paragraphListValue != null && paragraphListValue.ContainsKey((Entity) ownerParagraph))
        {
          WListLevel listLevel = ownerParagraph.GetListLevel(listFormatValue);
          switch (referencekind)
          {
            case ReferenceKind.NumberFullContext:
            case ReferenceKind.NumberRelativeContext:
              str = this.GetParagraphNumber(refFields[index1], bookmarkOfCrossRefField, ownerParagraph, listLevel, empty, referencekind, paragraphListValue, paragraphLevelNumbers);
              break;
            case ReferenceKind.NumberNoContext:
              str = paragraphListValue[(Entity) ownerParagraph];
              break;
          }
        }
        string text = str.TrimEnd('.');
        if (flag && refFields[index1].CompareOwnerTextBody((Entity) bookmarkOfCrossRefField))
          refFields[index1].UpdateFieldResult($"{text} {refFields[index1].GetPositionValue(bookmarkOfCrossRefField)}");
        else
          refFields[index1].UpdateFieldResult(text);
      }
    }
    this.ClearLists();
  }

  private void UpdateListValue(
    WTextBody textbody,
    int index,
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    foreach (TextBodyItem childEntity1 in (CollectionImpl) textbody.ChildEntities)
    {
      switch (childEntity1)
      {
        case WParagraph _:
          WListFormat listFormatValue = (childEntity1 as WParagraph).GetListFormatValue();
          if (listFormatValue != null && listFormatValue.CurrentListStyle != null)
          {
            WListLevel listLevel = (childEntity1 as WParagraph).GetListLevel(listFormatValue);
            listValueCollections[index].Add((Entity) childEntity1, this.UpdateListValue(childEntity1 as WParagraph, listFormatValue, listLevel));
            levelNumberCollections[index].Add((Entity) childEntity1, listLevel.LevelNumber);
            continue;
          }
          continue;
        case WTable _:
          IEnumerator enumerator = (childEntity1 as WTable).ChildEntities.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              foreach (WTextBody childEntity2 in (CollectionImpl) ((WTableRow) enumerator.Current).ChildEntities)
                this.UpdateListValue(childEntity2, index, listValueCollections, levelNumberCollections);
            }
            continue;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        case BlockContentControl _:
          if (childEntity1 is BlockContentControl blockContentControl)
          {
            this.UpdateListValue(blockContentControl.TextBody, index, listValueCollections, levelNumberCollections);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void UpdateHeaderFooterListValues(
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    if (listValueCollections[1] != null)
      return;
    listValueCollections[1] = new Dictionary<Entity, string>();
    levelNumberCollections[1] = new Dictionary<Entity, int>();
    this.ClearLists();
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      for (int index = 0; index < section.ChildEntities.Count; ++index)
      {
        if (section.ChildEntities[index].EntityType == EntityType.HeaderFooter)
          this.UpdateListValue(section.ChildEntities[index] as WTextBody, 1, listValueCollections, levelNumberCollections);
      }
    }
  }

  private void UpdateShapeListValues(
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    if (listValueCollections[2] != null)
      return;
    this.ClearLists();
    listValueCollections[2] = new Dictionary<Entity, string>();
    levelNumberCollections[2] = new Dictionary<Entity, int>();
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WParagraph paragraph in (IEnumerable) section.Paragraphs)
      {
        foreach (ParagraphItem childEntity in (CollectionImpl) paragraph.ChildEntities)
        {
          switch (childEntity.EntityType)
          {
            case EntityType.Shape:
            case EntityType.AutoShape:
              this.UpdateListValue((childEntity as Shape).TextBody, 2, listValueCollections, levelNumberCollections);
              continue;
            case EntityType.TextBox:
              this.UpdateListValue((childEntity as WTextBox).TextBoxBody, 2, listValueCollections, levelNumberCollections);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  private void UpdateEndNoteListValues(
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    if (listValueCollections[5] != null)
      return;
    this.ClearLists();
    listValueCollections[5] = new Dictionary<Entity, string>();
    levelNumberCollections[5] = new Dictionary<Entity, int>();
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WParagraph paragraph in (IEnumerable) section.Paragraphs)
      {
        foreach (ParagraphItem childEntity in (CollectionImpl) paragraph.ChildEntities)
        {
          if (childEntity is WFootnote && (childEntity as WFootnote).FootnoteType == FootnoteType.Endnote)
            this.UpdateListValue((childEntity as WFootnote).TextBody, 5, listValueCollections, levelNumberCollections);
        }
      }
    }
  }

  private void UpdateFootNoteListValues(
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    if (listValueCollections[4] != null)
      return;
    this.ClearLists();
    listValueCollections[4] = new Dictionary<Entity, string>();
    levelNumberCollections[4] = new Dictionary<Entity, int>();
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WParagraph paragraph in (IEnumerable) section.Paragraphs)
      {
        foreach (ParagraphItem childEntity in (CollectionImpl) paragraph.ChildEntities)
        {
          if (childEntity is WFootnote && (childEntity as WFootnote).FootnoteType == FootnoteType.Footnote)
            this.UpdateListValue((childEntity as WFootnote).TextBody, 4, listValueCollections, levelNumberCollections);
        }
      }
    }
  }

  private void UpdateCommentListValues(
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    if (listValueCollections[3] != null)
      return;
    this.ClearLists();
    listValueCollections[3] = new Dictionary<Entity, string>();
    levelNumberCollections[3] = new Dictionary<Entity, int>();
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (WParagraph paragraph in (IEnumerable) section.Paragraphs)
      {
        foreach (ParagraphItem childEntity in (CollectionImpl) paragraph.ChildEntities)
        {
          if (childEntity.EntityType == EntityType.Comment)
            this.UpdateListValue((childEntity as WComment).TextBody, 3, listValueCollections, levelNumberCollections);
        }
      }
    }
  }

  private void UpdateSectionListValues(
    Dictionary<Entity, string>[] listValueCollections,
    Dictionary<Entity, int>[] levelNumberCollections)
  {
    if (listValueCollections[0] != null)
      return;
    listValueCollections[0] = new Dictionary<Entity, string>();
    levelNumberCollections[0] = new Dictionary<Entity, int>();
    this.ClearLists();
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      for (int index = 0; index < section.ChildEntities.Count; ++index)
      {
        if (section.ChildEntities[index].EntityType == EntityType.TextBody)
          this.UpdateListValue(section.ChildEntities[index] as WTextBody, 0, listValueCollections, levelNumberCollections);
      }
    }
  }

  private string GetParagraphNumber(
    WField refFields,
    BookmarkStart bkStart,
    WParagraph ownerPara,
    WListLevel level,
    string separator,
    ReferenceKind referencekind,
    Dictionary<Entity, string> paragraphListValue,
    Dictionary<Entity, int> paragraphLevelNumbers)
  {
    string paragraphNumber = "0";
    bool flag = false;
    List<Entity> entityList = new List<Entity>();
    foreach (Entity key in paragraphListValue.Keys)
      entityList.Add(key);
    int num1 = entityList.IndexOf((Entity) ownerPara);
    if (level.PatternType == ListPatternType.Bullet)
      paragraphNumber = paragraphListValue[(Entity) ownerPara];
    else if (referencekind == ReferenceKind.NumberRelativeContext && refFields.CompareOwnerTextBody((Entity) bkStart))
    {
      string positionValue = refFields.GetPositionValue(bkStart);
      if (ownerPara == refFields.OwnerParagraph)
        paragraphNumber = paragraphListValue[(Entity) ownerPara];
      else if (!paragraphListValue.ContainsKey((Entity) refFields.OwnerParagraph) && positionValue == "below")
      {
        flag = true;
      }
      else
      {
        int paragraphLevelNumber1 = paragraphLevelNumbers[(Entity) ownerPara];
        int num2 = -1;
        int num3 = -1;
        for (int index = num1 + 1; index < entityList.Count; ++index)
        {
          if (paragraphLevelNumbers[entityList[index]] < paragraphLevelNumber1)
          {
            if (paragraphLevelNumbers[entityList[index]] == 0)
            {
              if (paragraphListValue[entityList[index]] != "1.")
                num2 = paragraphLevelNumbers[entityList[index]];
              num3 = index;
              break;
            }
            num2 = paragraphLevelNumbers[entityList[index]];
            num3 = index;
          }
        }
        if (num2 == -1 && refFields.GetPositionValue(bkStart) == "above")
          paragraphNumber = paragraphListValue[(Entity) ownerPara];
        else if (paragraphListValue.ContainsKey((Entity) refFields.OwnerParagraph) && positionValue == "below" && entityList.IndexOf((Entity) refFields.OwnerParagraph) < num1)
        {
          paragraphNumber = paragraphListValue[(Entity) ownerPara];
          int paragraphLevelNumber2 = paragraphLevelNumbers[(Entity) ownerPara];
          for (int index = num1 - 1; index >= 0; --index)
          {
            if (paragraphLevelNumbers[entityList[index]] == paragraphLevelNumber2 - 1)
            {
              if (entityList[index] != refFields.OwnerParagraph)
              {
                paragraphNumber = paragraphListValue[entityList[index]] + (separator != string.Empty ? separator : string.Empty) + paragraphNumber;
                paragraphLevelNumber2 = paragraphLevelNumbers[entityList[index]];
              }
              else
                break;
            }
          }
        }
        else if (paragraphListValue.ContainsKey((Entity) refFields.OwnerParagraph) && positionValue == "above" && entityList.IndexOf((Entity) refFields.OwnerParagraph) > num1 && entityList.IndexOf((Entity) refFields.OwnerParagraph) < num3)
        {
          paragraphNumber = paragraphListValue[(Entity) ownerPara];
          int paragraphLevelNumber3 = paragraphLevelNumbers[(Entity) ownerPara];
          for (int index = num1 - 1; index >= 0; --index)
          {
            if (paragraphLevelNumbers[entityList[index]] == paragraphLevelNumber3 - 1)
            {
              paragraphNumber = paragraphListValue[entityList[index]] + (separator != string.Empty ? separator : string.Empty) + paragraphNumber;
              paragraphLevelNumber3 = paragraphLevelNumbers[entityList[index]];
              if (paragraphLevelNumbers[(Entity) refFields.OwnerParagraph] == paragraphLevelNumbers[entityList[index]])
                break;
            }
          }
        }
        else if (num2 == 0 && positionValue == "above" && paragraphListValue.ContainsKey((Entity) refFields.OwnerParagraph) && entityList.IndexOf((Entity) refFields.OwnerParagraph) < num3)
        {
          paragraphNumber = paragraphListValue[(Entity) ownerPara];
        }
        else
        {
          paragraphNumber = paragraphListValue[(Entity) ownerPara];
          int paragraphLevelNumber4 = paragraphLevelNumbers[(Entity) ownerPara];
          for (int index = num1 - 1; index >= 0; --index)
          {
            if (paragraphLevelNumbers[entityList[index]] == paragraphLevelNumber4 - 1)
            {
              paragraphNumber = paragraphListValue[entityList[index]] + (separator != string.Empty ? separator : string.Empty) + paragraphNumber;
              paragraphLevelNumber4 = paragraphLevelNumbers[entityList[index]];
              if (num2 == paragraphLevelNumber4)
                break;
            }
          }
        }
      }
    }
    else
      flag = true;
    if (level.PatternType != ListPatternType.Bullet && (referencekind == ReferenceKind.NumberRelativeContext && flag || referencekind == ReferenceKind.NumberFullContext))
    {
      paragraphNumber = paragraphListValue[(Entity) ownerPara];
      int paragraphLevelNumber = paragraphLevelNumbers[(Entity) ownerPara];
      if (referencekind == ReferenceKind.NumberRelativeContext)
        separator = string.Empty;
      for (int index = num1 - 1; index >= 0; --index)
      {
        if (paragraphLevelNumbers[entityList[index]] == paragraphLevelNumber - 1)
        {
          paragraphNumber = paragraphListValue[entityList[index]] + (separator != string.Empty ? separator : string.Empty) + paragraphNumber;
          paragraphLevelNumber = paragraphLevelNumbers[entityList[index]];
        }
      }
    }
    return paragraphNumber;
  }

  internal WTextBody GetEntityOwnerTextBody(WParagraph para)
  {
    WTextBody ownerTextBody = para.OwnerTextBody;
    while (ownerTextBody is WTableCell)
    {
      ownerTextBody = (ownerTextBody as WTableCell).OwnerRow.OwnerTable.OwnerTextBody;
      if (ownerTextBody != null && ownerTextBody.Owner is BlockContentControl)
        ownerTextBody = (ownerTextBody.Owner as BlockContentControl).OwnerTextBody;
    }
    if (ownerTextBody != null && ownerTextBody.Owner is BlockContentControl)
      ownerTextBody = (ownerTextBody.Owner as BlockContentControl).OwnerTextBody;
    return ownerTextBody;
  }

  private bool IsContainNumPagesField(List<WField> pagereffields, List<WField> refFields)
  {
    bool flag = false;
    if (this.m_fields != null && this.m_fields.Count > 0)
    {
      for (int index = 0; index < this.m_fields.Count; ++index)
      {
        if (this.m_fields[index].FieldType == FieldType.FieldNumPages || this.m_fields[index].FieldType == FieldType.FieldPage)
          flag = true;
        else if (this.m_fields[index].FieldType == FieldType.FieldPageRef || this.m_fields[index].FieldType == FieldType.FieldRef)
        {
          bool isHiddenBookmark = false;
          if (this.m_fields[index].GetBookmarkOfCrossRefField(ref isHiddenBookmark) != null)
          {
            if (this.m_fields[index].FieldType == FieldType.FieldRef && (this.m_fields[index].InternalFieldCode.Contains("\\n") || this.m_fields[index].InternalFieldCode.Contains("\\w") || this.m_fields[index].InternalFieldCode.Contains("\\r")))
              refFields.Add(this.m_fields[index]);
            else if (this.m_fields[index].FieldType == FieldType.FieldPageRef)
            {
              pagereffields.Add(this.m_fields[index]);
              flag = true;
            }
          }
          else if (!isHiddenBookmark && this.m_fields[index].FieldType == FieldType.FieldPageRef)
            this.m_fields[index].UpdateFieldResult("Error! No bookmark name given.");
        }
      }
    }
    return flag;
  }

  private void CalculateForTextBody(BodyItemCollection bodyItems)
  {
    foreach (TextBodyItem bodyItem in (CollectionImpl) bodyItems)
    {
      switch (bodyItem)
      {
        case WParagraph _:
          this.CalculateForParagraphs(bodyItem as WParagraph);
          continue;
        case WTable _:
          this.CalculateForTabls(bodyItem as WTable);
          continue;
        case BlockContentControl _:
          if ((bodyItem as BlockContentControl).TextBody != null)
          {
            this.CalculateForTextBody((bodyItem as BlockContentControl).TextBody.Items);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  private void CalculateForTabls(WTable table)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
        this.CalculateForTextBody(cell.Items);
    }
  }

  private void CalculateForParagraphs(WParagraph para)
  {
    string text = para.Text;
    if (!(para.Text != string.Empty))
      return;
    ++this.m_paraCount;
    foreach (string str in text.Split(" ".ToCharArray()))
    {
      if (str != string.Empty)
        ++this.m_wordCount;
    }
    this.m_charCount += text.Replace(" ", string.Empty).Length;
  }

  public void UpdateTableOfContents() => this.UpdateTableOfContent();

  internal void UpdateTableOfContent()
  {
    if (!this.HasTOC)
      return;
    List<ParagraphItem> paragraphItemList = (List<ParagraphItem>) null;
    TableOfContent tableOfContent1 = (TableOfContent) null;
    Entity entity = (Entity) null;
    bool flag1 = false;
    bool flag2 = false;
    if (this.Styles != null && this.Styles.FindByName("Hyperlink", StyleType.CharacterStyle) == null)
      this.AddStyle(BuiltinStyle.Hyperlink);
    bool flag3 = false;
    foreach (TableOfContent tableOfContent2 in this.TOC.Values)
    {
      if (!tableOfContent2.FormattingString.Contains("\\c") && !tableOfContent2.FormattingString.Contains("\\a"))
      {
        List<string> tocLinkCharacterStyleNames = tableOfContent2.UpdateTOCStyleLevels();
        tableOfContent2.RemoveUpdatedTocEntries();
        if (!flag3)
        {
          tableOfContent2.RemoveExistingTocBookmarks();
          flag3 = true;
        }
        paragraphItemList = tableOfContent2.ParseDocument(tocLinkCharacterStyleNames);
        if (!flag2 && tableOfContent2.IncludePageNumbers)
          flag2 = true;
        tableOfContent1 = tableOfContent2;
      }
    }
    if (tableOfContent1 != null)
    {
      Dictionary<int, List<string>> tocLevels = tableOfContent1.TOCLevels;
      flag1 = tableOfContent1.UseTableEntryFields;
      entity = tableOfContent1.m_tocEntryLastEntity;
    }
    if (flag2 && entity != null)
    {
      Dictionary<Entity, int> entryPageNumbers = new DocumentLayouter()
      {
        UseTCFields = flag1,
        tocParaItems = paragraphItemList,
        LastTocEntity = entity
      }.GetTOCEntryPageNumbers(this.Document);
      if (entryPageNumbers != null)
      {
        foreach (TableOfContent tableOfContent3 in this.TOC.Values)
        {
          if (tableOfContent3.IncludePageNumbers)
            tableOfContent3.UpdatePageNumbers(entryPageNumbers);
        }
      }
    }
    this.ClearLists();
  }

  internal string UpdateListValue(WParagraph paragraph, WListFormat listFormat, WListLevel level)
  {
    if (paragraph.BreakCharacterFormat.IsDeleteRevision)
      return string.Empty;
    string customStyleName = listFormat.CustomStyleName;
    if (paragraph.Owner != null && paragraph.Owner.Owner is WTextBox)
      customStyleName += "_textbox";
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    if (listFormat.LFOStyleName != null && listFormat.LFOStyleName.Length > 0)
      listOverrideStyle = this.ListOverrides.FindByName(listFormat.LFOStyleName);
    if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(level.LevelNumber) && listOverrideStyle.OverrideLevels[level.LevelNumber].OverrideStartAtValue && !this.PreviousListLevelOverrideStyle.Contains(listOverrideStyle.Name))
    {
      this.EnsureLevelRestart(listFormat, customStyleName, true, level);
      this.PreviousListLevelOverrideStyle.Add(listOverrideStyle.Name);
    }
    else if (paragraph.ListFormat.RestartNumbering)
      this.EnsureLevelRestart(listFormat, customStyleName, true, level);
    else if (this.PreviousListLevel.ContainsKey(customStyleName) && level.LevelNumber > this.PreviousListLevel[customStyleName])
      this.EnsureLevelRestart(listFormat, customStyleName, false, level);
    bool isIncreseStartVal = false;
    if (this.PreviousListLevel.ContainsKey(customStyleName) && this.PreviousListLevel[customStyleName] > level.LevelNumber)
      isIncreseStartVal = true;
    string empty = string.Empty;
    int listItemIndex = this.GetListItemIndex(listFormat, customStyleName, level, isIncreseStartVal);
    level.NumberPrefix = this.UpdateNumberPrefix(level.NumberPrefix, listFormat);
    string str = level.GetListItemText(listItemIndex, listFormat.ListType, paragraph);
    int listStartValue = this.GetListStartValue(listFormat, customStyleName, level);
    if (level.NumberPrefix != null && level.NumberPrefix.Contains("\0"))
      str = this.GetListValue(listFormat, customStyleName, level, listStartValue, listItemIndex);
    if (level.PatternType == ListPatternType.Bullet)
      str = level.BulletCharacter;
    else if (string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(level.LevelText))
      str = this.GetListLevelText(level.LevelText);
    if (this.PreviousListLevel.ContainsKey(customStyleName))
      this.PreviousListLevel[customStyleName] = level.LevelNumber;
    else
      this.PreviousListLevel.Add(customStyleName, level.LevelNumber);
    return str;
  }

  private string UpdateNumberPrefix(string numberPrefix, WListFormat listFormat)
  {
    if (string.IsNullOrEmpty(numberPrefix) || numberPrefix.Contains("\0"))
      return numberPrefix;
    string[] strArray = new string[9]
    {
      "\0",
      "\u0001",
      "\u0002",
      "\u0003",
      "\u0004",
      "\u0005",
      "\u0006",
      "\a",
      "\b"
    };
    bool flag = false;
    foreach (string str in strArray)
    {
      if (numberPrefix.Contains(str))
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return numberPrefix;
    numberPrefix = DocxSerializator.UpdateNumberPrefix(numberPrefix);
    string empty = string.Empty;
    char ch1 = char.MinValue;
    foreach (char ch2 in numberPrefix)
    {
      if (ch1 == '%')
      {
        bool leadZero = false;
        int num = int.Parse(ch2.ToString());
        WListLevel level = listFormat.CurrentListStyle.Levels[num - 1];
        int startAt = level.StartAt;
        if (level.PatternType == ListPatternType.LeadingZero && level.StartAt < 10)
          leadZero = true;
        empty += this.GetNumberedListValue(level, level.StartAt, leadZero, "");
      }
      else if (ch2 != '%')
        empty += ch2.ToString();
      ch1 = ch2;
    }
    return empty;
  }

  private string GetListLevelText(string levelText)
  {
    if (!levelText.Contains("%"))
      return levelText;
    string listLevelText = string.Empty;
    bool flag = false;
    foreach (char ch in levelText)
    {
      if (flag)
      {
        flag = false;
        int result = int.MinValue;
        if (int.TryParse(ch.ToString(), out result))
        {
          if (result > 1)
            listLevelText += (string) (object) (result - 1);
          else if (result == 0)
            return (string) null;
        }
        else
          listLevelText = $"{listLevelText}%{(object) ch}";
      }
      else if (ch == '%')
        flag = true;
      else
        listLevelText += (string) (object) ch;
    }
    return listLevelText;
  }

  internal void ClearLists()
  {
    this.PreviousListLevel.Clear();
    this.PreviousListLevelOverrideStyle.Clear();
    this.Lists.Clear();
    this.ListNames.Clear();
  }

  private void EnsureLevelRestart(
    WListFormat format,
    string styleName,
    bool fullRestart,
    WListLevel listLevel)
  {
    if (this.m_listNames == null)
      return;
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    if (format.LFOStyleName != null && format.LFOStyleName.Length > 0)
      listOverrideStyle = this.ListOverrides.FindByName(format.LFOStyleName);
    if (!(this.ListNames[(object) styleName] is HybridDictionary listName))
      return;
    ICollection keys = listName.Keys;
    IEnumerator enumerator = keys.GetEnumerator();
    int count = keys.Count;
    int[] numArray = new int[count];
    int index1 = 0;
    while (enumerator.MoveNext())
    {
      numArray[index1] = (int) enumerator.Current;
      ++index1;
    }
    bool flag = false;
    for (int index2 = 0; index2 < count; ++index2)
    {
      if (fullRestart || numArray[index2] >= listLevel.LevelNumber && !format.CurrentListStyle.Levels[numArray[index2]].NoRestartByHigher)
      {
        int startAt = format.CurrentListStyle.Levels[listLevel.LevelNumber].StartAt;
        if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(listLevel.LevelNumber) && this.Lists.ContainsKey(styleName))
        {
          startAt = listLevel.StartAt;
          if (listOverrideStyle.OverrideLevels[listLevel.LevelNumber].OverrideStartAtValue)
          {
            startAt = listOverrideStyle.OverrideLevels[format.ListLevelNumber].StartAt;
            if (startAt != 1)
              flag = true;
          }
          Dictionary<int, int> list = this.Lists[styleName];
          for (int levelNumber = listLevel.LevelNumber; list.ContainsKey(levelNumber); ++levelNumber)
            list[levelNumber] = 1;
        }
        if (listLevel.LevelNumber == numArray[index2])
          listName[(object) numArray[index2]] = (object) startAt;
      }
    }
    if (!flag)
      return;
    Dictionary<int, int> list1 = this.Lists[styleName];
    if (!list1.ContainsKey(listLevel.LevelNumber) || count <= listLevel.LevelNumber)
      return;
    list1[listLevel.LevelNumber] = (int) listName[(object) numArray[listLevel.LevelNumber]];
  }

  private int GetListItemIndex(
    WListFormat format,
    string styleName,
    WListLevel listLevel,
    bool isIncreseStartVal)
  {
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    if (format.LFOStyleName != null && format.LFOStyleName.Length > 0)
      listOverrideStyle = this.ListOverrides.FindByName(format.LFOStyleName);
    if (!(this.ListNames[(object) styleName] is HybridDictionary listName))
    {
      HybridDictionary hybridDictionary = new HybridDictionary();
      this.ListNames.Add((object) styleName, (object) hybridDictionary);
      int index = listLevel.LevelNumber;
      if (listLevel.LevelText != null && listLevel.LevelText.Length > 1 && listLevel.LevelText.IndexOf("%") == 0 && listLevel.LevelText.IndexOf('.') == 2 && listLevel.LevelNumber == int.Parse(listLevel.LevelText[1].ToString()))
        index = int.Parse(listLevel.LevelText[1].ToString()) - 1;
      int startAt = format.CurrentListStyle.Levels[index].StartAt;
      if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(listLevel.LevelNumber) && listOverrideStyle.OverrideLevels[listLevel.LevelNumber].OverrideStartAtValue)
        startAt = listOverrideStyle.OverrideLevels[listLevel.LevelNumber].StartAt;
      hybridDictionary.Add((object) listLevel.LevelNumber, (object) (startAt + 1));
      return startAt - 1;
    }
    if (listName[(object) listLevel.LevelNumber] != null)
    {
      int num = (int) listName[(object) listLevel.LevelNumber];
      listName[(object) listLevel.LevelNumber] = (object) (num + 1);
      return num - 1;
    }
    int startAt1 = format.CurrentListStyle.Levels[listLevel.LevelNumber].StartAt;
    if (isIncreseStartVal)
      ++startAt1;
    if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(listLevel.LevelNumber) && listOverrideStyle.OverrideLevels[listLevel.LevelNumber].OverrideStartAtValue)
      startAt1 = listOverrideStyle.OverrideLevels[listLevel.LevelNumber].StartAt;
    listName.Add((object) listLevel.LevelNumber, (object) (startAt1 + 1));
    return startAt1 - 1;
  }

  private int GetListStartValue(WListFormat format, string styleName, WListLevel listLevel)
  {
    if (listLevel != null && listLevel.PatternType == ListPatternType.Bullet)
      return 1;
    if (!this.Lists.ContainsKey(styleName))
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      this.Lists.Add(styleName, dictionary);
      if (format.CurrentListStyle == null)
        return 1;
      WListLevel level = format.CurrentListStyle.Levels[listLevel.LevelNumber];
      ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
      if (format.LFOStyleName != null && format.LFOStyleName.Length > 0)
        listOverrideStyle = this.ListOverrides.FindByName(format.LFOStyleName);
      if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(listLevel.LevelNumber) && this.Lists.ContainsKey(styleName) && listOverrideStyle.OverrideLevels[listLevel.LevelNumber].OverrideStartAtValue)
      {
        int startAt = listOverrideStyle.OverrideLevels[format.ListLevelNumber].StartAt;
        format.CurrentListStyle.Levels[level.LevelNumber].StartAt = startAt;
      }
      for (int index = 0; index <= level.LevelNumber; ++index)
      {
        if (format.CurrentListStyle.Levels[index].PatternType != ListPatternType.Bullet)
          dictionary.Add(index, format.CurrentListStyle.Levels[index].StartAt + 1);
      }
      return format.CurrentListStyle.Levels[level.LevelNumber].StartAt;
    }
    Dictionary<int, int> list = this.Lists[styleName];
    if (list.ContainsKey(listLevel.LevelNumber))
    {
      int listStartValue = list[listLevel.LevelNumber];
      list[listLevel.LevelNumber] = listStartValue + 1;
      for (int levelNumber = listLevel.LevelNumber; list.ContainsKey(levelNumber + 1); ++levelNumber)
        list[levelNumber + 1] = 1;
      this.ResetInbetweenLevels(format, styleName, listLevel, list);
      return listStartValue;
    }
    WListLevel level1 = format.CurrentListStyle.Levels[listLevel.LevelNumber];
    ListOverrideStyle listOverrideStyle1 = (ListOverrideStyle) null;
    if (format.LFOStyleName != null && format.LFOStyleName.Length > 0)
      listOverrideStyle1 = this.ListOverrides.FindByName(format.LFOStyleName);
    if (listOverrideStyle1 != null && listOverrideStyle1.OverrideLevels.HasOverrideLevel(listLevel.LevelNumber) && this.Lists.ContainsKey(styleName) && listOverrideStyle1.OverrideLevels[listLevel.LevelNumber].OverrideStartAtValue)
    {
      int startAt = listOverrideStyle1.OverrideLevels[format.ListLevelNumber].StartAt;
      level1.StartAt = startAt;
    }
    for (int index = 0; index <= level1.LevelNumber; ++index)
    {
      if (format.CurrentListStyle.Levels[index].PatternType != ListPatternType.Bullet && !list.ContainsKey(index))
        list.Add(index, format.CurrentListStyle.Levels[index].StartAt + 1);
    }
    this.ResetInbetweenLevels(format, styleName, listLevel, list);
    return level1.StartAt;
  }

  private void ResetInbetweenLevels(
    WListFormat format,
    string styleName,
    WListLevel listLevel,
    Dictionary<int, int> lstStyle)
  {
    if (this.PreviousListLevel[styleName] - listLevel.LevelNumber >= -1)
      return;
    for (int index = this.PreviousListLevel[styleName] + 1; index < listLevel.LevelNumber; ++index)
      lstStyle[index] = format.CurrentListStyle.Levels[index].StartAt + 1;
  }

  private string GetListValue(
    WListFormat listFormat,
    string styleName,
    WListLevel level,
    int startAt,
    int listItemIndex)
  {
    string listValue = string.Empty;
    int levelNumber = level.LevelNumber;
    string str1 = (string) null;
    string[] strArray = new string[9]
    {
      "\0",
      "\u0001",
      "\u0002",
      "\u0003",
      "\u0004",
      "\u0005",
      "\u0006",
      "\a",
      "\b"
    };
    for (int index = 0; index <= levelNumber; ++index)
    {
      if (level.NumberPrefix.Contains(strArray[index]))
        str1 += index.ToString();
    }
    if (this.Lists.ContainsKey(styleName))
    {
      string str2 = string.Empty;
      Dictionary<int, int> list = this.Lists[styleName];
      int[] numArray = new int[list.Count];
      list.Keys.CopyTo(numArray, 0);
      int sortKey = this.SortKeys(numArray)[0];
      bool leadZero = listItemIndex < 9;
      string orderedNumberPrefix = this.GetOrderedNumberPrefix(level.NumberPrefix);
      for (int index = sortKey; (sortKey == levelNumber ? (index <= levelNumber ? 1 : 0) : (index < levelNumber ? 1 : 0)) != 0; ++index)
      {
        if (str1.Contains(index.ToString()) && list.ContainsKey(index))
        {
          WListLevel level1 = listFormat.CurrentListStyle.Levels[index];
          string prefixByItsLevel = this.GetListPrefixByItsLevel(index + 1, orderedNumberPrefix);
          str2 = !level.IsLegalStyleNumbering ? str2 + this.GetNumberedListValue(level1, list[index] - 1, leadZero, prefixByItsLevel) : str2 + Convert.ToString(list[index] - 1) + prefixByItsLevel;
        }
      }
      if (level.PatternType == ListPatternType.LeadingZero && startAt < 10)
        leadZero = true;
      string str3 = str2 + this.GetNumberedListValue(level, startAt, leadZero, "");
      string numberPrefix = level.NumberPrefix;
      if (!numberPrefix.StartsWithExt("\0") && numberPrefix.Contains("\0"))
        str3 = numberPrefix.Substring(0, numberPrefix.IndexOf("\0", StringComparison.Ordinal)) + str3;
      listValue = str3 + level.NumberSuffix;
    }
    return listValue;
  }

  private string GetOrderedNumberPrefix(string levelNumberPrefix)
  {
    string[] array = new string[9]
    {
      "\0",
      "\u0001",
      "\u0002",
      "\u0003",
      "\u0004",
      "\u0005",
      "\u0006",
      "\a",
      "\b"
    };
    char[] chArray = new char[levelNumberPrefix.Length];
    int index1 = 0;
    foreach (char ch in levelNumberPrefix)
    {
      if (Array.IndexOf<string>(array, ch.ToString()) != -1)
      {
        if (index1 == 0 || (int) chArray[index1 - 1] <= (int) ch)
        {
          chArray[index1] = ch;
        }
        else
        {
          chArray[index1] = chArray[index1 - 1];
          chArray[index1 - 1] = ch;
        }
        ++index1;
      }
    }
    string empty = string.Empty;
    int index2 = 0;
    foreach (char ch in levelNumberPrefix)
    {
      string str = ch.ToString();
      if (Array.IndexOf<string>(array, str) != -1)
      {
        empty += chArray[index2].ToString();
        ++index2;
      }
      else
        empty += str;
    }
    return empty;
  }

  private string GetListPrefixByItsLevel(int levelNo, string listPrefix)
  {
    string prefixByItsLevel = (string) null;
    switch (levelNo)
    {
      case 1:
        int startIndex1 = listPrefix.IndexOf("\0", StringComparison.Ordinal) + 1;
        int num1 = listPrefix.Contains("\u0001") ? listPrefix.IndexOf("\u0001", StringComparison.Ordinal) : (listPrefix.Contains("\u0002") ? listPrefix.IndexOf("\u0002", StringComparison.Ordinal) : listPrefix.Length);
        prefixByItsLevel = listPrefix.Substring(startIndex1, num1 - startIndex1);
        break;
      case 2:
        int startIndex2 = listPrefix.IndexOf("\u0001", StringComparison.Ordinal) + 1;
        int num2 = listPrefix.Contains("\u0002") ? listPrefix.IndexOf("\u0002", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex2, num2 - startIndex2);
        break;
      case 3:
        int startIndex3 = listPrefix.IndexOf("\u0002", StringComparison.Ordinal) + 1;
        int num3 = listPrefix.Contains("\u0003") ? listPrefix.IndexOf("\u0003", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex3, num3 - startIndex3);
        break;
      case 4:
        int startIndex4 = listPrefix.IndexOf("\u0003", StringComparison.Ordinal) + 1;
        int num4 = listPrefix.Contains("\u0004") ? listPrefix.IndexOf("\u0004", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex4, num4 - startIndex4);
        break;
      case 5:
        int startIndex5 = listPrefix.IndexOf("\u0004", StringComparison.Ordinal) + 1;
        int num5 = listPrefix.Contains("\u0005") ? listPrefix.IndexOf("\u0005", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex5, num5 - startIndex5);
        break;
      case 6:
        int startIndex6 = listPrefix.IndexOf("\u0005", StringComparison.Ordinal) + 1;
        int num6 = listPrefix.Contains("\u0006") ? listPrefix.IndexOf("\u0006", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex6, num6 - startIndex6);
        break;
      case 7:
        int startIndex7 = listPrefix.IndexOf("\u0006", StringComparison.Ordinal) + 1;
        int num7 = listPrefix.Contains("\a") ? listPrefix.IndexOf("\a", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex7, num7 - startIndex7);
        break;
      case 8:
        int startIndex8 = listPrefix.IndexOf("\a", StringComparison.Ordinal) + 1;
        int num8 = listPrefix.Contains("\b") ? listPrefix.IndexOf("\b", StringComparison.Ordinal) : listPrefix.Length;
        prefixByItsLevel = listPrefix.Substring(startIndex8, num8 - startIndex8);
        break;
    }
    return prefixByItsLevel;
  }

  private string GetNumberedListValue(
    WListLevel prevLevel,
    int num,
    bool leadZero,
    string listPrefix)
  {
    if (prevLevel.IsLegalStyleNumbering && prevLevel.PatternType != ListPatternType.LeadingZero)
      return Convert.ToString(num) + listPrefix;
    string empty = string.Empty;
    string numberedListValue;
    switch (prevLevel.PatternType)
    {
      case ListPatternType.Arabic:
        numberedListValue = Convert.ToString(num) + listPrefix;
        break;
      case ListPatternType.UpRoman:
        numberedListValue = this.Document.GetAsRoman(num).ToUpper() + listPrefix;
        break;
      case ListPatternType.LowRoman:
        numberedListValue = this.Document.GetAsRoman(num).ToLower() + listPrefix;
        break;
      case ListPatternType.UpLetter:
        numberedListValue = this.Document.GetAsLetter(num).ToUpper() + listPrefix;
        break;
      case ListPatternType.LowLetter:
        numberedListValue = this.Document.GetAsLetter(num).ToLower() + listPrefix;
        break;
      case ListPatternType.Ordinal:
        numberedListValue = this.Document.GetOrdinal(num, prevLevel.CharacterFormat) + listPrefix;
        break;
      case ListPatternType.LeadingZero:
        numberedListValue = !leadZero ? Convert.ToString(num) + listPrefix : $"0{Convert.ToString(num)}{listPrefix}";
        break;
      case ListPatternType.None:
        numberedListValue = listPrefix;
        break;
      default:
        numberedListValue = Convert.ToString(num) + listPrefix;
        break;
    }
    return numberedListValue;
  }

  private int[] SortKeys(int[] keys)
  {
    for (int index1 = 0; index1 < keys.Length - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 < keys.Length; ++index2)
      {
        if (keys[index1] > keys[index2])
        {
          int key = keys[index1];
          keys[index1] = keys[index2];
          keys[index2] = key;
        }
      }
    }
    return keys;
  }

  public int ReplaceSingleLine(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return this.ReplaceSingleLine(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replace);
  }

  public int ReplaceSingleLine(Regex pattern, string replace)
  {
    TextBodyItem startItem = this.Sections[0].Body.Items[0];
    int num = this.ReplaceSingleLine(pattern, replace, startItem);
    return this.ReplaceFirst && num > 0 ? num : num + this.ReplaceHFSingleLine(pattern, replace);
  }

  public int ReplaceSingleLine(
    string given,
    TextSelection replacement,
    bool caseSensitive,
    bool wholeWord)
  {
    return this.ReplaceSingleLine(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replacement);
  }

  public int ReplaceSingleLine(Regex pattern, TextSelection replacement)
  {
    int num = 0;
    TextBodyItem startBodyItem = this.Sections[0].Body.Items[0];
    for (TextSelection[] nextSingleLine = this.FindNextSingleLine(startBodyItem, pattern); nextSingleLine != null; nextSingleLine = this.FindNextSingleLine(startBodyItem, pattern))
    {
      this.m_nextParaItem = (ParagraphItem) null;
      TextReplacer.Instance.ReplaceSingleLine(nextSingleLine, replacement);
      ++num;
      if (this.ReplaceFirst)
        break;
    }
    return num;
  }

  public int ReplaceSingleLine(
    string given,
    TextBodyPart replacement,
    bool caseSensitive,
    bool wholeWord)
  {
    return this.ReplaceSingleLine(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replacement);
  }

  public int ReplaceSingleLine(Regex pattern, TextBodyPart replacement)
  {
    int num = 0;
    TextBodyItem startBodyItem = this.Sections[0].Body.Items[0];
    for (TextSelection[] nextSingleLine = this.FindNextSingleLine(startBodyItem, pattern); nextSingleLine != null; nextSingleLine = this.FindNextSingleLine(startBodyItem, pattern))
    {
      this.m_nextParaItem = (ParagraphItem) null;
      TextReplacer.Instance.ReplaceSingleLine(nextSingleLine, replacement);
      ++num;
      if (this.ReplaceFirst)
        break;
    }
    return num;
  }

  private int ReplaceHFSingleLine(Regex pattern, string replace)
  {
    this.ResetSingleLineReplace();
    int num = 0;
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      foreach (HeaderFooter headersFooter in section.HeadersFooters)
      {
        if (headersFooter.Items.Count > 0)
          num += this.ReplaceSingleLine(pattern, replace, headersFooter.Items[0]);
      }
    }
    this.ResetSingleLineReplace();
    return num;
  }

  private void ResetSingleLineReplace()
  {
    this.ResetFindNext();
    TextFinder.Instance.SingleLinePCol.Clear();
  }

  private int ReplaceSingleLine(Regex pattern, string replace, TextBodyItem startItem)
  {
    int num = 0;
    for (TextSelection[] nextSingleLine = this.FindNextSingleLine(startItem, pattern); nextSingleLine != null; nextSingleLine = this.FindNextSingleLine(startItem, pattern))
    {
      this.m_nextParaItem = (ParagraphItem) null;
      TextReplacer.Instance.ReplaceSingleLine(nextSingleLine, replace);
      ++num;
      if (this.ReplaceFirst)
        break;
    }
    return num;
  }

  private bool IsFloatingItemsContainSameZIndexValue()
  {
    for (int index1 = 0; index1 < this.FloatingItems.Count; ++index1)
    {
      int zorder = this.FloatingItems[index1].GetZOrder();
      for (int index2 = 0; index2 < this.FloatingItems.Count; ++index2)
      {
        if (index1 != index2 && zorder == this.FloatingItems[index2].GetZOrder())
          return true;
      }
    }
    return false;
  }

  internal void SortByZIndex(bool isFromHTMLExport)
  {
    if (!isFromHTMLExport && !this.IsFloatingItemsContainSameZIndexValue())
      return;
    for (int index1 = 1; index1 < this.FloatingItems.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.FloatingItems.Count - 1; ++index2)
      {
        double num1 = Math.Abs((double) this.FloatingItems[index2 + 1].GetZOrder());
        double num2 = Math.Abs((double) this.FloatingItems[index2].GetZOrder());
        if (num1 < num2 || num1 == num2 && this.FloatingItems[index2 + 1].IsNeedToSortByItsPosition(this.FloatingItems[index2]))
        {
          Entity floatingItem = this.FloatingItems[index2];
          this.FloatingItems[index2] = this.FloatingItems[index2 + 1];
          this.FloatingItems[index2 + 1] = floatingItem;
        }
      }
    }
    this.SetZOrderPosition();
  }

  private void SetZOrderPosition()
  {
    int num = 1024 /*0x0400*/;
    foreach (Entity floatingItem in this.FloatingItems)
    {
      switch (floatingItem.EntityType)
      {
        case EntityType.Picture:
          (floatingItem as WPicture).OrderIndex = (floatingItem as WPicture).OrderIndex >= 0 ? num : -num;
          break;
        case EntityType.Shape:
        case EntityType.AutoShape:
          (floatingItem as Shape).ZOrderPosition = (floatingItem as Shape).ZOrderPosition >= 0 ? num : -num;
          break;
        case EntityType.TextBox:
          (floatingItem as WTextBox).TextBoxFormat.OrderIndex = (floatingItem as WTextBox).TextBoxFormat.OrderIndex >= 0 ? num : -num;
          if ((floatingItem as WTextBox).IsShape && (floatingItem as WTextBox).Shape != null)
          {
            (floatingItem as WTextBox).Shape.ZOrderPosition = (floatingItem as WTextBox).Shape.ZOrderPosition >= 0 ? num : -num;
            break;
          }
          break;
        case EntityType.XmlParaItem:
          (floatingItem as XmlParagraphItem).ZOrderIndex = (floatingItem as XmlParagraphItem).ZOrderIndex >= 0 ? num : -num;
          break;
        case EntityType.Chart:
          (floatingItem as WChart).ZOrderPosition = (floatingItem as WChart).ZOrderPosition >= 0 ? num : -num;
          break;
        case EntityType.OleObject:
          if ((floatingItem as WOleObject).OlePicture != null)
          {
            (floatingItem as WOleObject).OlePicture.OrderIndex = (floatingItem as WOleObject).OlePicture.OrderIndex >= 0 ? num : -num;
            break;
          }
          break;
        case EntityType.GroupShape:
          (floatingItem as GroupShape).ZOrderPosition = (floatingItem as GroupShape).ZOrderPosition >= 0 ? num : -num;
          break;
      }
      num += 1024 /*0x0400*/;
    }
  }

  public TextSelection FindNext(
    TextBodyItem startTextBodyItem,
    string given,
    bool caseSensitive,
    bool wholeWord)
  {
    Regex regex = FindUtils.StringToRegex(given, caseSensitive, wholeWord);
    return this.FindNext(startTextBodyItem, regex);
  }

  public TextSelection FindNext(TextBodyItem startBodyItem, Regex pattern)
  {
    if (startBodyItem == null)
      throw new ArgumentException("Start body item can't be null", nameof (startBodyItem));
    if (this.m_prevBodyItem == null)
      this.m_prevBodyItem = startBodyItem;
    else if (this.m_prevBodyItem != startBodyItem)
    {
      this.m_nextParaItem = (ParagraphItem) null;
      this.m_prevBodyItem = startBodyItem;
    }
    if (this.m_nextParaItem != null && this.m_nextParaItem.OwnerParagraph != null)
    {
      TextSelection next = this.FindNext(pattern);
      if (next != null)
      {
        next.GetAsOneRange();
        this.UpdateNextItem(next);
        return next;
      }
      startBodyItem = this.m_nextParaItem.OwnerParagraph.NextTextBodyItem;
      if (startBodyItem == null)
      {
        this.m_nextParaItem = (ParagraphItem) null;
        return (TextSelection) null;
      }
    }
    TextBodyItem textBodyItem = startBodyItem;
    do
    {
      TextSelection next = textBodyItem.Find(pattern);
      if (this.CheckSelection(next))
      {
        next.GetAsOneRange();
        this.UpdateNextItem(next);
        return next;
      }
      textBodyItem = textBodyItem.NextTextBodyItem;
    }
    while (textBodyItem != null);
    return (TextSelection) null;
  }

  private TextSelection FindNext(Regex pattern)
  {
    TextSelectionList all = this.m_nextParaItem.OwnerParagraph.FindAll(pattern);
    if (all.Count > 0)
    {
      int inOwnerCollection1 = this.m_nextParaItem.GetIndexInOwnerCollection();
      foreach (TextSelection textSel in (List<TextSelection>) all)
      {
        if (this.CheckSelection(textSel))
        {
          textSel.StartTextRange.GetIndexInOwnerCollection();
          int inOwnerCollection2 = textSel.EndTextRange.GetIndexInOwnerCollection();
          if (inOwnerCollection1 <= inOwnerCollection2)
            return textSel;
        }
      }
    }
    return (TextSelection) null;
  }

  private void UpdateNextItem(TextSelection selection)
  {
    this.m_nextParaItem = (ParagraphItem) null;
    WTextRange[] ranges = selection.GetRanges();
    if (ranges != null)
    {
      WTextRange wtextRange = ranges[ranges.Length - 1];
      if (wtextRange.NextSibling != null)
      {
        this.m_nextParaItem = wtextRange.NextSibling as ParagraphItem;
        return;
      }
    }
    TextBodyItem tbItem = (TextBodyItem) selection.OwnerParagraph;
    while (tbItem.NextTextBodyItem != null)
    {
      tbItem = tbItem.NextTextBodyItem;
      this.m_nextParaItem = this.GetNextItem(tbItem);
      if (this.m_nextParaItem != null)
        break;
    }
  }

  private ParagraphItem GetNextItem(TextBodyItem tbItem)
  {
    if (tbItem == null)
      return (ParagraphItem) null;
    if (tbItem is WTable)
    {
      foreach (WTableRow row in (CollectionImpl) (tbItem as WTable).Rows)
      {
        foreach (WTextBody cell in (CollectionImpl) row.Cells)
        {
          ParagraphItem nextItem = this.GetNextItem(cell);
          if (nextItem != null)
            return nextItem;
        }
      }
    }
    else if (tbItem is BlockContentControl)
    {
      ParagraphItem nextItem = this.GetNextItem((tbItem as BlockContentControl).TextBody);
      if (nextItem != null)
        return nextItem;
    }
    else
    {
      if ((tbItem as WParagraph).Items.Count > 0)
        return (tbItem as WParagraph).Items[0];
      if (tbItem.NextSibling != null)
        return this.GetNextItem(tbItem.NextSibling as TextBodyItem);
    }
    return (ParagraphItem) null;
  }

  private ParagraphItem GetNextItem(WTextBody textBody)
  {
    foreach (TextBodyItem tbItem in (CollectionImpl) textBody.Items)
    {
      ParagraphItem nextItem = this.GetNextItem(tbItem);
      if (tbItem != null)
        return nextItem;
    }
    return (ParagraphItem) null;
  }

  private bool CheckSelection(TextSelection textSel) => textSel != null && textSel.Count > 0;

  public TextSelection[] FindNextSingleLine(
    TextBodyItem startTextBodyItem,
    string given,
    bool caseSensitive,
    bool wholeWord)
  {
    Regex regex = FindUtils.StringToRegex(given, caseSensitive, wholeWord);
    return this.FindNextSingleLine(startTextBodyItem, regex);
  }

  public TextSelection[] FindNextSingleLine(TextBodyItem startBodyItem, Regex pattern)
  {
    if (startBodyItem == null)
      throw new ArgumentException("Start body item can't be null", nameof (startBodyItem));
    if (this.m_prevBodyItem == null)
      this.m_prevBodyItem = startBodyItem;
    else if (this.m_prevBodyItem != startBodyItem)
    {
      this.m_nextParaItem = (ParagraphItem) null;
      this.m_prevBodyItem = startBodyItem;
    }
    if (this.m_nextParaItem == null)
      this.m_nextParaItem = this.GetNextItem(startBodyItem);
    TextSelection[] nextSingleLine = this.FindNextSingleLine(pattern);
    if (nextSingleLine != null)
    {
      TextSelection selection = nextSingleLine[nextSingleLine.Length - 1];
      selection.GetAsOneRange();
      this.UpdateNextItem(selection);
      return nextSingleLine;
    }
    this.m_nextParaItem = (ParagraphItem) null;
    return (TextSelection[]) null;
  }

  private TextSelection[] FindNextSingleLine(Regex pattern)
  {
    if (this.m_nextParaItem == null)
      return (TextSelection[]) null;
    WParagraph ownerParagraph = this.m_nextParaItem.OwnerParagraph;
    int inOwnerCollection1 = ownerParagraph.GetIndexInOwnerCollection();
    if (inOwnerCollection1 == 0)
      TextFinder.Instance.SingleLinePCol.Clear();
    int inOwnerCollection2 = this.m_nextParaItem.GetIndexInOwnerCollection();
    TextSelection[] nextSingleLine = TextFinder.Instance.FindInItems(ownerParagraph, pattern, inOwnerCollection2, ownerParagraph.Items.Count - 1);
    if (nextSingleLine == null)
    {
      WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
      if (ownerTextBody != null)
        nextSingleLine = TextFinder.Instance.FindSingleLine(ownerTextBody, pattern, inOwnerCollection1 + 1, ownerTextBody.Items.Count - 1);
      if (nextSingleLine == null)
      {
        TextBodyItem nextTextBodyItem;
        for (nextTextBodyItem = ownerTextBody.Items[ownerTextBody.Items.Count - 1].NextTextBodyItem; nextTextBodyItem != null; nextTextBodyItem = nextTextBodyItem.NextTextBodyItem)
        {
          if (nextTextBodyItem.GetIndexInOwnerCollection() == 0)
            TextFinder.Instance.SingleLinePCol.Clear();
          this.m_nextParaItem = this.GetNextItem(nextTextBodyItem);
          if (this.m_nextParaItem != null)
            break;
        }
        if (nextTextBodyItem != null)
          nextSingleLine = this.FindNextSingleLine(pattern);
      }
    }
    return nextSingleLine;
  }

  public void ResetFindNext()
  {
    this.m_nextParaItem = (ParagraphItem) null;
    this.m_prevBodyItem = (TextBodyItem) null;
  }

  public ParagraphItem CreateParagraphItem(ParagraphItemType itemType)
  {
    switch (itemType)
    {
      case ParagraphItemType.TextRange:
        return (ParagraphItem) new WTextRange((IWordDocument) this);
      case ParagraphItemType.Picture:
        return (ParagraphItem) new WPicture((IWordDocument) this);
      case ParagraphItemType.Field:
        return (ParagraphItem) new WField((IWordDocument) this);
      case ParagraphItemType.FieldMark:
        return (ParagraphItem) new WFieldMark((IWordDocument) this);
      case ParagraphItemType.MergeField:
        return (ParagraphItem) new WMergeField((IWordDocument) this);
      case ParagraphItemType.CheckBox:
        return (ParagraphItem) new WCheckBox((IWordDocument) this);
      case ParagraphItemType.TextFormField:
        return (ParagraphItem) new WTextFormField((IWordDocument) this);
      case ParagraphItemType.DropDownFormField:
        return (ParagraphItem) new WDropDownFormField((IWordDocument) this);
      case ParagraphItemType.EmbedField:
        return (ParagraphItem) new WEmbedField((IWordDocument) this);
      case ParagraphItemType.BookmarkStart:
        return (ParagraphItem) new BookmarkStart(this);
      case ParagraphItemType.BookmarkEnd:
        return (ParagraphItem) new BookmarkEnd(this);
      case ParagraphItemType.ShapeObject:
        return (ParagraphItem) new ShapeObject((IWordDocument) this);
      case ParagraphItemType.InlineShapeObject:
        return (ParagraphItem) new InlineShapeObject((IWordDocument) this);
      case ParagraphItemType.Comment:
        return (ParagraphItem) new WComment((IWordDocument) this);
      case ParagraphItemType.Footnote:
        return (ParagraphItem) new WFootnote((IWordDocument) this);
      case ParagraphItemType.TextBox:
        return (ParagraphItem) new WTextBox((IWordDocument) this);
      case ParagraphItemType.Break:
        return (ParagraphItem) new Break((IWordDocument) this);
      case ParagraphItemType.Symbol:
        return (ParagraphItem) new WSymbol((IWordDocument) this);
      case ParagraphItemType.TOC:
        return (ParagraphItem) new TableOfContent((IWordDocument) this);
      case ParagraphItemType.Chart:
        return (ParagraphItem) new WChart(this);
      case ParagraphItemType.OleObject:
        return (ParagraphItem) new WOleObject(this);
      case ParagraphItemType.InlineContentControl:
        return (ParagraphItem) new InlineContentControl(this);
      case ParagraphItemType.Math:
        return (ParagraphItem) new WMath((IWordDocument) this);
      default:
        throw new ArgumentException("Invalid type of paragraph item");
    }
  }

  protected override object CloneImpl()
  {
    lock (WordDocument.m_threadLocker)
      return (object) new WordDocument(this);
  }

  protected internal WCharacterFormat CreateCharacterFormatImpl()
  {
    return new WCharacterFormat((IWordDocument) this);
  }

  protected internal ListStyle CreateListStyleImpl() => new ListStyle(this);

  protected internal WListLevel CreateListLevelImpl(ListStyle style) => new WListLevel(style);

  protected internal WParagraphFormat CreateParagraphFormatImpl()
  {
    return new WParagraphFormat((IWordDocument) this);
  }

  protected internal RowFormat CreateTableFormatImpl() => new RowFormat();

  protected internal CellFormat CreateCellFormatImpl() => new CellFormat();

  protected internal WTextBoxFormat CreateTextboxFormatImpl() => new WTextBoxFormat(this);

  protected internal WTextBoxCollection CreateTextBoxCollectionImpl()
  {
    return new WTextBoxCollection((IWordDocument) this);
  }

  protected internal WListFormat CreateListFormatImpl(IWParagraph owner) => new WListFormat(owner);

  internal ICompoundFile CreateCompoundFile() => (ICompoundFile) new Syncfusion.CompoundFile.DocIO.Net.CompoundFile();

  internal ICompoundFile CreateCompoundFile(Stream stream)
  {
    return (ICompoundFile) new Syncfusion.CompoundFile.DocIO.Net.CompoundFile(stream);
  }

  internal bool CheckForEncryption(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    bool flag = false;
    if (Syncfusion.CompoundFile.DocIO.Net.CompoundFile.CheckHeader(stream))
      flag = true;
    return flag;
  }

  internal void EnsureParagraphStyle(IWParagraph paragraph)
  {
    if (paragraph.StyleName != null)
      return;
    if (this.Styles.FindByName("Normal") == null)
      this.AddStyle(StyleType.ParagraphStyle, "Normal");
    (paragraph as WParagraph).ApplyStyle("Normal", false);
  }

  private void AddEmptyParagraph()
  {
    if (this.LastSection == null || this.LastSection.Body.ChildEntities.Count <= 0 || !(this.LastSection.Body.ChildEntities[this.LastSection.Body.ChildEntities.Count - 1] is WTable))
      return;
    this.LastSection.Body.AddParagraph();
  }

  internal void CloneShapeEscher(WordDocument destDoc, IParagraphItem shapeItem)
  {
    if (this.Escher == null)
      return;
    if (destDoc.m_escher == null)
      destDoc.m_escher = new EscherClass(destDoc);
    switch (shapeItem)
    {
      case null:
        return;
      case IWPicture _:
        this.ClonePictureContainer(destDoc, shapeItem as WPicture);
        break;
      case IWTextBox _:
        this.CloneTextBoxContainer(destDoc, shapeItem as WTextBox);
        break;
      case ShapeObject _:
        this.CloneAutoShapeContainer(destDoc, shapeItem as ShapeObject);
        break;
    }
    ++this.m_defShapeId;
  }

  internal void CloneProperties(
    Dictionary<string, Stream> sourceProps,
    ref Dictionary<string, Stream> destinationProps)
  {
    destinationProps = new Dictionary<string, Stream>();
    foreach (KeyValuePair<string, Stream> sourceProp in sourceProps)
      destinationProps.Add(sourceProp.Key, this.CloneStream(sourceProp.Value));
  }

  internal void CloneProperties(
    Dictionary<string, string> sourceProps,
    ref Dictionary<string, string> destinationProps)
  {
    destinationProps = new Dictionary<string, string>();
    foreach (KeyValuePair<string, string> sourceProp in sourceProps)
      destinationProps.Add(sourceProp.Key, sourceProp.Value);
  }

  internal void CloneProperties(List<Stream> sourceProps, ref List<Stream> destinationProps)
  {
    destinationProps = new List<Stream>();
    foreach (Stream sourceProp in sourceProps)
      destinationProps.Add(this.CloneStream(sourceProp));
  }

  internal Stream CloneStream(Stream input)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] buffer = new byte[input.Length];
    input.Seek(0L, SeekOrigin.Begin);
    int count = input.Read(buffer, 0, buffer.Length);
    if (count > 0)
      memoryStream.Write(buffer, 0, count);
    return (Stream) memoryStream;
  }

  internal string GetPasswordValue() => this.m_password;

  internal bool IsNeedToAddLineNumbers()
  {
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      if (section.LineNumbersEnabled())
        return true;
    }
    return false;
  }

  internal void InsertWatermark(WatermarkType type)
  {
    this.ResetWatermark();
    if (type != WatermarkType.NoWatermark && this.m_escher == null)
      this.m_escher = new EscherClass(this);
    if (type == WatermarkType.PictureWatermark)
      this.m_watermark = (Watermark) new PictureWatermark(this);
    else if (type == WatermarkType.TextWatermark)
      this.m_watermark = (Watermark) new TextWatermark(this);
    else
      this.m_watermark = new Watermark(this, type);
  }

  internal void ReadBackground()
  {
    if (this.m_escher == null)
      return;
    this.m_background = new Background(this);
  }

  internal void ToDetailedDlsStream(MemoryStream stream) => this.SaveXml((Stream) stream);

  internal bool HasListStyle() => this.m_listStyles != null && this.m_listStyles.Count > 0;

  internal void UpdateStartPosOfParaItems(ParagraphItem pItem, int offset)
  {
    int num = pItem.OwnerParagraph.Items.IndexOf((IEntity) pItem);
    if (!(pItem.Owner is InlineContentControl) && num < 0)
      throw new InvalidOperationException("pItem haven't found in paragraph items");
    int index = num + 1;
    for (int count = pItem.OwnerParagraph.Items.Count; index < count; ++index)
      this.UpdateStartPos(pItem.OwnerParagraph.Items[index], offset);
  }

  internal void UpdateStartPos(ParagraphItem item, int offset)
  {
    if (item is InlineContentControl)
      this.UpdateStartPosOfInlineContentControlItems(item as InlineContentControl, 0, offset);
    if (item == null)
      return;
    item.StartPos += offset;
  }

  internal void UpdateStartPosOfInlineContentControlItems(
    InlineContentControl inlineContentControl,
    int index,
    int offset)
  {
    for (; index < inlineContentControl.ParagraphItems.Count; ++index)
      this.UpdateStartPos(inlineContentControl.ParagraphItems[index], offset);
  }

  internal bool IsDOCX()
  {
    return this.ActualFormatType == FormatType.Docx || this.ActualFormatType == FormatType.Word2007 || this.ActualFormatType == FormatType.Word2010 || this.ActualFormatType == FormatType.Word2013 || this.ActualFormatType == FormatType.Word2007Dotx || this.ActualFormatType == FormatType.Word2010Dotx || this.ActualFormatType == FormatType.Word2013Dotx;
  }

  private void InitDefaultParagraphFormat()
  {
    this.m_defParaFormat = new WParagraphFormat((IWordDocument) this);
    if (!(this.Styles.FindByName("Normal", StyleType.ParagraphStyle) is WParagraphStyle byName))
      return;
    this.m_defParaFormat.ImportContainer((FormatBase) byName.ParagraphFormat);
    this.m_defParaFormat.CopyProperties((FormatBase) byName.ParagraphFormat);
  }

  private void Init()
  {
    this.IsNormalStyleDefined = false;
    this.IsDefaultParagraphFontStyleDefined = false;
    this.CloseStyles();
    if (this.m_mailMerge != null)
    {
      this.m_mailMerge.Close();
      this.m_mailMerge = (MailMerge) null;
    }
    if (this.m_viewSetup != null)
    {
      this.m_viewSetup.Close();
      this.m_viewSetup = (ViewSetup) null;
    }
    if (this.m_builtinProp != null)
    {
      this.m_builtinProp.Close();
      this.m_builtinProp = (BuiltinDocumentProperties) null;
    }
    if (this.m_customProp != null)
    {
      this.m_customProp.Close();
      this.m_customProp = (CustomDocumentProperties) null;
    }
    if (this.m_txbxItems != null)
    {
      this.m_txbxItems.Close();
      this.m_txbxItems = (TextBoxCollection) null;
    }
    if (this.m_background != null)
    {
      this.m_background.Close();
      this.m_background = (Background) null;
    }
    if (this.m_dop != null)
      this.m_dop = (DOPDescriptor) null;
    if (this.m_sections != null)
    {
      this.m_sections.Close();
      this.m_sections = (WSectionCollection) null;
    }
    if (this.m_bookmarks != null)
    {
      this.m_bookmarks.Close();
      this.m_bookmarks = (BookmarkCollection) null;
    }
    if (this.m_editableRanges != null)
    {
      this.m_editableRanges.Close();
      this.m_editableRanges = (EditableRangeCollection) null;
    }
    if (this.m_revisions != null)
    {
      this.m_revisions.Close();
      this.m_revisions = (RevisionCollection) null;
    }
    this.DocHasThemes = false;
    if (this.m_themes != null)
      this.m_themes = (Themes) null;
    if (this.m_watermark != null)
    {
      this.m_watermark.Close();
      this.m_watermark = (Watermark) null;
    }
    if (this.m_styleNameIds != null)
      this.m_styleNameIds.Clear();
    if (this.m_settings != null)
      this.m_settings.CompatibilityOptions.PropertiesHash.Clear();
    if (this.m_usedFonts != null)
      this.m_usedFonts.Clear();
    if (this.m_fontSubstitutionTable != null)
      this.m_fontSubstitutionTable.Clear();
    if (this.m_docxProps != null)
      this.m_docxProps.Clear();
    if (this.m_Comments != null)
      this.m_Comments.Clear();
    if (this.m_CommentsEx != null)
    {
      this.m_CommentsEx.Close();
      this.m_Comments = (CommentsCollection) null;
    }
    if (this.m_fields != null)
    {
      if (this.m_fields.m_sortedAutoNumFields != null)
        this.m_fields.m_sortedAutoNumFields.Clear();
      if (this.m_fields.m_sortedAutoNumFieldIndexes != null)
        this.m_fields.m_sortedAutoNumFieldIndexes.Clear();
      this.m_fields.Clear();
    }
    this.m_htmlValidationOption = XHTMLValidationType.Transitional;
    this.ThrowExceptionsForUnsupportedElements = false;
    this.HasPicture = false;
    this.WriteProtected = false;
    this.UpdateFields = false;
    this.IsEncrypted = false;
    this.ReplaceFirst = false;
    this.IsReadOnly = false;
    this.ImportStyles = true;
    this.m_defShapeId = 1;
    this.m_tableOfContent = (Dictionary<WField, TableOfContent>) null;
    this.m_latentStyles2010 = (MemoryStream) null;
    this.m_latentStyles = (XmlNode) null;
    this.m_standardBidiFont = (string) null;
    this.m_standardNonFarEastFont = (string) null;
    this.m_standardFarEastFont = (string) null;
    this.m_standardAsciiFont = (string) null;
    this.m_macroCommands = (byte[]) null;
    this.m_macrosData = (byte[]) null;
    this.m_password = (string) null;
    this.m_assocStrings = (SttbfAssoc) null;
    this.m_saveOptions = (SaveOptions) null;
    this.m_prevBodyItem = (TextBodyItem) null;
    this.m_nextParaItem = (ParagraphItem) null;
    this.m_props = (DocProperties) null;
    this.m_variables = (DocVariables) null;
    this.m_docxPackage = (Package) null;
    this.m_grammarSpellingData = (GrammarSpelling) null;
    if (this.m_revisionOptions != null)
      this.m_revisionOptions = (RevisionOptions) null;
    if (this.m_fontSettings != null)
    {
      this.m_fontSettings.Close();
      this.m_fontSettings = (FontSettings) null;
    }
    if (this.m_imageCollection != null)
    {
      this.m_imageCollection.Clear();
      this.m_imageCollection = (ImageCollection) null;
    }
    if (this.m_defParaFormat != null)
    {
      this.m_defParaFormat.Close();
      this.m_defParaFormat = (WParagraphFormat) null;
    }
    if (this.m_defCharFormat != null)
    {
      this.m_defCharFormat.Close();
      this.m_defCharFormat = (WCharacterFormat) null;
    }
    if (this.m_escher != null)
    {
      this.m_escher.Close();
      this.m_escher = (EscherClass) null;
    }
    this.RemoveMacros();
    if (this.ClonedFields != null)
      this.ClonedFields.Clear();
    if (this.m_FloatingItems != null)
    {
      this.m_FloatingItems.Clear();
      this.m_FloatingItems = (List<Entity>) null;
    }
    this.m_notSupportedElementFlag = 0;
    this.m_supportedElementFlag_1 = 0;
    this.m_supportedElementFlag_2 = 0;
    this.ResetSingleLineReplace();
    TextFinder.Close();
    MemoryConverter.Close();
    UnitsConvertor.Close();
  }

  internal bool IsInternalManipulation()
  {
    return this.IsOpening || this.IsCloning || this.IsHTMLImport || this.IsSkipFieldDetach;
  }

  private void UpdateImportOption()
  {
    ImportOptions importOptions = ImportOptions.UseDestinationStyles;
    if ((this.m_importOption & ImportOptions.UseDestinationStyles) != (ImportOptions) 0)
      importOptions = ImportOptions.UseDestinationStyles;
    else if ((this.m_importOption & ImportOptions.MergeFormatting) != (ImportOptions) 0)
      importOptions = ImportOptions.MergeFormatting;
    else if ((this.m_importOption & ImportOptions.KeepTextOnly) != (ImportOptions) 0)
      importOptions = ImportOptions.KeepTextOnly;
    else if ((this.m_importOption & ImportOptions.KeepSourceFormatting) != (ImportOptions) 0)
      importOptions = ImportOptions.KeepSourceFormatting;
    if ((this.m_importOption & ImportOptions.ListContinueNumbering) != (ImportOptions) 0)
      importOptions |= ImportOptions.ListContinueNumbering;
    else if ((this.m_importOption & ImportOptions.ListRestartNumbering) != (ImportOptions) 0)
      importOptions |= ImportOptions.ListRestartNumbering;
    this.m_importOption = importOptions;
  }

  private void CopyBinaryData(byte[] srcData, ref byte[] destData)
  {
    if (srcData == null)
      return;
    destData = new byte[srcData.Length];
    srcData.CopyTo((Array) destData, 0);
  }

  private void ClonePictureContainer(WordDocument destDoc, WPicture picture)
  {
    int shapeId = picture.ShapeId;
    if (!this.CheckContainer(EscherShapeType.msosptPictureFrame, shapeId))
      return;
    WordSubdocument docType = picture.IsHeaderPicture ? WordSubdocument.HeaderFooter : WordSubdocument.Main;
    this.m_defShapeId = this.Escher.CloneContainerBySpid(destDoc, docType, shapeId, this.m_defShapeId);
    if (this.m_defShapeId == -1)
      return;
    picture.ShapeId = this.m_defShapeId;
  }

  private void CloneTextBoxContainer(WordDocument destDoc, WTextBox textBox)
  {
    int textBoxShapeId = textBox.TextBoxFormat.TextBoxShapeID;
    if (!this.CheckContainer(EscherShapeType.msosptTextBox, textBoxShapeId))
      return;
    WordSubdocument docType = textBox.TextBoxFormat.IsHeaderTextBox ? WordSubdocument.HeaderFooter : WordSubdocument.Main;
    this.m_defShapeId = this.Escher.CloneContainerBySpid(destDoc, docType, textBoxShapeId, this.m_defShapeId);
    if (this.m_defShapeId == -1)
      return;
    textBox.TextBoxFormat.TextBoxShapeID = this.m_defShapeId;
  }

  private void CloneAutoShapeContainer(WordDocument destDoc, ShapeObject shapeObj)
  {
    int spid = shapeObj.FSPA.Spid;
    WordSubdocument docType = shapeObj.IsHeaderAutoShape ? WordSubdocument.HeaderFooter : WordSubdocument.Main;
    this.m_defShapeId = this.Escher.CloneContainerBySpid(destDoc, docType, spid, this.m_defShapeId);
    if (this.m_defShapeId == -1)
      return;
    shapeObj.FSPA.Spid = this.m_defShapeId;
  }

  private bool CheckContainer(EscherShapeType type, int spid)
  {
    if (this.Escher.Containers.ContainsKey(spid) && (!(this.Escher.Containers[spid] is MsofbtSpContainer) || (this.Escher.Containers[spid] as MsofbtSpContainer).Shape.ShapeType == type))
      return true;
    this.m_defShapeId = -1;
    return false;
  }

  private Image GetBackGndImage()
  {
    return this.Background.Type == BackgroundType.Picture ? this.Background.Picture : (Image) null;
  }

  private void SetBackgroundImageValue(Image image)
  {
    this.Background.Picture = image;
    this.Background.Type = BackgroundType.Picture;
  }

  private bool HasTrackedChanges()
  {
    if (this.m_sections == null || this.m_sections.Count == 0)
      return false;
    foreach (WSection section in (CollectionImpl) this.m_sections)
    {
      if (section.HasTrackedChanges())
        return true;
    }
    return false;
  }

  internal bool IsSecurityGranted()
  {
    SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
    bool flag = false;
    try
    {
      securityPermission.Demand();
      flag = true;
    }
    catch (SecurityException ex)
    {
    }
    return flag;
  }

  private string CheckExtension(string fileName, FormatType formatType)
  {
    FileInfo fileInfo = new FileInfo(fileName);
    if (!fileInfo.Exists && formatType != FormatType.Html)
    {
      string extension = fileInfo.Extension;
      if (extension != formatType.ToString())
      {
        if (extension != string.Empty)
        {
          int startIndex = fileName.LastIndexOf(extension);
          fileName = fileName.Remove(startIndex);
        }
        fileName = $"{fileName}.{formatType.ToString()}";
      }
    }
    return fileName;
  }

  private void CheckFileName(string fileName)
  {
    bool flag = false;
    if (fileName.Length >= 260)
      flag = true;
    else if (Path.GetDirectoryName(fileName).Length >= 248)
      flag = true;
    if (flag)
      throw new PathTooLongException("The file name is too long. The fully qualified file name must be less than 260 characters and the directory name must be less than 248 characters");
  }

  private void ResetWatermark()
  {
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      section.HeadersFooters.EvenHeader.WriteWatermark = false;
      section.HeadersFooters.OddHeader.WriteWatermark = false;
      section.HeadersFooters.FirstPageHeader.WriteWatermark = false;
    }
    if (this.m_watermark == null || !(this.m_watermark is PictureWatermark) || (this.m_watermark as PictureWatermark).WordPicture == null || (this.m_watermark as PictureWatermark).WordPicture.Document == null || (this.m_watermark as PictureWatermark).WordPicture.ImageRecord == null)
      return;
    --(this.m_watermark as PictureWatermark).WordPicture.ImageRecord.OccurenceCount;
  }

  internal void UpdateHeaderWatermark(Watermark watermark)
  {
    foreach (WSection section in (CollectionImpl) this.Sections)
    {
      section.HeadersFooters.EvenHeader.WriteWatermark = true;
      section.HeadersFooters.OddHeader.WriteWatermark = true;
      section.HeadersFooters.FirstPageHeader.WriteWatermark = true;
      section.HeadersFooters.EvenHeader.Watermark = watermark;
      section.HeadersFooters.OddHeader.Watermark = watermark;
      section.HeadersFooters.FirstPageHeader.Watermark = watermark;
    }
  }

  private void SetProtection(ProtectionType type)
  {
    if (type != ProtectionType.AllowOnlyFormFields)
      return;
    foreach (WSection section in (CollectionImpl) this.Sections)
      section.ProtectForm = true;
  }

  private void WriteXml(XmlWriter writer)
  {
    new XDLSWriter(writer).Serialize((IXDLSSerializable) this);
  }

  private void ReadXml(XmlReader reader)
  {
    new XDLSReader(reader).Deserialize((IXDLSSerializable) this);
  }

  XmlSchema IXmlSerializable.GetSchema() => this.GetSchema();

  void IXmlSerializable.ReadXml(XmlReader reader) => this.ReadXml(reader);

  void IXmlSerializable.WriteXml(XmlWriter writer) => this.WriteXml(writer);

  protected XmlSchema GetSchema() => DocIOXsdGenerator.GetDocIOLocalSchema();

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("styles", (object) this.Styles);
    this.XDLSHolder.AddElement("liststyles", (object) this.ListStyles);
    this.XDLSHolder.AddElement("sections", (object) this.Sections);
    this.XDLSHolder.AddElement("view-setup", (object) this.ViewSetup);
    this.XDLSHolder.AddElement("builtin-properties", (object) this.BuiltinDocumentProperties);
    this.XDLSHolder.AddElement("custom-properties", (object) this.CustomDocumentProperties);
    this.XDLSHolder.AddElement("list-overrides", (object) this.ListOverrides);
    this.XDLSHolder.AddElement("background", (object) this.Background);
    this.XDLSHolder.AddElement("watermark", (object) this.Watermark);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.MacrosData != null)
      writer.WriteChildBinaryElement("macros", this.MacrosData);
    if (this.MacroCommands != null)
      writer.WriteChildBinaryElement("macros-commands", this.MacroCommands);
    if (this.m_escher != null)
    {
      MemoryStream memoryStream1 = new MemoryStream();
      this.m_escher.WriteContainersData((Stream) memoryStream1);
      this.m_escherDataContainers = memoryStream1.ToArray();
      writer.WriteChildBinaryElement("escher-data", this.m_escherDataContainers);
      memoryStream1.Close();
      MemoryStream memoryStream2 = new MemoryStream();
      int num = (int) this.m_escher.WriteContainers((Stream) memoryStream2);
      this.m_escherContainers = memoryStream2.ToArray();
      writer.WriteChildBinaryElement("escher-containers", this.m_escherContainers);
      memoryStream2.Close();
      this.m_escherDataContainers = (byte[]) null;
      this.m_escherContainers = (byte[]) null;
    }
    if (this.m_dop != null)
    {
      MemoryStream memoryStream = new MemoryStream();
      int num = (int) this.m_dop.Write((Stream) memoryStream);
      byte[] array = memoryStream.ToArray();
      writer.WriteChildBinaryElement("dop-internal", array);
    }
    if (this.m_grammarSpellingData == null || this.GrammarSpellingData.PlcfgramData == null || this.GrammarSpellingData.PlcfsplData == null)
      return;
    writer.WriteChildBinaryElement("grammar-data", this.GrammarSpellingData.PlcfgramData);
    writer.WriteChildBinaryElement("spelling-data", this.GrammarSpellingData.PlcfsplData);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    if (reader.TagName == "macros")
      this.MacrosData = reader.ReadChildBinaryElement();
    if (reader.TagName == "macros-commands")
      this.MacroCommands = reader.ReadChildBinaryElement();
    if (reader.TagName == "escher-containers")
      this.m_escherContainers = reader.ReadChildBinaryElement();
    if (reader.TagName == "escher-data")
      this.m_escherDataContainers = reader.ReadChildBinaryElement();
    if (this.m_escherDataContainers != null && this.m_escherContainers != null)
    {
      MemoryStream docStream = new MemoryStream(this.m_escherDataContainers, 0, this.m_escherDataContainers.Length);
      MemoryStream tableStream = new MemoryStream(this.m_escherContainers, 0, this.m_escherContainers.Length);
      this.m_escher = new EscherClass((Stream) tableStream, (Stream) docStream, 0L, (long) (int) tableStream.Length, this);
      docStream.Close();
      tableStream.Close();
      this.m_escherDataContainers = (byte[]) null;
      this.m_escherContainers = (byte[]) null;
    }
    if (reader.TagName == "dop-internal")
    {
      MemoryStream memoryStream = new MemoryStream(reader.ReadChildBinaryElement());
      this.m_dop = new DOPDescriptor((Stream) memoryStream, 0, (int) memoryStream.Length, false);
      memoryStream.Close();
    }
    if (this.m_grammarSpellingData == null)
      this.m_grammarSpellingData = new GrammarSpelling();
    if (reader.TagName == "grammar-data")
      this.m_grammarSpellingData.PlcfgramData = reader.ReadChildBinaryElement();
    if (reader.TagName == "spelling-data")
      this.m_grammarSpellingData.PlcfsplData = reader.ReadChildBinaryElement();
    return base.ReadXmlContent(reader);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.m_standardAsciiFont != null)
      writer.WriteValue("StandardAscii", this.m_standardAsciiFont);
    if (this.m_standardFarEastFont != null)
      writer.WriteValue("StandardFarEast", this.m_standardFarEastFont);
    if (this.m_standardNonFarEastFont != null)
      writer.WriteValue("StandardNonFarEast", this.m_standardNonFarEastFont);
    if (this.Watermark == null || this.Watermark.Type == WatermarkType.NoWatermark)
      return;
    writer.WriteValue("WatermarkType", (Enum) this.Watermark.Type);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("StandardAscii"))
      this.m_standardAsciiFont = reader.ReadString("StandardAscii");
    if (reader.HasAttribute("StandardFarEast"))
      this.m_standardFarEastFont = reader.ReadString("StandardFarEast");
    if (reader.HasAttribute("StandardNonFarEast"))
      this.m_standardNonFarEastFont = reader.ReadString("StandardNonFarEast");
    if (!reader.HasAttribute("WatermarkType"))
      return;
    this.InsertWatermark((WatermarkType) reader.ReadEnum("WatermarkType", typeof (WatermarkType)));
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Vertical);
  }

  protected override IEntityCollectionBase WidgetCollection
  {
    get => (IEntityCollectionBase) this.Sections;
  }

  internal byte[] SttbfRMark
  {
    get => this.m_sttbfRMark;
    set => this.m_sttbfRMark = value;
  }

  public void RemoveMacros()
  {
    if (this.VbaProject != null)
    {
      this.VbaProject.Close();
      this.VbaProject = (Stream) null;
    }
    if (this.VbaProjectSignature != null)
    {
      this.VbaProjectSignature.Close();
      this.VbaProjectSignature = (Stream) null;
    }
    if (this.VbaProjectSignatureAgile != null)
    {
      this.VbaProjectSignatureAgile.Close();
      this.VbaProjectSignatureAgile = (Stream) null;
    }
    this.VbaData.Clear();
    this.DocEvents.Clear();
  }

  internal void SetTriggerElement(ref int flag, int bitPosition)
  {
    if (this.HasElement(flag, bitPosition))
      return;
    flag |= 1 << bitPosition;
  }

  internal bool HasElement(int flag, int bitPosition) => (flag & 1 << bitPosition) != 0;

  internal void ParagraphItemRevision(
    ParagraphItem item,
    RevisionType revisionType,
    string revAuthorName,
    DateTime revDateTime,
    string name,
    bool isNestedRevision,
    Revision moveRevision,
    Revision contentRevision,
    Stack<Revision> m_trackchangeRevisionDetails)
  {
    Revision revision1 = (Revision) null;
    bool isChildRevision = false;
    Revision rowRevision = (Revision) null;
    WParagraph ownerParagraphValue = item.GetOwnerParagraphValue();
    if (ownerParagraphValue != null && ownerParagraphValue.IsInCell && (ownerParagraphValue.GetOwnerEntity() as WTableCell).OwnerRow.RowFormat.Revisions.Count > 0)
      revision1 = this.GetRowRevision(item, revisionType, revAuthorName, ref isChildRevision, ref rowRevision);
    if (moveRevision != null && moveRevision.RevisionType == revisionType && moveRevision.Author == revAuthorName)
      revision1 = moveRevision;
    if (contentRevision != null && contentRevision.RevisionType == revisionType && contentRevision.Author == revAuthorName)
      revision1 = contentRevision;
    if (revision1 == null && !this.HasRenderableItemBefore(item))
      revision1 = this.GetRevisionForFirstParaItem(item, isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision);
    else if (revision1 == null)
      revision1 = this.GetRevisionForRemainingParaItem(item, isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision);
    if (revision1 != null)
    {
      bool flag = false;
      foreach (Revision revision2 in item.RevisionsInternal)
      {
        if (revision2.Author == revision1.Author && revision2.RevisionType == revision1.RevisionType && revision2.Date == revision1.Date)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        int num = item.PreviousSibling != null ? revision1.Range.Items.IndexOf((object) item.PreviousSibling) : -1;
        revision1.Range.Items.Insert(num != -1 ? num + 1 : revision1.Range.Items.Count, (object) item);
        item.RevisionsInternal.Add(revision1);
      }
      else if (!revision1.Range.Items.Contains((object) item))
      {
        int num = item.PreviousSibling != null ? revision1.Range.Items.IndexOf((object) item.PreviousSibling) : -1;
        revision1.Range.Items.Insert(num != -1 ? num + 1 : revision1.Range.Items.Count, (object) item);
      }
    }
    if (isNestedRevision)
      return;
    foreach (Revision trackchangeRevisionDetail in m_trackchangeRevisionDetails)
    {
      if (trackchangeRevisionDetail.RevisionType == RevisionType.Insertions || trackchangeRevisionDetail.RevisionType == RevisionType.MoveTo)
        item.SetInsertRev(true, trackchangeRevisionDetail.Author, trackchangeRevisionDetail.Date);
      else if (trackchangeRevisionDetail.RevisionType == RevisionType.Deletions || trackchangeRevisionDetail.RevisionType == RevisionType.MoveFrom)
        item.SetDeleteRev(true, trackchangeRevisionDetail.Author, trackchangeRevisionDetail.Date);
      this.ParagraphItemRevision(item, trackchangeRevisionDetail.RevisionType, trackchangeRevisionDetail.Author, trackchangeRevisionDetail.Date, trackchangeRevisionDetail.Name, true, moveRevision, contentRevision, m_trackchangeRevisionDetails);
    }
  }

  private Revision GetRevisionForRemainingParaItem(
    ParagraphItem item,
    bool isChildRevision,
    RevisionType revisionType,
    string revAuthorName,
    DateTime revDateTime,
    string name,
    Revision rowRevision,
    Revision moveRevision)
  {
    Revision remainingParaItem = (Revision) null;
    if (item.PreviousSibling != null && item.PreviousSibling is Entity)
    {
      Entity entity = item.PreviousSibling as Entity;
label_2:
      entity = entity is InlineContentControl ? ((entity as InlineContentControl).ParagraphItems.Count > 0 ? (entity as InlineContentControl).ParagraphItems.LastItem : entity) : entity;
      while (true)
      {
        switch (entity)
        {
          case BookmarkStart _:
          case BookmarkEnd _:
          case EditableRangeStart _:
          case EditableRangeEnd _:
            entity = entity.PreviousSibling as Entity;
            continue;
          case InlineContentControl _:
            goto label_2;
          case WFieldMark _ when (entity as WFieldMark).Type == FieldMarkType.FieldEnd:
            goto label_5;
          default:
            goto label_6;
        }
      }
label_5:
      entity = (Entity) (entity as WFieldMark).ParentField;
label_6:
      if (entity != null && entity.RevisionsInternal.Count > 0)
        remainingParaItem = this.GetExistingRevision(entity.RevisionsInternal, revisionType, revAuthorName);
      if (remainingParaItem == null)
        remainingParaItem = this.CreateRevision(isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision, (Entity) item);
    }
    else
      remainingParaItem = this.CreateRevision(isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision, (Entity) item);
    return remainingParaItem;
  }

  private Revision GetRevisionForFirstParaItem(
    ParagraphItem item,
    bool isChildRevision,
    RevisionType revisionType,
    string revAuthorName,
    DateTime revDateTime,
    string name,
    Revision rowRevision,
    Revision moveRevision)
  {
    Revision forFirstParaItem = (Revision) null;
    if (item.Owner is WParagraph && item.Owner.PreviousSibling != null)
    {
      Entity previousSibling = item.Owner.PreviousSibling as Entity;
      switch (previousSibling)
      {
        case WParagraph _:
          if (previousSibling is WParagraph wparagraph && wparagraph.BreakCharacterFormat.Revisions.Count > 0)
            forFirstParaItem = this.GetExistingRevision(wparagraph.BreakCharacterFormat.Revisions, revisionType, revAuthorName);
          if (forFirstParaItem == null)
          {
            forFirstParaItem = this.CreateRevision(isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision, (Entity) item);
            break;
          }
          break;
        case WTable _:
          if (previousSibling is WTable wtable && wtable.LastRow != null && wtable.LastRow.RowFormat.Revisions.Count > 0)
            forFirstParaItem = this.GetExistingRevision(wtable.LastRow.RowFormat.Revisions, revisionType, revAuthorName);
          if (forFirstParaItem == null)
          {
            forFirstParaItem = this.CreateRevision(isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision, (Entity) item);
            break;
          }
          break;
        default:
          forFirstParaItem = this.CreateRevision(isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision, (Entity) item);
          break;
      }
    }
    else
      forFirstParaItem = this.CreateRevision(isChildRevision, revisionType, revAuthorName, revDateTime, name, rowRevision, moveRevision, (Entity) item);
    return forFirstParaItem;
  }

  private Revision GetRowRevision(
    ParagraphItem item,
    RevisionType revisionType,
    string revAuthorName,
    ref bool isChildRevision,
    ref Revision rowRevision)
  {
    foreach (Revision revision in (item.GetOwnerParagraphValue().OwnerTextBody as WTableCell).OwnerRow.RowFormat.Revisions)
    {
      if (revision.RevisionType == revisionType)
      {
        if (revision.Author == revAuthorName)
          return revision;
        isChildRevision = true;
        rowRevision = revision;
      }
    }
    return (Revision) null;
  }

  private Revision GetExistingRevision(
    List<Revision> revisions,
    RevisionType revisionType,
    string revAuthorName)
  {
    foreach (Revision revision in revisions)
    {
      if (revision.RevisionType == revisionType && revision.Author == revAuthorName)
        return revision;
    }
    return (Revision) null;
  }

  private Revision CreateRevision(
    bool isChildRevision,
    RevisionType revisionType,
    string revAuthorName,
    DateTime revDateTime,
    string name,
    Revision rowRevision,
    Revision moveRevision,
    Entity item)
  {
    Revision revision;
    if (isChildRevision)
    {
      revision = this.CreateNewChildRevision(revisionType, revAuthorName, revDateTime, name);
      rowRevision.ChildRevisions.Add(revision);
    }
    else if (moveRevision != null)
    {
      if (moveRevision.RevisionType != revisionType)
      {
        revision = this.CreateNewRevision(revisionType, revAuthorName, revDateTime, name);
      }
      else
      {
        revision = this.CreateNewChildRevision(revisionType, revAuthorName, revDateTime, name);
        moveRevision.ChildRevisions.Add(revision);
      }
      moveRevision.Range.Items.Add((object) item);
    }
    else
      revision = this.CreateNewRevision(revisionType, revAuthorName, revDateTime, name);
    return revision;
  }

  internal void TableRowRevision(
    RevisionType revisionType,
    WTableRow tableRow,
    WordReaderBase reader)
  {
    Revision revision = (Revision) null;
    string str = this.ActualFormatType == FormatType.Doc ? this.Document.GetAuthorName(reader, revisionType == RevisionType.Insertions) : (revisionType == RevisionType.Formatting ? tableRow.RowFormat.FormatChangeAuthorName : tableRow.CharacterFormat.AuthorName);
    DateTime dateTime = this.ActualFormatType == FormatType.Doc ? this.Document.GetDateTime(reader, revisionType == RevisionType.Insertions, tableRow.CharacterFormat) : (revisionType == RevisionType.Formatting ? tableRow.RowFormat.FormatChangeDateTime : tableRow.CharacterFormat.RevDateTime);
    if (tableRow.OwnerTable.IsInCell && (tableRow.OwnerTable.OwnerTextBody as WTableCell).OwnerRow.RowFormat.Revisions.Count > 0)
      this.LinkNestedTableRowRevision(revisionType, tableRow, str, dateTime);
    else
      revision = tableRow.PreviousSibling == null || (tableRow.PreviousSibling as WTableRow).RowFormat.Revisions.Count <= 0 ? (tableRow.PreviousSibling != null || tableRow.OwnerTable.PreviousSibling == null || !(tableRow.OwnerTable.PreviousSibling is WParagraph) || (tableRow.OwnerTable.PreviousSibling as WParagraph).BreakCharacterFormat.Revisions.Count <= 0 ? (tableRow.PreviousSibling != null || tableRow.OwnerTable.PreviousSibling == null || !(tableRow.OwnerTable.PreviousSibling is WTable) || (tableRow.OwnerTable.PreviousSibling as WTable).LastRow.RowFormat.Revisions.Count <= 0 ? this.CreateNewRevision(revisionType, str, dateTime, (string) null) : this.GetExistingRevision((tableRow.OwnerTable.PreviousSibling as WTable).LastRow.RowFormat.Revisions, revisionType, str) ?? this.CreateNewRevision(revisionType, str, dateTime, (string) null)) : this.GetExistingRevision((tableRow.OwnerTable.PreviousSibling as WParagraph).BreakCharacterFormat.Revisions, revisionType, str) ?? this.CreateNewRevision(revisionType, str, dateTime, (string) null)) : this.GetExistingRevision((tableRow.PreviousSibling as WTableRow).RowFormat.Revisions, revisionType, str) ?? this.CreateNewRevision(revisionType, str, dateTime, (string) null);
    if (revision == null)
      return;
    tableRow.RowFormat.Revisions.Add(revision);
    revision.Range.InnerList.Add((object) tableRow.RowFormat);
  }

  private void LinkNestedTableRowRevision(
    RevisionType revisionType,
    WTableRow tableRow,
    string revAuthorName,
    DateTime revDateTime)
  {
    foreach (Revision revision in (tableRow.OwnerTable.OwnerTextBody as WTableCell).OwnerRow.RowFormat.Revisions)
    {
      if (revision.RevisionType == revisionType)
      {
        if (revision.Author == revAuthorName)
        {
          tableRow.RowFormat.Revisions.Add(revision);
          revision.Range.Items.Add((object) tableRow.RowFormat);
        }
        else
        {
          Revision newChildRevision = this.CreateNewChildRevision(revisionType, revAuthorName, revDateTime, (string) null);
          tableRow.RowFormat.Revisions.Add(newChildRevision);
          newChildRevision.Range.Items.Add((object) tableRow.RowFormat);
          revision.ChildRevisions.Add(newChildRevision);
        }
      }
      else
      {
        Revision newRevision = this.CreateNewRevision(revisionType, revAuthorName, revDateTime, (string) null);
        tableRow.RowFormat.Revisions.Add(newRevision);
        newRevision.Range.InnerList.Add((object) tableRow.RowFormat);
      }
    }
  }

  internal Revision CreateNewRevision(
    RevisionType revisionType,
    string authorName,
    DateTime dateTime,
    string name)
  {
    Revision revision = new Revision(this.m_doc);
    revision.RevisionType = revisionType;
    if (!string.IsNullOrEmpty(authorName))
      revision.Author = authorName;
    if (dateTime.Year > 1900)
      revision.Date = dateTime;
    if (!string.IsNullOrEmpty(name))
      revision.Name = name;
    this.Revisions.Add(revision);
    return revision;
  }

  internal Revision CreateNewChildRevision(
    RevisionType revisionType,
    string authorName,
    DateTime dateTime,
    string name)
  {
    Revision newChildRevision = new Revision(this.m_doc);
    newChildRevision.RevisionType = revisionType;
    if (!string.IsNullOrEmpty(authorName))
      newChildRevision.Author = authorName;
    if (dateTime.Year > 1900)
      newChildRevision.Date = dateTime;
    if (!string.IsNullOrEmpty(name))
      newChildRevision.Name = name;
    return newChildRevision;
  }

  internal void BreakCharacterFormatRevision(
    RevisionType revisionType,
    WCharacterFormat charFormat,
    Revision moveRevision,
    WordReaderBase reader)
  {
    string authorName = charFormat.AuthorName;
    DateTime dateTime = charFormat.RevDateTime;
    string revisionName = charFormat.RevisionName;
    if (reader != null)
    {
      authorName = this.GetAuthorName(reader, revisionType == RevisionType.Insertions);
      dateTime = this.GetDateTime(reader, revisionType == RevisionType.Insertions, charFormat);
    }
    if (moveRevision != null)
      this.LinkMoveRevForBreakCharacterFormat(charFormat, moveRevision, revisionType, authorName, dateTime, revisionName);
    else if (charFormat.OwnerBase is WParagraph && (charFormat.OwnerBase as WParagraph).IsInCell && ((charFormat.OwnerBase as WParagraph).OwnerTextBody as WTableCell).OwnerRow.RowFormat.Revisions.Count > 0)
    {
      this.LineTableRevForBreakCharacterFormat(charFormat, moveRevision, revisionType, authorName, dateTime, revisionName);
    }
    else
    {
      Revision newRevision = this.CreateNewRevision(revisionType, authorName, dateTime, revisionName);
      charFormat.Revisions.Add(newRevision);
      newRevision.Range.Items.Add((object) charFormat);
    }
  }

  internal string GetAuthorName(WordReaderBase reader, bool isInsertKey)
  {
    string authorName = string.Empty;
    short key = isInsertKey ? reader.CHPXSprms.GetShort(18436, (short) 0) : reader.CHPXSprms.GetShort(18531, (short) 0);
    if (reader.SttbfRMarkAuthorNames != null && reader.SttbfRMarkAuthorNames.Count > 0 && reader.SttbfRMarkAuthorNames.ContainsKey((int) key))
      authorName = reader.SttbfRMarkAuthorNames[(int) key];
    return authorName;
  }

  internal DateTime GetDateTime(
    WordReaderBase reader,
    bool isInsertKey,
    WCharacterFormat charFormat)
  {
    SinglePropertyModifierRecord propertyModifierRecord = isInsertKey ? reader.CHPXSprms[26629] : reader.CHPXSprms[26724];
    DateTime dateTime = new DateTime();
    if (propertyModifierRecord != null)
      dateTime = charFormat.ParseDTTM(propertyModifierRecord.IntValue);
    if (dateTime.Year < 1900)
      dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
    return dateTime;
  }

  private void LineTableRevForBreakCharacterFormat(
    WCharacterFormat charFormat,
    Revision moveRevision,
    RevisionType revisionType,
    string revAuthorName,
    DateTime revDateTime,
    string name)
  {
    foreach (Revision revision in ((charFormat.OwnerBase as WParagraph).OwnerTextBody as WTableCell).OwnerRow.RowFormat.Revisions)
    {
      if (revision.RevisionType == revisionType)
      {
        if (revision.Author == revAuthorName)
        {
          charFormat.Revisions.Add(revision);
          revision.Range.Items.Add((object) charFormat);
        }
        else
        {
          Revision newChildRevision = this.CreateNewChildRevision(revisionType, revAuthorName, revDateTime, name);
          charFormat.Revisions.Add(newChildRevision);
          newChildRevision.Range.Items.Add((object) charFormat);
          revision.ChildRevisions.Add(newChildRevision);
        }
      }
      else
      {
        Revision newRevision = this.CreateNewRevision(revisionType, revAuthorName, revDateTime, name);
        charFormat.Revisions.Add(newRevision);
        newRevision.Range.Items.Add((object) charFormat);
      }
    }
  }

  private void LinkMoveRevForBreakCharacterFormat(
    WCharacterFormat charFormat,
    Revision moveRevision,
    RevisionType revisionType,
    string revAuthorName,
    DateTime revDateTime,
    string name)
  {
    if (moveRevision.RevisionType == revisionType)
    {
      if (moveRevision.Author == revAuthorName)
      {
        charFormat.Revisions.Add(moveRevision);
        moveRevision.Range.Items.Add((object) charFormat);
      }
      else
      {
        Revision newChildRevision = this.CreateNewChildRevision(revisionType, revAuthorName, revDateTime, name);
        charFormat.Revisions.Add(newChildRevision);
        newChildRevision.Range.Items.Add((object) charFormat);
        moveRevision.ChildRevisions.Add(newChildRevision);
      }
    }
    else
    {
      Revision newRevision = this.CreateNewRevision(revisionType, revAuthorName, revDateTime, name);
      charFormat.Revisions.Add(newRevision);
      newRevision.Range.Items.Add((object) charFormat);
    }
  }

  internal void MoveRevisionRanges(Revision sourceRevision, Revision destinationRevision)
  {
    for (int index = 0; index < sourceRevision.Range.InnerList.Count; ++index)
    {
      if (sourceRevision.Range.InnerList[index] is FormatBase)
      {
        FormatBase inner = sourceRevision.Range.InnerList[index] as FormatBase;
        inner.Revisions.Remove(sourceRevision);
        inner.Revisions.Add(destinationRevision);
        destinationRevision.Range.Items.Add((object) inner);
      }
      else
      {
        Entity inner = sourceRevision.Range.InnerList[index] as Entity;
        inner.RevisionsInternal.Remove(sourceRevision);
        inner.RevisionsInternal.Add(destinationRevision);
        destinationRevision.Range.Items.Add((object) inner);
      }
    }
    sourceRevision.Range.Items.Clear();
  }

  internal void ParaFormatChangeRevision(WParagraphFormat paragraphFormat)
  {
    Revision revision = (Revision) null;
    WParagraph wparagraph = (WParagraph) null;
    if (paragraphFormat.OwnerBase is WParagraph)
    {
      wparagraph = paragraphFormat.OwnerBase as WParagraph;
    }
    else
    {
      if (!(paragraphFormat.OwnerBase is Style) || (paragraphFormat.OwnerBase as Style).StyleType != StyleType.ParagraphStyle)
        return;
      revision = this.CreateNewRevision(RevisionType.StyleDefinitionChange, paragraphFormat.FormatChangeAuthorName, paragraphFormat.FormatChangeDateTime, (string) null);
    }
    if (wparagraph != null && wparagraph.PreviousSibling != null && wparagraph.PreviousSibling is WParagraph)
    {
      WParagraph previousSibling = wparagraph.PreviousSibling as WParagraph;
      if (previousSibling.ParagraphFormat.Revisions.Count > 0)
        revision = this.GetSameRevision((FormatBase) previousSibling.ParagraphFormat, (FormatBase) paragraphFormat);
    }
    else if (wparagraph != null && wparagraph.PreviousSibling == null && wparagraph.IsInCell)
    {
      WTableCell ownerTableCell = wparagraph.GetOwnerTableCell(wparagraph.OwnerTextBody);
      if (ownerTableCell != null && ownerTableCell.PreviousSibling != null)
      {
        WTableCell previousSibling = ownerTableCell.PreviousSibling as WTableCell;
        if (previousSibling.LastParagraph != null && previousSibling.LastParagraph.ParagraphFormat.Revisions.Count > 0)
          revision = this.GetSameRevision((FormatBase) previousSibling.LastParagraph.ParagraphFormat, (FormatBase) paragraphFormat);
      }
      else if (ownerTableCell != null && ownerTableCell.OwnerRow.Index == 0 && ownerTableCell.OwnerRow.OwnerTable.PreviousSibling != null && ownerTableCell.OwnerRow.OwnerTable.PreviousSibling is WParagraph && ownerTableCell.OwnerRow.OwnerTable.PreviousSibling is WParagraph previousSibling1 && previousSibling1.ParagraphFormat.Revisions.Count > 0)
        revision = this.GetSameRevision((FormatBase) previousSibling1.ParagraphFormat, (FormatBase) paragraphFormat);
    }
    if (revision == null)
      revision = this.CreateNewRevision(RevisionType.Formatting, paragraphFormat.FormatChangeAuthorName, paragraphFormat.FormatChangeDateTime, (string) null);
    if (revision == null)
      return;
    revision.Range.Items.Add((object) paragraphFormat);
    paragraphFormat.Revisions.Add(revision);
  }

  internal void SectionFormatChangeRevision(WSection section)
  {
    Revision revision1 = (Revision) null;
    string authorName = section.SectionFormat.OldPropertiesHash.ContainsKey(5) ? (string) section.SectionFormat.OldPropertiesHash[5] : string.Empty;
    DateTime dateTime = section.SectionFormat.OldPropertiesHash.ContainsKey(6) ? (DateTime) section.SectionFormat.OldPropertiesHash[6] : DateTime.MinValue;
    if (section != null && section.PreviousSibling != null && section.PreviousSibling is WSection)
    {
      WSection previousSibling = section.PreviousSibling as WSection;
      if (previousSibling.SectionFormat.Revisions.Count > 0)
      {
        foreach (Revision revision2 in previousSibling.SectionFormat.Revisions)
        {
          if (revision2.RevisionType == RevisionType.Formatting && revision2.Author == authorName && section.SectionFormat.Compare(previousSibling.SectionFormat))
          {
            revision1 = revision2;
            break;
          }
        }
      }
    }
    if (revision1 == null)
      revision1 = this.m_doc.CreateNewRevision(RevisionType.Formatting, authorName, dateTime, (string) null);
    if (revision1 == null)
      return;
    revision1.Range.Items.Add((object) section.SectionFormat);
    section.SectionFormat.Revisions.Add(revision1);
  }

  private Revision GetSameRevision(FormatBase previousFormat, FormatBase currentFormat)
  {
    WCharacterFormat wcharacterFormat = currentFormat is WCharacterFormat ? currentFormat as WCharacterFormat : (WCharacterFormat) null;
    WParagraphFormat wparagraphFormat = currentFormat is WParagraphFormat ? currentFormat as WParagraphFormat : (WParagraphFormat) null;
    foreach (Revision revision in previousFormat.Revisions)
    {
      bool flag = revision.RevisionType == RevisionType.Formatting;
      if (wcharacterFormat != null && flag && revision.Author == wcharacterFormat.FormatChangeAuthorName && wcharacterFormat.Compare(previousFormat as WCharacterFormat) || wparagraphFormat != null && flag && revision.Author == wparagraphFormat.FormatChangeAuthorName && wparagraphFormat.Compare(previousFormat as WParagraphFormat))
        return revision;
    }
    return (Revision) null;
  }

  internal void CharFormatChangeRevision(WCharacterFormat charFormat, ParagraphItem item)
  {
    if (charFormat.OwnerBase == null || charFormat.OwnerBase is Style && (charFormat.OwnerBase as Style).StyleType != StyleType.ParagraphStyle && (charFormat.OwnerBase as Style).StyleType != StyleType.CharacterStyle)
      return;
    Revision revision = (Revision) null;
    string changeAuthorName = charFormat.FormatChangeAuthorName;
    DateTime formatChangeDateTime = charFormat.FormatChangeDateTime;
    if (item != null)
    {
      if (!this.HasRenderableItemBefore(item))
        revision = this.GetCharRevisionForFirstParaItem(item, charFormat);
      else if (item.PreviousSibling != null && item.PreviousSibling is Entity)
      {
        Entity previousSibling = item.PreviousSibling as Entity;
        while (true)
        {
          switch (previousSibling)
          {
            case BookmarkStart _:
            case BookmarkEnd _:
            case EditableRangeStart _:
            case EditableRangeEnd _:
              previousSibling = previousSibling.PreviousSibling as Entity;
              continue;
            case null:
              goto label_14;
            default:
              goto label_8;
          }
        }
label_8:
        if ((previousSibling as ParagraphItem).GetCharFormat().Revisions.Count > 0)
          revision = this.GetSameRevision((FormatBase) (previousSibling as ParagraphItem).GetCharFormat(), (FormatBase) charFormat);
      }
    }
    else if (charFormat.OwnerBase is Style && ((charFormat.OwnerBase as Style).StyleType == StyleType.ParagraphStyle || (charFormat.OwnerBase as Style).StyleType == StyleType.CharacterStyle))
      revision = this.GetCharRevisionForStyle(charFormat, changeAuthorName, formatChangeDateTime);
    else if (charFormat.OwnerBase is WParagraph)
      revision = this.GetPreviousBreakCharFmtChange(charFormat.OwnerBase as WParagraph, charFormat);
label_14:
    if (revision == null)
      revision = this.CreateNewRevision(RevisionType.Formatting, changeAuthorName, formatChangeDateTime, (string) null);
    revision.Range.Items.Add((object) charFormat);
    charFormat.Revisions.Add(revision);
  }

  private Revision GetCharRevisionForStyle(
    WCharacterFormat charFormat,
    string formatRevAuthorName,
    DateTime formatRevDateTime)
  {
    Revision destinationRevision = (Revision) null;
    Style ownerBase = charFormat.OwnerBase as Style;
    if (ownerBase.StyleType == StyleType.ParagraphStyle)
    {
      WParagraphFormat paragraphFormat = this.GetParagraphFormat(ownerBase);
      if (paragraphFormat.Revisions.Count > 0)
      {
        using (List<Revision>.Enumerator enumerator = paragraphFormat.Revisions.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            Revision current = enumerator.Current;
            if (current.Author == formatRevAuthorName)
            {
              destinationRevision = current;
            }
            else
            {
              destinationRevision = this.CreateNewRevision(RevisionType.StyleDefinitionChange, formatRevAuthorName, formatRevDateTime, (string) null);
              this.Revisions.Remove(current);
              this.MoveRevisionRanges(current, destinationRevision);
            }
          }
        }
      }
    }
    return destinationRevision;
  }

  private Revision GetCharRevisionForFirstParaItem(ParagraphItem item, WCharacterFormat charFormat)
  {
    Revision forFirstParaItem = (Revision) null;
    WParagraph ownerParagraphValue = item.GetOwnerParagraphValue();
    if (ownerParagraphValue != null && ownerParagraphValue.PreviousSibling is WParagraph)
    {
      if (ownerParagraphValue.PreviousSibling is WParagraph previousSibling && previousSibling.BreakCharacterFormat.Revisions.Count > 0)
        forFirstParaItem = this.GetSameRevision((FormatBase) previousSibling.BreakCharacterFormat, (FormatBase) charFormat);
    }
    else
      forFirstParaItem = this.GetPreviousBreakCharFmtChange(ownerParagraphValue, charFormat);
    return forFirstParaItem;
  }

  private Revision GetPreviousBreakCharFmtChange(WParagraph paragraph, WCharacterFormat charFormat)
  {
    Revision breakCharFmtChange = (Revision) null;
    if (paragraph != null && paragraph.PreviousSibling == null && paragraph.IsInCell)
    {
      WTableCell ownerTableCell = paragraph.GetOwnerTableCell(paragraph.OwnerTextBody);
      if (ownerTableCell != null && ownerTableCell.PreviousSibling != null)
      {
        WTableCell previousSibling = ownerTableCell.PreviousSibling as WTableCell;
        if (previousSibling.LastParagraph != null && previousSibling.LastParagraph.BreakCharacterFormat.Revisions.Count > 0)
          breakCharFmtChange = this.GetSameRevision((FormatBase) previousSibling.LastParagraph.BreakCharacterFormat, (FormatBase) charFormat);
      }
      else if (ownerTableCell != null && ownerTableCell.OwnerRow.Index == 0 && ownerTableCell.OwnerRow.OwnerTable.PreviousSibling != null && ownerTableCell.OwnerRow.OwnerTable.PreviousSibling is WParagraph && ownerTableCell.OwnerRow.OwnerTable.PreviousSibling is WParagraph previousSibling1 && previousSibling1.BreakCharacterFormat.Revisions.Count > 0)
        breakCharFmtChange = this.GetSameRevision((FormatBase) previousSibling1.BreakCharacterFormat, (FormatBase) charFormat);
    }
    return breakCharFmtChange;
  }

  private WParagraphFormat GetParagraphFormat(Style style)
  {
    if (style.StyleType == StyleType.TableStyle)
      return (style as WTableStyle).ParagraphFormat;
    return style.StyleType != StyleType.NumberingStyle ? (style as WParagraphStyle).ParagraphFormat : (style as WNumberingStyle).ParagraphFormat;
  }

  internal bool HasRenderableItemBefore(ParagraphItem item)
  {
    if (!(item.PreviousSibling is Entity entity) && item.Owner is InlineContentControl)
      entity = (item.Owner as InlineContentControl).PreviousSibling as Entity;
label_2:
    entity = entity is InlineContentControl ? (entity as InlineContentControl).ParagraphItems.LastItem : entity;
    while (true)
    {
      switch (entity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case EditableRangeStart _:
        case EditableRangeEnd _:
          entity = entity.PreviousSibling as Entity;
          continue;
        case InlineContentControl _:
          goto label_2;
        case null:
          goto label_6;
        default:
          goto label_5;
      }
    }
label_5:
    return true;
label_6:
    return false;
  }

  internal void UpdateTableFormatRevision(WTableRow row)
  {
    Revision formattingRevision = this.GetRowFormattingRevision(row);
    Revision revision1 = (Revision) null;
    if (row.PreviousSibling is WTableRow row1 && row.OwnerTable.PreviousSibling is WTable)
      row1 = (row.OwnerTable.PreviousSibling as WTable).LastRow;
    if (row1 != null)
      revision1 = this.GetRowFormattingRevision(row1);
    if (formattingRevision != null && revision1 != null && formattingRevision.Author == revision1.Author && formattingRevision != revision1)
    {
      this.Revisions.Remove(revision1);
      this.MoveRevisionRanges(revision1, formattingRevision);
    }
    if (row.Index != 0 || row.OwnerTable.DocxTableFormat.Format.Revisions.Count <= 0)
      return;
    foreach (Revision revision2 in row.OwnerTable.DocxTableFormat.Format.Revisions)
    {
      if (formattingRevision != null && revision2.RevisionType == formattingRevision.RevisionType)
      {
        this.Revisions.Remove(revision2);
        this.MoveRevisionRanges(revision2, formattingRevision);
        break;
      }
      if (formattingRevision == null)
      {
        this.Revisions.Remove(revision2);
        row.OwnerTable.DocxTableFormat.Format.Revisions.Remove(revision2);
        break;
      }
    }
  }

  internal void UpdateRowFormatRevision(RowFormat rowFormat)
  {
    Revision revision1 = (Revision) null;
    string changeAuthorName = rowFormat.FormatChangeAuthorName;
    DateTime formatChangeDateTime = rowFormat.FormatChangeDateTime;
    if (rowFormat.Revisions.Count > 0)
    {
      foreach (Revision revision2 in rowFormat.Revisions)
      {
        if (revision2.RevisionType == RevisionType.Formatting)
        {
          revision1 = revision2;
          break;
        }
      }
    }
    if (revision1 != null)
    {
      if (!(revision1.Author != changeAuthorName))
        return;
      Revision newRevision = this.CreateNewRevision(RevisionType.Formatting, changeAuthorName, formatChangeDateTime, (string) null);
      this.Revisions.Remove(revision1);
      this.MoveRevisionRanges(revision1, newRevision);
    }
    else
    {
      Revision newRevision = this.CreateNewRevision(RevisionType.Formatting, changeAuthorName, formatChangeDateTime, (string) null);
      rowFormat.Revisions.Add(newRevision);
      newRevision.Range.Items.Add((object) rowFormat);
    }
  }

  internal void UpdateCellFormatRevision(WTableCell tableCell)
  {
    Revision revision1 = (Revision) null;
    string changeAuthorName = tableCell.CellFormat.FormatChangeAuthorName;
    DateTime formatChangeDateTime = tableCell.CellFormat.FormatChangeDateTime;
    for (int index = tableCell.Index - 1; index >= 0; --index)
    {
      WTableCell cell = tableCell.OwnerRow.Cells[index];
      if (cell.CellFormat.Revisions.Count > 0)
      {
        foreach (Revision revision2 in cell.CellFormat.Revisions)
        {
          if (revision2.RevisionType == RevisionType.Formatting)
          {
            revision1 = revision2;
            break;
          }
        }
      }
      if (revision1 != null)
        break;
    }
    if (revision1 == null && tableCell.OwnerRow.RowFormat.Revisions.Count > 0)
    {
      foreach (Revision revision3 in tableCell.OwnerRow.RowFormat.Revisions)
      {
        if (revision3.RevisionType == RevisionType.Formatting)
        {
          revision1 = revision3;
          break;
        }
      }
    }
    if (revision1 != null)
    {
      if (revision1.Author == changeAuthorName)
      {
        tableCell.CellFormat.Revisions.Add(revision1);
        revision1.Range.Items.Add((object) tableCell.CellFormat);
      }
      else
      {
        Revision newRevision = this.CreateNewRevision(RevisionType.Formatting, changeAuthorName, formatChangeDateTime, (string) null);
        tableCell.CellFormat.Revisions.Add(newRevision);
        newRevision.Range.Items.Add((object) tableCell.CellFormat);
        this.Revisions.Remove(revision1);
        this.MoveRevisionRanges(revision1, newRevision);
      }
    }
    else
    {
      Revision newRevision = this.CreateNewRevision(RevisionType.Formatting, changeAuthorName, formatChangeDateTime, (string) null);
      tableCell.CellFormat.Revisions.Add(newRevision);
      newRevision.Range.Items.Add((object) tableCell.CellFormat);
    }
  }

  private Revision GetRowFormattingRevision(WTableRow row)
  {
    if (row.RowFormat.Revisions.Count > 0)
    {
      foreach (Revision revision in row.RowFormat.Revisions)
      {
        if (revision.RevisionType == RevisionType.Formatting)
          return revision;
      }
    }
    for (int index = 0; index < row.Cells.Count; ++index)
    {
      WTableCell cell = row.Cells[index];
      if (cell.CellFormat.Revisions.Count > 0)
      {
        foreach (Revision revision in cell.CellFormat.Revisions)
        {
          if (revision.RevisionType == RevisionType.Formatting)
            return revision;
        }
      }
    }
    return (Revision) null;
  }

  internal void UpdateFieldRevision(WField field)
  {
    if (field.RevisionsInternal.Count <= 0)
      return;
    Stack<WField> nestedField = new Stack<WField>();
    foreach (Revision fieldRevision in field.RevisionsInternal)
    {
      if (field.Range.Items.Count == 0)
        field.UpdateFieldRange();
      WParagraph ownerParagraphValue = field.GetOwnerParagraphValue();
      if (field.FieldEnd.OwnerParagraph != ownerParagraphValue)
        this.RemoveFieldRevisions(ownerParagraphValue.BreakCharacterFormat.Revisions, (Entity) null, nestedField, (FormatBase) ownerParagraphValue.BreakCharacterFormat, fieldRevision);
      for (int index = 0; index < field.Range.Items.Count; ++index)
      {
        Entity entity = field.Range.Items[index] as Entity;
        switch (entity)
        {
          case ParagraphItem _:
            this.RemoveFieldRangeRevision(entity as ParagraphItem, nestedField, fieldRevision, field);
            break;
          case TextBodyItem _:
            this.RemoveFieldRangeRevision(entity as TextBodyItem, nestedField, fieldRevision, field);
            break;
        }
      }
      field.Range.Items.Clear();
    }
  }

  private void RemoveFieldRangeRevision(
    TextBodyItem bodyItemEntity,
    Stack<WField> nestedField,
    Revision fieldRevision,
    WField CurrentField)
  {
    switch (bodyItemEntity.EntityType)
    {
      case EntityType.Paragraph:
        WParagraph wparagraph = bodyItemEntity as WParagraph;
        if (CurrentField.FieldEnd.OwnerParagraph != wparagraph)
          this.RemoveFieldRevisions(wparagraph.BreakCharacterFormat.Revisions, (Entity) null, nestedField, (FormatBase) wparagraph.BreakCharacterFormat, fieldRevision);
        this.RemoveFieldRangeRevision(wparagraph.Items, nestedField, fieldRevision, CurrentField);
        break;
      case EntityType.BlockContentControl:
        this.RemoveFieldRangeRevision((bodyItemEntity as BlockContentControl).TextBody, nestedField, fieldRevision, CurrentField);
        break;
      case EntityType.Table:
        this.RemoveFieldRangeRevision(bodyItemEntity as WTable, nestedField, fieldRevision);
        break;
    }
  }

  private void RemoveFieldRangeRevision(
    ParagraphItem paraItem,
    Stack<WField> nestedField,
    Revision fieldRevision,
    WField CurrentField)
  {
    switch (paraItem)
    {
      case InlineContentControl _:
        this.RemoveFieldRangeRevision((paraItem as InlineContentControl).ParagraphItems, nestedField, fieldRevision, CurrentField);
        break;
      case WTextBox _:
        this.RemoveFieldRangeRevision((paraItem as WTextBox).TextBoxBody, nestedField, fieldRevision, CurrentField);
        break;
      case Shape _:
        this.RemoveFieldRangeRevision((paraItem as Shape).TextBody, nestedField, fieldRevision, CurrentField);
        break;
      default:
        if (paraItem.RevisionsInternal.Count <= 0)
          break;
        this.RemoveFieldRevisions(paraItem.RevisionsInternal, (Entity) paraItem, nestedField, (FormatBase) null, fieldRevision);
        break;
    }
  }

  private void RemoveFieldRangeRevision(
    WTextBody textBody,
    Stack<WField> nestedField,
    Revision fieldRevision,
    WField CurrentField)
  {
    for (int index = 0; index < textBody.ChildEntities.Count; ++index)
    {
      IEntity childEntity = (IEntity) textBody.ChildEntities[index];
      switch (childEntity.EntityType)
      {
        case EntityType.Paragraph:
          WParagraph wparagraph = childEntity as WParagraph;
          if (CurrentField.FieldEnd.OwnerParagraph != wparagraph)
            this.RemoveFieldRevisions(wparagraph.BreakCharacterFormat.Revisions, (Entity) null, nestedField, (FormatBase) wparagraph.BreakCharacterFormat, fieldRevision);
          this.RemoveFieldRangeRevision(wparagraph.Items, nestedField, fieldRevision, CurrentField);
          break;
        case EntityType.BlockContentControl:
          this.RemoveFieldRangeRevision((childEntity as BlockContentControl).TextBody, nestedField, fieldRevision, CurrentField);
          break;
        case EntityType.Table:
          this.RemoveFieldRangeRevision(childEntity as WTable, nestedField, fieldRevision);
          break;
      }
    }
  }

  private void RemoveFieldRangeRevision(
    ParagraphItemCollection paraItems,
    Stack<WField> nestedField,
    Revision fieldRevision,
    WField CurrentField)
  {
    for (int index = 0; index < paraItems.Count; ++index)
    {
      if (paraItems[index] is InlineContentControl)
        this.RemoveFieldRangeRevision((paraItems[index] as InlineContentControl).ParagraphItems, nestedField, fieldRevision, CurrentField);
      else if (paraItems[index] is WTextBox)
        this.RemoveFieldRangeRevision((paraItems[index] as WTextBox).TextBoxBody, nestedField, fieldRevision, CurrentField);
      else if (paraItems[index] is Shape)
        this.RemoveFieldRangeRevision((paraItems[index] as Shape).TextBody, nestedField, fieldRevision, CurrentField);
      else if (paraItems[index].RevisionsInternal.Count > 0)
        this.RemoveFieldRevisions(paraItems[index].RevisionsInternal, (Entity) paraItems[index], nestedField, (FormatBase) null, fieldRevision);
    }
  }

  private void RemoveFieldRangeRevision(
    WTable table,
    Stack<WField> nestedField,
    Revision fieldRevision)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
      this.RemoveFieldRevisions(row.RowFormat.Revisions, (Entity) null, nestedField, (FormatBase) row.RowFormat, fieldRevision);
  }

  private void RemoveFieldRevisions(
    List<Revision> revisions,
    Entity entity,
    Stack<WField> nestedField,
    FormatBase format,
    Revision fieldRevision)
  {
    foreach (Revision revision in revisions)
    {
      if ((nestedField.Count == 0 || nestedField.Peek().RevisionsInternal.Count == 0) && fieldRevision != null && fieldRevision.RevisionType == revision.RevisionType)
      {
        if (fieldRevision == revision)
          break;
        if (this.Revisions.InnerList.Contains((object) revision))
          this.Revisions.Remove(revision);
        if (fieldRevision.Author != revision.Author)
        {
          bool flag = false;
          foreach (Revision childRevision in (CollectionImpl) fieldRevision.ChildRevisions)
          {
            if (childRevision == revision)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
          fieldRevision.ChildRevisions.Add(revision);
          break;
        }
        if (entity != null)
        {
          entity.RevisionsInternal.Remove(revision);
          entity.RevisionsInternal.Add(fieldRevision);
          fieldRevision.Range.InnerList.Add((object) entity);
          break;
        }
        if (format is RowFormat)
        {
          this.MoveRevisionRanges(revision, fieldRevision);
          break;
        }
        format.Revisions.Remove(revision);
        format.Revisions.Add(fieldRevision);
        fieldRevision.Range.InnerList.Add((object) format);
        break;
      }
      if (entity is WField)
        nestedField.Push(entity as WField);
      else if (nestedField.Count > 0 && entity is WFieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldEnd)
        nestedField.Pop();
    }
  }

  internal void UpdateTableRowRevision(WTableRow tableRow)
  {
    if (tableRow.RowFormat.Revisions.Count <= 0)
      return;
    foreach (Revision revision in tableRow.RowFormat.Revisions)
    {
      if (revision.RevisionType == RevisionType.Insertions || revision.RevisionType == RevisionType.Deletions)
      {
        Revision rowRevision = revision;
        for (int index = 0; index < tableRow.Cells.Count; ++index)
          this.RemoveTableCellRevision((WTextBody) tableRow.Cells[index], rowRevision);
      }
    }
  }

  private void RemoveTableCellRevision(WTextBody textBody, Revision rowRevision)
  {
    for (int index = 0; index < textBody.ChildEntities.Count; ++index)
    {
      IEntity childEntity = (IEntity) textBody.ChildEntities[index];
      switch (childEntity.EntityType)
      {
        case EntityType.Paragraph:
          WParagraph wparagraph = childEntity as WParagraph;
          this.RemoveTableCellRevision(wparagraph.BreakCharacterFormat.Revisions, (Entity) null, (FormatBase) wparagraph.BreakCharacterFormat, rowRevision);
          this.RemoveTableCellRevision(wparagraph.Items, rowRevision);
          break;
        case EntityType.BlockContentControl:
          this.RemoveTableCellRevision((childEntity as BlockContentControl).TextBody, rowRevision);
          break;
        case EntityType.Table:
          this.RemoveTableCellRevision(childEntity as WTable, rowRevision);
          break;
      }
    }
  }

  private void RemoveTableCellRevision(ParagraphItemCollection paraItems, Revision rowRevision)
  {
    for (int index = 0; index < paraItems.Count; ++index)
    {
      if (paraItems[index] is InlineContentControl)
        this.RemoveTableCellRevision((paraItems[index] as InlineContentControl).ParagraphItems, rowRevision);
      else if (paraItems[index] is WTextBox)
        this.RemoveTableCellRevision((paraItems[index] as WTextBox).TextBoxBody, rowRevision);
      else if (paraItems[index] is Shape)
        this.RemoveTableCellRevision((paraItems[index] as Shape).TextBody, rowRevision);
      else if (paraItems[index].RevisionsInternal.Count > 0)
        this.RemoveTableCellRevision(paraItems[index].RevisionsInternal, (Entity) paraItems[index], (FormatBase) null, rowRevision);
    }
  }

  private void RemoveTableCellRevision(WTable table, Revision rowRevision)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
      this.RemoveTableCellRevision(row.RowFormat.Revisions, (Entity) null, (FormatBase) row.RowFormat, rowRevision);
  }

  private void RemoveTableCellRevision(
    List<Revision> revisions,
    Entity entity,
    FormatBase format,
    Revision rowRevision)
  {
    foreach (Revision revision in revisions)
    {
      if (rowRevision != null && rowRevision.RevisionType == revision.RevisionType)
      {
        if (rowRevision == revision)
          break;
        if (this.Revisions.InnerList.Contains((object) revision))
          this.Revisions.Remove(revision);
        if (rowRevision.Author != revision.Author)
        {
          bool flag = false;
          foreach (Revision childRevision in (CollectionImpl) rowRevision.ChildRevisions)
          {
            if (childRevision == revision)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
          rowRevision.ChildRevisions.Add(revision);
          break;
        }
        if (entity != null)
        {
          entity.RevisionsInternal.Remove(revision);
          entity.RevisionsInternal.Add(rowRevision);
          rowRevision.Range.InnerList.Add((object) entity);
          break;
        }
        if (format is RowFormat)
        {
          this.MoveRevisionRanges(revision, rowRevision);
          break;
        }
        format.Revisions.Remove(revision);
        format.Revisions.Add(rowRevision);
        rowRevision.Range.InnerList.Add((object) format);
        break;
      }
    }
  }

  internal void UpdateLastItemRevision(IWParagraph paragraph, ParagraphItemCollection items)
  {
    if ((paragraph as WParagraph).IsEmptyParagraph())
    {
      this.LinkEmptyParaBreakCharacterFormat(paragraph, items);
    }
    else
    {
      List<Revision> revisions = paragraph.BreakCharacterFormat.Revisions;
      Entity entity = items.LastItem;
      while (true)
      {
        switch (entity)
        {
          case BookmarkStart _:
          case BookmarkEnd _:
          case EditableRangeStart _:
          case EditableRangeEnd _:
            entity = entity.PreviousSibling as Entity;
            continue;
          default:
            goto label_5;
        }
      }
label_5:
      if (revisions.Count > 0 && entity != null && entity.RevisionsInternal.Count > 0)
        this.LinkLastItemWithBreakCharFormat(paragraph, revisions, entity.RevisionsInternal);
      else if (revisions.Count > 0 && entity is WFieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldEnd && (entity as WFieldMark).ParentField.RevisionsInternal.Count > 0)
        this.LinkLastItemWithBreakCharFormat(paragraph, revisions, (entity as WFieldMark).ParentField.RevisionsInternal);
      else if (revisions.Count > 0 && entity is InlineContentControl)
        this.LinkContentControlWithBreakCharFormat(entity, revisions);
      WCharacterFormat charFormat;
      if (entity == null || (charFormat = (entity as ParagraphItem).GetCharFormat()) == null || !charFormat.HasKey(105) || !paragraph.BreakCharacterFormat.HasKey(105) || revisions.Count <= 0 || charFormat.Revisions.Count <= 0)
        return;
      for (int index = 0; index > charFormat.Revisions.Count; ++index)
      {
        Revision revision = charFormat.Revisions[index];
        foreach (Revision sourceRevision in revisions)
        {
          if (revision != sourceRevision && revision.RevisionType == sourceRevision.RevisionType && revision.Author == sourceRevision.Author && charFormat.Compare(paragraph.BreakCharacterFormat))
          {
            sourceRevision.RemoveSelf();
            this.MoveRevisionRanges(sourceRevision, revision);
            break;
          }
        }
      }
    }
  }

  private void LinkContentControlWithBreakCharFormat(Entity item, List<Revision> breakRevisions)
  {
    Entity entity = (item as InlineContentControl).ParagraphItems.LastItem;
label_1:
    entity = entity is InlineContentControl ? (entity as InlineContentControl).ParagraphItems.LastItem : entity;
    while (true)
    {
      switch (entity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case EditableRangeStart _:
        case EditableRangeEnd _:
          entity = entity.PreviousSibling as Entity;
          continue;
        case InlineContentControl _:
          goto label_1;
        case null:
          goto label_15;
        default:
          goto label_4;
      }
    }
label_15:
    return;
label_4:
    if (entity.RevisionsInternal.Count <= 0)
      return;
    foreach (Revision destinationRevision in entity.RevisionsInternal)
    {
      foreach (Revision breakRevision in breakRevisions)
      {
        if (destinationRevision != breakRevision && destinationRevision.RevisionType == breakRevision.RevisionType && destinationRevision.Author == breakRevision.Author)
        {
          breakRevision.RemoveSelf();
          this.MoveRevisionRanges(breakRevision, destinationRevision);
          break;
        }
      }
    }
  }

  private void LinkLastItemWithBreakCharFormat(
    IWParagraph paragraph,
    List<Revision> breakRevisions,
    List<Revision> itemRevisions)
  {
    foreach (Revision itemRevision in itemRevisions)
    {
      foreach (Revision breakRevision in breakRevisions)
      {
        if (itemRevision != breakRevision && itemRevision.RevisionType == breakRevision.RevisionType && itemRevision.Author == breakRevision.Author)
        {
          breakRevision.RemoveSelf();
          this.MoveRevisionRanges(breakRevision, itemRevision);
          break;
        }
      }
    }
  }

  internal void LinkEmptyParaBreakCharacterFormat(
    IWParagraph paragraph,
    ParagraphItemCollection items)
  {
    if (paragraph.BreakCharacterFormat.Revisions.Count <= 0)
      return;
    for (int index = 0; index < paragraph.BreakCharacterFormat.Revisions.Count; ++index)
    {
      Revision revision = paragraph.BreakCharacterFormat.Revisions[index];
      if (paragraph.PreviousSibling != null)
      {
        Entity previousSibling = paragraph.PreviousSibling as Entity;
        switch (previousSibling)
        {
          case WParagraph _:
            if (previousSibling is WParagraph wparagraph && wparagraph.BreakCharacterFormat.Revisions.Count > 0)
            {
              using (List<Revision>.Enumerator enumerator = wparagraph.BreakCharacterFormat.Revisions.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Revision current = enumerator.Current;
                  if (current != revision && current.RevisionType == revision.RevisionType && current.Author == revision.Author && (current.RevisionType != RevisionType.Formatting || paragraph.BreakCharacterFormat.Compare(wparagraph.BreakCharacterFormat)))
                  {
                    revision.RemoveSelf();
                    this.MoveRevisionRanges(revision, current);
                    break;
                  }
                }
                continue;
              }
            }
            continue;
          case WTable _:
            if (previousSibling is WTable wtable && wtable.LastRow != null && wtable.LastRow.RowFormat.Revisions.Count > 0)
            {
              using (List<Revision>.Enumerator enumerator = wtable.LastRow.RowFormat.Revisions.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Revision current = enumerator.Current;
                  if (current != revision && current.RevisionType != RevisionType.Formatting && current.RevisionType == revision.RevisionType && current.Author == revision.Author)
                  {
                    revision.RemoveSelf();
                    this.MoveRevisionRanges(revision, current);
                    break;
                  }
                }
                continue;
              }
            }
            continue;
          default:
            continue;
        }
      }
    }
  }

  internal void RemoveRevisionFromCollection(WTextBody textBody)
  {
    for (int index = 0; index < textBody.ChildEntities.Count; ++index)
    {
      IEntity childEntity = (IEntity) textBody.ChildEntities[index];
      switch (childEntity.EntityType)
      {
        case EntityType.Paragraph:
          WParagraph wparagraph = childEntity as WParagraph;
          if (wparagraph.BreakCharacterFormat.Revisions.Count > 0)
            this.RemoveRevisions(wparagraph.BreakCharacterFormat.Revisions);
          if (wparagraph.ParagraphFormat.Revisions.Count > 0)
            this.RemoveRevisions(wparagraph.BreakCharacterFormat.Revisions);
          this.RemoveRevisionFromCollection(wparagraph.Items);
          break;
        case EntityType.BlockContentControl:
          this.RemoveRevisionFromCollection((childEntity as BlockContentControl).TextBody);
          break;
        case EntityType.Table:
          this.RemoveRevisionFromCollection(childEntity as WTable);
          break;
      }
    }
  }

  private void RemoveRevisionFromCollection(ParagraphItemCollection paraItems)
  {
    for (int index = 0; index < paraItems.Count; ++index)
    {
      if (paraItems[index] is InlineContentControl)
        this.RemoveRevisionFromCollection((paraItems[index] as InlineContentControl).ParagraphItems);
      else if (paraItems[index] is WTextBox)
        this.RemoveRevisionFromCollection((paraItems[index] as WTextBox).TextBoxBody);
      else if (paraItems[index] is Shape)
        this.RemoveRevisionFromCollection((paraItems[index] as Shape).TextBody);
      else if (paraItems[index].RevisionsInternal.Count > 0)
      {
        if (paraItems[index] is WTextRange && (paraItems[index] as WTextRange).CharacterFormat.Revisions.Count > 0)
          this.RemoveRevisions((paraItems[index] as WTextRange).CharacterFormat.Revisions);
        this.RemoveRevisions(paraItems[index].RevisionsInternal);
      }
    }
  }

  private void RemoveRevisionFromCollection(WTable table)
  {
    if (table.TableFormat.Revisions.Count > 0)
      this.RemoveRevisions(table.TableFormat.Revisions);
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      if (row.RowFormat.Revisions.Count > 0)
        this.RemoveRevisions(row.RowFormat.Revisions);
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if (cell.CellFormat.Revisions.Count > 0)
          this.RemoveRevisions(cell.CellFormat.Revisions);
        this.RemoveRevisionFromCollection((WTextBody) cell);
      }
    }
  }

  private void RemoveRevisions(List<Revision> revisions)
  {
    foreach (Revision revision in revisions)
    {
      if (this.Revisions.InnerList.Contains((object) revision))
        this.Revisions.Remove(revision);
    }
  }

  internal void CharacterFormatChange(
    WCharacterFormat charFormat,
    ParagraphItem item,
    WordReaderBase reader)
  {
    if (charFormat.HasKey(105))
      this.CharFormatChangeRevision(charFormat, item);
    if (item != null)
      return;
    if (charFormat.IsInsertRevision)
      this.BreakCharacterFormatRevision(RevisionType.Insertions, charFormat, (Revision) null, reader);
    if (!charFormat.IsDeleteRevision)
      return;
    this.BreakCharacterFormatRevision(RevisionType.Deletions, charFormat, (Revision) null, reader);
  }

  internal void ParagraphFormatChange(WParagraphFormat paraFormat)
  {
    if (!paraFormat.HasKey(65))
      return;
    this.ParaFormatChangeRevision(paraFormat);
  }

  internal void SectionFormatChange(WSection section)
  {
    if (!section.SectionFormat.HasKey(4))
      return;
    this.SectionFormatChangeRevision(section);
  }

  internal void UpdateTableRevision(WTable table)
  {
    Revision newRevision = this.m_doc.CreateNewRevision(RevisionType.Formatting, table.DocxTableFormat.Format.FormatChangeAuthorName, table.DocxTableFormat.Format.FormatChangeDateTime, (string) null);
    table.DocxTableFormat.Format.Revisions.Add(newRevision);
    newRevision.Range.Items.Add((object) table.DocxTableFormat.Format);
  }

  internal static Encoding GetEncoding(string codePageName) => Encoding.GetEncoding(codePageName);

  public WTableStyle AddTableStyle(string styleName)
  {
    return this.AddStyle(StyleType.TableStyle, styleName) as WTableStyle;
  }
}
