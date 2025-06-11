// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ParagraphPropertyException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ParagraphPropertyException : BaseWordRecord
{
  protected ushort m_usStyleId;
  protected SinglePropertyModifierArray m_arrSprms = new SinglePropertyModifierArray();
  private byte m_bFlags;

  internal ParagraphPropertyException()
  {
  }

  internal ParagraphPropertyException(Stream stream, int iCount, bool isHugePapx)
  {
    this.IsHugePapx = isHugePapx;
    this.Parse(stream, iCount);
  }

  internal ParagraphPropertyException(UniversalPropertyException property)
  {
    byte[] data = property.Data;
    this.Parse(data, 0, data.Length);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_arrSprms == null)
      return;
    this.m_arrSprms = (SinglePropertyModifierArray) null;
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    if (iCount < 2)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    if (iCount + iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException("iCount + iOffset");
    if (!this.IsHugePapx)
    {
      this.m_usStyleId = BitConverter.ToUInt16(arrData, 0);
      iOffset += 2;
      iCount -= 2;
    }
    this.m_arrSprms.Parse(arrData, iOffset, iCount);
  }

  internal override int Save(Stream stream)
  {
    int num = 2;
    stream.Write(BitConverter.GetBytes(this.m_usStyleId), 0, 2);
    if (this.m_arrSprms != null)
      num += this.m_arrSprms.Save(stream);
    return num;
  }

  internal int SaveHugePapx(Stream stream)
  {
    int num = 0;
    if (this.m_arrSprms != null)
      num += this.m_arrSprms.Save(stream);
    return num;
  }

  private bool IsHugePapx
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal ushort StyleIndex
  {
    get => this.m_usStyleId;
    set => this.m_usStyleId = value;
  }

  internal SinglePropertyModifierArray PropertyModifiers
  {
    get => this.m_arrSprms;
    set => this.m_arrSprms = value;
  }

  internal int ModifiersCount => this.m_arrSprms.Count;

  internal override int Length => 2 + this.m_arrSprms.Length;

  internal ushort ParagraphStyleId
  {
    get => this.m_arrSprms.GetUShort(17920, (ushort) 0);
    set
    {
      if ((int) this.ParagraphStyleId == (int) value)
        return;
      this.m_arrSprms.SetUShortValue(17920, value);
    }
  }

  internal ParagraphPropertyException ClonePapx(
    bool stickProperties,
    ParagraphPropertyException papx)
  {
    ParagraphPropertyException propertyException = new ParagraphPropertyException();
    propertyException.PropertyModifiers = papx.PropertyModifiers.Clone();
    propertyException.m_usStyleId = papx.StyleIndex;
    papx = new ParagraphPropertyException();
    if (stickProperties && propertyException != null)
    {
      int sprmIndex = 0;
      for (int modifiersCount = propertyException.ModifiersCount; sprmIndex < modifiersCount; ++sprmIndex)
      {
        SinglePropertyModifierRecord sprmByIndex = propertyException.PropertyModifiers.GetSprmByIndex(sprmIndex);
        if (sprmByIndex.TypedOptions != 54792 && sprmByIndex.TypedOptions != 54789 && sprmByIndex.TypedOptions != 38402 && sprmByIndex.TypedOptions != 38401 && sprmByIndex.TypedOptions != 37895 && sprmByIndex.TypedOptions != 9239 && sprmByIndex.TypedOptions != 9291 && sprmByIndex.TypedOptions != 9292)
        {
          SinglePropertyModifierRecord modifier = propertyException.PropertyModifiers.GetSprmByIndex(sprmIndex).Clone();
          if (modifier != null)
            papx.PropertyModifiers.Add(modifier);
        }
      }
    }
    return propertyException;
  }
}
