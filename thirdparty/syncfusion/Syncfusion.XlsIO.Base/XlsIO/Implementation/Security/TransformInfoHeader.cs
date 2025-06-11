// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.TransformInfoHeader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class TransformInfoHeader
{
  private int m_iTransformType = 1;
  private string m_strTransformId;
  private string m_strTransformName;
  private int m_iReaderVersion = 1;
  private int m_iUpdaterVersion = 1;
  private int m_iWriterVersion = 1;

  public int TransformType
  {
    get => this.m_iTransformType;
    set => this.m_iTransformType = value;
  }

  public string TransformId
  {
    get => this.m_strTransformId;
    set => this.m_strTransformId = value;
  }

  public string TransformName
  {
    get => this.m_strTransformName;
    set => this.m_strTransformName = value;
  }

  public int ReaderVersion
  {
    get => this.m_iReaderVersion;
    set => this.m_iReaderVersion = value;
  }

  public int UpdaterVersion
  {
    get => this.m_iUpdaterVersion;
    set => this.m_iUpdaterVersion = value;
  }

  public int WriterVersion
  {
    get => this.m_iWriterVersion;
    set => this.m_iWriterVersion = value;
  }

  public TransformInfoHeader()
  {
  }

  public TransformInfoHeader(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException("steram");
    byte[] buffer = new byte[4];
    SecurityHelper.ReadInt32(stream, buffer);
    this.m_iTransformType = SecurityHelper.ReadInt32(stream, buffer);
    this.m_strTransformId = SecurityHelper.ReadUnicodeStringP4(stream);
    this.m_strTransformName = SecurityHelper.ReadUnicodeStringP4(stream);
    this.m_iReaderVersion = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iUpdaterVersion = SecurityHelper.ReadInt32(stream, buffer);
    this.m_iWriterVersion = SecurityHelper.ReadInt32(stream, buffer);
  }

  public void Serialize(Stream stream)
  {
    long num1 = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    stream.Position += 4L;
    SecurityHelper.WriteInt32(stream, this.m_iTransformType);
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strTransformId);
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strTransformName);
    SecurityHelper.WriteInt32(stream, this.m_iReaderVersion);
    SecurityHelper.WriteInt32(stream, this.m_iUpdaterVersion);
    SecurityHelper.WriteInt32(stream, this.m_iWriterVersion);
    long position = stream.Position;
    int num2 = (int) (position - num1);
    stream.Position = num1;
    SecurityHelper.WriteInt32(stream, num2);
    stream.Position = position;
    SecurityHelper.WriteUnicodeStringP4(stream, this.m_strTransformName);
    SecurityHelper.WriteInt32(stream, this.m_iReaderVersion);
    SecurityHelper.WriteInt32(stream, this.m_iUpdaterVersion);
    SecurityHelper.WriteInt32(stream, this.m_iWriterVersion);
  }
}
