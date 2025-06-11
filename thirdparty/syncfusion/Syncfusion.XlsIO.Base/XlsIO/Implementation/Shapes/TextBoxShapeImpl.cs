// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.TextBoxShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class TextBoxShapeImpl : 
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
  private bool m_isFldText;
  private Rectangle m_2007Coordinates = new Rectangle(0, 1, 2076450, 1557338);
  private string m_id;
  private string m_type;
  private string m_textLink;
  internal bool IsAutoSize;
  private bool m_locksText = true;
  private bool m_noChangeAspect;
  private TextBodyPropertiesHolder textBodyPropertiesHolder;
  private bool m_bFlipVertical;
  private bool m_bFlipHorizontal;
  private Stream m_fldElementStream;
  private bool m_isLineProperties;
  private bool m_isCreated;
  private bool m_isFill;
  private bool m_isNoFill;
  private bool m_isGroupFill;

  internal bool IsFldText
  {
    get => this.m_isFldText;
    set => this.m_isFldText = value;
  }

  internal bool IsCreated
  {
    get => this.m_isCreated;
    set => this.m_isCreated = value;
  }

  internal bool IsLineProperties
  {
    get => this.m_isLineProperties;
    set => this.m_isLineProperties = value;
  }

  internal bool IsFill
  {
    get => this.m_isFill;
    set => this.m_isFill = value;
  }

  internal bool IsNoFill
  {
    get => this.m_isNoFill;
    set => this.m_isNoFill = value;
  }

  internal new bool IsGroupFill
  {
    get => this.m_isGroupFill;
    set => this.m_isGroupFill = value;
  }

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
    get
    {
      if (this.m_textLink != string.Empty && this.m_textLink != null && this.m_textLink.StartsWith("="))
      {
        int num = 0;
        string textLink = this.m_textLink;
        for (int index = 0; index < textLink.Length && textLink[index] == '='; ++index)
          ++num;
        this.m_textLink = this.m_textLink.Substring(num - 1, this.m_textLink.Length - num + 1);
      }
      return this.m_textLink;
    }
    set
    {
      this.m_textLink = value != null && value.Length > 0 && value[0] == '=' ? value : throw new ArgumentException("Refrence is not valid");
    }
  }

  internal bool LocksText
  {
    get => this.m_locksText;
    set => this.m_locksText = value;
  }

  internal bool NoChangeAspect
  {
    get => this.m_noChangeAspect;
    set => this.m_noChangeAspect = value;
  }

  public new string Text
  {
    get => base.Text;
    set
    {
      if (this.IsAutoSize && value.Length > base.Text.Length)
        this.IsAutoSize = false;
      base.Text = value;
    }
  }

  public new IHyperLink Hyperlink
  {
    get => base.Hyperlink;
    internal set => base.Hyperlink = value;
  }

  internal TextBodyPropertiesHolder TextBodyPropertiesHolder
  {
    get
    {
      if (this.textBodyPropertiesHolder == null)
        this.textBodyPropertiesHolder = new TextBodyPropertiesHolder();
      return this.textBodyPropertiesHolder;
    }
  }

  internal bool FlipVertical
  {
    get => this.m_bFlipVertical;
    set => this.m_bFlipVertical = value;
  }

  internal bool FlipHorizontal
  {
    get => this.m_bFlipHorizontal;
    set => this.m_bFlipHorizontal = value;
  }

  internal Stream FldElementStream
  {
    get => this.m_fldElementStream;
    set => this.m_fldElementStream = value;
  }

  public TextBoxShapeImpl(IApplication application, object parent, WorksheetImpl sheet)
    : base(application, parent)
  {
    this.ShapeType = ExcelShapeType.TextBox;
    this.Fill.ForeColor = ColorExtension.White;
    this.Line.ForeColor = Color.FromArgb((int) byte.MaxValue, 188, 188, 188);
    this.Line.BackColor = ColorExtension.DarkGray;
    this.m_sheet = sheet;
  }

  [CLSCompliant(false)]
  public TextBoxShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options)
    : base(application, parent, shapeContainer, options)
  {
    this.ShapeType = ExcelShapeType.TextBox;
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
      record3.Printable = this.PrintWithSheet;
      record3.Locked = this.LockWithSheet;
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
    if (this.UnKnown != null)
      record1.AddItem((MsoBase) this.UnKnown);
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
    textbox.m_isCreated = this.m_isCreated;
    if (this.textBodyPropertiesHolder != null)
      textbox.textBodyPropertiesHolder = this.textBodyPropertiesHolder.Clone();
    if (addToCollections)
      (textbox.Worksheet.TextBoxes as TextBoxCollection).AddTextBox((ITextBoxShape) textbox);
    return (IShape) textbox;
  }
}
