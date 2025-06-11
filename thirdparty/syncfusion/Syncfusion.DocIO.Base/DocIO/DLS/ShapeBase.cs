// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ShapeBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class ShapeBase : ShapeCommon
{
  private const byte LeftEdgeExtentKey = 1;
  private const byte RightEdgeExtentKey = 2;
  private const byte TopEdgeExtentKey = 3;
  private const byte BottomEdgeExtentKey = 4;
  private int m_ZOrderPosition = int.MaxValue;
  private HorizontalOrigin m_HorizontalOrigin;
  private VerticalOrigin m_VerticalOrigin;
  private HorizontalOrigin m_relHorzOrigin;
  private VerticalOrigin m_relVertOrigin;
  private HorizontalOrigin m_relWidthHorzOrigin;
  private VerticalOrigin m_relHeightVertOrigin;
  internal WrapFormat m_WrapFormat;
  private float m_relHorzpos;
  private float m_relvertpos;
  private float m_relHeight;
  private float m_relWidth;
  private ShapeHorizontalAlignment m_horAlignment;
  private ShapeVerticalAlignment m_vertAlignment;
  private float m_horizPosition;
  private float m_vertPosition;
  private byte m_bFlags;
  internal byte m_bFlags1 = 24;

  public HorizontalOrigin HorizontalOrigin
  {
    get => this.m_HorizontalOrigin;
    set => this.m_HorizontalOrigin = value;
  }

  internal HorizontalOrigin RelativeWidthHorizontalOrigin
  {
    get => this.m_relWidthHorzOrigin;
    set => this.m_relWidthHorzOrigin = value;
  }

  internal VerticalOrigin RelativeHeightVerticalOrigin
  {
    get => this.m_relHeightVertOrigin;
    set => this.m_relHeightVertOrigin = value;
  }

  internal HorizontalOrigin RelativeHorizontalOrigin
  {
    get => this.m_relHorzOrigin;
    set => this.m_relHorzOrigin = value;
  }

  internal VerticalOrigin RelativeVerticalOrigin
  {
    get => this.m_relVertOrigin;
    set => this.m_relVertOrigin = value;
  }

  public ShapeHorizontalAlignment HorizontalAlignment
  {
    get => this.m_horAlignment;
    set => this.m_horAlignment = value;
  }

  public float HorizontalPosition
  {
    get => this.m_horizPosition;
    set => this.m_horizPosition = value;
  }

  internal float RelativeHorizontalPosition
  {
    get => this.m_relHorzpos;
    set => this.m_relHorzpos = value;
  }

  internal float RelativeVerticalPosition
  {
    get => this.m_relvertpos;
    set => this.m_relvertpos = value;
  }

  internal float RelativeHeight
  {
    get => this.m_relHeight;
    set => this.m_relHeight = value;
  }

  internal float RelativeWidth
  {
    get => this.m_relWidth;
    set => this.m_relWidth = value;
  }

  internal bool IsRelativeVerticalPosition
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsRelativeHorizontalPosition
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool IsRelativeHeight
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsRelativeWidth
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  public VerticalOrigin VerticalOrigin
  {
    get => this.m_VerticalOrigin;
    set => this.m_VerticalOrigin = value;
  }

  public ShapeVerticalAlignment VerticalAlignment
  {
    get => this.m_vertAlignment;
    set => this.m_vertAlignment = value;
  }

  public float VerticalPosition
  {
    get => this.m_vertPosition;
    set => this.m_vertPosition = value;
  }

  public WrapFormat WrapFormat
  {
    get
    {
      if (this.m_WrapFormat == null)
        this.m_WrapFormat = new WrapFormat();
      return this.m_WrapFormat;
    }
    set => this.m_WrapFormat = value;
  }

  internal int ZOrderPosition
  {
    get => this.m_ZOrderPosition;
    set => this.m_ZOrderPosition = value;
  }

  public bool IsBelowText
  {
    get => this.WrapFormat.IsBelowText;
    set => this.WrapFormat.IsBelowText = value;
  }

  internal bool LayoutInCell
  {
    get => ((int) this.m_bFlags1 & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 239 | (value ? 1 : 0) << 4);
  }

  public bool LockAnchor
  {
    get => ((int) this.m_bFlags1 & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 223 | (value ? 1 : 0) << 5);
  }

  internal new bool IsCloned
  {
    get => ((int) this.m_bFlags1 & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 191 | (value ? 1 : 0) << 6);
  }

  public bool Visible
  {
    get => ((int) this.m_bFlags1 & 8) >> 3 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 247 | (value ? 1 : 0) << 3);
  }

  internal float LeftEdgeExtent
  {
    get => !this.PropertiesHash.ContainsKey(1) ? 0.0f : (float) this.PropertiesHash[1];
    set => this.SetKeyValue(1, (object) value);
  }

  internal float TopEdgeExtent
  {
    get => !this.PropertiesHash.ContainsKey(3) ? 0.0f : (float) this.PropertiesHash[3];
    set => this.SetKeyValue(3, (object) value);
  }

  internal float RightEdgeExtent
  {
    get => !this.PropertiesHash.ContainsKey(2) ? 0.0f : (float) this.PropertiesHash[2];
    set => this.SetKeyValue(2, (object) value);
  }

  internal float BottomEdgeExtent
  {
    get => !this.PropertiesHash.ContainsKey(4) ? 0.0f : (float) this.PropertiesHash[4];
    set => this.SetKeyValue(4, (object) value);
  }

  internal ShapeBase(WordDocument doc)
    : base(doc)
  {
  }

  protected override object CloneImpl()
  {
    ShapeBase shapeBase = (ShapeBase) base.CloneImpl();
    if (this.WrapFormat != null && this.WrapFormat.WrapPolygon != null)
      shapeBase.WrapFormat.WrapPolygon = this.WrapFormat.WrapPolygon.Clone();
    return (object) shapeBase;
  }

  internal override void Close()
  {
    if (this.m_WrapFormat != null)
    {
      this.m_WrapFormat.Close();
      this.m_WrapFormat = (WrapFormat) null;
    }
    if (this.m_docxProps != null)
    {
      foreach (Stream stream in this.m_docxProps.Values)
        stream.Close();
      this.m_docxProps.Clear();
      this.m_docxProps = (Dictionary<string, Stream>) null;
    }
    base.Close();
  }
}
