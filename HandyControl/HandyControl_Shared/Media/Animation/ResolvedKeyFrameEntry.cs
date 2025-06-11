// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.ResolvedKeyFrameEntry
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Media.Animation;

internal struct ResolvedKeyFrameEntry : IComparable
{
  internal int _originalKeyFrameIndex;
  internal TimeSpan _resolvedKeyTime;

  public int CompareTo(object other)
  {
    ResolvedKeyFrameEntry resolvedKeyFrameEntry = (ResolvedKeyFrameEntry) other;
    if (resolvedKeyFrameEntry._resolvedKeyTime > this._resolvedKeyTime)
      return -1;
    if (resolvedKeyFrameEntry._resolvedKeyTime < this._resolvedKeyTime)
      return 1;
    if (resolvedKeyFrameEntry._originalKeyFrameIndex > this._originalKeyFrameIndex)
      return -1;
    return resolvedKeyFrameEntry._originalKeyFrameIndex < this._originalKeyFrameIndex ? 1 : 0;
  }
}
