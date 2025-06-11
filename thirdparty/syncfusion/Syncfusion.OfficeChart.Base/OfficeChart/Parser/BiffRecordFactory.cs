// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.BiffRecordFactory
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser;

[CLSCompliant(false)]
internal class BiffRecordFactory
{
  private const int DEF_RESERVE_SIZE = 200;
  private static Dictionary<int, BiffRecordRaw> m_dict = new Dictionary<int, BiffRecordRaw>(200);

  static BiffRecordFactory() => BiffRecordFactory.FillFactory();

  private static void FillFactory()
  {
    BiffRecordFactory.m_dict[449] = (BiffRecordRaw) new RecalcIdRecord();
    BiffRecordFactory.m_dict[448] = (BiffRecordRaw) new UnknownBeginRecord();
    BiffRecordFactory.m_dict[29] = (BiffRecordRaw) new SelectionRecord();
    BiffRecordFactory.m_dict[42] = (BiffRecordRaw) new PrintHeadersRecord();
    BiffRecordFactory.m_dict[516] = (BiffRecordRaw) new LabelRecord();
    BiffRecordFactory.m_dict[10] = (BiffRecordRaw) new EOFRecord();
    BiffRecordFactory.m_dict[34] = (BiffRecordRaw) new DateWindow1904Record();
    BiffRecordFactory.m_dict[4197] = (BiffRecordRaw) new ChartSiIndexRecord();
    BiffRecordFactory.m_dict[4099] = (BiffRecordRaw) new ChartSeriesRecord();
    BiffRecordFactory.m_dict[4121] = (BiffRecordRaw) new ChartPieRecord();
    BiffRecordFactory.m_dict[4130] = (BiffRecordRaw) new ChartFormatLinkRecord();
    BiffRecordFactory.m_dict[4125] = (BiffRecordRaw) new ChartAxisRecord();
    BiffRecordFactory.m_dict[4166] = (BiffRecordRaw) new ChartAxesUsedRecord();
    BiffRecordFactory.m_dict[92] = (BiffRecordRaw) new WriteAccessRecord();
    BiffRecordFactory.m_dict[566] = (BiffRecordRaw) new TableRecord();
    BiffRecordFactory.m_dict[18] = (BiffRecordRaw) new ProtectRecord();
    BiffRecordFactory.m_dict[189] = (BiffRecordRaw) new MulRKRecord();
    BiffRecordFactory.m_dict[236] = (BiffRecordRaw) new MSODrawingRecord();
    BiffRecordRaw biffRecordRaw1 = (BiffRecordRaw) new MarginRecord();
    biffRecordRaw1.SetRecordCode(38);
    BiffRecordFactory.m_dict[38] = biffRecordRaw1;
    BiffRecordRaw biffRecordRaw2 = (BiffRecordRaw) new MarginRecord();
    biffRecordRaw2.SetRecordCode(41);
    BiffRecordFactory.m_dict[41] = biffRecordRaw2;
    BiffRecordRaw biffRecordRaw3 = (BiffRecordRaw) new MarginRecord();
    biffRecordRaw3.SetRecordCode(39);
    BiffRecordFactory.m_dict[39] = biffRecordRaw3;
    BiffRecordRaw biffRecordRaw4 = (BiffRecordRaw) new MarginRecord();
    biffRecordRaw4.SetRecordCode(40);
    BiffRecordFactory.m_dict[40] = biffRecordRaw4;
    BiffRecordFactory.m_dict[434] = (BiffRecordRaw) new DValRecord();
    BiffRecordFactory.m_dict[549] = (BiffRecordRaw) new DefaultRowHeightRecord();
    BiffRecordFactory.m_dict[1048] = (BiffRecordRaw) new CustomPropertyRecord();
    BiffRecordFactory.m_dict[2129] = (BiffRecordRaw) new ChartWrapperRecord();
    BiffRecordFactory.m_dict[4165] = (BiffRecordRaw) new ChartSertocrtRecord();
    BiffRecordFactory.m_dict[4135] = (BiffRecordRaw) new ChartObjectLinkRecord();
    BiffRecordFactory.m_dict[4198] = (BiffRecordRaw) new ChartGelFrameRecord();
    BiffRecordFactory.m_dict[4098] = (BiffRecordRaw) new ChartChartRecord();
    BiffRecordFactory.m_dict[4191] = (BiffRecordRaw) new Chart3DDataFormatRecord();
    BiffRecordFactory.m_dict[218] = (BiffRecordRaw) new BookBoolRecord();
    BiffRecordFactory.m_dict[445] = (BiffRecordRaw) new UnkMacrosDisable();
    BiffRecordFactory.m_dict[6] = (BiffRecordRaw) new FormulaRecord();
    BiffRecordFactory.m_dict[47] = (BiffRecordRaw) new FilePassRecord();
    BiffRecordFactory.m_dict[4129] = (BiffRecordRaw) new ChartAxisLineFormatRecord();
    BiffRecordFactory.m_dict[2057] = (BiffRecordRaw) new BOFRecord();
    BiffRecordFactory.m_dict[129] = (BiffRecordRaw) new WSBoolRecord();
    BiffRecordFactory.m_dict[26] = (BiffRecordRaw) new VerticalPageBreaksRecord();
    BiffRecordFactory.m_dict[28] = (BiffRecordRaw) new NoteRecord();
    BiffRecordFactory.m_dict[432] = (BiffRecordRaw) new CondFMTRecord();
    BiffRecordFactory.m_dict[4097] = (BiffRecordRaw) new ChartUnitsRecord();
    BiffRecordFactory.m_dict[4170] = (BiffRecordRaw) new ChartSerParentRecord();
    BiffRecordFactory.m_dict[4171] = (BiffRecordRaw) new ChartSerAuxTrendRecord();
    BiffRecordFactory.m_dict[4158] = (BiffRecordRaw) new ChartRadarRecord();
    BiffRecordFactory.m_dict[4175] = (BiffRecordRaw) new ChartPosRecord();
    BiffRecordFactory.m_dict[2155] = (BiffRecordRaw) new ChartDataLabelsRecord();
    BiffRecordFactory.m_dict[4108] = (BiffRecordRaw) new ChartAttachedLabelRecord();
    BiffRecordFactory.m_dict[2205] = (BiffRecordRaw) new ChartAttachedLabelLayoutRecord();
    BiffRecordFactory.m_dict[2215] = (BiffRecordRaw) new ChartPlotAreaLayoutRecord();
    BiffRecordFactory.m_dict[517] = (BiffRecordRaw) new BoolErrRecord();
    BiffRecordFactory.m_dict[95] = (BiffRecordRaw) new SaveRecalcRecord();
    BiffRecordFactory.m_dict[19] = (BiffRecordRaw) new PasswordRecord();
    BiffRecordFactory.m_dict[93] = (BiffRecordRaw) new OBJRecord();
    BiffRecordFactory.m_dict[440] = (BiffRecordRaw) new HLinkRecord();
    BiffRecordFactory.m_dict[4193] = (BiffRecordRaw) new ChartBoppopRecord();
    BiffRecordFactory.m_dict[4161] = (BiffRecordRaw) new ChartAxisParentRecord();
    BiffRecordFactory.m_dict[2135] = (BiffRecordRaw) new ChartAxisDisplayUnitsRecord();
    BiffRecordFactory.m_dict[433] = (BiffRecordRaw) new CFRecord();
    BiffRecordFactory.m_dict[2171] = (BiffRecordRaw) new CFExRecord();
    BiffRecordFactory.m_dict[2170] = (BiffRecordRaw) new CF12Record();
    BiffRecordFactory.m_dict[2169] = (BiffRecordRaw) new CondFmt12Record();
    BiffRecordFactory.m_dict[12] = (BiffRecordRaw) new CalcCountRecord();
    BiffRecordFactory.m_dict[61] = (BiffRecordRaw) new WindowOneRecord();
    BiffRecordFactory.m_dict[161] = (BiffRecordRaw) new PrintSetupRecord();
    BiffRecordFactory.m_dict[523] = (BiffRecordRaw) new IndexRecord();
    BiffRecordFactory.m_dict[235] = (BiffRecordRaw) new MSODrawingGroupRecord();
    BiffRecordFactory.m_dict[2150] = (BiffRecordRaw) new HeaderFooterImageRecord();
    BiffRecordFactory.m_dict[1054] = (BiffRecordRaw) new FormatRecord();
    BiffRecordFactory.m_dict[4164] = (BiffRecordRaw) new ChartShtpropsRecord();
    BiffRecordFactory.m_dict[4199] = (BiffRecordRaw) new ChartBoppCustomRecord();
    BiffRecordFactory.m_dict[89] = (BiffRecordRaw) new XCTRecord();
    BiffRecordFactory.m_dict[352] = (BiffRecordRaw) new UseSelFSRecord();
    BiffRecordFactory.m_dict[214] = (BiffRecordRaw) new RStringRecord();
    BiffRecordFactory.m_dict[222] = (BiffRecordRaw) new OleSizeRecord();
    BiffRecordFactory.m_dict[437] = (BiffRecordRaw) new DConBinRecord();
    BiffRecordFactory.m_dict[66] = (BiffRecordRaw) new CodepageRecord();
    BiffRecordFactory.m_dict[4132] = (BiffRecordRaw) new ChartDefaultTextRecord();
    BiffRecordFactory.m_dict[4102] = (BiffRecordRaw) new ChartDataFormatRecord();
    BiffRecordFactory.m_dict[4122] = (BiffRecordRaw) new ChartAreaRecord();
    BiffRecordFactory.m_dict[134] = (BiffRecordRaw) new WriteProtection();
    BiffRecordFactory.m_dict[431] = (BiffRecordRaw) new ProtectionRev4Record();
    BiffRecordFactory.m_dict[444] = (BiffRecordRaw) new PasswordRev4Record();
    BiffRecordFactory.m_dict[190] = (BiffRecordRaw) new MulBlankRecord();
    BiffRecordFactory.m_dict[253] = (BiffRecordRaw) new LabelSSTRecord();
    BiffRecordFactory.m_dict[353] = (BiffRecordRaw) new DSFRecord();
    BiffRecordFactory.m_dict[82] = (BiffRecordRaw) new DConNameRecord();
    BiffRecordFactory.m_dict[4133] = (BiffRecordRaw) new ChartTextRecord();
    BiffRecordFactory.m_dict[4149] = (BiffRecordRaw) new ChartPlotAreaRecord();
    BiffRecordFactory.m_dict[4103] = (BiffRecordRaw) new ChartLineFormatRecord();
    BiffRecordFactory.m_dict[4117] = (BiffRecordRaw) new ChartLegendRecord();
    BiffRecordFactory.m_dict[15] = (BiffRecordRaw) new RefModeRecord();
    BiffRecordFactory.m_dict[14] = (BiffRecordRaw) new PrecisionRecord();
    BiffRecordFactory.m_dict[99] = (BiffRecordRaw) new ObjectProtectRecord();
    BiffRecordFactory.m_dict[27] = (BiffRecordRaw) new HorizontalPageBreaksRecord();
    BiffRecordFactory.m_dict[141] = (BiffRecordRaw) new HideObjRecord();
    BiffRecordFactory.m_dict[23] = (BiffRecordRaw) new ExternSheetRecord();
    BiffRecordFactory.m_dict[4123] = (BiffRecordRaw) new ChartScatterRecord();
    BiffRecordFactory.m_dict[4107] = (BiffRecordRaw) new ChartPieFormatRecord();
    BiffRecordFactory.m_dict[4105] = (BiffRecordRaw) new ChartMarkerFormatRecord();
    BiffRecordFactory.m_dict[2132] = (BiffRecordRaw) new ChartBegDispUnitRecord();
    BiffRecordFactory.m_dict[4119] = (BiffRecordRaw) new ChartBarRecord();
    BiffRecordFactory.m_dict[4106] = (BiffRecordRaw) new ChartAreaFormatRecord();
    BiffRecordFactory.m_dict[4177] = (BiffRecordRaw) new ChartAIRecord();
    BiffRecordFactory.m_dict[64 /*0x40*/] = (BiffRecordRaw) new BackupRecord();
    BiffRecordFactory.m_dict[25] = (BiffRecordRaw) new WindowProtectRecord();
    BiffRecordFactory.m_dict[438] = (BiffRecordRaw) new TextObjectRecord();
    BiffRecordFactory.m_dict[430] = (BiffRecordRaw) new SupBookRecord();
    BiffRecordFactory.m_dict[24] = (BiffRecordRaw) new NameRecord();
    BiffRecordFactory.m_dict[211] = (BiffRecordRaw) new HasBasicRecord();
    BiffRecordFactory.m_dict[91] = (BiffRecordRaw) new FileSharingRecord();
    BiffRecordFactory.m_dict[215] = (BiffRecordRaw) new DBCellRecord();
    BiffRecordFactory.m_dict[125] = (BiffRecordRaw) new ColumnInfoRecord();
    BiffRecordFactory.m_dict[153] = (BiffRecordRaw) new DxGCol();
    BiffRecordFactory.m_dict[4163] = (BiffRecordRaw) new ChartLegendxnRecord();
    BiffRecordFactory.m_dict[2133] = (BiffRecordRaw) new ChartEndDispUnitRecord();
    BiffRecordFactory.m_dict[233] = (BiffRecordRaw) new BitmapRecord();
    BiffRecordFactory.m_dict[1212] = (BiffRecordRaw) new SharedFormulaRecord();
    BiffRecordFactory.m_dict[446] = (BiffRecordRaw) new DVRecord();
    BiffRecordFactory.m_dict[4174] = (BiffRecordRaw) new ChartIfmtRecord();
    BiffRecordFactory.m_dict[4192] = (BiffRecordRaw) new ChartFbiRecord();
    BiffRecordFactory.m_dict[4195] = (BiffRecordRaw) new ChartDatRecord();
    BiffRecordFactory.m_dict[4147] = (BiffRecordRaw) new BeginRecord();
    BiffRecordFactory.m_dict[0] = (BiffRecordRaw) new UnknownRecord();
    BiffRecordFactory.m_dict[2146] = (BiffRecordRaw) new SheetLayoutRecord();
    BiffRecordFactory.m_dict[520] = (BiffRecordRaw) new RowRecord();
    BiffRecordFactory.m_dict[226] = (BiffRecordRaw) new InterfaceEndRecord();
    BiffRecordFactory.m_dict[128 /*0x80*/] = (BiffRecordRaw) new GutsRecord();
    BiffRecordFactory.m_dict[130] = (BiffRecordRaw) new GridsetRecord();
    BiffRecordFactory.m_dict[155] = (BiffRecordRaw) new FilterModeRecord();
    BiffRecordFactory.m_dict[140] = (BiffRecordRaw) new CountryRecord();
    BiffRecordFactory.m_dict[4109] = (BiffRecordRaw) new ChartSeriesTextRecord();
    BiffRecordFactory.m_dict[133] = (BiffRecordRaw) new BoundSheetRecord();
    BiffRecordFactory.m_dict[574] = (BiffRecordRaw) new WindowTwoRecord();
    BiffRecordFactory.m_dict[317] = (BiffRecordRaw) new TabIdRecord();
    BiffRecordFactory.m_dict[77] = (BiffRecordRaw) new PrinterSettingsRecord();
    BiffRecordFactory.m_dict[193] = (BiffRecordRaw) new MMSRecord();
    BiffRecordRaw biffRecordRaw5 = (BiffRecordRaw) new HeaderFooterRecord();
    biffRecordRaw5.SetRecordCode(20);
    BiffRecordFactory.m_dict[20] = biffRecordRaw5;
    BiffRecordRaw biffRecordRaw6 = (BiffRecordRaw) new HeaderFooterRecord();
    biffRecordRaw6.SetRecordCode(21);
    BiffRecordFactory.m_dict[21] = biffRecordRaw6;
    BiffRecordFactory.m_dict[156] = (BiffRecordRaw) new FnGroupCountRecord();
    BiffRecordFactory.m_dict[(int) byte.MaxValue] = (BiffRecordRaw) new ExtSSTRecord();
    BiffRecordFactory.m_dict[4095 /*0x0FFF*/] = (BiffRecordRaw) new ExtSSTInfoSubRecord();
    BiffRecordFactory.m_dict[22] = (BiffRecordRaw) new ExternCountRecord();
    BiffRecordFactory.m_dict[224 /*0xE0*/] = (BiffRecordRaw) new ExtendedFormatRecord();
    BiffRecordFactory.m_dict[2172] = (BiffRecordRaw) new ExtendedFormatCRC();
    BiffRecordFactory.m_dict[2173] = (BiffRecordRaw) new ExtendedXFRecord();
    BiffRecordFactory.m_dict[2172] = (BiffRecordRaw) new ExtendedFormatCRC();
    BiffRecordFactory.m_dict[2173] = (BiffRecordRaw) new ExtendedXFRecord();
    BiffRecordFactory.m_dict[4148] = (BiffRecordRaw) new EndRecord();
    BiffRecordFactory.m_dict[16 /*0x10*/] = (BiffRecordRaw) new DeltaRecord();
    BiffRecordFactory.m_dict[85] = (BiffRecordRaw) new DefaultColWidthRecord();
    BiffRecordFactory.m_dict[80 /*0x50*/] = (BiffRecordRaw) new DCONRecord();
    BiffRecordFactory.m_dict[60] = (BiffRecordRaw) new ContinueRecord();
    BiffRecordFactory.m_dict[442] = (BiffRecordRaw) new CodeNameRecord();
    BiffRecordFactory.m_dict[4118] = (BiffRecordRaw) new ChartSeriesListRecord();
    BiffRecordFactory.m_dict[4196] = (BiffRecordRaw) new ChartPlotGrowthRecord();
    BiffRecordFactory.m_dict[4120] = (BiffRecordRaw) new ChartLineRecord();
    BiffRecordFactory.m_dict[513] = (BiffRecordRaw) new BlankRecord();
    BiffRecordFactory.m_dict[519] = (BiffRecordRaw) new StringRecord();
    BiffRecordFactory.m_dict[252] = (BiffRecordRaw) new SSTRecord();
    BiffRecordRaw biffRecordRaw7 = (BiffRecordRaw) new SheetCenterRecord();
    biffRecordRaw7.SetRecordCode(131);
    BiffRecordFactory.m_dict[131] = biffRecordRaw7;
    BiffRecordRaw biffRecordRaw8 = (BiffRecordRaw) new SheetCenterRecord();
    biffRecordRaw8.SetRecordCode(132);
    BiffRecordFactory.m_dict[132] = biffRecordRaw8;
    BiffRecordFactory.m_dict[2048 /*0x0800*/] = (BiffRecordRaw) new QuickTipRecord();
    BiffRecordFactory.m_dict[43] = (BiffRecordRaw) new PrintGridlinesRecord();
    BiffRecordFactory.m_dict[90] = (BiffRecordRaw) new CRNRecord();
    BiffRecordFactory.m_dict[51] = (BiffRecordRaw) new PrintedChartSizeRecord();
    BiffRecordFactory.m_dict[4126] = (BiffRecordRaw) new ChartTickRecord();
    BiffRecordFactory.m_dict[4160] = (BiffRecordRaw) new ChartRadarAreaRecord();
    BiffRecordFactory.m_dict[4146] = (BiffRecordRaw) new ChartFrameRecord();
    BiffRecordFactory.m_dict[4134] = (BiffRecordRaw) new ChartFontxRecord();
    BiffRecordFactory.m_dict[4116] = (BiffRecordRaw) new ChartChartFormatRecord();
    BiffRecordFactory.m_dict[239] = (BiffRecordRaw) new UnknownMarkerRecord();
    BiffRecordFactory.m_dict[96 /*0x60*/] = (BiffRecordRaw) new TemplateRecord();
    BiffRecordFactory.m_dict[659] = (BiffRecordRaw) new StyleRecord();
    BiffRecordFactory.m_dict[2194] = (BiffRecordRaw) new StyleExtRecord();
    BiffRecordFactory.m_dict[638] = (BiffRecordRaw) new RKRecord();
    BiffRecordFactory.m_dict[439] = (BiffRecordRaw) new RefreshAllRecord();
    BiffRecordFactory.m_dict[2152] = (BiffRecordRaw) new RangeProtectionRecord();
    BiffRecordFactory.m_dict[515] = (BiffRecordRaw) new NumberRecord();
    BiffRecordFactory.m_dict[351] = (BiffRecordRaw) new LabelRangesRecord();
    BiffRecordFactory.m_dict[225] = (BiffRecordRaw) new InterfaceHdrRecord();
    BiffRecordFactory.m_dict[81] = (BiffRecordRaw) new DConRefRecord();
    BiffRecordFactory.m_dict[4127] = (BiffRecordRaw) new ChartValueRangeRecord();
    BiffRecordFactory.m_dict[4189] = (BiffRecordRaw) new ChartSerFmtRecord();
    BiffRecordFactory.m_dict[2134] = (BiffRecordRaw) new ChartAxisOffsetRecord();
    BiffRecordFactory.m_dict[545] = (BiffRecordRaw) new ArrayRecord();
    BiffRecordFactory.m_dict[160 /*0xA0*/] = (BiffRecordRaw) new WindowZoomRecord();
    BiffRecordFactory.m_dict[221] = (BiffRecordRaw) new ScenProtectRecord();
    BiffRecordFactory.m_dict[65] = (BiffRecordRaw) new PaneRecord();
    BiffRecordFactory.m_dict[146] = (BiffRecordRaw) new PaletteRecord();
    BiffRecordFactory.m_dict[49] = (BiffRecordRaw) new FontRecord();
    BiffRecordFactory.m_dict[512 /*0x0200*/] = (BiffRecordRaw) new DimensionsRecord();
    BiffRecordFactory.m_dict[4159] = (BiffRecordRaw) new ChartSurfaceRecord();
    BiffRecordFactory.m_dict[4187] = (BiffRecordRaw) new ChartSerAuxErrBarRecord();
    BiffRecordFactory.m_dict[4157] = (BiffRecordRaw) new ChartDropBarRecord();
    BiffRecordFactory.m_dict[4124] = (BiffRecordRaw) new ChartChartLineRecord();
    BiffRecordFactory.m_dict[13] = (BiffRecordRaw) new CalcModeRecord();
    BiffRecordFactory.m_dict[158] = (BiffRecordRaw) new AutoFilterRecord();
    BiffRecordFactory.m_dict[144 /*0x90*/] = (BiffRecordRaw) new SortRecord();
    BiffRecordFactory.m_dict[2151] = (BiffRecordRaw) new SheetProtectionRecord();
    BiffRecordFactory.m_dict[229] = (BiffRecordRaw) new MergeCellsRecord();
    BiffRecordFactory.m_dict[17] = (BiffRecordRaw) new IterationRecord();
    BiffRecordFactory.m_dict[35] = (BiffRecordRaw) new ExternNameRecord();
    BiffRecordFactory.m_dict[4168] = (BiffRecordRaw) new ChartSbaserefRecord();
    BiffRecordFactory.m_dict[4156] = (BiffRecordRaw) new ChartPicfRecord();
    BiffRecordFactory.m_dict[4128] = (BiffRecordRaw) new ChartCatserRangeRecord();
    BiffRecordFactory.m_dict[4194] = (BiffRecordRaw) new ChartAxcextRecord();
    BiffRecordFactory.m_dict[4176] = (BiffRecordRaw) new ChartAlrunsRecord();
    BiffRecordFactory.m_dict[4154] = (BiffRecordRaw) new Chart3DRecord();
    BiffRecordFactory.m_dict[157] = (BiffRecordRaw) new AutoFilterInfoRecord();
    BiffRecordFactory.m_dict[(int) sbyte.MaxValue] = (BiffRecordRaw) new ImageDataRecord();
    BiffRecordFactory.m_dict[2206] = (BiffRecordRaw) new UnknownRecord();
    BiffRecordFactory.m_dict[2188] = (BiffRecordRaw) new CompatibilityRecord();
    BiffRecordFactory.m_dict[2166] = (BiffRecordRaw) new UnknownRecord();
    BiffRecordFactory.m_dict[2204] = (BiffRecordRaw) new HeaderAndFooterRecord();
    BiffRecordRaw biffRecordRaw9 = (BiffRecordRaw) new PageLayoutView();
    biffRecordRaw9.SetRecordCode(2187);
    BiffRecordFactory.m_dict[2187] = biffRecordRaw9;
  }

  public static BiffRecordRaw GetRecord(TBIFFRecord type)
  {
    return BiffRecordFactory.GetRecord((int) type);
  }

  public static BiffRecordRaw GetRecord(int type)
  {
    object obj = BiffRecordFactory.m_dict.ContainsKey(type) ? (object) BiffRecordFactory.m_dict[type] : (object) (BiffRecordRaw) null;
    ICloneable cloneable = (ICloneable) null;
    if (obj != null)
      cloneable = obj as ICloneable;
    else if (BiffRecordFactory.m_dict.ContainsKey(0))
    {
      UnknownRecord unknownRecord = (UnknownRecord) BiffRecordFactory.m_dict[0];
      unknownRecord.RecordCode = type;
      cloneable = (ICloneable) unknownRecord;
    }
    return cloneable != null ? cloneable.Clone() as BiffRecordRaw : (BiffRecordRaw) null;
  }

  public static BiffRecordRaw GetUntypedRecord(Stream stream)
  {
    int itemSize;
    return (BiffRecordRaw) new UnknownRecord(stream, out itemSize);
  }

  public static BiffRecordRaw GetUntypedRecord(BinaryReader reader)
  {
    int itemSize;
    return (BiffRecordRaw) new UnknownRecord(reader, out itemSize);
  }

  public static BiffRecordRaw GetRecord(TBIFFRecord type, BinaryReader reader, byte[] arrBuffer)
  {
    return BiffRecordFactory.GetRecord((int) type, reader, arrBuffer);
  }

  public static BiffRecordRaw GetRecord(int type, BinaryReader reader, byte[] arrBuffer)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    return BiffRecordFactory.GetRecord(type);
  }

  public static BiffRecordRaw GetRecord(DataProvider provider, int iOffset, OfficeVersion version)
  {
    int type = provider != null ? (int) provider.ReadInt16(iOffset) : throw new ArgumentNullException(nameof (provider));
    iOffset += 2;
    BiffRecordRaw record = BiffRecordFactory.GetRecord(type);
    int iLength = (int) provider.ReadInt16(iOffset);
    record.Length = iLength;
    iOffset += 2;
    record.ParseStructure(provider, iOffset, iLength, version);
    return record;
  }

  public static int ExtractRecordType(BinaryReader reader)
  {
    Stream stream = reader != null ? reader.BaseStream : throw new ArgumentNullException(nameof (reader));
    long position = stream.Position;
    int num = (int) reader.ReadInt16();
    stream.Position = position;
    return num != 0 ? num : throw new ApplicationException("Cannot find record identifier in stream!");
  }

  public static int ExtractRecordType(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (!stream.CanSeek || !stream.CanRead)
      throw new ApplicationException("Stream must permit seeking and reading operations");
    long position = stream.Position;
    int key = (stream.ReadByte() & (int) byte.MaxValue) + ((stream.ReadByte() & (int) byte.MaxValue) << 8);
    if (key == 0)
      throw new ApplicationException("Cannot find record identifier in stream!");
    if (!BiffRecordFactory.m_dict.ContainsKey(key))
      key = 0;
    return key;
  }
}
