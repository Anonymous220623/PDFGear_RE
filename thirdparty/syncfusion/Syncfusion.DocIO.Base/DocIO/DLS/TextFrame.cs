// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextFrame
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextFrame
{
  internal const byte ShapeAutoFitKey = 0;
  internal const byte NoAutoFitKey = 1;
  internal const byte NormalAutoFitKey = 2;
  private bool m_bNoWrap;
  private bool m_spAutoFit;
  private bool m_normAutoFit;
  private bool m_noAutoFit;
  private TextDirection m_TextDirection;
  private byte m_flag;
  private Shape m_shape;
  private ChildShape m_childShape;
  private float m_widthRelativePercent;
  private float m_heightRelativePercent;
  private WidthOrigin m_widthRelation = WidthOrigin.Page;
  private HeightOrigin m_heightRelation = HeightOrigin.Page;
  protected Dictionary<int, object> m_propertiesHash;
  private VerticalAlignment m_TextVerticalAlignment;
  private float m_HorizontalRelativePercent = float.MinValue;
  private float m_VerticalRelativePercent = float.MinValue;
  internal InternalMargin m_intMargin;

  internal bool HasInternalMargin
  {
    get => ((int) this.m_flag & 1) != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 254 | (value ? 1 : 0));
  }

  internal bool Upright
  {
    get => ((int) this.m_flag & 4) >> 2 != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 251 | (value ? 1 : 0) << 2);
  }

  public TextDirection TextDirection
  {
    get => this.m_TextDirection;
    set => this.m_TextDirection = value;
  }

  public VerticalAlignment TextVerticalAlignment
  {
    get => this.m_TextVerticalAlignment;
    set => this.m_TextVerticalAlignment = value;
  }

  internal float WidthRelativePercent
  {
    get => this.m_widthRelativePercent;
    set => this.m_widthRelativePercent = value;
  }

  internal float HeightRelativePercent
  {
    get => this.m_heightRelativePercent;
    set => this.m_heightRelativePercent = value;
  }

  internal WidthOrigin WidthOrigin
  {
    get => this.m_widthRelation;
    set => this.m_widthRelation = value;
  }

  internal HeightOrigin HeightOrigin
  {
    get => this.m_heightRelation;
    set => this.m_heightRelation = value;
  }

  internal float HorizontalRelativePercent
  {
    get => this.m_HorizontalRelativePercent;
    set => this.m_HorizontalRelativePercent = value;
  }

  internal float VerticalRelativePercent
  {
    get => this.m_VerticalRelativePercent;
    set => this.m_VerticalRelativePercent = value;
  }

  internal InternalMargin InternalMargin
  {
    get
    {
      if (this.m_intMargin == null)
        this.m_intMargin = new InternalMargin();
      return this.m_intMargin;
    }
  }

  internal bool NoWrap
  {
    get => this.m_bNoWrap;
    set => this.m_bNoWrap = value;
  }

  internal bool NoAutoFit
  {
    get => this.HasKey(1) ? (bool) this.PropertiesHash[1] : this.m_noAutoFit;
    set
    {
      this.m_noAutoFit = value;
      this.SetKeyValue(1, (object) value);
    }
  }

  internal bool NormalAutoFit
  {
    get => this.HasKey(2) ? (bool) this.PropertiesHash[2] : this.m_normAutoFit;
    set
    {
      this.m_normAutoFit = value;
      this.SetKeyValue(2, (object) value);
    }
  }

  internal bool ShapeAutoFit
  {
    get => this.HasKey(0) ? (bool) this.PropertiesHash[0] : this.m_spAutoFit;
    set
    {
      this.m_spAutoFit = value;
      this.SetKeyValue(0, (object) value);
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

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }

  internal bool HasKey(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  private void SetKeyValue(int propKey, object value) => this[propKey] = value;

  internal TextFrame(Shape shape) => this.m_shape = shape;

  internal TextFrame(ChildShape childShape) => this.m_childShape = childShape;
}
