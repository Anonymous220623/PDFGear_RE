// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.Connector
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class Connector : Shape, IConnector, IShape, ISlideItem
{
  private const double _defaultFirstSegLength = 20.0;
  private int _beginConnectedShapeId = -1;
  private int _beginConnectionSiteIndex = -1;
  private int _endConnectedShapeId = -1;
  private int _endConnectionSiteIndex = -1;
  private ConnectorType _type;
  private bool _isChanged;
  private PointF _runPoint;

  internal Connector(ShapeType type, BaseSlide baseSlide)
    : base(ShapeType.CxnSp, baseSlide)
  {
  }

  internal PointF RunPoint
  {
    get => this._runPoint;
    set => this._runPoint = value;
  }

  internal bool BeginConnected => this.BeginConnectedShape != null;

  public IShape BeginConnectedShape
  {
    get
    {
      return this._beginConnectedShapeId != -1 ? (IShape) this.BaseSlide.GetShapeWithId(this.BaseSlide.Shapes as Shapes, this._beginConnectedShapeId) : (IShape) null;
    }
  }

  public int BeginConnectionSiteIndex
  {
    get => this.BeginConnected ? this._beginConnectionSiteIndex : -1;
    internal set => this._beginConnectionSiteIndex = value;
  }

  internal bool EndConnected => this.EndConnectedShape != null;

  public IShape EndConnectedShape
  {
    get
    {
      return this._endConnectedShapeId != -1 ? (IShape) this.BaseSlide.GetShapeWithId(this.BaseSlide.Shapes as Shapes, this._endConnectedShapeId) : (IShape) null;
    }
  }

  public int EndConnectionSiteIndex
  {
    get => this.EndConnected ? this._endConnectionSiteIndex : -1;
    internal set => this._endConnectionSiteIndex = value;
  }

  public ConnectorType Type
  {
    get => this._type;
    set
    {
      this._isChanged = true;
      this._type = value;
    }
  }

  internal bool IsChanged
  {
    get => this._isChanged;
    set => this._isChanged = value;
  }

  public void BeginConnect(IShape connectedShape, int connectionSiteIndex)
  {
    if (connectedShape == null)
      throw new Exception("Shape is invalid");
    if (connectionSiteIndex >= connectedShape.ConnectionSiteCount)
      throw new ArgumentException("Index of connection site must be less than connectedShape.ConnectionSiteCount");
    this._isChanged = true;
    this._beginConnectedShapeId = (connectedShape as Shape).ShapeId;
    this._beginConnectionSiteIndex = connectionSiteIndex;
  }

  public void BeginDisconnect()
  {
    this._beginConnectedShapeId = -1;
    this._beginConnectionSiteIndex = -1;
  }

  public void EndConnect(IShape connectedShape, int connectionSiteIndex)
  {
    if (connectedShape == null)
      throw new Exception("Shape is invalid");
    if (connectionSiteIndex >= connectedShape.ConnectionSiteCount)
      throw new ArgumentException("Index of connection site must be less than connectedShape.ConnectionSiteCount");
    this._isChanged = true;
    this._endConnectedShapeId = (connectedShape as Shape).ShapeId;
    this._endConnectionSiteIndex = connectionSiteIndex;
  }

  public void EndDisconnect()
  {
    this._endConnectedShapeId = -1;
    this._endConnectionSiteIndex = -1;
  }

  public void Update()
  {
    if (!this.BeginConnected && !this.EndConnected)
      return;
    this.ShapeGuide.Clear();
    PointF sourcePoint = new PointF();
    PointF targetPoint = new PointF();
    if (this.BeginConnected && this.EndConnected)
    {
      sourcePoint = this.GetPositionsWithConnectionSite(this.BeginConnectedShape, this._beginConnectionSiteIndex);
      targetPoint = this.GetPositionsWithConnectionSite(this.EndConnectedShape, this._endConnectionSiteIndex);
    }
    else
    {
      this.GetXYFromBounds(this, ref sourcePoint, ref targetPoint);
      if (this.BeginConnected)
        sourcePoint = this.GetPositionsWithConnectionSite(this.BeginConnectedShape, this._beginConnectionSiteIndex);
      if (this.EndConnected)
        targetPoint = this.GetPositionsWithConnectionSite(this.EndConnectedShape, this._endConnectionSiteIndex);
    }
    if (this.BeginConnected)
      this.UpdateRotationAndFlip(this.BeginConnectedShape as Shape, ref sourcePoint);
    if (this.EndConnected)
      this.UpdateRotationAndFlip(this.EndConnectedShape as Shape, ref targetPoint);
    if (this._type == ConnectorType.Straight)
      this.UpdateConnectorBounds(this, sourcePoint.X, sourcePoint.Y, targetPoint.X, targetPoint.Y);
    else if (this._type == ConnectorType.Elbow || this._type == ConnectorType.Curve)
    {
      List<PointF> bentPoints = this.GetBentPoints(sourcePoint, targetPoint);
      if (bentPoints.Count == 2 || bentPoints.Count == 1 || this.BeginConnected && !this.EndConnected || !this.BeginConnected && this.EndConnected)
        this.UpdateConnectorBounds(this, sourcePoint.X, sourcePoint.Y, targetPoint.X, targetPoint.Y);
      else
        this.UpdateElbowConnectorBounds(this, sourcePoint.X, sourcePoint.Y, targetPoint.X, targetPoint.Y, bentPoints);
    }
    this.IsChanged = false;
  }

  internal void SetBeginShapeId(int id) => this._beginConnectedShapeId = id;

  internal void SetEndShapeId(int id) => this._endConnectedShapeId = id;

  internal void UpdateRotationAndFlip(Shape shape, ref PointF targetPoint)
  {
    if (shape.Rotation == 0 && !shape.ShapeFrame.FlipHorizontal && !shape.ShapeFrame.FlipVertical && (shape.Group == null || !shape.IsGroupFlipV(shape.Group) && !shape.IsGroupFlipH(shape.Group)))
      return;
    double rotation = (double) shape.Rotation;
    float left = (float) shape.Left;
    float top = (float) shape.Top;
    float width = (float) shape.Width;
    float height = (float) shape.Height;
    PointF s = new PointF(left + width / 2f, top + height / 2f);
    double num = Math.Atan2((double) targetPoint.Y - (double) s.Y, (double) targetPoint.X - (double) s.X) * 180.0 / Math.PI;
    double length = Math.Sqrt(Math.Pow((double) targetPoint.X - (double) s.X, 2.0) + Math.Pow((double) targetPoint.Y - (double) s.Y, 2.0));
    if (shape.Group == null)
    {
      if (shape.ShapeFrame.FlipVertical)
        num = 360.0 - num;
      if (shape.ShapeFrame.FlipHorizontal)
        num = 180.0 - num;
    }
    else
    {
      int flipVcount = shape.GetFlipVCount(shape.Group, shape.ShapeFrame.FlipVertical ? 1 : 0);
      int flipHcount = shape.GetFlipHCount(shape.Group, shape.ShapeFrame.FlipHorizontal ? 1 : 0);
      bool flag1 = flipVcount % 2 != 0;
      bool flag2 = flipHcount % 2 != 0;
      if (flag1)
        num = 360.0 - num;
      if (flag2)
        num = 180.0 - num;
    }
    double angle = rotation + num;
    PointF pointF = this.Transform(s, length, angle);
    targetPoint = pointF;
  }

  internal void GetXYFromBounds(Connector connector, ref PointF startPoint, ref PointF endPoint)
  {
    float left = (float) connector.Left;
    float top = (float) connector.Top;
    float width = (float) connector.Width;
    float height = (float) connector.Height;
    bool flipVertical = connector.ShapeFrame.FlipVertical;
    if (connector.ShapeFrame.FlipHorizontal)
    {
      endPoint.X = left;
      startPoint.X = width + endPoint.X;
    }
    else
    {
      startPoint.X = left;
      endPoint.X = width + startPoint.X;
    }
    if (flipVertical)
    {
      endPoint.Y = top;
      startPoint.Y = height + endPoint.Y;
    }
    else
    {
      startPoint.Y = top;
      endPoint.Y = height + startPoint.Y;
    }
  }

  internal void UpdateConnectorBounds(
    Connector connector,
    float beginX,
    float beginY,
    float endX,
    float endY)
  {
    bool flag1 = false;
    bool flag2 = false;
    int rotation = -1;
    float point1;
    float point2;
    if ((double) beginX <= (double) endX)
    {
      point1 = beginX;
      point2 = endX - beginX;
    }
    else
    {
      point1 = endX;
      point2 = beginX - endX;
      flag2 = true;
    }
    float point3;
    float point4;
    if ((double) beginY <= (double) endY)
    {
      point3 = beginY;
      point4 = endY - beginY;
    }
    else
    {
      point3 = endY;
      point4 = beginY - endY;
      flag1 = true;
    }
    long emuLong1 = Helper.PointToEmuLong((double) point1);
    long emuLong2 = Helper.PointToEmuLong((double) point3);
    long emuLong3 = Helper.PointToEmuLong((double) point2);
    long emuLong4 = Helper.PointToEmuLong((double) point4);
    if (rotation == 0)
      rotation = -1;
    connector.ShapeFrame.SetAnchor(new bool?(flag1), new bool?(flag2), rotation, emuLong1, emuLong2, emuLong3, emuLong4);
  }

  internal void UpdateElbowConnectorBounds(
    Connector connector,
    float beginX,
    float beginY,
    float endX,
    float endY,
    List<PointF> linePoints)
  {
    float point1 = 0.0f;
    float point2 = 0.0f;
    float src = 0.0f;
    float tar = 0.0f;
    bool flag1 = false;
    bool flag2 = false;
    int rotation = -1;
    if (connector.BeginConnected && connector.EndConnected)
    {
      switch (linePoints.Count)
      {
        case 3:
          if (connector.Type == ConnectorType.Elbow)
          {
            connector.AutoShapeType = AutoShapeType.BentConnector2;
            break;
          }
          connector.AutoShapeType = AutoShapeType.CurvedConnector2;
          break;
        case 5:
          if (connector.Type == ConnectorType.Elbow)
          {
            connector.AutoShapeType = AutoShapeType.BentConnector4;
            break;
          }
          connector.AutoShapeType = AutoShapeType.CurvedConnector4;
          break;
        case 6:
          if (connector.Type == ConnectorType.Elbow)
          {
            connector.AutoShapeType = AutoShapeType.BentConnector5;
            break;
          }
          connector.AutoShapeType = AutoShapeType.CurvedConnector5;
          break;
        default:
          if (connector.Type == ConnectorType.Elbow)
          {
            connector.AutoShapeType = AutoShapeType.ElbowConnector;
            break;
          }
          connector.AutoShapeType = AutoShapeType.CurvedConnector;
          break;
      }
      double x1 = (double) linePoints[0].X;
      PointF linePoint = linePoints[linePoints.Count - 1];
      double x2 = (double) linePoint.X;
      if (x1 > x2)
      {
        linePoint = linePoints[0];
        double x3 = (double) linePoint.X;
        linePoint = linePoints[linePoints.Count - 1];
        double x4 = (double) linePoint.X;
        src = (float) (x3 - x4);
      }
      else
      {
        linePoint = linePoints[linePoints.Count - 1];
        double x5 = (double) linePoint.X;
        linePoint = linePoints[0];
        double x6 = (double) linePoint.X;
        src = (float) (x5 - x6);
      }
      linePoint = linePoints[0];
      double y1 = (double) linePoint.Y;
      linePoint = linePoints[linePoints.Count - 1];
      double y2 = (double) linePoint.Y;
      if (y1 > y2)
      {
        linePoint = linePoints[0];
        double y3 = (double) linePoint.Y;
        linePoint = linePoints[linePoints.Count - 1];
        double y4 = (double) linePoint.Y;
        tar = (float) (y3 - y4);
      }
      else
      {
        linePoint = linePoints[linePoints.Count - 1];
        double y5 = (double) linePoint.Y;
        linePoint = linePoints[0];
        double y6 = (double) linePoint.Y;
        tar = (float) (y5 - y6);
      }
      if (connector.AutoShapeType == AutoShapeType.BentConnector2 || connector.AutoShapeType == AutoShapeType.CurvedConnector2)
      {
        linePoint = linePoints[0];
        int x7 = (int) linePoint.X;
        linePoint = linePoints[2];
        int x8 = (int) linePoint.X;
        if (x7 > x8)
        {
          linePoint = linePoints[0];
          int x9 = (int) linePoint.X;
          linePoint = linePoints[1];
          int x10 = (int) linePoint.X;
          if (x9 == x10)
          {
            linePoint = linePoints[1];
            int x11 = (int) linePoint.X;
            linePoint = linePoints[2];
            int x12 = (int) linePoint.X;
            if (x11 > x12)
            {
              linePoint = linePoints[1];
              int y7 = (int) linePoint.Y;
              linePoint = linePoints[2];
              int y8 = (int) linePoint.Y;
              if (y7 == y8)
              {
                linePoint = linePoints[0];
                int y9 = (int) linePoint.Y;
                linePoint = linePoints[1];
                int y10 = (int) linePoint.Y;
                if (y9 < y10)
                {
                  linePoint = linePoints[0];
                  int y11 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y12 = (int) linePoint.Y;
                  if (y11 < y12)
                  {
                    linePoint = linePoints[2];
                    double x13 = (double) linePoint.X + (double) src / 2.0;
                    linePoint = linePoints[0];
                    double y13 = (double) linePoint.Y + (double) tar / 2.0;
                    PointF pointF = this.Transform(new PointF((float) x13, (float) y13), (double) tar / 2.0, 180.0);
                    point1 = pointF.X;
                    point2 = pointF.Y - src / 2f;
                    rotation = 5400000;
                    this.Swap<float>(ref src, ref tar);
                    goto label_270;
                  }
                }
                linePoint = linePoints[0];
                int y14 = (int) linePoint.Y;
                linePoint = linePoints[1];
                int y15 = (int) linePoint.Y;
                if (y14 > y15)
                {
                  linePoint = linePoints[0];
                  int y16 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y17 = (int) linePoint.Y;
                  if (y16 > y17)
                  {
                    linePoint = linePoints[0];
                    double x14 = (double) linePoint.X + (double) src / 2.0;
                    linePoint = linePoints[1];
                    double y18 = (double) linePoint.Y + (double) tar / 2.0;
                    PointF pointF = this.Transform(new PointF((float) x14, (float) y18), (double) tar / 2.0, 180.0);
                    point1 = pointF.X - src;
                    point2 = pointF.Y - src / 2f;
                    rotation = 16200000;
                    flag1 = true;
                    this.Swap<float>(ref src, ref tar);
                    goto label_270;
                  }
                }
                this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                goto label_270;
              }
            }
          }
          linePoint = linePoints[0];
          int x15 = (int) linePoint.X;
          linePoint = linePoints[1];
          int x16 = (int) linePoint.X;
          if (x15 > x16)
          {
            linePoint = linePoints[1];
            int x17 = (int) linePoint.X;
            linePoint = linePoints[2];
            int x18 = (int) linePoint.X;
            if (x17 == x18)
            {
              linePoint = linePoints[0];
              int y19 = (int) linePoint.Y;
              linePoint = linePoints[1];
              int y20 = (int) linePoint.Y;
              if (y19 == y20)
              {
                linePoint = linePoints[0];
                int y21 = (int) linePoint.Y;
                linePoint = linePoints[2];
                int y22 = (int) linePoint.Y;
                if (y21 < y22)
                {
                  linePoint = linePoints[1];
                  int y23 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y24 = (int) linePoint.Y;
                  if (y23 < y24)
                  {
                    linePoint = linePoints[0];
                    point1 = linePoint.X - src;
                    linePoint = linePoints[0];
                    point2 = linePoint.Y;
                    rotation = 10800000;
                    flag1 = true;
                    goto label_270;
                  }
                }
                linePoint = linePoints[0];
                int y25 = (int) linePoint.Y;
                linePoint = linePoints[2];
                int y26 = (int) linePoint.Y;
                if (y25 > y26)
                {
                  linePoint = linePoints[1];
                  int y27 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y28 = (int) linePoint.Y;
                  if (y27 > y28)
                  {
                    linePoint = linePoints[2];
                    point1 = linePoint.X;
                    linePoint = linePoints[2];
                    point2 = linePoint.Y;
                    rotation = 10800000;
                    goto label_270;
                  }
                }
                this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                goto label_270;
              }
            }
          }
          this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
        }
        else
        {
          linePoint = linePoints[0];
          int x19 = (int) linePoint.X;
          linePoint = linePoints[2];
          int x20 = (int) linePoint.X;
          if (x19 < x20)
          {
            linePoint = linePoints[0];
            int x21 = (int) linePoint.X;
            linePoint = linePoints[1];
            int x22 = (int) linePoint.X;
            if (x21 < x22)
            {
              linePoint = linePoints[1];
              int x23 = (int) linePoint.X;
              linePoint = linePoints[2];
              int x24 = (int) linePoint.X;
              if (x23 == x24)
              {
                linePoint = linePoints[0];
                int y29 = (int) linePoint.Y;
                linePoint = linePoints[1];
                int y30 = (int) linePoint.Y;
                if (y29 == y30)
                {
                  linePoint = linePoints[0];
                  int y31 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y32 = (int) linePoint.Y;
                  if (y31 < y32)
                  {
                    linePoint = linePoints[1];
                    int y33 = (int) linePoint.Y;
                    linePoint = linePoints[2];
                    int y34 = (int) linePoint.Y;
                    if (y33 < y34)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int y35 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y36 = (int) linePoint.Y;
                  if (y35 > y36)
                  {
                    linePoint = linePoints[1];
                    int y37 = (int) linePoint.Y;
                    linePoint = linePoints[2];
                    int y38 = (int) linePoint.Y;
                    if (y37 > y38)
                    {
                      flag1 = true;
                      linePoint = linePoints[2];
                      point1 = linePoint.X - src;
                      linePoint = linePoints[2];
                      point2 = linePoint.Y;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
            }
            linePoint = linePoints[0];
            int x25 = (int) linePoint.X;
            linePoint = linePoints[1];
            int x26 = (int) linePoint.X;
            if (x25 == x26)
            {
              linePoint = linePoints[1];
              int x27 = (int) linePoint.X;
              linePoint = linePoints[2];
              int x28 = (int) linePoint.X;
              if (x27 < x28)
              {
                linePoint = linePoints[1];
                int y39 = (int) linePoint.Y;
                linePoint = linePoints[2];
                int y40 = (int) linePoint.Y;
                if (y39 == y40)
                {
                  linePoint = linePoints[0];
                  int y41 = (int) linePoint.Y;
                  linePoint = linePoints[1];
                  int y42 = (int) linePoint.Y;
                  if (y41 < y42)
                  {
                    linePoint = linePoints[0];
                    int y43 = (int) linePoint.Y;
                    linePoint = linePoints[2];
                    int y44 = (int) linePoint.Y;
                    if (y43 < y44)
                    {
                      linePoint = linePoints[0];
                      double x29 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y45 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x29, (float) y45), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      rotation = 16200000;
                      flag2 = true;
                      this.Swap<float>(ref src, ref tar);
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int y46 = (int) linePoint.Y;
                  linePoint = linePoints[1];
                  int y47 = (int) linePoint.Y;
                  if (y46 > y47)
                  {
                    linePoint = linePoints[0];
                    int y48 = (int) linePoint.Y;
                    linePoint = linePoints[2];
                    int y49 = (int) linePoint.Y;
                    if (y48 > y49)
                    {
                      linePoint = linePoints[1];
                      double x30 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[1];
                      double y50 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x30, (float) y50), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      rotation = 5400000;
                      flag2 = true;
                      flag1 = true;
                      this.Swap<float>(ref src, ref tar);
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
            }
            this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
          }
          else
            this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
        }
      }
      else if (connector.AutoShapeType == AutoShapeType.ElbowConnector || connector.AutoShapeType == AutoShapeType.CurvedConnector)
      {
        linePoint = linePoints[1];
        int x31 = (int) linePoint.X;
        linePoint = linePoints[2];
        int x32 = (int) linePoint.X;
        if (x31 == x32)
        {
          linePoint = linePoints[0];
          int y51 = (int) linePoint.Y;
          linePoint = linePoints[1];
          int y52 = (int) linePoint.Y;
          if (y51 == y52)
          {
            linePoint = linePoints[2];
            int y53 = (int) linePoint.Y;
            linePoint = linePoints[3];
            int y54 = (int) linePoint.Y;
            if (y53 == y54)
            {
              linePoint = linePoints[0];
              int x33 = (int) linePoint.X;
              linePoint = linePoints[1];
              int x34 = (int) linePoint.X;
              if (x33 > x34)
              {
                linePoint = linePoints[2];
                int x35 = (int) linePoint.X;
                linePoint = linePoints[3];
                int x36 = (int) linePoint.X;
                if (x35 > x36)
                {
                  linePoint = linePoints[1];
                  int y55 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y56 = (int) linePoint.Y;
                  if (y55 < y56)
                  {
                    linePoint = linePoints[0];
                    point1 = linePoint.X - src;
                    linePoint = linePoints[0];
                    point2 = linePoint.Y;
                    rotation = 10800000;
                    flag1 = true;
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int y57 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y58 = (int) linePoint.Y;
                  if (y57 > y58)
                  {
                    linePoint = linePoints[3];
                    point1 = linePoint.X;
                    linePoint = linePoints[3];
                    point2 = linePoint.Y;
                    rotation = 10800000;
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              linePoint = linePoints[0];
              int x37 = (int) linePoint.X;
              linePoint = linePoints[1];
              int x38 = (int) linePoint.X;
              if (x37 < x38)
              {
                linePoint = linePoints[2];
                int x39 = (int) linePoint.X;
                linePoint = linePoints[3];
                int x40 = (int) linePoint.X;
                if (x39 < x40)
                {
                  linePoint = linePoints[1];
                  int y59 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y60 = (int) linePoint.Y;
                  if (y59 > y60)
                  {
                    linePoint = linePoints[3];
                    point1 = linePoint.X - src;
                    linePoint = linePoints[3];
                    point2 = linePoint.Y;
                    flag1 = true;
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int y61 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y62 = (int) linePoint.Y;
                  if (y61 < y62)
                  {
                    linePoint = linePoints[0];
                    point1 = linePoint.X;
                    linePoint = linePoints[0];
                    point2 = linePoint.Y;
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              linePoint = linePoints[0];
              int x41 = (int) linePoint.X;
              linePoint = linePoints[1];
              int x42 = (int) linePoint.X;
              if (x41 < x42)
              {
                linePoint = linePoints[2];
                int x43 = (int) linePoint.X;
                linePoint = linePoints[3];
                int x44 = (int) linePoint.X;
                if (x43 > x44)
                {
                  linePoint = linePoints[1];
                  int y63 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y64 = (int) linePoint.Y;
                  if (y63 < y64)
                  {
                    linePoint = linePoints[0];
                    int x45 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x46 = (int) linePoint.X;
                    if (x45 > x46)
                    {
                      linePoint = linePoints[3];
                      point1 = linePoint.X;
                      linePoint = linePoints[3];
                      point2 = linePoint.Y - tar;
                      linePoint = linePoints[0];
                      double x47 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x48 = (double) linePoint.X;
                      double num = 100000.0 * (x47 - x48) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      flag2 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int x49 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x50 = (int) linePoint.X;
                    if (x49 < x50)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x51 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x52 = (double) linePoint.X;
                      double num = 100000.0 * (x51 - x52) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int y65 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y66 = (int) linePoint.Y;
                  if (y65 > y66)
                  {
                    linePoint = linePoints[0];
                    int x53 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x54 = (int) linePoint.X;
                    if (x53 > x54)
                    {
                      linePoint = linePoints[3];
                      point1 = linePoint.X;
                      linePoint = linePoints[3];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x55 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x56 = (double) linePoint.X;
                      double num = 100000.0 * (x55 - x56) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int x57 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x58 = (int) linePoint.X;
                    if (x57 < x58)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y - tar;
                      linePoint = linePoints[1];
                      double x59 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x60 = (double) linePoint.X;
                      double num = 100000.0 * (x59 - x60) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      flag1 = true;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              linePoint = linePoints[0];
              int x61 = (int) linePoint.X;
              linePoint = linePoints[1];
              int x62 = (int) linePoint.X;
              if (x61 > x62)
              {
                linePoint = linePoints[2];
                int x63 = (int) linePoint.X;
                linePoint = linePoints[3];
                int x64 = (int) linePoint.X;
                if (x63 < x64)
                {
                  linePoint = linePoints[1];
                  int y67 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y68 = (int) linePoint.Y;
                  if (y67 < y68)
                  {
                    linePoint = linePoints[0];
                    int x65 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x66 = (int) linePoint.X;
                    if (x65 > x66)
                    {
                      linePoint = linePoints[3];
                      point1 = linePoint.X;
                      linePoint = linePoints[3];
                      point2 = linePoint.Y - tar;
                      linePoint = linePoints[0];
                      double x67 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x68 = (double) linePoint.X;
                      double num = 100000.0 * (x67 - x68) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 10800000;
                      flag1 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int x69 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x70 = (int) linePoint.X;
                    if (x69 < x70)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x71 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x72 = (double) linePoint.X;
                      double num = 100000.0 * (x71 - x72) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 10800000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int y69 = (int) linePoint.Y;
                  linePoint = linePoints[2];
                  int y70 = (int) linePoint.Y;
                  if (y69 > y70)
                  {
                    linePoint = linePoints[0];
                    int x73 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x74 = (int) linePoint.X;
                    if (x73 > x74)
                    {
                      linePoint = linePoints[3];
                      point1 = linePoint.X;
                      linePoint = linePoints[3];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x75 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x76 = (double) linePoint.X;
                      double num = 100000.0 * (x75 - x76) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 10800000;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int x77 = (int) linePoint.X;
                    linePoint = linePoints[3];
                    int x78 = (int) linePoint.X;
                    if (x77 < x78)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y - tar;
                      linePoint = linePoints[1];
                      double x79 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x80 = (double) linePoint.X;
                      double num = 100000.0 * (x79 - x80) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 10800000;
                      flag2 = true;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
                goto label_270;
              }
              goto label_270;
            }
          }
        }
        linePoint = linePoints[0];
        int x81 = (int) linePoint.X;
        linePoint = linePoints[1];
        int x82 = (int) linePoint.X;
        if (x81 == x82)
        {
          linePoint = linePoints[2];
          int x83 = (int) linePoint.X;
          linePoint = linePoints[3];
          int x84 = (int) linePoint.X;
          if (x83 == x84)
          {
            linePoint = linePoints[1];
            int y71 = (int) linePoint.Y;
            linePoint = linePoints[2];
            int y72 = (int) linePoint.Y;
            if (y71 == y72)
            {
              linePoint = linePoints[0];
              int y73 = (int) linePoint.Y;
              linePoint = linePoints[1];
              int y74 = (int) linePoint.Y;
              if (y73 > y74)
              {
                linePoint = linePoints[2];
                int y75 = (int) linePoint.Y;
                linePoint = linePoints[3];
                int y76 = (int) linePoint.Y;
                if (y75 > y76)
                {
                  linePoint = linePoints[1];
                  int x85 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x86 = (int) linePoint.X;
                  if (x85 < x86)
                  {
                    linePoint = linePoints[0];
                    double x87 = (double) linePoint.X + (double) src / 2.0;
                    linePoint = linePoints[0];
                    double y77 = (double) linePoint.Y - (double) tar / 2.0;
                    PointF pointF = this.Transform(new PointF((float) x87, (float) y77), (double) src / 2.0, 270.0);
                    point1 = pointF.X - tar / 2f;
                    point2 = pointF.Y;
                    this.Swap<float>(ref src, ref tar);
                    rotation = 5400000;
                    flag2 = true;
                    flag1 = true;
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int x88 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x89 = (int) linePoint.X;
                  if (x88 > x89)
                  {
                    linePoint = linePoints[0];
                    double x90 = (double) linePoint.X - (double) src / 2.0;
                    linePoint = linePoints[0];
                    double y78 = (double) linePoint.Y - (double) tar / 2.0;
                    PointF pointF = this.Transform(new PointF((float) x90, (float) y78), (double) src / 2.0, 270.0);
                    point1 = pointF.X - tar / 2f;
                    point2 = pointF.Y;
                    this.Swap<float>(ref src, ref tar);
                    rotation = 16200000;
                    flag1 = true;
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              linePoint = linePoints[0];
              int y79 = (int) linePoint.Y;
              linePoint = linePoints[1];
              int y80 = (int) linePoint.Y;
              if (y79 < y80)
              {
                linePoint = linePoints[2];
                int y81 = (int) linePoint.Y;
                linePoint = linePoints[3];
                int y82 = (int) linePoint.Y;
                if (y81 < y82)
                {
                  linePoint = linePoints[1];
                  int x91 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x92 = (int) linePoint.X;
                  if (x91 > x92)
                  {
                    linePoint = linePoints[0];
                    double x93 = (double) linePoint.X - (double) src / 2.0;
                    linePoint = linePoints[0];
                    double y83 = (double) linePoint.Y + (double) tar / 2.0;
                    PointF pointF = this.Transform(new PointF((float) x93, (float) y83), (double) src / 2.0, 270.0);
                    point1 = pointF.X - tar / 2f;
                    point2 = pointF.Y;
                    this.Swap<float>(ref src, ref tar);
                    rotation = 5400000;
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int x94 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x95 = (int) linePoint.X;
                  if (x94 < x95)
                  {
                    linePoint = linePoints[0];
                    double x96 = (double) linePoint.X + (double) src / 2.0;
                    linePoint = linePoints[0];
                    double y84 = (double) linePoint.Y + (double) tar / 2.0;
                    PointF pointF = this.Transform(new PointF((float) x96, (float) y84), (double) src / 2.0, 270.0);
                    point1 = pointF.X - tar / 2f;
                    point2 = pointF.Y;
                    this.Swap<float>(ref src, ref tar);
                    rotation = 16200000;
                    flag2 = true;
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              linePoint = linePoints[0];
              int y85 = (int) linePoint.Y;
              linePoint = linePoints[1];
              int y86 = (int) linePoint.Y;
              if (y85 < y86)
              {
                linePoint = linePoints[2];
                int y87 = (int) linePoint.Y;
                linePoint = linePoints[3];
                int y88 = (int) linePoint.Y;
                if (y87 > y88)
                {
                  linePoint = linePoints[1];
                  int x97 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x98 = (int) linePoint.X;
                  if (x97 < x98)
                  {
                    linePoint = linePoints[0];
                    int y89 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y90 = (int) linePoint.Y;
                    if (y89 > y90)
                    {
                      linePoint = linePoints[3];
                      double x99 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[3];
                      double y91 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x99, (float) y91), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y92 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y93 = (double) linePoint.Y;
                      double num = 100000.0 * (y92 - y93) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 5400000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int y94 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y95 = (int) linePoint.Y;
                    if (y94 < y95)
                    {
                      linePoint = linePoints[0];
                      double x100 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y96 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x100, (float) y96), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y97 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y98 = (double) linePoint.Y;
                      double num = 100000.0 * (y97 - y98) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 16200000;
                      flag2 = true;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int x101 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x102 = (int) linePoint.X;
                  if (x101 > x102)
                  {
                    linePoint = linePoints[0];
                    int y99 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y100 = (int) linePoint.Y;
                    if (y99 > y100)
                    {
                      linePoint = linePoints[3];
                      double x103 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[3];
                      double y101 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x103, (float) y101), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y102 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y103 = (double) linePoint.Y;
                      double num = 100000.0 * (y102 - y103) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 5400000;
                      flag2 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int y104 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y105 = (int) linePoint.Y;
                    if (y104 < y105)
                    {
                      linePoint = linePoints[0];
                      double x104 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y106 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x104, (float) y106), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y107 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y108 = (double) linePoint.Y;
                      double num = 100000.0 * (y107 - y108) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 5400000;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              linePoint = linePoints[0];
              int y109 = (int) linePoint.Y;
              linePoint = linePoints[1];
              int y110 = (int) linePoint.Y;
              if (y109 > y110)
              {
                linePoint = linePoints[2];
                int y111 = (int) linePoint.Y;
                linePoint = linePoints[3];
                int y112 = (int) linePoint.Y;
                if (y111 < y112)
                {
                  linePoint = linePoints[1];
                  int x105 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x106 = (int) linePoint.X;
                  if (x105 < x106)
                  {
                    linePoint = linePoints[0];
                    int y113 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y114 = (int) linePoint.Y;
                    if (y113 > y114)
                    {
                      linePoint = linePoints[0];
                      double x107 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y115 = (double) linePoint.Y - (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x107, (float) y115), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y116 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y117 = (double) linePoint.Y;
                      double num = 100000.0 * (y116 - y117) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 5400000;
                      flag1 = true;
                      flag2 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int y118 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y119 = (int) linePoint.Y;
                    if (y118 < y119)
                    {
                      linePoint = linePoints[0];
                      double x108 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y120 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x108, (float) y120), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y121 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y122 = (double) linePoint.Y;
                      double num = 100000.0 * (y121 - y122) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 16200000;
                      flag2 = true;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  linePoint = linePoints[1];
                  int x109 = (int) linePoint.X;
                  linePoint = linePoints[2];
                  int x110 = (int) linePoint.X;
                  if (x109 > x110)
                  {
                    linePoint = linePoints[0];
                    int y123 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y124 = (int) linePoint.Y;
                    if (y123 > y124)
                    {
                      linePoint = linePoints[0];
                      double x111 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y125 = (double) linePoint.Y - (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x111, (float) y125), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y126 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y127 = (double) linePoint.Y;
                      double num = 100000.0 * (y126 - y127) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 16200000;
                      flag1 = true;
                      goto label_270;
                    }
                    linePoint = linePoints[0];
                    int y128 = (int) linePoint.Y;
                    linePoint = linePoints[3];
                    int y129 = (int) linePoint.Y;
                    if (y128 < y129)
                    {
                      linePoint = linePoints[0];
                      double x112 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y130 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x112, (float) y130), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y131 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y132 = (double) linePoint.Y;
                      double num = 100000.0 * (y131 - y132) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num));
                      rotation = 16200000;
                      flag1 = true;
                      flag2 = true;
                      goto label_270;
                    }
                    this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                    goto label_270;
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
              this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
              goto label_270;
            }
          }
        }
        this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
      }
      else if (connector.AutoShapeType == AutoShapeType.BentConnector4 || connector.AutoShapeType == AutoShapeType.CurvedConnector4)
      {
        linePoint = linePoints[0];
        int x113 = (int) linePoint.X;
        linePoint = linePoints[1];
        int x114 = (int) linePoint.X;
        if (x113 == x114)
        {
          linePoint = linePoints[2];
          int x115 = (int) linePoint.X;
          linePoint = linePoints[3];
          int x116 = (int) linePoint.X;
          if (x115 == x116)
          {
            linePoint = linePoints[1];
            int y133 = (int) linePoint.Y;
            linePoint = linePoints[2];
            int y134 = (int) linePoint.Y;
            if (y133 == y134)
            {
              linePoint = linePoints[3];
              int y135 = (int) linePoint.Y;
              linePoint = linePoints[4];
              int y136 = (int) linePoint.Y;
              if (y135 == y136)
              {
                linePoint = linePoints[2];
                int y137 = (int) linePoint.Y;
                linePoint = linePoints[3];
                int y138 = (int) linePoint.Y;
                if (y137 < y138)
                {
                  linePoint = linePoints[0];
                  int x117 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x118 = (int) linePoint.X;
                  if (x117 < x118)
                  {
                    linePoint = linePoints[0];
                    int y139 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y140 = (int) linePoint.Y;
                    if (y139 < y140)
                    {
                      linePoint = linePoints[0];
                      double x119 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y141 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x119, (float) y141), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y142 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y143 = (double) linePoint.Y;
                      double num1 = 100000.0 * (y142 - y143) / (double) src;
                      linePoint = linePoints[2];
                      double x120 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x121 = (double) linePoint.X;
                      double num2 = 100000.0 * (x120 - x121) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num1));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num2));
                      rotation = 16200000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x122 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x123 = (int) linePoint.X;
                  if (x122 < x123)
                  {
                    linePoint = linePoints[0];
                    int y144 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y145 = (int) linePoint.Y;
                    if (y144 > y145)
                    {
                      linePoint = linePoints[0];
                      double x124 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y146 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x124, (float) y146), (double) src / 2.0, 270.0);
                      point1 = pointF.X - tar / 2f;
                      point2 = pointF.Y - tar;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y147 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y148 = (double) linePoint.Y;
                      double num3 = 100000.0 * (y147 - y148) / (double) src;
                      linePoint = linePoints[2];
                      double x125 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x126 = (double) linePoint.X;
                      double num4 = 100000.0 * (x125 - x126) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num3));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num4));
                      rotation = 5400000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x127 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x128 = (int) linePoint.X;
                  if (x127 > x128)
                  {
                    linePoint = linePoints[0];
                    int y149 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y150 = (int) linePoint.Y;
                    if (y149 > y150)
                    {
                      linePoint = linePoints[0];
                      double x129 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y151 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x129, (float) y151), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - (tar + src / 2f);
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y152 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y153 = (double) linePoint.Y;
                      double num5 = 100000.0 * (y152 - y153) / (double) src;
                      linePoint = linePoints[1];
                      double x130 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x131 = (double) linePoint.X;
                      double num6 = 100000.0 * (x130 - x131) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num5));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num6));
                      rotation = 16200000;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x132 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x133 = (int) linePoint.X;
                  if (x132 > x133)
                  {
                    linePoint = linePoints[0];
                    int y154 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y155 = (int) linePoint.Y;
                    if (y154 < y155)
                    {
                      linePoint = linePoints[0];
                      double x134 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y156 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x134, (float) y156), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y157 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y158 = (double) linePoint.Y;
                      double num7 = 100000.0 * (y157 - y158) / (double) src;
                      linePoint = linePoints[1];
                      double x135 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x136 = (double) linePoint.X;
                      double num8 = 100000.0 * (x135 - x136) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num7));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num8));
                      rotation = 16200000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
                linePoint = linePoints[2];
                int y159 = (int) linePoint.Y;
                linePoint = linePoints[3];
                int y160 = (int) linePoint.Y;
                if (y159 > y160)
                {
                  linePoint = linePoints[0];
                  int x137 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x138 = (int) linePoint.X;
                  if (x137 < x138)
                  {
                    linePoint = linePoints[0];
                    int y161 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y162 = (int) linePoint.Y;
                    if (y161 > y162)
                    {
                      linePoint = linePoints[0];
                      double x139 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y163 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x139, (float) y163), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - (tar + src / 2f);
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y164 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y165 = (double) linePoint.Y;
                      double num9 = 100000.0 * (y164 - y165) / (double) src;
                      linePoint = linePoints[2];
                      double x140 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x141 = (double) linePoint.X;
                      double num10 = 100000.0 * (x140 - x141) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num9));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num10));
                      rotation = 5400000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x142 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x143 = (int) linePoint.X;
                  if (x142 < x143)
                  {
                    linePoint = linePoints[0];
                    int y166 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y167 = (int) linePoint.Y;
                    if (y166 < y167)
                    {
                      linePoint = linePoints[0];
                      double x144 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y168 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x144, (float) y168), (double) src / 2.0, 270.0);
                      point1 = pointF.X - tar / 2f;
                      point2 = pointF.Y;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y169 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y170 = (double) linePoint.Y;
                      double num11 = 100000.0 * (y169 - y170) / (double) src;
                      linePoint = linePoints[2];
                      double x145 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x146 = (double) linePoint.X;
                      double num12 = 100000.0 * (x145 - x146) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num11));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num12));
                      rotation = 16200000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x147 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x148 = (int) linePoint.X;
                  if (x147 > x148)
                  {
                    linePoint = linePoints[0];
                    int y171 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y172 = (int) linePoint.Y;
                    if (y171 > y172)
                    {
                      linePoint = linePoints[0];
                      double x149 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y173 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x149, (float) y173), (double) tar / 2.0, 180.0);
                      point1 = pointF.X - src;
                      point2 = pointF.Y - (tar + src / 2f);
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y174 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y175 = (double) linePoint.Y;
                      double num13 = 100000.0 * (y174 - y175) / (double) src;
                      linePoint = linePoints[1];
                      double x150 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x151 = (double) linePoint.X;
                      double num14 = 100000.0 * (x150 - x151) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num13));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num14));
                      rotation = 5400000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x152 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x153 = (int) linePoint.X;
                  if (x152 > x153)
                  {
                    linePoint = linePoints[0];
                    int y176 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y177 = (int) linePoint.Y;
                    if (y176 < y177)
                    {
                      linePoint = linePoints[0];
                      double x154 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y178 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x154, (float) y178), (double) src / 2.0, 270.0);
                      point1 = pointF.X - (src + tar / 2f);
                      point2 = pointF.Y;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y179 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y180 = (double) linePoint.Y;
                      double num15 = 100000.0 * (y179 - y180) / (double) src;
                      linePoint = linePoints[1];
                      double x155 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x156 = (double) linePoint.X;
                      double num16 = 100000.0 * (x155 - x156) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num15));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num16));
                      rotation = 5400000;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
                this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                goto label_270;
              }
            }
          }
        }
        linePoint = linePoints[1];
        int x157 = (int) linePoint.X;
        linePoint = linePoints[2];
        int x158 = (int) linePoint.X;
        if (x157 == x158)
        {
          linePoint = linePoints[3];
          int x159 = (int) linePoint.X;
          linePoint = linePoints[4];
          int x160 = (int) linePoint.X;
          if (x159 == x160)
          {
            linePoint = linePoints[0];
            int y181 = (int) linePoint.Y;
            linePoint = linePoints[1];
            int y182 = (int) linePoint.Y;
            if (y181 == y182)
            {
              linePoint = linePoints[2];
              int y183 = (int) linePoint.Y;
              linePoint = linePoints[3];
              int y184 = (int) linePoint.Y;
              if (y183 == y184)
              {
                linePoint = linePoints[2];
                int x161 = (int) linePoint.X;
                linePoint = linePoints[3];
                int x162 = (int) linePoint.X;
                if (x161 > x162)
                {
                  linePoint = linePoints[0];
                  int x163 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x164 = (int) linePoint.X;
                  if (x163 < x164)
                  {
                    linePoint = linePoints[0];
                    int y185 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y186 = (int) linePoint.Y;
                    if (y185 < y186)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x165 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x166 = (double) linePoint.X;
                      double num17 = 100000.0 * (x165 - x166) / (double) src;
                      linePoint = linePoints[2];
                      double y187 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y188 = (double) linePoint.Y;
                      double num18 = 100000.0 * (y187 - y188) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num17));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num18));
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x167 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x168 = (int) linePoint.X;
                  if (x167 > x168)
                  {
                    linePoint = linePoints[0];
                    int y189 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y190 = (int) linePoint.Y;
                    if (y189 > y190)
                    {
                      linePoint = linePoints[4];
                      point1 = linePoint.X;
                      linePoint = linePoints[4];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x169 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x170 = (double) linePoint.X;
                      double num19 = 100000.0 * (x169 - x170) / (double) src;
                      linePoint = linePoints[1];
                      double y191 = (double) linePoint.Y;
                      linePoint = linePoints[2];
                      double y192 = (double) linePoint.Y;
                      double num20 = 100000.0 * (y191 - y192) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num19));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num20));
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x171 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x172 = (int) linePoint.X;
                  if (x171 > x172)
                  {
                    linePoint = linePoints[0];
                    int y193 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y194 = (int) linePoint.Y;
                    if (y193 < y194)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X - src;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x173 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x174 = (double) linePoint.X;
                      double num21 = 100000.0 * (x173 - x174) / (double) src;
                      linePoint = linePoints[2];
                      double y195 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y196 = (double) linePoint.Y;
                      double num22 = 100000.0 * (y195 - y196) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num21));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num22));
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x175 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x176 = (int) linePoint.X;
                  if (x175 < x176)
                  {
                    linePoint = linePoints[0];
                    int y197 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y198 = (int) linePoint.Y;
                    if (y197 > y198)
                    {
                      linePoint = linePoints[4];
                      point1 = linePoint.X - src;
                      linePoint = linePoints[4];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x177 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x178 = (double) linePoint.X;
                      double num23 = 100000.0 * (x177 - x178) / (double) src;
                      linePoint = linePoints[1];
                      double y199 = (double) linePoint.Y;
                      linePoint = linePoints[2];
                      double y200 = (double) linePoint.Y;
                      double num24 = 100000.0 * (y199 - y200) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num23));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num24));
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
                linePoint = linePoints[2];
                int x179 = (int) linePoint.X;
                linePoint = linePoints[3];
                int x180 = (int) linePoint.X;
                if (x179 < x180)
                {
                  linePoint = linePoints[0];
                  int x181 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x182 = (int) linePoint.X;
                  if (x181 < x182)
                  {
                    linePoint = linePoints[0];
                    int y201 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y202 = (int) linePoint.Y;
                    if (y201 < y202)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x183 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x184 = (double) linePoint.X;
                      double num25 = 100000.0 * (x183 - x184) / (double) src;
                      linePoint = linePoints[2];
                      double y203 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y204 = (double) linePoint.Y;
                      double num26 = 100000.0 * (y203 - y204) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num25));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num26));
                      rotation = 10800000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x185 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x186 = (int) linePoint.X;
                  if (x185 < x186)
                  {
                    linePoint = linePoints[0];
                    int y205 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y206 = (int) linePoint.Y;
                    if (y205 > y206)
                    {
                      linePoint = linePoints[4];
                      point1 = linePoint.X - src;
                      linePoint = linePoints[4];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x187 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x188 = (double) linePoint.X;
                      double num27 = 100000.0 * (x187 - x188) / (double) src;
                      linePoint = linePoints[1];
                      double y207 = (double) linePoint.Y;
                      linePoint = linePoints[2];
                      double y208 = (double) linePoint.Y;
                      double num28 = 100000.0 * (y207 - y208) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num27));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num28));
                      rotation = 10800000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x189 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x190 = (int) linePoint.X;
                  if (x189 > x190)
                  {
                    linePoint = linePoints[0];
                    int y209 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y210 = (int) linePoint.Y;
                    if (y209 < y210)
                    {
                      linePoint = linePoints[4];
                      point1 = linePoint.X;
                      linePoint = linePoints[4];
                      point2 = linePoint.Y - tar;
                      linePoint = linePoints[0];
                      double x191 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x192 = (double) linePoint.X;
                      double num29 = 100000.0 * (x191 - x192) / (double) src;
                      linePoint = linePoints[2];
                      double y211 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y212 = (double) linePoint.Y;
                      double num30 = 100000.0 * (y211 - y212) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num29));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num30));
                      rotation = 10800000;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x193 = (int) linePoint.X;
                  linePoint = linePoints[4];
                  int x194 = (int) linePoint.X;
                  if (x193 > x194)
                  {
                    linePoint = linePoints[0];
                    int y213 = (int) linePoint.Y;
                    linePoint = linePoints[4];
                    int y214 = (int) linePoint.Y;
                    if (y213 > y214)
                    {
                      linePoint = linePoints[4];
                      point1 = linePoint.X;
                      linePoint = linePoints[4];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x195 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x196 = (double) linePoint.X;
                      double num31 = 100000.0 * (x195 - x196) / (double) src;
                      linePoint = linePoints[1];
                      double y215 = (double) linePoint.Y;
                      linePoint = linePoints[2];
                      double y216 = (double) linePoint.Y;
                      double num32 = 100000.0 * (y215 - y216) / (double) tar;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num31));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num32));
                      rotation = 10800000;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
                this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                goto label_270;
              }
            }
          }
        }
        this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
      }
      else if (connector.AutoShapeType == AutoShapeType.BentConnector5 || connector.AutoShapeType == AutoShapeType.CurvedConnector5)
      {
        linePoint = linePoints[1];
        int x197 = (int) linePoint.X;
        linePoint = linePoints[2];
        int x198 = (int) linePoint.X;
        if (x197 == x198)
        {
          linePoint = linePoints[3];
          int x199 = (int) linePoint.X;
          linePoint = linePoints[4];
          int x200 = (int) linePoint.X;
          if (x199 == x200)
          {
            linePoint = linePoints[0];
            int y217 = (int) linePoint.Y;
            linePoint = linePoints[1];
            int y218 = (int) linePoint.Y;
            if (y217 == y218)
            {
              linePoint = linePoints[2];
              int y219 = (int) linePoint.Y;
              linePoint = linePoints[3];
              int y220 = (int) linePoint.Y;
              if (y219 == y220)
              {
                linePoint = linePoints[4];
                int y221 = (int) linePoint.Y;
                linePoint = linePoints[5];
                int y222 = (int) linePoint.Y;
                if (y221 == y222)
                {
                  linePoint = linePoints[0];
                  int x201 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x202 = (int) linePoint.X;
                  if (x201 < x202)
                  {
                    linePoint = linePoints[0];
                    int y223 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y224 = (int) linePoint.Y;
                    if (y223 > y224)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[5];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x203 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x204 = (double) linePoint.X;
                      double num33 = 100000.0 * (x203 - x204) / (double) src;
                      linePoint = linePoints[1];
                      double y225 = (double) linePoint.Y;
                      linePoint = linePoints[2];
                      double y226 = (double) linePoint.Y;
                      double num34 = 100000.0 * (y225 - y226) / (double) tar;
                      linePoint = linePoints[3];
                      double x205 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x206 = (double) linePoint.X;
                      double num35 = 100000.0 * (x205 - x206) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num33));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num34));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num35));
                      rotation = 10800000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x207 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x208 = (int) linePoint.X;
                  if (x207 > x208)
                  {
                    linePoint = linePoints[0];
                    int y227 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y228 = (int) linePoint.Y;
                    if (y227 > y228)
                    {
                      linePoint = linePoints[5];
                      point1 = linePoint.X;
                      linePoint = linePoints[5];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x209 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x210 = (double) linePoint.X;
                      double num36 = 100000.0 * (x209 - x210) / (double) src;
                      linePoint = linePoints[1];
                      double y229 = (double) linePoint.Y;
                      linePoint = linePoints[2];
                      double y230 = (double) linePoint.Y;
                      double num37 = 100000.0 * (y229 - y230) / (double) tar;
                      linePoint = linePoints[2];
                      double x211 = (double) linePoint.X;
                      linePoint = linePoints[3];
                      double x212 = (double) linePoint.X;
                      double num38 = 100000.0 * (x211 - x212) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num36));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num37));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num38));
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x213 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x214 = (int) linePoint.X;
                  if (x213 < x214)
                  {
                    linePoint = linePoints[0];
                    int y231 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y232 = (int) linePoint.Y;
                    if (y231 < y232)
                    {
                      linePoint = linePoints[0];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[1];
                      double x215 = (double) linePoint.X;
                      linePoint = linePoints[0];
                      double x216 = (double) linePoint.X;
                      double num39 = 100000.0 * (x215 - x216) / (double) src;
                      linePoint = linePoints[2];
                      double y233 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y234 = (double) linePoint.Y;
                      double num40 = 100000.0 * (y233 - y234) / (double) tar;
                      linePoint = linePoints[3];
                      double x217 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x218 = (double) linePoint.X;
                      double num41 = 100000.0 * (x217 - x218) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num39));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num40));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num41));
                      rotation = 10800000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x219 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x220 = (int) linePoint.X;
                  if (x219 > x220)
                  {
                    linePoint = linePoints[0];
                    int y235 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y236 = (int) linePoint.Y;
                    if (y235 < y236)
                    {
                      linePoint = linePoints[5];
                      point1 = linePoint.X;
                      linePoint = linePoints[0];
                      point2 = linePoint.Y;
                      linePoint = linePoints[0];
                      double x221 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x222 = (double) linePoint.X;
                      double num42 = 100000.0 * (x221 - x222) / (double) src;
                      linePoint = linePoints[2];
                      double y237 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y238 = (double) linePoint.Y;
                      double num43 = 100000.0 * (y237 - y238) / (double) tar;
                      linePoint = linePoints[2];
                      double x223 = (double) linePoint.X;
                      linePoint = linePoints[3];
                      double x224 = (double) linePoint.X;
                      double num44 = 100000.0 * (x223 - x224) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num42));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num43));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num44));
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
            }
          }
        }
        linePoint = linePoints[0];
        int x225 = (int) linePoint.X;
        linePoint = linePoints[1];
        int x226 = (int) linePoint.X;
        if (x225 == x226)
        {
          linePoint = linePoints[2];
          int x227 = (int) linePoint.X;
          linePoint = linePoints[3];
          int x228 = (int) linePoint.X;
          if (x227 == x228)
          {
            linePoint = linePoints[4];
            int x229 = (int) linePoint.X;
            linePoint = linePoints[5];
            int x230 = (int) linePoint.X;
            if (x229 == x230)
            {
              linePoint = linePoints[1];
              int y239 = (int) linePoint.Y;
              linePoint = linePoints[2];
              int y240 = (int) linePoint.Y;
              if (y239 == y240)
              {
                linePoint = linePoints[3];
                int y241 = (int) linePoint.Y;
                linePoint = linePoints[4];
                int y242 = (int) linePoint.Y;
                if (y241 == y242)
                {
                  linePoint = linePoints[0];
                  int x231 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x232 = (int) linePoint.X;
                  if (x231 < x232)
                  {
                    linePoint = linePoints[0];
                    int y243 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y244 = (int) linePoint.Y;
                    if (y243 > y244)
                    {
                      linePoint = linePoints[0];
                      double x233 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y245 = (double) linePoint.Y - (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x233, (float) y245), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y246 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y247 = (double) linePoint.Y;
                      double num45 = 100000.0 * (y246 - y247) / (double) src;
                      linePoint = linePoints[2];
                      double x234 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x235 = (double) linePoint.X;
                      double num46 = 100000.0 * (x234 - x235) / (double) tar;
                      linePoint = linePoints[0];
                      double y248 = (double) linePoint.Y;
                      linePoint = linePoints[3];
                      double y249 = (double) linePoint.Y;
                      double num47 = 100000.0 * (y248 - y249) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num45));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num46));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num47));
                      rotation = 5400000;
                      flag2 = true;
                      flag1 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x236 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x237 = (int) linePoint.X;
                  if (x236 > x237)
                  {
                    linePoint = linePoints[0];
                    int y250 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y251 = (int) linePoint.Y;
                    if (y250 > y251)
                    {
                      linePoint = linePoints[0];
                      double x238 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y252 = (double) linePoint.Y - (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x238, (float) y252), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[0];
                      double y253 = (double) linePoint.Y;
                      linePoint = linePoints[1];
                      double y254 = (double) linePoint.Y;
                      double num48 = 100000.0 * (y253 - y254) / (double) src;
                      linePoint = linePoints[1];
                      double x239 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x240 = (double) linePoint.X;
                      double num49 = 100000.0 * (x239 - x240) / (double) tar;
                      linePoint = linePoints[0];
                      double y255 = (double) linePoint.Y;
                      linePoint = linePoints[3];
                      double y256 = (double) linePoint.Y;
                      double num50 = 100000.0 * (y255 - y256) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num48));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num49));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num50));
                      rotation = 5400000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x241 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x242 = (int) linePoint.X;
                  if (x241 < x242)
                  {
                    linePoint = linePoints[0];
                    int y257 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y258 = (int) linePoint.Y;
                    if (y257 < y258)
                    {
                      linePoint = linePoints[0];
                      double x243 = (double) linePoint.X + (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y259 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x243, (float) y259), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y260 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y261 = (double) linePoint.Y;
                      double num51 = 100000.0 * (y260 - y261) / (double) src;
                      linePoint = linePoints[2];
                      double x244 = (double) linePoint.X;
                      linePoint = linePoints[1];
                      double x245 = (double) linePoint.X;
                      double num52 = 100000.0 * (x244 - x245) / (double) tar;
                      linePoint = linePoints[3];
                      double y262 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y263 = (double) linePoint.Y;
                      double num53 = 100000.0 * (y262 - y263) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num51));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num52));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num53));
                      rotation = 16200000;
                      flag2 = true;
                      goto label_270;
                    }
                  }
                  linePoint = linePoints[0];
                  int x246 = (int) linePoint.X;
                  linePoint = linePoints[5];
                  int x247 = (int) linePoint.X;
                  if (x246 > x247)
                  {
                    linePoint = linePoints[0];
                    int y264 = (int) linePoint.Y;
                    linePoint = linePoints[5];
                    int y265 = (int) linePoint.Y;
                    if (y264 < y265)
                    {
                      linePoint = linePoints[0];
                      double x248 = (double) linePoint.X - (double) src / 2.0;
                      linePoint = linePoints[0];
                      double y266 = (double) linePoint.Y + (double) tar / 2.0;
                      PointF pointF = this.Transform(new PointF((float) x248, (float) y266), (double) tar / 2.0, 180.0);
                      point1 = pointF.X;
                      point2 = pointF.Y - src / 2f;
                      this.Swap<float>(ref src, ref tar);
                      linePoint = linePoints[1];
                      double y267 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y268 = (double) linePoint.Y;
                      double num54 = 100000.0 * (y267 - y268) / (double) src;
                      linePoint = linePoints[1];
                      double x249 = (double) linePoint.X;
                      linePoint = linePoints[2];
                      double x250 = (double) linePoint.X;
                      double num55 = 100000.0 * (x249 - x250) / (double) tar;
                      linePoint = linePoints[3];
                      double y269 = (double) linePoint.Y;
                      linePoint = linePoints[0];
                      double y270 = (double) linePoint.Y;
                      double num56 = 100000.0 * (y269 - y270) / (double) src;
                      connector.ShapeGuide.Add("adj1", "val " + Helper.ToString((int) num54));
                      connector.ShapeGuide.Add("adj2", "val " + Helper.ToString((int) num55));
                      connector.ShapeGuide.Add("adj3", "val " + Helper.ToString((int) num56));
                      rotation = 5400000;
                      goto label_270;
                    }
                  }
                  this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
                  goto label_270;
                }
              }
            }
          }
        }
        this.UpdateConnectorBounds(connector, beginX, beginY, endX, endY);
      }
    }
label_270:
    if ((double) point1 == 0.0 && (double) point2 == 0.0)
      return;
    long emuLong1 = Helper.PointToEmuLong((double) point1);
    long emuLong2 = Helper.PointToEmuLong((double) point2);
    long emuLong3 = Helper.PointToEmuLong((double) src);
    long emuLong4 = Helper.PointToEmuLong((double) tar);
    if (rotation == 0)
      rotation = -1;
    connector.ShapeFrame.SetAnchor(new bool?(flag1), new bool?(flag2), rotation, emuLong1, emuLong2, emuLong3, emuLong4);
  }

  internal PointF GetPositionsWithConnectionSite(IShape shape, int connnectionSite)
  {
    RectangleF shapeBounds = this.GetShapeBounds(shape as Shape);
    FormulaValues formulaValues = new FormulaValues(new RectangleF(shapeBounds.X, shapeBounds.Y, shapeBounds.Width, shapeBounds.Height), (shape as Shape).ShapeGuide);
    Dictionary<string, float> shapeFormula1;
    switch (shape.AutoShapeType)
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.Diamond:
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Cross:
      case AutoShapeType.FoldedCorner:
      case AutoShapeType.Sun:
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.Plaque:
      case AutoShapeType.QuadArrow:
      case AutoShapeType.LeftRightArrowCallout:
      case AutoShapeType.QuadArrowCallout:
      case AutoShapeType.FlowChartProcess:
      case AutoShapeType.FlowChartAlternateProcess:
      case AutoShapeType.FlowChartDecision:
      case AutoShapeType.FlowChartPredefinedProcess:
      case AutoShapeType.FlowChartInternalStorage:
      case AutoShapeType.FlowChartTerminator:
      case AutoShapeType.FlowChartPreparation:
      case AutoShapeType.FlowChartOffPageConnector:
      case AutoShapeType.FlowChartCard:
      case AutoShapeType.FlowChartSort:
      case AutoShapeType.FlowChartDelay:
      case AutoShapeType.FlowChartDisplay:
      case AutoShapeType.Star4Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.RoundSingleCornerRectangle:
      case AutoShapeType.RoundSameSideCornerRectangle:
      case AutoShapeType.RoundDiagonalCornerRectangle:
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
      case AutoShapeType.SnipSingleCornerRectangle:
      case AutoShapeType.SnipSameSideCornerRectangle:
      case AutoShapeType.SnipDiagonalCornerRectangle:
      case AutoShapeType.Frame:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Parallelogram:
        Dictionary<string, float> shapeFormula2 = formulaValues.ParseShapeFormula(AutoShapeType.Parallelogram);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula2["x2"]) / 2.0), shapeBounds.Y);
          case 2:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula2["x5"]) / 2.0), (float) (((double) shapeBounds.Y * 2.0 + (double) shapeBounds.Height) / 2.0));
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula2["x5"]) / 2.0), shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 5:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula2["x2"]) / 2.0), (float) (((double) shapeBounds.Y * 2.0 + (double) shapeBounds.Height) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Trapezoid:
        Dictionary<string, float> shapeFormula3 = formulaValues.ParseShapeFormula(AutoShapeType.Trapezoid);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula3["x2"] + (double) shapeFormula3["x3"]) / 2.0), shapeBounds.Y);
          case 1:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula3["x2"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height) / 2.0));
          case 2:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width) / 2.0), shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula3["x3"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Octagon:
        Dictionary<string, float> shapeFormula4 = formulaValues.ParseShapeFormula(AutoShapeType.Octagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeFormula4["x1"]);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeFormula4["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula4["x2"], shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula4["x1"], shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula4["y2"]);
          case 5:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula4["x1"]);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula4["x1"], shapeBounds.Y);
          case 7:
            return new PointF(shapeBounds.X + shapeFormula4["x2"], shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.IsoscelesTriangle:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 4f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height);
          case 5:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeBounds.Width / 2.0 + (double) shapeBounds.Width / 4.0), shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RightTriangle:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height);
          case 5:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Oval:
      case AutoShapeType.SmileyFace:
      case AutoShapeType.Donut:
      case AutoShapeType.NoSymbol:
      case AutoShapeType.FlowChartConnector:
      case AutoShapeType.FlowChartSummingJunction:
      case AutoShapeType.FlowChartOr:
        switch (connnectionSite)
        {
          case 0:
            PointF arcPoint1 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 270f));
            return new PointF(shapeBounds.X + arcPoint1.X, shapeBounds.Y + arcPoint1.Y);
          case 1:
            PointF arcPoint2 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 225f));
            return new PointF(shapeBounds.X + arcPoint2.X, shapeBounds.Y + arcPoint2.Y);
          case 2:
            PointF arcPoint3 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 180f));
            return new PointF(shapeBounds.X + arcPoint3.X, shapeBounds.Y + arcPoint3.Y);
          case 3:
            PointF arcPoint4 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 135f));
            return new PointF(shapeBounds.X + arcPoint4.X, shapeBounds.Y + arcPoint4.Y);
          case 4:
            PointF arcPoint5 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 90f));
            return new PointF(shapeBounds.X + arcPoint5.X, shapeBounds.Y + arcPoint5.Y);
          case 5:
            PointF arcPoint6 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 45f));
            return new PointF(shapeBounds.X + arcPoint6.X, shapeBounds.Y + arcPoint6.Y);
          case 6:
            PointF arcPoint7 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 0.0f));
            return new PointF(shapeBounds.X + arcPoint7.X, shapeBounds.Y + arcPoint7.Y);
          case 7:
            PointF arcPoint8 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 315f));
            return new PointF(shapeBounds.X + arcPoint8.X, shapeBounds.Y + arcPoint8.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Hexagon:
        Dictionary<string, float> shapeFormula5 = formulaValues.ParseShapeFormula(AutoShapeType.Hexagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula5["x2"], shapeBounds.Y + shapeFormula5["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula5["x1"], shapeBounds.Y + shapeFormula5["y2"]);
          case 3:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula5["x1"], shapeBounds.Y + shapeFormula5["y1"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula5["x2"], shapeBounds.Y + shapeFormula5["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RegularPentagon:
        Dictionary<string, float> shapeFormula6 = formulaValues.ParseShapeFormula(AutoShapeType.RegularPentagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula6["x1"], shapeBounds.Y + shapeFormula6["y1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula6["x2"], shapeBounds.Y + shapeFormula6["y2"]);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula6["x3"], shapeBounds.Y + shapeFormula6["y2"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula6["x4"], shapeBounds.Y + shapeFormula6["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Can:
        Dictionary<string, float> shapeFormula7 = formulaValues.ParseShapeFormula(AutoShapeType.Can);
        switch (connnectionSite)
        {
          case 0:
            PointF arcPoint9 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeFormula7["y1"], 90f);
            return new PointF(shapeBounds.X + arcPoint9.X, shapeBounds.Y + arcPoint9.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF();
        }
      case AutoShapeType.Cube:
        Dictionary<string, float> shapeFormula8 = formulaValues.ParseShapeFormula(AutoShapeType.Cube);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula8["y1"]) / 2.0), shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula8["x4"] / 2f, shapeBounds.Y + shapeFormula8["y1"]);
          case 2:
            return new PointF(shapeBounds.X, (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height + (double) shapeFormula8["y1"]) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeFormula8["x4"] / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula8["x4"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height + (double) shapeFormula8["y1"]) / 2.0));
          case 5:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeFormula8["y4"] / 2f);
          default:
            return new PointF();
        }
      case AutoShapeType.Bevel:
        Dictionary<string, float> shapeFormula9 = formulaValues.ParseShapeFormula(AutoShapeType.Bevel);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula9["x2"], shapeBounds.Y + (float) (((double) shapeFormula9["y2"] - (double) shapeFormula9["x1"]) / 2.0) + shapeFormula9["x1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + (float) (((double) shapeFormula9["x2"] - (double) shapeFormula9["x1"]) / 2.0) + shapeFormula9["x1"], shapeBounds.Y + shapeFormula9["y2"]);
          case 4:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula9["x1"], shapeBounds.Y + (float) (((double) shapeFormula9["y2"] - (double) shapeFormula9["x1"]) / 2.0) + shapeFormula9["x1"]);
          case 6:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 7:
            return new PointF(shapeBounds.X + (float) (((double) shapeFormula9["x2"] - (double) shapeFormula9["x1"]) / 2.0) + shapeFormula9["x1"], shapeBounds.Y + shapeFormula9["x1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.BlockArc:
        Dictionary<string, float> shapeFormula10 = formulaValues.ParseShapeFormula(AutoShapeType.BlockArc);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula10["dr"] / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.Right - shapeFormula10["dr"] / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Heart:
        shapeFormula1 = formulaValues.ParseShapeFormula(AutoShapeType.Heart);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height / 4f);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LightningBolt:
        shapeFormula1 = formulaValues.ParseShapeFormula(AutoShapeType.LightningBolt);
        switch (connnectionSite)
        {
          case 0:
            return this.GetXYPosition(shapeBounds, 8472f, 0.0f, 21600f);
          case 1:
            return this.GetXYPosition(shapeBounds, 0.0f, 3890f, 21600f);
          case 2:
            return this.GetXYPosition(shapeBounds, 5022f, 9705f, 21600f);
          case 3:
            return this.GetXYPosition(shapeBounds, 10012f, 14915f, 21600f);
          case 4:
            return this.GetXYPosition(shapeBounds, 21600f, 21600f, 21600f);
          case 5:
            return this.GetXYPosition(shapeBounds, 16577f, 12007f, 21600f);
          case 6:
            return this.GetXYPosition(shapeBounds, 12860f, 6080f, 21600f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Moon:
        Dictionary<string, float> shapeFormula11 = formulaValues.ParseShapeFormula(AutoShapeType.Moon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.Right, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height);
          case 3:
            RectangleF rectangleF1 = new RectangleF(shapeBounds.X + shapeFormula11["g0w"], shapeBounds.Y + shapeBounds.Height / 2f - shapeFormula11["dy1"], shapeFormula11["g18w"] * 2f, shapeFormula11["dy1"] * 2f);
            PointF arcPoint10 = this.GetArcPoint((double) rectangleF1.Width / 2.0, (double) rectangleF1.Height / 2.0, this.GetEllipseAngleWithCircleAngle(rectangleF1.Width, rectangleF1.Height, 180f));
            return new PointF(rectangleF1.X + arcPoint10.X, rectangleF1.Y + arcPoint10.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Arc:
        Dictionary<string, float> shapeFormula12 = formulaValues.ParseShapeFormula(AutoShapeType.Arc);
        switch (connnectionSite)
        {
          case 0:
            PointF arcPoint11 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, shapeFormula12["stAng"] / 60000f);
            return new PointF(shapeBounds.X + arcPoint11.X, shapeBounds.Y + arcPoint11.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            PointF arcPoint12 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, (float) ((double) shapeFormula12["stAng"] / 60000.0 + (double) shapeFormula12["swAng"] / 60000.0));
            return new PointF(shapeBounds.X + arcPoint12.X, shapeBounds.Y + arcPoint12.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftBracket:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.Right, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Bottom);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RightBracket:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Bottom);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftBrace:
        Dictionary<string, float> shapeFormula13 = formulaValues.ParseShapeFormula(AutoShapeType.LeftBrace);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.Right, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula13["y4"] - shapeFormula13["y1"]);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Bottom);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RightBrace:
        Dictionary<string, float> shapeFormula14 = formulaValues.ParseShapeFormula(AutoShapeType.RightBrace);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.Right, (float) ((double) shapeBounds.Y + (double) shapeFormula14["y2"] - (double) shapeFormula14["y1"] + (double) shapeFormula14["y1"] * 2.0));
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Bottom);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RightArrow:
        Dictionary<string, float> shapeFormula15 = formulaValues.ParseShapeFormula(AutoShapeType.RightArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula15["x1"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula15["x1"], shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftArrow:
        Dictionary<string, float> shapeFormula16 = formulaValues.ParseShapeFormula(AutoShapeType.LeftArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula16["x2"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula16["x2"], shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.UpArrow:
        Dictionary<string, float> shapeFormula17 = formulaValues.ParseShapeFormula(AutoShapeType.UpArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula17["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula17["y2"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.DownArrow:
        Dictionary<string, float> shapeFormula18 = formulaValues.ParseShapeFormula(AutoShapeType.DownArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula18["y1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula18["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftRightArrow:
        Dictionary<string, float> shapeFormula19 = formulaValues.ParseShapeFormula(AutoShapeType.LeftRightArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula19["x3"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeFormula19["y1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula19["x2"], shapeBounds.Y);
          case 3:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula19["x2"], shapeBounds.Bottom);
          case 5:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeFormula19["y2"]);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula19["x3"], shapeBounds.Bottom);
          case 7:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.UpDownArrow:
        Dictionary<string, float> shapeFormula20 = formulaValues.ParseShapeFormula(AutoShapeType.UpDownArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula20["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula20["x1"], shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula20["y3"]);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 5:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula20["y3"]);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula20["x2"], shapeBounds.Y + shapeBounds.Height / 2f);
          case 7:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula20["y2"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftRightUpArrow:
        Dictionary<string, float> shapeFormula21 = formulaValues.ParseShapeFormula(AutoShapeType.LeftRightUpArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula21["y4"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeFormula21["y5"]);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula21["y4"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.BentArrow:
        Dictionary<string, float> shapeFormula22 = formulaValues.ParseShapeFormula(AutoShapeType.BentArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula22["x4"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula22["x4"], shapeBounds.Y + shapeFormula22["y4"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula22["th"] / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula22["aw2"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.UTurnArrow:
        Dictionary<string, float> shapeFormula23 = formulaValues.ParseShapeFormula(AutoShapeType.UTurnArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula23["x6"], shapeBounds.Y + shapeFormula23["y4"]);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula23["x8"], shapeBounds.Y + shapeFormula23["y5"]);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula23["y4"]);
          case 3:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula23["bd"] + ((double) shapeFormula23["x4"] - (double) shapeFormula23["bd"]) / 2.0), shapeBounds.Y);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula23["th"] / 2f, shapeBounds.Bottom);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftUpArrow:
        Dictionary<string, float> shapeFormula24 = formulaValues.ParseShapeFormula(AutoShapeType.LeftUpArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula24["x4"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula24["x2"], shapeBounds.Y + shapeFormula24["x1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula24["x1"], shapeBounds.Y + shapeFormula24["y2"]);
          case 3:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula24["y4"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula24["x1"], shapeBounds.Bottom);
          case 5:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula24["x1"] + ((double) shapeFormula24["x5"] - (double) shapeFormula24["x1"]) / 2.0), shapeBounds.Y + shapeFormula24["y5"]);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula24["x5"], (float) ((double) shapeBounds.Y + (double) shapeFormula24["x1"] + ((double) shapeFormula24["y5"] - (double) shapeFormula24["x1"]) / 2.0));
          case 7:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula24["x1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.BentUpArrow:
        Dictionary<string, float> shapeFormula25 = formulaValues.ParseShapeFormula(AutoShapeType.BentUpArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula25["x3"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula25["x1"], shapeBounds.Y + shapeFormula25["y1"]);
          case 2:
            return new PointF(shapeBounds.X, (float) ((double) shapeBounds.Y + (double) shapeFormula25["y2"] + ((double) shapeBounds.Bottom - ((double) shapeBounds.Y + (double) shapeFormula25["y2"])) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeFormula25["x4"] / 2f, shapeBounds.Bottom);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula25["x4"], (float) ((double) shapeBounds.Y + (double) shapeFormula25["y1"] + ((double) shapeBounds.Bottom - ((double) shapeBounds.Y + (double) shapeFormula25["y1"])) / 2.0));
          case 5:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula25["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CurvedRightArrow:
        Dictionary<string, float> shapeFormula26 = formulaValues.ParseShapeFormula(AutoShapeType.CurvedRightArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula26["x1"], shapeBounds.Y + shapeFormula26["y8"]);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula26["y6"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula26["x1"], shapeBounds.Y + shapeFormula26["y4"]);
          case 4:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula26["th"] / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CurvedLeftArrow:
        Dictionary<string, float> shapeFormula27 = formulaValues.ParseShapeFormula(AutoShapeType.CurvedLeftArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula27["th"] / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula27["x1"], shapeBounds.Y + shapeFormula27["y4"]);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula27["y6"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula27["x1"], shapeBounds.Y + shapeFormula27["y8"]);
          case 4:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CurvedUpArrow:
        Dictionary<string, float> shapeFormula28 = formulaValues.ParseShapeFormula(AutoShapeType.CurvedUpArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula28["x6"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula28["x4"], shapeBounds.Y + shapeFormula28["y1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula28["th"] / 2f, shapeBounds.Y);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula28["x8"], shapeBounds.Y + shapeFormula28["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CurvedDownArrow:
        Dictionary<string, float> shapeFormula29 = formulaValues.ParseShapeFormula(AutoShapeType.CurvedDownArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula29["th"] / 2f, shapeBounds.Bottom);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula29["x4"], shapeBounds.Y + shapeFormula29["y1"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula29["x6"], shapeBounds.Bottom);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula29["x8"], shapeBounds.Y + shapeFormula29["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.StripedRightArrow:
        Dictionary<string, float> shapeFormula30 = formulaValues.ParseShapeFormula(AutoShapeType.StripedRightArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula30["x5"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula30["x5"], shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.NotchedRightArrow:
        Dictionary<string, float> shapeFormula31 = formulaValues.ParseShapeFormula(AutoShapeType.NotchedRightArrow);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula31["x2"], shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula31["x1"], shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula31["x2"], shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Pentagon:
        Dictionary<string, float> shapeFormula32 = formulaValues.ParseShapeFormula(AutoShapeType.Pentagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula32["x1"] / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula32["x1"] / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Chevron:
        Dictionary<string, float> shapeFormula33 = formulaValues.ParseShapeFormula(AutoShapeType.Chevron);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula33["x2"] / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula33["x1"], shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula33["x2"] / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RightArrowCallout:
        Dictionary<string, float> shapeFormula34 = formulaValues.ParseShapeFormula(AutoShapeType.RightArrowCallout);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula34["x2"] / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula34["x2"] / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LeftArrowCallout:
        Dictionary<string, float> shapeFormula35 = formulaValues.ParseShapeFormula(AutoShapeType.LeftArrowCallout);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula35["x2"] + ((double) shapeBounds.Right - ((double) shapeBounds.X + (double) shapeFormula35["x2"])) / 2.0), shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula35["x2"] + ((double) shapeBounds.Right - ((double) shapeBounds.X + (double) shapeFormula35["x2"])) / 2.0), shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.UpArrowCallout:
        Dictionary<string, float> shapeFormula36 = formulaValues.ParseShapeFormula(AutoShapeType.UpArrowCallout);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula36["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula36["y2"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.DownArrowCallout:
        Dictionary<string, float> shapeFormula37 = formulaValues.ParseShapeFormula(AutoShapeType.DownArrowCallout);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula37["y2"] / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeFormula37["y2"] / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CircularArrow:
        Dictionary<string, float> shapeFormula38 = formulaValues.ParseShapeFormula(AutoShapeType.CircularArrow);
        switch (connnectionSite)
        {
          case 0:
            SizeF sizeF1 = new SizeF(shapeFormula38["rw1"], shapeFormula38["rh1"]);
            SizeF sizeF2 = new SizeF(shapeFormula38["rw2"], shapeFormula38["rh2"]);
            PointF pointF1 = new PointF(shapeBounds.X + shapeFormula38["xE"], shapeBounds.Y + shapeFormula38["yE"]);
            RectangleF rectangleF2 = new RectangleF(pointF1.X - sizeF1.Width * 2f, pointF1.Y - sizeF1.Height, sizeF1.Width * 2f, sizeF1.Height * 2f);
            RectangleF rectangleF3 = new RectangleF(pointF1.X - sizeF1.Width - sizeF2.Width, pointF1.Y - sizeF2.Height, sizeF2.Width * 2f, sizeF2.Height * 2f);
            return new PointF(rectangleF2.X + (float) (((double) rectangleF3.X - (double) rectangleF2.X) / 2.0), rectangleF2.Y + rectangleF2.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula38["xGp"], shapeBounds.Y + shapeFormula38["yGp"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula38["xA"], shapeBounds.Y + shapeFormula38["yA"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula38["xBp"], shapeBounds.Y + shapeFormula38["yBp"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartData:
        Dictionary<string, float> shapeFormula39 = formulaValues.ParseShapeFormula(AutoShapeType.Parallelogram);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeBounds.Width / 2.0 + (double) shapeFormula39["x2"] / 2.0), shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula39["x2"] / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula39["x5"] / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 5:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula39["x5"]) / 2.0), (float) (((double) shapeBounds.Y * 2.0 + (double) shapeBounds.Height) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartDocument:
        PointF xyPosition1 = this.GetXYPosition(shapeBounds, 10800f, 16800f, 21600f);
        PointF xyPosition2 = this.GetXYPosition(shapeBounds, 10800f, 23400f, 21600f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF((float) (((double) xyPosition2.X + (double) xyPosition1.X) / 2.0), (float) (((double) xyPosition2.Y + (double) xyPosition1.Y) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartMultiDocument:
        PointF xyPosition3 = this.GetXYPosition(shapeBounds, 2972f, 0.0f, 21600f);
        PointF xyPosition4 = this.GetXYPosition(shapeBounds, 21600f, 0.0f, 21600f);
        PointF xyPosition5 = this.GetXYPosition(shapeBounds, 9298f, 18022f, 21600f);
        PointF xyPosition6 = this.GetXYPosition(shapeBounds, 9298f, 23542f, 21600f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) (((double) xyPosition4.X + (double) xyPosition3.X) / 2.0), (float) (((double) xyPosition4.Y + (double) xyPosition3.Y) / 2.0));
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF((float) (((double) xyPosition5.X + (double) xyPosition6.X) / 2.0), (float) (((double) xyPosition5.Y + (double) xyPosition6.Y) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartManualInput:
        PointF xyPosition7 = this.GetXYPosition(shapeBounds, 0.0f, 1f, 5f);
        PointF xyPosition8 = this.GetXYPosition(shapeBounds, 5f, 0.0f, 5f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) (((double) xyPosition7.X + (double) xyPosition8.X) / 2.0), (float) (((double) xyPosition7.Y + (double) xyPosition8.Y) / 2.0));
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartManualOperation:
        PointF xyPosition9 = this.GetXYPosition(shapeBounds, 0.0f, 0.0f, 5f);
        PointF xyPosition10 = this.GetXYPosition(shapeBounds, 5f, 0.0f, 5f);
        PointF xyPosition11 = this.GetXYPosition(shapeBounds, 4f, 5f, 5f);
        PointF xyPosition12 = this.GetXYPosition(shapeBounds, 1f, 5f, 5f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) (((double) xyPosition9.X + (double) xyPosition10.X) / 2.0), (float) (((double) xyPosition9.Y + (double) xyPosition10.Y) / 2.0));
          case 1:
            return new PointF((float) (((double) xyPosition12.X + (double) xyPosition9.X) / 2.0), (float) (((double) xyPosition12.Y + (double) xyPosition9.Y) / 2.0));
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF((float) (((double) xyPosition11.X + (double) xyPosition10.X) / 2.0), (float) (((double) xyPosition11.Y + (double) xyPosition10.Y) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartPunchedTape:
        RectangleF rectangleF4 = new RectangleF(this.GetXYPosition(shapeBounds, 0.0f, 2f, 20f), new SizeF((float) ((double) shapeBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) shapeBounds.Height * 2.0 / 20.0 * 2.0)));
        PointF arcPoint13 = this.GetArcPoint((double) rectangleF4.Width / 2.0, (double) rectangleF4.Height / 2.0, -180f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + arcPoint13.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            rectangleF4 = new RectangleF(new PointF(this.GetXYPosition(shapeBounds, 20f, 18f, 20f).X - rectangleF4.Width, this.GetXYPosition(shapeBounds, 20f, 18f, 20f).Y), new SizeF((float) ((double) shapeBounds.Width * 5.0 / 20.0 * 2.0), (float) ((double) shapeBounds.Height * 2.0 / 20.0 * 2.0)));
            PointF arcPoint14 = this.GetArcPoint((double) rectangleF4.Width / 2.0, (double) rectangleF4.Height / 2.0, -180f);
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height - arcPoint14.Y);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartCollate:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartExtract:
        PointF xyPosition13 = this.GetXYPosition(shapeBounds, 0.0f, 0.0f, 2f);
        PointF xyPosition14 = this.GetXYPosition(shapeBounds, 2f, 0.0f, 2f);
        PointF xyPosition15 = this.GetXYPosition(shapeBounds, 1f, 2f, 2f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) (((double) xyPosition13.X + (double) xyPosition14.X) / 2.0), (float) (((double) xyPosition13.Y + (double) xyPosition14.Y) / 2.0));
          case 1:
            return new PointF((float) (((double) xyPosition15.X + (double) xyPosition13.X) / 2.0), (float) (((double) xyPosition15.Y + (double) xyPosition13.Y) / 2.0));
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF((float) (((double) xyPosition15.X + (double) xyPosition14.X) / 2.0), (float) (((double) xyPosition15.Y + (double) xyPosition14.Y) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartMerge:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 4f, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeBounds.Width / 2.0 + (double) shapeBounds.Width / 4.0), shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartStoredData:
        SizeF sizeF3 = new SizeF((float) ((double) shapeBounds.Width / 6.0 * 2.0), (float) ((double) shapeBounds.Height / 2.0 * 2.0));
        PointF xyPosition16 = this.GetXYPosition(shapeBounds, 6f, 0.0f, 6f);
        RectangleF rectangleF5 = new RectangleF(xyPosition16.X - sizeF3.Width / 2f, xyPosition16.Y, sizeF3.Width, sizeF3.Height);
        PointF arcPoint15 = this.GetArcPoint((double) rectangleF5.Width / 2.0, (double) rectangleF5.Height / 2.0, 180f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(rectangleF5.X + arcPoint15.X, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF();
        }
      case AutoShapeType.FlowChartSequentialAccessStorage:
        switch (connnectionSite)
        {
          case 0:
            PointF arcPoint16 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 270f);
            return new PointF(shapeBounds.X + arcPoint16.X, shapeBounds.Y + arcPoint16.Y);
          case 1:
            PointF arcPoint17 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 180f);
            return new PointF(shapeBounds.X + arcPoint17.X, shapeBounds.Y + arcPoint17.Y);
          case 2:
            PointF arcPoint18 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 90f);
            return new PointF(shapeBounds.X + arcPoint18.X, shapeBounds.Y + arcPoint18.Y);
          case 3:
            PointF arcPoint19 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 0.0f);
            return new PointF(shapeBounds.X + arcPoint19.X, shapeBounds.Y + arcPoint19.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.FlowChartMagneticDisk:
        shapeFormula1 = formulaValues.ParseShapeFormula(AutoShapeType.FlowChartMagneticDisk);
        SizeF sizeF4 = new SizeF(shapeBounds.Width, shapeBounds.Height / 3f);
        PointF pointF2 = this.GetXYPosition(shapeBounds, 6f, 1f, 6f);
        RectangleF rectangleF6 = new RectangleF(pointF2.X - sizeF4.Width, pointF2.Y - sizeF4.Height / 2f, sizeF4.Width, sizeF4.Height);
        switch (connnectionSite)
        {
          case 0:
            pointF2 = this.GetArcPoint((double) rectangleF6.Width / 2.0, (double) rectangleF6.Height / 2.0, 90f);
            return new PointF(shapeBounds.X + pointF2.X, shapeBounds.Y + pointF2.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF();
        }
      case AutoShapeType.FlowChartDirectAccessStorage:
        shapeFormula1 = formulaValues.ParseShapeFormula(AutoShapeType.FlowChartMagneticDisk);
        SizeF sizeF5 = new SizeF(shapeBounds.Width / 3f, shapeBounds.Height);
        PointF xyPosition17 = this.GetXYPosition(shapeBounds, 5f, 6f, 6f);
        RectangleF rectangleF7 = new RectangleF(xyPosition17.X - sizeF5.Width / 2f, xyPosition17.Y, sizeF5.Width, sizeF5.Height);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            PointF arcPoint20 = this.GetArcPoint((double) rectangleF7.Width / 2.0, (double) rectangleF7.Height / 2.0, 180f);
            return new PointF(rectangleF7.X + arcPoint20.X, shapeBounds.Y + arcPoint20.Y);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF();
        }
      case AutoShapeType.Explosion1:
        switch (connnectionSite)
        {
          case 0:
            return this.GetXYPosition(shapeBounds, 14522f, 0.0f, 21600f);
          case 1:
            return this.GetXYPosition(shapeBounds, 0.0f, 8615f, 21600f);
          case 2:
            return this.GetXYPosition(shapeBounds, 8485f, 21600f, 21600f);
          case 3:
            return this.GetXYPosition(shapeBounds, 21600f, 13290f, 21600f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Explosion2:
        switch (connnectionSite)
        {
          case 0:
            return this.GetXYPosition(shapeBounds, 9722f, 1887f, 21600f);
          case 1:
            return this.GetXYPosition(shapeBounds, 0.0f, 12877f, 21600f);
          case 2:
            return this.GetXYPosition(shapeBounds, 11612f, 18842f, 21600f);
          case 3:
            return this.GetXYPosition(shapeBounds, 21600f, 6645f, 21600f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star5Point:
        Dictionary<string, float> shapeFormula40 = formulaValues.ParseShapeFormula(AutoShapeType.Star5Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula40["x1"], shapeBounds.Y + shapeFormula40["y1"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula40["x2"], shapeBounds.Y + shapeFormula40["y2"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula40["x3"], shapeBounds.Y + shapeFormula40["y2"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula40["x4"], shapeBounds.Y + shapeFormula40["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star8Point:
        Dictionary<string, float> shapeFormula41 = formulaValues.ParseShapeFormula(AutoShapeType.Star8Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula41["x2"], shapeBounds.Y + shapeFormula41["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula41["x1"], shapeBounds.Y + shapeFormula41["y2"]);
          case 4:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula41["x1"], shapeBounds.Y + shapeFormula41["y1"]);
          case 6:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 7:
            return new PointF(shapeBounds.X + shapeFormula41["x2"], shapeBounds.Y + shapeFormula41["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star16Point:
        Dictionary<string, float> shapeFormula42 = formulaValues.ParseShapeFormula(AutoShapeType.Star16Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula42["x5"], shapeBounds.Y + shapeFormula42["y2"]);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula42["x6"], shapeBounds.Y + shapeFormula42["y3"]);
          case 2:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula42["x6"], shapeBounds.Y + shapeFormula42["y4"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula42["x5"], shapeBounds.Y + shapeFormula42["y5"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula42["x4"], shapeBounds.Y + shapeFormula42["y6"]);
          case 6:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 7:
            return new PointF(shapeBounds.X + shapeFormula42["x3"], shapeBounds.Y + shapeFormula42["y6"]);
          case 8:
            return new PointF(shapeBounds.X + shapeFormula42["x2"], shapeBounds.Y + shapeFormula42["y5"]);
          case 9:
            return new PointF(shapeBounds.X + shapeFormula42["x1"], shapeBounds.Y + shapeFormula42["y4"]);
          case 10:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 11:
            return new PointF(shapeBounds.X + shapeFormula42["x1"], shapeBounds.Y + shapeFormula42["y3"]);
          case 12:
            return new PointF(shapeBounds.X + shapeFormula42["x2"], shapeBounds.Y + shapeFormula42["y2"]);
          case 13:
            return new PointF(shapeBounds.X + shapeFormula42["x3"], shapeBounds.Y + shapeFormula42["y1"]);
          case 14:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 15:
            return new PointF(shapeBounds.X + shapeFormula42["x4"], shapeBounds.Y + shapeFormula42["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.UpRibbon:
        Dictionary<string, float> shapeFormula43 = formulaValues.ParseShapeFormula(AutoShapeType.UpRibbon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 8f, shapeBounds.Y + shapeFormula43["y3"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeFormula43["y2"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula43["x10"], shapeBounds.Y + shapeFormula43["y3"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.DownRibbon:
        Dictionary<string, float> shapeFormula44 = formulaValues.ParseShapeFormula(AutoShapeType.DownRibbon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, (float) ((double) shapeBounds.Y + (double) shapeFormula44["y1"] + (double) shapeFormula44["hR"] * 2.0));
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 8f, shapeBounds.Y + shapeFormula44["y3"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula44["x10"], shapeBounds.Y + shapeFormula44["y3"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CurvedUpRibbon:
        Dictionary<string, float> shapeFormula45 = formulaValues.ParseShapeFormula(AutoShapeType.CurvedUpRibbon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 8f, shapeBounds.Y + shapeFormula45["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, (float) ((double) shapeBounds.Y + (double) shapeFormula45["cy3"] + ((double) shapeBounds.Y + (double) shapeFormula45["y3"] - ((double) shapeBounds.Y + (double) shapeFormula45["cy3"])) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeFormula45["x6"], shapeBounds.Y + shapeFormula45["y2"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.CurvedDownRibbon:
        Dictionary<string, float> shapeFormula46 = formulaValues.ParseShapeFormula(AutoShapeType.CurvedDownRibbon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, (float) ((double) shapeBounds.Y + (double) shapeFormula46["cy3"] + ((double) shapeBounds.Y + (double) shapeFormula46["y3"] - ((double) shapeBounds.Y + (double) shapeFormula46["cy3"])) / 2.0));
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 8f, shapeBounds.Y + shapeFormula46["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula46["x6"], shapeBounds.Y + shapeFormula46["y2"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.VerticalScroll:
        Dictionary<string, float> shapeFormula47 = formulaValues.ParseShapeFormula(AutoShapeType.VerticalScroll);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula47["ch"], shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula47["x6"], shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.HorizontalScroll:
        Dictionary<string, float> shapeFormula48 = formulaValues.ParseShapeFormula(AutoShapeType.HorizontalScroll);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeFormula48["y3"] - shapeFormula48["ch2"]);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeFormula48["y6"]);
          case 3:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Wave:
        Dictionary<string, float> shapeFormula49 = formulaValues.ParseShapeFormula(AutoShapeType.Wave);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula49["x2"] + ((double) shapeFormula49["x5"] - (double) shapeFormula49["x2"]) / 2.0), shapeBounds.Y + shapeFormula49["y1"]);
          case 1:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula49["x2"] + ((double) shapeFormula49["x6"] - (double) shapeFormula49["x2"]) / 2.0), (float) ((double) shapeBounds.Y + (double) shapeFormula49["y1"] + ((double) shapeFormula49["y4"] - (double) shapeFormula49["y1"]) / 2.0));
          case 2:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula49["x6"] + ((double) shapeFormula49["x10"] - (double) shapeFormula49["x6"]) / 2.0), shapeBounds.Y + shapeFormula49["y4"]);
          case 3:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula49["x5"] + ((double) shapeFormula49["x10"] - (double) shapeFormula49["x5"]) / 2.0), (float) ((double) shapeBounds.Y + (double) shapeFormula49["y1"] + ((double) shapeFormula49["y4"] - (double) shapeFormula49["y1"]) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.DoubleWave:
        Dictionary<string, float> shapeFormula50 = formulaValues.ParseShapeFormula(AutoShapeType.DoubleWave);
        switch (connnectionSite)
        {
          case 0:
            return (double) shapeFormula50["x9"] > (double) shapeFormula50["x2"] ? new PointF((float) ((double) shapeBounds.X + (double) shapeFormula50["x5"] + ((double) shapeFormula50["x8"] - (double) shapeFormula50["x5"]) / 2.0), shapeBounds.Y + shapeFormula50["y1"]) : new PointF((float) ((double) shapeBounds.X + (double) shapeFormula50["x2"] + ((double) shapeFormula50["x5"] - (double) shapeFormula50["x2"]) / 2.0), shapeBounds.Y + shapeFormula50["y1"]);
          case 1:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula50["x2"] + ((double) shapeFormula50["x9"] - (double) shapeFormula50["x2"]) / 2.0), (float) ((double) shapeBounds.Y + (double) shapeFormula50["y1"] + ((double) shapeFormula50["y4"] - (double) shapeFormula50["y1"]) / 2.0));
          case 2:
            return (double) shapeFormula50["x9"] > (double) shapeFormula50["x2"] ? new PointF((float) ((double) shapeBounds.X + (double) shapeFormula50["x9"] + ((double) shapeFormula50["x12"] - (double) shapeFormula50["x9"]) / 2.0), shapeBounds.Y + shapeFormula50["y4"]) : new PointF((float) ((double) shapeBounds.X + (double) shapeFormula50["x12"] + ((double) shapeFormula50["x15"] - (double) shapeFormula50["x12"]) / 2.0), shapeBounds.Y + shapeFormula50["y4"]);
          case 3:
            return new PointF((float) ((double) shapeBounds.X + (double) shapeFormula50["x8"] + ((double) shapeFormula50["x15"] - (double) shapeFormula50["x8"]) / 2.0), (float) ((double) shapeBounds.Y + (double) shapeFormula50["y1"] + ((double) shapeFormula50["y4"] - (double) shapeFormula50["y1"]) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.RoundedRectangularCallout:
        Dictionary<string, float> shapeFormula51 = formulaValues.ParseShapeFormula(AutoShapeType.RectangularCallout);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 1:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula51["xb"], shapeBounds.Y + shapeFormula51["yb"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.OvalCallout:
        Dictionary<string, float> shapeFormula52 = formulaValues.ParseShapeFormula(AutoShapeType.OvalCallout);
        switch (connnectionSite)
        {
          case 0:
            PointF arcPoint21 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 270f));
            return new PointF(shapeBounds.X + arcPoint21.X, shapeBounds.Y + arcPoint21.Y);
          case 1:
            PointF arcPoint22 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 225f));
            return new PointF(shapeBounds.X + arcPoint22.X, shapeBounds.Y + arcPoint22.Y);
          case 2:
            PointF arcPoint23 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 180f));
            return new PointF(shapeBounds.X + arcPoint23.X, shapeBounds.Y + arcPoint23.Y);
          case 3:
            PointF arcPoint24 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 135f));
            return new PointF(shapeBounds.X + arcPoint24.X, shapeBounds.Y + arcPoint24.Y);
          case 4:
            PointF arcPoint25 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 90f));
            return new PointF(shapeBounds.X + arcPoint25.X, shapeBounds.Y + arcPoint25.Y);
          case 5:
            PointF arcPoint26 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 45f));
            return new PointF(shapeBounds.X + arcPoint26.X, shapeBounds.Y + arcPoint26.Y);
          case 6:
            PointF arcPoint27 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 0.0f));
            return new PointF(shapeBounds.X + arcPoint27.X, shapeBounds.Y + arcPoint27.Y);
          case 7:
            PointF arcPoint28 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 315f));
            return new PointF(shapeBounds.X + arcPoint28.X, shapeBounds.Y + arcPoint28.Y);
          case 8:
            return new PointF(shapeBounds.X + shapeFormula52["xPos"], shapeBounds.Y + shapeFormula52["yPos"]);
          default:
            return new PointF();
        }
      case AutoShapeType.CloudCallout:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            SizeF sizeF6 = new SizeF((float) ((double) shapeBounds.Width * 5333.0 / 43200.0 * 2.0), (float) ((double) shapeBounds.Height * 7267.0 / 43200.0 * 2.0));
            RectangleF rectangleF8 = new RectangleF(shapeBounds.X + (float) (13469.0 * (double) shapeBounds.Width / 43200.0), shapeBounds.Y + (float) (1304.0 * (double) shapeBounds.Height / 43200.0), sizeF6.Width, sizeF6.Height);
            PointF arcPoint29 = this.GetArcPoint((double) rectangleF8.Width / 2.0, (double) rectangleF8.Height / 2.0, 304f);
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, rectangleF8.Y + arcPoint29.Y);
          case 4:
            return new PointF(0.0f, 0.0f);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout1NoBorder:
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout2NoBorder:
      case AutoShapeType.LineCallout3NoBorder:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Width / 2f);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          default:
            return new PointF();
        }
      case AutoShapeType.DiagonalStripe:
        Dictionary<string, float> shapeFormula53 = formulaValues.ParseShapeFormula(AutoShapeType.DiagonalStripe);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height) / 2.0));
          case 1:
            return new PointF(shapeBounds.X, (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height + (double) shapeFormula53["y2"]) / 2.0));
          case 2:
            return new PointF(shapeBounds.X + shapeFormula53["x2"] / 2f, shapeBounds.Y + shapeFormula53["y2"] / 2f);
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula53["x2"]) / 2.0), shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Pie:
        shapeFormula1 = formulaValues.ParseShapeFormula(AutoShapeType.Pie);
        switch (connnectionSite)
        {
          case 0:
            PointF arcPoint30 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 0.0f);
            return new PointF(shapeBounds.X + arcPoint30.X, shapeBounds.Y + arcPoint30.Y);
          case 1:
            PointF arcPoint31 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 90f);
            return new PointF(shapeBounds.X + arcPoint31.X, shapeBounds.Y + arcPoint31.Y);
          case 2:
            PointF arcPoint32 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 180f);
            return new PointF(shapeBounds.X + arcPoint32.X, shapeBounds.Y + arcPoint32.Y);
          case 3:
            PointF arcPoint33 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, 270f);
            return new PointF(shapeBounds.X + arcPoint33.X, shapeBounds.Y + arcPoint33.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Decagon:
        Dictionary<string, float> shapeFormula54 = formulaValues.ParseShapeFormula(AutoShapeType.Decagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula54["x4"], shapeBounds.Y + shapeFormula54["y2"]);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula54["x4"], shapeBounds.Y + shapeFormula54["y3"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula54["x3"], shapeBounds.Y + shapeFormula54["y4"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula54["x2"], shapeBounds.Y + shapeFormula54["y4"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula54["x1"], shapeBounds.Y + shapeFormula54["y3"]);
          case 6:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 7:
            return new PointF(shapeBounds.X + shapeFormula54["x1"], shapeBounds.Y + shapeFormula54["y2"]);
          case 8:
            return new PointF(shapeBounds.X + shapeFormula54["x2"], shapeBounds.Y + shapeFormula54["y1"]);
          case 9:
            return new PointF(shapeBounds.X + shapeFormula54["x3"], shapeBounds.Y + shapeFormula54["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Heptagon:
        Dictionary<string, float> shapeFormula55 = formulaValues.ParseShapeFormula(AutoShapeType.Heptagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula55["x5"], shapeBounds.Y + shapeFormula55["y1"]);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula55["x6"], shapeBounds.Y + shapeFormula55["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula55["x4"], shapeBounds.Y + shapeFormula55["y3"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula55["x3"], shapeBounds.Y + shapeFormula55["y3"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula55["x1"], shapeBounds.Y + shapeFormula55["y2"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula55["x2"], shapeBounds.Y + shapeFormula55["y1"]);
          case 6:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Dodecagon:
        Dictionary<string, float> shapeFormula56 = formulaValues.ParseShapeFormula(AutoShapeType.Dodecagon);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula56["x4"], shapeBounds.Y + shapeFormula56["y1"]);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeFormula56["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeFormula56["y3"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula56["x4"], shapeBounds.Y + shapeFormula56["y4"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula56["x3"], shapeBounds.Y + shapeBounds.Height);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula56["x2"], shapeBounds.Y + shapeBounds.Height);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula56["x1"], shapeBounds.Y + shapeFormula56["y4"]);
          case 7:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula56["y3"]);
          case 8:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeFormula56["y2"]);
          case 9:
            return new PointF(shapeBounds.X + shapeFormula56["x1"], shapeBounds.Y + shapeFormula56["y1"]);
          case 10:
            return new PointF(shapeBounds.X + shapeFormula56["x2"], shapeBounds.Y);
          case 11:
            return new PointF(shapeBounds.X + shapeFormula56["x3"], shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star6Point:
        Dictionary<string, float> shapeFormula57 = formulaValues.ParseShapeFormula(AutoShapeType.Star6Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula57["x2"], shapeBounds.Y + shapeBounds.Height / 4f);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula57["x2"], shapeBounds.Y + shapeFormula57["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula57["x1"], shapeBounds.Y + shapeFormula57["y2"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula57["x1"], shapeBounds.Y + shapeBounds.Height / 4f);
          case 5:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star7Point:
        Dictionary<string, float> shapeFormula58 = formulaValues.ParseShapeFormula(AutoShapeType.Star7Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula58["x5"], shapeBounds.Y + shapeFormula58["y1"]);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula58["x6"], shapeBounds.Y + shapeFormula58["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula58["x4"], shapeBounds.Y + shapeFormula58["y3"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula58["x3"], shapeBounds.Y + shapeFormula58["y3"]);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula58["x1"], shapeBounds.Y + shapeFormula58["y2"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula58["x2"], shapeBounds.Y + shapeFormula58["y1"]);
          case 6:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star10Point:
        Dictionary<string, float> shapeFormula59 = formulaValues.ParseShapeFormula(AutoShapeType.Star10Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula59["x4"], shapeBounds.Y + shapeFormula59["y2"]);
          case 1:
            return new PointF(shapeBounds.X + shapeFormula59["x4"], shapeBounds.Y + shapeFormula59["y3"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula59["x3"], shapeBounds.Y + shapeFormula59["y4"]);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 4:
            return new PointF(shapeBounds.X + shapeFormula59["x2"], shapeBounds.Y + shapeFormula59["y4"]);
          case 5:
            return new PointF(shapeBounds.X + shapeFormula59["x1"], shapeBounds.Y + shapeFormula59["y3"]);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula59["x1"], shapeBounds.Y + shapeFormula59["y2"]);
          case 7:
            return new PointF(shapeBounds.X + shapeFormula59["x2"], shapeBounds.Y + shapeFormula59["y1"]);
          case 8:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 9:
            return new PointF(shapeBounds.X + shapeFormula59["x3"], shapeBounds.Y + shapeFormula59["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Star12Point:
        Dictionary<string, float> shapeFormula60 = formulaValues.ParseShapeFormula(AutoShapeType.Star12Point);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula60["x4"], shapeBounds.Y + shapeBounds.Height / 4f);
          case 1:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula60["x4"], shapeBounds.Y + shapeFormula60["y3"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula60["x3"], shapeBounds.Y + shapeFormula60["y4"]);
          case 4:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Bottom);
          case 5:
            return new PointF(shapeBounds.X + shapeBounds.Width / 4f, shapeBounds.Y + shapeFormula60["y4"]);
          case 6:
            return new PointF(shapeBounds.X + shapeFormula60["x1"], shapeBounds.Y + shapeFormula60["y3"]);
          case 7:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 8:
            return new PointF(shapeBounds.X + shapeFormula60["x1"], shapeBounds.Y + shapeBounds.Height / 4f);
          case 9:
            return new PointF(shapeBounds.X + shapeBounds.Width / 4f, shapeBounds.Y + shapeFormula60["y1"]);
          case 10:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 11:
            return new PointF(shapeBounds.X + shapeFormula60["x3"], shapeBounds.Y + shapeFormula60["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.HalfFrame:
        Dictionary<string, float> shapeFormula61 = formulaValues.ParseShapeFormula(AutoShapeType.HalfFrame);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeBounds.Width + (double) shapeFormula61["x2"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula61["y1"]) / 2.0));
          case 1:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula61["x1"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height + (double) shapeFormula61["y2"]) / 2.0));
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Teardrop:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            PointF arcPoint34 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 45f));
            return new PointF(shapeBounds.X + arcPoint34.X, shapeBounds.Y + arcPoint34.Y);
          case 2:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 3:
            PointF arcPoint35 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 135f));
            return new PointF(shapeBounds.X + arcPoint35.X, shapeBounds.Y + arcPoint35.Y);
          case 4:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 5:
            PointF arcPoint36 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, this.GetEllipseAngleWithCircleAngle(shapeBounds.Width, shapeBounds.Height, 225f));
            return new PointF(shapeBounds.X + arcPoint36.X, shapeBounds.Y + arcPoint36.Y);
          case 6:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y);
          case 7:
            Dictionary<string, float> shapeFormula62 = formulaValues.ParseShapeFormula(AutoShapeType.Teardrop);
            PointF pointF3 = new PointF(shapeBounds.X + shapeFormula62["x1"], shapeBounds.Y + shapeFormula62["y1"]);
            return new PointF(pointF3.X, pointF3.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Chord:
        Dictionary<string, float> shapeFormula63 = formulaValues.ParseShapeFormula(AutoShapeType.Chord);
        PointF arcPoint37 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, shapeFormula63["stAng"] / 60000f);
        PointF arcPoint38 = this.GetArcPoint((double) shapeBounds.Width / 2.0, (double) shapeBounds.Height / 2.0, (float) ((double) shapeFormula63["stAng"] / 60000.0 + (double) shapeFormula63["swAng"] / 60000.0));
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + arcPoint37.X, shapeBounds.Y + arcPoint37.Y);
          case 1:
            return new PointF(shapeBounds.X + arcPoint38.X, shapeBounds.Y + arcPoint38.Y);
          case 2:
            return new PointF(shapeBounds.X + (float) (((double) arcPoint37.X + (double) arcPoint38.X) / 2.0), shapeBounds.Y + (float) (((double) arcPoint37.Y + (double) arcPoint38.Y) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Corner:
        Dictionary<string, float> shapeFormula64 = formulaValues.ParseShapeFormula(AutoShapeType.Corner);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeBounds.Width, (float) ((2.0 * (double) shapeBounds.Y + (double) shapeBounds.Height + (double) shapeFormula64["y1"]) / 2.0));
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula64["x1"]) / 2.0), shapeBounds.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.MathPlus:
        Dictionary<string, float> shapeFormula65 = formulaValues.ParseShapeFormula(AutoShapeType.MathPlus);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula65["x4"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula65["y3"] + (double) shapeFormula65["y2"]) / 2.0));
          case 1:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula65["x2"] + (double) shapeFormula65["x3"]) / 2.0), shapeBounds.Y + shapeFormula65["y4"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula65["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula65["y3"] + (double) shapeFormula65["y2"]) / 2.0));
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula65["x2"] + (double) shapeFormula65["x3"]) / 2.0), shapeBounds.Y + shapeFormula65["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.MathMinus:
        Dictionary<string, float> shapeFormula66 = formulaValues.ParseShapeFormula(AutoShapeType.MathMinus);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula66["x2"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula66["y1"] + (double) shapeFormula66["y2"]) / 2.0));
          case 1:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula66["x2"] + (double) shapeFormula66["x1"]) / 2.0), shapeBounds.Y + shapeFormula66["y2"]);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula66["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula66["y1"] + (double) shapeFormula66["y2"]) / 2.0));
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula66["x2"] + (double) shapeFormula66["x1"]) / 2.0), shapeBounds.Y + shapeFormula66["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.MathMultiply:
        Dictionary<string, float> shapeFormula67 = formulaValues.ParseShapeFormula(AutoShapeType.MathMultiply);
        switch (connnectionSite)
        {
          case 0:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula67["xA"] + (double) shapeFormula67["xB"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula67["yA"] + (double) shapeFormula67["yB"]) / 2.0));
          case 1:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula67["xD"] + (double) shapeFormula67["xE"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula67["yA"] + (double) shapeFormula67["yB"]) / 2.0));
          case 2:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula67["xE"] + (double) shapeFormula67["xD"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula67["yG"] + (double) shapeFormula67["yH"]) / 2.0));
          case 3:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula67["xA"] + (double) shapeFormula67["xB"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula67["yG"] + (double) shapeFormula67["yH"]) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.MathDivision:
        Dictionary<string, float> shapeFormula68 = formulaValues.ParseShapeFormula(AutoShapeType.MathDivision);
        PointF arcPoint39 = this.GetArcPoint((double) shapeFormula68["rad"], (double) shapeFormula68["rad"], 270f);
        PointF arcPoint40 = this.GetArcPoint((double) shapeFormula68["rad"], (double) shapeFormula68["rad"], 90f);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula68["x3"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula68["y3"] + (double) shapeFormula68["y4"]) / 2.0));
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f - shapeFormula68["rad"] + arcPoint40.X, (float) ((double) shapeBounds.Y + (double) shapeFormula68["y5"] - (double) shapeFormula68["rad"] * 2.0) + arcPoint40.Y);
          case 2:
            return new PointF(shapeBounds.X + shapeFormula68["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula68["y3"] + (double) shapeFormula68["y4"]) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f - shapeFormula68["rad"] + arcPoint39.X, shapeBounds.Y + shapeFormula68["y1"] + arcPoint39.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.MathEqual:
        Dictionary<string, float> shapeFormula69 = formulaValues.ParseShapeFormula(AutoShapeType.MathEqual);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula69["x2"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula69["y1"] + (double) shapeFormula69["y2"]) / 2.0));
          case 1:
            return new PointF(shapeBounds.X + shapeFormula69["x2"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula69["y3"] + (double) shapeFormula69["y4"]) / 2.0));
          case 2:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula69["x2"] + (double) shapeFormula69["x1"]) / 2.0), shapeBounds.Y + shapeFormula69["y4"]);
          case 3:
            return new PointF(shapeBounds.X + shapeFormula69["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula69["y1"] + (double) shapeFormula69["y2"]) / 2.0));
          case 4:
            return new PointF(shapeBounds.X + shapeFormula69["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula69["y3"] + (double) shapeFormula69["y4"]) / 2.0));
          case 5:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula69["x2"] + (double) shapeFormula69["x1"]) / 2.0), shapeBounds.Y + shapeFormula69["y1"]);
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.MathNotEqual:
        Dictionary<string, float> shapeFormula70 = formulaValues.ParseShapeFormula(AutoShapeType.MathNotEqual);
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.X + shapeFormula70["x8"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula70["y1"] + (double) shapeFormula70["y2"]) / 2.0));
          case 1:
            return new PointF(shapeBounds.X + shapeFormula70["x8"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula70["y3"] + (double) shapeFormula70["y4"]) / 2.0));
          case 2:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula70["drx"] + (double) shapeFormula70["dlx"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula70["dry"] + (double) shapeFormula70["dly"]) / 2.0));
          case 3:
            return new PointF(shapeBounds.X + shapeFormula70["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula70["y1"] + (double) shapeFormula70["y2"]) / 2.0));
          case 4:
            return new PointF(shapeBounds.X + shapeFormula70["x1"], (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula70["y3"] + (double) shapeFormula70["y4"]) / 2.0));
          case 5:
            return new PointF((float) ((2.0 * (double) shapeBounds.X + (double) shapeFormula70["rx"] + (double) shapeFormula70["lx"]) / 2.0), (float) ((2.0 * (double) shapeBounds.Y + (double) shapeFormula70["ry"] + (double) shapeFormula70["ly"]) / 2.0));
          default:
            return new PointF(0.0f, 0.0f);
        }
      case AutoShapeType.Cloud:
        switch (connnectionSite)
        {
          case 0:
            return new PointF(shapeBounds.Right, shapeBounds.Y + shapeBounds.Height / 2f);
          case 1:
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, shapeBounds.Y + shapeBounds.Height);
          case 2:
            return new PointF(shapeBounds.X, shapeBounds.Y + shapeBounds.Height / 2f);
          case 3:
            SizeF sizeF7 = new SizeF((float) ((double) shapeBounds.Width * 5333.0 / 43200.0 * 2.0), (float) ((double) shapeBounds.Height * 7267.0 / 43200.0 * 2.0));
            RectangleF rectangleF9 = new RectangleF(shapeBounds.X + (float) (13469.0 * (double) shapeBounds.Width / 43200.0), shapeBounds.Y + (float) (1304.0 * (double) shapeBounds.Height / 43200.0), sizeF7.Width, sizeF7.Height);
            PointF arcPoint41 = this.GetArcPoint((double) rectangleF9.Width / 2.0, (double) rectangleF9.Height / 2.0, 304f);
            return new PointF(shapeBounds.X + shapeBounds.Width / 2f, rectangleF9.Y + arcPoint41.Y);
          default:
            return new PointF(0.0f, 0.0f);
        }
      default:
        return new PointF(0.0f, 0.0f);
    }
  }

  private PointF GetXYPosition(
    RectangleF _rectBounds,
    float xDifference,
    float yDifference,
    float positionRatio)
  {
    return new PointF(_rectBounds.X + _rectBounds.Width * xDifference / positionRatio, _rectBounds.Y + _rectBounds.Height * yDifference / positionRatio);
  }

  private float GetEllipseAngleWithCircleAngle(
    float ellipseWidth,
    float ellipseHeight,
    float circleAngle)
  {
    while ((double) circleAngle > 360.0)
      circleAngle -= 360f;
    while ((double) circleAngle < 0.0)
      circleAngle += 360f;
    float num1 = circleAngle;
    while ((double) num1 > 90.0)
      num1 -= 90f;
    if ((double) num1 == 0.0 || (double) num1 == 90.0)
      return circleAngle;
    if ((double) num1 == 45.0)
      num1 = (float) (Math.Atan((double) ellipseHeight / (double) ellipseWidth) * 180.0 / 3.1415927410125732);
    float num2;
    if ((double) circleAngle > 0.0 && (double) circleAngle < 90.0)
      return num2 = num1 + 0.0f;
    if ((double) circleAngle > 90.0 && (double) circleAngle < 180.0)
      return 180f - num1;
    if ((double) circleAngle > 180.0 && (double) circleAngle < 270.0)
      return num2 = num1 + 180f;
    return (double) circleAngle > 270.0 && (double) circleAngle < 360.0 ? 360f - num1 : circleAngle;
  }

  private PointF GetArcPoint(double xRadius, double yRadius, float angle)
  {
    angle %= 360f;
    double x = Math.Abs(Math.Tan((double) angle * (Math.PI / 180.0)));
    double num1 = Math.Sqrt(Math.Pow(xRadius, 2.0) * Math.Pow(yRadius, 2.0) / (Math.Pow(yRadius, 2.0) + Math.Pow(xRadius, 2.0) * Math.Pow(x, 2.0)));
    double num2 = num1 * x;
    if ((double) angle >= 0.0 && (double) angle < 90.0)
      return new PointF((float) (xRadius + num1), (float) (yRadius + num2));
    if ((double) angle >= 90.0 && (double) angle < 180.0)
      return new PointF((float) (xRadius - num1), (float) (yRadius + num2));
    return (double) angle >= 180.0 && (double) angle < 270.0 ? new PointF((float) (xRadius - num1), (float) (yRadius - num2)) : new PointF((float) (xRadius + num1), (float) (yRadius - num2));
  }

  internal AutoShapeType GetConnectorTypeWithShapeType(ConnectorType connectorType)
  {
    switch (connectorType)
    {
      case ConnectorType.Straight:
        return AutoShapeType.StraightConnector;
      case ConnectorType.Elbow:
        return AutoShapeType.ElbowConnector;
      case ConnectorType.Curve:
        return AutoShapeType.CurvedConnector;
      default:
        return AutoShapeType.StraightConnector;
    }
  }

  internal ConnectorType GetConnectorTypeWithAutoShapeType(AutoShapeType autoShapeType)
  {
    switch (autoShapeType)
    {
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.BentConnector2:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.BentConnector5:
        return ConnectorType.Elbow;
      case AutoShapeType.CurvedConnector:
      case AutoShapeType.CurvedConnector2:
      case AutoShapeType.CurvedConnector4:
      case AutoShapeType.CurvedConnector5:
        return ConnectorType.Curve;
      default:
        return ConnectorType.Straight;
    }
  }

  internal void SetConnectorType(ConnectorType connectorType) => this._type = connectorType;

  private bool IsSupportedShape(AutoShapeType autoShapeType)
  {
    switch (autoShapeType)
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.IsoscelesTriangle:
      case AutoShapeType.Oval:
        return true;
      default:
        return false;
    }
  }

  internal RectangleF GetShapeBounds(Shape shape)
  {
    return new RectangleF((float) shape.Left, (float) shape.Top, (float) shape.Width, (float) shape.Height);
  }

  internal List<PointF> GetBentPoints(PointF sourcePoint, PointF targetPoint)
  {
    List<PointF> bentPoints = new List<PointF>();
    List<PointF> linePoints = new List<PointF>();
    if (this.BeginConnected && this.EndConnected)
      this.DefaultOrthoConnection(sourcePoint, targetPoint, ref linePoints);
    foreach (PointF pointF in linePoints)
      bentPoints.Add(new PointF()
      {
        X = (float) Math.Round((double) pointF.X, 3),
        Y = (float) Math.Round((double) pointF.Y, 3)
      });
    linePoints.Clear();
    return bentPoints;
  }

  internal void DefaultOrthoConnection(
    PointF srcPoint,
    PointF tarPoint,
    ref List<PointF> linePoints)
  {
    RectangleF shapeBounds1 = this.GetShapeBounds(this.BeginConnectedShape as Shape);
    RectangleF shapeBounds2 = this.GetShapeBounds(this.EndConnectedShape as Shape);
    OrthogonalDirection direction1 = this.GetDirection(srcPoint, shapeBounds1);
    OrthogonalDirection direction2 = this.GetDirection(tarPoint, shapeBounds2);
    RectCorners rectCorners1 = new RectCorners(shapeBounds1);
    RectCorners rectCorners2 = new RectCorners(shapeBounds2);
    NoOfSeg noOfSeg = NoOfSeg.Zero;
    bool flag = false;
    switch (direction1)
    {
      case OrthogonalDirection.Bottom:
        if (direction2 == OrthogonalDirection.Right)
        {
          flag = true;
          break;
        }
        break;
      case OrthogonalDirection.Left:
        switch (direction2)
        {
          case OrthogonalDirection.Right:
          case OrthogonalDirection.Bottom:
            flag = true;
            break;
        }
        break;
      case OrthogonalDirection.Top:
        switch (direction2)
        {
          case OrthogonalDirection.Right:
          case OrthogonalDirection.Bottom:
          case OrthogonalDirection.Left:
            flag = true;
            break;
        }
        break;
    }
    if (flag)
    {
      this.Swap<PointF>(ref srcPoint, ref tarPoint);
      this.Swap<OrthogonalDirection>(ref direction1, ref direction2);
      this.Swap<RectCorners>(ref rectCorners1, ref rectCorners2);
      this.Swap<RectangleF>(ref shapeBounds1, ref shapeBounds2);
    }
    Thickness thickness1 = new Thickness(5.0);
    Thickness thickness2 = new Thickness(10.0);
    double num1 = Math.Round((double) Math.Abs(srcPoint.X - tarPoint.X), 4);
    double num2 = Math.Round((double) Math.Abs(srcPoint.Y - tarPoint.Y), 4);
    Thickness thickness3;
    if (direction1 == OrthogonalDirection.Right && direction2 == OrthogonalDirection.Left)
    {
      PointF pt1 = new PointF(Math.Max(srcPoint.X, shapeBounds1.Right), srcPoint.Y);
      PointF pt2 = new PointF(Math.Min(tarPoint.X, shapeBounds2.Left), tarPoint.Y);
      double uniformLength = 10.0;
      if ((double) rectCorners1.Bottom.Y + uniformLength >= (double) rectCorners2.Top.Y - uniformLength && (double) rectCorners1.Top.Y - uniformLength <= (double) rectCorners2.Bottom.Y + uniformLength)
        uniformLength = 0.0;
      Thickness thickness4 = new Thickness(uniformLength);
      Thickness thickness5 = new Thickness(uniformLength);
      noOfSeg = num2 != 0.0 || (double) rectCorners1.Right.X >= (double) rectCorners2.Left.X ? ((double) rectCorners1.Right.X + thickness4.Right >= (double) rectCorners2.Left.X - thickness5.Left ? (shapeBounds1.Contains(pt2) || shapeBounds2.Contains(pt1) ? NoOfSeg.Three : ((double) rectCorners1.Bottom.Y > (double) rectCorners2.Top.Y ? ((double) rectCorners1.Top.Y < (double) rectCorners2.Top.Y ? ((double) srcPoint.Y > (double) rectCorners2.Top.Y ? ((double) srcPoint.Y < (double) rectCorners2.Bottom.Y ? NoOfSeg.Five : NoOfSeg.Five) : NoOfSeg.Five) : NoOfSeg.Five) : NoOfSeg.Five)) : NoOfSeg.Three) : NoOfSeg.One;
    }
    else if (direction1 == OrthogonalDirection.Right && direction2 == OrthogonalDirection.Right)
    {
      thickness3 = new Thickness(10.0);
      Thickness thickness6 = new Thickness(10.0);
      noOfSeg = (double) rectCorners1.Right.X < (double) rectCorners2.Right.X ? ((double) rectCorners1.Bottom.Y >= (double) tarPoint.Y ? ((double) rectCorners1.Top.Y <= (double) tarPoint.Y ? ((double) rectCorners1.Right.X >= (double) rectCorners2.Left.X ? (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three) : NoOfSeg.Five) : NoOfSeg.Three) : NoOfSeg.Three) : ((double) srcPoint.Y >= (double) rectCorners2.Top.Y ? ((double) srcPoint.Y <= (double) rectCorners2.Bottom.Y + thickness6.Bottom || (double) rectCorners1.Top.Y <= (double) rectCorners2.Bottom.Y ? ((double) srcPoint.Y >= (double) rectCorners2.Top.Y || (double) rectCorners1.Bottom.Y <= (double) rectCorners2.Top.Y ? ((double) rectCorners1.Right.X < (double) rectCorners2.Left.X || (double) rectCorners2.Right.X < (double) rectCorners1.Left.X ? NoOfSeg.Five : (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three)) : NoOfSeg.Three) : NoOfSeg.Three) : NoOfSeg.Three);
    }
    else if (direction1 == OrthogonalDirection.Right && direction2 == OrthogonalDirection.Top)
    {
      PointF pt3 = new PointF(Math.Max(srcPoint.X, shapeBounds1.Right), srcPoint.Y);
      PointF pt4 = new PointF(tarPoint.X, Math.Min(tarPoint.Y, shapeBounds2.Top));
      double y1 = (double) srcPoint.Y;
      PointF pointF = rectCorners2.Top;
      double num3 = (double) pointF.Y - thickness2.Top;
      if (y1 < num3)
      {
        pointF = rectCorners1.Bottom;
        double y2 = (double) pointF.Y;
        pointF = rectCorners2.Top;
        double y3 = (double) pointF.Y;
        if (y2 < y3)
        {
          pointF = rectCorners1.Right;
          noOfSeg = (double) pointF.X + thickness1.Right >= (double) tarPoint.X ? NoOfSeg.Four : NoOfSeg.Two;
        }
        else
        {
          pointF = rectCorners1.Left;
          noOfSeg = (double) pointF.X <= (double) tarPoint.X ? NoOfSeg.Two : NoOfSeg.Four;
        }
      }
      else
      {
        pointF = rectCorners1.Right;
        double x1 = (double) pointF.X;
        pointF = rectCorners2.Left;
        double x2 = (double) pointF.X;
        if ((double) Math.Abs((float) (x1 - x2)) <= 25.0)
        {
          double y4 = (double) srcPoint.Y;
          pointF = rectCorners2.Top;
          double y5 = (double) pointF.Y;
          if ((double) Math.Abs((float) (y4 - y5)) <= 25.0)
          {
            noOfSeg = NoOfSeg.Two;
            goto label_77;
          }
        }
        pointF = rectCorners1.Right;
        double x3 = (double) pointF.X;
        pointF = rectCorners2.Left;
        double x4 = (double) pointF.X;
        noOfSeg = x3 >= x4 ? (shapeBounds1.Contains(pt4) || shapeBounds2.Contains(pt3) ? NoOfSeg.Two : NoOfSeg.Four) : NoOfSeg.Four;
      }
    }
    else if (direction1 == OrthogonalDirection.Right && direction2 == OrthogonalDirection.Bottom)
    {
      PointF pt5 = new PointF(Math.Max(srcPoint.X, shapeBounds1.Right), srcPoint.Y);
      PointF pt6 = new PointF(tarPoint.X, Math.Max(tarPoint.Y, shapeBounds2.Bottom));
      double y6 = (double) srcPoint.Y;
      PointF pointF = rectCorners2.Bottom;
      double num4 = (double) pointF.Y + thickness2.Bottom;
      if (y6 > num4)
      {
        pointF = rectCorners1.Top;
        double y7 = (double) pointF.Y;
        pointF = rectCorners2.Bottom;
        double y8 = (double) pointF.Y;
        if (y7 > y8)
        {
          pointF = rectCorners1.Right;
          noOfSeg = (double) pointF.X + thickness1.Right >= (double) tarPoint.X ? NoOfSeg.Four : NoOfSeg.Two;
        }
        else
        {
          pointF = rectCorners1.Left;
          noOfSeg = (double) pointF.X <= (double) tarPoint.X ? NoOfSeg.Two : NoOfSeg.Four;
        }
      }
      else
      {
        pointF = rectCorners1.Right;
        double x5 = (double) pointF.X;
        pointF = rectCorners2.Left;
        double x6 = (double) pointF.X;
        if ((double) Math.Abs((float) (x5 - x6)) <= 25.0)
        {
          double y9 = (double) srcPoint.Y;
          pointF = rectCorners2.Bottom;
          double y10 = (double) pointF.Y;
          if ((double) Math.Abs((float) (y9 - y10)) <= 25.0)
          {
            noOfSeg = NoOfSeg.Two;
            goto label_77;
          }
        }
        pointF = rectCorners1.Right;
        double x7 = (double) pointF.X;
        pointF = rectCorners2.Left;
        double x8 = (double) pointF.X;
        noOfSeg = x7 >= x8 ? (shapeBounds1.Contains(pt6) || shapeBounds2.Contains(pt5) ? NoOfSeg.Two : NoOfSeg.Four) : NoOfSeg.Four;
      }
    }
    else if (direction1 == OrthogonalDirection.Bottom && direction2 == OrthogonalDirection.Top)
    {
      PointF pt7 = new PointF(srcPoint.X, Math.Max(srcPoint.Y, shapeBounds1.Bottom));
      PointF pt8 = new PointF(tarPoint.X, Math.Min(tarPoint.Y, shapeBounds2.Top));
      double uniformLength = 10.0;
      double num5 = (double) rectCorners1.Right.X + uniformLength;
      PointF pointF = rectCorners2.Left;
      double num6 = (double) pointF.X - uniformLength;
      if (num5 >= num6)
      {
        pointF = rectCorners1.Left;
        double num7 = (double) pointF.X - uniformLength;
        pointF = rectCorners2.Right;
        double num8 = (double) pointF.X + uniformLength;
        if (num7 <= num8)
          uniformLength = 0.0;
      }
      Thickness thickness7 = new Thickness(uniformLength);
      Thickness thickness8 = new Thickness(uniformLength);
      if (num1 == 0.0)
      {
        pointF = rectCorners1.Bottom;
        double y11 = (double) pointF.Y;
        pointF = rectCorners2.Top;
        double y12 = (double) pointF.Y;
        if (y11 < y12)
        {
          noOfSeg = NoOfSeg.One;
          goto label_77;
        }
      }
      pointF = rectCorners1.Bottom;
      double num9 = (double) pointF.Y + thickness7.Bottom;
      pointF = rectCorners2.Top;
      double num10 = (double) pointF.Y - thickness8.Top;
      if (num9 < num10)
      {
        noOfSeg = NoOfSeg.Three;
      }
      else
      {
        pointF = rectCorners1.Right;
        double num11 = (double) pointF.X + thickness7.Right;
        pointF = rectCorners2.Left;
        double num12 = (double) pointF.X - thickness8.Left;
        if (num11 < num12)
        {
          noOfSeg = NoOfSeg.Five;
        }
        else
        {
          pointF = rectCorners1.Left;
          double num13 = (double) pointF.X - thickness7.Left;
          pointF = rectCorners2.Right;
          double num14 = (double) pointF.X + thickness8.Right;
          noOfSeg = num13 <= num14 ? (shapeBounds1.Contains(pt8) || shapeBounds2.Contains(pt7) ? NoOfSeg.Three : NoOfSeg.Five) : NoOfSeg.Five;
        }
      }
    }
    else if (direction1 == OrthogonalDirection.Bottom && direction2 == OrthogonalDirection.Bottom)
    {
      thickness3 = new Thickness(10.0);
      Thickness thickness9 = new Thickness(10.0);
      noOfSeg = (double) rectCorners1.Bottom.Y >= (double) rectCorners2.Bottom.Y ? ((double) rectCorners1.Left.X <= (double) tarPoint.X ? ((double) rectCorners1.Right.X >= (double) tarPoint.X ? ((double) rectCorners1.Top.Y <= (double) rectCorners2.Bottom.Y ? (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three) : NoOfSeg.Five) : NoOfSeg.Three) : NoOfSeg.Three) : ((double) srcPoint.X >= (double) rectCorners2.Left.X - thickness9.Left ? ((double) srcPoint.X <= (double) rectCorners2.Right.X + thickness9.Right ? ((double) rectCorners1.Bottom.Y >= (double) rectCorners2.Top.Y ? (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three) : NoOfSeg.Five) : NoOfSeg.Three) : NoOfSeg.Three);
    }
    else if (direction1 == OrthogonalDirection.Bottom && direction2 == OrthogonalDirection.Left)
    {
      PointF pt9 = new PointF(srcPoint.X, Math.Max(srcPoint.Y, shapeBounds1.Bottom));
      PointF pt10 = new PointF(Math.Min(tarPoint.X, shapeBounds2.Left), tarPoint.Y);
      double x9 = (double) srcPoint.X;
      PointF pointF = rectCorners2.Left;
      double num15 = (double) pointF.X - thickness2.Left;
      if (x9 < num15)
      {
        pointF = rectCorners1.Right;
        double x10 = (double) pointF.X;
        pointF = rectCorners2.Left;
        double x11 = (double) pointF.X;
        if (x10 < x11)
        {
          pointF = rectCorners1.Bottom;
          noOfSeg = (double) pointF.Y + thickness1.Bottom >= (double) tarPoint.Y ? NoOfSeg.Four : NoOfSeg.Two;
        }
        else
        {
          pointF = rectCorners1.Top;
          noOfSeg = (double) pointF.Y <= (double) tarPoint.Y ? NoOfSeg.Two : NoOfSeg.Four;
        }
      }
      else
      {
        pointF = rectCorners1.Right;
        double x12 = (double) pointF.X;
        pointF = rectCorners2.Left;
        double x13 = (double) pointF.X;
        if ((double) Math.Abs((float) (x12 - x13)) <= 25.0)
        {
          double y13 = (double) tarPoint.Y;
          pointF = rectCorners1.Bottom;
          double y14 = (double) pointF.Y;
          if ((double) Math.Abs((float) (y13 - y14)) <= 25.0)
          {
            noOfSeg = NoOfSeg.Two;
            goto label_77;
          }
        }
        noOfSeg = shapeBounds1.Contains(pt10) || shapeBounds2.Contains(pt9) ? NoOfSeg.Two : NoOfSeg.Four;
      }
    }
    else if (direction1 == OrthogonalDirection.Left && direction2 == OrthogonalDirection.Left)
    {
      Thickness thickness10 = new Thickness(10.0);
      Thickness thickness11 = new Thickness(10.0);
      noOfSeg = (double) rectCorners1.Left.X >= (double) rectCorners2.Left.X ? ((double) srcPoint.Y >= (double) rectCorners2.Top.Y - thickness11.Top ? ((double) srcPoint.Y <= (double) rectCorners2.Bottom.Y + thickness11.Bottom ? ((double) rectCorners1.Left.X <= (double) rectCorners2.Right.X ? (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three) : NoOfSeg.Five) : NoOfSeg.Three) : NoOfSeg.Three) : ((double) rectCorners1.Bottom.Y + thickness10.Bottom >= (double) tarPoint.Y ? ((double) rectCorners1.Top.Y - thickness10.Top <= (double) tarPoint.Y ? ((double) rectCorners1.Right.X < (double) rectCorners2.Left.X || (double) rectCorners2.Right.X < (double) rectCorners1.Left.X ? NoOfSeg.Five : (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three)) : NoOfSeg.Three) : NoOfSeg.Three);
    }
    else if (direction1 == OrthogonalDirection.Left && direction2 == OrthogonalDirection.Top)
    {
      PointF pt11 = new PointF(Math.Min(srcPoint.X, shapeBounds1.Left), srcPoint.Y);
      PointF pt12 = new PointF(tarPoint.X, Math.Min(tarPoint.Y, shapeBounds2.Top));
      double y15 = (double) srcPoint.Y;
      PointF pointF = rectCorners2.Top;
      double num16 = (double) pointF.Y - thickness2.Top;
      if (y15 < num16)
      {
        pointF = rectCorners1.Bottom;
        double y16 = (double) pointF.Y;
        pointF = rectCorners2.Top;
        double y17 = (double) pointF.Y;
        if (y16 < y17)
        {
          pointF = rectCorners1.Left;
          noOfSeg = (double) pointF.X - thickness1.Left <= (double) tarPoint.X ? NoOfSeg.Four : NoOfSeg.Two;
        }
        else
        {
          pointF = rectCorners1.Right;
          noOfSeg = (double) pointF.X >= (double) tarPoint.X ? NoOfSeg.Two : NoOfSeg.Four;
        }
      }
      else
      {
        pointF = rectCorners1.Left;
        double x14 = (double) pointF.X;
        pointF = rectCorners2.Right;
        double x15 = (double) pointF.X;
        if ((double) Math.Abs((float) (x14 - x15)) <= 25.0)
        {
          double y18 = (double) srcPoint.Y;
          pointF = rectCorners2.Top;
          double y19 = (double) pointF.Y;
          if ((double) Math.Abs((float) (y18 - y19)) <= 25.0)
          {
            noOfSeg = NoOfSeg.Two;
            goto label_77;
          }
        }
        if (shapeBounds1.Contains(pt12) || shapeBounds2.Contains(pt11))
        {
          noOfSeg = NoOfSeg.Two;
        }
        else
        {
          pointF = rectCorners1.Left;
          double x16 = (double) pointF.X;
          pointF = rectCorners2.Right;
          double x17 = (double) pointF.X;
          noOfSeg = x16 <= x17 ? NoOfSeg.Four : NoOfSeg.Four;
        }
      }
    }
    else if (direction1 == OrthogonalDirection.Top && direction2 == OrthogonalDirection.Top)
    {
      thickness3 = new Thickness(10.0);
      Thickness thickness12 = new Thickness(10.0);
      if ((double) rectCorners1.Top.Y < (double) rectCorners2.Top.Y)
      {
        noOfSeg = (double) rectCorners1.Left.X <= (double) tarPoint.X ? ((double) rectCorners1.Right.X >= (double) tarPoint.X ? ((double) rectCorners1.Bottom.Y >= (double) rectCorners2.Top.Y ? (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three) : NoOfSeg.Five) : NoOfSeg.Three) : NoOfSeg.Three;
      }
      else
      {
        double x18 = (double) srcPoint.X;
        PointF pointF = rectCorners2.Right;
        double x19 = (double) pointF.X;
        if (x18 > x19)
        {
          noOfSeg = NoOfSeg.Three;
        }
        else
        {
          double x20 = (double) srcPoint.X;
          pointF = rectCorners2.Left;
          double x21 = (double) pointF.X;
          if (x20 < x21)
          {
            noOfSeg = NoOfSeg.Three;
          }
          else
          {
            pointF = rectCorners1.Top;
            double y20 = (double) pointF.Y;
            pointF = rectCorners2.Bottom;
            double y21 = (double) pointF.Y;
            noOfSeg = y20 <= y21 ? (num1 == 0.0 || num2 == 0.0 ? NoOfSeg.One : NoOfSeg.Three) : NoOfSeg.Five;
          }
        }
      }
    }
label_77:
    if (flag)
    {
      this.Swap<PointF>(ref srcPoint, ref tarPoint);
      this.Swap<OrthogonalDirection>(ref direction1, ref direction2);
      this.Swap<RectCorners>(ref rectCorners1, ref rectCorners2);
      this.Swap<RectangleF>(ref shapeBounds1, ref shapeBounds2);
    }
    this.RunPoint = srcPoint;
    linePoints.Add(srcPoint);
    this.AddOrthoSegments(noOfSeg, new PointF?(srcPoint), new PointF?(tarPoint), ref rectCorners1, ref rectCorners2, new OrthogonalDirection?(direction1), new OrthogonalDirection?(direction2), ref linePoints);
  }

  internal OrthogonalDirection GetOppositeDirection(OrthogonalDirection direction)
  {
    switch (direction)
    {
      case OrthogonalDirection.Right:
        return OrthogonalDirection.Left;
      case OrthogonalDirection.Bottom:
        return OrthogonalDirection.Top;
      case OrthogonalDirection.Left:
        return OrthogonalDirection.Right;
      case OrthogonalDirection.Top:
        return OrthogonalDirection.Bottom;
      default:
        return OrthogonalDirection.Auto;
    }
  }

  protected void AddLineSegment(PointF point, ref List<PointF> linePoints)
  {
    this.RunPoint = point;
    linePoints.Add(point);
  }

  protected void AddLineSegment(double radius, double angle, ref List<PointF> linePoints)
  {
    PointF pointF = this.Transform(this.RunPoint, radius, angle);
    this.RunPoint = pointF;
    linePoints.Add(pointF);
  }

  internal PointF Transform(PointF s, double length, double angle)
  {
    return new PointF(s.X + (float) length * (float) Math.Cos(angle * Math.PI / 180.0), s.Y + (float) length * (float) Math.Sin(angle * Math.PI / 180.0));
  }

  private void AddOrthoSegments(
    NoOfSeg noOfSeg,
    PointF? srcPoint,
    PointF? tarPoint,
    ref RectCorners sourceCorners,
    ref RectCorners targetCorners,
    OrthogonalDirection? srcDirection,
    OrthogonalDirection? tarDirection,
    ref List<PointF> linePoints)
  {
    double num = 20.0;
    if (srcDirection.Value != tarDirection.Value || noOfSeg == NoOfSeg.Five)
    {
      OrthogonalDirection? nullable = srcDirection;
      OrthogonalDirection oppositeDirection = this.GetOppositeDirection(tarDirection.Value);
      if ((nullable.GetValueOrDefault() != oppositeDirection ? 1 : (!nullable.HasValue ? 1 : 0)) != 0 || noOfSeg == NoOfSeg.Three)
      {
        ref OrthogonalDirection? local = ref srcDirection;
        OrthogonalDirection valueOrDefault = local.GetValueOrDefault();
        if (local.HasValue)
        {
          switch (valueOrDefault)
          {
            case OrthogonalDirection.Right:
              if ((double) sourceCorners.Right.X < (double) targetCorners.Left.X)
              {
                num = Math.Min(num, ((double) targetCorners.Left.X - (double) sourceCorners.Right.X) / 2.0);
                break;
              }
              break;
            case OrthogonalDirection.Bottom:
              if ((double) sourceCorners.Bottom.Y < (double) targetCorners.Top.Y)
              {
                num = Math.Min(num, ((double) targetCorners.Top.Y - (double) sourceCorners.Bottom.Y) / 2.0);
                break;
              }
              break;
            case OrthogonalDirection.Left:
              if ((double) sourceCorners.Left.X > (double) targetCorners.Right.X)
              {
                num = Math.Min(num, ((double) sourceCorners.Left.X - (double) targetCorners.Right.X) / 2.0);
                break;
              }
              break;
            case OrthogonalDirection.Top:
              if ((double) sourceCorners.Top.Y > (double) targetCorners.Bottom.Y)
              {
                num = Math.Min(num, ((double) sourceCorners.Top.Y - (double) targetCorners.Bottom.Y) / 2.0);
                break;
              }
              break;
          }
        }
      }
    }
    switch (noOfSeg)
    {
      case NoOfSeg.One:
        this.AddLineSegment(tarPoint.Value, ref linePoints);
        break;
      case NoOfSeg.Two:
        ref OrthogonalDirection? local1 = ref srcDirection;
        OrthogonalDirection valueOrDefault1 = local1.GetValueOrDefault();
        if (!local1.HasValue)
          break;
        switch (valueOrDefault1)
        {
          case OrthogonalDirection.Right:
          case OrthogonalDirection.Left:
            this.RunPoint = srcPoint.Value;
            this.AddLineSegment(new PointF(tarPoint.Value.X, srcPoint.Value.Y), ref linePoints);
            this.AddLineSegment(tarPoint.Value, ref linePoints);
            return;
          case OrthogonalDirection.Bottom:
          case OrthogonalDirection.Top:
            this.RunPoint = srcPoint.Value;
            this.AddLineSegment(new PointF(srcPoint.Value.X, tarPoint.Value.Y), ref linePoints);
            this.AddLineSegment(tarPoint.Value, ref linePoints);
            return;
          default:
            return;
        }
      case NoOfSeg.Three:
        this.OrthoConnection3Segment(srcPoint.Value, tarPoint.Value, new Thickness(5.0, 5.0, 5.0, 5.0), new Thickness(5.0, 5.0, 5.0, 5.0), srcDirection.Value, tarDirection, ref linePoints, num);
        break;
      case NoOfSeg.Four:
        this.OrthoConnection4Segment(srcPoint.Value, tarPoint.Value, ref sourceCorners, ref targetCorners, new Thickness(5.0, 5.0, 5.0, 5.0), new Thickness(5.0, 5.0, 5.0, 5.0), srcDirection.Value, tarDirection.Value, ref linePoints, new OrthogonalDirection?(), num);
        break;
      case NoOfSeg.Five:
        this.OrthoConnection5Segment(srcPoint.Value, tarPoint.Value, ref sourceCorners, ref targetCorners, new Thickness(5.0, 5.0, 5.0, 5.0), new Thickness(5.0, 5.0, 5.0, 5.0), srcDirection.Value, tarDirection.Value, ref linePoints, num);
        break;
    }
  }

  internal Orientation GetOrientation(OrthogonalDirection direction)
  {
    return direction == OrthogonalDirection.Top || direction == OrthogonalDirection.Bottom ? Orientation.Vertical : Orientation.Horizontal;
  }

  internal OrthogonalDirection FindOrthogonalDirection(PointF s, PointF e)
  {
    return (double) e.X == (double) s.X ? ((double) e.Y > (double) s.Y ? OrthogonalDirection.Bottom : OrthogonalDirection.Top) : ((double) e.X > (double) s.X ? OrthogonalDirection.Right : OrthogonalDirection.Left);
  }

  private void OrthoConnection3Segment(
    PointF srcPoint,
    PointF tarPoint,
    Thickness srcMargin,
    Thickness tarMargin,
    OrthogonalDirection srcDirection,
    OrthogonalDirection? tarDirection,
    ref List<PointF> linePoints,
    double extra)
  {
    double num1 = (double) tarPoint.X - (double) srcPoint.X;
    double num2 = (double) tarPoint.Y - (double) srcPoint.Y;
    double num3 = 0.0;
    double num4 = 0.0;
    double num5 = num3 == 0.0 ? extra : num3 + extra / 2.0;
    double num6 = num4 == 0.0 ? extra : num4 + extra / 2.0;
    if ((Math.Abs(num1) < 0.001 || Math.Abs(num2) < 0.001) && (!tarDirection.HasValue || this.FindOrthogonalDirection(tarPoint, srcPoint) == tarDirection.Value))
    {
      this.RunPoint = srcPoint;
      this.AddLineSegment(tarPoint, ref linePoints);
    }
    else
    {
      switch (srcDirection)
      {
        case OrthogonalDirection.Right:
        case OrthogonalDirection.Left:
          if (srcDirection == OrthogonalDirection.Right)
          {
            if (tarDirection.HasValue)
            {
              OrthogonalDirection? nullable = tarDirection;
              if ((nullable.GetValueOrDefault() != OrthogonalDirection.Right ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                extra = (double) srcPoint.X + num5 >= (double) tarPoint.X + num6 ? num5 : (double) tarPoint.X - (double) srcPoint.X + num6;
            }
          }
          else if (srcDirection == OrthogonalDirection.Left)
          {
            if (tarDirection.HasValue)
            {
              OrthogonalDirection? nullable = tarDirection;
              if ((nullable.GetValueOrDefault() != OrthogonalDirection.Left ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                extra = (double) srcPoint.X - num5 <= (double) tarPoint.X - num6 ? num5 : (double) srcPoint.X - (double) tarPoint.X + num6;
            }
            extra = -extra;
          }
          double num7 = (double) tarPoint.Y - (double) this.RunPoint.Y;
          this.AddLineSegment(extra, 0.0, ref linePoints);
          if ((double) tarPoint.Y - (double) this.RunPoint.Y != 0.0)
            this.AddLineSegment((double) tarPoint.Y - (double) this.RunPoint.Y, 90.0, ref linePoints);
          this.AddLineSegment(tarPoint, ref linePoints);
          break;
        case OrthogonalDirection.Bottom:
        case OrthogonalDirection.Top:
          if (srcDirection == OrthogonalDirection.Bottom)
          {
            if (tarDirection.HasValue)
            {
              OrthogonalDirection? nullable = tarDirection;
              if ((nullable.GetValueOrDefault() != OrthogonalDirection.Bottom ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                extra = (double) srcPoint.Y + num5 >= (double) tarPoint.Y + num6 ? num5 : (double) tarPoint.Y - (double) srcPoint.Y + num6;
            }
          }
          else if (srcDirection == OrthogonalDirection.Top)
          {
            if (tarDirection.HasValue)
            {
              OrthogonalDirection? nullable = tarDirection;
              if ((nullable.GetValueOrDefault() != OrthogonalDirection.Top ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                extra = (double) srcPoint.Y - num5 <= (double) tarPoint.Y - num6 ? num5 : (double) srcPoint.Y - (double) tarPoint.Y + num6;
            }
            extra = -extra;
          }
          double num8 = (double) tarPoint.X - (double) this.RunPoint.X;
          if (srcDirection == OrthogonalDirection.Top)
            this.AddLineSegment(extra, 90.0, ref linePoints);
          else
            this.AddLineSegment(extra, 90.0, ref linePoints);
          if ((double) tarPoint.X - (double) this.RunPoint.X != 0.0)
            this.AddLineSegment((double) tarPoint.X - (double) this.RunPoint.X, 0.0, ref linePoints);
          this.AddLineSegment(tarPoint, ref linePoints);
          break;
      }
    }
  }

  private void OrthoConnection3Segment(
    PointF srcPoint,
    PointF tarPoint,
    ref RectCorners srcCorners,
    ref RectCorners tarCorners,
    Thickness srcMargin,
    Thickness tarMargin,
    Orientation srcOrientation,
    OrthogonalDirection srcDirection,
    OrthogonalDirection tarDirection,
    ref List<PointF> linePoints)
  {
    Rectangle rectangle1 = Rectangle.Round(srcCorners.OuterBounds);
    Rectangle rectangle2 = Rectangle.Round(tarCorners.OuterBounds);
    double radius = 20.0;
    double val2 = radius;
    double num1 = 0.0;
    double num2 = 0.0;
    double num3 = num1 == 0.0 ? val2 : num1 + val2 / 2.0;
    double num4 = num2 == 0.0 ? val2 : num2 + val2 / 2.0;
    switch (srcOrientation)
    {
      case Orientation.Vertical:
        switch (tarDirection)
        {
          case OrthogonalDirection.Right:
          case OrthogonalDirection.Left:
            throw new InvalidOperationException("3 segments cannot be drawn");
          case OrthogonalDirection.Bottom:
            if ((double) rectangle1.Top - srcMargin.Top > (double) rectangle2.Bottom + tarMargin.Bottom && (this.GetOrientation(srcDirection) != Orientation.Horizontal || (double) (rectangle1.Top - rectangle2.Bottom) > radius || (double) rectangle1.Left - srcMargin.Left <= (double) tarPoint.X && (double) rectangle1.Right + srcMargin.Right >= (double) tarPoint.X))
            {
              double num5 = Math.Min((double) (Math.Abs(rectangle1.Top - rectangle2.Bottom) / 2), val2);
              radius = (double) rectangle1.Top - (double) srcPoint.Y - num5;
              break;
            }
            radius = (srcDirection == OrthogonalDirection.Left && (double) srcPoint.X > (double) tarPoint.X || srcDirection == OrthogonalDirection.Right && (double) srcPoint.X < (double) tarPoint.X ? (double) Math.Max((float) rectangle2.Bottom, srcPoint.Y) - (double) srcPoint.Y : ((double) srcPoint.Y < (double) rectangle1.Top - srcMargin.Top || (double) srcPoint.Y > (double) rectangle1.Bottom + srcMargin.Bottom ? (double) rectangle2.Bottom - (double) srcPoint.Y : (double) Math.Max(rectangle2.Bottom, rectangle1.Bottom) - (double) srcPoint.Y)) + ((double) Math.Max(srcPoint.Y, tarPoint.Y) == (double) srcPoint.Y ? num3 : num4);
            break;
          case OrthogonalDirection.Top:
            if ((double) rectangle1.Bottom + srcMargin.Bottom < (double) rectangle2.Top - tarMargin.Top && (this.GetOrientation(srcDirection) != Orientation.Horizontal || (double) (rectangle2.Top - rectangle1.Bottom) > radius || (double) rectangle1.Left - srcMargin.Left <= (double) tarPoint.X && (double) rectangle1.Right + srcMargin.Right >= (double) tarPoint.X))
            {
              double num6 = Math.Min((double) (Math.Abs(rectangle2.Top - rectangle1.Bottom) / 2), val2);
              radius = (double) rectangle1.Bottom - (double) srcPoint.Y + num6;
              break;
            }
            radius = (srcDirection == OrthogonalDirection.Left && (double) srcPoint.X > (double) tarPoint.X || srcDirection == OrthogonalDirection.Right && (double) srcPoint.X < (double) tarPoint.X ? (double) Math.Min((float) rectangle2.Top, srcPoint.Y) - (double) srcPoint.Y : ((double) srcPoint.Y < (double) rectangle1.Top - srcMargin.Top || (double) srcPoint.Y > (double) rectangle1.Bottom + srcMargin.Bottom ? (double) rectangle2.Top - (double) srcPoint.Y : (double) Math.Min(rectangle2.Top, rectangle1.Top) - (double) srcPoint.Y)) - ((double) Math.Min(srcPoint.Y, tarPoint.Y) == (double) srcPoint.Y ? num3 : num4);
            break;
        }
        this.AddLineSegment(radius, 90.0, ref linePoints);
        this.AddLineSegment((double) tarPoint.X - (double) srcPoint.X, 0.0, ref linePoints);
        this.AddLineSegment(tarPoint, ref linePoints);
        break;
      case Orientation.Horizontal:
        switch (tarDirection)
        {
          case OrthogonalDirection.Right:
            if ((double) rectangle1.Left - srcMargin.Left > (double) rectangle2.Right + tarMargin.Right && (this.GetOrientation(srcDirection) != Orientation.Vertical || (double) (rectangle1.Left - rectangle2.Right) > radius || (double) rectangle1.Top - srcMargin.Top <= (double) tarPoint.Y && (double) rectangle1.Bottom + srcMargin.Bottom >= (double) tarPoint.Y))
            {
              double num7 = Math.Min((double) (Math.Abs(rectangle1.Left - rectangle2.Right) / 2), val2);
              radius = (double) rectangle1.Left - (double) srcPoint.X - num7;
              break;
            }
            radius = (srcDirection == OrthogonalDirection.Top && (double) srcPoint.Y > (double) tarPoint.Y || srcDirection == OrthogonalDirection.Bottom && (double) srcPoint.Y < (double) tarPoint.Y ? (double) Math.Max((float) rectangle2.Right, srcPoint.X) - (double) srcPoint.X : ((double) srcPoint.X < (double) rectangle1.Left - srcMargin.Left || (double) srcPoint.X > (double) rectangle1.Right + srcMargin.Right ? (double) rectangle2.Right - (double) srcPoint.X : (double) Math.Max(rectangle2.Right, rectangle1.Right) - (double) srcPoint.X)) + ((double) Math.Max(srcPoint.X, tarPoint.X) == (double) srcPoint.X ? num3 : num4);
            break;
          case OrthogonalDirection.Bottom:
          case OrthogonalDirection.Top:
            throw new InvalidOperationException("3 segments cannot be drawn");
          case OrthogonalDirection.Left:
            if ((double) rectangle1.Right + srcMargin.Right < (double) rectangle2.Left - tarMargin.Left && (this.GetOrientation(srcDirection) != Orientation.Vertical || (double) (rectangle2.Left - rectangle1.Right) > radius || (double) rectangle1.Top - srcMargin.Top <= (double) tarPoint.Y && (double) rectangle1.Bottom + srcMargin.Bottom >= (double) tarPoint.Y))
            {
              double num8 = Math.Min((double) (Math.Abs(rectangle2.Left - rectangle1.Right) / 2), val2);
              radius = (double) rectangle1.Right - (double) srcPoint.X + num8;
              break;
            }
            radius = (srcDirection == OrthogonalDirection.Top && (double) srcPoint.Y > (double) tarPoint.Y || srcDirection == OrthogonalDirection.Bottom && (double) srcPoint.Y < (double) tarPoint.Y ? (double) Math.Min((float) rectangle2.Left, srcPoint.X) - (double) srcPoint.X : ((double) srcPoint.X < (double) rectangle1.Left - srcMargin.Left || (double) srcPoint.X > (double) rectangle1.Right + srcMargin.Right ? (double) rectangle2.Left - (double) srcPoint.X : (double) Math.Min(rectangle2.Left, rectangle1.Left) - (double) srcPoint.X)) - ((double) Math.Min(srcPoint.X, tarPoint.X) == (double) srcPoint.X ? num3 : num4);
            break;
        }
        this.AddLineSegment(radius, 0.0, ref linePoints);
        this.AddLineSegment((double) tarPoint.Y - (double) srcPoint.Y, 90.0, ref linePoints);
        this.AddLineSegment(tarPoint, ref linePoints);
        break;
    }
  }

  private void OrthoConnection4Segment(
    PointF srcPoint,
    PointF tarPoint,
    ref RectCorners sourceCorners,
    ref RectCorners targetCorners,
    Thickness srcMargin,
    Thickness tarMargin,
    OrthogonalDirection srcDirection,
    OrthogonalDirection tarDirection,
    ref List<PointF> linePoints,
    OrthogonalDirection? prevDirection,
    double extra)
  {
    Rectangle rectangle1 = Rectangle.Round(sourceCorners.OuterBounds);
    Rectangle rectangle2 = Rectangle.Round(targetCorners.OuterBounds);
    if (!prevDirection.HasValue)
    {
      srcMargin = new Thickness(2.0, 2.0, 2.0, 2.0);
      tarMargin = this.GetOrientation(tarDirection) != Orientation.Vertical ? new Thickness(5.0, 0.0, 5.0, 0.0) : new Thickness(0.0, 5.0, 0.0, 5.0);
    }
    else
    {
      switch (srcDirection)
      {
        case OrthogonalDirection.Right:
          if (rectangle2.Left > rectangle1.Right && rectangle2.Left - rectangle1.Right < 20)
          {
            extra = (double) ((rectangle2.Left - rectangle1.Right) / 2);
            break;
          }
          break;
        case OrthogonalDirection.Bottom:
          if (rectangle2.Top > rectangle1.Bottom && rectangle2.Top - rectangle1.Bottom < 20)
          {
            extra = (double) ((rectangle2.Top - rectangle1.Bottom) / 2);
            break;
          }
          break;
        case OrthogonalDirection.Left:
          if (rectangle2.Right < rectangle1.Left && rectangle1.Left - rectangle2.Right < 20)
          {
            extra = (double) ((rectangle1.Left - rectangle2.Right) / 2);
            break;
          }
          break;
        case OrthogonalDirection.Top:
          if (rectangle2.Bottom < rectangle1.Top && rectangle1.Top - rectangle2.Bottom < 20)
          {
            extra = (double) ((rectangle1.Top - rectangle2.Bottom) / 2);
            break;
          }
          break;
      }
    }
    switch (srcDirection)
    {
      case OrthogonalDirection.Right:
        if ((double) rectangle1.Right + srcMargin.Right < (double) rectangle2.Right + tarMargin.Right && (double) rectangle1.Right + srcMargin.Right > (double) rectangle2.Left - tarMargin.Left)
        {
          if (tarDirection == OrthogonalDirection.Bottom && (double) srcPoint.Y < (double) tarPoint.Y)
            extra += (double) (rectangle2.Right - rectangle1.Right);
          else if (tarDirection == OrthogonalDirection.Top && (double) srcPoint.Y > (double) tarPoint.Y)
            extra += (double) (rectangle2.Right - rectangle1.Right);
          extra += (double) rectangle1.Right - (double) srcPoint.X;
        }
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Top && tarDirection == OrthogonalDirection.Bottom && (double) srcPoint.Y < (double) tarPoint.Y)
          extra += (double) Math.Abs(srcPoint.X - (float) rectangle2.Right);
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Bottom && tarDirection == OrthogonalDirection.Top && (double) tarPoint.Y < (double) srcPoint.Y)
          extra += (double) Math.Abs(srcPoint.X - (float) rectangle2.Right);
        else
          extra += (double) rectangle1.Right - (double) srcPoint.X;
        this.AddLineSegment(extra, 0.0, ref linePoints);
        break;
      case OrthogonalDirection.Bottom:
        if ((double) rectangle1.Bottom + srcMargin.Bottom < (double) rectangle2.Bottom + tarMargin.Bottom && (double) rectangle1.Bottom + srcMargin.Bottom > (double) rectangle2.Top - tarMargin.Top)
        {
          if (tarDirection == OrthogonalDirection.Right && (double) srcPoint.X < (double) tarPoint.X)
            extra += (double) (rectangle2.Bottom - rectangle1.Bottom);
          else if (tarDirection == OrthogonalDirection.Left && (double) srcPoint.X > (double) tarPoint.X)
            extra += (double) (rectangle2.Bottom - rectangle1.Bottom);
          extra += (double) rectangle1.Bottom - (double) srcPoint.Y;
        }
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Left && tarDirection == OrthogonalDirection.Right && (double) srcPoint.X < (double) tarPoint.X)
          extra += (double) Math.Abs(srcPoint.Y - (float) rectangle2.Bottom);
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Right && tarDirection == OrthogonalDirection.Left && (double) tarPoint.X < (double) srcPoint.X)
          extra += (double) Math.Abs(srcPoint.Y - (float) rectangle2.Bottom);
        else
          extra += (double) rectangle1.Bottom - (double) srcPoint.Y;
        this.AddLineSegment(extra, 90.0, ref linePoints);
        break;
      case OrthogonalDirection.Left:
        if ((double) rectangle1.Left - srcMargin.Left > (double) rectangle2.Left - tarMargin.Left && (double) rectangle1.Left - srcMargin.Left < (double) rectangle2.Right + tarMargin.Right)
        {
          if (tarDirection == OrthogonalDirection.Bottom && (double) srcPoint.Y < (double) tarPoint.Y)
            extra += (double) (rectangle1.Left - rectangle2.Left);
          else if (tarDirection == OrthogonalDirection.Top && (double) srcPoint.Y > (double) tarPoint.Y)
            extra += (double) (rectangle1.Left - rectangle2.Left);
          extra += (double) srcPoint.X - (double) rectangle1.Left;
        }
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Top && tarDirection == OrthogonalDirection.Bottom && (double) srcPoint.Y < (double) tarPoint.Y)
          extra += (double) Math.Abs(srcPoint.X - (float) rectangle2.Right);
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Bottom && tarDirection == OrthogonalDirection.Top && (double) tarPoint.Y < (double) srcPoint.Y)
          extra += (double) Math.Abs(srcPoint.X - (float) rectangle2.Right);
        else
          extra += (double) srcPoint.X - (double) rectangle1.Left;
        this.AddLineSegment(extra, 180.0, ref linePoints);
        break;
      case OrthogonalDirection.Top:
        if ((double) rectangle1.Top - srcMargin.Top > (double) rectangle2.Top + tarMargin.Top && (double) rectangle1.Top - srcMargin.Top < (double) rectangle2.Bottom + tarMargin.Bottom)
        {
          if (tarDirection == OrthogonalDirection.Right && (double) srcPoint.X < (double) tarPoint.X)
            extra += (double) (rectangle1.Top - rectangle2.Top);
          else if (tarDirection == OrthogonalDirection.Left && (double) srcPoint.X > (double) tarPoint.X)
            extra += (double) (rectangle1.Top - rectangle2.Top);
          extra += (double) srcPoint.Y - (double) rectangle1.Top;
        }
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Left && tarDirection == OrthogonalDirection.Right && (double) srcPoint.X < (double) tarPoint.X)
          extra += (double) Math.Abs(srcPoint.Y - (float) rectangle2.Bottom);
        else if (prevDirection.HasValue && prevDirection.Value != OrthogonalDirection.Right && tarDirection == OrthogonalDirection.Left && (double) tarPoint.X < (double) srcPoint.X)
          extra += (double) Math.Abs(srcPoint.Y - (float) rectangle2.Bottom);
        else
          extra += (double) srcPoint.Y - (double) rectangle1.Top;
        this.AddLineSegment(extra, 270.0, ref linePoints);
        break;
    }
    switch (srcDirection)
    {
      case OrthogonalDirection.Right:
      case OrthogonalDirection.Left:
        this.OrthoConnection3Segment(this.RunPoint, tarPoint, ref sourceCorners, ref targetCorners, new Thickness(10.0, 0.0, 10.0, 0.0), new Thickness(10.0, 0.0, 10.0, 0.0), Orientation.Vertical, srcDirection, tarDirection, ref linePoints);
        break;
      case OrthogonalDirection.Bottom:
      case OrthogonalDirection.Top:
        this.OrthoConnection3Segment(this.RunPoint, tarPoint, ref sourceCorners, ref targetCorners, new Thickness(0.0, 10.0, 0.0, 10.0), new Thickness(0.0, 10.0, 0.0, 10.0), Orientation.Horizontal, srcDirection, tarDirection, ref linePoints);
        break;
    }
  }

  private void OrthoConnection5Segment(
    PointF srcPoint,
    PointF tarPoint,
    ref RectCorners sourceCorners,
    ref RectCorners targetCorners,
    Thickness srcMargin,
    Thickness tarMargin,
    OrthogonalDirection srcDirection,
    OrthogonalDirection tarDirection,
    ref List<PointF> linePoints,
    double extra)
  {
    double radius = extra;
    double num1 = (double) sourceCorners.Left.X - srcMargin.Left;
    double num2 = (double) sourceCorners.Right.X + srcMargin.Right;
    double num3 = (double) sourceCorners.Bottom.Y + srcMargin.Bottom;
    double num4 = (double) sourceCorners.Top.Y - srcMargin.Top;
    double num5 = (double) targetCorners.Left.X - tarMargin.Left;
    double num6 = (double) targetCorners.Right.X + tarMargin.Right;
    double num7 = (double) targetCorners.Bottom.Y + tarMargin.Bottom;
    double num8 = (double) targetCorners.Top.Y - tarMargin.Top;
    switch (srcDirection)
    {
      case OrthogonalDirection.Right:
        if ((num4 > num8 && num4 < num7 || num3 < num7 && num3 > num8) && num2 < num6 && num2 >= num5 && extra >= 20.0)
          radius = (double) targetCorners.Right.X - (double) srcPoint.X + radius;
        this.AddLineSegment(radius, 0.0, ref linePoints);
        break;
      case OrthogonalDirection.Bottom:
        if ((num1 > num5 && num1 < num6 || num2 < num6 && num2 > num5) && num3 < num7 && num3 >= num8 && extra >= 20.0)
          radius = (double) targetCorners.Bottom.Y - (double) srcPoint.Y + radius;
        this.AddLineSegment(radius, 90.0, ref linePoints);
        break;
      case OrthogonalDirection.Left:
        if ((num4 > num8 && num4 < num7 || num3 < num7 && num3 > num8) && num1 > num5 && num1 <= num6 && extra >= 20.0)
          radius = (double) srcPoint.X - (double) targetCorners.Left.X + radius;
        this.AddLineSegment(radius, 180.0, ref linePoints);
        break;
      case OrthogonalDirection.Top:
        if ((num1 > num5 && num1 < num6 || num2 < num6 && num2 > num5) && num4 > num8 && num4 <= num7 && extra >= 20.0)
          radius = (double) srcPoint.Y - (double) targetCorners.Top.Y + radius;
        this.AddLineSegment(radius, 270.0, ref linePoints);
        break;
    }
    if (srcDirection == OrthogonalDirection.Top || srcDirection == OrthogonalDirection.Bottom)
      this.OrthoConnection4Segment(this.RunPoint, tarPoint, ref sourceCorners, ref targetCorners, srcMargin, tarMargin, (double) srcPoint.X > (double) tarPoint.X ? OrthogonalDirection.Left : OrthogonalDirection.Right, tarDirection, ref linePoints, new OrthogonalDirection?(srcDirection), 20.0);
    else
      this.OrthoConnection4Segment(this.RunPoint, tarPoint, ref sourceCorners, ref targetCorners, srcMargin, tarMargin, (double) srcPoint.Y > (double) tarPoint.Y ? OrthogonalDirection.Top : OrthogonalDirection.Bottom, tarDirection, ref linePoints, new OrthogonalDirection?(srcDirection), 20.0);
  }

  private void Swap<T>(ref T src, ref T tar)
  {
    T obj = src;
    src = tar;
    tar = obj;
  }

  public OrthogonalDirection GetDirection(PointF port, RectangleF bounds)
  {
    PointF pointF1 = new PointF();
    PointF pointF2 = new PointF();
    PointF pointF3 = new PointF();
    PointF pointF4 = new PointF();
    PointF pointF5 = new PointF();
    RectCorners rectCorners = new RectCorners(bounds);
    PointF topLeft = rectCorners.TopLeft;
    PointF topRight = rectCorners.TopRight;
    PointF bottomRight = rectCorners.BottomRight;
    PointF bottomLeft = rectCorners.BottomLeft;
    PointF center = rectCorners.Center;
    double angle1 = this.FindAngle(center, port);
    double angle2 = this.FindAngle(center, bottomRight);
    double angle3 = this.FindAngle(center, bottomLeft);
    double angle4 = this.FindAngle(center, topLeft);
    double angle5 = this.FindAngle(center, topRight);
    return angle1 <= angle4 || angle1 >= angle5 ? (angle1 < angle2 || angle1 >= angle3 ? (angle1 < angle3 || angle1 > angle4 ? (angle1 >= angle5 || angle1 < angle2 ? OrthogonalDirection.Right : OrthogonalDirection.Right) : OrthogonalDirection.Left) : OrthogonalDirection.Bottom) : OrthogonalDirection.Top;
  }

  public double FindAngle(PointF s, PointF e)
  {
    if (s.Equals((object) e))
      return 0.0;
    PointF pointF = new PointF(e.X, s.Y);
    this.FindLength(s, pointF);
    double d = Math.Asin(this.FindLength(pointF, e) / this.FindLength(e, s));
    if (double.IsNaN(d))
      d = 0.0;
    double angle = d * 180.0 / Math.PI;
    if ((double) s.X < (double) e.X)
    {
      if ((double) s.Y >= (double) e.Y)
        angle = 360.0 - angle;
    }
    else
      angle = (double) s.Y >= (double) e.Y ? 180.0 + angle : 180.0 - angle;
    return angle;
  }

  private double FindLength(PointF s, PointF e)
  {
    return Math.Round(Math.Sqrt(Math.Pow((double) s.X - (double) e.X, 2.0) + Math.Pow((double) s.Y - (double) e.Y, 2.0)), 4);
  }

  public override ISlideItem Clone()
  {
    Connector connector = (Connector) this.MemberwiseClone();
    this.Clone((Shape) connector);
    return (ISlideItem) connector;
  }

  internal override void Close() => base.Close();
}
