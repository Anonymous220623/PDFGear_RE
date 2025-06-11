// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.RichTextString
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class RichTextString : CommonWrapper, IRichTextString, IParentApplication, IOptimizedUpdate
{
  private const char DEF_ZERO = 'X';
  protected TextWithFormat m_text;
  private string m_rtfText;
  protected WorkbookImpl m_book;
  private bool m_bIsReadOnly;
  private object m_rtfParent;
  private static readonly char[] DEF_DIGITS = new char[10]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9'
  };
  private object m_parent;
  private int m_iFontIndex;
  private string m_imageRTF;
  private PreservationLogger m_logger;

  public RichTextString(IApplication application, object parent)
  {
    this.m_parent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
    this.SetParents();
    this.m_logger = new PreservationLogger();
  }

  public RichTextString(IApplication application, object parent, bool isReadOnly)
    : this(application, parent, isReadOnly, false)
  {
  }

  public RichTextString(
    IApplication application,
    object parent,
    object rtfParent,
    bool isReadOnly,
    bool bCreateText)
    : this(application, parent, isReadOnly, bCreateText)
  {
    this.m_rtfParent = rtfParent;
  }

  public RichTextString(
    IApplication application,
    object parent,
    bool isReadOnly,
    bool bCreateText)
    : this(application, parent)
  {
    this.m_bIsReadOnly = isReadOnly;
    if (!bCreateText)
      return;
    this.m_text = new TextWithFormat();
  }

  internal RichTextString(
    IApplication application,
    object parent,
    bool isReadOnly,
    bool bCreateText,
    PreservationLogger logger)
    : this(application, parent)
  {
    this.m_bIsReadOnly = isReadOnly;
    if (bCreateText)
      this.m_text = new TextWithFormat();
    this.m_logger = logger;
  }

  public RichTextString(IApplication application, object parent, TextWithFormat text)
    : this(application, parent)
  {
    this.m_text = text;
  }

  protected virtual void SetParents()
  {
    this.m_book = CommonObject.FindParent(this.m_parent, typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  public IOfficeFont GetFont(int iPosition)
  {
    if (iPosition < 0 || iPosition >= this.m_text.Text.Length)
      throw new ArgumentOutOfRangeException(nameof (iPosition));
    return (IOfficeFont) new FontWrapper(this.GetFontByIndex(this.m_text.GetTextFontIndex(iPosition)), true, false);
  }

  public IOfficeFont GetFont(int iPosition, bool isCopy)
  {
    if (iPosition < 0 || iPosition >= this.m_text.Text.Length)
      throw new ArgumentOutOfRangeException(nameof (iPosition));
    return (IOfficeFont) new FontWrapper(this.GetFontByIndex(this.m_text.GetTextFontIndex(iPosition, isCopy)), true, false);
  }

  public void SetFont(int iStartPos, int iEndPos, IOfficeFont font)
  {
    this.BeginUpdate();
    int iFontIndex = this.AddFont(font);
    if (iStartPos == 0)
    {
      if (this.m_text.FormattingRunsCount > 0)
      {
        int length = this.m_text.Text.Length;
        int defaultFontIndex = this.DefaultFontIndex;
        int num = length < iEndPos + 1 ? this.m_text.GetTextFontIndex(iEndPos + 1) : -1;
        this.m_text.ReplaceFont(0, defaultFontIndex);
        if (num >= 0)
          this.m_text.FormattingRuns[iEndPos + 1] = num;
        this.m_text.SetTextFontIndex(iStartPos, iEndPos, iFontIndex);
      }
      else if (iEndPos < this.m_text.Text.Length - 1)
        this.SetFont(iEndPos + 1, this.m_text.Text.Length - 1, (IOfficeFont) this.DefaultFont);
      this.DefaultFont = font is FontWrapper ? (font as FontWrapper).Wrapped : font as FontImpl;
      if (iFontIndex < 0)
        iFontIndex = 0;
      else
        this.DefaultFontIndex = iFontIndex;
      if (this.m_text.Text.Length > 0)
        this.m_text.SetTextFontIndex(iStartPos, iEndPos, iFontIndex);
    }
    else
      this.m_text.SetTextFontIndex(iStartPos, iEndPos, iFontIndex);
    this.EndUpdate();
  }

  public void ClearFormatting()
  {
    if (this.m_text == null || !this.IsFormatted)
      return;
    this.BeginUpdate();
    this.m_text.ClearFormatting();
    this.EndUpdate();
  }

  public string Text
  {
    get
    {
      if (this.m_text == null)
        this.m_text = new TextWithFormat();
      return this.m_text.Text;
    }
    set
    {
      this.BeginUpdate();
      string text = this.m_text.Text;
      this.m_text.Text = value;
      if (text.Length < this.m_text.Text.Length && this.IsFormatted)
        this.m_text.SetTextFontIndex(text.Length, this.m_text.Text.Length - 1, this.m_text.FormattingRuns.Values[this.m_text.FormattingRuns.Values.Count - 1]);
      else if (this.IsFormatted)
      {
        int iFontIndex = this.m_text.FormattingRuns.Values[0];
        this.m_text.ClearFormatting();
        this.m_text.SetTextFontIndex(0, this.m_text.Text.Length - 1, iFontIndex);
      }
      this.EndUpdate();
    }
  }

  public string RtfText
  {
    get => this.m_rtfParent != null ? this.m_rtfText : this.GenerateRtfText();
    set
    {
      this.m_rtfText = value;
      if (this.m_rtfParent == null || !(this.m_rtfParent is TextBoxShapeBase))
        return;
      (this.m_rtfParent as TextBoxShapeImpl).RichTextReader.SetRTF(this.m_rtfParent, this.m_rtfText);
    }
  }

  public bool IsFormatted => this.m_text.FormattingRunsCount > 0;

  public void Append(string text, IOfficeFont font)
  {
    this.BeginUpdate();
    int length = this.m_text.Text.Length;
    this.m_text.Text += text;
    this.SetFont(length, length + text.Length - 1, font);
    this.EndUpdate();
  }

  public void Substring(int startIndex, int length)
  {
    string text = this.m_text.Text;
    if (startIndex > 0)
    {
      if (startIndex >= text.Length)
      {
        this.m_text.Text = string.Empty;
        this.ClearFormatting();
      }
      else
        this.m_text.RemoveAtStart(startIndex);
    }
    this.m_text.RemoveAtEnd(this.m_text.Text.Length - length);
  }

  internal void SetText(string text)
  {
    this.BeginUpdate();
    string text1 = this.m_text.Text;
    this.m_text.Text = text;
    this.EndUpdate();
  }

  internal void UpdateRTF(IRange range, FontImpl font)
  {
    IWorkbook workbook = range.Worksheet.Workbook;
    IRichTextString richText = range.RichText;
    string text = range.RichText.Text;
    workbook.CreateFont();
    IOfficeFont font1 = (IOfficeFont) font;
    richText.SetFont(0, text.Length - 1, font1);
  }

  public object Parent => this.m_parent;

  public IApplication Application => this.m_book.Application;

  public SizeF StringSize
  {
    get
    {
      SizeF stringSize = new SizeF(0.0f, 0.0f);
      int iStartPos = 0;
      int iIndex = 0;
      for (int formattingRunsCount = this.m_text.FormattingRunsCount; iIndex < formattingRunsCount; ++iIndex)
      {
        int positionByIndex = this.m_text.GetPositionByIndex(iIndex);
        SizeF sizePart = this.GetSizePart(iStartPos, positionByIndex);
        stringSize.Width += sizePart.Width;
        stringSize.Height = Math.Max(sizePart.Height, stringSize.Height);
        iStartPos = positionByIndex;
      }
      SizeF sizePart1 = this.GetSizePart(iStartPos, this.Text.Length);
      stringSize.Width += sizePart1.Width;
      stringSize.Height = Math.Max(sizePart1.Height, stringSize.Height);
      return stringSize;
    }
  }

  public virtual FontImpl DefaultFont
  {
    get => (FontImpl) this.m_book.InnerFonts[this.m_iFontIndex];
    internal set => this.m_iFontIndex = value.Index;
  }

  public TextWithFormat TextObject => this.m_text;

  public WorkbookImpl Workbook => this.m_book;

  public int DefaultFontIndex
  {
    get => this.m_iFontIndex;
    set => this.m_iFontIndex = value;
  }

  internal string ImageRTF
  {
    get => this.m_imageRTF;
    set => this.m_imageRTF = value;
  }

  protected virtual int GetFontIndex(int iPosition) => this.m_text.GetFontByIndex(iPosition);

  protected virtual FontImpl GetFontByIndex(int iFontIndex)
  {
    return iFontIndex != 0 || this.DefaultFontIndex < 0 ? (FontImpl) this.m_book.InnerFonts[iFontIndex] : this.DefaultFont;
  }

  public override void BeginUpdate()
  {
    if (this.m_bIsReadOnly)
      throw new ReadOnlyException();
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    this.m_logger.SetFlag(PreservedFlag.RichText);
  }

  public virtual void CopyFrom(RichTextString source, Dictionary<int, int> dicFontIndexes)
  {
    this.BeginUpdate();
    this.m_text = source.m_text.Clone(dicFontIndexes);
    this.EndUpdate();
  }

  public virtual void Parse(
    TextWithFormat text,
    Dictionary<int, int> dicFontIndexes,
    OfficeParseOptions options)
  {
    this.m_text = text != null ? text.TypedClone() : throw new ArgumentNullException(nameof (text));
    FontsCollection innerFonts = this.m_book.InnerFonts;
    int formattingRunsCount = text.FormattingRunsCount;
    if (formattingRunsCount <= 0)
      return;
    for (int index = 0; index < formattingRunsCount; ++index)
    {
      int iFontIndex = FontImpl.UpdateFontIndexes(text.GetFontByIndex(index), dicFontIndexes, options);
      if (iFontIndex > innerFonts.Count)
        iFontIndex = 0;
      this.m_text.SetFontByIndex(index, iFontIndex);
    }
  }

  public override object Clone(object parent)
  {
    RichTextString richTextString = parent != null ? (RichTextString) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    richTextString.m_parent = parent;
    richTextString.m_text = this.m_text.TypedClone();
    this.SetParents();
    return (object) richTextString;
  }

  public virtual void Clear()
  {
    this.m_text = this.m_text.TypedClone();
    this.m_text.ClearFormatting();
    this.m_text.Text = string.Empty;
  }

  protected virtual int AddFont(IOfficeFont font)
  {
    return (this.m_book.InnerFonts.Add((IOfficeFont) ((IInternalFont) font).Font) as FontImpl).Index;
  }

  internal void SetTextObject(TextWithFormat commentText)
  {
    this.m_text = commentText != null ? commentText : throw new ArgumentNullException(nameof (commentText));
  }

  internal FontImpl GetFontObject(int iPosition)
  {
    if (iPosition < 0 || iPosition >= this.m_text.Text.Length)
      throw new ArgumentOutOfRangeException(nameof (iPosition));
    return this.GetFontByIndex(this.m_text.GetTextFontIndex(iPosition));
  }

  private SizeF GetSizePart(int iStartPos, int iEndPos)
  {
    if (iStartPos >= iEndPos)
      return new SizeF(0.0f, 0.0f);
    FontImpl fontObject = this.GetFontObject(iStartPos);
    int length = iEndPos - iStartPos;
    string strValue = this.Text.Substring(iStartPos, length);
    strValue.IndexOfAny(RichTextString.DEF_DIGITS);
    return fontObject.MeasureStringSpecial(strValue);
  }

  internal string GenerateRtfText()
  {
    this.m_text.Defragment();
    RtfTextWriter writer = new RtfTextWriter();
    this.AddFonts(writer);
    string text = this.m_text.Text;
    int formattingRunsCount = this.m_text.FormattingRunsCount;
    int iStartPos = 0;
    if (text.Length > 0)
    {
      if (formattingRunsCount > 0)
      {
        for (int iRunIndex = 0; iRunIndex <= formattingRunsCount; ++iRunIndex)
          iStartPos = this.WriteFormattingRun(writer, iRunIndex, iStartPos);
      }
      else
        this.WriteText(writer, this.DefaultFont.Index, text);
    }
    writer.WriteTag(RtfTags.RtfEnd);
    return writer.ToString();
  }

  internal string GenerateRtfText(string alignment)
  {
    this.m_text.Defragment();
    RtfTextWriter writer = new RtfTextWriter();
    this.AddFonts(writer, alignment);
    string text = this.m_text.Text;
    int formattingRunsCount = this.m_text.FormattingRunsCount;
    int iStartPos = 0;
    if (text.Length > 0)
    {
      if (formattingRunsCount > 0)
      {
        for (int iRunIndex = 0; iRunIndex <= formattingRunsCount; ++iRunIndex)
          iStartPos = this.WriteFormattingRun(writer, iRunIndex, iStartPos, alignment);
      }
      else
        this.WriteText(writer, this.DefaultFont.Index, text, alignment);
    }
    writer.WriteTag(RtfTags.RtfEnd);
    return writer.ToString();
  }

  private int WriteFormattingRun(RtfTextWriter writer, int iRunIndex, int iStartPos)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int formattingRunsCount = this.m_text.FormattingRunsCount;
    if (iRunIndex < 0 || iRunIndex > formattingRunsCount)
      throw new ArgumentOutOfRangeException(nameof (iRunIndex), "Value cannot be less than 0 and greater than iCount - 1");
    string text = this.m_text.Text;
    int num = iRunIndex == formattingRunsCount ? text.Length : this.m_text.GetPositionByIndex(iRunIndex);
    if (num == iStartPos)
      return iStartPos;
    string strText = text.Substring(iStartPos, num - iStartPos);
    int iFontIndex = iRunIndex == 0 ? 0 : this.m_text.GetFontByIndex(iRunIndex - 1);
    if (iFontIndex == 0)
      iFontIndex = this.DefaultFont.Index;
    this.WriteText(writer, iFontIndex, strText);
    return num;
  }

  private int WriteFormattingRun(
    RtfTextWriter writer,
    int iRunIndex,
    int iStartPos,
    string alignment)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int formattingRunsCount = this.m_text.FormattingRunsCount;
    if (iRunIndex < 0 || iRunIndex > formattingRunsCount)
      throw new ArgumentOutOfRangeException(nameof (iRunIndex), "Value cannot be less than 0 and greater than iCount - 1");
    string text = this.m_text.Text;
    int num = iRunIndex == formattingRunsCount ? text.Length : this.m_text.GetPositionByIndex(iRunIndex);
    if (num == iStartPos)
      return iStartPos;
    string strText = text.Substring(iStartPos, num - iStartPos);
    int iFontIndex = iRunIndex == 0 ? 0 : this.m_text.GetFontByIndex(iRunIndex - 1);
    if (iFontIndex == 0)
      iFontIndex = this.DefaultFont.Index;
    this.WriteText(writer, iFontIndex, strText, alignment);
    return num;
  }

  private void AddFonts(RtfTextWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int iIndex = 0;
    for (int formattingRunsCount = this.m_text.FormattingRunsCount; iIndex < formattingRunsCount; ++iIndex)
    {
      int fontByIndex = this.m_text.GetFontByIndex(iIndex);
      if (fontByIndex != 0)
        this.AddFont(this.GetFontByIndex(fontByIndex), writer);
    }
    this.AddFont(this.DefaultFont, writer);
    writer.WriteTag(RtfTags.RtfBegin);
    writer.WriteFontTable();
    writer.WriteColorTable();
  }

  private void AddFonts(RtfTextWriter writer, string alignment)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    int iIndex = 0;
    for (int formattingRunsCount = this.m_text.FormattingRunsCount; iIndex < formattingRunsCount; ++iIndex)
    {
      int fontByIndex = this.m_text.GetFontByIndex(iIndex);
      if (fontByIndex != 0)
        this.AddFont(this.GetFontByIndex(fontByIndex), writer);
    }
    this.AddFont(this.DefaultFont, writer);
    writer.WriteTag(RtfTags.RtfBegin);
    writer.WriteFontTable();
    writer.WriteColorTable();
    writer.WriteAlignment(alignment);
  }

  private void AddFont(FontImpl fontToAdd, RtfTextWriter writer)
  {
    if (fontToAdd == null)
      throw new ArgumentNullException(nameof (fontToAdd));
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    Font nativeFont = fontToAdd.GenerateNativeFont();
    writer.AddFont(nativeFont);
    writer.AddColor(fontToAdd.RGBColor);
  }

  private void WriteText(RtfTextWriter writer, int iFontIndex, string strText)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (strText)
    {
      case null:
        throw new ArgumentNullException(nameof (strText));
      case "":
        throw new ArgumentException("strText - string cannot be empty");
      default:
        IOfficeFont fontByIndex = (IOfficeFont) this.GetFontByIndex(iFontIndex);
        writer.WriteText(fontByIndex, strText);
        break;
    }
  }

  private void WriteText(RtfTextWriter writer, int iFontIndex, string strText, string alignment)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    switch (strText)
    {
      case null:
        throw new ArgumentNullException(nameof (strText));
      case "":
        throw new ArgumentException("strText - string cannot be empty");
      default:
        IOfficeFont fontByIndex = (IOfficeFont) this.GetFontByIndex(iFontIndex);
        writer.WriteImageText(fontByIndex, strText, this.ImageRTF, alignment);
        break;
    }
  }

  internal void AddText(string text, IOfficeFont font)
  {
    int length = this.m_text.Text.Replace("\r\n", string.Empty).Length;
    this.m_text.Text += text;
    this.SetFont(length, this.m_text.Text.Length - 1, font);
  }
}
