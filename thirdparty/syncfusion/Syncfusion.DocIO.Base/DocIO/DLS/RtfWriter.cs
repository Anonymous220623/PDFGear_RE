// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RtfWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.DocIO.ReaderWriter.Escher;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class RtfWriter : RtfNavigator
{
  private const char DEF_FOOTNOTE_SYMBOL = '\u0002';
  private const string DEF_FONT_NAME = "Times New Roman";
  private const int MM_ANISOTROPIC = 8;
  private readonly string c_lineBreak = '\v'.ToString();
  private readonly string c_transfer = ' '.ToString();
  private readonly string c_symbol92 = '\\'.ToString();
  private readonly string c_symbol31 = '\u001F'.ToString();
  private readonly string c_symbol30 = '\u001E'.ToString();
  private readonly string c_symbol61553 = '\uF071'.ToString();
  private readonly string c_symbol61549 = '\uF06D'.ToString();
  private readonly string c_symbol123 = '{'.ToString();
  private readonly string c_symbol125 = '}'.ToString();
  private readonly string c_slashSymbol = '\\'.ToString();
  private readonly string c_symbol8226 = '•'.ToString();
  private readonly string c_enDash = '–'.ToString();
  private readonly string c_emDash = '—'.ToString();
  private readonly string c_enSpace = ' '.ToString();
  private readonly string c_emSpace = ' '.ToString();
  private readonly string c_section = '§'.ToString();
  private readonly string c_copyRight = '©'.ToString();
  private readonly string c_registered = '®'.ToString();
  private readonly string c_paraMark = '¶'.ToString();
  private readonly string c_tradeMark = '™'.ToString();
  private readonly string c_singleOpenQuote = '‘'.ToString();
  private readonly string c_singleCloseQuote = '’'.ToString();
  private readonly string c_doubleOpenQuote = '“'.ToString();
  private readonly string c_doubleCloseQuote = '”'.ToString();
  private WordDocument m_doc;
  private Stream m_stream;
  private Encoding m_encoding;
  private byte[] m_defStyleBytes;
  private byte[] m_listTableBytes;
  private byte[] m_listOverrideTableBytes;
  private byte[] m_styleBytes;
  private byte[] m_colorBytes;
  private byte[] m_fontBytes;
  private List<byte[]> m_mainBodyBytesList;
  private int m_fontId;
  private int m_uniqueId = 1;
  private int m_cellEndPos;
  private int m_tableNestedLevel;
  private int m_colorId = 1;
  private Dictionary<string, string> m_styles;
  private Dictionary<string, string> m_stylesNumb;
  private Dictionary<string, Dictionary<int, int>> m_listStart;
  private Dictionary<string, int> m_listsIds;
  private Dictionary<string, string> m_fontEntries;
  private Dictionary<string, string> m_associatedFontEntries;
  private bool m_hasFootnote;
  private bool m_hasEndnote;
  private bool m_isCyrillicText;
  private Dictionary<int, string> m_listOverride;
  private Dictionary<string, string> m_commentIds;
  private Stack<object> m_currentField;
  private Dictionary<Color, int> m_colorTable;
  private bool m_isField;

  private Dictionary<string, string> FontEntries
  {
    get
    {
      if (this.m_fontEntries == null)
        this.m_fontEntries = new Dictionary<string, string>();
      return this.m_fontEntries;
    }
  }

  private Dictionary<string, string> AssociatedFontEntries
  {
    get
    {
      if (this.m_associatedFontEntries == null)
        this.m_associatedFontEntries = new Dictionary<string, string>();
      return this.m_associatedFontEntries;
    }
  }

  private Dictionary<string, int> ListsIds
  {
    get
    {
      if (this.m_listsIds == null)
        this.m_listsIds = new Dictionary<string, int>();
      return this.m_listsIds;
    }
  }

  private Dictionary<int, string> ListOverrideAr
  {
    get
    {
      if (this.m_listOverride == null)
        this.m_listOverride = new Dictionary<int, string>();
      return this.m_listOverride;
    }
  }

  private Dictionary<string, string> Styles
  {
    get
    {
      if (this.m_styles == null)
        this.m_styles = new Dictionary<string, string>();
      return this.m_styles;
    }
  }

  private Dictionary<string, string> StyleNumb
  {
    get
    {
      if (this.m_stylesNumb == null)
        this.m_stylesNumb = new Dictionary<string, string>();
      return this.m_stylesNumb;
    }
  }

  private Dictionary<string, Dictionary<int, int>> ListStart
  {
    get
    {
      if (this.m_listStart == null)
        this.m_listStart = new Dictionary<string, Dictionary<int, int>>();
      return this.m_listStart;
    }
  }

  private Dictionary<string, string> CommentIds
  {
    get
    {
      if (this.m_commentIds == null)
        this.m_commentIds = new Dictionary<string, string>();
      return this.m_commentIds;
    }
  }

  private Stack<object> CurrentField
  {
    get
    {
      if (this.m_currentField == null)
        this.m_currentField = new Stack<object>();
      return this.m_currentField;
    }
  }

  private Dictionary<Color, int> ColorTable
  {
    get
    {
      if (this.m_colorTable == null)
        this.m_colorTable = new Dictionary<Color, int>();
      return this.m_colorTable;
    }
  }

  public RtfWriter()
  {
    this.m_encoding = WordDocument.GetEncoding("ASCII");
    this.m_styleBytes = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}stylesheet");
    this.m_fontBytes = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}fonttbl");
    this.m_colorBytes = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}colortbl;");
  }

  internal void Write(string fileName, IWordDocument document)
  {
    FileStream fileStream = new FileStream(fileName, FileMode.Create);
    this.Write((Stream) fileStream, document);
    fileStream.Dispose();
  }

  internal void Write(Stream stream, IWordDocument document)
  {
    this.m_doc = document as WordDocument;
    this.m_stream = stream;
    this.BuildDefaultStyles();
    this.AppendListStyles();
    this.BuildStyleSheet();
    this.BuildSections();
    this.AppendOverrideList();
    byte[] bytes1 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}rtf1{this.c_slashSymbol}ansi");
    this.m_stream.Write(bytes1, 0, bytes1.Length);
    this.WriteBody();
    byte[] bytes2 = this.m_encoding.GetBytes("}");
    this.m_stream.Write(bytes2, 0, bytes2.Length);
  }

  internal string GetRtfText(IWordDocument document)
  {
    this.m_doc = document as WordDocument;
    this.m_stream = (Stream) new MemoryStream();
    this.Write(this.m_stream, (IWordDocument) this.m_doc);
    this.m_stream.Position = 0L;
    byte[] numArray = new byte[this.m_stream.Length];
    this.m_stream.Read(numArray, 0, numArray.Length);
    this.m_stream.Dispose();
    return this.m_encoding.GetString(numArray, 0, numArray.Length);
  }

  private void WriteBody()
  {
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
    {
      this.m_stream.Write(this.m_fontBytes, 0, this.m_fontBytes.Length);
      this.m_stream.WriteByte((byte) 125);
      this.m_stream.WriteByte((byte) 13);
      this.m_stream.WriteByte((byte) 10);
      this.m_fontBytes = (byte[]) null;
      this.m_stream.Write(this.m_colorBytes, 0, this.m_colorBytes.Length);
      this.m_stream.WriteByte((byte) 125);
      this.m_stream.WriteByte((byte) 13);
      this.m_stream.WriteByte((byte) 10);
      this.m_colorBytes = (byte[]) null;
      this.m_stream.Write(this.m_defStyleBytes, 0, this.m_defStyleBytes.Length);
      this.m_defStyleBytes = (byte[]) null;
      this.m_stream.Write(this.m_styleBytes, 0, this.m_styleBytes.Length);
      this.m_stream.WriteByte((byte) 125);
      this.m_stream.WriteByte((byte) 13);
      this.m_stream.WriteByte((byte) 10);
      this.m_styleBytes = (byte[]) null;
      if (this.m_listTableBytes != null)
      {
        this.m_stream.Write(this.m_listTableBytes, 0, this.m_listTableBytes.Length);
        this.m_listTableBytes = (byte[]) null;
      }
      this.m_stream.Write(this.m_listOverrideTableBytes, 0, this.m_listOverrideTableBytes.Length);
      this.m_listOverrideTableBytes = (byte[]) null;
    }
    else
    {
      if (this.m_fontBytes.Length > 9)
      {
        this.m_stream.Write(this.m_fontBytes, 0, this.m_fontBytes.Length);
        this.m_stream.WriteByte((byte) 125);
        this.m_stream.WriteByte((byte) 13);
        this.m_stream.WriteByte((byte) 10);
        this.m_fontBytes = (byte[]) null;
      }
      if (this.m_colorBytes.Length > 11)
      {
        this.m_stream.Write(this.m_colorBytes, 0, this.m_colorBytes.Length);
        this.m_stream.WriteByte((byte) 125);
        this.m_stream.WriteByte((byte) 13);
        this.m_stream.WriteByte((byte) 10);
        this.m_colorBytes = (byte[]) null;
      }
      if (this.m_doc.HasStyleSheets)
      {
        this.m_stream.Write(this.m_defStyleBytes, 0, this.m_defStyleBytes.Length);
        this.m_defStyleBytes = (byte[]) null;
        this.m_stream.Write(this.m_styleBytes, 0, this.m_styleBytes.Length);
        this.m_stream.WriteByte((byte) 125);
        this.m_stream.WriteByte((byte) 13);
        this.m_stream.WriteByte((byte) 10);
        this.m_styleBytes = (byte[]) null;
      }
      if (this.m_listTableBytes != null && this.m_listOverrideTableBytes != null)
      {
        this.m_stream.Write(this.m_listTableBytes, 0, this.m_listTableBytes.Length);
        this.m_listTableBytes = (byte[]) null;
        this.m_stream.Write(this.m_listOverrideTableBytes, 0, this.m_listOverrideTableBytes.Length);
        this.m_listOverrideTableBytes = (byte[]) null;
      }
    }
    this.BuildDocProperties();
    if (this.m_doc.DOP.AutoHyphen)
    {
      byte[] bytes = this.m_encoding.GetBytes(this.c_slashSymbol + "hyphauto1");
      this.m_stream.Write(bytes, 0, bytes.Length);
    }
    if (!this.m_doc.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark])
    {
      byte[] bytes = this.m_encoding.GetBytes(this.c_slashSymbol + "spltpgpar");
      this.m_stream.Write(bytes, 0, bytes.Length);
    }
    if (this.m_doc.Settings.CompatibilityOptions[CompatibilityOption.PrintMet])
    {
      byte[] bytes = this.m_encoding.GetBytes(this.c_slashSymbol + "lytprtmet");
      this.m_stream.Write(bytes, 0, bytes.Length);
    }
    if (!this.m_doc.Settings.CompatibilityOptions[CompatibilityOption.DontUseHTMLParagraphAutoSpacing])
    {
      byte[] bytes = this.m_encoding.GetBytes(this.c_slashSymbol + "htmautsp");
      this.m_stream.Write(bytes, 0, bytes.Length);
    }
    if (this.m_doc.DOP.Dop2000.Copts.Copts80.Copts60.NoSpaceRaiseLower)
    {
      byte[] bytes = this.m_encoding.GetBytes(this.c_slashSymbol + "noextrasprl");
      this.m_stream.Write(bytes, 0, bytes.Length);
    }
    int count = this.m_mainBodyBytesList.Count;
    for (int index = 0; index < count; ++index)
    {
      this.m_stream.Write(this.m_mainBodyBytesList[0], 0, this.m_mainBodyBytesList[0].Length);
      this.m_mainBodyBytesList.RemoveAt(0);
    }
    this.m_mainBodyBytesList = (List<byte[]>) null;
  }

  private void BuildDocProperties()
  {
    WPageSetup pageSetup = this.m_doc.Sections[0].PageSetup;
    byte[] bytes1 = this.m_encoding.GetBytes($"{this.c_slashSymbol}paperw{(object) (int) Math.Round((double) pageSetup.PageSize.Width * 20.0)}");
    this.m_stream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes($"{this.c_slashSymbol}paperh{(object) (int) Math.Round((double) pageSetup.PageSize.Height * 20.0)}");
    this.m_stream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margl{(object) (int) Math.Round((double) pageSetup.Margins.Left * 20.0)}");
    this.m_stream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margr{(object) (int) Math.Round((double) pageSetup.Margins.Right * 20.0)}");
    this.m_stream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margt{(object) (int) Math.Round((double) pageSetup.Margins.Top * 20.0)}");
    this.m_stream.Write(bytes5, 0, bytes5.Length);
    byte[] bytes6 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margb{(object) (int) Math.Round((double) pageSetup.Margins.Bottom * 20.0)}");
    this.m_stream.Write(bytes6, 0, bytes6.Length);
  }

  private void BuildDefaultStyles()
  {
    WParagraphStyle byName = this.m_doc.Styles.FindByName("Normal") as WParagraphStyle;
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}defchp");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(byName.CharacterFormat));
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}defpap");
    memoryStream.Write(bytes5, 0, bytes5.Length);
    byte[] bytes6 = this.m_encoding.GetBytes(this.BuildParagraphFormat(byName.ParagraphFormat, (WParagraph) null));
    memoryStream.Write(bytes6, 0, bytes6.Length);
    byte[] bytes7 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes7, 0, bytes7.Length);
    byte[] bytes8 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes8, 0, bytes8.Length);
    this.m_defStyleBytes = memoryStream.ToArray();
  }

  private void BuildSections()
  {
    this.m_mainBodyBytesList = new List<byte[]>();
    this.BuildBackground();
    int index = 0;
    for (int count = this.m_doc.Sections.Count; index < count; ++index)
    {
      this.m_hasFootnote = this.m_hasEndnote = false;
      WSection section = this.m_doc.Sections[index];
      this.BuildSectionProp(section);
      this.CheckFootEndnote();
      this.BuildSection(section);
    }
  }

  private void BuildBackground()
  {
    if (this.m_doc.Background.Type == BackgroundType.NoBackground)
      return;
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes(this.c_slashSymbol + "viewbksp1");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}background");
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}shp");
    memoryStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}shpinst");
    memoryStream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = this.m_encoding.GetBytes(this.BuildShapeFill(this.m_doc.Background, true));
    memoryStream.Write(bytes5, 0, bytes5.Length);
    byte[] bytes6 = this.m_encoding.GetBytes("}}}");
    memoryStream.Write(bytes6, 0, bytes6.Length);
    byte[] bytes7 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes7, 0, bytes7.Length);
    this.m_mainBodyBytesList.Add(memoryStream.ToArray());
  }

  private void BuildSection(WSection section)
  {
    IEntity nextSibling = section.NextSibling;
    this.BuildHeadersFooters(section.HeadersFooters);
    this.BuildSectionBodyItems(section.Body.Items);
    this.m_mainBodyBytesList.Add(this.m_encoding.GetBytes(Environment.NewLine));
  }

  private void BuildSectionBodyItems(BodyItemCollection bodyItems)
  {
    for (int index = 0; index < bodyItems.Count; ++index)
    {
      switch (bodyItems[index].EntityType)
      {
        case EntityType.Paragraph:
          WParagraph bodyItem = bodyItems[index] as WParagraph;
          bodyItem.SplitTextRange();
          this.m_mainBodyBytesList.Add(this.BuildParagraph(bodyItem));
          break;
        case EntityType.BlockContentControl:
          this.m_mainBodyBytesList.Add(this.BuildBodyItems((bodyItems[index] as BlockContentControl).TextBody.Items));
          break;
        case EntityType.Table:
          this.m_mainBodyBytesList.Add(this.BuildTable(bodyItems[index] as WTable));
          break;
      }
    }
    if (!(bodyItems.LastItem is WTable))
      return;
    this.m_mainBodyBytesList.Add(this.BuildParagraph(new WParagraph((IWordDocument) bodyItems.Document)));
  }

  private byte[] BuildBodyItems(BodyItemCollection bodyItems)
  {
    MemoryStream memoryStream = new MemoryStream();
    for (int index = 0; index < bodyItems.Count; ++index)
    {
      switch (bodyItems[index].EntityType)
      {
        case EntityType.Paragraph:
          WParagraph bodyItem = bodyItems[index] as WParagraph;
          bodyItem.SplitTextRange();
          byte[] buffer1 = this.BuildParagraph(bodyItem);
          memoryStream.Write(buffer1, 0, buffer1.Length);
          break;
        case EntityType.BlockContentControl:
          byte[] buffer2 = this.BuildBodyItems((bodyItems[index] as BlockContentControl).TextBody.Items);
          memoryStream.Write(buffer2, 0, buffer2.Length);
          break;
        case EntityType.Table:
          byte[] buffer3 = this.BuildTable(bodyItems[index] as WTable);
          memoryStream.Write(buffer3, 0, buffer3.Length);
          break;
      }
    }
    if (bodyItems.LastItem is WTable)
    {
      byte[] buffer = this.BuildParagraph(new WParagraph((IWordDocument) bodyItems.Document));
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    return memoryStream.ToArray();
  }

  private void BuildHeadersFooters(WHeadersFooters headerFooters)
  {
    if (headerFooters == null && this.m_doc.Watermark == null)
      return;
    if (headerFooters.EvenHeader.Items.Count > 0 || headerFooters.EvenHeader.Watermark.Type != WatermarkType.NoWatermark && headerFooters.EvenHeader.WriteWatermark)
      this.BuildHeaderFooter($"{{{this.c_slashSymbol}headerl", headerFooters.EvenHeader.Items, this.GetWatermark(headerFooters.EvenHeader.Watermark), headerFooters.EvenHeader.WriteWatermark);
    if (headerFooters.OddHeader.Items.Count > 0 || headerFooters.OddHeader.Watermark.Type != WatermarkType.NoWatermark && headerFooters.OddHeader.WriteWatermark)
      this.BuildHeaderFooter($"{{{this.c_slashSymbol}headerr", headerFooters.OddHeader.Items, this.GetWatermark(headerFooters.OddHeader.Watermark), headerFooters.OddHeader.WriteWatermark);
    if (headerFooters.EvenFooter.Items.Count > 0)
      this.BuildHeaderFooter($"{{{this.c_slashSymbol}footerl", headerFooters.EvenFooter.Items, string.Empty, false);
    if (headerFooters.OddFooter.Items.Count > 0)
      this.BuildHeaderFooter($"{{{this.c_slashSymbol}footerr", headerFooters.OddFooter.Items, string.Empty, false);
    if (headerFooters.FirstPageHeader.Items.Count > 0 || headerFooters.FirstPageHeader.Watermark.Type != WatermarkType.NoWatermark && headerFooters.FirstPageHeader.WriteWatermark)
      this.BuildHeaderFooter($"{{{this.c_slashSymbol}headerf", headerFooters.FirstPageHeader.Items, this.GetWatermark(headerFooters.FirstPageHeader.Watermark), headerFooters.FirstPageHeader.WriteWatermark);
    if (headerFooters.FirstPageFooter.Items.Count <= 0)
      return;
    this.BuildHeaderFooter($"{{{this.c_slashSymbol}footerf", headerFooters.FirstPageFooter.Items, string.Empty, false);
  }

  private string GetWatermark(Watermark waterMark)
  {
    if (waterMark == null || waterMark.Type == WatermarkType.NoWatermark || waterMark is PictureWatermark && (waterMark as PictureWatermark).Picture == null)
      return string.Empty;
    return waterMark.Type == WatermarkType.TextWatermark ? this.BuildTextWtrmarkBody(waterMark as TextWatermark) : this.BuildPictWtrmarkBody(waterMark as PictureWatermark);
  }

  private void BuildHeaderFooter(
    string name,
    BodyItemCollection collect,
    string watermarkStr,
    bool writeWaterMark)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes(name);
    memoryStream.Write(bytes1, 0, bytes1.Length);
    if (writeWaterMark)
    {
      byte[] bytes2 = this.m_encoding.GetBytes(watermarkStr);
      memoryStream.Write(bytes2, 0, bytes2.Length);
    }
    byte[] buffer = this.BuildBodyItems(collect);
    memoryStream.Write(buffer, 0, buffer.Length);
    byte[] bytes3 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes3, 0, bytes3.Length);
    this.m_mainBodyBytesList.Add(memoryStream.ToArray());
  }

  private byte[] BuildParagraph(WParagraph para)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes(this.BuildListText(para, para.GetListFormatValue()));
    memoryStream.Write(bytes1, 0, bytes1.Length);
    if (para.PreviousSibling == null || !(para.PreviousSibling is WParagraph) || !para.ParagraphFormat.Compare((para.PreviousSibling as WParagraph).ParagraphFormat) || !this.CompareListFormat(para) || this.IsPreviousParagraphHasFieldEnd(para.PreviousSibling as WParagraph) || para.ParaStyle == null || !(para.ParaStyle as Style).Compare((para.PreviousSibling as WParagraph).ParaStyle as Style))
    {
      byte[] bytes2 = this.m_encoding.GetBytes(this.BuildParagraphFormat(para.ParagraphFormat, para));
      memoryStream.Write(bytes2, 0, bytes2.Length);
    }
    for (int index = 0; index < para.Items.Count; ++index)
    {
      byte[] buffer = this.BuildParagraphItem(para.Items[index]);
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    if (this.HasParaEnd(para))
    {
      byte[] buffer = this.BuildParagraphEnd(para);
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    else
    {
      byte[] bytes3 = this.m_encoding.GetBytes(this.BuildCharacterFormat(para.BreakCharacterFormat));
      memoryStream.Write(bytes3, 0, bytes3.Length);
      if (para.Owner != null && para.OwnerTextBody.OwnerBase is WSection && (para.OwnerTextBody.OwnerBase as WSection).PreviousSibling == null && para.Items.Count == 0 && para.NextSibling == null && !(para.PreviousSibling is WTable) && para.Document.Sections.Count > 1)
        this.BuildSectToken(memoryStream);
    }
    return memoryStream.ToArray();
  }

  private bool CompareListFormat(WParagraph paragraph)
  {
    WListFormat listFormat = (paragraph.PreviousSibling as WParagraph).ListFormat;
    return paragraph.ListFormat.Compare(listFormat) && (paragraph.ListFormat.CurrentListStyle != null && listFormat.CurrentListStyle != null && paragraph.ListFormat.CurrentListStyle.Compare(listFormat.CurrentListStyle) || paragraph.ListFormat.CurrentListStyle == null && listFormat.CurrentListStyle == null);
  }

  private bool IsPreviousParagraphHasFieldEnd(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index].EntityType == EntityType.FieldMark && (paragraph.Items[index] as WFieldMark).Type == FieldMarkType.FieldEnd)
        return true;
    }
    return false;
  }

  private byte[] BuildParagraphEnd(WParagraph para)
  {
    WSection ownerTextBody = para.GetOwnerTextBody((Entity) para) as WSection;
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(para.BreakCharacterFormat));
    memoryStream.Write(bytes2, 0, bytes2.Length);
    if (!(para.GetOwnerTextBody((Entity) para) is HeaderFooter) && para.NextSibling == null && ownerTextBody != null && ownerTextBody.NextSibling != null)
      this.BuildSectToken(memoryStream);
    else
      bytes2 = this.m_encoding.GetBytes(this.c_slashSymbol + "par");
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes4, 0, bytes4.Length);
    return memoryStream.ToArray();
  }

  private void BuildSectToken(MemoryStream memoryStream)
  {
    byte[] bytes = this.m_encoding.GetBytes(this.c_slashSymbol + "sect");
    memoryStream.Write(bytes, 0, bytes.Length);
  }

  private string BuildCharacterFormat(WCharacterFormat cFormat)
  {
    if (cFormat == null)
      return string.Empty;
    WCharacterFormat baseCFormat = (WCharacterFormat) null;
    if (!string.IsNullOrEmpty(cFormat.CharStyleName) && this.m_doc.Styles.FindByName(cFormat.CharStyleName) is Style byName && byName.CharacterFormat != null)
      baseCFormat = byName.CharacterFormat;
    StringBuilder strBuilder = new StringBuilder();
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
      strBuilder.Append($"{this.c_slashSymbol}rtlch{this.c_slashSymbol}fcs0{this.c_slashSymbol}lang1033");
    else
      strBuilder.Append(this.c_slashSymbol + "rtlch");
    if (cFormat.Bidi && cFormat.HasValue(58) && !this.m_isField)
      strBuilder.Insert(0, this.WriteFontNameBidi(cFormat));
    else
      strBuilder.Append(this.WriteFontNameBidi(cFormat));
    if (cFormat.OwnerBase is WParagraphStyle)
    {
      strBuilder.Append(this.c_slashSymbol + "afs");
      strBuilder.Append((short) Math.Round((double) cFormat.FontSizeBidi * 2.0));
    }
    else if (cFormat.HasValue(3))
    {
      strBuilder.Append(this.c_slashSymbol + "afs");
      strBuilder.Append((short) Math.Round((double) cFormat.FontSizeBidi * 2.0));
    }
    string empty = string.Empty;
    string str;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
      str = $"{this.c_slashSymbol}ltrch{this.c_slashSymbol}fcs0{this.c_slashSymbol}lang1033";
    else
      str = this.c_slashSymbol + "ltrch";
    if (cFormat.Bidi && cFormat.HasValue(58) && !this.m_isField)
      strBuilder.Insert(0, str);
    else
      strBuilder.Append(str);
    strBuilder.Append(this.WriteFontName(cFormat));
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && cFormat.HasValue(18) || cFormat.HasValue(18) && (double) cFormat.CharacterSpacing != 0.0)
    {
      short num1 = (short) Math.Round((double) cFormat.CharacterSpacing * 4.0);
      strBuilder.Append($"{this.c_slashSymbol}expnd{(object) num1}");
      short num2 = (short) Math.Round((double) cFormat.CharacterSpacing * 20.0);
      strBuilder.Append($"{this.c_slashSymbol}expndtw{(object) num2}");
    }
    if (cFormat.OwnerBase is WParagraphStyle)
    {
      strBuilder.Append(this.c_slashSymbol + "fs");
      strBuilder.Append((short) Math.Round((double) cFormat.FontSize * 2.0));
    }
    else if (cFormat.HasValue(3))
    {
      strBuilder.Append(this.c_slashSymbol + "fs");
      strBuilder.Append((short) Math.Round((double) cFormat.FontSize * 2.0));
    }
    else if (cFormat.Bidi && cFormat.HasValue(58) && !this.m_isField && cFormat.OwnerBase is WTextRange)
    {
      strBuilder.Append(this.c_slashSymbol + "fs");
      strBuilder.Append((short) Math.Round((double) cFormat.FontSizeBidi * 2.0));
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
    {
      if (cFormat.HasValue(73))
        strBuilder.Append($"{this.c_slashSymbol}lang{(object) cFormat.LocaleIdASCII}");
      if (cFormat.HasValue(74))
        strBuilder.Append($"{this.c_slashSymbol}langfenp{(object) cFormat.LocaleIdFarEast}");
      if (cFormat.HasValue(74))
        strBuilder.Append($"{this.c_slashSymbol}langfe{(object) cFormat.LocaleIdBidi}");
    }
    if (cFormat.HasValue(17))
    {
      if ((double) cFormat.Position > 0.0)
      {
        strBuilder.Append(this.c_slashSymbol + "up");
        strBuilder.Append(cFormat.Position * 2f);
      }
      if ((double) cFormat.Position < 0.0)
      {
        strBuilder.Append(this.c_slashSymbol + "dn");
        strBuilder.Append((float) (-(double) cFormat.Position * 2.0));
      }
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && cFormat.HasValue((int) sbyte.MaxValue) || cFormat.HasValue((int) sbyte.MaxValue) && (double) cFormat.Scaling != 100.0)
    {
      strBuilder.Append(this.c_slashSymbol + "charscalex");
      strBuilder.Append(cFormat.Scaling);
    }
    if (!string.IsNullOrEmpty(cFormat.CharStyleName) && this.Styles.ContainsKey(cFormat.CharStyleName))
      strBuilder.Append(this.Styles[cFormat.CharStyleName]);
    bool bold;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((bold = cFormat.Bold) || cFormat.HasValue(4)) || cFormat.HasValue(4) && (bold = cFormat.Bold))
      strBuilder.Append(this.c_slashSymbol + (bold ? "b" : "b0"));
    bool italic;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((italic = cFormat.Italic) || cFormat.HasValue(5)) || cFormat.HasValue(5) && (italic = cFormat.Italic))
      strBuilder.Append(this.c_slashSymbol + (italic ? "i" : "i0"));
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && cFormat.HasValue(7) || cFormat.HasValue(7) && cFormat.UnderlineStyle != UnderlineStyle.None)
      this.BuildUnderLineStyle(cFormat.UnderlineStyle, strBuilder);
    else if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && baseCFormat != null && baseCFormat.HasValue(7) || baseCFormat != null && baseCFormat.HasValue(7) && cFormat.UnderlineStyle != UnderlineStyle.None)
      this.BuildUnderLineStyle(baseCFormat.UnderlineStyle, strBuilder);
    bool strikeout;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((strikeout = cFormat.Strikeout) || cFormat.HasValue(6)) || cFormat.HasValue(6) && (strikeout = cFormat.Strikeout))
      strBuilder.Append(this.c_slashSymbol + (strikeout ? "strike" : "strike0"));
    bool doubleStrike;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((doubleStrike = cFormat.DoubleStrike) || cFormat.HasValue(14)) || cFormat.HasValue(14) && (doubleStrike = cFormat.DoubleStrike))
      strBuilder.Append(this.c_slashSymbol + (doubleStrike ? "striked1" : "striked0"));
    bool shadow;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((shadow = cFormat.Shadow) || cFormat.HasValue(50)) || cFormat.HasValue(50) && (shadow = cFormat.Shadow))
      strBuilder.Append(this.c_slashSymbol + (shadow ? "shad" : "shad0"));
    if (cFormat.HasValue(10))
    {
      if (cFormat.SubSuperScript == SubSuperScript.SuperScript)
        strBuilder.Append(this.c_slashSymbol + "super");
      else if (cFormat.SubSuperScript == SubSuperScript.SubScript)
        strBuilder.Append(this.c_slashSymbol + "sub");
      else if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && cFormat.SubSuperScript == SubSuperScript.None)
        strBuilder.Append(this.c_slashSymbol + "nosupersub");
    }
    else if (baseCFormat != null && baseCFormat.HasValue(10))
    {
      if (baseCFormat.SubSuperScript == SubSuperScript.SuperScript)
        strBuilder.Append(this.c_slashSymbol + "super");
      else if (baseCFormat.SubSuperScript == SubSuperScript.SubScript)
        strBuilder.Append(this.c_slashSymbol + "sub");
      else if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && baseCFormat.SubSuperScript == SubSuperScript.None)
        strBuilder.Append(this.c_slashSymbol + "nosupersub");
    }
    Color baseCFormatColor = Color.Empty;
    if (baseCFormat != null)
      baseCFormatColor = baseCFormat.TextColor;
    strBuilder.Append(this.BuildColorValue(cFormat, cFormat.TextColor, baseCFormat, baseCFormatColor, 1, this.c_slashSymbol + "cf"));
    if (baseCFormat != null)
      baseCFormatColor = baseCFormat.TextBackgroundColor;
    strBuilder.Append(this.BuildColorValue(cFormat, cFormat.TextBackgroundColor, baseCFormat, baseCFormatColor, 9, this.c_slashSymbol + "chcbpat"));
    if (baseCFormat != null)
      baseCFormatColor = baseCFormat.ForeColor;
    strBuilder.Append(this.BuildColorValue(cFormat, cFormat.ForeColor, baseCFormat, baseCFormatColor, 77, this.c_slashSymbol + "chcfpat"));
    if (baseCFormat != null)
      baseCFormatColor = baseCFormat.HighlightColor;
    strBuilder.Append(this.BuildColorValue(cFormat, cFormat.HighlightColor, baseCFormat, baseCFormatColor, 63 /*0x3F*/, this.c_slashSymbol + "highlight"));
    strBuilder.Append(this.BuildTextBorder(cFormat.Border));
    bool smallCaps;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((smallCaps = cFormat.SmallCaps) || cFormat.HasValue(55)) || cFormat.HasValue(55) && (smallCaps = cFormat.SmallCaps))
      strBuilder.Append(this.c_slashSymbol + (smallCaps ? "scaps" : "scaps0"));
    bool hidden;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((hidden = cFormat.Hidden) || cFormat.HasValue(53)) || cFormat.HasValue(53) && (hidden = cFormat.Hidden))
      strBuilder.Append(this.c_slashSymbol + (hidden ? "v" : "v0"));
    bool outLine;
    if ((outLine = cFormat.OutLine) || cFormat.HasValue(71))
      strBuilder.Append(this.c_slashSymbol + (outLine ? "outl" : "outl0"));
    bool allCaps;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((allCaps = cFormat.AllCaps) || cFormat.HasValue(54)) || cFormat.HasValue(54) && (allCaps = cFormat.AllCaps))
      strBuilder.Append(this.c_slashSymbol + (allCaps ? "caps" : "caps0"));
    bool emboss;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((emboss = cFormat.Emboss) || cFormat.HasValue(51)) || cFormat.HasValue(51) && (emboss = cFormat.Emboss))
      strBuilder.Append(this.c_slashSymbol + (emboss ? "embo" : "embo0"));
    bool engrave;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((engrave = cFormat.Engrave) || cFormat.HasValue(52)) || cFormat.HasValue(52) && (engrave = cFormat.Engrave))
      strBuilder.Append(this.c_slashSymbol + (engrave ? "impr" : "impr0"));
    bool specVanish;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && ((specVanish = cFormat.SpecVanish) || cFormat.HasValue(24)) || cFormat.HasValue(24) && (specVanish = cFormat.SpecVanish))
      strBuilder.Append(this.c_slashSymbol + (specVanish ? "spv" : "spv0"));
    return strBuilder.ToString();
  }

  private string GetParagraphAlignment(HorizontalAlignment hAlignment)
  {
    switch (hAlignment)
    {
      case HorizontalAlignment.Center:
        return "qc";
      case HorizontalAlignment.Right:
        return "qr";
      case HorizontalAlignment.Justify:
      case HorizontalAlignment.Right | HorizontalAlignment.Distribute:
        return "qj";
      case HorizontalAlignment.Distribute:
        return "qd";
      case HorizontalAlignment.JustifyMedium:
        return "qk10";
      case HorizontalAlignment.JustifyHigh:
        return "qk20";
      case HorizontalAlignment.JustifyLow:
        return "qk0";
      case HorizontalAlignment.ThaiJustify:
        return "qt";
      default:
        return "ql";
    }
  }

  private string BuildParagraphFormat(WParagraphFormat pFormat, WParagraph para, bool isParaText)
  {
    if (pFormat == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    string str1 = string.Empty;
    string empty = string.Empty;
    if (para != null)
    {
      string str2 = para.StyleName;
      if (string.IsNullOrEmpty(str2))
        str2 = "Normal";
      if (!this.Styles.ContainsKey(str2))
        this.BuildStyle(str2);
      if (this.Styles.ContainsKey(str2))
        str1 = this.Styles[str2];
    }
    WParagraphFormat wparagraphFormat = (WParagraphFormat) null;
    if (para != null && !string.IsNullOrEmpty(para.StyleName) && this.m_doc.Styles.FindByName(para.StyleName) is Style byName && byName is WParagraphStyle && (byName as WParagraphStyle).ParagraphFormat != null)
      wparagraphFormat = (byName as WParagraphStyle).ParagraphFormat;
    if (isParaText)
    {
      stringBuilder.Append(this.c_slashSymbol + "pard");
      stringBuilder.Append(this.c_slashSymbol + "plain");
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
      stringBuilder.Append(this.c_slashSymbol + "lang1033");
    if (pFormat.WidowControl)
      stringBuilder.Append(this.c_slashSymbol + "widctlpar");
    else
      stringBuilder.Append(this.c_slashSymbol + "nowidctlpar");
    stringBuilder.Append(str1);
    if (pFormat.Bidi)
      stringBuilder.Append(this.c_slashSymbol + "rtlpar");
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.HorizontalAlignment != HorizontalAlignment.Left)
      stringBuilder.Append(this.c_slashSymbol + this.GetParagraphAlignment(pFormat.HorizontalAlignment));
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || pFormat.IsFrame)
    {
      if (pFormat.HasValue(71))
      {
        switch (pFormat.FrameHorizontalPos)
        {
          case 0:
            stringBuilder.Append(this.c_slashSymbol + "phcol");
            break;
          case 1:
            stringBuilder.Append(this.c_slashSymbol + "phmrg");
            break;
          case 2:
            stringBuilder.Append(this.c_slashSymbol + "phpg");
            break;
        }
      }
      if (pFormat.HasValue(72))
      {
        switch (pFormat.FrameVerticalPos)
        {
          case 0:
            stringBuilder.Append(this.c_slashSymbol + "pvmrg");
            break;
          case 1:
            stringBuilder.Append(this.c_slashSymbol + "pvpg");
            break;
          case 2:
            stringBuilder.Append(this.c_slashSymbol + "pvpara");
            break;
        }
      }
      if (pFormat.HasValue(76))
      {
        int num = (int) Math.Round((double) pFormat.FrameWidth * 20.0);
        stringBuilder.Append($"{this.c_slashSymbol}absw{(object) num}");
      }
      if (pFormat.HasValue(77))
      {
        ushort num1 = (ushort) Math.Round((double) pFormat.FrameHeight * 20.0);
        if (((int) num1 & 32768 /*0x8000*/) != 0)
        {
          int num2 = (int) num1 & (int) short.MaxValue;
          stringBuilder.Append($"{this.c_slashSymbol}absh{(object) num2}");
        }
        else
          stringBuilder.Append($"{this.c_slashSymbol}absh-{(object) num1}");
      }
      if (pFormat.HasValue(73))
      {
        if (pFormat.IsFrameXAlign(pFormat.FrameX))
        {
          switch ((PageNumberAlignment) pFormat.FrameX)
          {
            case PageNumberAlignment.Outside:
              stringBuilder.Append(this.c_slashSymbol + "posxo");
              break;
            case PageNumberAlignment.Inside:
              stringBuilder.Append(this.c_slashSymbol + "posxi");
              break;
            case PageNumberAlignment.Right:
              stringBuilder.Append(this.c_slashSymbol + "posxr");
              break;
            case PageNumberAlignment.Center:
              stringBuilder.Append(this.c_slashSymbol + "posxc");
              break;
            case PageNumberAlignment.Left:
              stringBuilder.Append(this.c_slashSymbol + "posxl");
              break;
          }
        }
        else
        {
          int num = (int) Math.Round((double) pFormat.FrameX * 20.0);
          if (num < 0)
            stringBuilder.Append($"{this.c_slashSymbol}posnegx{(object) num}");
          else
            stringBuilder.Append($"{this.c_slashSymbol}posx{(object) num}");
        }
      }
      if (pFormat.HasValue(74))
      {
        if (pFormat.IsFrameYAlign(pFormat.FrameY))
        {
          switch ((FrameVerticalPosition) pFormat.FrameY)
          {
            case FrameVerticalPosition.Outside:
              stringBuilder.Append(this.c_slashSymbol + "posyout");
              break;
            case FrameVerticalPosition.Inside:
              stringBuilder.Append(this.c_slashSymbol + "posyin");
              break;
            case FrameVerticalPosition.Bottom:
              stringBuilder.Append(this.c_slashSymbol + "posyb");
              break;
            case FrameVerticalPosition.Center:
              stringBuilder.Append(this.c_slashSymbol + "posyc");
              break;
            case FrameVerticalPosition.Top:
              stringBuilder.Append(this.c_slashSymbol + "posyt");
              break;
            case FrameVerticalPosition.Inline:
              stringBuilder.Append(this.c_slashSymbol + "posyil");
              break;
          }
        }
        else
        {
          int num = (int) Math.Round((double) pFormat.FrameY * 20.0);
          if (num < 0)
            stringBuilder.Append($"{this.c_slashSymbol}posnegy{(object) num}");
          else
            stringBuilder.Append($"{this.c_slashSymbol}posy{(object) num}");
        }
      }
      if (pFormat.HasValue(88))
      {
        switch (pFormat.WrapFrameAround)
        {
          case FrameWrapMode.Auto:
            stringBuilder.Append(this.c_slashSymbol + "wrapdefault");
            break;
          case FrameWrapMode.Around:
            stringBuilder.Append(this.c_slashSymbol + "wraparound");
            break;
          case FrameWrapMode.None:
            stringBuilder.Append(this.c_slashSymbol + "nowrap");
            break;
          case FrameWrapMode.Tight:
            stringBuilder.Append(this.c_slashSymbol + "wraptight");
            break;
          case FrameWrapMode.Through:
            stringBuilder.Append(this.c_slashSymbol + "wrapthrough");
            break;
        }
      }
      if (pFormat.HasValue(83))
      {
        int num = (int) Math.Round((double) pFormat.FrameHorizontalDistanceFromText * 20.0);
        stringBuilder.Append($"{this.c_slashSymbol}dfrmtxtx{(object) num}");
      }
      if (pFormat.HasValue(84))
      {
        int num = (int) Math.Round((double) pFormat.FrameVerticalDistanceFromText * 20.0);
        stringBuilder.Append($"{this.c_slashSymbol}dfrmtxty{(object) num}");
      }
    }
    int num3 = (int) Math.Round((double) pFormat.FirstLineIndent * 20.0);
    int num4 = (int) Math.Round((double) pFormat.LeftIndent * 20.0);
    int num5 = (int) Math.Round((double) pFormat.RightIndent * 20.0);
    if (para != null && !para.ListFormat.IsEmptyList)
    {
      WListFormat listFormatValue = para.GetListFormatValue();
      if (listFormatValue != null && listFormatValue.CurrentListStyle != null)
      {
        ListStyle currentListStyle = listFormatValue.CurrentListStyle;
        WListLevel listLevel = para.GetListLevel(listFormatValue);
        if (currentListStyle.ListType == ListType.Numbered || currentListStyle.ListType == ListType.Bulleted)
        {
          if (listLevel.ParagraphFormat.HasValue(5))
            num3 = (int) Math.Round((double) listLevel.ParagraphFormat.FirstLineIndent * 20.0);
          if (listLevel.ParagraphFormat.HasValue(2))
            num4 = (int) Math.Round((double) listLevel.ParagraphFormat.LeftIndent * 20.0);
          if (listLevel.ParagraphFormat.HasValue(3))
            num5 = (int) Math.Round((double) listLevel.ParagraphFormat.RightIndent * 20.0);
        }
      }
    }
    if (pFormat.HasValue(5))
      num3 = (int) Math.Round((double) pFormat.FirstLineIndent * 20.0);
    if (pFormat.HasValue(2))
      num4 = (int) Math.Round((double) pFormat.LeftIndent * 20.0);
    if (pFormat.HasValue(3))
      num5 = (int) Math.Round((double) pFormat.RightIndent * 20.0);
    if (pFormat.Bidi && (pFormat.Document.ActualFormatType == FormatType.Doc || pFormat.Document.ActualFormatType == FormatType.Docx || pFormat.Document.ActualFormatType == FormatType.Rtf))
    {
      int num6 = num4;
      num4 = num5;
      num5 = num6;
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || num3 != 0)
    {
      stringBuilder.Append(this.c_slashSymbol + "fi");
      stringBuilder.Append(num3);
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || num4 != 0)
    {
      stringBuilder.Append(this.c_slashSymbol + "li");
      stringBuilder.Append(num4);
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || num5 != 0)
    {
      stringBuilder.Append(this.c_slashSymbol + "ri");
      stringBuilder.Append(num5);
    }
    if (pFormat.SuppressAutoHyphens)
      stringBuilder.Append(this.c_slashSymbol + "hyphpar0");
    if (pFormat.MirrorIndents)
      stringBuilder.Append(this.c_slashSymbol + "indmirror");
    stringBuilder.Append(this.BuildFrameProps(pFormat));
    bool isInCell = para != null && para.IsInCell;
    if (isInCell)
      stringBuilder.Append(this.c_slashSymbol + "intbl");
    stringBuilder.Append(this.BuildParaBorders(pFormat));
    stringBuilder.Append(this.BuildParaSpacing(pFormat, isInCell));
    if (pFormat.Keep)
      stringBuilder.Append(this.c_slashSymbol + "keep");
    if (pFormat.KeepFollow)
      stringBuilder.Append(this.c_slashSymbol + "keepn");
    if (pFormat.PageBreakBefore)
      stringBuilder.Append(this.c_slashSymbol + "pagebb");
    if ((byte) pFormat.OutlineLevel >= (byte) 0 && (byte) pFormat.OutlineLevel < (byte) 9)
    {
      stringBuilder.Append(this.c_slashSymbol + "outlinelevel");
      stringBuilder.Append((byte) pFormat.OutlineLevel);
    }
    string str3 = string.Empty;
    string str4 = string.Empty;
    string str5 = string.Empty;
    if (para != null && para.ParaStyle.ParagraphFormat.HasShading())
    {
      if (!para.ParaStyle.ParagraphFormat.BackColor.IsEmpty)
        str3 = this.BuildColor(para.ParaStyle.ParagraphFormat.BackColor, this.c_slashSymbol + "cbpat");
      if (!para.ParaStyle.ParagraphFormat.ForeColor.IsEmpty)
        str4 = this.BuildColor(para.ParaStyle.ParagraphFormat.ForeColor, this.c_slashSymbol + "cfpat");
      str5 = this.BuildTextureStyle(para.ParaStyle.ParagraphFormat.TextureStyle);
    }
    if (pFormat.HasShading())
    {
      if (!pFormat.BackColor.IsEmpty)
        str3 = this.BuildColor(pFormat.BackColor, this.c_slashSymbol + "cbpat");
      if (!pFormat.ForeColor.IsEmpty)
        str4 = this.BuildColor(pFormat.ForeColor, this.c_slashSymbol + "cfpat");
      str5 = this.BuildTextureStyle(pFormat.TextureStyle);
    }
    if (str3 != string.Empty)
      stringBuilder.Append(str3);
    if (str4 != string.Empty)
      stringBuilder.Append(str4);
    if (str5 != string.Empty)
      stringBuilder.Append(str5);
    stringBuilder.Append(this.BuildParaListId(para, pFormat));
    TabCollection tabs1 = wparagraphFormat?.Tabs;
    TabCollection tabs2 = pFormat.Tabs.Count > 0 ? pFormat.Tabs : tabs1;
    stringBuilder.Append(this.BuildTabs(tabs2));
    WListFormat listFormatValue1 = para?.GetListFormatValue();
    TabCollection tabs3 = listFormatValue1 == null || listFormatValue1.ListType == ListType.NoList || listFormatValue1.CurrentListLevel.ParagraphFormat.Tabs.Count <= 0 ? (TabCollection) null : listFormatValue1.CurrentListLevel.ParagraphFormat.Tabs;
    stringBuilder.Append(this.BuildTabs(tabs3));
    if (para != null && para.ParaStyle != null)
    {
      WCharacterFormat characterFormat = para.ParaStyle.CharacterFormat;
      stringBuilder.Append(this.BuildCharacterFormat(characterFormat));
    }
    else if (this.m_doc.DefCharFormat != null)
      stringBuilder.Append(this.BuildCharacterFormat(this.m_doc.DefCharFormat));
    if (wparagraphFormat != null && wparagraphFormat.ContextualSpacing)
      stringBuilder.Append(this.c_slashSymbol + "contextualspace");
    if (para != null && this.m_tableNestedLevel > 1)
    {
      int num7 = this.m_tableNestedLevel > 0 ? this.m_tableNestedLevel : 1;
      stringBuilder.Append($"{this.c_slashSymbol}itap{(object) num7}");
    }
    stringBuilder.Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  private string BuildParagraphFormat(WParagraphFormat pFormat, WParagraph para)
  {
    return this.BuildParagraphFormat(pFormat, para, true);
  }

  private string BuildParaSpacing(WParagraphFormat pFormat, bool isInCell)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.HasValueWithParent(8) || pFormat.HasValueWithParent(8) && (double) pFormat.BeforeSpacing != 0.0)
      stringBuilder.Append(this.BuildSpacing(this.c_slashSymbol + "sb", pFormat.BeforeSpacing));
    else if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && this.m_doc.m_defParaFormat != null && this.m_doc.m_defParaFormat.HasValue(8) && !isInCell)
      stringBuilder.Append(this.BuildSpacing(this.c_slashSymbol + "sb", this.m_doc.m_defParaFormat.BeforeSpacing));
    if (pFormat.HasValueWithParent(9))
      stringBuilder.Append(this.BuildSpacing(this.c_slashSymbol + "sa", pFormat.AfterSpacing));
    else if (this.m_doc.m_defParaFormat != null && this.m_doc.m_defParaFormat.HasValue(9) && !isInCell)
      stringBuilder.Append(this.BuildSpacing(this.c_slashSymbol + "sa", this.m_doc.m_defParaFormat.AfterSpacing));
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.HasValueWithParent(54) || pFormat.HasValueWithParent(54) && pFormat.SpaceBeforeAuto)
      stringBuilder.Append(this.BuildAutoSpacing(this.c_slashSymbol + "sbauto", pFormat.SpaceBeforeAuto));
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.HasValueWithParent(55) || pFormat.HasValueWithParent(55) && pFormat.SpaceAfterAuto)
      stringBuilder.Append(this.BuildAutoSpacing(this.c_slashSymbol + "saauto", pFormat.SpaceAfterAuto));
    if (pFormat.HasValueWithParent(52))
      stringBuilder.Append(this.BuildLineSpacing(pFormat));
    else if (this.m_doc.m_defParaFormat != null && this.m_doc.m_defParaFormat.HasValue(52) && !isInCell)
      stringBuilder.Append(this.BuildLineSpacing(this.m_doc.m_defParaFormat));
    if (pFormat.ContextualSpacing)
      stringBuilder.Append(this.c_slashSymbol + "contextualspace");
    return stringBuilder.ToString();
  }

  private string BuildSpacing(string attribute, float value)
  {
    int num = (int) Math.Round((double) value * 20.0);
    return attribute + num.ToString();
  }

  private string BuildAutoSpacing(string value, bool hasSpacing)
  {
    return hasSpacing ? value + "1" : value + "0";
  }

  private string BuildLineSpacing(WParagraphFormat pFormat)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.c_slashSymbol + "sl");
    int num = (int) Math.Abs(Math.Round((double) pFormat.LineSpacing * 20.0));
    if (pFormat.LineSpacingRule == LineSpacingRule.Exactly)
      stringBuilder.Append("-" + (object) num);
    else
      stringBuilder.Append(num);
    switch (pFormat.LineSpacingRule)
    {
      case LineSpacingRule.AtLeast:
      case LineSpacingRule.Exactly:
        stringBuilder.Append(this.c_slashSymbol + "slmult0");
        break;
      case LineSpacingRule.Multiple:
        stringBuilder.Append(this.c_slashSymbol + "slmult1");
        break;
    }
    return stringBuilder.ToString();
  }

  private string BuildTextureStyle(TextureStyle style)
  {
    switch (style)
    {
      case TextureStyle.Texture5Percent:
        return this.c_slashSymbol + "shading500";
      case TextureStyle.Texture10Percent:
        return this.c_slashSymbol + "shading1000";
      case TextureStyle.Texture20Percent:
        return this.c_slashSymbol + "shading2000";
      case TextureStyle.Texture25Percent:
        return this.c_slashSymbol + "shading2500";
      case TextureStyle.Texture30Percent:
        return this.c_slashSymbol + "shading3000";
      case TextureStyle.Texture40Percent:
        return this.c_slashSymbol + "shading4000";
      case TextureStyle.Texture50Percent:
        return this.c_slashSymbol + "shading5000";
      case TextureStyle.Texture60Percent:
        return this.c_slashSymbol + "shading6000";
      case TextureStyle.Texture70Percent:
        return this.c_slashSymbol + "shading7000";
      case TextureStyle.Texture75Percent:
        return this.c_slashSymbol + "shading7500";
      case TextureStyle.Texture80Percent:
        return this.c_slashSymbol + "shading8000";
      case TextureStyle.Texture90Percent:
        return this.c_slashSymbol + "shading9000";
      case TextureStyle.TextureDarkHorizontal:
        return this.c_slashSymbol + "bgdkhoriz";
      case TextureStyle.TextureDarkVertical:
        return this.c_slashSymbol + "bgdkvert";
      case TextureStyle.TextureDarkDiagonalDown:
        return this.c_slashSymbol + "bgdkbdiag";
      case TextureStyle.TextureDarkDiagonalUp:
        return this.c_slashSymbol + "bgdkfdiag";
      case TextureStyle.TextureDarkCross:
        return this.c_slashSymbol + "bgdkcross";
      case TextureStyle.TextureDarkDiagonalCross:
        return this.c_slashSymbol + "bgdkdcross";
      case TextureStyle.TextureHorizontal:
        return this.c_slashSymbol + "bghoriz";
      case TextureStyle.TextureVertical:
        return this.c_slashSymbol + "bgvert";
      case TextureStyle.TextureDiagonalDown:
        return this.c_slashSymbol + "bgbdiag";
      case TextureStyle.TextureDiagonalUp:
        return this.c_slashSymbol + "bgfdiag";
      case TextureStyle.TextureCross:
        return this.c_slashSymbol + "bgcross";
      case TextureStyle.TextureDiagonalCross:
        return this.c_slashSymbol + "bgdcross";
      case TextureStyle.Texture2Pt5Percent:
        return this.c_slashSymbol + "shading250";
      case TextureStyle.Texture7Pt5Percent:
        return this.c_slashSymbol + "shading750";
      case TextureStyle.Texture12Pt5Percent:
        return this.c_slashSymbol + "shading1250";
      case TextureStyle.Texture15Percent:
        return this.c_slashSymbol + "shading1500";
      case TextureStyle.Texture17Pt5Percent:
        return this.c_slashSymbol + "shading1750";
      case TextureStyle.Texture27Pt5Percent:
        return this.c_slashSymbol + "shading2750";
      case TextureStyle.Texture32Pt5Percent:
        return this.c_slashSymbol + "shading3250";
      case TextureStyle.Texture35Percent:
        return this.c_slashSymbol + "shading3500";
      case TextureStyle.Texture37Pt5Percent:
        return this.c_slashSymbol + "shading3750";
      case TextureStyle.Texture42Pt5Percent:
        return this.c_slashSymbol + "shading4250";
      case TextureStyle.Texture45Percent:
        return this.c_slashSymbol + "shading4500";
      case TextureStyle.Texture47Pt5Percent:
        return this.c_slashSymbol + "shading4750";
      case TextureStyle.Texture52Pt5Percent:
        return this.c_slashSymbol + "shading5250";
      case TextureStyle.Texture55Percent:
        return this.c_slashSymbol + "shading5500";
      case TextureStyle.Texture57Pt5Percent:
        return this.c_slashSymbol + "shading5750";
      case TextureStyle.Texture62Pt5Percent:
        return this.c_slashSymbol + "shading6250";
      case TextureStyle.Texture65Percent:
        return this.c_slashSymbol + "shading6500";
      case TextureStyle.Texture67Pt5Percent:
        return this.c_slashSymbol + "shading6750";
      case TextureStyle.Texture72Pt5Percent:
        return this.c_slashSymbol + "shading7250";
      case TextureStyle.Texture77Pt5Percent:
        return this.c_slashSymbol + "shading7750";
      case TextureStyle.Texture82Pt5Percent:
        return this.c_slashSymbol + "shading8250";
      case TextureStyle.Texture85Percent:
        return this.c_slashSymbol + "shading8500";
      case TextureStyle.Texture87Pt5Percent:
        return this.c_slashSymbol + "shading8750";
      case TextureStyle.Texture92Pt5Percent:
        return this.c_slashSymbol + "shading9250";
      case TextureStyle.Texture95Percent:
        return this.c_slashSymbol + "shading9500";
      case TextureStyle.Texture97Pt5Percent:
        return this.c_slashSymbol + "shading9750";
      default:
        return string.Empty;
    }
  }

  private void BuildSectionProp(WSection section)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes(this.c_slashSymbol + "sectd");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    switch (section.BreakCode)
    {
      case SectionBreakCode.NoBreak:
        byte[] bytes2 = this.m_encoding.GetBytes(this.c_slashSymbol + "sbknone");
        memoryStream.Write(bytes2, 0, bytes2.Length);
        break;
      case SectionBreakCode.NewColumn:
        byte[] bytes3 = this.m_encoding.GetBytes(this.c_slashSymbol + "sbkcol");
        memoryStream.Write(bytes3, 0, bytes3.Length);
        break;
      case SectionBreakCode.EvenPage:
        byte[] bytes4 = this.m_encoding.GetBytes(this.c_slashSymbol + "sbkeven");
        memoryStream.Write(bytes4, 0, bytes4.Length);
        break;
      case SectionBreakCode.Oddpage:
        byte[] bytes5 = this.m_encoding.GetBytes(this.c_slashSymbol + "sbkodd");
        memoryStream.Write(bytes5, 0, bytes5.Length);
        break;
    }
    if (section.PageSetup.Orientation == PageOrientation.Landscape)
    {
      byte[] bytes6 = this.m_encoding.GetBytes(this.c_slashSymbol + "lndscpsxn");
      memoryStream.Write(bytes6, 0, bytes6.Length);
    }
    if (section.TextDirection == DocTextDirection.LeftToRight)
    {
      byte[] bytes7 = this.m_encoding.GetBytes(this.c_slashSymbol + "ltrsect");
      memoryStream.Write(bytes7, 0, bytes7.Length);
    }
    else if (section.TextDirection == DocTextDirection.RightToLeft)
    {
      byte[] bytes8 = this.m_encoding.GetBytes(this.c_slashSymbol + "rtlsect");
      memoryStream.Write(bytes8, 0, bytes8.Length);
    }
    if (section.PageSetup.FirstPageTray > PrinterPaperTray.DefaultBin)
    {
      byte[] bytes9 = this.m_encoding.GetBytes($"{this.c_slashSymbol}binfsxn{(object) section.PageSetup.FirstPageTray}");
      memoryStream.Write(bytes9, 0, bytes9.Length);
    }
    if (section.PageSetup.OtherPagesTray > PrinterPaperTray.DefaultBin)
    {
      byte[] bytes10 = this.m_encoding.GetBytes($"{this.c_slashSymbol}binsxn{(object) section.PageSetup.OtherPagesTray}");
      memoryStream.Write(bytes10, 0, bytes10.Length);
    }
    byte[] bytes11 = this.m_encoding.GetBytes(this.c_slashSymbol + "nofeaturethrottle1");
    memoryStream.Write(bytes11, 0, bytes11.Length);
    byte[] bytes12 = this.m_encoding.GetBytes(this.c_slashSymbol + "formshade");
    memoryStream.Write(bytes12, 0, bytes12.Length);
    byte[] bytes13 = this.m_encoding.GetBytes(this.c_slashSymbol + "splytwnine");
    memoryStream.Write(bytes13, 0, bytes13.Length);
    byte[] buffer = this.BuildPageSetup(section.PageSetup);
    memoryStream.Write(buffer, 0, buffer.Length);
    byte[] bytes14 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes14, 0, bytes14.Length);
    this.m_mainBodyBytesList.Add(memoryStream.ToArray());
  }

  private byte[] BuildPageSetup(WPageSetup pSetup)
  {
    MemoryStream memoryStream = new MemoryStream();
    if ((double) pSetup.HeaderDistance >= 0.0)
    {
      byte[] bytes = this.m_encoding.GetBytes($"{this.c_slashSymbol}headery{(object) (int) Math.Round((double) pSetup.HeaderDistance * 20.0)}");
      memoryStream.Write(bytes, 0, bytes.Length);
    }
    if ((double) pSetup.FooterDistance >= 0.0)
    {
      byte[] bytes = this.m_encoding.GetBytes($"{this.c_slashSymbol}footery{(object) (int) Math.Round((double) pSetup.FooterDistance * 20.0)}");
      memoryStream.Write(bytes, 0, bytes.Length);
    }
    switch (pSetup.VerticalAlignment)
    {
      case PageAlignment.Top:
        byte[] bytes1 = this.m_encoding.GetBytes(this.c_slashSymbol + "vertalt");
        memoryStream.Write(bytes1, 0, bytes1.Length);
        break;
      case PageAlignment.Middle:
        byte[] bytes2 = this.m_encoding.GetBytes(this.c_slashSymbol + "vertalc");
        memoryStream.Write(bytes2, 0, bytes2.Length);
        break;
      case PageAlignment.Justified:
        byte[] bytes3 = this.m_encoding.GetBytes(this.c_slashSymbol + "vertalj");
        memoryStream.Write(bytes3, 0, bytes3.Length);
        break;
      case PageAlignment.Bottom:
        byte[] bytes4 = this.m_encoding.GetBytes(this.c_slashSymbol + "vertalb");
        memoryStream.Write(bytes4, 0, bytes4.Length);
        break;
    }
    if (pSetup.DifferentFirstPage)
    {
      byte[] bytes5 = this.m_encoding.GetBytes(this.c_slashSymbol + "titlepg");
      memoryStream.Write(bytes5, 0, bytes5.Length);
    }
    if (this.m_doc.DifferentOddAndEvenPages)
    {
      byte[] bytes6 = this.m_encoding.GetBytes(this.c_slashSymbol + "facingp");
      memoryStream.Write(bytes6, 0, bytes6.Length);
    }
    byte[] bytes7 = this.m_encoding.GetBytes($"{this.c_slashSymbol}paperw{(object) (int) Math.Round((double) pSetup.PageSize.Width * 20.0)}");
    memoryStream.Write(bytes7, 0, bytes7.Length);
    byte[] bytes8 = this.m_encoding.GetBytes($"{this.c_slashSymbol}paperh{(object) (int) Math.Round((double) pSetup.PageSize.Height * 20.0)}");
    memoryStream.Write(bytes8, 0, bytes8.Length);
    byte[] bytes9 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margl{(object) (int) Math.Round((double) pSetup.Margins.Left * 20.0)}");
    memoryStream.Write(bytes9, 0, bytes9.Length);
    byte[] bytes10 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margr{(object) (int) Math.Round((double) pSetup.Margins.Right * 20.0)}");
    memoryStream.Write(bytes10, 0, bytes10.Length);
    byte[] bytes11 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margt{(object) (int) Math.Round((double) pSetup.Margins.Top * 20.0)}");
    memoryStream.Write(bytes11, 0, bytes11.Length);
    byte[] bytes12 = this.m_encoding.GetBytes($"{this.c_slashSymbol}margb{(object) (int) Math.Round((double) pSetup.Margins.Bottom * 20.0)}");
    memoryStream.Write(bytes12, 0, bytes12.Length);
    byte[] bytes13 = this.m_encoding.GetBytes($"{this.c_slashSymbol}gutter{(object) (int) Math.Round((double) pSetup.Margins.Gutter * 20.0)}");
    memoryStream.Write(bytes13, 0, bytes13.Length);
    byte[] bytes14 = this.m_encoding.GetBytes($"{this.c_slashSymbol}deftab{(object) (int) Math.Round((double) this.m_doc.DefaultTabWidth * 20.0)}");
    memoryStream.Write(bytes14, 0, bytes14.Length);
    if (this.m_doc.DOP.GutterAtTop)
    {
      byte[] bytes15 = this.m_encoding.GetBytes(this.c_slashSymbol + "gutterprl");
      memoryStream.Write(bytes15, 0, bytes15.Length);
    }
    switch (this.m_doc.MultiplePage)
    {
      case MultiplePage.MirrorMargins:
        byte[] bytes16 = this.m_encoding.GetBytes(this.c_slashSymbol + "margmirror");
        memoryStream.Write(bytes16, 0, bytes16.Length);
        break;
      case MultiplePage.TwoPagesPerSheet:
        byte[] bytes17 = this.m_encoding.GetBytes(this.c_slashSymbol + "twoonone");
        memoryStream.Write(bytes17, 0, bytes17.Length);
        break;
      case MultiplePage.BookFold:
        byte[] bytes18 = this.m_encoding.GetBytes(this.c_slashSymbol + "bookfold");
        memoryStream.Write(bytes18, 0, bytes18.Length);
        break;
      case MultiplePage.ReverseBookFold:
        byte[] bytes19 = this.m_encoding.GetBytes(this.c_slashSymbol + "bookfoldrev");
        memoryStream.Write(bytes19, 0, bytes19.Length);
        break;
    }
    if (this.m_doc.SheetsPerBooklet != 0)
    {
      byte[] bytes20 = this.m_encoding.GetBytes($"{this.c_slashSymbol}bookfoldsheets{(object) this.m_doc.SheetsPerBooklet}");
      memoryStream.Write(bytes20, 0, bytes20.Length);
    }
    if (pSetup.RestartPageNumbering)
    {
      byte[] bytes21 = this.m_encoding.GetBytes(this.c_slashSymbol + "pgnrestart");
      memoryStream.Write(bytes21, 0, bytes21.Length);
      byte[] bytes22 = this.m_encoding.GetBytes($"{this.c_slashSymbol}pgnstarts{(object) pSetup.PageStartingNumber}");
      memoryStream.Write(bytes22, 0, bytes22.Length);
      byte[] bytes23 = this.m_encoding.GetBytes(this.BuildPageNumStyle(pSetup.PageNumberStyle));
      memoryStream.Write(bytes23, 0, bytes23.Length);
    }
    byte[] bytes24 = this.m_encoding.GetBytes(this.c_slashSymbol + "pgncont");
    memoryStream.Write(bytes24, 0, bytes24.Length);
    if (pSetup.LineNumberingMode != LineNumberingMode.None || pSetup.LineNumberingStep > 0)
    {
      byte[] bytes25 = this.m_encoding.GetBytes($"{this.c_slashSymbol}linemod{(object) pSetup.LineNumberingStep}");
      memoryStream.Write(bytes25, 0, bytes25.Length);
      byte[] bytes26 = this.m_encoding.GetBytes($"{this.c_slashSymbol}linex{(object) pSetup.LineNumberingDistanceFromText}");
      memoryStream.Write(bytes26, 0, bytes26.Length);
      byte[] bytes27 = this.m_encoding.GetBytes($"{this.c_slashSymbol}linestarts{(object) pSetup.LineNumberingStartValue}");
      memoryStream.Write(bytes27, 0, bytes27.Length);
      switch (pSetup.LineNumberingMode)
      {
        case LineNumberingMode.RestartPage:
          byte[] bytes28 = this.m_encoding.GetBytes(this.c_slashSymbol + "lineppage");
          memoryStream.Write(bytes28, 0, bytes28.Length);
          break;
        case LineNumberingMode.RestartSection:
          byte[] bytes29 = this.m_encoding.GetBytes(this.c_slashSymbol + "linerestart");
          memoryStream.Write(bytes29, 0, bytes29.Length);
          break;
        case LineNumberingMode.Continuous:
          byte[] bytes30 = this.m_encoding.GetBytes(this.c_slashSymbol + "linecont");
          memoryStream.Write(bytes30, 0, bytes30.Length);
          break;
      }
    }
    byte[] bytes31 = this.m_encoding.GetBytes($"{this.c_slashSymbol}sectlinegrid{(object) (int) Math.Round((double) pSetup.LinePitch * 20.0)}");
    memoryStream.Write(bytes31, 0, bytes31.Length);
    if (pSetup.PitchType == GridPitchType.LinesOnly)
    {
      byte[] bytes32 = this.m_encoding.GetBytes(this.c_slashSymbol + "sectspecifyl");
      memoryStream.Write(bytes32, 0, bytes32.Length);
    }
    byte[] bytes33 = this.m_encoding.GetBytes(this.BuildPageBorders(pSetup.Borders));
    memoryStream.Write(bytes33, 0, bytes33.Length);
    byte[] buffer = this.BuildColumns((pSetup.OwnerBase as WSection).Columns);
    memoryStream.Write(buffer, 0, buffer.Length);
    return memoryStream.ToArray();
  }

  private string BuildPageNumStyle(PageNumberStyle pageNumSt)
  {
    switch (pageNumSt)
    {
      case PageNumberStyle.RomanUpper:
        return this.c_slashSymbol + "pgnucrm";
      case PageNumberStyle.RomanLower:
        return this.c_slashSymbol + "pgnlcrm";
      case PageNumberStyle.LetterUpper:
        return this.c_slashSymbol + "pgnucltr";
      case PageNumberStyle.LetterLower:
        return this.c_slashSymbol + "pgnlcltr";
      default:
        return this.c_slashSymbol + "pgndec";
    }
  }

  private byte[] BuildColumns(ColumnCollection cols)
  {
    MemoryStream memoryStream = new MemoryStream();
    StringBuilder stringBuilder = new StringBuilder();
    WPageSetup pageSetup = cols.OwnerSection.PageSetup;
    byte[] bytes1 = this.m_encoding.GetBytes($"{this.c_slashSymbol}cols{(object) cols.Count}");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    if (!pageSetup.EqualColumnWidth && cols.Count != 1)
    {
      int index = 0;
      for (int count = cols.Count; index < count; ++index)
      {
        Column col = cols[index];
        byte[] bytes2 = this.m_encoding.GetBytes($"{this.c_slashSymbol}colno{(object) (index + 1)}");
        memoryStream.Write(bytes2, 0, bytes2.Length);
        byte[] bytes3 = this.m_encoding.GetBytes($"{this.c_slashSymbol}colw{(object) (int) Math.Round((double) col.Width * 20.0)}");
        memoryStream.Write(bytes3, 0, bytes3.Length);
        byte[] bytes4 = this.m_encoding.GetBytes($"{this.c_slashSymbol}colsr{(object) (int) Math.Round((double) col.Space * 20.0)}");
        memoryStream.Write(bytes4, 0, bytes4.Length);
      }
    }
    if (pageSetup.DrawLinesBetweenCols)
    {
      byte[] bytes5 = this.m_encoding.GetBytes(this.c_slashSymbol + "linebetcol");
      memoryStream.Write(bytes5, 0, bytes5.Length);
    }
    return memoryStream.ToArray();
  }

  private void BuildUnderLineStyle(UnderlineStyle style, StringBuilder strBuilder)
  {
    switch (style)
    {
      case UnderlineStyle.None:
        strBuilder.Append(this.c_slashSymbol + "ulnone");
        break;
      case UnderlineStyle.Single:
        strBuilder.Append(this.c_slashSymbol + "ul");
        break;
      case UnderlineStyle.Words:
        strBuilder.Append(this.c_slashSymbol + "ulw");
        break;
      case UnderlineStyle.Double:
        strBuilder.Append(this.c_slashSymbol + "uldb");
        break;
      case UnderlineStyle.Dotted:
        strBuilder.Append(this.c_slashSymbol + "uld");
        break;
      case UnderlineStyle.Thick:
        strBuilder.Append(this.c_slashSymbol + "ulth");
        break;
      case UnderlineStyle.Dash:
        strBuilder.Append(this.c_slashSymbol + "uldash");
        break;
      case UnderlineStyle.Wavy:
        strBuilder.Append(this.c_slashSymbol + "ulwave");
        break;
      case UnderlineStyle.WavyHeavy:
        strBuilder.Append(this.c_slashSymbol + "ulhwave");
        break;
      case UnderlineStyle.DashLong:
        strBuilder.Append(this.c_slashSymbol + "ulldash");
        break;
      case UnderlineStyle.WavyDouble:
        strBuilder.Append(this.c_slashSymbol + "ululdbwave");
        break;
    }
  }

  private string BuildTabs(TabCollection tabs)
  {
    if (tabs == null)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    if (tabs.Count > 0)
    {
      int index = 0;
      for (int count = tabs.Count; index < count; ++index)
      {
        Tab tab = tabs[index];
        switch (tab.Justification)
        {
          case TabJustification.Centered:
            stringBuilder.Append(this.c_slashSymbol + "tqc");
            break;
          case TabJustification.Right:
            stringBuilder.Append(this.c_slashSymbol + "tqr");
            break;
          case TabJustification.Decimal:
            stringBuilder.Append(this.c_slashSymbol + "tqdec");
            break;
        }
        if (tab.TabLeader != TabLeader.NoLeader)
        {
          switch (tab.TabLeader)
          {
            case TabLeader.Dotted:
              stringBuilder.Append(this.c_slashSymbol + "tldot");
              break;
            case TabLeader.Hyphenated:
              stringBuilder.Append(this.c_slashSymbol + "tlhyph");
              break;
            case TabLeader.Single:
              stringBuilder.Append(this.c_slashSymbol + "tlth");
              break;
            case TabLeader.Heavy:
              stringBuilder.Append(this.c_slashSymbol + "tleq");
              break;
          }
        }
        stringBuilder.Append(this.c_slashSymbol + "tx");
        stringBuilder.Append(tab.Position * 20f);
      }
    }
    return stringBuilder.ToString();
  }

  private string BuildParaBorders(WParagraphFormat pFormat)
  {
    Borders borders = pFormat.Borders;
    StringBuilder stringBuilder = new StringBuilder();
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.Borders.Top.IsBorderDefined || pFormat.Borders.Top.IsBorderDefined && pFormat.Borders.Top.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "brdrt");
      stringBuilder.Append(this.BuildBorder(borders.Top, false));
    }
    if (pFormat.Borders.Top.Shadow)
      stringBuilder.Append(this.c_slashSymbol + "brdrsh");
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.Borders.Left.IsBorderDefined || pFormat.Borders.Left.IsBorderDefined && pFormat.Borders.Left.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "brdrl");
      stringBuilder.Append(this.BuildBorder(borders.Left, false));
    }
    if (pFormat.Borders.Left.Shadow)
      stringBuilder.Append(this.c_slashSymbol + "brdrsh");
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.Borders.Bottom.IsBorderDefined || pFormat.Borders.Bottom.IsBorderDefined && pFormat.Borders.Bottom.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "brdrb");
      stringBuilder.Append(this.BuildBorder(borders.Bottom, false));
    }
    if (pFormat.Borders.Bottom.Shadow)
      stringBuilder.Append(this.c_slashSymbol + "brdrsh");
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && pFormat.Borders.Right.IsBorderDefined || pFormat.Borders.Right.IsBorderDefined && pFormat.Borders.Right.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "brdrr");
      stringBuilder.Append(this.BuildBorder(borders.Right, false));
    }
    if (pFormat.Borders.Right.Shadow)
      stringBuilder.Append(this.c_slashSymbol + "brdrsh");
    return stringBuilder.ToString();
  }

  private string BuildPageBorders(Borders borders)
  {
    if (borders.NoBorder)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.c_slashSymbol + "pgbrdropt32");
    stringBuilder.Append(this.c_slashSymbol + "pgbrdrt");
    stringBuilder.Append(this.BuildBorder(borders.Top, false));
    stringBuilder.Append(this.c_slashSymbol + "pgbrdrb");
    stringBuilder.Append(this.BuildBorder(borders.Bottom, false));
    stringBuilder.Append(this.c_slashSymbol + "pgbrdrl");
    stringBuilder.Append(this.BuildBorder(borders.Left, false));
    stringBuilder.Append(this.c_slashSymbol + "pgbrdrr");
    stringBuilder.Append(this.BuildBorder(borders.Right, false));
    return stringBuilder.ToString();
  }

  private string BuildBorder(Border border, bool isTable)
  {
    BorderStyle borderType = border.BorderType;
    int num1 = (int) (border.LineWidth * 20f);
    float num2 = border.Space * 20f;
    if (borderType == BorderStyle.None && !border.HasNoneStyle)
      return this.BuildColor(Color.Black, $"{this.c_slashSymbol}brdrs{this.c_slashSymbol}brdrw10{this.c_slashSymbol}brdrcf");
    StringBuilder stringBuilder = new StringBuilder();
    if (borderType != BorderStyle.None && borderType != BorderStyle.Cleared)
    {
      stringBuilder.Append(this.BuildBorderStyle(borderType));
      stringBuilder.Append(this.c_slashSymbol + "brdrw");
      stringBuilder.Append(num1);
      stringBuilder.Append(this.BuildColor(border.Color, this.c_slashSymbol + "brdrcf"));
    }
    if ((double) num2 > 0.0)
    {
      stringBuilder.Append(this.c_slashSymbol + "brsp");
      stringBuilder.Append(num2);
    }
    return stringBuilder.ToString();
  }

  private string BuildBorderStyle(BorderStyle borderStyle)
  {
    switch (borderStyle)
    {
      case BorderStyle.None:
        return this.c_slashSymbol + "brdrnone";
      case BorderStyle.Thick:
        return this.c_slashSymbol + "brdrth";
      case BorderStyle.Double:
        return this.c_slashSymbol + "brdrdb";
      case BorderStyle.Hairline:
        return this.c_slashSymbol + "brdrhair";
      case BorderStyle.Dot:
        return this.c_slashSymbol + "brdrdot";
      case BorderStyle.DashLargeGap:
        return this.c_slashSymbol + "brdrdash";
      case BorderStyle.DotDash:
        return this.c_slashSymbol + "brdrdashd";
      case BorderStyle.DotDotDash:
        return this.c_slashSymbol + "brdrdashdd";
      case BorderStyle.Triple:
        return this.c_slashSymbol + "brdrtriple";
      case BorderStyle.ThinThickSmallGap:
        return this.c_slashSymbol + "brdrthtnsg";
      case BorderStyle.ThinThinSmallGap:
        return this.c_slashSymbol + "brdrtnthsg";
      case BorderStyle.ThinThickThinSmallGap:
        return this.c_slashSymbol + "brdrtnthtnsg";
      case BorderStyle.ThinThickMediumGap:
        return this.c_slashSymbol + "brdrthtnmg";
      case BorderStyle.ThickThinMediumGap:
        return this.c_slashSymbol + "brdrtnthmg";
      case BorderStyle.ThickThickThinMediumGap:
        return this.c_slashSymbol + "brdrtnthtnmg";
      case BorderStyle.ThinThickLargeGap:
        return this.c_slashSymbol + "brdrthtnlg";
      case BorderStyle.ThickThinLargeGap:
        return this.c_slashSymbol + "brdrtnthlg";
      case BorderStyle.ThinThickThinLargeGap:
        return this.c_slashSymbol + "brdrtnthtnlg";
      case BorderStyle.Wave:
        return this.c_slashSymbol + "brdrwavy";
      case BorderStyle.DoubleWave:
        return this.c_slashSymbol + "brdrwavydb";
      case BorderStyle.DashSmallGap:
        return this.c_slashSymbol + "brdrdashsm";
      case BorderStyle.DashDotStroker:
        return this.c_slashSymbol + "brdrdashdotstr";
      case BorderStyle.Emboss3D:
        return this.c_slashSymbol + "brdremboss";
      case BorderStyle.Engrave3D:
        return this.c_slashSymbol + "brdrengrave";
      case BorderStyle.Outset:
        return this.c_slashSymbol + "brdroutset";
      case BorderStyle.Inset:
        return this.c_slashSymbol + "brdrinset";
      default:
        return this.c_slashSymbol + "brdrs";
    }
  }

  private void BuildStyleSheet()
  {
    int num = 1;
    foreach (Style style in (IEnumerable) this.m_doc.Styles)
    {
      if (!this.StyleNumb.ContainsKey(style.Name))
      {
        this.StyleNumb.Add(style.Name, num.ToString());
        ++num;
      }
    }
    foreach (Style style in (IEnumerable) this.m_doc.Styles)
      this.BuildStyle(style);
  }

  private void BuildStyle(Style style)
  {
    if (this.Styles.ContainsKey(style.Name))
      return;
    MemoryStream memoryStream = new MemoryStream();
    memoryStream.Write(this.m_styleBytes, 0, this.m_styleBytes.Length);
    string s = string.Empty;
    if (style.StyleType == StyleType.ParagraphStyle)
    {
      if (this.StyleNumb.ContainsKey(style.Name))
        s = $"{this.c_slashSymbol}s{this.StyleNumb[style.Name]}";
    }
    else if (style.StyleType == StyleType.CharacterStyle && this.StyleNumb.ContainsKey(style.Name))
      s = $"{this.c_slashSymbol}cs{this.StyleNumb[style.Name]}";
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(s);
    memoryStream.Write(bytes2, 0, bytes2.Length);
    if (style.StyleType == StyleType.ParagraphStyle)
    {
      byte[] bytes3 = this.m_encoding.GetBytes(this.BuildParagraphFormat((style as WParagraphStyle).ParagraphFormat, (WParagraph) null, false));
      memoryStream.Write(bytes3, 0, bytes3.Length);
    }
    if (style.StyleType == StyleType.CharacterStyle)
    {
      byte[] bytes4 = this.m_encoding.GetBytes(this.c_slashSymbol + "additive");
      memoryStream.Write(bytes4, 0, bytes4.Length);
    }
    byte[] bytes5 = this.m_encoding.GetBytes(this.BuildCharacterFormat(style.CharacterFormat));
    memoryStream.Write(bytes5, 0, bytes5.Length);
    if (style.BaseStyle != null && !string.IsNullOrEmpty(style.BaseStyle.Name) && this.StyleNumb.ContainsKey(style.BaseStyle.Name))
    {
      byte[] bytes6 = this.m_encoding.GetBytes($"{this.c_slashSymbol}sbasedon{this.StyleNumb[style.BaseStyle.Name]}");
      memoryStream.Write(bytes6, 0, bytes6.Length);
    }
    if (!string.IsNullOrEmpty(style.LinkStyle) && this.StyleNumb.ContainsKey(style.LinkStyle))
    {
      byte[] bytes7 = this.m_encoding.GetBytes($"{this.c_slashSymbol}slink{this.StyleNumb[style.LinkStyle]}");
      memoryStream.Write(bytes7, 0, bytes7.Length);
    }
    byte[] bytes8 = this.m_encoding.GetBytes(this.c_slashSymbol + "sqformat");
    memoryStream.Write(bytes8, 0, bytes8.Length);
    string str = this.PrepareText(style.Name);
    if (this.m_isCyrillicText)
    {
      byte[] bytes9 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}falt {str}}}");
      memoryStream.Write(bytes9, 0, bytes9.Length);
      this.m_isCyrillicText = false;
    }
    else
    {
      byte[] bytes10 = this.m_encoding.GetBytes(" " + str);
      memoryStream.Write(bytes10, 0, bytes10.Length);
    }
    byte[] bytes11 = this.m_encoding.GetBytes(";}");
    memoryStream.Write(bytes11, 0, bytes11.Length);
    this.m_styleBytes = memoryStream.ToArray();
    this.Styles.Add(style.Name, s);
  }

  private void BuildStyle(string styleName)
  {
    if (!(this.m_doc.Styles.FindByName(styleName) is Style byName))
      return;
    this.BuildStyle(byName);
  }

  private string BuildTextBorder(Border brd)
  {
    if (brd == null || brd.BorderType == BorderStyle.None && !brd.HasNoneStyle)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.c_slashSymbol + "chbrdr");
    stringBuilder.Append(this.BuildBorder(brd, false));
    return stringBuilder.ToString();
  }

  private string BuildFrameProps(WParagraphFormat pFormat)
  {
    if (!pFormat.IsFrame)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    switch (pFormat.FrameHorizontalPos)
    {
      case 0:
        stringBuilder.Append(this.c_slashSymbol + "phcol");
        break;
      case 1:
        stringBuilder.Append(this.c_slashSymbol + "phmrg");
        break;
      case 2:
        stringBuilder.Append(this.c_slashSymbol + "phpg");
        break;
    }
    switch (pFormat.FrameVerticalPos)
    {
      case 0:
        stringBuilder.Append(this.c_slashSymbol + "pvmrg");
        break;
      case 1:
        stringBuilder.Append(this.c_slashSymbol + "pvpg");
        break;
      case 2:
        stringBuilder.Append(this.c_slashSymbol + "pvpara");
        break;
    }
    if ((double) pFormat.FrameX < 0.0)
    {
      switch ((short) pFormat.FrameX)
      {
        case -16:
          stringBuilder.Append(this.c_slashSymbol + "posxo");
          break;
        case -12:
          stringBuilder.Append(this.c_slashSymbol + "posxi");
          break;
        case -8:
          stringBuilder.Append(this.c_slashSymbol + "posxr");
          break;
        case -4:
          stringBuilder.Append(this.c_slashSymbol + "posxc");
          break;
        case 0:
          stringBuilder.Append(this.c_slashSymbol + "posxl");
          break;
      }
    }
    return stringBuilder.ToString();
  }

  private string BuildParaListId(WParagraph para, WParagraphFormat pFormat)
  {
    if (pFormat.OwnerBase == null)
      return string.Empty;
    WParagraphStyle wparagraphStyle = (WParagraphStyle) null;
    int key = -1;
    if (pFormat.OwnerBase is WParagraphStyle)
      wparagraphStyle = pFormat.OwnerBase as WParagraphStyle;
    else if (pFormat.OwnerBase is WParagraph)
      wparagraphStyle = this.m_doc.Styles.FindByName((pFormat.OwnerBase as WParagraph).StyleName) as WParagraphStyle;
    string str = string.Empty;
    if (para != null && para.ListFormat.IsEmptyList)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    if (para != null && para.ListFormat.ListType != ListType.NoList && !string.IsNullOrEmpty(para.ListFormat.CurrentListStyle.Name))
    {
      key = this.ListsIds[para.ListFormat.CurrentListStyle.Name] + 1;
      str = para.ListFormat.LFOStyleName;
    }
    else if (wparagraphStyle != null && wparagraphStyle.ListFormat != null && wparagraphStyle.ListFormat.CurrentListStyle != null && !string.IsNullOrEmpty(wparagraphStyle.ListFormat.CurrentListStyle.Name))
    {
      key = this.ListsIds[wparagraphStyle.ListFormat.CurrentListStyle.Name] + 1;
      str = wparagraphStyle.ListFormat.LFOStyleName;
    }
    if (key != -1)
    {
      stringBuilder.Append(this.c_slashSymbol + "ls");
      stringBuilder.Append(key);
      if (!this.ListOverrideAr.ContainsKey(key))
        this.ListOverrideAr.Add(key, str);
      else if (string.IsNullOrEmpty(this.ListOverrideAr[key]) && !string.IsNullOrEmpty(str))
        this.ListOverrideAr[key] = str;
      if (para != null)
      {
        stringBuilder.Append(this.c_slashSymbol + "ilvl");
        stringBuilder.Append(para.ListFormat.ListLevelNumber);
      }
    }
    return stringBuilder.ToString();
  }

  private byte[] BuildTable(WTable table)
  {
    MemoryStream memoryStream = new MemoryStream();
    ++this.m_tableNestedLevel;
    table.ApplyBaseStyleFormats();
    table.IsUpdateCellWidthByPartitioning = true;
    table.UpdateGridSpan();
    if (!table.IsUpdateCellWidthByPartitioning)
      table.UpdateUnDefinedCellWidth();
    int index1 = 0;
    for (int count = table.Rows.Count; index1 < count; ++index1)
    {
      byte[] buffer = this.BuildTableRow(table.Rows[index1]);
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    int index2 = 0;
    for (int count = table.Rows.Count; index2 < count; ++index2)
    {
      WTableRow row = table.Rows[index2];
      if (row != null)
      {
        for (int index3 = 0; index3 < row.Cells.Count; ++index3)
          row.Cells[index3].InitCellLayoutInfo();
      }
    }
    --this.m_tableNestedLevel;
    return memoryStream.ToArray();
  }

  private byte[] BuildTableRow(WTableRow row)
  {
    MemoryStream memoryStream = new MemoryStream();
    string s = this.BuildTRowFormat(row.RowFormat);
    if (this.m_tableNestedLevel == 1)
    {
      byte[] bytes = this.m_encoding.GetBytes(s);
      memoryStream.Write(bytes, 0, bytes.Length);
    }
    int index = 0;
    for (int count = row.Cells.Count; index < count; ++index)
    {
      WTableCell cell = row.Cells[index];
      if (cell.CellFormat.HorizontalMerge != CellMerge.Continue)
      {
        byte[] buffer = this.BuildTableCell(cell);
        memoryStream.Write(buffer, 0, buffer.Length);
      }
    }
    if (this.m_tableNestedLevel != 1)
      s = $"{{{this.c_slashSymbol}*{this.c_slashSymbol}nesttableprops{s}{this.c_slashSymbol}nestrow}}";
    byte[] bytes1 = this.m_encoding.GetBytes(s);
    memoryStream.Write(bytes1, 0, bytes1.Length);
    if (row.OwnerTable != null && row.OwnerTable.Owner != null && row.OwnerTable.Owner.OwnerBase != null && row.OwnerTable.Owner.OwnerBase is WTableRow)
    {
      byte[] bytes2 = this.m_encoding.GetBytes(this.BuildTRowFormat((row.OwnerTable.Owner.OwnerBase as WTableRow).RowFormat));
      memoryStream.Write(bytes2, 0, bytes2.Length);
    }
    else
    {
      byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}row}}");
      memoryStream.Write(bytes3, 0, bytes3.Length);
    }
    return memoryStream.ToArray();
  }

  private string BuildTRowFormat(RowFormat rowFormat)
  {
    StringBuilder stringBuilder = new StringBuilder();
    WTableRow ownerRow = rowFormat.OwnerRow;
    stringBuilder.Append(this.c_slashSymbol + "trowd");
    if (ownerRow != null && ownerRow.OwnerTable != null && ownerRow == ownerRow.OwnerTable.LastRow)
      stringBuilder.Append(this.c_slashSymbol + "lastrow");
    if (ownerRow.IsHeader)
      stringBuilder.Append(this.c_slashSymbol + "trhdr");
    if (!ownerRow.RowFormat.IsBreakAcrossPages)
      stringBuilder.Append(this.c_slashSymbol + "trkeep");
    if (ownerRow.RowFormat.Bidi)
      stringBuilder.Append(this.c_slashSymbol + "rtlrow");
    float num1 = 0.0f;
    if (ownerRow.OwnerTable.FirstRow.Cells.Count > 0)
    {
      WTableCell cell = ownerRow.OwnerTable.FirstRow.Cells[0];
      num1 = cell.CellFormat.Paddings.Left;
      if (cell.CellFormat.SamePaddingsAsTable)
        num1 = ownerRow.OwnerTable.TableFormat.Paddings.Left;
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
    {
      stringBuilder.Append($"{this.c_slashSymbol}tblind{Math.Round((double) rowFormat.LeftIndent * 20.0).ToString()}");
      stringBuilder.Append(this.c_slashSymbol + "tblindtype3");
    }
    if ((double) rowFormat.LeftIndent != 0.0 && !ownerRow.OwnerTable.TableFormat.WrapTextAround && ownerRow.OwnerTable.TableFormat.HorizontalAlignment == RowAlignment.Left)
    {
      int num2 = (int) Math.Round(((double) rowFormat.LeftIndent - (double) num1) * 20.0) - ((double) num1 != 0.0 ? 0 : 5);
      stringBuilder.Append($"{this.c_slashSymbol}trleft{num2.ToString()}");
      this.m_cellEndPos = num2;
    }
    else
    {
      int num3 = (int) Math.Round((double) -num1 * 20.0);
      int num4 = (double) num1 != 0.0 ? num3 : -5;
      stringBuilder.Append($"{this.c_slashSymbol}trleft{num4.ToString()}");
      this.m_cellEndPos = num4;
    }
    if (rowFormat.Positioning.VertRelationTo == VerticalRelation.Paragraph)
      stringBuilder.Append(this.c_slashSymbol + "tpvpara");
    else if (rowFormat.Positioning.VertRelationTo == VerticalRelation.Page)
      stringBuilder.Append(this.c_slashSymbol + "tpvpg");
    if (rowFormat.Positioning.HorizRelationTo == HorizontalRelation.Page)
      stringBuilder.Append(this.c_slashSymbol + "tphpg");
    else if (rowFormat.Positioning.HorizRelationTo == HorizontalRelation.Margin)
      stringBuilder.Append(this.c_slashSymbol + "tphmrg");
    RowFormat.TablePositioning tablePositioning = rowFormat.WrapTextAround ? rowFormat.Positioning : (ownerRow.OwnerTable.TableFormat.WrapTextAround ? ownerRow.OwnerTable.TableFormat.Positioning : (RowFormat.TablePositioning) null);
    if (tablePositioning != null)
    {
      switch (tablePositioning.HorizPositionAbs)
      {
        case HorizontalPosition.Outside:
          stringBuilder.Append(this.c_slashSymbol + "tposxo");
          break;
        case HorizontalPosition.Inside:
          stringBuilder.Append(this.c_slashSymbol + "tposxi");
          break;
        case HorizontalPosition.Right:
          stringBuilder.Append(this.c_slashSymbol + "tposxr");
          break;
        case HorizontalPosition.Center:
          stringBuilder.Append(this.c_slashSymbol + "tposxc");
          break;
        default:
          if (tablePositioning.HorizPositionAbs == HorizontalPosition.Left && (double) tablePositioning.HorizPosition == 0.0)
          {
            stringBuilder.Append(this.c_slashSymbol + "tposxl");
            break;
          }
          int num5 = (int) Math.Round((double) tablePositioning.HorizPosition * 20.0);
          stringBuilder.Append($"{this.c_slashSymbol}tposx{num5.ToString()}");
          break;
      }
      switch (tablePositioning.VertPositionAbs)
      {
        case VerticalPosition.Outside:
          stringBuilder.Append(this.c_slashSymbol + "tposyout");
          break;
        case VerticalPosition.Inside:
          stringBuilder.Append(this.c_slashSymbol + "tposyin");
          break;
        case VerticalPosition.Bottom:
          stringBuilder.Append(this.c_slashSymbol + "tposyb");
          break;
        case VerticalPosition.Center:
          stringBuilder.Append(this.c_slashSymbol + "tposyc");
          break;
        case VerticalPosition.Top:
          stringBuilder.Append(this.c_slashSymbol + "tposyt");
          break;
        default:
          int num6 = (int) Math.Round((double) tablePositioning.VertPosition * 20.0);
          stringBuilder.Append($"{this.c_slashSymbol}tposy{num6.ToString()}");
          break;
      }
      int num7 = (int) Math.Round((double) tablePositioning.DistanceFromLeft * 20.0);
      if (num7 != 0)
        stringBuilder.Append($"{this.c_slashSymbol}tdfrmtxtLeft{num7.ToString()}");
      if ((int) Math.Round((double) tablePositioning.DistanceFromRight * 20.0) != 0)
        stringBuilder.Append($"{this.c_slashSymbol}tdfrmtxtRight{num7.ToString()}");
      if ((int) Math.Round((double) tablePositioning.DistanceFromTop * 20.0) != 0)
        stringBuilder.Append($"{this.c_slashSymbol}tdfrmtxtTop{num7.ToString()}");
      if ((int) Math.Round((double) tablePositioning.DistanceFromBottom * 20.0) != 0)
        stringBuilder.Append($"{this.c_slashSymbol}tdfrmtxtBottom{num7.ToString()}");
    }
    if (!rowFormat.Positioning.AllowOverlap)
      stringBuilder.Append(this.c_slashSymbol + "tabsnoovrlp1");
    if ((double) rowFormat.CellSpacing != -1.0)
    {
      string str = ((int) Math.Round((double) rowFormat.CellSpacing * 20.0)).ToString();
      stringBuilder.Append($"{this.c_slashSymbol}trspdl{str}");
      stringBuilder.Append($"{this.c_slashSymbol}trspdr{str}");
      stringBuilder.Append($"{this.c_slashSymbol}trspdb{str}");
      stringBuilder.Append($"{this.c_slashSymbol}trspdt{str}");
      stringBuilder.Append($"{this.c_slashSymbol}trspdfl3{this.c_slashSymbol}trspdft3{this.c_slashSymbol}trspdfb3{this.c_slashSymbol}trspdfr3");
    }
    else if ((double) rowFormat.LeftIndent > 0.0)
      stringBuilder.Append(this.c_slashSymbol + "trgaph108");
    if ((double) rowFormat.Height != 0.0)
    {
      if (ownerRow.HeightType == TableRowHeightType.Exactly && (double) ownerRow.Height > 0.0)
        stringBuilder.Append($"{this.c_slashSymbol}trrh{((int) Math.Round(-((double) ownerRow.Height * 20.0))).ToString()}");
      else
        stringBuilder.Append($"{this.c_slashSymbol}trrh{((int) Math.Round((double) ownerRow.Height * 20.0)).ToString()}");
    }
    if (ownerRow != null && ownerRow.OwnerTable != null)
    {
      int num8 = 0;
      string str = string.Empty;
      if (ownerRow.OwnerTable.TableFormat.PreferredWidth.WidthType != FtsWidth.Auto)
      {
        if (ownerRow.OwnerTable.TableGrid.Count > 0 && ownerRow.OwnerTable.DocxTableFormat.HasFormat)
        {
          num8 = (int) ownerRow.OwnerTable.TableGrid[ownerRow.OwnerTable.TableGrid.Count - 1].EndOffset;
          str = "trftsWidth3";
        }
        else if (ownerRow.OwnerTable.TableFormat.PreferredWidth.WidthType == FtsWidth.Percentage)
        {
          num8 = (int) Math.Round((double) ownerRow.OwnerTable.PreferredTableWidth.Width * 50.0);
          str = "trftsWidth2";
        }
        else if (ownerRow.OwnerTable.TableFormat.PreferredWidth.WidthType == FtsWidth.Point)
        {
          num8 = (int) Math.Round((double) ownerRow.OwnerTable.PreferredTableWidth.Width * 20.0);
          str = "trftsWidth3";
        }
        if (str != string.Empty)
          stringBuilder.Append(this.c_slashSymbol + str);
        if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || num8 != 0)
          stringBuilder.Append($"{this.c_slashSymbol}trwWidth{num8.ToString()}");
      }
    }
    if (rowFormat.IsAutoResized)
      stringBuilder.Append(this.c_slashSymbol + "trautofit1");
    if ((double) rowFormat.GridBeforeWidth.Width > 0.0 && rowFormat.GridBeforeWidth.WidthType != FtsWidth.Auto && rowFormat.GridBeforeWidth.WidthType != FtsWidth.None)
    {
      int num9 = 0;
      string str = string.Empty;
      if (rowFormat.GridBeforeWidth.WidthType == FtsWidth.Percentage)
      {
        num9 = (int) Math.Round((double) rowFormat.GridBeforeWidth.Width * 50.0);
        str = "trftsWidthB2";
      }
      else if (rowFormat.GridBeforeWidth.WidthType == FtsWidth.Point)
      {
        num9 = (int) Math.Round((double) rowFormat.GridBeforeWidth.Width * 20.0);
        str = "trftsWidthB3";
      }
      if (str != string.Empty)
        stringBuilder.Append(this.c_slashSymbol + str);
      stringBuilder.Append($"{this.c_slashSymbol}trftsWidthB{num9.ToString()}");
    }
    if ((double) rowFormat.GridAfterWidth.Width > 0.0 && rowFormat.GridAfterWidth.WidthType != FtsWidth.Auto && rowFormat.GridAfterWidth.WidthType != FtsWidth.None)
    {
      int num10 = 0;
      string str = string.Empty;
      if (rowFormat.GridAfterWidth.WidthType == FtsWidth.Percentage)
      {
        num10 = (int) Math.Round((double) rowFormat.GridAfterWidth.Width * 50.0);
        str = "trftsWidthA2";
      }
      else if (rowFormat.GridAfterWidth.WidthType == FtsWidth.Point)
      {
        num10 = (int) Math.Round((double) rowFormat.GridAfterWidth.Width * 20.0);
        str = "trftsWidthA3";
      }
      if (str != string.Empty)
        stringBuilder.Append(this.c_slashSymbol + str);
      stringBuilder.Append($"{this.c_slashSymbol}trftsWidthA{num10.ToString()}");
    }
    if (ownerRow != null && !this.m_doc.SaveOptions.OptimizeRtfFileSize)
      stringBuilder.Append(this.BuildCharacterFormat(ownerRow.CharacterFormat));
    RowAlignment rowAlignment = rowFormat.HorizontalAlignment;
    if (rowFormat.Bidi && (rowFormat.Document.ActualFormatType == FormatType.Doc || rowFormat.Document.ActualFormatType == FormatType.Docx || rowFormat.Document.ActualFormatType == FormatType.Rtf))
    {
      if (rowAlignment == RowAlignment.Right)
        rowAlignment = RowAlignment.Left;
      else if (rowAlignment == RowAlignment.Left)
        rowAlignment = RowAlignment.Right;
    }
    if (rowAlignment == RowAlignment.Right)
      stringBuilder.Append(this.c_slashSymbol + "trqr");
    else if (rowAlignment == RowAlignment.Center)
      stringBuilder.Append(this.c_slashSymbol + "trqc");
    else if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
      stringBuilder.Append(this.c_slashSymbol + "trql");
    stringBuilder.Append(this.BuildTRowBorders(rowFormat.Borders));
    stringBuilder.Append(this.BuildPadding(rowFormat.Paddings, true));
    this.GetOwnerSection((Entity) ownerRow);
    if (ownerRow != null)
    {
      int index = 0;
      for (int count = ownerRow.Cells.Count; index < count; ++index)
      {
        WTableCell cell = ownerRow.Cells[index];
        if (cell != null && cell.CellFormat != null && cell.CellFormat.HorizontalMerge != CellMerge.Continue)
        {
          stringBuilder.Append(Environment.NewLine);
          stringBuilder.Append(this.BuildTCellFormat(cell.CellFormat));
        }
      }
    }
    this.m_cellEndPos = 0;
    stringBuilder.Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  private string BuildTCellFormat(CellFormat cFormat)
  {
    StringBuilder stringBuilder = new StringBuilder();
    WTableCell ownerBase = cFormat.OwnerBase as WTableCell;
    if (ownerBase.CellFormat.VerticalMerge == CellMerge.Start)
    {
      stringBuilder.Append(this.c_slashSymbol + "clvmgf");
      stringBuilder.Append(this.BuildVertAlignment(ownerBase.CellFormat.VerticalAlignment));
    }
    else if (ownerBase.CellFormat.VerticalMerge == CellMerge.Continue)
    {
      stringBuilder.Append(this.c_slashSymbol + "clvmrg");
      stringBuilder.Append(this.BuildVertAlignment(cFormat.VerticalAlignment));
    }
    else if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || cFormat.VerticalAlignment != VerticalAlignment.Top)
      stringBuilder.Append(this.BuildVertAlignment(cFormat.VerticalAlignment));
    if (cFormat.Borders.IsCellHasNoBorder)
      stringBuilder.Append(this.BuildTCellBorders(ownerBase, ownerBase.OwnerRow.RowFormat.Borders, (Borders) null));
    else
      stringBuilder.Append(this.BuildTCellBorders(ownerBase, cFormat.Borders, ownerBase.OwnerRow.RowFormat.Borders));
    switch (ownerBase.CellFormat.TextDirection)
    {
      case TextDirection.VerticalFarEast:
        stringBuilder.Append(this.c_slashSymbol + "cltxtbrlv");
        break;
      case TextDirection.VerticalBottomToTop:
        stringBuilder.Append(this.c_slashSymbol + "cltxbtlr");
        break;
      case TextDirection.VerticalTopToBottom:
        stringBuilder.Append(this.c_slashSymbol + "cltxtbrl");
        break;
      case TextDirection.HorizontalFarEast:
        stringBuilder.Append(this.c_slashSymbol + "cltxlrtbv");
        break;
      default:
        if (!this.m_doc.SaveOptions.OptimizeRtfFileSize)
        {
          stringBuilder.Append(this.c_slashSymbol + "cltxlrtb");
          break;
        }
        break;
    }
    if (!cFormat.BackColor.IsEmpty)
      stringBuilder.Append(this.BuildColor(cFormat.BackColor, this.c_slashSymbol + "clcbpat"));
    if (cFormat.FitText)
      stringBuilder.Append(this.c_slashSymbol + "clFitText");
    if (!cFormat.TextWrap)
      stringBuilder.Append(this.c_slashSymbol + "clNoWrap");
    int num1 = 0;
    string str = string.Empty;
    int num2 = (int) Math.Round((double) ownerBase.Width * 20.0);
    if (ownerBase.CellFormat.PreferredWidth.WidthType == FtsWidth.None)
      str = "clftsWidth0";
    else if (ownerBase.CellFormat.PreferredWidth.WidthType == FtsWidth.Auto)
      str = "clftsWidth1";
    else if (ownerBase.CellFormat.PreferredWidth.WidthType == FtsWidth.Percentage)
    {
      num1 = (int) Math.Round((double) ownerBase.CellFormat.PreferredWidth.Width * 50.0);
      str = "clftsWidth2";
    }
    else if (ownerBase.CellFormat.PreferredWidth.WidthType == FtsWidth.Point)
    {
      num1 = (int) Math.Round((double) ownerBase.CellFormat.PreferredWidth.Width * 20.0);
      str = "clftsWidth3";
    }
    if (ownerBase.CellFormat.HorizontalMerge == CellMerge.Start)
    {
      for (int index = ownerBase.GetCellIndex() + 1; index < (ownerBase.Owner as WTableRow).ChildEntities.Count; ++index)
      {
        WTableCell childEntity = (ownerBase.Owner as WTableRow).ChildEntities[index] as WTableCell;
        if (childEntity.CellFormat.HorizontalMerge == CellMerge.Continue)
          num2 += (int) Math.Round((double) childEntity.Width * 20.0);
        else
          break;
      }
    }
    if (str != string.Empty && !this.m_doc.SaveOptions.OptimizeRtfFileSize || str != "clftsWidth0")
      stringBuilder.Append(this.c_slashSymbol + str);
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || num1 != 0)
      stringBuilder.Append($"{this.c_slashSymbol}clwWidth{num1.ToString()}");
    this.m_cellEndPos += num2;
    if (!cFormat.SamePaddingsAsTable)
      stringBuilder.Append(this.BuildPadding(cFormat.Paddings, false));
    if (cFormat.HideMark)
      stringBuilder.Append(this.c_slashSymbol + "clhidemark");
    stringBuilder.Append(this.c_slashSymbol + "cellx");
    stringBuilder.Append(this.m_cellEndPos);
    return stringBuilder.ToString();
  }

  private string BuildTRowBorders(Borders borders)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || borders.Top.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "trbrdrt");
      stringBuilder.Append(this.BuildBorder(borders.Top, true));
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || borders.Bottom.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "trbrdrb");
      stringBuilder.Append(this.BuildBorder(borders.Bottom, true));
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || borders.Left.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "trbrdrl");
      stringBuilder.Append(this.BuildBorder(borders.Left, true));
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || borders.Right.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "trbrdrr");
      stringBuilder.Append(this.BuildBorder(borders.Right, true));
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || borders.Horizontal.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "trbrdrh");
      stringBuilder.Append(this.BuildBorder(borders.Horizontal, true));
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize || borders.Vertical.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "trbrdrv");
      stringBuilder.Append(this.BuildBorder(borders.Vertical, true));
    }
    return stringBuilder.ToString();
  }

  private string BuildTCellBorders(WTableCell cell, Borders borders, Borders rowBorders)
  {
    CellLayoutInfo cellLayoutInfo = cell.CreateCellLayoutInfo();
    bool skipTopBorder = cellLayoutInfo.SkipTopBorder;
    bool skipBottomBorder = cellLayoutInfo.SkipBottomBorder;
    bool skipLeftBorder = cellLayoutInfo.SkipLeftBorder;
    bool skipRightBorder = cellLayoutInfo.SkipRightBorder;
    StringBuilder stringBuilder = new StringBuilder();
    if (!skipTopBorder && (!this.m_doc.SaveOptions.OptimizeRtfFileSize || !borders.Top.HasNoneStyle))
    {
      stringBuilder.Append(this.c_slashSymbol + "clbrdrt");
      if (this.CheckCellBorders(cell, RtfWriter.BorderType.Top) && borders.Top.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(cell.OwnerRow.RowFormat.Borders.Horizontal, true));
      else if (!borders.Top.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(borders.Top, true));
      else if (rowBorders != null)
        stringBuilder.Append(this.BuildBorder(rowBorders.Top, true));
    }
    if (!skipBottomBorder && (!this.m_doc.SaveOptions.OptimizeRtfFileSize || !borders.Bottom.HasNoneStyle))
    {
      stringBuilder.Append(this.c_slashSymbol + "clbrdrb");
      if (this.CheckCellBorders(cell, RtfWriter.BorderType.Bottom) && borders.Bottom.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(cell.OwnerRow.RowFormat.Borders.Horizontal, true));
      else if (!borders.Bottom.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(borders.Bottom, true));
      else if (rowBorders != null)
        stringBuilder.Append(this.BuildBorder(rowBorders.Bottom, true));
    }
    if (!skipLeftBorder && (!this.m_doc.SaveOptions.OptimizeRtfFileSize || !borders.Left.HasNoneStyle))
    {
      stringBuilder.Append(this.c_slashSymbol + "clbrdrl");
      if (this.CheckCellBorders(cell, RtfWriter.BorderType.Left) && borders.Left.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(cell.OwnerRow.RowFormat.Borders.Vertical, true));
      else if (!borders.Left.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(borders.Left, true));
      else if (rowBorders != null)
        stringBuilder.Append(this.BuildBorder(rowBorders.Left, true));
    }
    if (!skipRightBorder && (!this.m_doc.SaveOptions.OptimizeRtfFileSize || !borders.Right.HasNoneStyle))
    {
      stringBuilder.Append(this.c_slashSymbol + "clbrdrr");
      if (this.CheckCellBorders(cell, RtfWriter.BorderType.Right) && borders.Right.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(cell.OwnerRow.RowFormat.Borders.Vertical, true));
      else if (!borders.Right.HasNoneStyle)
        stringBuilder.Append(this.BuildBorder(borders.Right, true));
      else if (rowBorders != null)
        stringBuilder.Append(this.BuildBorder(rowBorders.Right, true));
    }
    if (borders.DiagonalDown.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "cldglu");
      stringBuilder.Append(this.BuildBorder(borders.DiagonalDown, true));
    }
    if (borders.DiagonalUp.BorderType != BorderStyle.None)
    {
      stringBuilder.Append(this.c_slashSymbol + "cldgll");
      stringBuilder.Append(this.BuildBorder(borders.DiagonalUp, true));
    }
    return stringBuilder.ToString();
  }

  private byte[] BuildTableCell(WTableCell cell)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] buffer = this.BuildBodyItems(cell.Items);
    memoryStream.Write(buffer, 0, buffer.Length);
    if (this.m_tableNestedLevel > 1)
    {
      byte[] bytes = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}nestcell}}");
      memoryStream.Write(bytes, 0, bytes.Length);
    }
    else
    {
      byte[] bytes = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}cell}}");
      memoryStream.Write(bytes, 0, bytes.Length);
    }
    byte[] bytes1 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes1, 0, bytes1.Length);
    return memoryStream.ToArray();
  }

  private string BuildPadding(Paddings paddings, bool isRow)
  {
    StringBuilder stringBuilder = new StringBuilder();
    string str = this.c_slashSymbol + (isRow ? "trpadd" : "clpad");
    bool flag = false;
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && paddings.HasKey(1) || paddings.HasKey(1) && (double) paddings.Left != 0.0)
    {
      stringBuilder.Append(str + (isRow ? "l" : "t"));
      stringBuilder.Append((int) Math.Round((double) paddings.Left * 20.0));
      flag = true;
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && paddings.HasKey(2) || paddings.HasKey(2) && (double) paddings.Top != 0.0)
    {
      stringBuilder.Append(str + (isRow ? "t" : "l"));
      stringBuilder.Append((int) Math.Round((double) paddings.Top * 20.0));
      flag = true;
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && paddings.HasKey(3) || paddings.HasKey(3) && (double) paddings.Bottom != 0.0)
    {
      stringBuilder.Append(str + "b");
      stringBuilder.Append((int) Math.Round((double) paddings.Bottom * 20.0));
      flag = true;
    }
    if (!this.m_doc.SaveOptions.OptimizeRtfFileSize && paddings.HasKey(4) || paddings.HasKey(4) && (double) paddings.Right != 0.0)
    {
      stringBuilder.Append(str + "r");
      stringBuilder.Append((int) Math.Round((double) paddings.Right * 20.0));
      flag = true;
    }
    if (flag)
    {
      if (isRow)
        stringBuilder.Append($"{this.c_slashSymbol}trpaddfl3{this.c_slashSymbol}trpaddft3{this.c_slashSymbol}trpaddfb3{this.c_slashSymbol}trpaddfr3");
      else
        stringBuilder.Append($"{this.c_slashSymbol}clpadfl3{this.c_slashSymbol}clpadft3{this.c_slashSymbol}clpadfb3{this.c_slashSymbol}clpadfr3");
    }
    return stringBuilder.ToString();
  }

  private string BuildVertAlignment(VerticalAlignment alignment)
  {
    switch (alignment)
    {
      case VerticalAlignment.Top:
        return this.c_slashSymbol + "clvertalt";
      case VerticalAlignment.Middle:
        return this.c_slashSymbol + "clvertalc";
      case VerticalAlignment.Bottom:
        return this.c_slashSymbol + "clvertalb";
      default:
        return string.Empty;
    }
  }

  private bool CheckCellBorders(WTableCell cell, RtfWriter.BorderType borderType)
  {
    WTableRow ownerRow = cell.OwnerRow;
    switch (borderType)
    {
      case RtfWriter.BorderType.Right:
        if (cell.NextSibling is WTableCell)
          return true;
        break;
      case RtfWriter.BorderType.Left:
        if (cell.PreviousSibling is WTableCell)
          return true;
        break;
      case RtfWriter.BorderType.Top:
        if (ownerRow.PreviousSibling is WTableRow)
          return true;
        break;
      case RtfWriter.BorderType.Bottom:
        if (ownerRow.NextSibling is WTableRow)
          return true;
        break;
    }
    return false;
  }

  private byte[] BuildParagraphItem(ParagraphItem item)
  {
    MemoryStream memoryStream = new MemoryStream();
    StringBuilder stringBuilder = new StringBuilder();
    switch (item.EntityType)
    {
      case EntityType.InlineContentControl:
        ParagraphItemCollection paragraphItems = (item as InlineContentControl).ParagraphItems;
        for (int index = 0; index < paragraphItems.Count; ++index)
        {
          byte[] buffer = this.BuildParagraphItem(paragraphItems[index]);
          memoryStream.Write(buffer, 0, buffer.Length);
        }
        break;
      case EntityType.TextRange:
        if (item is WTextRange textRange)
        {
          byte[] bytes = this.m_encoding.GetBytes(this.BuildTextRange(textRange));
          memoryStream.Write(bytes, 0, bytes.Length);
          break;
        }
        break;
      case EntityType.Picture:
        if ((item as WPicture).ImageRecord != null)
        {
          byte[] bytes = this.m_encoding.GetBytes(this.BuildPicture(item as WPicture, false));
          memoryStream.Write(bytes, 0, bytes.Length);
          break;
        }
        break;
      case EntityType.Field:
      case EntityType.MergeField:
      case EntityType.SeqField:
      case EntityType.ControlField:
      case EntityType.OleObject:
        if (item is WMergeField)
          (item as WMergeField).UpdateFieldMarks();
        WField field = item is WField ? item as WField : (item as WOleObject).Field;
        if (field.FieldEnd != null && field.FieldType != FieldType.FieldUnknown)
        {
          this.CurrentField.Push((object) field);
          byte[] bytes = this.m_encoding.GetBytes(this.BuildField(field));
          memoryStream.Write(bytes, 0, bytes.Length);
          break;
        }
        break;
      case EntityType.FieldMark:
        byte[] bytes1 = this.m_encoding.GetBytes(this.BuildFieldMark(item as WFieldMark));
        memoryStream.Write(bytes1, 0, bytes1.Length);
        break;
      case EntityType.TextFormField:
        byte[] bytes2 = this.m_encoding.GetBytes(this.BuildFormField((WFormField) (item as WTextFormField)));
        memoryStream.Write(bytes2, 0, bytes2.Length);
        break;
      case EntityType.DropDownFormField:
        byte[] bytes3 = this.m_encoding.GetBytes(this.BuildFormField((WFormField) (item as WDropDownFormField)));
        memoryStream.Write(bytes3, 0, bytes3.Length);
        break;
      case EntityType.CheckBox:
        byte[] bytes4 = this.m_encoding.GetBytes(this.BuildFormField((WFormField) (item as WCheckBox)));
        memoryStream.Write(bytes4, 0, bytes4.Length);
        break;
      case EntityType.BookmarkStart:
        byte[] buffer1 = this.InsertBkmkStart(item as BookmarkStart);
        memoryStream.Write(buffer1, 0, buffer1.Length);
        break;
      case EntityType.BookmarkEnd:
        byte[] buffer2 = this.InsertBkmkEnd(item as BookmarkEnd);
        memoryStream.Write(buffer2, 0, buffer2.Length);
        break;
      case EntityType.Comment:
        byte[] buffer3 = this.BuildComment(item as WComment);
        memoryStream.Write(buffer3, 0, buffer3.Length);
        break;
      case EntityType.Footnote:
        byte[] buffer4 = this.BuildFootnoteEndnote(item as WFootnote);
        memoryStream.Write(buffer4, 0, buffer4.Length);
        break;
      case EntityType.TextBox:
        byte[] buffer5 = this.BuildTextBox(item as WTextBox);
        memoryStream.Write(buffer5, 0, buffer5.Length);
        break;
      case EntityType.Break:
        byte[] buffer6 = this.InsertLineBreak(item as Break);
        memoryStream.Write(buffer6, 0, buffer6.Length);
        break;
      case EntityType.Symbol:
        byte[] buffer7 = this.BuildSymbol(item as WSymbol);
        memoryStream.Write(buffer7, 0, buffer7.Length);
        break;
      case EntityType.TOC:
        byte[] buffer8 = this.BuildTocField(item as TableOfContent);
        memoryStream.Write(buffer8, 0, buffer8.Length);
        break;
      case EntityType.XmlParaItem:
        if (item is XmlParagraphItem xmlParagraphItem && xmlParagraphItem.MathParaItemsCollection != null && xmlParagraphItem.MathParaItemsCollection.Count > 0)
        {
          IEnumerator enumerator = xmlParagraphItem.MathParaItemsCollection.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              byte[] buffer9 = this.BuildParagraphItem((ParagraphItem) enumerator.Current);
              memoryStream.Write(buffer9, 0, buffer9.Length);
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        }
        else
          break;
      case EntityType.CommentMark:
        byte[] buffer10 = this.BuildCommentMark(item as WCommentMark);
        memoryStream.Write(buffer10, 0, buffer10.Length);
        break;
      case EntityType.AutoShape:
        if (item is Shape)
        {
          byte[] bytes5 = this.m_encoding.GetBytes(this.BuildShape(item as Shape));
          memoryStream.Write(bytes5, 0, bytes5.Length);
          break;
        }
        break;
    }
    return memoryStream.ToArray();
  }

  private byte[] BuildSymbol(WSymbol symbol)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(symbol.CharacterFormat));
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}field{{{this.c_slashSymbol}*{this.c_slashSymbol}fldinst SYMBOL ");
    memoryStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes(symbol.CharacterCode.ToString());
    memoryStream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = this.m_encoding.GetBytes($" {this.c_slashSymbol}{this.c_slashSymbol}f \"{symbol.FontName}\"}}");
    memoryStream.Write(bytes5, 0, bytes5.Length);
    byte[] bytes6 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}fldrslt}}");
    memoryStream.Write(bytes6, 0, bytes6.Length);
    byte[] bytes7 = this.m_encoding.GetBytes("}}");
    memoryStream.Write(bytes7, 0, bytes7.Length);
    return memoryStream.ToArray();
  }

  private byte[] BuildFootnoteEndnote(WFootnote footnote)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    if (string.IsNullOrEmpty(footnote.CustomMarker) && !footnote.CustomMarkerIsSymbol)
    {
      byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(footnote.ParaItemCharFormat));
      memoryStream.Write(bytes2, 0, bytes2.Length);
      byte[] bytes3 = this.m_encoding.GetBytes(this.c_slashSymbol + "chftn");
      memoryStream.Write(bytes3, 0, bytes3.Length);
    }
    else if (footnote.CustomMarkerIsSymbol)
    {
      byte[] bytes4 = this.m_encoding.GetBytes(this.BuildCharacterFormat(footnote.MarkerCharacterFormat));
      memoryStream.Write(bytes4, 0, bytes4.Length);
      byte[] buffer = this.BuildSymbol(new WSymbol((IWordDocument) this.m_doc)
      {
        CharacterCode = footnote.SymbolCode,
        FontName = footnote.SymbolFontName
      });
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    else
    {
      byte[] bytes5 = this.m_encoding.GetBytes(this.BuildCharacterFormat(footnote.MarkerCharacterFormat));
      memoryStream.Write(bytes5, 0, bytes5.Length);
      byte[] bytes6 = this.m_encoding.GetBytes(" " + footnote.m_strCustomMarker);
      memoryStream.Write(bytes6, 0, bytes6.Length);
    }
    byte[] bytes7 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}footnote");
    memoryStream.Write(bytes7, 0, bytes7.Length);
    if (footnote.FootnoteType == FootnoteType.Endnote)
    {
      byte[] bytes8 = this.m_encoding.GetBytes(this.c_slashSymbol + "ftnalt");
      memoryStream.Write(bytes8, 0, bytes8.Length);
    }
    if (string.IsNullOrEmpty(footnote.CustomMarker) && !footnote.CustomMarkerIsSymbol)
    {
      byte[] bytes9 = this.m_encoding.GetBytes(this.BuildCharacterFormat(footnote.MarkerCharacterFormat));
      memoryStream.Write(bytes9, 0, bytes9.Length);
      byte[] bytes10 = this.m_encoding.GetBytes(this.c_slashSymbol + "chftn");
      memoryStream.Write(bytes10, 0, bytes10.Length);
    }
    byte[] bytes11 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes11, 0, bytes11.Length);
    byte[] buffer1 = this.BuildBodyItems(footnote.TextBody.Items);
    memoryStream.Write(buffer1, 0, buffer1.Length);
    byte[] bytes12 = this.m_encoding.GetBytes("}}");
    memoryStream.Write(bytes12, 0, bytes12.Length);
    byte[] bytes13 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes13, 0, bytes13.Length);
    if (footnote.FootnoteType == FootnoteType.Footnote)
    {
      byte[] bytes14 = this.m_encoding.GetBytes(this.BuildFootnoteProp());
      memoryStream.Write(bytes14, 0, bytes14.Length);
    }
    else
    {
      byte[] bytes15 = this.m_encoding.GetBytes(this.BuildEndnoteProp());
      memoryStream.Write(bytes15, 0, bytes15.Length);
    }
    return memoryStream.ToArray();
  }

  private string BuildFootnoteProp()
  {
    if (this.m_hasFootnote)
      return string.Empty;
    this.m_hasFootnote = true;
    StringBuilder stringBuilder = new StringBuilder();
    if (this.m_doc.RestartIndexForFootnotes == FootnoteRestartIndex.DoNotRestart)
      stringBuilder.Append(this.c_slashSymbol + "sftnrstcont");
    else if (this.m_doc.RestartIndexForFootnotes == FootnoteRestartIndex.RestartForEachPage)
      stringBuilder.Append(this.c_slashSymbol + "sftnrstpg");
    else if (this.m_doc.RestartIndexForFootnotes == FootnoteRestartIndex.RestartForEachSection)
      stringBuilder.Append(this.c_slashSymbol + "sftnrestart");
    stringBuilder.Append(this.c_slashSymbol + "sftnstart");
    stringBuilder.Append(this.m_doc.InitialFootnoteNumber);
    switch (this.m_doc.FootnoteNumberFormat)
    {
      case FootEndNoteNumberFormat.Arabic:
        stringBuilder.Append(this.c_slashSymbol + "sftnnar");
        break;
      case FootEndNoteNumberFormat.UpperCaseRoman:
        stringBuilder.Append(this.c_slashSymbol + "sftnnruc");
        break;
      case FootEndNoteNumberFormat.LowerCaseRoman:
        stringBuilder.Append(this.c_slashSymbol + "sftnnrlc");
        break;
      case FootEndNoteNumberFormat.UpperCaseLetter:
        stringBuilder.Append(this.c_slashSymbol + "sftnnauc");
        break;
      case FootEndNoteNumberFormat.LowerCaseLetter:
        stringBuilder.Append(this.c_slashSymbol + "sftnnalc");
        break;
      default:
        stringBuilder.Append(this.c_slashSymbol + "sftnnchi");
        break;
    }
    if (this.m_doc.FootnotePosition == FootnotePosition.PrintAtBottomOfPage)
      stringBuilder.Append(this.c_slashSymbol + "ftnbj");
    else if (this.m_doc.FootnotePosition == FootnotePosition.PrintImmediatelyBeneathText)
      stringBuilder.Append(this.c_slashSymbol + "ftntj");
    else if (this.m_doc.FootnotePosition == FootnotePosition.PrintAsEndnotes)
    {
      if (this.m_doc.EndnotePosition == EndnotePosition.DisplayEndOfDocument)
        stringBuilder.Append(this.c_slashSymbol + "enddoc");
      else if (this.m_doc.EndnotePosition == EndnotePosition.DisplayEndOfSection)
        stringBuilder.Append(this.c_slashSymbol + "endnotes");
    }
    return stringBuilder.ToString();
  }

  private string BuildEndnoteProp()
  {
    if (this.m_hasEndnote)
      return string.Empty;
    this.m_hasEndnote = true;
    StringBuilder stringBuilder = new StringBuilder();
    if (this.m_doc.RestartIndexForEndnote == EndnoteRestartIndex.DoNotRestart)
      stringBuilder.Append(this.c_slashSymbol + "saftnrstcont");
    else if (this.m_doc.RestartIndexForEndnote == EndnoteRestartIndex.RestartForEachSection)
      stringBuilder.Append(this.c_slashSymbol + "saftnrestart");
    stringBuilder.Append(this.c_slashSymbol + "saftnstart");
    stringBuilder.Append(this.m_doc.InitialEndnoteNumber);
    switch (this.m_doc.EndnoteNumberFormat)
    {
      case FootEndNoteNumberFormat.Arabic:
        stringBuilder.Append(this.c_slashSymbol + "saftnnar");
        break;
      case FootEndNoteNumberFormat.UpperCaseRoman:
        stringBuilder.Append(this.c_slashSymbol + "saftnnruc");
        break;
      case FootEndNoteNumberFormat.LowerCaseRoman:
        stringBuilder.Append(this.c_slashSymbol + "saftnnrlc");
        break;
      case FootEndNoteNumberFormat.UpperCaseLetter:
        stringBuilder.Append(this.c_slashSymbol + "saftnnauc");
        break;
      case FootEndNoteNumberFormat.LowerCaseLetter:
        stringBuilder.Append(this.c_slashSymbol + "saftnnalc");
        break;
    }
    if (this.m_doc.EndnotePosition == EndnotePosition.DisplayEndOfDocument)
      stringBuilder.Append(this.c_slashSymbol + "aenddoc");
    else if (this.m_doc.EndnotePosition == EndnotePosition.DisplayEndOfSection)
      stringBuilder.Append(this.c_slashSymbol + "aendnotes");
    return stringBuilder.ToString();
  }

  private string BuildFieldMark(WFieldMark fieldMark)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (this.m_currentField == null || this.CurrentField.Count == 0)
      return string.Empty;
    if (fieldMark.Type == FieldMarkType.FieldSeparator || fieldMark.ParentField != null && fieldMark.ParentField.FieldSeparator == null)
    {
      string str = string.Empty;
      if (fieldMark.ParentField is WTextFormField)
        str = this.BuildTextFormField(fieldMark.ParentField as WTextFormField);
      else if (fieldMark.ParentField is WCheckBox)
        str = this.BuildCheckBox(fieldMark.ParentField as WCheckBox);
      else if (fieldMark.ParentField is WDropDownFormField)
        str = this.BuildDropDownField(fieldMark.ParentField as WDropDownFormField);
      if (!string.IsNullOrEmpty(str))
        stringBuilder.Append(str);
    }
    if (fieldMark.Type == FieldMarkType.FieldSeparator)
    {
      if (!(this.CurrentField.Peek() as WField).IsFormField())
        stringBuilder.Append("}");
      stringBuilder.Append($"}}{{{this.c_slashSymbol}fldrslt");
    }
    else if (fieldMark.ParentField != null && fieldMark.ParentField.FieldSeparator == null)
    {
      if (!(this.CurrentField.Peek() as WField).IsFormField())
        stringBuilder.Append("}");
      stringBuilder.Append($"}}{{{this.c_slashSymbol}fldrslt}}}}");
      this.CurrentField.Pop();
    }
    else if (!(fieldMark.PreviousSibling is WField))
    {
      stringBuilder.Append("}}");
      this.CurrentField.Pop();
    }
    else if (this.WriteFieldEnd(fieldMark))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}fldrslt}}}}");
      this.CurrentField.Pop();
    }
    else
    {
      stringBuilder.Append("}");
      this.CurrentField.Pop();
    }
    if (this.m_isField && this.CurrentField.Count == 0)
      this.m_isField = false;
    return stringBuilder.ToString();
  }

  private string BuildField(WField field)
  {
    StringBuilder stringBuilder = new StringBuilder();
    this.m_isField = true;
    stringBuilder.Append($"{{{this.c_slashSymbol}field");
    if (WordDocument.DisableDateTimeUpdating && (field.FieldType == FieldType.FieldDate || field.FieldType == FieldType.FieldTime))
      stringBuilder.Append(this.c_slashSymbol + "fldlock");
    stringBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}fldinst{{");
    if (field.FieldType != FieldType.FieldNoteRef)
      stringBuilder.Append(this.BuildCharacterFormat(field.CharacterFormat));
    return stringBuilder.ToString();
  }

  private byte[] InsertLineBreak(Break brk)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(brk.CharacterFormat));
    memoryStream.Write(bytes2, 0, bytes2.Length);
    switch (brk.BreakType)
    {
      case BreakType.PageBreak:
        byte[] bytes3 = this.m_encoding.GetBytes(this.c_slashSymbol + "page");
        memoryStream.Write(bytes3, 0, bytes3.Length);
        break;
      case BreakType.ColumnBreak:
        byte[] bytes4 = this.m_encoding.GetBytes(this.c_slashSymbol + "column");
        memoryStream.Write(bytes4, 0, bytes4.Length);
        break;
      case BreakType.LineBreak:
        byte[] bytes5 = this.m_encoding.GetBytes(this.c_slashSymbol + "line");
        memoryStream.Write(bytes5, 0, bytes5.Length);
        break;
    }
    byte[] bytes6 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes6, 0, bytes6.Length);
    return memoryStream.ToArray();
  }

  private string BuildTextRange(WTextRange textRange)
  {
    textRange.Text = textRange.Text.Replace('\u0002'.ToString(), string.Empty);
    if (textRange.Text == string.Empty)
      return string.Empty;
    string text = textRange.Text;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.BuildTextRangeStr(textRange.CharacterFormat, this.PrepareText(text)));
    return stringBuilder.ToString();
  }

  private byte[] InsertBkmkEnd(BookmarkEnd bkmkEnd)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes($"{this.c_slashSymbol}*{this.c_slashSymbol}bkmkend ");
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes(bkmkEnd.Name);
    memoryStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes5, 0, bytes5.Length);
    return memoryStream.ToArray();
  }

  private byte[] InsertBkmkStart(BookmarkStart bkmkStart)
  {
    StringBuilder stringBuilder = new StringBuilder();
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes($"{this.c_slashSymbol}*{this.c_slashSymbol}bkmkstart ");
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes(bkmkStart.Name);
    memoryStream.Write(bytes3, 0, bytes3.Length);
    if (bkmkStart.ColumnFirst >= (short) 0)
    {
      byte[] bytes4 = this.m_encoding.GetBytes($"{this.c_slashSymbol}bkmkcolf{(object) bkmkStart.ColumnFirst}");
      memoryStream.Write(bytes4, 0, bytes4.Length);
    }
    if (bkmkStart.ColumnLast >= (short) 0)
    {
      byte[] bytes5 = this.m_encoding.GetBytes($"{this.c_slashSymbol}bkmkcoll{(object) ((int) bkmkStart.ColumnLast + 1)}");
      memoryStream.Write(bytes5, 0, bytes5.Length);
    }
    byte[] bytes6 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes6, 0, bytes6.Length);
    byte[] bytes7 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes7, 0, bytes7.Length);
    return memoryStream.ToArray();
  }

  private byte[] BuildTocField(TableOfContent toc)
  {
    this.CurrentField.Push((object) toc.TOCField);
    return this.m_encoding.GetBytes(this.BuildField(toc.TOCField));
  }

  private string BuildPicture(WPicture pic, bool isFieldShape)
  {
    return pic.TextWrappingStyle == TextWrappingStyle.Inline ? this.BuildInLineImage(pic, isFieldShape) : this.BuildShapeImage(pic, isFieldShape);
  }

  private string BuildShapeImage(WPicture pic, bool isFielsShape)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("{");
    stringBuilder.Append(this.BuildCharacterFormat(pic.CharacterFormat));
    stringBuilder.Append($"{{{this.c_slashSymbol}shp");
    stringBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}shpinst");
    int shapeW;
    int shapeH;
    if ((double) pic.Rotation >= 44.0 && (double) pic.Rotation <= 134.0 || (double) pic.Rotation >= 225.0 && (double) pic.Rotation <= 314.0)
    {
      float num = Math.Abs(pic.Width - pic.Height) / 2f;
      if ((double) pic.Width < (double) pic.Height)
      {
        pic.HorizontalPosition -= num;
        pic.VerticalPosition += num;
      }
      else if ((double) pic.Width > (double) pic.Height)
      {
        pic.VerticalPosition -= num;
        pic.HorizontalPosition += num;
      }
      shapeW = (int) Math.Round((double) pic.Height * 20.0);
      shapeH = (int) Math.Round((double) pic.Width * 20.0);
    }
    else
    {
      shapeW = (int) Math.Round((double) pic.Width * 20.0);
      shapeH = (int) Math.Round((double) pic.Height * 20.0);
    }
    stringBuilder.Append(this.BuildShapePosition(pic.HorizontalPosition, pic.VerticalPosition, shapeW, shapeH));
    if (pic.IsHeaderPicture)
      stringBuilder.Append(this.c_slashSymbol + "shpfhdr1");
    else
      stringBuilder.Append(this.c_slashSymbol + "shpfhdr0");
    if (pic.HorizontalOrigin == HorizontalOrigin.Page)
      stringBuilder.Append(this.c_slashSymbol + "shpbxpage");
    else if (pic.HorizontalOrigin == HorizontalOrigin.Margin || pic.HorizontalOrigin == HorizontalOrigin.LeftMargin || pic.HorizontalOrigin == HorizontalOrigin.RightMargin || pic.HorizontalOrigin == HorizontalOrigin.InsideMargin || pic.HorizontalOrigin == HorizontalOrigin.OutsideMargin)
      stringBuilder.Append(this.c_slashSymbol + "shpbxmargin");
    else if (pic.HorizontalOrigin == HorizontalOrigin.Column)
      stringBuilder.Append(this.c_slashSymbol + "shpbxcolumn");
    if (pic.VerticalOrigin == VerticalOrigin.Page)
      stringBuilder.Append(this.c_slashSymbol + "shpbypage");
    else if (pic.VerticalOrigin == VerticalOrigin.Margin)
      stringBuilder.Append(this.c_slashSymbol + "shpbymargin");
    else if (pic.VerticalOrigin == VerticalOrigin.Paragraph)
      stringBuilder.Append(this.c_slashSymbol + "shpbypara");
    stringBuilder.Append(this.BuildWrappingStyle(pic.TextWrappingStyle, pic.TextWrappingType));
    stringBuilder.Append($"{this.c_slashSymbol}shpz{(object) (pic.OrderIndex / 1024 /*0x0400*/)}");
    stringBuilder.Append(this.BuildShapeProp("shapeType", "75"));
    double num1 = (double) pic.Rotation * 65536.0;
    if (num1 != 0.0)
      stringBuilder.Append(this.BuildShapeProp("rotation", num1.ToString()));
    if (!string.IsNullOrEmpty(pic.ExternalLink))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}sp");
      stringBuilder.Append($"{{{this.c_slashSymbol}sn ");
      stringBuilder.Append("pibName");
      stringBuilder.Append("}");
      stringBuilder.Append($"{{{this.c_slashSymbol}sv ");
      stringBuilder.Append(pic.ExternalLink);
      stringBuilder.Append("}}");
    }
    if (!string.IsNullOrEmpty(pic.LinkType))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}sp");
      stringBuilder.Append($"{{{this.c_slashSymbol}sn ");
      stringBuilder.Append("pibFlags");
      stringBuilder.Append("}");
      stringBuilder.Append($"{{{this.c_slashSymbol}sv ");
      stringBuilder.Append(pic.LinkType);
      stringBuilder.Append("}}");
    }
    if (!string.IsNullOrEmpty(pic.Href))
      stringBuilder.Append(this.BuildShapeProp("pihlShape", this.BuildPictureLink(pic)));
    stringBuilder.Append(this.BuildShapeProp("pib", this.BuildPictureProp(pic, isFielsShape)));
    if (pic.PictureShape.PictureDescriptor.BorderTop.IsDefault && pic.PictureShape.PictureDescriptor.BorderLeft.IsDefault && pic.PictureShape.PictureDescriptor.BorderRight.IsDefault && pic.PictureShape.PictureDescriptor.BorderBottom.IsDefault)
      stringBuilder.Append(this.BuildShapeProp("fLine", "0"));
    else
      stringBuilder.Append(this.BuildShapeProp("fLine", "1"));
    stringBuilder.Append(this.BuildHorAlignm(pic.HorizontalAlignment));
    stringBuilder.Append(this.BuildHorPos(pic.HorizontalOrigin));
    stringBuilder.Append(this.BuildVertAlignm(pic.VerticalAlignment));
    stringBuilder.Append(this.BuildVertPos(pic.VerticalOrigin));
    stringBuilder.Append(this.BuildLayoutInCell(pic.LayoutInCell));
    if (pic.TextWrappingStyle == TextWrappingStyle.Behind)
      stringBuilder.Append(this.BuildShapeProp("fBehindDocument", "1"));
    stringBuilder.Append("}}}");
    stringBuilder.Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  private string BuildInLineImage(WPicture pic, bool isfieldShape)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("{");
    stringBuilder.Append(this.BuildCharacterFormat(pic.CharacterFormat));
    if (pic.IsMetaFile && this.IsWmfImage(pic))
    {
      stringBuilder.Append(this.BuildMetafileProp(pic));
    }
    else
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}shppict");
      if ((double) pic.Size.Height > 1638.0 || (double) pic.Size.Width > 1638.0)
      {
        WPicture pic1 = pic.Clone() as WPicture;
        pic1.WidthScale = 2f * pic1.WidthScale;
        pic1.HeightScale = 2f * pic1.HeightScale;
        pic1.Size = new SizeF(pic1.Size.Width / 2f, pic1.Size.Height / 2f);
        stringBuilder.Append(this.BuildPictureProp(pic1, isfieldShape));
      }
      else
        stringBuilder.Append(this.BuildPictureProp(pic, isfieldShape));
      stringBuilder.Append("}");
      stringBuilder.Append($"{{{this.c_slashSymbol}nonshppict");
      stringBuilder.Append(this.BuildMetafileProp(pic));
      stringBuilder.Append("}");
    }
    stringBuilder.Append("}");
    return stringBuilder.ToString();
  }

  private string BuildShape(Shape shape)
  {
    if (shape.AutoShapeType == AutoShapeType.Unknown)
      return string.Empty;
    StringBuilder strBuilder = new StringBuilder();
    if (shape.FallbackPic == null && shape.WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline)
      shape.WrapFormat.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
    strBuilder.Append($"{{{this.c_slashSymbol}shp");
    strBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}shpinst");
    this.BuildShapeBasicTokens(shape, strBuilder);
    this.BuildShapePositionTokens(shape, strBuilder);
    this.BuildShapeObjectTypeTokens(shape, strBuilder);
    this.BuildShapeHorizontalLineTokens(shape, strBuilder);
    this.BuildShapeLineTokens(shape, strBuilder);
    if (shape.IsFillStyleInline)
      this.BuildShapeFillTokens(shape, strBuilder);
    if (shape.ShapeGuide.Count != 0)
      this.BuildShapeAdjustValuesTokens(shape, strBuilder);
    if (shape.EffectList.Count >= 1 && shape.EffectList[0].IsShadowEffect)
      this.BuildShapeShadowTokens(shape, strBuilder);
    if (shape.EffectList.Count == 2 && shape.EffectList[1].IsShapeProperties)
      this.BuildShape3DTokens(shape, strBuilder);
    strBuilder.Append(this.BuildLayoutInCell(shape.LayoutInCell));
    strBuilder.Append("}");
    int tableNestedLevel = this.m_tableNestedLevel;
    this.m_tableNestedLevel = 0;
    strBuilder.Append($"{{{this.c_slashSymbol}shptxt");
    byte[] bytes = this.BuildBodyItems(shape.TextBody.Items);
    string str = this.m_encoding.GetString(bytes, 0, bytes.Length);
    strBuilder.Append(str + "}");
    this.m_tableNestedLevel = tableNestedLevel;
    strBuilder.Append("}");
    if (shape.FallbackPic != null)
      strBuilder.Append(this.BuildPicture(shape.FallbackPic, true));
    strBuilder.Append(Environment.NewLine);
    return strBuilder.ToString();
  }

  private void BuildShapeBasicTokens(Shape shape, StringBuilder strBuilder)
  {
    if ((double) shape.Rotation >= 44.0 && (double) shape.Rotation <= 134.0 || (double) shape.Rotation >= 225.0 && (double) shape.Rotation <= 314.0)
    {
      float height = shape.Height;
      shape.Height = shape.Width;
      shape.Width = height;
      float num = Math.Abs(shape.Height - shape.Width) / 2f;
      if ((double) shape.Height < (double) shape.Width)
      {
        shape.HorizontalPosition -= num;
        shape.VerticalPosition += num;
      }
      if ((double) shape.Height > (double) shape.Width)
      {
        shape.VerticalPosition -= num;
        shape.HorizontalPosition += num;
      }
    }
    int shapeW = (int) Math.Round((double) shape.Width * 20.0);
    int shapeH = (int) Math.Round((double) shape.Height * 20.0);
    strBuilder.Append(this.BuildShapePosition(shape.HorizontalPosition, shape.VerticalPosition, shapeW, shapeH));
    if (shape.HorizontalOrigin == HorizontalOrigin.Page)
      strBuilder.Append(this.c_slashSymbol + "shpbxpage");
    else if (shape.HorizontalOrigin == HorizontalOrigin.Margin || shape.HorizontalOrigin == HorizontalOrigin.LeftMargin || shape.HorizontalOrigin == HorizontalOrigin.RightMargin || shape.HorizontalOrigin == HorizontalOrigin.InsideMargin || shape.HorizontalOrigin == HorizontalOrigin.OutsideMargin)
      strBuilder.Append(this.c_slashSymbol + "shpbxmargin");
    else if (shape.HorizontalOrigin == HorizontalOrigin.Column)
      strBuilder.Append(this.c_slashSymbol + "shpbxcolumn");
    if (shape.HorizontalOrigin == HorizontalOrigin.Character || shape.HorizontalOrigin.ToString().Contains("Margin") || shape.IsRelativeHeight || shape.IsRelativeHorizontalPosition)
      strBuilder.Append(this.c_slashSymbol + "shpbxignore");
    if (shape.VerticalOrigin == VerticalOrigin.Page)
      strBuilder.Append(this.c_slashSymbol + "shpbypage");
    else if (shape.VerticalOrigin == VerticalOrigin.Margin)
      strBuilder.Append(this.c_slashSymbol + "shpbymargin");
    else if (shape.VerticalOrigin == VerticalOrigin.Paragraph)
      strBuilder.Append(this.c_slashSymbol + "shpbypara");
    if (shape.VerticalOrigin.ToString().Contains("Margin") || shape.IsRelativeWidth || shape.IsRelativeVerticalPosition)
      strBuilder.Append(this.c_slashSymbol + "shpbyignore");
    strBuilder.Append(this.BuildWrappingStyle(shape.WrapFormat.TextWrappingStyle, shape.WrapFormat.TextWrappingType));
    strBuilder.Append(this.c_slashSymbol + "shpz");
    strBuilder.Append(shape.ZOrderPosition);
    strBuilder.Append(this.c_slashSymbol + "shplid");
    strBuilder.Append(shape.ShapeID);
    if (shape.LockAnchor)
      strBuilder.Append(this.c_slashSymbol + "shplockanchor");
    strBuilder.Append(this.BuildShapeProp("shapeType", this.GetAutoShapeType(shape.AutoShapeType.ToString()).ToString()));
  }

  private void BuildShapePositionTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    strBuilder.Append(this.BuildHorAlignm(shape.HorizontalAlignment));
    strBuilder.Append(this.BuildVertAlignm(shape.VerticalAlignment));
    if (shape.IsRelativeHorizontalPosition)
      strBuilder.Append(this.BuildShapeProp("posrelh", ((int) shape.RelativeHorizontalOrigin).ToString()));
    else
      strBuilder.Append(this.BuildShapeProp("posrelh", ((int) shape.HorizontalOrigin).ToString()));
    if (shape.IsRelativeVerticalPosition)
      strBuilder.Append(this.BuildShapeProp("posrelv", ((int) shape.RelativeHeightVerticalOrigin).ToString()));
    else
      strBuilder.Append(this.BuildShapeProp("posrelv", ((int) shape.VerticalOrigin).ToString()));
    if ((double) shape.RelativeWidth != 0.0)
      strBuilder.Append(this.BuildShapeProp("pctHoriz", (shape.RelativeWidth * 10f).ToString()));
    if ((double) shape.RelativeHeight != 0.0)
      strBuilder.Append(this.BuildShapeProp("pctVert", (shape.RelativeHeight * 10f).ToString()));
    if ((double) shape.RelativeVerticalPosition != 0.0)
      strBuilder.Append(this.BuildShapeProp("pctVertPos", (shape.RelativeVerticalPosition * 10f).ToString()));
    if ((double) shape.RelativeHorizontalPosition != 0.0)
      strBuilder.Append(this.BuildShapeProp("pctHorizPos", (shape.RelativeHorizontalPosition * 10f).ToString()));
    if (shape.IsRelativeHeight)
      strBuilder.Append(this.BuildShapeProp("sizerelv", ((int) shape.RelativeHeightVerticalOrigin).ToString()));
    if (shape.IsRelativeWidth)
      strBuilder.Append(this.BuildShapeProp("sizerelh", ((int) shape.RelativeWidthHorizontalOrigin).ToString()));
    if (!shape.WrapFormat.AllowOverlap)
      strBuilder.Append(this.BuildShapeProp("fAllowOverlap", "0"));
    if (shape.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      return;
    strBuilder.Append(this.BuildShapeProp("fPseudoInline", "1"));
  }

  private void BuildShapeObjectTypeTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    double num = (double) shape.Rotation * 65536.0;
    if (num != 0.0)
      strBuilder.Append(this.BuildShapeProp("rotation", num.ToString()));
    strBuilder.Append(this.BuildShapeProp("fFlipV", !shape.FlipVertical ? "0" : "1"));
    strBuilder.Append(this.BuildShapeProp("fFlipH", !shape.FlipHorizontal ? "0" : "1"));
    if (shape.WrapFormat.WrapPolygon.Vertices.Count != 0)
    {
      strBuilder.Append($"{{{this.c_slashSymbol}sp");
      strBuilder.Append($"{{{this.c_slashSymbol}sn ");
      strBuilder.Append("pWrapPolygonVertices");
      strBuilder.Append("}");
      strBuilder.Append($"{{{this.c_slashSymbol}sv ");
      foreach (PointF vertex in shape.WrapFormat.WrapPolygon.Vertices)
      {
        string str = vertex.ToString().Replace("{X=", "(").Replace("}", ");").Replace("Y=", (string) null);
        strBuilder.Append(str);
      }
      strBuilder.Append("}");
      strBuilder.Append("}");
    }
    if ((double) shape.WrapFormat.DistanceLeft != 0.0)
      strBuilder.Append(this.BuildShapeProp("dxWrapDistLeft", Math.Round((double) shape.WrapFormat.DistanceLeft * 12700.0, 2).ToString()));
    if ((double) shape.WrapFormat.DistanceTop != 0.0)
      strBuilder.Append(this.BuildShapeProp("dyWrapDistTop", Math.Round((double) shape.WrapFormat.DistanceTop * 12700.0, 2).ToString()));
    if ((double) shape.WrapFormat.DistanceRight != 0.0)
      strBuilder.Append(this.BuildShapeProp("dxWrapDistRight", Math.Round((double) shape.WrapFormat.DistanceRight * 12700.0, 2).ToString()));
    if ((double) shape.WrapFormat.DistanceBottom != 0.0)
      strBuilder.Append(this.BuildShapeProp("dyWrapDistBottom", Math.Round((double) shape.WrapFormat.DistanceBottom * 12700.0, 2).ToString()));
    if (shape.WrapFormat.TextWrappingStyle == TextWrappingStyle.Behind)
      strBuilder.Append(this.BuildShapeProp("fBehindDocument", "1"));
    if (shape.WrapFormat.WrapPolygon.Edited)
      strBuilder.Append(this.BuildShapeProp("fEditedWrap", "1"));
    if (shape.Visible)
      return;
    strBuilder.Append(this.BuildShapeProp("fHidden", "1"));
  }

  private void BuildShapeHorizontalLineTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    switch (shape.HorizontalAlignment)
    {
      case ShapeHorizontalAlignment.Left:
        strBuilder.Append(this.BuildShapeProp("alignHR", "0"));
        break;
      case ShapeHorizontalAlignment.Center:
        strBuilder.Append(this.BuildShapeProp("alignHR", "1"));
        break;
      case ShapeHorizontalAlignment.Right:
        strBuilder.Append(this.BuildShapeProp("alignHR", "2"));
        break;
    }
    if (shape.IsHorizontalRule)
      strBuilder.Append(this.BuildShapeProp("fHorizRule", shape.IsHorizontalRule ? "1" : "0"));
    if (shape.UseStandardColorHR)
      strBuilder.Append(this.BuildShapeProp("fStandardHR", shape.UseStandardColorHR ? "1" : "0"));
    if (shape.UseNoShadeHR)
      strBuilder.Append(this.BuildShapeProp("fNoShadeHR", shape.UseNoShadeHR ? "1" : "0"));
    if ((double) shape.WidthScale == 0.0)
      return;
    strBuilder.Append(this.BuildShapeProp("pctHR", (shape.WidthScale * 10f).ToString()));
  }

  private void BuildShapeLineTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    strBuilder.Append(this.BuildShapeProp("fLine", shape.LineFormat.Line ? "1" : "0"));
    if (!shape.LineFormat.Line)
      return;
    strBuilder.Append(this.BuildShapeProp("lineColor", this.GetRTFAutoShapeColor(shape.LineFormat.Color)));
    strBuilder.Append(this.BuildShapeProp("lineBackColor", shape.LineFormat.ForeColor.ToArgb().ToString()));
    if (shape.LineFormat.LineFormatType != LineFormatType.Solid)
      strBuilder.Append(this.BuildShapeProp("lineType", ((int) (shape.LineFormat.LineFormatType - 1)).ToString()));
    if ((double) shape.LineFormat.Weight != 0.0)
      strBuilder.Append(this.BuildShapeProp("lineWidth", (shape.LineFormat.Weight * 12700f).ToString()));
    if (shape.LineFormat.LineJoin != LineJoin.Round)
      strBuilder.Append(this.BuildShapeProp("lineJoinStyle", ((int) shape.LineFormat.LineJoin).ToString()));
    if (shape.LineFormat.MiterJoinLimit != null)
      strBuilder.Append(this.BuildShapeProp("MiterJoinLimit", shape.LineFormat.MiterJoinLimit));
    if (shape.LineFormat.Style != LineStyle.Single)
      strBuilder.Append(this.BuildShapeProp("lineStyle", this.GetLineStyle(shape.LineFormat.Style.ToString()).ToString()));
    if (shape.LineFormat.DashStyle != LineDashing.Solid)
      strBuilder.Append(this.BuildShapeProp("lineDashing", ((int) shape.LineFormat.DashStyle).ToString()));
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowheadNone)
      strBuilder.Append(this.BuildShapeProp("lineStartArrowhead", ((int) shape.LineFormat.BeginArrowheadStyle).ToString()));
    if (shape.LineFormat.EndArrowheadStyle != ArrowheadStyle.ArrowheadNone)
      strBuilder.Append(this.BuildShapeProp("lineEndArrowhead", ((int) shape.LineFormat.EndArrowheadStyle).ToString()));
    if (shape.LineFormat.BeginArrowheadWidth != LineEndWidth.MediumWidthArrow)
      strBuilder.Append(this.BuildShapeProp("lineStartArrowWidth", ((int) shape.LineFormat.BeginArrowheadWidth).ToString()));
    if (shape.LineFormat.BeginArrowheadLength != LineEndLength.MediumLenArrow)
      strBuilder.Append(this.BuildShapeProp("lineStartArrowLength", ((int) shape.LineFormat.BeginArrowheadLength).ToString()));
    if (shape.LineFormat.LineCap != LineCap.Flat)
      strBuilder.Append(this.BuildShapeProp("lineEndCapStyle", ((int) shape.LineFormat.LineCap).ToString()));
    if (shape.LineFormat.EndArrowheadWidth != LineEndWidth.MediumWidthArrow)
      strBuilder.Append(this.BuildShapeProp("lineEndArrowWidth", ((int) shape.LineFormat.EndArrowheadWidth).ToString()));
    if (shape.LineFormat.EndArrowheadLength != LineEndLength.MediumLenArrow)
      strBuilder.Append(this.BuildShapeProp("lineEndArrowLength", ((int) shape.LineFormat.EndArrowheadLength).ToString()));
    if ((double) shape.LineFormat.Transparency == 0.0)
      return;
    strBuilder.Append(this.BuildShapeProp("lineOpacity", shape.LineFormat.Transparency.ToString()));
  }

  private void BuildShapeFillTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    if (shape.FillFormat.Fill)
    {
      strBuilder.Append(this.BuildShapeProp("fillType", shape.FillFormat.FillType.ToString()));
      strBuilder.Append(this.BuildShapeProp("fillColor", this.GetRTFAutoShapeColor(shape.FillFormat.Color)));
      if (!shape.FillFormat.ForeColor.IsEmpty)
        strBuilder.Append(this.BuildShapeProp("fillBackColor", shape.FillFormat.ForeColor.ToString()));
    }
    if ((double) shape.FillFormat.Transparency != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillOpacity", Convert.ToUInt32((float) (65536.0 * (1.0 - (double) shape.FillFormat.Transparency / 100.0))).ToString()));
    if ((double) shape.FillFormat.SecondaryOpacity != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillBackOpacity", shape.FillFormat.SecondaryOpacity.ToString()));
    if ((double) shape.FillFormat.Focus != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillFocus", shape.FillFormat.Focus.ToString()));
    if (shape.FillFormat.TextureHorizontalScale != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillAngle", shape.FillFormat.TextureHorizontalScale.ToString()));
    strBuilder.Append(this.BuildShapeProp("fRecolorFillAsPicture", shape.FillFormat.ReColor ? "1" : "0"));
    if ((double) shape.FillFormat.FillRectangle.BottomOffset != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillRectBottom", shape.FillFormat.FillRectangle.BottomOffset.ToString()));
    if ((double) shape.FillFormat.FillRectangle.RightOffset != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillRectRight", shape.FillFormat.FillRectangle.RightOffset.ToString()));
    if ((double) shape.FillFormat.FillRectangle.LeftOffset != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillRectLeft", shape.FillFormat.FillRectangle.LeftOffset.ToString()));
    if ((double) shape.FillFormat.FillRectangle.TopOffset != 0.0)
      strBuilder.Append(this.BuildShapeProp("fillRectTop", shape.FillFormat.FillRectangle.TopOffset.ToString()));
    if (shape.FillFormat.Fill || !(shape.FillFormat.Color == Color.Empty))
      return;
    strBuilder.Append(this.BuildShapeProp("fFilled", "0"));
  }

  private string GetRTFAutoShapeColor(Color color)
  {
    return (65536 /*0x010000*/ * (int) color.B + 256 /*0x0100*/ * (int) color.G + (int) color.R).ToString();
  }

  private void BuildShapeAdjustValuesTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
    {
      if (keyValuePair.Key.Contains("adjust"))
      {
        strBuilder.Append(this.BuildShapeProp(keyValuePair.Key, keyValuePair.Value));
      }
      else
      {
        this.ConvertDocxAdjustValues(shape, strBuilder);
        break;
      }
    }
  }

  private void BuildShapeShadowTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    if (shape.EffectList[0].ShadowFormat.ShadowType != ShadowType.Single)
      strBuilder.Append(this.BuildShapeProp("shadowType", ((int) shape.EffectList[0].ShadowFormat.ShadowType).ToString()));
    strBuilder.Append(this.BuildShapeProp("shadowColor", this.GetRTFAutoShapeColor(shape.EffectList[0].ShadowFormat.Color)));
    if ((double) shape.EffectList[0].ShadowFormat.Transparency != 0.0)
      strBuilder.Append(this.BuildShapeProp("shadowOpacity", "32768"));
    if ((double) shape.EffectList[0].ShadowFormat.NonChoiceShadowOffsetX != 0.0 || (double) shape.EffectList[0].ShadowFormat.NonChoiceShadowOffsetY != 0.0)
    {
      this.GenerateOffsetXandY(shape, strBuilder);
    }
    else
    {
      strBuilder.Append(this.BuildShapeProp("shadowOffsetX", shape.EffectList[0].ShadowFormat.ShadowOffsetX.ToString()));
      strBuilder.Append(this.BuildShapeProp("shadowOffsetY", shape.EffectList[0].ShadowFormat.ShadowOffsetY.ToString()));
    }
    strBuilder.Append(this.BuildShapeProp("shadowSecondOffsetX", shape.EffectList[0].ShadowFormat.ShadowOffset2X.ToString()));
    strBuilder.Append(this.BuildShapeProp("shadowSecondOffsetY", shape.EffectList[0].ShadowFormat.ShadowOffset2Y.ToString()));
    if (shape.EffectList[0].ShadowFormat.HorizontalSkewAngle != (short) 0)
      strBuilder.Append(this.BuildShapeProp("shadowScaleYToX", shape.EffectList[0].ShadowFormat.HorizontalSkewAngle.ToString()));
    if (shape.EffectList[0].ShadowFormat.VerticalSkewAngle != (short) 0)
      strBuilder.Append(this.BuildShapeProp("shadowScaleXToY", shape.EffectList[0].ShadowFormat.VerticalSkewAngle.ToString()));
    if (shape.EffectList[0].ShadowFormat.VerticalScalingFactor != 100.0)
      strBuilder.Append(this.BuildShapeProp("shadowScaleYToY", shape.EffectList[0].ShadowFormat.VerticalScalingFactor.ToString()));
    if (shape.EffectList[0].ShadowFormat.HorizontalScalingFactor != 100.0)
      strBuilder.Append(this.BuildShapeProp("shadowScaleXToX", shape.EffectList[0].ShadowFormat.HorizontalScalingFactor.ToString()));
    if (shape.EffectList[0].ShadowFormat.ShadowPerspectiveMatrix != null)
      strBuilder.Append(this.BuildShapeProp("shadowPerspectiveY", shape.EffectList[0].ShadowFormat.ShadowPerspectiveMatrix));
    strBuilder.Append(this.BuildShapeProp("shadowOriginX", shape.EffectList[0].ShadowFormat.OriginX.ToString()));
    strBuilder.Append(this.BuildShapeProp("ShadowOriginY", shape.EffectList[0].ShadowFormat.OriginY.ToString()));
    if (shape.EffectList[0].ShadowFormat.Obscured)
      strBuilder.Append(this.BuildShapeProp("fshadowObscured", shape.EffectList[0].ShadowFormat.Obscured.ToString()));
    if (!shape.EffectList[0].ShadowFormat.Visible)
      return;
    strBuilder.Append(this.BuildShapeProp("fShadow", (shape.EffectList[0].ShadowFormat.Visible ? (object) "1" : (object) "0").ToString()));
  }

  private void GenerateOffsetXandY(Shape shape, StringBuilder strBuilder)
  {
    if (shape.EffectList[0].ShadowFormat.NonChoiceShadowOffsetX.ToString().Contains("25400") && shape.EffectList[0].ShadowFormat.NonChoiceShadowOffsetY.ToString().Contains("25400"))
      return;
    double num1 = 12700.0 * (double) shape.EffectList[0].ShadowFormat.NonChoiceShadowOffsetX;
    double num2 = 12700.0 * (double) shape.EffectList[0].ShadowFormat.NonChoiceShadowOffsetY;
    strBuilder.Append(this.BuildShapeProp("shadowOffsetX", num1.ToString()));
    strBuilder.Append(this.BuildShapeProp("shadowOffsetY", num2.ToString()));
  }

  private void BuildShape3DTokens(Shape shape, StringBuilder strBuilder)
  {
    strBuilder.Append(Environment.NewLine);
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(61))
      strBuilder.Append(this.BuildShapeProp("c3DSpecularAmt", shape.EffectList[1].ThreeDFormat.Specularity.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(37))
      strBuilder.Append(this.BuildShapeProp("c3DDiffuseAmt", shape.EffectList[1].ThreeDFormat.Diffusity.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(58))
      strBuilder.Append(this.BuildShapeProp("c3DShininess", shape.EffectList[1].ThreeDFormat.Shininess.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(38))
      strBuilder.Append(this.BuildShapeProp("c3DEdgeThickness", shape.EffectList[1].ThreeDFormat.Edge.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(40))
      strBuilder.Append(this.BuildShapeProp("c3DExtrudeForward", shape.EffectList[1].ThreeDFormat.ForeDepth.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(74))
      strBuilder.Append(this.BuildShapeProp("c3DExtrudeBackward", Math.Round((double) shape.EffectList[1].ThreeDFormat.BackDepth * 12700.0).ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(50))
      strBuilder.Append(this.BuildExtrusionplane(shape.EffectList[1].ThreeDFormat.ExtrusionPlane));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(12))
      strBuilder.Append(this.BuildShapeProp("c3DExtrusionColor", this.GetRtfPageBackgroundColor(shape.EffectList[1].ThreeDFormat.ExtrusionColor)));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(36))
      strBuilder.Append(this.BuildShapeProp("c3DExtrusionColorExtMod", shape.EffectList[1].ThreeDFormat.ColorMode.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(67))
      strBuilder.Append(this.BuildShapeProp("f3D", !shape.EffectList[1].ThreeDFormat.Visible ? "0" : "1"));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(72))
      strBuilder.Append(this.BuildShapeProp("fc3DMetallic", !shape.EffectList[1].ThreeDFormat.Metal ? "0" : "1"));
    if (shape.EffectList[1].ThreeDFormat.HasExtrusionColor)
      strBuilder.Append(this.BuildShapeProp("fc3DUseExtrusionColor", "1"));
    if (!shape.EffectList[1].ThreeDFormat.HasLightRigEffect)
      strBuilder.Append(this.BuildShapeProp("fc3DLightFace", "0"));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(54))
      strBuilder.Append(this.BuildShapeProp("c3DYRotationAngle", shape.EffectList[1].ThreeDFormat.RotationAngleY.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(53))
      strBuilder.Append(this.BuildShapeProp("c3DXRotationAngle", shape.EffectList[1].ThreeDFormat.RotationAngleX.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(46))
      strBuilder.Append(this.BuildShapeProp("c3DRotationAxisX", shape.EffectList[1].ThreeDFormat.RotationX.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(47))
      strBuilder.Append(this.BuildShapeProp("c3DRotationAxisY", shape.EffectList[1].ThreeDFormat.RotationY.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(48 /*0x30*/))
      strBuilder.Append(this.BuildShapeProp("c3DRotationAxisZ", shape.EffectList[1].ThreeDFormat.RotationZ.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(49))
      strBuilder.Append(this.BuildShapeProp("c3DRotationAngle", shape.EffectList[1].ThreeDFormat.OrientationAngle.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(75))
      strBuilder.Append(this.BuildShapeProp("fc3DRotationCenterAuto", !shape.EffectList[1].ThreeDFormat.AutoRotationCenter ? "0" : "1"));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(55))
      strBuilder.Append(this.BuildShapeProp("c3DRotationCenterX", shape.EffectList[1].ThreeDFormat.RotationCenterX.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(56))
      strBuilder.Append(this.BuildShapeProp("c3DRotationCenterY", shape.EffectList[1].ThreeDFormat.RotationCenterY.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(57))
      strBuilder.Append(this.BuildShapeProp("c3DRotationCenterZ", shape.EffectList[1].ThreeDFormat.RotationCenterZ.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(52))
      strBuilder.Append(this.BuildRenderMode(shape.EffectList[1].ThreeDFormat.ExtrusionRenderMode));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(62))
      strBuilder.Append(this.BuildShapeProp("c3DXViewpoint", shape.EffectList[1].ThreeDFormat.ViewPointX.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(63 /*0x3F*/))
      strBuilder.Append(this.BuildShapeProp("c3DYViewpoint", shape.EffectList[1].ThreeDFormat.ViewPointY.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(64 /*0x40*/))
      strBuilder.Append(this.BuildShapeProp("c3DZViewpoint", shape.EffectList[1].ThreeDFormat.ViewPointZ.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(65))
      strBuilder.Append(this.BuildShapeProp("c3DOriginX", shape.EffectList[1].ThreeDFormat.ViewPointOriginX.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(66))
      strBuilder.Append(this.BuildShapeProp("c3DOriginY", shape.EffectList[1].ThreeDFormat.ViewPointOriginY.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(60))
      strBuilder.Append(this.BuildShapeProp("c3DSkewAngle", shape.EffectList[1].ThreeDFormat.SkewAngle.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(59))
      strBuilder.Append(this.BuildShapeProp("c3DSkewAmount", shape.EffectList[1].ThreeDFormat.SkewAmount.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(34))
      strBuilder.Append(this.BuildShapeProp("c3DAmbientIntensity", shape.EffectList[1].ThreeDFormat.Brightness.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(31 /*0x1F*/))
      strBuilder.Append(this.BuildShapeProp("c3DKeyX", shape.EffectList[1].ThreeDFormat.LightRigRotationX.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(32 /*0x20*/))
      strBuilder.Append(this.BuildShapeProp("c3DKeyY", shape.EffectList[1].ThreeDFormat.LightRigRotationY.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(33))
      strBuilder.Append(this.BuildShapeProp("c3DKeyZ", shape.EffectList[1].ThreeDFormat.LightRigRotationZ.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(41))
      strBuilder.Append(this.BuildShapeProp("c3DKeyIntensity", shape.EffectList[1].ThreeDFormat.LightLevel.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(43))
      strBuilder.Append(this.BuildShapeProp("c3DFillX", shape.EffectList[1].ThreeDFormat.LightRigRotation2X.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(44))
      strBuilder.Append(this.BuildShapeProp("c3DFillY", shape.EffectList[1].ThreeDFormat.LightRigRotation2Y.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(45))
      strBuilder.Append(this.BuildShapeProp("c3DFillZ", shape.EffectList[1].ThreeDFormat.LightRigRotation2Z.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(42))
      strBuilder.Append(this.BuildShapeProp("c3DFillIntensity", shape.EffectList[1].ThreeDFormat.LightLevel2.ToString()));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(69))
      strBuilder.Append(this.BuildShapeProp("fc3DKeyHarsh", !shape.EffectList[1].ThreeDFormat.LightHarsh ? "0" : "1"));
    if (shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(70))
      strBuilder.Append(this.BuildShapeProp("fc3DFillHarsh", !shape.EffectList[1].ThreeDFormat.LightHarsh2 ? "0" : "1"));
    if (!shape.EffectList[1].ThreeDFormat.PropertiesHash.ContainsKey(24))
      return;
    if (shape.EffectList[1].ThreeDFormat.CameraPresetType.ToString().ToLower().Contains("oblique"))
      strBuilder.Append(this.BuildShapeProp("fc3DParallel", "1"));
    else
      strBuilder.Append(this.BuildShapeProp("fc3DParallel", "0"));
  }

  private void ConvertDocxAdjustValues(Shape shape, StringBuilder strBuilder)
  {
    switch (shape.AutoShapeType)
    {
      case AutoShapeType.Parallelogram:
        if ((double) shape.Height < (double) shape.Width)
        {
          float num1 = shape.Width - shape.Height;
          double num2 = Math.Round(25000.0 / (double) shape.Height * (double) num1 + 25000.0);
          using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            KeyValuePair<string, string> current = enumerator.Current;
            if ((double) int.Parse(current.Value.Substring(4)) != num2)
            {
              double a = (double) int.Parse(current.Value.Substring(4)) / (4.62962963 / (double) shape.Height * (double) num1 + 4.62962963);
              strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
              break;
            }
            shape.ShapeGuide.Clear();
            break;
          }
        }
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          if (!enumerator.MoveNext())
            break;
          KeyValuePair<string, string> current = enumerator.Current;
          if (int.Parse(current.Value.Substring(4)) == 25000)
          {
            shape.ShapeGuide.Clear();
            break;
          }
          double a = (double) int.Parse(current.Value.Substring(4)) / 4.63;
          strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
          break;
        }
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Octagon:
      case AutoShapeType.IsoscelesTriangle:
      case AutoShapeType.Cross:
      case AutoShapeType.Cube:
      case AutoShapeType.Bevel:
      case AutoShapeType.Sun:
      case AutoShapeType.Moon:
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.Plaque:
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            double num = (double) int.Parse(enumerator.Current.Value.Substring(4)) / 4.63;
            strBuilder.Append(this.BuildShapeProp("adjustValue", num.ToString()));
          }
          break;
        }
      case AutoShapeType.Hexagon:
        if ((double) shape.Height < (double) shape.Width)
        {
          float num3 = shape.Width - shape.Height;
          double num4 = Math.Round(25000.0 / (double) shape.Height * (double) num3 + 25000.0);
          using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            KeyValuePair<string, string> current = enumerator.Current;
            if (num4 != (double) int.Parse(current.Value.Substring(4)))
            {
              double a = (double) int.Parse(current.Value.Substring(4)) / (4.62962963 / (double) shape.Height * (double) num3 + 4.62962963);
              strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
              break;
            }
            shape.ShapeGuide.Clear();
            break;
          }
        }
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          if (!enumerator.MoveNext())
            break;
          KeyValuePair<string, string> current = enumerator.Current;
          if (int.Parse(current.Value.Substring(4)) == 25000)
          {
            shape.ShapeGuide.Clear();
            break;
          }
          double num = (double) int.Parse(current.Value.Substring(4)) / 4.63;
          strBuilder.Append(this.BuildShapeProp("adjustValue", num.ToString()));
          break;
        }
      case AutoShapeType.Can:
        if ((double) shape.Height > (double) shape.Width)
        {
          float num5 = shape.Height - shape.Width;
          double num6 = Math.Round(25000.0 / (double) shape.Width * (double) num5 + 25000.0);
          using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, string> current = enumerator.Current;
              if (num6 != (double) int.Parse(current.Value.Substring(4)))
              {
                double a = (double) int.Parse(current.Value.Substring(4)) / (4.62962963 / (double) shape.Width * (double) num5 + 4.62962963);
                strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
                break;
              }
            }
            break;
          }
        }
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          if (!enumerator.MoveNext())
            break;
          KeyValuePair<string, string> current = enumerator.Current;
          if (int.Parse(current.Value.Substring(4)) == 25000)
          {
            shape.ShapeGuide.Clear();
            break;
          }
          double num = (double) int.Parse(current.Value.Substring(4)) / 4.63;
          strBuilder.Append(this.BuildShapeProp("adjustValue", num.ToString()));
          break;
        }
      case AutoShapeType.FoldedCorner:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            double num = Math.Round(Math.Abs((double) int.Parse(enumerator.Current.Value.Substring(4)) / 4.63 - 21600.0));
            strBuilder.Append(this.BuildShapeProp("adjustValue", num.ToString()));
          }
          break;
        }
      case AutoShapeType.SmileyFace:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            double num = (double) Math.Abs(int.Parse(enumerator.Current.Value.Substring(4)));
            if (num != 4653.0)
            {
              double a = Math.Abs((num + 4563.0) / 4.63 - 17520.0);
              strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
            }
          }
          break;
        }
      case AutoShapeType.LeftBracket:
      case AutoShapeType.RightBracket:
        if ((double) shape.Height > (double) shape.Width)
        {
          float num7 = shape.Height - shape.Width;
          double num8 = Math.Round((double) (8333f / shape.Width) * (double) num7 + 8333.0);
          using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, string> current = enumerator.Current;
              if (num8 != (double) int.Parse(current.Value.Substring(4)))
              {
                double a = (double) int.Parse(current.Value.Substring(4)) / (4.62962963 / (double) shape.Width * (double) num7 + 4.62962963);
                strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
              }
            }
            break;
          }
        }
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, string> current = enumerator.Current;
            if (current.Value.Substring(4) == "8333")
            {
              shape.ShapeGuide.Clear();
              break;
            }
            double a = (double) int.Parse(current.Value.Substring(4)) / 4.63;
            strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
          }
          break;
        }
      case AutoShapeType.LeftBrace:
      case AutoShapeType.RightBrace:
        if ((double) shape.Height > (double) shape.Width)
        {
          float num9 = shape.Height - shape.Width;
          double num10 = Math.Round((double) (8333f / shape.Width) * (double) num9 + 8333.0);
          using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, string> current = enumerator.Current;
              double num11;
              if (num10 != (double) int.Parse(current.Value.Substring(4)))
              {
                if (current.Key == "adj1")
                {
                  double a = (double) int.Parse(current.Value.Substring(4)) / (4.62962963 / (double) shape.Width * (double) num9 + 4.62962963);
                  StringBuilder stringBuilder = strBuilder;
                  num11 = Math.Round(a);
                  string str = this.BuildShapeProp("adjustValue", num11.ToString());
                  stringBuilder.Append(str);
                }
                else
                {
                  double a = (double) int.Parse(current.Value.Substring(4)) / 4.63;
                  StringBuilder stringBuilder = strBuilder;
                  num11 = Math.Round(a);
                  string str = this.BuildShapeProp("adjust2Value", num11.ToString());
                  stringBuilder.Append(str);
                }
              }
            }
            break;
          }
        }
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, string> current = enumerator.Current;
            if (current.Value.Substring(4) == "8333")
            {
              shape.ShapeGuide.Clear();
              break;
            }
            double a = (double) int.Parse(current.Value.Substring(4)) / 4.63;
            double num;
            if (current.Key == "adj1")
            {
              StringBuilder stringBuilder = strBuilder;
              num = Math.Round(a);
              string str = this.BuildShapeProp("adjustValue", num.ToString());
              stringBuilder.Append(str);
            }
            else if (current.Key == "adj2" && current.Value.Substring(4) == "50000")
            {
              StringBuilder stringBuilder = strBuilder;
              num = Math.Round(a);
              string str = this.BuildShapeProp("adjust2Value", num.ToString());
              stringBuilder.Append(str);
            }
          }
          break;
        }
      case AutoShapeType.Star4Point:
      case AutoShapeType.Star8Point:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            double a = (double) (50000 - int.Parse(enumerator.Current.Value.Substring(4))) / 4.62962963;
            strBuilder.Append(this.BuildShapeProp("adjustValue", Math.Round(a).ToString()));
          }
          break;
        }
      case AutoShapeType.UpRibbon:
        double num12 = -1.0;
        double num13 = -1.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          float num14 = (float) int.Parse(keyValuePair.Value.Substring(4));
          if (keyValuePair.Key == "adj2")
            num12 = Math.Round(2700.0 + Math.Abs(((double) num14 - 75000.0) / 9.25925926));
          else
            num13 = Math.Round(21600.0 - (double) num14 / 4.62962963);
        }
        if (num12 >= 0.0)
          strBuilder.Append(this.BuildShapeProp("adjustValue", num12.ToString()));
        if (num13 < 0.0)
          break;
        strBuilder.Append(this.BuildShapeProp("adjust2Value", num13.ToString()));
        break;
      case AutoShapeType.DownRibbon:
        double num15 = -1.0;
        double num16 = -1.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          if (keyValuePair.Value != "val 12500" && keyValuePair.Value != "val 50000")
          {
            double num17 = Math.Round(2700.0 + Math.Abs(((double) int.Parse(keyValuePair.Value.Substring(4)) - 75000.0) / 9.25925926));
            if (keyValuePair.Key == "adj2")
              num15 = num17;
            else
              num16 = num17;
          }
        }
        if (num15 >= 0.0)
          strBuilder.Append(this.BuildShapeProp("adjustValue", num15.ToString()));
        if (num16 < 0.0)
          break;
        strBuilder.Append(this.BuildShapeProp("adjust2Value", num16.ToString()));
        break;
      case AutoShapeType.CurvedUpRibbon:
        double num18 = -1.0;
        double num19 = -1.0;
        double num20 = -1.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          float num21 = (float) int.Parse(keyValuePair.Value.Substring(4));
          if (keyValuePair.Key == "adj1" && (double) num21 != 25000.0)
            num19 = Math.Round(12600.0 + (41667.0 - (double) num21) / 4.62962963);
          else if (keyValuePair.Key == "adj2" && (double) num21 != 50000.0)
            num18 = Math.Round(2700.0 + (75000.0 - (double) num21) / 9.25925926);
          else if (keyValuePair.Key == "adj3" && (double) num21 != 12500.0)
            num20 = Math.Round((double) num21 / 4.62962963);
        }
        if (num18 >= 0.0)
          strBuilder.Append(this.BuildShapeProp("adjustValue", num18.ToString()));
        if (num19 >= 0.0)
          strBuilder.Append(this.BuildShapeProp("adjust2Value", num19.ToString()));
        if (num20 < 0.0)
          break;
        strBuilder.Append(this.BuildShapeProp("adjust3Value", num20.ToString()));
        break;
      case AutoShapeType.CurvedDownRibbon:
        double num22 = -1.0;
        double num23 = -1.0;
        double num24 = -1.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          float num25 = (float) int.Parse(keyValuePair.Value.Substring(4));
          if (keyValuePair.Key == "adj1" && (double) num25 != 25000.0)
            num23 = Math.Round(9000.0 - (41667.0 - (double) num25) / 4.62962963);
          else if (keyValuePair.Key == "adj2" && (double) num25 != 50000.0)
            num22 = Math.Round(2700.0 + (75000.0 - (double) num25) / 9.25925926);
          else if (keyValuePair.Key == "adj3" && (double) num25 != 12500.0)
            num24 = Math.Round(20925.0 - ((double) num25 - 3125.0) / 4.62962963);
        }
        if (num22 >= 0.0)
          strBuilder.Append(this.BuildShapeProp("adjustValue", num22.ToString()));
        if (num23 >= 0.0)
          strBuilder.Append(this.BuildShapeProp("adjust2Value", num23.ToString()));
        if (num24 < 0.0)
          break;
        strBuilder.Append(this.BuildShapeProp("adjust3Value", num24.ToString()));
        break;
      case AutoShapeType.VerticalScroll:
      case AutoShapeType.HorizontalScroll:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            double num26 = Math.Round((double) int.Parse(enumerator.Current.Value.Substring(4)) / 4.62962963);
            strBuilder.Append(this.BuildShapeProp("adjustValue", num26.ToString()));
          }
          break;
        }
      case AutoShapeType.Wave:
      case AutoShapeType.DoubleWave:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, string> current = enumerator.Current;
            float num27 = (float) int.Parse(current.Value.Substring(4));
            if (current.Key == "adj1")
            {
              double num28 = Math.Round((double) num27 / 4.62962963);
              strBuilder.Append(this.BuildShapeProp("adjustValue", num28.ToString()));
            }
            else
            {
              double num29 = 10800.0 + Math.Round((double) num27 / 4.62962963);
              strBuilder.Append(this.BuildShapeProp("adjustValue", num29.ToString()));
            }
          }
          break;
        }
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.RoundedRectangularCallout:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.CloudCallout:
        using (Dictionary<string, string>.Enumerator enumerator = shape.ShapeGuide.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, string> current = enumerator.Current;
            double num30 = Math.Round(((double) int.Parse(current.Value.Substring(4)) + 50000.0) / 4.62962963);
            if (current.Key == "adj1")
              strBuilder.Append(this.BuildShapeProp("adjustValue", num30.ToString()));
            else if (current.Key == "adj2")
              strBuilder.Append(this.BuildShapeProp("adjust2Value", num30.ToString()));
          }
          break;
        }
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
      case AutoShapeType.LineCallout1AccentBar:
        double num31 = 0.0;
        double num32 = 0.0;
        double num33 = 0.0;
        double num34 = 0.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          double num35 = Math.Round((double) int.Parse(keyValuePair.Value.Substring(4)) / 4.62962963);
          if (keyValuePair.Key == "adj1")
            num34 = num35;
          else if (keyValuePair.Key == "adj2")
            num33 = num35;
          else if (keyValuePair.Key == "adj3")
            num32 = num35;
          else if (keyValuePair.Key == "adj4")
            num31 = num35;
        }
        strBuilder.Append(this.BuildShapeProp("adjustValue", num31.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust2Value", num32.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust3Value", num33.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust4Value", num34.ToString()));
        break;
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2NoBorder:
        double num36 = 0.0;
        double num37 = 0.0;
        double num38 = 0.0;
        double num39 = 0.0;
        double num40 = 0.0;
        double num41 = 0.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          double num42 = Math.Round((double) int.Parse(keyValuePair.Value.Substring(4)) / 4.62962963);
          if (keyValuePair.Key == "adj1")
            num41 = num42;
          else if (keyValuePair.Key == "adj2")
            num40 = num42;
          else if (keyValuePair.Key == "adj3")
            num39 = num42;
          else if (keyValuePair.Key == "adj4")
            num38 = num42;
          else if (keyValuePair.Key == "adj5")
            num37 = num42;
          else if (keyValuePair.Key == "adj6")
            num36 = num42;
        }
        strBuilder.Append(this.BuildShapeProp("adjustValue", num36.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust2Value", num37.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust3Value", num38.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust4Value", num39.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust5Value", num40.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust6Value", num41.ToString()));
        break;
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3NoBorder:
        double num43 = 0.0;
        double num44 = 0.0;
        double num45 = 0.0;
        double num46 = 0.0;
        double num47 = 0.0;
        double num48 = 0.0;
        double num49 = 0.0;
        double num50 = 0.0;
        foreach (KeyValuePair<string, string> keyValuePair in shape.ShapeGuide)
        {
          double num51 = Math.Round((double) int.Parse(keyValuePair.Value.Substring(4)) / 4.62962963);
          if (keyValuePair.Key == "adj1")
            num50 = num51;
          else if (keyValuePair.Key == "adj2")
            num49 = num51;
          else if (keyValuePair.Key == "adj3")
            num48 = num51;
          else if (keyValuePair.Key == "adj4")
            num47 = num51;
          else if (keyValuePair.Key == "adj5")
            num46 = num51;
          else if (keyValuePair.Key == "adj6")
            num45 = num51;
          else if (keyValuePair.Key == "adj7")
            num44 = num51;
          else if (keyValuePair.Key == "adj8")
            num43 = num51;
        }
        strBuilder.Append(this.BuildShapeProp("adjustValue", num43.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust2Value", num44.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust3Value", num45.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust4Value", num46.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust5Value", num47.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust6Value", num48.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust7Value", num49.ToString()));
        strBuilder.Append(this.BuildShapeProp("adjust8Value", num50.ToString()));
        break;
    }
  }

  private string BuildRenderMode(ExtrusionRenderMode extrusionRenderMode)
  {
    switch (extrusionRenderMode)
    {
      case ExtrusionRenderMode.Solid:
        return this.BuildShapeProp("c3DRenderMode", "0");
      case ExtrusionRenderMode.Wireframe:
        return this.BuildShapeProp("c3DRenderMode", "1");
      case ExtrusionRenderMode.BoundingCube:
        return this.BuildShapeProp("c3DRenderMode", "2");
      default:
        return string.Empty;
    }
  }

  private string BuildExtrusionplane(ExtrusionPlane extrusionPlane)
  {
    switch (extrusionPlane)
    {
      case ExtrusionPlane.XY:
        return this.BuildShapeProp("c3DExtrudePlane", "0");
      case ExtrusionPlane.YZ:
        return this.BuildShapeProp("c3DExtrudePlane", "1");
      case ExtrusionPlane.ZX:
        return this.BuildShapeProp("c3DExtrudePlane", "2");
      default:
        return string.Empty;
    }
  }

  private bool IsWmfImage(WPicture picture)
  {
    if (WordDocument.EnablePartialTrustCode)
    {
      if (picture.ImageForPartialTrustMode.Format == Syncfusion.DocIO.DLS.Entities.ImageFormat.Wmf)
        return true;
      return picture.PictureShape != null && picture.PictureShape.ShapeContainer != null && picture.PictureShape.ShapeContainer.Bse != null && picture.PictureShape.ShapeContainer.Bse.Blip != null && picture.PictureShape.ShapeContainer.Bse.Blip != null && picture.PictureShape.ShapeContainer.Bse.Blip.Header != null && picture.PictureShape.ShapeContainer.Bse.Blip.Header.Type == MSOFBT.msofbtBlipWMF;
    }
    if (picture.ImageRecord.ImageFormat == System.Drawing.Imaging.ImageFormat.Wmf || picture.ImageRecord.ImageFormat.Guid.ToString() == "B96B3CAD-0728-11D3-9D7B-0000F81EF32E")
      return true;
    return picture.PictureShape != null && picture.PictureShape.ShapeContainer != null && picture.PictureShape.ShapeContainer.Bse != null && picture.PictureShape.ShapeContainer.Bse.Blip != null && picture.PictureShape.ShapeContainer.Bse.Blip != null && picture.PictureShape.ShapeContainer.Bse.Blip.Header != null && picture.PictureShape.ShapeContainer.Bse.Blip.Header.Type == MSOFBT.msofbtBlipWMF;
  }

  private string BuildPictureProp(WPicture pic, bool isFieldshape)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append($"{{{this.c_slashSymbol}pict");
    if (isFieldshape)
      stringBuilder1.Append("{\\*\\picprop\\defshp{\\sp{\\sn fPseudoInline}{\\sv 1}}{\\sp{\\sn fLayoutInCell}{\\sv 1}}{\\sp{\\sn fLockPosition}{\\sv 1}}{\\sp{\\sn fLockRotation}{\\sv 1}}}");
    stringBuilder1.Append(this.c_slashSymbol + "picscalex");
    stringBuilder1.Append(Math.Round((double) pic.WidthScale));
    stringBuilder1.Append(this.c_slashSymbol + "picscaley");
    stringBuilder1.Append(Math.Round((double) pic.HeightScale));
    stringBuilder1.Append(this.c_slashSymbol + "picwgoal");
    stringBuilder1.Append(Math.Round((double) pic.Size.Width * 20.0));
    stringBuilder1.Append(this.c_slashSymbol + "pichgoal");
    stringBuilder1.Append(Math.Round((double) pic.Size.Height * 20.0));
    stringBuilder1.Append(this.c_slashSymbol + "picw");
    stringBuilder1.Append(Math.Round((double) pic.Size.Width * 35.5));
    stringBuilder1.Append(this.c_slashSymbol + "pich");
    stringBuilder1.Append(Math.Round((double) pic.Size.Height * 35.5));
    if (pic.IsMetaFile)
      stringBuilder1.Append(this.c_slashSymbol + "wmetafile8");
    else if (pic.Image.RawFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Png))
      stringBuilder1.Append(this.c_slashSymbol + "pngblip");
    else
      stringBuilder1.Append(this.c_slashSymbol + "jpegblip");
    stringBuilder1.Append(" ");
    byte[] numArray = pic.ImageBytes;
    if (pic.ImageRecord != null && pic.IsMetaFile && this.IsWmfImage(pic) && pic.ImageRecord.IsMetafileHeaderPresent(pic.ImageBytes))
    {
      byte[] dst = new byte[pic.ImageBytes.Length - 22];
      Buffer.BlockCopy((Array) pic.ImageBytes, 22, (Array) dst, 0, pic.ImageBytes.Length - 22);
      numArray = dst;
    }
    StringBuilder stringBuilder2 = new StringBuilder(BitConverter.ToString(numArray));
    stringBuilder2.Replace("-", "");
    stringBuilder1.Append((object) stringBuilder2);
    stringBuilder1.Append("}");
    stringBuilder1.Append(Environment.NewLine);
    return stringBuilder1.ToString();
  }

  private string BuildPictureLink(WPicture pic)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}hl");
    stringBuilder.Append($"{{{this.c_slashSymbol}hlfr ");
    stringBuilder.Append(pic.Href);
    stringBuilder.Append("}");
    stringBuilder.Append($"{{{this.c_slashSymbol}hlsrc ");
    stringBuilder.Append(pic.Href);
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private string BuildMetafileProp(WPicture pic)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append($"{{{this.c_slashSymbol}pict");
    stringBuilder1.Append(this.c_slashSymbol + "picscalex");
    stringBuilder1.Append(Math.Round((double) pic.WidthScale));
    stringBuilder1.Append(this.c_slashSymbol + "picscaley");
    stringBuilder1.Append(Math.Round((double) pic.HeightScale));
    stringBuilder1.Append(this.c_slashSymbol + "picwgoal");
    stringBuilder1.Append(Math.Round((double) pic.Size.Width * 20.0));
    stringBuilder1.Append(this.c_slashSymbol + "pichgoal");
    stringBuilder1.Append(Math.Round((double) pic.Size.Height * 20.0));
    stringBuilder1.Append(this.c_slashSymbol + "picw");
    stringBuilder1.Append(Math.Round((double) pic.Size.Width * 35.5));
    stringBuilder1.Append(this.c_slashSymbol + "pich");
    stringBuilder1.Append(Math.Round((double) pic.Size.Height * 35.5));
    stringBuilder1.Append(this.c_slashSymbol + "wmetafile8");
    stringBuilder1.Append(" ");
    byte[] numArray;
    if (pic.IsMetaFile && this.IsWmfImage(pic))
    {
      numArray = pic.ImageBytes;
      if (pic.ImageRecord.IsMetafileHeaderPresent(pic.ImageBytes))
      {
        byte[] dst = new byte[pic.ImageBytes.Length - 22];
        Buffer.BlockCopy((Array) pic.ImageBytes, 22, (Array) dst, 0, pic.ImageBytes.Length - 22);
        numArray = dst;
      }
    }
    else
      numArray = !WordDocument.EnablePartialTrustCode ? this.GetRtfImage(pic.GetImage(pic.ImageBytes, false)) : pic.ImageBytes;
    if (numArray != null)
    {
      StringBuilder stringBuilder2 = new StringBuilder(BitConverter.ToString(numArray));
      stringBuilder2.Replace("-", "");
      stringBuilder1.Append((object) stringBuilder2);
    }
    stringBuilder1.Append("}");
    stringBuilder1.Append(Environment.NewLine);
    return stringBuilder1.ToString();
  }

  private byte[] GetRtfImage(System.Drawing.Image image)
  {
    MemoryStream memoryStream = (MemoryStream) null;
    Graphics graphics = (Graphics) null;
    System.Drawing.Imaging.Metafile metafile = (System.Drawing.Imaging.Metafile) null;
    try
    {
      memoryStream = new MemoryStream();
      using (graphics = Graphics.FromImage((System.Drawing.Image) new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb)))
      {
        IntPtr hdc = graphics.GetHdc();
        metafile = new System.Drawing.Imaging.Metafile((Stream) memoryStream, hdc);
        graphics.ReleaseHdc(hdc);
      }
      using (graphics = Graphics.FromImage((System.Drawing.Image) metafile))
        graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
      IntPtr henhmetafile = metafile.GetHenhmetafile();
      try
      {
        uint wmfBits1 = RtfWriter.GdipEmfToWmfBits(henhmetafile, 0U, (byte[]) null, 8, RtfNavigator.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
        byte[] buffer = new byte[(IntPtr) wmfBits1];
        int wmfBits2 = (int) RtfWriter.GdipEmfToWmfBits(henhmetafile, wmfBits1, buffer, 8, RtfNavigator.EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
        return buffer;
      }
      finally
      {
        RtfWriter.DeleteEnhMetaFile(henhmetafile);
      }
    }
    finally
    {
      graphics?.Dispose();
      metafile?.Dispose();
      memoryStream?.Close();
    }
  }

  [DllImport("gdiplus.dll")]
  private static extern uint GdipEmfToWmfBits(
    IntPtr hEmf,
    uint bufferSize,
    byte[] buffer,
    int mappingMode,
    RtfNavigator.EmfToWmfBitsFlags flags);

  [DllImport("gdi32.dll")]
  private static extern bool DeleteEnhMetaFile(IntPtr hemf);

  private string BuildWrappingStyle(TextWrappingStyle style, TextWrappingType type)
  {
    switch (style)
    {
      case TextWrappingStyle.Inline:
        return $"{this.c_slashSymbol}shpwr3{this.BuildWrappingType(type)}\\shpfblwtxt1";
      case TextWrappingStyle.TopAndBottom:
        return $"{this.c_slashSymbol}shpwr1{this.BuildWrappingType(type)}";
      case TextWrappingStyle.Square:
        return $"{this.c_slashSymbol}shpwr2{this.BuildWrappingType(type)}";
      case TextWrappingStyle.InFrontOfText:
        return $"{this.c_slashSymbol}shpwr3{this.BuildWrappingType(type)}\\shpfblwtxt0";
      case TextWrappingStyle.Tight:
        return $"{this.c_slashSymbol}shpwr4{this.BuildWrappingType(type)}";
      case TextWrappingStyle.Through:
        return $"{this.c_slashSymbol}shpwr5{this.BuildWrappingType(type)}";
      case TextWrappingStyle.Behind:
        return $"{this.c_slashSymbol}shpwr3{this.BuildWrappingType(type)}\\shpfblwtxt1";
      default:
        return string.Empty;
    }
  }

  private string BuildWrappingType(TextWrappingType type)
  {
    switch (type)
    {
      case TextWrappingType.Both:
        return this.c_slashSymbol + "shpwrk0";
      case TextWrappingType.Left:
        return this.c_slashSymbol + "shpwrk1";
      case TextWrappingType.Right:
        return this.c_slashSymbol + "shpwrk2";
      case TextWrappingType.Largest:
        return this.c_slashSymbol + "shpwrk3";
      default:
        return string.Empty;
    }
  }

  private string BuildShapeProp(string propName, string propValue)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}sp");
    stringBuilder.Append($"{{{this.c_slashSymbol}sn ");
    stringBuilder.Append(propName);
    stringBuilder.Append("}");
    stringBuilder.Append($"{{{this.c_slashSymbol}sv ");
    stringBuilder.Append(propValue);
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private int GetAutoShapeType(string shapeValue)
  {
    switch (shapeValue)
    {
      case "Rectangle":
        return 1;
      case "RoundedRectangle":
        return 2;
      case "Oval":
        return 3;
      case "Diamond":
        return 4;
      case "IsoscelesTriangle":
        return 5;
      case "RightTriangle":
        return 6;
      case "Parallelogram":
        return 7;
      case "Trapezoid":
        return 8;
      case "Hexagon":
        return 9;
      case "Octagon":
        return 10;
      case "Cross":
        return 11;
      case "Star5Point":
        return 12;
      case "RightArrow":
        return 13;
      case "Pentagon":
        return 15;
      case "Cube":
        return 16 /*0x10*/;
      case "RoundedRectangularCallout":
        return 17;
      case "Star16Point":
        return 18;
      case "Arc":
        return 19;
      case "Line":
        return 20;
      case "Plaque":
        return 21;
      case "Can":
        return 22;
      case "Donut":
        return 23;
      case "StraightConnector":
        return 32 /*0x20*/;
      case "BentConnector2":
        return 33;
      case "ElbowConnector":
        return 34;
      case "BentConnector4":
        return 35;
      case "BentConnector5":
        return 36;
      case "CurvedConnector2":
        return 37;
      case "CurvedConnector":
        return 38;
      case "CurvedConnector4":
        return 39;
      case "CurvedConnector5":
        return 40;
      case "LineCallout1NoBorder":
        return 41;
      case "LineCallout2NoBorder":
        return 42;
      case "LineCallout3NoBorder":
        return 43;
      case "LineCallout1AccentBar":
        return 44;
      case "LineCallout2AccentBar":
        return 45;
      case "LineCallout3AccentBar":
        return 46;
      case "LineCallout1":
        return 47;
      case "LineCallout2":
        return 48 /*0x30*/;
      case "LineCallout3":
        return 49;
      case "LineCallout1BorderAndAccentBar":
        return 50;
      case "LineCallout2BorderAndAccentBar":
        return 51;
      case "LineCallout3BorderAndAccentBar":
        return 52;
      case "DownRibbon":
        return 53;
      case "UpRibbon":
        return 54;
      case "Chevron":
        return 55;
      case "RegularPentagon":
        return 56;
      case "NoSymbol":
        return 57;
      case "Star8Point":
        return 58;
      case "Star32Point":
        return 60;
      case "RectangularCallout":
        return 61;
      case "OvalCallout":
        return 63 /*0x3F*/;
      case "Wave":
        return 64 /*0x40*/;
      case "FoldedCorner":
        return 65;
      case "LeftArrow":
        return 66;
      case "DownArrow":
        return 67;
      case "UpArrow":
        return 68;
      case "LeftRightArrow":
        return 69;
      case "UpDownArrow":
        return 70;
      case "Explosion1":
        return 71;
      case "Explosion2":
        return 72;
      case "LightningBolt":
        return 73;
      case "Heart":
        return 74;
      case "QuadArrow":
        return 76;
      case "LeftArrowCallout":
        return 77;
      case "RightArrowCallout":
        return 78;
      case "UpArrowCallout":
        return 79;
      case "DownArrowCallout":
        return 80 /*0x50*/;
      case "LeftRightArrowCallout":
        return 81;
      case "UpDownArrowCallout":
        return 82;
      case "QuadArrowCallout":
        return 83;
      case "Bevel":
        return 84;
      case "LeftBracket":
        return 85;
      case "RightBracket":
        return 86;
      case "LeftBrace":
        return 87;
      case "RightBrace":
        return 88;
      case "LeftUpArrow":
        return 89;
      case "BentUpArrow":
        return 90;
      case "BentArrow":
        return 91;
      case "Star24Point":
        return 92;
      case "StripedRightArrow":
        return 93;
      case "NotchedRightArrow":
        return 94;
      case "BlockArc":
        return 95;
      case "SmileyFace":
        return 96 /*0x60*/;
      case "VerticalScroll":
        return 97;
      case "HorizontalScroll":
        return 98;
      case "CircularArrow":
        return 99;
      case "UTurnArrow":
        return 101;
      case "CurvedRightArrow":
        return 102;
      case "CurvedLeftArrow":
        return 103;
      case "CurvedUpArrow":
        return 104;
      case "CurvedDownArrow":
        return 105;
      case "CloudCallout":
        return 106;
      case "CurvedDownRibbon":
        return 107;
      case "CurvedUpRibbon":
        return 108;
      case "FlowChartProcess":
        return 109;
      case "FlowChartDecision":
        return 110;
      case "FlowChartData":
        return 111;
      case "FlowChartPredefinedProcess":
        return 112 /*0x70*/;
      case "FlowChartInternalStorage":
        return 113;
      case "FlowChartDocument":
        return 114;
      case "FlowChartMultiDocument":
        return 115;
      case "FlowChartTerminator":
        return 116;
      case "FlowChartPreparation":
        return 117;
      case "FlowChartManualInput":
        return 118;
      case "FlowChartManualOperation":
        return 119;
      case "FlowChartConnector":
        return 120;
      case "FlowChartCard":
        return 121;
      case "FlowChartPunchedTape":
        return 122;
      case "FlowChartSummingJunction":
        return 123;
      case "FlowChartOr":
        return 124;
      case "FlowChartCollate":
        return 125;
      case "FlowChartSort":
        return 126;
      case "FlowChartExtract":
        return (int) sbyte.MaxValue;
      case "FlowChartMerge":
        return 128 /*0x80*/;
      case "FlowChartStoredData":
        return 130;
      case "FlowChartSequentialAccessStorage":
        return 131;
      case "FlowChartMagneticDisk":
        return 132;
      case "FlowChartDirectAccessStorage":
        return 133;
      case "FlowChartDisplay":
        return 134;
      case "FlowChartDelay":
        return 135;
      case "FlowChartAlternateProcess":
        return 176 /*0xB0*/;
      case "FlowChartOffPageConnector":
        return 177;
      case "LeftRightUpArrow":
        return 182;
      case "Sun":
        return 183;
      case "Moon":
        return 184;
      case "DoubleBracket":
        return 185;
      case "DoubleBrace":
        return 186;
      case "Star4Point":
        return 187;
      case "DoubleWave":
        return 188;
      default:
        return 0;
    }
  }

  private int GetLineStyle(string lineStyle)
  {
    switch (lineStyle)
    {
      case "ThinThin":
        return 1;
      case "ThickThin":
        return 2;
      case "ThinThick":
        return 3;
      case "ThickBetweenThin":
        return 4;
      default:
        return 0;
    }
  }

  private string BuildLayoutInCell(bool isLayoutInCell)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}sp");
    stringBuilder.Append($"{{{this.c_slashSymbol}sn ");
    stringBuilder.Append("fLayoutInCell");
    stringBuilder.Append("}");
    stringBuilder.Append($"{{{this.c_slashSymbol}sv ");
    stringBuilder.Append(isLayoutInCell ? "1" : "0");
    stringBuilder.Append("}}");
    stringBuilder.Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  private string BuildHorAlignm(ShapeHorizontalAlignment hAlignm)
  {
    switch (hAlignm)
    {
      case ShapeHorizontalAlignment.Left:
        return this.BuildShapeProp("posh", "1");
      case ShapeHorizontalAlignment.Center:
        return this.BuildShapeProp("posh", "2");
      case ShapeHorizontalAlignment.Right:
        return this.BuildShapeProp("posh", "3");
      case ShapeHorizontalAlignment.Inside:
        return this.BuildShapeProp("posh", "4");
      case ShapeHorizontalAlignment.Outside:
        return this.BuildShapeProp("posh", "5");
      default:
        return string.Empty;
    }
  }

  private string BuildVertAlignm(ShapeVerticalAlignment vAlignm)
  {
    switch (vAlignm)
    {
      case ShapeVerticalAlignment.Top:
        return this.BuildShapeProp("posv", "1");
      case ShapeVerticalAlignment.Center:
        return this.BuildShapeProp("posv", "2");
      case ShapeVerticalAlignment.Bottom:
        return this.BuildShapeProp("posv", "3");
      case ShapeVerticalAlignment.Inside:
        return this.BuildShapeProp("posv", "4");
      case ShapeVerticalAlignment.Outside:
        return this.BuildShapeProp("posv", "5");
      default:
        return string.Empty;
    }
  }

  private string BuildHiddenTextBox(bool value)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}sp");
    stringBuilder.Append($"{{{this.c_slashSymbol}sn ");
    stringBuilder.Append("fHidden");
    stringBuilder.Append("}");
    stringBuilder.Append($"{{{this.c_slashSymbol}sv ");
    if (value)
      stringBuilder.Append("0");
    else
      stringBuilder.Append("1");
    stringBuilder.Append("}}");
    stringBuilder.Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  private string BuildHorPos(HorizontalOrigin hPos)
  {
    switch (hPos)
    {
      case HorizontalOrigin.Margin:
        return this.BuildShapeProp("posrelh", "0");
      case HorizontalOrigin.Page:
        return this.BuildShapeProp("posrelh", "1");
      case HorizontalOrigin.Column:
        return this.BuildShapeProp("posrelh", "2");
      case HorizontalOrigin.Character:
        return this.BuildShapeProp("posrelh", "3");
      default:
        return string.Empty;
    }
  }

  private string BuildVertPos(VerticalOrigin vPos)
  {
    switch (vPos)
    {
      case VerticalOrigin.Margin:
        return this.BuildShapeProp("posrelv", "0");
      case VerticalOrigin.Page:
        return this.BuildShapeProp("posrelv", "1");
      case VerticalOrigin.Paragraph:
        return this.BuildShapeProp("posrelv", "2");
      case VerticalOrigin.Line:
        return this.BuildShapeProp("posrelv", "3");
      default:
        return string.Empty;
    }
  }

  private string BuildShapePosition(float horPos, float vertPos, int shapeW, int shapeH)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if ((double) horPos == 0.0 && (double) vertPos == 0.0)
    {
      stringBuilder.Append(this.c_slashSymbol + "shpright");
      stringBuilder.Append(shapeW);
      stringBuilder.Append(this.c_slashSymbol + "shpbottom");
      stringBuilder.Append(shapeH);
    }
    else
    {
      stringBuilder.Append(this.c_slashSymbol + "shpleft");
      stringBuilder.Append(Math.Round((double) horPos * 20.0));
      stringBuilder.Append(this.c_slashSymbol + "shptop");
      stringBuilder.Append(Math.Round((double) vertPos * 20.0));
      stringBuilder.Append(this.c_slashSymbol + "shpright");
      stringBuilder.Append(Math.Round((double) horPos * 20.0 + (double) shapeW));
      stringBuilder.Append(this.c_slashSymbol + "shpbottom");
      stringBuilder.Append(Math.Round((double) vertPos * 20.0 + (double) shapeH));
    }
    return stringBuilder.ToString();
  }

  private byte[] BuildTextBox(WTextBox textBox)
  {
    MemoryStream textBoxStream = new MemoryStream();
    StringBuilder strBuilder = new StringBuilder();
    if ((textBox.IsShape && textBox.Shape != null && textBox.Shape.FallbackPic == null || !textBox.IsShape) && textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
      textBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    textBoxStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(textBox.CharacterFormat));
    textBoxStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}shp");
    textBoxStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}shpinst");
    textBoxStream.Write(bytes4, 0, bytes4.Length);
    int shapeW = (int) Math.Round((double) textBox.TextBoxFormat.Width * 20.0);
    int shapeH = (int) Math.Round((double) textBox.TextBoxFormat.Height * 20.0);
    byte[] bytes5 = this.m_encoding.GetBytes(this.BuildShapePosition(textBox.TextBoxFormat.HorizontalPosition, textBox.TextBoxFormat.VerticalPosition, shapeW, shapeH));
    textBoxStream.Write(bytes5, 0, bytes5.Length);
    if (textBox.TextBoxFormat.IsHeaderTextBox)
    {
      byte[] bytes6 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpfhdr1");
      textBoxStream.Write(bytes6, 0, bytes6.Length);
    }
    else
    {
      byte[] bytes7 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpfhdr0");
      textBoxStream.Write(bytes7, 0, bytes7.Length);
    }
    if (textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.Page)
    {
      byte[] bytes8 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbxpage");
      textBoxStream.Write(bytes8, 0, bytes8.Length);
    }
    else if (textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.Margin || textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.LeftMargin || textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.RightMargin || textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.InsideMargin || textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.OutsideMargin)
    {
      byte[] bytes9 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbxmargin");
      textBoxStream.Write(bytes9, 0, bytes9.Length);
    }
    else if (textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.Column)
    {
      byte[] bytes10 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbxcolumn");
      textBoxStream.Write(bytes10, 0, bytes10.Length);
    }
    if (textBox.TextBoxFormat.HorizontalOrigin == HorizontalOrigin.Character || textBox.TextBoxFormat.HorizontalOrigin.ToString().Contains("Margin"))
    {
      byte[] bytes11 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbxignore");
      textBoxStream.Write(bytes11, 0, bytes11.Length);
    }
    if (textBox.TextBoxFormat.VerticalOrigin == VerticalOrigin.Page)
    {
      byte[] bytes12 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbypage");
      textBoxStream.Write(bytes12, 0, bytes12.Length);
    }
    else if (textBox.TextBoxFormat.VerticalOrigin == VerticalOrigin.Margin)
    {
      byte[] bytes13 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbymargin");
      textBoxStream.Write(bytes13, 0, bytes13.Length);
    }
    else if (textBox.TextBoxFormat.VerticalOrigin == VerticalOrigin.Paragraph)
    {
      byte[] bytes14 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbypara");
      textBoxStream.Write(bytes14, 0, bytes14.Length);
    }
    byte[] bytes15 = this.m_encoding.GetBytes(this.c_slashSymbol + "shpbyignore");
    textBoxStream.Write(bytes15, 0, bytes15.Length);
    byte[] bytes16 = this.m_encoding.GetBytes($"{this.c_slashSymbol}shpz{(object) textBox.TextBoxFormat.OrderIndex}");
    textBoxStream.Write(bytes16, 0, bytes16.Length);
    if (textBox.Shape != null && textBox.IsShape && textBox.Shape.LockAnchor)
    {
      byte[] bytes17 = this.m_encoding.GetBytes(this.c_slashSymbol + "shplockanchor");
      textBoxStream.Write(bytes17, 0, bytes17.Length);
    }
    byte[] bytes18 = this.m_encoding.GetBytes(this.BuildWrappingStyle(textBox.TextBoxFormat.TextWrappingStyle, textBox.TextBoxFormat.TextWrappingType));
    textBoxStream.Write(bytes18, 0, bytes18.Length);
    byte[] bytes19 = this.m_encoding.GetBytes(this.BuildShapeProp("shapeType", "202"));
    textBoxStream.Write(bytes19, 0, bytes19.Length);
    if (textBox.Shape != null && !textBox.Shape.FillFormat.Fill)
    {
      bytes19 = this.m_encoding.GetBytes(this.BuildShapeProp("fFilled", "0"));
      textBoxStream.Write(bytes19, 0, bytes19.Length);
    }
    this.BuildTextBoxPositionTokens(bytes19, textBox, textBoxStream);
    this.BuildTextBoxLineTokens(bytes19, textBox, textBoxStream);
    byte[] bytes20 = this.m_encoding.GetBytes(this.BuildShapeFill(textBox.TextBoxFormat.FillEfects, false));
    textBoxStream.Write(bytes20, 0, bytes20.Length);
    byte[] bytes21 = this.m_encoding.GetBytes(this.BuildHiddenTextBox(textBox.Visible));
    textBoxStream.Write(bytes21, 0, bytes21.Length);
    if ((double) textBox.TextBoxFormat.InternalMargin.Left != 0.0)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("dxTextLeft", Math.Round((double) textBox.TextBoxFormat.InternalMargin.Left * 12700.0).ToString()));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if ((double) textBox.TextBoxFormat.InternalMargin.Top != 0.0)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("dyTextTop", Math.Round((double) textBox.TextBoxFormat.InternalMargin.Top * 12700.0).ToString()));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if ((double) textBox.TextBoxFormat.InternalMargin.Right != 0.0)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("dxTextRight", Math.Round((double) textBox.TextBoxFormat.InternalMargin.Right * 12700.0).ToString()));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if ((double) textBox.TextBoxFormat.InternalMargin.Bottom != 0.0)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("dyTextBottom", Math.Round((double) textBox.TextBoxFormat.InternalMargin.Bottom * 12700.0).ToString()));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if (textBox.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.InFrontOfText)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildWrapText(textBox.TextBoxFormat.TextWrappingStyle));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if (textBox.TextBoxFormat.TextDirection != TextDirection.Horizontal)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildTextFlow(textBox.TextBoxFormat.TextDirection));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if ((double) textBox.CharacterFormat.Scaling != 0.0)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("scaleText", textBox.CharacterFormat.Scaling.ToString()));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if (textBox.Shape != null && textBox.IsShape && textBox.Shape.TextFrame.TextVerticalAlignment != VerticalAlignment.Top)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("anchorText", ((int) textBox.Shape.TextFrame.TextVerticalAlignment).ToString()));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if (textBox.TextBoxFormat.AutoFit)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("fFitShapeToText", "1"));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    if (textBox.Shape != null && textBox.IsShape && textBox.Shape.TextFrame.NoWrap)
    {
      bytes21 = this.m_encoding.GetBytes(this.BuildShapeProp("WrapText", "2"));
      textBoxStream.Write(bytes21, 0, bytes21.Length);
    }
    this.BuildTextBoxFillTokens(bytes21, textBox, textBoxStream);
    if (textBox.Shape != null && textBox.Shape.EffectList.Count >= 1 && textBox.Shape.EffectList[0].IsShadowEffect)
      this.BuildShapeShadowTokens(textBox.Shape, strBuilder);
    if (textBox.Shape != null && textBox.Shape.EffectList.Count == 2 && textBox.Shape.EffectList[1].IsShapeProperties)
      this.BuildShape3DTokens(textBox.Shape, strBuilder);
    byte[] bytes22 = this.m_encoding.GetBytes(this.BuildLayoutInCell(textBox.TextBoxFormat.AllowInCell));
    textBoxStream.Write(bytes22, 0, bytes22.Length);
    int tableNestedLevel = this.m_tableNestedLevel;
    this.m_tableNestedLevel = 0;
    byte[] bytes23 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}shptxt");
    textBoxStream.Write(bytes23, 0, bytes23.Length);
    byte[] buffer = this.BuildBodyItems(textBox.TextBoxBody.Items);
    textBoxStream.Write(buffer, 0, buffer.Length);
    byte[] bytes24 = this.m_encoding.GetBytes("}");
    textBoxStream.Write(bytes24, 0, bytes24.Length);
    this.m_tableNestedLevel = tableNestedLevel;
    if (textBox.IsShape && textBox.Shape != null && textBox.Shape.FallbackPic != null)
    {
      byte[] bytes25 = this.m_encoding.GetBytes(this.BuildPicture(textBox.Shape.FallbackPic, true));
      textBoxStream.Write(bytes25, 0, bytes25.Length);
    }
    byte[] bytes26 = this.m_encoding.GetBytes("}}}");
    textBoxStream.Write(bytes26, 0, bytes26.Length);
    byte[] bytes27 = this.m_encoding.GetBytes(Environment.NewLine);
    textBoxStream.Write(bytes27, 0, bytes27.Length);
    return textBoxStream.ToArray();
  }

  private void BuildTextBoxPositionTokens(
    byte[] byteArr,
    WTextBox textBox,
    MemoryStream textBoxStream)
  {
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("posh", ((int) textBox.TextBoxFormat.HorizontalAlignment).ToString()));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("posv", ((int) textBox.TextBoxFormat.VerticalAlignment).ToString()));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("posrelh", ((int) textBox.TextBoxFormat.HorizontalOrigin).ToString()));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("posrelv", ((int) textBox.TextBoxFormat.VerticalOrigin).ToString()));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
    if (textBox.Shape != null && textBox.IsShape)
    {
      if (textBox.Shape.IsRelativeHorizontalPosition)
      {
        byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("pctHorizPos", ((int) Math.Round((double) textBox.Shape.RelativeHorizontalPosition * 10.0)).ToString()));
        textBoxStream.Write(byteArr, 0, byteArr.Length);
      }
      if (textBox.Shape.IsRelativeVerticalPosition)
      {
        byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("pctVertPos", ((int) Math.Round((double) textBox.Shape.RelativeVerticalPosition * 10.0)).ToString()));
        textBoxStream.Write(byteArr, 0, byteArr.Length);
      }
    }
    if (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Behind)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fBehindDocument", "1"));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Square || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Through || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Tight)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("dxWrapDistLeft", Math.Round((double) textBox.TextBoxFormat.WrapDistanceLeft * 12700.0, 2).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Square || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Through || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Tight)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("dxWrapDistRight", Math.Round((double) textBox.TextBoxFormat.WrapDistanceRight * 12700.0, 2).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Square || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.TopAndBottom)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("dyWrapDistTop", Math.Round((double) textBox.TextBoxFormat.WrapDistanceTop * 12700.0, 2).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Square || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.TopAndBottom)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("dyWrapDistBottom", Math.Round((double) textBox.TextBoxFormat.WrapDistanceBottom * 12700.0, 2).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if ((double) textBox.TextBoxFormat.HeightRelativePercent != 0.0)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("pctHoriz", (textBox.TextBoxFormat.HeightRelativePercent * 10f).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if ((double) textBox.TextBoxFormat.WidthRelativePercent != 0.0)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("pctVert", (textBox.TextBoxFormat.WidthRelativePercent * 10f).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if ((double) textBox.TextBoxFormat.HeightRelativePercent != 0.0)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("sizerelh", ((int) textBox.TextBoxFormat.WidthOrigin).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if ((double) textBox.TextBoxFormat.WidthRelativePercent != 0.0)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("sizerelv", ((int) textBox.TextBoxFormat.HeightOrigin).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (!textBox.TextBoxFormat.AllowOverlap)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fAllowOverlap", "0"));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (textBox.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      return;
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fPseudoInline", "1"));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
  }

  private void BuildTextBoxFillTokens(byte[] byteArr, WTextBox textBox, MemoryStream textBoxStream)
  {
    if (textBox.TextBoxFormat.FillColor != Color.Empty)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fillColor", this.GetRTFAutoShapeColor(textBox.TextBoxFormat.FillColor)));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    else
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fillOpacity", "0"));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (!textBox.IsShape)
      return;
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fillOpacity", Convert.ToUInt32((float) (65536.0 * (1.0 - (double) textBox.Shape.FillFormat.Transparency / 100.0))).ToString()));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
  }

  private void BuildTextBoxLineTokens(byte[] byteArr, WTextBox textBox, MemoryStream textBoxStream)
  {
    byteArr = this.m_encoding.GetBytes(this.BuildShapeLines(textBox.TextBoxFormat.LineColor, textBox.TextBoxFormat.LineWidth));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
    byteArr = this.m_encoding.GetBytes(this.BuildTextBoxLineStyle(textBox.TextBoxFormat.LineStyle));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
    if (textBox.Shape != null && textBox.IsShape && textBox.Shape.LineFormat.LineCap != LineCap.Flat)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("lineEndCapStyle", ((int) textBox.Shape.LineFormat.LineCap).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    LineDashing lineDashing = textBox.TextBoxFormat.LineDashing;
    if (lineDashing == LineDashing.DotGEL)
      lineDashing = LineDashing.Dot;
    if (lineDashing != LineDashing.Solid)
    {
      byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("lineDashing", ((int) lineDashing).ToString()));
      textBoxStream.Write(byteArr, 0, byteArr.Length);
    }
    if (!textBox.TextBoxFormat.NoLine)
      return;
    byteArr = this.m_encoding.GetBytes(this.BuildShapeProp("fLine", "0"));
    textBoxStream.Write(byteArr, 0, byteArr.Length);
  }

  private string BuildTextFlow(TextDirection textDirection)
  {
    switch (textDirection)
    {
      case TextDirection.VerticalFarEast:
        return this.BuildShapeProp("txflTextFlow", "1");
      case TextDirection.VerticalBottomToTop:
        return this.BuildShapeProp("txflTextFlow", "2");
      case TextDirection.VerticalTopToBottom:
        return this.BuildShapeProp("txflTextFlow", "3");
      case TextDirection.HorizontalFarEast:
        return this.BuildShapeProp("txflTextFlow", "4");
      case TextDirection.Vertical:
        return this.BuildShapeProp("txflTextFlow", "5");
      default:
        return this.BuildShapeProp("txflTextFlow", "0");
    }
  }

  private string BuildWrapText(TextWrappingStyle textWrappingStyle)
  {
    switch (textWrappingStyle)
    {
      case TextWrappingStyle.TopAndBottom:
        return this.BuildShapeProp("WrapText", "3");
      case TextWrappingStyle.Square:
        return this.BuildShapeProp("WrapText", "0");
      case TextWrappingStyle.Tight:
        return this.BuildShapeProp("WrapText", "1");
      case TextWrappingStyle.Through:
        return this.BuildShapeProp("WrapText", "4");
      default:
        return string.Empty;
    }
  }

  private string BuildTextBoxLineStyle(TextBoxLineStyle style)
  {
    switch (style)
    {
      case TextBoxLineStyle.Double:
        return this.BuildShapeProp("lineStyle", "1");
      case TextBoxLineStyle.ThickThin:
        return this.BuildShapeProp("lineStyle", "2");
      case TextBoxLineStyle.ThinThick:
        return this.BuildShapeProp("lineStyle", "3");
      case TextBoxLineStyle.Triple:
        return this.BuildShapeProp("lineStyle", "4");
      default:
        return this.BuildShapeProp("lineStyle", "0");
    }
  }

  private string BuildShapeLines(Color col, float lineWidth)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (!col.IsEmpty && col.Name != Color.Black.Name)
      stringBuilder.Append(this.BuildShapeProp("lineColor", this.GetRTFAutoShapeColor(col)));
    stringBuilder.Append(this.BuildShapeProp(nameof (lineWidth), (lineWidth * 12700f).ToString()));
    return stringBuilder.ToString();
  }

  private string BuildShapeFill(Background backgr, bool isPageBackground)
  {
    StringBuilder stringBuilder = new StringBuilder();
    switch (backgr.Type)
    {
      case BackgroundType.Gradient:
        switch (backgr.Gradient.ShadingStyle)
        {
          case GradientShadingStyle.Horizontal:
            stringBuilder.Append(this.BuildShapeProp("fillType", "7"));
            stringBuilder.Append(this.BuildGradientVariant(backgr.Gradient.ShadingVariant));
            break;
          case GradientShadingStyle.Vertical:
            stringBuilder.Append(this.BuildShapeProp("fillType", "7"));
            stringBuilder.Append(this.BuildShapeProp("fillAngle", "-5898240"));
            stringBuilder.Append(this.BuildGradientVariant(backgr.Gradient.ShadingVariant));
            break;
          case GradientShadingStyle.DiagonalUp:
            stringBuilder.Append(this.BuildShapeProp("fillType", "7"));
            stringBuilder.Append(this.BuildShapeProp("fillAngle", "-8847360"));
            stringBuilder.Append(this.BuildGradientVariant(backgr.Gradient.ShadingVariant));
            break;
          case GradientShadingStyle.DiagonalDown:
            stringBuilder.Append(this.BuildShapeProp("fillType", "7"));
            stringBuilder.Append(this.BuildShapeProp("fillAngle", "-2949120"));
            stringBuilder.Append(this.BuildGradientVariant(backgr.Gradient.ShadingVariant));
            break;
          case GradientShadingStyle.FromCorner:
            stringBuilder.Append(this.BuildShapeProp("fillType", "5"));
            stringBuilder.Append(this.BuildShapeProp("fillFocus", "100"));
            if (backgr.Gradient.ShadingVariant == GradientShadingVariant.ShadingDown)
            {
              stringBuilder.Append(this.BuildShapeProp("fillToLeft", "65536"));
              stringBuilder.Append(this.BuildShapeProp("fillToRight", "65536"));
              break;
            }
            if (backgr.Gradient.ShadingVariant == GradientShadingVariant.ShadingMiddle)
            {
              stringBuilder.Append(this.BuildShapeProp("fillToLeft", "65536"));
              stringBuilder.Append(this.BuildShapeProp("fillToTop", "65536"));
              stringBuilder.Append(this.BuildShapeProp("fillToRight", "65536"));
              stringBuilder.Append(this.BuildShapeProp("fillToBottom", "65536"));
              break;
            }
            if (backgr.Gradient.ShadingVariant == GradientShadingVariant.ShadingOut)
            {
              stringBuilder.Append(this.BuildShapeProp("fillToTop", "65536"));
              stringBuilder.Append(this.BuildShapeProp("fillToBottom", "65536"));
              break;
            }
            break;
          case GradientShadingStyle.FromCenter:
            stringBuilder.Append(this.BuildShapeProp("fillType", "6"));
            if (backgr.Gradient.ShadingVariant == GradientShadingVariant.ShadingUp)
              stringBuilder.Append(this.BuildShapeProp("fillFocus", "100"));
            stringBuilder.Append(this.BuildShapeProp("fillToLeft", "32768"));
            stringBuilder.Append(this.BuildShapeProp("fillToRight", "32768"));
            stringBuilder.Append(this.BuildShapeProp("fillToTop", "32768"));
            stringBuilder.Append(this.BuildShapeProp("fillToBottom", "32768"));
            break;
        }
        stringBuilder.Append(this.BuildShapeProp("fillColor", this.GetRtfShapeColor(backgr.Gradient.Color1)));
        stringBuilder.Append(this.BuildShapeProp("fillBackColor", this.GetRtfShapeColor(backgr.Gradient.Color2)));
        break;
      case BackgroundType.Picture:
      case BackgroundType.Texture:
        if (backgr.Type == BackgroundType.Picture)
          stringBuilder.Append(this.BuildShapeProp("fillType", "3"));
        else
          stringBuilder.Append(this.BuildShapeProp("fillType", "2"));
        WPicture pic = new WPicture((IWordDocument) this.m_doc);
        pic.LoadImage(backgr.Picture);
        stringBuilder.Append(this.BuildShapeProp("fillBlip", this.BuildPicture(pic, false)));
        break;
      case BackgroundType.Color:
        if (backgr.Color.IsEmpty || backgr.Color.Name == Color.White.Name)
          return string.Empty;
        if (isPageBackground)
        {
          stringBuilder.Append(this.BuildShapeProp("fillColor", this.GetRtfPageBackgroundColor(backgr.Color)));
          break;
        }
        stringBuilder.Append(this.BuildShapeProp("fillColor", this.GetRtfShapeColor(backgr.Color)));
        break;
      default:
        return string.Empty;
    }
    return stringBuilder.ToString();
  }

  private string BuildGradientVariant(GradientShadingVariant variant)
  {
    switch (variant)
    {
      case GradientShadingVariant.ShadingUp:
        return this.BuildShapeProp("fillFocus", "100");
      case GradientShadingVariant.ShadingOut:
        return this.BuildShapeProp("fillFocus", "-50");
      case GradientShadingVariant.ShadingMiddle:
        return this.BuildShapeProp("fillFocus", "50");
      default:
        return string.Empty;
    }
  }

  private void AppendListStyles()
  {
    ListStyleCollection listStyles = this.m_doc.ListStyles;
    MemoryStream memoryStream = new MemoryStream();
    if (listStyles.Count == 0)
      return;
    byte[] bytes1 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}listtable");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    int index1 = 0;
    for (int count1 = listStyles.Count; index1 < count1; ++index1)
    {
      ListStyle listStyle = listStyles[index1];
      byte[] bytes2 = this.m_encoding.GetBytes(Environment.NewLine);
      memoryStream.Write(bytes2, 0, bytes2.Length);
      byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}list");
      memoryStream.Write(bytes3, 0, bytes3.Length);
      if (listStyle.IsSimple)
      {
        byte[] bytes4 = this.m_encoding.GetBytes(this.c_slashSymbol + "listsimple1");
        memoryStream.Write(bytes4, 0, bytes4.Length);
      }
      else
      {
        byte[] bytes5 = this.m_encoding.GetBytes(this.c_slashSymbol + "listsimple0");
        memoryStream.Write(bytes5, 0, bytes5.Length);
      }
      if (listStyle.IsHybrid)
      {
        byte[] bytes6 = this.m_encoding.GetBytes(this.c_slashSymbol + "listhybrid");
        memoryStream.Write(bytes6, 0, bytes6.Length);
      }
      int index2 = 0;
      for (int count2 = listStyle.Levels.Count; index2 < count2; ++index2)
      {
        byte[] bytes7 = this.m_encoding.GetBytes(this.BuildListLevel(listStyle.Levels[index2]));
        memoryStream.Write(bytes7, 0, bytes7.Length);
      }
      byte[] bytes8 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}listname ");
      memoryStream.Write(bytes8, 0, bytes8.Length);
      byte[] bytes9 = this.m_encoding.GetBytes(listStyle.Name);
      memoryStream.Write(bytes9, 0, bytes9.Length);
      byte[] bytes10 = this.m_encoding.GetBytes(" ;}");
      memoryStream.Write(bytes10, 0, bytes10.Length);
      byte[] bytes11 = this.m_encoding.GetBytes($"{this.c_slashSymbol}listid{index1.ToString()}");
      memoryStream.Write(bytes11, 0, bytes11.Length);
      byte[] bytes12 = this.m_encoding.GetBytes(Environment.NewLine);
      memoryStream.Write(bytes12, 0, bytes12.Length);
      byte[] bytes13 = this.m_encoding.GetBytes("}");
      memoryStream.Write(bytes13, 0, bytes13.Length);
      this.ListsIds.Add(listStyle.Name, index1);
    }
    byte[] bytes14 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes14, 0, bytes14.Length);
    this.m_listTableBytes = memoryStream.ToArray();
  }

  private string BuildListLevel(WListLevel listLevel)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}listlevel");
    stringBuilder.Append(this.BuildLevelFormatting(listLevel.PatternType));
    if (listLevel.NumberAlignment == ListNumberAlignment.Left)
      stringBuilder.Append($"{this.c_slashSymbol}leveljc0{this.c_slashSymbol}leveljcn0");
    else if (listLevel.NumberAlignment == ListNumberAlignment.Center)
      stringBuilder.Append($"{this.c_slashSymbol}leveljc1{this.c_slashSymbol}leveljcn1");
    else if (listLevel.NumberAlignment == ListNumberAlignment.Right)
      stringBuilder.Append($"{this.c_slashSymbol}leveljc2{this.c_slashSymbol}leveljcn2");
    if (listLevel.FollowCharacter == FollowCharacterType.Tab)
      stringBuilder.Append(this.c_slashSymbol + "levelfollow0");
    else if (listLevel.FollowCharacter == FollowCharacterType.Space)
      stringBuilder.Append(this.c_slashSymbol + "levelfollow1");
    else
      stringBuilder.Append(this.c_slashSymbol + "levelfollow2");
    stringBuilder.Append(this.c_slashSymbol + "levelstartat");
    stringBuilder.Append(listLevel.StartAt.ToString());
    if (listLevel.Word6Legacy)
      stringBuilder.Append(this.c_slashSymbol + "levelold");
    stringBuilder.Append(this.c_slashSymbol + "levelspace");
    stringBuilder.Append(listLevel.LegacySpace);
    stringBuilder.Append(this.c_slashSymbol + "levelindent");
    stringBuilder.Append(listLevel.LegacyIndent);
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(this.BuildLevelText(listLevel));
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(this.BuildLevelNumbers(listLevel));
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(this.BuildParagraphFormat(listLevel.ParagraphFormat, (WParagraph) null));
    stringBuilder.Append(this.BuildCharacterFormat(listLevel.CharacterFormat));
    stringBuilder.Append("}");
    stringBuilder.Append(Environment.NewLine);
    return stringBuilder.ToString();
  }

  private string UpdateNumberPrefix(string prefix)
  {
    return prefix.Replace("\0", this.c_slashSymbol + "'00").Replace("\u0001", this.c_slashSymbol + "'01").Replace("\u0002", this.c_slashSymbol + "'02").Replace("\u0003", this.c_slashSymbol + "'03").Replace("\u0004", this.c_slashSymbol + "'04").Replace("\u0005", this.c_slashSymbol + "'05").Replace("\u0006", this.c_slashSymbol + "'06").Replace("\a", this.c_slashSymbol + "'07").Replace("\b", this.c_slashSymbol + "'08");
  }

  private string BuildLevelText(WListLevel listLevel)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}leveltext");
    if (listLevel.PatternType == ListPatternType.Bullet)
    {
      stringBuilder.Append(this.c_slashSymbol + "'01");
      string bulletCharacter = listLevel.BulletCharacter;
      if (this.IsChanged(ref bulletCharacter))
      {
        stringBuilder.Append(bulletCharacter);
      }
      else
      {
        byte num1 = string.IsNullOrEmpty(listLevel.BulletCharacter) ? (byte) 0 : (byte) listLevel.BulletCharacter[0];
        if (!string.IsNullOrEmpty(listLevel.BulletCharacter) && (listLevel.CharacterFormat.FontName == "Symbol" || (num1 <= (byte) 64 /*0x40*/ || num1 >= (byte) 91) && (num1 <= (byte) 96 /*0x60*/ || num1 >= (byte) 123)))
        {
          if (listLevel.BulletCharacter[0] > '\u007F')
          {
            string str = $"\\u{(object) Convert.ToUInt32(listLevel.BulletCharacter[0])}?";
            stringBuilder.Append(str);
          }
          else
          {
            int num2 = 4096 /*0x1000*/ - (int) num1;
            stringBuilder.Append($"{this.c_slashSymbol}u-{num2.ToString()} ?");
          }
        }
        else if (!string.IsNullOrEmpty(listLevel.BulletCharacter))
          stringBuilder.Append(listLevel.BulletCharacter[0].ToString());
      }
    }
    else
    {
      string levelText = this.GetLevelText(listLevel, false);
      int levelTextLeng = this.GetLevelTextLeng(levelText);
      stringBuilder.Append($"{this.c_slashSymbol}'{levelTextLeng.ToString("X2")}{levelText}");
    }
    stringBuilder.Append(";}");
    return stringBuilder.ToString();
  }

  private string BuildLevelNumbers(WListLevel listLevel)
  {
    StringBuilder stringBuilder = new StringBuilder();
    string levelText = this.GetLevelText(listLevel, true);
    stringBuilder.Append($"{{{this.c_slashSymbol}levelnumbers");
    if (listLevel.PatternType != ListPatternType.Bullet && levelText != string.Empty)
    {
      int levelTextLeng = this.GetLevelTextLeng(levelText.Trim());
      if (!string.IsNullOrEmpty(listLevel.NumberPrefix) && !this.IsComplexList(listLevel.NumberPrefix))
      {
        for (int index = listLevel.NumberPrefix.Length + 1; index <= levelTextLeng; index += 2)
          stringBuilder.Append($"{this.c_slashSymbol}'{index.ToString("X2")}");
      }
      else
      {
        for (int index = 1; index <= levelTextLeng; index += 2)
          stringBuilder.Append($"{this.c_slashSymbol}'{index.ToString("X2")}");
      }
    }
    stringBuilder.Append(";}");
    return stringBuilder.ToString();
  }

  private int GetLevelTextLeng(string levelText)
  {
    levelText = levelText.Replace(this.c_slashSymbol + "'0", string.Empty);
    return levelText.Length;
  }

  private string GetLevelText(WListLevel listLevel, bool isLevelNumbers)
  {
    return $"{(string.IsNullOrEmpty(listLevel.NumberPrefix) ? listLevel.NumberPrefix : this.UpdateNumberPrefix(listLevel.NumberPrefix))}{this.c_slashSymbol}'0{listLevel.LevelNumber.ToString()}{(isLevelNumbers ? string.Empty : listLevel.NumberSuffix)}";
  }

  private string BuildLevelFormatting(ListPatternType type)
  {
    switch (type)
    {
      case ListPatternType.Arabic:
        return $"{this.c_slashSymbol}levelnfc0{this.c_slashSymbol}levelnfcn0";
      case ListPatternType.UpRoman:
        return $"{this.c_slashSymbol}levelnfc1{this.c_slashSymbol}levelnfcn1";
      case ListPatternType.LowRoman:
        return $"{this.c_slashSymbol}levelnfc2{this.c_slashSymbol}levelnfcn2";
      case ListPatternType.UpLetter:
        return $"{this.c_slashSymbol}levelnfc3{this.c_slashSymbol}levelnfcn3";
      case ListPatternType.LowLetter:
        return $"{this.c_slashSymbol}levelnfc4{this.c_slashSymbol}levelnfcn4";
      case ListPatternType.Ordinal:
        return $"{this.c_slashSymbol}levelnfc5{this.c_slashSymbol}levelnfcn5";
      case ListPatternType.OrdinalText:
        return $"{this.c_slashSymbol}levelnfc7{this.c_slashSymbol}levelnfcn7";
      case ListPatternType.LeadingZero:
        return $"{this.c_slashSymbol}levelnfc22{this.c_slashSymbol}levelnfcn22";
      case ListPatternType.Bullet:
        return $"{this.c_slashSymbol}levelnfc23{this.c_slashSymbol}levelnfcn23";
      default:
        return $"{this.c_slashSymbol}levelnfc255{this.c_slashSymbol}levelnfcn255";
    }
  }

  private void AppendOverrideList()
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] bytes1 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}listoverridetable");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    foreach (int key in this.ListOverrideAr.Keys)
    {
      byte[] bytes2 = this.m_encoding.GetBytes(Environment.NewLine);
      memoryStream.Write(bytes2, 0, bytes2.Length);
      byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}listoverride");
      memoryStream.Write(bytes3, 0, bytes3.Length);
      byte[] bytes4 = this.m_encoding.GetBytes($"{this.c_slashSymbol}listid{(key - 1).ToString()}");
      memoryStream.Write(bytes4, 0, bytes4.Length);
      string name = this.ListOverrideAr[key];
      if (!string.IsNullOrEmpty(name))
      {
        ListOverrideStyle byName = this.m_doc.ListOverrides.FindByName(name);
        byte[] bytes5 = this.m_encoding.GetBytes($"{this.c_slashSymbol}listoverridecount{(object) byName.OverrideLevels.Count}");
        memoryStream.Write(bytes5, 0, bytes5.Length);
        byte[] bytes6 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}lfolevel{this.c_slashSymbol}listoverrideformat");
        memoryStream.Write(bytes6, 0, bytes6.Length);
        byte[] bytes7 = this.m_encoding.GetBytes(Environment.NewLine);
        memoryStream.Write(bytes7, 0, bytes7.Length);
        foreach (OverrideLevelFormat overrideLevel in (CollectionImpl) byName.OverrideLevels)
        {
          byte[] bytes8 = this.m_encoding.GetBytes(this.BuildListLevel(overrideLevel.OverrideListLevel));
          memoryStream.Write(bytes8, 0, bytes8.Length);
        }
        byte[] bytes9 = this.m_encoding.GetBytes("}");
        memoryStream.Write(bytes9, 0, bytes9.Length);
      }
      else
      {
        byte[] bytes10 = this.m_encoding.GetBytes(this.c_slashSymbol + "listoverridecount0");
        memoryStream.Write(bytes10, 0, bytes10.Length);
      }
      byte[] bytes11 = this.m_encoding.GetBytes($"{this.c_slashSymbol}ls{key.ToString()}");
      memoryStream.Write(bytes11, 0, bytes11.Length);
      byte[] bytes12 = this.m_encoding.GetBytes("}");
      memoryStream.Write(bytes12, 0, bytes12.Length);
    }
    byte[] bytes13 = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes13, 0, bytes13.Length);
    byte[] bytes14 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes14, 0, bytes14.Length);
    this.m_listOverrideTableBytes = memoryStream.ToArray();
  }

  private string BuildListText(WParagraph paragraph, WListFormat listFormat)
  {
    if (listFormat == null || listFormat.ListType == ListType.NoList || listFormat.CurrentListStyle == null)
      return string.Empty;
    WListLevel listLevel = paragraph.GetListLevel(listFormat);
    StringBuilder stringBuilder = new StringBuilder();
    if (listFormat.CurrentListStyle.ListType == ListType.Numbered || listFormat.CurrentListStyle.ListType == ListType.Bulleted)
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}listtext");
      stringBuilder.Append(this.BuildParagraphFormat(listLevel.ParagraphFormat, (WParagraph) null));
      stringBuilder.Append(this.BuildCharacterFormat(listLevel.CharacterFormat));
      stringBuilder.Append(this.BuildListText(listLevel, listFormat, paragraph));
      stringBuilder.Append(this.c_slashSymbol + "tab}");
      stringBuilder.Append(Environment.NewLine);
    }
    return stringBuilder.ToString();
  }

  private string BuildListText(WListLevel listLevel, WListFormat listFormat, WParagraph paragraph)
  {
    if (listLevel == null && listFormat == null)
      return string.Empty;
    if (listLevel.PatternType == ListPatternType.LowLetter || listLevel.PatternType == ListPatternType.UpLetter)
      return this.BuildLstLetterSymbol(listFormat);
    bool isPicBullet = false;
    return listLevel.PatternType == ListPatternType.Arabic ? this.c_slashSymbol + paragraph.GetListText(false, ref isPicBullet) : " " + paragraph.GetListText(false, ref isPicBullet);
  }

  private int GetLstStartVal(WListFormat format)
  {
    if (!this.ListStart.ContainsKey(format.CustomStyleName))
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      this.ListStart.Add(format.CustomStyleName, dictionary);
      WListLevel level = format.CurrentListStyle.Levels[format.ListLevelNumber];
      dictionary.Add(format.ListLevelNumber, level.StartAt + 1);
      return level.StartAt;
    }
    Dictionary<int, int> dictionary1 = this.ListStart[format.CustomStyleName];
    if (dictionary1.ContainsKey(format.ListLevelNumber))
    {
      int lstStartVal = dictionary1[format.ListLevelNumber];
      dictionary1[format.ListLevelNumber] = lstStartVal + 1;
      return lstStartVal;
    }
    WListLevel level1 = format.CurrentListStyle.Levels[format.ListLevelNumber];
    dictionary1.Add(format.ListLevelNumber, level1.StartAt + 1);
    return level1.StartAt + 1;
  }

  private string BuildLstLetterSymbol(WListFormat format)
  {
    int num1 = format.CurrentListLevel.PatternType == ListPatternType.LowLetter ? 96 /*0x60*/ : 64 /*0x40*/;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(format.CurrentListLevel.NumberPrefix);
    int lstStartVal = this.GetLstStartVal(format);
    int num2 = 1;
    while (lstStartVal > 26)
    {
      lstStartVal -= 26;
      ++num2;
    }
    string str = ((char) (lstStartVal + num1)).ToString();
    for (int index = 0; index < num2; ++index)
      stringBuilder.Append(str);
    stringBuilder.Append(format.CurrentListLevel.NumberSuffix);
    return stringBuilder.ToString();
  }

  private bool IsChanged(ref string listLText)
  {
    string str = listLText;
    listLText = listLText.Replace(this.c_symbol8226, this.c_slashSymbol + "'95");
    return !(str == listLText);
  }

  private bool IsComplexList(string prefix)
  {
    return string.IsNullOrEmpty(prefix) || prefix.Contains("\0") || prefix.Contains("\u0001") || prefix.Contains("\u0002") || prefix.Contains("\u0003") || prefix.Contains("\u0004") || prefix.Contains("\u0005") || prefix.Contains("\u0006") || prefix.Contains("\a") || prefix.Contains("\b");
  }

  private string BuildPictWtrmarkBody(PictureWatermark picWatermark)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}shp");
    stringBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}shpinst");
    stringBuilder.Append(this.c_slashSymbol + "shpleft0");
    stringBuilder.Append(this.c_slashSymbol + "shptop0");
    stringBuilder.Append(this.c_slashSymbol + "shpwr3");
    stringBuilder.Append(this.c_slashSymbol + "shpright");
    stringBuilder.Append(Math.Round((double) picWatermark.WordPicture.Width * 20.0));
    stringBuilder.Append(this.c_slashSymbol + "shpbottom");
    stringBuilder.Append(Math.Round((double) picWatermark.WordPicture.Height * 20.0));
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(this.BuildShapeProp("shapeType", "75"));
    stringBuilder.Append(this.BuildShapeProp("pib", this.BuildPictureProp(picWatermark.WordPicture, false)));
    stringBuilder.Append(this.BuildDefWtrmarkProp());
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private string BuildTextWtrmarkBody(TextWatermark textWatermark)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}shp");
    stringBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}shpinst");
    stringBuilder.Append(this.c_slashSymbol + "shpleft0");
    stringBuilder.Append(this.c_slashSymbol + "shptop0");
    stringBuilder.Append(this.c_slashSymbol + "shpright");
    stringBuilder.Append(textWatermark.Width * 20f);
    stringBuilder.Append(this.c_slashSymbol + "shpbottom");
    stringBuilder.Append(textWatermark.Height * 20f);
    stringBuilder.Append(this.c_slashSymbol + "shpwr3");
    stringBuilder.Append(Environment.NewLine);
    stringBuilder.Append(this.BuildShapeProp("shapeType", "136"));
    if (textWatermark.Layout == WatermarkLayout.Diagonal)
      stringBuilder.Append(this.BuildShapeProp("rotation", "20643840"));
    string propValue1 = textWatermark.Text.Replace(this.c_slashSymbol + "0", string.Empty);
    stringBuilder.Append(this.BuildShapeProp("gtextUNICODE", propValue1));
    int num = (int) Math.Round((double) textWatermark.Size * 65536.0);
    stringBuilder.Append(this.BuildShapeProp("gtextSize", num.ToString()));
    string propValue2 = textWatermark.FontName.Replace(this.c_slashSymbol + "0", string.Empty);
    stringBuilder.Append(this.BuildShapeProp("gtextFont", propValue2));
    stringBuilder.Append(this.BuildShapeProp("fillColor", this.GetRtfShapeColor(textWatermark.Color)));
    if (textWatermark.Semitransparent)
      stringBuilder.Append(this.BuildShapeProp("fillOpacity", "32768"));
    stringBuilder.Append(this.BuildDefWtrmarkProp());
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private string BuildDefWtrmarkProp()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.BuildShapeProp("fLine", "0"));
    stringBuilder.Append(this.BuildShapeProp("posh", "2"));
    stringBuilder.Append(this.BuildShapeProp("posrelh", "0"));
    stringBuilder.Append(this.BuildShapeProp("posv", "2"));
    stringBuilder.Append(this.BuildShapeProp("posrelv", "0"));
    return stringBuilder.ToString();
  }

  private string BuildTextFormField(WTextFormField textField)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}formfield");
    stringBuilder.Append($"{{{this.c_slashSymbol}fftype0");
    switch (textField.Type)
    {
      case TextFormFieldType.RegularText:
        stringBuilder.Append(this.c_slashSymbol + "fftypetxt0");
        break;
      case TextFormFieldType.NumberText:
        stringBuilder.Append(this.c_slashSymbol + "fftypetxt1");
        break;
      case TextFormFieldType.DateText:
        stringBuilder.Append(this.c_slashSymbol + "fftypetxt2");
        break;
      case TextFormFieldType.CurrentDateText:
        stringBuilder.Append(this.c_slashSymbol + "fftypetxt3");
        break;
      case TextFormFieldType.CurrentTimeText:
        stringBuilder.Append(this.c_slashSymbol + "fftypetxt4");
        break;
      case TextFormFieldType.Calculation:
        stringBuilder.Append(this.c_slashSymbol + "fftypetxt5");
        break;
    }
    if (textField.CalculateOnExit)
      stringBuilder.Append(this.c_slashSymbol + "ffrecalc");
    if (textField.MaximumLength != 0)
      stringBuilder.Append($"{this.c_slashSymbol}ffmaxlen{textField.MaximumLength.ToString()}");
    stringBuilder.Append(this.c_slashSymbol + "ffhps20");
    if (!string.IsNullOrEmpty(textField.Name))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}ffname ");
      stringBuilder.Append(textField.Name + "}");
    }
    if (!string.IsNullOrEmpty(textField.DefaultText))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}ffdeftext ");
      stringBuilder.Append(textField.DefaultText + "}");
    }
    if (textField.TextFormat != TextFormat.None)
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}ffformat ");
      switch (textField.TextFormat)
      {
        case TextFormat.Uppercase:
          stringBuilder.Append("Uppercase");
          break;
        case TextFormat.Lowercase:
          stringBuilder.Append("Lowercase");
          break;
        case TextFormat.FirstCapital:
          stringBuilder.Append("FirstCapital");
          break;
        case TextFormat.Titlecase:
          stringBuilder.Append("Titlecase");
          break;
      }
      stringBuilder.Append("}");
    }
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private string BuildCheckBox(WCheckBox checkBox)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}formfield");
    stringBuilder.Append($"{{{this.c_slashSymbol}fftype1");
    stringBuilder.Append(this.c_slashSymbol + "ffres25");
    if (checkBox.SizeType == CheckBoxSizeType.Auto)
      stringBuilder.Append(this.c_slashSymbol + "ffsize0");
    else
      stringBuilder.Append(this.c_slashSymbol + "ffsize1");
    stringBuilder.Append(this.c_slashSymbol + "fftypetxt0");
    if (checkBox.CalculateOnExit)
      stringBuilder.Append(this.c_slashSymbol + "ffrecalc");
    stringBuilder.Append($"{this.c_slashSymbol}ffhps{(checkBox.CheckBoxSize * 2).ToString()}");
    if (!string.IsNullOrEmpty(checkBox.Name))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}ffname ");
      stringBuilder.Append(checkBox.Name + "}");
    }
    if (checkBox.Checked)
      stringBuilder.Append(this.c_slashSymbol + "ffdefres1");
    else
      stringBuilder.Append(this.c_slashSymbol + "ffdefres0");
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private string BuildDropDownField(WDropDownFormField dropDownField)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}formfield");
    stringBuilder.Append($"{{{this.c_slashSymbol}fftype2");
    if (dropDownField.DropDownSelectedIndex >= 0)
      stringBuilder.Append($"{this.c_slashSymbol}ffres{dropDownField.DropDownSelectedIndex.ToString()}");
    stringBuilder.Append(this.c_slashSymbol + "fftypetxt0");
    if (dropDownField.CalculateOnExit)
      stringBuilder.Append(this.c_slashSymbol + "ffrecalc");
    stringBuilder.Append(this.c_slashSymbol + "ffhaslistbox");
    stringBuilder.Append(this.c_slashSymbol + "ffhps20");
    if (!string.IsNullOrEmpty(dropDownField.Name))
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}ffname ");
      stringBuilder.Append(dropDownField.Name + "}");
    }
    stringBuilder.Append(this.c_slashSymbol + "ffdefres0");
    int index = 0;
    for (int count = dropDownField.DropDownItems.Count; index < count; ++index)
    {
      stringBuilder.Append($"{{{this.c_slashSymbol}ffl ");
      stringBuilder.Append(dropDownField.DropDownItems[index].Text);
      stringBuilder.Append("}");
    }
    stringBuilder.Append("}}");
    return stringBuilder.ToString();
  }

  private string BuildFormField(WFormField formField)
  {
    this.CurrentField.Push((object) formField);
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append($"{{{this.c_slashSymbol}field");
    stringBuilder.Append($"{{{this.c_slashSymbol}*{this.c_slashSymbol}fldinst");
    return stringBuilder.ToString();
  }

  private byte[] BuildCommentMark(WCommentMark cMark)
  {
    MemoryStream memoryStream = new MemoryStream();
    if (!(cMark.CommentId != "-1"))
      return memoryStream.ToArray();
    if (cMark.Type == CommentMarkType.CommentStart)
    {
      byte[] bytes1 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}atrfstart ");
      memoryStream.Write(bytes1, 0, bytes1.Length);
      string str = this.GetNextId().ToString();
      byte[] bytes2 = this.m_encoding.GetBytes(str.ToString());
      memoryStream.Write(bytes2, 0, bytes2.Length);
      this.CommentIds.Add(cMark.CommentId, str);
    }
    else
    {
      byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}atrfend ");
      memoryStream.Write(bytes3, 0, bytes3.Length);
      byte[] bytes4 = this.m_encoding.GetBytes(this.CommentIds[cMark.CommentId].ToString());
      memoryStream.Write(bytes4, 0, bytes4.Length);
    }
    byte[] bytes = this.m_encoding.GetBytes("}");
    memoryStream.Write(bytes, 0, bytes.Length);
    return memoryStream.ToArray();
  }

  private byte[] BuildComment(WComment comment)
  {
    StringBuilder stringBuilder = new StringBuilder();
    MemoryStream memoryStream = new MemoryStream();
    string str = (string) null;
    if (this.m_commentIds != null && this.CommentIds.ContainsKey(comment.Format.TagBkmk))
      str = this.CommentIds[comment.Format.TagBkmk].ToString();
    if (str == null && comment.CommentedItems.Count != 0)
      str = this.GetNextId().ToString();
    if (comment.AppendItems)
    {
      byte[] buffer = this.BuildComItems(comment, str);
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    if (!string.IsNullOrEmpty(comment.Format.UserInitials))
    {
      byte[] bytes2 = this.m_encoding.GetBytes(this.BuildCharacterFormat(comment.ParaItemCharFormat));
      memoryStream.Write(bytes2, 0, bytes2.Length);
      byte[] bytes3 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}atnid ");
      memoryStream.Write(bytes3, 0, bytes3.Length);
      byte[] bytes4 = this.m_encoding.GetBytes(comment.Format.UserInitials);
      memoryStream.Write(bytes4, 0, bytes4.Length);
      byte[] bytes5 = this.m_encoding.GetBytes("}");
      memoryStream.Write(bytes5, 0, bytes5.Length);
    }
    if (!string.IsNullOrEmpty(comment.Format.User))
    {
      byte[] bytes6 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}atnauthor ");
      memoryStream.Write(bytes6, 0, bytes6.Length);
      byte[] bytes7 = this.m_encoding.GetBytes(comment.Format.User);
      memoryStream.Write(bytes7, 0, bytes7.Length);
      byte[] bytes8 = this.m_encoding.GetBytes("}");
      memoryStream.Write(bytes8, 0, bytes8.Length);
    }
    byte[] bytes9 = this.m_encoding.GetBytes(this.c_slashSymbol + "chatn ");
    memoryStream.Write(bytes9, 0, bytes9.Length);
    byte[] bytes10 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}annotation");
    memoryStream.Write(bytes10, 0, bytes10.Length);
    if (str != null && comment.Format.TagBkmk != "-1" && !this.HasEmptyCommentedItems(comment))
    {
      byte[] bytes11 = this.m_encoding.GetBytes($"{{{this.c_slashSymbol}*{this.c_slashSymbol}atnref ");
      memoryStream.Write(bytes11, 0, bytes11.Length);
      byte[] bytes12 = this.m_encoding.GetBytes(str);
      memoryStream.Write(bytes12, 0, bytes12.Length);
      byte[] bytes13 = this.m_encoding.GetBytes("}");
      memoryStream.Write(bytes13, 0, bytes13.Length);
    }
    byte[] buffer1 = this.BuildBodyItems(comment.TextBody.Items);
    memoryStream.Write(buffer1, 0, buffer1.Length);
    byte[] bytes14 = this.m_encoding.GetBytes("}}");
    memoryStream.Write(bytes14, 0, bytes14.Length);
    return memoryStream.ToArray();
  }

  private byte[] BuildComItems(WComment comment, string id)
  {
    MemoryStream memoryStream = new MemoryStream();
    WCommentMark cMark1 = new WCommentMark(comment.Document, id, CommentMarkType.CommentStart);
    WCommentMark cMark2 = new WCommentMark(comment.Document, id, CommentMarkType.CommentEnd);
    byte[] buffer1 = this.BuildCommentMark(cMark1);
    memoryStream.Write(buffer1, 0, buffer1.Length);
    foreach (ParagraphItem commentedItem in (CollectionImpl) comment.CommentedItems)
    {
      byte[] buffer2 = this.BuildParagraphItem(commentedItem);
      memoryStream.Write(buffer2, 0, buffer2.Length);
    }
    byte[] buffer3 = this.BuildCommentMark(cMark2);
    memoryStream.Write(buffer3, 0, buffer3.Length);
    return memoryStream.ToArray();
  }

  private bool HasEmptyCommentedItems(WComment comment)
  {
    if (comment.CommentRangeStart.OwnerParagraph != comment.CommentRangeEnd.OwnerParagraph || comment.CommentedItems.Count <= 0)
      return false;
    foreach (ParagraphItem commentedItem in (CollectionImpl) comment.CommentedItems)
    {
      if (!(commentedItem is WTextRange) || !((commentedItem as WTextRange).Text == string.Empty))
        return false;
    }
    return true;
  }

  private string BuildColorValue(
    WCharacterFormat cFormat,
    Color cFormatColor,
    WCharacterFormat baseCFormat,
    Color baseCFormatColor,
    int optionKey,
    string value)
  {
    if (optionKey == 63 /*0x3F*/ && !cFormatColor.IsEmpty)
      cFormatColor = WordColor.ColorsArray[(int) (byte) WordColor.ConvertColorToId(cFormatColor)];
    if (cFormat.HasValue(optionKey) && !cFormatColor.IsEmpty)
    {
      if (!cFormatColor.IsNamedColor || !cFormatColor.IsKnownColor)
        return this.BuildColor(cFormatColor, value);
      return 63 /*0x3F*/ == optionKey ? this.BuildHighlightNamedColor(cFormatColor, value) : this.BuildNamedColor(cFormatColor, value);
    }
    if (baseCFormat == null || !baseCFormat.HasValue(optionKey) || baseCFormatColor.IsEmpty)
      return string.Empty;
    if (!baseCFormatColor.IsNamedColor || !baseCFormatColor.IsKnownColor)
      return this.BuildColor(baseCFormatColor, value);
    return 63 /*0x3F*/ == optionKey ? this.BuildHighlightNamedColor(baseCFormatColor, value) : this.BuildNamedColor(baseCFormatColor, value);
  }

  private string BuildHighlightNamedColor(Color color, string value)
  {
    Color color1 = new Color();
    switch (color.Name)
    {
      case "Maroon":
        color1 = Color.FromArgb((int) byte.MaxValue, 0, 0);
        break;
      case "Green":
        color1 = Color.FromArgb(0, (int) byte.MaxValue, 0);
        break;
      case "Olive":
        color1 = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 0);
        break;
      case "Navy":
        color1 = Color.FromArgb(0, 0, 128 /*0x80*/);
        break;
      case "Purple":
        color1 = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
        break;
      case "Teal":
        color1 = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
        break;
      case "Red":
        color1 = Color.FromArgb((int) byte.MaxValue, 0, 0);
        break;
      case "Lime":
        color1 = Color.FromArgb(0, 128 /*0x80*/, 0);
        break;
      case "Yellow":
        color1 = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0);
        break;
      case "Blue":
        color1 = Color.FromArgb(0, 0, (int) byte.MaxValue);
        break;
      case "Fuchsia":
        color1 = Color.FromArgb(128 /*0x80*/, 0, 128 /*0x80*/);
        break;
      case "Aqua":
        color1 = Color.FromArgb(0, 128 /*0x80*/, 128 /*0x80*/);
        break;
      case "Gold":
        color1 = Color.FromArgb(128 /*0x80*/, 100, 0);
        break;
    }
    return !color1.IsEmpty ? this.BuildColor(color1, value) : this.BuildColor(color, value);
  }

  private string BuildNamedColor(Color color, string value)
  {
    Color color1 = new Color();
    switch (color.Name)
    {
      case "Maroon":
        color1 = Color.FromArgb(128 /*0x80*/, 0, 0);
        break;
      case "Green":
        color1 = Color.FromArgb(0, 128 /*0x80*/, 0);
        break;
      case "Olive":
        color1 = Color.FromArgb(128 /*0x80*/, 128 /*0x80*/, 0);
        break;
      case "Navy":
        color1 = Color.FromArgb(0, 0, 128 /*0x80*/);
        break;
      case "Purple":
        color1 = Color.FromArgb(128 /*0x80*/, 0, 128 /*0x80*/);
        break;
      case "Teal":
        color1 = Color.FromArgb(0, 128 /*0x80*/, 128 /*0x80*/);
        break;
      case "Red":
        color1 = Color.FromArgb((int) byte.MaxValue, 0, 0);
        break;
      case "Lime":
        color1 = Color.FromArgb(0, (int) byte.MaxValue, 0);
        break;
      case "Yellow":
        color1 = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0);
        break;
      case "Blue":
        color1 = Color.FromArgb(0, 0, (int) byte.MaxValue);
        break;
      case "Fuchsia":
        color1 = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
        break;
      case "Aqua":
        color1 = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
        break;
      case "Gold":
        color1 = Color.FromArgb((int) byte.MaxValue, 215, 0);
        break;
    }
    return !color1.IsEmpty ? this.BuildColor(color1, value) : this.BuildColor(color, value);
  }

  private void CheckFootEndnote()
  {
    this.m_mainBodyBytesList.Add(!this.m_hasFootnote || !this.m_hasEndnote ? (!this.m_hasEndnote ? this.m_encoding.GetBytes(this.c_slashSymbol + "fet0") : this.m_encoding.GetBytes(this.c_slashSymbol + "fet1")) : this.m_encoding.GetBytes(this.c_slashSymbol + "fet2"));
  }

  private string BuildFieldType(FieldType type)
  {
    switch (type)
    {
      case FieldType.FieldRef:
        return "REF ";
      case FieldType.FieldIf:
        return "IF ";
      case FieldType.FieldIndex:
        return "INDEX ";
      case FieldType.FieldStyleRef:
        return "STYLEREF ";
      case FieldType.FieldSequence:
        return "SEQ ";
      case FieldType.FieldTOC:
        return "TOC ";
      case FieldType.FieldInfo:
        return "INFO ";
      case FieldType.FieldTitle:
        return "TITLE ";
      case FieldType.FieldSubject:
        return "SUBJECT ";
      case FieldType.FieldAuthor:
        return "AUTHOR ";
      case FieldType.FieldKeyWord:
        return "KEYWORDS ";
      case FieldType.FieldComments:
        return "COMMENTS ";
      case FieldType.FieldLastSavedBy:
        return "LASTSAVEDBY ";
      case FieldType.FieldCreateDate:
        return "CREATEDATE ";
      case FieldType.FieldSaveDate:
        return "SAVEDATE ";
      case FieldType.FieldPrintDate:
        return "PRINTDATE ";
      case FieldType.FieldRevisionNum:
        return "REVNUM ";
      case FieldType.FieldEditTime:
        return "EDITTIME ";
      case FieldType.FieldNumPages:
        return "NUMPAGES ";
      case FieldType.FieldNumWords:
        return "NUMWORDS ";
      case FieldType.FieldNumChars:
        return "NUMCHARS ";
      case FieldType.FieldFileName:
        return "FILENAME ";
      case FieldType.FieldTemplate:
        return "TEMPLATE ";
      case FieldType.FieldDate:
        return "DATE ";
      case FieldType.FieldTime:
        return "TIME ";
      case FieldType.FieldPage:
        return "PAGE ";
      case FieldType.FieldQuote:
        return "QUOTE ";
      case FieldType.FieldPageRef:
        return "PAGEREF ";
      case FieldType.FieldAsk:
        return "ASK ";
      case FieldType.FieldFillIn:
        return "FILLIN ";
      case FieldType.FieldPrint:
        return "PRINT ";
      case FieldType.FieldGoToButton:
        return "GOTOBUTTON ";
      case FieldType.FieldMacroButton:
        return "MACROBUTTON ";
      case FieldType.FieldAutoNumOutline:
        return "AUTONUMOUT ";
      case FieldType.FieldAutoNumLegal:
        return "AUTONUMLGL ";
      case FieldType.FieldAutoNum:
        return "AUTONUM ";
      case FieldType.FieldLink:
        return "LINK ";
      case FieldType.FieldSymbol:
        return "SYMBOL ";
      case FieldType.FieldMergeField:
        return "MERGEFIELD ";
      case FieldType.FieldUserName:
        return "USERNAME ";
      case FieldType.FieldUserInitials:
        return "USERINITIALS ";
      case FieldType.FieldUserAddress:
        return "USERADDRESS ";
      case FieldType.FieldBarCode:
        return "BARCODE ";
      case FieldType.FieldDocVariable:
        return "DOCVARIABLE ";
      case FieldType.FieldSection:
        return "SECTION ";
      case FieldType.FieldSectionPages:
        return "SECTIONPAGES ";
      case FieldType.FieldIncludePicture:
        return "INCLUDEPICTURE ";
      case FieldType.FieldIncludeText:
        return "INCLUDETEXT ";
      case FieldType.FieldFileSize:
        return "FILESIZE ";
      case FieldType.FieldFormTextInput:
        return "FORMTEXT ";
      case FieldType.FieldFormCheckBox:
        return "FORMCHECKBOX ";
      case FieldType.FieldNoteRef:
        return "NOTEREF ";
      case FieldType.FieldTOA:
        return "TOA ";
      case FieldType.FieldPrivate:
        return "PRIVATE ";
      case FieldType.FieldAutoText:
        return "AUTOTEXT ";
      case FieldType.FieldAddin:
        return "ADDIN ";
      case FieldType.FieldFormDropDown:
        return "FORMDROPDOWN ";
      case FieldType.FieldAdvance:
        return "ADVANCE ";
      case FieldType.FieldDocProperty:
        return "DOCPROPERTY ";
      case FieldType.FieldHyperlink:
        return "HYPERLINK ";
      case FieldType.FieldAutoTextList:
        return "AUTOTEXTLIST ";
      case FieldType.FieldListNum:
        return "LISTNUM ";
      default:
        return string.Empty;
    }
  }

  private string GetRtfShapeColor(Color color)
  {
    string str1 = color.R.ToString("X");
    string str2 = color.G.ToString("X");
    int num = int.Parse(color.B.ToString("X") + str2 + str1, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
    if (color != Color.Red && color != Color.Green && color != Color.Blue && color.R != (byte) 128 /*0x80*/ && color.G != (byte) 128 /*0x80*/ && color.B != (byte) 128 /*0x80*/ && color.R != (byte) 192 /*0xC0*/ && color.G != (byte) 192 /*0xC0*/ && color.B != (byte) 192 /*0xC0*/)
      num *= 16 /*0x10*/;
    return num.ToString();
  }

  private string GetRtfPageBackgroundColor(Color color)
  {
    string str1 = color.R.ToString("X");
    string str2 = color.G.ToString("X");
    return int.Parse(color.B.ToString("X") + str2 + str1, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture).ToString();
  }

  private void WriteElements(string param)
  {
    if (string.IsNullOrEmpty(param))
      return;
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    this.m_stream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(param);
    this.m_stream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes("}");
    this.m_stream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes(Environment.NewLine);
    this.m_stream.Write(bytes4, 0, bytes4.Length);
  }

  private string GetNextFontId(bool isBidi)
  {
    return isBidi ? $"{this.c_slashSymbol}af{(object) this.m_fontId++}" : $"{this.c_slashSymbol}f{(object) this.m_fontId++}";
  }

  private int GetNextId() => this.m_uniqueId++;

  private int GetNextColorId() => this.m_colorId++;

  private string IsFontEntryExits(string fontName, bool IsBidi)
  {
    string key = $"{(!this.m_isCyrillicText ? (fontName[0] >= 'A' && fontName[0] <= 'Z' || fontName[0] >= 'a' && fontName[0] <= 'z' ? "fcharset0" : "fcharset134") : "fcharset204")}-{fontName}";
    if (!IsBidi)
    {
      if (this.FontEntries.ContainsKey(key))
        return this.FontEntries[key];
    }
    else if (this.AssociatedFontEntries.ContainsKey(key))
      return this.AssociatedFontEntries[key];
    return (string) null;
  }

  private void AppendFont(string fontId, string fontName)
  {
    MemoryStream memoryStream = new MemoryStream();
    memoryStream.Write(this.m_fontBytes, 0, this.m_fontBytes.Length);
    byte[] bytes1 = this.m_encoding.GetBytes("{");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes(fontId);
    memoryStream.Write(bytes2, 0, bytes2.Length);
    string str;
    if (this.m_isCyrillicText)
    {
      byte[] bytes3 = this.m_encoding.GetBytes(this.c_slashSymbol + "fcharset204");
      memoryStream.Write(bytes3, 0, bytes3.Length);
      str = "fcharset204";
    }
    else if (fontName[0] >= 'A' && fontName[0] <= 'Z' || fontName[0] >= 'a' && fontName[0] <= 'z')
    {
      byte[] bytes4 = this.m_encoding.GetBytes(this.c_slashSymbol + "fcharset0");
      memoryStream.Write(bytes4, 0, bytes4.Length);
      str = "fcharset0";
    }
    else
    {
      byte[] bytes5 = this.m_encoding.GetBytes(this.c_slashSymbol + "fcharset134");
      memoryStream.Write(bytes5, 0, bytes5.Length);
      str = "fcharset134";
    }
    byte[] bytes6 = this.m_encoding.GetBytes(" ");
    memoryStream.Write(bytes6, 0, bytes6.Length);
    if (fontName[0] >= 'A' && fontName[0] <= 'Z' || fontName[0] >= 'a' && fontName[0] <= 'z')
    {
      byte[] bytes7 = this.m_encoding.GetBytes(fontName);
      memoryStream.Write(bytes7, 0, bytes7.Length);
    }
    else
    {
      byte[] bytes8 = this.m_encoding.GetBytes("{\\*\\falt ");
      memoryStream.Write(bytes8, 0, bytes8.Length);
      byte[] bytes9 = this.m_encoding.GetBytes(this.PrepareText(fontName));
      memoryStream.Write(bytes9, 0, bytes9.Length);
      byte[] bytes10 = this.m_encoding.GetBytes(" }");
      memoryStream.Write(bytes10, 0, bytes10.Length);
      foreach (byte num in Encoding.GetEncoding("GB2312").GetBytes(fontName))
      {
        byte[] bytes11 = this.m_encoding.GetBytes("\\'");
        memoryStream.Write(bytes11, 0, bytes11.Length);
        byte[] bytes12 = this.m_encoding.GetBytes(num.ToString("X").ToLower());
        memoryStream.Write(bytes12, 0, bytes12.Length);
      }
    }
    string key = $"{str}-{fontName}";
    byte[] bytes13 = this.m_encoding.GetBytes(";}");
    memoryStream.Write(bytes13, 0, bytes13.Length);
    if (fontId.StartsWithExt("\\a"))
      this.AssociatedFontEntries.Add(key, fontId);
    else
      this.FontEntries.Add(key, fontId);
    byte[] bytes14 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes14, 0, bytes14.Length);
    this.m_fontBytes = memoryStream.ToArray();
  }

  private string BuildColor(Color color, string attributeStr)
  {
    if (color.IsEmpty)
      return string.Empty;
    if (this.ColorTable.ContainsKey(color))
      return attributeStr + this.ColorTable[color].ToString();
    this.ColorTable.Add(color, this.m_colorId);
    MemoryStream memoryStream = new MemoryStream();
    memoryStream.Write(this.m_colorBytes, 0, this.m_colorBytes.Length);
    byte[] bytes1 = this.m_encoding.GetBytes($"{this.c_slashSymbol}red{(object) color.R}");
    memoryStream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = this.m_encoding.GetBytes($"{this.c_slashSymbol}green{(object) color.G}");
    memoryStream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = this.m_encoding.GetBytes($"{this.c_slashSymbol}blue{(object) color.B}");
    memoryStream.Write(bytes3, 0, bytes3.Length);
    byte[] bytes4 = this.m_encoding.GetBytes(";");
    memoryStream.Write(bytes4, 0, bytes4.Length);
    byte[] bytes5 = this.m_encoding.GetBytes(Environment.NewLine);
    memoryStream.Write(bytes5, 0, bytes5.Length);
    this.m_colorBytes = memoryStream.ToArray();
    return attributeStr + (object) this.GetNextColorId();
  }

  private string WriteFontName(WCharacterFormat cFormat)
  {
    string str = this.IsFontEntryExits(cFormat.FontName, false);
    if (str != null)
      return str;
    string nextFontId = this.GetNextFontId(false);
    this.AppendFont(nextFontId, cFormat.FontName);
    return nextFontId;
  }

  private string WriteFontNameBidi(WCharacterFormat cFormat)
  {
    string str = this.IsFontEntryExits(cFormat.FontName, true);
    if (str != null)
      return str;
    string nextFontId = this.GetNextFontId(true);
    this.AppendFont(nextFontId, cFormat.FontName);
    return nextFontId;
  }

  private bool HasParaEnd(WParagraph para)
  {
    return (!para.IsInCell || para.NextSibling != null) && para.OwnerTextBody != null && (!(para.OwnerTextBody.OwnerBase is WFootnote) || para.NextSibling != null) && (!(para.OwnerTextBody.OwnerBase is WComment) || para.NextSibling != null) && (!(para.OwnerTextBody.OwnerBase is WSection) || (para.OwnerTextBody.OwnerBase as WSection).PreviousSibling != null || para.Items.Count != 0 || para.NextSibling != null || para.PreviousSibling is WTable || para.Document.Sections.Count <= 1);
  }

  private string PrepareText(string text)
  {
    if (string.IsNullOrEmpty(text))
      return (string) null;
    text = text.Replace(this.c_symbol92, this.c_symbol92 + this.c_symbol92);
    string empty = string.Empty;
    foreach (char ch in text.ToCharArray())
    {
      if (ch >= '͐' && ch <= 'я')
      {
        string str = $"{this.c_slashSymbol}u{((int) ch).ToString()}\\'3f";
        empty += str;
        this.m_isCyrillicText = true;
      }
      else
        empty += ch.ToString();
    }
    string str1 = " " + empty;
    for (int startIndex = str1.IndexOf('\t'); startIndex != -1; startIndex = str1.IndexOf('\t'))
      str1 = str1.Remove(startIndex, 1).Insert(startIndex, this.c_slashSymbol + "tab");
    return this.ReplaceUnicode(str1.Replace(this.c_transfer, this.c_slashSymbol + "~").Replace(this.c_symbol123, this.c_slashSymbol + "{").Replace(this.c_symbol125, this.c_slashSymbol + "}").Replace(this.c_symbol30, this.c_slashSymbol + "_").Replace(this.c_symbol31, this.c_slashSymbol + "-").Replace(this.c_symbol61553, $"{this.c_slashSymbol}u-3983{this.c_slashSymbol}'3f").Replace(this.c_symbol61549, $"{this.c_slashSymbol}u-3987{this.c_slashSymbol}'3f").Replace(this.c_singleOpenQuote, this.c_slashSymbol + "lquote ").Replace(this.c_singleCloseQuote, this.c_slashSymbol + "rquote ").Replace(this.c_enDash, this.c_slashSymbol + "endash ").Replace(this.c_emDash, this.c_slashSymbol + "emdash ").Replace(this.c_enSpace, $"{this.c_slashSymbol}u8194{this.c_slashSymbol}'20").Replace(this.c_emSpace, $"{this.c_slashSymbol}u8195{this.c_slashSymbol}'20").Replace(this.c_copyRight, this.c_slashSymbol + "'a9").Replace(this.c_registered, this.c_slashSymbol + "'ae").Replace(this.c_tradeMark, this.c_slashSymbol + "'99").Replace(this.c_section, this.c_slashSymbol + "'a7").Replace(this.c_lineBreak, this.c_slashSymbol + "line").Replace(this.c_paraMark, this.c_slashSymbol + "'b6").Replace(this.c_doubleOpenQuote, this.c_slashSymbol + "'93").Replace(this.c_doubleCloseQuote, this.c_slashSymbol + "'94"));
  }

  private string ReplaceUnicode(string text)
  {
    foreach (char ch in text.ToCharArray())
    {
      if (ch > '\u007F')
      {
        string newValue = $"\\u{(object) Convert.ToUInt32(ch)}?";
        text = text.Replace(ch.ToString(), newValue);
      }
    }
    return text;
  }

  private string BuildTextRangeStr(WCharacterFormat cFormat, string text)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("{");
    stringBuilder.Append(text);
    stringBuilder.Insert(1, this.BuildCharacterFormat(cFormat));
    stringBuilder.Append("}");
    stringBuilder.Append(Environment.NewLine);
    this.m_isCyrillicText = false;
    return stringBuilder.ToString();
  }

  private bool WriteFieldEnd(WFieldMark mark)
  {
    return mark.PreviousSibling != null && (mark.PreviousSibling is WCheckBox || mark.PreviousSibling is WDropDownFormField || mark.PreviousSibling is WField && (mark.PreviousSibling as WField).FieldType == FieldType.FieldGoToButton);
  }

  private WSection GetOwnerSection(Entity entity)
  {
    while (entity != null && entity.EntityType != EntityType.Section)
      entity = entity.Owner;
    return entity != null ? entity as WSection : (WSection) null;
  }

  private void InitCellEndPos()
  {
  }

  private enum BorderType
  {
    Right,
    Left,
    Top,
    Bottom,
  }
}
