// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.InternalMargin
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class InternalMargin : OwnerHolder
{
  internal const float DEF_HORIZMARGIN = 7.087f;
  internal const float DEF_VERTMARGIN = 3.685f;
  internal const byte LeftKey = 0;
  internal const byte RightKey = 1;
  internal const byte TopKey = 2;
  internal const byte BottomKey = 3;
  private float m_intLeftMarg;
  private float m_intRightMarg;
  private float m_intTopMarg;
  private float m_intBottomMarg;
  protected Dictionary<int, object> m_propertiesHash;

  public float Left
  {
    get => this.HasKey(0) ? (float) this.PropertiesHash[0] : this.m_intLeftMarg;
    set
    {
      if ((double) value < 0.0)
        throw new ArgumentOutOfRangeException(nameof (Left), "Internal left margin must be higher than 0");
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.InternalMargin.Left = value;
      this.m_intLeftMarg = value;
      this.SetKeyValue(0, (object) value);
    }
  }

  public float Right
  {
    get => this.HasKey(1) ? (float) this.PropertiesHash[1] : this.m_intRightMarg;
    set
    {
      if ((double) value < 0.0)
        throw new ArgumentOutOfRangeException(nameof (Right), "Internal right margin must be higher than 0");
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.InternalMargin.Right = value;
      this.m_intRightMarg = value;
      this.SetKeyValue(1, (object) value);
    }
  }

  public float Top
  {
    get => this.HasKey(2) ? (float) this.PropertiesHash[2] : this.m_intTopMarg;
    set
    {
      if ((double) value < 0.0)
        throw new ArgumentOutOfRangeException(nameof (Top), "Internal top margin must be higher than 0");
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.InternalMargin.Top = value;
      this.m_intTopMarg = value;
      this.SetKeyValue(2, (object) value);
    }
  }

  public float Bottom
  {
    get => this.HasKey(3) ? (float) this.PropertiesHash[3] : this.m_intBottomMarg;
    set
    {
      if ((double) value < 0.0)
        throw new ArgumentOutOfRangeException(nameof (Bottom), "Internal bottom margin must be higher than 0");
      if (this.Document != null && !this.Document.IsOpening && this.OwnerBase is WTextBox && (this.OwnerBase as WTextBox).IsShape)
        (this.OwnerBase as WTextBox).Shape.TextFrame.InternalMargin.Bottom = value;
      this.m_intBottomMarg = value;
      this.SetKeyValue(3, (object) value);
    }
  }

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }

  public InternalMargin()
  {
    this.m_intLeftMarg = 7.087f;
    this.m_intRightMarg = 7.087f;
    this.m_intTopMarg = 3.685f;
    this.m_intBottomMarg = 3.685f;
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

  internal bool HasKey(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  internal void SetKeyValue(int propKey, object value) => this[propKey] = value;

  internal InternalMargin Clone() => (InternalMargin) this.MemberwiseClone();
}
