// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.OptionButtonShapeImpl
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

public class OptionButtonShapeImpl : 
  TextBoxShapeBase,
  IOptionButtonShape,
  ITextBoxShape,
  ITextBox,
  IShape,
  IParentApplication
{
  internal const int ShapeInstance = 201;
  private const int ShapeVersion = 2;
  private static double m_linkedCellValue;
  private static int m_objIndex = 0;
  private ExcelCheckState m_checkState;
  private IRange m_cellLinkRange;
  private bool m_isFirstButton;
  private bool m_display3DShading;
  private byte m_nextButton;
  private bool m_invokeEvent;
  private int m_iIndex;

  internal event ValueChangedEventHandler CheckStateChanged;

  public bool InvokeEvent
  {
    get => this.m_invokeEvent;
    set => this.m_invokeEvent = value;
  }

  internal int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  internal int NextButtonId => (int) this.m_nextButton;

  public new IHyperLink Hyperlink => throw new NotSupportedException("HyperLink");

  public OptionButtonShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.ShapeType = ExcelShapeType.TextBox;
    this.Line.ForeColor = ColorExtension.DarkGray;
    this.Line.BackColor = ColorExtension.DarkGray;
    this.FillColor = ColorExtension.Empty;
    this.Line.HasPattern = false;
    this.HasFill = false;
    this.AlternativeText = (string) null;
    this.VmlShape = true;
  }

  [CLSCompliant(false)]
  public OptionButtonShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options)
    : base(application, parent, shapeContainer, options)
  {
    this.ShapeType = ExcelShapeType.TextBox;
    this.VmlShape = true;
  }

  [CLSCompliant(false)]
  public OptionButtonShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options,
    int optionButtonId)
    : base(application, parent, shapeContainer, options)
  {
    this.ShapeType = ExcelShapeType.TextBox;
    this.VmlShape = true;
    this.m_iIndex = optionButtonId;
  }

  private void InitializeShape()
  {
    this.ShapeType = ExcelShapeType.TextBox;
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
    ftRboData record7;
    if (record3 == null)
    {
      record3 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record5 = new ftCmo();
      record5.ObjectType = TObjType.otOptionBtn;
      record5.Printable = true;
      record5.Locked = this.IsTextLocked;
      record5.AutoLine = false;
      ftEnd record8 = new ftEnd();
      ftCbls record9 = new ftCbls();
      record6 = new ftCblsData();
      record4 = new ftCblsFmla();
      ftRbo record10 = new ftRbo();
      record7 = new ftRboData();
      record3.AddSubRecord((ObjSubRecord) record5);
      record3.AddSubRecord((ObjSubRecord) record9);
      record3.AddSubRecord((ObjSubRecord) record10);
      if (this.IsFirstButton && this.LinkedCell != null)
        record3.AddSubRecord((ObjSubRecord) record4);
      record3.AddSubRecord((ObjSubRecord) record6);
      record3.AddSubRecord((ObjSubRecord) record7);
      record3.AddSubRecord((ObjSubRecord) record8);
    }
    else
    {
      record5 = record3.RecordsList[0] as ftCmo;
      ftCbls subRecord1 = (ftCbls) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftCbls);
      record6 = (ftCblsData) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftCblsData);
      ftRbo subRecord2 = (ftRbo) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftRbo);
      record7 = (ftRboData) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftRboData);
      if (this.LinkedCell != null)
      {
        record4 = (ftCblsFmla) this.FindSubRecord(record3.RecordsList, TObjSubRecordType.ftCblsFmla);
        OptionButtonShapeImpl.m_linkedCellValue = this.LinkedCell.Number;
      }
      if (this.IsFirstButton)
        OptionButtonShapeImpl.m_objIndex = 0;
      ++OptionButtonShapeImpl.m_objIndex;
      double linkedCellValue = OptionButtonShapeImpl.m_linkedCellValue;
      if (OptionButtonShapeImpl.m_linkedCellValue == (double) OptionButtonShapeImpl.m_objIndex)
        this.CheckState = ExcelCheckState.Checked;
    }
    record2.AddRecord((BiffRecordRaw) record3);
    record6.CheckState = this.m_checkState;
    record6.Display3DShading = this.Display3DShading;
    record7.IsFirstButton = this.IsFirstButton;
    record7.NextButton = this.m_nextButton;
    if (record4 != null && record4.Formula == null && this.LinkedCell != null)
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
    this.IsTextLocked = this.IsTextLocked;
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
        case TObjSubRecordType.ftRboData:
          this.m_nextButton = ((ftRboData) objSubRecord).NextButton;
          this.m_isFirstButton = ((ftRboData) objSubRecord).IsFirstButton;
          break;
        case TObjSubRecordType.ftCblsData:
          this.m_checkState = ((ftCblsData) objSubRecord).CheckState;
          this.m_display3DShading = ((ftCblsData) objSubRecord).Display3DShading;
          break;
        case TObjSubRecordType.ftCblsFmla:
          this.m_cellLinkRange = (((ftCblsFmla) objSubRecord).Formula[0] as IRangeGetter).GetRange((IWorkbook) this.ParentWorkbook, this.Worksheet as IWorksheet);
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
    OptionButtonShapeImpl optionButton = (OptionButtonShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    WorksheetBaseImpl worksheet = optionButton.Worksheet;
    WorkbookImpl parentWorkbook = worksheet.ParentWorkbook;
    if (this.m_cellLinkRange != null)
      optionButton.m_cellLinkRange = (this.m_cellLinkRange as ICombinedRange).Clone((object) worksheet, hashNewNames, parentWorkbook);
    if (addToCollections)
      (optionButton.Worksheet.OptionButtons as OptionButtonCollection).AddOptionButton((IOptionButtonShape) optionButton);
    return (IShape) optionButton;
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

  public RichTextString RichText
  {
    set => throw new NotSupportedException("Rich Text");
  }

  public ExcelCheckState CheckState
  {
    get => this.m_checkState;
    set
    {
      if (value == ExcelCheckState.Mixed)
        throw new NotSupportedException("Mixed state not Supported");
      ExcelCheckState checkState = this.m_checkState;
      this.m_checkState = value;
      if (checkState != value && this.CheckStateChanged != null && this.m_invokeEvent)
        this.CheckStateChanged((object) this, new ValueChangedEventArgs((object) checkState, (object) value, this.Name));
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
      IRange cellLinkRange = this.m_cellLinkRange;
      string addressGlobal1 = cellLinkRange?.AddressGlobal;
      string addressGlobal2 = value?.AddressGlobal;
      this.m_cellLinkRange = value;
      if (addressGlobal1 != addressGlobal2 && this.m_invokeEvent && this.LinkedCellValueChanged != null)
        this.LinkedCellValueChanged((object) this, new ValueChangedEventArgs((object) cellLinkRange, (object) value, this.Name));
      if (this.m_cellLinkRange == null)
        return;
      (this.m_cellLinkRange as INativePTG).GetNativePtg();
    }
  }

  public bool IsFirstButton
  {
    get => this.m_isFirstButton;
    set => this.m_isFirstButton = value;
  }

  public bool Display3DShading
  {
    get => this.m_display3DShading;
    set => this.m_display3DShading = value;
  }

  internal event ValueChangedEventHandler LinkedCellValueChanged;
}
