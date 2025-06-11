// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.GradientFill
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class GradientFill
{
  private FlipOrientation m_flip;
  private List<GradientStop> m_gradientStops;
  private LinearGradient m_linearGradient;
  private PathGradient m_pathGradient;
  private TileRectangle m_tileRectangle;
  private string m_Focus;
  private byte m_bFlags;

  internal string Focus
  {
    get => this.m_Focus;
    set => this.m_Focus = value;
  }

  internal bool RotateWithShape
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal FlipOrientation Flip
  {
    get => this.m_flip;
    set => this.m_flip = value;
  }

  internal List<GradientStop> GradientStops
  {
    get
    {
      if (this.m_gradientStops == null)
        this.m_gradientStops = new List<GradientStop>();
      return this.m_gradientStops;
    }
  }

  internal LinearGradient LinearGradient
  {
    get => this.m_linearGradient;
    set => this.m_linearGradient = value;
  }

  internal PathGradient PathGradient
  {
    get => this.m_pathGradient;
    set => this.m_pathGradient = value;
  }

  internal TileRectangle TileRectangle
  {
    get
    {
      if (this.m_tileRectangle == null)
        this.m_tileRectangle = new TileRectangle();
      return this.m_tileRectangle;
    }
  }

  internal bool IsEmptyElement
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal GradientFill()
  {
  }

  internal GradientFill Clone()
  {
    GradientFill gradientFill = (GradientFill) this.MemberwiseClone();
    if (this.GradientStops != null && this.GradientStops.Count > 0)
    {
      gradientFill.m_gradientStops = new List<GradientStop>();
      foreach (GradientStop gradientStop in this.GradientStops)
        gradientFill.GradientStops.Add(gradientStop.Clone());
    }
    if (this.LinearGradient != null)
      gradientFill.LinearGradient = this.LinearGradient.Clone();
    if (this.PathGradient != null)
      gradientFill.PathGradient = this.PathGradient.Clone();
    if (this.TileRectangle != null)
      gradientFill.m_tileRectangle = this.TileRectangle.Clone();
    return gradientFill;
  }

  internal void Close()
  {
    if (this.m_gradientStops != null)
    {
      this.m_gradientStops.Clear();
      this.m_gradientStops = (List<GradientStop>) null;
    }
    this.m_linearGradient = (LinearGradient) null;
    this.m_pathGradient = (PathGradient) null;
    this.m_tileRectangle = (TileRectangle) null;
  }
}
