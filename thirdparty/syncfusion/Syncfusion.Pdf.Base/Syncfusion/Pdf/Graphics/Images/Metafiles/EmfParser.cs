// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.EmfParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class EmfParser : MetafileParser
{
  private const int PointsPerInch = 72;
  internal const float DegreeCount = 180f;
  private static object syncLock = new object();
  private EmfObjectData m_objects;
  private MetafileType m_type;
  internal float TextAngle;
  internal Font m_selectedFont;
  internal float FontTextAngle;
  private int m_selectedId;
  private Dictionary<int, Pen> m_selectedPen = new Dictionary<int, Pen>();
  private Dictionary<int, FontEx> m_fontAngleWithID = new Dictionary<int, FontEx>();
  private Dictionary<int, Brush> m_selectedBrush = new Dictionary<int, Brush>();
  private SizeF m_emfScalingFactor = SizeF.Empty;
  private SIZE newWindowSize;
  private bool m_isBKMode;
  private bool m_isWindowPort;
  private int m_mapMode;
  private PointF[] m_previousClipBounds;
  private PointF[] m_pattenPoints;
  private bool m_clipReset;
  private List<RectangleF> m_clipPoints = new List<RectangleF>();
  private PointF[] m_polygonPoints;
  private bool ignorePrivateData;

  public override MetafileType Type => this.m_type;

  private EmfObjectData Objects => this.m_objects;

  private TextRegionManager TextRegions => this.Context as TextRegionManager;

  private ImageRegionManager ImageRegions => this.ImageContext as ImageRegionManager;

  public EmfParser(MetafileType type, SizeF dpi)
  {
    this.m_type = type;
    this.Context = (object) new TextRegionManager();
    this.ImageContext = (object) new ImageRegionManager();
    this.m_objects = new EmfObjectData(dpi);
  }

  internal EmfParser(SizeF dpi)
  {
    this.Context = (object) new TextRegionManager();
    this.ImageContext = (object) new ImageRegionManager();
    this.m_objects = new EmfObjectData(dpi);
  }

  internal EmfParser(PdfEmfRenderer renderer)
    : base(renderer)
  {
  }

  public override void Dispose()
  {
    base.Dispose();
    if (this.m_objects != null)
    {
      this.m_objects.Dispose();
      this.m_objects = (EmfObjectData) null;
    }
    this.m_clipPoints.Clear();
  }

  protected override System.Drawing.Graphics.EnumerateMetafileProc CreateParsingHandler()
  {
    return new System.Drawing.Graphics.EnumerateMetafileProc(this.EnumerateMetafile);
  }

  internal bool EnumerateMetafile(
    EmfPlusRecordType recordType,
    int flags,
    int dataSize,
    IntPtr ptrData,
    PlayRecordCallback callbackData)
  {
    this.Renderer.m_recordType = recordType;
    if (recordType != EmfPlusRecordType.EmfExtTextOutW)
      this.Renderer.m_EMFState = false;
    lock (EmfParser.syncLock)
    {
      try
      {
        switch (recordType)
        {
          case EmfPlusRecordType.EmfHeader:
            this.Header(this.GetData(ptrData, dataSize));
            break;
          case EmfPlusRecordType.EmfPolyBezier:
            this.PolyBezier(ptrData);
            break;
          case EmfPlusRecordType.EmfPolygon:
            this.Polygon(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyline:
            this.Polyline(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyBezierTo:
            this.PolyBezierTo(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyLineTo:
            this.PolyLineTo(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyPolyline:
            this.PolyPolyline(this.GetData(ptrData, dataSize), true);
            break;
          case EmfPlusRecordType.EmfPolyPolygon:
            this.PolyPolygon(this.GetData(ptrData, dataSize), true);
            break;
          case EmfPlusRecordType.EmfSetWindowExtEx:
            this.SetWindowExtEx(ptrData);
            break;
          case EmfPlusRecordType.EmfSetWindowOrgEx:
            this.SetWindowOrgEx(ptrData);
            break;
          case EmfPlusRecordType.EmfSetViewportExtEx:
            this.SetViewportExtEx(ptrData);
            break;
          case EmfPlusRecordType.EmfSetViewportOrgEx:
            this.SetViewportOrgEx(ptrData);
            break;
          case EmfPlusRecordType.EmfEof:
            this.EndOfFile();
            break;
          case EmfPlusRecordType.EmfSetPixelV:
            this.SetPixelV(ptrData);
            break;
          case EmfPlusRecordType.EmfSetMapMode:
            this.SetMapMode(ptrData);
            break;
          case EmfPlusRecordType.EmfSetBkMode:
            this.SetBkMode(ptrData);
            break;
          case EmfPlusRecordType.EmfSetPolyFillMode:
            this.SetPolyFillMode(ptrData);
            break;
          case EmfPlusRecordType.EmfSetROP2:
            this.SetROP2(ptrData);
            break;
          case EmfPlusRecordType.EmfSetStretchBltMode:
            this.SetStretchBltMode(ptrData);
            break;
          case EmfPlusRecordType.EmfSetTextAlign:
            this.SetTextAlign(ptrData);
            break;
          case EmfPlusRecordType.EmfSetTextColor:
            this.SetTextColor(ptrData);
            break;
          case EmfPlusRecordType.EmfSetBkColor:
            this.SetBkColor(ptrData);
            break;
          case EmfPlusRecordType.EmfOffsetClipRgn:
            this.OffsetClipRgn(ptrData);
            break;
          case EmfPlusRecordType.EmfMoveToEx:
            this.MoveToEx(ptrData);
            break;
          case EmfPlusRecordType.EmfSetMetaRgn:
            this.SetMetaRgn(ptrData);
            break;
          case EmfPlusRecordType.EmfExcludeClipRect:
            this.ExcludeClipRect(ptrData);
            break;
          case EmfPlusRecordType.EmfIntersectClipRect:
            this.IntersectClipRect(ptrData);
            break;
          case EmfPlusRecordType.EmfScaleViewportExtEx:
            this.ScaleViewportExtEx(ptrData);
            break;
          case EmfPlusRecordType.EmfScaleWindowExtEx:
            this.ScaleWindowExtEx(ptrData);
            break;
          case EmfPlusRecordType.EmfSaveDC:
            this.SaveDC(this.GetData(ptrData, dataSize));
            break;
          case EmfPlusRecordType.EmfRestoreDC:
            this.RestoreDC(this.GetData(ptrData, dataSize));
            break;
          case EmfPlusRecordType.EmfSetWorldTransform:
            this.SetWorldTransform(ptrData);
            break;
          case EmfPlusRecordType.EmfModifyWorldTransform:
            this.ModifyWorldTransform(ptrData);
            break;
          case EmfPlusRecordType.EmfSelectObject:
            this.SelectObject(this.GetData(ptrData, dataSize));
            break;
          case EmfPlusRecordType.EmfCreatePen:
            this.CreatePen(ptrData);
            break;
          case EmfPlusRecordType.EmfCreateBrushIndirect:
            this.CreateBrushIndirect(ptrData);
            break;
          case EmfPlusRecordType.EmfDeleteObject:
            this.DeleteObject(this.GetData(ptrData, dataSize));
            break;
          case EmfPlusRecordType.EmfAngleArc:
            this.AngleArc(ptrData);
            break;
          case EmfPlusRecordType.EmfEllipse:
            this.Ellipse(ptrData);
            break;
          case EmfPlusRecordType.EmfRectangle:
            this.RectangleEx(ptrData);
            break;
          case EmfPlusRecordType.EmfRoundRect:
            this.RoundRect(ptrData);
            break;
          case EmfPlusRecordType.EmfRoundArc:
            this.ArcTo(ptrData, false);
            break;
          case EmfPlusRecordType.EmfChord:
            this.Chord(ptrData);
            break;
          case EmfPlusRecordType.EmfPie:
            this.Pie(ptrData);
            break;
          case EmfPlusRecordType.EmfLineTo:
            this.LineTo(ptrData);
            break;
          case EmfPlusRecordType.EmfArcTo:
            this.ArcTo(ptrData, true);
            break;
          case EmfPlusRecordType.EmfPolyDraw:
            this.PolyDraw(this.GetData(ptrData, dataSize), true);
            break;
          case EmfPlusRecordType.EmfSetArcDirection:
            this.SetArcDirection(ptrData);
            break;
          case EmfPlusRecordType.EmfSetMiterLimit:
            this.SetMiterLimit(ptrData);
            break;
          case EmfPlusRecordType.EmfBeginPath:
            this.BeginPath(ptrData);
            break;
          case EmfPlusRecordType.EmfEndPath:
            this.EndPath(ptrData);
            break;
          case EmfPlusRecordType.EmfCloseFigure:
            this.CloseFigure(ptrData);
            break;
          case EmfPlusRecordType.EmfFillPath:
            this.FillPath(ptrData);
            break;
          case EmfPlusRecordType.EmfStrokeAndFillPath:
            this.StrokeAndFillPath(ptrData);
            break;
          case EmfPlusRecordType.EmfStrokePath:
            this.StrokePath(ptrData);
            break;
          case EmfPlusRecordType.EmfFlattenPath:
            this.FlattenPath(ptrData);
            break;
          case EmfPlusRecordType.EmfWidenPath:
            this.WidenPath(ptrData);
            break;
          case EmfPlusRecordType.EmfSelectClipPath:
            this.SelectClipPath(ptrData);
            break;
          case EmfPlusRecordType.EmfAbortPath:
            this.AbortPath(ptrData);
            break;
          case EmfPlusRecordType.EmfGdiComment:
            this.ignorePrivateData = !this.ignorePrivateData;
            break;
          case EmfPlusRecordType.EmfFillRgn:
            this.FillRgn(this.GetData(ptrData, dataSize), ptrData);
            break;
          case EmfPlusRecordType.EmfPaintRgn:
            this.PaintRgn(this.GetData(ptrData, dataSize), ptrData);
            break;
          case EmfPlusRecordType.EmfExtSelectClipRgn:
            this.ExtSelectClipRgn(this.GetData(ptrData, dataSize), ptrData);
            break;
          case EmfPlusRecordType.EmfBitBlt:
            this.BitBlt(ptrData);
            break;
          case EmfPlusRecordType.EmfStretchBlt:
            this.StretchBlt(ptrData);
            break;
          case EmfPlusRecordType.EmfMaskBlt:
            this.MaskBlt(ptrData);
            break;
          case EmfPlusRecordType.EmfStretchDIBits:
            this.StretchDIBits(ptrData);
            break;
          case EmfPlusRecordType.EmfExtCreateFontIndirect:
            this.ExtCreateFontIndirect(ptrData);
            break;
          case EmfPlusRecordType.EmfExtTextOutA:
            this.ExtTextOut(ptrData, false);
            break;
          case EmfPlusRecordType.EmfExtTextOutW:
            this.ExtTextOut(ptrData, true);
            break;
          case EmfPlusRecordType.EmfPolyBezier16:
            this.PolyBezier16(ptrData);
            break;
          case EmfPlusRecordType.EmfPolygon16:
            this.Polygon16(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyline16:
            this.Polyline16(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyBezierTo16:
            this.PolyBezierTo16(ptrData);
            break;
          case EmfPlusRecordType.EmfPolylineTo16:
            this.PolyLineTo16(ptrData);
            break;
          case EmfPlusRecordType.EmfPolyPolyline16:
            this.PolyPolyline(this.GetData(ptrData, dataSize), false);
            break;
          case EmfPlusRecordType.EmfPolyPolygon16:
            this.PolyPolygon(this.GetData(ptrData, dataSize), false);
            break;
          case EmfPlusRecordType.EmfPolyDraw16:
            this.PolyDraw(this.GetData(ptrData, dataSize), false);
            break;
          case EmfPlusRecordType.EmfCreateDibPatternBrushPt:
            this.CreateDibPatternBrushPt(ptrData);
            break;
          case EmfPlusRecordType.EmfExtCreatePen:
            this.ExtCreatePen(ptrData);
            break;
          case EmfPlusRecordType.EmfSetIcmMode:
            this.SetIcmMode(ptrData);
            break;
          case EmfPlusRecordType.EmfAlphaBlend:
            this.AlphaBlend(ptrData);
            break;
          case EmfPlusRecordType.EmfSetLayout:
            this.SetLayout(ptrData);
            break;
          case EmfPlusRecordType.EmfTransparentBlt:
            this.TransparentBlt(ptrData);
            break;
          case EmfPlusRecordType.EmfGradientFill:
            this.GradientFill(this.GetData(ptrData, dataSize));
            break;
          default:
            this.Renderer.m_EMFState = false;
            break;
        }
      }
      catch (Exception ex)
      {
      }
      this.Renderer.m_previousRecordtype = recordType;
      return true;
    }
  }

  private void Header(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int index = 0;
    RectangleF rectangleF1 = (RectangleF) this.ReadRECT(data, ref index);
    RectangleF rectangleF2 = (RectangleF) this.ReadRECT(data, ref index);
    int intSize = MetafileParser.IntSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    int int32 = BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    BitConverter.ToInt32(data, index);
    index += intSize;
    GdiApi.SetMapMode(this.Objects.Handle, 8);
    POINT lpPoint = new POINT();
    double width = (double) rectangleF1.Width;
    double height = (double) rectangleF1.Height;
    RectangleF rectangleF3 = new RectangleF(PointF.Empty, new SizeF(rectangleF1.Width + 1f, rectangleF1.Height + 1f));
    SizeF size = this.Renderer.Graphics.Size;
    float num1 = size.Width / rectangleF3.Width;
    float num2 = size.Height / rectangleF3.Height;
    bool flag1 = (double) num1 != 1.0 || (double) num2 != 1.0;
    this.Renderer.m_originalLocation = rectangleF1.Location;
    PointF location = rectangleF1.Location;
    if ((double) location.X >= 0.0 || (double) location.Y >= 0.0)
    {
      if (((double) rectangleF2.X != 0.0 || (double) rectangleF2.Y != 0.0 || !flag1 || (double) location.X != 0.0 || (double) location.Y >= 0.0) && ((double) rectangleF2.X != 0.0 || (double) rectangleF2.Y != 0.0 || (double) num1 == 1.0 || (double) location.X >= 0.0 || (double) location.Y != 0.0))
        GdiApi.SetWindowOrgEx(this.Objects.Handle, (int) rectangleF1.X, (int) rectangleF1.Y, ref lpPoint);
      if ((double) location.X > 0.0 && (double) location.Y > 0.0)
      {
        bool flag2 = false;
        if (int32 > 4 && ((double) location.X < (double) location.Y && (double) rectangleF1.Width > (double) rectangleF1.Height || (double) location.X > (double) location.Y && (double) rectangleF1.Width < (double) rectangleF1.Height))
          flag2 = true;
        if (flag1 && (double) rectangleF2.X > 0.0 && (double) rectangleF2.Y > 0.0)
          this.Renderer.TranslateTransform(rectangleF1.X, rectangleF1.Y, MatrixOrder.Append);
        else if (flag1 && (double) rectangleF2.X == 0.0 && (double) rectangleF2.Y == 0.0 && (double) location.X > 0.0 && (double) location.Y > 0.0)
          this.Renderer.TranslateTransform(rectangleF1.X, rectangleF1.Y, MatrixOrder.Append);
        else if (flag1 && (double) rectangleF2.X == 0.0 && (double) rectangleF2.Y == 0.0)
          this.Renderer.TranslateTransform(rectangleF1.X, -rectangleF1.Y, MatrixOrder.Append);
        else if (!flag2 && !flag1 && (double) rectangleF2.X > 0.0 && (double) rectangleF2.Y > 0.0)
          this.Renderer.TranslateTransform(-rectangleF1.X, -rectangleF1.Y, MatrixOrder.Append);
      }
      else if ((double) location.X == 0.0 && (double) location.Y > 0.0 && !flag1 && (double) rectangleF2.X == 0.0 && (double) rectangleF2.Y > 0.0)
        this.Renderer.TranslateTransform(rectangleF1.X, -rectangleF1.Y, MatrixOrder.Append);
      else if ((double) location.X < 0.0 && (double) location.Y > 0.0)
      {
        bool flag3 = false;
        if (int32 > 4 && ((double) location.X + (double) rectangleF1.Width < 0.0 && (double) rectangleF1.Width < (double) rectangleF1.Height || (double) location.X + (double) rectangleF1.Width > 0.0 && (double) rectangleF1.Width > (double) rectangleF1.Height))
          flag3 = true;
        if (!flag3 && !flag1 && (double) rectangleF2.X < 0.0 && (double) rectangleF2.Y > 0.0)
          this.Renderer.TranslateTransform(-rectangleF1.X, -rectangleF1.Y, MatrixOrder.Append);
        else if ((double) rectangleF2.X == 0.0 && (double) rectangleF2.Y == 0.0)
          this.Renderer.TranslateTransform(rectangleF1.X, rectangleF1.Y, MatrixOrder.Append);
      }
      else if ((double) location.X > 0.0 && (double) location.Y < 0.0)
      {
        if (flag1 && (double) rectangleF2.X > 0.0 && (double) rectangleF2.Y > 0.0)
          this.Renderer.TranslateTransform(rectangleF1.X, rectangleF1.Y, MatrixOrder.Append);
        else if (!flag1 && (double) rectangleF2.X > 0.0 && (double) rectangleF2.Y < 0.0)
          this.Renderer.TranslateTransform(-rectangleF1.X, -rectangleF1.Y, MatrixOrder.Append);
      }
      else if ((double) location.X > 0.0 && (double) location.Y == 0.0 && flag1 && (double) rectangleF2.X == 0.0 && (double) rectangleF2.Y == 0.0)
        this.Renderer.TranslateTransform(rectangleF1.X, rectangleF1.Y, MatrixOrder.Append);
    }
    else if ((double) location.X < 0.0 && (double) location.Y < 0.0 && !flag1)
    {
      GdiApi.SetWindowOrgEx(this.Objects.Handle, (int) rectangleF1.X, (int) rectangleF1.Y, ref lpPoint);
      if (int32 <= 4 && (double) rectangleF2.X < 0.0 && (double) rectangleF2.Y < 0.0 && (double) location.X != (double) location.Y && (double) location.X + (double) rectangleF1.Width > 0.0 && (double) location.Y + (double) rectangleF1.Height > 0.0)
        this.Renderer.TranslateTransform(-rectangleF1.X, -rectangleF1.Y, MatrixOrder.Append);
    }
    this.Renderer.BeforeStart();
  }

  private void EndOfFile() => this.Renderer.BeforeEnd();

  private void SaveDC(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    GdiApi.SaveDC(this.Objects.Handle);
    this.Renderer.StateChanged = true;
    this.Objects.GraphicsState = this.Renderer.Save();
    this.Objects.Save();
  }

  private void RestoreDC(byte[] data)
  {
    int num = data != null ? BitConverter.ToInt32(data, 0) : throw new ArgumentNullException(nameof (data));
    RectangleF clipBounds = this.Renderer.NativeGraphics.ClipBounds;
    GdiApi.RestoreDC(this.Objects.Handle, num);
    this.Renderer.Restore(this.Objects.GraphicsState);
    this.m_clipReset = clipBounds == this.Renderer.NativeGraphics.ClipBounds && (double) this.Renderer.NativeGraphics.ClipBounds.X > 0.0;
    this.Objects.Restore(num);
  }

  private void SetMiterLimit(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETMITERLIMIT);
    EMR_SETMITERLIMIT structure = (EMR_SETMITERLIMIT) this.GetStructure(ptr, type);
    MetafileParser.CheckResult(GdiApi.SetMiterLimit(this.Objects.Handle, (float) structure.eMiterLimit, out float _));
    if (this.Objects.Pen == null)
      return;
    this.Objects.Pen.MiterLimit = this.LPtoDPX((float) structure.eMiterLimit);
  }

  private void ModifyWorldTransform(IntPtr ptr)
  {
    System.Type type = typeof (EMR_MODIFYWORLDTRANSFORM);
    EMR_MODIFYWORLDTRANSFORM structure = (EMR_MODIFYWORLDTRANSFORM) this.GetStructure(ptr, type);
    if ((double) Math.Abs(structure.xform.eM12) < 0.001 && (double) Math.Abs(structure.xform.eM21) < 0.001)
    {
      structure.xform.eM12 = 0.0f;
      structure.xform.eM21 = 0.0f;
    }
    this.TextAngle = this.CalculateRotationAngle(structure);
    this.SetValidGraphicsMode();
    MetafileParser.CheckResult(GdiApi.ModifyWorldTransform(this.Objects.Handle, ref structure.xform, (int) structure.iMode));
  }

  private void ExtCreatePen(IntPtr ptr)
  {
    EMR_EXTCREATEPEN structure = new EMR_EXTCREATEPEN();
    EMR_EXTCREATEPEN structureEx = (EMR_EXTCREATEPEN) this.GetStructureEx(ptr, (ValueType) structure);
    PS_PEN_TYPE psPenType = (PS_PEN_TYPE) (structureEx.elpPenStyle & 983040 /*0x0F0000*/);
    float num1 = (float) structureEx.elpWidth;
    if (psPenType == PS_PEN_TYPE.PS_GEOMETRIC)
    {
      float num2 = num1;
      num1 = this.LPtoDPWidth(num1);
      if ((double) num1 == 0.0 && (double) num2 > 0.0)
        num1 = num2 / 16f;
    }
    Pen pen = new Pen(ColorTranslator.FromWin32(structureEx.elpColor), num1);
    float peLimit;
    if (GdiApi.GetMiterLimit(this.Objects.Handle, out peLimit))
      pen.MiterLimit = peLimit;
    PS_PEN_STYLE psPenStyle = (PS_PEN_STYLE) (structureEx.elpPenStyle & 15);
    if (psPenStyle < PS_PEN_STYLE.PS_NULL)
      pen.DashStyle = (DashStyle) psPenStyle;
    if (psPenStyle == PS_PEN_STYLE.PS_USERSTYLE && structureEx.elpStyleEntry.Length != structureEx.elpNumEntries)
    {
      if ((double) pen.Width < 0.0)
        pen.Width = -pen.Width;
      float[] numArray = new float[structureEx.elpNumEntries];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = pen.Width;
      pen.DashPattern = numArray;
    }
    switch ((PS_PEN_CAP_STYLE) (structureEx.elpPenStyle & 3840 /*0x0F00*/))
    {
      case PS_PEN_CAP_STYLE.PS_ENDCAP_ROUND:
        pen.EndCap = LineCap.Round;
        break;
      case PS_PEN_CAP_STYLE.PS_ENDCAP_SQUARE:
        pen.EndCap = LineCap.Square;
        break;
      case PS_PEN_CAP_STYLE.PS_ENDCAP_FLAT:
        pen.EndCap = LineCap.Flat;
        break;
    }
    switch ((PS_PEN_JOIN_STYLE) (structureEx.elpPenStyle & 61440 /*0xF000*/))
    {
      case PS_PEN_JOIN_STYLE.PS_JOIN_ROUND:
        pen.LineJoin = LineJoin.Round;
        break;
      case PS_PEN_JOIN_STYLE.PS_JOIN_BEVEL:
        pen.LineJoin = LineJoin.Bevel;
        break;
      case PS_PEN_JOIN_STYLE.PS_JOIN_MITER:
        pen.LineJoin = LineJoin.Miter;
        break;
    }
    int ihPen = structureEx.ihPen;
    this.Objects.SelectedObjects.AddObject((object) pen, ihPen);
  }

  private void SelectObject(byte[] data)
  {
    int num = data != null ? BitConverter.ToInt32(data, 0) : throw new ArgumentNullException(nameof (data));
    this.m_selectedId = num;
    object obj = this.Objects.SelectedObjects.SelectObject(num);
    if (obj == null)
    {
      if (this.m_selectedPen.Count > 0 && this.m_selectedPen.ContainsKey(num))
        obj = (object) this.m_selectedPen[num];
      if (this.m_selectedBrush.Count > 0 && this.m_selectedBrush.ContainsKey(num))
        obj = (object) this.m_selectedBrush[num];
    }
    this.Objects.SelectObject(obj);
    if (obj is FontEx)
    {
      this.m_selectedFont = (obj as FontEx).Font;
      this.FontTextAngle = (obj as FontEx).Angle;
      if (!this.m_fontAngleWithID.ContainsKey(num))
        this.m_fontAngleWithID.Add(num, obj as FontEx);
      else
        this.m_fontAngleWithID[num] = obj as FontEx;
    }
    if (obj is Pen && !this.m_selectedPen.ContainsKey(num))
      this.m_selectedPen.Add(num, obj as Pen);
    if (!(obj is Brush) || this.m_selectedBrush.ContainsKey(num))
      return;
    this.m_selectedBrush.Add(num, obj as Brush);
  }

  private void DeleteObject(byte[] data)
  {
    int num = data != null ? BitConverter.ToInt32(data, 0) : throw new ArgumentNullException(nameof (data));
    this.Objects.DeleteObject(this.Objects.SelectedObjects.DeleteObject(num));
    if (this.m_selectedPen.ContainsKey(num))
      this.m_selectedPen.Remove(num);
    if (!this.m_fontAngleWithID.ContainsKey(num))
      return;
    this.m_fontAngleWithID.Remove(num);
  }

  private void CreateBrushIndirect(IntPtr ptr)
  {
    System.Type type = typeof (EMR_CREATEBRUSHINDIRECT);
    EMR_CREATEBRUSHINDIRECT structure = (EMR_CREATEBRUSHINDIRECT) this.GetStructure(ptr, type);
    Brush brush = (Brush) null;
    Color color = ColorTranslator.FromWin32(structure.lb.lbColor);
    switch (structure.lb.lbStyle)
    {
      case BS_BRUSH_STYLE.BS_SOLID:
        brush = (Brush) new SolidBrush(color);
        break;
      case BS_BRUSH_STYLE.BS_NULL:
        brush = (Brush) new SolidBrush(Color.Empty);
        break;
      case BS_BRUSH_STYLE.BS_HATCHED:
        HatchStyle lbHatch = (HatchStyle) structure.lb.lbHatch;
        Color backColor = ColorTranslator.FromWin32(GdiApi.GetBkColor(this.Objects.Handle));
        brush = (Brush) new HatchBrush(lbHatch, color, backColor);
        break;
    }
    if (brush == null)
      return;
    int ihBrush = structure.ihBrush;
    this.Objects.SelectedObjects.AddObject((object) brush, ihBrush);
  }

  private void SetPolyFillMode(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    this.Objects.FillMode = (FillMode) (((EMR_SELECTCLIPPATH) this.GetStructure(ptr, type)).iMode - 1);
  }

  private void PolyPolygon(byte[] data, bool bIs32Bit)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int index1 = 0;
    int intSize = MetafileParser.IntSize;
    this.ReadRECT(data, ref index1);
    int int32_1 = BitConverter.ToInt32(data, index1);
    index1 += intSize;
    int int32_2 = BitConverter.ToInt32(data, index1);
    index1 += intSize;
    int[] numArray = this.ReadInt32Array(data, int32_1, ref index1);
    Point[] pointArray = this.ReadPointArray(data, int32_2, ref index1, bIs32Bit);
    int num = 0;
    GraphicsPath graphicsPath = new GraphicsPath(this.Objects.FillMode);
    for (int index2 = 0; index2 < int32_1; ++index2)
    {
      int length = numArray[index2];
      PointF[] points = new PointF[length];
      int index3 = 0;
      bool flag = false;
      int index4 = num;
      for (int index5 = num + length; index4 < index5; ++index4)
      {
        PointF pointF = (PointF) pointArray[index4];
        points[index3] = pointF;
        if (index3 > 0 && points[index3 - 1] != pointF)
          flag = true;
        ++index3;
      }
      num += index3;
      if (flag)
      {
        graphicsPath.AddPolygon(points);
        graphicsPath.CloseFigure();
      }
    }
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddPath(graphicsPath, false);
    }
    else
    {
      if (this.Objects.Brush != null && this.Objects.FillMode == FillMode.Winding)
        this.Renderer.FillPath(this.Objects.Brush, graphicsPath);
      else if (this.Objects.Brush != null && this.Objects.FillMode == FillMode.Alternate)
        this.Renderer.FillRegion(this.Objects.Brush, new Region(graphicsPath));
      if (this.Objects.Pen != null && this.Objects.Pen.Color.A != (byte) 0 && (double) this.Objects.Pen.Width != 0.0)
        this.Renderer.DrawPath(this.Objects.Pen, graphicsPath);
    }
    graphicsPath.Dispose();
  }

  private void SetMapMode(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    EMR_SELECTCLIPPATH structure = (EMR_SELECTCLIPPATH) this.GetStructure(ptr, type);
    GdiApi.SetMapMode(this.Objects.Handle, structure.iMode);
    this.m_mapMode = structure.iMode;
  }

  private void SetWindowOrgEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETVIEWPORTEXTEX);
    SIZE szlExtent = ((EMR_SETVIEWPORTEXTEX) this.GetStructure(ptr, type)).szlExtent;
    POINT lpPoint = new POINT();
    GdiApi.SetWindowOrgEx(this.Objects.Handle, szlExtent.cx, szlExtent.cy, ref lpPoint);
  }

  private void SetWindowExtEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETVIEWPORTEXTEX);
    SIZE szlExtent = ((EMR_SETVIEWPORTEXTEX) this.GetStructure(ptr, type)).szlExtent;
    SIZE lpSize1 = new SIZE();
    this.m_isWindowPort = true;
    this.newWindowSize = szlExtent;
    GdiApi.SetWindowExtEx(this.Objects.Handle, szlExtent.cx, szlExtent.cy, ref lpSize1);
    SIZE lpSize2 = new SIZE();
    GdiApi.GetViewportExtEx(this.Objects.Handle, ref lpSize2);
    if (lpSize2.cx > 1 || lpSize2.cy > 1)
      return;
    SIZE lpSize3 = new SIZE();
    GdiApi.SetViewportExtEx(this.Objects.Handle, szlExtent.cx, szlExtent.cy, ref lpSize3);
  }

  private void SetViewportOrgEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETVIEWPORTORGEX);
    POINT ptlOrigin = ((EMR_SETVIEWPORTORGEX) this.GetStructure(ptr, type)).ptlOrigin;
    POINT lpPoint = new POINT();
    GdiApi.SetViewportOrgEx(this.Objects.Handle, ptlOrigin.x, ptlOrigin.y, ref lpPoint);
  }

  private void SetViewportExtEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETVIEWPORTEXTEX);
    EMR_SETVIEWPORTEXTEX structure = (EMR_SETVIEWPORTEXTEX) this.GetStructure(ptr, type);
    SIZE szlExtent = structure.szlExtent;
    SIZE lpSize = new SIZE();
    if (!this.m_isBKMode && szlExtent.cx < szlExtent.cy && this.m_isWindowPort && this.newWindowSize.cx > 0 && this.newWindowSize.cy > 0)
      this.Renderer.ScaleTransform((float) structure.szlExtent.cx / (float) this.newWindowSize.cx, (float) structure.szlExtent.cy / (float) this.newWindowSize.cy, MatrixOrder.Append);
    else
      GdiApi.SetViewportExtEx(this.Objects.Handle, szlExtent.cx, szlExtent.cy, ref lpSize);
  }

  private bool IsExtentEmpty(SIZE extent) => extent.cx <= 1 && extent.cy <= 1;

  private void ScaleViewportExtEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SCALEVIEWPORTEXTEX);
    EMR_SCALEVIEWPORTEXTEX structure = (EMR_SCALEVIEWPORTEXTEX) this.GetStructure(ptr, type);
    SIZE lpSize = new SIZE();
    MetafileParser.CheckResult(GdiApi.ScaleViewportExtEx(this.Objects.Handle, structure.xNum, structure.xDenom, structure.yNum, structure.yDenom, ref lpSize));
  }

  private void ScaleWindowExtEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SCALEVIEWPORTEXTEX);
    EMR_SCALEVIEWPORTEXTEX structure = (EMR_SCALEVIEWPORTEXTEX) this.GetStructure(ptr, type);
    SIZE lpSize = new SIZE();
    MetafileParser.CheckResult(GdiApi.ScaleWindowExtEx(this.Objects.Handle, structure.xNum, structure.xDenom, structure.yNum, structure.yDenom, ref lpSize));
  }

  private void BeginPath(IntPtr ptr)
  {
    MetafileParser.CheckResult(GdiApi.BeginPath(this.Objects.Handle));
    this.Objects.Path = new GraphicsPath(this.Objects.FillMode);
    this.Objects.IsOpenPath = true;
  }

  private void MoveToEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_LINETO);
    this.Objects.CurrentPoint = (PointF) ((EMR_LINETO) this.GetStructure(ptr, type)).ptl;
    if (this.Renderer.m_previousRecordtype != EmfPlusRecordType.EmfPolylineTo16 || this.Objects.Path.PointCount <= 0)
      return;
    this.Renderer.DrawLines(this.Objects.Pen, this.Objects.Path.PathPoints);
    this.Objects.Path = new GraphicsPath();
  }

  private void LineTo(IntPtr ptr)
  {
    System.Type type = typeof (EMR_LINETO);
    POINT ptl = ((EMR_LINETO) this.GetStructure(ptr, type)).ptl;
    PointF currentPoint = this.Objects.CurrentPoint;
    PointF point = new PointF((float) ptl.x, (float) ptl.y);
    PointF pt1 = this.LPtoDP(currentPoint);
    pt1.X += this.Objects.Pen.Width / 2f;
    pt1.Y += this.Objects.Pen.Width / 2f;
    PointF pt2 = this.LPtoDP(point);
    pt2.X += this.Objects.Pen.Width / 2f;
    pt2.Y += this.Objects.Pen.Width / 2f;
    MetafileParser.CheckResult(GdiApi.LineTo(this.Objects.Handle, ptl.x, ptl.y));
    if (this.Objects.IsOpenPath)
    {
      this.Objects.Path.AddLine(pt1, pt2);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawLines(this.Objects.Pen, new PointF[2]
      {
        pt1,
        pt2
      });
    }
  }

  private void EndPath(IntPtr ptr)
  {
    MetafileParser.CheckResult(GdiApi.EndPath(this.Objects.Handle));
    this.Objects.IsOpenPath = false;
  }

  private void AbortPath(IntPtr ptr)
  {
    MetafileParser.CheckResult(GdiApi.AbortPath(this.Objects.Handle));
    if (this.Objects.Path != null)
      this.Objects.Path = (GraphicsPath) null;
    this.Objects.IsOpenPath = false;
  }

  private void SelectClipPath(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    EMR_SELECTCLIPPATH structure = (EMR_SELECTCLIPPATH) this.GetStructure(ptr, type);
    MetafileParser.CheckResult(GdiApi.SelectClipPath(this.Objects.Handle, structure.iMode));
    GraphicsPath path = this.Objects.Path;
    if (path == null)
      return;
    COMBINE_RGN iMode = (COMBINE_RGN) structure.iMode;
    CombineMode mode = iMode == COMBINE_RGN.RGN_COPY ? CombineMode.Replace : (CombineMode) iMode;
    this.Renderer.SetClip(path, mode);
  }

  private void Polygon16(IntPtr ptr)
  {
    EMR_POLYLINE16 structure = new EMR_POLYLINE16();
    PointF[] points = this.ConvertType(((EMR_POLYLINE16) this.GetStructureEx(ptr, (ValueType) structure)).apts);
    try
    {
      if (points.Length <= 2)
        return;
      RectangleF rectangleF = RectangleF.Empty;
      if (this.Objects.IsOpenPath)
      {
        GraphicsPath graphicsPath = new GraphicsPath();
        graphicsPath.AddLines(points);
        rectangleF = graphicsPath.GetBounds();
        graphicsPath.Dispose();
      }
      if (this.Objects.IsOpenPath && rectangleF != RectangleF.Empty && (!this.m_clipPoints.Contains(rectangleF) || (double) this.Renderer.NativeGraphics.ClipBounds.X < 0.0 && (double) this.Renderer.NativeGraphics.ClipBounds.Y < 0.0))
      {
        this.Objects.Path.AddPolygon(points);
        this.m_clipPoints.Add(rectangleF);
      }
      else
      {
        if (this.Objects.IsOpenPath)
          return;
        GraphicsPath path = new GraphicsPath(this.Objects.FillMode);
        path.AddPolygon(points);
        if (this.ignorePrivateData)
          this.m_polygonPoints = points;
        SolidBrush brush = this.Objects.Brush as SolidBrush;
        if (this.Objects.Brush != null)
        {
          if (this.m_clipReset && brush != null && brush.Color.A == (byte) 0)
            this.Renderer.ResetClip();
          if (!this.Renderer.m_transparentBackMode)
            this.Renderer.FillPath(this.Objects.Brush, path);
          if (this.Objects.Brush is TextureBrush)
            this.m_pattenPoints = points;
        }
        if (this.Objects.Pen != null && this.Objects.Pen.Color.A != (byte) 0 && (!this.m_clipReset || brush == null || brush.Color.A != (byte) 0))
        {
          if (!this.Objects.isNewPen)
            this.Renderer.DrawPath(this.Objects.Pen, path);
          else if (this.m_selectedPen.ContainsKey(this.m_selectedId))
            this.Renderer.DrawPath(this.m_selectedPen[this.m_selectedId], path);
          else
            this.Renderer.DrawPath(this.Objects.Pen, path);
        }
        path.Dispose();
      }
    }
    catch
    {
    }
  }

  private void SetIcmMode(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    GdiApi.SetICMMode(this.Objects.Handle, ((EMR_SELECTCLIPPATH) this.GetStructure(ptr, type)).iMode);
  }

  private void AlphaBlend(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_ALPHABLEND);
    EMR_ALPHABLEND structure1 = (EMR_ALPHABLEND) this.GetStructure(ptr, type1);
    if (this.Objects.Image != null)
    {
      RectangleF empty = RectangleF.Empty with
      {
        X = this.LPtoDPX((float) structure1.xSrc),
        Y = this.LPtoDPY((float) structure1.ySrc),
        Width = this.LPtoDPWidth((float) structure1.cxSrc),
        Height = this.LPtoDPHeight((float) structure1.cySrc)
      };
      this.Renderer.DrawImage((Image) this.Objects.Image, RectangleF.Empty with
      {
        X = this.LPtoDPX((float) structure1.xDest),
        Y = this.LPtoDPY((float) structure1.yDest),
        Width = this.LPtoDPWidth((float) structure1.cxDest),
        Height = this.LPtoDPHeight((float) structure1.cyDest)
      }, empty, this.Objects.Graphics.PageUnit);
    }
    else
    {
      int num1 = structure1.offBmiSrc - 2 * MetafileParser.IntSize;
      IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
      System.Type type2 = typeof (BITMAPINFOHEADER);
      BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
      int offBitsSrc = structure1.offBitsSrc;
      uint cbBitsSrc = structure1.cbBitsSrc;
      Rectangle rc = new Rectangle(structure1.xSrc, structure1.ySrc, structure1.cxSrc, structure1.cySrc);
      Rectangle destRect = Rectangle.Truncate(this.LPtoDP((RectangleF) new Rectangle(structure1.xDest, structure1.yDest, structure1.cxDest, structure1.cyDest)));
      Rectangle srcRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc));
      Image alphaBlendedBitmap = this.GetAlphaBlendedBitmap(offBitsSrc, cbBitsSrc, ptr, num2, structure2, structure1.iUsageSrc);
      System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(alphaBlendedBitmap);
      if (destRect.Height > 0 && destRect.Location.Y != 0)
        this.ImageRegions.Add(new ImageRegion((float) destRect.Location.Y, (float) destRect.Height));
      this.Renderer.DrawImage(alphaBlendedBitmap, (RectangleF) destRect, (RectangleF) srcRect, graphics.PageUnit);
      graphics.Dispose();
    }
  }

  private void PolyBezier(IntPtr ptr)
  {
    EMR_POLYLINE structure = new EMR_POLYLINE();
    PointF[] points = this.ConvertType(((EMR_POLYLINE) this.GetStructureEx(ptr, (ValueType) structure)).apts);
    if (this.Objects.IsOpenPath)
    {
      this.Objects.Path.AddBeziers(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawBeziers(this.Objects.Pen, points);
    }
  }

  private void PolyBezier16(IntPtr ptr)
  {
    EMR_POLYLINE16 structure = new EMR_POLYLINE16();
    PointF[] points = this.ConvertType(((EMR_POLYLINE16) this.GetStructureEx(ptr, (ValueType) structure)).apts);
    if (this.Objects.IsOpenPath)
    {
      this.Objects.Path.AddBeziers(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawBeziers(this.Objects.Pen, points);
    }
  }

  private void Polygon(IntPtr ptr)
  {
    EMR_POLYLINE structure = new EMR_POLYLINE();
    PointF[] points = this.ConvertType(((EMR_POLYLINE) this.GetStructureEx(ptr, (ValueType) structure)).apts);
    try
    {
      if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
      {
        this.Objects.Path.AddPolygon(points);
      }
      else
      {
        GraphicsPath path = new GraphicsPath(this.Objects.FillMode);
        path.AddPolygon(points);
        if (this.Objects.Brush != null)
        {
          if (this.m_clipReset)
            this.Renderer.ResetClip();
          this.Renderer.FillPath(this.Objects.Brush, path);
        }
        if (this.Objects.Pen != null)
          this.Renderer.DrawPath(this.Objects.Pen, path);
        path.Dispose();
      }
    }
    catch
    {
    }
  }

  private void Polyline(IntPtr ptr)
  {
    EMR_POLYLINE structure = new EMR_POLYLINE();
    PointF[] points = this.ConvertType(((EMR_POLYLINE) this.GetStructureEx(ptr, (ValueType) structure)).apts);
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddLines(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      GraphicsPath path = new GraphicsPath();
      path.AddLines(points);
      this.Renderer.DrawPath(this.Objects.Pen, path);
    }
  }

  private void Polyline16(IntPtr ptr)
  {
    EMR_POLYLINE16 structure = new EMR_POLYLINE16();
    PointF[] points = this.ConvertType(((EMR_POLYLINE16) this.GetStructureEx(ptr, (ValueType) structure)).apts);
    if (this.Objects.IsOpenPath)
    {
      this.Objects.Path.AddLines(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      GraphicsPath path = new GraphicsPath();
      path.AddLines(points);
      this.Renderer.DrawPath(this.Objects.Pen, path);
    }
  }

  private void PolyBezierTo(IntPtr ptr)
  {
    EMR_POLYLINE structure = new EMR_POLYLINE();
    EMR_POLYLINE structureEx = (EMR_POLYLINE) this.GetStructureEx(ptr, (ValueType) structure);
    PointF[] points = this.AddPenWidthToPoints(this.AddCurrentPointTo(this.ConvertType(structureEx.apts)));
    GdiApi.PolyBezierTo(this.Objects.Handle, structureEx.apts, structureEx.cpts);
    if (this.Objects.IsOpenPath)
    {
      this.Objects.Path.AddBeziers(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawBeziers(this.Objects.Pen, points);
    }
  }

  private PointF[] AddPenWidthToPoints(PointF[] points)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    for (int index = 0; index < points.Length; ++index)
    {
      points[index].X += this.Objects.Pen.Width / 2f;
      points[index].Y += this.Objects.Pen.Width / 2f;
    }
    return points;
  }

  private void PolyBezierTo16(IntPtr ptr)
  {
    EMR_POLYLINE16 structure = new EMR_POLYLINE16();
    EMR_POLYLINE16 structureEx = (EMR_POLYLINE16) this.GetStructureEx(ptr, (ValueType) structure);
    PointF[] points = this.AddCurrentPointTo(this.ConvertType(structureEx.apts));
    GdiApi.PolyBezierTo(this.Objects.Handle, this.ConvertTypeEx(structureEx.apts), structureEx.cpts);
    if (this.Objects.IsOpenPath)
    {
      this.Objects.Path.AddBeziers(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawBeziers(this.Objects.Pen, points);
    }
  }

  private void PolyLineTo(IntPtr ptr)
  {
    EMR_POLYLINE structure = new EMR_POLYLINE();
    EMR_POLYLINE structureEx = (EMR_POLYLINE) this.GetStructureEx(ptr, (ValueType) structure);
    PointF[] points = this.AddCurrentPointTo(this.ConvertType(structureEx.apts));
    GdiApi.PolylineTo(this.Objects.Handle, structureEx.apts, structureEx.cpts);
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddLines(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      GraphicsPath path = new GraphicsPath();
      path.AddLines(points);
      this.Renderer.DrawPath(this.Objects.Pen, path);
    }
  }

  private void PolyLineTo16(IntPtr ptr)
  {
    EMR_POLYLINE16 structure = new EMR_POLYLINE16();
    EMR_POLYLINE16 structureEx = (EMR_POLYLINE16) this.GetStructureEx(ptr, (ValueType) structure);
    PointF[] points = this.AddCurrentPointTo(this.ConvertType(structureEx.apts));
    GdiApi.PolylineTo(this.Objects.Handle, this.ConvertTypeEx(structureEx.apts), structureEx.cpts);
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddLines(points);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      GraphicsPath path = new GraphicsPath();
      path.AddLines(points);
      this.Renderer.DrawPath(this.Objects.Pen, path);
    }
  }

  private void PolyPolyline(byte[] data, bool bIs32Bit)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int index1 = 0;
    int intSize = MetafileParser.IntSize;
    this.ReadRECT(data, ref index1);
    int int32_1 = BitConverter.ToInt32(data, index1);
    index1 += intSize;
    int int32_2 = BitConverter.ToInt32(data, index1);
    index1 += intSize;
    int[] numArray = this.ReadInt32Array(data, int32_1, ref index1);
    Point[] pointArray = this.ReadPointArray(data, int32_2, ref index1, bIs32Bit);
    int num = 0;
    GraphicsPath graphicsPath = new GraphicsPath(this.Objects.FillMode);
    for (int index2 = 0; index2 < int32_1; ++index2)
    {
      int length = numArray[index2];
      PointF[] points = new PointF[length];
      int index3 = 0;
      int index4 = num;
      for (int index5 = num + length; index4 < index5; ++index4)
      {
        PointF pointF = (PointF) pointArray[index4];
        points[index3] = pointF;
        ++index3;
      }
      num += index3;
      if (this.Objects.Pen != null)
      {
        graphicsPath.AddLines(points);
        if (points[0] == points[points.Length - 1])
          graphicsPath.CloseFigure();
      }
    }
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddPath(graphicsPath, false);
    }
    else
    {
      if (this.Objects.Pen == null || (double) this.Objects.Pen.Width == 0.0)
        return;
      this.Renderer.DrawPath(this.Objects.Pen, graphicsPath);
      graphicsPath.Dispose();
    }
  }

  private void OffsetClipRgn(IntPtr ptr)
  {
    System.Type type = typeof (EMR_OFFSETCLIPRGN);
    EMR_OFFSETCLIPRGN structure = (EMR_OFFSETCLIPRGN) this.GetStructure(ptr, type);
    this.Renderer.TranslateClip(this.LPtoDPX((float) structure.ptlOffset.x), this.LPtoDPY((float) structure.ptlOffset.y));
  }

  private void ExcludeClipRect(IntPtr ptr)
  {
    System.Type type = typeof (EMR_EXCLUDECLIPRECT);
    EMR_EXCLUDECLIPRECT structure = (EMR_EXCLUDECLIPRECT) this.GetStructure(ptr, type);
    RectangleF rectangleF = this.LPtoDP((RectangleF) structure.rclClip);
    if (((double) rectangleF.X > 0.0 || (double) rectangleF.Y > 0.0) && (this.Renderer.m_clipPath || structure.rclClip.left <= 0 || structure.rclClip.top <= 0))
      return;
    this.Renderer.ExcludeClip(Rectangle.Truncate(rectangleF));
  }

  private void IntersectClipRect(IntPtr ptr)
  {
    System.Type type = typeof (EMR_EXCLUDECLIPRECT);
    RectangleF rect = this.LPtoDP((RectangleF) ((EMR_EXCLUDECLIPRECT) this.GetStructure(ptr, type)).rclClip);
    this.Renderer.IntersectClip(rect);
    if (((double) rect.X > 0.0 || (double) rect.Y > 0.0) && this.Renderer.m_clipPath)
      return;
    this.Renderer.ExcludeClip(new Rectangle((int) rect.Left, (int) rect.Top, (int) rect.Width, (int) rect.Height));
  }

  private void PolyDraw(byte[] data, bool bIs32Bit)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int index1 = 0;
    int intSize = MetafileParser.IntSize;
    this.ReadRECT(data, ref index1);
    int int32 = BitConverter.ToInt32(data, index1);
    index1 += intSize;
    Point[] pts = this.ReadPointArray(data, int32, ref index1, bIs32Bit);
    int length = Math.Min(pts.Length, data.Length - index1);
    PointF currentPoint = this.Objects.CurrentPoint;
    byte[] types = new byte[length];
    for (int index2 = index1; index2 < length; index2 += intSize)
    {
      PT_POINT_TYPE ptPointType = (PT_POINT_TYPE) data[index2];
      PathPointType pathPointType = PathPointType.Start;
      if ((ptPointType & PT_POINT_TYPE.PT_CLOSEFIGURE) > (PT_POINT_TYPE) 0)
        pathPointType |= PathPointType.CloseSubpath;
      if ((ptPointType & PT_POINT_TYPE.PT_LINETO) > (PT_POINT_TYPE) 0)
        pathPointType |= PathPointType.Line;
      if ((ptPointType & PT_POINT_TYPE.PT_BEZIERTO) > (PT_POINT_TYPE) 0)
        pathPointType |= PathPointType.Bezier;
      PointF pointF = (PointF) pts[index2 - index1];
      types[index2] = (byte) pathPointType;
    }
    GraphicsPath graphicsPath = new GraphicsPath(pts, types);
    graphicsPath.FillMode = this.Objects.FillMode;
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddPath(graphicsPath, false);
    }
    else
    {
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawPath(this.Objects.Pen, graphicsPath);
      graphicsPath.Dispose();
    }
  }

  private void SetArcDirection(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETARCDIRECTION);
    this.Objects.ArcDirection = (AD_ANGLEDIRECTION) ((EMR_SETARCDIRECTION) this.GetStructure(ptr, type)).iArcDirection;
  }

  private void FlattenPath(IntPtr ptr)
  {
    if (this.Objects.Path == null)
      return;
    this.Objects.Path.Flatten();
  }

  private void WidenPath(IntPtr ptr)
  {
    if (this.Objects.Path == null || this.Objects.Pen == null)
      return;
    this.Objects.Path.Widen(this.Objects.Pen);
  }

  private void FillRgn(byte[] data, IntPtr ptr)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    System.Type type = typeof (EMR_FILLRGN);
    EMR_FILLRGN structure = (EMR_FILLRGN) this.GetStructure(ptr, type);
    int num1 = Marshal.SizeOf(type);
    int nCount = structure.RgnData.rdh.nCount;
    int index1 = num1;
    int num2 = Math.Min(nCount * MetafileParser.IntSize * 4, data.Length - index1);
    int num3 = Marshal.SizeOf(typeof (RECT));
    Region region = new Region(Rectangle.Empty);
    for (int index2 = 0; index2 < num2; index2 += num3)
    {
      RectangleF rect = this.LPtoDP((RectangleF) this.ReadRECT(data, ref index1));
      region.Union(rect);
    }
    if (this.Objects.SelectedObjects.CreatedGraphicObjects[(object) structure.ihBrush] is Brush createdGraphicObject)
      this.Renderer.FillRegion(createdGraphicObject, region);
    region.Dispose();
  }

  private void PaintRgn(byte[] data, IntPtr ptr)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    System.Type type = typeof (EMR_INVERTRGN);
    EMR_INVERTRGN structure = (EMR_INVERTRGN) this.GetStructure(ptr, type);
    int num1 = Marshal.SizeOf(type);
    int nCount = structure.RgnData.rdh.nCount;
    int index1 = num1;
    int num2 = Math.Min(nCount * MetafileParser.IntSize * 4, data.Length - index1);
    int num3 = Marshal.SizeOf(typeof (RECT));
    Region region = new Region(Rectangle.Empty);
    for (int index2 = 0; index2 < num2; index2 += num3)
    {
      RectangleF rect = this.LPtoDP((RectangleF) this.ReadRECT(data, ref index1));
      region.Union(rect);
    }
    if (this.Objects.Brush != null)
      this.Renderer.FillRegion(this.Objects.Brush, region);
    region.Dispose();
  }

  private void ExtSelectClipRgn(byte[] data, IntPtr ptr)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    System.Type type = typeof (EMR_EXTSELECTCLIPRGN);
    if (data.Length > 12)
    {
      EMR_EXTSELECTCLIPRGN structure = (EMR_EXTSELECTCLIPRGN) this.GetStructure(ptr, type);
      int num1 = Marshal.SizeOf(type);
      if (structure.cbRgnData <= 0)
        return;
      int nCount = structure.RgnData.rdh.nCount;
      int index1 = num1;
      int num2 = Math.Min(nCount * MetafileParser.IntSize * 4, data.Length - index1);
      int num3 = Marshal.SizeOf(typeof (RECT));
      bool flag = true;
      Region region = new Region(Rectangle.Empty);
      GraphicsPath path = new GraphicsPath();
      for (int index2 = 0; index2 < num2; index2 += num3)
      {
        RectangleF rectangleF1 = (RectangleF) this.ReadRECT(data, ref index1);
        PointF originalLocation = this.Renderer.m_originalLocation;
        if ((double) originalLocation.X > 0.0 || (double) originalLocation.Y > 0.0)
          rectangleF1 = new RectangleF(rectangleF1.X - originalLocation.X, rectangleF1.Y - originalLocation.Y, rectangleF1.Width, rectangleF1.Height);
        else if ((double) originalLocation.X >= 0.0 || (double) originalLocation.Y >= 0.0)
        {
          RectangleF rectangleF2 = rectangleF1;
          rectangleF1 = this.LPtoDP(rectangleF1);
          if ((double) rectangleF1.Height > 0.0)
            rectangleF1 = rectangleF2;
        }
        switch (structure.iMode)
        {
          case 1:
            if (nCount > 1)
            {
              path.AddRectangle(rectangleF1);
              break;
            }
            region.Intersect(rectangleF1);
            break;
          case 2:
            region.Union(rectangleF1);
            break;
          case 3:
            region.Xor(rectangleF1);
            break;
          case 4:
            region.Exclude(rectangleF1);
            break;
          case 5:
            path.AddRectangle(rectangleF1);
            break;
        }
      }
      if (path.GetBounds() == RectangleF.Empty && this.Renderer.NativeGraphics.ClipBounds != RectangleF.Empty && structure.iMode == 5)
      {
        if (this.m_previousClipBounds == null)
        {
          this.m_previousClipBounds = this.m_polygonPoints;
          this.Renderer.m_transparentBackMode = true;
        }
        path.AddLines(this.m_previousClipBounds);
        this.Renderer.previousClipBounds = path.PathPoints;
        this.Renderer.SetClip(path, CombineMode.Replace);
        path.Dispose();
      }
      else if (structure.iMode == 5 || structure.iMode == 1 && path.PointCount > 0)
      {
        this.Renderer.previousClipBounds = path.PathPoints;
        this.Renderer.SetClip(path, CombineMode.Replace);
        this.m_previousClipBounds = path.PathPoints;
        path.Dispose();
      }
      else
      {
        COMBINE_RGN iMode = (COMBINE_RGN) structure.iMode;
        CombineMode mode = iMode == COMBINE_RGN.RGN_COPY ? CombineMode.Replace : (CombineMode) iMode;
        if (mode == CombineMode.Replace && (double) this.Renderer.NativeGraphics.ClipBounds.X > 0.0 || flag)
          this.Renderer.SetClip(region, mode);
        region.Dispose();
      }
    }
    else
    {
      if (this.Renderer.m_previousRecordtype != EmfPlusRecordType.EmfSelectClipPath)
        return;
      this.Renderer.ResetClip();
    }
  }

  private void SetBkMode(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    EMR_SELECTCLIPPATH structure = (EMR_SELECTCLIPPATH) this.GetStructure(ptr, type);
    GdiApi.SetBkMode(this.Objects.Handle, structure.iMode);
    this.m_isBKMode = true;
    this.Objects.BACKGROUNDMODE = structure.iMode;
  }

  private void SetTextAlign(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    this.Objects.TextAlign = (TA_TEXT_ALIGN) ((EMR_SELECTCLIPPATH) this.GetStructure(ptr, type)).iMode;
  }

  private void SetTextColor(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETTEXTCOLOR);
    this.Objects.ForeColor = ColorTranslator.FromWin32(((EMR_SETTEXTCOLOR) this.GetStructure(ptr, type)).crColor);
  }

  private void SetBkColor(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETTEXTCOLOR);
    this.Objects.BackColor = ColorTranslator.FromWin32(((EMR_SETTEXTCOLOR) this.GetStructure(ptr, type)).crColor);
  }

  private void SetWorldTransform(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETWORLDTRANSFORM);
    EMR_SETWORLDTRANSFORM structure = (EMR_SETWORLDTRANSFORM) this.GetStructure(ptr, type);
    this.SetValidGraphicsMode();
    MetafileParser.CheckResult(GdiApi.SetWorldTransform(this.Objects.Handle, ref structure.xform));
  }

  private void CreatePen(IntPtr ptr)
  {
    System.Type type = typeof (EMR_CREATEPEN);
    EMR_CREATEPEN structure = (EMR_CREATEPEN) this.GetStructure(ptr, type);
    Pen pen = new Pen(ColorTranslator.FromWin32(structure.lopn.lopnColor));
    Point lopnWidth = (Point) structure.lopn.lopnWidth;
    pen.Width = this.LPtoDPWidth((float) lopnWidth.X);
    if ((double) pen.Width == 0.0)
      pen.Width = 0.5f;
    if (structure.lopn.lopnStyle != 6U)
    {
      DashStyle lopnStyle = (DashStyle) structure.lopn.lopnStyle;
      pen.DashStyle = lopnStyle;
    }
    int ihPen = structure.ihPen;
    this.Objects.SelectedObjects.AddObject((object) pen, ihPen);
  }

  private void AngleArc(IntPtr ptr)
  {
    System.Type type = typeof (EMR_ANGLEARC);
    EMR_ANGLEARC structure = (EMR_ANGLEARC) this.GetStructure(ptr, type);
    int nRadius = structure.nRadius;
    RectangleF empty = RectangleF.Empty with
    {
      X = (float) (structure.ptlCenter.x - nRadius),
      Y = (float) (structure.ptlCenter.y - nRadius),
      Width = (float) (2 * nRadius),
      Height = (float) (2 * nRadius)
    };
    PointF currentPoint1 = this.Objects.CurrentPoint;
    MetafileParser.CheckResult(GdiApi.AngleArc(this.Objects.Handle, structure.ptlCenter.x, structure.ptlCenter.y, structure.nRadius, structure.eStartAngle, structure.eStartAngle));
    PointF currentPoint2 = this.Objects.CurrentPoint;
    float startAngle = -structure.eStartAngle;
    float sweepAngle = -structure.eSweepAngle;
    RectangleF rect = this.LPtoDP(empty);
    PointF pt1 = this.LPtoDP(currentPoint1);
    PointF pt2 = this.LPtoDP(currentPoint2);
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddLine(pt1, pt2);
      this.Objects.Path.AddArc(rect, startAngle, sweepAngle);
    }
    else if (this.Objects.Pen != null)
    {
      this.Renderer.DrawLines(this.Objects.Pen, new PointF[2]
      {
        pt1,
        pt2
      });
      this.Renderer.DrawArc(this.Objects.Pen, rect, startAngle, sweepAngle);
    }
    GdiApi.AngleArc(this.Objects.Handle, structure.ptlCenter.x, structure.ptlCenter.y, nRadius, structure.eStartAngle, structure.eSweepAngle);
  }

  private void Ellipse(IntPtr ptr)
  {
    System.Type type = typeof (EMR_RECTANGLE);
    RectangleF rect = this.LPtoDP((RectangleF) ((EMR_RECTANGLE) this.GetStructure(ptr, type)).rclBox);
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddEllipse(rect);
    }
    else
    {
      if (this.Objects.Brush != null)
        this.Renderer.FillEllipse(this.Objects.Brush, rect);
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawEllipse(this.Objects.Pen, rect);
    }
  }

  private void RectangleEx(IntPtr ptr)
  {
    System.Type type = typeof (EMR_RECTANGLE);
    RectangleF rect = this.LPtoDP((RectangleF) ((EMR_RECTANGLE) this.GetStructure(ptr, type)).rclBox);
    RectangleF[] rects = new RectangleF[1]{ rect };
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddRectangle(rect);
    }
    else
    {
      if (this.Objects.Brush != null)
      {
        if ((double) this.Renderer.Graphics.Size.Width < (double) rects[0].Width && (double) this.Renderer.Graphics.Size.Height < (double) rects[0].Height && !this.Renderer.m_taggedPDF && !this.Renderer.m_skipInnerScale)
        {
          this.Renderer.Graphics.ScaleTransform(this.Renderer.Graphics.Size.Width / rects[0].Width, this.Renderer.Graphics.Size.Height / rects[0].Height);
          if ((double) rects[0].X < 0.0 && (double) rects[0].Y < 0.0)
            this.Renderer.Graphics.TranslateTransform(-rects[0].X, -rects[0].Y);
        }
        this.Renderer.FillRectangles(this.Objects.Brush, rects);
        if (this.Objects.Pen != null && this.Objects.Pen.Color.A != (byte) 0 && (this.Objects.BackColor.ToArgb() != this.Objects.Pen.Color.ToArgb() || (this.Objects.Brush as SolidBrush).Color.A != (byte) 0))
          this.Renderer.DrawRectangles(this.Objects.Pen, rects);
      }
      if (this.Objects.Pen == null || this.Objects.Pen.Color.A == (byte) 0 || this.Objects.Brush != null)
        return;
      this.Renderer.DrawRectangles(this.Objects.Pen, rects);
    }
  }

  private void RoundRect(IntPtr ptr)
  {
    System.Type type = typeof (EMR_ROUNDRECT);
    EMR_ROUNDRECT structure = (EMR_ROUNDRECT) this.GetStructure(ptr, type);
    float logicValue1 = (float) structure.szlCorner.cx / 2f;
    float logicValue2 = (float) structure.szlCorner.cy / 2f;
    RectangleF rclBox = (RectangleF) structure.rclBox;
    GraphicsPath graphicsPath = new GraphicsPath();
    float num1 = this.LPtoDPWidth(logicValue1);
    float num2 = this.LPtoDPHeight(logicValue2);
    RectangleF rectangleF = this.LPtoDP(rclBox);
    float x = rectangleF.X;
    float y1_1 = rectangleF.Bottom - num2;
    float x2_1 = x;
    float y2_1 = rectangleF.Y + num2;
    graphicsPath.AddLine(x, y1_1, x2_1, y2_1);
    float startAngle1 = 180f;
    float sweepAngle1 = 90f;
    RectangleF empty = RectangleF.Empty with
    {
      Location = rectangleF.Location,
      Width = this.LPtoDPWidth((float) structure.szlCorner.cx),
      Height = this.LPtoDPHeight((float) structure.szlCorner.cy)
    };
    graphicsPath.AddArc(empty, startAngle1, sweepAngle1);
    float x1_1 = rectangleF.X + num1;
    float y = rectangleF.Y;
    float x2_2 = rectangleF.Right - num1;
    float y2_2 = y;
    graphicsPath.AddLine(x1_1, y, x2_2, y2_2);
    float startAngle2 = -90f;
    float sweepAngle2 = 90f;
    empty.X = rectangleF.Right - empty.Width;
    empty.Y = rectangleF.Y;
    graphicsPath.AddArc(empty, startAngle2, sweepAngle2);
    float right = rectangleF.Right;
    float y1_2 = rectangleF.Y + num2;
    float x2_3 = right;
    float y2_3 = rectangleF.Bottom - num2;
    graphicsPath.AddLine(right, y1_2, x2_3, y2_3);
    float startAngle3 = 0.0f;
    float sweepAngle3 = 90f;
    empty.X = rectangleF.Right - empty.Width;
    empty.Y = rectangleF.Bottom - empty.Height;
    graphicsPath.AddArc(empty, startAngle3, sweepAngle3);
    float x1_2 = rectangleF.Right - num1;
    float bottom = rectangleF.Bottom;
    float x2_4 = rectangleF.X + num1;
    float y2_4 = bottom;
    graphicsPath.AddLine(x1_2, bottom, x2_4, y2_4);
    float startAngle4 = 90f;
    float sweepAngle4 = 90f;
    empty.X = rectangleF.X;
    empty.Y = rectangleF.Bottom - empty.Height;
    graphicsPath.AddArc(empty, startAngle4, sweepAngle4);
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddPath(graphicsPath, false);
    }
    else
    {
      if (this.Objects.Brush != null)
        this.Renderer.FillPath(this.Objects.Brush, graphicsPath);
      if (this.Objects.Pen != null)
        this.Renderer.DrawPath(this.Objects.Pen, graphicsPath);
    }
    graphicsPath.Dispose();
  }

  private void Chord(IntPtr ptr)
  {
    System.Type type = typeof (EMR_ARC);
    EMR_ARC structure = (EMR_ARC) this.GetStructure(ptr, type);
    RectangleF rect = this.LPtoDP((RectangleF) structure.rclBox);
    float x0 = (rect.Left + rect.Right) / 2f;
    float y0 = (rect.Top + rect.Bottom) / 2f;
    PointF pointF1 = this.LPtoDP((PointF) structure.ptlStart);
    PointF pointF2 = this.LPtoDP((PointF) structure.ptlEnd);
    float angle = this.GetAngle(x0, y0, pointF1.X, pointF1.Y);
    float num = this.GetAngle(x0, y0, pointF2.X, pointF2.Y) - angle;
    float sweepAngle = (double) num < 0.0 ? num : num - 360f;
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddArc(rect, angle, sweepAngle);
    graphicsPath.CloseFigure();
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddPath(graphicsPath, false);
    }
    else
    {
      if (this.Objects.Brush != null)
        this.Renderer.FillPath(this.Objects.Brush, graphicsPath);
      if (this.Objects.Pen != null)
        this.Renderer.DrawPath(this.Objects.Pen, graphicsPath);
    }
    graphicsPath.Dispose();
  }

  private void Pie(IntPtr ptr)
  {
    System.Type type = typeof (EMR_ARC);
    EMR_ARC structure = (EMR_ARC) this.GetStructure(ptr, type);
    Rectangle rect = Rectangle.Truncate(this.LPtoDP((RectangleF) structure.rclBox));
    float x0 = (float) (rect.Left + rect.Right) / 2f;
    float y0 = (float) (rect.Top + rect.Bottom) / 2f;
    PointF pointF1 = this.LPtoDP((PointF) structure.ptlStart);
    PointF pointF2 = this.LPtoDP((PointF) structure.ptlEnd);
    float angle = this.GetAngle(x0, y0, pointF1.X, pointF1.Y);
    float num = this.GetAngle(x0, y0, pointF2.X, pointF2.Y) - angle;
    float sweepAngle = (double) num < 0.0 ? num : num - 360f;
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      this.Objects.Path.AddPie(rect, angle, sweepAngle);
    }
    else
    {
      if (this.Objects.Brush != null)
        this.Renderer.FillPie(this.Objects.Brush, (float) rect.X, (float) rect.Y, (float) rect.Width, (float) rect.Height, angle, sweepAngle);
      if (this.Objects.Pen == null)
        return;
      this.Renderer.DrawPie(this.Objects.Pen, (RectangleF) rect, angle, sweepAngle);
    }
  }

  private void ArcTo(IntPtr ptr, bool bIsArcTo)
  {
    System.Type type = typeof (EMR_ARC);
    EMR_ARC structure = (EMR_ARC) this.GetStructure(ptr, type);
    RectangleF rect = this.LPtoDP((RectangleF) structure.rclBox);
    if (structure.rclBox.Width < 0 && structure.rclBox.Height > 0)
      rect = new RectangleF(rect.X - rect.Width, rect.Y - rect.Height, rect.Width, rect.Height);
    else if (structure.rclBox.Height < 0 && structure.rclBox.Width > 0 && (double) rect.Width < 0.0 && (double) rect.Height < 0.0)
      rect = new RectangleF(rect.X, rect.Y, -rect.Width, -rect.Height);
    else if (structure.rclBox.Height < 0 && structure.rclBox.Width < 0)
      rect = new RectangleF(rect.X, rect.Y + rect.Height, rect.Width, -rect.Height);
    float x0 = (rect.Left + rect.Right) / 2f;
    float y0 = (rect.Top + rect.Bottom) / 2f;
    PointF pointF1 = this.LPtoDP((PointF) structure.ptlStart);
    PointF pointF2 = this.LPtoDP((PointF) structure.ptlEnd);
    float angle = this.GetAngle(x0, y0, pointF1.X, pointF1.Y);
    float num = this.GetAngle(x0, y0, pointF2.X, pointF2.Y) - angle;
    float sweepAngle = (double) num < 0.0 ? num : num - 360f;
    PointF pt1 = PointF.Empty;
    PointF pt2 = PointF.Empty;
    if (bIsArcTo)
    {
      pt1 = this.LPtoDP(this.Objects.CurrentPoint);
      pt2 = this.LPtoDP(this.GetStartPoint((Rectangle) structure.rclBox, (Point) structure.ptlStart));
    }
    if (this.Objects.IsOpenPath && this.IsColorVisible(this.Objects))
    {
      if (bIsArcTo)
        this.Objects.Path.AddLine(pt1, pt2);
      this.Objects.Path.AddArc(rect, angle, sweepAngle);
    }
    else if (this.Objects.Pen != null)
    {
      if (bIsArcTo)
        this.Renderer.DrawLines(this.Objects.Pen, new PointF[2]
        {
          pt1,
          pt2
        });
      this.Renderer.DrawArc(this.Objects.Pen, rect, angle, sweepAngle);
    }
    Point ptlStart = (Point) structure.ptlStart;
    Point ptlEnd = (Point) structure.ptlEnd;
    if (bIsArcTo)
      GdiApi.ArcTo(this.Objects.Handle, structure.rclBox.left, structure.rclBox.top, structure.rclBox.right, structure.rclBox.bottom, ptlStart.X, ptlStart.Y, ptlEnd.X, ptlEnd.Y);
    else
      GdiApi.Arc(this.Objects.Handle, structure.rclBox.left, structure.rclBox.top, structure.rclBox.right, structure.rclBox.bottom, ptlStart.X, ptlStart.Y, ptlEnd.X, ptlEnd.Y);
  }

  private void CloseFigure(IntPtr ptr)
  {
    if (!this.Objects.IsOpenPath)
      return;
    this.Objects.Path.CloseFigure();
    GdiApi.CloseFigure(this.Objects.Handle);
  }

  private void FillPath(IntPtr ptr)
  {
    System.Type type = typeof (EMR_FILLPATH);
    EMR_FILLPATH structure = (EMR_FILLPATH) this.GetStructure(ptr, type);
    if (this.Objects.Path != null && !this.Objects.IsOpenPath)
    {
      this.Objects.Path.CloseAllFigures();
      if (this.Objects.Brush != null)
      {
        this.Objects.Path.FillMode = this.Objects.FillMode;
        this.Renderer.FillPath(this.Objects.Brush, this.Objects.Path);
      }
    }
    GdiApi.FillPath(this.Objects.Handle);
    this.Objects.Path = (GraphicsPath) null;
  }

  private void StrokeAndFillPath(IntPtr ptr)
  {
    System.Type type = typeof (EMR_FILLPATH);
    EMR_FILLPATH structure = (EMR_FILLPATH) this.GetStructure(ptr, type);
    if (this.Objects.Path != null && !this.Objects.IsOpenPath)
    {
      this.Objects.Path.CloseAllFigures();
      this.Objects.Path.FillMode = this.Objects.FillMode;
      if (this.Objects.Brush != null)
        this.Renderer.FillPath(this.Objects.Brush, this.Objects.Path);
      if (this.Objects.Pen != null)
        this.Renderer.DrawPath(this.Objects.Pen, this.Objects.Path);
    }
    GdiApi.StrokeAndFillPath(this.Objects.Handle);
    this.Objects.Path = (GraphicsPath) null;
  }

  private void StrokePath(IntPtr ptr)
  {
    System.Type type = typeof (EMR_FILLPATH);
    EMR_FILLPATH structure = (EMR_FILLPATH) this.GetStructure(ptr, type);
    if (this.Objects.Path != null && !this.Objects.IsOpenPath && this.Objects.Pen != null && (structure.rclBounds.bottom >= 0 || !this.Objects.IsOpenPath))
    {
      this.Objects.Path.FillMode = this.Objects.FillMode;
      this.Renderer.DrawPath(this.Objects.Pen, this.Objects.Path);
    }
    GdiApi.StrokePath(this.Objects.Handle);
    this.Objects.Path = (GraphicsPath) null;
  }

  private void StretchDIBits(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_STRETCHDIBITS);
    EMR_STRETCHDIBITS structure1 = (EMR_STRETCHDIBITS) this.GetStructure(ptr, type1);
    int num1 = structure1.offBmiSrc - 2 * MetafileParser.IntSize;
    IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
    System.Type type2 = typeof (BITMAPINFOHEADER);
    BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
    int offBitsSrc = structure1.offBitsSrc;
    uint cbBitsSrc = structure1.cbBitsSrc;
    Rectangle rc = new Rectangle(structure1.xSrc, structure1.ySrc, structure1.cxSrc, structure1.cySrc);
    Rectangle destRect = Rectangle.Truncate(this.LPtoDP((RectangleF) new Rectangle(structure1.xDest, structure1.yDest, structure1.cxDest, structure1.cyDest)));
    Rectangle srcRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc));
    this.DrawImage(offBitsSrc, cbBitsSrc, ptr, num2, (RectangleF) destRect, (RectangleF) srcRect, (RASTER_CODE) structure1.dwRop, structure1.iUsageSrc);
  }

  private void BitBlt(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_BITBLT);
    EMR_BITBLT structure1 = (EMR_BITBLT) this.GetStructure(ptr, type1);
    int num1 = structure1.offBmiSrc - 2 * MetafileParser.IntSize;
    IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
    System.Type type2 = typeof (BITMAPINFOHEADER);
    BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
    int offBitsSrc = structure1.offBitsSrc;
    uint cbBitsSrc = structure1.cbBitsSrc;
    Rectangle rc = new Rectangle(structure1.xSrc, structure1.ySrc, structure1.cxDest, structure1.cyDest);
    Rectangle destRect = Rectangle.Truncate(this.LPtoDP((RectangleF) new Rectangle(structure1.xDest, structure1.yDest, structure1.cxDest, structure1.cyDest)));
    Rectangle srcRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc));
    this.DrawImage(offBitsSrc, cbBitsSrc, ptr, num2, (RectangleF) destRect, (RectangleF) srcRect, structure1.dwRop, structure1.iUsageSrc);
  }

  private void TransparentBlt(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_TRANSPARENTBLT);
    EMR_TRANSPARENTBLT structure1 = (EMR_TRANSPARENTBLT) this.GetStructure(ptr, type1);
    int num1 = structure1.offBmiSrc - 2 * MetafileParser.IntSize;
    IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
    System.Type type2 = typeof (BITMAPINFOHEADER);
    BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
    int offBitsSrc = structure1.offBitsSrc;
    uint cbBitsSrc = structure1.cbBitsSrc;
    Rectangle rc = new Rectangle(structure1.xSrc, structure1.ySrc, structure1.cxDest, structure1.cyDest);
    Rectangle destRect = Rectangle.Truncate(this.LPtoDP((RectangleF) new Rectangle(structure1.xDest, structure1.yDest, structure1.cxDest, structure1.cyDest)));
    Rectangle srcRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc));
    this.DrawTransparentImage(offBitsSrc, cbBitsSrc, ptr, num2, (RectangleF) destRect, (RectangleF) srcRect, structure1.iUsageSrc);
  }

  private void GradientFill(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int index1 = 0;
    int intSize = MetafileParser.IntSize;
    RectangleF rectangleF = (RectangleF) this.ReadRECT(data, ref index1);
    uint uint32_1 = BitConverter.ToUInt32(data, index1);
    index1 += intSize;
    uint uint32_2 = BitConverter.ToUInt32(data, index1);
    index1 += intSize;
    int int32_1 = BitConverter.ToInt32(data, index1);
    Trivertex[] trivertexArray = new Trivertex[(IntPtr) uint32_1];
    index1 += intSize;
    for (int index2 = 0; (long) index2 < (long) uint32_1; ++index2)
    {
      Trivertex trivertex = new Trivertex();
      int int32_2 = BitConverter.ToInt32(data, index1);
      index1 += intSize;
      int int32_3 = BitConverter.ToInt32(data, index1);
      index1 += intSize;
      PointF pointF = new PointF((float) int32_2, (float) int32_3);
      trivertex.x = int32_2;
      trivertex.y = int32_3;
      ushort num1 = (ushort) ((uint) BitConverter.ToUInt16(data, index1) >> 8);
      trivertex.red = num1;
      index1 += MetafileParser.ShortSize;
      ushort num2 = (ushort) ((uint) BitConverter.ToUInt16(data, index1) >> 8);
      trivertex.green = num2;
      index1 += MetafileParser.ShortSize;
      ushort num3 = (ushort) ((uint) BitConverter.ToUInt16(data, index1) >> 8);
      trivertex.blue = num3;
      index1 += MetafileParser.ShortSize;
      ushort uint16 = BitConverter.ToUInt16(data, index1);
      trivertex.alpha = uint16;
      index1 += MetafileParser.ShortSize;
      trivertexArray[index2] = trivertex;
    }
    if (int32_1 != 2)
      return;
    GRADIENTTRIANGLE[] gradienttriangleArray = new GRADIENTTRIANGLE[(IntPtr) uint32_2];
    for (int index3 = 0; (long) index3 < (long) uint32_2; ++index3)
    {
      gradienttriangleArray[index3].vertex1 = BitConverter.ToInt32(data, index1);
      index1 += intSize;
      gradienttriangleArray[index3].vertex2 = BitConverter.ToInt32(data, index1);
      index1 += intSize;
      gradienttriangleArray[index3].vertex3 = BitConverter.ToInt32(data, index1);
      index1 += intSize;
    }
    int vertex1_1 = gradienttriangleArray[0].vertex1;
    int vertex2_1 = gradienttriangleArray[0].vertex2;
    int vertex3_1 = gradienttriangleArray[0].vertex3;
    int vertex1_2 = gradienttriangleArray[1].vertex1;
    int vertex2_2 = gradienttriangleArray[1].vertex2;
    int vertex3_2 = gradienttriangleArray[1].vertex3;
    for (int index4 = 0; index4 < trivertexArray.Length - 1; ++index4)
    {
      Color color1 = Color.FromArgb((int) byte.MaxValue, (int) trivertexArray[trivertexArray.Length - 1].red, (int) trivertexArray[trivertexArray.Length - 1].green, (int) trivertexArray[trivertexArray.Length - 1].blue);
      Color color2 = Color.FromArgb((int) byte.MaxValue, (int) trivertexArray[index4].red, (int) trivertexArray[index4].green, (int) trivertexArray[index4].blue);
      this.Renderer.FillPath((Brush) new LinearGradientBrush(new PointF((float) trivertexArray[index4].x, (float) trivertexArray[index4].y), new PointF((float) trivertexArray[trivertexArray.Length - 1].x, (float) trivertexArray[trivertexArray.Length - 1].y), color1, color2), this.Objects.Path);
    }
  }

  private void DrawTransparentImage(
    int imageOffset,
    uint imgSize,
    IntPtr ptr,
    IntPtr bitmapInfoPtr,
    RectangleF destRect,
    RectangleF srcRect,
    int iUsageSrc)
  {
    Bitmap bitmap = (Bitmap) null;
    if (imageOffset > 0 && imgSize > 0U && (double) destRect.Width > 0.0 && (double) destRect.Height > 0.0)
      bitmap = this.GetBitmap(imageOffset, imgSize, ptr, bitmapInfoPtr, iUsageSrc);
    if ((double) destRect.Height > 0.0 && (double) destRect.Location.Y != 0.0 && bitmap != null)
      this.ImageRegions.Add(new ImageRegion(destRect.Location.Y, destRect.Height));
    bitmap.MakeTransparent();
    Image image = (Image) bitmap;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      bitmap.Save((Stream) memoryStream, ImageFormat.Png);
      memoryStream.Position = 0L;
      image = Image.FromStream((Stream) memoryStream);
    }
    if (image != null)
      this.Renderer.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
    bitmap?.Dispose();
    image?.Dispose();
  }

  private void StretchBlt(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_STRETCHBLT);
    EMR_STRETCHBLT structure1 = (EMR_STRETCHBLT) this.GetStructure(ptr, type1);
    int num1 = structure1.offBmiSrc - 2 * MetafileParser.IntSize;
    IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
    System.Type type2 = typeof (BITMAPINFOHEADER);
    BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
    int offBitsSrc = structure1.offBitsSrc;
    uint cbBitsSrc = structure1.cbBitsSrc;
    Rectangle rc = new Rectangle(structure1.xSrc, structure1.ySrc, structure1.cxSrc, structure1.cySrc);
    Rectangle destRect = Rectangle.Truncate(this.LPtoDP((RectangleF) new Rectangle(structure1.xDest, structure1.yDest, structure1.cxDest, structure1.cyDest)));
    Rectangle srcRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc));
    this.DrawImage(offBitsSrc, cbBitsSrc, ptr, num2, (RectangleF) destRect, (RectangleF) srcRect, structure1.dwRop, structure1.iUsageSrc);
  }

  private void MaskBlt(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_MASKBLT);
    EMR_MASKBLT structure1 = (EMR_MASKBLT) this.GetStructure(ptr, type1);
    int num1 = structure1.offBmiSrc - 2 * MetafileParser.IntSize;
    IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
    int offBitsSrc = structure1.offBitsSrc;
    uint cbBitsSrc = structure1.cbBitsSrc;
    Bitmap bitmap1 = (Bitmap) null;
    if (offBitsSrc > 0 && cbBitsSrc > 0U)
      bitmap1 = this.GetBitmap(offBitsSrc, cbBitsSrc, ptr, num2, structure1.iUsageSrc);
    int num3 = structure1.offBmiMask - 2 * MetafileParser.IntSize;
    num2 = new IntPtr(ptr.ToInt64() + (long) num3);
    System.Type type2 = typeof (BITMAPINFOHEADER);
    BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
    int offBitsMask = structure1.offBitsMask;
    uint cbBitsMask = structure1.cbBitsMask;
    Bitmap bitmap2 = (Bitmap) null;
    IntPtr zero = IntPtr.Zero;
    if (offBitsMask > 0 && cbBitsMask > 0U)
      bitmap2 = this.GetBitmap(offBitsMask, cbBitsMask, ptr, num2, 1);
    Rectangle rc1 = new Rectangle(structure1.xDest, structure1.yDest, structure1.cxDest, structure1.cyDest);
    Rectangle rc2 = new Rectangle(structure1.xSrc, structure1.ySrc, structure1.cxDest, structure1.cyDest);
    Rectangle rc3 = new Rectangle(structure1.xMask, structure1.yMask, structure1.cxDest, structure1.cyDest);
    Rectangle destRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc1));
    Rectangle srcRect = Rectangle.Truncate(this.LPtoDP((RectangleF) rc2));
    Rectangle.Truncate(this.LPtoDP((RectangleF) rc3));
    if (destRect.Height > 0)
      this.ImageRegions.Add(new ImageRegion((float) destRect.Location.Y, (float) destRect.Height));
    if (bitmap1 == null)
      return;
    this.Renderer.DrawImage((Image) bitmap1, this.Objects.Brush, (RectangleF) destRect, (RectangleF) srcRect, (uint) structure1.dwRop);
    bitmap1.Dispose();
    bitmap2?.Dispose();
  }

  private int MMToPoint(float mm, float dpi) => (int) Math.Round((double) mm / 25.4 * (double) dpi);

  private void ExtCreateFontIndirect(IntPtr ptr)
  {
    System.Type type = typeof (EMR_EXTCREATEFONTINDIRECTW);
    EMR_EXTCREATEFONTINDIRECTW structure = (EMR_EXTCREATEFONTINDIRECTW) this.GetStructure(ptr, type);
    string str = this.TrimFontName(structure.elfw.lfFaceName);
    float lfHeight = (float) structure.elfw.lfHeight;
    float emSize = this.m_mapMode != 2 ? this.GetFontSize(lfHeight) : (float) this.MMToPoint(lfHeight / 10f, this.m_objects.Resolution.X) * 1.33333337f;
    if ((double) emSize <= 0.0)
      return;
    FontStyle style = FontStyle.Regular;
    if (structure.elfw.lfItalic)
      style |= FontStyle.Italic;
    if (structure.elfw.lfUnderline)
      style |= FontStyle.Underline;
    if (structure.elfw.lfStrikeOut)
      style |= FontStyle.Strikeout;
    if (structure.elfw.lfWeight > FW_FONT_WEIGHT.FW_SEMIBOLD)
      style |= FontStyle.Bold;
    Font font = (Font) null;
    try
    {
      if (structure.elfw.lfCharSet > (byte) 1)
      {
        font = Font.FromLogFont((object) structure.elfw);
        if ((double) font.Size != (double) emSize)
        {
          if ((double) font.Size > (double) emSize)
            font = new Font(str, emSize, font.Style, GraphicsUnit.Point, structure.elfw.lfCharSet);
        }
      }
      else
      {
        font = new Font(str, emSize, style, GraphicsUnit.Point, structure.elfw.lfCharSet);
        if (font.Name != str)
        {
          str = str.Replace(style.ToString(), "").Trim(' ');
          font = new Font(str, emSize, style, GraphicsUnit.Point, structure.elfw.lfCharSet);
        }
      }
    }
    catch (ArgumentException ex)
    {
      if (ex.Message == "Font 'Monotype Corsiva' does not support style 'Regular'.")
        font = new Font(str, emSize, FontStyle.Italic);
      if (font == null)
      {
        if (str != string.Empty)
        {
          str = str.Replace(style.ToString(), "").Trim(' ');
          font = new Font(str, emSize, style, GraphicsUnit.Point, structure.elfw.lfCharSet);
        }
      }
    }
    int customFontIndex = this.Renderer.GetCustomFontIndex(str);
    if (customFontIndex > -1)
      font = new Font(this.Renderer.CustomFontCollection.FontCollection.Families[customFontIndex], emSize, style, GraphicsUnit.Point, structure.elfw.lfCharSet);
    this.m_selectedFont = font;
    this.Objects.SelectedObjects.AddObject((object) new FontEx(font, structure.elfw), structure.ihFonts);
  }

  private void ExtTextOut(IntPtr ptr, bool bIsUnicode)
  {
    System.Type type = typeof (EMR_EXTTEXTOUTA);
    EMR_EXTTEXTOUTA structure = (EMR_EXTTEXTOUTA) this.GetStructure(ptr, type);
    this.Renderer.Graphics.m_isEMF = true;
    RectangleF rclBounds = (RectangleF) structure.rclBounds;
    PointF ptlReference1 = (PointF) structure.emrtext.ptlReference;
    if ((double) ptlReference1.X > 0.0 && (double) ptlReference1.Y > 0.0)
      this.Renderer.isPositiveLogicPoints = true;
    if ((double) this.Objects.TextAngle > 360.0)
      this.Objects.TextAngle %= 360f;
    if (this.m_mapMode == 2)
    {
      if ((double) ptlReference1.Y < 0.0 && (double) ptlReference1.X >= 0.0 || (double) ptlReference1.X > 0.0 && (double) ptlReference1.Y > 0.0)
        this.Objects.TextAngle = -this.Objects.TextAngle;
    }
    else if ((double) this.Objects.TextAngle > 0.0 && (double) this.Objects.TextAngle != 360.0 && (double) ptlReference1.Y < 0.0 && (double) ptlReference1.X >= 0.0)
      this.Objects.TextAngle = -this.Objects.TextAngle;
    PointF pointF = this.LPtoDP(ptlReference1);
    int nChars = structure.emrtext.nChars;
    int num1 = structure.emrtext.offString - 2 * MetafileParser.IntSize;
    IntPtr source1 = new IntPtr(ptr.ToInt64() + (long) num1);
    if (structure.iGraphicsMode == 1)
      this.m_emfScalingFactor = new SizeF(structure.exScale, structure.eyScale);
    int length1 = bIsUnicode ? nChars * 2 : nChars;
    byte[] numArray = new byte[length1];
    Marshal.Copy(source1, numArray, 0, length1);
    float[] widths = (float[]) null;
    if (this.Renderer.m_EMFState && (double) this.Renderer.NativeGraphics.ClipBounds.X > 0.0)
      this.Renderer.NativeGraphics.ResetClip();
    if (structure.emrtext.offDx > 0)
    {
      int num2 = structure.emrtext.offDx - 2 * MetafileParser.IntSize;
      IntPtr source2 = new IntPtr(ptr.ToInt64() + (long) num2);
      int[] destination = new int[nChars];
      Marshal.Copy(source2, destination, 0, nChars);
      int length2 = destination.Length;
      widths = new float[length2];
      for (int index = 0; index < length2; ++index)
        widths[index] = this.LPtoDPWidth((float) destination[index]);
    }
    ETO fOptions = (ETO) structure.emrtext.fOptions;
    if ((fOptions & ETO.OPAQUE) == ETO.OPAQUE && (this.Objects.Brush as SolidBrush).Color.ToArgb() != this.Objects.BackColor.ToArgb())
    {
      using (Brush brush = (Brush) new SolidBrush(this.Objects.BackColor))
        this.Renderer.FillRectangles(brush, new RectangleF[1]
        {
          rclBounds
        });
    }
    string empty = string.Empty;
    string str = !bIsUnicode ? Encoding.UTF8.GetString(numArray) : Encoding.Unicode.GetString(numArray);
    switch (str)
    {
      case null:
        break;
      case "":
        break;
      default:
        Font font = this.m_selectedFont == null ? this.Objects.Font : this.m_selectedFont;
        OUTLINETEXTMETRIC fontMetrix = this.GetFontMetrix(font);
        PointF location = pointF;
        StringFormat stringFormat = this.GetStringFormat(str, fontMetrix, ref location);
        if ((stringFormat.FormatFlags & StringFormatFlags.DirectionRightToLeft) != StringFormatFlags.DirectionRightToLeft)
          rclBounds.Location = location;
        if ((fOptions & (ETO.OPAQUE | ETO.CLIPPED)) != (ETO) 0)
        {
          RectangleF rcl = (RectangleF) structure.emrtext.rcl;
          stringFormat.FormatFlags &= ~StringFormatFlags.NoClip;
          if ((fOptions & ETO.OPAQUE) != (ETO) 0)
          {
            using (Brush brush = (Brush) new SolidBrush(this.Objects.BackColor))
              this.Renderer.FillRectangles(brush, new RectangleF[1]
              {
                rclBounds
              });
          }
        }
        else
        {
          stringFormat.FormatFlags |= StringFormatFlags.NoClip;
          stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
          stringFormat.Trimming = StringTrimming.None;
        }
        if ((fOptions & ETO.GLYPH_INDEX) != (ETO) 0 && font != null)
        {
          string text = this.ConvertGlyphIndices(str, font);
          if (!this.IsComplexScript(text))
            str = text;
        }
        if ((fOptions & ETO.RTLREADING) != (ETO) 0)
          stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        else if ((fOptions & ETO.GLYPH_INDEX) != (ETO) 0 && (stringFormat.FormatFlags & StringFormatFlags.DirectionRightToLeft) != (StringFormatFlags) 0)
          stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
        if (!string.IsNullOrEmpty(str.Trim()))
        {
          if (this.Objects.BACKGROUNDMODE == 2 && (double) this.Objects.TextAngle == 0.0 && (double) this.TextAngle == 0.0)
          {
            using (Brush brush = (Brush) new SolidBrush(this.Objects.BackColor))
              this.Renderer.FillRectangles(brush, new RectangleF[1]
              {
                rclBounds
              });
          }
          this.DrawText(str, stringFormat, rclBounds, fontMetrix, widths);
          if ((double) rclBounds.Height > 0.0)
            this.TextRegions.Add(!this.Renderer.Graphics.m_isBaselineFormat ? new TextRegion(rclBounds.Location.Y, rclBounds.Height) : new TextRegion(rclBounds.Location.Y - font.Size, rclBounds.Height));
          if ((this.Objects.TextAlign & TA_TEXT_ALIGN.TA_UPDATECP) > TA_TEXT_ALIGN.TA_NOUPDATECP)
          {
            int num3 = structure.emrtext.offDx - 2 * MetafileParser.IntSize;
            IntPtr lpDx = new IntPtr(ptr.ToInt64() + (long) num3);
            POINT ptlReference2 = structure.emrtext.ptlReference;
            MetafileParser.CheckResult(GdiApi.ExtTextOut(this.Objects.Handle, ptlReference2.x, ptlReference2.y, structure.emrtext.fOptions, ref structure.emrtext.rcl, str, nChars, lpDx));
          }
        }
        stringFormat.Dispose();
        break;
    }
  }

  private bool IsComplexScript(string text)
  {
    foreach (char ch in text)
    {
      if (ch > 'ஂ' && ch < '౯' || ch > 'ऀ' && ch < '৻' || ch > 'ઁ' && ch < '૱' || ch > 'ಂ' && ch < 'ೲ')
        return true;
    }
    foreach (char ch in text)
    {
      if (ch != ' ')
        return false;
    }
    return true;
  }

  private void CreateDibPatternBrushPt(IntPtr ptr)
  {
    System.Type type1 = typeof (EMR_CREATEDIBPATTERNBRUSHPT);
    EMR_CREATEDIBPATTERNBRUSHPT structure1 = (EMR_CREATEDIBPATTERNBRUSHPT) this.GetStructure(ptr, type1);
    int num1 = structure1.offBmi - 2 * MetafileParser.IntSize;
    IntPtr num2 = new IntPtr(ptr.ToInt64() + (long) num1);
    System.Type type2 = typeof (BITMAPINFOHEADER);
    BITMAPINFOHEADER structure2 = (BITMAPINFOHEADER) this.GetStructure(num2, type2);
    int offBits = structure1.offBits;
    uint cbBits = structure1.cbBits;
    Rectangle rectangle = new Rectangle(0, 0, structure2.biWidth, Math.Abs(structure2.biHeight));
    Bitmap bitmap = this.GetBitmap(offBits, cbBits, ptr, num2, structure1.iUsage);
    if (bitmap == null)
      return;
    this.Objects.SelectedObjects.AddObject((object) new TextureBrush((Image) bitmap), structure1.ihBrush);
  }

  private void SetStretchBltMode(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    GdiApi.SetStretchBltMode(this.Objects.Handle, ((EMR_SELECTCLIPPATH) this.GetStructure(ptr, type)).iMode);
  }

  private void SetLayout(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SELECTCLIPPATH);
    GdiApi.SetLayout(this.Objects.Handle, ((EMR_SELECTCLIPPATH) this.GetStructure(ptr, type)).iMode);
  }

  private void SetPixelV(IntPtr ptr)
  {
    System.Type type = typeof (EMR_SETPIXELV);
    EMR_SETPIXELV structure = (EMR_SETPIXELV) this.GetStructure(ptr, type);
    Color color = ColorTranslator.FromWin32(structure.crColor);
    PointF pointF = this.LPtoDP((PointF) structure.ptlPixel);
    PointF[] points = new PointF[2]{ pointF, pointF };
    using (Pen pen = new Pen(color))
      this.Renderer.DrawLines(pen, points);
  }

  private void SetMetaRgn(IntPtr ptr) => GdiApi.SetMetaRgn(this.Objects.Handle);

  private bool IsColorVisible(EmfObjectData objectData)
  {
    bool flag = true;
    if (objectData != null && objectData.Pen != null)
    {
      Color color = objectData.Pen.Color;
      if (objectData.Pen.Color.A == (byte) 0)
        flag = false;
    }
    return flag;
  }

  private string ConvertGlyphIndices(string text, Font font)
  {
    UnicodeTrueTypeFont unicodeTrueTypeFont = new UnicodeTrueTypeFont(font, font.Size, CompositeFontType.Type0);
    unicodeTrueTypeFont.CreateInternals();
    TtfReader ttfReader = unicodeTrueTypeFont.TtfReader;
    string empty = string.Empty;
    foreach (char glyphIndex in text)
    {
      TtfGlyphInfo glyph = ttfReader.GetGlyph((int) glyphIndex);
      empty += (string) (object) (char) glyph.CharCode;
    }
    return empty;
  }

  private string TrimFontName(string fontName)
  {
    int length = fontName.IndexOf('(');
    if (length > 0)
    {
      fontName = fontName.Substring(0, length);
      fontName = fontName.Trim();
    }
    return fontName;
  }

  private void DumpData(byte[] data, EmfPlusRecordType type)
  {
    int num = 0;
    while (num < data.Length)
      ++num;
  }

  private ValueType GetStructure(IntPtr ptr, System.Type type)
  {
    return !(type == (System.Type) null) ? (ValueType) Marshal.PtrToStructure(ptr, type) : throw new ArgumentNullException(nameof (type));
  }

  private ValueType GetStructureEx(IntPtr ptr, ValueType structure)
  {
    object obj = (object) structure;
    System.Type type = structure.GetType();
    IntPtr ptr1 = new IntPtr(ptr.ToInt64());
    FieldInfo[] fields = type.GetFields();
    uint num1 = 0;
    int index1 = 0;
    for (int length = fields.Length; index1 < length; ++index1)
    {
      FieldInfo fieldInfo = fields[index1];
      System.Type fieldType = fieldInfo.FieldType;
      if (!fieldType.IsArray)
      {
        object structure1 = Marshal.PtrToStructure(ptr1, fieldType);
        fieldInfo.SetValue(obj, structure1, BindingFlags.Public, (Binder) null, CultureInfo.InvariantCulture);
        if (fieldType == typeof (uint))
          num1 = (uint) structure1;
        int num2 = Marshal.SizeOf(fieldType);
        ptr1 = new IntPtr(ptr1.ToInt64() + (long) num2);
      }
      else
      {
        System.Type elementType = fieldType.GetElementType();
        int num3 = Marshal.SizeOf(elementType);
        ArrayList arrayList = new ArrayList();
        for (int index2 = 0; (long) index2 < (long) num1; ++index2)
        {
          object structure2 = Marshal.PtrToStructure(ptr1, elementType);
          arrayList.Add(structure2);
          ptr1 = new IntPtr(ptr1.ToInt64() + (long) num3);
        }
        Array array = arrayList.ToArray(elementType);
        fieldInfo.SetValue(obj, (object) array, BindingFlags.Instance, (Binder) null, CultureInfo.InvariantCulture);
      }
    }
    structure = (ValueType) obj;
    return structure;
  }

  private PointF LPtoDP(PointF point)
  {
    POINT[] lpPoints = new POINT[1]
    {
      (POINT) Point.Truncate(point)
    };
    GdiApi.LPtoDP(this.Objects.Handle, lpPoints, 1);
    return (PointF) (Point) lpPoints[0];
  }

  public RectangleF LPtoDP(RectangleF rc)
  {
    return RectangleF.Empty with
    {
      Location = this.LPtoDP(rc.Location),
      Width = this.LPtoDPWidth(rc.Width),
      Height = this.LPtoDPHeight(rc.Height)
    };
  }

  private float LPtoDPX(float x)
  {
    POINT[] lpPoints = new POINT[1]
    {
      (POINT) new Point((int) x, (int) x)
    };
    GdiApi.LPtoDP(this.Objects.Handle, lpPoints, 1);
    return (float) ((Point) lpPoints[0]).X;
  }

  private float LPtoDPY(float y)
  {
    POINT[] lpPoints = new POINT[1]
    {
      (POINT) new Point((int) y, (int) y)
    };
    GdiApi.LPtoDP(this.Objects.Handle, lpPoints, 1);
    return (float) ((Point) lpPoints[0]).Y;
  }

  private float LPtoDPWidth(float logicValue)
  {
    return this.LPtoDP(new PointF(logicValue, logicValue)).X - this.LPtoDP(PointF.Empty).X;
  }

  private float LPtoDPHeight(float logicValue)
  {
    return this.LPtoDP(new PointF(logicValue, logicValue)).Y - this.LPtoDP(PointF.Empty).Y;
  }

  private PointF DPtoLP(PointF point)
  {
    POINT[] lpPoints = new POINT[1]
    {
      (POINT) Point.Truncate(point)
    };
    GdiApi.DPtoLP(this.Objects.Handle, lpPoints, 1);
    return (PointF) (Point) lpPoints[0];
  }

  private float DPtoLPWidth(float deviceValue)
  {
    return this.DPtoLP(new PointF(deviceValue, deviceValue)).X - this.DPtoLP(PointF.Empty).X;
  }

  private float DPtoLPHeight(float deviceValue)
  {
    return this.DPtoLP(new PointF(deviceValue, deviceValue)).Y - this.DPtoLP(PointF.Empty).Y;
  }

  private RectangleF DPtoLP(RectangleF rc)
  {
    return RectangleF.Empty with
    {
      Location = this.DPtoLP(rc.Location),
      Width = this.DPtoLPWidth(rc.Width),
      Height = this.DPtoLPHeight(rc.Height)
    };
  }

  private RECT ReadRECT(byte[] data, ref int index)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int intSize = MetafileParser.IntSize;
    int int32_1 = BitConverter.ToInt32(data, index);
    index += intSize;
    int int32_2 = BitConverter.ToInt32(data, index);
    index += intSize;
    int int32_3 = BitConverter.ToInt32(data, index);
    index += intSize;
    int int32_4 = BitConverter.ToInt32(data, index);
    index += intSize;
    return new RECT(int32_1, int32_2, int32_3, int32_4);
  }

  private int[] ReadInt32Array(byte[] data, int dataSize, ref int index)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int[] numArray = new int[dataSize];
    for (int index1 = 0; index1 < dataSize; ++index1)
    {
      int int32 = BitConverter.ToInt32(data, index);
      index += MetafileParser.IntSize;
      numArray[index1] = int32;
    }
    return numArray;
  }

  private Point[] ReadPointArray(byte[] data, int dataSize, ref int index, bool bIs32bit)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    Point[] pointArray = new Point[dataSize];
    for (int index1 = 0; index1 < dataSize; ++index1)
    {
      Point point = new Point(this.ReadInteger(data, ref index, bIs32bit), this.ReadInteger(data, ref index, bIs32bit));
      pointArray[index1] = !bIs32bit ? Point.Truncate(this.LPtoDP((PointF) point)) : Point.Truncate(this.LPtoDP((PointF) point));
    }
    return pointArray;
  }

  private int ReadInteger(byte[] data, ref int index, bool bIs32)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int num = bIs32 ? BitConverter.ToInt32(data, index) : (int) BitConverter.ToInt16(data, index);
    index += bIs32 ? MetafileParser.IntSize : MetafileParser.ShortSize;
    return num;
  }

  private void SetValidGraphicsMode()
  {
    int graphicsMode = GdiApi.GetGraphicsMode(this.Objects.Handle);
    if (graphicsMode <= 0 || graphicsMode == 2)
      return;
    GdiApi.SetGraphicsMode(this.Objects.Handle, 2);
  }

  private PointF[] ConvertType(POINTS[] points)
  {
    PointF[] pointFArray = points != null ? new PointF[points.Length] : throw new ArgumentNullException(nameof (points));
    int index = 0;
    for (int length = points.Length; index < length; ++index)
      pointFArray[index] = this.LPtoDP((PointF) points[index]);
    return pointFArray;
  }

  private PointF[] ConvertType(POINT[] points)
  {
    PointF[] pointFArray = points != null ? new PointF[points.Length] : throw new ArgumentNullException(nameof (points));
    int index = 0;
    for (int length = points.Length; index < length; ++index)
      pointFArray[index] = this.LPtoDP((PointF) points[index]);
    return pointFArray;
  }

  private POINT[] ConvertTypeEx(POINTS[] points)
  {
    POINT[] pointArray = points != null ? new POINT[points.Length] : throw new ArgumentNullException(nameof (points));
    int index = 0;
    for (int length = points.Length; index < length; ++index)
    {
      POINTS point = points[index];
      pointArray[index] = new POINT((int) point.x, (int) point.y);
    }
    return pointArray;
  }

  private PointF[] AddCurrentPointTo(PointF[] points)
  {
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    PointF currentPoint = this.Objects.CurrentPoint;
    PointF[] destinationArray = new PointF[points.Length + 1];
    destinationArray[0] = this.LPtoDP(this.Objects.CurrentPoint);
    Array.Copy((Array) points, 0, (Array) destinationArray, 1, points.Length);
    return destinationArray;
  }

  private Matrix ConvertMatrix(XFORM xMatrix)
  {
    return new Matrix(xMatrix.eM11, xMatrix.eM12, xMatrix.eM21, xMatrix.eM22, xMatrix.eDx, xMatrix.eDy);
  }

  private float GetAngle(float x0, float y0, float x1, float y1)
  {
    double num1 = Math.Sqrt(Math.Pow((double) x1 - (double) x0, 2.0) + Math.Pow((double) y1 - (double) y0, 2.0));
    float angle = 0.0f;
    if (num1 != 0.0)
    {
      double d = ((double) x1 - (double) x0) / num1;
      if ((d >= 1.0 || d <= -1.0) && d != 1.0 && d != -1.0)
        d = (double) (int) d;
      float num2 = (float) (Math.Acos(d) / Math.PI * 180.0);
      if ((double) y1 < (double) y0)
        num2 = -num2;
      angle = (double) num2 >= 0.0 ? num2 : 360f + num2;
    }
    return angle;
  }

  private PointF GetStartPoint(Rectangle bounds, Point radialPoint)
  {
    PointF currentPoint1 = this.Objects.CurrentPoint;
    GdiApi.ArcTo(this.Objects.Handle, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom, radialPoint.X, radialPoint.Y, radialPoint.X, radialPoint.Y);
    PointF currentPoint2 = this.Objects.CurrentPoint;
    this.Objects.CurrentPoint = currentPoint1;
    return currentPoint2;
  }

  private Image GetAlphaBlendedBitmap(
    int imageOffset,
    uint imgSize,
    IntPtr ptr,
    IntPtr bitmapInfoPtr,
    BITMAPINFOHEADER bmiHeader,
    int iUsageSrc)
  {
    imageOffset -= 2 * MetafileParser.IntSize;
    IntPtr source = new IntPtr(ptr.ToInt64() + (long) imageOffset);
    byte[] numArray = new byte[(IntPtr) imgSize];
    Marshal.Copy(source, numArray, 0, numArray.Length);
    Bitmap bitmap = new Bitmap(bmiHeader.biWidth, bmiHeader.biHeight, PixelFormat.Format32bppPArgb);
    BitmapData bitmapData = new BitmapData();
    Rectangle rect = new Rectangle(0, 0, bmiHeader.biWidth, bmiHeader.biHeight);
    bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb, bitmapData);
    Marshal.Copy(numArray, 0, bitmapData.Scan0, numArray.Length);
    bitmap.UnlockBits(bitmapData);
    bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
    MemoryStream memoryStream = new MemoryStream();
    bitmap.Save((Stream) memoryStream, ImageFormat.Png);
    bitmap.Dispose();
    return Image.FromStream((Stream) memoryStream);
  }

  private Bitmap GetBitmap(
    int imageOffset,
    uint imgSize,
    IntPtr ptr,
    IntPtr bitmapInfoPtr,
    int iUsageSrc)
  {
    imageOffset -= 2 * MetafileParser.IntSize;
    IntPtr source = new IntPtr(ptr.ToInt64() + (long) imageOffset);
    byte[] numArray = new byte[(IntPtr) imgSize];
    Marshal.Copy(source, numArray, 0, numArray.Length);
    IntPtr diBitmap = GdiApi.CreateDIBitmap(this.Objects.Handle, bitmapInfoPtr, 4U, numArray, bitmapInfoPtr, (uint) iUsageSrc);
    Bitmap bitmap = (Bitmap) null;
    if (diBitmap != IntPtr.Zero)
      bitmap = Image.FromHbitmap(diBitmap).Clone() as Bitmap;
    GdiApi.DeleteObject(diBitmap);
    return bitmap;
  }

  private IntPtr GetHBitmap(
    int imageOffset,
    uint imgSize,
    IntPtr ptr,
    IntPtr bitmapInfoPtr,
    int iUsageSrc)
  {
    imageOffset -= 2 * MetafileParser.IntSize;
    IntPtr source = new IntPtr(ptr.ToInt64() + (long) imageOffset);
    byte[] numArray = new byte[(IntPtr) imgSize];
    Marshal.Copy(source, numArray, 0, numArray.Length);
    return GdiApi.CreateDIBitmap(this.Objects.Handle, bitmapInfoPtr, 4U, numArray, bitmapInfoPtr, (uint) iUsageSrc);
  }

  private float GetFontSize(float logHeight)
  {
    logHeight = Math.Abs(logHeight);
    return Math.Abs(this.LPtoDPHeight(logHeight));
  }

  private OUTLINETEXTMETRIC GetFontMetrix(Font font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    OUTLINETEXTMETRIC fontMetrix = new OUTLINETEXTMETRIC();
    IntPtr hfont = font.ToHfont();
    using (Bitmap bitmap = new Bitmap(1, 1))
    {
      using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap))
      {
        IntPtr hdc = graphics.GetHdc();
        IntPtr hgdiobj = GdiApi.SelectObject(hdc, hfont);
        int outlineTextMetricsEx = GdiApi.GetOutlineTextMetricsEx(hdc, 0, IntPtr.Zero);
        if (outlineTextMetricsEx != 0)
        {
          IntPtr num = Marshal.AllocHGlobal(outlineTextMetricsEx);
          if (GdiApi.GetOutlineTextMetricsEx(hdc, outlineTextMetricsEx, num) != 0)
            fontMetrix = (OUTLINETEXTMETRIC) this.GetStructure(num, typeof (OUTLINETEXTMETRIC));
          Marshal.FreeHGlobal(num);
        }
        GdiApi.SelectObject(this.Objects.Handle, hgdiobj);
        GdiApi.DeleteObject(hfont);
        graphics.ReleaseHdc(hdc);
      }
    }
    return fontMetrix;
  }

  private StringFormat GetStringFormat(string text, OUTLINETEXTMETRIC metric, ref PointF location)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    SIZE lpSize = new SIZE();
    GdiApi.GetTextExtentPoint32(this.Objects.Handle, text, text.Length, out lpSize);
    float num1 = this.LPtoDPWidth((float) lpSize.cx);
    TA_TEXT_ALIGN textAlign = this.Objects.TextAlign;
    StringFormat stringFormat = StringFormat.GenericTypographic.Clone() as StringFormat;
    if ((textAlign & TA_TEXT_ALIGN.TA_UPDATECP) > TA_TEXT_ALIGN.TA_NOUPDATECP)
      location = this.LPtoDP(this.Objects.CurrentPoint);
    switch (textAlign & TA_TEXT_ALIGN.TA_CENTER)
    {
      case TA_TEXT_ALIGN.TA_NOUPDATECP:
        stringFormat.Alignment = StringAlignment.Near;
        break;
      case TA_TEXT_ALIGN.TA_RIGHT:
        stringFormat.Alignment = StringAlignment.Far;
        location.X -= num1;
        break;
      case TA_TEXT_ALIGN.TA_CENTER:
        stringFormat.Alignment = StringAlignment.Center;
        location.X -= num1 / 2f;
        break;
    }
    switch (textAlign & TA_TEXT_ALIGN.TA_BASELINE)
    {
      case TA_TEXT_ALIGN.TA_NOUPDATECP:
        this.Renderer.Graphics.m_isBaselineFormat = false;
        stringFormat.LineAlignment = StringAlignment.Near;
        break;
      case TA_TEXT_ALIGN.TA_BOTTOM:
        this.Renderer.Graphics.m_isBaselineFormat = false;
        float num2 = stringFormat.Alignment != StringAlignment.Near ? (float) (metric.otmrcFontBox.top - metric.otmrcFontBox.bottom) : (metric.otmrcFontBox.bottom <= 0 ? (float) (metric.otmrcFontBox.top + metric.otmrcFontBox.bottom) : (float) (metric.otmrcFontBox.top - metric.otmrcFontBox.bottom));
        location.Y -= num2;
        stringFormat.LineAlignment = StringAlignment.Far;
        break;
      case TA_TEXT_ALIGN.TA_BASELINE:
        if ((double) this.Objects.TextAngle != 0.0 || this.Renderer.m_taggedPDF)
        {
          Font selectedFont = this.m_selectedFont;
          float num3 = selectedFont.SizeInPoints / (float) selectedFont.FontFamily.GetEmHeight(selectedFont.Style) * (float) selectedFont.FontFamily.GetCellAscent(selectedFont.Style);
          location.Y -= num3;
          stringFormat.LineAlignment = StringAlignment.Center;
        }
        if (this.Renderer.m_taggedPDF)
        {
          this.Renderer.Graphics.m_isBaselineFormat = false;
          break;
        }
        this.Renderer.Graphics.m_isBaselineFormat = true;
        break;
    }
    if ((textAlign & TA_TEXT_ALIGN.TA_RTLREADING) > TA_TEXT_ALIGN.TA_NOUPDATECP)
      stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    return stringFormat;
  }

  private void DrawText(
    string text,
    StringFormat format,
    RectangleF bounds,
    OUTLINETEXTMETRIC metric,
    float[] widths)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (!this.m_emfScalingFactor.IsEmpty)
      this.Renderer.Graphics.m_emfScalingFactor = this.m_emfScalingFactor;
    Font font;
    if (this.m_selectedFont != null)
    {
      font = this.m_selectedFont;
      if (this.m_fontAngleWithID.ContainsKey(this.m_selectedId) && (double) this.Objects.TextAngle == 0.0)
      {
        this.Objects.TextAngle = this.m_fontAngleWithID[this.m_selectedId].Angle;
        font = this.m_fontAngleWithID[this.m_selectedId].Font;
      }
    }
    else
      font = this.Objects.Font;
    if (this.Objects.IsOpenPath && font != null)
    {
      this.Objects.Path.AddString(text, font.FontFamily, (int) font.Style, font.SizeInPoints, bounds.Location, format);
    }
    else
    {
      if (font == null)
        return;
      using (Brush brush = (Brush) new SolidBrush(this.Objects.ForeColor))
      {
        if (widths == null)
        {
          if ((double) this.TextAngle != 0.0)
            this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X - font.Size, bounds.Y - font.Size, bounds.Width, bounds.Height), format, this.TextAngle);
          else if ((double) this.Objects.TextAngle != 0.0)
            this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X - font.Size, bounds.Y - font.Size, bounds.Width, bounds.Height), format, this.Objects.TextAngle);
          else
            this.Renderer.DrawString(text, font, brush, bounds, format);
        }
        else
        {
          RectangleF rectangleF = bounds;
          bool flag = false;
          if (format.Alignment != StringAlignment.Near && (format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != (StringFormatFlags) 0)
          {
            flag = true;
            float num = 0.0f;
            foreach (float width in widths)
              num += width;
            if (format.Alignment == StringAlignment.Center)
            {
              rectangleF.X += (float) (((double) rectangleF.Width - (double) num) / 2.0);
              format.Alignment = StringAlignment.Near;
            }
            else if ((format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != (StringFormatFlags) 0 ? format.Alignment == StringAlignment.Near : format.Alignment == StringAlignment.Far)
              rectangleF.X += rectangleF.Width - num;
            format.Alignment = (format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != (StringFormatFlags) 0 ? StringAlignment.Far : StringAlignment.Near;
          }
          if ((double) this.TextAngle != 0.0 && (double) this.Objects.TextAngle == 0.0)
          {
            this.Renderer.Graphics.m_isBaselineFormat = false;
            if ((double) this.TextAngle < 0.0)
              this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X - font.Size, bounds.Y + font.Size, bounds.Width, bounds.Height), format, this.TextAngle);
            else
              this.Renderer.DrawString(text, font, brush, bounds, format, this.TextAngle);
          }
          else if ((double) this.TextAngle != 0.0 && (double) this.Objects.TextAngle != 0.0)
          {
            this.Renderer.Graphics.m_isBaselineFormat = false;
            float textAngle = (float) Math.Round((double) this.TextAngle, MidpointRounding.AwayFromZero) + this.Objects.TextAngle;
            if ((double) textAngle == 0.0)
              this.Renderer.DrawString(text, font, brush, bounds, format);
            else if (flag)
              this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X - font.Size, bounds.Y + font.Size, bounds.Width, bounds.Height), format, textAngle);
            else
              this.Renderer.DrawString(text, font, brush, bounds, format, textAngle);
          }
          else if ((double) this.Objects.TextAngle >= -180.0 && (double) this.Objects.TextAngle < -90.0)
          {
            this.Renderer.Graphics.m_isBaselineFormat = false;
            this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X, bounds.Y + (float) font.Height, bounds.Width, bounds.Height), format, this.Objects.TextAngle);
          }
          else if ((double) this.Objects.TextAngle < -180.0 && (double) this.Objects.TextAngle > -360.0)
          {
            this.Renderer.Graphics.m_isBaselineFormat = false;
            this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X + font.Size, bounds.Y + font.Size, bounds.Width, bounds.Height), format, this.Objects.TextAngle);
          }
          else if ((double) this.Objects.TextAngle >= -90.0 && (double) this.Objects.TextAngle != 0.0)
          {
            this.Renderer.Graphics.m_isBaselineFormat = false;
            if ((double) this.Objects.TextAngle != 360.0)
            {
              if ((double) this.Objects.TextAngle == -90.0 || (double) this.Objects.TextAngle == 90.0)
              {
                if (format.LineAlignment == StringAlignment.Center || format.LineAlignment == StringAlignment.Far)
                  this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X - font.Size, bounds.Y + font.Size, bounds.Width, bounds.Height), format, this.Objects.TextAngle);
                else
                  this.Renderer.DrawString(text, font, brush, bounds, format, this.Objects.TextAngle);
              }
              else
                this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X - font.Size / 2f, bounds.Y + font.Size / 4f, bounds.Width, bounds.Height), format, this.Objects.TextAngle);
            }
            else
              this.Renderer.DrawString(text, font, brush, new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height), format, this.Objects.TextAngle);
          }
          else
            this.Renderer.DrawString(text, font, brush, bounds, format);
        }
      }
    }
  }

  private MAPPING_MODE GetMapMode() => (MAPPING_MODE) GdiApi.GetMapMode(this.Objects.Handle);

  private byte[] GetData(IntPtr ptrData, int dataSize)
  {
    byte[] destination = new byte[dataSize];
    if (ptrData != IntPtr.Zero)
      Marshal.Copy(ptrData, destination, 0, dataSize);
    return destination;
  }

  private void DrawImage(
    int imageOffset,
    uint imgSize,
    IntPtr ptr,
    IntPtr bitmapInfoPtr,
    RectangleF destRect,
    RectangleF srcRect,
    RASTER_CODE dwRop,
    int iUsageSrc)
  {
    Bitmap bitmap = (Bitmap) null;
    float height = destRect.Height;
    if ((double) destRect.Height < 0.0)
    {
      destRect.Y += destRect.Height;
      destRect.Height = -destRect.Height;
    }
    if (imageOffset > 0 && imgSize > 0U && (double) destRect.Width > 0.0 && (double) destRect.Height > 0.0)
    {
      bitmap = this.GetBitmap(imageOffset, imgSize, ptr, bitmapInfoPtr, iUsageSrc);
      if ((double) height < 0.0)
        bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
    }
    if ((double) destRect.Height > 0.0 && (double) destRect.Location.Y != 0.0 && bitmap != null)
      this.ImageRegions.Add(new ImageRegion(destRect.Location.Y, destRect.Height));
    if (this.Objects.Brush is SolidBrush && (this.Objects.Brush as SolidBrush).Color.A == (byte) 0)
    {
      if (Enum.IsDefined(typeof (RASTER_CODE), (object) dwRop) && dwRop != RASTER_CODE.SRCCOPY)
        this.Renderer.DrawImage((Image) bitmap, this.Objects.Brush, destRect, srcRect, (uint) dwRop);
      else
        this.Renderer.DrawImage((Image) bitmap, destRect, srcRect, GraphicsUnit.Pixel);
    }
    else if (Enum.IsDefined(typeof (RASTER_CODE), (object) dwRop))
    {
      if (this.m_pattenPoints != null && RASTER_CODE.PATINVERT == dwRop && this.Objects.Brush is SolidBrush)
      {
        SolidBrush brush = this.Objects.Brush as SolidBrush;
        brush.Color = Color.FromArgb((int) ~brush.Color.R, (int) ~brush.Color.G, (int) ~brush.Color.B);
        PdfPath path = new PdfPath();
        path.AddLines(this.m_pattenPoints);
        RectangleF bounds = path.GetBounds();
        if ((double) bounds.X == (double) destRect.X && (double) bounds.Y == (double) destRect.Y)
        {
          this.Renderer.Graphics.Save();
          if (brush.Color == Color.White || brush.Color.Name == "ffffffff")
            this.Renderer.Graphics.SetTransparency(1f, 1f, PdfBlendMode.Darken);
          else
            this.Renderer.Graphics.SetTransparency(1f, 1f, PdfBlendMode.Lighten);
          this.Renderer.Graphics.DrawPath((PdfBrush) new PdfSolidBrush((PdfColor) brush.Color), path);
          this.Renderer.Graphics.Restore();
        }
      }
      else
        this.Renderer.DrawImage((Image) bitmap, this.Objects.Brush, destRect, srcRect, (uint) dwRop);
    }
    bitmap?.Dispose();
  }

  private float CalculateRotationAngle(EMR_MODIFYWORLDTRANSFORM recordData)
  {
    double eM12 = (double) recordData.xform.eM12;
    double d = Math.Asin(eM12);
    if (double.IsNaN(d))
      d = Math.Asin((double) (int) eM12);
    return (float) (d * 180.0 / Math.PI);
  }

  private void SetROP2(IntPtr ptrData)
  {
    System.Type type = typeof (EMR_EmfSetROP2);
    switch (((EMR_EmfSetROP2) this.GetStructure(ptrData, type)).rasterOpertation)
    {
      case RasterOperation.R2_NOP:
        if (this.Objects.Pen == null)
          break;
        this.Objects.Pen.Color = this.Objects.BackColor;
        break;
      case RasterOperation.R2_COPYPEN:
        if (this.Objects.Pen == null)
          break;
        this.Objects.Pen.Color = this.Objects.Pen.Color;
        break;
    }
  }
}
