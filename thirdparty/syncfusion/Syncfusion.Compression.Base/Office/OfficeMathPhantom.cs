// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathPhantom
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathPhantom : 
  OfficeMathFunctionBase,
  IOfficeMathPhantom,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short ShowKey = 46;
  internal const short SmashKey = 47;
  internal const short TransparentKey = 48 /*0x30*/;
  internal const short ZeroAscentKey = 49;
  internal const short ZeroDescentKey = 50;
  internal const short ZeroWidthKey = 51;
  private OfficeMath m_equation;
  private Dictionary<int, object> m_propertiesHash;
  private byte m_bFlags;
  internal IOfficeRunFormat m_controlProperties;

  public bool Show
  {
    get => (bool) this.GetPropertyValue(46);
    set => this.SetPropertyValue(46, (object) value);
  }

  internal bool Smash
  {
    get => (bool) this.GetPropertyValue(47);
    set => this.SetPropertyValue(47, (object) value);
  }

  public bool Transparent
  {
    get => (bool) this.GetPropertyValue(48 /*0x30*/);
    set => this.SetPropertyValue(48 /*0x30*/, (object) value);
  }

  public bool ZeroAscent
  {
    get => (bool) this.GetPropertyValue(49);
    set => this.SetPropertyValue(49, (object) value);
  }

  public bool ZeroDescent
  {
    get => (bool) this.GetPropertyValue(50);
    set => this.SetPropertyValue(50, (object) value);
  }

  public bool ZeroWidth
  {
    get => (bool) this.GetPropertyValue(51);
    set => this.SetPropertyValue(51, (object) value);
  }

  public IOfficeMath Equation => (IOfficeMath) this.m_equation;

  internal bool IsDefault
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
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

  internal OfficeMathPhantom(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.Phantom;
    this.m_equation = new OfficeMath((IOfficeMathEntity) this);
    this.m_propertiesHash = new Dictionary<int, object>();
    this.IsDefault = true;
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 46:
        return (object) true;
      case 47:
        return (object) false;
      case 48 /*0x30*/:
        return (object) false;
      case 49:
        return (object) false;
      case 50:
        return (object) false;
      case 51:
        return (object) false;
      default:
        return (object) new ArgumentException("key has invalid value");
    }
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
    set
    {
      this.PropertiesHash[key] = value;
      this.IsDefault = false;
    }
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathPhantom owner1 = (OfficeMathPhantom) this.MemberwiseClone();
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
