// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.AutoShapeImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class AutoShapeImpl : ShapeImpl
{
  private ShapeImplExt m_shapeExt;
  private bool m_isMoveWithCell;
  private bool m_isSizeWithCell;

  internal AutoShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.ShapeType = OfficeShapeType.AutoShape;
    this.m_bSupportOptions = true;
    this.m_isMoveWithCell = true;
    this.m_isSizeWithCell = false;
  }

  internal ShapeImplExt ShapeExt
  {
    get => this.m_shapeExt;
    set => this.m_shapeExt = value;
  }

  public override ITextFrame TextFrame => (ITextFrame) this.m_shapeExt.TextFrame;

  internal Syncfusion.Drawing.TextFrame TextFrameInternal => this.m_shapeExt.TextFrame;

  public override string AlternativeText
  {
    get => this.m_shapeExt.Description;
    set => this.m_shapeExt.Description = value;
  }

  public override int Id => this.m_shapeExt.ShapeID;

  public override string Name
  {
    get => this.m_shapeExt.Name;
    set => this.m_shapeExt.Name = value;
  }

  public override int BottomRow
  {
    get => this.m_shapeExt.ClientAnchor.BottomRow + 1;
    set => this.m_shapeExt.ClientAnchor.BottomRow = value - 1;
  }

  public override int BottomRowOffset
  {
    get => this.m_shapeExt.ClientAnchor.BottomRowOffset;
    set => this.m_shapeExt.ClientAnchor.BottomRowOffset = value;
  }

  public override int Height
  {
    get => this.m_shapeExt.ClientAnchor.Height;
    set => this.m_shapeExt.ClientAnchor.Height = value;
  }

  public override int Left
  {
    get => this.m_shapeExt.ClientAnchor.Left;
    set => this.m_shapeExt.ClientAnchor.Left = value;
  }

  public override int LeftColumn
  {
    get => this.m_shapeExt.ClientAnchor.LeftColumn + 1;
    set => this.m_shapeExt.ClientAnchor.LeftColumn = value - 1;
  }

  public override int LeftColumnOffset
  {
    get => this.m_shapeExt.ClientAnchor.LeftColumnOffset;
    set => this.m_shapeExt.ClientAnchor.LeftColumnOffset = value;
  }

  public override int RightColumn
  {
    get => this.m_shapeExt.ClientAnchor.RightColumn + 1;
    set => this.m_shapeExt.ClientAnchor.RightColumn = value - 1;
  }

  public override int RightColumnOffset
  {
    get => this.m_shapeExt.ClientAnchor.RightColumnOffset;
    set => this.m_shapeExt.ClientAnchor.RightColumnOffset = value;
  }

  public override int Top
  {
    get => this.m_shapeExt.ClientAnchor.Top;
    set => this.m_shapeExt.ClientAnchor.Top = value;
  }

  public override int TopRow
  {
    get => this.m_shapeExt.ClientAnchor.TopRow + 1;
    set => this.m_shapeExt.ClientAnchor.TopRow = value - 1;
  }

  public override int TopRowOffset
  {
    get => this.m_shapeExt.ClientAnchor.TopRowOffset;
    set => this.m_shapeExt.ClientAnchor.TopRowOffset = value;
  }

  public override int Width
  {
    get => this.m_shapeExt.ClientAnchor.Width;
    set => this.m_shapeExt.ClientAnchor.Width = value;
  }

  public override int ShapeRotation
  {
    get => base.ShapeRotation;
    set => base.ShapeRotation = value;
  }

  public override IOfficeFill Fill => (IOfficeFill) this.m_shapeExt.Fill;

  public override IShapeLineFormat Line => (IShapeLineFormat) this.m_shapeExt.Line;

  public bool IsHidden
  {
    get => this.m_shapeExt.IsHidden;
    set => this.m_shapeExt.IsHidden = value;
  }

  public string Title
  {
    get => this.m_shapeExt.Title;
    set => this.m_shapeExt.Title = value;
  }

  public override bool IsMoveWithCell
  {
    get => this.m_isMoveWithCell;
    set
    {
      this.m_isMoveWithCell = value;
      this.SetPlacementValue();
    }
  }

  public override bool IsSizeWithCell
  {
    get => this.m_isSizeWithCell;
    set
    {
      this.m_isSizeWithCell = value;
      this.SetPlacementValue();
    }
  }

  internal void CreateShape(AutoShapeType type, WorksheetImpl sheetImpl)
  {
    this.m_shapeExt = new ShapeImplExt(type, sheetImpl);
  }

  internal void SetShapeID(int shapeId) => this.m_shapeExt.ShapeID = shapeId;

  private void SetPlacementValue()
  {
    if (this.m_isMoveWithCell && this.m_isSizeWithCell)
      this.m_shapeExt.ClientAnchor.Placement = PlacementType.MoveAndSize;
    if (this.m_isMoveWithCell && !this.m_isSizeWithCell)
      this.m_shapeExt.ClientAnchor.Placement = PlacementType.Move;
    if (this.m_isMoveWithCell || this.m_isSizeWithCell)
      return;
    this.m_shapeExt.ClientAnchor.Placement = PlacementType.FreeFloating;
  }
}
