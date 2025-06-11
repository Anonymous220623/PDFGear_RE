// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffRecordRawWithDataProvider
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public abstract class BiffRecordRawWithDataProvider : BiffRecordWithStreamPos, IDisposable
{
  protected DataProvider m_provider;

  ~BiffRecordRawWithDataProvider() => this.Dispose();

  public abstract void ParseStructure();

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_provider = ApplicationImpl.CreateDataProvider();
    this.m_provider.EnsureCapacity(iLength);
    this.m_iLength = iLength;
    provider.CopyTo(iOffset, this.m_provider, 0, iLength);
    this.ParseStructure();
    if (this.NeedDataArray)
      return;
    this.m_provider.Clear();
    this.AutoGrowData = true;
  }

  public abstract void InfillInternalData(ExcelVersion version);

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    this.InfillInternalData(version);
    if (this.m_iLength <= 0)
      return;
    this.m_provider.CopyTo(0, provider, iOffset, this.m_iLength);
  }

  public override object Clone()
  {
    BiffRecordRawWithDataProvider withDataProvider = (BiffRecordRawWithDataProvider) base.Clone();
    IntPtr heapHandle = IntPtr.Zero;
    if (this.m_provider is IntPtrDataProvider)
      heapHandle = (this.m_provider as IntPtrDataProvider).HeapHandle;
    withDataProvider.m_provider = heapHandle != IntPtr.Zero ? ApplicationImpl.CreateDataProvider(heapHandle) : ApplicationImpl.CreateDataProvider();
    return (object) withDataProvider;
  }

  protected internal string GetString(int offset, int iStrLen)
  {
    return this.GetString(offset, iStrLen, out int _);
  }

  protected internal string GetString(int offset, int iStrLen, out int iBytesInString)
  {
    return this.GetString(offset, iStrLen, out iBytesInString, false);
  }

  protected internal string GetString(
    int offset,
    int iStrLen,
    out int iBytesInString,
    bool isByteCounted)
  {
    return this.m_provider.ReadString(offset, iStrLen, out iBytesInString, isByteCounted);
  }

  protected string GetUnkTypeString(
    int offset,
    IList<int> continuePos,
    int continueCount,
    ref int iBreakIndex,
    out int length,
    out byte[] rich,
    out byte[] extended)
  {
    string str1 = (string) null;
    int num1 = 3;
    rich = (byte[]) null;
    extended = (byte[]) null;
    int iOffset = offset;
    ushort num2 = this.m_provider.ReadUInt16(iOffset);
    byte num3 = this.m_provider.ReadByte(iOffset + 2);
    bool isUnicode = ((int) num3 & 1) == 1;
    bool flag1 = ((int) num3 & 4) != 0;
    bool flag2 = ((int) num3 & 8) != 0;
    int num4 = 3;
    short num5 = 0;
    int size1 = 0;
    if (flag2)
    {
      num5 = this.m_provider.ReadInt16(iOffset + num4);
      num4 += 2;
      num1 += 2;
    }
    if (flag1)
    {
      size1 = this.m_provider.ReadInt32(iOffset + num4);
      num4 += 4;
      num1 += 4;
    }
    int num6 = iOffset + num4;
    int num7 = 0;
    Encoding encoding = isUnicode ? Encoding.Unicode : BiffRecordRaw.LatinEncoding;
    while (num7 < (int) num2)
    {
      int stringLength1 = isUnicode ? ((int) num2 - num7) * 2 : (int) num2 - num7;
      int stringLength2 = BiffRecordRaw.FindNextBreak(continuePos, continueCount, num6, ref iBreakIndex) - num6;
      if (stringLength1 <= stringLength2)
      {
        string str2 = this.m_provider.ReadString(num6, stringLength1, encoding, isUnicode);
        str1 = str1 == null ? str2 : str1 + str2;
        num1 += stringLength1;
        break;
      }
      string str3 = this.m_provider.ReadString(num6, stringLength2, encoding, isUnicode);
      str1 = str1 == null ? str3 : str1 + str3;
      num7 += isUnicode ? stringLength2 / 2 : stringLength2;
      byte num8 = this.m_provider.ReadByte(num6 + stringLength2);
      switch (num8)
      {
        case 0:
        case 1:
          isUnicode = num8 == (byte) 1;
          encoding = isUnicode ? Encoding.Unicode : BiffRecordRaw.LatinEncoding;
          ++num6;
          ++num1;
          break;
      }
      num6 += stringLength2;
      num1 += stringLength2;
    }
    if (flag2)
    {
      int size2 = (int) num5 * 4;
      rich = new byte[size2];
      this.m_provider.ReadArray(offset + num1, rich, size2);
      num1 += size2;
    }
    if (flag1)
    {
      extended = new byte[size1];
      this.m_provider.ReadArray(offset + num1, extended, size1);
      num1 += size1;
    }
    length = num1;
    return str1 ?? string.Empty;
  }

  protected internal void SetByte(int offset, byte value)
  {
    this.m_provider.WriteByte(offset, value);
  }

  protected internal void SetUInt16(int offset, ushort value)
  {
    this.m_provider.WriteUInt16(offset, value);
  }

  protected internal void SetBytes(int offset, byte[] value, int pos, int length)
  {
    this.m_provider.WriteBytes(offset, value, pos, length);
  }

  protected internal void SetBytes(int offset, byte[] value)
  {
    this.SetBytes(offset, value, 0, value.Length);
  }

  protected internal int SetStringNoLen(int offset, string value)
  {
    return this.SetStringNoLen(offset, value, false);
  }

  protected internal int SetStringNoLen(int offset, string value, bool bEmptyCompressed)
  {
    if (value == null || value.Length == 0)
    {
      if (!bEmptyCompressed)
        return 0;
      if (this.AutoGrowData)
        this.m_provider.EnsureCapacity(offset);
      this.m_provider.WriteByte(offset, (byte) 0);
      return 1;
    }
    byte[] bytes = Encoding.Unicode.GetBytes(value);
    if (this.AutoGrowData)
      this.m_provider.EnsureCapacity(offset + bytes.Length);
    this.m_provider.WriteByte(offset, (byte) 1);
    this.m_provider.WriteBytes(offset + 1, bytes, 0, bytes.Length);
    return bytes.Length + 1;
  }

  public void Dispose()
  {
    if (this.m_provider != null)
    {
      this.m_provider.Dispose();
      this.m_provider = (DataProvider) null;
    }
    GC.SuppressFinalize((object) this);
  }
}
