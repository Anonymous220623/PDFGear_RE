// Decompiled with JetBrains decompiler
// Type: SoundTouch.Beat
// Assembly: SoundTouch.Net, Version=2.3.0.0, Culture=neutral, PublicKeyToken=fd94608088498f78
// MVID: 3EFEC8B2-F004-4B74-B172-E7BC33D87326
// Assembly location: D:\PDFGear\bin\SoundTouch.Net.dll

#nullable disable
namespace SoundTouch;

internal readonly struct Beat(float pos, float strength)
{
  public float Position { get; } = pos;

  public float Strength { get; } = strength;
}
