// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.DocumentPropertyCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

public class DocumentPropertyCollection
{
  private const int ByteOrder = 65534;
  private static readonly Guid FirstSectionGuid = new Guid("f29f85e0-4ff9-1068-ab91-08002b27b3d9");
  private int m_iFirstSectionOffset = -1;
  private List<PropertySection> m_lstSections = new List<PropertySection>();

  public List<PropertySection> Sections => this.m_lstSections;

  public DocumentPropertyCollection()
  {
  }

  public DocumentPropertyCollection(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    stream.Position = 0L;
    this.ReadHeader(stream);
    this.ParseSections(stream);
  }

  private void ParseSections(Stream stream)
  {
    int index = 0;
    for (int count = this.m_lstSections.Count; index < count; ++index)
      this.m_lstSections[index].Parse(stream);
  }

  private void ReadHeader(Stream stream)
  {
    byte[] numArray = new byte[16 /*0x10*/];
    stream.Read(numArray, 0, 4);
    int int32_1 = BitConverter.ToInt32(numArray, 0);
    if (int32_1 != 65534)
      throw new IOException($"iValue = {int32_1} instead of {65534}");
    stream.Read(numArray, 0, 2);
    stream.Read(numArray, 0, 2);
    stream.Read(numArray, 0, 16 /*0x10*/);
    stream.Read(numArray, 0, 4);
    int int32_2 = BitConverter.ToInt32(numArray, 0);
    for (int index = 0; index < int32_2; ++index)
    {
      stream.Read(numArray, 0, 16 /*0x10*/);
      this.m_lstSections.Add(new PropertySection(new Guid(numArray), StreamHelper.ReadInt32(stream, numArray)));
    }
  }

  private void WriteSections(Stream stream)
  {
    int index = 0;
    for (int count = this.m_lstSections.Count; index < count; ++index)
      this.m_lstSections[index].Serialize(stream);
  }

  private void WriteHeader(Stream stream)
  {
    StreamHelper.WriteInt32(stream, 65534);
    StreamHelper.WriteInt16(stream, (short) 261);
    StreamHelper.WriteInt16(stream, (short) 2);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      stream.WriteByte((byte) 0);
    int count = this.m_lstSections.Count;
    StreamHelper.WriteInt32(stream, count);
    List<long> longList = new List<long>();
    for (int index = 0; index < count; ++index)
    {
      byte[] byteArray = this.m_lstSections[index].Id.ToByteArray();
      stream.Write(byteArray, 0, byteArray.Length);
      longList.Add(stream.Position);
      StreamHelper.WriteInt32(stream, 0);
    }
    for (int index = 0; index < count; ++index)
    {
      PropertySection lstSection = this.m_lstSections[index];
      long position = stream.Position;
      stream.Position = longList[index];
      StreamHelper.WriteInt32(stream, (int) position);
      stream.Position = position;
      lstSection.Serialize(stream);
    }
  }

  public void Serialize(Stream stream) => this.WriteHeader(stream);
}
