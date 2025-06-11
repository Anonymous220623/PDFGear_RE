// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathGroupCharacter
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathGroupCharacter : 
  OfficeMathFunctionBase,
  IOfficeMathGroupCharacter,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short HasAlignTopKey = 30;
  internal const short CharKey = 31 /*0x1F*/;
  internal const short HasCharTopKey = 32 /*0x20*/;
  private Dictionary<int, object> m_propertiesHash;
  private OfficeMath m_equation;
  internal IOfficeRunFormat m_controlProperties;

  public bool HasAlignTop
  {
    get => (bool) this.GetPropertyValue(30);
    set => this.SetPropertyValue(30, (object) value);
  }

  public string GroupCharacter
  {
    get => (string) this.GetPropertyValue(31 /*0x1F*/);
    set => this.SetPropertyValue(31 /*0x1F*/, (object) value);
  }

  public bool HasCharacterTop
  {
    get => (bool) this.GetPropertyValue(32 /*0x20*/);
    set => this.SetPropertyValue(32 /*0x20*/, (object) value);
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

  internal OfficeMathGroupCharacter(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.GroupCharacter;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 30:
        return (object) true;
      case 31 /*0x1F*/:
        return (object) Convert.ToString(Convert.ToChar(9183));
      case 32 /*0x20*/:
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
    OfficeMathGroupCharacter owner1 = (OfficeMathGroupCharacter) this.MemberwiseClone();
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
