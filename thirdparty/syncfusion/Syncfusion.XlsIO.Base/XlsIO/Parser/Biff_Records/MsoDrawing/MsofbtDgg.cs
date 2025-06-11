// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsofbtDgg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtDgg)]
[CLSCompliant(false)]
public class MsofbtDgg : MsoBase
{
  private const int DEF_ARRAY_OFFSET = 16 /*0x10*/;
  [BiffRecordPos(0, 4)]
  private uint m_uiIdMax;
  [BiffRecordPos(4, 4)]
  private uint m_uiNumberOfIdClus;
  [BiffRecordPos(8, 4)]
  private uint m_uiTotalShapes;
  [BiffRecordPos(12, 4)]
  private uint m_uiTotalDrawings;
  private List<MsofbtDgg.ClusterID> m_arrClusters = new List<MsofbtDgg.ClusterID>();

  public MsofbtDgg(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtDgg(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public uint IdMax
  {
    get => this.m_uiIdMax;
    set => this.m_uiIdMax = value;
  }

  public uint NumberOfIdClus => this.m_uiNumberOfIdClus;

  public uint TotalShapes
  {
    get => this.m_uiTotalShapes;
    set => this.m_uiTotalShapes = value;
  }

  public uint TotalDrawings
  {
    get => this.m_uiTotalDrawings;
    set => this.m_uiTotalDrawings = value;
  }

  public MsofbtDgg.ClusterID[] ClusterIDs => this.m_arrClusters.ToArray();

  public override void ParseStructure(Stream stream)
  {
    this.m_uiIdMax = MsoBase.ReadUInt32(stream);
    this.m_uiNumberOfIdClus = MsoBase.ReadUInt32(stream);
    this.m_uiTotalShapes = MsoBase.ReadUInt32(stream);
    this.m_uiTotalDrawings = MsoBase.ReadUInt32(stream);
    int num1 = 16 /*0x10*/;
    if (this.m_uiNumberOfIdClus <= 0U)
      return;
    int num2 = 0;
    while ((long) num2 < (long) (this.m_uiNumberOfIdClus - 1U))
    {
      this.m_arrClusters.Add(new MsofbtDgg.ClusterID(stream));
      ++num2;
      num1 += MsofbtDgg.ClusterID.Size;
    }
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    MsoBase.WriteUInt32(stream, this.m_uiIdMax);
    MsoBase.WriteUInt32(stream, this.m_uiNumberOfIdClus);
    MsoBase.WriteUInt32(stream, this.m_uiTotalShapes);
    MsoBase.WriteUInt32(stream, this.m_uiTotalDrawings);
    this.m_iLength = 16 /*0x10*/;
    int index = 0;
    int count = this.m_arrClusters.Count;
    while (index < count)
    {
      this.m_arrClusters[index].Write(stream);
      ++index;
      this.m_iLength += MsofbtDgg.ClusterID.Size;
    }
  }

  protected override object InternalClone()
  {
    MsofbtDgg msofbtDgg = (MsofbtDgg) base.InternalClone();
    if (this.m_arrClusters != null)
    {
      int count = this.m_arrClusters.Count;
      List<MsofbtDgg.ClusterID> clusterIdList = new List<MsofbtDgg.ClusterID>(count);
      for (int index = 0; index < count; ++index)
      {
        MsofbtDgg.ClusterID clusterId = (MsofbtDgg.ClusterID) this.m_arrClusters[index].Clone();
        clusterIdList.Add(clusterId);
      }
      msofbtDgg.m_arrClusters = clusterIdList;
    }
    return (object) msofbtDgg;
  }

  public void AddCluster(uint uiGroupId, uint uiNumber)
  {
    this.m_arrClusters.Add(new MsofbtDgg.ClusterID(uiGroupId, uiNumber));
    this.m_uiNumberOfIdClus = (uint) (this.m_arrClusters.Count + 1);
    this.m_uiTotalDrawings = (uint) this.m_arrClusters.Count;
  }

  public class ClusterID : ICloneable
  {
    private const int DEF_SIZE = 8;
    private uint m_uiGroupId;
    private uint m_uiNumber;

    public ClusterID(uint groupId, uint number)
    {
      this.m_uiGroupId = groupId;
      this.m_uiNumber = number;
    }

    public ClusterID(byte[] data, int iOffset)
    {
      this.GroupId = BitConverter.ToUInt32(data, iOffset);
      iOffset += 4;
      this.Number = BitConverter.ToUInt32(data, iOffset);
    }

    public ClusterID(Stream stream)
    {
      this.GroupId = MsoBase.ReadUInt32(stream);
      this.Number = MsoBase.ReadUInt32(stream);
    }

    public uint GroupId
    {
      get => this.m_uiGroupId;
      set => this.m_uiGroupId = value;
    }

    public uint Number
    {
      get => this.m_uiNumber;
      set => this.m_uiNumber = value;
    }

    public static int Size => 8;

    public byte[] GetBytes()
    {
      byte[] bytes = new byte[MsofbtDgg.ClusterID.Size];
      BitConverter.GetBytes(this.m_uiGroupId).CopyTo((Array) bytes, 0);
      BitConverter.GetBytes(this.m_uiNumber).CopyTo((Array) bytes, 4);
      return bytes;
    }

    public void Write(Stream stream)
    {
      MsoBase.WriteUInt32(stream, this.m_uiGroupId);
      MsoBase.WriteUInt32(stream, this.m_uiNumber);
    }

    public object Clone() => (object) (MsofbtDgg.ClusterID) this.MemberwiseClone();
  }
}
