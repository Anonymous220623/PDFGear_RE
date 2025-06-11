// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExportEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public class ExportEventArgs : EventArgs
{
  private IRange m_Range;
  private string m_errorMessage;
  private string m_Property;
  private object m_newValue;
  private object m_currentValue;
  private Type m_propertyType;
  private string m_cellValueType;
  private int m_index;

  public IRange Range => this.m_Range;

  public string ErrorMessage => this.m_errorMessage;

  public string Property => this.m_Property;

  public object NewValue
  {
    get => this.m_newValue;
    set => this.m_newValue = value;
  }

  public object CurrentValue => this.m_currentValue;

  public Type PropertyType => this.m_propertyType;

  public string CellValueType => this.m_cellValueType;

  public int Index => this.m_index;

  public ExportEventArgs(
    IRange range,
    string error,
    string errorProp,
    object cellValue,
    Type propertyType,
    string cellValueType,
    int rowRecordIndex)
  {
    this.m_Range = range;
    this.m_errorMessage = error;
    this.m_Property = errorProp;
    this.m_currentValue = cellValue;
    this.m_propertyType = propertyType;
    this.m_cellValueType = cellValueType;
    this.m_index = rowRecordIndex;
  }
}
