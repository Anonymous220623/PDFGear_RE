// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextConverter
{
  private StreamWriter m_writer;
  private string m_text = "";
  private int m_curSectionIndex;
  private bool m_bGetString;
  private WordDocument m_document;
  private WParagraph m_lastPara;
  private bool isFieldEnd;
  private bool isFieldSeparator;
  private WFieldMark seperator;

  public string GetText(WordDocument document)
  {
    this.m_document = document;
    this.m_bGetString = true;
    this.Write();
    this.m_bGetString = false;
    return this.m_text;
  }

  public void Write(StreamWriter writer, IWordDocument document)
  {
    this.m_writer = writer;
    this.m_document = document as WordDocument;
    this.Write();
  }

  public void Read(StreamReader reader, IWordDocument document)
  {
    this.Read(reader.ReadToEnd(), document);
  }

  internal void Read(string text, IWordDocument document)
  {
    text = text.Replace(ControlChar.CrLf, ControlChar.LineFeed);
    text = text.Replace(ControlChar.CarriegeReturn, ControlChar.LineFeed);
    string[] textLines = text.Split(ControlChar.LineFeed.ToCharArray());
    if (document.LastParagraph == null)
    {
      if (document.LastSection == null)
        document.EnsureMinimal();
      else
        document.LastSection.Body.AddParagraph();
    }
    int index = 0;
    for (int length = textLines.Length; index < length; ++index)
    {
      string text1 = textLines[index].Trim(ControlChar.CarriegeReturn.ToCharArray());
      if (index > 0 && (index + 1 < length || !string.IsNullOrEmpty(text1)))
        document.LastSection.Body.AddParagraph();
      if (!string.IsNullOrEmpty(text1))
        document.LastParagraph.AppendText(text1);
    }
    this.InitBuiltinDocumentProperties(text, textLines, document);
  }

  private void InitBuiltinDocumentProperties(string text, string[] textLines, IWordDocument doc)
  {
    int length = textLines.Length;
    int num = 0;
    foreach (string textLine in textLines)
    {
      if (textLine == ControlChar.CarriegeReturn || textLine == string.Empty)
      {
        --length;
      }
      else
      {
        foreach (string str in textLine.Split(" ".ToCharArray()))
        {
          if (str != string.Empty)
            ++num;
        }
      }
    }
    text = text.Replace(" ", string.Empty);
    text = text.Replace(ControlChar.LineFeed, string.Empty);
    text = text.Replace(ControlChar.CarriegeReturn, string.Empty);
    doc.BuiltinDocumentProperties.ParagraphCount = length;
    doc.BuiltinDocumentProperties.WordCount = num;
    doc.BuiltinDocumentProperties.CharCount = text.Length;
  }

  protected void WriteHFBody(WordDocument document)
  {
  }

  protected void WriteBody(ITextBody body)
  {
    int num = body.ChildEntities.Count - 1;
    for (int index = 0; index <= num; ++index)
    {
      TextBodyItem childEntity = body.ChildEntities[index] as TextBodyItem;
      switch (childEntity.EntityType)
      {
        case EntityType.Paragraph:
          bool lastPara = childEntity as WParagraph == this.m_lastPara;
          this.WriteParagraph((childEntity as IWParagraph).ChildEntities as ParagraphItemCollection, lastPara);
          break;
        case EntityType.BlockContentControl:
          this.WriteBody((ITextBody) (childEntity as IBlockContentControl).TextBody);
          break;
        case EntityType.Table:
          this.WriteTable(childEntity as IWTable);
          break;
      }
    }
  }

  protected void WriteParagraph(ParagraphItemCollection paragraphItems, bool lastPara)
  {
    IWParagraph paragraph = !(paragraphItems.Owner is InlineContentControl) ? paragraphItems.Owner as IWParagraph : (IWParagraph) (paragraphItems.Owner as InlineContentControl).GetOwnerParagraphValue();
    if (!paragraph.ListFormat.IsEmptyList && paragraph.ChildEntities.Count != 0 && !(paragraph as WParagraph).SectionEndMark)
      this.WriteList(paragraph);
    int index = 0;
    for (int count = paragraphItems.Count; index < count; ++index)
    {
      IParagraphItem paragraphItem = (IParagraphItem) paragraphItems[index];
      if (this.isFieldEnd && paragraphItem is WFieldMark && (paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd)
        this.isFieldEnd = false;
      if (!this.isFieldEnd)
      {
        switch (paragraphItem.EntityType)
        {
          case EntityType.InlineContentControl:
            this.WriteParagraph((paragraphItem as InlineContentControl).ParagraphItems, true);
            continue;
          case EntityType.TextRange:
            if (!this.isFieldSeparator)
            {
              this.WriteText((paragraphItem as IWTextRange).Text);
              continue;
            }
            continue;
          case EntityType.Field:
          case EntityType.MergeField:
          case EntityType.SeqField:
          case EntityType.EmbededField:
          case EntityType.ControlField:
            WField wfield = paragraphItem as WField;
            switch (wfield.FieldType)
            {
              case FieldType.FieldDocVariable:
                this.WriteText(wfield.Document.Variables[wfield.FieldValue]);
                if (wfield.FieldEnd != null && wfield.FieldEnd.Owner is WParagraph)
                {
                  this.isFieldEnd = true;
                  break;
                }
                break;
              case FieldType.FieldUnknown:
                if (wfield.FieldEnd != null && wfield.FieldEnd.Owner is WParagraph)
                {
                  this.isFieldEnd = true;
                  break;
                }
                break;
            }
            if (wfield.FieldType == FieldType.FieldIf && this.seperator == null)
              this.seperator = wfield.FieldSeparator;
            if (wfield.FieldEnd != null)
            {
              this.isFieldSeparator = true;
              continue;
            }
            continue;
          case EntityType.FieldMark:
            if (paragraphItem == this.seperator)
            {
              this.seperator = (WFieldMark) null;
              this.isFieldSeparator = false;
              continue;
            }
            if (this.seperator == null)
            {
              this.isFieldSeparator = false;
              continue;
            }
            continue;
          case EntityType.TextFormField:
          case EntityType.DropDownFormField:
          case EntityType.CheckBox:
            if (this.seperator == null)
              this.seperator = (paragraphItem as WFormField).FieldSeparator;
            if ((paragraphItem as WFormField).FieldEnd != null)
            {
              this.isFieldSeparator = true;
              continue;
            }
            continue;
          case EntityType.TextBox:
            this.WriteBody((ITextBody) (paragraphItem as WTextBox).TextBoxBody);
            continue;
          case EntityType.Break:
            if ((paragraphItem as Break).BreakType == BreakType.LineBreak)
            {
              this.WriteNewLine();
              continue;
            }
            continue;
          case EntityType.TOC:
            if ((paragraphItem as TableOfContent).TOCField.FieldEnd != null)
            {
              this.isFieldSeparator = true;
              continue;
            }
            continue;
          case EntityType.XmlParaItem:
            if (paragraphItem is XmlParagraphItem xmlParagraphItem && xmlParagraphItem.MathParaItemsCollection != null && xmlParagraphItem.MathParaItemsCollection.Count > 0)
            {
              IEnumerator enumerator = xmlParagraphItem.MathParaItemsCollection.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  ParagraphItem current = (ParagraphItem) enumerator.Current;
                  if (current is WTextRange)
                    this.WriteText((current as IWTextRange).Text);
                }
                continue;
              }
              finally
              {
                if (enumerator is IDisposable disposable)
                  disposable.Dispose();
              }
            }
            else
              continue;
          case EntityType.OleObject:
            if ((paragraphItem as WOleObject).Field.FieldEnd != null)
            {
              this.isFieldSeparator = true;
              continue;
            }
            continue;
          case EntityType.AutoShape:
            this.WriteBody((ITextBody) (paragraphItem as Shape).TextBody);
            continue;
          default:
            continue;
        }
      }
    }
    if (lastPara)
      return;
    this.WriteNewLine();
  }

  protected void WriteTable(IWTable table)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (ITextBody cell in (CollectionImpl) row.Cells)
        this.WriteBody(cell);
    }
  }

  protected void WriteSectionEnd(IWSection section, bool lastSection)
  {
    if (this.m_bGetString)
      this.m_text += ControlChar.CrLf;
    else if (!lastSection)
      this.m_writer.WriteLine("");
    ++this.m_curSectionIndex;
  }

  protected void WriteText(string text)
  {
    if (this.m_bGetString)
      this.m_text += text;
    else
      this.m_writer.Write(text);
  }

  protected void WriteList(IWParagraph paragraph)
  {
    bool isPicBullet = false;
    string listText = (paragraph as WParagraph).GetListText(true, ref isPicBullet);
    if (!isPicBullet)
      (paragraph as WParagraph).m_liststring = listText;
    if (this.m_bGetString)
      this.m_text += listText;
    else
      this.m_writer.Write(listText);
  }

  protected void WriteNewLine()
  {
    if (this.m_bGetString)
      this.m_text += ControlChar.CrLf;
    else
      this.m_writer.WriteLine("");
  }

  private void UpdateLastPara()
  {
    if (this.m_document.LastSection.HeadersFooters.Footer.ChildEntities.Count > 0)
      this.m_lastPara = this.m_document.LastSection.HeadersFooters.Footer.LastParagraph as WParagraph;
    else
      this.m_lastPara = this.m_document.LastParagraph;
  }

  private void Write()
  {
    int num = this.m_document.Sections.Count - 1;
    this.UpdateLastPara();
    if (this.m_document.WriteWarning)
    {
      if (this.m_bGetString)
        this.m_text += "Created with a trial version of Syncfusion Essential DocIO.\r\n";
      else
        this.m_writer.WriteLine("Created with a trial version of Syncfusion Essential DocIO.");
    }
    for (int index = 0; index <= num; ++index)
    {
      WSection section = this.m_document.Sections[index];
      bool lastSection = index == num;
      this.WriteBody(this.GetHeader(section, this.m_curSectionIndex));
      this.WriteBody((ITextBody) section.Body);
      this.WriteSectionEnd((IWSection) section, lastSection);
      this.WriteBody(this.GetFooter(section, this.m_curSectionIndex - 1));
    }
    this.m_document.ClearLists();
  }

  private ITextBody GetFooter(WSection section, int sectionIndex)
  {
    HeaderFooterType hfType = section.PageSetup.DifferentFirstPage ? HeaderFooterType.FirstPageFooter : HeaderFooterType.OddFooter;
    if (section.HeadersFooters[hfType].LinkToPrevious && sectionIndex > 0)
    {
      int index = sectionIndex - 1;
      while (index >= 0)
      {
        WSection section1 = this.m_document.Sections[index];
        HeaderFooter headersFooter = section1.HeadersFooters[hfType];
        if (hfType == headersFooter.Type && headersFooter.LinkToPrevious)
          --index;
        else if (hfType == headersFooter.Type && !headersFooter.LinkToPrevious)
        {
          section.HeadersFooters[hfType] = section1.HeadersFooters[hfType];
          break;
        }
      }
    }
    return section.PageSetup.DifferentFirstPage ? (ITextBody) section.HeadersFooters.FirstPageFooter : (ITextBody) section.HeadersFooters.Footer;
  }

  private ITextBody GetHeader(WSection section, int sectionIndex)
  {
    HeaderFooterType hfType = section.PageSetup.DifferentFirstPage ? HeaderFooterType.FirstPageHeader : HeaderFooterType.OddHeader;
    if (section.HeadersFooters[hfType].LinkToPrevious && sectionIndex > 0)
    {
      int index = sectionIndex - 1;
      while (index >= 0)
      {
        WSection section1 = this.m_document.Sections[index];
        HeaderFooter headersFooter = section1.HeadersFooters[hfType];
        if (hfType == headersFooter.Type && headersFooter.LinkToPrevious)
          --index;
        else if (hfType == headersFooter.Type && !headersFooter.LinkToPrevious)
        {
          section.HeadersFooters[hfType] = section1.HeadersFooters[hfType];
          break;
        }
      }
    }
    return section.PageSetup.DifferentFirstPage ? (ITextBody) section.HeadersFooters.FirstPageHeader : (ITextBody) section.HeadersFooters.Header;
  }
}
