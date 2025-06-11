// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathBorderBox
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathBorderBox : 
  OfficeMathFunctionBase,
  IOfficeMathBorderBox,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short HideTopKey = 3;
  internal const short HideBottomKey = 4;
  internal const short HideRightKey = 5;
  internal const short HideLeftKey = 6;
  internal const short StrikeBLTRKey = 7;
  internal const short StrikeTLBRKey = 8;
  internal const short StrikeVerticalKey = 9;
  internal const short StrikeHorizontalKey = 10;
  private OfficeMath m_equation;
  private Dictionary<int, object> m_propertiesHash;
  internal IOfficeRunFormat m_controlProperties;

  public bool HideTop
  {
    get => (bool) this.GetPropertyValue(3);
    set => this.SetPropertyValue(3, (object) value);
  }

  public bool HideBottom
  {
    get => (bool) this.GetPropertyValue(4);
    set => this.SetPropertyValue(4, (object) value);
  }

  public bool HideRight
  {
    get => (bool) this.GetPropertyValue(5);
    set => this.SetPropertyValue(5, (object) value);
  }

  public bool HideLeft
  {
    get => (bool) this.GetPropertyValue(6);
    set => this.SetPropertyValue(6, (object) value);
  }

  public bool StrikeDiagonalUp
  {
    get => (bool) this.GetPropertyValue(7);
    set => this.SetPropertyValue(7, (object) value);
  }

  public bool StrikeDiagonalDown
  {
    get => (bool) this.GetPropertyValue(8);
    set => this.SetPropertyValue(8, (object) value);
  }

  public bool StrikeVertical
  {
    get => (bool) this.GetPropertyValue(9);
    set => this.SetPropertyValue(9, (object) value);
  }

  public bool StrikeHorizontal
  {
    get => (bool) this.GetPropertyValue(10);
    set => this.SetPropertyValue(10, (object) value);
  }

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

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

  internal OfficeMathBorderBox(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.BorderBox;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 3:
      case 4:
      case 5:
      case 6:
      case 7:
      case 8:
      case 9:
      case 10:
        return (object) false;
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
    OfficeMathBorderBox owner1 = (OfficeMathBorderBox) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
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
