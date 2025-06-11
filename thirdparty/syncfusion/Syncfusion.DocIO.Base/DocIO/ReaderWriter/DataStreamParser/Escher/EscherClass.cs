// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.EscherClass
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class EscherClass
{
  private const int DEF_MAIN_DRAWING_ID = 1;
  private const int DEF_HF_DRAWING_ID = 2;
  private const int DEF_MAIN_SPID = 1024 /*0x0400*/;
  private const int DEF_HF_SPID = 2048 /*0x0800*/;
  private const int DEF_MAIN_SPIDMAX = 2050;
  private const int DEF_HF_SPIDMAX = 3074;
  internal MsofbtDggContainer m_msofbtDggContainer;
  internal ContainerCollection m_dgContainers;
  private Dictionary<int, BaseContainer> m_containers;
  private MsofbtSpContainer m_backgroundContainer;
  private WordDocument m_doc;

  internal WordDocument Document => this.m_doc;

  internal Dictionary<int, BaseContainer> Containers
  {
    get
    {
      if (this.m_containers == null)
        this.m_containers = new Dictionary<int, BaseContainer>();
      return this.m_containers;
    }
  }

  internal MsofbtSpContainer BackgroundContainer
  {
    get
    {
      if (this.m_backgroundContainer == null)
        this.m_backgroundContainer = this.GetBackgroundContainerValue();
      return this.m_backgroundContainer;
    }
  }

  internal EscherClass(WordDocument doc)
  {
    this.m_doc = doc;
    this.m_dgContainers = new ContainerCollection(this.m_doc);
    this.CreateDefaultDgg();
  }

  internal EscherClass(
    Stream tableStream,
    Stream docStream,
    long dggInfoOffset,
    long dggInfoLength,
    WordDocument doc)
    : this(doc)
  {
    tableStream.Position = dggInfoOffset;
    if (dggInfoLength == 0L)
      return;
    this.Read(tableStream, dggInfoLength, docStream);
  }

  internal void Read(Stream tableStream, long dggInfoLength, Stream docStream)
  {
    long num1 = tableStream.Position + dggInfoLength;
    this.m_msofbtDggContainer = _MSOFBH.ReadHeaderWithRecord(tableStream, this.m_doc) as MsofbtDggContainer;
    if (this.m_msofbtDggContainer == null)
      throw new ArgumentException("First Escher record is not DggContainer.");
    while (tableStream.Position < num1)
    {
      int num2 = tableStream.ReadByte();
      if (!(_MSOFBH.ReadHeaderWithRecord(tableStream, this.m_doc) is MsofbtDgContainer msofbtDgContainer))
        throw new ArgumentException("Expected DgContainer records only.");
      msofbtDgContainer.ShapeDocType = (ShapeDocType) num2;
      this.m_dgContainers.Add((object) msofbtDgContainer);
    }
    this.FillCollectionForSearch();
    this.ReadContainersData(docStream);
  }

  internal void ReadContainersData(Stream Stream)
  {
    foreach (BaseContainer dgContainer in (List<object>) this.m_dgContainers)
      this.ReadBseData(dgContainer, Stream);
  }

  internal void WriteContainersData(Stream stream)
  {
    if (this.m_msofbtDggContainer == null || this.m_msofbtDggContainer.BstoreContainer == null)
      return;
    int index = 0;
    for (int count = this.m_msofbtDggContainer.BstoreContainer.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = this.m_msofbtDggContainer.BstoreContainer.Children[index] as BaseEscherRecord;
      if (child is MsofbtBSE)
        (child as MsofbtBSE).Write(stream);
      else
        child.WriteMsofbhWithRecord(stream);
    }
  }

  internal uint WriteContainers(Stream stream)
  {
    if (this.m_msofbtDggContainer == null)
      return 0;
    long position = stream.Position;
    this.InitWriting();
    this.WriteDggContainer(stream);
    this.WriteDgContainers(stream);
    return (uint) (stream.Position - position);
  }

  internal void AddContainerForSubDocument(
    WordSubdocument documentType,
    BaseEscherRecord baseEscherRecord)
  {
    if (this.m_msofbtDggContainer == null)
      this.CreateDefaultDgg();
    this.CreateDgForSubDocuments();
    this.FindDgContainerForSubDocType(EscherClass.ConvertToShapeDocType(documentType)).PatriarchGroupContainer.Children.Add((object) baseEscherRecord);
    this.AddParentContainer(baseEscherRecord as BaseContainer);
    this.FillCollectionForSearch(baseEscherRecord as BaseContainer);
  }

  internal MsofbtDgContainer FindDgContainerForSubDocType(ShapeDocType ShapeDocType)
  {
    foreach (MsofbtDgContainer dgContainer in (List<object>) this.m_dgContainers)
    {
      if (dgContainer.ShapeDocType == ShapeDocType)
        return dgContainer;
    }
    return (MsofbtDgContainer) null;
  }

  internal void InitShapeSpids()
  {
  }

  internal void RemoveHeaderContainer()
  {
    if (this.m_dgContainers.Count <= 1)
      return;
    if ((this.m_dgContainers[1] as MsofbtDgContainer).ShapeDocType != ShapeDocType.HeaderFooter)
      throw new ArgumentException("Expected header drawing, but got something else.");
    this.m_dgContainers.RemoveAt(1);
  }

  internal BaseContainer FindParentContainer(BaseContainer baseContainer)
  {
    foreach (BaseContainer dgContainer in (List<object>) this.m_dgContainers)
    {
      BaseContainer parentContainer = dgContainer.FindParentContainer(baseContainer);
      if (parentContainer != null)
        return parentContainer;
    }
    return (BaseContainer) null;
  }

  internal int GetTxid(int spid) => this.FindInDgContainers(spid).Txid;

  internal int GetShapeOrderIndex(int spId)
  {
    int shapeOrderIndex = -1;
    if (this.Containers != null && this.Containers.ContainsKey(spId) && this.Containers[spId] is MsofbtSpContainer)
    {
      int[] array = new int[this.Containers.Keys.Count];
      this.Containers.Keys.CopyTo(array, 0);
      shapeOrderIndex = Array.IndexOf<int>(array, spId);
    }
    return shapeOrderIndex;
  }

  internal void SetTxid(int spid, int txid)
  {
    MsofbtSpContainer inDgContainers = this.FindInDgContainers(spid);
    inDgContainers.Txid = txid;
    if (!(inDgContainers.FindContainerByMsofbt(MSOFBT.msofbtClientTextbox) is MsofbtClientTextbox containerByMsofbt))
      return;
    containerByMsofbt.Txid = txid;
  }

  internal BaseContainer FindContainerBySpid(int spid)
  {
    MsofbtSpContainer inDgContainers = this.FindInDgContainers(spid);
    if (inDgContainers == null)
      return (BaseContainer) null;
    return inDgContainers.Shape.ShapeType == EscherShapeType.msosptMin && inDgContainers.Shape.IsGroup ? this.FindParentContainer((BaseContainer) inDgContainers) : (BaseContainer) inDgContainers;
  }

  internal MsofbtSpContainer FindInDgContainers(int spid)
  {
    foreach (BaseContainer dgContainer in (List<object>) this.m_dgContainers)
    {
      MsofbtSpContainer containerAmongChildren = EscherClass.FindContainerAmongChildren(dgContainer, spid);
      if (containerAmongChildren != null)
        return containerAmongChildren;
    }
    return (MsofbtSpContainer) null;
  }

  internal EscherShapeType GetBaseEscherRecordType(MsofbtSpContainer spCon)
  {
    return spCon.Shape.ShapeType;
  }

  internal int CloneContainerBySpid(
    WordDocument destDoc,
    WordSubdocument docType,
    int spid,
    int newSpid)
  {
    BaseContainer baseContainer1 = (BaseContainer) null;
    if (destDoc.Escher.Containers.ContainsKey(newSpid))
      baseContainer1 = destDoc.Escher.Containers[newSpid];
    for (; baseContainer1 != null && destDoc.Escher.Containers.ContainsKey(newSpid); ++newSpid)
      baseContainer1 = destDoc.Escher.Containers[newSpid];
    if (!this.Containers.ContainsKey(spid))
      return -1;
    BaseContainer baseContainer2 = (BaseContainer) this.Containers[spid].Clone();
    baseContainer2.SetSpid(newSpid);
    destDoc.Escher.AddContainerForSubDocument(docType, (BaseEscherRecord) baseContainer2);
    baseContainer2.CloneRelationsTo(destDoc);
    return newSpid;
  }

  internal void RemoveEscherOle()
  {
    foreach (BaseContainer dgContainer in (List<object>) this.m_dgContainers)
      dgContainer.RemoveBaseContainerOle();
  }

  internal void RemoveContainerBySpid(int spid, bool isHeaderContainer)
  {
    MsofbtDgContainer containerForSubDocType = this.FindDgContainerForSubDocType(isHeaderContainer ? ShapeDocType.HeaderFooter : ShapeDocType.Main);
    bool flag = false;
    this.Containers.Remove(spid);
    int index = 0;
    for (int count = containerForSubDocType.Children.Count; index < count && !flag; ++index)
    {
      BaseEscherRecord child = containerForSubDocType.Children[index] as BaseEscherRecord;
      switch (child)
      {
        case MsofbtSpContainer _:
          if ((child as MsofbtSpContainer).Shape.ShapeId == spid)
          {
            containerForSubDocType.Children.Remove((object) child);
            flag = true;
            break;
          }
          break;
        case BaseContainer _:
          flag = BaseContainer.RemoveContainerBySpid(child as BaseContainer, spid);
          break;
      }
    }
  }

  internal void RemoveBStoreByPid(int pib)
  {
    foreach (BaseEscherRecord child in (List<object>) this.m_msofbtDggContainer.Children)
    {
      if (child is MsofbtBstoreContainer && pib <= (child as MsofbtBstoreContainer).Children.Count)
      {
        (child as MsofbtBstoreContainer).Children.RemoveAt(pib - 1);
        break;
      }
    }
  }

  internal void ModifyBStoreByPid(int pib, MsofbtBSE bse)
  {
    foreach (BaseEscherRecord child in (List<object>) this.m_msofbtDggContainer.Children)
    {
      if (child is MsofbtBstoreContainer && pib <= (child as MsofbtBstoreContainer).Children.Count)
      {
        ((child as MsofbtBstoreContainer).Children[pib - 1] as MsofbtBSE).Blip.ImageRecord = bse.Blip.ImageRecord;
        break;
      }
    }
  }

  internal MsofbtSpContainer GetBackgroundContainerValue()
  {
    MsofbtDgContainer containerForSubDocType = this.FindDgContainerForSubDocType(ShapeDocType.Main);
    if (containerForSubDocType != null)
    {
      foreach (BaseEscherRecord child in (List<object>) containerForSubDocType.Children)
      {
        if (child is MsofbtSpContainer)
          return child as MsofbtSpContainer;
      }
    }
    return (MsofbtSpContainer) null;
  }

  internal bool CheckBStoreContByPid(int pib)
  {
    bool flag = false;
    if (this.m_msofbtDggContainer.BstoreContainer != null)
    {
      MsofbtBstoreContainer bstoreContainer = this.m_msofbtDggContainer.BstoreContainer;
      if (bstoreContainer.Children.Count >= pib && ((MsofbtBSE) bstoreContainer.Children[pib - 1]).Blip != null)
        flag = true;
    }
    return flag;
  }

  private void ReadBseData(BaseContainer baseContainer, Stream stream)
  {
    for (int index = 0; index < baseContainer.Children.Count; ++index)
    {
      BaseEscherRecord child1 = baseContainer.Children[index] as BaseEscherRecord;
      if (child1 is MsofbtSpContainer)
      {
        MsofbtSpContainer spContainer = child1 as MsofbtSpContainer;
        if (spContainer.Shape != null)
        {
          int blipId = this.GetBlipId(spContainer);
          if (blipId >= 0)
          {
            MsofbtBSE child2 = this.m_msofbtDggContainer.BstoreContainer.Children[blipId] as MsofbtBSE;
            if (child2.Fbse.m_size != 0)
            {
              child2.Read(stream);
              spContainer.Bse = child2;
            }
            else
              continue;
          }
        }
      }
      if (child1 is BaseContainer)
        this.ReadBseData(child1 as BaseContainer, stream);
    }
  }

  private void InitWriting()
  {
    MsofbtDgg dgg = this.m_msofbtDggContainer.Dgg;
    int num = 0;
    dgg.Fidcls.Clear();
    for (int index = 0; index < this.m_dgContainers.Count; ++index)
    {
      MsofbtDgContainer dgContainer = this.m_dgContainers[index] as MsofbtDgContainer;
      dgContainer.InitWriting();
      if (dgContainer.ShapeDocType == ShapeDocType.HeaderFooter && dgContainer.Dg.ShapeCount <= 1)
      {
        this.RemoveContainerBySpid(2048 /*0x0800*/, true);
        this.m_dgContainers.RemoveAt(index);
        --index;
        --num;
      }
      else
      {
        int shapeCount = this.GetShapeCount((BaseContainer) dgContainer.PatriarchGroupContainer);
        this.m_msofbtDggContainer.Dgg.Fidcls.Add(new FIDCL(dgContainer.Dg.DrawingId, shapeCount + 1));
        num += shapeCount + 1;
      }
    }
    dgg.DrawingCount = this.m_dgContainers.Count;
    dgg.ShapeCount = num;
    dgg.SpidMax = (this.m_dgContainers.Count + 1) * 1024 /*0x0400*/ + 2;
  }

  private void WriteDggContainer(Stream stream)
  {
    this.m_msofbtDggContainer.WriteMsofbhWithRecord(stream);
  }

  private void WriteDgContainers(Stream stream)
  {
    foreach (MsofbtDgContainer dgContainer in (List<object>) this.m_dgContainers)
    {
      stream.WriteByte((byte) dgContainer.ShapeDocType);
      dgContainer.WriteMsofbhWithRecord(stream);
    }
  }

  internal void CreateDgForSubDocuments()
  {
    if (this.FindDgContainerForSubDocType(ShapeDocType.Main) == null)
      this.CreateDgForSubDocument(ShapeDocType.Main, 1, 1024 /*0x0400*/);
    if (this.FindDgContainerForSubDocType(ShapeDocType.HeaderFooter) != null)
      return;
    this.CreateDgForSubDocument(ShapeDocType.HeaderFooter, 2, 2048 /*0x0800*/);
  }

  private void CreateDefaultDgg()
  {
    this.m_msofbtDggContainer = new MsofbtDggContainer(this.m_doc);
    this.m_msofbtDggContainer.Children.Add((object) new MsofbtDgg(this.m_doc));
    this.m_msofbtDggContainer.Children.Add((object) new MsofbtBstoreContainer(this.m_doc));
  }

  private void CreateDgForSubDocument(ShapeDocType shapeDocType, int drawingId, int shapeId)
  {
    MsofbtDgContainer msofbtDgContainer = new MsofbtDgContainer(this.m_doc);
    msofbtDgContainer.ShapeDocType = shapeDocType;
    this.m_dgContainers.Add((object) msofbtDgContainer);
    msofbtDgContainer.Children.Add((object) new MsofbtDg(this.m_doc)
    {
      DrawingId = drawingId,
      ShapeCount = 1,
      SpidLast = shapeId
    });
    MsofbtSpgrContainer msofbtSpgrContainer = new MsofbtSpgrContainer(this.m_doc);
    msofbtDgContainer.Children.Add((object) msofbtSpgrContainer);
    MsofbtSpContainer msofbtSpContainer1 = new MsofbtSpContainer(this.m_doc);
    msofbtSpgrContainer.Children.Add((object) msofbtSpContainer1);
    MsofbtSpgr msofbtSpgr = new MsofbtSpgr(this.m_doc);
    msofbtSpContainer1.Children.Add((object) msofbtSpgr);
    msofbtSpContainer1.Children.Add((object) new MsofbtSp(this.m_doc)
    {
      ShapeId = shapeId,
      IsGroup = true,
      IsPatriarch = true,
      ShapeType = EscherShapeType.msosptMin
    });
    this.Containers.Add(msofbtSpContainer1.Shape.ShapeId, (BaseContainer) msofbtSpContainer1);
    if (shapeDocType == ShapeDocType.Main)
    {
      MsofbtSpContainer msofbtSpContainer2 = new MsofbtSpContainer(this.m_doc);
      msofbtDgContainer.Children.Add((object) msofbtSpContainer2.CreateRectangleContainer());
      this.Containers.Add(msofbtSpContainer2.Shape.ShapeId, (BaseContainer) msofbtSpContainer1);
    }
    msofbtDgContainer.Children.Add((object) new MsofbtSolverContainer(this.m_doc));
  }

  private static MsofbtSpContainer FindContainerAmongChildren(
    BaseContainer parentContainer,
    int spid)
  {
    int index = 0;
    for (int count = parentContainer.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = parentContainer.Children[index] as BaseEscherRecord;
      switch (child)
      {
        case MsofbtSp _:
          if ((child as MsofbtSp).ShapeId == spid)
            return parentContainer as MsofbtSpContainer;
          break;
        case BaseContainer _:
          MsofbtSpContainer containerAmongChildren = EscherClass.FindContainerAmongChildren(child as BaseContainer, spid);
          if (containerAmongChildren != null)
            return containerAmongChildren;
          break;
      }
    }
    return (MsofbtSpContainer) null;
  }

  private void AddShapeBse(MsofbtSpContainer spContainer)
  {
    if (spContainer.IsWatermark && spContainer.Pib != -1 || spContainer.Bse == null)
      return;
    if (this.m_msofbtDggContainer.BstoreContainer == null)
      this.m_msofbtDggContainer.Children.Add((object) new MsofbtBstoreContainer(this.m_doc));
    this.m_msofbtDggContainer.BstoreContainer.Children.Add((object) spContainer.Bse);
    if (spContainer.Shape.ShapeType == EscherShapeType.msosptPictureFrame)
    {
      spContainer.Pib = this.m_msofbtDggContainer.BstoreContainer.Children.Count;
    }
    else
    {
      if (!(spContainer.ShapeOptions.Properties[390] is FOPTEBid property))
        return;
      property.Value = (uint) this.m_msofbtDggContainer.BstoreContainer.Children.Count;
    }
  }

  internal static ShapeDocType ConvertToShapeDocType(WordSubdocument docType)
  {
    switch (docType)
    {
      case WordSubdocument.Main:
        return ShapeDocType.Main;
      case WordSubdocument.HeaderFooter:
        return ShapeDocType.HeaderFooter;
      default:
        throw new Exception($"Windows.Media for {docType.ToString()} document is not available");
    }
  }

  private void AddParentContainer(BaseContainer baseContainer)
  {
    if (baseContainer is MsofbtSpContainer)
      this.AddShapeBse(baseContainer as MsofbtSpContainer);
    for (int index = 0; index < baseContainer.Children.Count; ++index)
    {
      BaseEscherRecord child = baseContainer.Children[index] as BaseEscherRecord;
      switch (child)
      {
        case MsofbtSpContainer _:
          this.AddShapeBse(child as MsofbtSpContainer);
          break;
        case BaseContainer _:
          this.AddParentContainer(child as BaseContainer);
          break;
      }
    }
  }

  private MsofbtGeneral CreateGeneralData()
  {
    return new MsofbtGeneral(this.m_doc)
    {
      Data = new byte[16 /*0x10*/]
      {
        byte.MaxValue,
        byte.MaxValue,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        byte.MaxValue,
        (byte) 0,
        (byte) 128 /*0x80*/,
        (byte) 128 /*0x80*/,
        (byte) 128 /*0x80*/,
        (byte) 0,
        (byte) 247,
        (byte) 0,
        (byte) 0,
        (byte) 16 /*0x10*/
      }
    };
  }

  private void InitFidcl(int dgid, int cspidCur)
  {
    FIDCL fidclObj = new FIDCL(dgid, cspidCur);
    if (this.m_msofbtDggContainer.Dgg.Fidcls.Count != 0 && this.FindFIDCLDgid(dgid, fidclObj))
      return;
    this.m_msofbtDggContainer.Dgg.Fidcls.Add(fidclObj);
  }

  private bool FindFIDCLDgid(int dgid, FIDCL fidclObj)
  {
    int index = 0;
    foreach (FIDCL fidcl in this.m_msofbtDggContainer.Dgg.Fidcls)
    {
      if (fidcl.m_dgid == dgid)
      {
        this.m_msofbtDggContainer.Dgg.Fidcls[index] = fidclObj;
        return true;
      }
      ++index;
    }
    return false;
  }

  private int GetShapeCount(BaseContainer baseContainer)
  {
    int shapeCount = 0;
    foreach (BaseEscherRecord child in (List<object>) baseContainer.Children)
    {
      if (child is MsofbtSpContainer)
        ++shapeCount;
      else if (child is BaseContainer)
        shapeCount += this.GetShapeCount(child as BaseContainer);
    }
    return shapeCount;
  }

  private int GetBlipId(MsofbtSpContainer spContainer)
  {
    uint propertyValue1 = spContainer.GetPropertyValue(390);
    if (propertyValue1 != uint.MaxValue)
      return (int) propertyValue1 - 1;
    uint propertyValue2 = spContainer.GetPropertyValue(260);
    return propertyValue2 != uint.MaxValue ? (int) propertyValue2 - 1 : -1;
  }

  private void FillCollectionForSearch()
  {
    foreach (BaseContainer dgContainer in (List<object>) this.m_dgContainers)
      this.FillCollectionForSearch(dgContainer);
  }

  internal void FillCollectionForSearch(BaseContainer baseContainer)
  {
    if (baseContainer is MsofbtSpContainer)
    {
      this.AddSpContToSearchCol(baseContainer as MsofbtSpContainer);
    }
    else
    {
      foreach (BaseEscherRecord child in (List<object>) baseContainer.Children)
      {
        if (child is BaseContainer)
          this.FillCollectionForSearch(child as BaseContainer);
        if (child is MsofbtSpgrContainer)
        {
          MsofbtSpgrContainer msofbtSpgrContainer = child as MsofbtSpgrContainer;
          if (!this.Containers.ContainsKey(msofbtSpgrContainer.Shape.ShapeId))
            this.Containers.Add(msofbtSpgrContainer.Shape.ShapeId, (BaseContainer) msofbtSpgrContainer);
        }
      }
    }
  }

  private void AddSpContToSearchCol(MsofbtSpContainer spContainer)
  {
    if (this.Containers.ContainsKey(spContainer.Shape.ShapeId))
      return;
    if (spContainer.Shape.IsGroup)
      this.Containers.Add(spContainer.Shape.ShapeId, this.FindParentContainer((BaseContainer) spContainer));
    else
      this.Containers.Add(spContainer.Shape.ShapeId, (BaseContainer) spContainer);
  }

  internal void Close()
  {
    if (this.m_containers != null)
    {
      this.m_containers.Clear();
      this.m_containers = (Dictionary<int, BaseContainer>) null;
    }
    this.m_doc = (WordDocument) null;
    MsofbtDgContainer msofbtDgContainer = (MsofbtDgContainer) null;
    int index = 0;
    for (int count = this.m_dgContainers.Count; index < count; ++index)
    {
      (this.m_dgContainers[index] as MsofbtDgContainer).Close();
      msofbtDgContainer = (MsofbtDgContainer) null;
    }
    if (this.m_msofbtDggContainer != null)
    {
      this.m_msofbtDggContainer.Close();
      this.m_msofbtDggContainer = (MsofbtDggContainer) null;
    }
    if (this.m_backgroundContainer == null)
      return;
    this.m_backgroundContainer.Close();
    this.m_backgroundContainer = (MsofbtSpContainer) null;
  }
}
