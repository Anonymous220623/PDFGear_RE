// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.LookupCachingMode
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

[Flags]
public enum LookupCachingMode
{
  None = 0,
  VLOOKUP = 1,
  HLOOKUP = 2,
  Both = HLOOKUP | VLOOKUP, // 0x00000003
  OptimizeForMatches = 4,
}
