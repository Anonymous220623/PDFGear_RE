// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ShapeCollectionBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public abstract class ShapeCollectionBase : CollectionBaseEx<IShape>
{
  public const int DEF_ID_PER_GROUP = 1024 /*0x0400*/;
  public const int DEF_SHAPES_ROUND_VALUE = 1024 /*0x0400*/;
  private MsofbtSpContainer m_groupInfo;
  protected WorksheetBaseImpl m_sheet;
  private int m_iCollectionIndex;
  private int m_iLastId;
  private int m_iStartId;
  private System.Collections.Generic.List<MsofbtRegroupItems> m_arrRegroundItems = new System.Collections.Generic.List<MsofbtRegroupItems>();
  private Stream m_layoutStream;

  public ShapeCollectionBase(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeCollection();
  }

  [CLSCompliant(false)]
  public ShapeCollectionBase(
    IApplication application,
    object parent,
    MsofbtSpgrContainer container,
    ExcelParseOptions options)
    : this(application, parent)
  {
    this.Parse(container, options);
  }

  protected virtual void InitializeCollection() => this.SetParents();

  protected void SetParents()
  {
    this.m_sheet = this.FindParent(typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Can't find parent worksheet.");
  }

  public int ShapesCount
  {
    get
    {
      int count = this.Count;
      return count <= 0 ? 0 : count + 1;
    }
  }

  public int ShapesTotalCount
  {
    get
    {
      int num = 0;
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        ShapeImpl shapeImpl = (ShapeImpl) this[index];
        num += shapeImpl.ShapeCount;
      }
      return num <= 0 ? 0 : num + 1;
    }
  }

  public WorksheetBaseImpl WorksheetBase => this.m_sheet;

  public WorksheetImpl Worksheet => this.m_sheet as WorksheetImpl;

  public WorkbookImpl Workbook => this.m_sheet.ParentWorkbook;

  public new IShape this[int index] => this.List[index];

  public IShape this[string strShapeName]
  {
    get
    {
      switch (strShapeName)
      {
        case null:
          throw new ArgumentNullException(nameof (strShapeName));
        case "":
          throw new ArgumentException("Shape name cannot be null.");
        default:
          IList<IShape> list = this.List;
          int index = 0;
          for (int count = list.Count; index < count; ++index)
          {
            IShape shape = list[index];
            if (shape.Name == strShapeName)
              return shape;
          }
          return (IShape) null;
      }
    }
  }

  internal Stream ShapeLayoutStream
  {
    get => this.m_layoutStream;
    set => this.m_layoutStream = value;
  }

  public abstract TBIFFRecord RecordCode { get; }

  public abstract WorkbookShapeDataImpl ShapeData { get; }

  internal int CollectionIndex
  {
    get => this.m_iCollectionIndex;
    set => this.m_iCollectionIndex = value;
  }

  internal int LastId
  {
    get => this.m_iLastId;
    set => this.m_iLastId = value;
  }

  internal int StartId
  {
    get => this.m_iStartId;
    set => this.m_iStartId = value;
  }

  private void Parse(MsofbtSpgrContainer container, ExcelParseOptions options)
  {
    System.Collections.Generic.List<MsoBase> itemsList = container.ItemsList;
    this.ParseGroupDescription((MsofbtSpContainer) itemsList[0]);
    int index = 1;
    for (int count = itemsList.Count; index < count; ++index)
      this.AddShape(itemsList[index], options);
  }

  private void ParseGroupDescription(MsofbtSpContainer groupDescription)
  {
    this.m_groupInfo = (MsofbtSpContainer) groupDescription.Clone();
    if (!(this.m_groupInfo.Items[1] is MsofbtSp msofbtSp))
      return;
    this.m_iStartId = msofbtSp.ShapeId;
  }

  public void ParseMsoStructures(System.Collections.Generic.List<MsoBase> arrStructures, ExcelParseOptions options)
  {
    int index = 0;
    for (int count = arrStructures.Count; index < count; ++index)
    {
      MsoBase arrStructure = arrStructures[index];
      if (arrStructure.MsoRecordType != MsoRecords.msofbtDgContainer)
        throw new ArgumentOutOfRangeException("Unexcpected MsoDrawing record");
      this.ParseMsoDgContainer((MsofbtDgContainer) arrStructure, options);
    }
  }

  private void ParseMsoDgContainer(MsofbtDgContainer dgContainer, ExcelParseOptions options)
  {
    System.Collections.Generic.List<MsoBase> itemsList = dgContainer.ItemsList;
    int index = 0;
    for (int count = itemsList.Count; index < count; ++index)
    {
      MsoBase msoBase = itemsList[index];
      switch (msoBase.MsoRecordType)
      {
        case MsoRecords.msofbtSpgrContainer:
          this.Parse((MsofbtSpgrContainer) msoBase, options);
          break;
        case MsoRecords.msofbtDg:
          this.ParseMsoDg((MsofbtDg) msoBase);
          break;
        case MsoRecords.msofbtRegroupItems:
          this.m_arrRegroundItems.Add((MsofbtRegroupItems) msoBase);
          break;
      }
    }
  }

  private void ParseMsoDg(MsofbtDg dgRecord)
  {
    this.CollectionIndex = dgRecord.Instance;
    this.m_iLastId = dgRecord.LastId;
  }

  public IShape AddCopy(ShapeImpl sourceShape)
  {
    return this.AddCopy(sourceShape, (Dictionary<string, string>) null, (Dictionary<int, int>) null);
  }

  public IShape AddCopy(
    ShapeImpl sourceShape,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes)
  {
    IShape destShape = sourceShape.Clone((object) this, hashNewNames, dicFontIndexes, true);
    switch (destShape)
    {
      case BitmapShapeImpl _:
      case AutoShapeImpl _:
      case TextBoxShapeImpl _:
        if (destShape.Hyperlink != null)
        {
          this.Worksheet.InnerHyperLinks.Add(destShape.Hyperlink);
          break;
        }
        break;
    }
    string onAction = sourceShape.OnAction;
    if (sourceShape.ParentWorkbook != this.Workbook && onAction != string.Empty)
      destShape.OnAction = this.UpdateMacro(sourceShape, destShape as ShapeImpl, onAction);
    (destShape as ShapeImpl).ShapeId = 0;
    return destShape;
  }

  internal string UpdateMacro(ShapeImpl sourceShape, ShapeImpl destShape, string onAction)
  {
    if (!string.IsNullOrEmpty(onAction))
    {
      if (!onAction.StartsWith("["))
        onAction = "[0]!" + onAction;
      int result = -1;
      string empty = string.Empty;
      WorkbookImpl parentWorkbook1 = sourceShape.ParentWorkbook;
      WorkbookImpl parentWorkbook2 = destShape.ParentWorkbook;
      int num1 = onAction.IndexOf("[");
      if (num1 >= 0)
      {
        int num2;
        for (num2 = num1 + 1; onAction[num2] != ']'; ++num2)
          empty += (string) (object) onAction[num2];
        if (int.TryParse(empty, out result))
        {
          if (result > 0 && result <= parentWorkbook1.ExternWorkbooks.Count)
          {
            int num3 = 0;
            ExternWorkbookImpl externWorkbookImpl = (ExternWorkbookImpl) null;
            for (int index = 0; index < parentWorkbook1.ExternWorkbooks.Count; ++index)
            {
              externWorkbookImpl = parentWorkbook1.ExternWorkbooks[index];
              if (!externWorkbookImpl.IsInternalReference || !string.IsNullOrEmpty(externWorkbookImpl.URL))
                ++num3;
              if (num3 == result)
              {
                result = index;
                break;
              }
            }
            if (parentWorkbook2.FullFileName != null && (parentWorkbook2.FullFileName == externWorkbookImpl.URL || parentWorkbook2.FullFileName == parentWorkbook1.GetFilePath(parentWorkbook2.FullFileName) + externWorkbookImpl.URL) || parentWorkbook1.GetFilePath(externWorkbookImpl.URL) == "'" + parentWorkbook1.GetFilePath(parentWorkbook2.FullFileName) && parentWorkbook1.GetFileName(externWorkbookImpl.URL).Contains(parentWorkbook1.GetFileName(parentWorkbook2.FullFileName)))
              result = 0;
          }
          else if (result == 0)
          {
            int bookIndex = parentWorkbook1.ExternWorkbooks.GetFirstInternalIndex();
            if (bookIndex < 0)
              bookIndex = parentWorkbook1.ExternWorkbooks.InsertSelfSupbook();
            int index1 = CellRecordCollection.GetExternBook(parentWorkbook1, parentWorkbook2, bookIndex).Index;
            int num4 = 0;
            for (int index2 = index1 - 1; index2 >= 0; --index2)
            {
              ExternWorkbookImpl externWorkbook = parentWorkbook2.ExternWorkbooks[index2];
              if (externWorkbook.IsInternalReference || string.IsNullOrEmpty(externWorkbook.URL))
                ++num4;
            }
            result = index1 - num4 + 1;
          }
        }
        string str = onAction.Substring(num2);
        onAction = onAction.Substring(0, num2).Replace(empty, result.ToString()) + str;
      }
    }
    return onAction;
  }

  public IShape AddCopy(IShape sourceShape) => this.AddCopy((ShapeImpl) sourceShape);

  public IShape AddCopy(
    IShape sourceShape,
    Dictionary<string, string> hashNewNames,
    System.Collections.Generic.List<int> arrFontIndexes)
  {
    return this.AddCopy(sourceShape, hashNewNames, arrFontIndexes);
  }

  public ShapeImpl AddShape(ShapeImpl newShape)
  {
    switch (newShape)
    {
      case null:
        throw new ArgumentNullException(nameof (newShape));
      case BitmapShapeImpl _:
      case AutoShapeImpl _:
        if (newShape.Hyperlink != null)
        {
          this.Worksheet.InnerHyperLinks.Add(newShape.Hyperlink);
          break;
        }
        break;
    }
    this.Add((IShape) newShape);
    return newShape;
  }

  [CLSCompliant(false)]
  protected ShapeImpl AddShape(MsoBase shape, ExcelParseOptions options)
  {
    ShapeImpl newShape;
    switch (shape)
    {
      case MsofbtSpContainer _:
        newShape = this.AddShape(shape as MsofbtSpContainer, options);
        break;
      case MsofbtSpgrContainer _:
        newShape = this.AddGroupShape(shape as MsofbtSpgrContainer, options);
        break;
      default:
        newShape = new ShapeImpl(this.Application, (object) this, shape, options);
        this.AddShape(newShape);
        break;
    }
    if (newShape.ShapeRecord != null && newShape.ShapeRecord.IsOleShape && string.IsNullOrEmpty(newShape.Name))
      newShape.Name = CollectionBaseEx<IShape>.GenerateDefaultName((ICollection<IShape>) this, "Object ");
    return newShape;
  }

  protected ShapeImpl AddGroupShape(MsofbtSpgrContainer shapes, ExcelParseOptions options)
  {
    return this.AddShape(this.CreateGroupShape(shapes, options));
  }

  protected ShapeImpl CreateGroupShape(MsofbtSpgrContainer shapes, ExcelParseOptions options)
  {
    ShapeImpl groupShape = new ShapeImpl(this.Application, (object) this, (MsoBase) shapes, options);
    foreach (MsoBase items in shapes.ItemsList)
    {
      switch (items)
      {
        case MsofbtSpContainer _:
          groupShape.ChildShapes.Add(this.AddChildShapes(items as MsofbtSpContainer, options));
          continue;
        case MsofbtSpgrContainer _:
          groupShape.ChildShapes.Add(this.CreateGroupShape(items as MsofbtSpgrContainer, options));
          continue;
        default:
          groupShape.ChildShapes.Add(new ShapeImpl(this.Application, (object) this, items, options));
          continue;
      }
    }
    return groupShape;
  }

  [CLSCompliant(false)]
  protected ShapeImpl AddChildShapes(MsofbtSpContainer shapeContainer, ExcelParseOptions options)
  {
    ShapeImpl shapeImpl = (ShapeImpl) null;
    System.Collections.Generic.List<MsoBase> itemsList = shapeContainer.ItemsList;
    int index1 = 0;
    for (int count1 = itemsList.Count; index1 < count1; ++index1)
    {
      if (itemsList[index1] is MsofbtClientData)
      {
        System.Collections.Generic.List<ObjSubRecord> recordsList = (itemsList[index1] as MsofbtClientData).ObjectRecord.RecordsList;
        int index2 = 0;
        for (int count2 = recordsList.Count; index2 < count2; ++index2)
        {
          if (recordsList[index2].Type == TObjSubRecordType.ftCmo)
          {
            ftCmo ftCmo = recordsList[index2] as ftCmo;
            shapeImpl = this.CreateShape(ftCmo.ObjectType, shapeContainer, options, recordsList, (int) ftCmo.ID);
            break;
          }
        }
        break;
      }
    }
    if (shapeImpl == null)
      shapeImpl = new ShapeImpl(this.Application, (object) this, shapeContainer, ExcelParseOptions.Default);
    return shapeImpl;
  }

  [CLSCompliant(false)]
  protected virtual ShapeImpl AddShape(MsofbtSpContainer shapeContainer, ExcelParseOptions options)
  {
    ShapeImpl newShape = (ShapeImpl) null;
    System.Collections.Generic.List<MsoBase> itemsList = shapeContainer.ItemsList;
    int index1 = 0;
    for (int count1 = itemsList.Count; index1 < count1; ++index1)
    {
      if (itemsList[index1] is MsofbtClientData)
      {
        System.Collections.Generic.List<ObjSubRecord> recordsList = (itemsList[index1] as MsofbtClientData).ObjectRecord.RecordsList;
        int num = 0;
        for (int count2 = recordsList.Count; num < count2; ++num)
        {
          if (recordsList[num].Type == TObjSubRecordType.ftCmo)
          {
            newShape = this.CreateShape((recordsList[num] as ftCmo).ObjectType, shapeContainer, options, recordsList, num);
            break;
          }
        }
        break;
      }
    }
    bool flag = true;
    int index2 = 0;
    for (int count = itemsList.Count; index2 < count; ++index2)
    {
      if (itemsList[index2] is MsofbtOPT)
      {
        MsofbtOPT msofbtOpt = itemsList[index2] as MsofbtOPT;
        for (int index3 = 0; index3 < msofbtOpt.PropertyList.Count; ++index3)
        {
          MsofbtOPT.FOPTE property = msofbtOpt.PropertyList[index3];
          if (property.Id == MsoOptions.CommentShowAlways)
          {
            flag = ((int) property.MainData[2] & 2) == 0;
            break;
          }
        }
        break;
      }
    }
    if (newShape == null)
      newShape = new ShapeImpl(this.Application, (object) this, shapeContainer, ExcelParseOptions.Default);
    newShape.IsShapeVisible = flag;
    return this.AddShape(newShape);
  }

  [CLSCompliant(false)]
  protected virtual ShapeImpl CreateShape(
    TObjType objType,
    MsofbtSpContainer shapeContainer,
    ExcelParseOptions options,
    System.Collections.Generic.List<ObjSubRecord> subRecords,
    int cmoIndex)
  {
    throw new NotImplementedException("This method must be overriden in child classes");
  }

  public void Remove(IShape shape)
  {
    int index = 0;
    for (int count = this.Count; index < count && (this[index].ShapeType != ExcelShapeType.Group || this.Workbook.Loading || !(this[index] as IGroupShape as GroupShapeImpl).RemoveGroupShapeItem(shape)); ++index)
    {
      if (this[index] == shape)
      {
        switch (shape)
        {
          case BitmapShapeImpl _:
          case AutoShapeImpl _:
          case TextBoxShapeImpl _:
            if (shape.Hyperlink != null)
            {
              (this.Worksheet.HyperLinks as HyperLinksCollection).Remove(shape.Hyperlink);
              break;
            }
            break;
        }
        this.RemoveAt(index);
        break;
      }
    }
  }

  public override object Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ConstructorInfo constructor = this.GetType().GetConstructor(new Type[2]
    {
      typeof (IApplication),
      typeof (object)
    });
    if (constructor == (ConstructorInfo) null)
      throw new ApplicationException("Cannot find required constructor.");
    ShapeCollectionBase parent1 = constructor.Invoke(new object[2]
    {
      (object) this.Application,
      parent
    }) as ShapeCollectionBase;
    parent1.m_iCollectionIndex = this.m_iCollectionIndex;
    parent1.m_iLastId = this.m_iLastId;
    parent1.m_iStartId = this.m_iStartId;
    parent1.RegisterInWorksheet();
    System.Collections.Generic.List<IShape> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IShape shape1 = innerList[index];
      IShape shape2;
      switch (shape1)
      {
        case ShapeImpl _:
          shape2 = (IShape) ((ShapeImpl) shape1).Clone((object) parent1);
          break;
        case ICloneable _:
          shape2 = (IShape) ((ICloneable) shape1).Clone();
          break;
      }
    }
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_groupInfo = (MsofbtSpContainer) CloneUtils.CloneCloneable((ICloneable) this.m_groupInfo);
    return (object) parent1;
  }

  protected virtual void RegisterInWorksheet() => this.WorksheetBase.InnerShapesBase = this;

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (this.ShapesCount == 0)
      return;
    System.Collections.Generic.List<int> chartExIndexesList = this.GetChartExIndexesList();
    if (chartExIndexesList.Count == this.Count)
      return;
    MsofbtDgContainer record1 = (MsofbtDgContainer) MsoFactory.GetRecord(MsoRecords.msofbtDgContainer);
    MsofbtDg record2 = (MsofbtDg) MsoFactory.GetRecord(MsoRecords.msofbtDg);
    MsofbtSpgrContainer record3 = (MsofbtSpgrContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpgrContainer);
    MsofbtSpContainer record4 = (MsofbtSpContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpContainer);
    MsofbtSpgr record5 = (MsofbtSpgr) MsoFactory.GetRecord(MsoRecords.msofbtSpgr);
    MsofbtSp record6 = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
    record6.IsGroup = true;
    record6.IsPatriarch = true;
    record6.ShapeId = this.m_iStartId;
    record1.AddItem((MsoBase) record2);
    foreach (MsofbtRegroupItems arrRegroundItem in this.m_arrRegroundItems)
      record1.AddItem((MsoBase) arrRegroundItem);
    record1.AddItem((MsoBase) record3);
    record3.AddItem((MsoBase) record4);
    record4.AddItem((MsoBase) record5);
    record4.AddItem((MsoBase) record6);
    System.Collections.Generic.List<IShape> innerList = this.InnerList;
    int index1 = 0;
    for (int count = innerList.Count; index1 < count; ++index1)
    {
      if (!chartExIndexesList.Contains(index1))
      {
        ShapeImpl shapeImpl = innerList[index1] as ShapeImpl;
        shapeImpl.PrepareForSerialization();
        shapeImpl.Serialize(record3);
      }
    }
    System.Collections.Generic.List<int> arrBreaks = new System.Collections.Generic.List<int>();
    System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> arrRecords = new System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>>();
    record2.ShapesNumber = (uint) (this.ShapesTotalCount - chartExIndexesList.Count);
    record2.LastId = this.m_iLastId;
    if (this.m_iCollectionIndex > 0)
      record2.Instance = this.m_iCollectionIndex;
    MemoryStream buffer1 = new MemoryStream();
    buffer1.Position = 8L;
    this.CreateData((Stream) buffer1, record1, arrBreaks, arrRecords);
    if (arrBreaks.Count != arrRecords.Count)
      throw new ArgumentException("Breaks and records do not fit each other.");
    int iCurPos = 0;
    TBIFFRecord recordCode = this.RecordCode;
    if (arrBreaks.Count > 0)
    {
      int index2 = 0;
      for (int count = arrBreaks.Count; index2 < count; ++index2)
      {
        int num1 = arrBreaks[index2];
        System.Collections.Generic.List<BiffRecordRaw> biffRecordRawList = arrRecords[index2];
        int num2 = num1 - iCurPos;
        if (num1 > 8224)
        {
          while (num2 > 0)
          {
            int num3 = Math.Min(num2, 8224);
            BiffRecordRaw record7 = BiffRecordFactory.GetRecord(TBIFFRecord.Continue);
            (record7 as ContinueRecord).SetLength(num3);
            this.WriteData(record7, buffer1, iCurPos, num3);
            num2 -= num3;
            iCurPos += num3;
            records.Add((IBiffStorage) record7);
          }
        }
        else
        {
          BiffRecordRaw record8 = BiffRecordFactory.GetRecord(recordCode);
          this.WriteData(record8, buffer1, iCurPos, num2);
          records.Add((IBiffStorage) record8);
        }
        iCurPos = num1;
        records.AddList((IList) biffRecordRawList);
      }
    }
    else
    {
      BiffRecordRaw record9 = BiffRecordFactory.GetRecord(recordCode);
      int length = (int) (buffer1.Length - 8L);
      byte[] buffer2 = new byte[length];
      buffer1.Position = 8L;
      buffer1.Read(buffer2, 0, length);
      record9.Data = buffer2;
      ((ILengthSetter) record9).SetLength(length);
      records.Add((IBiffStorage) record9);
    }
  }

  private System.Collections.Generic.List<int> GetChartExIndexesList()
  {
    System.Collections.Generic.List<IShape> innerList = this.InnerList;
    System.Collections.Generic.List<int> chartExIndexesList = new System.Collections.Generic.List<int>(innerList.Count);
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      if (innerList[index] is ChartShapeImpl chartShapeImpl && ChartImpl.IsChartExSerieType(chartShapeImpl.ChartType))
        chartExIndexesList.Add(index);
    }
    return chartExIndexesList;
  }

  private void WriteData(BiffRecordRaw record, MemoryStream buffer, int iCurPos, int size)
  {
    byte[] buffer1 = new byte[size];
    buffer.Position = (long) (iCurPos + 8);
    buffer.Read(buffer1, 0, size);
    record.Data = buffer1;
    ((ILengthSetter) record).SetLength(size);
  }

  [CLSCompliant(false)]
  protected virtual void CreateData(
    Stream stream,
    MsofbtDgContainer dgContainer,
    System.Collections.Generic.List<int> arrBreaks,
    System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> arrRecords)
  {
    dgContainer.FillArray(stream, 8, arrBreaks, arrRecords);
  }
}
