// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordReader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using Syncfusion.CompoundFile.DocIO.Net;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.DocIO.ReaderWriter.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordReader : WordReaderBase, IWordReader, IWordReaderBase, IDisposable
{
  private const int DEF_PID_CODEPAGE = 1;
  protected bool m_bDisposed;
  protected bool m_bDestroyStream;
  private SectionProperties m_secProperties;
  private bool m_bHeaderRead;
  private IWordSubdocumentReader m_lastReader;
  private BuiltinDocumentProperties m_builtinProp = new BuiltinDocumentProperties();
  private CustomDocumentProperties m_custProp = new CustomDocumentProperties();
  private Encoding m_strEncoding;
  private int m_customFnSplittedTextLength = -1;
  private Dictionary<int, string> m_sttbFRAuthorNames;

  public WordReader(Stream stream)
  {
    this.m_streamsManager = new StreamsManager(stream, false);
    this.InitClass();
  }

  public WordReader(string fileName)
  {
    this.m_streamsManager = new StreamsManager(fileName, false);
    this.InitClass();
  }

  public event NeedPasswordEventHandler NeedPassword;

  public DOPDescriptor DOP => this.m_docInfo.TablesData.DOP;

  public MainStatePositions StatePositions => (MainStatePositions) this.m_statePositions;

  public int SectionNumber => this.StatePositions.SectionIndex + 1;

  public SectionProperties SectionProperties => this.m_secProperties;

  public BuiltinDocumentProperties BuiltinDocumentProperties => this.m_builtinProp;

  public CustomDocumentProperties CustomDocumentProperties => this.m_custProp;

  public MemoryStream MacrosStream => this.m_streamsManager.MacrosStream;

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

  public GrammarSpelling GrammarSpellingData => this.m_docInfo.TablesData.GrammarSpellingData;

  public bool IsFootnote
  {
    get
    {
      if (this.TextChunk.Length == 0)
        return false;
      int length = this.TextChunk.Length;
      int startPosition = this.CurrentTextPosition - length;
      int num = length >= 10 ? 10 : length;
      for (int index = 0; index < num; ++index)
      {
        if (this.m_docInfo.TablesData.Footnotes.HasReference(startPosition + index))
          return true;
        if (num >= 10 && index == num - 1)
        {
          int textLength = 0;
          if (this.m_docInfo.TablesData.Footnotes.HasReference(startPosition, this.CurrentTextPosition, ref textLength))
          {
            this.m_customFnSplittedTextLength = textLength;
            return true;
          }
        }
      }
      return false;
    }
  }

  internal int CustomFnSplittedTextLength
  {
    get => this.m_customFnSplittedTextLength;
    set => this.m_customFnSplittedTextLength = value;
  }

  public bool IsEndnote
  {
    get
    {
      if (this.TextChunk.Length == 0)
        return false;
      int length = this.TextChunk.Length;
      if (this.TextChunk.TrimStart(' ') != string.Empty)
        length = this.TextChunk.TrimStart(' ').Length;
      int num1 = this.CurrentTextPosition - length;
      int num2 = length >= 10 ? 10 : length;
      for (int index = 0; index < num2; ++index)
      {
        if (this.m_docInfo.TablesData.Endnotes.HasReference(num1 + index))
          return true;
      }
      return false;
    }
  }

  public string StandardAsciiFont => this.m_docInfo.TablesData.StandardAsciiFont;

  public string StandardFarEastFont => this.m_docInfo.TablesData.StandardFarEastFont;

  public string StandardNonFarEastFont => this.m_docInfo.TablesData.StandardNonFarEastFont;

  public string StandardBidiFont => this.m_docInfo.TablesData.StandardBidiFont;

  public bool IsEncrypted => this.m_docInfo.Fib.FEncrypted;

  internal byte[] AssociatedStrings
  {
    get => this.m_docInfo.TablesData.AsociatedStrings;
    set => this.m_docInfo.TablesData.AsociatedStrings = value;
  }

  internal byte[] SttbfRMark
  {
    get => this.m_docInfo.TablesData.SttbfRMark;
    set => this.m_docInfo.TablesData.SttbfRMark = value;
  }

  public new Dictionary<int, string> SttbfRMarkAuthorNames
  {
    get
    {
      if (this.SttbfRMark != null && this.m_sttbFRAuthorNames == null)
        this.m_sttbFRAuthorNames = this.GetSTTBFRNames(this.SttbfRMark);
      return this.m_sttbFRAuthorNames;
    }
  }

  public IWordSubdocumentReader GetSubdocumentReader(WordSubdocument subDocumentType)
  {
    if (!this.m_bHeaderRead)
      throw new InvalidOperationException("Call ReadDocumentHeader() before this method");
    switch (subDocumentType)
    {
      case WordSubdocument.Footnote:
        return this.m_lastReader = (IWordSubdocumentReader) new WordFootnoteReader(this);
      case WordSubdocument.HeaderFooter:
        return this.m_lastReader = (IWordSubdocumentReader) new WordHeaderFooterReader(this);
      case WordSubdocument.Endnote:
        return this.m_lastReader = (IWordSubdocumentReader) new WordEndnoteReader(this);
      case WordSubdocument.Annotation:
        return this.m_lastReader = (IWordSubdocumentReader) new WordAnnotationReader(this);
      case WordSubdocument.TextBox:
        return this.m_lastReader = (IWordSubdocumentReader) new WordTextBoxReader(this);
      case WordSubdocument.HeaderTextBox:
        return this.m_lastReader = (IWordSubdocumentReader) new WordHFTextBoxReader(this);
      default:
        return (IWordSubdocumentReader) null;
    }
  }

  public void ReadDocumentHeader(WordDocument doc)
  {
    this.m_bHeaderRead = !this.m_bHeaderRead ? true : throw new InvalidOperationException("Method ReadDocumentHeader() already called!");
    this.m_docInfo.Fib.Read((Stream) this.m_streamsManager.MainStream);
    if (this.m_docInfo.Fib.FComplex && !doc.Settings.SkipIncrementalSaveValidation)
      throw new NotImplementedException("Complex format is not supported");
    this.m_streamsManager.LoadTableStream(this.m_docInfo.Fib.FWhichTblStm ? "1Table" : "0Table");
    if (this.m_docInfo.Fib.FEncrypted)
    {
      if (this.NeedPassword == null)
        throw new ArgumentException("Document is encrypted, password is needed to open the document");
      string password = this.NeedPassword();
      WordDecryptor wordDecryptor = new WordDecryptor(this.m_streamsManager.TableStream, this.m_streamsManager.MainStream, this.m_streamsManager.DataStream, this.m_docInfo.Fib);
      if (!wordDecryptor.CheckPassword(password))
        throw new Exception($"Specified password \"{password}\" is incorrect!");
      wordDecryptor.Decrypt();
      this.m_streamsManager.UpdateStreams(wordDecryptor.MainStream, wordDecryptor.TableStream, wordDecryptor.DataStream);
      this.m_docInfo.Fib.ReadAfterDecryption((Stream) this.m_streamsManager.MainStream);
    }
    doc.Password = (string) null;
    this.m_docInfo.TablesData.Read((Stream) this.m_streamsManager.TableStream);
    this.UpdateBookmarks();
    this.UpdateStyleSheet();
    this.m_docInfo.FkpData.Read(this.m_streamsManager.MainStream);
    this.UpdateCharacterProperties();
    this.UpdateParagraphProperties();
    this.UpdateSectionProperties();
    this.ReadSummaryManaged();
    if (this.m_docInfo.Fib.FibRgFcLcb97LcbDggInfo != 0U)
      this.Escher = new EscherClass((Stream) this.m_streamsManager.TableStream, (Stream) this.m_streamsManager.MainStream, (long) this.m_docInfo.Fib.FibRgFcLcb97FcDggInfo, (long) this.m_docInfo.Fib.FibRgFcLcb97LcbDggInfo, doc);
    this.m_statePositions.InitStartEndPos();
    this.m_streamsManager.MainStream.Position = (long) this.m_statePositions.StartText;
  }

  private void ReadSummaryManaged()
  {
    this.m_streamsManager.LoadSummaryInfoStream();
    if (this.m_streamsManager.SummaryInfoStream != null && this.m_streamsManager.SummaryInfoStream.Length > 0L)
      this.ReadDocumentProperties(new DocumentPropertyCollection((Stream) this.m_streamsManager.SummaryInfoStream));
    this.m_streamsManager.LoadDocumentSummaryInfoStream();
    if (this.m_streamsManager.DocumentSummaryInfoStream == null || this.m_streamsManager.DocumentSummaryInfoStream.Length <= 0L)
      return;
    this.ReadDocumentProperties(new DocumentPropertyCollection((Stream) this.m_streamsManager.DocumentSummaryInfoStream));
  }

  private void ReadDocumentProperties(DocumentPropertyCollection properties)
  {
    Guid guid1 = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
    Guid guid2 = new Guid("D5CDD502-2E9C-101B-9397-08002B2CF9AE");
    Guid guid3 = new Guid("D5CDD505-2E9C-101B-9397-08002B2CF9AE");
    List<PropertySection> sections = properties.Sections;
    int index = 0;
    for (int count = sections.Count; index < count; ++index)
    {
      PropertySection section = sections[index];
      if (section.Id == guid1)
        this.ReadProperties(section, (IDictionary) this.m_builtinProp.SummaryHash, true, true);
      else if (section.Id == guid2)
        this.ReadProperties(section, (IDictionary) this.m_builtinProp.DocumentHash, false, true);
      else if (section.Id == guid3)
        this.ReadProperties(section, (IDictionary) this.m_custProp.CustomHash, true, false);
    }
  }

  private void ReadProperties(
    PropertySection section,
    IDictionary dicProperties,
    bool bSummary,
    bool bBuiltIn)
  {
    Dictionary<int, DocumentProperty> dictionary = (Dictionary<int, DocumentProperty>) null;
    if (!bBuiltIn)
      dictionary = new Dictionary<int, DocumentProperty>();
    List<PropertyData> properties = section.Properties;
    int index = 0;
    for (int count = properties.Count; index < count; ++index)
    {
      PropertyData variant = properties[index];
      if (bSummary || !bBuiltIn || variant.IsValidProperty())
      {
        if (variant.IsLinkToSource)
        {
          int parentId = variant.ParentId;
          dictionary[parentId].SetLinkSource((IPropertyData) variant);
        }
        else if (!(variant.Data is ClipboardData))
        {
          DocumentProperty documentProperty = new DocumentProperty((IPropertyData) variant, bSummary);
          object key = bBuiltIn ? (object) (int) documentProperty.PropertyId : (object) documentProperty.Name;
          if (!bBuiltIn)
            dictionary.Add(variant.Id, documentProperty);
          if (documentProperty.Value != null)
          {
            if (!bSummary && bBuiltIn && !dicProperties.Contains(key))
              dicProperties.Add(key, (object) documentProperty);
            else
              dicProperties[key] = (object) documentProperty;
          }
        }
      }
    }
  }

  public override WordChunkType ReadChunk()
  {
    if (!this.m_bHeaderRead)
      throw new InvalidOperationException("Call ReadDocumentHeader() before this method");
    return base.ReadChunk();
  }

  public void ReadDocumentEnd() => this.m_streamsManager.CloseStg();

  public override FieldDescriptor GetFld()
  {
    return this.m_docInfo.TablesData.Fields.FindFld(this.m_type, this.CalcCP(this.StatePositions.StartText, 1));
  }

  internal override void Close()
  {
    base.Close();
    this.m_secProperties = (SectionProperties) null;
    if (this.m_builtinProp != null)
      this.m_builtinProp = (BuiltinDocumentProperties) null;
    if (this.m_custProp != null)
      this.m_custProp = (CustomDocumentProperties) null;
    this.m_strEncoding = (Encoding) null;
  }

  public override FileShapeAddress GetFSPA()
  {
    int CP = this.CalcCP(this.StatePositions.StartText, 1);
    return this.m_docInfo.TablesData.FileArtObjects == null ? (FileShapeAddress) null : this.m_docInfo.TablesData.FileArtObjects.FindFileShape(this.m_type, CP);
  }

  public override void FreezeStreamPos()
  {
    if (this.m_lastReader != null)
      (this.m_lastReader as WordReaderBase).FreezeStreamPos();
    base.FreezeStreamPos();
  }

  public override void UnfreezeStreamPos()
  {
    if (this.m_lastReader != null)
      (this.m_lastReader as WordReaderBase).FreezeStreamPos();
    base.UnfreezeStreamPos();
  }

  protected override void InitClass()
  {
    this.m_secProperties = new SectionProperties();
    base.InitClass();
    this.m_docInfo = new DocInfo(this.m_streamsManager);
    this.m_statePositions = (StatePositionsBase) new MainStatePositions(this.m_docInfo.FkpData);
    this.m_type = WordSubdocument.Main;
    this.m_startTextPos = 0;
    this.m_endTextPos = 0;
  }

  protected override void UpdateEndPositions(long iEndPos)
  {
    base.UpdateEndPositions(iEndPos);
    if (!this.StatePositions.UpdateSepxEndPos(iEndPos))
      return;
    this.UpdateSectionProperties();
  }

  private void UpdateSectionProperties()
  {
    this.m_secProperties = new SectionProperties(this.StatePositions.CurrentSepx);
  }

  private void UpdateStyleSheet()
  {
    WPTablesData tablesData = this.m_docInfo.TablesData;
    StyleSheetInfoRecord styleSheetInfo = tablesData.StyleSheetInfo;
    FontFamilyNameRecord[] familyNameRecords = tablesData.FFNStringTable.FontFamilyNameRecords;
    string[] names = new string[familyNameRecords.Length];
    int index1 = 0;
    for (int length = names.Length; index1 < length; ++index1)
    {
      names[index1] = familyNameRecords[index1].FontName;
      this.StyleSheet.UpdateFontSubstitutionTable(familyNameRecords[index1]);
    }
    this.StyleSheet.ClearFontNames();
    this.StyleSheet.UpdateFontNames(names);
    StyleDefinitionRecord[] styleDefinitions = tablesData.StyleDefinitions;
    int index2 = 0;
    for (int length = styleDefinitions.Length; index2 < 15 && index2 < length; ++index2)
    {
      StyleDefinitionRecord record = styleDefinitions[index2];
      if (record.CharacterProperty != null && record.CharacterProperty.FontAscii == ushort.MaxValue && record.BaseStyle == (ushort) 4095 /*0x0FFF*/)
      {
        record.CharacterProperty.FontAscii = styleSheetInfo.StandardChpStsh[0];
        record.CharacterProperty.FontFarEast = styleSheetInfo.StandardChpStsh[1];
        record.CharacterProperty.FontNonFarEast = styleSheetInfo.StandardChpStsh[2];
      }
      if (record.StyleName != null)
      {
        WordStyle style = this.StyleSheet.UpdateStyle(index2, record.StyleName);
        style.ID = (int) record.StyleId;
        style.BaseStyleIndex = (int) record.BaseStyle;
        style.NextStyleIndex = (int) record.NextStyleId;
        style.LinkStyleIndex = (int) record.LinkStyleId;
        style.IsCharacterStyle = record.TypeCode == WordStyleType.CharacterStyle;
        style.IsPrimary = record.IsQFormat;
        style.IsSemiHidden = record.IsSemiHidden;
        style.UnhideWhenUsed = record.UnhideWhenUsed;
        style.TypeCode = record.TypeCode;
        if (record.UpxNumber == (ushort) 3 && record.Tapx != null)
        {
          style.TableStyleData = new byte[record.Tapx.Length];
          Buffer.BlockCopy((Array) record.Tapx, 0, (Array) style.TableStyleData, 0, record.Tapx.Length);
        }
        this.UpdateStyleProperties(style, record);
      }
    }
    int stylesCount = this.StyleSheet.StylesCount;
    for (int length = styleDefinitions.Length; stylesCount < length; ++stylesCount)
    {
      StyleDefinitionRecord record = styleDefinitions[stylesCount];
      if (record.StyleName == null)
      {
        this.StyleSheet.AddEmptyStyle();
      }
      else
      {
        WordStyle style = this.StyleSheet.CreateStyle(record.StyleName, false);
        style.ID = (int) record.StyleId;
        style.BaseStyleIndex = (int) record.BaseStyle;
        style.NextStyleIndex = (int) record.NextStyleId;
        style.LinkStyleIndex = (int) record.LinkStyleId;
        style.IsPrimary = record.IsQFormat;
        style.IsSemiHidden = record.IsSemiHidden;
        style.UnhideWhenUsed = record.UnhideWhenUsed;
        style.IsCharacterStyle = record.TypeCode == WordStyleType.CharacterStyle;
        style.TypeCode = record.TypeCode;
        if (record.UpxNumber == (ushort) 3 && record.Tapx != null)
        {
          style.TableStyleData = new byte[record.Tapx.Length];
          Buffer.BlockCopy((Array) record.Tapx, 0, (Array) style.TableStyleData, 0, record.Tapx.Length);
        }
        this.UpdateStyleProperties(style, record);
      }
    }
    tablesData.StyleDefinitions = (StyleDefinitionRecord[]) null;
    tablesData.StyleSheetInfo = (StyleSheetInfoRecord) null;
  }

  private void UpdateStyleProperties(WordStyle style, StyleDefinitionRecord record)
  {
    style.CHPX = record.CharacterProperty;
    style.PAPX = record.ParagraphProperty;
  }

  private MemoryStream CopyStream(Stream entryStream)
  {
    try
    {
      byte[] buffer = new byte[entryStream.Length];
      long position = entryStream.Position;
      entryStream.Position = 0L;
      entryStream.Read(buffer, 0, buffer.Length);
      entryStream.Position = position;
      return new MemoryStream(buffer);
    }
    catch
    {
      throw new ArgumentException("Cannot read data from stream ");
    }
  }

  public void Dispose()
  {
  }
}
