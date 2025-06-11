// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ProgressEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

public class ProgressEventArgs : EventArgs
{
  private long m_lPosition;
  private long m_lSize;

  public long Position => this.m_lPosition;

  public long FullSize => this.m_lSize;

  public ProgressEventArgs(long curPos, long fullSize)
  {
    this.m_lPosition = curPos;
    this.m_lSize = fullSize;
  }
}
