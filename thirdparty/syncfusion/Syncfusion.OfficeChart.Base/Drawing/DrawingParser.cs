// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.DrawingParser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Shapes;
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
  public bool IsHidden;
  public bool FlipVertical;
  public bool FlipHorizontal;
  public string preFix = "xdr";
  public string shapeType;

  internal void AddShape(AutoShapeImpl autoShapeImpl, WorksheetImpl sheet)
  {
    autoShapeImpl.CreateShape(this.autoShapeType, sheet);
    autoShapeImpl.ShapeExt.AnchorType = Helper.GetAnchorType(this.anchorName);
    autoShapeImpl.ShapeExt.ShapeType = !(this.shapeType == "cxnSp") ? ExcelAutoShapeType.sp : ExcelAutoShapeType.cxnSp;
    this.clientAnchor = autoShapeImpl.ShapeExt.ClientAnchor;
    if (this.anchorName == "absoluteAnchor")
      this.clientAnchor.SetAnchor(this.posX, this.posY, this.cx, this.cy);
    else if (this.anchorName == "oneCellAnchor")
      this.clientAnchor.SetAnchor(this.topRow, this.topRowOffset, this.leftColumn, this.leftColumnOffset, this.cy, this.cx);
    else if (this.anchorName == "freeFloating")
    {
      int resolution = 96 /*0x60*/;
      this.clientAnchor.SetAnchor(Helper.ConvertEmuToOffset(this.posX, resolution), Helper.ConvertEmuToOffset(this.posY, resolution), Helper.ConvertEmuToOffset(this.extCX, resolution), Helper.ConvertEmuToOffset(this.extCY, resolution));
    }
    else if (this.anchorName == "relSizeAnchor")
      this.clientAnchor.SetAnchor((int) (this.topX * 4000.0), (int) (this.topY * 4000.0), (int) ((this.bottomX - this.topX) * 4000.0), (int) ((this.bottomY - this.topY) * 4000.0));
    else
      this.clientAnchor.SetAnchor(this.topRow, this.topRowOffset, this.leftColumn, this.leftColumnOffset, this.bottomRow, this.bottomRowOffset, this.rightColumn, this.rightColumnOffset);
    if (this.placement != null && this.placement.Length != 0)
      this.clientAnchor.Placement = Helper.GetPlacementType(this.placement);
    autoShapeImpl.SetShapeID(this.id);
    autoShapeImpl.Name = this.name;
    autoShapeImpl.AlternativeText = this.descr;
    autoShapeImpl.IsHidden = this.IsHidden;
    autoShapeImpl.Title = this.tittle;
    autoShapeImpl.ShapeExt.Rotation = this.shapeRotation;
    autoShapeImpl.ShapeExt.FlipVertical = this.FlipVertical;
    autoShapeImpl.ShapeExt.FlipHorizontal = this.FlipHorizontal;
    if (this.CustGeomStream == null || this.CustGeomStream.Length <= 0L)
      return;
    autoShapeImpl.ShapeExt.PreservedElements.Add("avLst", this.CustGeomStream);
  }
}
