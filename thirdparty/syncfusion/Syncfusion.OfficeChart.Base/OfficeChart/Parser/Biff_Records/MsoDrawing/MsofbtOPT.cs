// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtOPT
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtOPT)]
internal class MsofbtOPT : MsoBase, ICloneable, IFopteOptionWrapper
{
  private const int DEF_MINOPTION_INDEX = 127 /*0x7F*/;
  private List<MsofbtOPT.FOPTE> m_arrProperties = new List<MsofbtOPT.FOPTE>();

  public MsofbtOPT(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtOPT(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void ParseStructure(Stream stream)
  {
    int num = 0;
    for (int iLength = this.m_iLength; num < iLength; num += MsofbtOPT.FOPTE.Size)
    {
      MsofbtOPT.FOPTE option = new MsofbtOPT.FOPTE(stream);
      this.AddOptions(option);
      if (option.IsComplex)
        iLength -= (int) option.UInt32Value;
    }
    int index = 0;
    for (int count = this.m_arrProperties.Count; index < count; ++index)
      this.m_arrProperties[index].ReadComplexData(stream);
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    if (this.Instance == 0)
      this.m_usVersionAndInst = (ushort) 51;
    int count1 = this.m_arrProperties.Count;
    if (count1 > 0)
    {
      int id1 = (int) this.m_arrProperties[0].Id;
      int num1 = id1 > 0 ? 1 : 0;
      int num2 = id1;
      bool flag = true;
      for (int index = 1; index < count1; ++index)
      {
        MsofbtOPT.FOPTE arrProperty = this.m_arrProperties[index];
        if (arrProperty.Id > (MsoOptions) id1)
        {
          id1 = (int) arrProperty.Id;
          ++num1;
        }
        else
          flag = false;
      }
      int count2 = this.m_arrProperties.Count;
      if (num2 <= 4)
      {
        int id2 = (int) this.m_arrProperties[count2 - 1].Id;
        if ((id2 > 1000 || num1 != this.m_arrProperties.Count) && num1 > 10 && id2 > 100 && flag)
          --num1;
      }
      this.Instance = num1;
    }
    this.m_iLength = 0;
    for (int index = 0; index < count1; ++index)
    {
      byte[] mainData = this.m_arrProperties[index].MainData;
      int length = mainData.Length;
      stream.Write(mainData, 0, length);
      this.m_iLength += length;
    }
    for (int index = 0; index < count1; ++index)
    {
      MsofbtOPT.FOPTE arrProperty = this.m_arrProperties[index];
      if (arrProperty.AdditionalData != null)
      {
        byte[] additionalData = arrProperty.AdditionalData;
        int length = additionalData.Length;
        stream.Write(additionalData, 0, length);
        this.m_iLength += length;
      }
    }
  }

  public override object Clone() => this.InternalClone();

  protected override object InternalClone()
  {
    MsofbtOPT msofbtOpt = (MsofbtOPT) base.InternalClone();
    msofbtOpt.m_arrProperties = CloneUtils.CloneCloneable<MsofbtOPT.FOPTE>(this.m_arrProperties);
    return (object) msofbtOpt;
  }

  public MsofbtOPT.FOPTE[] Properties => this.m_arrProperties.ToArray();

  public MsofbtOPT.FOPTE this[int index]
  {
    get
    {
      if (index < 0 || index >= this.m_arrProperties.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than than Count - 1.");
      return this.m_arrProperties[index];
    }
  }

  public IList<MsofbtOPT.FOPTE> PropertyList => (IList<MsofbtOPT.FOPTE>) this.m_arrProperties;

  public void AddOptions(MsofbtOPT.FOPTE option) => this.m_arrProperties.Add(option);

  public void AddOptions(ICollection options)
  {
    foreach (MsofbtOPT.FOPTE option in (IEnumerable) options)
      this.AddOptions(option);
  }

  public void AddOptionsOrReplace(MsofbtOPT.FOPTE option)
  {
    int index = this.IndexOf(option);
    if (index == this.m_arrProperties.Count)
      this.m_arrProperties.Add(option);
    else
      this.m_arrProperties[index] = option;
  }

  public void AddOptionSorted(MsofbtOPT.FOPTE option)
  {
    int index = 0;
    int count = this.m_arrProperties.Count;
    MsoOptions id = option.Id;
    int num = count;
    while (index < num && this.m_arrProperties[index].Id < id)
      ++index;
    if (index < count)
    {
      if (this.m_arrProperties[index].Id == id)
        this.m_arrProperties[index] = option;
      else
        this.m_arrProperties.Insert(index, option);
    }
    else
      this.m_arrProperties.Add(option);
  }

  private int IndexOf(MsofbtOPT.FOPTE option) => this.IndexOf(option.Id);

  public void RemoveOption(int index)
  {
    int index1 = 0;
    for (int count = this.m_arrProperties.Count; index1 < count; ++index1)
    {
      if (this.m_arrProperties[index1].Id == (MsoOptions) index)
      {
        this.m_arrProperties.RemoveAt(index1);
        break;
      }
    }
  }

  public int IndexOf(MsoOptions optionId)
  {
    int index = 0;
    int count = this.m_arrProperties.Count;
    while (index < count && this.m_arrProperties[index].Id != optionId)
      ++index;
    return index;
  }

  public class FOPTE : ICloneable
  {
    private const ushort DEF_ID_MASK = 16383 /*0x3FFF*/;
    private const ushort DEF_VALID_MASK = 16384 /*0x4000*/;
    private const ushort DEF_COMPLEX_MASK = 32768 /*0x8000*/;
    private const int DEF_RECORD_SIZE = 6;
    private ushort m_usId;
    private bool m_bIdValid;
    private bool m_bComplex;
    private uint m_uiValue;
    private byte[] m_arrData;

    public MsoOptions Id
    {
      get => (MsoOptions) this.m_usId;
      set => this.m_usId = (ushort) value;
    }

    public bool IsValid
    {
      get => this.m_bIdValid;
      set => this.m_bIdValid = value;
    }

    public bool IsComplex
    {
      get => this.m_bComplex;
      set => this.m_bComplex = value;
    }

    public uint UInt32Value
    {
      get => this.m_uiValue;
      set => this.m_uiValue = value;
    }

    public int Int32Value
    {
      get => (int) this.m_uiValue;
      set => this.m_uiValue = (uint) value;
    }

    public byte[] AdditionalData
    {
      get => this.m_arrData;
      set => this.m_arrData = value;
    }

    public byte[] MainData
    {
      get
      {
        byte[] mainData = new byte[MsofbtOPT.FOPTE.Size];
        ushort num = (ushort) ((uint) this.m_usId & 16383U /*0x3FFF*/);
        if (this.m_bIdValid)
          num += (ushort) 16384 /*0x4000*/;
        if (this.m_bComplex)
          num += (ushort) 32768 /*0x8000*/;
        BitConverter.GetBytes(num).CopyTo((Array) mainData, 0);
        BitConverter.GetBytes(this.m_uiValue).CopyTo((Array) mainData, 2);
        return mainData;
      }
    }

    public static int Size => 6;

    public FOPTE()
    {
    }

    public FOPTE(byte[] data, ref int iOffset)
    {
      ushort uint16 = BitConverter.ToUInt16(data, iOffset);
      this.m_usId = (ushort) ((uint) uint16 & 16383U /*0x3FFF*/);
      this.m_bIdValid = ((int) uint16 & 16384 /*0x4000*/) != 0;
      this.m_bComplex = ((int) uint16 & 32768 /*0x8000*/) != 0;
      iOffset += 2;
      this.m_uiValue = BitConverter.ToUInt32(data, iOffset);
      iOffset += 4;
    }

    public FOPTE(Stream stream)
    {
      ushort num = MsoBase.ReadUInt16(stream);
      this.m_usId = (ushort) ((uint) num & 16383U /*0x3FFF*/);
      this.m_bIdValid = ((int) num & 16384 /*0x4000*/) != 0;
      this.m_bComplex = ((int) num & 32768 /*0x8000*/) != 0;
      this.m_uiValue = MsoBase.ReadUInt32(stream);
    }

    public void ReadComplexData(byte[] m_data, ref int iOffset)
    {
      if (!this.IsComplex)
        return;
      this.m_arrData = new byte[(IntPtr) this.UInt32Value];
      Array.Copy((Array) m_data, iOffset, (Array) this.m_arrData, 0, (int) this.UInt32Value);
      iOffset += (int) this.UInt32Value;
    }

    public void ReadComplexData(Stream stream)
    {
      if (!this.IsComplex)
        return;
      int uint32Value = (int) this.UInt32Value;
      this.m_arrData = new byte[uint32Value];
      stream.Read(this.m_arrData, 0, uint32Value);
    }

    public object Clone()
    {
      MsofbtOPT.FOPTE fopte = (MsofbtOPT.FOPTE) this.MemberwiseClone();
      if (this.m_arrData != null)
        fopte.m_arrData = CloneUtils.CloneByteArray(this.m_arrData);
      return (object) fopte;
    }
  }
}
