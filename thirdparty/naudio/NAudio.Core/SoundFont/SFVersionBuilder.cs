// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.SFVersionBuilder
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.SoundFont;

internal class SFVersionBuilder : StructureBuilder<SFVersion>
{
  public override SFVersion Read(BinaryReader br)
  {
    SFVersion sfVersion = new SFVersion();
    sfVersion.Major = br.ReadInt16();
    sfVersion.Minor = br.ReadInt16();
    this.data.Add(sfVersion);
    return sfVersion;
  }

  public override void Write(BinaryWriter bw, SFVersion v)
  {
    bw.Write(v.Major);
    bw.Write(v.Minor);
  }

  public override int Length => 4;
}
