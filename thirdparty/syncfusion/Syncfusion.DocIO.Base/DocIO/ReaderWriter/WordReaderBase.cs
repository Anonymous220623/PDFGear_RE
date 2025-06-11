// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordReaderBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal abstract class WordReaderBase : IWordReaderBase
{
  private const int INVALID_CHUNK_LENGTH = -1;
  private const int DEF_WORD9_DOP_LEN = 544;
  private const int DEF_WORD10_DOP_LEN = 594;
  private const int DEF_WORD11_DOP_LEN = 616;
  public StreamsManager m_streamsManager;
  public DocInfo m_docInfo;
  protected WordStyleSheet m_styleSheet;
  protected string m_textChunk = string.Empty;
  protected WordChunkType m_chunkType;
  protected int m_currStyleIndex;
  protected StatePositionsBase m_statePositions;
  protected WordSubdocument m_type;
  protected int m_startTextPos;
  protected int m_endTextPos;
  private ParagraphPropertyException m_papx;
  private CharacterPropertyException m_chpx;
  private BookmarkInfo[] m_bookmarks;
  private long m_iSavedStreamPosition = -1;
  private bool m_bStreamPosSaved;
  private BookmarkInfo m_currentBookmark;
  private BookmarkInfo m_bookmarkAfterParaEnd;
  private bool m_isBookmarkStart;
  private bool m_isBKMKStartAfterParaEnd;
  private int m_cellCounter;
  private bool m_isCellMark;
  private Dictionary<int, string> m_sttbFRAuthorNames;
  private Stack<Dictionary<WTableRow, short>> m_tableRowWidthStack;
  private List<short> m_tableMaxRowWidth;

  public WordReaderBase(StreamsManager streamsManager) => this.m_streamsManager = streamsManager;

  protected WordReaderBase()
  {
  }

  public int CurrentStyleIndex => this.m_currStyleIndex;

  public WordStyleSheet StyleSheet => this.m_styleSheet;

  public WordChunkType ChunkType => this.m_chunkType;

  public string TextChunk
  {
    get => this.m_textChunk;
    set => this.m_textChunk = value;
  }

  public CharacterPropertyException CHPX => this.m_chpx;

  internal SinglePropertyModifierArray CHPXSprms
  {
    get => this.m_chpx != null ? this.m_chpx.PropertyModifiers : (SinglePropertyModifierArray) null;
  }

  public ParagraphPropertyException PAPX => this.m_papx;

  internal SinglePropertyModifierArray PAPXSprms
  {
    get => this.m_papx != null ? this.m_papx.PropertyModifiers : (SinglePropertyModifierArray) null;
  }

  public ListInfo ListInfo => this.m_docInfo.TablesData.ListInfo;

  public bool HasTableBody => this.PAPXSprms.GetBoolean(9238, false);

  public EscherClass Escher
  {
    get => this.m_docInfo.TablesData.Escher;
    set => this.m_docInfo.TablesData.Escher = value;
  }

  public Fields Fields => this.m_docInfo.TablesData.Fields;

  public WPTablesData TablesData => this.m_docInfo.TablesData;

  public int CurrentTextPosition
  {
    get
    {
      return (int) this.m_docInfo.TablesData.ConvertFCToCP((uint) this.m_streamsManager.MainStream.Position);
    }
    set
    {
      this.m_streamsManager.MainStream.Position = (long) this.m_docInfo.TablesData.ConvertCharPosToFileCharPos((uint) value);
    }
  }

  public BookmarkInfo[] Bookmarks
  {
    get
    {
      if (this.m_bookmarks == null)
        this.m_bookmarks = this.m_docInfo.TablesData.GetBookmarks();
      return this.m_bookmarks;
    }
    set => this.m_bookmarks = value;
  }

  public BookmarkInfo CurrentBookmark
  {
    get => this.m_currentBookmark;
    set => this.m_currentBookmark = value;
  }

  public BookmarkInfo BookmarkAfterParaEnd
  {
    get => this.m_bookmarkAfterParaEnd;
    set => this.m_bookmarkAfterParaEnd = value;
  }

  public bool IsBKMKStartAfterParaEnd
  {
    get => this.m_isBKMKStartAfterParaEnd;
    set => this.m_isBKMKStartAfterParaEnd = value;
  }

  public bool IsBookmarkStart => this.m_isBookmarkStart;

  public DocumentVersion Version => this.GetDocVersion();

  protected Encoding Encoding
  {
    get => this.m_docInfo.TablesData.GetEncodingByFC(this.m_streamsManager.MainStream.Position);
  }

  protected internal int EncodingCharSize
  {
    get
    {
      int encodingCharSize = 1;
      if (this.Encoding == Encoding.ASCII || this.Encoding == Encoding.UTF8)
        encodingCharSize = 1;
      else if (this.Encoding == Encoding.Unicode)
        encodingCharSize = 2;
      return encodingCharSize;
    }
  }

  protected WordStyle CurrentStyle => this.StyleSheet.GetStyleByIndex(this.CurrentStyleIndex);

  internal int StartTextPos => this.m_startTextPos;

  internal int EndTextPos => this.m_endTextPos;

  public Dictionary<int, string> SttbfRMarkAuthorNames
  {
    get
    {
      if (this.m_docInfo.TablesData.SttbfRMark != null && this.m_sttbFRAuthorNames == null)
        this.m_sttbFRAuthorNames = this.GetSTTBFRNames(this.m_docInfo.TablesData.SttbfRMark);
      return this.m_sttbFRAuthorNames;
    }
  }

  public Stack<Dictionary<WTableRow, short>> TableRowWidthStack
  {
    get
    {
      if (this.m_tableRowWidthStack == null)
        this.m_tableRowWidthStack = new Stack<Dictionary<WTableRow, short>>();
      return this.m_tableRowWidthStack;
    }
  }

  public List<short> MaximumTableRowWidth
  {
    get
    {
      if (this.m_tableMaxRowWidth == null)
        this.m_tableMaxRowWidth = new List<short>();
      return this.m_tableMaxRowWidth;
    }
  }

  public virtual WordChunkType ReadChunk()
  {
    this.UnfreezeStreamPos();
    this.m_textChunk = string.Empty;
    this.m_chunkType = WordChunkType.Text;
    int chunkLength = this.CalculateChunkLength();
    if (this.m_chunkType != WordChunkType.DocumentEnd && this.m_chunkType != WordChunkType.EndOfSubdocText && chunkLength > 0)
    {
      this.m_startTextPos = this.m_endTextPos;
      this.m_statePositions.CurrentTextPosition += this.ReadAndParseTextChunk(chunkLength);
      this.m_endTextPos = this.m_startTextPos + this.m_textChunk.Length;
      this.UpdateChunkType();
    }
    bool flag = false;
    foreach (char ch in this.m_textChunk)
    {
      if (ch == '(')
      {
        flag = true;
      }
      else
      {
        flag = false;
        break;
      }
    }
    if (flag)
      this.m_chunkType = this.CHPXSprms[27145] == null ? WordChunkType.Text : WordChunkType.Symbol;
    return this.m_chunkType;
  }

  public virtual IWordImageReader GetImageReader(WordDocument doc)
  {
    return (IWordImageReader) this.m_docInfo.GetImageReader(this.m_streamsManager, this.GetPicLocation(), doc);
  }

  public virtual FileShapeAddress GetFSPA() => (FileShapeAddress) null;

  public Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase GetDrawingObject()
  {
    this.UnfreezeStreamPos();
    FileShapeAddress fspa = this.GetFSPA();
    if (fspa == null)
      return (Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase) null;
    MsofbtSpContainer spContainer = (MsofbtSpContainer) null;
    if (this.Escher.Containers.ContainsKey(fspa.Spid))
      spContainer = this.Escher.Containers[fspa.Spid] as MsofbtSpContainer;
    Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase drawingObject = (Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase) null;
    if (spContainer != null)
    {
      switch (spContainer.Shape.ShapeType)
      {
        case EscherShapeType.msosptPictureFrame:
          drawingObject = (Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase) this.ReadPictureProps(spContainer, fspa);
          break;
        case EscherShapeType.msosptTextBox:
          drawingObject = (Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase) this.ReadTextBoxProps(spContainer, fspa);
          break;
      }
    }
    return drawingObject;
  }

  public FormField GetFormField(FieldType fieldType)
  {
    this.m_streamsManager.DataStream.Position = (long) this.GetPicLocation();
    return new FormField(fieldType, this.m_streamsManager.DataReader);
  }

  public virtual FieldDescriptor GetFld() => throw new NotImplementedException();

  public bool ReadWatermark(WordDocument doc, WTextBody m_textBody)
  {
    bool flag = false;
    HeaderFooter header = (HeaderFooter) null;
    if (m_textBody != null & m_textBody is HeaderFooter)
      header = m_textBody as HeaderFooter;
    if (this is WordHeaderFooterReader)
    {
      this.UnfreezeStreamPos();
      FileShapeAddress fspa = this.GetFSPA();
      if (fspa == null)
        return false;
      MsofbtSpContainer spContainer = (MsofbtSpContainer) null;
      if (this.Escher.Containers.ContainsKey(fspa.Spid))
        spContainer = this.Escher.Containers[fspa.Spid] as MsofbtSpContainer;
      if (spContainer != null && this.IsWatermark(spContainer))
      {
        flag = true;
        if (header != null && header.Watermark != null && header.Watermark.Type == WatermarkType.NoWatermark)
        {
          if (spContainer.Shape.ShapeType == EscherShapeType.msosptTextPlainText)
          {
            this.ReadTextWatermark(spContainer, doc, fspa, header);
            (header.Watermark as TextWatermark).Height = (float) (fspa.Height / 20);
            (header.Watermark as TextWatermark).Width = (float) (fspa.Width / 20);
            doc.SetTriggerElement(ref doc.m_supportedElementFlag_2, 17);
          }
          else if (spContainer.Shape.ShapeType == EscherShapeType.msosptPictureFrame || spContainer.Shape.ShapeType == EscherShapeType.msosptCustomShape)
          {
            this.ReadPictureWatermark(spContainer, doc, fspa, header);
            doc.SetTriggerElement(ref doc.m_supportedElementFlag_2, 0);
          }
          header.Watermark.ShapeId = fspa.Spid;
        }
        else
          this.Escher.RemoveContainerBySpid(spContainer.Shape.ShapeId, true);
      }
    }
    return flag;
  }

  public BookmarkInfo[] GetBookmarks()
  {
    this.m_bookmarks = this.m_docInfo.TablesData.GetBookmarks();
    return this.m_bookmarks;
  }

  public bool SubdocumentExist()
  {
    bool flag = true;
    long position = this.m_streamsManager.MainStream.Position;
    if (this.m_statePositions.IsEndOfText(position) || this.GetChunkEndPosition(position) < 0L)
      flag = false;
    return flag;
  }

  internal virtual void Close()
  {
    if (this.m_streamsManager != null)
    {
      this.m_streamsManager.CloseStg();
      this.m_streamsManager = (StreamsManager) null;
    }
    if (this.m_docInfo != null)
    {
      this.m_docInfo.Close();
      this.m_docInfo = (DocInfo) null;
    }
    this.m_styleSheet = (WordStyleSheet) null;
    this.m_statePositions = (StatePositionsBase) null;
    this.m_chpx = (CharacterPropertyException) null;
    this.m_papx = (ParagraphPropertyException) null;
    this.m_bookmarks = (BookmarkInfo[]) null;
    this.m_currentBookmark = (BookmarkInfo) null;
    this.m_bookmarkAfterParaEnd = (BookmarkInfo) null;
    if (this.m_tableRowWidthStack != null)
    {
      this.m_tableRowWidthStack.Clear();
      this.m_tableRowWidthStack = (Stack<Dictionary<WTableRow, short>>) null;
    }
    if (this.m_tableMaxRowWidth == null)
      return;
    this.m_tableMaxRowWidth.Clear();
    this.m_tableMaxRowWidth = (List<short>) null;
  }

  public virtual void FreezeStreamPos()
  {
    if (this.m_bStreamPosSaved)
      return;
    this.m_iSavedStreamPosition = this.m_streamsManager.MainStream.Position;
    this.m_bStreamPosSaved = true;
  }

  public virtual void UnfreezeStreamPos()
  {
    if (!this.m_bStreamPosSaved)
      return;
    this.m_streamsManager.MainStream.Position = this.m_iSavedStreamPosition;
    this.m_bStreamPosSaved = false;
  }

  public bool HasList() => this.m_docInfo.TablesData.HasList();

  internal void RestoreBookmark()
  {
    if (this.m_currentBookmark == null)
      return;
    int index = this.m_currentBookmark.Index;
    if (this.m_isBookmarkStart)
    {
      this.m_bookmarks[index].StartPos = this.m_currentBookmark.StartPos;
      this.m_bookmarks[index].StartCellIndex = this.m_currentBookmark.StartCellIndex;
    }
    else
    {
      this.m_bookmarks[index].EndPos = this.m_currentBookmark.EndPos;
      this.m_bookmarks[index].EndCellIndex = this.m_currentBookmark.EndCellIndex;
    }
    this.m_currentBookmark = (BookmarkInfo) null;
  }

  protected void UpdateBookmarks()
  {
    if (this.m_bookmarks != null)
      return;
    this.m_bookmarks = this.m_docInfo.TablesData.GetBookmarks();
  }

  protected virtual void InitClass()
  {
    if (this.m_styleSheet == null)
      this.m_styleSheet = new WordStyleSheet();
    this.m_currStyleIndex = this.m_styleSheet.DefaultStyleIndex;
  }

  protected virtual long GetChunkEndPosition(long iCurrentPos)
  {
    if (!this.m_statePositions.IsFirstPass(iCurrentPos))
      this.UpdateEndPositions(iCurrentPos);
    return this.m_statePositions.GetMinEndPos(iCurrentPos);
  }

  protected virtual void UpdateEndPositions(long iEndPos)
  {
    if (this.m_statePositions.UpdateCHPxEndPos(iEndPos))
      this.UpdateCharacterProperties();
    if (!this.m_statePositions.UpdatePAPxEndPos(iEndPos))
      return;
    this.UpdateParagraphProperties();
  }

  protected virtual void UpdateChunkType()
  {
    if (this.m_textChunk.Length > 1)
      this.m_chunkType = WordChunkType.Text;
    else if (this.m_textChunk.Length == 1)
    {
      switch (this.m_textChunk[0])
      {
        case char.MinValue:
          this.m_chunkType = WordChunkType.CurrentPageNumber;
          break;
        case '\u0001':
          this.m_chunkType = WordChunkType.Image;
          break;
        case '\u0002':
          this.m_chunkType = WordChunkType.Footnote;
          break;
        case '\u0005':
          this.m_chunkType = WordChunkType.Annotation;
          break;
        case '\a':
          if (this.PAPXSprms.GetBoolean(9239, false) && this.PAPXSprms.GetBoolean(9238, false) || this.PAPXSprms.GetByteArray(29706) != null)
          {
            this.m_chunkType = WordChunkType.TableRow;
            break;
          }
          if (this.PAPXSprms.GetBoolean(9238, false))
          {
            this.m_chunkType = WordChunkType.TableCell;
            break;
          }
          if (this.m_streamsManager.MainStream.Position == this.m_statePositions.m_iEndPAPxPos)
          {
            this.m_chunkType = WordChunkType.ParagraphEnd;
            break;
          }
          this.m_chunkType = WordChunkType.Table;
          break;
        case '\b':
          this.m_chunkType = WordChunkType.Shape;
          break;
        case '\v':
          this.m_chunkType = WordChunkType.LineBreak;
          break;
        case '\f':
          int[] positions = this.m_docInfo.FkpData.Tables.SectionsTable.Positions;
          int charPos = 0;
          if (this.m_statePositions is MainStatePositions)
            charPos = positions[(this.m_statePositions as MainStatePositions).SectionIndex + 1];
          else if (this.m_statePositions is HFStatePositions)
            charPos = positions[(this.m_statePositions as HFStatePositions).SectionIndex + 1];
          if ((long) this.m_docInfo.FkpData.Tables.ConvertCharPosToFileCharPos((uint) charPos) == this.m_streamsManager.MainStream.Position)
          {
            this.m_chunkType = WordChunkType.SectionEnd;
            break;
          }
          this.m_chunkType = WordChunkType.PageBreak;
          break;
        case '\r':
          if (this.PAPXSprms.GetInt(26185, 1) > 1)
          {
            if (this.PAPXSprms.GetBoolean(9292, false))
            {
              this.m_chunkType = WordChunkType.TableRow;
              break;
            }
            if (this.PAPXSprms.GetByte(9291, (byte) 0) == (byte) 1)
            {
              if (this.m_streamsManager.MainStream.Position >= this.m_statePositions.m_iEndPAPxPos)
              {
                this.m_chunkType = WordChunkType.TableCell;
                break;
              }
              this.m_chunkType = WordChunkType.Text;
              break;
            }
            this.m_chunkType = WordChunkType.ParagraphEnd;
            break;
          }
          if (Array.BinarySearch<int>(this.m_docInfo.TablesData.SectionsTable.Positions, this.CurrentTextPosition) > 0 && this.CurrentTextPosition != this.m_docInfo.Fib.CcpText)
          {
            this.m_chunkType = WordChunkType.SectionEnd;
            break;
          }
          this.m_chunkType = WordChunkType.ParagraphEnd;
          break;
        case '\u000E':
          this.m_chunkType = WordChunkType.ColumnBreak;
          break;
        case '\u0013':
          this.m_chunkType = WordChunkType.FieldBeginMark;
          break;
        case '\u0014':
          this.m_chunkType = WordChunkType.FieldSeparator;
          break;
        case '\u0015':
          this.m_chunkType = WordChunkType.FieldEndMark;
          break;
        case '(':
          if (this.CHPXSprms[27145] != null)
          {
            this.m_chunkType = WordChunkType.Symbol;
            break;
          }
          this.m_chunkType = WordChunkType.Text;
          break;
        default:
          this.m_chunkType = WordChunkType.Text;
          break;
      }
    }
    else
    {
      if (this.m_textChunk.Length != 0)
        return;
      this.m_chunkType = WordChunkType.Text;
    }
  }

  protected void UpdateCharacterProperties() => this.m_chpx = this.m_statePositions.CurrentChpx;

  protected void UpdateParagraphProperties()
  {
    ParagraphPropertyException currentPapx = this.m_statePositions.CurrentPapx;
    ParagraphPropertyException propertyException = (ParagraphPropertyException) null;
    if (currentPapx.PropertyModifiers[26181] != null || currentPapx.PropertyModifiers[26182] != null)
    {
      this.m_streamsManager.DataStream.Position = (long) BitConverter.ToInt32(currentPapx.PropertyModifiers.GetByteArray(26182) ?? currentPapx.PropertyModifiers.GetByteArray(26181), 0);
      byte[] buffer = new byte[2];
      this.m_streamsManager.DataStream.Read(buffer, 0, buffer.Length);
      propertyException = new ParagraphPropertyException((Stream) this.m_streamsManager.DataStream, (int) BitConverter.ToInt16(buffer, 0), true);
    }
    if (propertyException == null)
      propertyException = currentPapx;
    this.m_currStyleIndex = (int) currentPapx.StyleIndex;
    WordStyle currentStyle = this.CurrentStyle;
    this.m_papx = propertyException;
  }

  protected void ReadChunkString(int length)
  {
    byte[] numArray = length >= 1 ? new byte[length] : throw new ArgumentOutOfRangeException("length must be larger than 0");
    this.m_streamsManager.MainStream.Read(numArray, 0, length);
    this.m_textChunk = this.Encoding.GetString(numArray);
  }

  protected int CalcCP(int startPos, int length)
  {
    return (int) ((long) (this.m_docInfo.TablesData.ConvertFCToCP((uint) this.m_streamsManager.MainStream.Position) - this.m_docInfo.TablesData.ConvertFCToCP((uint) startPos)) - (long) length);
  }

  private bool IsWatermark(MsofbtSpContainer spContainer)
  {
    bool flag = false;
    byte[] complexPropValue = spContainer.GetComplexPropValue(896);
    if (complexPropValue != null)
    {
      string str = Encoding.Unicode.GetString(complexPropValue);
      if (str.StartsWithExt("WordPictureWatermark") || str.StartsWithExt("PowerPlusWaterMarkObject"))
        flag = true;
    }
    return flag;
  }

  private TextBoxShape ReadTextBoxProps(MsofbtSpContainer spContainer, FileShapeAddress fspa)
  {
    TextBoxShape shape = new TextBoxShape();
    this.InitBaseShapeProps(fspa, (Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase) shape, spContainer);
    this.InitTextBoxProps(shape, spContainer);
    return shape;
  }

  private PictureShape ReadPictureProps(MsofbtSpContainer spContainer, FileShapeAddress fspa)
  {
    _Blip fromShapeContainer = MsofbtSpContainer.GetBlipFromShapeContainer((BaseEscherRecord) spContainer);
    PictureShape pictureShape = (PictureShape) null;
    if (fromShapeContainer != null)
    {
      try
      {
        pictureShape = new PictureShape(fromShapeContainer.ImageRecord);
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException("Document image format is incorrect.");
      }
      this.InitBaseShapeProps(fspa, (Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase) pictureShape, spContainer);
      this.InitPictureProps(pictureShape, spContainer);
    }
    return pictureShape;
  }

  private void ReadTextWatermark(
    MsofbtSpContainer spContainer,
    WordDocument doc,
    FileShapeAddress fspa,
    HeaderFooter header)
  {
    TextWatermark textWatermark = header.InsertWatermark(WatermarkType.TextWatermark) as TextWatermark;
    textWatermark.SetOwner((OwnerHolder) header);
    byte[] complexPropValue1 = spContainer.GetComplexPropValue(192 /*0xC0*/);
    if (complexPropValue1 != null)
      textWatermark.Text = Encoding.Unicode.GetString(complexPropValue1);
    textWatermark.Text = textWatermark.Text == null ? string.Empty : textWatermark.Text.Replace("\0", string.Empty);
    uint propertyValue1 = spContainer.GetPropertyValue(195);
    if (propertyValue1 != uint.MaxValue)
      textWatermark.Size = (float) (propertyValue1 >> 16 /*0x10*/);
    byte[] complexPropValue2 = spContainer.GetComplexPropValue(197);
    if (complexPropValue2 != null)
      textWatermark.FontName = Encoding.Unicode.GetString(complexPropValue2);
    textWatermark.FontName = textWatermark.FontName == null ? string.Empty : textWatermark.FontName.Replace("\0", string.Empty);
    uint propertyValue2 = spContainer.GetPropertyValue(385);
    if (propertyValue2 != uint.MaxValue)
      textWatermark.Color = WordColor.ConvertRGBToColor(propertyValue2);
    if (spContainer.GetPropertyValue(386) == uint.MaxValue)
      textWatermark.Semitransparent = false;
    uint propertyValue3 = spContainer.GetPropertyValue(4);
    if (propertyValue3 == uint.MaxValue)
      textWatermark.Layout = WatermarkLayout.Horizontal;
    else
      textWatermark.Rotation = (int) propertyValue3 / 65536 /*0x010000*/;
    this.UpdateTextWatermarkPositions(textWatermark, spContainer, fspa);
    if (doc.Watermark == null || doc.Watermark.Type != WatermarkType.NoWatermark)
      return;
    doc.Watermark = (Watermark) textWatermark;
  }

  private void UpdateTextWatermarkPositions(
    TextWatermark textWatermark,
    MsofbtSpContainer spContainer,
    FileShapeAddress fspa)
  {
    textWatermark.VerticalPosition = (float) fspa.YaTop / 20f;
    textWatermark.HorizontalPosition = (float) fspa.XaLeft / 20f;
    if (spContainer.ShapePosition.YRelTo != uint.MaxValue)
      textWatermark.VerticalOrigin = (VerticalOrigin) spContainer.ShapePosition.YRelTo;
    if (spContainer.ShapePosition.XRelTo != uint.MaxValue)
      textWatermark.HorizontalOrigin = (HorizontalOrigin) spContainer.ShapePosition.XRelTo;
    if (spContainer.ShapePosition.XAlign != uint.MaxValue)
      textWatermark.HorizontalAlignment = (ShapeHorizontalAlignment) spContainer.ShapePosition.XAlign;
    if (spContainer.ShapePosition.YAlign != uint.MaxValue)
      textWatermark.VerticalAlignment = (ShapeVerticalAlignment) spContainer.ShapePosition.YAlign;
    textWatermark.TextWrappingStyle = fspa.TextWrappingStyle;
  }

  private void ReadPictureWatermark(
    MsofbtSpContainer spContainer,
    WordDocument doc,
    FileShapeAddress fspa,
    HeaderFooter header)
  {
    PictureWatermark pictureWatermark = header.InsertWatermark(WatermarkType.PictureWatermark) as PictureWatermark;
    pictureWatermark.SetOwner((OwnerHolder) header);
    if (spContainer.Pib > 0 && this.Escher.m_msofbtDggContainer.BstoreContainer.Children[spContainer.Pib - 1] is MsofbtBSE child && child.Blip != null)
    {
      pictureWatermark.WordPicture.LoadImage(child.Blip.ImageRecord);
      pictureWatermark.OriginalPib = spContainer.Pib;
      this.ApplyShapeProperties(pictureWatermark.WordPicture, fspa, spContainer.ShapePosition);
    }
    uint propertyValue1 = spContainer.GetPropertyValue(265);
    uint propertyValue2 = spContainer.GetPropertyValue(264);
    pictureWatermark.Washout = propertyValue1 != uint.MaxValue || propertyValue2 != uint.MaxValue;
    if (doc.Watermark == null || doc.Watermark.Type != WatermarkType.NoWatermark)
      return;
    doc.Watermark = (Watermark) pictureWatermark;
  }

  private void ApplyShapeProperties(
    WPicture picture,
    FileShapeAddress fspa,
    MsofbtTertiaryFOPT shapePosition)
  {
    picture.Height = (float) fspa.Height / 20f;
    picture.Width = (float) fspa.Width / 20f;
    picture.VerticalPosition = (float) fspa.YaTop / 20f;
    picture.HorizontalPosition = (float) fspa.XaLeft / 20f;
    if (shapePosition.YRelTo != uint.MaxValue)
      picture.VerticalOrigin = (VerticalOrigin) shapePosition.YRelTo;
    if (shapePosition.XRelTo != uint.MaxValue)
      picture.HorizontalOrigin = (HorizontalOrigin) shapePosition.XRelTo;
    if (shapePosition.XAlign != uint.MaxValue)
      picture.HorizontalAlignment = (ShapeHorizontalAlignment) shapePosition.XAlign;
    if (shapePosition.YAlign != uint.MaxValue)
      picture.VerticalAlignment = (ShapeVerticalAlignment) shapePosition.YAlign;
    picture.SetTextWrappingStyleValue(fspa.TextWrappingStyle);
    picture.TextWrappingType = fspa.TextWrappingType;
    picture.IsBelowText = fspa.IsBelowText;
    picture.ShapeId = fspa.Spid;
  }

  private void InitBaseShapeProps(
    FileShapeAddress fspa,
    Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase shape,
    MsofbtSpContainer container)
  {
    shape.ShapeProps.Height = fspa.Height;
    shape.ShapeProps.RelHrzPos = fspa.RelHrzPos;
    shape.ShapeProps.RelVrtPos = fspa.RelVrtPos;
    shape.ShapeProps.Spid = fspa.Spid;
    shape.ShapeProps.TextWrappingStyle = fspa.TextWrappingStyle;
    shape.ShapeProps.TextWrappingType = fspa.TextWrappingType;
    shape.ShapeProps.Width = fspa.Width;
    shape.ShapeProps.XaLeft = fspa.XaLeft;
    shape.ShapeProps.XaRight = fspa.XaRight;
    shape.ShapeProps.YaBottom = fspa.YaBottom;
    shape.ShapeProps.YaTop = fspa.YaTop;
    shape.ShapeProps.Spid = fspa.Spid;
    shape.ShapeProps.IsHeaderShape = this is WordHeaderFooterReader;
    if (container.ShapePosition != null)
    {
      switch (container.ShapePosition.XAlign)
      {
        case 1:
          shape.ShapeProps.HorizontalAlignment = ShapeHorizontalAlignment.Left;
          break;
        case 2:
          shape.ShapeProps.HorizontalAlignment = ShapeHorizontalAlignment.Center;
          break;
        case 3:
          shape.ShapeProps.HorizontalAlignment = ShapeHorizontalAlignment.Right;
          break;
      }
      switch (container.ShapePosition.YAlign)
      {
        case 1:
          shape.ShapeProps.VerticalAlignment = ShapeVerticalAlignment.Top;
          break;
        case 2:
          shape.ShapeProps.VerticalAlignment = ShapeVerticalAlignment.Center;
          break;
        case 3:
          shape.ShapeProps.VerticalAlignment = ShapeVerticalAlignment.Bottom;
          break;
        case 4:
          shape.ShapeProps.VerticalAlignment = ShapeVerticalAlignment.Inside;
          break;
        case 5:
          shape.ShapeProps.VerticalAlignment = ShapeVerticalAlignment.Outside;
          break;
      }
      if (container.ShapePosition.XRelTo != uint.MaxValue)
        shape.ShapeProps.RelHrzPos = (HorizontalOrigin) container.ShapePosition.XRelTo;
      if (container.ShapePosition.YRelTo != uint.MaxValue)
        shape.ShapeProps.RelVrtPos = (VerticalOrigin) container.ShapePosition.YRelTo;
    }
    uint propertyValue = container.GetPropertyValue(959);
    if (propertyValue != uint.MaxValue)
      shape.ShapeProps.IsBelowText = ((int) propertyValue & 32 /*0x20*/) == 32 /*0x20*/;
    else
      shape.ShapeProps.IsBelowText = false;
  }

  private void InitTextBoxProps(TextBoxShape shape, MsofbtSpContainer container)
  {
    uint propertyValue1 = container.GetPropertyValue(459);
    if (propertyValue1 != uint.MaxValue)
      shape.TextBoxProps.TxbxLineWidth = (float) propertyValue1 / 12700f;
    uint propertyValue2 = container.GetPropertyValue(461);
    if (propertyValue2 != uint.MaxValue)
      shape.TextBoxProps.LineStyle = (TextBoxLineStyle) propertyValue2;
    uint propertyValue3 = container.GetPropertyValue(462);
    if (propertyValue3 != uint.MaxValue)
      shape.TextBoxProps.LineDashing = (LineDashing) propertyValue3;
    uint propertyValue4 = container.GetPropertyValue(133);
    if (propertyValue4 != uint.MaxValue)
      shape.TextBoxProps.WrapText = (WrapMode) propertyValue4;
    uint propertyValue5 = container.GetPropertyValue(385);
    if (propertyValue5 != uint.MaxValue)
      shape.TextBoxProps.FillColor = WordColor.ConvertRGBToColor(propertyValue5);
    uint propertyValue6 = container.GetPropertyValue(448);
    if (propertyValue6 != uint.MaxValue)
      shape.TextBoxProps.LineColor = WordColor.ConvertRGBToColor(propertyValue6);
    if (((int) container.GetPropertyValue(447) & 16 /*0x10*/) != 16 /*0x10*/)
      shape.TextBoxProps.FillColor = Color.Empty;
    uint propertyValue7 = container.GetPropertyValue(511 /*0x01FF*/);
    if (propertyValue7 != uint.MaxValue)
      shape.TextBoxProps.NoLine = ((int) propertyValue7 & 8) == 0;
    uint propertyValue8 = container.GetPropertyValue(191);
    if (propertyValue8 != uint.MaxValue)
      shape.TextBoxProps.FitShapeToText = ((int) propertyValue8 & 2) != 0;
    shape.TextBoxProps.TXID = (float) container.GetPropertyValue(128 /*0x80*/);
    if (container.ShapePosition != null)
    {
      if (container.ShapePosition.XAlign != uint.MaxValue)
        shape.TextBoxProps.HorizontalAlignment = (ShapeHorizontalAlignment) container.ShapePosition.XAlign;
      if (container.ShapePosition.YAlign != uint.MaxValue)
        shape.TextBoxProps.VerticalAlignment = (ShapeVerticalAlignment) container.ShapePosition.YAlign;
      if (container.ShapePosition.XRelTo != uint.MaxValue)
        shape.TextBoxProps.RelHrzPos = (HorizontalOrigin) container.ShapePosition.XRelTo;
      if (container.ShapePosition.YRelTo != uint.MaxValue)
        shape.TextBoxProps.RelVrtPos = (VerticalOrigin) container.ShapePosition.YRelTo;
    }
    uint propertyValue9 = container.GetPropertyValue(129);
    if (propertyValue9 != uint.MaxValue)
      shape.TextBoxProps.LeftMargin = propertyValue9;
    uint propertyValue10 = container.GetPropertyValue(131);
    if (propertyValue10 != uint.MaxValue)
      shape.TextBoxProps.RightMargin = propertyValue10;
    uint propertyValue11 = container.GetPropertyValue(130);
    if (propertyValue11 != uint.MaxValue)
      shape.TextBoxProps.TopMargin = propertyValue11;
    uint propertyValue12 = container.GetPropertyValue(132);
    if (propertyValue12 == uint.MaxValue)
      return;
    shape.TextBoxProps.BottomMargin = propertyValue12;
  }

  private void InitPictureProps(PictureShape pictShape, MsofbtSpContainer spContainer)
  {
    byte[] complexPropValue1 = spContainer.GetComplexPropValue(897);
    if (complexPropValue1 != null)
      pictShape.PictureProps.AlternativeText = Encoding.Unicode.GetString(complexPropValue1).Replace("\0", string.Empty);
    byte[] complexPropValue2 = spContainer.GetComplexPropValue(896);
    if (complexPropValue2 == null)
      return;
    pictShape.PictureProps.Name = Encoding.Unicode.GetString(complexPropValue2).Replace("\0", string.Empty);
  }

  private int ReadAndParseTextChunk(int iLength)
  {
    this.ReadChunkString(iLength);
    if (this.m_textChunk.Length > 1)
    {
      if (this.m_textChunk.Length >= 10 && this.CHPXSprms.GetBoolean(2133, false) && this.CHPXSprms[27139] == null)
      {
        this.m_textChunk = string.Empty;
        return iLength;
      }
      if (this.m_textChunk.Length > 10 && this.IsZeroChunk())
      {
        this.m_textChunk = string.Empty;
        return iLength;
      }
      this.m_textChunk = this.m_textChunk.Trim(new char[1]);
      int length;
      if ((length = this.m_textChunk.IndexOfAny(SpecialCharacters.SpecialSymbolArr)) > -1)
      {
        if (length == 0)
          length = 1;
        this.m_textChunk = this.m_textChunk.Substring(0, length);
        this.m_streamsManager.MainStream.Position -= (long) (iLength - length * this.EncodingCharSize);
        return length;
      }
    }
    return iLength;
  }

  private int CheckSpecCharacters(int iLength)
  {
    int length;
    if ((length = this.m_textChunk.IndexOfAny(SpecialCharacters.SpecialSymbolArr)) > -1)
    {
      if (length == 0)
        length = 1;
      this.m_textChunk = this.m_textChunk.Substring(0, length);
      this.m_streamsManager.MainStream.Position -= (long) (iLength - length * this.EncodingCharSize);
      iLength = length;
    }
    return iLength;
  }

  private bool IsZeroChunk()
  {
    bool flag = false;
    if (this.m_textChunk[0] == char.MinValue)
    {
      flag = true;
      for (int index = 1; index < this.m_textChunk.Length - 1; ++index)
      {
        if ((int) this.m_textChunk[index] != (int) this.m_textChunk[index + 1])
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  private int CalculateChunkLength()
  {
    long num = this.m_streamsManager.MainStream.Position;
    if (this.m_statePositions.IsEndOfText(num))
    {
      this.m_chunkType = WordChunkType.DocumentEnd;
      return 0;
    }
    long chunkEndPosition = this.GetChunkEndPosition(num);
    if (num < this.m_statePositions.m_iStartPieceTablePos)
      num = this.m_streamsManager.MainStream.Position = this.m_statePositions.m_iStartPieceTablePos;
    if (this.m_statePositions.IsEndOfSubdocItemText(num))
    {
      this.m_chunkType = WordChunkType.EndOfSubdocText;
      return 0;
    }
    if (chunkEndPosition < 0L || this.m_statePositions.IsEndOfText(num))
    {
      this.m_chunkType = WordChunkType.DocumentEnd;
      return 0;
    }
    if (this is WordSubdocumentReader && (this as WordSubdocumentReader).IsNextItemPos)
    {
      this.m_chunkType = WordChunkType.Text;
      return 0;
    }
    return this.m_bookmarks != null && this.m_bookmarks.Length != 0 ? this.GetBookmarkChunkLen((long) (int) num, (long) (int) chunkEndPosition) : (int) (chunkEndPosition - num);
  }

  private bool IsNextItem()
  {
    switch (this)
    {
      case WordEndnoteReader _:
        return (this as WordEndnoteReader).IsNextItem;
      case WordFootnoteReader _:
        return (this as WordFootnoteReader).IsNextItem;
      default:
        return false;
    }
  }

  private int GetFldChunkLen(int curPos, int endPos)
  {
    int fldChunkLen = endPos - curPos;
    DocIOSortedList<int, FieldDescriptor> fieldsForSubDoc = this.m_docInfo.TablesData.Fields.GetFieldsForSubDoc(this.m_type);
    if (fieldsForSubDoc != null)
    {
      int currentTextPosition = this.CurrentTextPosition;
      List<int> intList = new List<int>();
      foreach (int key in (IEnumerable<int>) fieldsForSubDoc.Keys)
      {
        if (key >= currentTextPosition || key <= endPos)
          intList.Add(key);
      }
      if (intList.Count > 0)
      {
        intList.Sort();
        int num = intList[0] - currentTextPosition;
        fldChunkLen = num + (num == 0 ? 1 : 0);
      }
    }
    return fldChunkLen;
  }

  private bool CheckCurTextChunk(int curChunkPosLen)
  {
    this.m_isCellMark = false;
    if (curChunkPosLen < 0)
      return false;
    if (this.m_chunkType == WordChunkType.TableCell)
    {
      ++this.m_cellCounter;
      this.m_isCellMark = true;
    }
    else if (this.m_chunkType == WordChunkType.TableRow)
      this.m_cellCounter = 0;
    if (!(this.m_textChunk == string.Empty) && !this.IsZeroChunk() && (this.m_textChunk.Length <= 10 || !this.CHPXSprms.GetBoolean(2133, false) || this.CHPXSprms[27139] != null))
      return true;
    this.m_textChunk = string.Empty;
    return false;
  }

  private int CalculateBkmkChunkLen(int startStreamPos, int endStreamPos)
  {
    int num = (int) this.m_docInfo.TablesData.ConvertCharPosToFileCharPos(this.m_isBookmarkStart ? (uint) this.m_currentBookmark.StartPos : (uint) this.m_currentBookmark.EndPos);
    if (num > endStreamPos)
      num = endStreamPos;
    return num - startStreamPos;
  }

  private int GetBookmarkChunkLen(long curDocStreamPos, long endDocStreamPos)
  {
    this.m_currentBookmark = (BookmarkInfo) null;
    int curChunkPosLen = (int) (endDocStreamPos - curDocStreamPos);
    int curChunkLen = this.CheckAndGetCurChunkLen(curChunkPosLen);
    if (curChunkLen == -1)
      return curChunkPosLen;
    int currentTextPosition = this.CurrentTextPosition;
    int num1 = currentTextPosition + curChunkLen;
    if (this.HasTableBody)
    {
      this.CheckTableBookmark(currentTextPosition, num1);
      if (this.m_currentBookmark != null)
        return !this.m_isCellMark ? 0 : curChunkPosLen;
    }
    this.SetCurrentBookmark(currentTextPosition, num1);
    if (this.m_currentBookmark != null)
    {
      int num2 = (int) this.m_docInfo.TablesData.ConvertCharPosToFileCharPos(this.m_isBookmarkStart ? (uint) this.m_currentBookmark.StartPos : (uint) this.m_currentBookmark.EndPos);
      if ((long) num2 > endDocStreamPos)
        num2 = (int) endDocStreamPos;
      curChunkPosLen = num2 - (int) curDocStreamPos;
    }
    return curChunkPosLen;
  }

  private void CheckTableBookmark(int startTextPos, int endTextPos)
  {
    int bookmarkIndex = -1;
    int index = 0;
    for (int length = this.m_bookmarks.Length; index < length; ++index)
    {
      BookmarkInfo bookmark = this.m_bookmarks[index];
      if ((int) bookmark.StartCellIndex == this.m_cellCounter && this.m_isCellMark)
      {
        if (bookmark.StartPos <= startTextPos && bookmark.EndPos > startTextPos)
        {
          this.m_isBookmarkStart = true;
          bookmarkIndex = index;
          break;
        }
      }
      else if ((int) bookmark.EndCellIndex == this.m_cellCounter - 1 && this.m_isCellMark && bookmark.EndPos <= endTextPos && bookmark.StartCellIndex == (short) -1)
      {
        this.m_isBookmarkStart = false;
        bookmarkIndex = index;
        break;
      }
    }
    this.DisableBookmark(bookmarkIndex);
  }

  private void SetCurrentBookmark(int startPos, int endPos)
  {
    int bookmarkIndex = -1;
    int num1 = int.MaxValue;
    for (int index = 0; index < this.m_bookmarks.Length; ++index)
    {
      BookmarkInfo bookmark = this.m_bookmarks[index];
      bool flag = true;
      while (true)
      {
        int num2 = flag ? bookmark.StartPos : bookmark.EndPos;
        if (num2 != int.MaxValue && num2 >= startPos && num2 < num1 && num2 <= endPos)
        {
          num1 = num2;
          bookmarkIndex = index;
          this.m_isBookmarkStart = flag;
        }
        if (flag)
          flag = false;
        else
          break;
      }
    }
    this.DisableBookmark(bookmarkIndex);
  }

  private void DisableBookmark(int bookmarkIndex)
  {
    if (bookmarkIndex != -1)
    {
      BookmarkInfo bookmark = this.m_bookmarks[bookmarkIndex];
      this.m_currentBookmark = bookmark.Clone();
      if (this.m_isBookmarkStart)
      {
        bookmark.StartPos = int.MaxValue;
        bookmark.StartCellIndex = (short) -1;
      }
      else
      {
        bookmark.EndPos = int.MaxValue;
        bookmark.EndCellIndex = (short) -1;
      }
    }
    else
      this.m_currentBookmark = (BookmarkInfo) null;
  }

  private int CheckAndGetCurChunkLen(int curChunkPosLen)
  {
    this.m_isCellMark = false;
    int curChunkLen1 = -1;
    if (curChunkPosLen < 1)
      return curChunkLen1;
    long position = this.m_streamsManager.MainStream.Position;
    this.ReadChunkString(curChunkPosLen);
    this.UpdateChunkType();
    if (this.m_chunkType == WordChunkType.TableCell)
    {
      ++this.m_cellCounter;
      this.m_isCellMark = true;
    }
    else if (this.m_chunkType == WordChunkType.TableRow)
      this.m_cellCounter = 0;
    int curChunkLen2;
    if (this.m_textChunk == string.Empty || this.IsZeroChunk() || this.m_textChunk.Length > 10 && this.CHPXSprms.GetBoolean(2133, false) && this.CHPXSprms[27139] == null)
    {
      curChunkLen2 = -1;
    }
    else
    {
      this.CheckSpecCharacters(curChunkPosLen);
      curChunkLen2 = this.m_textChunk.Length;
    }
    this.m_textChunk = string.Empty;
    this.m_chunkType = WordChunkType.Text;
    this.m_streamsManager.MainStream.Position = position;
    return curChunkLen2;
  }

  private int CheckForSymbols(int chunkLen)
  {
    int num = chunkLen;
    if (this.CHPXSprms[27145] != null && chunkLen > 1)
    {
      long position = this.m_streamsManager.MainStream.Position;
      this.ReadChunkString(chunkLen);
      if (this.m_textChunk[0] == '(')
        num = 1;
      this.m_streamsManager.MainStream.Position = position;
      this.m_textChunk = string.Empty;
    }
    return num;
  }

  private DocumentVersion GetDocVersion()
  {
    uint fibRgFcLcb97LcbDop = this.m_docInfo.Fib.FibRgFcLcb97LcbDop;
    if (fibRgFcLcb97LcbDop < 544U)
      return DocumentVersion.Word97;
    if (fibRgFcLcb97LcbDop == 544U)
      return DocumentVersion.Word2000;
    if (fibRgFcLcb97LcbDop <= 594U)
      return DocumentVersion.Word2002;
    return fibRgFcLcb97LcbDop <= 616U ? DocumentVersion.Word2003 : DocumentVersion.Word2007;
  }

  internal SymbolDescriptor GetSymbolDescriptor()
  {
    byte[] byteArray = this.CHPXSprms.GetByteArray(27145);
    SymbolDescriptor symbolDescriptor = new SymbolDescriptor();
    if (byteArray != null)
      symbolDescriptor.Parse(byteArray);
    return symbolDescriptor;
  }

  internal string GetFontName(int wordSprmOption)
  {
    return this.m_styleSheet.FontNamesList.Count == 0 ? string.Empty : this.m_styleSheet.FontNamesList[(int) this.CHPXSprms.GetUShort(wordSprmOption, (ushort) 0)];
  }

  internal int GetPicLocation()
  {
    int picLocation = 0;
    if (this.CHPXSprms[27139] != null)
      picLocation = this.CHPXSprms.GetInt(27139, 0);
    return picLocation;
  }

  internal Dictionary<int, string> GetSTTBFRNames(byte[] sttb)
  {
    if (sttb == null)
      return (Dictionary<int, string>) null;
    int startIndex1 = 0;
    bool flag = false;
    if (sttb.Length < 2)
      return (Dictionary<int, string>) null;
    ushort uint16_1 = BitConverter.ToUInt16(sttb, startIndex1);
    int startIndex2 = startIndex1 + 2;
    if (uint16_1 == ushort.MaxValue)
      flag = true;
    if (sttb.Length < startIndex2 + 2)
      return (Dictionary<int, string>) null;
    int capacity = (int) BitConverter.ToUInt16(sttb, startIndex2);
    int startIndex3;
    if (capacity >= (int) ushort.MaxValue)
    {
      if (sttb.Length < startIndex2 + 4)
        return (Dictionary<int, string>) null;
      capacity = BitConverter.ToInt32(sttb, startIndex2);
      if (capacity <= 0)
        return (Dictionary<int, string>) null;
      startIndex3 = startIndex2 + 4;
    }
    else
      startIndex3 = startIndex2 + 2;
    if (sttb.Length < startIndex3 + 2)
      return (Dictionary<int, string>) null;
    ushort uint16_2 = BitConverter.ToUInt16(sttb, startIndex3);
    int startIndex4 = startIndex3 + 2;
    Dictionary<int, string> sttbfrNames = new Dictionary<int, string>(capacity);
    for (int key = 0; key < capacity; ++key)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string str;
      int index1;
      if (flag)
      {
        if (sttb.Length < startIndex4 + 2)
          return sttbfrNames;
        int uint16_3 = (int) BitConverter.ToUInt16(sttb, startIndex4);
        int index2 = startIndex4 + 2;
        if (sttb.Length < index2 + 2 * uint16_3)
          return sttbfrNames;
        str = Encoding.Unicode.GetString(sttb, index2, 2 * uint16_3);
        index1 = index2 + 2 * uint16_3;
      }
      else
      {
        int count = (int) sttb[startIndex4];
        int index3 = startIndex4 + 1;
        if (sttb.Length < index3 + count)
          return sttbfrNames;
        str = Encoding.Default.GetString(sttb, index3, count);
        index1 = index3 + count;
      }
      if (sttb.Length < index1 + (int) uint16_2)
        return sttbfrNames;
      Encoding.UTF8.GetString(sttb, index1, (int) uint16_2);
      startIndex4 = index1 + (int) uint16_2;
      sttbfrNames[key] = str;
    }
    return sttbfrNames;
  }
}
