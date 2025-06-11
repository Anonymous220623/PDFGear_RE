// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.UnitsConvertor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Layouting;

internal class UnitsConvertor
{
  internal const int STANDART_DPI = 96 /*0x60*/;
  private Graphics _graph;
  private double[] m_Proportions;
  [ThreadStatic]
  private static UnitsConvertor m_instance;

  private double[] Proportions
  {
    get
    {
      if (this.m_Proportions == null)
        this.InitDefProporsions();
      return this.m_Proportions;
    }
  }

  public Graphics EmptyGraphics
  {
    get
    {
      if (this._graph == null)
        this._graph = Graphics.FromImage((Image) new Bitmap(1, 1));
      return this._graph;
    }
  }

  public static UnitsConvertor Instance
  {
    get
    {
      if (UnitsConvertor.m_instance == null)
        UnitsConvertor.m_instance = new UnitsConvertor();
      return UnitsConvertor.m_instance;
    }
  }

  public UnitsConvertor() => this.UpdateProportions(this.EmptyGraphics);

  public UnitsConvertor(Graphics g) => this.UpdateProportions(g);

  public double ConvertUnits(double value, PrintUnits from, PrintUnits to)
  {
    return from == to ? value : this.ConvertFromPixels(this.ConvertToPixels(value, from), to);
  }

  public float ConvertToPixels(float value, PrintUnits from)
  {
    return from == PrintUnits.Pixel ? value : value * (float) this.m_Proportions[(int) from];
  }

  public double ConvertToPixels(double value, PrintUnits from)
  {
    return from == PrintUnits.Pixel ? value : value * this.m_Proportions[(int) from];
  }

  public RectangleF ConvertToPixels(RectangleF rect, PrintUnits from)
  {
    return new RectangleF(this.ConvertToPixels(rect.X, from), this.ConvertToPixels(rect.Y, from), this.ConvertToPixels(rect.Width, from), this.ConvertToPixels(rect.Height, from));
  }

  public PointF ConvertToPixels(PointF point, PrintUnits from)
  {
    return new PointF(this.ConvertToPixels(point.X, from), this.ConvertToPixels(point.Y, from));
  }

  public SizeF ConvertToPixels(SizeF size, PrintUnits from)
  {
    return new SizeF(this.ConvertToPixels(size.Width, from), this.ConvertToPixels(size.Height, from));
  }

  public float ConvertFromPixels(float value, PrintUnits to)
  {
    return to == PrintUnits.Pixel ? value : value / (float) this.m_Proportions[(int) to];
  }

  public double ConvertFromPixels(double value, PrintUnits to)
  {
    return to == PrintUnits.Pixel ? value : value / this.m_Proportions[(int) to];
  }

  public RectangleF ConvertFromPixels(RectangleF rect, PrintUnits to)
  {
    return new RectangleF(this.ConvertFromPixels(rect.X, to), this.ConvertFromPixels(rect.Y, to), this.ConvertFromPixels(rect.Width, to), this.ConvertFromPixels(rect.Height, to));
  }

  public PointF ConvertFromPixels(PointF point, PrintUnits to)
  {
    return new PointF(this.ConvertFromPixels(point.X, to), this.ConvertFromPixels(point.Y, to));
  }

  public SizeF ConvertFromPixels(Size size, PrintUnits to)
  {
    return new SizeF(this.ConvertFromPixels((float) size.Width, to), this.ConvertFromPixels((float) size.Height, to));
  }

  public SizeF ConvertFromPixels(SizeF size, PrintUnits to)
  {
    return new SizeF(this.ConvertFromPixels(size.Width, to), this.ConvertFromPixels(size.Height, to));
  }

  public float ConvertToPixels(float value, PrintUnits from, float dpi)
  {
    if (from == PrintUnits.Pixel)
      return value;
    double[] proporsion = this.GetProporsion(dpi);
    return value * (float) proporsion[(int) from];
  }

  public double ConvertToPixels(double value, PrintUnits from, float dpi)
  {
    if (from == PrintUnits.Pixel)
      return value;
    double[] proporsion = this.GetProporsion(dpi);
    return value * proporsion[(int) from];
  }

  public float ConvertFromPixels(float value, PrintUnits to, float dpi)
  {
    if (to == PrintUnits.Pixel)
      return value;
    double[] proporsion = this.GetProporsion(dpi);
    return value / (float) proporsion[(int) to];
  }

  public double ConvertFromPixels(double value, PrintUnits to, float dpi)
  {
    if (to == PrintUnits.Pixel)
      return value;
    double[] proporsion = this.GetProporsion(dpi);
    return value / proporsion[(int) to];
  }

  public SizeF ConvertFromPixels(SizeF size, PrintUnits to, float dpi)
  {
    return new SizeF(this.ConvertFromPixels(size.Width, to, dpi), this.ConvertFromPixels(size.Height, to, dpi));
  }

  private double[] GetProporsion(float dpi)
  {
    return new double[7]
    {
      (double) dpi / 75.0,
      (double) dpi / 300.0,
      (double) dpi,
      (double) dpi / 25.399999618530273,
      (double) dpi / 2.5399999618530273,
      1.0,
      (double) dpi / 72.0
    };
  }

  internal void InitDefProporsions()
  {
    double num = 96.0;
    this.m_Proportions = new double[8]
    {
      num / 75.0,
      num / 300.0,
      num,
      num / 25.399999618530273,
      num / 2.5399999618530273,
      1.0,
      num / 72.0,
      num / 914400.0
    };
  }

  internal static void Close()
  {
    if (UnitsConvertor.m_instance == null)
      return;
    UnitsConvertor.m_instance = (UnitsConvertor) null;
  }

  private void UpdateProportions(Graphics g)
  {
    if (g == null)
      throw new ArgumentNullException(nameof (g));
    PointF[] pts = new PointF[1]{ new PointF(1f, 1f) };
    GraphicsContainer container = g.BeginContainer(new RectangleF(0.0f, 0.0f, 1f, 1f), new RectangleF(0.0f, 0.0f, 1f, 1f), GraphicsUnit.Pixel);
    g.PageUnit = GraphicsUnit.Inch;
    g.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, pts);
    g.EndContainer(container);
    double x = (double) pts[0].X;
    this.m_Proportions = new double[8]
    {
      x / 75.0,
      x / 300.0,
      x,
      x / 25.399999618530273,
      x / 2.5399999618530273,
      1.0,
      x / 72.0,
      x / 914400.0
    };
  }
}
