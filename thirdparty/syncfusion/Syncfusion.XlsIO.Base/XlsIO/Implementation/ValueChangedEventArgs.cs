// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ValueChangedEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

[DebuggerStepThrough]
public class ValueChangedEventArgs : EventArgs
{
  private static ValueChangedEventArgs _empty = new ValueChangedEventArgs();
  private object m_old;
  private object m_new;
  private string m_strName;
  private ValueChangedEventArgs m_next;

  public object newValue
  {
    [DebuggerStepThrough] get => this.m_new;
  }

  public object oldValue
  {
    [DebuggerStepThrough] get => this.m_old;
  }

  public string Name
  {
    [DebuggerStepThrough] get => this.m_strName;
  }

  public ValueChangedEventArgs Next
  {
    [DebuggerStepThrough] get => this.m_next;
    set => this.m_next = (ValueChangedEventArgs) null;
  }

  private ValueChangedEventArgs()
  {
  }

  public ValueChangedEventArgs(object old, object newValue, string objectName)
    : this(old, newValue, objectName, (ValueChangedEventArgs) null)
  {
  }

  public ValueChangedEventArgs(
    object old,
    object newValue,
    string objectName,
    ValueChangedEventArgs next)
  {
    this.m_old = old;
    this.m_new = newValue;
    this.m_strName = objectName;
    this.m_next = next;
  }

  public static ValueChangedEventArgs Empty
  {
    [DebuggerStepThrough] get => ValueChangedEventArgs._empty;
  }
}
