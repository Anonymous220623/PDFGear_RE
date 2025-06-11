// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFontWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFontWriter : IDisposable
{
  private readonly BinaryWriter writer;
  private readonly Stream stream;

  public SystemFontFontWriter()
  {
    this.stream = (Stream) new MemoryStream();
    this.writer = new BinaryWriter(this.stream);
  }

  private void WriteBE(byte[] buffer, int count)
  {
    for (int index = count - 1; index >= 0; --index)
      this.Write(buffer[index]);
  }

  public void Write(byte b) => this.writer.Write(b);

  public void WriteChar(sbyte ch) => this.Write((byte) ch);

  public void WriteUShort(ushort us) => this.WriteBE(BitConverter.GetBytes(us), 2);

  public void WriteShort(short s) => this.WriteBE(BitConverter.GetBytes(s), 2);

  public void WriteULong(uint ul) => this.WriteBE(BitConverter.GetBytes(ul), 4);

  public void WriteLong(int l) => this.WriteBE(BitConverter.GetBytes(l), 4);

  public void WriteString(string str)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(str);
    this.Write((byte) bytes.Length);
    for (int index = 0; index < bytes.Length; ++index)
      this.Write(bytes[index]);
  }

  public byte[] GetBytes() => SystemFontExtensions.ReadAllBytes(this.stream);

  public void Dispose()
  {
    this.writer.Flush();
    this.stream.Close();
    this.writer.Close();
  }
}
