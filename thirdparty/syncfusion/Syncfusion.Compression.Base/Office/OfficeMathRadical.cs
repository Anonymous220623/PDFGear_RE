// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathRadical
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathRadical : 
  OfficeMathFunctionBase,
  IOfficeMathRadical,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short HideDegreeKey = 51;
  private Dictionary<int, object> m_propertiesHash;
  public OfficeMath m_degree;
  public OfficeMath m_equation;
  internal IOfficeRunFormat m_controlProperties;

  public IOfficeMath Degree => (IOfficeMath) this.m_degree;

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

  public bool HideDegree
  {
    get => (bool) this.GetPropertyValue(51);
    set => this.SetPropertyValue(51, (object) value);
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

  internal OfficeMathRadical(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Radical;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_degree = new OfficeMath((IOfficeMathEntity) this);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    return key == 51 ? (object) false : (object) new ArgumentException("key has invalid value");
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathRadical owner1 = (OfficeMathRadical) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_degree = this.m_degree.CloneImpl((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_degree != null)
      this.m_degree.Close();
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
