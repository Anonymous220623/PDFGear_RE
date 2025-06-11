// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncLazyFlags
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;

#nullable disable
namespace Nito.AsyncEx;

[Flags]
public enum AsyncLazyFlags
{
  None = 0,
  ExecuteOnCallingThread = 1,
  RetryOnFailure = 2,
}
