// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocReaderAdapter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class DocReaderAdapter : DocReaderAdapterBase
{
  private DocReaderAdapter.AnnotationAdapter m_annAdapter;
  private DocReaderAdapter.FootnoteAdapter m_ftnAdapter;
  private DocReaderAdapter.EndnoteAdapter m_endNoteAdapter;
  private DocReaderAdapter.TextboxAdapter m_txbxAdapter;
  private DocReaderAdapter.HeaderFooterAdapter m_hfAdapter;

  public void Read(WordReader reader, WordDocument wordDoc)
  {
    AdapterListIDHolder.Instance.LfoStyleIDtoName.Clear();
    AdapterListIDHolder.Instance.ListStyleIDtoName.Clear();
    this.Init(wordDoc);
    this.ReadPassword(reader);
    reader.ReadDocumentHeader(wordDoc);
    wordDoc.WriteProtected = reader.TablesData.Fib.FReadOnlyRecommended;
    wordDoc.HasPicture = reader.TablesData.Fib.FHasPic;
    wordDoc.WordVersion = reader.TablesData.Fib.FibVersion;
    this.ReadDOP(reader);
    wordDoc.IsOpening = true;
    this.ReadStyleSheet(reader);
    wordDoc.FontSubstitutionTable = reader.StyleSheet.FontSubstitutionTable;
    this.ReadEscher(reader);
    this.ReadBackground();
    this.ReadSubDocument(reader, WordSubdocument.Footnote);
    this.ReadSubDocument(reader, WordSubdocument.Annotation);
    this.ReadSubDocument(reader, WordSubdocument.Endnote);
    this.ReadSubDocument(reader, WordSubdocument.TextBox);
    do
    {
      IWSection wsection = wordDoc.AddSection();
      this.ReadTextBody((WordReaderBase) reader, wsection.Body);
      this.ReadSectionFormat(reader, wsection);
      this.DocumentEx.SectionFormatChange(wsection as WSection);
    }
    while (reader.ChunkType != WordChunkType.DocumentEnd);
    this.ReadSubDocument(reader, WordSubdocument.HeaderFooter);
    this.ReadDocumentProperties(reader);
    if (wordDoc.Watermark.Type == WatermarkType.NoWatermark)
      this.CheckWatermark(wordDoc.Sections[0]);
    if (this.DocumentEx.HasListStyle())
      this.ParseListPicture();
    reader.ReadDocumentEnd();
    wordDoc.FFNStringTable = reader.m_docInfo.TablesData.FFNStringTable;
    if (reader.StyleSheet.StylesCount > 0)
      wordDoc.HasStyleSheets = true;
    wordDoc.Settings.SetCompatibilityModeValue(CompatibilityMode.Word2003);
    wordDoc.IsOpening = false;
    reader.Close();
    this.Close();
  }

  private void ReadPassword(WordReader reader)
  {
    if (this.DocumentEx.Password == null)
      return;
    reader.NeedPassword += new NeedPasswordEventHandler(this.DocumentEx.GetPasswordValue);
  }

  private void ReadStyleSheet(WordReader reader)
  {
    WordStyleSheet styleSheet = reader.StyleSheet;
    int stylesCount = styleSheet.StylesCount;
    Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
    for (int index = 0; index < stylesCount; ++index)
    {
      WordStyle styleByIndex = styleSheet.GetStyleByIndex(index);
      if (styleByIndex.Name != null)
      {
        string str1 = this.UpdateStyleNameBasedOnId(styleByIndex);
        if (dictionary1.ContainsKey(str1))
        {
          string str2 = str1;
          str1 = $"{str1}_{dictionary1[str1].ToString()}";
          while (dictionary1.ContainsKey(str1))
            str1 = $"{str1}_{dictionary1[str1].ToString()}";
          Dictionary<string, int> dictionary2;
          string key;
          (dictionary2 = dictionary1)[key = str2] = dictionary2[key] + 1;
        }
        else
          dictionary1.Add(str1, 0);
        if (index == 13)
        {
          styleSheet.IsFixedIndex13HasStyle = true;
          styleSheet.FixedIndex13StyleName = str1;
        }
        if (index == 14)
        {
          styleSheet.IsFixedIndex14HasStyle = true;
          styleSheet.FixedIndex14StyleName = str1;
        }
        styleByIndex.Name = str1;
        styleSheet.StyleNames.Add(index, str1);
        if (!styleByIndex.IsCharacterStyle)
        {
          IStyle style1 = this.DocumentEx.AddStyle(StyleType.ParagraphStyle, str1);
          WParagraphStyle wparagraphStyle = style1 as WParagraphStyle;
          wparagraphStyle.StyleId = styleByIndex.ID;
          wparagraphStyle.IsPrimaryStyle = styleByIndex.IsPrimary;
          wparagraphStyle.IsSemiHidden = styleByIndex.IsSemiHidden;
          wparagraphStyle.UnhideWhenUsed = styleByIndex.UnhideWhenUsed;
          (style1 as Style).TypeCode = styleByIndex.TypeCode;
          if (styleByIndex.TypeCode == WordStyleType.TableStyle && styleByIndex.TableStyleData != null)
          {
            Style style2 = style1 as Style;
            style2.TableStyleData = new byte[styleByIndex.TableStyleData.Length];
            Buffer.BlockCopy((Array) styleByIndex.TableStyleData, 0, (Array) style2.TableStyleData, 0, styleByIndex.TableStyleData.Length);
          }
          if (styleByIndex.PAPX != null)
            ParagraphPropertiesConverter.SprmsToFormat(styleByIndex.PAPX.PropertyModifiers, wparagraphStyle.ParagraphFormat, reader.SttbfRMarkAuthorNames, reader.StyleSheet);
          if (styleByIndex.CHPX != null)
            CharacterPropertiesConverter.SprmsToFormat(styleByIndex.CHPX.PropertyModifiers, wparagraphStyle.CharacterFormat, styleByIndex.StyleSheet, reader.SttbfRMarkAuthorNames, true);
          this.AddUsedFonts(wparagraphStyle.CharacterFormat);
          if (reader.HasList())
          {
            if (styleByIndex.PAPX != null)
              ListPropertiesConverter.Export(wparagraphStyle.ListFormat, styleByIndex.PAPX.PropertyModifiers, (WordReaderBase) reader);
            if (wparagraphStyle.ListFormat.CurrentListLevel != null)
              wparagraphStyle.ListFormat.CurrentListLevel.ParaStyleName = wparagraphStyle.Name;
          }
        }
        else
        {
          WCharacterStyle wcharacterStyle = (WCharacterStyle) this.DocumentEx.AddStyle(StyleType.CharacterStyle, str1);
          wcharacterStyle.StyleId = styleByIndex.ID;
          wcharacterStyle.IsPrimaryStyle = styleByIndex.IsPrimary;
          wcharacterStyle.IsSemiHidden = styleByIndex.IsSemiHidden;
          wcharacterStyle.UnhideWhenUsed = styleByIndex.UnhideWhenUsed;
          wcharacterStyle.TypeCode = styleByIndex.TypeCode;
          if (styleByIndex.TypeCode == WordStyleType.TableStyle && styleByIndex.TableStyleData != null)
          {
            Style style = (Style) wcharacterStyle;
            style.TableStyleData = new byte[styleByIndex.TableStyleData.Length];
            Buffer.BlockCopy((Array) styleByIndex.TableStyleData, 0, (Array) style.TableStyleData, 0, styleByIndex.TableStyleData.Length);
          }
          if (styleByIndex.CHPX != null)
            CharacterPropertiesConverter.SprmsToFormat(styleByIndex.CHPX.PropertyModifiers, wcharacterStyle.CharacterFormat, styleByIndex.StyleSheet, reader.SttbfRMarkAuthorNames, true);
          this.AddUsedFonts(wcharacterStyle.CharacterFormat);
        }
      }
    }
    int index1 = 0;
    for (int count = this.DocumentEx.Styles.Count; index1 < count; ++index1)
    {
      Style style = this.DocumentEx.Styles[index1] as Style;
      if (!string.IsNullOrEmpty(style.Name))
      {
        int index2 = styleSheet.StyleNameToIndex(style.Name, style.StyleType == StyleType.CharacterStyle);
        int baseStyleIndex = styleSheet.GetStyleByIndex(index2).BaseStyleIndex;
        if (baseStyleIndex != 4095 /*0x0FFF*/)
        {
          if (styleSheet.StyleNames.ContainsKey(baseStyleIndex) && style.StyleId != 0)
          {
            string styleName = styleSheet.StyleNames[baseStyleIndex];
            if (styleName != null)
              style.ApplyBaseStyle(styleName);
          }
        }
        else if (style.BaseStyle != null && style is WParagraphStyle)
          style.RemoveBaseStyle();
        int nextStyleIndex = styleSheet.GetStyleByIndex(index2).NextStyleIndex;
        if (nextStyleIndex != 4095 /*0x0FFF*/)
        {
          WordStyle styleByIndex = styleSheet.GetStyleByIndex(nextStyleIndex);
          if (styleByIndex != null)
            style.NextStyle = styleByIndex.Name;
        }
        int linkStyleIndex = styleSheet.GetStyleByIndex(index2).LinkStyleIndex;
        switch (linkStyleIndex)
        {
          case 0:
          case 4095 /*0x0FFF*/:
            continue;
          default:
            WordStyle styleByIndex1 = styleSheet.GetStyleByIndex(linkStyleIndex);
            if (styleByIndex1 != null)
            {
              style.LinkStyle = styleByIndex1.Name;
              continue;
            }
            continue;
        }
      }
    }
    this.DocumentEx.Styles.FixedIndex13HasStyle = styleSheet.IsFixedIndex13HasStyle;
    this.DocumentEx.Styles.FixedIndex14HasStyle = styleSheet.IsFixedIndex14HasStyle;
    this.DocumentEx.Styles.FixedIndex13StyleName = styleSheet.FixedIndex13StyleName;
    this.DocumentEx.Styles.FixedIndex14StyleName = styleSheet.FixedIndex14StyleName;
  }

  private string UpdateStyleNameBasedOnId(WordStyle wordStyle)
  {
    Style style = (Style) new WParagraphStyle((IWordDocument) this.DocumentEx);
    string name = wordStyle.Name;
    Dictionary<string, int> builtinStyleIds = style.GetBuiltinStyleIds();
    Dictionary<string, string> builtinStyles = style.GetBuiltinStyles();
    if (builtinStyleIds.ContainsValue(wordStyle.ID))
    {
      foreach (KeyValuePair<string, int> keyValuePair in builtinStyleIds)
      {
        if (keyValuePair.Value == wordStyle.ID)
        {
          using (Dictionary<string, string>.Enumerator enumerator = builtinStyles.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, string> current = enumerator.Current;
              if (current.Key.Replace(" ", string.Empty) == keyValuePair.Key)
              {
                name = current.Value;
                if (!this.DocumentEx.StyleNameIds.ContainsKey(keyValuePair.Key))
                {
                  this.DocumentEx.StyleNameIds.Add(keyValuePair.Key, current.Value);
                  break;
                }
                break;
              }
            }
            break;
          }
        }
      }
    }
    return name;
  }

  private void ReadEscher(WordReader reader)
  {
    if (reader.Escher == null)
      return;
    this.DocumentEx.Escher = reader.Escher;
  }

  private void ReadBackground() => this.DocumentEx.ReadBackground();

  private void ReadSubDocument(WordReader reader, WordSubdocument wsType)
  {
    if (!this.SubDocumentExists(reader, wsType))
      return;
    DocReaderAdapter.SubDocumentAdapter subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) null;
    switch (wsType)
    {
      case WordSubdocument.Footnote:
        subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) (this.m_ftnAdapter = new DocReaderAdapter.FootnoteAdapter());
        break;
      case WordSubdocument.HeaderFooter:
        subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) (this.m_hfAdapter = new DocReaderAdapter.HeaderFooterAdapter());
        break;
      case WordSubdocument.Endnote:
        subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) (this.m_endNoteAdapter = new DocReaderAdapter.EndnoteAdapter());
        break;
      case WordSubdocument.Annotation:
        subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) (this.m_annAdapter = new DocReaderAdapter.AnnotationAdapter());
        break;
      case WordSubdocument.TextBox:
        subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) (this.m_txbxAdapter = new DocReaderAdapter.TextboxAdapter());
        break;
    }
    subDocumentAdapter?.ReadSubDocBody(reader, this.DocumentEx);
  }

  private void ReadSectionFormat(WordReader reader, IWSection sec)
  {
    if (reader.ChunkType != WordChunkType.DocumentEnd)
    {
      this.ReadListFormat((WordReaderBase) reader, this.CurrentParagraph.ListFormat);
      this.ReadCharacterFormat((WordReaderBase) reader, this.CurrentParagraph.BreakCharacterFormat);
      this.ReadParagraphFormat((WordReaderBase) reader, (IWParagraph) this.CurrentParagraph);
      this.CheckTrackChanges(this.CurrentParagraph, (WordReaderBase) reader);
    }
    SectionPropertiesConverter.Export(reader, sec as WSection, true);
    this.m_currParagraph = (ITextBodyItem) null;
  }

  private void ReadDocumentProperties(WordReader reader)
  {
    if (reader.BuiltinDocumentProperties != null)
      this.DocumentEx.m_builtinProp = reader.BuiltinDocumentProperties;
    if (reader.CustomDocumentProperties != null)
      this.DocumentEx.m_customProp = reader.CustomDocumentProperties;
    this.DocumentEx.SttbfRMark = reader.SttbfRMark;
    if (reader.MacrosStream != null)
      this.DocumentEx.MacrosData = reader.MacrosStream.GetBuffer();
    if (reader.Variables != null)
      this.DocumentEx.Variables.UpdateVariables(reader.Variables);
    if (reader.MacroCommands != null)
      this.DocumentEx.MacroCommands = reader.MacroCommands;
    if (reader.AssociatedStrings != null)
      this.DocumentEx.AssociatedStrings.Parse(reader.AssociatedStrings);
    if (reader.GrammarSpellingData != null)
      this.DocumentEx.GrammarSpellingData = reader.GrammarSpellingData;
    if (reader.DOP != null)
    {
      this.DocumentEx.DifferentOddAndEvenPages = reader.DOP.OddAndEvenPagesHeaderFooter;
      this.DocumentEx.DefaultTabWidth = (float) reader.DOP.DefaultTabWidth / 20f;
    }
    if (reader.DOP.ViewType != (byte) 1)
      this.DocumentEx.ViewSetup.DocumentViewType = (DocumentViewType) reader.DOP.ViewType;
    if (reader.DOP.ZoomType != (byte) 0)
      this.DocumentEx.ViewSetup.ZoomType = (ZoomType) reader.DOP.ZoomType;
    if (reader.DOP.ZoomPercent != (ushort) 0 && reader.DOP.ZoomPercent != (ushort) 100)
      this.DocumentEx.ViewSetup.SetZoomPercentValue((int) reader.DOP.ZoomPercent);
    this.DocumentEx.StandardAsciiFont = reader.StandardAsciiFont;
    this.DocumentEx.StandardFarEastFont = reader.StandardFarEastFont;
    this.DocumentEx.StandardNonFarEastFont = reader.StandardNonFarEastFont;
    this.DocumentEx.StandardBidiFont = reader.StandardBidiFont;
    this.DocumentEx.Properties.SetVersion(reader.Version);
  }

  private void ReadBuiltInDocumentProperties(WordReader reader)
  {
    if (reader.BuiltinDocumentProperties == null)
      return;
    this.DocumentEx.m_builtinProp = reader.BuiltinDocumentProperties;
  }

  private void ReadDOP(WordReader reader)
  {
    if (reader.DOP == null)
      return;
    this.DocumentEx.DOP = reader.DOP;
  }

  private bool SubDocumentExists(WordReader reader, WordSubdocument wsType)
  {
    return reader.TablesData.HasSubdocument(wsType);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_annAdapter != null)
    {
      this.m_annAdapter.Close();
      this.m_annAdapter = (DocReaderAdapter.AnnotationAdapter) null;
    }
    if (this.m_ftnAdapter != null)
    {
      this.m_ftnAdapter.Close();
      this.m_ftnAdapter = (DocReaderAdapter.FootnoteAdapter) null;
    }
    if (this.m_endNoteAdapter != null)
    {
      this.m_endNoteAdapter.Close();
      this.m_endNoteAdapter = (DocReaderAdapter.EndnoteAdapter) null;
    }
    if (this.m_txbxAdapter != null)
    {
      this.m_txbxAdapter.Close();
      this.m_txbxAdapter = (DocReaderAdapter.TextboxAdapter) null;
    }
    if (this.m_hfAdapter == null)
      return;
    this.m_hfAdapter.Close();
    this.m_hfAdapter = (DocReaderAdapter.HeaderFooterAdapter) null;
  }

  protected override bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
  {
    return chunkType == WordChunkType.SectionEnd || chunkType == WordChunkType.DocumentEnd;
  }

  protected override void ReadAnnotation(WordReaderBase reader)
  {
    if (this.m_annAdapter == null)
      return;
    WComment nextComment = this.m_annAdapter.GetNextComment();
    if (nextComment == null)
      return;
    if (nextComment.Format.TagBkmk == "" || nextComment.Format.TagBkmk == "-1")
      nextComment.Format.UpdateTagBkmk();
    this.UpdateCommentMarks(nextComment);
    this.CurrentParagraph.Items.Add((IEntity) nextComment);
    this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 10);
    this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 6);
  }

  protected override void ReadFootnote(WordReaderBase reader)
  {
    WordReader wReader = reader as WordReader;
    bool flag = true;
    bool isCustomFootnoteSplittedText = false;
    string empty = string.Empty;
    while (wReader != null && flag)
    {
      WFootnote footnote = (WFootnote) null;
      string footNoteMarker = reader.TextChunk;
      if (wReader.IsFootnote)
      {
        footnote = this.m_ftnAdapter.GetNextFootEndNote();
        if (wReader.CustomFnSplittedTextLength > -1)
        {
          isCustomFootnoteSplittedText = true;
          footNoteMarker = wReader.CustomFnSplittedTextLength < wReader.TextChunk.Length ? reader.TextChunk.Substring(wReader.CustomFnSplittedTextLength) : footNoteMarker;
        }
      }
      else if (wReader.IsEndnote)
        footnote = this.m_endNoteAdapter.GetNextFootEndNote();
      flag = this.IsMultipleFootNoteEndNoteMarker(ref footNoteMarker, wReader, footnote, isCustomFootnoteSplittedText);
      if (footnote != null)
      {
        this.CurrentParagraph.Items.Add((IEntity) footnote);
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 21);
        if (reader.ChunkType != WordChunkType.Footnote)
        {
          footnote.CustomMarker = footNoteMarker;
          footnote.IsAutoNumbered = false;
        }
        if (reader.ChunkType == WordChunkType.Symbol)
        {
          SymbolDescriptor symbolDescriptor = reader.GetSymbolDescriptor();
          footnote.SymbolCode = symbolDescriptor.CharCode;
          footnote.SymbolFontName = reader.StyleSheet.FontNamesList[(int) symbolDescriptor.FontCode];
        }
        this.ReadCharacterFormat(reader, footnote.MarkerCharacterFormat);
        this.CheckTrackChanges((ParagraphItem) footnote, reader);
        this.ReadParagraphFormat(reader, (IWParagraph) this.CurrentParagraph);
        this.DocumentEx.ParagraphFormatChange(this.CurrentParagraph.ParagraphFormat);
      }
    }
  }

  internal new bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  private bool IsMultipleFootNoteEndNoteMarker(
    ref string footNoteMarker,
    WordReader wReader,
    WFootnote footnote,
    bool isCustomFootnoteSplittedText)
  {
    bool flag = wReader.CustomFnSplittedTextLength < wReader.TextChunk.Length;
    string str1 = !isCustomFootnoteSplittedText || !flag ? wReader.TextChunk : wReader.TextChunk.Substring(wReader.CustomFnSplittedTextLength);
    if (footnote == null || footnote.TextBody.Paragraphs.Count <= 0 || this.StartsWithExt(footnote.TextBody.Paragraphs[0].Text, str1))
      return false;
    int index = 0;
    footNoteMarker = str1[0].ToString();
    while (++index < str1.Length && this.StartsWithExt(footnote.TextBody.Paragraphs[0].Text, footNoteMarker + str1[index].ToString()))
      footNoteMarker += str1[index].ToString();
    string str2 = str1.Replace(footNoteMarker, string.Empty);
    WParagraph wparagraph = (WParagraph) null;
    if (wReader.IsFootnote && this.m_ftnAdapter.m_currFootEndnoteIndex < this.m_ftnAdapter.m_footEndNotes.Count)
      wparagraph = this.m_ftnAdapter.m_footEndNotes[this.m_ftnAdapter.m_currFootEndnoteIndex].TextBody.Paragraphs[0];
    else if (wReader.IsEndnote && this.m_endNoteAdapter.m_currFootEndnoteIndex < this.m_endNoteAdapter.m_footEndNotes.Count)
      wparagraph = this.m_endNoteAdapter.m_footEndNotes[this.m_endNoteAdapter.m_currFootEndnoteIndex].TextBody.Paragraphs[0];
    if (wparagraph != null && this.StartsWithExt(wparagraph.Text, str2))
    {
      wReader.TextChunk = str2;
      return true;
    }
    footNoteMarker = !isCustomFootnoteSplittedText || !flag ? wReader.TextChunk : footNoteMarker;
    return false;
  }

  protected override void ReadDocumentEnd(WordReaderBase reader)
  {
  }

  protected override void ReadTextBox(WordReaderBase reader, FileShapeAddress fspa)
  {
    if (this.m_txbxAdapter == null)
      return;
    bool skipPositionOrigins = reader.TablesData.Fib.NFibNew > (ushort) 193;
    WTextBox wtextBox = this.m_txbxAdapter.ReadTextBoxShape(fspa, skipPositionOrigins);
    if (wtextBox == null)
      return;
    CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, wtextBox.CharacterFormat);
    this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 13);
    this.CurrentParagraph.Items.Add((IEntity) wtextBox);
    this.CheckTrackChanges((ParagraphItem) wtextBox, reader);
  }

  protected override void CheckTextEmbed(Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase shape, WPicture picture)
  {
    if (this.m_txbxAdapter == null)
      return;
    WTextBox autoShapeTextBox = this.m_txbxAdapter.GetAutoShapeTextBox(shape.ShapeProps.Spid);
    if (autoShapeTextBox == null)
      return;
    picture.EmbedBody = autoShapeTextBox.TextBoxBody;
  }

  protected override void ReadAutoShapeTextBox(int shapeId, ShapeObject shapeObj)
  {
    if (this.m_txbxAdapter == null)
      return;
    WTextBox autoShapeTextBox = this.m_txbxAdapter.GetAutoShapeTextBox(shapeId);
    if (autoShapeTextBox == null)
      return;
    shapeObj.IsHeaderAutoShape = false;
    shapeObj.AutoShapeTextCollection.Add((IWTextBox) autoShapeTextBox);
  }

  protected override void ProcessCommText(WordReaderBase reader, WParagraph para)
  {
    if (!(reader is WordReader) || this.m_annAdapter == null || para == null || para.Items.Count == 0 || this.m_annAdapter.Comments == null || this.m_annAdapter.Comments.Count == 0)
      return;
    WComment currentComment = this.m_annAdapter.CurrentComment;
    if (currentComment == null)
      return;
    if (reader.StartTextPos >= currentComment.Format.StartTextPos && reader.EndTextPos <= currentComment.Format.Position)
    {
      currentComment.CommentedItems.InnerList.Add((object) para.LastItem);
    }
    else
    {
      if (!(para.LastItem is WTextRange) || currentComment.Format.StartTextPos > reader.EndTextPos)
        return;
      if (reader.StartTextPos < currentComment.Format.StartTextPos && reader.EndTextPos <= currentComment.Format.Position)
      {
        this.SplitCommText(para, reader.StartTextPos, currentComment.Format.StartTextPos);
        currentComment.CommentedItems.InnerList.Add((object) para.LastItem);
      }
      else if (reader.StartTextPos > currentComment.Format.StartTextPos && reader.EndTextPos > currentComment.Format.Position && reader.StartTextPos < currentComment.Format.Position)
      {
        this.SplitCommText(para, reader.StartTextPos, currentComment.Format.Position);
        currentComment.CommentedItems.InnerList.Add((object) (para.LastItem.PreviousSibling as ParagraphItem));
      }
      else
      {
        if (reader.StartTextPos >= currentComment.Format.StartTextPos || reader.EndTextPos <= currentComment.Format.Position)
          return;
        this.SplitCommText(para, reader.StartTextPos, currentComment.Format.StartTextPos);
        this.SplitCommText(para, reader.StartTextPos, currentComment.Format.Position);
        currentComment.CommentedItems.InnerList.Add((object) (para.LastItem.PreviousSibling as ParagraphItem));
      }
    }
  }

  private void SplitCommText(WParagraph para, int startTextPos, int splitPos)
  {
    if (splitPos <= startTextPos)
      return;
    WTextRange lastItem = para.LastItem as WTextRange;
    string text1 = lastItem.Text;
    int num = splitPos - startTextPos;
    if (num > text1.Length)
      return;
    lastItem.Text = text1.Substring(0, num);
    string text2 = text1.Substring(num, text1.Length - num);
    para.AppendText(text2).ApplyCharacterFormat(lastItem.CharacterFormat);
  }

  private void UpdateCommentMarks(WComment comment)
  {
    int count = comment.CommentedItems.Count;
    if (comment.CommentedItems.Count == 0)
      return;
    WCommentMark wcommentMark1 = new WCommentMark(this.DocumentEx, comment.Format.TagBkmk);
    WCommentMark wcommentMark2 = new WCommentMark(this.DocumentEx, comment.Format.TagBkmk, CommentMarkType.CommentEnd);
    wcommentMark1.Comment = comment;
    wcommentMark2.Comment = comment;
    comment.CommentRangeStart = wcommentMark1;
    comment.CommentRangeEnd = wcommentMark2;
    ParagraphItem commentedItem1 = comment.CommentedItems[0];
    if (commentedItem1.PreviousSibling == null)
    {
      commentedItem1.OwnerParagraph.Items.Insert(0, (IEntity) wcommentMark1);
    }
    else
    {
      int inOwnerCollection = commentedItem1.GetIndexInOwnerCollection();
      commentedItem1.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wcommentMark1);
    }
    ParagraphItem commentedItem2 = comment.CommentedItems[count - 1];
    if (commentedItem2.NextSibling == null)
    {
      commentedItem2.OwnerParagraph.Items.Add((IEntity) wcommentMark2);
    }
    else
    {
      int inOwnerCollection = commentedItem2.GetIndexInOwnerCollection();
      commentedItem1.OwnerParagraph.Items.Insert(inOwnerCollection + 1, (IEntity) wcommentMark2);
    }
  }

  private void ParseListPicture()
  {
    foreach (ListStyle listStyle in (CollectionImpl) this.DocumentEx.ListStyles)
    {
      int index = 0;
      for (int count = listStyle.Levels.Count; index < count; ++index)
      {
        WListLevel level = listStyle.Levels[index];
        int picIndex = level.PicIndex;
        if (picIndex >= 0 && picIndex != int.MaxValue)
        {
          if (picIndex <= this.ListPictures.Count - 1)
          {
            WPicture listPicture = this.ListPictures[picIndex];
            level.PicBullet = listPicture;
          }
          else
          {
            WPicture wpicture = new WPicture((IWordDocument) this.DocumentEx);
            Bitmap bitmap = new Bitmap(3, 3);
            wpicture.LoadImage((Image) bitmap);
            level.PicBullet = wpicture;
            level.IsEmptyPicture = true;
          }
        }
      }
    }
  }

  private void CheckWatermark(WSection section)
  {
    if (section.HeadersFooters.OddHeader.WriteWatermark || section.HeadersFooters.FirstPageHeader.WriteWatermark || section.HeadersFooters.EvenHeader.WriteWatermark)
      return;
    section.Document.InsertWatermark(WatermarkType.NoWatermark);
  }

  internal abstract class SubDocumentAdapter : DocReaderAdapterBase
  {
    internal void ReadSubDocBody(WordReader reader, WordDocument documentEx)
    {
      this.Init(documentEx);
      this.Read(reader);
      reader.UnfreezeStreamPos();
    }

    internal abstract void Read(WordReader reader);
  }

  internal class HeaderFooterAdapter : DocReaderAdapter.SubDocumentAdapter
  {
    private int m_currentHFType;
    private bool m_itemEnd;
    private DocReaderAdapter.HFTextboxAdapter m_hfTxbxAdapter;

    internal DocReaderAdapter.HFTextboxAdapter TextBoxAdapter => this.m_hfTxbxAdapter;

    internal override void Read(WordReader reader)
    {
      WordHeaderFooterReader subdocumentReader = reader.GetSubdocumentReader(WordSubdocument.HeaderFooter) as WordHeaderFooterReader;
      subdocumentReader.Bookmarks = reader.Bookmarks;
      if (reader.TablesData.HasSubdocument(WordSubdocument.HeaderTextBox))
        this.ReadSubdocument(reader, WordSubdocument.HeaderTextBox);
      int count = this.DocumentEx.Sections.Count;
      this.m_finalize = false;
      subdocumentReader.MoveToSection(1);
      for (int index = 0; index < 6; ++index)
      {
        WTextBody textBody = new WTextBody(this.DocumentEx, (Entity) null);
        this.ReadTextBody((WordReaderBase) subdocumentReader, textBody);
        this.RemoveLastParagraph(textBody);
        if (textBody.ChildEntities.Count > 0)
          this.SetSeparatorBody(textBody, index);
      }
      for (int index = 0; index < count; ++index)
      {
        subdocumentReader.MoveToSection(index + 1);
        WSection section = this.DocumentEx.Sections[index];
        this.m_itemEnd = false;
        subdocumentReader.MoveToItem(6);
        subdocumentReader.HeaderType = HeaderType.EvenHeader;
        this.m_currentHFType = 0;
        while (!this.m_itemEnd)
        {
          WTextBody headersFooter = (WTextBody) section.HeadersFooters[this.m_currentHFType];
          this.ReadTextBody((WordReaderBase) subdocumentReader, headersFooter);
          if (subdocumentReader.HeaderType == HeaderType.EvenFooter || subdocumentReader.HeaderType == HeaderType.FirstPageFooter || subdocumentReader.HeaderType == HeaderType.OddFooter)
            this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 20);
          else if (subdocumentReader.HeaderType != HeaderType.InvalidValue)
            this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 22);
          this.m_nestedTextBodies.Clear();
        }
        this.m_itemEnd = false;
        this.m_currentHFType = 0;
        this.RemoveHFLastParagraphs((IWSection) section);
      }
    }

    private void SetSeparatorBody(WTextBody textBody, int index)
    {
      switch (index)
      {
        case 0:
          this.DocumentEx.Footnotes.Separator = textBody;
          break;
        case 1:
          this.DocumentEx.Footnotes.ContinuationSeparator = textBody;
          break;
        case 2:
          this.DocumentEx.Footnotes.ContinuationNotice = textBody;
          break;
        case 3:
          this.DocumentEx.Endnotes.Separator = textBody;
          break;
        case 4:
          this.DocumentEx.Endnotes.ContinuationSeparator = textBody;
          break;
        case 5:
          this.DocumentEx.Endnotes.ContinuationNotice = textBody;
          break;
      }
    }

    internal override void Close()
    {
      base.Close();
      if (this.m_hfTxbxAdapter == null)
        return;
      this.m_hfTxbxAdapter.Close();
      this.m_hfTxbxAdapter = (DocReaderAdapter.HFTextboxAdapter) null;
    }

    protected override bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
    {
      this.m_currentHFType += chunkType != WordChunkType.EndOfSubdocText || this.m_currentHFType >= 5 ? 0 : 1;
      this.m_itemEnd = chunkType == WordChunkType.DocumentEnd;
      return chunkType == WordChunkType.DocumentEnd || chunkType == WordChunkType.EndOfSubdocText;
    }

    protected override void ReadTextBox(WordReaderBase reader, FileShapeAddress fspa)
    {
      if (this.m_hfTxbxAdapter == null)
        return;
      bool skipPositionOrigins = reader.TablesData.Fib.NFibNew > (ushort) 193;
      WTextBox wtextBox = this.m_hfTxbxAdapter.ReadTextBoxShape(fspa, skipPositionOrigins);
      if (wtextBox == null)
        return;
      CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, wtextBox.CharacterFormat);
      this.CurrentParagraph.Items.Add((IEntity) wtextBox);
      this.CheckTrackChanges((ParagraphItem) wtextBox, reader);
    }

    protected override bool ReadWatermark(WordReaderBase reader)
    {
      return reader.ReadWatermark(this.DocumentEx, this.m_textBody);
    }

    protected override void ReadAutoShapeTextBox(int shapeId, ShapeObject shapeObj)
    {
      if (this.m_hfTxbxAdapter == null)
        return;
      WTextBox autoShapeTextBox = this.m_hfTxbxAdapter.GetAutoShapeTextBox(shapeId);
      if (autoShapeTextBox == null)
        return;
      shapeObj.IsHeaderAutoShape = true;
      autoShapeTextBox.SetOwner((OwnerHolder) shapeObj);
      shapeObj.AutoShapeTextCollection.Add((IWTextBox) autoShapeTextBox);
    }

    protected override void CheckTextEmbed(Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase shape, WPicture picture)
    {
      if (this.m_hfTxbxAdapter == null)
        return;
      WTextBox autoShapeTextBox = this.m_hfTxbxAdapter.GetAutoShapeTextBox(shape.ShapeProps.Spid);
      if (autoShapeTextBox == null)
        return;
      picture.EmbedBody = autoShapeTextBox.TextBoxBody;
    }

    private void ReadSubdocument(WordReader reader, WordSubdocument wsType)
    {
      DocReaderAdapter.SubDocumentAdapter subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) null;
      if (wsType == WordSubdocument.HeaderTextBox)
        subDocumentAdapter = (DocReaderAdapter.SubDocumentAdapter) (this.m_hfTxbxAdapter = new DocReaderAdapter.HFTextboxAdapter());
      subDocumentAdapter?.ReadSubDocBody(reader, this.DocumentEx);
    }

    private void RemoveHFLastParagraphs(IWSection section)
    {
      for (int index = 0; index < 6; ++index)
      {
        BodyItemCollection items = section.HeadersFooters[index].Items;
        if (items.LastItem is IWParagraph lastItem && lastItem.Items.Count == 0)
          items.Remove((IEntity) lastItem);
      }
    }

    private void RemoveLastParagraph(WTextBody textBody)
    {
      if (!(textBody.Items.LastItem is IWParagraph lastItem) || lastItem.Items.Count != 0)
        return;
      textBody.Items.Remove((IEntity) lastItem);
    }
  }

  internal class AnnotationAdapter : DocReaderAdapter.SubDocumentAdapter
  {
    private List<WComment> m_comments = new List<WComment>();
    private WComment m_currComment;
    private int m_currCommentIndex;

    internal WComment CurrentComment
    {
      get
      {
        return this.m_currCommentIndex < this.m_comments.Count ? this.m_comments[this.m_currCommentIndex] : (WComment) null;
      }
    }

    internal List<WComment> Comments => this.m_comments;

    internal override void Read(WordReader reader)
    {
      WordAnnotationReader subdocumentReader = reader.GetSubdocumentReader(WordSubdocument.Annotation) as WordAnnotationReader;
      subdocumentReader.Bookmarks = reader.Bookmarks;
      do
      {
        this.AddComment(subdocumentReader);
        this.ReadTextBody((WordReaderBase) subdocumentReader, this.m_currComment.TextBody);
        ++this.m_currCommentIndex;
      }
      while (subdocumentReader.ChunkType != WordChunkType.DocumentEnd);
      this.m_currCommentIndex = 0;
    }

    internal WComment GetNextComment()
    {
      return this.m_currCommentIndex < this.m_comments.Count ? this.m_comments[this.m_currCommentIndex++] : (WComment) null;
    }

    private void AddComment(WordAnnotationReader reader)
    {
      this.m_currComment = new WComment((IWordDocument) this.DocumentEx);
      this.ReadCommentFormat(reader, this.m_currComment.Format);
      this.m_comments.Add(this.m_currComment);
    }

    protected override bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
    {
      return (reader as WordAnnotationReader).ItemNumber != this.m_currCommentIndex || chunkType == WordChunkType.DocumentEnd;
    }

    internal override void Close()
    {
      base.Close();
      if (this.m_comments != null)
      {
        this.m_comments.Clear();
        this.m_comments = (List<WComment>) null;
      }
      this.m_currComment = (WComment) null;
    }

    private void ReadCommentFormat(WordAnnotationReader reader, WCommentFormat format)
    {
      AnnotationDescriptor descriptor = reader.Descriptor;
      if (descriptor == null)
        return;
      format.UserInitials = descriptor.UserInitials;
      format.User = reader.User;
      format.BookmarkStartOffset = reader.BookmarkStartOffset;
      format.BookmarkEndOffset = reader.BookmarkEndOffset;
      format.Position = reader.Position;
      format.TagBkmk = descriptor.TagBkmk.ToString();
    }
  }

  internal class FootnoteAdapter : DocReaderAdapter.SubDocumentAdapter
  {
    internal List<WFootnote> m_footEndNotes = new List<WFootnote>();
    protected WFootnote m_currFootEndNote;
    internal int m_currFootEndnoteIndex;
    protected int m_footEndNotesCount;

    internal override void Read(WordReader reader)
    {
      WordSubdocumentReader reader1 = this.Init(reader);
      reader1.Bookmarks = reader.Bookmarks;
      for (int index = 0; index < this.m_footEndNotesCount; ++index)
      {
        this.AddFootEndNote((IWordSubdocumentReader) reader1);
        reader1.MoveToItem(this.m_currFootEndnoteIndex);
        this.ReadTextBody((WordReaderBase) reader1, this.m_currFootEndNote.TextBody);
        ++this.m_currFootEndnoteIndex;
      }
      this.m_currFootEndnoteIndex = 0;
    }

    internal WFootnote GetNextFootEndNote()
    {
      return this.m_currFootEndnoteIndex < this.m_footEndNotes.Count ? this.m_footEndNotes[this.m_currFootEndnoteIndex++] : (WFootnote) null;
    }

    protected virtual WordSubdocumentReader Init(WordReader reader)
    {
      this.m_footEndNotesCount = reader.TablesData.Footnotes.Count - 1;
      return (WordSubdocumentReader) (reader.GetSubdocumentReader(WordSubdocument.Footnote) as WordFootnoteReader);
    }

    protected virtual void AddFootEndNote(IWordSubdocumentReader reader)
    {
      this.m_currFootEndNote = new WFootnote((IWordDocument) this.DocumentEx);
      this.m_currFootEndNote.FootnoteType = FootnoteType.Footnote;
      this.m_footEndNotes.Add(this.m_currFootEndNote);
    }

    internal override void Close()
    {
      base.Close();
      if (this.m_footEndNotes != null)
      {
        this.m_footEndNotes.Clear();
        this.m_footEndNotes = (List<WFootnote>) null;
      }
      this.m_currFootEndNote = (WFootnote) null;
    }

    protected override bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
    {
      return (reader as WordFootnoteReader).ItemNumber != this.m_currFootEndnoteIndex || chunkType == WordChunkType.DocumentEnd;
    }

    private void ReadFootnoteFormat(IWordSubdocumentReader reader)
    {
    }
  }

  internal class EndnoteAdapter : DocReaderAdapter.FootnoteAdapter
  {
    protected override WordSubdocumentReader Init(WordReader reader)
    {
      this.m_footEndNotesCount = reader.TablesData.Endnotes.Count - 1;
      return (WordSubdocumentReader) (reader.GetSubdocumentReader(WordSubdocument.Endnote) as WordEndnoteReader);
    }

    protected override void AddFootEndNote(IWordSubdocumentReader reader)
    {
      this.m_currFootEndNote = new WFootnote((IWordDocument) this.DocumentEx);
      this.m_currFootEndNote.FootnoteType = FootnoteType.Endnote;
      this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 17);
      this.m_footEndNotes.Add(this.m_currFootEndNote);
    }
  }

  internal class TextboxAdapter : DocReaderAdapter.SubDocumentAdapter
  {
    protected WTextBox m_currTextBox;
    protected int m_txbxCount;
    protected WordSubdocument m_textBoxType;
    protected int m_currentTxbxIndex;
    protected ShapeObjectTextCollection m_textBoxCollection = new ShapeObjectTextCollection();

    internal override void Read(WordReader reader)
    {
      WordSubdocumentReader reader1 = this.Init(reader);
      reader1.Bookmarks = reader.Bookmarks;
      this.m_finalize = false;
      for (int index = 0; index < this.m_txbxCount - 1; ++index)
      {
        if (this.CreateAndAddTextBox((WordReaderBase) reader))
        {
          reader1.MoveToItem(this.m_currentTxbxIndex);
          this.ReadTextBody((WordReaderBase) reader1, this.m_currTextBox.TextBoxBody);
          this.m_nestedTextBodies.Clear();
        }
        ++this.m_currentTxbxIndex;
      }
    }

    internal override void Close()
    {
      base.Close();
      this.m_currTextBox = (WTextBox) null;
      if (this.m_textBoxCollection == null)
        return;
      this.m_textBoxCollection.Close();
      this.m_textBoxCollection = (ShapeObjectTextCollection) null;
    }

    protected override bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
    {
      return chunkType == WordChunkType.DocumentEnd || this.m_currentTxbxIndex != (reader as WordTextBoxReader).ItemNumber;
    }

    protected virtual WordSubdocumentReader Init(WordReader reader)
    {
      if (reader.TablesData.ArtObj.MainDocTxBxs != null)
        this.m_txbxCount = reader.TablesData.ArtObj.MainDocTxBxs.Count;
      this.m_textBoxType = WordSubdocument.TextBox;
      return (WordSubdocumentReader) (reader.GetSubdocumentReader(WordSubdocument.TextBox) as WordTextBoxReader);
    }

    private bool CreateAndAddTextBox(WordReaderBase baseReader)
    {
      bool andAddTextBox = false;
      int shapeObjectId = baseReader.TablesData.ArtObj.GetShapeObjectId(this.m_textBoxType, this.m_currentTxbxIndex);
      if (shapeObjectId != 0)
      {
        this.m_currTextBox = new WTextBox((IWordDocument) this.DocumentEx);
        this.m_textBoxCollection.AddTextBox(shapeObjectId, this.m_currTextBox);
        andAddTextBox = true;
      }
      return andAddTextBox;
    }

    internal WTextBox ReadTextBoxShape(FileShapeAddress fspa, bool skipPositionOrigins)
    {
      WTextBox textBox = this.m_textBoxCollection.GetTextBox(fspa.Spid);
      if (textBox == null)
        return (WTextBox) null;
      MsofbtSpContainer txbxContainer = (MsofbtSpContainer) null;
      if (this.DocumentEx.Escher.Containers.ContainsKey(fspa.Spid))
        txbxContainer = this.DocumentEx.Escher.Containers[fspa.Spid] as MsofbtSpContainer;
      TextBoxPropertiesConverter.Export(txbxContainer, fspa, textBox.TextBoxFormat, skipPositionOrigins);
      textBox.Visible = txbxContainer.ShapeOptions.Visible;
      return textBox;
    }

    internal WTextBox GetAutoShapeTextBox(int shapeId)
    {
      return this.m_textBoxCollection.GetTextBox(shapeId);
    }
  }

  internal class HFTextboxAdapter : DocReaderAdapter.TextboxAdapter
  {
    protected override WordSubdocumentReader Init(WordReader reader)
    {
      if (reader.TablesData.ArtObj.HfDocTxBxs != null)
        this.m_txbxCount = reader.TablesData.ArtObj.HfDocTxBxs.Count;
      this.m_textBoxType = WordSubdocument.HeaderTextBox;
      return (WordSubdocumentReader) (reader.GetSubdocumentReader(WordSubdocument.HeaderTextBox) as WordHFTextBoxReader);
    }

    protected override bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
    {
      return chunkType == WordChunkType.DocumentEnd || this.m_currentTxbxIndex != (reader as WordHFTextBoxReader).ItemNumber;
    }
  }
}
