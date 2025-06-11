// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathMatrixColumn
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathMatrixColumn : OwnerHolder, IOfficeMathMatrixColumn, IOfficeMathEntity
{
  internal const short ColumnAlignKey = 40;
  internal MathHorizontalAlignment m_alignment;
  internal OfficeMaths m_args;
  private Dictionary<int, object> m_propertiesHash;

  public int ColumnIndex
  {
    get
    {
      return this.OwnerMathEntity is OfficeMathMatrix ownerMathEntity ? (ownerMathEntity.Columns as OfficeMathMatrixColumns).InnerList.IndexOf((object) this) : -1;
    }
  }

  public IOfficeMaths Arguments
  {
    get
    {
      OfficeMathMatrix ownerMathEntity = this.OwnerMathEntity as OfficeMathMatrix;
      OfficeMaths maths = new OfficeMaths((IOfficeMathEntity) this);
      ownerMathEntity.GetRangeOfArguments(this.ColumnIndex, 0, this.ColumnIndex, ownerMathEntity.Rows.Count - 1, maths);
      return (IOfficeMaths) maths;
    }
  }

  public MathHorizontalAlignment HorizontalAlignment
  {
    get => this.m_alignment;
    set
    {
      this.m_alignment = value;
      if (!(this.OwnerMathEntity is OfficeMathMatrix ownerMathEntity))
        return;
      ownerMathEntity.UpdateColumnProperties(ownerMathEntity);
    }
  }

  internal OfficeMathMatrixColumn(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_propertiesHash = new Dictionary<int, object>();
    this.m_args = new OfficeMaths((IOfficeMathEntity) this);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    return key == 40 ? (object) MathHorizontalAlignment.Center : (object) new ArgumentException("key has invalid value");
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

  internal void OnColumnAdded()
  {
    OfficeMathMatrix ownerMathEntity = this.OwnerMathEntity as OfficeMathMatrix;
    if (ownerMathEntity.Rows.Count <= 0)
      return;
    ownerMathEntity.CreateArguments(this.ColumnIndex, 0, this.ColumnIndex, ownerMathEntity.Rows.Count - 1);
  }

  internal OfficeMathMatrixColumn Clone(IOfficeMathEntity owner)
  {
    OfficeMathMatrixColumn owner1 = (OfficeMathMatrixColumn) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_args = new OfficeMaths((IOfficeMathEntity) owner1);
    this.m_args.CloneItemsTo(owner1.m_args);
    return owner1;
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_args != null)
    {
      this.m_args.Close();
      this.m_args.Clear();
      this.m_args = (OfficeMaths) null;
    }
    base.Close();
  }
}
