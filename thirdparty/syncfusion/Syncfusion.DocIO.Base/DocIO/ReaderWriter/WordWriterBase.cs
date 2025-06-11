// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordWriterBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal abstract class WordWriterBase : IWordWriterBase
{
  private const int DEF_FIELDSHAPETYPE_VAL = 2;
  public StreamsManager m_streamsManager;
  public DocInfo m_docInfo;
  protected WordStyleSheet m_styleSheet;
  protected int m_nextPicLocation;
  protected CharacterPropertyException m_chpx;
  protected CharacterPropertyException m_breakChpx;
  protected ParagraphPropertyException m_papx;
  protected ListProperties m_listProperties;
  private Stack<FieldDescriptor> m_endStack = new Stack<FieldDescriptor>();
  private int m_iCountCell;
  private int m_currStyleIndex;
  protected int m_curTxbxId;
  protected int m_curPicId;
  private int m_curTxid;
  protected int m_textColIndex;
  protected int m_iStartText;
  protected BinaryWriter m_textWriter;
  private byte m_bFlag = 7;
  protected WordSubdocument m_type;

  public WordStyleSheet StyleSheet => this.m_styleSheet;

  public int CurrentStyleIndex
  {
    get => this.m_currStyleIndex;
    set
    {
      if (this.m_currStyleIndex == value)
        return;
      if (value < 0 || value > this.m_styleSheet.StylesCount - 1)
        throw new ArgumentOutOfRangeException(nameof (CurrentStyleIndex), $"value must be between 0 and {this.m_styleSheet.StylesCount - 1}");
      this.m_currStyleIndex = value;
    }
  }

  public CharacterPropertyException CHPX
  {
    get => this.m_chpx;
    set => this.m_chpx = value;
  }

  public CharacterPropertyException BreakCHPX
  {
    get => this.m_breakChpx;
    set => this.m_breakChpx = value;
  }

  public ParagraphPropertyException PAPX
  {
    get => this.m_papx;
    set => this.m_papx = value;
  }

  public bool BreakCHPXStickProperties
  {
    get => ((int) this.m_bFlag & 1) != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 254 | (value ? 1 : 0));
  }

  public bool CHPXStickProperties
  {
    get => ((int) this.m_bFlag & 2) != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 253 | (value ? 1 : 0) << 1);
  }

  public bool PAPXStickProperties
  {
    get => ((int) this.m_bFlag & 4) != 0;
    set => this.m_bFlag = (byte) ((int) this.m_bFlag & 251 | (value ? 1 : 0) << 2);
  }

  public ListProperties ListProperties
  {
    get
    {
      if (this.m_listProperties == null)
        this.m_listProperties = new ListProperties(this.m_docInfo.TablesData.ListInfo, this.m_papx);
      return this.m_listProperties;
    }
  }

  public EscherClass Escher
  {
    get => this.m_docInfo.TablesData.Escher;
    set => this.m_docInfo.TablesData.Escher = value;
  }

  public StreamsManager StreamsManager => this.m_streamsManager;

  public BinaryWriter MainWriter
  {
    get
    {
      if (this.m_textWriter == null)
        this.m_textWriter = new BinaryWriter((Stream) this.m_streamsManager.MainStream, this.m_docInfo.Fib.Encoding);
      return this.m_textWriter;
    }
  }

  internal int NextTextId
  {
    get
    {
      this.m_curTxid += 65536 /*0x010000*/;
      return this.m_curTxid;
    }
  }

  public virtual void WriteChunk(string textChunk)
  {
    bool chpxStickProperties1 = this.CHPXStickProperties;
    bool papxStickProperties = this.PAPXStickProperties;
    bool chpxStickProperties2 = this.BreakCHPXStickProperties;
    bool boolean1 = this.PAPX.PropertyModifiers.GetBoolean(9238, false);
    bool flag = this.PAPX.PropertyModifiers.GetByte(9291, (byte) 0) == (byte) 1;
    bool boolean2 = this.PAPX.PropertyModifiers.GetBoolean(9292, false);
    textChunk = textChunk.Replace(ControlChar.CrLf, ControlChar.CarriegeReturn);
    textChunk = textChunk.Replace(ControlChar.LineFeedChar, '\r');
    string[] strArray = textChunk.Split(ControlChar.CarriegeReturn.ToCharArray());
    int length = strArray.Length;
    if (length > 1)
    {
      this.CHPXStickProperties = true;
      this.PAPXStickProperties = true;
      this.BreakCHPXStickProperties = true;
      if (flag)
        this.PAPX.PropertyModifiers.RemoveValue(9291);
      if (boolean2)
        this.PAPX.PropertyModifiers.RemoveValue(9292);
    }
    for (int index = 0; index < length; ++index)
    {
      this.WriteString(strArray[index]);
      if (index < length - 1)
        this.WriteChar('\r');
    }
    if (length > 1)
    {
      if (!chpxStickProperties1)
        this.CHPX.PropertyModifiers.Clear();
      if (flag)
        this.PAPX.PropertyModifiers.SetBoolValue(9291, true);
      if (boolean2)
        this.PAPX.PropertyModifiers.SetBoolValue(9292, true);
    }
    this.CHPXStickProperties = chpxStickProperties1;
    this.PAPXStickProperties = papxStickProperties;
    this.BreakCHPXStickProperties = chpxStickProperties2;
    if (!boolean1)
      return;
    this.SetCellMark(this.PAPX.PropertyModifiers, boolean1);
  }

  public virtual void WriteSafeChunk(string textChunk)
  {
    bool boolean = this.PAPX.PropertyModifiers.GetBoolean(9238, false);
    this.WriteString(textChunk);
    if (!this.CHPXStickProperties)
      this.CHPX.PropertyModifiers.Clear();
    if (!boolean)
      return;
    this.SetCellMark(this.PAPX.PropertyModifiers, boolean);
  }

  public void WriteCellMark(int nestingLevel)
  {
    if (nestingLevel == 1)
    {
      this.WriteMarker(WordChunkType.TableCell);
    }
    else
    {
      this.SetCellMark(this.PAPX.PropertyModifiers, true);
      this.SetTableNestingLevel(this.PAPX.PropertyModifiers, nestingLevel);
      if (this.PAPX.PropertyModifiers.HasSprm(9291))
        this.PAPX.PropertyModifiers.RemoveValue(9291);
      this.PAPX.PropertyModifiers.InsertAt(new SinglePropertyModifierRecord(9291)
      {
        BoolValue = true
      }, 0);
      this.WriteNestedMark();
    }
  }

  public void WriteRowMark(int nestingLevel, int cellCount)
  {
    if (nestingLevel == 1)
    {
      this.m_iCountCell = cellCount;
      this.WriteMarker(WordChunkType.TableRow);
    }
    else
    {
      if (this.PAPX.PropertyModifiers[54789] == null)
      {
        byte[] arr = new byte[24];
        Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = this.CreateTableBorders();
        for (int index = 0; index < 6; ++index)
          tableBorders[index].SaveBytes(arr, index * 4);
        this.PAPX.PropertyModifiers.SetByteArrayValue(54789, arr);
      }
      this.SetCellMark(this.PAPX.PropertyModifiers, true);
      this.SetTableNestingLevel(this.PAPX.PropertyModifiers, nestingLevel);
      this.PAPX.PropertyModifiers.SetBoolValue(9291, true);
      this.PAPX.PropertyModifiers.SetBoolValue(9292, true);
      this.WriteNestedMark();
      this.SetCellMark(this.PAPX.PropertyModifiers, false);
    }
  }

  public virtual void WriteMarker(WordChunkType chunkType)
  {
    switch (chunkType)
    {
      case WordChunkType.ParagraphEnd:
        this.WriteChar('\r');
        break;
      case WordChunkType.PageBreak:
        this.WriteChar('\f');
        break;
      case WordChunkType.Image:
        this.WriteChar('\u0001');
        break;
      case WordChunkType.Shape:
        this.CHPX.PropertyModifiers.SetIntValue(27139, 0);
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('\b');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
      case WordChunkType.Table:
        this.WriteChar('\a');
        break;
      case WordChunkType.TableRow:
        if (this.PAPX.PropertyModifiers[54789] == null)
        {
          byte[] arr = new byte[24];
          Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders tableBorders = this.CreateTableBorders();
          for (int index = 0; index < 6; ++index)
            tableBorders[index].SaveBytes(arr, index * 4);
          this.PAPX.PropertyModifiers.SetByteArrayValue(54789, arr);
        }
        this.SetCellMark(this.PAPX.PropertyModifiers, true);
        this.PAPX.PropertyModifiers.SetBoolValue(9239, true);
        this.WriteChar('\a');
        this.SetCellMark(this.PAPX.PropertyModifiers, false);
        this.m_iCountCell = 0;
        break;
      case WordChunkType.TableCell:
        this.SetCellMark(this.PAPX.PropertyModifiers, true);
        this.WriteChar('\a');
        ++this.m_iCountCell;
        break;
      case WordChunkType.Footnote:
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('\u0002');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
      case WordChunkType.FieldBeginMark:
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('\u0013');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
      case WordChunkType.FieldSeparator:
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('\u0014');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
      case WordChunkType.FieldEndMark:
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('\u0015');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
      case WordChunkType.Tab:
        this.WriteChar('\t');
        break;
      case WordChunkType.Annotation:
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('\u0005');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
      case WordChunkType.LineBreak:
        this.WriteChar('\v');
        break;
      case WordChunkType.Symbol:
        this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
        this.WriteChar('(');
        this.CHPX.PropertyModifiers.RemoveValue(2133);
        break;
    }
  }

  private void SetCellMark(SinglePropertyModifierArray sprms, bool value)
  {
    if (value)
      sprms.SetBoolValue(9238, value);
    else
      sprms.RemoveValue(9238);
  }

  private void SetTableNestingLevel(SinglePropertyModifierArray sprms, int value)
  {
    if (value > 0)
      sprms.SetIntValue(26185, value);
    else
      sprms.RemoveValue(26185);
  }

  public Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders CreateTableBorders()
  {
    return new Syncfusion.DocIO.ReaderWriter.Biff_Records.TableBorders();
  }

  public void InsertStartField(string fieldcode, bool hasSeparator)
  {
    if (fieldcode == null || fieldcode.Length == 0)
      throw new ArgumentException("fieldcode must be present.");
    bool chpxStickProperties = this.CHPXStickProperties;
    this.CHPXStickProperties = true;
    int textPos1 = this.GetTextPos();
    FieldDescriptor fld = this.WriteFieldStart(FieldTypeDefiner.GetFieldType(fieldcode));
    this.AddFieldDescriptor(fld, textPos1);
    this.WriteSafeChunk(fieldcode);
    this.CHPXStickProperties = chpxStickProperties;
    if (hasSeparator)
    {
      int textPos2 = this.GetTextPos();
      this.AddFieldDescriptor(this.WriteFieldSeparator(), textPos2);
    }
    FieldDescriptor fieldDescriptor = new FieldDescriptor();
    fieldDescriptor.HasSeparator = hasSeparator;
    if (WordDocument.DisableDateTimeUpdating && (fld.Type == FieldType.FieldDate || fld.Type == FieldType.FieldTime))
      fieldDescriptor.IsLocked = true;
    this.m_endStack.Push(fieldDescriptor);
  }

  public void InsertStartField(string fieldcode, WField field, bool hasSeparator)
  {
    bool chpxStickProperties = this.CHPXStickProperties;
    this.CHPXStickProperties = true;
    int textPos1 = this.GetTextPos();
    FieldDescriptor fld = this.WriteFieldStart(field.FieldType);
    if (field.FieldType == FieldType.FieldUnknown)
      fld.Type = (FieldType) field.SourceFieldType;
    this.AddFieldDescriptor(fld, textPos1);
    if (!string.IsNullOrEmpty(fieldcode))
      this.WriteSafeChunk(fieldcode);
    if (field.FieldType == FieldType.FieldRef || field.FieldType == FieldType.FieldPageRef || field.FieldType == FieldType.FieldNoteRef)
      this.WriteNilPICFAndBinData(field);
    this.CHPXStickProperties = chpxStickProperties;
    if (hasSeparator)
    {
      int textPos2 = this.GetTextPos();
      this.AddFieldDescriptor(this.WriteFieldSeparator(), textPos2);
    }
    FieldDescriptor fieldDescriptor = new FieldDescriptor();
    fieldDescriptor.HasSeparator = true;
    if (WordDocument.DisableDateTimeUpdating && (field.FieldType == FieldType.FieldDate || field.FieldType == FieldType.FieldTime))
      fieldDescriptor.IsLocked = true;
    this.m_endStack.Push(fieldDescriptor);
  }

  public void InsertFieldSeparator()
  {
    int textPos = this.GetTextPos();
    this.AddFieldDescriptor(this.WriteFieldSeparator(), textPos);
  }

  public void InsertEndField()
  {
    if (this.m_endStack.Count <= 0)
      return;
    FieldDescriptor fld = this.m_endStack.Pop();
    fld.FieldBoundary = (byte) 21;
    fld.IsNested = this.m_endStack.Count != 0;
    int textPos = this.GetTextPos();
    this.AddFieldDescriptor(fld, textPos);
    this.WriteFieldEnd();
  }

  public void InsertFieldIndexEntry(string fieldCode)
  {
    bool chpxStickProperties = this.CHPXStickProperties;
    this.CHPXStickProperties = true;
    this.WriteMarker(WordChunkType.FieldBeginMark);
    this.WriteSafeChunk(fieldCode);
    this.WriteMarker(WordChunkType.FieldEndMark);
    this.CHPX.PropertyModifiers.Clear();
    this.CHPXStickProperties = chpxStickProperties;
  }

  public void InsertFormField(string fieldcode, FormField formField, WFormField wFormField)
  {
    bool chpxStickProperties = this.CHPXStickProperties;
    this.CHPXStickProperties = true;
    bool flag;
    if (formField != null)
    {
      flag = formField.FormFieldType == FormFieldType.TextInput;
      if (formField.Params == (short) 1)
        flag = true;
    }
    else
      flag = true;
    int textPos = this.GetTextPos();
    this.AddFieldDescriptor(this.WriteFieldStart(wFormField.FieldType), textPos);
    if (!string.IsNullOrEmpty(fieldcode))
      this.WriteSafeChunk(fieldcode);
    this.CHPXStickProperties = false;
    int position = (int) this.m_streamsManager.DataStream.Position;
    if (formField != null)
    {
      formField.Write((Stream) this.m_streamsManager.DataStream);
      this.CHPX.PropertyModifiers.SetBoolValue(2050, true);
      this.CHPX.PropertyModifiers.SetIntValue(27139, position);
      this.CHPX.PropertyModifiers.SetBoolValue(2054, true);
      this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
      this.WriteMarker(WordChunkType.Image);
      this.CHPXStickProperties = true;
    }
    this.CHPXStickProperties = chpxStickProperties;
    this.m_endStack.Push(new FieldDescriptor()
    {
      HasSeparator = flag
    });
  }

  public void InsertHyperlink(string displayText, string url, bool isLocalUrl)
  {
    this.InsertStartField($"HYPERLINK {(!isLocalUrl ? (object) "\\l " : (object) "")}\"{url}\"", true);
    this.WriteChunk(displayText);
    this.InsertEndField();
  }

  public void InsertImage(WPicture picture)
  {
    if (picture.ImageRecord == null)
      return;
    Size size = picture.ImageRecord.Size;
    this.InsertImage(picture, size.Height, size.Width);
  }

  public void InsertImage(WPicture picture, int height, int width)
  {
    if (picture.ImageRecord == null)
      return;
    this.m_nextPicLocation = (int) this.m_streamsManager.DataStream.Position;
    this.CHPX.PropertyModifiers.SetIntValue(27139, this.m_nextPicLocation);
    this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
    this.m_docInfo.ImageWriter.WriteImage(picture, height, width);
    this.WriteMarker(WordChunkType.Image);
    this.CHPX.PropertyModifiers.RemoveValue(2133);
  }

  public void InsertShape(WPicture pict, PictureShapeProps pictProps)
  {
    bool flag = false;
    int textPos = this.GetTextPos();
    this.WriteMarker(WordChunkType.Shape);
    MsofbtSpContainer spContainer = (MsofbtSpContainer) null;
    if (this.Escher.Containers.ContainsKey(pictProps.Spid))
      spContainer = this.Escher.Containers[pictProps.Spid] as MsofbtSpContainer;
    int[] array = new int[this.Escher.Containers.Count];
    this.Escher.Containers.Keys.CopyTo(array, 0);
    if (pict.IsCloned)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        if (this.Escher.Containers[array[index]] is MsofbtSpContainer)
        {
          MsofbtSpContainer container = this.Escher.Containers[array[index]] as MsofbtSpContainer;
          if (container.Bse != null && container.Bse.Blip.ImageRecord == pict.ImageRecord)
          {
            pictProps.Spid = array[index];
            this.m_docInfo.TablesData.ArtObj.AddFSPA((FileShapeAddress) pictProps, this.m_type, textPos);
            flag = true;
            break;
          }
        }
        else
        {
          flag = false;
          break;
        }
      }
    }
    if (flag)
      return;
    this.SetFSPASpid((BaseProps) pictProps);
    this.AddPictContainer(pict, spContainer, pictProps);
    this.m_docInfo.TablesData.ArtObj.AddFSPA((FileShapeAddress) pictProps, this.m_type, textPos);
  }

  public int InsertTextBox(bool visible, WTextBoxFormat txbxFormat)
  {
    MsofbtSpContainer spContainer = (MsofbtSpContainer) null;
    if (this.Escher.Containers.ContainsKey(txbxFormat.TextBoxShapeID))
      spContainer = this.Escher.Containers[txbxFormat.TextBoxShapeID] as MsofbtSpContainer;
    if (spContainer == null && txbxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
      return this.InsertInlineTextBox(visible, txbxFormat);
    if (spContainer == null || spContainer.ShapeOptions.Txid == null)
      this.AddTxBxContainer(visible, txbxFormat);
    else
      this.SyncTxBxContainer(spContainer, visible, txbxFormat);
    int textPos = this.GetTextPos();
    FileShapeAddress fspa = new FileShapeAddress();
    TextBoxPropertiesConverter.Import(fspa, txbxFormat);
    this.m_docInfo.TablesData.ArtObj.AddFSPA(fspa, this.m_type, textPos);
    this.WriteMarker(WordChunkType.Shape);
    return txbxFormat.TextBoxShapeID;
  }

  public int InsertInlineTextBox(bool visible, WTextBoxFormat txbxFormat)
  {
    txbxFormat.TextBoxShapeID = this.m_curTxbxId;
    FileShapeAddress fspa = new FileShapeAddress();
    TextBoxPropertiesConverter.Import(fspa, txbxFormat);
    this.AddTxBxContainer(visible, txbxFormat);
    this.InsertStartField(" SHAPE \\*MERGEFORMAT ", true);
    int textPos = this.GetTextPos();
    fspa.TxbxCount = 0;
    fspa.IsAnchorLock = true;
    fspa.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
    this.m_docInfo.TablesData.ArtObj.AddFSPA(fspa, this.m_type, textPos);
    this.WriteMarker(WordChunkType.Shape);
    this.m_nextPicLocation = (int) this.m_streamsManager.DataStream.Position;
    this.CHPX.PropertyModifiers.SetIntValue(27139, this.m_nextPicLocation);
    this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
    this.m_nextPicLocation = this.m_docInfo.ImageWriter.WriteInlineTxBxPicture(txbxFormat);
    this.WriteMarker(WordChunkType.Image);
    this.CHPX.PropertyModifiers.RemoveValue(2133);
    this.InsertEndField();
    return txbxFormat.TextBoxShapeID;
  }

  public void InsertShapeObject(ShapeObject shapeObj)
  {
    if (this.Escher.Containers.ContainsKey(shapeObj.FSPA.Spid))
    {
      BaseContainer container = this.Escher.Containers[shapeObj.FSPA.Spid];
      WTextBoxCollection shapeTextCollection = shapeObj.AutoShapeTextCollection;
      this.m_textColIndex = 0;
      container.SynchronizeIdent(shapeTextCollection, ref this.m_curTxbxId, ref this.m_curPicId, ref this.m_curTxid, ref this.m_textColIndex);
      int spid = container.GetSpid();
      shapeObj.FSPA.Spid = spid;
      this.Escher.FillCollectionForSearch(container);
    }
    int textPos = this.GetTextPos();
    this.m_docInfo.TablesData.ArtObj.AddFSPA(shapeObj.FSPA, this.m_type, textPos);
    this.WriteMarker(WordChunkType.Shape);
  }

  public void InsertInlineShapeObject(InlineShapeObject shapeObj)
  {
    if (shapeObj.IsOLE && shapeObj.OLEContainerId != -1)
    {
      this.CHPX.PropertyModifiers.SetIntValue(27139, shapeObj.OLEContainerId);
      this.CHPX.PropertyModifiers.SetBoolValue(2058, true);
    }
    else
    {
      int position = (int) this.m_streamsManager.DataStream.Position;
      this.m_nextPicLocation = position;
      this.CHPX.PropertyModifiers.SetIntValue(27139, this.m_nextPicLocation);
      this.m_nextPicLocation = this.m_docInfo.ImageWriter.WriteInlineShapeObject(shapeObj);
      if (position == this.m_nextPicLocation)
        ++this.m_streamsManager.DataStream.Position;
    }
    this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
    this.WriteMarker(WordChunkType.Image);
  }

  public void InsertBookmarkStart(string name, BookmarkStart start)
  {
    this.m_docInfo.TablesData.BookmarkStrings.Add(name);
    this.m_docInfo.TablesData.BookmarkDescriptor.Add(this.GetTextPos());
    int bookmarkIndex = this.m_docInfo.TablesData.BookmarkStrings.Find(name);
    if (start.ColumnLast <= (short) -1 || bookmarkIndex == -1)
      return;
    BookmarkDescriptor bookmarkDescriptor = this.m_docInfo.TablesData.BookmarkDescriptor;
    bookmarkDescriptor.SetCellGroup(bookmarkIndex, true);
    bookmarkDescriptor.SetStartCellIndex(bookmarkIndex, (int) start.ColumnFirst);
    bookmarkDescriptor.SetEndCellIndex(bookmarkIndex, (int) start.ColumnLast + 1);
  }

  public void InsertBookmarkEnd(string name)
  {
    int i = this.m_docInfo.TablesData.BookmarkStrings.Find(name);
    if (i == -1)
      return;
    int textPos = this.GetTextPos();
    this.m_docInfo.TablesData.BookmarkDescriptor.SetEndPos(i, textPos);
  }

  public void InsertWatermark(Watermark watermark, UnitsConvertor unitsConvertor, float maxWidth)
  {
    FileShapeAddress watermarkFspa = this.CreateWatermarkFSPA();
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer(watermark.Document);
    this.Escher.AddContainerForSubDocument(WordSubdocument.HeaderFooter, watermark.Type != WatermarkType.TextWatermark ? (BaseEscherRecord) this.InsertPictureWatermark(watermark, watermarkFspa, unitsConvertor, maxWidth) : (BaseEscherRecord) this.InsertTextWatermark(watermark, watermarkFspa, unitsConvertor));
    int textPos = this.GetTextPos();
    this.m_docInfo.TablesData.ArtObj.AddFSPA(watermarkFspa, this.m_type, textPos);
    this.WriteMarker(WordChunkType.Shape);
  }

  public WordWriterBase(StreamsManager streamsManager) => this.m_streamsManager = streamsManager;

  protected WordWriterBase()
  {
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
    this.m_chpx = (CharacterPropertyException) null;
    this.m_papx = (ParagraphPropertyException) null;
    this.m_breakChpx = (CharacterPropertyException) null;
    if (this.m_listProperties != null)
    {
      this.m_listProperties.Close();
      this.m_listProperties = (ListProperties) null;
    }
    if (this.m_endStack != null)
    {
      this.m_endStack.Clear();
      this.m_endStack = (Stack<FieldDescriptor>) null;
    }
    if (this.m_textWriter == null)
      return;
    this.m_textWriter.Close();
    this.m_textWriter = (BinaryWriter) null;
  }

  protected abstract void IncreaseCcp(int dataLength);

  protected virtual void InitClass()
  {
    this.m_curTxbxId = 3026;
    this.m_curPicId = 17000;
    if (this.m_styleSheet == null)
      this.m_styleSheet = new WordStyleSheet(true);
    this.m_chpx = new CharacterPropertyException();
    this.m_breakChpx = new CharacterPropertyException();
    this.BreakCHPXStickProperties = true;
    this.CHPXStickProperties = true;
    this.m_papx = new ParagraphPropertyException();
    this.m_currStyleIndex = this.m_styleSheet.DefaultStyleIndex;
  }

  protected void WriteString(string text)
  {
    if (text == null || !(text != string.Empty))
      return;
    byte[] bytes = this.m_docInfo.Fib.Encoding.GetBytes(text);
    this.m_streamsManager.MainStream.Write(bytes, 0, bytes.Length);
    this.AddChpxProperties(false);
    this.IncreaseCcp(text.Length);
  }

  protected void AddChpxProperties(bool isParaBreak)
  {
    this.m_docInfo.FkpData.AddChpxProperties((uint) this.m_streamsManager.MainStream.Position, new CharacterPropertyException()
    {
      PropertyModifiers = !isParaBreak ? this.CHPX.PropertyModifiers.Clone() : this.BreakCHPX.PropertyModifiers.Clone()
    });
  }

  protected void AddPapxProperties()
  {
    MemoryStream mainStream = this.m_streamsManager.MainStream;
    ParagraphPropertyException papx1 = this.PAPX;
    ParagraphExceptionInDiskPage papx2 = new ParagraphExceptionInDiskPage(this.PAPX.ClonePapx(this.PAPXStickProperties, this.PAPX));
    if (papx1.PropertyModifiers[17920] != null && (this.StyleSheet.GetStyleByIndex((int) papx1.PropertyModifiers[17920].ShortValue).ID > 0 || this.CurrentStyleIndex != 0))
      papx2.ParagraphStyleId = (ushort) this.CurrentStyleIndex;
    this.UpdateShadingSprms(papx1);
    papx2.PropertyModifiers.RemoveValue(25707);
    papx2.PropertyModifiers.RemoveValue(26182);
    papx2.StyleIndex = (ushort) this.CurrentStyleIndex;
    this.m_docInfo.FkpData.AddPapxProperties((uint) mainStream.Position, papx2, this.m_streamsManager.DataStream);
    this.PAPX.PropertyModifiers.Clear();
  }

  private void UpdateShadingSprms(ParagraphPropertyException paraPropertyException)
  {
    if (paraPropertyException.PropertyModifiers[54802] != null)
    {
      SinglePropertyModifierRecord propertyModifier = paraPropertyException.PropertyModifiers[54802];
      paraPropertyException.PropertyModifiers.SetByteArrayValue(54896, propertyModifier.Operand);
    }
    if (paraPropertyException.PropertyModifiers[54806] != null)
    {
      SinglePropertyModifierRecord propertyModifier = paraPropertyException.PropertyModifiers[54806];
      paraPropertyException.PropertyModifiers.SetByteArrayValue(54897, propertyModifier.Operand);
    }
    if (paraPropertyException.PropertyModifiers[54796] == null)
      return;
    SinglePropertyModifierRecord propertyModifier1 = paraPropertyException.PropertyModifiers[54796];
    paraPropertyException.PropertyModifiers.SetByteArrayValue(54898, propertyModifier1.Operand);
  }

  protected void WriteSymbol(char symbol)
  {
    MemoryStream mainStream = this.m_streamsManager.MainStream;
    byte[] bytes = this.m_docInfo.Fib.Encoding.GetBytes(symbol.ToString());
    mainStream.Write(bytes, 0, bytes.Length);
  }

  protected void WriteChar(char symbol)
  {
    this.WriteSymbol(symbol);
    this.AddChpxProperties(symbol == '\r' || symbol == '\f');
    if (symbol == '\r' || symbol == '\f' || symbol == '\a')
      this.AddPapxProperties();
    this.IncreaseCcp(1);
  }

  protected void WriteNestedMark()
  {
    this.WriteSymbol('\r');
    this.AddChpxProperties(false);
    this.AddPapxProperties();
    this.IncreaseCcp(1);
  }

  internal virtual int GetTextPos()
  {
    return (int) (this.m_streamsManager.MainStream.Position - (long) this.m_iStartText) / this.m_docInfo.Fib.EncodingCharSize;
  }

  private void SetFSPASpid(BaseProps props)
  {
    if (props is PictureShapeProps)
      props.Spid = this.m_curPicId;
    else
      props.Spid = this.m_curTxbxId;
  }

  protected FieldDescriptor WriteFieldStart(FieldType fieldType)
  {
    FieldDescriptor fieldDescriptor = new FieldDescriptor();
    fieldDescriptor.FieldBoundary = (byte) 19;
    fieldDescriptor.Type = fieldType != FieldType.FieldShape ? fieldType : (FieldType) 2;
    this.WriteMarker(WordChunkType.FieldBeginMark);
    return fieldDescriptor;
  }

  protected FieldDescriptor WriteFieldSeparator()
  {
    FieldDescriptor fieldDescriptor = new FieldDescriptor();
    fieldDescriptor.FieldBoundary = (byte) 20;
    fieldDescriptor.IsNested = this.m_endStack.Count > 1;
    this.WriteMarker(WordChunkType.FieldSeparator);
    return fieldDescriptor;
  }

  protected void WriteFieldEnd() => this.WriteMarker(WordChunkType.FieldEndMark);

  protected void WriteNilPICFAndBinData(WField field)
  {
    this.CHPXStickProperties = false;
    int position = (int) this.m_streamsManager.DataStream.Position;
    BinaryWriter binaryWriter = new BinaryWriter((Stream) this.m_streamsManager.DataStream);
    PICF picf = new PICF();
    picf.lcb = 29 + (field.FieldValue.Length + 1) * 2 + (int) picf.cbHeader;
    picf.Write((Stream) this.m_streamsManager.DataStream);
    byte[] buffer = new byte[picf.lcb - (int) picf.cbHeader];
    buffer[0] = Convert.ToByte(8);
    string[] strArray = "D0 C9 EA 79 F9 BA CE 11 8C 82 00 AA 00 4B A9 0B".Split(' ');
    byte[] numArray = new byte[strArray.Length];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = Convert.ToByte(int.Parse(strArray[index], NumberStyles.HexNumber).ToString());
    numArray.CopyTo((Array) buffer, 1);
    buffer[17] = Convert.ToByte(2U);
    buffer[21] = Convert.ToByte(8);
    string str = this.RemoveFormattingString(field.FieldValue);
    buffer[25] = Convert.ToByte(str.Length + 1);
    Encoding.Unicode.GetBytes((str + "\0").ToCharArray()).CopyTo((Array) buffer, 29);
    binaryWriter.Write(buffer);
    this.CHPX.PropertyModifiers.SetBoolValue(2050, true);
    this.CHPX.PropertyModifiers.SetIntValue(27139, position);
    this.CHPX.PropertyModifiers.SetBoolValue(2054, true);
    this.CHPX.PropertyModifiers.SetBoolValue(2133, true);
    this.WriteMarker(WordChunkType.Image);
    this.CHPXStickProperties = true;
  }

  private string RemoveFormattingString(string value)
  {
    foreach (Group group in new Regex("([\\\\+].)+").Match(value).Groups)
    {
      if (group.Value != string.Empty)
        value = value.Replace(group.Value, string.Empty);
    }
    return value.Trim();
  }

  protected void AddFieldDescriptor(FieldDescriptor fld, int pos)
  {
    this.m_docInfo.TablesData.Fields.AddField(this.m_type, fld, pos);
  }

  public void AddTxBxContainer(bool visible, WTextBoxFormat txbxFormat)
  {
    txbxFormat.TextBoxShapeID = this.m_curTxbxId;
    txbxFormat.TextBoxIdentificator = (float) this.NextTextId;
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer(txbxFormat.Document);
    msofbtSpContainer.CreateTextBoxContainer(visible, txbxFormat);
    this.Escher.AddContainerForSubDocument(this.m_type, (BaseEscherRecord) msofbtSpContainer);
    ++this.m_curTxbxId;
  }

  internal void SyncTxBxContainer(
    MsofbtSpContainer spContainer,
    bool visible,
    WTextBoxFormat txbxFormat)
  {
    this.m_textColIndex = 0;
    if (spContainer.ShapeOptions == null)
      return;
    this.UpdateContainers(spContainer.Shape.ShapeId, this.m_curTxbxId, spContainer);
    int num = (int) spContainer.ShapeOptions.Txid.Value;
    txbxFormat.TextBoxShapeID = this.m_curTxbxId;
    spContainer.SynchronizeIdent((WTextBoxCollection) null, ref this.m_curTxbxId, ref this.m_curPicId, ref this.m_curTxid, ref this.m_textColIndex);
    txbxFormat.TextBoxIdentificator = (float) this.m_curTxid;
    spContainer.WriteTextBoxOptions(visible, txbxFormat);
  }

  internal void AddPictContainer(
    WPicture pict,
    MsofbtSpContainer spContainer,
    PictureShapeProps pictProps)
  {
    if (spContainer == null || spContainer.Bse == null || pict.IsMetaFile && spContainer.Bse.Blip is MsofbtImage || spContainer != null && spContainer.Bse.Blip.IsDib || pict.Document.IsReadOnly)
    {
      spContainer = new MsofbtSpContainer(pict.Document);
      spContainer.CreateImageContainer(pict, pictProps);
      ++this.m_curPicId;
      this.Escher.AddContainerForSubDocument(this.m_type, (BaseEscherRecord) spContainer);
    }
    else
    {
      this.Escher.ModifyBStoreByPid((int) spContainer.ShapeOptions.Pib.Value, spContainer.Bse);
      this.SyncPictContainer(spContainer, pictProps, pict);
    }
  }

  internal void SyncPictContainer(
    MsofbtSpContainer spContainer,
    PictureShapeProps pictProps,
    WPicture pic)
  {
    this.UpdateContainers(spContainer.Shape.ShapeId, this.m_curPicId, spContainer);
    spContainer.Shape.HasAnchor = true;
    spContainer.Shape.ShapeId = this.m_curPicId;
    ++this.m_curPicId;
    spContainer.WritePictureOptions(pictProps, pic);
  }

  private void UpdateContainers(int oldId, int newId, MsofbtSpContainer spContainer)
  {
    try
    {
      this.Escher.Containers.Add(newId, (BaseContainer) spContainer);
    }
    catch
    {
    }
  }

  private FileShapeAddress CreateWatermarkFSPA()
  {
    return new FileShapeAddress()
    {
      Height = 2000,
      Width = 2000,
      RelHrzPos = HorizontalOrigin.Column,
      RelVrtPos = VerticalOrigin.Paragraph,
      TextWrappingStyle = TextWrappingStyle.InFrontOfText,
      TextWrappingType = TextWrappingType.Both
    };
  }

  private MsofbtSpContainer InsertTextWatermark(
    Watermark watermark,
    FileShapeAddress fspa,
    UnitsConvertor unitsConvertor)
  {
    TextWatermark textWatermark = watermark as TextWatermark;
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer(watermark.Document);
    msofbtSpContainer.CreateTextWatermarkContainer(this.GetWatermarkNumber(), textWatermark);
    if ((double) textWatermark.Height != -1.0)
    {
      fspa.Height = (int) textWatermark.Height * 20;
      fspa.Width = (int) textWatermark.Width * 20;
    }
    else
    {
      fspa.Height = (int) ((double) textWatermark.ShapeSize.Height * 13.430000305175781);
      fspa.Width = (int) ((double) textWatermark.ShapeSize.Width * 13.880000114440918);
    }
    textWatermark.VerticalPosition = 0.0f;
    textWatermark.HorizontalPosition = 0.0f;
    fspa.YaTop = (int) textWatermark.VerticalPosition * 20;
    fspa.XaLeft = (int) textWatermark.HorizontalPosition * 20;
    fspa.Spid = this.m_curTxbxId;
    msofbtSpContainer.Shape.ShapeId = this.m_curTxbxId;
    ++this.m_curTxbxId;
    msofbtSpContainer.IsWatermark = true;
    return msofbtSpContainer;
  }

  private MsofbtSpContainer InsertPictureWatermark(
    Watermark watermark,
    FileShapeAddress fspa,
    UnitsConvertor unitsConvertor,
    float maxWidth)
  {
    MsofbtSpContainer pictContainer = new MsofbtSpContainer(watermark.Document);
    PictureWatermark pictureWatermark = watermark as PictureWatermark;
    SizeF page = this.FitPictureToPage(pictureWatermark, maxWidth, unitsConvertor);
    this.CreatePictureWatermarkCont(pictureWatermark, pictContainer);
    float num1 = page.Height * 20f;
    float num2 = page.Width * 20f;
    fspa.Width = (int) ((double) num2 / 100.0 * (double) pictureWatermark.Scaling);
    fspa.Height = (int) ((double) num1 / 100.0 * (double) pictureWatermark.Scaling);
    fspa.YaTop = (int) Math.Round((double) pictureWatermark.WordPicture.VerticalPosition * 20.0);
    fspa.XaLeft = (int) Math.Round((double) pictureWatermark.WordPicture.HorizontalPosition * 20.0);
    fspa.RelVrtPos = pictureWatermark.WordPicture.VerticalOrigin;
    fspa.RelHrzPos = pictureWatermark.WordPicture.HorizontalOrigin == HorizontalOrigin.LeftMargin || pictureWatermark.WordPicture.HorizontalOrigin == HorizontalOrigin.RightMargin || pictureWatermark.WordPicture.HorizontalOrigin == HorizontalOrigin.InsideMargin || pictureWatermark.WordPicture.HorizontalOrigin == HorizontalOrigin.OutsideMargin ? HorizontalOrigin.Margin : pictureWatermark.WordPicture.HorizontalOrigin;
    fspa.TextWrappingStyle = pictureWatermark.WordPicture.TextWrappingStyle;
    fspa.TextWrappingType = pictureWatermark.WordPicture.TextWrappingType;
    fspa.IsBelowText = pictureWatermark.WordPicture.IsBelowText;
    fspa.Spid = this.m_curPicId;
    pictContainer.Shape.ShapeId = this.m_curPicId;
    ++this.m_curPicId;
    pictContainer.IsWatermark = true;
    return pictContainer;
  }

  private SizeF FitPictureToPage(
    PictureWatermark picWatermark,
    float maxWidth,
    UnitsConvertor unitsConvertor)
  {
    float height = picWatermark.WordPicture.Size.Height;
    float width = picWatermark.WordPicture.Size.Width;
    SizeF page = new SizeF(width, height);
    if ((double) width > (double) maxWidth && !WordDocument.EnablePartialTrustCode)
    {
      Image picture = picWatermark.Picture;
    }
    return page;
  }

  private void CreatePictureWatermarkCont(
    PictureWatermark pictWatermark,
    MsofbtSpContainer pictContainer)
  {
    if (pictWatermark.OriginalPib != -1)
    {
      bool flag = this.Escher.CheckBStoreContByPid(pictWatermark.OriginalPib);
      pictWatermark.OriginalPib = flag ? pictWatermark.OriginalPib : -1;
    }
    pictContainer.CreatePictWatermarkContainer(this.GetWatermarkNumber(), pictWatermark);
    pictContainer.Pib = pictWatermark.OriginalPib;
  }

  private int GetWatermarkNumber()
  {
    int watermarkNumber;
    switch ((this as WordHeaderFooterWriter).HeaderType)
    {
      case HeaderType.OddHeader:
        watermarkNumber = 2;
        break;
      case HeaderType.FirstPageHeader:
        watermarkNumber = 1;
        break;
      default:
        watermarkNumber = 3;
        break;
    }
    return watermarkNumber;
  }

  private bool IsImageEqual(byte[] imageHash1, byte[] imageHash2)
  {
    bool flag = true;
    for (int index = 0; index < imageHash1.Length && index < imageHash2.Length; ++index)
    {
      if ((int) imageHash1[index] != (int) imageHash2[index])
      {
        flag = false;
        break;
      }
    }
    return flag;
  }
}
