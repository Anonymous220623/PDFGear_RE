// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TableStyleTableProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TableStyleTableProperties : FormatBase
{
  internal const int BordersKey = 1;
  internal const int PaddingsKey = 3;
  internal const int ColumnStripeKey = 4;
  internal const int RowStripeKey = 5;
  internal const int CellSpacingKey = 52;
  internal const int LeftIndentKey = 53;
  internal const int AllowPageBreaksKey = 8;
  internal const int RowAlignmentKey = 105;
  internal const int ShadingColorKey = 108;
  internal const int ForeColorKey = 111;
  internal const int TextureStyleKey = 110;

  public Color BackColor
  {
    get => (Color) this.GetPropertyValue(108);
    set => this.SetPropertyValue(108, (object) value);
  }

  public Color ForeColor
  {
    get => (Color) this.GetPropertyValue(111);
    set => this.SetPropertyValue(111, (object) value);
  }

  public TextureStyle TextureStyle
  {
    get => (TextureStyle) this.GetPropertyValue(110);
    set => this.SetPropertyValue(110, (object) value);
  }

  public Borders Borders => this.GetPropertyValue(1) as Borders;

  public Paddings Paddings => this.GetPropertyValue(3) as Paddings;

  public float CellSpacing
  {
    get => (float) this.GetPropertyValue(52);
    set => this.SetPropertyValue(52, (object) value);
  }

  public float LeftIndent
  {
    get => (float) this.GetPropertyValue(53);
    set => this.SetPropertyValue(53, (object) value);
  }

  public bool AllowPageBreaks
  {
    get => (bool) this.GetPropertyValue(8);
    set => this.SetPropertyValue(8, (object) value);
  }

  public RowAlignment HorizontalAlignment
  {
    get => (RowAlignment) this.GetPropertyValue(105);
    set => this.SetPropertyValue(105, (object) value);
  }

  public long ColumnStripe
  {
    get => (long) this.GetPropertyValue(4);
    set => this.SetPropertyValue(4, (object) value);
  }

  public long RowStripe
  {
    get => (long) this.GetPropertyValue(5);
    set => this.SetPropertyValue(5, (object) value);
  }

  internal TableStyleTableProperties(IWordDocument doc)
    : base(doc)
  {
  }

  internal FormatBase GetAsTableFormat()
  {
    RowFormat asTableFormat = new RowFormat();
    asTableFormat.UpdateProperties((FormatBase) this);
    if (this.BaseFormat != null)
      asTableFormat.ApplyBase((this.BaseFormat as TableStyleTableProperties).GetAsTableFormat());
    return (FormatBase) asTableFormat;
  }

  internal object GetPropertyValue(int propertyKey) => this[propertyKey];

  internal void SetPropertyValue(int propertyKey, object value) => this[propertyKey] = value;

  internal override bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal override void ApplyBase(FormatBase baseFormat)
  {
    base.ApplyBase(baseFormat);
    this.Borders.ApplyBase((FormatBase) (baseFormat as TableStyleTableProperties).Borders);
    this.Paddings.ApplyBase((FormatBase) (baseFormat as TableStyleTableProperties).Paddings);
  }

  protected internal override void EnsureComposites()
  {
    this.EnsureComposites(1);
    this.EnsureComposites(3);
  }

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 4:
        return (object) 0L;
      case 5:
        return (object) 0L;
      case 8:
        return (object) true;
      case 52:
        return (object) -1f;
      case 53:
        return (object) 0.0f;
      case 105:
        return (object) RowAlignment.Left;
      case 108:
        return (object) Color.Empty;
      case 110:
        return (object) TextureStyle.TextureNone;
      case 111:
        return (object) Color.Empty;
      default:
        throw new NotImplementedException();
    }
  }

  protected override FormatBase GetDefComposite(int key)
  {
    switch (key)
    {
      case 1:
        return this.GetDefComposite(1, (FormatBase) new Borders((FormatBase) this, 1));
      case 3:
        return this.GetDefComposite(3, (FormatBase) new Paddings((FormatBase) this, 3));
      default:
        return (FormatBase) null;
    }
  }
}
