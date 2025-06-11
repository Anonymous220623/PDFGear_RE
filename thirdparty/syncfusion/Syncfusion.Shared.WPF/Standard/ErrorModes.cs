// Decompiled with JetBrains decompiler
// Type: Standard.ErrorModes
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Standard;

[Flags]
internal enum ErrorModes
{
  Default = 0,
  FailCriticalErrors = 1,
  NoGpFaultErrorBox = 2,
  NoAlignmentFaultExcept = 4,
  NoOpenFileErrorBox = 32768, // 0x00008000
}
