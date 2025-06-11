// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoContainerBase
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
internal abstract class MsoContainerBase : MsoBase
{
  private List<MsoBase> m_arrItems = new List<MsoBase>();

  public MsoContainerBase(MsoBase parent)
    : base(parent)
  {
  }

  public MsoContainerBase(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsoContainerBase(
    MsoBase parent,
    byte[] data,
    int iOffset,
    GetNextMsoDrawingData dataGetter)
    : base(parent, data, iOffset, dataGetter)
  {
  }

  private void ParseItems(Stream data, int iOffset)
  {
    long num = data.Position + (long) this.m_iLength;
    while (num > data.Position)
      this.m_arrItems.Add(this.DataGetter == null ? MsoFactory.CreateMsoRecord((MsoBase) this, data) : MsoFactory.CreateMsoRecord((MsoBase) this, data, this.DataGetter));
  }

  public void AddItem(MsoBase itemToAdd)
  {
    if (itemToAdd == null)
      throw new ArgumentNullException(nameof (itemToAdd));
    this.m_arrItems.Add(itemToAdd);
  }

  public void AddItems(ICollection<MsoBase> items)
  {
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    this.m_arrItems.AddRange((IEnumerable<MsoBase>) items);
  }

  public MsoBase[] Items => this.m_arrItems.ToArray();

  internal List<MsoBase> ItemsList => this.m_arrItems;

  public override void ParseStructure(Stream stream) => this.ParseItems(stream, 0);

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    long position = stream.Position;
    int index = 0;
    for (int count = this.m_arrItems.Count; index < count; ++index)
      this.m_arrItems[index].FillArray(stream, (int) stream.Position, arrBreaks, arrRecords);
    this.m_iLength = (int) (stream.Position - position);
  }

  protected override object InternalClone()
  {
    MsoContainerBase parent = (MsoContainerBase) base.InternalClone();
    if (parent.m_arrItems != null)
    {
      int count = this.m_arrItems.Count;
      List<MsoBase> msoBaseList = new List<MsoBase>(count);
      for (int index = 0; index < count; ++index)
      {
        MsoBase msoBase = this.m_arrItems[index].Clone((MsoBase) parent);
        msoBaseList.Add(msoBase);
      }
      parent.m_arrItems = msoBaseList;
    }
    return (object) parent;
  }
}
