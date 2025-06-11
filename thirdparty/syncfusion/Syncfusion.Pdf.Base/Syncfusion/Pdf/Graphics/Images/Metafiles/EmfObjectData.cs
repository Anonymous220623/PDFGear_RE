// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.EmfObjectData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class EmfObjectData : IDisposable
{
  private const float UnitsInInch = 2540f;
  private EmfObjectCollection m_objects;
  private IntPtr m_handle;
  private Font m_font;
  private Brush m_brush;
  private Pen m_pen;
  private GraphicsPath m_path;
  private Bitmap m_image;
  private GraphicsState m_state;
  private System.Drawing.Graphics m_graphics;
  private bool m_bOpenPath;
  private float m_textAngle;
  private Stack m_contStack;
  private Bitmap m_bmp;
  private PointF m_defResolution;
  internal bool isNewPen;
  private int m_bkmode;

  public EmfObjectData(SizeF dpi)
  {
    this.m_bmp = new Bitmap(1, 1);
    this.m_defResolution = dpi.ToPointF();
  }

  public EmfObjectCollection SelectedObjects
  {
    get
    {
      if (this.m_objects == null)
        this.m_objects = new EmfObjectCollection();
      return this.m_objects;
    }
  }

  public IntPtr Handle
  {
    get
    {
      if (this.m_handle == IntPtr.Zero)
        this.m_handle = this.Graphics.GetHdc();
      return this.m_handle;
    }
  }

  public GraphicsState GraphicsState
  {
    get => this.m_state;
    set => this.m_state = value;
  }

  public Font Font
  {
    get
    {
      if (this.m_font == null)
        this.m_font = this.SelectedObjects.GetStockObject(STOCK.DEFAULT_GUI_FONT) as Font;
      return this.m_font;
    }
    set
    {
      if (this.m_font == value)
        return;
      this.m_font = value;
    }
  }

  public Brush Brush
  {
    get
    {
      if (this.m_brush == null)
        this.m_brush = this.SelectedObjects.GetStockObject(STOCK.DC_BRUSH) as Brush;
      return this.m_brush;
    }
    set
    {
      if (this.m_brush == value)
        return;
      this.m_brush = value;
    }
  }

  public Pen Pen
  {
    get
    {
      if (this.m_pen == null)
      {
        this.isNewPen = true;
        this.m_pen = this.SelectedObjects.GetStockObject(STOCK.DC_PEN) as Pen;
      }
      return this.m_pen;
    }
    set
    {
      this.isNewPen = false;
      if (this.m_pen == value)
        return;
      this.m_pen = value;
    }
  }

  public GraphicsPath Path
  {
    get => this.m_path;
    set
    {
      if (this.m_path == value)
        return;
      if (this.m_path != null && !this.SelectedObjects.IsStockObject((object) this.m_path))
        this.m_path.Dispose();
      this.m_path = value;
    }
  }

  public Bitmap Image
  {
    get => this.m_image;
    set
    {
      if (this.m_image == value)
        return;
      this.m_image = value;
    }
  }

  public System.Drawing.Graphics Graphics
  {
    get
    {
      if (this.m_graphics == null)
        this.m_graphics = System.Drawing.Graphics.FromImage((System.Drawing.Image) this.m_bmp);
      return this.m_graphics;
    }
  }

  public PointF CurrentPoint
  {
    get
    {
      POINT lpPoint1 = new POINT();
      bool ex = GdiApi.MoveToEx(this.Handle, 0, 0, ref lpPoint1);
      MetafileParser.CheckResult(ex);
      if (ex)
      {
        POINT lpPoint2 = new POINT();
        MetafileParser.CheckResult(GdiApi.MoveToEx(this.Handle, lpPoint1.x, lpPoint1.y, ref lpPoint2));
      }
      return (PointF) lpPoint1;
    }
    set
    {
      POINT lpPoint = new POINT();
      MetafileParser.CheckResult(GdiApi.MoveToEx(this.Handle, (int) value.X, (int) value.Y, ref lpPoint));
    }
  }

  public bool IsOpenPath
  {
    get => this.m_bOpenPath && this.Path != null;
    set
    {
      if (this.m_bOpenPath == value)
        return;
      this.m_bOpenPath = value;
    }
  }

  public AD_ANGLEDIRECTION ArcDirection
  {
    get
    {
      int arcDirection = GdiApi.GetArcDirection(this.Handle);
      if (arcDirection == 0)
        MetafileParser.CheckResult(false);
      return (AD_ANGLEDIRECTION) arcDirection;
    }
    set
    {
      if (GdiApi.SetArcDirection(this.Handle, (int) value) != 0)
        return;
      MetafileParser.CheckResult(false);
    }
  }

  public TA_TEXT_ALIGN TextAlign
  {
    get => (TA_TEXT_ALIGN) GdiApi.GetTextAlign(this.Handle);
    set => GdiApi.SetTextAlign(this.Handle, (int) value);
  }

  internal int BACKGROUNDMODE
  {
    get => this.m_bkmode;
    set => this.m_bkmode = value;
  }

  public Color ForeColor
  {
    get => ColorTranslator.FromWin32(GdiApi.GetTextColor(this.Handle));
    set => GdiApi.SetTextColor(this.Handle, ColorTranslator.ToWin32(value));
  }

  public Color BackColor
  {
    get => ColorTranslator.FromWin32(GdiApi.GetBkColor(this.Handle));
    set => GdiApi.SetBkColor(this.Handle, ColorTranslator.ToWin32(value));
  }

  public FillMode FillMode
  {
    get
    {
      FillMode fillMode = FillMode.Winding;
      int polyFillMode = GdiApi.GetPolyFillMode(this.Handle);
      if (polyFillMode > 0)
      {
        int num;
        fillMode = (FillMode) (num = polyFillMode - 1);
      }
      else
        MetafileParser.CheckResult(false);
      return fillMode;
    }
    set
    {
      if (GdiApi.SetPolyFillMode(this.Handle, (int) (value + 1)) != 0)
        return;
      MetafileParser.CheckResult(false);
    }
  }

  public PointF Resolution => this.m_defResolution;

  public float TextAngle
  {
    get => this.m_textAngle;
    set
    {
      if ((double) this.m_textAngle == (double) value)
        return;
      this.m_textAngle = value;
    }
  }

  private Stack ContextStack
  {
    get
    {
      if (this.m_contStack == null)
        this.m_contStack = new Stack();
      return this.m_contStack;
    }
  }

  public void Dispose()
  {
    if (this.Font != null && !this.SelectedObjects.IsStockObject((object) this.Font))
      this.Font.Dispose();
    if (this.Pen != null && !this.SelectedObjects.IsStockObject((object) this.Pen))
      this.Pen.Dispose();
    if (this.Brush != null && !this.SelectedObjects.IsStockObject((object) this.Brush))
      this.Brush.Dispose();
    if (this.Path != null && !this.SelectedObjects.IsStockObject((object) this.Path))
      this.Path.Dispose();
    if (this.m_handle != IntPtr.Zero && this.m_graphics != null)
    {
      this.m_graphics.ReleaseHdc(this.m_handle);
      this.m_graphics.Dispose();
      this.m_graphics = (System.Drawing.Graphics) null;
      this.m_bmp.Dispose();
    }
    if (this.m_bmp != null)
    {
      this.m_bmp.Dispose();
      this.m_bmp = (Bitmap) null;
    }
    if (this.m_contStack != null)
    {
      this.m_contStack.Clear();
      this.m_contStack = (Stack) null;
    }
    this.DisposeSelectedObjects();
    this.m_font = (Font) null;
    this.m_pen = (Pen) null;
    this.m_brush = (Brush) null;
    this.m_path = (GraphicsPath) null;
  }

  public void SelectObject(object obj)
  {
    switch (obj)
    {
      case Pen pen:
        this.Pen = pen;
        break;
      case Brush brush:
        this.Brush = brush;
        break;
      case FontEx fontEx:
        this.Font = fontEx.Font;
        this.TextAngle = fontEx.Angle;
        break;
      case Font font:
        this.Font = font;
        this.TextAngle = 0.0f;
        break;
    }
  }

  public void DeleteObject(object obj)
  {
    switch (obj)
    {
      case Pen pen:
        pen.Dispose();
        break;
      case Brush brush:
        brush.Dispose();
        break;
      case FontEx fontEx:
        fontEx.Dispose();
        break;
      case Font font:
        font.Dispose();
        break;
    }
  }

  public void Save()
  {
    EmfObjectData data = new EmfObjectData(new SizeF(this.m_defResolution.X, this.m_defResolution.Y));
    this.CopyTo(data);
    this.ContextStack.Push((object) data);
  }

  public void Restore(int index)
  {
    int num = index >= 0 ? Math.Max(this.ContextStack.Count - index, 0) : Math.Min(-index, this.ContextStack.Count);
    if (num <= 0)
      return;
    EmfObjectData emfObjectData = (EmfObjectData) null;
    while (num-- != 0)
      emfObjectData = (EmfObjectData) this.ContextStack.Pop();
    emfObjectData.CopyTo(this);
  }

  private void CopyTo(EmfObjectData data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data.m_objects = this.m_objects;
    data.m_handle = this.m_handle;
    data.m_font = this.m_font;
    data.m_brush = this.m_brush;
    data.m_pen = this.m_pen;
    data.m_path = this.m_path;
    data.m_image = this.m_image;
    data.m_state = this.m_state;
    data.m_graphics = this.m_graphics;
    data.m_bOpenPath = this.m_bOpenPath;
    data.m_textAngle = this.m_textAngle;
    data.m_contStack = this.m_contStack;
  }

  private void DisposeSelectedObjects()
  {
    if (this.SelectedObjects.CreatedGraphicObjects.Count > 0)
    {
      foreach (object key in (IEnumerable) this.SelectedObjects.CreatedGraphicObjects.Keys)
      {
        object createdGraphicObject = this.SelectedObjects.CreatedGraphicObjects[key];
        if (createdGraphicObject != null)
          this.DeleteObject(createdGraphicObject);
      }
    }
    this.SelectedObjects.Clear();
  }
}
