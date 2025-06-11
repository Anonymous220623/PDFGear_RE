// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathBox
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathBox : 
  OfficeMathFunctionBase,
  IOfficeMathBox,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short BoxAlignKey = 11;
  internal const short NoBreakKey = 12;
  internal const short EnableDifferentialKey = 14;
  internal const short OperatorEmulatorKey = 15;
  private OfficeMath m_equation;
  private OfficeMathBreak m_break;
  private Dictionary<int, object> m_propertiesHash;
  internal IOfficeRunFormat m_controlProperties;

  public bool Alignment
  {
    get => (bool) this.GetPropertyValue(11);
    set => this.SetPropertyValue(11, (object) value);
  }

  public bool EnableDifferential
  {
    get => (bool) this.GetPropertyValue(14);
    set => this.SetPropertyValue(14, (object) value);
  }

  public bool NoBreak
  {
    get => (bool) this.GetPropertyValue(12);
    set => this.SetPropertyValue(12, (object) value);
  }

  public bool OperatorEmulator
  {
    get => (bool) this.GetPropertyValue(15);
    set => this.SetPropertyValue(15, (object) value);
  }

  public IOfficeMathBreak Break
  {
    get => (IOfficeMathBreak) this.m_break;
    set => this.m_break = (OfficeMathBreak) value;
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

  public OfficeMathBox(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Box;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 11:
      case 14:
      case 15:
        return (object) false;
      case 12:
        return (object) true;
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
    OfficeMathBox owner1 = (OfficeMathBox) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (owner1.m_controlProperties != null)
      owner1.m_controlProperties = this.m_controlProperties.Clone();
    owner1.m_equation = this.m_equation.CloneImpl((IOfficeMathEntity) owner1);
    if (this.m_break != null)
      owner1.m_break = this.m_break.Clone((IOfficeMathEntity) owner1);
    return (OfficeMathFunctionBase) owner1;
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_break != null)
      this.m_break.Close();
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
