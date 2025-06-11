// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.BaseContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class BaseContainer : BaseEscherRecord
{
  private ContainerCollection m_childrenContainers;

  internal ContainerCollection Children => this.m_childrenContainers;

  internal BaseContainer(WordDocument doc)
    : base(doc)
  {
    this.m_childrenContainers = new ContainerCollection(doc);
  }

  internal BaseContainer(MSOFBT type, WordDocument doc)
    : base(doc)
  {
    this.m_childrenContainers = new ContainerCollection(doc);
    this.Header.IsContainer = true;
    this.Header.Type = type;
  }

  internal static bool RemoveContainerBySpid(BaseContainer baseContainer, int spid)
  {
    bool flag = false;
    int index = 0;
    for (int count = baseContainer.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = baseContainer.Children[index] as BaseEscherRecord;
      if (!flag)
      {
        switch (child)
        {
          case MsofbtSpContainer _:
            if ((child as MsofbtSpContainer).Shape.ShapeId == spid)
            {
              baseContainer.Children.Remove((object) child);
              flag = true;
              goto label_8;
            }
            break;
          case BaseContainer _:
            flag = BaseContainer.RemoveContainerBySpid(child as BaseContainer, spid);
            break;
        }
      }
      else
        break;
    }
label_8:
    return flag;
  }

  internal void SynchronizeIdent(
    WTextBoxCollection autoShapeCollection,
    ref int txbxShapeId,
    ref int pictShapeId,
    ref int txId,
    ref int textColIndex)
  {
    int index = 0;
    for (int count = this.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = this.Children[index] as BaseEscherRecord;
      if (child is MsofbtSp)
        this.SyncSpRecord(child as MsofbtSp, autoShapeCollection, ref txbxShapeId, ref pictShapeId, ref textColIndex);
      if (child is MsofbtOPT)
        this.SyncOPTTxid(child as MsofbtOPT, ref txId);
      if (child is MsofbtClientTextbox)
        (child as MsofbtClientTextbox).Txid = txId;
      if (child is BaseContainer)
        (child as BaseContainer).SynchronizeIdent(autoShapeCollection, ref txbxShapeId, ref pictShapeId, ref txId, ref textColIndex);
    }
  }

  internal int GetSpid()
  {
    int spid = 0;
    int index = 0;
    for (int count = this.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = this.Children[index] as BaseEscherRecord;
      if (spid == 0)
      {
        switch (child)
        {
          case MsofbtSp _:
            spid = (child as MsofbtSp).ShapeId;
            goto label_7;
          case BaseContainer _:
            spid = (child as BaseContainer).GetSpid();
            break;
        }
      }
      else
        break;
    }
label_7:
    return spid;
  }

  internal bool SetSpid(int spid)
  {
    bool flag = false;
    int index = 0;
    for (int count = this.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = this.Children[index] as BaseEscherRecord;
      if (!flag)
      {
        switch (child)
        {
          case MsofbtSp _:
            (child as MsofbtSp).ShapeId = spid;
            flag = true;
            goto label_7;
          case BaseContainer _:
            flag = (child as BaseContainer).SetSpid(spid);
            break;
        }
      }
      else
        break;
    }
label_7:
    return flag;
  }

  internal BaseEscherRecord FindContainerByMsofbt(MSOFBT msofbt)
  {
    for (int index = 0; index < this.m_childrenContainers.Count; ++index)
    {
      BaseEscherRecord childrenContainer = this.m_childrenContainers[index] as BaseEscherRecord;
      if (childrenContainer.Header.Type == msofbt)
        return childrenContainer;
    }
    return (BaseEscherRecord) null;
  }

  internal BaseEscherRecord FindContainerByType(Type type)
  {
    for (int index = 0; index < this.m_childrenContainers.Count; ++index)
    {
      BaseEscherRecord childrenContainer = this.m_childrenContainers[index] as BaseEscherRecord;
      if (childrenContainer.GetType() == type)
        return childrenContainer;
    }
    return (BaseEscherRecord) null;
  }

  internal BaseContainer FindParentContainer(BaseContainer baseContainer)
  {
    for (int index = 0; index < this.m_childrenContainers.Count; ++index)
    {
      BaseContainer childrenContainer = this.m_childrenContainers[index] as BaseContainer;
      if (childrenContainer == baseContainer)
        return this;
      if (childrenContainer != null)
      {
        BaseContainer parentContainer = childrenContainer.FindParentContainer(baseContainer);
        if (parentContainer != null)
          return parentContainer;
      }
    }
    return (BaseContainer) null;
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_childrenContainers.Read(stream, this.Header.Length);
  }

  protected override void WriteRecordData(Stream stream) => this.m_childrenContainers.Write(stream);

  internal override BaseEscherRecord Clone()
  {
    int type = (int) this.Header.Type;
    BaseEscherRecord recordFromHeader = this.Header.CreateRecordFromHeader();
    foreach (BaseEscherRecord child in (List<object>) this.Children)
    {
      child.Header.CreateRecordFromHeader().Header = child.Header.Clone();
      (recordFromHeader as BaseContainer).Children.Add((object) child.Clone());
    }
    recordFromHeader.m_doc = this.m_doc;
    return recordFromHeader;
  }

  internal virtual void CloneRelationsTo(WordDocument doc)
  {
  }

  internal void RemoveBaseContainerOle()
  {
    int index = 0;
    for (int count = this.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = this.Children[index] as BaseEscherRecord;
      switch (child)
      {
        case MsofbtSpContainer _:
          if ((child as MsofbtSpContainer).Shape.IsOle)
          {
            (child as MsofbtSpContainer).RemoveSpContainerOle();
            break;
          }
          break;
        case BaseContainer _:
          (child as BaseContainer).RemoveBaseContainerOle();
          break;
      }
    }
  }

  private void SyncOPTTxid(MsofbtOPT optRecord, ref int txId)
  {
    int num = txId;
    FOPTEBid fopteBid = (FOPTEBid) null;
    if (optRecord.Properties.ContainsKey(128 /*0x80*/))
    {
      FOPTEBid property = optRecord.Properties[128 /*0x80*/] as FOPTEBid;
      txId += 65536 /*0x010000*/;
      property.Value = (uint) txId;
    }
    if (optRecord.Properties.ContainsKey(267) && (this as MsofbtSpContainer).Shape.ShapeType == EscherShapeType.msosptHostControl)
    {
      FOPTEBid property = optRecord.Properties[267] as FOPTEBid;
      if (num == txId)
        txId += 65536 /*0x010000*/;
      property.Value = (uint) txId;
    }
    if (!optRecord.Properties.ContainsKey(138))
      return;
    fopteBid = optRecord.Properties[138] as FOPTEBid;
    optRecord.Properties.Remove(138);
  }

  private void SyncSpRecord(
    MsofbtSp msofbtSp,
    WTextBoxCollection autoShapeCollection,
    ref int txbxShapeId,
    ref int pictShapeId,
    ref int textColIndex)
  {
    if (msofbtSp.ShapeType == EscherShapeType.msosptPictureFrame)
    {
      msofbtSp.ShapeId = pictShapeId;
      ++pictShapeId;
    }
    else
    {
      msofbtSp.ShapeId = txbxShapeId;
      ++txbxShapeId;
      if (autoShapeCollection == null || !(this is MsofbtSpContainer) || (this as MsofbtSpContainer).ShapeOptions == null || (this as MsofbtSpContainer).ShapeOptions.Txid == null && (this as MsofbtSpContainer).Shape.ShapeType != EscherShapeType.msosptHostControl || autoShapeCollection.Count <= 0)
        return;
      (autoShapeCollection[textColIndex] as WTextBox).TextBoxSpid = msofbtSp.ShapeId;
      ++textColIndex;
    }
  }

  internal override void Close()
  {
    if (this.m_childrenContainers == null || this.m_childrenContainers.Count == 0)
    {
      this.m_childrenContainers = (ContainerCollection) null;
    }
    else
    {
      int index = 0;
      for (int count = this.m_childrenContainers.Count; index < count; ++index)
      {
        object childrenContainer = this.m_childrenContainers[index];
        switch (childrenContainer)
        {
          case BaseContainer _:
            (childrenContainer as BaseContainer).Close();
            break;
          case BaseEscherRecord _:
            (childrenContainer as BaseEscherRecord).Close();
            break;
          case BaseWordRecord _:
            (childrenContainer as BaseWordRecord).Close();
            break;
        }
      }
    }
  }
}
