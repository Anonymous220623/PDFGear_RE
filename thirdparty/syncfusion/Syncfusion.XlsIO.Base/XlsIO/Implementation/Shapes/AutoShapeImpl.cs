// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.AutoShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class AutoShapeImpl : ShapeImpl
{
  private ShapeImplExt m_shapeExt;
  private bool m_isMoveWithCell;
  private bool m_isSizeWithCell;
  private bool m_isFill;
  private bool m_isNoFill;
  private bool m_isGroupFill;

  internal AutoShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.ShapeType = ExcelShapeType.AutoShape;
    this.m_bSupportOptions = true;
    this.m_isMoveWithCell = true;
    this.m_isSizeWithCell = false;
  }

  internal new bool IsGroupFill
  {
    get => this.m_isGroupFill;
    set => this.m_isGroupFill = value;
  }

  internal bool IsFill
  {
    get => this.m_isFill;
    set => this.m_isFill = true;
  }

  internal bool IsNoFill
  {
    get => this.m_isNoFill;
    set => this.m_isNoFill = value;
  }

  internal ShapeImplExt ShapeExt
  {
    get => this.m_shapeExt;
    set => this.m_shapeExt = value;
  }

  public bool FlipVertical
  {
    get => this.m_shapeExt.FlipVertical;
    set => this.m_shapeExt.FlipVertical = value;
  }

  public bool FlipHorizontal
  {
    get => this.m_shapeExt.FlipHorizontal;
    set => this.m_shapeExt.FlipHorizontal = value;
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

  public override int ShapeRotation
  {
    get => base.ShapeRotation;
    set
    {
      base.ShapeRotation = value;
      this.ShapeExt.Rotation = (double) value;
    }
  }

  public override IFill Fill => (IFill) this.m_shapeExt.Fill;

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

  internal void CreateShape(AutoShapeType type, WorksheetBaseImpl sheetImpl)
  {
    if (sheetImpl is WorksheetImpl)
      this.m_shapeExt = new ShapeImplExt(type, sheetImpl as WorksheetImpl);
    else
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
