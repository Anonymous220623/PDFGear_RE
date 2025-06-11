// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.TransformInfoHeader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class TransformInfoHeader
{
  private int m_iTransformType = 1;
  private string m_strTransformId;
  private string m_strTransformName;
  private int m_iReaderVersion = 1;
  private int m_iUpdaterVersion = 1;
  private int m_iWriterVersion = 1;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal int TransformType
  {
    get => this.m_iTransformType;
    set => this.m_iTransformType = value;
  }

  internal string TransformId
  {
    get => this.m_strTransformId;
    set => this.m_strTransformId = value;
  }

  internal string TransformName
  {
    get => this.m_strTransformName;
    set => this.m_strTransformName = value;
  }

  internal int ReaderVersion
  {
    get => this.m_iReaderVersion;
    set => this.m_iReaderVersion = value;
  }

  internal int UpdaterVersion
  {
    get => this.m_iUpdaterVersion;
    set => this.m_iUpdaterVersion = value;
  }

  internal int WriterVersion
  {
    get => this.m_iWriterVersion;
    set => this.m_iWriterVersion = value;
  }

  internal TransformInfoHeader()
  {
  }

  internal TransformInfoHeader(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[4];
    this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iTransformType = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_strTransformId = this.m_securityHelper.ReadUnicodeString(stream);
    this.m_strTransformName = this.m_securityHelper.ReadUnicodeString(stream);
    this.m_iReaderVersion = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iUpdaterVersion = this.m_securityHelper.ReadInt32(stream, buffer);
    this.m_iWriterVersion = this.m_securityHelper.ReadInt32(stream, buffer);
  }

  internal void Serialize(Stream stream)
  {
    long num1 = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    stream.Position += 4L;
    this.m_securityHelper.WriteInt32(stream, this.m_iTransformType);
    this.m_securityHelper.WriteUnicodeString(stream, this.m_strTransformId);
    long position = stream.Position;
    int num2 = (int) (position - num1);
    stream.Position = num1;
    this.m_securityHelper.WriteInt32(stream, num2);
    stream.Position = position;
    this.m_securityHelper.WriteUnicodeString(stream, this.m_strTransformName);
    this.m_securityHelper.WriteInt32(stream, this.m_iReaderVersion);
    this.m_securityHelper.WriteInt32(stream, this.m_iUpdaterVersion);
    this.m_securityHelper.WriteInt32(stream, this.m_iWriterVersion);
  }
}
