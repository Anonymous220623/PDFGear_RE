// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.CompoundStream
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO;

public abstract class CompoundStream : Stream
{
  private string m_strStreamName;

  public CompoundStream(string streamName) => this.m_strStreamName = streamName;

  public virtual void CopyTo(CompoundStream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] buffer = new byte[32768 /*0x8000*/];
    long position = this.Position;
    int count;
    while ((count = this.Read(buffer, 0, 32768 /*0x8000*/)) > 0)
      stream.Write(buffer, 0, count);
  }

  public string Name
  {
    get => this.m_strStreamName;
    protected set => this.m_strStreamName = value;
  }
}
