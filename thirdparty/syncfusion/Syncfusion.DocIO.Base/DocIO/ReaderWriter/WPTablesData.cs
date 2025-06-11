// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WPTablesData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WPTablesData
{
  internal const string DEF_TABLESTREAM_NAME = "1Table";
  private const string EXC_NOTREAD_TABLE_MESS = "You can not get \"table\" object without calling the Read() method!";
  internal EscherClass m_escher;
  private Fib m_fib;
  private SectionExceptionsTable m_sectionTable;
  internal PieceTable m_pieceTable;
  private FontFamilyNameStringTable m_ffnStringTable;
  private BookmarkNameStringTable m_bkmkStringTable;
  private BookmarkDescriptor m_bkmkDescriptor;
  private BinaryTable m_binTableCHPX;
  private BinaryTable m_binTablePAPX;
  private ListInfo m_listInfo;
  private CharPosTableRecord m_charPosTableHF;
  private StyleSheetInfoRecord m_styleSheetInfo = new StyleSheetInfoRecord((ushort) 18);
  private StyleDefinitionRecord[] m_arrStyleDefinitions;
  private List<uint> m_pieceTablePositions = new List<uint>();
  internal List<Encoding> m_pieceTableEncodings = new List<Encoding>();
  private DOPDescriptor m_dopDescriptor = new DOPDescriptor();
  private ArtObjectsRW m_artObjects;
  private AnnotationsRW m_anotations;
  private FootnotesRW m_footnotes;
  private EndnotesRW m_endnotes;
  private Fields m_fields;
  private byte[] m_macroCommands;
  private byte[] m_variables;
  private byte[] m_assocStrings;
  private byte[] m_sttbfRMark;
  private SinglePropertyModifierArray m_clxModifiers;
  private List<int> m_secPositions = new List<int>();
  private List<int> m_sepxPositions = new List<int>();
  private List<uint> m_papPositions = new List<uint>();
  private List<int> m_papxPositions = new List<int>();
  private List<uint> m_chpPositions = new List<uint>();
  private List<int> m_chpxPositions = new List<int>();
  private int[] m_headerPositions;
  private string m_standardAsciiFont;
  private string m_standardFarEastFont;
  private string m_standardNonFarEastFont;
  private string m_standardBidiFont;
  private GrammarSpelling m_grammarSpellingTablesData;

  internal WPTablesData(Fib fib) => this.m_fib = fib;

  internal ArtObjectsRW ArtObj
  {
    get
    {
      if (this.m_artObjects == null)
        this.m_artObjects = new ArtObjectsRW();
      return this.m_artObjects;
    }
  }

  internal AnnotationsRW Annotations
  {
    get
    {
      if (this.m_anotations == null)
        this.m_anotations = new AnnotationsRW();
      return this.m_anotations;
    }
  }

  internal FootnotesRW Footnotes
  {
    get
    {
      if (this.m_footnotes == null)
        this.m_footnotes = new FootnotesRW();
      return this.m_footnotes;
    }
  }

  internal EndnotesRW Endnotes
  {
    get
    {
      if (this.m_endnotes == null)
        this.m_endnotes = new EndnotesRW();
      return this.m_endnotes;
    }
  }

  internal SectionExceptionsTable SectionsTable
  {
    get
    {
      return this.m_sectionTable != null ? this.m_sectionTable : throw new InvalidOperationException("You can not get \"table\" object without calling the Read() method!");
    }
  }

  internal FontFamilyNameStringTable FFNStringTable
  {
    get
    {
      return this.m_ffnStringTable != null ? this.m_ffnStringTable : throw new InvalidOperationException("You can not get \"table\" object without calling the Read() method!");
    }
    set => this.m_ffnStringTable = value;
  }

  internal StyleSheetInfoRecord StyleSheetInfo
  {
    get => this.m_styleSheetInfo;
    set => this.m_styleSheetInfo = value;
  }

  internal StyleDefinitionRecord[] StyleDefinitions
  {
    get => this.m_arrStyleDefinitions;
    set => this.m_arrStyleDefinitions = value;
  }

  internal CharPosTableRecord HeaderFooterCharPosTable => this.m_charPosTableHF;

  internal BinaryTable CHPXBinaryTable => this.m_binTableCHPX;

  internal BinaryTable PAPXBinaryTable => this.m_binTablePAPX;

  internal int[] HeaderPositions
  {
    get => this.m_headerPositions;
    set => this.m_headerPositions = value;
  }

  internal int SectionCount => this.m_secPositions.Count + 1;

  internal BookmarkNameStringTable BookmarkStrings
  {
    get
    {
      if (this.m_bkmkStringTable == null)
        this.m_bkmkStringTable = new BookmarkNameStringTable();
      return this.m_bkmkStringTable;
    }
  }

  internal BookmarkDescriptor BookmarkDescriptor
  {
    get
    {
      if (this.m_bkmkDescriptor == null)
        this.m_bkmkDescriptor = new BookmarkDescriptor();
      return this.m_bkmkDescriptor;
    }
  }

  internal DOPDescriptor DOP
  {
    get => this.m_dopDescriptor;
    set => this.m_dopDescriptor = value;
  }

  internal ListInfo ListInfo
  {
    get
    {
      if (this.m_listInfo == null)
        this.m_listInfo = new ListInfo();
      return this.m_listInfo;
    }
    set => this.m_listInfo = value;
  }

  internal List<uint> PieceTablePositions => this.m_pieceTablePositions;

  internal ArtObjectsRW FileArtObjects => this.m_artObjects;

  internal EscherClass Escher
  {
    get
    {
      if (this.m_escher == null)
        this.m_escher = new EscherClass((WordDocument) null);
      return this.m_escher;
    }
    set => this.m_escher = value;
  }

  internal Fields Fields
  {
    get
    {
      if (this.m_fields == null)
        this.m_fields = new Fields();
      return this.m_fields;
    }
    set => this.m_fields = value;
  }

  internal byte[] MacroCommands
  {
    get => this.m_macroCommands;
    set => this.m_macroCommands = value;
  }

  internal Fib Fib => this.m_fib;

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

  internal GrammarSpelling GrammarSpellingData
  {
    get => this.m_grammarSpellingTablesData;
    set => this.m_grammarSpellingTablesData = value;
  }

  internal byte[] Variables
  {
    get => this.m_variables;
    set => this.m_variables = value;
  }

  internal byte[] AsociatedStrings
  {
    get => this.m_assocStrings;
    set => this.m_assocStrings = value;
  }

  internal byte[] SttbfRMark
  {
    get => this.m_sttbfRMark;
    set => this.m_sttbfRMark = value;
  }

  internal void Read(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    Fib fib = this.m_fib;
    stream.Position = (long) fib.FibRgFcLcb97FcPlcfSed;
    this.m_sectionTable = new SectionExceptionsTable(stream, (int) fib.FibRgFcLcb97LcbPlcfSed);
    this.ReadFFNTable(stream, fib);
    this.ReadBookmarks(fib, stream);
    stream.Position = (long) fib.FibRgFcLcb97FcPlcftxbxTxt;
    this.ReadFields(fib, stream);
    stream.Position = (long) fib.FibRgFcLcb97FcPlcfBteChpx;
    this.m_binTableCHPX = new BinaryTable(stream, (int) fib.FibRgFcLcb97LcbPlcfBteChpx);
    stream.Position = (long) fib.FibRgFcLcb97FcPlcfBtePapx;
    this.m_binTablePAPX = new BinaryTable(stream, (int) fib.FibRgFcLcb97LcbPlcfBtePapx);
    this.ReadStyleSheet(stream);
    if (fib.FibRgFcLcb97LcbPlcfHdd > 0U)
    {
      stream.Position = (long) fib.FibRgFcLcb97FcPlcfHdd;
      this.m_charPosTableHF = new CharPosTableRecord(stream, (int) fib.FibRgFcLcb97LcbPlcfHdd);
    }
    this.ReadLists(fib, stream);
    this.ReadDocumentProperties(fib, stream);
    this.ReadComplexPart(stream);
    this.ParsePieceTableEncodings();
    this.ReadFootnotes(fib, stream);
    this.ReadAnnotations(fib, stream);
    this.ReadEndnotes(fib, stream);
    this.ReadArtObjects(fib, stream);
    this.ReadMacroCommands(fib, stream);
    this.ReadGrammarSpellingData(fib, stream);
    this.ReadVariables(fib, stream);
    this.ReadAssocStrings(fib, stream);
    this.ReadSttbfRMark(fib, stream);
  }

  internal void Write(Stream stream, bool hasSubDocument)
  {
    Fib fib = this.m_fib;
    this.GenerateTables(hasSubDocument);
    this.WriteStyleSheet(stream);
    if (hasSubDocument)
      this.WriteFootnotes(fib, stream);
    if (hasSubDocument)
      this.WriteAnnotations(fib, stream);
    uint position1 = (uint) stream.Position;
    uint num1 = (uint) this.m_sectionTable.Save(stream);
    if (num1 > 0U)
    {
      fib.FibRgFcLcb97FcPlcfSed = position1;
      fib.FibRgFcLcb97LcbPlcfSed = num1;
    }
    if (hasSubDocument && this.m_charPosTableHF != null && this.m_charPosTableHF.Positions != null)
    {
      uint position2 = (uint) stream.Position;
      uint num2 = (uint) this.m_charPosTableHF.Save(stream);
      if (num2 > 0U)
      {
        fib.FibRgFcLcb97FcPlcfHdd = position2;
        fib.FibRgFcLcb97LcbPlcfHdd = num2;
      }
    }
    uint position3 = (uint) stream.Position;
    uint num3 = (uint) this.m_binTableCHPX.Save(stream);
    if (num3 > 0U)
    {
      fib.FibRgFcLcb97FcPlcfBteChpx = position3;
      fib.FibRgFcLcb97LcbPlcfBteChpx = num3;
    }
    uint position4 = (uint) stream.Position;
    uint num4 = (uint) this.m_binTablePAPX.Save(stream);
    if (num4 > 0U)
    {
      fib.FibRgFcLcb97FcPlcfBtePapx = position4;
      fib.FibRgFcLcb97LcbPlcfBtePapx = num4;
    }
    uint position5 = (uint) stream.Position;
    this.WriteFFNTable(stream);
    uint position6 = (uint) stream.Position;
    if (position6 > position5)
    {
      fib.FibRgFcLcb97FcSttbfFfn = position5;
      fib.FibRgFcLcb97LcbSttbfFfn = position6 - position5;
    }
    uint position7 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.Main);
    uint position8 = (uint) stream.Position;
    if (position8 > position7)
    {
      fib.FibRgFcLcb97FcPlcfFldMom = position7;
      fib.FibRgFcLcb97LcbPlcfFldMom = position8 - position7;
    }
    uint position9 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.HeaderFooter);
    uint position10 = (uint) stream.Position;
    if (position10 > position9)
    {
      fib.FibRgFcLcb97FcPlcfFldHdr = position9;
      fib.FibRgFcLcb97LcbPlcfFldHdr = position10 - position9;
    }
    uint position11 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.Footnote);
    uint position12 = (uint) stream.Position;
    if (position12 > position11)
    {
      fib.FibRgFcLcb97FcPlcfFldFtn = position11;
      fib.FibRgFcLcb97LcbPlcfFldFtn = position12 - position11;
    }
    uint position13 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.Annotation);
    uint position14 = (uint) stream.Position;
    if (position14 > position13)
    {
      fib.FibRgFcLcb97FcPlcfFldAtn = position13;
      fib.FibRgFcLcb97LcbPlcfFldAtn = position14 - position13;
    }
    this.WriteBookmarks(stream, fib);
    this.WriteMacroCommands(fib, stream);
    this.WriteDocumentProperties(stream, fib);
    this.WriteAssocStrings(fib, stream);
    this.WriteSttbfRMark(fib, stream);
    fib.FibRgFcLcb97FcClx = (uint) stream.Position;
    this.m_pieceTable.Save(stream);
    fib.FibRgFcLcb97LcbClx = (uint) this.m_pieceTable.Length;
    if (hasSubDocument)
      this.WriteEndnotes(fib, stream);
    uint position15 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.Endnote);
    uint position16 = (uint) stream.Position;
    if (position16 > position15)
    {
      fib.FibRgFcLcb97FcPlcfFldEdn = position15;
      fib.FibRgFcLcb97LcbPlcfFldEdn = position16 - position15;
    }
    this.WriteArtObjects(stream, fib);
    if (this.m_grammarSpellingTablesData != null && this.m_grammarSpellingTablesData.PlcfsplData != null)
    {
      uint position17 = (uint) stream.Position;
      uint length = (uint) this.m_grammarSpellingTablesData.PlcfsplData.Length;
      if (length > 0U)
      {
        fib.FibRgFcLcb97FcPlcfSpl = position17;
        stream.Write(this.m_grammarSpellingTablesData.PlcfsplData, 0, (int) length);
        fib.FibRgFcLcb97LcbPlcfSpl = length;
      }
    }
    uint position18 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.TextBox);
    uint position19 = (uint) stream.Position;
    if (position19 > position18)
    {
      fib.FibRgFcLcb97FcPlcfFldTxbx = position18;
      fib.FibRgFcLcb97LcbPlcfFldTxbx = position19 - position18;
    }
    uint position20 = (uint) stream.Position;
    this.WriteFields(stream, WordSubdocument.HeaderTextBox);
    uint position21 = (uint) stream.Position;
    if (position21 > position20)
    {
      fib.FibRgFcLcb97FcPlcffldHdrTxbx = position20;
      fib.FibRgFcLcb97LcbPlcffldHdrTxbx = position21 - position20;
    }
    this.WriteVariables(fib, stream);
    if (this.m_listInfo != null)
    {
      uint position22 = (uint) stream.Position;
      uint num5 = (uint) this.m_listInfo.WriteLst(stream);
      if (num5 > 0U)
      {
        this.m_fib.FibRgFcLcb97FcPlfLst = position22;
        this.m_fib.FibRgFcLcb97LcbPlfLst = num5;
      }
      uint position23 = (uint) stream.Position;
      uint num6 = (uint) this.m_listInfo.WriteLfo(stream);
      if (num6 > 0U)
      {
        this.m_fib.FibRgFcLcb97FcPlfLfo = position23;
        this.m_fib.FibRgFcLcb97LcbPlfLfo = num6;
      }
    }
    if (this.m_grammarSpellingTablesData != null && this.m_grammarSpellingTablesData.PlcfgramData != null)
    {
      uint position24 = (uint) stream.Position;
      uint length = (uint) this.m_grammarSpellingTablesData.PlcfgramData.Length;
      if (length > 0U)
      {
        fib.FibRgFcLcb97FcPlcfGram = position24;
        stream.Write(this.m_grammarSpellingTablesData.PlcfgramData, 0, (int) length);
        fib.FibRgFcLcb97LcbPlcfGram = length;
      }
    }
    if (this.m_listInfo != null)
    {
      uint position25 = (uint) stream.Position;
      uint num7 = (uint) (2 + this.m_listInfo.WriteStringTable(stream));
      if (num7 > 0U)
      {
        this.m_fib.FibRgFcLcb97FcSttbListNames = position25;
        this.m_fib.FibRgFcLcb97LcbSttbListNames = num7;
      }
    }
    uint position26 = (uint) stream.Position;
    this.WriteRmdThreading(stream);
    uint position27 = (uint) stream.Position;
    if (position27 <= position26)
      return;
    fib.FibRgFcLcb2000FcRmdThreading = position26;
    fib.FibRgFcLcb2000LcbRmdThreading = position27 - position26;
  }

  internal uint ConvertCharPosToFileCharPos(uint charPos)
  {
    uint fileCharPos = uint.MaxValue;
    PieceTable pieceTable = this.m_pieceTable;
    int index1 = 0;
    for (int entriesCount = pieceTable.EntriesCount; index1 < entriesCount; ++index1)
    {
      if (charPos >= pieceTable.FileCharacterPos[index1] && charPos < pieceTable.FileCharacterPos[index1 + 1])
      {
        bool bIsUnicode;
        uint num = this.NormalizeFileCharPos(pieceTable.Entries[index1].FileOffset, out bIsUnicode);
        if (!bIsUnicode)
        {
          fileCharPos = num + (charPos - pieceTable.FileCharacterPos[index1]);
          this.m_fib.Encoding = Encoding.ASCII;
          break;
        }
        fileCharPos = num + (uint) (((int) charPos - (int) pieceTable.FileCharacterPos[index1]) * 2);
        this.m_fib.Encoding = Encoding.Unicode;
        break;
      }
    }
    if (fileCharPos == uint.MaxValue)
    {
      int index2 = pieceTable.EntriesCount - 1;
      bool bIsUnicode;
      uint num = this.NormalizeFileCharPos(pieceTable.Entries[index2].FileOffset, out bIsUnicode);
      if (!bIsUnicode)
      {
        fileCharPos = num + (charPos - pieceTable.FileCharacterPos[index2]);
        this.m_fib.Encoding = Encoding.ASCII;
      }
      else
      {
        fileCharPos = num + (uint) (((int) charPos - (int) pieceTable.FileCharacterPos[index2]) * 2);
        this.m_fib.Encoding = Encoding.Unicode;
      }
    }
    return fileCharPos;
  }

  internal BookmarkInfo[] GetBookmarks()
  {
    if (this.m_bkmkStringTable == null || this.m_bkmkStringTable.BookmarkCount == 0)
      return (BookmarkInfo[]) null;
    int bookmarkCount = this.m_bkmkStringTable.BookmarkCount;
    BookmarkInfo[] bookmarks = new BookmarkInfo[bookmarkCount];
    for (int index = 0; index < bookmarkCount; ++index)
    {
      bookmarks[index] = new BookmarkInfo(this.m_bkmkStringTable[index], this.m_bkmkDescriptor.GetBeginPos(index), this.m_bkmkDescriptor.GetEndPos(index), this.m_bkmkDescriptor.IsCellGroup(index), this.m_bkmkDescriptor.GetStartCellIndex(index), this.m_bkmkDescriptor.GetEndCellIndex(index));
      bookmarks[index].Index = index;
    }
    return bookmarks;
  }

  internal void AddSectionRecord(int charPos, int sepxPos)
  {
    this.m_secPositions.Add(charPos);
    this.m_sepxPositions.Add(sepxPos);
  }

  internal void AddPapxRecord(uint charPos, int papxPos)
  {
    this.m_papPositions.Add(charPos);
    this.m_papxPositions.Add(papxPos);
  }

  internal void AddChpxRecord(uint pos, int chpxPos)
  {
    this.m_chpPositions.Add(pos);
    this.m_chpxPositions.Add(chpxPos);
  }

  internal void AddStyleSheetTable(WordStyleSheet styleSheet)
  {
    int stylesCount = styleSheet.StylesCount;
    this.StyleSheetInfo.StylesCount = (ushort) stylesCount;
    StyleDefinitionRecord[] definitionRecordArray = new StyleDefinitionRecord[stylesCount];
    ushort num = 4094;
    for (int index = 0; index < stylesCount; ++index)
    {
      WordStyle styleByIndex = styleSheet.GetStyleByIndex(index);
      StyleDefinitionRecord definitionRecord = (StyleDefinitionRecord) null;
      if (styleByIndex != WordStyle.Empty)
      {
        ushort styleId = styleByIndex.ID > -1 ? (ushort) styleByIndex.ID : num;
        definitionRecord = new StyleDefinitionRecord(styleByIndex.Name, styleId, this.StyleSheetInfo);
        if (styleId > (ushort) 0 && styleId < (ushort) 10)
        {
          definitionRecord.IsQFormat = true;
          definitionRecord.UnhideWhenUsed = true;
        }
        else if (styleByIndex.IsPrimary)
          definitionRecord.IsQFormat = true;
        definitionRecord.BaseStyle = (ushort) styleByIndex.BaseStyleIndex;
        definitionRecord.NextStyleId = (ushort) styleByIndex.NextStyleIndex;
        definitionRecord.HasUpe = styleByIndex.HasUpe;
        definitionRecord.IsSemiHidden = styleByIndex.IsSemiHidden;
        definitionRecord.UnhideWhenUsed = styleByIndex.UnhideWhenUsed;
        definitionRecord.TypeCode = styleByIndex.TypeCode;
        if (styleByIndex.LinkStyleIndex >= 0 && styleByIndex.LinkStyleIndex < stylesCount)
          definitionRecord.LinkStyleId = (ushort) styleByIndex.LinkStyleIndex;
        if (styleByIndex.IsCharacterStyle)
        {
          definitionRecord.TypeCode = WordStyleType.CharacterStyle;
          definitionRecord.CharacterProperty = styleByIndex.CHPX;
          definitionRecord.ParagraphProperty = (ParagraphPropertyException) null;
        }
        else if (definitionRecord.TypeCode == WordStyleType.TableStyle && styleByIndex.TableStyleData != null)
        {
          definitionRecord.UpxNumber = (ushort) 3;
          definitionRecord.Tapx = new byte[styleByIndex.TableStyleData.Length];
          Buffer.BlockCopy((Array) styleByIndex.TableStyleData, 0, (Array) definitionRecord.Tapx, 0, styleByIndex.TableStyleData.Length);
        }
        else
        {
          definitionRecord.TypeCode = WordStyleType.ParagraphStyle;
          definitionRecord.ParagraphProperty = styleByIndex.PAPX;
          definitionRecord.CharacterProperty = styleByIndex.CHPX;
        }
      }
      definitionRecordArray[index] = definitionRecord;
    }
    this.StyleDefinitions = definitionRecordArray;
    ushort[] numArray = new ushort[3];
    if (this.m_standardAsciiFont != null)
      numArray[0] = (ushort) styleSheet.FontNameToIndex(this.m_standardAsciiFont);
    if (this.m_standardFarEastFont != null)
      numArray[1] = (ushort) styleSheet.FontNameToIndex(this.m_standardFarEastFont);
    if (this.m_standardNonFarEastFont != null)
      numArray[2] = (ushort) styleSheet.FontNameToIndex(this.m_standardNonFarEastFont);
    if (this.m_standardBidiFont != null)
      this.StyleSheetInfo.FtcBi = (ushort) styleSheet.FontNameToIndex(this.m_standardBidiFont);
    this.StyleSheetInfo.StandardChpStsh = numArray;
    this.AddFFNTable(styleSheet);
  }

  internal void Close()
  {
    if (this.m_escher != null)
      this.m_escher = (EscherClass) null;
    if (this.m_fib != null)
    {
      this.m_fib.Encoding = (Encoding) null;
      this.m_fib = (Fib) null;
    }
    if (this.m_sectionTable != null)
      this.m_sectionTable = (SectionExceptionsTable) null;
    if (this.m_pieceTable != null)
      this.m_pieceTable = (PieceTable) null;
    if (this.m_ffnStringTable != null)
      this.m_ffnStringTable = (FontFamilyNameStringTable) null;
    if (this.m_bkmkStringTable != null)
      this.m_bkmkStringTable = (BookmarkNameStringTable) null;
    if (this.m_bkmkDescriptor != null)
      this.m_bkmkDescriptor = (BookmarkDescriptor) null;
    if (this.m_binTableCHPX != null)
      this.m_binTableCHPX = (BinaryTable) null;
    if (this.m_binTablePAPX != null)
      this.m_binTablePAPX = (BinaryTable) null;
    if (this.m_listInfo != null)
      this.m_listInfo = (ListInfo) null;
    if (this.m_charPosTableHF != null)
      this.m_charPosTableHF = (CharPosTableRecord) null;
    if (this.m_styleSheetInfo != null)
    {
      this.m_styleSheetInfo.Close();
      this.m_styleSheetInfo = (StyleSheetInfoRecord) null;
    }
    if (this.m_arrStyleDefinitions != null)
    {
      foreach (StyleDefinitionRecord arrStyleDefinition in this.m_arrStyleDefinitions)
        arrStyleDefinition?.Close();
      this.m_arrStyleDefinitions = (StyleDefinitionRecord[]) null;
    }
    if (this.m_pieceTablePositions != null)
    {
      this.m_pieceTablePositions.Clear();
      this.m_pieceTablePositions = (List<uint>) null;
    }
    if (this.m_pieceTableEncodings != null)
    {
      this.m_pieceTableEncodings.Clear();
      this.m_pieceTableEncodings = (List<Encoding>) null;
    }
    this.m_dopDescriptor = (DOPDescriptor) null;
    if (this.m_artObjects != null)
    {
      this.m_artObjects.Close();
      this.m_artObjects = (ArtObjectsRW) null;
    }
    if (this.m_anotations != null)
    {
      this.m_anotations.Close();
      this.m_anotations = (AnnotationsRW) null;
    }
    if (this.m_footnotes != null)
    {
      this.m_footnotes.Close();
      this.m_footnotes = (FootnotesRW) null;
    }
    if (this.m_endnotes != null)
    {
      this.m_endnotes.Close();
      this.m_endnotes = (EndnotesRW) null;
    }
    if (this.m_fields != null)
    {
      this.m_fields.Close();
      this.m_fields = (Fields) null;
    }
    this.m_macroCommands = (byte[]) null;
    this.m_variables = (byte[]) null;
    this.m_assocStrings = (byte[]) null;
    if (this.m_clxModifiers != null)
    {
      this.m_clxModifiers.Close();
      this.m_clxModifiers = (SinglePropertyModifierArray) null;
    }
    if (this.m_secPositions != null)
    {
      this.m_secPositions.Clear();
      this.m_secPositions = (List<int>) null;
    }
    if (this.m_sepxPositions != null)
    {
      this.m_sepxPositions.Clear();
      this.m_sepxPositions = (List<int>) null;
    }
    if (this.m_papPositions != null)
    {
      this.m_papPositions.Clear();
      this.m_papPositions = (List<uint>) null;
    }
    if (this.m_papxPositions != null)
    {
      this.m_papxPositions.Clear();
      this.m_papxPositions = (List<int>) null;
    }
    if (this.m_chpPositions != null)
    {
      this.m_chpPositions.Clear();
      this.m_chpPositions = (List<uint>) null;
    }
    if (this.m_chpxPositions != null)
    {
      this.m_chpxPositions.Clear();
      this.m_chpxPositions = (List<int>) null;
    }
    this.m_headerPositions = (int[]) null;
    if (this.m_grammarSpellingTablesData == null)
      return;
    this.m_grammarSpellingTablesData = (GrammarSpelling) null;
  }

  private void ReadFFNTable(Stream stream, Fib fib)
  {
    this.m_ffnStringTable = new FontFamilyNameStringTable();
    stream.Position = (long) fib.FibRgFcLcb97FcSttbfFfn;
    this.m_ffnStringTable.Parse(stream, (int) fib.FibRgFcLcb97LcbSttbfFfn);
  }

  internal Encoding GetEncodingByFC(long position)
  {
    for (int index = 0; index < this.m_pieceTable.EntriesCount; ++index)
    {
      if ((long) this.m_pieceTablePositions[index] <= position && position <= (long) this.m_pieceTablePositions[index + 1])
        return this.m_pieceTableEncodings[index];
    }
    return this.m_fib.Encoding;
  }

  internal uint ConvertFCToCP(uint fc)
  {
    uint cp = 0;
    int index1 = 0;
    for (int index2 = this.m_pieceTablePositions.Count - 1; index1 < index2; ++index1)
    {
      if (fc >= this.m_pieceTablePositions[index1] && fc <= this.m_pieceTablePositions[index1 + 1])
      {
        uint num1 = this.m_pieceTableEncodings[index1] == Encoding.Unicode ? 2U : 1U;
        if ((int) fc == (int) this.m_pieceTablePositions[index1 + 1])
          ++index1;
        uint num2 = (fc - this.m_pieceTablePositions[index1]) / num1;
        cp = this.m_pieceTable.FileCharacterPos[index1] + num2;
        break;
      }
    }
    return cp;
  }

  internal bool HasSubdocument(WordSubdocument wsType)
  {
    switch (wsType)
    {
      case WordSubdocument.Footnote:
        return this.m_footnotes != null && this.m_footnotes.Count > 0;
      case WordSubdocument.Endnote:
        return this.m_endnotes != null && this.m_endnotes.Count > 0;
      case WordSubdocument.Annotation:
        return this.m_anotations != null && this.m_anotations.Count > 0;
      case WordSubdocument.TextBox:
        return this.m_artObjects != null && this.m_artObjects.MainDocTxBxs != null && this.m_artObjects.MainDocTxBxs.Count > 0;
      case WordSubdocument.HeaderTextBox:
        return this.m_artObjects != null && this.m_artObjects.HfDocTxBxs != null && this.m_artObjects.HfDocTxBxs.Count > 0;
      default:
        return true;
    }
  }

  internal bool HasList() => this.m_listInfo != null;

  private void AddFFNTable(WordStyleSheet styleSheet)
  {
    FontFamilyNameStringTable familyNameStringTable = new FontFamilyNameStringTable();
    familyNameStringTable.RecordsCount = styleSheet.FontNamesList.Count;
    int index1 = 0;
    for (int length = familyNameStringTable.FontFamilyNameRecords.Length; index1 < length; ++index1)
    {
      bool flag = true;
      if (this.m_ffnStringTable != null)
      {
        for (int index2 = 0; index2 < this.FFNStringTable.RecordsCount; ++index2)
        {
          if (this.FFNStringTable.FontFamilyNameRecords[index2].FontName == styleSheet.FontNamesList[index1])
          {
            familyNameStringTable.FontFamilyNameRecords[index1] = this.FFNStringTable.FontFamilyNameRecords[index2];
            flag = false;
            break;
          }
        }
      }
      if (flag)
      {
        FontFamilyNameRecord familyNameRecord = new FontFamilyNameRecord();
        if (styleSheet.FontSubstitutionTable.ContainsKey(styleSheet.FontNamesList[index1]))
          familyNameRecord.AlternativeFontName = styleSheet.FontSubstitutionTable[styleSheet.FontNamesList[index1]];
        familyNameRecord.FontName = styleSheet.FontNamesList[index1];
        familyNameRecord.PitchRequest = (byte) 2;
        familyNameRecord.TrueType = true;
        familyNameRecord.FontFamilyID = (byte) 1;
        familyNameRecord.Weight = (short) 400;
        familyNameStringTable.FontFamilyNameRecords[index1] = familyNameRecord;
      }
    }
    this.FFNStringTable = familyNameStringTable;
  }

  private void WriteFFNTable(Stream stream) => this.m_ffnStringTable.Save(stream);

  private void WriteStyleSheet(Stream stream)
  {
    uint position = (uint) stream.Position;
    ushort length1 = (ushort) this.m_arrStyleDefinitions.Length;
    ushort length2 = (ushort) this.m_styleSheetInfo.Length;
    this.WriteShort(stream, length2);
    this.m_styleSheetInfo.StylesCount = length1;
    this.m_styleSheetInfo.Save(stream);
    int index = 0;
    for (int length3 = this.m_arrStyleDefinitions.Length; index < length3; ++index)
    {
      StyleDefinitionRecord arrStyleDefinition = this.m_arrStyleDefinitions[index];
      if (arrStyleDefinition == null || arrStyleDefinition.StyleName == null)
      {
        stream.Write(new byte[2], 0, 2);
      }
      else
      {
        if (arrStyleDefinition.StyleId == (ushort) 0 || arrStyleDefinition.StyleId == (ushort) 65)
          arrStyleDefinition.BaseStyle = (ushort) 4095 /*0x0FFF*/;
        this.WriteShort(stream, (ushort) arrStyleDefinition.Length);
        arrStyleDefinition.Save(stream);
      }
    }
    this.m_fib.FibRgFcLcb97FcStshfOrig = this.m_fib.FibRgFcLcb97FcStshf = position;
    this.m_fib.FibRgFcLcb97LcbStshfOrig = this.m_fib.FibRgFcLcb97LcbStshf = (uint) ((ulong) stream.Position - (ulong) position);
  }

  private int WriteShort(Stream stream, ushort val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    stream.Write(bytes, 0, bytes.Length);
    return bytes.Length;
  }

  private void ReadStyleSheet(Stream stream)
  {
    long lNextBlockStart = this.ReadStyleSheetTable(stream);
    this.ReadStylesDefinitions(stream, lNextBlockStart);
    if ((int) this.StyleSheetInfo.StandardChpStsh[0] < this.m_ffnStringTable.FontFamilyNameRecords.Length)
      this.m_standardAsciiFont = this.m_ffnStringTable.FontFamilyNameRecords[(int) this.StyleSheetInfo.StandardChpStsh[0]].FontName;
    if ((int) this.StyleSheetInfo.StandardChpStsh[1] < this.m_ffnStringTable.FontFamilyNameRecords.Length)
      this.m_standardFarEastFont = this.m_ffnStringTable.FontFamilyNameRecords[(int) this.StyleSheetInfo.StandardChpStsh[1]].FontName;
    if ((int) this.StyleSheetInfo.StandardChpStsh[2] < this.m_ffnStringTable.FontFamilyNameRecords.Length)
      this.m_standardNonFarEastFont = this.m_ffnStringTable.FontFamilyNameRecords[(int) this.StyleSheetInfo.StandardChpStsh[2]].FontName;
    if ((int) this.StyleSheetInfo.FtcBi < this.m_ffnStringTable.FontFamilyNameRecords.Length)
      this.m_standardBidiFont = this.m_ffnStringTable.FontFamilyNameRecords[(int) this.StyleSheetInfo.FtcBi].FontName;
    if (string.IsNullOrEmpty(this.m_standardAsciiFont))
      this.m_standardAsciiFont = "Times New Roman";
    if (string.IsNullOrEmpty(this.m_standardFarEastFont))
      this.m_standardFarEastFont = "Times New Roman";
    if (string.IsNullOrEmpty(this.m_standardNonFarEastFont))
      this.m_standardNonFarEastFont = "Times New Roman";
    if (!string.IsNullOrEmpty(this.m_standardBidiFont))
      return;
    this.m_standardBidiFont = "Times New Roman";
  }

  private long ReadStyleSheetTable(Stream stream)
  {
    uint rgFcLcb97FcStshf = this.m_fib.FibRgFcLcb97FcStshf;
    uint rgFcLcb97LcbStshf = this.m_fib.FibRgFcLcb97LcbStshf;
    if (rgFcLcb97LcbStshf <= 0U)
      throw new Exception("Length of StyleSheetInfo record can not be less 0!");
    stream.Position = (long) rgFcLcb97FcStshf;
    ushort iCount = BaseWordRecord.ReadUInt16(stream);
    this.m_styleSheetInfo = new StyleSheetInfoRecord(stream, (int) iCount);
    return (long) (rgFcLcb97FcStshf + rgFcLcb97LcbStshf);
  }

  private void ReadStylesDefinitions(Stream stream, long lNextBlockStart)
  {
    int stylesCount = (int) this.m_styleSheetInfo.StylesCount;
    StyleDefinitionRecord[] definitionRecordArray = new StyleDefinitionRecord[stylesCount];
    for (int index = 0; index < stylesCount; ++index)
    {
      ushort iCount = BaseWordRecord.ReadUInt16(stream);
      StyleDefinitionRecord definitionRecord = new StyleDefinitionRecord(stream, (int) iCount, this.m_styleSheetInfo);
      definitionRecordArray[index] = definitionRecord;
    }
    this.m_arrStyleDefinitions = definitionRecordArray;
  }

  private void GenerateTables(bool hasSubDocument)
  {
    this.m_sectionTable = new SectionExceptionsTable();
    this.m_sectionTable.EntriesCount = this.m_secPositions.Count;
    int index1 = 0;
    for (int count = this.m_secPositions.Count; index1 < count; ++index1)
    {
      this.m_sectionTable.Positions[index1] = this.m_secPositions[index1];
      this.m_sectionTable.Descriptors[index1].MacPrintOffset = -1;
      this.m_sectionTable.Descriptors[index1].SepxPosition = (uint) (512 /*0x0200*/ * this.m_sepxPositions[index1]);
    }
    int num = this.m_fib.CcpText + this.m_fib.CcpFtn + this.m_fib.CcpHdd + this.m_fib.CcpAtn + this.m_fib.CcpEdn + this.m_fib.CcpTxbx + this.m_fib.CcpHdrTxbx;
    this.m_sectionTable.Positions[this.m_secPositions.Count] = num;
    this.m_binTableCHPX = new BinaryTable();
    this.m_binTableCHPX.EntriesCount = this.m_chpPositions.Count;
    int index2 = 0;
    for (int count = this.m_chpPositions.Count; index2 < count; ++index2)
    {
      this.m_binTableCHPX.FileCharacterPos[index2] = this.m_chpPositions[index2];
      this.m_binTableCHPX.Entries[index2] = new BinTableEntry()
      {
        Value = this.m_chpxPositions[index2]
      };
    }
    this.m_binTableCHPX.FileCharacterPos[this.m_binTableCHPX.EntriesCount] = this.m_fib.BaseReserved6;
    this.m_binTablePAPX = new BinaryTable();
    this.m_binTablePAPX.EntriesCount = this.m_papPositions.Count;
    int index3 = 0;
    for (int count = this.m_papPositions.Count; index3 < count; ++index3)
    {
      this.m_binTablePAPX.FileCharacterPos[index3] = this.m_papPositions[index3];
      this.m_binTablePAPX.Entries[index3] = new BinTableEntry()
      {
        Value = this.m_papxPositions[index3]
      };
    }
    this.m_binTablePAPX.FileCharacterPos[this.m_binTablePAPX.EntriesCount] = this.m_fib.BaseReserved6;
    this.m_pieceTable = new PieceTable();
    this.m_pieceTable.EntriesCount = 1;
    this.m_pieceTable.FileCharacterPos[0] = 0U;
    this.m_pieceTable.FileCharacterPos[1] = (uint) num;
    this.m_pieceTable.Entries[0].FileOffset = this.m_fib.Encoding == Encoding.Unicode ? 2048U /*0x0800*/ : 1073745920U /*0x40001000*/;
    this.m_pieceTable.Entries[0].fCopied = true;
    if (!hasSubDocument)
      return;
    this.m_charPosTableHF = new CharPosTableRecord();
    this.m_charPosTableHF.Positions = this.m_headerPositions;
  }

  private uint NormalizeFileCharPos(uint fileCharPos, out bool bIsUnicode)
  {
    bIsUnicode = true;
    if (((int) fileCharPos & 1073741824 /*0x40000000*/) != 0)
    {
      fileCharPos &= 3221225471U /*0xBFFFFFFF*/;
      fileCharPos /= 2U;
      bIsUnicode = false;
    }
    return fileCharPos;
  }

  private void ReadComplexPart(Stream stream)
  {
    uint fibRgFcLcb97FcClx = this.m_fib.FibRgFcLcb97FcClx;
    uint fibRgFcLcb97LcbClx = this.m_fib.FibRgFcLcb97LcbClx;
    long num = (long) (fibRgFcLcb97FcClx + fibRgFcLcb97LcbClx);
    stream.Position = (long) fibRgFcLcb97FcClx;
    while (stream.Position < num)
    {
      switch (stream.ReadByte())
      {
        case 1:
          int length = (int) this.ReadUInt16(stream);
          byte[] numArray1 = new byte[length];
          if (stream.Read(numArray1, 0, length) != length)
            throw new Exception("Was unable to read specified number of bytes");
          this.m_clxModifiers = new SinglePropertyModifierArray();
          this.m_clxModifiers.Parse(numArray1, 0, length);
          continue;
        case 2:
          byte[] buffer = new byte[4];
          int count = stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToInt32(buffer, 0) : throw new Exception("Was unable to read bytes from the stream");
          byte[] numArray2 = new byte[count];
          if (count != stream.Read(numArray2, 0, count))
            throw new Exception("Was unable to read bytes from the stream");
          this.m_pieceTable = new PieceTable(numArray2);
          continue;
        default:
          throw new ArgumentOutOfRangeException("Unknown block type.");
      }
    }
  }

  private ushort ReadUInt16(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[2];
    return stream.Read(buffer, 0, 2) == 2 ? BitConverter.ToUInt16(buffer, 0) : throw new Exception("Unable to read enough data from the stream");
  }

  private uint ReadUInt32(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToUInt32(buffer, 0) : throw new Exception("Unable to read enough data from the stream");
  }

  private void ReadLists(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbPlfLst == 0U || fib.FibRgFcLcb97LcbPlfLfo == 0U)
      return;
    this.m_listInfo = new ListInfo(fib, stream);
  }

  private void WriteDocumentProperties(Stream stream, Fib fib)
  {
    fib.FibRgFcLcb97FcDop = (uint) stream.Position;
    fib.FibRgFcLcb97LcbDop = this.m_dopDescriptor.Write(stream);
  }

  private void ReadDocumentProperties(Fib fib, Stream stream)
  {
    stream.Position = (long) fib.FibRgFcLcb97FcDop;
    int fibRgFcLcb97LcbDop = (int) fib.FibRgFcLcb97LcbDop;
    this.m_dopDescriptor = new DOPDescriptor(stream, (int) fib.FibRgFcLcb97FcDop, fibRgFcLcb97LcbDop, fib.FDot);
  }

  private void WriteFields(Stream stream, WordSubdocument subDocument)
  {
    if (this.m_fields == null)
      return;
    uint endPosition = this.m_pieceTable.FileCharacterPos[this.m_pieceTable.EntriesCount] + 1U;
    this.m_fields.Write(stream, endPosition, subDocument);
  }

  private void ReadFields(Fib fib, Stream stream)
  {
    this.m_fields = new Fields(fib, new BinaryReader(stream));
  }

  private void WriteBookmarks(Stream stream, Fib fib)
  {
    if (this.m_bkmkStringTable == null || this.m_bkmkStringTable.BookmarkNamesLength == 0)
      return;
    this.m_bkmkStringTable.Save(stream, fib);
    uint endChar = this.m_pieceTable.FileCharacterPos[this.m_pieceTable.EntriesCount] + 2U;
    this.m_bkmkDescriptor.Save(stream, fib, endChar);
  }

  private void ReadBookmarks(Fib fib, Stream stream)
  {
    stream.Position = (long) fib.FibRgFcLcb97FcSttbfBkmk;
    int lcb97LcbSttbfBkmk = (int) fib.FibRgFcLcb97LcbSttbfBkmk;
    int fcLcb97FcPlcfBkf = (int) fib.FibRgFcLcb97FcPlcfBkf;
    int fcLcb97LcbPlcfBkf = (int) fib.FibRgFcLcb97LcbPlcfBkf;
    int fcLcb97FcPlcfBkl = (int) fib.FibRgFcLcb97FcPlcfBkl;
    int fcLcb97LcbPlcfBkl = (int) fib.FibRgFcLcb97LcbPlcfBkl;
    if (lcb97LcbSttbfBkmk <= 0 || fcLcb97LcbPlcfBkf <= 0 || fcLcb97LcbPlcfBkl <= 0)
      return;
    this.m_bkmkStringTable = new BookmarkNameStringTable(stream, lcb97LcbSttbfBkmk);
    this.m_bkmkDescriptor = new BookmarkDescriptor(stream, this.m_bkmkStringTable.BookmarkCount, fcLcb97FcPlcfBkf, fcLcb97LcbPlcfBkf, fcLcb97FcPlcfBkl, fcLcb97LcbPlcfBkl);
  }

  private void ParsePieceTableEncodings()
  {
    this.m_pieceTablePositions.Clear();
    this.m_pieceTableEncodings.Clear();
    int index = 0;
    for (int entriesCount = this.m_pieceTable.EntriesCount; index < entriesCount; ++index)
    {
      bool bIsUnicode;
      this.m_pieceTablePositions.Add(this.NormalizeFileCharPos(this.m_pieceTable.Entries[index].FileOffset, out bIsUnicode));
      this.m_pieceTableEncodings.Add(bIsUnicode ? Encoding.Unicode : WordDocument.GetEncoding("Windows-1252"));
    }
    this.m_pieceTablePositions.Add(this.ConvertCharPosToFileCharPos(this.m_pieceTable.FileCharacterPos[this.m_pieceTable.EntriesCount]));
  }

  private void ReadArtObjects(Fib fib, Stream stream)
  {
    if (!this.ContainShapes(fib))
      return;
    this.m_artObjects = new ArtObjectsRW(fib, stream);
  }

  private void ReadAnnotations(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbPlcfandTxt <= 0U && fib.FibRgFcLcb97lcbPlcfandRef <= 0U)
      return;
    this.m_anotations = new AnnotationsRW(stream, fib);
  }

  private void ReadFootnotes(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbPlcffndTxt <= 0U && fib.FibRgFcLcb97LcbPlcffndRef <= 0U)
      return;
    this.m_footnotes = new FootnotesRW(stream, fib);
  }

  private void ReadEndnotes(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbPlcfendTxt <= 0U && fib.FibRgFcLcb97LcbPlcfendRef <= 0U)
      return;
    this.m_endnotes = new EndnotesRW(stream, fib);
  }

  private void WriteArtObjects(Stream stream, Fib fib)
  {
    MsofbtDgContainer containerForSubDocType1 = this.m_escher.FindDgContainerForSubDocType(ShapeDocType.Main);
    MsofbtDgContainer containerForSubDocType2 = this.m_escher.FindDgContainerForSubDocType(ShapeDocType.HeaderFooter);
    if (containerForSubDocType1 == null && containerForSubDocType2 == null)
      return;
    if (this.m_artObjects != null)
    {
      if (this.m_artObjects.MainDocFSPAs != null)
      {
        ContainerCollection children1 = containerForSubDocType1.PatriarchGroupContainer.Children;
      }
      if (this.m_artObjects.HfDocFSPAs != null)
      {
        ContainerCollection children2 = containerForSubDocType2.PatriarchGroupContainer.Children;
      }
      int entriesCount = this.m_pieceTable.EntriesCount;
      int endHeader = fib.CcpHdd + 2;
      int fileCharacterPo = (int) this.m_pieceTable.FileCharacterPos[entriesCount];
      this.m_artObjects.Write(stream, fib, fileCharacterPo, endHeader);
    }
    fib.FibRgFcLcb97FcDggInfo = (uint) stream.Position;
    fib.FibRgFcLcb97LcbDggInfo = this.m_escher.WriteContainers(stream);
  }

  private void WriteAnnotations(Fib fib, Stream stream)
  {
    if (this.m_anotations == null)
      return;
    this.m_anotations.Write(stream, fib);
  }

  private void WriteFootnotes(Fib fib, Stream stream)
  {
    if (this.m_footnotes == null)
      return;
    this.m_footnotes.InitialDescriptorNumber = this.DOP.InitialFootnoteNumber;
    this.m_footnotes.Write(stream, fib);
  }

  private void WriteEndnotes(Fib fib, Stream stream)
  {
    if (this.m_endnotes == null)
      return;
    this.m_endnotes.InitialDescriptorNumber = this.DOP.InitialEndnoteNumber;
    this.m_endnotes.Write(stream, fib);
  }

  private static void SynchronizeSpids(
    ContainerCollection collection,
    IDictionaryEnumerator mainEnumerator)
  {
    foreach (BaseEscherRecord baseEscherRecord in (List<object>) collection)
    {
      if (baseEscherRecord is MsofbtSpContainer msofbtSpContainer && msofbtSpContainer.Shape.ShapeType == EscherShapeType.msosptPictureFrame)
      {
        mainEnumerator.MoveNext();
        (mainEnumerator.Value as FileShapeAddress).Spid = msofbtSpContainer.Shape.ShapeId;
      }
    }
  }

  private void ReadMacroCommands(Fib fib, Stream stream)
  {
    stream.Position = (long) fib.FibRgFcLcb97FcCmds;
    this.m_macroCommands = new byte[(IntPtr) fib.FibRgFcLcb97LcbCmds];
    stream.Read(this.m_macroCommands, 0, this.m_macroCommands.Length);
  }

  private void WriteMacroCommands(Fib fib, Stream stream)
  {
    if (this.m_macroCommands == null || this.m_macroCommands.Length <= 0)
      return;
    fib.FibRgFcLcb97FcCmds = (uint) stream.Position;
    stream.Write(this.m_macroCommands, 0, this.m_macroCommands.Length);
    fib.FibRgFcLcb97LcbCmds = (uint) this.m_macroCommands.Length;
  }

  private void ReadGrammarSpellingData(Fib fib, Stream stream)
  {
    this.m_grammarSpellingTablesData = new GrammarSpelling(fib, stream, this.m_charPosTableHF);
  }

  private void WriteGrammarSpellingData(Fib fib, Stream stream)
  {
    if (this.m_grammarSpellingTablesData == null)
      return;
    this.m_grammarSpellingTablesData.Write(fib, stream);
  }

  private bool ContainShapes(Fib fib)
  {
    bool flag = false;
    if (fib.FibRgFcLcb97LcbPlcSpaMom > 0U || fib.FibRgFcLcb97LcbPlcSpaHdr > 0U || fib.FibRgFcLcb97LcbPlcftxbxTxt > 0U || fib.FibRgFcLcb97LcbPlcfHdrtxbxTxt > 0U || fib.FibRgFcLcb97LcbPlcfTxbxBkd > 0U || fib.FibRgFcLcb97LcbPlcfTxbxHdrBkd > 0U)
      flag = true;
    return flag;
  }

  private void ReadVariables(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbStwUser <= 0U)
      return;
    stream.Position = (long) (int) fib.FibRgFcLcb97FcStwUser;
    this.m_variables = new byte[(IntPtr) fib.FibRgFcLcb97LcbStwUser];
    stream.Read(this.m_variables, 0, this.m_variables.Length);
  }

  private void WriteVariables(Fib fib, Stream stream)
  {
    if (this.m_variables == null)
      return;
    fib.FibRgFcLcb97FcStwUser = (uint) stream.Position;
    stream.Write(this.m_variables, 0, this.m_variables.Length);
    fib.FibRgFcLcb97LcbStwUser = (uint) this.m_variables.Length;
  }

  private void ReadAssocStrings(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbSttbfAssoc <= 0U)
      return;
    stream.Position = (long) fib.FibRgFcLcb97FcSttbfAssoc;
    this.m_assocStrings = new byte[(IntPtr) fib.FibRgFcLcb97LcbSttbfAssoc];
    stream.Read(this.m_assocStrings, 0, this.m_assocStrings.Length);
  }

  private void ReadSttbfRMark(Fib fib, Stream stream)
  {
    if (fib.FibRgFcLcb97LcbSttbfRMark <= 0U)
      return;
    stream.Position = (long) fib.FibRgFcLcb97FcSttbfRMark;
    this.m_sttbfRMark = new byte[(IntPtr) fib.FibRgFcLcb97LcbSttbfRMark];
    stream.Read(this.m_sttbfRMark, 0, this.m_sttbfRMark.Length);
  }

  private void WriteAssocStrings(Fib fib, Stream stream)
  {
    if (this.m_assocStrings == null)
      return;
    fib.FibRgFcLcb97FcSttbfAssoc = (uint) stream.Position;
    stream.Write(this.m_assocStrings, 0, this.m_assocStrings.Length);
    fib.FibRgFcLcb97LcbSttbfAssoc = (uint) this.m_assocStrings.Length;
  }

  private void WriteSttbfRMark(Fib fib, Stream stream)
  {
    if (CharacterPropertiesConverter.AuthorNames.Count <= 0)
      return;
    fib.FibRgFcLcb97FcSttbfRMark = (uint) stream.Position;
    this.UpdateSTTBStructure();
    stream.Write(this.m_sttbfRMark, 0, this.m_sttbfRMark.Length);
    fib.FibRgFcLcb97LcbSttbfRMark = (uint) this.m_sttbfRMark.Length;
  }

  private void UpdateSTTBStructure()
  {
    byte[] bytes1 = BitConverter.GetBytes(ushort.MaxValue);
    List<string> authorNames = CharacterPropertiesConverter.AuthorNames;
    byte[] src1 = authorNames.Count >= (int) ushort.MaxValue ? BitConverter.GetBytes(authorNames.Count) : BitConverter.GetBytes((ushort) authorNames.Count);
    byte[] bytes2 = BitConverter.GetBytes((ushort) 0);
    int num1 = 0;
    List<byte[]> numArrayList = new List<byte[]>(authorNames.Count);
    for (int index = 0; index < authorNames.Count; ++index)
    {
      string s = authorNames[index];
      byte[] bytes3 = BitConverter.GetBytes((ushort) s.Length);
      numArrayList.Add(bytes3);
      int num2 = num1 + bytes3.Length;
      byte[] bytes4 = Encoding.Unicode.GetBytes(s);
      numArrayList.Add(bytes4);
      num1 = num2 + bytes4.Length;
    }
    this.m_sttbfRMark = new byte[bytes1.Length + src1.Length + bytes2.Length + num1];
    Buffer.BlockCopy((Array) bytes1, 0, (Array) this.m_sttbfRMark, 0, bytes1.Length);
    Buffer.BlockCopy((Array) src1, 0, (Array) this.m_sttbfRMark, bytes1.Length, src1.Length);
    int dstOffset1 = src1.Length + bytes1.Length;
    Buffer.BlockCopy((Array) bytes2, 0, (Array) this.m_sttbfRMark, dstOffset1, bytes2.Length);
    int dstOffset2 = dstOffset1 + bytes2.Length;
    foreach (byte[] src2 in numArrayList)
    {
      Buffer.BlockCopy((Array) src2, 0, (Array) this.m_sttbfRMark, dstOffset2, src2.Length);
      dstOffset2 += src2.Length;
    }
    numArrayList.Clear();
  }

  private void WriteRmdThreading(Stream stream)
  {
    byte[] buffer = new byte[48 /*0x30*/]
    {
      byte.MaxValue,
      byte.MaxValue,
      (byte) 1,
      (byte) 0,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 2,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 2,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    stream.Write(buffer, 0, buffer.Length);
  }
}
