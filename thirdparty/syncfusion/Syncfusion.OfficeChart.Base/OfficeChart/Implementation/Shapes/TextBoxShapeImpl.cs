// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.TextBoxShapeImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class TextBoxShapeImpl : 
  TextBoxShapeBase,
  ITextBoxShapeEx,
  ITextBoxShape,
  ITextBox,
  IShape,
  IParentApplication
{
  private const int ShapeInstance = 202;
  private const int ShapeVersion = 2;
  internal const string EmbedString = "Forms.TextBox.1";
  private Rectangle m_2007Coordinates = new Rectangle(0, 1, 2076450, 1557338);
  private string m_id;
  private string m_type;
  private string m_textLink;

  public Rectangle Coordinates2007
  {
    get => this.m_2007Coordinates;
    set => this.m_2007Coordinates = value;
  }

  public string FieldId
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public string FieldType
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public string TextLink
  {
    get => this.m_textLink;
    set
    {
      this.m_textLink = value != null && value.Length > 0 && value[0] == '=' ? value : throw new ArgumentException("Refrence is not valid");
    }
  }

  public TextBoxShapeImpl(IApplication application, object parent, WorksheetImpl sheet)
    : base(application, parent)
  {
    this.ShapeType = OfficeShapeType.TextBox;
    this.Fill.ForeColor = ColorExtension.White;
    this.Line.ForeColor = ColorExtension.DarkGray;
    this.Line.BackColor = ColorExtension.DarkGray;
    this.m_sheet = sheet;
  }

  [CLSCompliant(false)]
  public TextBoxShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer shapeContainer,
    OfficeParseOptions options)
    : base(application, parent, shapeContainer, options)
  {
    this.ShapeType = OfficeShapeType.TextBox;
  }

  private void InitializeShape()
  {
    this.ShapeType = OfficeShapeType.TextBox;
    this.m_bUpdateLineFill = true;
  }

  protected override void OnPrepareForSerialization()
  {
    this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
    this.m_shape.Version = 2;
    this.m_shape.Instance = 202;
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
    ftCmo record3;
    if (this.Obj == null)
    {
      OBJRecord record4 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record3 = new ftCmo();
      record3.ObjectType = TObjType.otText;
      record3.Printable = true;
      record3.Locked = true;
      record3.AutoLine = true;
      ftEnd record5 = new ftEnd();
      record4.AddSubRecord((ObjSubRecord) record3);
      record4.AddSubRecord((ObjSubRecord) record5);
      record2.AddRecord((BiffRecordRaw) record4);
    }
    else
    {
      record3 = this.Obj.RecordsList[0] as ftCmo;
      record2.AddRecord((BiffRecordRaw) this.Obj);
    }
    record3.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
    record1.AddItem((MsoBase) this.m_shape);
    MsofbtOPT itemToAdd = this.SerializeOptions((MsoBase) record1);
    if (itemToAdd.Properties.Length > 0)
      record1.AddItem((MsoBase) itemToAdd);
    record1.AddItem((MsoBase) this.ClientAnchor);
    record1.AddItem((MsoBase) record2);
    if (this.Text.Length > 0)
      record1.AddItem((MsoBase) this.GetClientTextBoxRecord((MsoBase) record1));
    spgrContainer.AddItem((MsoBase) record1);
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT CreateDefaultOptions()
  {
    MsofbtOPT defaultOptions = base.CreateDefaultOptions();
    defaultOptions.Version = 3;
    defaultOptions.Instance = 2;
    return defaultOptions;
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    TextBoxShapeImpl textbox = (TextBoxShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollections);
    if (addToCollections)
      (textbox.Worksheet.TextBoxes as TextBoxCollection).AddTextBox((ITextBoxShape) textbox);
    return (IShape) textbox;
  }
}
