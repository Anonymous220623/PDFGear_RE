// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathDelimiter
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathDelimiter : 
  OfficeMathFunctionBase,
  IOfficeMathDelimiter,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short BegCharKey = 17;
  internal const short EndCharKey = 18;
  internal const short NoRightCharKey = 19;
  internal const short NoLeftCharKey = 20;
  internal const short SeperatorCharKey = 21;
  internal const short GrowKey = 22;
  internal const short ShapeKey = 23;
  private Dictionary<int, object> m_propertiesHash;
  internal OfficeMaths m_equation;
  internal IOfficeRunFormat m_controlProperties;

  public string BeginCharacter
  {
    get => (string) this.GetPropertyValue(17);
    set => this.SetPropertyValue(17, (object) value);
  }

  public string EndCharacter
  {
    get => (string) this.GetPropertyValue(18);
    set => this.SetPropertyValue(18, (object) value);
  }

  public bool IsGrow
  {
    get => (bool) this.GetPropertyValue(22);
    set => this.SetPropertyValue(22, (object) value);
  }

  public string Seperator
  {
    get => (string) this.GetPropertyValue(21);
    set => this.SetPropertyValue(21, (object) value);
  }

  public MathDelimiterShapeType DelimiterShape
  {
    get => (MathDelimiterShapeType) this.GetPropertyValue(23);
    set => this.SetPropertyValue(23, (object) value);
  }

  public IOfficeMaths Equation => (IOfficeMaths) this.m_equation;

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

  internal OfficeMathDelimiter(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Delimiter;
    this.m_equation = new OfficeMaths((IOfficeMathEntity) this);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 17:
        return (object) "(";
      case 18:
        return (object) ")";
      case 19:
      case 20:
        return (object) false;
      case 21:
        return (object) "|";
      case 22:
        return (object) true;
      case 23:
        return (object) MathDelimiterShapeType.Centered;
      default:
        return (object) new ArgumentException("key has invalid value");
    }
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathDelimiter owner1 = (OfficeMathDelimiter) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_equation = new OfficeMaths((IOfficeMathEntity) owner1);
    this.m_equation.CloneItemsTo(owner1.m_equation);
    return (OfficeMathFunctionBase) owner1;
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_equation != null)
      this.m_equation.Close();
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Dispose();
      this.m_controlProperties = (IOfficeRunFormat) null;
    }
    base.Close();
  }
}
