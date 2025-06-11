// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathFraction
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathFraction : 
  OfficeMathFunctionBase,
  IOfficeMathFraction,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short TypeKey = 29;
  private OfficeMath m_denominator;
  private OfficeMath m_numerator;
  private Dictionary<int, object> m_propertiesHash;
  internal IOfficeRunFormat m_controlProperties;

  public MathFractionType FractionType
  {
    get => (MathFractionType) this.GetPropertyValue(29);
    set => this.SetPropertyValue(29, (object) value);
  }

  public IOfficeMath Denominator => (IOfficeMath) this.m_denominator;

  public IOfficeMath Numerator => (IOfficeMath) this.m_numerator;

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

  internal OfficeMathFraction(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Fraction;
    this.m_denominator = new OfficeMath((IOfficeMathEntity) this);
    this.m_numerator = new OfficeMath((IOfficeMathEntity) this);
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_denominator != null)
      this.m_denominator.Close();
    if (this.m_numerator != null)
      this.m_numerator.Close();
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
    return key == 29 ? (object) MathFractionType.NormalFractionBar : (object) new ArgumentException("key has invalid value");
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathFraction owner1 = (OfficeMathFraction) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_denominator = this.m_denominator.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_numerator = this.m_numerator.CloneImpl((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }
}
