// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.BufferHelpers
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

#nullable disable
namespace NAudio.Utils;

public static class BufferHelpers
{
  public static byte[] Ensure(byte[] buffer, int bytesRequired)
  {
    if (buffer == null || buffer.Length < bytesRequired)
      buffer = new byte[bytesRequired];
    return buffer;
  }

  public static float[] Ensure(float[] buffer, int samplesRequired)
  {
    if (buffer == null || buffer.Length < samplesRequired)
      buffer = new float[samplesRequired];
    return buffer;
  }
}
