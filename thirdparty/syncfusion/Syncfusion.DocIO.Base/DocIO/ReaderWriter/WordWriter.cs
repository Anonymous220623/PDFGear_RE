// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordWriter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO.Net;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;
using Syncfusion.DocIO.ReaderWriter.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordWriter : WordWriterBase, IWordWriter, IWordWriterBase, IDisposable
{
  private bool m_bHeaderWritten;
  private int m_commentID = -1;
  private bool m_bLastParagrapfEnd;
  private SectionProperties m_secProperties;
  private IWordSubdocumentWriter m_lastWriter;
  private BuiltinDocumentProperties m_builtinProp = new BuiltinDocumentProperties();
  private CustomDocumentProperties m_customProp = new CustomDocumentProperties();
  private bool m_isTemplate;
  private bool m_bIsWriteProtected;
  private bool m_bHasPicture;

  public SectionProperties SectionProperties => this.m_secProperties;

  public BuiltinDocumentProperties BuiltinDocumentProperties
  {
    get => this.m_builtinProp;
    set => this.m_builtinProp = value;
  }

  public CustomDocumentProperties CustomDocumentProperties
  {
    get => this.m_customProp;
    set => this.m_customProp = value;
  }

  public DOPDescriptor DOP
  {
    get => this.m_docInfo.TablesData.DOP;
    set => this.m_docInfo.TablesData.DOP = value;
  }

  public GrammarSpelling GrammarSpellingData
  {
    get => this.m_docInfo.TablesData.GrammarSpellingData;
    set => this.m_docInfo.TablesData.GrammarSpellingData = value;
  }

  public MemoryStream MacrosStream
  {
    get => this.m_streamsManager.MacrosStream;
    set => this.m_streamsManager.MacrosStream = value;
  }

  public MemoryStream ObjectPoolStream
  {
    get => this.m_streamsManager.ObjectPoolStream;
    set => this.m_streamsManager.ObjectPoolStream = value;
  }

  public byte[] MacroCommands
  {
    get => this.m_docInfo.TablesData.MacroCommands;
    set => this.m_docInfo.TablesData.MacroCommands = value;
  }

  public byte[] Variables
  {
    get => this.m_docInfo.TablesData.Variables;
    set => this.m_docInfo.TablesData.Variables = value;
  }

  public string StandardAsciiFont
  {
    get => this.m_docInfo.TablesData.StandardAsciiFont;
    set => this.m_docInfo.TablesData.StandardAsciiFont = value;
  }

  public string StandardFarEastFont
  {
    get => this.m_docInfo.TablesData.StandardFarEastFont;
    set => this.m_docInfo.TablesData.StandardFarEastFont = value;
  }

  public string StandardBidiFont
  {
    get => this.m_docInfo.TablesData.StandardBidiFont;
    set => this.m_docInfo.TablesData.StandardBidiFont = value;
  }

  public string StandardNonFarEastFont
  {
    get => this.m_docInfo.TablesData.StandardNonFarEastFont;
    set => this.m_docInfo.TablesData.StandardNonFarEastFont = value;
  }

  internal bool IsTemplate
  {
    get => this.m_isTemplate;
    set => this.m_isTemplate = value;
  }

  public byte[] AssociatedStrings
  {
    get => this.m_docInfo.TablesData.AsociatedStrings;
    set => this.m_docInfo.TablesData.AsociatedStrings = value;
  }

  public byte[] SttbfRMark
  {
    get => this.m_docInfo.TablesData.SttbfRMark;
    set => this.m_docInfo.TablesData.SttbfRMark = value;
  }

  internal bool WriteProtected
  {
    get => this.m_bIsWriteProtected;
    set => this.m_bIsWriteProtected = value;
  }

  internal bool HasPicture
  {
    get => this.m_bHasPicture;
    set => this.m_bHasPicture = value;
  }

  public WordWriter(Stream stream)
  {
    this.m_streamsManager = new StreamsManager(stream, true);
    this.InitClass();
  }

  public WordWriter(string fileName)
  {
    this.m_streamsManager = new StreamsManager(fileName, true);
    this.InitClass();
  }

  public void WriteDocumentHeader()
  {
    this.m_bHeaderWritten = true;
    this.AddSepxProperties();
  }

  public IWordSubdocumentWriter GetSubdocumentWriter(WordSubdocument subDocumentType)
  {
    if (!this.m_bHeaderWritten)
      throw new InvalidOperationException("Call WriteDocumentHeader before this method");
    if (!this.m_bLastParagrapfEnd)
    {
      this.WriteMarker(WordChunkType.ParagraphEnd);
      this.m_bLastParagrapfEnd = true;
    }
    switch (subDocumentType)
    {
      case WordSubdocument.Footnote:
        return this.m_lastWriter = (IWordSubdocumentWriter) new WordFootnoteWriter(this);
      case WordSubdocument.HeaderFooter:
        return this.m_lastWriter = (IWordSubdocumentWriter) new WordHeaderFooterWriter(this);
      case WordSubdocument.Endnote:
        return this.m_lastWriter = (IWordSubdocumentWriter) new WordEndnoteWriter(this);
      case WordSubdocument.Annotation:
        return this.m_lastWriter = (IWordSubdocumentWriter) new WordAnnotationWriter(this);
      case WordSubdocument.TextBox:
        return this.m_lastWriter = (IWordSubdocumentWriter) new WordTextBoxWriter(this);
      case WordSubdocument.HeaderTextBox:
        return this.m_lastWriter = (IWordSubdocumentWriter) new WordHFTextBoxWriter(this);
      default:
        return (IWordSubdocumentWriter) null;
    }
  }

  public override void WriteChunk(string textChunk)
  {
    if (!this.m_bHeaderWritten)
      throw new InvalidOperationException("Call WriteDocumentHeader before this method");
    base.WriteChunk(textChunk);
  }

  public override void WriteSafeChunk(string textChunk)
  {
    if (!this.m_bHeaderWritten)
      throw new InvalidOperationException("Call WriteDocumentHeader before this method");
    base.WriteSafeChunk(textChunk);
  }

  public override void WriteMarker(WordChunkType chunkType)
  {
    if (!this.m_bHeaderWritten)
      throw new InvalidOperationException("Call WriteDocumentHeader before this method");
    base.WriteMarker(chunkType);
    switch (chunkType)
    {
      case WordChunkType.SectionEnd:
        this.WriteChar('\f');
        this.AddSepxProperties();
        break;
      case WordChunkType.ColumnBreak:
        this.WriteChar('\u000E');
        break;
    }
  }

  public void WriteDocumentEnd(
    string password,
    string author,
    ushort fibVersion,
    Dictionary<string, Storage> oleObjectCollection)
  {
    this.m_docInfo.Fib.FDot = this.m_isTemplate;
    this.m_docInfo.Fib.FReadOnlyRecommended = this.WriteProtected;
    this.m_docInfo.Fib.FHasPic = this.HasPicture;
    this.CompleteMainStream();
    if (!string.IsNullOrEmpty(password))
    {
      byte[] buffer = new byte[52];
      this.m_streamsManager.TableStream.Position = 0L;
      this.m_streamsManager.TableStream.Write(buffer, 0, buffer.Length);
      this.m_docInfo.Fib.FEncrypted = true;
    }
    this.WriteTables(author);
    this.m_docInfo.Fib.Write((Stream) this.m_streamsManager.MainStream, fibVersion);
    this.WriteSummary();
    if (!string.IsNullOrEmpty(password))
    {
      WordDecryptor wordDecryptor = new WordDecryptor(this.m_streamsManager.TableStream, this.m_streamsManager.MainStream, this.m_streamsManager.DataStream, this.m_docInfo.Fib);
      wordDecryptor.Encrypt(password);
      this.m_streamsManager.UpdateStreams(wordDecryptor.MainStream, wordDecryptor.TableStream, wordDecryptor.DataStream);
      this.m_docInfo.Fib.WriteAfterEncryption((Stream) this.m_streamsManager.MainStream);
    }
    this.m_streamsManager.SaveStg(oleObjectCollection);
  }

  public void InsertComment(WCommentFormat format)
  {
    ++this.m_commentID;
    AnnotationDescriptor atrd = new AnnotationDescriptor();
    int num = this.m_docInfo.TablesData.Annotations.AddGXAO(format.User);
    int textPos = this.GetTextPos();
    atrd.UserInitials = format.UserInitials;
    atrd.IndexToGrpOwner = (short) num;
    atrd.TagBkmk = this.m_commentID;
    this.m_docInfo.TablesData.Annotations.AddDescriptor(atrd, textPos, textPos - format.BookmarkStartOffset, textPos + format.BookmarkEndOffset);
    this.WriteMarker(WordChunkType.Annotation);
  }

  public void InsertFootnote(WFootnote footnote)
  {
    int textPos = this.GetTextPos();
    if (footnote.FootnoteType == FootnoteType.Footnote)
      this.m_docInfo.TablesData.Footnotes.AddReferense(textPos, footnote.IsAutoNumbered);
    else
      this.m_docInfo.TablesData.Endnotes.AddReferense(textPos, footnote.IsAutoNumbered);
    if (footnote.IsAutoNumbered)
      this.WriteMarker(WordChunkType.Footnote);
    else if (footnote.CustomMarkerIsSymbol)
    {
      int index = this.StyleSheet.FontNameToIndex(footnote.SymbolFontName);
      if (index >= 0)
      {
        this.CHPX.PropertyModifiers.SetUShortValue(19023, (ushort) index);
      }
      else
      {
        this.CHPX.PropertyModifiers.SetUShortValue(19023, (ushort) this.StyleSheet.FontNamesList.Count);
        this.StyleSheet.UpdateFontName(footnote.SymbolFontName);
      }
      this.CHPX.PropertyModifiers.SetByteArrayValue(27145, new SymbolDescriptor()
      {
        CharCode = footnote.SymbolCode,
        FontCode = ((short) this.StyleSheet.FontNameToIndex(footnote.SymbolFontName))
      }.Save());
      this.WriteMarker(WordChunkType.Symbol);
    }
    else
      this.WriteString(footnote.m_strCustomMarker);
  }

  public void InsertPageBreak()
  {
    Stream mainStream = (Stream) this.m_streamsManager.MainStream;
    byte[] bytes = this.m_docInfo.Fib.Encoding.GetBytes(SpecialCharacters.PageBreakStr);
    mainStream.Write(bytes, 0, bytes.Length);
    this.AddChpxProperties(false);
    this.IncreaseCcp(1);
  }

  private void CompleteMainStream()
  {
    if (this.m_lastWriter != null)
      this.m_lastWriter.WriteMarker(WordChunkType.ParagraphEnd);
    if (!this.m_bLastParagrapfEnd)
    {
      this.WriteMarker(WordChunkType.ParagraphEnd);
      this.m_bLastParagrapfEnd = true;
    }
    this.m_docInfo.Fib.UpdateFcMac();
    while (this.m_streamsManager.MainStream.Position < (long) this.m_docInfo.Fib.BaseReserved6)
      this.m_streamsManager.MainWriter.Write((byte) 0);
    uint pos = (uint) (this.m_iStartText + this.m_docInfo.Fib.CcpText * this.m_docInfo.Fib.EncodingCharSize);
    this.m_docInfo.FkpData.CloneAndAddLastPapx(pos);
    this.m_docInfo.FkpData.CloneAndAddLastChpx(pos);
    this.m_docInfo.FkpData.Write((Stream) this.m_streamsManager.MainStream);
    if (this.Escher != null)
      this.Escher.WriteContainersData((Stream) this.m_streamsManager.MainStream);
    this.m_docInfo.Fib.CbMac = (int) this.m_streamsManager.MainStream.Position;
  }

  private void WriteZeroBlock(int size)
  {
    byte[] buffer = new byte[size];
    for (int index = 0; index < buffer.Length; ++index)
      buffer[index] = (byte) 0;
    this.m_streamsManager.MainWriter.Write(buffer, 0, buffer.Length);
  }

  private void WriteTables(string author)
  {
    this.m_docInfo.TablesData.AddStyleSheetTable(this.StyleSheet);
    this.m_docInfo.TablesData.Write((Stream) this.m_streamsManager.TableStream, this.m_lastWriter != null);
  }

  private void WriteSummary()
  {
    Guid guid1 = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
    Guid guid2 = new Guid("D5CDD502-2E9C-101B-9397-08002B2CF9AE");
    DocumentPropertyCollection propertyCollection1 = new DocumentPropertyCollection();
    PropertySection section1 = new PropertySection(guid1, -1);
    propertyCollection1.Sections.Add(section1);
    DocumentPropertyCollection propertyCollection2 = new DocumentPropertyCollection();
    PropertySection section2 = new PropertySection(guid2, -1);
    propertyCollection2.Sections.Add(section2);
    PropertySection section3 = (PropertySection) null;
    if (this.m_customProp.CustomHash.Values.Count > 0)
    {
      section3 = new PropertySection(new Guid("D5CDD505-2E9C-101B-9397-08002B2CF9AE"), -1);
      propertyCollection2.Sections.Add(section3);
    }
    this.WriteProps(section1, (ICollection) this.m_builtinProp.SummaryHash.Values);
    this.WriteProps(section2, (ICollection) this.m_builtinProp.DocumentHash.Values);
    if (section3 != null)
      this.WriteProps(section3, (ICollection) this.m_customProp.CustomHash.Values);
    propertyCollection1.Serialize((Stream) this.StreamsManager.SummaryInfoStream);
    propertyCollection2.Serialize((Stream) this.StreamsManager.DocumentSummaryInfoStream);
  }

  private void WriteProps(PropertySection section, ICollection values)
  {
    int iPropertyId = 2;
    foreach (DocumentProperty property in (IEnumerable) values)
    {
      PropertyData propertyData = this.ConvertToPropertyData(property, iPropertyId);
      section.Properties.Add(propertyData);
      ++iPropertyId;
    }
  }

  private PropertyData ConvertToPropertyData(DocumentProperty property, int iPropertyId)
  {
    PropertyData variant = new PropertyData();
    property.FillPropVariant((IPropertyData) variant, iPropertyId);
    if (property.Value != null && property.PropertyType == Syncfusion.CompoundFile.DocIO.PropertyType.Empty && property.Value is string)
      variant.Type = VarEnum.VT_LPWSTR;
    if (property.InternalName != null)
      variant.Name = property.InternalName;
    return variant;
  }

  private void AddSepxProperties()
  {
    this.m_docInfo.FkpData.AddSepxProperties(this.GetTextPos(), this.SectionProperties.CloneSepx());
    this.m_secProperties = new SectionProperties();
  }

  internal override void Close()
  {
    base.Close();
    this.m_secProperties = (SectionProperties) null;
    this.m_builtinProp = (BuiltinDocumentProperties) null;
    this.m_customProp = (CustomDocumentProperties) null;
  }

  protected override void InitClass()
  {
    base.InitClass();
    this.m_type = WordSubdocument.Main;
    this.m_secProperties = new SectionProperties();
    this.m_docInfo = new DocInfo(this.m_streamsManager);
    this.m_streamsManager.MainStream.Seek((long) this.m_docInfo.Fib.BaseReserved5, SeekOrigin.Begin);
    this.m_iStartText = (int) this.m_streamsManager.MainStream.Position;
  }

  protected override void IncreaseCcp(int dataLength) => this.m_docInfo.Fib.CcpText += dataLength;

  public void Dispose()
  {
  }
}
