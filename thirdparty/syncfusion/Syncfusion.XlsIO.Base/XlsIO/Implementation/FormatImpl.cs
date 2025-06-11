// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.FormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.FormatParser;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class FormatImpl : CommonObject, INumberFormat, IParentApplication, ICloneParent
{
  private FormatRecord m_format;
  private FormatSectionCollection m_parsedFormat;
  private FormatParserImpl m_parser;
  private CFApplier formatApplier = new CFApplier();
  private Dictionary<string, string> m_alternateFormats;
  private bool m_isUsed;

  internal Dictionary<string, string> AlternateFomrats
  {
    get
    {
      if (this.m_alternateFormats == null)
        this.m_alternateFormats = this.GetAlternateFormats();
      return this.m_alternateFormats;
    }
  }

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

  public ExcelFormatType FormatType
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

  internal int NumeratorLen
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].NumeratorLen;
    }
  }

  internal int FractionBase
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].FractionBase;
    }
  }

  internal int DenumaratorLen
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].DenumaratorLen;
    }
  }

  internal int NoOfSignificantDigits
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].SignificantDigits;
    }
  }

  internal int MinExponentDigits
  {
    get
    {
      this.PrepareFormat();
      return this.m_parsedFormat[0].ExponentDigits;
    }
  }

  internal bool isUsed
  {
    get => this.m_isUsed;
    set => this.m_isUsed = value;
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
    string str = this.FormatString;
    if (this.Parent != null && this.Parent is FormatsCollection && Array.IndexOf<string>((this.Parent as FormatsCollection).DEF_FORMAT_STRING, str) >= 0)
      str = str.Replace("$", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol.ToString());
    if (this.AlternateFomrats.ContainsKey(str))
      str = this.AlternateFomrats[str];
    this.m_parsedFormat = this.m_parser.Parse(str);
  }

  private Dictionary<string, string> GetAlternateFormats()
  {
    return new Dictionary<string, string>()
    {
      {
        "aaa",
        "ddd"
      },
      {
        "aaaa",
        "dddd"
      },
      {
        "AAAA",
        "DDDD"
      },
      {
        "AAA",
        "DDD"
      }
    };
  }

  public ExcelFormatType GetFormatType(double value)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.GetFormatType(value);
  }

  public ExcelFormatType GetFormatType(string value)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.GetFormatType(value);
  }

  public string ApplyFormat(double value) => this.ApplyFormat(value, false);

  public string ApplyFormat(double value, bool bShowHiddenSymbols)
  {
    return this.ApplyFormat(value, bShowHiddenSymbols, (RangeImpl) null);
  }

  internal string ApplyFormat(double value, bool bShowHiddenSymbols, RangeImpl cell)
  {
    this.PrepareFormat();
    return this.m_parsedFormat.ApplyFormat(value, bShowHiddenSymbols, cell);
  }

  internal string ApplyCFFormat(
    double value,
    RangeImpl cell,
    FormatImpl formatImpl,
    out ExtendedFormatImpl result)
  {
    WorkbookImpl workbook = cell.Workbook;
    ExtendedFormatsCollection innerExtFormats = workbook.InnerExtFormats;
    this.formatApplier.SetRange(new Rectangle(cell.Column, cell.Row, cell.LastColumn - cell.Column, cell.LastRow - cell.Row));
    ExtendedFormatImpl formatWithoutRegister = workbook.CreateExtFormatWithoutRegister((IExtendedFormat) innerExtFormats[workbook.DefaultXFIndex]);
    result = this.formatApplier.ApplyCFNumberFormats((IRange) cell, formatWithoutRegister);
    string str = !(result.NumberFormat != "General") || !(formatWithoutRegister.NumberFormat == "General") ? formatImpl.ApplyFormat(value, false, cell) : cell.Workbook.InnerFormats[result.NumberFormat].ApplyFormat(value, false, cell);
    if (this.formatApplier.CalculationEnabled)
    {
      (cell.Worksheet.Workbook as WorkbookImpl).CalcEngineMemberValuesOnSheet(true);
      cell.Worksheet.DisableSheetCalculations();
      this.formatApplier.CalculationEnabled = false;
    }
    return str;
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
