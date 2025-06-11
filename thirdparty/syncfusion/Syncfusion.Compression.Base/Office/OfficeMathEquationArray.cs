// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathEquationArray
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathEquationArray : 
  OfficeMathFunctionBase,
  IOfficeMathEquationArray,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal const short AlignKey = 24;
  internal const short MaxDistKey = 25;
  internal const short ObjDistKey = 26;
  internal const short RowSpacingKey = 27;
  internal const short RowSpacingRuleKey = 28;
  internal OfficeMaths m_equation;
  private Dictionary<int, object> m_propertiesHash;
  internal IOfficeRunFormat m_controlProperties;

  public IOfficeMaths Equation => (IOfficeMaths) this.m_equation;

  public MathVerticalAlignment VerticalAlignment
  {
    get => (MathVerticalAlignment) this.GetPropertyValue(24);
    set => this.SetPropertyValue(24, (object) value);
  }

  public bool ExpandEquationContainer
  {
    get => (bool) this.GetPropertyValue(25);
    set => this.SetPropertyValue(25, (object) value);
  }

  public bool ExpandEquationContent
  {
    get => (bool) this.GetPropertyValue(26);
    set => this.SetPropertyValue(26, (object) value);
  }

  public float RowSpacing
  {
    get => (float) this.GetPropertyValue(27);
    set
    {
      if (this.RowSpacingRule == SpacingRule.Exactly && ((double) value < 0.0 || (double) value > 1584.0))
        throw new ArgumentException("RowSpacing must be between 0 pt and 1584 pt for Exactly spacing rule.");
      if (this.RowSpacingRule == SpacingRule.Multiple && ((double) value < 0.0 || (double) value > 132.0))
        throw new ArgumentException("RowSpacing must be between 0 li and 1584 li for Multiple spacing rule.");
      this.SetPropertyValue(27, (object) value);
    }
  }

  public SpacingRule RowSpacingRule
  {
    get => (SpacingRule) this.GetPropertyValue(28);
    set => this.SetPropertyValue(28, (object) value);
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

  internal OfficeMathEquationArray(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = MathFunctionType.EquationArray;
    this.m_equation = new OfficeMaths((IOfficeMathEntity) this);
    this.m_propertiesHash = new Dictionary<int, object>();
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 24:
        return (object) MathVerticalAlignment.Center;
      case 25:
        return (object) false;
      case 26:
        return (object) false;
      case 27:
        return (object) 0.0f;
      case 28:
        return (object) SpacingRule.Single;
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
    set => this.PropertiesHash[key] = value;
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal override OfficeMathFunctionBase Clone(IOfficeMathEntity owner)
  {
    OfficeMathEquationArray owner1 = (OfficeMathEquationArray) this.MemberwiseClone();
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
    {
      this.m_equation.Close();
      this.m_equation = (OfficeMaths) null;
    }
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Dispose();
      this.m_controlProperties = (IOfficeRunFormat) null;
    }
    base.Close();
  }
}
