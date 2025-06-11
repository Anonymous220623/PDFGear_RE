// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.CheckBoxShapeImpl
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
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class CheckBoxShapeImpl : 
  TextBoxShapeBase,
  ICheckBoxShape,
  ITextBoxShape,
  ITextBox,
  IShape,
  IParentApplication
{
  public const int ShapeInstance = 201;
  private const int ShapeVersion = 2;
  private ExcelCheckState m_checkState;
  private IRange m_cellLinkRange;
  private bool m_display3DShading;

  public CheckBoxShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.ShapeType = ExcelShapeType.CheckBox;
    this.Fill.BackColor = ColorExtension.Empty;
    this.Fill.ForeColor = ColorExtension.White;
    this.Line.ForeColor = ColorExtension.DarkGray;
    this.Line.BackColor = ColorExtension.Empty;
    this.Fill.Transparency = 1.0;
    this.VmlShape = true;
    this.HasFill = false;
  }

  [CLSCompliant(false)]
  public CheckBoxShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options)
    : base(application, parent, shapeContainer, options)
  {
    this.ShapeType = ExcelShapeType.CheckBox;
    this.VmlShape = true;
  }

  private void InitializeShape()
  {
    this.ShapeType = ExcelShapeType.CheckBox;
    this.m_bUpdateLineFill = true;
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
    ftCblsFmla record4 = (ftCblsFmla) null;
    ftCmo record5;
    ftCblsData record6;
    if (record3 == null)
    {
      record3 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record5 = new ftCmo();
      record5.ObjectType = TObjType.otCheckBox;
      record5.Printable = true;
      record5.Locked = true;
      record5.AutoLine = false;
      ftEnd record7 = new ftEnd();
      ftCbls record8 = new ftCbls();
      record6 = new ftCblsData();
      record3.AddSubRecord((ObjSubRecord) record5);
      record3.AddSubRecord((ObjSubRecord) record8);
      if (this.LinkedCell != null)
      {
        record4 = new ftCblsFmla();
        record3.AddSubRecord((ObjSubRecord) record4);
      }
      record3.AddSubRecord((ObjSubRecord) record6);
      record3.AddSubRecord((ObjSubRecord) record7);
    }
    else
    {
      record5 = record3.RecordsList[0] as ftCmo;
      ftCbls subRecord = (ftCbls) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftCbls);
      record6 = (ftCblsData) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftCblsData);
      if (this.LinkedCell != null)
        record4 = (ftCblsFmla) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftCblsFmla);
    }
    record2.AddRecord((BiffRecordRaw) record3);
    record6.Display3DShading = this.Display3DShading;
    record6.CheckState = this.m_checkState;
    if (this.LinkedCell != null)
      record4.Formula = (this.LinkedCell as INativePTG).GetNativePtg();
    record5.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
    record1.AddItem((MsoBase) this.m_shape);
    MsofbtOPT itemToAdd = this.SerializeOptions((MsoBase) record1);
    if (itemToAdd.Properties.Length > 0)
      record1.AddItem((MsoBase) itemToAdd);
    if (this.ChildAnchor != null)
      record1.AddItem((MsoBase) this.ChildAnchor);
    else
      record1.AddItem((MsoBase) this.ClientAnchor);
    record1.AddItem((MsoBase) record2);
    this.IsTextLocked = false;
    MsofbtClientTextBox clientTextBoxRecord = this.GetClientTextBoxRecord((MsoBase) record1, ExcelCommentVAlign.Center);
    record1.AddItem((MsoBase) clientTextBoxRecord);
    spgrContainer.AddItem((MsoBase) record1);
  }

  private ObjSubRecord FindSubRecord(List<ObjSubRecord> records, TObjSubRecordType recordType)
  {
    ObjSubRecord subRecord = (ObjSubRecord) null;
    int index = 0;
    for (int count = records.Count; index < count; ++index)
    {
      ObjSubRecord record = records[index];
      if (record.Type == recordType)
      {
        subRecord = record;
        break;
      }
    }
    return subRecord;
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT SerializeOptions(MsoBase parent)
  {
    MsofbtOPT msofbtOpt = base.SerializeOptions(parent);
    MsofbtOPT.FOPTE fopte = new MsofbtOPT.FOPTE();
    msofbtOpt.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.SizeTextToFitShape,
      Int32Value = 1703944
    });
    return msofbtOpt;
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT CreateDefaultOptions()
  {
    MsofbtOPT defaultOptions = base.CreateDefaultOptions();
    defaultOptions.Version = 3;
    defaultOptions.Instance = 2;
    return defaultOptions;
  }

  [CLSCompliant(false)]
  protected override void ParseClientData(MsofbtClientData clientData, ExcelParseOptions options)
  {
    base.ParseClientData(clientData, options);
    List<ObjSubRecord> recordsList = clientData.ObjectRecord.RecordsList;
    int index = 0;
    for (int count = recordsList.Count; index < count; ++index)
    {
      ObjSubRecord objSubRecord = recordsList[index];
      switch (objSubRecord.Type)
      {
        case TObjSubRecordType.ftCblsData:
          this.m_checkState = ((ftCblsData) objSubRecord).CheckState;
          this.m_display3DShading = ((ftCblsData) objSubRecord).Display3DShading;
          break;
        case TObjSubRecordType.ftCblsFmla:
          this.m_cellLinkRange = (((ftCblsFmla) objSubRecord).Formula[0] as IRangeGetter).GetRange(this.Workbook, this.Worksheet as IWorksheet);
          break;
      }
    }
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    CheckBoxShapeImpl checkbox = (CheckBoxShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    WorksheetBaseImpl worksheet = checkbox.Worksheet;
    WorkbookImpl parentWorkbook = worksheet.ParentWorkbook;
    if (this.m_cellLinkRange != null)
      checkbox.m_cellLinkRange = (this.m_cellLinkRange as ICombinedRange).Clone((object) worksheet, hashNewNames, parentWorkbook);
    if (addToCollections)
      (checkbox.Worksheet.CheckBoxes as CheckBoxCollection).AddCheckBox((ICheckBoxShape) checkbox);
    return (IShape) checkbox;
  }

  public new ExcelCommentHAlign HAlignment
  {
    get => throw new NotSupportedException("Alignment");
    set => throw new NotSupportedException("Alignment");
  }

  public new ExcelCommentVAlign VAlignment
  {
    get => throw new NotSupportedException("Alignment");
    set => throw new NotSupportedException("Alignment");
  }

  public new ExcelTextRotation TextRotation
  {
    get => throw new NotSupportedException("Rotation");
    set => throw new NotSupportedException("Rotation");
  }

  public new IHyperLink Hyperlink => throw new NotSupportedException("HyperLink");

  public ExcelCheckState CheckState
  {
    get => this.m_checkState;
    set
    {
      if (this.m_checkState != value)
      {
        this.m_checkState = value;
        if (this.m_cellLinkRange != null && !(this.m_cellLinkRange is ExternalRange))
          this.m_cellLinkRange.Boolean = Convert.ToBoolean(this.GetCheckState(this.m_checkState));
      }
      if (!(this.Workbook as WorkbookImpl).IsLoaded || this.Worksheet.DataHolder == null || this.Worksheet.DataHolder.ControlsStream == null)
        return;
      this.Worksheet.DataHolder.ControlsStream = (Stream) null;
    }
  }

  public IRange LinkedCell
  {
    get => this.m_cellLinkRange;
    set
    {
      if (value is IRanges)
        throw new ArgumentOutOfRangeException("The Referenced Range is not valid");
      if (value == this.m_cellLinkRange)
        return;
      string checkState = this.GetCheckState(this.m_checkState);
      this.m_cellLinkRange = value;
      if (!(value is ExternalRange) && !(this.Workbook as WorkbookImpl).Loading)
      {
        if (checkState == "TRUE" || checkState == "FALSE")
          this.m_cellLinkRange.Boolean = Convert.ToBoolean(checkState);
        else
          this.m_cellLinkRange.Value = "#N/A";
      }
      (this.m_cellLinkRange as INativePTG).GetNativePtg();
    }
  }

  public bool Display3DShading
  {
    get => this.m_display3DShading;
    set => this.m_display3DShading = value;
  }

  private string GetCheckState(ExcelCheckState excelCheckState)
  {
    switch (excelCheckState)
    {
      case ExcelCheckState.Unchecked:
        return "FALSE";
      case ExcelCheckState.Checked:
        return "TRUE";
      case ExcelCheckState.Mixed:
        return "#N/A";
      default:
        return (string) null;
    }
  }
}
