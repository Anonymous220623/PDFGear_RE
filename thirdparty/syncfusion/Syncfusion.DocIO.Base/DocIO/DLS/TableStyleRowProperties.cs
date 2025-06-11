// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TableStyleRowProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TableStyleRowProperties : FormatBase
{
  internal const int CellSpacingKey = 52;
  internal const int IsHiddenKey = 4;
  internal const int IsHeaderKey = 5;
  internal const int IsBreakAcrossPagesKey = 106;
  internal const int RowAlignmentKey = 105;

  public bool IsHidden
  {
    get => (bool) this.GetPropertyValue(4);
    set => this.SetPropertyValue(4, (object) value);
  }

  public bool IsHeader
  {
    get => (bool) this.GetPropertyValue(5);
    set => this.SetPropertyValue(5, (object) value);
  }

  public bool IsBreakAcrossPages
  {
    get => (bool) this.GetPropertyValue(106);
    set => this.SetPropertyValue(106, (object) value);
  }

  public float CellSpacing
  {
    get => (float) this.GetPropertyValue(52);
    set => this.SetPropertyValue(52, (object) value);
  }

  public RowAlignment HorizontalAlignment
  {
    get => (RowAlignment) this.GetPropertyValue(105);
    set => this.SetPropertyValue(105, (object) value);
  }

  internal TableStyleRowProperties(IWordDocument doc)
    : base(doc)
  {
  }

  internal FormatBase GetAsRowFormat()
  {
    RowFormat asRowFormat = new RowFormat();
    asRowFormat.UpdateProperties((FormatBase) this);
    if (this.BaseFormat != null)
      asRowFormat.ApplyBase((this.BaseFormat as TableStyleRowProperties).GetAsRowFormat());
    return (FormatBase) asRowFormat;
  }

  internal object GetPropertyValue(int propertyKey) => this[propertyKey];

  internal void SetPropertyValue(int propertyKey, object value) => this[propertyKey] = value;

  internal override bool HasValue(int propertyKey) => this.HasKey(propertyKey);

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 4:
        return (object) false;
      case 5:
        return (object) false;
      case 52:
        return (object) -1f;
      case 105:
        return (object) RowAlignment.Left;
      case 106:
        return (object) true;
      default:
        throw new NotImplementedException();
    }
  }
}
