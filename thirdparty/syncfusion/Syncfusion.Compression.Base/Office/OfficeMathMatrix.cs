// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathMatrix
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathMatrix : 
  OfficeMathFunctionBase,
  IOfficeMathMatrix,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short RowSpacingKey = 27;
  internal const short RowSpacingRuleKey = 28;
  internal const short MatrixAlignKey = 33;
  internal const short ColumnWidthKey = 34;
  internal const short ColumnSpacingRuleKey = 35;
  internal const short ColumnSpacingKey = 36;
  internal const short PlaceHoldHiddenKey = 37;
  internal OfficeMathMatrixRows m_mathMatrixRows;
  internal OfficeMathMatrixColumns m_mathMatrixColumns;
  private Dictionary<int, object> m_propertiesHash;
  private List<MatrixColumnProperties> m_columnProperties;
  internal IOfficeRunFormat m_controlProperties;

  public MathVerticalAlignment VerticalAlignment
  {
    get => (MathVerticalAlignment) this.GetPropertyValue(33);
    set => this.SetPropertyValue(33, (object) value);
  }

  public float ColumnWidth
  {
    get => (float) this.GetPropertyValue(34);
    set
    {
      if ((double) value < 0.0 || (double) value > 1584.0)
        throw new ArgumentException("ColumnWidth must be between 0 pt and 1584 pt.");
      this.SetPropertyValue(34, (object) value);
    }
  }

  public SpacingRule ColumnSpacingRule
  {
    get => (SpacingRule) this.GetPropertyValue(35);
    set => this.SetPropertyValue(35, (object) value);
  }

  public IOfficeMathMatrixColumns Columns => (IOfficeMathMatrixColumns) this.m_mathMatrixColumns;

  public float ColumnSpacing
  {
    get => (float) this.GetPropertyValue(36);
    set
    {
      if ((this.ColumnSpacingRule == SpacingRule.Exactly || this.ColumnSpacingRule == SpacingRule.Multiple) && ((double) value < 0.0 || (double) value > 1584.0))
        throw new ArgumentException("ColumnSpacing must be between 0 pt and 1584 pt");
      if (this.ColumnSpacingRule == SpacingRule.Multiple)
      {
        int num = 6;
        value = (float) (int) Math.Floor((double) value - (double) value % (double) num);
      }
      this.SetPropertyValue(36, (object) value);
    }
  }

  public bool HidePlaceHolders
  {
    get => (bool) this.GetPropertyValue(37);
    set => this.SetPropertyValue(37, (object) value);
  }

  internal List<MatrixColumnProperties> ColumnProperties
  {
    get => this.m_columnProperties;
    set => this.m_columnProperties = value;
  }

  public IOfficeMathMatrixRows Rows => (IOfficeMathMatrixRows) this.m_mathMatrixRows;

  public float RowSpacing
  {
    get => (float) this.GetPropertyValue(27);
    set
    {
      if (this.RowSpacingRule == SpacingRule.Exactly && ((double) value < 0.0 || (double) value > 1584.0))
        throw new ArgumentException("RowSpacing must be between 0 pt and 1584 pt for Exactly spacing rule.");
      if (this.RowSpacingRule == SpacingRule.Multiple && ((double) value < 0.0 || (double) value > 132.0))
        throw new ArgumentException("RowSpacing must be between 0 li and 132 li for Multiple spacing rule.");
      this.SetPropertyValue(27, (object) value);
    }
  }

  public SpacingRule RowSpacingRule
  {
    get => (SpacingRule) this.GetPropertyValue(28);
    set => this.SetPropertyValue(28, (object) value);
  }

  public IOfficeRunFormat ControlProperties
  {
    get
    {
      if (this.m_controlProperties == null)
        this.m_controlProperties = this.GetDefaultControlProperties();
      return this.m_controlProperties;
    }
    set => this.m_controlProperties = value;
  }

  internal OfficeMathMatrix(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Matrix;
    this.m_propertiesHash = new Dictionary<int, object>();
    this.m_mathMatrixRows = new OfficeMathMatrixRows((IOfficeMathEntity) this);
    this.m_mathMatrixColumns = new OfficeMathMatrixColumns((IOfficeMathEntity) this);
    this.m_columnProperties = new List<MatrixColumnProperties>();
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 27:
        return (object) 0.0f;
      case 28:
        return (object) SpacingRule.Single;
      case 33:
        return (object) MathVerticalAlignment.Center;
      case 34:
        return (object) 0.0f;
      case 35:
        return (object) SpacingRule.Single;
      case 36:
        return (object) 0.0f;
      case 37:
        return (object) false;
      default:
        return (object) new ArgumentException("key has invalid value");
    }
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  internal object this[int key]
  {
    get => !this.PropertiesHash.ContainsKey(key) ? this.GetDefValue(key) : this.PropertiesHash[key];
    set => this.PropertiesHash[key] = value;
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathMatrix owner1 = (OfficeMathMatrix) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_mathMatrixColumns = new OfficeMathMatrixColumns((IOfficeMathEntity) owner1);
    this.m_mathMatrixColumns.CloneItemsTo(owner1.m_mathMatrixColumns);
    owner1.m_mathMatrixRows = new OfficeMathMatrixRows((IOfficeMathEntity) owner1);
    this.m_mathMatrixRows.CloneItemsTo(owner1.m_mathMatrixRows);
    return (OfficeMathFunctionBase) owner1;
  }

  internal void RemoveMatrixItems(
    int startColIndex,
    int startRowIndex,
    int endColIndex,
    int endRowIndex)
  {
    for (; startRowIndex <= endRowIndex; ++startRowIndex)
    {
      OfficeMathMatrixRow mathMatrixRow = this.m_mathMatrixRows[startRowIndex] as OfficeMathMatrixRow;
      for (int index = startColIndex; index <= endColIndex; ++index)
        mathMatrixRow?.m_args.InnerList.RemoveAt(startColIndex);
    }
  }

  internal void ApplyColumnProperties()
  {
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < this.ColumnProperties.Count; ++index1)
    {
      MatrixColumnProperties columnProperty = this.ColumnProperties[index1];
      for (int index2 = num1; index2 < num1 + columnProperty.Count; ++index2)
      {
        if (this.Columns[index2] is OfficeMathMatrixColumn column)
          column.m_alignment = columnProperty.Alignment;
        ++num2;
      }
      num1 = num2;
    }
  }

  internal void UpdateColumnProperties(OfficeMathMatrix mathMatrix)
  {
    mathMatrix.ColumnProperties.Clear();
    MatrixColumnProperties columnProperties = new MatrixColumnProperties((IOfficeMathEntity) this);
    columnProperties.Alignment = this.Columns[0].HorizontalAlignment;
    columnProperties.Count = 1;
    IOfficeMathMatrixColumns columns = mathMatrix.Columns;
    int num = 1;
    for (int index = 1; index < columns.Count; ++index)
    {
      MathHorizontalAlignment horizontalAlignment = mathMatrix.Columns[index].HorizontalAlignment;
      if (columns[index - 1].HorizontalAlignment == columns[index].HorizontalAlignment)
      {
        ++num;
      }
      else
      {
        this.m_columnProperties.Add(columnProperties);
        columnProperties = new MatrixColumnProperties((IOfficeMathEntity) this);
        columnProperties.Alignment = horizontalAlignment;
        num = 1;
      }
      columnProperties.Count = num;
    }
    this.m_columnProperties.Add(columnProperties);
  }

  internal void GetRangeOfArguments(
    int startColIndex,
    int startRowIndex,
    int endColIndex,
    int endRowIndex,
    OfficeMaths maths)
  {
    for (; startRowIndex <= endRowIndex; ++startRowIndex)
    {
      OfficeMathMatrixRow mathMatrixRow = this.m_mathMatrixRows[startRowIndex] as OfficeMathMatrixRow;
      for (int index = startColIndex; index <= endColIndex; ++index)
      {
        if (startColIndex < mathMatrixRow.Arguments.Count)
          maths.InnerList.Add((object) mathMatrixRow.Arguments[startColIndex]);
      }
    }
  }

  internal void CreateArguments(
    int startColIndex,
    int startRowIndex,
    int endColIndex,
    int endRowIndex)
  {
    for (; startRowIndex <= endRowIndex; ++startRowIndex)
    {
      OfficeMathMatrixRow mathMatrixRow = this.m_mathMatrixRows[startRowIndex] as OfficeMathMatrixRow;
      for (int index = startColIndex; index <= endColIndex; ++index)
      {
        if (mathMatrixRow.Arguments.Count >= startColIndex)
          mathMatrixRow.m_args.InnerList.Add((object) new OfficeMath((IOfficeMathEntity) mathMatrixRow)
          {
            m_parentCol = (this.Columns[startColIndex] as OfficeMathMatrixColumn)
          });
      }
    }
  }

  internal void UpdateColumns()
  {
    int maximumCellCount = this.GetMaximumCellCount();
    for (int index = 0; index < maximumCellCount; ++index)
      this.Columns.Add();
  }

  private int GetMaximumCellCount()
  {
    int maximumCellCount = 0;
    for (int index = 0; index < this.Rows.Count; ++index)
    {
      if (maximumCellCount < this.Rows[index].Arguments.Count)
        maximumCellCount = this.Rows[index].Arguments.Count;
    }
    return maximumCellCount;
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_mathMatrixRows != null)
      this.m_mathMatrixRows.Close();
    if (this.m_mathMatrixColumns != null)
      this.m_mathMatrixColumns.Close();
    if (this.m_columnProperties != null)
    {
      this.m_columnProperties.Clear();
      this.m_columnProperties = (List<MatrixColumnProperties>) null;
    }
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Dispose();
      this.m_controlProperties = (IOfficeRunFormat) null;
    }
    base.Close();
  }
}
