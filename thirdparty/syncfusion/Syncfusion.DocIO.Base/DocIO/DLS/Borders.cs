// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Borders
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Borders : FormatBase
{
  public const int LeftKey = 1;
  public const int TopKey = 2;
  public const int BottomKey = 3;
  public const int RightKey = 4;
  public const int VerticalKey = 5;
  public const int HorizontalKey = 6;
  public const int DiagonalDownKey = 7;
  public const int DiagonalUpKey = 8;
  private WTableCell m_currTableCell;
  private WTableRow m_currTableRow;

  public bool NoBorder
  {
    get
    {
      return this.Left.BorderType == BorderStyle.None && this.Right.BorderType == BorderStyle.None && this.Top.BorderType == BorderStyle.None && this.Bottom.BorderType == BorderStyle.None && this.Horizontal.BorderType == BorderStyle.None;
    }
  }

  internal bool IsCellHasNoBorder
  {
    get
    {
      return this.Left.BorderType == BorderStyle.None && this.Right.BorderType == BorderStyle.None && this.Top.BorderType == BorderStyle.None && this.Bottom.BorderType == BorderStyle.None && this.DiagonalDown.BorderType == BorderStyle.None && this.DiagonalUp.BorderType == BorderStyle.None;
    }
  }

  public Border Left => this[1] as Border;

  public Border Top => this[2] as Border;

  public Border Right => this[4] as Border;

  public Border Bottom => this[3] as Border;

  public Border Vertical => this[5] as Border;

  public Border Horizontal => this[6] as Border;

  internal Border DiagonalDown => this[7] as Border;

  internal Border DiagonalUp => this[8] as Border;

  public Color Color
  {
    set => this.Left.Color = this.Right.Color = this.Top.Color = this.Bottom.Color = value;
  }

  public float LineWidth
  {
    set
    {
      this.Left.LineWidth = this.Right.LineWidth = this.Top.LineWidth = this.Bottom.LineWidth = value;
    }
  }

  public BorderStyle BorderType
  {
    set
    {
      this.Left.BorderType = this.Right.BorderType = this.Top.BorderType = this.Bottom.BorderType = value;
      this.Vertical.BorderType = this.Horizontal.BorderType = value;
    }
  }

  public float Space
  {
    set => this.SetSpacing(value);
  }

  public bool Shadow
  {
    set => this.Left.Shadow = this.Right.Shadow = this.Top.Shadow = this.Bottom.Shadow = value;
  }

  internal WTableCell CurrentCell
  {
    get
    {
      if (this.m_currTableCell == null && this.OwnerBase != null && this.OwnerBase is CellFormat)
      {
        CellFormat ownerBase = this.OwnerBase as CellFormat;
        if (ownerBase.OwnerBase != null)
          this.m_currTableCell = ownerBase.OwnerBase as WTableCell;
      }
      return this.m_currTableCell;
    }
  }

  internal WTableRow CurrentRow
  {
    get
    {
      if (this.m_currTableRow == null)
      {
        if (this.CurrentCell != null)
          this.m_currTableRow = this.CurrentCell.OwnerRow;
        else if (this.OwnerBase != null && this.OwnerBase is WTableRow)
          return (WTableRow) null;
      }
      return this.m_currTableRow;
    }
  }

  internal bool IsHTMLRead
  {
    set
    {
      this.Left.IsHTMLRead = this.Right.IsHTMLRead = this.Top.IsHTMLRead = this.Bottom.IsHTMLRead = value;
      this.Vertical.IsHTMLRead = this.Horizontal.IsHTMLRead = value;
    }
  }

  internal bool IsRead
  {
    set => this.Left.IsRead = this.Right.IsRead = this.Top.IsRead = this.Bottom.IsRead = value;
  }

  internal Borders(FormatBase parent, int baseKey)
    : base(parent, baseKey)
  {
    this.InitBorders();
  }

  public Borders() => this.InitBorders();

  internal Borders(Borders borders)
  {
    this.ImportContainer((FormatBase) borders);
    this.InitBorders();
  }

  protected internal override void EnsureComposites()
  {
    this.EnsureComposites(1, 4, 2, 3, 5, 6, 7, 8);
  }

  protected override object GetDefValue(int key)
  {
    throw new ArgumentException("key has invalid value");
  }

  protected override FormatBase GetDefComposite(int key)
  {
    switch (key)
    {
      case 1:
        return this.GetDefComposite(1, (FormatBase) new Border((FormatBase) this, 1));
      case 2:
        return this.GetDefComposite(2, (FormatBase) new Border((FormatBase) this, 2));
      case 3:
        return this.GetDefComposite(3, (FormatBase) new Border((FormatBase) this, 3));
      case 4:
        return this.GetDefComposite(4, (FormatBase) new Border((FormatBase) this, 4));
      case 5:
        return this.GetDefComposite(5, (FormatBase) new Border((FormatBase) this, 5));
      case 6:
        return this.GetDefComposite(6, (FormatBase) new Border((FormatBase) this, 6));
      case 7:
        return this.GetDefComposite(7, (FormatBase) new Border((FormatBase) this, 7));
      case 8:
        return this.GetDefComposite(8, (FormatBase) new Border((FormatBase) this, 8));
      default:
        return (FormatBase) null;
    }
  }

  protected override void InitXDLSHolder()
  {
    if (this.IsDefault)
      this.XDLSHolder.SkipMe = true;
    this.XDLSHolder.AddElement("Bottom", (object) this.Bottom);
    this.XDLSHolder.AddElement("Top", (object) this.Top);
    this.XDLSHolder.AddElement("Left", (object) this.Left);
    this.XDLSHolder.AddElement("Right", (object) this.Right);
    this.XDLSHolder.AddElement("Horizontal", (object) this.Horizontal);
    this.XDLSHolder.AddElement("Vertical", (object) this.Vertical);
  }

  public Borders Clone() => (Borders) this.CloneImpl();

  protected override object CloneImpl() => (object) new Borders(this);

  protected override void OnChange(FormatBase format, int propertyKey)
  {
    base.OnChange(format, propertyKey);
  }

  internal override void ApplyBase(FormatBase baseFormat)
  {
    base.ApplyBase(baseFormat);
    if (baseFormat == null)
    {
      this.Left.ApplyBase((FormatBase) null);
      this.Right.ApplyBase((FormatBase) null);
      this.Top.ApplyBase((FormatBase) null);
      this.Bottom.ApplyBase((FormatBase) null);
      this.Horizontal.ApplyBase((FormatBase) null);
      this.Vertical.ApplyBase((FormatBase) null);
      this.DiagonalDown.ApplyBase((FormatBase) null);
      this.DiagonalUp.ApplyBase((FormatBase) null);
    }
    else
    {
      this.Left.ApplyBase((FormatBase) (baseFormat as Borders).Left);
      this.Right.ApplyBase((FormatBase) (baseFormat as Borders).Right);
      this.Top.ApplyBase((FormatBase) (baseFormat as Borders).Top);
      this.Bottom.ApplyBase((FormatBase) (baseFormat as Borders).Bottom);
      this.Horizontal.ApplyBase((FormatBase) (baseFormat as Borders).Horizontal);
      this.Vertical.ApplyBase((FormatBase) (baseFormat as Borders).Vertical);
      this.DiagonalDown.ApplyBase((FormatBase) (baseFormat as Borders).DiagonalDown);
      this.DiagonalUp.ApplyBase((FormatBase) (baseFormat as Borders).DiagonalUp);
    }
  }

  internal void SetDefaultProperties()
  {
    this.Top.SetDefaultProperties();
    this.Left.SetDefaultProperties();
    this.Bottom.SetDefaultProperties();
    this.Right.SetDefaultProperties();
  }

  private void SetSpacing(float value)
  {
    if (this.ParentFormat is RowFormat || this.ParentFormat is CellFormat)
    {
      if (this.ParentFormat is RowFormat)
        (this.ParentFormat as RowFormat).Paddings.All = value;
      else
        (this.ParentFormat as CellFormat).Paddings.All = value;
    }
    else
    {
      if (!(this.ParentFormat is WParagraphFormat))
        return;
      this.Left.Space = value;
      this.Right.Space = value;
      this.Top.Space = value;
      this.Bottom.Space = value;
    }
  }

  internal bool IsAdjacentBorderSame(Border currentParagraphBorder, Border nextParagraphBorder)
  {
    return currentParagraphBorder.BorderType == nextParagraphBorder.BorderType && (double) currentParagraphBorder.LineWidth == (double) nextParagraphBorder.LineWidth && currentParagraphBorder.Color == nextParagraphBorder.Color && (double) currentParagraphBorder.Space == (double) nextParagraphBorder.Space && currentParagraphBorder.Shadow == nextParagraphBorder.Shadow;
  }

  private void InitBorders()
  {
    this.Left.SetOwner((OwnerHolder) this);
    this.Left.BorderPosition = Border.BorderPositions.Left;
    this.Top.SetOwner((OwnerHolder) this);
    this.Top.BorderPosition = Border.BorderPositions.Top;
    this.Right.SetOwner((OwnerHolder) this);
    this.Right.BorderPosition = Border.BorderPositions.Right;
    this.Bottom.SetOwner((OwnerHolder) this);
    this.Bottom.BorderPosition = Border.BorderPositions.Bottom;
    this.Vertical.SetOwner((OwnerHolder) this);
    this.Vertical.BorderPosition = Border.BorderPositions.Vertical;
    this.Horizontal.SetOwner((OwnerHolder) this);
    this.Horizontal.BorderPosition = Border.BorderPositions.Horizontal;
    this.DiagonalDown.SetOwner((OwnerHolder) this);
    this.DiagonalDown.BorderPosition = Border.BorderPositions.DiagonalDown;
    this.DiagonalUp.SetOwner((OwnerHolder) this);
    this.DiagonalUp.BorderPosition = Border.BorderPositions.DiagonalUp;
  }

  internal override void Close()
  {
    this.m_currTableCell = (WTableCell) null;
    this.m_currTableRow = (WTableRow) null;
  }

  internal void UpdateSourceFormatting(Borders borders)
  {
    this.Left.UpdateSourceFormatting(borders.Left);
    this.Right.UpdateSourceFormatting(borders.Right);
    this.Top.UpdateSourceFormatting(borders.Top);
    this.Bottom.UpdateSourceFormatting(borders.Bottom);
    this.Horizontal.UpdateSourceFormatting(borders.Horizontal);
    this.Vertical.UpdateSourceFormatting(borders.Vertical);
    this.DiagonalDown.UpdateSourceFormatting(borders.DiagonalDown);
    this.DiagonalUp.UpdateSourceFormatting(borders.DiagonalUp);
  }

  internal bool Compare(Borders borders)
  {
    return this.DiagonalUp.Compare(borders.DiagonalUp) && this.DiagonalDown.Compare(borders.DiagonalDown) && this.Horizontal.Compare(borders.Horizontal) && this.Vertical.Compare(borders.Vertical) && this.Top.Compare(borders.Top) && this.Bottom.Compare(borders.Bottom) && this.Left.Compare(borders.Left) && this.Right.Compare(borders.Right);
  }
}
