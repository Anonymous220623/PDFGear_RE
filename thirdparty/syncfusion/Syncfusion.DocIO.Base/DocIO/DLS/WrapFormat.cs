// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WrapFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WrapFormat
{
  private bool m_AllowOverlap;
  private float m_DistanceBottom;
  private float m_DistanceLeft;
  private float m_DistanceRight;
  private float m_DistanceTop;
  private TextWrappingType m_TextWrappingType;
  private TextWrappingStyle m_TextWrappingStyle;
  private WrapPolygon m_wrapPolygon;
  internal bool IsWrappingBoundsAdded;
  internal int WrapCollectionIndex = -1;
  private byte m_bFlags;

  public bool AllowOverlap
  {
    get => this.m_AllowOverlap;
    set => this.m_AllowOverlap = value;
  }

  public float DistanceBottom
  {
    get
    {
      return (double) this.m_DistanceBottom < 0.0 || (double) this.m_DistanceBottom > 1584.0 ? 0.0f : this.m_DistanceBottom;
    }
    set => this.m_DistanceBottom = value;
  }

  public float DistanceLeft
  {
    get
    {
      return (double) this.m_DistanceLeft < 0.0 || (double) this.m_DistanceLeft > 1584.0 ? 0.0f : this.m_DistanceLeft;
    }
    set => this.m_DistanceLeft = value;
  }

  public float DistanceRight
  {
    get
    {
      return (double) this.m_DistanceRight < 0.0 || (double) this.m_DistanceRight > 1584.0 ? 0.0f : this.m_DistanceRight;
    }
    set => this.m_DistanceRight = value;
  }

  public float DistanceTop
  {
    get
    {
      return (double) this.m_DistanceTop < 0.0 || (double) this.m_DistanceTop > 1584.0 ? 0.0f : this.m_DistanceTop;
    }
    set => this.m_DistanceTop = value;
  }

  internal bool IsBelowText
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
      if (value && this.TextWrappingStyle == TextWrappingStyle.InFrontOfText)
      {
        this.m_TextWrappingStyle = TextWrappingStyle.Behind;
      }
      else
      {
        if (value || this.TextWrappingStyle != TextWrappingStyle.Behind)
          return;
        this.m_TextWrappingStyle = TextWrappingStyle.InFrontOfText;
      }
    }
  }

  public TextWrappingType TextWrappingType
  {
    get => this.m_TextWrappingType;
    set => this.m_TextWrappingType = value;
  }

  public TextWrappingStyle TextWrappingStyle
  {
    get => this.m_TextWrappingStyle;
    set
    {
      this.m_TextWrappingStyle = value;
      if (this.m_TextWrappingStyle == TextWrappingStyle.Behind)
        this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | 1);
      else
        this.m_bFlags &= (byte) 254;
    }
  }

  internal void SetTextWrappingStyleValue(TextWrappingStyle textWrappingStyle)
  {
    this.m_TextWrappingStyle = textWrappingStyle;
  }

  internal WrapPolygon WrapPolygon
  {
    get
    {
      if (this.m_wrapPolygon == null)
      {
        this.m_wrapPolygon = new WrapPolygon();
        this.m_wrapPolygon.Edited = false;
        this.m_wrapPolygon.Vertices.Add(new PointF(0.0f, 0.0f));
        this.m_wrapPolygon.Vertices.Add(new PointF(0.0f, 21600f));
        this.m_wrapPolygon.Vertices.Add(new PointF(21600f, 21600f));
        this.m_wrapPolygon.Vertices.Add(new PointF(21600f, 0.0f));
        this.m_wrapPolygon.Vertices.Add(new PointF(0.0f, 0.0f));
      }
      return this.m_wrapPolygon;
    }
    set => this.m_wrapPolygon = value;
  }

  internal void Close()
  {
    if (this.m_wrapPolygon == null)
      return;
    this.m_wrapPolygon.Close();
    this.m_wrapPolygon = (WrapPolygon) null;
  }
}
