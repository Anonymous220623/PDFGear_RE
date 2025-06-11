// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCacheFieldImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotCacheFieldImpl
{
  internal const int MaxStringLength = 255 /*0xFF*/;
  private PivotFieldRecord m_field = (PivotFieldRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotField);
  private SQLDataTypeIdRecord m_typeId = (SQLDataTypeIdRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SQLDataTypeId);
  private List<IValueHolder> m_arrFieldsData = new List<IValueHolder>();
  private PivotCalculatedItems m_calculatedItems;
  private int m_iIndex;
  private PivotDataType m_fieldType;
  private bool? m_bIsDataBaseField;
  private string m_formula;
  private FieldGroupImpl m_fieldGroup;
  private string m_caption;
  private int m_iNumFormatIndex;
  private int m_iParentFieldGroupIndex = -1;
  private int m_iHierarchy;
  private int m_iLevel;
  private bool? m_bIsParsed;
  private IRange m_range;
  private IList<object> m_items;
  private bool? m_bIsMemberPropertyField;
  private bool m_bIsMixedType;

  public PivotCacheFieldImpl() => this.m_items = (IList<object>) new List<object>();

  [CLSCompliant(false)]
  public PivotCacheFieldImpl(BiffReader reader) => this.Parse(reader);

  public string Formula
  {
    get => this.m_formula;
    set => this.m_formula = value;
  }

  public bool? IsDataBaseField
  {
    get => this.m_bIsDataBaseField;
    set => this.m_bIsDataBaseField = value;
  }

  public bool IsInIndexList
  {
    get => this.m_field.IsInIndexList;
    set => this.m_field.IsInIndexList = value;
  }

  public bool IsDouble
  {
    get => this.m_field.IsDouble;
    set => this.m_field.IsDouble = value;
  }

  public bool IsDoubleInt
  {
    get => this.m_field.IsDoubleInt;
    set => this.m_field.IsDoubleInt = value;
  }

  public bool IsString
  {
    get => this.m_field.IsString;
    set => this.m_field.IsString = value;
  }

  public bool IsUnknown
  {
    get => this.m_field.IsUnknown;
    set => this.m_field.IsUnknown = value;
  }

  public bool IsLongIndex
  {
    get => this.m_field.IsLongIndex;
    set => this.m_field.IsLongIndex = value;
  }

  public bool IsUnknown2
  {
    get => this.m_field.IsUnknown2;
    set => this.m_field.IsUnknown2 = value;
  }

  public bool IsDate => this.m_field.IsDate;

  public int ItemCount => this.m_items != null ? this.m_items.Count : 0;

  public string Name
  {
    get => this.m_field.Name;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty");
        default:
          this.m_field.Name = value;
          break;
      }
    }
  }

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  public PivotDataType DataType
  {
    get => this.m_fieldType;
    internal set => this.m_fieldType = value;
  }

  public bool IsFormulaField => this.Formula != null;

  internal FieldGroupImpl FieldGroup
  {
    get => this.m_fieldGroup;
    set => this.m_fieldGroup = value;
  }

  internal FieldGroupImpl InternalFieldGroup
  {
    get
    {
      if (this.m_fieldGroup == null)
        this.m_fieldGroup = new FieldGroupImpl(this);
      return this.m_fieldGroup;
    }
  }

  public string Caption
  {
    get => this.m_caption;
    set => this.m_caption = value;
  }

  public int NumFormatIndex
  {
    get => this.m_iNumFormatIndex;
    set => this.m_iNumFormatIndex = value;
  }

  public PivotCalculatedItems CalculatedItems
  {
    get
    {
      if (this.m_calculatedItems == null)
        this.m_calculatedItems = new PivotCalculatedItems();
      return this.m_calculatedItems;
    }
  }

  public int ParentFeildGroupIndex
  {
    get => this.m_iParentFieldGroupIndex;
    set => this.m_iParentFieldGroupIndex = value;
  }

  public bool IsFieldGroup => this.m_fieldGroup != null;

  internal int Hierarchy
  {
    get => this.m_iHierarchy;
    set => this.m_iHierarchy = value;
  }

  internal int Level
  {
    get => this.m_iLevel;
    set => this.m_iLevel = value;
  }

  public bool? IsParsed
  {
    get => this.m_bIsParsed;
    set => this.m_bIsParsed = value;
  }

  internal IRange ItemRange
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  internal IList<object> Items
  {
    get
    {
      if (this.m_items == null)
        this.m_items = (IList<object>) new List<object>();
      return this.m_items;
    }
    set => this.m_items = value;
  }

  internal bool? IsMemberPropertyField
  {
    get => this.m_bIsMemberPropertyField;
    set => this.m_bIsMemberPropertyField = value;
  }

  internal bool IsMixedType
  {
    get => this.m_bIsMixedType;
    set => this.m_bIsMixedType = value;
  }

  public object GetValue(int index)
  {
    if (this.m_items == null && index < 0 || index >= this.ItemCount)
      throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than ItemCount");
    return this.m_items[index];
  }

  internal void Fill(IWorksheet sheet, int row, int lastRow, int column)
  {
    this.m_items = (IList<object>) new List<object>();
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(sheet.Application, sheet);
    this.m_typeId.DataType = SQLDataTypeIdRecord.SQLDataType.SQL_UNKNOWN_TYPE;
    Dictionary<object, int> dictionary = new Dictionary<object, int>();
    RangeImpl range = this.m_range as RangeImpl;
    range.LastRow = lastRow;
    this.m_fieldType = (PivotDataType) 0;
    this.m_items = range.GetUniqueValues(ref this.m_fieldType);
  }

  internal int AddValue(object value)
  {
    this.m_items.Add(value);
    switch (value)
    {
      case double num:
        this.m_fieldType |= PivotDataType.Number;
        double a = num;
        this.m_fieldType |= a > (double) int.MaxValue || a < (double) int.MinValue || Math.Round(a) != a ? PivotDataType.Float : PivotDataType.Integer;
        break;
      case TimeSpan _:
        this.m_fieldType |= PivotDataType.Date;
        break;
      case DateTime _:
        this.m_fieldType |= PivotDataType.Date;
        break;
      case string _:
        string str = (string) value;
        if (str.Length > 0 || str == string.Empty)
        {
          if (str.Length > (int) byte.MaxValue)
            this.m_fieldType |= PivotDataType.LongText;
          this.m_fieldType |= PivotDataType.String;
          break;
        }
        this.m_fieldType |= PivotDataType.Blank;
        break;
      case bool _:
        this.m_fieldType |= PivotDataType.Boolean;
        break;
      case ushort _:
        this.m_fieldType |= PivotDataType.Boolean;
        break;
      case null:
        this.m_fieldType |= PivotDataType.Blank;
        break;
      default:
        throw new NotImplementedException("cache Value");
    }
    this.m_field.ItemCount1 = this.m_field.ItemCount2 = (ushort) this.m_items.Count;
    return this.ItemCount - 1;
  }

  [CLSCompliant(false)]
  public void Parse(BiffReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.PeekRecordType() != TBIFFRecord.PivotField)
      throw new ArgumentOutOfRangeException("Unexpected record");
    this.m_arrFieldsData.Clear();
    this.m_field = (PivotFieldRecord) reader.GetRecord();
    this.m_typeId = (SQLDataTypeIdRecord) reader.GetRecord();
    int num = 0;
    for (int itemCount1 = (int) this.m_field.ItemCount1; num < itemCount1; ++num)
      this.m_arrFieldsData.Add((IValueHolder) reader.GetRecord());
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_field);
    records.Add((IBiffStorage) this.m_typeId);
    records.AddList((IList) this.m_arrFieldsData);
  }

  private BiffRecordRaw CreateRecordForValue(object value)
  {
    switch (value)
    {
      case null:
        return BiffRecordFactory.GetRecord(TBIFFRecord.PivotEmpty);
      case bool flag:
        PivotBooleanRecord record1 = (PivotBooleanRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotBoolean);
        record1.Value = flag;
        return (BiffRecordRaw) record1;
      case double num:
        PivotDoubleRecord record2 = (PivotDoubleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotDouble);
        record2.Value = num;
        return (BiffRecordRaw) record2;
      case string _:
        PivotStringRecord record3 = (PivotStringRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotString);
        record3.String = (string) value;
        return (BiffRecordRaw) record3;
      case ushort _:
        return BiffRecordFactory.GetRecord(TBIFFRecord.PivotError);
      default:
        throw new NotSupportedException();
    }
  }
}
