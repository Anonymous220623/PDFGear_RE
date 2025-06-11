// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.PivotCellInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class PivotCellInfo
{
  public int RowIndex;
  public int ColumnIndex;

  public PivotCellInfo() => this.CellType = PivotCellType.ValueCell;

  public double DoubleValue
  {
    get
    {
      if (this.Value != null)
      {
        double result = 0.0;
        if (double.TryParse(this.Value.ToString(), out result))
          return result;
      }
      return 0.0;
    }
  }

  public object Value { get; set; }

  public CoveredCellRange CellRange { get; set; }

  public PivotCellType CellType { get; set; }

  public string Key { get; set; }

  public string FormattedText { get; set; }

  public string Format { get; set; }

  public object Tag { get; set; }

  public SummaryBase Summary { get; set; }

  public PivotCellInfo ParentCell { get; set; }

  public string UniqueText { get; set; }

  [XmlIgnore]
  public List<object> RawValues { get; internal set; }

  public override string ToString() => this.FormattedText;

  public void Dispose()
  {
    this.CellRange = (CoveredCellRange) null;
    this.Format = (string) null;
    this.FormattedText = (string) null;
    this.Key = (string) null;
    this.ParentCell = (PivotCellInfo) null;
    if (this.RawValues != null)
      this.RawValues.Clear();
    this.Summary = (SummaryBase) null;
    this.Tag = (object) null;
    this.UniqueText = (string) null;
    this.Value = (object) null;
  }
}
