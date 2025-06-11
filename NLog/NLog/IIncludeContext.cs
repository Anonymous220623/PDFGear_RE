// Decompiled with JetBrains decompiler
// Type: NLog.IIncludeContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog;

internal interface IIncludeContext
{
  bool IncludeMdc { get; set; }

  bool IncludeNdc { get; set; }

  bool IncludeAllProperties { get; set; }

  bool IncludeMdlc { get; set; }

  bool IncludeNdlc { get; set; }
}
