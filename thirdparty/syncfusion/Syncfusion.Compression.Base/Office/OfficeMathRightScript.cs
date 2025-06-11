// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathRightScript
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathRightScript : 
  OfficeMathFunctionBase,
  IOfficeMathRightScript,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short SkipAlignKey = 52;
  private OfficeMath m_subscript;
  private OfficeMath m_superscript;
  private OfficeMath m_equation;
  private Dictionary<int, object> m_propertiesHash;
  internal IOfficeRunFormat m_controlProperties;

  public bool IsSkipAlign
  {
    get => (bool) this.GetPropertyValue(52);
    set => this.SetPropertyValue(52, (object) value);
  }

  public IOfficeMath Subscript => (IOfficeMath) this.m_subscript;

  public IOfficeMath Superscript => (IOfficeMath) this.m_superscript;

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

  internal OfficeMathRightScript(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.RightSubSuperscript;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_subscript = new OfficeMath((IOfficeMathEntity) this);
    this.m_superscript = new OfficeMath((IOfficeMathEntity) this);
  }

  internal void ToScrPre()
  {
  }

  internal void RemoveSub()
  {
  }

  internal void RemoveSup()
  {
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

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    return key == 52 ? (object) true : (object) new ArgumentException("key has invalid value");
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathRightScript owner1 = (OfficeMathRightScript) this.MemberwiseClone();
    owner1.SetOwner(owner);
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_subscript = this.m_subscript.CloneImpl((IOfficeMathEntity) owner1);
    owner1.m_superscript = this.m_superscript.CloneImpl((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }
}
