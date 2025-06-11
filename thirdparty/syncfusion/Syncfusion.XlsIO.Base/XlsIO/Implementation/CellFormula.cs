// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CellFormula
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.XmlSerialization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class CellFormula
{
  private bool m_calculateCell;
  private bool m_dataTable2D;
  private bool m_dataTableRow;
  private Excel2007Serializator.FormulaType m_formulaType;
  private string m_reference;
  private string m_firstDataTableCell;
  private string m_secondDataTableCell;
  private int m_sharedIndex;
  private string m_text;

  internal bool CalculateCell
  {
    get => this.m_calculateCell;
    set => this.m_calculateCell = value;
  }

  internal bool DataTable2D
  {
    get => this.m_dataTable2D;
    set => this.m_dataTable2D = value;
  }

  internal bool DataTableRow
  {
    get => this.m_dataTableRow;
    set => this.m_dataTableRow = value;
  }

  internal Excel2007Serializator.FormulaType FormulaType
  {
    get => this.m_formulaType;
    set => this.m_formulaType = value;
  }

  internal string Reference
  {
    get => this.m_reference;
    set => this.m_reference = value;
  }

  internal string FirstDataTableCell
  {
    get => this.m_firstDataTableCell;
    set => this.m_firstDataTableCell = value;
  }

  internal string SecondDataTableCell
  {
    get => this.m_secondDataTableCell;
    set => this.m_secondDataTableCell = value;
  }

  internal int SharedIndex
  {
    get => this.m_sharedIndex;
    set => this.m_sharedIndex = value;
  }

  internal string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal CellFormula()
  {
  }

  internal CellFormula(string text) => this.m_text = text;
}
