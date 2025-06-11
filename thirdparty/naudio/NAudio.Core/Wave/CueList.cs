// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.CueList
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace NAudio.Wave;

public class CueList
{
  private readonly List<Cue> cues = new List<Cue>();

  public CueList()
  {
  }

  public void Add(Cue cue) => this.cues.Add(cue);

  public int[] CuePositions
  {
    get
    {
      int[] cuePositions = new int[this.cues.Count];
      for (int index = 0; index < this.cues.Count; ++index)
        cuePositions[index] = this.cues[index].Position;
      return cuePositions;
    }
  }

  public string[] CueLabels
  {
    get
    {
      string[] cueLabels = new string[this.cues.Count];
      for (int index = 0; index < this.cues.Count; ++index)
        cueLabels[index] = this.cues[index].Label;
      return cueLabels;
    }
  }

  internal CueList(byte[] cueChunkData, byte[] listChunkData)
  {
    int int32_1 = BitConverter.ToInt32(cueChunkData, 0);
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int[] numArray = new int[int32_1];
    int index1 = 0;
    int startIndex1 = 4;
    while (cueChunkData.Length - startIndex1 >= 24)
    {
      dictionary[BitConverter.ToInt32(cueChunkData, startIndex1)] = index1;
      numArray[index1] = BitConverter.ToInt32(cueChunkData, startIndex1 + 20);
      startIndex1 += 24;
      ++index1;
    }
    string[] strArray = new string[int32_1];
    int num = 0;
    int int32_2 = ChunkIdentifier.ChunkIdentifierToInt32("labl");
    for (int startIndex2 = 4; listChunkData.Length - startIndex2 >= 16 /*0x10*/; startIndex2 += num + num % 2 + 12)
    {
      if (BitConverter.ToInt32(listChunkData, startIndex2) == int32_2)
      {
        num = BitConverter.ToInt32(listChunkData, startIndex2 + 4) - 4;
        int int32_3 = BitConverter.ToInt32(listChunkData, startIndex2 + 8);
        int index2 = dictionary[int32_3];
        strArray[index2] = Encoding.UTF8.GetString(listChunkData, startIndex2 + 12, num - 1);
      }
    }
    for (int index3 = 0; index3 < int32_1; ++index3)
      this.cues.Add(new Cue(numArray[index3], strArray[index3]));
  }

  internal byte[] GetRiffChunks()
  {
    if (this.Count == 0)
      return (byte[]) null;
    int num1 = 12 + 24 * this.Count;
    int num2 = 12;
    for (int index = 0; index < this.Count; ++index)
    {
      int num3 = Encoding.UTF8.GetBytes(this[index].Label).Length + 1;
      num2 += num3 + num3 % 2 + 12;
    }
    byte[] buffer = new byte[num1 + num2];
    int int32_1 = ChunkIdentifier.ChunkIdentifierToInt32("cue ");
    int int32_2 = ChunkIdentifier.ChunkIdentifierToInt32("data");
    int int32_3 = ChunkIdentifier.ChunkIdentifierToInt32("LIST");
    int int32_4 = ChunkIdentifier.ChunkIdentifierToInt32("adtl");
    int int32_5 = ChunkIdentifier.ChunkIdentifierToInt32("labl");
    using (MemoryStream output = new MemoryStream(buffer))
    {
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
      {
        binaryWriter.Write(int32_1);
        binaryWriter.Write(num1 - 8);
        binaryWriter.Write(this.Count);
        for (int index = 0; index < this.Count; ++index)
        {
          int position = this[index].Position;
          binaryWriter.Write(index);
          binaryWriter.Write(position);
          binaryWriter.Write(int32_2);
          binaryWriter.Seek(8, SeekOrigin.Current);
          binaryWriter.Write(position);
        }
        binaryWriter.Write(int32_3);
        binaryWriter.Write(num2 - 8);
        binaryWriter.Write(int32_4);
        for (int index = 0; index < this.Count; ++index)
        {
          byte[] bytes = Encoding.UTF8.GetBytes(this[index].Label);
          binaryWriter.Write(int32_5);
          binaryWriter.Write(bytes.Length + 1 + 4);
          binaryWriter.Write(index);
          binaryWriter.Write(bytes);
          if (bytes.Length % 2 == 0)
            binaryWriter.Seek(2, SeekOrigin.Current);
          else
            binaryWriter.Seek(1, SeekOrigin.Current);
        }
      }
    }
    return buffer;
  }

  public int Count => this.cues.Count;

  public Cue this[int index] => this.cues[index];

  internal static CueList FromChunks(WaveFileReader reader)
  {
    CueList cueList = (CueList) null;
    byte[] cueChunkData = (byte[]) null;
    byte[] listChunkData = (byte[]) null;
    foreach (RiffChunk extraChunk in reader.ExtraChunks)
    {
      if (extraChunk.IdentifierAsString.ToLower() == "cue ")
        cueChunkData = reader.GetChunkData(extraChunk);
      else if (extraChunk.IdentifierAsString.ToLower() == "list")
        listChunkData = reader.GetChunkData(extraChunk);
    }
    if (cueChunkData != null && listChunkData != null)
      cueList = new CueList(cueChunkData, listChunkData);
    return cueList;
  }
}
