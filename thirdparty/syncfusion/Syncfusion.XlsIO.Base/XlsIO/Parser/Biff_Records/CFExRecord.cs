// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CFExRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.CFEx)]
[CLSCompliant(false)]
public class CFExRecord : CondFMTRecord
{
  private const ushort DEF_MINIMUM_RECORD_SIZE = 18;
  private const ushort DEF_ISCF12_RECORD_SIZE = 25;
  private CF12Record m_cf12Record;
  private FutureHeader m_header;
  private bool m_isRefRange = true;
  private bool m_isFutureAlert;
  private byte m_headerAttribute = 1;
  private TAddr m_addrEncloseRange = new TAddr();
  private byte m_isCF12;
  private ushort m_CondFMTIndex;
  private ushort m_CFIndex;
  private byte m_compareOperator;
  private ushort m_template;
  private ushort m_priority;
  private byte m_undefined = 1;
  private bool m_cfExIsparsed;
  private byte m_hasDXF;
  private ushort m_sizeOfDXF;
  private ushort m_propertyCount;
  private List<ExtendedProperty> m_properties;
  private ushort m_templateParamCount = 16 /*0x10*/;
  private ushort m_reserved = ushort.MaxValue;
  private ushort m_defaultParameter;
  private DXFN m_dxfn;
  private CFExFilterParameter m_cfExFilterParam;
  internal CFExTextTemplateParameter m_cfExTextParam;
  internal CFExDateTemplateParameter m_cfExDateParam;
  private CFExAverageTemplateParameter m_cfExAverageParam;

  public new TAddr EncloseRange
  {
    get => this.m_addrEncloseRange;
    set => this.m_addrEncloseRange = value;
  }

  public byte IsCF12Extends
  {
    get => this.m_isCF12;
    set => this.m_isCF12 = value;
  }

  public ushort CondFmtIndex
  {
    get => this.m_CondFMTIndex;
    set => this.m_CondFMTIndex = value;
  }

  public ushort CFIndex
  {
    get => this.m_CFIndex;
    set => this.m_CFIndex = value;
  }

  public ExcelComparisonOperator ComparisonOperator
  {
    get => (ExcelComparisonOperator) this.m_compareOperator;
    set => this.m_compareOperator = (byte) value;
  }

  public ConditionalFormatTemplate Template
  {
    get => (ConditionalFormatTemplate) this.m_template;
    set => this.m_template = (ushort) (byte) value;
  }

  public ushort Priority
  {
    get => this.m_priority;
    set => this.m_priority = value;
  }

  public bool StopIfTrue
  {
    get => ((int) this.m_undefined & 2) == 2;
    set => this.m_undefined = value ? (byte) ((int) this.m_undefined | 2) : this.m_undefined;
  }

  public byte HasDXF
  {
    get => this.m_hasDXF;
    set => this.m_hasDXF = value;
  }

  public ushort SizeOfDXF
  {
    get => this.m_sizeOfDXF;
    set => this.m_sizeOfDXF = value;
  }

  public ushort PropertyCount
  {
    get => this.m_propertyCount;
    set => this.m_propertyCount = value;
  }

  public List<ExtendedProperty> Properties
  {
    get => this.m_properties;
    set => this.m_properties = value;
  }

  public override int MinimumRecordSize => 18;

  public bool IsCFExParsed
  {
    get => this.m_cfExIsparsed;
    set => this.m_cfExIsparsed = value;
  }

  public CF12Record CF12RecordIfExtends
  {
    get => this.m_cf12Record;
    set => this.m_cf12Record = value;
  }

  internal CFExFilterParameter TopBottomCFEx
  {
    get => this.m_cfExFilterParam;
    set => this.m_cfExFilterParam = value;
  }

  internal CFExAverageTemplateParameter AboveBelowAverageCFEx
  {
    get => this.m_cfExAverageParam;
    set => this.m_cfExAverageParam = value;
  }

  public CFExRecord()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2171;
    this.m_dxfn = new DXFN();
    this.m_properties = new List<ExtendedProperty>();
    this.m_cfExFilterParam = new CFExFilterParameter();
    this.m_cfExTextParam = new CFExTextTemplateParameter();
    this.m_cfExDateParam = new CFExDateTemplateParameter();
    this.m_cfExAverageParam = new CFExAverageTemplateParameter();
  }

  public CFExRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CFExRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_cfExIsparsed = true;
    this.m_header.Type = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_headerAttribute = provider.ReadByte(iOffset);
    iOffset += 2;
    this.m_addrEncloseRange = provider.ReadAddr(iOffset);
    iOffset += 8;
    this.m_isCF12 = (byte) provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_CondFMTIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    if (this.m_isCF12 != (byte) 0)
      return;
    this.m_CFIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_compareOperator = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_template = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_priority = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_undefined = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_hasDXF = provider.ReadByte(iOffset);
    ++iOffset;
    if (this.m_hasDXF != (byte) 0)
    {
      this.m_sizeOfDXF = (ushort) provider.ReadUInt32(iOffset);
      iOffset += 4;
      if (this.m_sizeOfDXF == (ushort) 0)
      {
        int num = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
      }
      int num1 = iOffset;
      if (this.m_sizeOfDXF > (ushort) 0)
      {
        this.m_dxfn = new DXFN();
        iOffset = this.m_dxfn.ParseDXFN(provider, iOffset, version);
      }
      if ((int) this.m_sizeOfDXF != iOffset - num1)
      {
        int num2 = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        this.m_reserved = provider.ReadUInt16(iOffset);
        iOffset += 2;
        int num3 = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        this.m_propertyCount = provider.ReadUInt16(iOffset);
        iOffset += 2;
        this.m_properties = new List<ExtendedProperty>();
        for (int index = 0; index < (int) this.m_propertyCount; ++index)
        {
          ExtendedProperty extendedProperty = new ExtendedProperty();
          iOffset = extendedProperty.ParseExtendedProperty(provider, iOffset, version);
          this.m_properties.Add(extendedProperty);
        }
      }
    }
    this.m_templateParamCount = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    iOffset = this.ParseCFExTemplateParameter(provider, iOffset, version);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    ushort maxValue = ushort.MaxValue;
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_header.Type);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_headerAttribute);
    iOffset += 2;
    provider.WriteAddr(iOffset, this.m_addrEncloseRange);
    iOffset += 8;
    provider.WriteUInt32(iOffset, (uint) this.m_isCF12);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_CondFMTIndex);
    iOffset += 2;
    if (this.m_isCF12 != (byte) 0)
      return;
    provider.WriteUInt16(iOffset, this.m_CFIndex);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_compareOperator);
    ++iOffset;
    provider.WriteByte(iOffset, (byte) this.m_template);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_priority);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_undefined);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_hasDXF);
    ++iOffset;
    if (this.m_hasDXF != (byte) 0)
    {
      provider.WriteUInt32(iOffset, (uint) this.m_sizeOfDXF);
      iOffset += 4;
      if (this.m_sizeOfDXF == (ushort) 0)
      {
        provider.WriteUInt16(iOffset, (ushort) 0);
        iOffset += 2;
      }
      int num = iOffset;
      if (this.m_sizeOfDXF > (ushort) 0)
        iOffset = this.m_dxfn.SerializeDXFN(provider, iOffset, version);
      if ((int) this.m_sizeOfDXF != iOffset - num)
      {
        provider.WriteUInt16(iOffset, (ushort) 0);
        iOffset += 2;
        provider.WriteUInt16(iOffset, maxValue);
        iOffset += 2;
        provider.WriteUInt16(iOffset, (ushort) 0);
        iOffset += 2;
        provider.WriteUInt16(iOffset, (ushort) this.m_properties.Count);
        iOffset += 2;
        foreach (ExtendedProperty property in this.m_properties)
          iOffset = property.InfillInternalData(provider, iOffset, version);
      }
    }
    provider.WriteByte(iOffset, (byte) this.m_templateParamCount);
    ++iOffset;
    iOffset = this.SerializeCFExTemplateParameter(provider, iOffset, version);
  }

  public int ParseCFExTemplateParameter(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (this.Template == ConditionalFormatTemplate.Filter)
      this.m_cfExFilterParam.ParseFilterTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.ContainsText)
      this.m_cfExTextParam.ParseTextTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.Today || this.Template == ConditionalFormatTemplate.Tomorrow || this.Template == ConditionalFormatTemplate.Yesterday || this.Template == ConditionalFormatTemplate.Last7Days || this.Template == ConditionalFormatTemplate.LastMonth || this.Template == ConditionalFormatTemplate.NextMonth || this.Template == ConditionalFormatTemplate.ThisWeek || this.Template == ConditionalFormatTemplate.NextWeek || this.Template == ConditionalFormatTemplate.LastWeek || this.Template == ConditionalFormatTemplate.ThisMonth)
      this.m_cfExDateParam.ParseDateTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.AboveAverage || this.Template == ConditionalFormatTemplate.BelowAverage || this.Template == ConditionalFormatTemplate.AboveOrEqualToAverage || this.Template == ConditionalFormatTemplate.BelowOrEqualToAverage)
    {
      this.m_cfExAverageParam.ParseAverageTemplateParameter(provider, iOffset, version);
      this.m_cfExAverageParam.CopyAverageTemplaterParameter(this.Template);
    }
    else
    {
      this.m_defaultParameter = provider.ReadUInt16(iOffset);
      iOffset += 16 /*0x10*/;
    }
    return iOffset;
  }

  public int SerializeCFExTemplateParameter(
    DataProvider provider,
    int iOffset,
    ExcelVersion version)
  {
    if (this.Template == ConditionalFormatTemplate.Filter)
      this.m_cfExFilterParam.SerializeFilterParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.ContainsText)
      this.m_cfExTextParam.SerializeTextTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.Today || this.Template == ConditionalFormatTemplate.Tomorrow || this.Template == ConditionalFormatTemplate.Yesterday || this.Template == ConditionalFormatTemplate.Last7Days || this.Template == ConditionalFormatTemplate.LastMonth || this.Template == ConditionalFormatTemplate.NextMonth || this.Template == ConditionalFormatTemplate.ThisWeek || this.Template == ConditionalFormatTemplate.NextWeek || this.Template == ConditionalFormatTemplate.LastWeek || this.Template == ConditionalFormatTemplate.ThisMonth)
      this.m_cfExDateParam.SerializeDateTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.AboveAverage || this.Template == ConditionalFormatTemplate.BelowAverage || this.Template == ConditionalFormatTemplate.AboveOrEqualToAverage || this.Template == ConditionalFormatTemplate.BelowOrEqualToAverage)
    {
      this.m_cfExAverageParam.SerializeAverageTemplateParameter(provider, iOffset, version);
    }
    else
    {
      provider.WriteInt64(iOffset, 0L);
      iOffset += 8;
      provider.WriteInt64(iOffset, 0L);
      iOffset += 8;
    }
    return iOffset;
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 18;
    int num = 0;
    if (this.m_isCF12 == (byte) 0)
    {
      storeSize += 25;
      if (this.m_hasDXF != (byte) 0)
      {
        storeSize += 4;
        if (this.m_sizeOfDXF == (ushort) 0)
          storeSize += 2;
        if (this.m_sizeOfDXF != (ushort) 0)
        {
          num = this.m_dxfn.GetStoreSize(version);
          storeSize += num;
        }
        if ((int) this.m_sizeOfDXF != num)
        {
          storeSize += 8;
          if (this.m_propertyCount > (ushort) 0)
          {
            foreach (ExtendedProperty property in this.m_properties)
              storeSize += (int) property.Size;
          }
        }
      }
    }
    return storeSize;
  }

  public override int GetHashCode()
  {
    return this.m_header.Type.GetHashCode() ^ this.m_isRefRange.GetHashCode() ^ this.m_isFutureAlert.GetHashCode() ^ this.CellList.GetHashCode() ^ this.m_isCF12.GetHashCode() ^ this.m_CondFMTIndex.GetHashCode() ^ this.m_CFIndex.GetHashCode() ^ this.ComparisonOperator.GetHashCode() ^ this.m_template.GetHashCode() ^ this.m_priority.GetHashCode() ^ this.m_undefined.GetHashCode() ^ this.m_hasDXF.GetHashCode() ^ this.m_sizeOfDXF.GetHashCode() ^ this.m_dxfn.GetHashCode() ^ this.m_propertyCount.GetHashCode() ^ this.m_properties.GetHashCode() ^ this.m_templateParamCount.GetHashCode() ^ this.m_cfExFilterParam.GetHashCode() ^ this.m_cfExTextParam.GetHashCode() ^ this.m_cfExDateParam.GetHashCode() ^ this.m_cfExAverageParam.GetHashCode() ^ this.m_defaultParameter.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    return obj is CFExRecord cfExRecord && (int) this.m_header.Type == (int) cfExRecord.m_header.Type && this.m_isRefRange == cfExRecord.m_isRefRange && this.m_isFutureAlert == cfExRecord.m_isFutureAlert && this.CellList == cfExRecord.CellList && (int) this.m_isCF12 == (int) cfExRecord.m_isCF12 && (int) this.m_CondFMTIndex == (int) cfExRecord.m_CondFMTIndex && (int) this.m_CFIndex == (int) cfExRecord.m_CFIndex && this.ComparisonOperator == cfExRecord.ComparisonOperator && (int) this.m_template == (int) cfExRecord.m_template && (int) this.m_priority == (int) cfExRecord.m_priority && (int) this.m_undefined == (int) cfExRecord.m_undefined && (int) this.m_hasDXF == (int) cfExRecord.m_hasDXF && (int) this.m_sizeOfDXF == (int) cfExRecord.m_sizeOfDXF && this.m_dxfn == cfExRecord.m_dxfn && (int) this.m_propertyCount == (int) cfExRecord.m_propertyCount && this.m_properties == cfExRecord.m_properties && (int) this.m_templateParamCount == (int) cfExRecord.m_templateParamCount && this.m_cfExFilterParam.Equals((object) cfExRecord.m_cfExFilterParam) && this.m_cfExTextParam == cfExRecord.m_cfExTextParam && this.m_cfExDateParam == cfExRecord.m_cfExDateParam && this.m_cfExAverageParam.Equals((object) cfExRecord.m_cfExAverageParam) && (int) this.m_defaultParameter == (int) cfExRecord.m_defaultParameter;
  }

  public override object Clone()
  {
    CFExRecord cfExRecord = (CFExRecord) this.MemberwiseClone();
    cfExRecord.m_cfExAverageParam = (CFExAverageTemplateParameter) this.m_cfExAverageParam.Clone();
    cfExRecord.m_cfExFilterParam = (CFExFilterParameter) this.m_cfExFilterParam.Clone();
    return (object) cfExRecord;
  }

  internal void ClearAll() => this.m_cf12Record.ClearAll();
}
