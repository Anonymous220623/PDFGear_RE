// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.PivotComputationInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class PivotComputationInfo : INotifyPropertyChanged, IXmlSerializable
{
  private bool allowSort;
  private bool allowFilter;
  private bool enableHyperLinks;
  private string fieldName;
  private string fieldHeader = string.Empty;
  private string fieldCaption;
  private string description;
  private SummaryDisplayLevel innerMostComputationsOnly;
  private string padString = "*";
  private string calculationName;
  private bool allowRunTimeGroupByField = true;
  private SummaryBase summary;
  private SummaryType summaryType;
  private string baseField;
  private DisplayOption displayOption = DisplayOption.All;
  private CalculationType calculationType;
  private string formula;
  private FilterExpression expression;
  private string format = "#.##";
  private object defaultValue;
  private string baseItem;
  private bool isTopColumnSummary;

  public PivotComputationInfo() => this.SummaryType = SummaryType.Count;

  public event PropertyChangedEventHandler PropertyChanged;

  public bool AllowSort
  {
    get => this.allowSort;
    set => this.allowSort = value;
  }

  public bool AllowFilter
  {
    get => this.allowFilter;
    set => this.allowFilter = value;
  }

  public bool IsTopColumnSummary
  {
    get => this.isTopColumnSummary;
    set => this.isTopColumnSummary = value;
  }

  public bool EnableHyperlinks
  {
    get => this.enableHyperLinks;
    set => this.enableHyperLinks = value;
  }

  public string FieldName
  {
    get => this.fieldName;
    set
    {
      this.fieldName = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.FieldName));
    }
  }

  public string FieldHeader
  {
    get => this.fieldHeader;
    set
    {
      this.fieldHeader = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.FieldHeader));
    }
  }

  public string FieldCaption
  {
    get => this.fieldCaption;
    set
    {
      this.fieldCaption = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.FieldCaption));
    }
  }

  public string Description
  {
    get => this.description;
    set
    {
      this.description = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.Description));
    }
  }

  public SummaryDisplayLevel InnerMostComputationsOnly
  {
    get => this.innerMostComputationsOnly;
    set
    {
      this.innerMostComputationsOnly = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.PadString));
    }
  }

  public string PadString
  {
    get => this.padString;
    set
    {
      this.padString = value;
      if (this.summary != null && this.summary is DisplayIfDiscreteValuesEqual)
        ((DisplayIfDiscreteValuesEqual) this.summary).PadString = this.padString;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.PadString));
    }
  }

  public string CalculationName
  {
    get => this.calculationName;
    set
    {
      this.calculationName = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.CalculationName));
    }
  }

  public bool AllowRunTimeGroupByField
  {
    get => this.allowRunTimeGroupByField;
    set
    {
      if (this.Formula != null)
        this.allowRunTimeGroupByField = false;
      else if (this.allowRunTimeGroupByField != value)
        this.allowRunTimeGroupByField = value;
      this.OnPropertyChanged<bool>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, bool>>) (pi => pi.AllowRunTimeGroupByField));
    }
  }

  [XmlIgnore]
  public SummaryBase Summary
  {
    get => this.summary;
    set
    {
      this.summary = value;
      this.OnPropertyChanged<SummaryBase>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, SummaryBase>>) (pi => pi.Summary));
    }
  }

  [System.ComponentModel.DefaultValue(SummaryType.Count)]
  public SummaryType SummaryType
  {
    get => this.summaryType;
    set
    {
      this.summaryType = value;
      if (this.summaryType == SummaryType.Custom)
        return;
      this.summary = PivotComputationInfo.GetSummaryInstance(this.summaryType);
      if (this.summaryType == SummaryType.DisplayIfDiscreteValuesEqual)
        ((DisplayIfDiscreteValuesEqual) this.summary).PadString = this.PadString;
      this.OnPropertyChanged<SummaryType>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, SummaryType>>) (pi => pi.SummaryType));
    }
  }

  public DisplayOption DisplayOption
  {
    get => this.displayOption;
    set
    {
      this.displayOption = value;
      this.OnPropertyChanged<DisplayOption>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, DisplayOption>>) (pi => pi.DisplayOption));
    }
  }

  [System.ComponentModel.DefaultValue(CalculationType.NoCalculation)]
  public CalculationType CalculationType
  {
    get => this.calculationType;
    set
    {
      this.calculationType = value;
      this.OnPropertyChanged<CalculationType>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, CalculationType>>) (pi => pi.CalculationType));
    }
  }

  public string BaseField
  {
    get => this.baseField;
    set
    {
      this.baseField = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.BaseField));
    }
  }

  public string BaseItem
  {
    get => this.baseItem;
    set
    {
      this.baseItem = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.BaseItem));
    }
  }

  public string Formula
  {
    get => this.formula;
    set => this.formula = value;
  }

  [XmlIgnore]
  public FilterExpression Expression
  {
    get => this.expression;
    set => this.expression = value;
  }

  public string FieldType { get; set; }

  public string Format
  {
    get => this.format;
    set
    {
      this.format = value;
      this.OnPropertyChanged<string>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, string>>) (pi => pi.Format));
    }
  }

  public object DefaultValue
  {
    get => this.defaultValue;
    set
    {
      this.defaultValue = value;
      this.OnPropertyChanged<object>((System.Linq.Expressions.Expression<Func<PivotComputationInfo, object>>) (pi => pi.DefaultValue));
    }
  }

  public static SummaryBase GetSummaryInstance(SummaryType st)
  {
    SummaryBase summaryInstance = (SummaryBase) null;
    switch (st)
    {
      case SummaryType.DoubleTotalSum:
        summaryInstance = (SummaryBase) new DoubleTotalSummary();
        break;
      case SummaryType.DoubleAverage:
        summaryInstance = (SummaryBase) new DoubleAverageSummary();
        break;
      case SummaryType.DoubleMaximum:
        summaryInstance = (SummaryBase) new DoubleMaxSummary();
        break;
      case SummaryType.DoubleMinimum:
        summaryInstance = (SummaryBase) new DoubleMinSummary();
        break;
      case SummaryType.DoubleStandardDeviation:
        summaryInstance = (SummaryBase) new DoubleStDevSummary();
        break;
      case SummaryType.DoubleVariance:
        summaryInstance = (SummaryBase) new DoubleVarianceSummary();
        break;
      case SummaryType.Count:
        summaryInstance = (SummaryBase) new CountSummary();
        break;
      case SummaryType.DecimalTotalSum:
        summaryInstance = (SummaryBase) new DecimalTotalSummary();
        break;
      case SummaryType.IntTotalSum:
        summaryInstance = (SummaryBase) new IntTotalSummary();
        break;
      case SummaryType.DisplayIfDiscreteValuesEqual:
        summaryInstance = (SummaryBase) new DisplayIfDiscreteValuesEqual();
        break;
      case SummaryType.Sum:
        summaryInstance = (SummaryBase) new Sum();
        break;
      case SummaryType.Average:
        summaryInstance = (SummaryBase) new Average();
        break;
      case SummaryType.Max:
        summaryInstance = (SummaryBase) new Max();
        break;
      case SummaryType.Min:
        summaryInstance = (SummaryBase) new Min();
        break;
      case SummaryType.CountNumbers:
        summaryInstance = (SummaryBase) new CountSummary();
        break;
      case SummaryType.StdDev:
        summaryInstance = (SummaryBase) new StdDev();
        break;
      case SummaryType.StdDevP:
        summaryInstance = (SummaryBase) new StdDevP();
        break;
      case SummaryType.Var:
        summaryInstance = (SummaryBase) new Variance();
        break;
      case SummaryType.VarP:
        summaryInstance = (SummaryBase) new VarianceP();
        break;
    }
    return summaryInstance;
  }

  public override string ToString() => this.CalculationName;

  public static List<string> GetComputationTypes()
  {
    List<string> computationTypes = new List<string>((IEnumerable<string>) Enum.GetNames(typeof (SummaryType)));
    computationTypes.Remove("Custom");
    computationTypes.Sort();
    return computationTypes;
  }

  public XmlSchema GetSchema() => (XmlSchema) null;

  public void ReadXml(XmlReader reader)
  {
    if (!reader.HasAttributes)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.Name == "CalculationName")
          this.CalculationName = reader.ReadElementContentAsString();
        else if (reader.Name == "FieldName")
          this.FieldName = reader.ReadElementContentAsString();
        else if (reader.Name == "Description")
          this.Description = reader.ReadElementContentAsString();
        else if (reader.Name == "SummaryType")
          this.SummaryType = (SummaryType) Enum.Parse(typeof (SummaryType), reader.ReadElementContentAsString());
        else if (reader.Name == "Format")
          this.Format = reader.ReadElementContentAsString();
        else if (reader.Name == "PadString")
          this.PadString = reader.ReadElementContentAsString();
        else if (reader.Name == "BaseField")
          this.BaseField = reader.ReadElementContentAsString();
        else if (reader.Name == "BaseItem")
          this.BaseItem = reader.ReadElementContentAsString();
        else if (reader.Name == "FieldCaption")
          this.FieldName = reader.ReadElementContentAsString();
      }
      reader.Read();
    }
    else
    {
      this.FieldHeader = reader.GetAttribute("FieldHeader");
      this.FieldName = reader.GetAttribute("FieldName");
      this.FieldCaption = reader.GetAttribute("FieldCaption");
      this.BaseField = reader.GetAttribute("BaseField");
      this.BaseItem = reader.GetAttribute("BaseItem");
      this.PadString = reader.GetAttribute("PadString");
      this.SummaryType = (SummaryType) Enum.Parse(typeof (SummaryType), reader.GetAttribute("SummaryType"), false);
      this.Format = reader.GetAttribute("Format");
      this.Description = reader.GetAttribute("Description");
      if (reader.GetAttribute("CalculationType") != null)
        this.CalculationType = (CalculationType) Enum.Parse(typeof (CalculationType), reader.GetAttribute("CalculationType"), false);
      if (reader.GetAttribute("Formula") != null)
        this.Formula = reader.GetAttribute("Formula");
      this.DefaultValue = (object) reader.GetAttribute("DefaultValue");
      if (reader.GetAttribute("AllowRunTimeGroupByField") != null)
        this.AllowRunTimeGroupByField = bool.Parse(reader.GetAttribute("AllowRunTimeGroupByField"));
      if (reader.GetAttribute("InnerMostComputationsOnly") != null)
        this.InnerMostComputationsOnly = (SummaryDisplayLevel) Enum.Parse(typeof (SummaryDisplayLevel), reader.GetAttribute("InnerMostComputationsOnly"), false);
      if (this.SummaryType == SummaryType.Custom)
      {
        reader.ReadStartElement(nameof (PivotComputationInfo));
        reader.ReadStartElement("CustomType");
        reader.ReadStartElement("Summary");
        Type type = Type.GetType(reader.ReadString());
        reader.ReadEndElement();
        reader.ReadStartElement("Value");
        this.Summary = new XmlSerializer(type).Deserialize(reader) as SummaryBase;
        reader.ReadEndElement();
        reader.ReadEndElement();
      }
      reader.Read();
    }
  }

  public void WriteXml(XmlWriter writer)
  {
    writer.WriteAttributeString("FieldHeader", this.FieldHeader);
    writer.WriteAttributeString("FieldName", this.FieldName);
    writer.WriteAttributeString("FieldCaption", this.FieldCaption);
    writer.WriteAttributeString("PadString", this.PadString);
    writer.WriteAttributeString("SummaryType", this.SummaryType.ToString());
    writer.WriteAttributeString("Format", this.Format);
    writer.WriteAttributeString("Formula", this.Formula);
    writer.WriteAttributeString("CalculationType", this.CalculationType.ToString());
    writer.WriteAttributeString("BaseField", this.BaseField);
    writer.WriteAttributeString("BaseItem", this.BaseItem);
    writer.WriteAttributeString("Description", this.Description);
    writer.WriteAttributeString("AllowRunTimeGroupByField", this.AllowRunTimeGroupByField.ToString());
    writer.WriteAttributeString("InnerMostComputationsOnly", this.InnerMostComputationsOnly.ToString());
    if (this.DefaultValue != null)
      writer.WriteAttributeString("DefaultValue", this.DefaultValue.ToString());
    if (this.SummaryType != SummaryType.Custom)
      return;
    writer.WriteStartElement("CustomType");
    writer.WriteStartElement("Summary");
    writer.WriteValue(this.Summary.GetType().AssemblyQualifiedName);
    writer.WriteEndElement();
    writer.WriteStartElement("Value");
    new XmlSerializer(this.Summary.GetType()).Serialize(writer, (object) this.Summary);
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  internal void Dispose()
  {
    this.FieldHeader = this.FieldName = this.Format = this.PadString = this.Description = this.BaseField = this.BaseItem = this.FieldType = this.FieldCaption = this.CalculationName = this.Formula = (string) null;
    this.Summary = (SummaryBase) null;
    this.SummaryType = SummaryType.DoubleTotalSum;
    this.PropertyChanged = (PropertyChangedEventHandler) null;
    this.DefaultValue = (object) null;
    this.Expression = (FilterExpression) null;
    this.AllowFilter = this.AllowSort = this.EnableHyperlinks = this.IsTopColumnSummary = false;
  }

  private void OnPropertyChanged<R>(System.Linq.Expressions.Expression<Func<PivotComputationInfo, R>> expr)
  {
    this.OnPropertyChanged(((MemberExpression) expr.Body).Member.Name);
  }

  private void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
