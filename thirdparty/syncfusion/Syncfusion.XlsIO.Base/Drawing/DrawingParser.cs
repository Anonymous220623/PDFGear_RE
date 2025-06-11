// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.DrawingParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Shapes;
using System.IO;

#nullable disable
namespace Syncfusion.Drawing;

internal class DrawingParser
{
  internal AutoShapeType autoShapeType = AutoShapeType.Unknown;
  internal bool isHyperLink;
  internal bool isGroupShape;
  internal double topX;
  internal double topY;
  internal double bottomX;
  internal double bottomY;
  internal AutoShapeConstant autoShapeConstant = AutoShapeConstant.Index_187;
  internal int leftColumn;
  internal int leftColumnOffset;
  internal int posX;
  internal int posY;
  internal int extCX;
  internal int extCY;
  internal int topRow;
  internal int topRowOffset;
  internal int rightColumn;
  internal int rightColumnOffset;
  internal int bottomRow;
  internal int bottomRowOffset;
  internal int cx;
  internal int cy;
  internal ClientAnchor clientAnchor;
  internal string placement;
  internal string relationID;
  internal string anchorName;
  internal int id;
  internal string name;
  internal string descr;
  internal string tittle;
  public double shapeRotation;
  public Stream CustGeomStream;
  internal Stream FillStream;
  public bool IsHidden;
  public bool FlipVertical;
  public bool FlipHorizontal;
  public string preFix = "xdr";
  public string shapeType;
  internal bool m_isCustomGeom;

  internal void AddShape(AutoShapeImpl autoShapeImpl, WorksheetBaseImpl sheet)
  {
    autoShapeImpl.CreateShape(this.autoShapeType, sheet);
    autoShapeImpl.ShapeExt.AnchorType = Helper.GetAnchorType(this.anchorName);
    autoShapeImpl.ShapeExt.ShapeType = !(this.shapeType == "cxnSp") ? ExcelAutoShapeType.sp : ExcelAutoShapeType.cxnSp;
    autoShapeImpl.SetShapeID(this.id);
    autoShapeImpl.Name = this.name;
    autoShapeImpl.AlternativeText = this.descr;
    autoShapeImpl.IsHidden = this.IsHidden;
    autoShapeImpl.IsShapeVisible = !this.IsHidden;
    autoShapeImpl.Title = this.tittle;
    autoShapeImpl.ShapeExt.Rotation = this.shapeRotation;
    autoShapeImpl.ShapeExt.FlipVertical = this.FlipVertical;
    autoShapeImpl.ShapeExt.FlipHorizontal = this.FlipHorizontal;
    if (this.CustGeomStream == null || this.CustGeomStream.Length <= 0L)
      return;
    autoShapeImpl.ShapeExt.PreservedElements.Add("avLst", this.CustGeomStream);
  }
}
