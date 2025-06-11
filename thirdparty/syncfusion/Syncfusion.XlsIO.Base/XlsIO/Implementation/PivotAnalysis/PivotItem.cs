// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.PivotItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class PivotItem : INotifyPropertyChanged
{
  private bool allowSort = true;
  private bool showSubTotal = true;
  private double width;
  private bool allowFilter = true;
  private bool enableHyperLinks;
  private string fieldCaption;
  private bool allowRunTimeGroupByField = true;

  public event PropertyChangedEventHandler PropertyChanged;

  public SummaryType SummaryType { get; set; }

  public SummaryBase Summary { get; set; }

  public bool ShowSubTotal
  {
    get => this.showSubTotal;
    set
    {
      this.showSubTotal = value;
      this.OnPropertyChanged(nameof (ShowSubTotal));
    }
  }

  public bool AllowSort
  {
    get => this.allowSort;
    set
    {
      this.allowSort = value;
      this.OnPropertyChanged(nameof (AllowSort));
    }
  }

  public double Width
  {
    get => this.width;
    set
    {
      this.width = value;
      this.OnPropertyChanged(nameof (Width));
    }
  }

  public bool AllowFilter
  {
    get => this.allowFilter;
    set
    {
      this.allowFilter = value;
      this.OnPropertyChanged(nameof (AllowFilter));
    }
  }

  public bool EnableHyperlinks
  {
    get => this.enableHyperLinks;
    set
    {
      this.enableHyperLinks = value;
      this.OnPropertyChanged(nameof (EnableHyperlinks));
    }
  }

  public string FieldMappingName { get; set; }

  public string FieldHeader { get; set; }

  public string FieldCaption
  {
    get => this.fieldCaption;
    set => this.fieldCaption = value;
  }

  public string TotalHeader { get; set; }

  public string Format { get; set; }

  [XmlIgnore]
  public IComparer Comparer { get; set; }

  public bool AllowRunTimeGroupByField
  {
    get => this.allowRunTimeGroupByField;
    set
    {
      this.allowRunTimeGroupByField = value;
      this.OnPropertyChanged(nameof (AllowRunTimeGroupByField));
    }
  }

  internal void Dispose()
  {
    this.FieldHeader = this.FieldMappingName = this.Format = this.TotalHeader = this.FieldCaption = (string) null;
    this.Summary = (SummaryBase) null;
    this.PropertyChanged = (PropertyChangedEventHandler) null;
    this.Comparer = (IComparer) null;
  }

  private void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
