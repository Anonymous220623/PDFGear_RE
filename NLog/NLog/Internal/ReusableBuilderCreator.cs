// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableBuilderCreator
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal class ReusableBuilderCreator : ReusableObjectCreator<StringBuilder>
{
  public ReusableBuilderCreator()
    : base(128 /*0x80*/, (Func<int, StringBuilder>) (cap => new StringBuilder(cap)), (Action<StringBuilder>) (sb => sb.ClearBuilder()))
  {
  }
}
