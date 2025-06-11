// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.ComboBoxShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class ComboBoxShapeImpl : ShapeImpl, IComboBoxShape, IShape, IParentApplication
{
  public const int ShapeInstance = 201;
  public const int ShapeVersion = 2;
  private const int DefaultDropLinesCount = 8;
  private IRange m_inputRange;
  private IRange m_cellLinkRange;
  private int m_iSelectedIndex;
  private int m_iDropLines = 8;
  private ExcelComboType m_comboType;
  private bool m_bThreeD;
  private string m_formulaMacro;

  public IRange ListFillRange
  {
    get => this.m_inputRange;
    set
    {
      this.m_inputRange = value;
      if (value == null)
        return;
      (this.m_inputRange as INativePTG).GetNativePtg();
    }
  }

  public IRange LinkedCell
  {
    get => this.m_cellLinkRange;
    set
    {
      this.m_cellLinkRange = !(value is IRanges) ? value : throw new ArgumentOutOfRangeException("The Referenced Range is not valid");
      if (value == null)
        return;
      (this.m_cellLinkRange as INativePTG).GetNativePtg();
    }
  }

  public int SelectedIndex
  {
    get
    {
      if (this.m_cellLinkRange != null && !(this.m_cellLinkRange is ExternalRange))
      {
        if (this.m_cellLinkRange.HasNumber)
          this.m_iSelectedIndex = (int) this.m_cellLinkRange.Number;
        else if (this.m_cellLinkRange.IsBlank)
          this.m_iSelectedIndex = 0;
      }
      return this.m_iSelectedIndex;
    }
    set
    {
      this.m_iSelectedIndex = value;
      if (this.m_cellLinkRange == null || this.m_inputRange == null || this.m_cellLinkRange is ExternalRange || (this.Workbook as WorkbookImpl).Loading)
        return;
      this.m_cellLinkRange.Number = (double) this.m_iSelectedIndex;
    }
  }

  public int DropDownLines
  {
    get => this.m_iDropLines;
    set
    {
      this.m_iDropLines = value >= 0 ? value : throw new ArgumentOutOfRangeException("DropLines");
    }
  }

  public ExcelComboType ComboType => this.m_comboType;

  public bool Display3DShading
  {
    get => this.m_bThreeD;
    set => this.m_bThreeD = value;
  }

  public string SelectedValue
  {
    get
    {
      int selectedIndex = this.SelectedIndex;
      return selectedIndex <= 0 ? (string) null : this.m_inputRange.Cells[selectedIndex - 1].Value;
    }
  }

  internal string FormulaMacro
  {
    get => this.m_formulaMacro;
    set => this.m_formulaMacro = value;
  }

  public new IHyperLink Hyperlink => throw new NotSupportedException("HyperLink");

  public ComboBoxShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.VmlShape = true;
  }

  public ComboBoxShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options,
    List<ObjSubRecord> subRecords)
    : base(application, parent, shapeContainer, options)
  {
    this.VmlShape = true;
    this.ParseSubRecords(subRecords);
  }

  private void ParseSubRecords(List<ObjSubRecord> subRecords)
  {
    if (subRecords == null)
      throw new ArgumentNullException(nameof (subRecords));
    int index = 0;
    for (int count = subRecords.Count; index < count; ++index)
    {
      ObjSubRecord subRecord = subRecords[index];
      switch (subRecord.Type)
      {
        case TObjSubRecordType.ftSbs:
          this.ParseSbsRecord((ftSbs) subRecord);
          break;
        case TObjSubRecordType.ftSbsFormula:
          this.ParseSbsFormula((ftSbsFormula) subRecord);
          break;
        case TObjSubRecordType.ftLbsData:
          this.ParseLbsData((ftLbsData) subRecord);
          break;
      }
    }
  }

  private void ParseSbsFormula(ftSbsFormula ftSbsFormula)
  {
    if (ftSbsFormula == null)
      throw new ArgumentNullException(nameof (ftSbsFormula));
    this.m_cellLinkRange = (ftSbsFormula.Formula[0] as IRangeGetter).GetRange(this.Workbook, this.Worksheet as IWorksheet);
  }

  private void ParseLbsData(ftLbsData ftLbsData)
  {
    Ptg[] ptgArray = ftLbsData != null ? ftLbsData.Formula : throw new ArgumentNullException(nameof (ftLbsData));
    if ((ptgArray != null ? ptgArray.Length : 0) > 0)
      this.m_inputRange = (ptgArray[0] as IRangeGetter).GetRange(this.Workbook, this.Worksheet as IWorksheet);
    this.m_iSelectedIndex = ftLbsData.SelectedIndex;
    this.m_comboType = ftLbsData.ComboType;
    this.m_bThreeD = !ftLbsData.NoThreeD;
    this.m_iDropLines = (int) ftLbsData.DropData.LinesNumber;
  }

  private void ParseSbsRecord(ftSbs ftSbs)
  {
    this.m_iSelectedIndex = ftSbs != null ? ftSbs.Value : throw new ArgumentNullException(nameof (ftSbs));
    this.m_iDropLines = ftSbs.Page;
  }

  protected override void OnPrepareForSerialization()
  {
    this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
    this.m_shape.Version = 2;
    this.m_shape.Instance = 201;
    this.m_shape.IsHaveAnchor = true;
    this.m_shape.IsHaveSpt = true;
  }

  [CLSCompliant(false)]
  protected override void SerializeShape(MsofbtSpgrContainer spgrContainer)
  {
    if (spgrContainer == null)
      throw new ArgumentNullException(nameof (spgrContainer));
    MsofbtSpContainer record1 = (MsofbtSpContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpContainer);
    MsofbtClientData record2 = (MsofbtClientData) MsoFactory.GetRecord(MsoRecords.msofbtClientData);
    OBJRecord record3 = this.Obj;
    bool bObjExist = record3 != null;
    this.UpdateCmo(ref record3, bObjExist, record2);
    this.UpdateSbs(record3, bObjExist);
    this.UpdateSbsFormula(record3, bObjExist);
    this.UpdateLbsData(record3, bObjExist);
    if (this.m_cellLinkRange == null)
    {
      int selectedIndex = this.SelectedIndex;
    }
    if (!bObjExist)
    {
      ftEnd record4 = new ftEnd();
      record3.AddSubRecord((ObjSubRecord) record4);
    }
    record2.AddRecord((BiffRecordRaw) record3);
    record1.AddItem((MsoBase) this.m_shape);
    MsofbtOPT itemToAdd = this.SerializeOptions((MsoBase) record1);
    if (itemToAdd.Properties.Length > 0)
      record1.AddItem((MsoBase) itemToAdd);
    record1.AddItem((MsoBase) this.ClientAnchor);
    record1.AddItem((MsoBase) record2);
    spgrContainer.AddItem((MsoBase) record1);
  }

  private void UpdateLbsData(OBJRecord obj, bool bObjExist)
  {
    ftLbsData record = (ftLbsData) null;
    bool flag = !bObjExist || (record = (ftLbsData) obj.FindSubRecord(TObjSubRecordType.ftLbsData)) == null;
    if (flag)
      record = new ftLbsData();
    record.Formula = this.m_inputRange != null ? (this.m_inputRange as INativePTG).GetNativePtg() : (Ptg[]) null;
    record.SelectedIndex = this.SelectedIndex;
    record.DropData.LinesNumber = (short) this.DropDownLines;
    record.EditId = 0;
    if (this.m_comboType != ExcelComboType.Regular)
      record.ComboType = this.m_comboType;
    record.NoThreeD = !this.m_bThreeD;
    if (!flag)
      return;
    obj.AddSubRecord((ObjSubRecord) record);
  }

  private void UpdateSbsFormula(OBJRecord obj, bool bObjExist)
  {
    ftSbsFormula record = (ftSbsFormula) null;
    if (this.m_cellLinkRange != null)
    {
      bool flag = !bObjExist || (record = (ftSbsFormula) obj.FindSubRecord(TObjSubRecordType.ftSbsFormula)) == null;
      if (flag)
        record = new ftSbsFormula();
      record.Formula = (this.m_cellLinkRange as INativePTG).GetNativePtg();
      if (!flag)
        return;
      obj.AddSubRecord((ObjSubRecord) record);
    }
    else
    {
      int subRecordIndex;
      if ((subRecordIndex = obj.FindSubRecordIndex(TObjSubRecordType.ftSbsFormula)) < 0)
        return;
      obj.RecordsList.RemoveAt(subRecordIndex);
    }
  }

  private void UpdateSbs(OBJRecord obj, bool bObjExist)
  {
    ftSbs record;
    if (!bObjExist || (record = (ftSbs) obj.FindSubRecord(TObjSubRecordType.ftSbs)) == null)
    {
      record = new ftSbs();
      record.Value = 2;
      record.ScrollBarWidth = 16 /*0x10*/;
      record.Minimum = 0;
      record.Maximum = 2;
      record.Increment = 1;
      record.Horizontal = 0;
      obj.AddSubRecord((ObjSubRecord) record);
    }
    record.Page = this.m_iDropLines;
  }

  private void UpdateCmo(ref OBJRecord obj, bool bObjExist, MsofbtClientData clientData)
  {
    ftCmo record;
    if (!bObjExist)
    {
      obj = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record = new ftCmo();
      record.ObjectType = TObjType.otComboBox;
      record.Printable = true;
      record.Locked = true;
      record.AutoLine = false;
      obj.AddSubRecord((ObjSubRecord) record);
    }
    else
      record = this.Obj.RecordsList[0] as ftCmo;
    record.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    ComboBoxShapeImpl combobox = (ComboBoxShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    WorksheetBaseImpl worksheet = combobox.Worksheet;
    WorkbookImpl parentWorkbook = worksheet.ParentWorkbook;
    if (this.m_inputRange != null)
      combobox.m_inputRange = (this.m_inputRange as ICombinedRange).Clone((object) worksheet, hashNewNames, parentWorkbook);
    if (this.m_cellLinkRange != null)
      combobox.m_cellLinkRange = (this.m_cellLinkRange as ICombinedRange).Clone((object) worksheet, hashNewNames, parentWorkbook);
    if (addToCollections)
      (combobox.Worksheet.ComboBoxes as ComboBoxCollection).AddComboBox((IComboBoxShape) combobox);
    return (IShape) combobox;
  }
}
