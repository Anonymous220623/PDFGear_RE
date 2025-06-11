// Decompiled with JetBrains decompiler
// Type: FileWatcher.UserNotificationState
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

#nullable disable
namespace FileWatcher;

public enum UserNotificationState
{
  NotPresent = 1,
  Busy = 2,
  RunningDirect3dFullScreen = 3,
  PresentationMode = 4,
  AcceptsNotifications = 5,
  QuietTime = 6,
  QUNS_APP = 7,
}
