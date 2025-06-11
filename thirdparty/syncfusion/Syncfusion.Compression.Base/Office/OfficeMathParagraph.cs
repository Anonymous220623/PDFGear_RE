// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathParagraph
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathParagraph : OwnerHolder, IOfficeMathParagraph, IOfficeMathEntity
{
  internal const short MathJustificationKey = 77;
  private OfficeMaths m_maths;
  private Dictionary<int, object> m_propertiesHash;
  private byte m_bFlags;
  private object m_ownerEntity;
  internal IOfficeRunFormat m_defaultMathCharacterFormat;

  public MathJustification Justification
  {
    get => (MathJustification) this.GetPropertyValue(77);
    set => this.SetPropertyValue(77, (object) value);
  }

  public IOfficeMaths Maths => (IOfficeMaths) this.m_maths;

  public object Owner => this.m_ownerEntity;

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

  internal IOfficeRunFormat DefaultMathCharacterFormat
  {
    get => this.m_defaultMathCharacterFormat;
    set => this.m_defaultMathCharacterFormat = value;
  }

  internal OfficeMathParagraph(object owner)
    : base((IOfficeMathEntity) null)
  {
    this.m_maths = new OfficeMaths((IOfficeMathEntity) this);
    this.m_ownerEntity = owner;
    this.IsDefault = true;
  }

  internal OfficeMathParagraph Clone()
  {
    OfficeMathParagraph owner = (OfficeMathParagraph) this.MemberwiseClone();
    owner.m_maths = new OfficeMaths((IOfficeMathEntity) owner);
    this.m_maths.CloneItemsTo(owner.m_maths);
    return owner;
  }

  internal override void Close()
  {
    if (this.m_maths != null)
    {
      this.m_maths.Close();
      this.m_maths = (OfficeMaths) null;
    }
    base.Close();
  }

  internal void SetOwner(object owner) => this.m_ownerEntity = owner;

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  internal bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key);
  }

  private object GetDefValue(int key)
  {
    return key == 77 ? (object) MathJustification.CenterGroup : (object) new ArgumentException("key has invalid value");
  }
}
