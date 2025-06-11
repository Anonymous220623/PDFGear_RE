// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FormatImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.FormatParser;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Threading;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class FormatImpl : CommonObject, INumberFormat, IParentApplication, ICloneParent
{
  private FormatRecord m_format;
  private FormatSectionCollection m_parsedFormat;
  private FormatParserImpl m_parser;

  protected FormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  [CLSCompliant(false)]
  public FormatImpl(IApplication application, object parent, FormatRecord format)
    : this(application, parent)
  {
    this.m_format = format != null ? (FormatRecord) format.Clone() : throw new ArgumentNullException(nameof (format));
  }

  public FormatImpl(IApplication application, object parent, int index, string strFormat)
    : this(application, parent)
  {
    switch (strFormat)
    {
      case null:
        throw new ArgumentNullException(nameof (strFormat));
      case "":
        throw new ArgumentException("strFormat - string cannot be empty.");
      default:
        this.m_format = (FormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Format);
        this.m_format.Index = index;
        this.m_format.FormatString = strFormat;
        break;
    }
  }

  private void SetParents()
  {
    this.m_parser = this.FindParent(typeof (FormatsCollection)) is FormatsCollection parent ? parent.Parser : throw new ArgumentNullException("Parent", "Can't find parent collection of formats.");
  }

  public int Index => this.m_format.Index;

  public string FormatString => this.m_format.FormatString;

  [CLSCompliant(false)]
  public FormatRecord Record => this.m_format;

  public OfficeFormatType FormatType
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].FormatType;
    }
  }

  public bool IsFraction
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].IsFraction;
    }
  }

  public bool IsScientific
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].IsScientific;
    }
  }

  public bool IsThousandSeparator
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].IsThousandSeparator;
    }
  }

  public int DecimalPlaces
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].DecimalNumber;
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_format == null)
      throw new ApplicationException("Format was not initialized propertly.");
    records.Add((IBiffStorage) this.m_format);
  }

  private void PrepareFormat()
  {
    if (this.m_parsedFormat != null)
      return;
    string strFormat = this.FormatString;
    if (this.Parent != null && this.Parent is FormatsCollection && Array.IndexOf<string>((this.Parent as FormatsCollection).DEF_FORMAT_STRING, strFormat) >= 0)
      strFormat = strFormat.Replace("$", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol.ToString());
    this.m_parsedFormat = this.m_parser.Parse(strFormat);
  }

  public OfficeFormatType GetFormatType(double value)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.GetFormatType(value);
  }

  public OfficeFormatType GetFormatType(string value)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.GetFormatType(value);
  }

  public string ApplyFormat(double value) => this.ApplyFormat(value, false);

  public string ApplyFormat(double value, bool bShowHiddenSymbols)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.ApplyFormat(value, bShowHiddenSymbols);
  }

  public string ApplyFormat(string value) => this.ApplyFormat(value, false);

  public string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.ApplyFormat(value, bShowHiddenSymbols);
  }

  internal bool IsTimeFormat(double value) => this.m_parsedFormat.IsTimeFormat(value);

  internal bool IsDateFormat(double value) => this.m_parsedFormat.IsDateFormat(value);

  public object Clone(object parent)
  {
    FormatImpl parent1 = (FormatImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_format = (FormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_format);
    if (this.m_parsedFormat != null)
      parent1.m_parsedFormat = (FormatSectionCollection) this.m_parsedFormat.Clone((object) parent1);
    return (object) parent1;
  }

  internal void Clear()
  {
    this.m_parser.Clear();
    if (this.m_parsedFormat != null)
    {
      this.m_parsedFormat.Dispose();
      this.m_parsedFormat.Clear();
    }
    this.m_format = (FormatRecord) null;
    this.m_parsedFormat = (FormatSectionCollection) null;
    this.Dispose();
  }
}
