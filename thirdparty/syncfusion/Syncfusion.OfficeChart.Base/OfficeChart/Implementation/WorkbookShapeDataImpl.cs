// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.WorkbookShapeDataImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class WorkbookShapeDataImpl : CommonObject, ICloneParent
{
  private static readonly MsoBlipType[] METAFILEBLIPS = new MsoBlipType[3]
  {
    MsoBlipType.msoblipEMF,
    MsoBlipType.msoblipWMF,
    MsoBlipType.msoblipPICT
  };
  private List<MsofbtBSE> m_arrPictures = new List<MsofbtBSE>();
  private List<MsoBase> m_arrDGRecords = new List<MsoBase>();
  private WorkbookImpl m_book;
  private Dictionary<ArrayWrapper, MsofbtBSE> m_dicImageIdToImage = new Dictionary<ArrayWrapper, MsofbtBSE>();
  private WorkbookImpl.ShapesGetterMethod m_shapeGetter;
  private int m_iLastCollectionId;
  private static readonly Dictionary<MsoBlipType, WorkbookShapeDataImpl.BlipParams> s_hashBlipTypeToParams = new Dictionary<MsoBlipType, WorkbookShapeDataImpl.BlipParams>();
  private MsofbtDgg m_preservedDgg;
  private string[] m_indexedpixel_notsupport = new string[6]
  {
    "Format1bppIndexed",
    "Format4bppIndexed",
    "Format8bppIndexed",
    "b96b3cac-0728-11d3-9d7b-0000f81ef32e",
    "b96b3cad-0728-11d3-9d7b-0000f81ef32e",
    "b96b3caa-0728-11d3-9d7b-0000f81ef32e"
  };

  static WorkbookShapeDataImpl()
  {
    WorkbookShapeDataImpl.s_hashBlipTypeToParams.Add(MsoBlipType.msoblipEMF, new WorkbookShapeDataImpl.BlipParams(980, (byte) 4, (byte) 2, 61466));
    WorkbookShapeDataImpl.s_hashBlipTypeToParams.Add(MsoBlipType.msoblipWMF, new WorkbookShapeDataImpl.BlipParams(534, (byte) 4, (byte) 3, 61467));
    WorkbookShapeDataImpl.s_hashBlipTypeToParams.Add(MsoBlipType.msoblipPNG, new WorkbookShapeDataImpl.BlipParams(1760, (byte) 6, (byte) 6, 61470));
    WorkbookShapeDataImpl.s_hashBlipTypeToParams.Add(MsoBlipType.msoblipJPEG, new WorkbookShapeDataImpl.BlipParams(1130, (byte) 5, (byte) 5, 61469));
  }

  public WorkbookShapeDataImpl(
    IApplication application,
    object parent,
    WorkbookImpl.ShapesGetterMethod shapeGetter)
    : base(application, parent)
  {
    this.m_shapeGetter = shapeGetter != null ? shapeGetter : throw new ArgumentNullException(nameof (shapeGetter));
    this.SetParents();
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("parent", "Can't find parent workbook");
  }

  [CLSCompliant(false)]
  public void ParseDrawGroup(MSODrawingGroupRecord drawGroup)
  {
    if (drawGroup == null)
      throw new ArgumentNullException(nameof (drawGroup));
    List<MsoBase> itemsList = ((MsoContainerBase) drawGroup.StructuresList[0]).ItemsList;
    int index = 0;
    for (int count = itemsList.Count; index < count; ++index)
    {
      MsoBase bStore = itemsList[index];
      switch (bStore.MsoRecordType)
      {
        case MsoRecords.msofbtBstoreContainer:
          this.ParsePictures((MsofbtBstoreContainer) bStore);
          continue;
        case MsoRecords.msofbtDgg:
          this.m_preservedDgg = (MsofbtDgg) bStore;
          continue;
        case MsoRecords.msofbtOPT:
          continue;
        default:
          this.m_arrDGRecords.Add(bStore);
          continue;
      }
    }
  }

  private void ParsePictures(MsofbtBstoreContainer bStore)
  {
    List<MsoBase> msoBaseList = bStore != null ? bStore.ItemsList : throw new ArgumentNullException(nameof (bStore));
    int index = 0;
    for (int count = msoBaseList.Count; index < count; ++index)
      this.m_arrPictures.Add(msoBaseList[index] as MsofbtBSE);
  }

  protected override void OnDispose()
  {
    base.OnDispose();
    this.m_arrDGRecords.Clear();
    this.m_arrPictures.Clear();
    this.m_arrDGRecords = (List<MsoBase>) null;
    this.m_arrPictures = (List<MsofbtBSE>) null;
    this.m_dicImageIdToImage.Clear();
    this.m_dicImageIdToImage = (Dictionary<ArrayWrapper, MsofbtBSE>) null;
    if (this.m_preservedDgg != null)
      this.m_preservedDgg.Dispose();
    this.m_book = (WorkbookImpl) null;
    this.m_bIsDisposed = true;
  }

  [CLSCompliant(false)]
  public void SerializeMsoDrawingGroup(
    OffsetArrayList records,
    TBIFFRecord recordCode,
    IdReserver shapeIds)
  {
    bool needMsoDrawingGroup = this.NeedMsoDrawingGroup;
    if (!needMsoDrawingGroup && this.m_preservedDgg == null)
      return;
    MSODrawingGroupRecord record = (MSODrawingGroupRecord) BiffRecordFactory.GetRecord(recordCode);
    MsofbtDggContainer msofbtDggContainer = new MsofbtDggContainer((MsoBase) null);
    MsofbtDgg msofbtDgg = new MsofbtDgg((MsoBase) msofbtDggContainer);
    msofbtDggContainer.AddItem((MsoBase) msofbtDgg);
    this.FillMsoDgg(msofbtDgg, this.m_shapeGetter, shapeIds);
    int count = this.m_arrPictures != null ? this.m_arrPictures.Count : 0;
    if (count > 0)
    {
      MsofbtBstoreContainer itemToAdd = new MsofbtBstoreContainer((MsoBase) msofbtDggContainer);
      msofbtDggContainer.AddItem((MsoBase) itemToAdd);
      for (int index = 0; index < count; ++index)
      {
        MsofbtBSE arrPicture = this.m_arrPictures[index];
        itemToAdd.AddItem((MsoBase) arrPicture);
      }
    }
    if (needMsoDrawingGroup)
      this.SerializeDrawingGroupOptions(msofbtDggContainer);
    msofbtDggContainer.AddItems((ICollection<MsoBase>) this.m_arrDGRecords);
    record.AddStructure((MsoBase) msofbtDggContainer);
    records.Add((IBiffStorage) record);
  }

  private void SerializeDrawingGroupOptions(MsofbtDggContainer dggContainer)
  {
    MsofbtOPT msofbtOpt = new MsofbtOPT((MsoBase) dggContainer);
    this.SerializeDefaultOptions(msofbtOpt);
    dggContainer.AddItem((MsoBase) msofbtOpt);
  }

  private void SerializeDefaultOptions(MsofbtOPT options)
  {
    options.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.SizeTextToFitShape,
      IsComplex = false,
      IsValid = false,
      UInt32Value = 524296U /*0x080008*/
    });
    options.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.ForeColor,
      IsComplex = false,
      IsValid = false,
      UInt32Value = 134217793U /*0x08000041*/
    });
    options.AddOptionsOrReplace(new MsofbtOPT.FOPTE()
    {
      Id = MsoOptions.LineColor,
      IsComplex = false,
      IsValid = false,
      UInt32Value = 134217792U /*0x08000040*/
    });
  }

  [CLSCompliant(false)]
  protected void FillMsoDgg(
    MsofbtDgg dgg,
    WorkbookImpl.ShapesGetterMethod shapeGetter,
    IdReserver shapeIds)
  {
    if (dgg == null)
      throw new ArgumentNullException(nameof (dgg));
    if (this.m_preservedDgg != null)
    {
      this.CopyData(this.m_preservedDgg, dgg);
    }
    else
    {
      uint num1 = 0;
      uint num2 = 0;
      uint num3 = 1024 /*0x0400*/;
      uint uiGroupId = 0;
      WorkbookObjectsCollection objects = this.m_book.Objects;
      int index = 0;
      for (int count = objects.Count; index < count; ++index)
      {
        WorksheetBaseImpl sheet = objects[index] as WorksheetBaseImpl;
        ShapeCollectionBase shapeCollectionBase = shapeGetter((ITabSheet) sheet);
        WorksheetImpl worksheet = shapeCollectionBase.Worksheet;
        if (shapeCollectionBase.ShapesCount != 0)
        {
          ++uiGroupId;
          ++num2;
          num1 += (uint) shapeCollectionBase.ShapesCount;
          uint shapesCount = (uint) shapeCollectionBase.ShapesCount;
          if (shapeIds == null)
            dgg.AddCluster(uiGroupId, shapesCount);
          uint num4 = num3 % 1024U /*0x0400*/;
          if (num4 != 0U)
            num3 = (uint) ((int) num3 - (int) num4 + 1024 /*0x0400*/);
          num3 += shapesCount;
        }
      }
      if (shapeIds != null)
      {
        int maximumId = shapeIds.MaximumId;
        num3 = (uint) maximumId;
        for (int id = 1024 /*0x0400*/; id < maximumId; id += 1024 /*0x0400*/)
        {
          int num5 = shapeIds.ReservedBy(id);
          int additionalShapesNumber = shapeIds.GetAdditionalShapesNumber(num5);
          dgg.AddCluster((uint) num5, (uint) additionalShapesNumber);
        }
      }
      dgg.TotalShapes = num1;
      dgg.TotalDrawings = num2;
      dgg.IdMax = num3;
    }
  }

  private void CopyData(MsofbtDgg source, MsofbtDgg destination)
  {
    foreach (MsofbtDgg.ClusterID clusterId in source.ClusterIDs)
      destination.AddCluster(clusterId.GroupId, clusterId.Number);
    destination.IdMax = source.IdMax;
    destination.TotalDrawings = source.TotalDrawings;
    destination.TotalShapes = source.TotalShapes;
  }

  public int AddPicture(Image image, ExcelImageFormat imageFormat, string strPictureName)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    int num1 = Array.IndexOf<string>(this.m_indexedpixel_notsupport, image.PixelFormat.ToString());
    int num2 = Array.IndexOf<string>(this.m_indexedpixel_notsupport, image.RawFormat.Guid.ToString());
    if (num1 == -1 && num2 == -1)
    {
      MemoryStream memoryStream = new MemoryStream();
      if (image.RawFormat.Guid.ToString() == ImageFormat.Jpeg.Guid.ToString() && Enum.GetName(typeof (PixelFormat), (object) image.PixelFormat) == null || image.RawFormat.Guid.ToString() == ImageFormat.Emf.Guid.ToString() || image.RawFormat.Guid.ToString() == ImageFormat.MemoryBmp.Guid.ToString())
        image.Save((Stream) memoryStream, ImageFormat.Bmp);
      else
        image.Save((Stream) memoryStream, image.RawFormat);
      image = Image.FromStream((Stream) memoryStream);
      Graphics graphics = Graphics.FromImage(image);
      GraphicsUnit pageUnit = GraphicsUnit.Pixel;
      RectangleF bounds = image.GetBounds(ref pageUnit);
      graphics.DrawImage(image, bounds);
      graphics.Dispose();
    }
    MsofbtBSE msofbtBse1 = new MsofbtBSE((MsoBase) null);
    msofbtBse1.BlipName = strPictureName;
    msofbtBse1.BlipType = WorkbookShapeDataImpl.ImageFormatToBlipType(image.RawFormat, imageFormat);
    msofbtBse1.BlipUsage = MsoBlipUsage.msoblipUsageDefault;
    WorkbookShapeDataImpl.BlipParams blipParams = WorkbookShapeDataImpl.GetBlipParams(msofbtBse1);
    msofbtBse1.RequiredMac = blipParams.ReqMac;
    msofbtBse1.Instance = (int) (msofbtBse1.RequiredWin32 = blipParams.ReqWin32);
    msofbtBse1.Version = 2;
    msofbtBse1.RefCount = 1U;
    IPictureRecord pictureRecord = WorkbookShapeDataImpl.IsBitmapBlip(msofbtBse1.BlipType) ? (IPictureRecord) new MsoBitmapPicture((MsoBase) msofbtBse1) : (IPictureRecord) new MsoMetafilePicture((MsoBase) msofbtBse1);
    (pictureRecord as MsoBase).Instance = blipParams.Instance;
    (pictureRecord as MsoBase).MsoRecordType = (MsoRecords) blipParams.SubRecordType;
    pictureRecord.Picture = image;
    if (num2 == -1)
    {
      ImageFormat rawFormat = image.RawFormat;
      MemoryStream memoryStream = new MemoryStream();
      image.Save((Stream) memoryStream, rawFormat);
      pictureRecord.PictureStream = (Stream) memoryStream;
    }
    ArrayWrapper key = new ArrayWrapper(pictureRecord.RgbUid);
    MsofbtBSE msofbtBse2;
    this.m_dicImageIdToImage.TryGetValue(key, out msofbtBse2);
    if (msofbtBse2 != null && msofbtBse2.BlipType == msofbtBse1.BlipType)
    {
      ++msofbtBse2.RefCount;
      return msofbtBse2.Index + 1;
    }
    msofbtBse1.PictureRecord = pictureRecord;
    this.m_dicImageIdToImage.Add(key, msofbtBse1);
    return this.AddPicture(msofbtBse1);
  }

  [CLSCompliant(false)]
  public int AddPicture(MsofbtBSE picture)
  {
    int count = this.m_arrPictures.Count;
    picture.Index = count;
    this.m_arrPictures.Add(picture);
    return count + 1;
  }

  [CLSCompliant(false)]
  public MsofbtBSE GetPicture(int iPictureId) => this.m_arrPictures[iPictureId - 1];

  [CLSCompliant(false)]
  public void RemovePicture(uint id, bool removeImage)
  {
    if (id < 1U || (long) id > (long) this.m_arrPictures.Count)
      return;
    MsofbtBSE arrPicture = this.m_arrPictures[(int) id - 1];
    --arrPicture.RefCount;
    if (arrPicture.RefCount > 0U || !removeImage)
      return;
    this.m_arrPictures.RemoveAt((int) id - 1);
    ArrayWrapper key = new ArrayWrapper(arrPicture.PictureRecord.RgbUid);
    if (this.m_dicImageIdToImage.ContainsKey(key))
      this.m_dicImageIdToImage.Remove(key);
    int index1 = (int) id - 1;
    for (int count = this.m_arrPictures.Count; index1 < count; ++index1)
      --this.m_arrPictures[index1].Index;
    WorkbookObjectsCollection objects = this.m_book.Objects;
    int index2 = 0;
    for (int count1 = objects.Count; index2 < count1; ++index2)
    {
      ShapeCollectionBase shapeCollectionBase = this.m_shapeGetter((ITabSheet) (objects[index2] as WorksheetBaseImpl));
      int index3 = 0;
      for (int count2 = shapeCollectionBase.Count; index3 < count2; ++index3)
      {
        if (shapeCollectionBase[index3] is BitmapShapeImpl bitmapShapeImpl && bitmapShapeImpl.BlipId > id)
          bitmapShapeImpl.SetBlipId(bitmapShapeImpl.BlipId - 1U);
      }
    }
  }

  public void Clear()
  {
    if (this.m_dicImageIdToImage != null)
      this.m_dicImageIdToImage.Clear();
    if (this.m_arrDGRecords != null)
      this.m_arrDGRecords.Clear();
    if (this.m_arrPictures == null)
      return;
    this.m_arrPictures.Clear();
  }

  public object Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    WorkbookShapeDataImpl workbookShapeDataImpl = (WorkbookShapeDataImpl) this.MemberwiseClone();
    workbookShapeDataImpl.SetParent(parent);
    workbookShapeDataImpl.SetParents();
    workbookShapeDataImpl.m_arrPictures = CloneUtils.CloneCloneable<MsofbtBSE>(this.m_arrPictures);
    workbookShapeDataImpl.m_arrDGRecords = CloneUtils.CloneCloneable<MsoBase>(this.m_arrDGRecords);
    workbookShapeDataImpl.m_dicImageIdToImage = CloneUtils.CloneHash<ArrayWrapper, MsofbtBSE>(this.m_dicImageIdToImage);
    return (object) workbookShapeDataImpl;
  }

  public int RegisterShapes()
  {
    ++this.m_iLastCollectionId;
    return this.m_iLastCollectionId;
  }

  public List<MsofbtBSE> Pictures => this.m_arrPictures;

  protected bool NeedMsoDrawingGroup
  {
    get
    {
      WorkbookObjectsCollection objects = this.m_book.Objects;
      foreach (ShapeCollectionBase enumerateShape in this.m_book.EnumerateShapes(this.m_shapeGetter))
      {
        if (enumerateShape != null && enumerateShape.Count > 0)
          return true;
      }
      return false;
    }
  }

  internal MsofbtDgg PreservedClusters => this.m_preservedDgg;

  public void ClearPreservedClusters() => this.m_preservedDgg = (MsofbtDgg) null;

  public static MsoBlipType ImageFormatToBlipType(ImageFormat format)
  {
    if (format.Equals((object) ImageFormat.Bmp))
      return MsoBlipType.msoblipDIB;
    if (format.Equals((object) ImageFormat.Jpeg))
      return MsoBlipType.msoblipJPEG;
    return format.Equals((object) ImageFormat.Png) || !format.Equals((object) ImageFormat.Emf) ? MsoBlipType.msoblipPNG : MsoBlipType.msoblipEMF;
  }

  public static MsoBlipType ImageFormatToBlipType(ImageFormat format, ExcelImageFormat imageFormat)
  {
    MsoBlipType blipType = WorkbookShapeDataImpl.ImageFormatToBlipType(format);
    return imageFormat == ExcelImageFormat.Original ? blipType : (MsoBlipType) imageFormat;
  }

  public static bool IsBitmapBlip(MsoBlipType blipType)
  {
    return Array.IndexOf<MsoBlipType>(WorkbookShapeDataImpl.METAFILEBLIPS, blipType) == -1;
  }

  [CLSCompliant(false)]
  protected static WorkbookShapeDataImpl.BlipParams GetBlipParams(MsofbtBSE bse)
  {
    MsoBlipType blipType = bse.BlipType;
    return WorkbookShapeDataImpl.s_hashBlipTypeToParams.ContainsKey(blipType) ? WorkbookShapeDataImpl.s_hashBlipTypeToParams[blipType] : WorkbookShapeDataImpl.s_hashBlipTypeToParams[MsoBlipType.msoblipPNG];
  }

  protected class BlipParams
  {
    public int Instance;
    public byte ReqMac;
    public byte ReqWin32;
    public int SubRecordType;

    public BlipParams(int inst, byte mac, byte win32, int subrec)
    {
      this.Instance = inst;
      this.ReqMac = mac;
      this.ReqWin32 = win32;
      this.SubRecordType = subrec;
    }
  }
}
