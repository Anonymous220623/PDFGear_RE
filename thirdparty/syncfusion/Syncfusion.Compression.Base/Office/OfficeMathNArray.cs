// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathNArray
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathNArray : 
  OfficeMathFunctionBase,
  IOfficeMathNArray,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short NAryCharKey = 41;
  internal const short HasGrowKey = 42;
  internal const short HideSubscriptKey = 43;
  internal const short HideSuperscriptKey = 44;
  internal const short SubSupLimitKey = 45;
  private OfficeMath m_equation;
  private OfficeMath m_subscript;
  private OfficeMath m_superscript;
  private Dictionary<int, object> m_propertiesHash;
  internal IOfficeRunFormat m_controlProperties;

  public string NArrayCharacter
  {
    get => (string) this.GetPropertyValue(41);
    set => this.SetPropertyValue(41, (object) value);
  }

  public bool HasGrow
  {
    get => (bool) this.GetPropertyValue(42);
    set => this.SetPropertyValue(42, (object) value);
  }

  public bool HideLowerLimit
  {
    get => (bool) this.GetPropertyValue(43);
    set => this.SetPropertyValue(43, (object) value);
  }

  public bool HideUpperLimit
  {
    get => (bool) this.GetPropertyValue(44);
    set => this.SetPropertyValue(44, (object) value);
  }

  public bool SubSuperscriptLimit
  {
    get => (bool) this.GetPropertyValue(45);
    set => this.SetPropertyValue(45, (object) value);
  }

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

  public IOfficeMath Subscript => (IOfficeMath) this.m_subscript;

  public IOfficeMath Superscript => (IOfficeMath) this.m_superscript;

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

  internal OfficeMathNArray(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.NArray;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_subscript = new OfficeMath((IOfficeMathEntity) this);
    this.m_superscript = new OfficeMath((IOfficeMathEntity) this);
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
    if (this.m_subscript != null)
      this.m_subscript.Close();
    if (this.m_superscript != null)
      this.m_superscript.Close();
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Dispose();
      this.m_controlProperties = (IOfficeRunFormat) null;
    }
    base.Close();
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 41:
        return (object) Convert.ToString(Convert.ToChar(8747));
      case 42:
      case 43:
      case 44:
        return (object) false;
      case 45:
        return this.NArrayCharacter.Equals(Convert.ToString(Convert.ToChar(8747))) ? (object) true : (object) false;
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
    OfficeMathNArray owner1 = (OfficeMathNArray) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_subscript = this.m_subscript.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_superscript = this.m_superscript.CloneImpl((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }
}
