// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.FormControlShapeImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class FormControlShapeImpl : ShapeImpl, IShape, IParentApplication
{
  private const int DEF_LOCKAGAINSGROUPDING_VALUE = 17039620;
  internal const int DEF_SIZETEXT_VALUE = 524296 /*0x080008*/;
  private const int DEF_NOLINE_VALUE = 524288 /*0x080000*/;
  private const int DEF_959_VALUE = 131072 /*0x020000*/;
  private static readonly byte[] DEF_CMO_DATA = new byte[12]
  {
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 108,
    (byte) 25,
    (byte) 42,
    (byte) 1,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private ftLbsData m_lbsData = new ftLbsData();

  public FormControlShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.Initialize();
  }

  [CLSCompliant(false)]
  public FormControlShapeImpl(
    IApplication application,
    object parent,
    MsoBase[] records,
    int index)
    : base(application, parent, records, index)
  {
    this.Initialize();
  }

  [CLSCompliant(false)]
  public FormControlShapeImpl(IApplication application, object parent, MsofbtSpContainer container)
    : base(application, parent, container)
  {
    this.Initialize();
  }

  [CLSCompliant(false)]
  public FormControlShapeImpl(IApplication application, object parent, MsoBase shapeRecord)
    : base(application, parent, shapeRecord, ExcelParseOptions.Default)
  {
    this.Initialize();
  }

  private void Initialize()
  {
    this.ShapeType = ExcelShapeType.FormControl;
    this.ClientAnchor.IsSizeWithCell = true;
    this.m_lbsData.ComboType = ExcelComboType.AutoFilter;
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollection)
  {
    FormControlShapeImpl controlShapeImpl = (FormControlShapeImpl) base.Clone(parent, hashNewNames, dicFontIndexes, addToCollection);
    controlShapeImpl.m_lbsData = (ftLbsData) this.m_lbsData.Clone();
    return (IShape) controlShapeImpl;
  }

  protected override void OnPrepareForSerialization()
  {
    if (this.m_shape != null)
      return;
    this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
    this.m_shape.Instance = 201;
    this.m_shape.IsHaveAnchor = true;
    this.m_shape.IsHaveSpt = true;
  }

  [CLSCompliant(false)]
  protected override void SerializeShape(MsofbtSpgrContainer spgrContainer)
  {
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer((MsoBase) spgrContainer);
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor((MsoBase) msofbtSpContainer);
    MsofbtClientData itemToAdd1 = new MsofbtClientData((MsoBase) msofbtSpContainer);
    OBJRecord record1 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
    ftCmo record2 = new ftCmo();
    record2.ObjectType = TObjType.otComboBox;
    record2.Printable = false;
    record2.Locked = true;
    record2.AutoFill = true;
    record2.AutoLine = false;
    record2.ChangeColor = true;
    record2.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
    FormControlShapeImpl.DEF_CMO_DATA.CopyTo((Array) record2.Reserved, 0);
    ftSbs record3 = new ftSbs();
    ftEnd record4 = new ftEnd();
    record1.AddSubRecord((ObjSubRecord) record2);
    record1.AddSubRecord((ObjSubRecord) record3);
    record1.AddSubRecord((ObjSubRecord) this.m_lbsData);
    record1.AddSubRecord((ObjSubRecord) record4);
    itemToAdd1.AddRecord((BiffRecordRaw) record1);
    MsofbtOPT itemToAdd2 = this.SerializeOptions((MsoBase) this.m_shape);
    msofbtSpContainer.AddItem((MsoBase) this.m_shape);
    msofbtSpContainer.AddItem((MsoBase) itemToAdd2);
    this.ClientAnchor.Options = (ushort) 1;
    msofbtSpContainer.AddItem((MsoBase) this.ClientAnchor);
    msofbtSpContainer.AddItem((MsoBase) itemToAdd1);
    spgrContainer.AddItem((MsoBase) msofbtSpContainer);
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT SerializeOptions(MsoBase parent)
  {
    if (!this.m_bUpdateLineFill && this.m_options != null)
      return this.m_options;
    MsofbtOPT options = base.SerializeOptions(parent);
    this.SerializeOption(options, MsoOptions.LockAgainstGrouping, 17039620);
    this.SerializeOption(options, MsoOptions.SizeTextToFitShape, 524296 /*0x080008*/);
    this.SerializeOption(options, MsoOptions.NoLineDrawDash, 524288 /*0x080000*/);
    this.SerializeOption(options, MsoOptions.CommentShowAlways, 131072 /*0x020000*/);
    options.Version = 3;
    options.Instance = 2;
    return options;
  }

  [CLSCompliant(false)]
  protected override void ParseClientData(MsofbtClientData clientData, ExcelParseOptions options)
  {
    if (clientData == null)
      throw new ArgumentNullException(nameof (clientData));
    base.ParseClientData(clientData, options);
    List<ObjSubRecord> recordsList = this.Obj.RecordsList;
    int index = 0;
    for (int count = recordsList.Count; index < count; ++index)
    {
      ObjSubRecord objSubRecord = recordsList[index];
      if (objSubRecord.Type == TObjSubRecordType.ftLbsData)
        this.m_lbsData = (ftLbsData) objSubRecord;
    }
  }

  public bool IsArrowSelectedColor
  {
    get => this.m_lbsData.IsSelectedColor;
    set => this.m_lbsData.IsSelectedColor = value;
  }

  public IHyperLink HyperLink => throw new NotSupportedException(nameof (HyperLink));
}
