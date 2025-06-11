// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TableStyleCellProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TableStyleCellProperties : FormatBase
{
  internal const int BordersKey = 1;
  internal const int PaddingsKey = 3;
  internal const int TextWrapKey = 9;
  internal const int VerticalAlignmentKey = 2;
  internal const int ShadingColorKey = 4;
  internal const int ForeColorKey = 5;
  internal const int TextureStyleKey = 7;
  internal const int TextDirectionKey = 11;
  internal const int HorizontalMergeKey = 8;
  internal const int VerticalMergeKey = 6;
  internal const int PreferredWidthTypeKey = 13;
  internal const int PreferredWidthKey = 14;
  internal const int CellWidthKey = 12;
  internal const int FitTextKey = 10;
  internal const int FormatChangeAuthorNameKey = 15;
  internal const int FormatChangeDateTimeKey = 16 /*0x10*/;

  public Color BackColor
  {
    get => (Color) this.GetPropertyValue(4);
    set => this.SetPropertyValue(4, (object) value);
  }

  public Color ForeColor
  {
    get => (Color) this.GetPropertyValue(5);
    set => this.SetPropertyValue(5, (object) value);
  }

  public TextureStyle TextureStyle
  {
    get => (TextureStyle) this.GetPropertyValue(7);
    set => this.SetPropertyValue(7, (object) value);
  }

  public Borders Borders => this.GetPropertyValue(1) as Borders;

  public Paddings Paddings => this.GetPropertyValue(3) as Paddings;

  public VerticalAlignment VerticalAlignment
  {
    get => (VerticalAlignment) this.GetPropertyValue(2);
    set => this.SetPropertyValue(2, (object) value);
  }

  public bool TextWrap
  {
    get => (bool) this[9];
    set => this[9] = (object) value;
  }

  internal TableStyleCellProperties(IWordDocument doc)
    : base(doc)
  {
  }

  internal object GetPropertyValue(int propertyKey) => this[propertyKey];

  internal void SetPropertyValue(int propertyKey, object value) => this[propertyKey] = value;

  internal override bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  internal override void ApplyBase(FormatBase baseFormat)
  {
    base.ApplyBase(baseFormat);
    this.Borders.ApplyBase((FormatBase) (baseFormat as TableStyleCellProperties).Borders);
    this.Paddings.ApplyBase((FormatBase) (baseFormat as TableStyleCellProperties).Paddings);
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
      case 1:
      case 3:
        return (object) this.GetDefComposite(key);
      case 2:
        return (object) VerticalAlignment.Top;
      case 4:
        return (object) Color.Empty;
      case 5:
        return (object) Color.Empty;
      case 6:
        return (object) CellMerge.None;
      case 7:
        return (object) TextureStyle.TextureNone;
      case 8:
        return (object) CellMerge.None;
      case 9:
        return (object) true;
      case 10:
        return (object) false;
      case 11:
        return (object) TextDirection.Horizontal;
      case 12:
      case 14:
        return (object) 0.0f;
      case 13:
        return (object) FtsWidth.None;
      case 15:
        return (object) string.Empty;
      case 16 /*0x10*/:
        return (object) DateTime.MinValue;
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
