// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.SecurityLogOnType
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Targets.Wrappers;

public enum SecurityLogOnType
{
  Interactive = 2,
  Network = 3,
  Batch = 4,
  Service = 5,
  NetworkClearText = 8,
  NewCredentials = 9,
}
