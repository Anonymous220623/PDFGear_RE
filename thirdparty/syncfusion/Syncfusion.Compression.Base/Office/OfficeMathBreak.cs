// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathBreak
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathBreak : OwnerHolder, IOfficeMathBreak
{
  internal const short AlignAtKey = 16 /*0x10*/;
  private Dictionary<int, object> m_propertiesHash;

  public int AlignAt
  {
    get => (int) this.GetPropertyValue(16 /*0x10*/);
    set => this.SetPropertyValue(16 /*0x10*/, (object) value);
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

  internal OfficeMathBreak(IOfficeMathEntity owner)
    : base(owner)
  {
  }

  private object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  private object GetDefValue(int key)
  {
    return key == 16 /*0x10*/ ? (object) 0 : (object) new ArgumentException("key has invalid value");
  }

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  internal OfficeMathBreak Clone(IOfficeMathEntity owner)
  {
    OfficeMathBreak officeMathBreak = (OfficeMathBreak) this.MemberwiseClone();
    officeMathBreak.SetOwner(owner);
    return officeMathBreak;
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    base.Close();
  }

  internal OfficeMathBreak Clone() => (OfficeMathBreak) this.MemberwiseClone();
}
