// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ParameterImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ParameterImpl : CommonObject, IParameter, IParentApplication
{
  private ExcelParameterDataType m_dataType;
  private ExcelParameterType m_type;
  private string m_promptString;
  private object m_value;
  private IRange m_sourceRange;
  private string m_name;
  private bool m_refreshOnChange;
  private string m_cellRange;
  private byte m_flag;

  public event PromptEventHandler Prompt;

  public ParameterImpl(IApplication application, object parent)
    : base(application, parent)
  {
  }

  public ParameterImpl(
    IApplication application,
    object parent,
    string name,
    ExcelParameterDataType dataType)
    : base(application, parent)
  {
    this.Name = name;
    this.DataType = dataType;
  }

  public void SetParam(ExcelParameterType type, object value)
  {
    this.Type = type;
    switch (type)
    {
      case ExcelParameterType.Prompt:
        this.m_promptString = value is string ? (string) value : throw new ArgumentException("The parameter is incorrect");
        break;
      case ExcelParameterType.Constant:
        switch (value.GetType().Name)
        {
          case "Boolean":
            this.Flag = (byte) 1;
            break;
          case "Double":
            this.Flag = (byte) 3;
            break;
          case "Int32":
            this.Flag = (byte) 4;
            break;
          case "String":
            this.Flag = (byte) 5;
            break;
          case "RangeImpl":
            this.Flag = (byte) 5;
            break;
          default:
            throw new ArgumentException("The parameter is incorrect");
        }
        this.Value = value;
        break;
      case ExcelParameterType.Range:
        this.Flag = (byte) 2;
        if (value is IRange)
          this.m_sourceRange = (IRange) value;
        else if (value is string)
        {
          this.m_cellRange = (string) value;
          if (this.FindParent(typeof (WorkbookImpl)) != null)
            this.m_sourceRange = ChartParser.GetRange((WorkbookImpl) this.FindParent(typeof (WorkbookImpl)), this.m_cellRange);
        }
        if (this.m_sourceRange != null)
          break;
        throw new ArgumentException("The parameter is incorrect");
    }
  }

  public ExcelParameterDataType DataType
  {
    get => this.m_dataType;
    set => this.m_dataType = value;
  }

  public ExcelParameterType Type
  {
    get => this.m_type;
    internal set => this.m_type = value;
  }

  public string PromptString
  {
    get => this.m_promptString;
    internal set => this.m_promptString = value;
  }

  public object Value
  {
    get => this.m_value;
    internal set => this.m_value = value;
  }

  public IRange SourceRange
  {
    get => this.m_sourceRange;
    internal set => this.m_sourceRange = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public bool RefreshOnChange
  {
    get => this.m_refreshOnChange;
    set => this.m_refreshOnChange = value;
  }

  internal string CellRange
  {
    get => this.m_cellRange;
    set => this.m_cellRange = value;
  }

  internal byte Flag
  {
    get => this.m_flag;
    set => this.m_flag = value;
  }

  internal bool RaiseEvent(out object value)
  {
    value = (object) null;
    if (this.Prompt == null)
      return false;
    PromptEventArgs e = new PromptEventArgs();
    this.Prompt((object) this, e);
    value = e.Value;
    return e.Value != null;
  }

  internal ParameterImpl Clone(object parent)
  {
    ParameterImpl parameterImpl = (ParameterImpl) this.MemberwiseClone();
    parameterImpl.SetParent(parent);
    if (this.m_sourceRange != null && this.FindParent(typeof (WorkbookImpl)) != null)
    {
      WorkbookImpl parent1 = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
      parameterImpl.m_sourceRange = (this.m_sourceRange as RangeImpl).Clone((object) this, (Dictionary<string, string>) null, parent1);
    }
    return parameterImpl;
  }

  internal new void Dispose()
  {
    base.Dispose();
    if (this.m_sourceRange == null)
      return;
    this.m_sourceRange = (IRange) null;
  }
}
