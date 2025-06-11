// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.SttbfAssoc
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class SttbfAssoc
{
  private ushort m_fExtend = ushort.MaxValue;
  private ushort m_cData = 18;
  private ushort m_cbExtra;
  private string m_template;
  private string m_title;
  private string m_subject;
  private string m_keyWords;
  private string m_author;
  private string m_lastModifiedBy;
  private string m_dataSource;
  private string m_headerDocument;
  private string m_writePassword;

  internal string AttachedTemplate
  {
    get => this.m_template;
    set => this.m_template = value;
  }

  internal string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal string Subject
  {
    get => this.m_subject;
    set => this.m_subject = value;
  }

  internal string KeyWords
  {
    get => this.m_keyWords;
    set => this.m_keyWords = value;
  }

  internal string Author
  {
    get => this.m_author;
    set => this.m_author = value;
  }

  internal string LastModifiedBy
  {
    get => this.m_lastModifiedBy;
    set => this.m_lastModifiedBy = value;
  }

  internal string MailMergeDataSource
  {
    get => this.m_dataSource;
    set => this.m_dataSource = value;
  }

  internal string MailMergeHeaderDocument
  {
    get => this.m_headerDocument;
    set => this.m_headerDocument = value;
  }

  internal string WritePassword
  {
    get => this.m_writePassword;
    set => this.m_writePassword = value;
  }

  internal SttbfAssoc()
  {
  }

  internal void Parse(byte[] associatedStrings)
  {
    if (associatedStrings.Length < 42)
      return;
    int iOffset1 = 0;
    this.m_fExtend = BaseWordRecord.ReadUInt16(associatedStrings, iOffset1);
    int iOffset2 = iOffset1 + 2;
    this.m_cData = BaseWordRecord.ReadUInt16(associatedStrings, iOffset2);
    int iOffset3 = iOffset2 + 2;
    this.m_cbExtra = BaseWordRecord.ReadUInt16(associatedStrings, iOffset3);
    int iOffset4 = iOffset3 + 2;
    ushort num1 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset4);
    int iOffset5 = iOffset4 + 2 + (int) num1 * 2;
    ushort num2 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset5);
    int iOffset6 = iOffset5 + 2;
    this.m_template = BaseWordRecord.ReadString(associatedStrings, iOffset6, (ushort) ((uint) num2 * 2U));
    int iOffset7 = iOffset6 + (int) num2 * 2;
    ushort num3 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset7);
    int iOffset8 = iOffset7 + 2;
    this.m_title = BaseWordRecord.ReadString(associatedStrings, iOffset8, (ushort) ((uint) num3 * 2U));
    int iOffset9 = iOffset8 + (int) num3 * 2;
    ushort num4 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset9);
    int iOffset10 = iOffset9 + 2;
    this.m_subject = BaseWordRecord.ReadString(associatedStrings, iOffset10, (ushort) ((uint) num4 * 2U));
    int iOffset11 = iOffset10 + (int) num4 * 2;
    ushort num5 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset11);
    int iOffset12 = iOffset11 + 2;
    this.m_keyWords = BaseWordRecord.ReadString(associatedStrings, iOffset12, (ushort) ((uint) num5 * 2U));
    int iOffset13 = iOffset12 + (int) num5 * 2;
    ushort num6 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset13);
    int iOffset14 = iOffset13 + 2 + (int) num6 * 2;
    ushort num7 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset14);
    int iOffset15 = iOffset14 + 2;
    this.m_author = BaseWordRecord.ReadString(associatedStrings, iOffset15, (ushort) ((uint) num7 * 2U));
    int iOffset16 = iOffset15 + (int) num7 * 2;
    ushort num8 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset16);
    int iOffset17 = iOffset16 + 2;
    this.m_lastModifiedBy = BaseWordRecord.ReadString(associatedStrings, iOffset17, (ushort) ((uint) num8 * 2U));
    int iOffset18 = iOffset17 + (int) num8 * 2;
    ushort num9 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset18);
    int iOffset19 = iOffset18 + 2;
    this.m_dataSource = BaseWordRecord.ReadString(associatedStrings, iOffset19, (ushort) ((uint) num9 * 2U));
    int iOffset20 = iOffset19 + (int) num9 * 2;
    ushort num10 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset20);
    int iOffset21 = iOffset20 + 2;
    this.m_headerDocument = BaseWordRecord.ReadString(associatedStrings, iOffset21, (ushort) ((uint) num10 * 2U));
    int iOffset22 = iOffset21 + (int) num10 * 2;
    ushort num11 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset22);
    int iOffset23 = iOffset22 + 2 + (int) num11 * 2;
    ushort num12 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset23);
    int iOffset24 = iOffset23 + 2 + (int) num12 * 2;
    ushort num13 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset24);
    int iOffset25 = iOffset24 + 2 + (int) num13 * 2;
    ushort num14 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset25);
    int iOffset26 = iOffset25 + 2 + (int) num14 * 2;
    ushort num15 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset26);
    int iOffset27 = iOffset26 + 2 + (int) num15 * 2;
    ushort num16 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset27);
    int iOffset28 = iOffset27 + 2 + (int) num16 * 2;
    ushort num17 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset28);
    int iOffset29 = iOffset28 + 2 + (int) num17 * 2;
    ushort num18 = BaseWordRecord.ReadUInt16(associatedStrings, iOffset29);
    int iOffset30 = iOffset29 + 2;
    if (num18 > (ushort) 0)
      this.m_writePassword = BaseWordRecord.ReadString(associatedStrings, iOffset30, (ushort) ((uint) num18 * 2U));
    int num19 = iOffset30 + (int) num18 * 2;
  }

  internal byte[] GetAssociatedStrings()
  {
    MemoryStream memoryStream = new MemoryStream();
    BaseWordRecord.WriteUInt16((Stream) memoryStream, this.m_fExtend);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, this.m_cData);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, this.m_cbExtra);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    if (string.IsNullOrEmpty(this.m_template))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_template);
    if (string.IsNullOrEmpty(this.m_title))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_title);
    if (string.IsNullOrEmpty(this.m_subject))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_subject);
    if (string.IsNullOrEmpty(this.m_keyWords))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_keyWords);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    if (string.IsNullOrEmpty(this.m_author))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_author);
    if (string.IsNullOrEmpty(this.m_lastModifiedBy))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_lastModifiedBy);
    if (string.IsNullOrEmpty(this.m_dataSource))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_dataSource);
    if (string.IsNullOrEmpty(this.m_headerDocument))
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    else
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_headerDocument);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    if (string.IsNullOrEmpty(this.m_writePassword))
    {
      BaseWordRecord.WriteUInt16((Stream) memoryStream, (ushort) 0);
    }
    else
    {
      if (this.m_writePassword.Length > 15)
        this.m_writePassword = this.m_writePassword.Substring(0, 15);
      BaseWordRecord.WriteString((Stream) memoryStream, this.m_writePassword);
    }
    byte[] dst = new byte[memoryStream.Length];
    Buffer.BlockCopy((Array) memoryStream.GetBuffer(), 0, (Array) dst, 0, (int) memoryStream.Length);
    return dst;
  }
}
