// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ExternSheetRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ExternSheet)]
internal class ExternSheetRecord : BiffRecordRawWithArray
{
  private const int DEF_FIXED_PART_SIZE = 2;
  public const int MaximumRefsCount = 1370;
  [BiffRecordPos(0, 2)]
  private ushort m_usRefCount;
  private List<ExternSheetRecord.TREF> m_arrRef;
  private ushort m_cXTI;

  public ushort RefCount
  {
    get => this.m_usRefCount;
    set => this.m_usRefCount = value;
  }

  public ExternSheetRecord.TREF[] Refs
  {
    get => this.m_arrRef == null ? (ExternSheetRecord.TREF[]) null : this.m_arrRef.ToArray();
    set
    {
      this.m_arrRef = new List<ExternSheetRecord.TREF>();
      this.m_arrRef.AddRange((IEnumerable<ExternSheetRecord.TREF>) value);
      this.m_usRefCount = (ushort) this.m_arrRef.Count;
    }
  }

  public List<ExternSheetRecord.TREF> RefList => this.m_arrRef;

  public override int MinimumRecordSize => 2;

  public ExternSheetRecord()
  {
  }

  public ExternSheetRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExternSheetRecord(int iReserve)
    : base(iReserve)
  {
  }

  public int AddReference(int supIndex, int firstSheet, int lastSheet)
  {
    if (this.m_arrRef != null)
    {
      int count = this.m_arrRef.Count;
      for (int index = 0; index < count; ++index)
      {
        ExternSheetRecord.TREF tref = this.m_arrRef[index];
        if ((int) tref.SupBookIndex == supIndex && (int) tref.FirstSheet == firstSheet && (int) tref.LastSheet == lastSheet)
          return index;
      }
    }
    this.AppendReference(new ExternSheetRecord.TREF(supIndex, firstSheet, lastSheet));
    return (int) this.m_usRefCount - 1;
  }

  public int GetBookReference(int iBookIndex)
  {
    int index = 0;
    for (int count = this.m_arrRef.Count; index < count; ++index)
    {
      if ((int) this.m_arrRef[index].SupBookIndex == iBookIndex)
        return index;
    }
    return -1;
  }

  public void AppendReference(ExternSheetRecord.TREF reference)
  {
    if (this.m_arrRef == null)
      this.m_arrRef = new List<ExternSheetRecord.TREF>();
    this.m_arrRef.Add(reference);
    ++this.m_usRefCount;
  }

  public void AppendReferences(IList<ExternSheetRecord.TREF> refs)
  {
    this.m_arrRef.AddRange((IEnumerable<ExternSheetRecord.TREF>) refs);
    this.m_usRefCount += (ushort) refs.Count;
  }

  public void PrependReferences(IList<ExternSheetRecord.TREF> refs)
  {
    this.m_arrRef.InsertRange(0, (IEnumerable<ExternSheetRecord.TREF>) refs);
    this.m_usRefCount += (ushort) refs.Count;
  }

  public override object Clone()
  {
    ExternSheetRecord externSheetRecord = (ExternSheetRecord) base.Clone();
    externSheetRecord.m_arrRef = CloneUtils.CloneCloneable<ExternSheetRecord.TREF>(this.m_arrRef);
    return (object) externSheetRecord;
  }

  public override void ParseStructure()
  {
    this.m_usRefCount = BitConverter.ToUInt16(this.m_data, 0);
    int iLength = this.m_iLength;
    int num1 = (int) this.m_usRefCount * 6 + 2;
    this.m_arrRef = new List<ExternSheetRecord.TREF>((int) this.m_usRefCount);
    int num2 = 0;
    int offset = 2;
    while (num2 < (int) this.m_usRefCount)
    {
      if (offset >= this.m_data.Length)
      {
        this.m_cXTI = this.m_usRefCount;
        this.m_usRefCount = (ushort) num2;
        break;
      }
      this.m_arrRef.Add(new ExternSheetRecord.TREF((int) this.GetUInt16(offset), (int) this.GetUInt16(offset + 2), (int) this.GetUInt16(offset + 4)));
      ++num2;
      offset += 6;
    }
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(OfficeVersion.Excel97to2003);
    this.m_data = new byte[this.m_iLength];
    if (this.m_cXTI != (ushort) 0)
      this.SetUInt16(0, this.m_cXTI);
    else
      this.SetUInt16(0, this.m_usRefCount);
    if (this.m_arrRef == null)
      return;
    int index = 0;
    int offset = 2;
    int count = this.m_arrRef.Count;
    while (index < count)
    {
      this.SetUInt16(offset, this.m_arrRef[index].SupBookIndex);
      this.SetUInt16(offset + 2, this.m_arrRef[index].FirstSheet);
      this.SetUInt16(offset + 4, this.m_arrRef[index].LastSheet);
      ++index;
      offset += 6;
    }
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 2 + (this.m_arrRef == null ? 0 : this.m_arrRef.Count * 6);
  }

  public class TREF
  {
    public const int DEF_TREF_SIZE = 6;
    private ushort m_usSupBookIndex;
    private ushort m_usFirstSheet;
    private ushort m_usLastSheet;

    public ushort SupBookIndex
    {
      get => this.m_usSupBookIndex;
      set => this.m_usSupBookIndex = value;
    }

    public ushort FirstSheet
    {
      get => this.m_usFirstSheet;
      set => this.m_usFirstSheet = value;
    }

    public ushort LastSheet
    {
      get => this.m_usLastSheet;
      set => this.m_usLastSheet = value;
    }

    public TREF(int supIndex, int firstSheet, int lastSheet)
    {
      this.FirstSheet = (ushort) firstSheet;
      this.LastSheet = (ushort) lastSheet;
      this.SupBookIndex = (ushort) supIndex;
    }
  }
}
