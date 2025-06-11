// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CF12Record
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CF12)]
public class CF12Record : BiffRecordRaw
{
  private const int DEF_MINIMUM_RECORD_SIZE = 46;
  private FutureHeader m_header;
  private bool m_isRange;
  private bool m_isFutureAlert;
  private TAddr m_addrEncloseRange = new TAddr();
  private byte m_typeOfCondition = 1;
  private byte m_compareOperator;
  private ushort m_usFirstFormulaSize;
  private ushort m_usSecondFormulaSize;
  private byte[] m_arrFirstFormula = new byte[0];
  private byte[] m_arrSecondFormula = new byte[0];
  private Ptg[] m_arrFirstFormulaParsed;
  private Ptg[] m_arrSecondFormulaParsed;
  private ushort m_formulaLength;
  private byte[] m_arrFormula = new byte[0];
  private Ptg[] m_arrFormulaParsed;
  private ushort m_sizeOfDXF;
  private ushort m_propertyCount;
  private List<ExtendedProperty> m_properties;
  private byte m_undefined;
  private ushort m_priority;
  private ushort m_template;
  private ushort m_templateParamCount = 16 /*0x10*/;
  private long m_defaultParameter;
  private ushort m_reserved = ushort.MaxValue;
  private DXFN m_dxfn;
  private CFExFilterParameter m_cfExFilterParam;
  private CFExTextTemplateParameter m_cfExTextParam;
  private CFExDateTemplateParameter m_cfExDateParam;
  private CFExAverageTemplateParameter m_cfExAverageParam;
  private DataBar m_dataBar;
  private CFIconSet m_iconSet;
  private ColorScale m_colorScale;
  private IColorScale m_colorImpl;
  private IDataBar m_dataBarImpl;
  private IIconSet m_icondSetImpl;
  private bool m_isParsed;

  public ExcelCFType FormatType
  {
    get => (ExcelCFType) this.m_typeOfCondition;
    set => this.m_typeOfCondition = (byte) value;
  }

  public ExcelComparisonOperator ComparisonOperator
  {
    get => (ExcelComparisonOperator) this.m_compareOperator;
    set => this.m_compareOperator = (byte) value;
  }

  public ushort FirstFormulaSize => this.m_usFirstFormulaSize;

  public ushort SecondFormulaSize => this.m_usSecondFormulaSize;

  public Ptg[] FirstFormulaPtgs
  {
    get => this.m_arrFirstFormulaParsed;
    set
    {
      this.m_arrFirstFormula = FormulaUtil.PtgArrayToByteArray(value, ExcelVersion.Excel2007);
      this.m_arrFirstFormulaParsed = value;
      this.m_usFirstFormulaSize = (ushort) this.m_arrFirstFormula.Length;
    }
  }

  public Ptg[] SecondFormulaPtgs
  {
    get => this.m_arrSecondFormulaParsed;
    set
    {
      this.m_arrSecondFormula = FormulaUtil.PtgArrayToByteArray(value, ExcelVersion.Excel2007);
      this.m_arrSecondFormulaParsed = value;
      this.m_usSecondFormulaSize = (ushort) this.m_arrSecondFormula.Length;
    }
  }

  public byte[] FirstFormulaBytes => this.m_arrFirstFormula;

  public byte[] SecondFormulaBytes => this.m_arrSecondFormula;

  public Ptg[] FormulaPtgs
  {
    get => this.m_arrFormulaParsed;
    set
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(value, ExcelVersion.Excel2007);
      this.m_arrFormulaParsed = value;
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
  }

  public byte[] FormulaBytes => this.m_arrFormula;

  public bool StopIfTrue
  {
    get => ((int) this.m_undefined & 2) == 2;
    set => this.m_undefined = value ? (byte) ((int) this.m_undefined | 2) : this.m_undefined;
  }

  public ushort Priority
  {
    get => this.m_priority;
    set => this.m_priority = value;
  }

  public ConditionalFormatTemplate Template
  {
    get => (ConditionalFormatTemplate) this.m_template;
    set => this.m_template = (ushort) (byte) value;
  }

  public IColorScale Criteria
  {
    get => this.m_colorImpl;
    set => this.m_colorImpl = value;
  }

  public IDataBar DataBarImpl
  {
    get => this.m_dataBarImpl;
    set => this.m_dataBarImpl = value;
  }

  public IIconSet IconSetImpl
  {
    get => this.m_icondSetImpl;
    set => this.m_icondSetImpl = value;
  }

  public bool IsParsed
  {
    get => this.m_isParsed;
    set => this.m_isParsed = value;
  }

  public ColorScale ColorScaleCF12
  {
    get => this.m_colorScale;
    set => this.m_colorScale = value;
  }

  public DataBar DataBarCF12
  {
    get => this.m_dataBar;
    set => this.m_dataBar = value;
  }

  public CFIconSet IconSetCF12
  {
    get => this.m_iconSet;
    set => this.m_iconSet = value;
  }

  internal CFExFilterParameter TopBottomCF12
  {
    get => this.m_cfExFilterParam;
    set => this.m_cfExFilterParam = value;
  }

  internal CFExAverageTemplateParameter AboveBelowAverageCF12
  {
    get => this.m_cfExAverageParam;
    set => this.m_cfExAverageParam = value;
  }

  internal List<ExtendedProperty> Properties
  {
    get => this.m_properties;
    set => this.m_properties = value;
  }

  public CF12Record()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2170;
    this.m_dxfn = new DXFN();
    this.m_colorScale = new ColorScale();
    this.m_dataBar = new DataBar();
    this.m_iconSet = new CFIconSet();
    this.m_properties = new List<ExtendedProperty>();
    this.m_cfExFilterParam = new CFExFilterParameter();
    this.m_cfExTextParam = new CFExTextTemplateParameter();
    this.m_cfExDateParam = new CFExDateTemplateParameter();
    this.m_cfExAverageParam = new CFExAverageTemplateParameter();
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.IsParsed = true;
    this.m_header.Type = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_isRange = provider.ReadBit(iOffset, 0);
    this.m_isFutureAlert = provider.ReadBit(iOffset, 1);
    iOffset += 2;
    this.m_addrEncloseRange = provider.ReadAddr(iOffset);
    iOffset += 8;
    this.m_typeOfCondition = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_compareOperator = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_usFirstFormulaSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usSecondFormulaSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
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
    this.m_arrFirstFormula = new byte[(int) this.m_usFirstFormulaSize];
    provider.ReadArray(iOffset, this.m_arrFirstFormula);
    iOffset += (int) this.m_usFirstFormulaSize;
    this.m_arrSecondFormula = new byte[(int) this.m_usSecondFormulaSize];
    provider.ReadArray(iOffset, this.m_arrSecondFormula);
    iOffset += (int) this.m_usSecondFormulaSize;
    if (!FormulaUtil.IsValid((DataProvider) new ByteArrayDataProvider(this.m_arrFirstFormula), (int) this.m_usFirstFormulaSize))
      return;
    this.m_arrFirstFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrFirstFormula), (int) this.m_usFirstFormulaSize, version);
    this.m_arrSecondFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrSecondFormula), (int) this.m_usSecondFormulaSize, version);
    if (version != ExcelVersion.Excel2007)
    {
      if (this.m_usFirstFormulaSize > (ushort) 0)
      {
        this.m_arrFirstFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFirstFormulaParsed, ExcelVersion.Excel2007);
        this.m_usFirstFormulaSize = (ushort) this.m_arrFirstFormula.Length;
      }
      if (this.m_usSecondFormulaSize > (ushort) 0)
      {
        this.m_arrSecondFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrSecondFormulaParsed, ExcelVersion.Excel2007);
        this.m_usSecondFormulaSize = (ushort) this.m_arrSecondFormula.Length;
      }
    }
    this.m_formulaLength = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_arrFormula = new byte[(int) this.m_formulaLength];
    provider.ReadArray(iOffset, this.m_arrFormula);
    iOffset += (int) this.m_formulaLength;
    this.m_arrFormulaParsed = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(this.m_arrFormula), (int) this.m_formulaLength, version);
    if (version != ExcelVersion.Excel2007 && this.m_formulaLength > (ushort) 0)
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, ExcelVersion.Excel2007);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    this.m_undefined = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_priority = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_template = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_templateParamCount = (ushort) provider.ReadByte(iOffset);
    ++iOffset;
    if (this.m_typeOfCondition == (byte) 5)
      iOffset += 19;
    iOffset = this.ParseCFExTemplateParameter(provider, iOffset, version);
    switch (this.FormatType)
    {
      case ExcelCFType.ColorScale:
        this.m_colorScale = new ColorScale();
        iOffset = this.m_colorScale.ParseColorScale(provider, iOffset, version);
        break;
      case ExcelCFType.DataBar:
        this.m_dataBar = new DataBar();
        iOffset = this.m_dataBar.ParseDataBar(provider, iOffset, version);
        break;
      case ExcelCFType.IconSet:
        this.m_iconSet = new CFIconSet();
        iOffset = this.m_iconSet.ParseIconSet(provider, iOffset, version);
        break;
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    if (this.m_arrFirstFormulaParsed != null && this.m_arrFirstFormulaParsed.Length > 0)
    {
      this.m_arrFirstFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFirstFormulaParsed, version);
      this.m_usFirstFormulaSize = (ushort) this.m_arrFirstFormula.Length;
    }
    else
    {
      this.m_arrFirstFormula = (byte[]) null;
      this.m_usFirstFormulaSize = (ushort) 0;
    }
    if (this.m_arrSecondFormulaParsed != null && this.m_arrSecondFormulaParsed.Length > 0)
    {
      this.m_arrSecondFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrSecondFormulaParsed, version);
      this.m_usSecondFormulaSize = (ushort) this.m_arrSecondFormula.Length;
    }
    else
    {
      this.m_arrSecondFormula = (byte[]) null;
      this.m_usSecondFormulaSize = (ushort) 0;
    }
    if (this.m_arrFormulaParsed != null && this.m_arrFormulaParsed.Length > 0)
    {
      this.m_arrFormula = FormulaUtil.PtgArrayToByteArray(this.m_arrFormulaParsed, version);
      this.m_formulaLength = (ushort) this.m_arrFormula.Length;
    }
    else
    {
      this.m_arrFormula = (byte[]) null;
      this.m_formulaLength = (ushort) 0;
    }
    if (this.FormatType == ExcelCFType.TopBottom)
      this.m_typeOfCondition = (byte) 5;
    else if (this.FormatType == ExcelCFType.AboveBelowAverage && this.m_arrFirstFormulaParsed != null && this.m_arrFirstFormulaParsed.Length > 0)
      this.m_typeOfCondition = (byte) 2;
    provider.WriteUInt16(iOffset, this.m_header.Type);
    iOffset += 2;
    provider.WriteBit(iOffset, this.m_isRange, 0);
    provider.WriteBit(iOffset, this.m_isFutureAlert, 1);
    iOffset += 2;
    provider.WriteAddr(iOffset, this.m_addrEncloseRange);
    iOffset += 8;
    provider.WriteByte(iOffset, this.m_typeOfCondition);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_compareOperator);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usFirstFormulaSize);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usSecondFormulaSize);
    iOffset += 2;
    if (this.FormatType == ExcelCFType.ColorScale || this.FormatType == ExcelCFType.DataBar || this.FormatType == ExcelCFType.IconSet)
      this.m_sizeOfDXF = (ushort) 0;
    provider.WriteUInt32(iOffset, (uint) this.m_sizeOfDXF);
    iOffset += 4;
    if (this.m_sizeOfDXF == (ushort) 0)
    {
      provider.WriteUInt16(iOffset, (ushort) 0);
      iOffset += 2;
    }
    int num = iOffset;
    if (this.m_sizeOfDXF != (ushort) 0)
      iOffset = this.m_dxfn.SerializeDXFN(provider, iOffset, version);
    if ((int) this.m_sizeOfDXF != iOffset - num)
    {
      provider.WriteUInt16(iOffset, (ushort) 0);
      iOffset += 2;
      provider.WriteUInt16(iOffset, this.m_reserved);
      iOffset += 2;
      provider.WriteUInt16(iOffset, (ushort) 0);
      iOffset += 2;
      provider.WriteUInt16(iOffset, this.m_propertyCount);
      iOffset += 2;
      foreach (ExtendedProperty property in this.m_properties)
        iOffset = property.InfillInternalData(provider, iOffset, version);
    }
    provider.WriteBytes(iOffset, this.m_arrFirstFormula, 0, (int) this.m_usFirstFormulaSize);
    iOffset += (int) this.m_usFirstFormulaSize;
    provider.WriteBytes(iOffset, this.m_arrSecondFormula, 0, (int) this.m_usSecondFormulaSize);
    iOffset += (int) this.m_usSecondFormulaSize;
    provider.WriteUInt16(iOffset, this.m_formulaLength);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_arrFormula, 0, (int) this.m_formulaLength);
    iOffset += (int) this.m_formulaLength;
    provider.WriteByte(iOffset, this.m_undefined);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_priority);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_template);
    iOffset += 2;
    provider.WriteByte(iOffset, (byte) this.m_templateParamCount);
    ++iOffset;
    if (this.m_typeOfCondition == (byte) 5)
    {
      provider.WriteInt64(iOffset, 0L);
      iOffset += 8;
      provider.WriteInt64(iOffset, 0L);
      iOffset += 8;
      provider.WriteByte(iOffset, (byte) 4);
      ++iOffset;
      provider.WriteInt16(iOffset, (short) 0);
      iOffset += 2;
    }
    iOffset = this.SerializeCFExTemplateParameter(provider, iOffset, version);
    switch (this.FormatType)
    {
      case ExcelCFType.ColorScale:
        if (this.Criteria != null && this.Criteria.Criteria.Count != 0)
          this.m_colorScale = new ColorScale();
        iOffset = this.m_colorScale.SerializeColorScale(provider, iOffset, version, this.Criteria);
        break;
      case ExcelCFType.DataBar:
        if (this.m_dataBarImpl != null)
          this.m_dataBar = new DataBar();
        iOffset = this.m_dataBar.SerializeDataBar(provider, iOffset, version, this.DataBarImpl);
        break;
      case ExcelCFType.IconSet:
        if (this.m_icondSetImpl != null)
          this.m_iconSet = new CFIconSet();
        iOffset = this.m_iconSet.SerializeIconSet(provider, iOffset, version, this.IconSetImpl);
        break;
    }
  }

  public int ParseCFExTemplateParameter(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (this.Template == ConditionalFormatTemplate.Filter)
    {
      this.m_cfExFilterParam.ParseFilterTemplateParameter(provider, iOffset, version);
      this.FormatType = ExcelCFType.TopBottom;
    }
    else if (this.Template == ConditionalFormatTemplate.ContainsText)
      this.m_cfExTextParam.ParseTextTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.Today || this.Template == ConditionalFormatTemplate.Tomorrow || this.Template == ConditionalFormatTemplate.Yesterday || this.Template == ConditionalFormatTemplate.Last7Days || this.Template == ConditionalFormatTemplate.LastMonth || this.Template == ConditionalFormatTemplate.NextMonth || this.Template == ConditionalFormatTemplate.ThisWeek || this.Template == ConditionalFormatTemplate.NextWeek || this.Template == ConditionalFormatTemplate.LastWeek || this.Template == ConditionalFormatTemplate.ThisMonth)
      this.m_cfExDateParam.ParseDateTemplateParameter(provider, iOffset, version);
    else if (this.Template == ConditionalFormatTemplate.AboveAverage || this.Template == ConditionalFormatTemplate.BelowAverage || this.Template == ConditionalFormatTemplate.AboveOrEqualToAverage || this.Template == ConditionalFormatTemplate.BelowOrEqualToAverage)
    {
      this.m_cfExAverageParam.ParseAverageTemplateParameter(provider, iOffset, version);
      this.m_cfExAverageParam.CopyAverageTemplaterParameter(this.Template);
      this.FormatType = ExcelCFType.AboveBelowAverage;
    }
    else
    {
      this.m_defaultParameter = provider.ReadInt64(iOffset);
      iOffset += 8;
      this.m_defaultParameter = provider.ReadInt64(iOffset);
      iOffset += 8;
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
    int storeSize1 = 46;
    byte[] numArray = (byte[]) null;
    if (this.m_sizeOfDXF == (ushort) 0)
      storeSize1 += 2;
    if (this.FormatType == ExcelCFType.ColorScale)
    {
      int num1;
      if (this.m_colorScale.ListCFInterpolationCurve.Count == 0)
      {
        for (int index = 0; index < this.Criteria.Criteria.Count; ++index)
        {
          CFInterpolationCurve interpolationCurve = new CFInterpolationCurve();
          interpolationCurve.CFVO.CFVOType = this.Criteria.Criteria[index].Type;
          if ((this.Criteria.Criteria[index] as ConditionValue).RefPtg != null)
            numArray = FormulaUtil.PtgArrayToByteArray((this.Criteria.Criteria[index] as ConditionValue).RefPtg, version);
          CFGradientItem cfGradientItem = new CFGradientItem();
          storeSize1 = storeSize1 + interpolationCurve.GetStoreSize(version) + cfGradientItem.GetStoreSize(version);
        }
        int num2 = storeSize1 + (int) this.m_colorScale.DefaultRecordSize;
        num1 = numArray != null ? (int) (ushort) numArray.Length + num2 : num2;
      }
      else
        num1 = storeSize1 + this.m_colorScale.GetStoreSize(version);
      storeSize1 = num1 + DVRecord.GetFormulaSize(this.m_arrFormulaParsed, version, true);
    }
    if (this.FormatType == ExcelCFType.DataBar)
    {
      if (this.m_dataBarImpl != null)
      {
        this.m_dataBar.MinCFVO = new CFVO();
        this.m_dataBar.MinCFVO.CFVOType = this.m_dataBarImpl.MinPoint.Type == ConditionValueType.Automatic ? (this.m_dataBarImpl.MinPoint.Type = ConditionValueType.LowestValue) : this.m_dataBarImpl.MinPoint.Type;
        this.m_dataBar.MaxCFVO = new CFVO();
        this.m_dataBar.MaxCFVO.CFVOType = this.m_dataBarImpl.MaxPoint.Type == ConditionValueType.Automatic ? (this.m_dataBarImpl.MaxPoint.Type = ConditionValueType.HighestValue) : this.m_dataBarImpl.MaxPoint.Type;
      }
      storeSize1 = storeSize1 + this.m_dataBar.GetStoreSize(version) + DVRecord.GetFormulaSize(this.m_arrFormulaParsed, version, true);
    }
    if (this.FormatType == ExcelCFType.IconSet)
    {
      int num;
      if (this.m_iconSet.ListCFIconSet.Count == 0)
      {
        for (int index = 0; index < this.IconSetImpl.IconCriteria.Count; ++index)
          storeSize1 += new CFIconMultiState()
          {
            CFVO = {
              CFVOType = this.IconSetImpl.IconCriteria[index].Type
            }
          }.GetStoreSize(version);
        num = storeSize1 + (int) this.m_iconSet.DefaultRecordSize;
      }
      else
        num = storeSize1 + this.m_iconSet.GetStoreSize(version);
      storeSize1 = num + DVRecord.GetFormulaSize(this.m_arrFormulaParsed, version, true);
    }
    if (this.FormatType == ExcelCFType.CellValue)
      storeSize1 = storeSize1 + (int) this.m_sizeOfDXF + DVRecord.GetFormulaSize(this.m_arrFirstFormulaParsed, version, true) + DVRecord.GetFormulaSize(this.m_arrSecondFormulaParsed, version, true);
    else if (this.FormatType == ExcelCFType.AboveBelowAverage)
    {
      if (this.m_sizeOfDXF != (ushort) 0)
      {
        int storeSize2 = this.m_dxfn.GetStoreSize(version);
        storeSize1 += storeSize2;
      }
      storeSize1 = storeSize1 + DVRecord.GetFormulaSize(this.m_arrFirstFormulaParsed, version, true) + 18;
    }
    else if (this.FormatType == ExcelCFType.TopBottom)
    {
      if (this.m_sizeOfDXF != (ushort) 0)
      {
        int storeSize3 = this.m_dxfn.GetStoreSize(version);
        storeSize1 += storeSize3;
      }
      storeSize1 += 24;
    }
    this.m_propertyCount = (ushort) this.m_properties.Count;
    if (this.m_propertyCount > (ushort) 0)
    {
      foreach (ExtendedProperty property in this.m_properties)
        storeSize1 += (int) property.Size;
    }
    return storeSize1;
  }

  public override int GetHashCode()
  {
    return this.m_header.Type.GetHashCode() ^ this.m_isRange.GetHashCode() ^ this.m_isFutureAlert.GetHashCode() ^ this.m_addrEncloseRange.GetHashCode() ^ this.m_typeOfCondition.GetHashCode() ^ this.m_compareOperator.GetHashCode() ^ this.m_usFirstFormulaSize.GetHashCode() ^ this.m_usSecondFormulaSize.GetHashCode() ^ this.m_sizeOfDXF.GetHashCode() ^ this.m_dxfn.GetHashCode() ^ this.m_propertyCount.GetHashCode() ^ this.m_properties.GetHashCode() ^ this.m_arrFirstFormula.GetHashCode() ^ this.m_arrSecondFormula.GetHashCode() ^ this.m_formulaLength.GetHashCode() ^ this.m_arrFormula.GetHashCode() ^ this.m_undefined.GetHashCode() ^ this.m_priority.GetHashCode() ^ this.m_template.GetHashCode() ^ this.m_templateParamCount.GetHashCode() ^ this.m_cfExTextParam.GetHashCode() ^ this.m_cfExDateParam.GetHashCode() ^ this.m_cfExFilterParam.GetHashCode() ^ this.m_cfExAverageParam.GetHashCode() ^ this.m_colorScale.GetHashCode() ^ this.m_dataBar.GetHashCode() ^ this.m_iconSet.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    return obj is CF12Record cf12Record && (int) this.m_typeOfCondition == (int) cf12Record.m_typeOfCondition && (int) this.m_compareOperator == (int) cf12Record.m_compareOperator && (int) this.m_usFirstFormulaSize == (int) cf12Record.m_usFirstFormulaSize && (int) this.m_usSecondFormulaSize == (int) cf12Record.m_usSecondFormulaSize && (int) this.m_sizeOfDXF == (int) cf12Record.m_sizeOfDXF && this.m_dxfn == cf12Record.m_dxfn && (int) this.m_propertyCount == (int) cf12Record.m_propertyCount && this.m_properties == cf12Record.m_properties && this.m_arrFirstFormula == cf12Record.m_arrFirstFormula && this.m_arrSecondFormula == cf12Record.m_arrSecondFormula && (int) this.m_formulaLength == (int) cf12Record.m_formulaLength && this.m_arrFormula == cf12Record.m_arrFormula && (int) this.m_undefined == (int) cf12Record.m_undefined && (int) this.m_priority == (int) cf12Record.m_priority && (int) this.m_template == (int) cf12Record.m_template && (int) this.m_templateParamCount == (int) cf12Record.m_templateParamCount && this.m_cfExAverageParam == cf12Record.m_cfExAverageParam && this.m_cfExDateParam == cf12Record.m_cfExDateParam && this.m_cfExFilterParam == cf12Record.m_cfExFilterParam && this.m_cfExTextParam == cf12Record.m_cfExTextParam && this.m_colorScale == cf12Record.m_colorScale && this.m_dataBar == cf12Record.m_dataBar && this.m_iconSet == cf12Record.m_iconSet;
  }

  internal void ClearAll()
  {
    this.m_iconSet.ClearAll();
    this.m_iconSet = (CFIconSet) null;
    this.m_header = (FutureHeader) null;
    this.m_dxfn = (DXFN) null;
    this.m_arrFirstFormulaParsed = (Ptg[]) null;
    this.m_arrFirstFormula = (byte[]) null;
    this.m_arrFormula = (byte[]) null;
    this.m_arrSecondFormula = (byte[]) null;
    this.m_arrSecondFormulaParsed = (Ptg[]) null;
  }
}
