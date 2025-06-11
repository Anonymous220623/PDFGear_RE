// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMath
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMath : OwnerHolder, IOfficeMath, IOfficeMathEntity
{
  internal const short ArgumentSizeKey = 74;
  internal const short AlignPointKey = 76;
  private OfficeMathBaseCollection m_functions;
  internal OfficeMathMatrixColumn m_parentCol;
  internal OfficeMathMatrixRow m_parentRow;
  private OfficeMathBreaks m_breaks;
  private Dictionary<int, object> m_propertiesHash;

  internal int AlignPoint
  {
    get => (int) this.GetPropertyValue(76);
    set => this.SetPropertyValue(76, (object) value);
  }

  public int ArgumentSize
  {
    get => (int) this.GetPropertyValue(74);
    set => this.SetPropertyValue(74, (object) value);
  }

  public IOfficeMathBaseCollection Functions => (IOfficeMathBaseCollection) this.m_functions;

  internal int NestingLevel => 0;

  public IOfficeMathMatrixColumn OwnerColumn => (IOfficeMathMatrixColumn) this.m_parentCol;

  public IOfficeMath OwnerMath => this.GetOwnerMath();

  public IOfficeMathMatrixRow OwnerRow => (IOfficeMathMatrixRow) this.m_parentRow;

  public IOfficeMathBreaks Breaks => (IOfficeMathBreaks) this.m_breaks;

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

  internal OfficeMath(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_functions = new OfficeMathBaseCollection(this);
    this.m_breaks = new OfficeMathBreaks(this.OwnerMathEntity);
    switch (owner)
    {
      case OfficeMathMatrixRow _:
        this.m_parentRow = owner as OfficeMathMatrixRow;
        break;
      case OfficeMathMatrixColumn _:
        this.m_parentCol = owner as OfficeMathMatrixColumn;
        break;
    }
  }

  private IOfficeMath GetOwnerMath()
  {
    object ownerMathEntity = (object) this.OwnerMathEntity;
    while (!(ownerMathEntity is IOfficeMath) && !(ownerMathEntity is IOfficeMathParagraph))
    {
      if (ownerMathEntity is OfficeMathFunctionBase)
        ownerMathEntity = (object) (ownerMathEntity as OfficeMathFunctionBase).OwnerMathEntity;
    }
    return (IOfficeMath) (ownerMathEntity as OfficeMath);
  }

  internal void Buildup()
  {
  }

  internal void ConvertToLiteralText()
  {
  }

  internal void ConvertToMathText()
  {
  }

  internal void ConvertToNormalText()
  {
  }

  internal void Linearize()
  {
  }

  internal void Remove()
  {
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_breaks != null)
    {
      this.m_breaks.Close();
      this.m_breaks = (OfficeMathBreaks) null;
    }
    if (this.m_functions != null)
    {
      this.m_functions.Close();
      this.m_functions = (OfficeMathBaseCollection) null;
    }
    base.Close();
  }

  internal OfficeMath CloneImpl(IOfficeMathEntity owner)
  {
    OfficeMath owner1 = (OfficeMath) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_breaks = new OfficeMathBreaks((IOfficeMathEntity) owner1);
    this.m_breaks.CloneItemsTo(owner1.m_breaks);
    owner1.m_functions = new OfficeMathBaseCollection(owner1);
    this.m_functions.CloneItemsTo(owner1.m_functions);
    return owner1;
  }

  private object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 74:
        if (!(this.OwnerMathEntity is OfficeMathFunctionBase ownerMathEntity))
          return (object) 0;
        switch (ownerMathEntity.Type)
        {
          case MathFunctionType.Box:
          case MathFunctionType.GroupCharacter:
            return (object) 0;
          case MathFunctionType.Limit:
          case MathFunctionType.NArray:
          case MathFunctionType.LeftSubSuperscript:
          case MathFunctionType.SubSuperscript:
          case MathFunctionType.RightSubSuperscript:
            return (object) -1;
          case MathFunctionType.Radical:
            return (object) -2;
          default:
            return (object) new ArgumentException("Cannot change argument size for this function");
        }
      case 76:
        return (object) -1;
      default:
        return (object) new ArgumentException("key has invalid value");
    }
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }
}
