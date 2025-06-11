// Decompiled with JetBrains decompiler
// Type: NAudio.SoundFont.InfoChunk
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.IO;

#nullable disable
namespace NAudio.SoundFont;

public class InfoChunk
{
  internal InfoChunk(RiffChunk chunk)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (chunk.ReadChunkID() != "INFO")
      throw new InvalidDataException("Not an INFO chunk");
    RiffChunk nextSubChunk;
    while ((nextSubChunk = chunk.GetNextSubChunk()) != null)
    {
      string chunkId = nextSubChunk.ChunkID;
      if (chunkId != null && chunkId.Length == 4)
      {
        switch (chunkId[2])
        {
          case 'A':
            if (chunkId == "INAM")
            {
              flag2 = true;
              this.BankName = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
          case 'F':
            if (chunkId == "ISFT")
            {
              this.Tools = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
          case 'M':
            if (chunkId == "ICMT")
            {
              this.Comments = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
          case 'N':
            if (chunkId == "IENG")
            {
              this.Author = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
          case 'O':
            if (chunkId == "ICOP")
            {
              this.Copyright = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
          case 'R':
            switch (chunkId)
            {
              case "ICRD":
                this.CreationDate = nextSubChunk.GetDataAsString();
                continue;
              case "IPRD":
                this.TargetProduct = nextSubChunk.GetDataAsString();
                continue;
            }
            break;
          case 'e':
            if (chunkId == "iver")
            {
              this.ROMVersion = nextSubChunk.GetDataAsStructure<SFVersion>((StructureBuilder<SFVersion>) new SFVersionBuilder());
              continue;
            }
            break;
          case 'i':
            if (chunkId == "ifil")
            {
              flag1 = true;
              this.SoundFontVersion = nextSubChunk.GetDataAsStructure<SFVersion>((StructureBuilder<SFVersion>) new SFVersionBuilder());
              continue;
            }
            break;
          case 'n':
            if (chunkId == "isng")
            {
              this.WaveTableSoundEngine = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
          case 'o':
            if (chunkId == "irom")
            {
              this.DataROM = nextSubChunk.GetDataAsString();
              continue;
            }
            break;
        }
      }
      throw new InvalidDataException("Unknown chunk type " + nextSubChunk.ChunkID);
    }
    if (!flag1)
      throw new InvalidDataException("Missing SoundFont version information");
    if (!flag2)
      throw new InvalidDataException("Missing SoundFont name information");
  }

  public SFVersion SoundFontVersion { get; }

  public string WaveTableSoundEngine { get; set; }

  public string BankName { get; set; }

  public string DataROM { get; set; }

  public string CreationDate { get; set; }

  public string Author { get; set; }

  public string TargetProduct { get; set; }

  public string Copyright { get; set; }

  public string Comments { get; set; }

  public string Tools { get; set; }

  public SFVersion ROMVersion { get; set; }

  public override string ToString()
  {
    return $"Bank Name: {this.BankName}\r\nAuthor: {this.Author}\r\nCopyright: {this.Copyright}\r\nCreation Date: {this.CreationDate}\r\nTools: {this.Tools}\r\nComments: {"TODO-fix comments"}\r\nSound Engine: {this.WaveTableSoundEngine}\r\nSoundFont Version: {this.SoundFontVersion}\r\nTarget Product: {this.TargetProduct}\r\nData ROM: {this.DataROM}\r\nROM Version: {this.ROMVersion}";
  }
}
