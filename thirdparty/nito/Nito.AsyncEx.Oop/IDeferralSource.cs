// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.IDeferralSource
// Assembly: Nito.AsyncEx.Oop, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15B42D27-4022-4533-A6B4-AA9F1FAB0E34
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Oop.dll

using System;

#nullable enable
namespace Nito.AsyncEx;

public interface IDeferralSource
{
  IDisposable GetDeferral();
}
