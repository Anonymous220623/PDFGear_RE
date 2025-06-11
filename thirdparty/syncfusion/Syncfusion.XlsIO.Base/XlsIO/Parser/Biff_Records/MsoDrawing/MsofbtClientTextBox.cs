// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsofbtClientTextBox
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtClientTextbox)]
public class MsofbtClientTextBox : MsoBase
{
  private List<BiffRecordRaw> m_arrAdditionalData = new List<BiffRecordRaw>(3);

  public TextObjectRecord TextObject
  {
    get
    {
      return this.m_arrAdditionalData.Count <= 0 ? (TextObjectRecord) null : this.m_arrAdditionalData[0] as TextObjectRecord;
    }
    set => this.m_arrAdditionalData.Insert(0, (BiffRecordRaw) value);
  }

  public string Text
  {
    get
    {
      TextObjectRecord textObject = this.TextObject;
      int textLen = textObject != null ? (int) textObject.TextLen : 0;
      int count = this.m_arrAdditionalData.Count;
      if (textLen == 0 || count <= 1)
        return (string) null;
      int num1 = 0;
      int num2 = 1;
      while (num1 < textLen)
      {
        ContinueRecord continueRecord = this.m_arrAdditionalData[num2] as ContinueRecord;
        bool flag = continueRecord.Data[0] != (byte) 0;
        int num3 = continueRecord.Length - 1;
        num1 += flag ? num3 / 2 : num3;
        ++num2;
      }
      return this.CombineAndExtractText(1, num2);
    }
  }

  private string CombineAndExtractText(int startIndex, int afterEndIndex)
  {
    string empty = string.Empty;
    for (int index = startIndex; index < afterEndIndex; ++index)
    {
      ContinueRecord continueRecord = (ContinueRecord) this.m_arrAdditionalData[index];
      byte[] data = continueRecord.Data;
      int length = continueRecord.Length;
      int iStrLen = data[0] != (byte) 0 ? (length - 1) / 2 : length - 1;
      empty += continueRecord.GetString(0, iStrLen);
    }
    return empty;
  }

  public byte[] FormattingRuns
  {
    get
    {
      TextObjectRecord textObject = this.TextObject;
      byte[] dst = (byte[]) null;
      if (textObject != null)
      {
        int formattingRunsLen = (int) this.TextObject.FormattingRunsLen;
        int num = 0;
        int count = this.m_arrAdditionalData.Count;
        int index1 = count - 1;
        while (num < formattingRunsLen)
        {
          BiffRecordRaw biffRecordRaw = this.m_arrAdditionalData[index1];
          num += biffRecordRaw.Length;
          --index1;
        }
        dst = formattingRunsLen > 0 ? new byte[formattingRunsLen] : (byte[]) null;
        int index2 = index1 + 1;
        int dstOffset = 0;
        for (; index2 < count; ++index2)
        {
          BiffRecordRaw biffRecordRaw = this.m_arrAdditionalData[index2];
          int length = biffRecordRaw.Length;
          Buffer.BlockCopy((Array) biffRecordRaw.Data, 0, (Array) dst, dstOffset, length);
          dstOffset += length;
        }
      }
      return dst;
    }
  }

  public BiffRecordRaw[] AdditionalData
  {
    get
    {
      return this.m_arrAdditionalData == null ? (BiffRecordRaw[]) null : this.m_arrAdditionalData.ToArray();
    }
  }

  public MsofbtClientTextBox(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtClientTextBox(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsofbtClientTextBox(
    MsoBase parent,
    byte[] data,
    int iOffset,
    GetNextMsoDrawingData dataGetter)
    : base(parent, data, iOffset, dataGetter)
  {
    BiffRecordRaw[] collection = dataGetter();
    if (collection == null)
      throw new ArgumentException("Additional data can't be null");
    this.m_arrAdditionalData.Clear();
    this.m_arrAdditionalData.AddRange((IEnumerable<BiffRecordRaw>) collection);
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_iLength = 0;
    if (arrBreaks == null || arrRecords == null)
      return;
    arrBreaks.Add(this.m_iLength + iOffset);
    arrRecords.Add(this.m_arrAdditionalData);
  }

  public override void ParseStructure(Stream stream)
  {
  }

  protected override object InternalClone()
  {
    MsofbtClientTextBox msofbtClientTextBox = (MsofbtClientTextBox) base.InternalClone();
    msofbtClientTextBox.m_arrAdditionalData = CloneUtils.CloneCloneable(this.m_arrAdditionalData);
    return (object) msofbtClientTextBox;
  }

  public override void UpdateNextMsoDrawingData()
  {
    BiffRecordRaw[] collection = this.DataGetter();
    if (collection == null)
      throw new ArgumentException("Additional data can't be null");
    this.m_arrAdditionalData.Clear();
    this.m_arrAdditionalData.AddRange((IEnumerable<BiffRecordRaw>) collection);
  }

  public void AddRecord(BiffRecordRaw record) => this.m_arrAdditionalData.Add(record);
}
