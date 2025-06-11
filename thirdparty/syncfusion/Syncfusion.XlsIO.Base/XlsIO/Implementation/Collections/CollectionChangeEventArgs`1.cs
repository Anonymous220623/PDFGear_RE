// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CollectionChangeEventArgs`1
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CollectionChangeEventArgs<T> : EventArgs
{
  private int m_iIndex;
  private T m_value;

  private CollectionChangeEventArgs()
  {
  }

  public CollectionChangeEventArgs(int index, T value)
  {
    this.m_iIndex = index;
    this.m_value = value;
  }

  public int Index => this.m_iIndex;

  public T Value => this.m_value;
}
