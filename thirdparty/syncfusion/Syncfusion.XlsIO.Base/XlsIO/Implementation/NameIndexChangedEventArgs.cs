// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.NameIndexChangedEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class NameIndexChangedEventArgs : EventArgs
{
  private int m_OldIndex;
  private int m_NewIndex;

  private NameIndexChangedEventArgs()
  {
  }

  public NameIndexChangedEventArgs(int oldIndex, int newIndex)
  {
    this.m_OldIndex = oldIndex;
    this.m_NewIndex = newIndex;
  }

  public int NewIndex => this.m_NewIndex;

  public int OldIndex => this.m_OldIndex;
}
