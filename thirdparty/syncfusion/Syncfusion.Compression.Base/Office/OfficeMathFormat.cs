// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathFormat
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathFormat : OwnerHolder, IOfficeMathFormat
{
  internal const short HasAlignmentKey = 53;
  internal const short HasLiteralKey = 54;
  internal const short HasNormalTextKey = 55;
  internal const short ScriptKey = 56;
  internal const short StyleKey = 57;
  private byte m_bFlags;
  private OfficeMathBreak m_break;
  private Dictionary<int, object> m_propertiesHash;

  public bool HasAlignment
  {
    get => (bool) this.GetPropertyValue(53);
    set => this.SetPropertyValue(53, (object) value);
  }

  public IOfficeMathBreak Break
  {
    get => (IOfficeMathBreak) this.m_break;
    set
    {
      this.m_break = (OfficeMathBreak) value;
      this.IsDefault = false;
    }
  }

  public bool HasLiteral
  {
    get => (bool) this.GetPropertyValue(54);
    set => this.SetPropertyValue(54, (object) value);
  }

  public bool HasNormalText
  {
    get => (bool) this.GetPropertyValue(55);
    set => this.SetPropertyValue(55, (object) value);
  }

  public MathFontType Font
  {
    get => (MathFontType) this.GetPropertyValue(56);
    set => this.SetPropertyValue(56, (object) value);
  }

  public MathStyleType Style
  {
    get => (MathStyleType) this.GetPropertyValue(57);
    set => this.SetPropertyValue(57, (object) value);
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

  internal bool IsDefault
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal OfficeMathFormat(IOfficeMathEntity owner)
    : base(owner)
  {
    this.IsDefault = true;
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
    base.Close();
  }

  private object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    switch (key)
    {
      case 53:
      case 54:
      case 55:
        return (object) false;
      case 56:
        return (object) MathFontType.Roman;
      case 57:
        return (object) MathStyleType.Italic;
      default:
        return (object) new ArgumentException("key has invalid value");
    }
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal OfficeMathFormat Clone(IOfficeMathEntity owner)
  {
    OfficeMathFormat owner1 = (OfficeMathFormat) this.MemberwiseClone();
    owner1.SetOwner(owner);
    if (this.m_break != null)
      owner1.m_break = this.m_break.Clone((IOfficeMathEntity) owner1);
    return owner1;
  }
}
