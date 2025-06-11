// Decompiled with JetBrains decompiler
// Type: FileWatcher.FocusAssistResult
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

#nullable disable
namespace FileWatcher;

public enum FocusAssistResult
{
  NOT_SUPPORTED = -2, // 0xFFFFFFFE
  FAILED = -1, // 0xFFFFFFFF
  OFF = 0,
  PRIORITY_ONLY = 1,
  ALARMS_ONLY = 2,
}
