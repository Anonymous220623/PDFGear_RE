// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GifFrameDimension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Data;

internal class GifFrameDimension
{
  public GifFrameDimension(Guid guid) => this.Guid = guid;

  public Guid Guid { get; }

  public static GifFrameDimension Time { get; } = new GifFrameDimension(new Guid("{6aedbd6d-3fb5-418a-83a6-7f45229dc872}"));

  public static GifFrameDimension Resolution { get; } = new GifFrameDimension(new Guid("{84236f7b-3bd3-428f-8dab-4ea1439ca315}"));

  public static GifFrameDimension Page { get; } = new GifFrameDimension(new Guid("{7462dc86-6180-4c7e-8e3f-ee7333a7a483}"));

  public override bool Equals(object o)
  {
    return o is GifFrameDimension gifFrameDimension && this.Guid == gifFrameDimension.Guid;
  }

  public override int GetHashCode() => this.Guid.GetHashCode();

  public override string ToString()
  {
    if (object.Equals((object) this, (object) GifFrameDimension.Time))
      return "Time";
    if (object.Equals((object) this, (object) GifFrameDimension.Resolution))
      return "Resolution";
    return object.Equals((object) this, (object) GifFrameDimension.Page) ? "Page" : $"[FrameDimension: {this.Guid.ToString()}]";
  }
}
