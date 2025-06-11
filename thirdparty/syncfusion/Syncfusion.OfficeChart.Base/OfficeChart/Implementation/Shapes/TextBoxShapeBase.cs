// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.TextBoxShapeBase
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class TextBoxShapeBase : ShapeImpl
{
  private const int DEF_CONTINUE_FR_SIZE = 8;
  private const uint DEF_TEXTDIRECTION = 2;
  private OfficeCommentHAlign m_hAlign = OfficeCommentHAlign.Left;
  private OfficeCommentVAlign m_vAlign = OfficeCommentVAlign.Top;
  private OfficeTextRotation m_textRotation;
  private bool m_bTextLocked = true;
  private RichTextString m_strText;
  private int m_iTextLen;
  private int m_iFormattingLen;
  private Color m_fillColor = ColorExtension.Empty;
  private Dictionary<string, string> m_unknownBodyProperties;
  private RichTextReader m_richTextReader;
  protected WorksheetImpl m_sheet;
  private ChartColor m_colorObject;

  public OfficeCommentHAlign HAlignment
  {
    get => this.m_hAlign;
    set => this.m_hAlign = value;
  }

  public OfficeCommentVAlign VAlignment
  {
    get => this.m_vAlign;
    set => this.m_vAlign = value;
  }

  public OfficeTextRotation TextRotation
  {
    get => this.m_textRotation;
    set => this.m_textRotation = value;
  }

  public bool IsTextLocked
  {
    get => this.m_bTextLocked;
    set => this.m_bTextLocked = value;
  }

  public IRichTextString RichText
  {
    get
    {
      if (this.m_strText == null)
        this.InitializeVariables();
      return (IRichTextString) this.m_strText;
    }
    set => this.m_strText = value as RichTextString;
  }

  internal RichTextReader RichTextReader
  {
    get
    {
      if (this.m_richTextReader == null)
        this.m_richTextReader = new RichTextReader((IWorksheet) this.m_sheet);
      return this.m_richTextReader;
    }
  }

  public string Text
  {
    get => this.RichText.Text;
    set => this.RichText.Text = value;
  }

  internal RichTextString InnerRichText => this.m_strText;

  public Color FillColor
  {
    get => this.m_fillColor;
    set => this.m_fillColor = value;
  }

  public Dictionary<string, string> UnknownBodyProperties
  {
    get => this.m_unknownBodyProperties;
    set => this.m_unknownBodyProperties = value;
  }

  public ChartColor ColorObject
  {
    get => this.m_colorObject;
    set => this.m_colorObject = value;
  }

  public TextBoxShapeBase(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeVariables();
  }

  [CLSCompliant(false)]
  public TextBoxShapeBase(
    IApplication application,
    object parent,
    MsofbtSpContainer container,
    OfficeParseOptions options)
    : base(application, parent, container, options)
  {
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    TextBoxShapeBase parent1 = (TextBoxShapeBase) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    int num = 0;
    if (parent1.m_strText != null)
    {
      parent1.m_strText = (RichTextString) this.m_strText.Clone((object) parent1);
      num = parent1.m_strText.TextObject.FormattingRunsCount;
    }
    WorkbookImpl workbook = parent1.Workbook as WorkbookImpl;
    for (int index = 0; index < num; ++index)
    {
      if (index < this.m_strText.Text.Length)
      {
        IOfficeFont font = this.m_strText.GetFont(index, true);
        FontWrapper fontWrapper = workbook.AddFont(font) as FontWrapper;
        parent1.m_strText.TextObject.SetFontByIndex(index, fontWrapper.FontIndex);
      }
    }
    if (this.m_unknownBodyProperties != null)
    {
      parent1.m_unknownBodyProperties = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> unknownBodyProperty in this.m_unknownBodyProperties)
        parent1.m_unknownBodyProperties.Add(unknownBodyProperty.Key, unknownBodyProperty.Value);
    }
    return (IShape) parent1;
  }

  internal void SetText(TextWithFormat text)
  {
    if (text == null)
      throw new ArgumentNullException("commentText");
    ((RichTextString) this.RichText).SetTextObject(text);
  }

  [CLSCompliant(false)]
  protected MsofbtClientTextBox GetClientTextBoxRecord(MsoBase parent)
  {
    MsofbtClientTextBox record1 = (MsofbtClientTextBox) MsoFactory.GetRecord(MsoRecords.msofbtClientTextbox);
    TextObjectRecord record2 = (TextObjectRecord) BiffRecordFactory.GetRecord(TBIFFRecord.TextObject);
    int length = this.m_strText != null ? this.m_strText.Text.Length : 0;
    record2.HAlignment = this.HAlignment;
    record2.VAlignment = this.VAlignment;
    record2.TextLen = (ushort) length;
    record2.FormattingRunsLen = (ushort) 0;
    record2.IsLockText = this.IsTextLocked;
    record2.Rotation = this.TextRotation;
    record1.TextObject = record2;
    if (length > 0)
    {
      this.AddTextContinueRecords(record1);
      this.AddFormattingContinueRecords(record1, record2);
    }
    return record1;
  }

  private void AddFormattingContinueRecords(MsofbtClientTextBox result, TextObjectRecord textObject)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    byte[] longFr = this.ConvertFromShortToLongFR(this.SerializeFormattingRuns());
    int length1 = longFr != null ? longFr.Length : 0;
    int length2;
    if (longFr != null)
    {
      for (int srcOffset = 0; srcOffset < length1; srcOffset += length2)
      {
        length2 = Math.Min(length1 - srcOffset, 8224);
        ContinueRecord record = (ContinueRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Continue);
        byte[] numArray = new byte[length2];
        Buffer.BlockCopy((Array) longFr, srcOffset, (Array) numArray, 0, length2);
        record.SetData(numArray);
        record.SetLength(length2);
        result.AddRecord((BiffRecordRaw) record);
      }
    }
    else
    {
      ContinueRecord record = (ContinueRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Continue);
      record.SetLength(0);
      result.AddRecord((BiffRecordRaw) record);
    }
    textObject.FormattingRunsLen = (ushort) length1;
  }

  private void AddTextContinueRecords(MsofbtClientTextBox result)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    string text = this.m_strText.Text;
    int length1 = text.Length;
    int startIndex = 0;
    while (startIndex < length1)
    {
      int length2 = Math.Min(length1 - startIndex, 4111);
      ContinueRecord record = (ContinueRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Continue);
      record.AutoGrowData = true;
      string str = text.Substring(startIndex, length2);
      int len = record.SetStringNoLenDetectEncoding(0, str);
      record.SetLength(len);
      startIndex += length2;
      result.AddRecord((BiffRecordRaw) record);
    }
  }

  private void ParseTextObject(TextObjectRecord textObject)
  {
    this.m_hAlign = textObject != null ? textObject.HAlignment : throw new ArgumentNullException(nameof (textObject));
    this.m_vAlign = textObject.VAlignment;
    this.m_textRotation = textObject.Rotation;
    this.m_bTextLocked = textObject.IsLockText;
    this.m_iTextLen = (int) textObject.TextLen;
    this.m_iFormattingLen = (int) textObject.FormattingRunsLen;
  }

  private void ParseContinueRecords(
    string strText,
    byte[] formattingRuns,
    OfficeParseOptions options)
  {
    TextWithFormat text = new TextWithFormat();
    text.Text = strText;
    if (formattingRuns != null)
    {
      int num = formattingRuns.Length / 8;
      byte[] numArray = new byte[num * 4];
      for (int index = 0; index < num; ++index)
        Buffer.BlockCopy((Array) formattingRuns, index * 8, (Array) numArray, index * 4, 4);
      text.ParseFormattingRuns(numArray);
    }
    this.m_strText.Parse(text, (Dictionary<int, int>) null, options);
  }

  private byte[] SerializeFormattingRuns()
  {
    byte[] numArray1 = this.m_strText.TextObject.SerializeFormatting();
    if (numArray1 == null || numArray1.Length == 0)
    {
      byte[] numArray2 = new byte[8];
      byte[] bytes = BitConverter.GetBytes((ushort) this.m_strText.Text.Length);
      numArray2[4] = bytes[0];
      numArray2[5] = bytes[1];
      return numArray2;
    }
    if (numArray1[0] != (byte) 0 || numArray1[1] != (byte) 0)
    {
      byte[] numArray3 = numArray1;
      numArray1 = new byte[numArray1.Length + 4];
      numArray3.CopyTo((Array) numArray1, 4);
      BitConverter.GetBytes((ushort) 0).CopyTo((Array) numArray1, 0);
      BitConverter.GetBytes((ushort) 0).CopyTo((Array) numArray1, 1);
    }
    int length = numArray1.Length;
    if ((int) BitConverter.ToUInt16(numArray1, length - 4) != this.m_strText.Text.Length)
    {
      byte[] numArray4 = numArray1;
      numArray1 = new byte[numArray1.Length + 4];
      numArray4.CopyTo((Array) numArray1, 0);
      BitConverter.GetBytes((ushort) this.m_strText.Text.Length).CopyTo((Array) numArray1, numArray4.Length);
      BitConverter.GetBytes((ushort) 0).CopyTo((Array) numArray1, numArray4.Length + 2);
    }
    return numArray1;
  }

  private byte[] ConvertFromShortToLongFR(byte[] arrShortFR)
  {
    if (arrShortFR == null)
      return (byte[]) null;
    int num = arrShortFR.Length / 4;
    byte[] dst = new byte[num * 8];
    for (int index = 0; index < num; ++index)
      Buffer.BlockCopy((Array) arrShortFR, index * 4, (Array) dst, index * 8, 4);
    return dst;
  }

  protected virtual void InitializeVariables()
  {
    this.m_strText = new RichTextString(this.Application, (object) this.ParentWorkbook, (object) this, false, true);
    this.m_bSupportOptions = true;
  }

  [CLSCompliant(false)]
  protected virtual void ParseClientTextBoxRecord(
    MsofbtClientTextBox textBox,
    OfficeParseOptions options)
  {
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    this.RichText.Text = string.Empty;
    this.ParseTextObject(textBox.TextObject);
    string text = textBox.Text;
    byte[] formattingRuns = textBox.FormattingRuns;
    if (text == null || formattingRuns == null)
      return;
    this.ParseContinueRecords(text, formattingRuns, options);
  }

  public void CopyFrom(TextBoxShapeBase source, Dictionary<int, int> dicFontIndexes)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.m_strText.CopyFrom(source.m_strText, dicFontIndexes);
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT CreateDefaultOptions()
  {
    MsofbtOPT defaultOptions = base.CreateDefaultOptions();
    defaultOptions.Version = 3;
    defaultOptions.Instance = 2;
    if (this.Text.Length != 0)
      this.SerializeOption(defaultOptions, MsoOptions.TextId, 19990000);
    this.SerializeTextDirection(defaultOptions);
    this.SerializeSizeTextToFit(defaultOptions);
    return defaultOptions;
  }

  [CLSCompliant(false)]
  protected void SerializeTextDirection(MsofbtOPT options)
  {
    this.SerializeOption(options, MsoOptions.TextDirection, 2U);
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT SerializeOptions(MsoBase parent)
  {
    if (this.m_options != null && this.m_options.Properties.Length != 0)
      return this.m_options;
    MsofbtOPT options = this.m_options;
    if (this.m_bUpdateLineFill || this.m_options == null)
    {
      MsofbtOPT msofbtOpt = this.m_options = this.CreateDefaultOptions();
      options = this.SerializeMsoOptions(this.m_options);
    }
    this.SerializeShapeName(options);
    this.SerializeName(options, MsoOptions.AlternativeText, this.AlternativeText);
    return this.m_options;
  }

  [CLSCompliant(false)]
  protected override void ParseOtherRecords(MsoBase subRecord, OfficeParseOptions options)
  {
    if (subRecord == null)
      throw new ArgumentNullException(nameof (subRecord));
    if (subRecord.MsoRecordType != MsoRecords.msofbtClientTextbox)
      return;
    this.ParseClientTextBoxRecord(subRecord as MsofbtClientTextBox, options);
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this.m_unknownBodyProperties != null)
      this.m_unknownBodyProperties.Clear();
    if (this.m_strText == null)
      return;
    this.m_strText = (RichTextString) null;
  }
}
