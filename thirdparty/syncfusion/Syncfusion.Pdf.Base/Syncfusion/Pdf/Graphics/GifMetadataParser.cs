// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.GifMetadataParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class GifMetadataParser : IImageMetadataParser
{
  private MemoryStream m_stream;
  private BinaryReader m_reader;

  internal MemoryStream Stream
  {
    get
    {
      if (this.m_stream == null)
        this.m_stream = new MemoryStream();
      return this.m_stream;
    }
  }

  internal GifMetadataParser(System.IO.Stream stream) => this.m_reader = new BinaryReader(stream);

  public MemoryStream GetMetadata()
  {
    if (Encoding.UTF8.GetString(this.m_reader.ReadBytes(3), 0, 3) == "GIF")
    {
      string str = Encoding.UTF8.GetString(this.m_reader.ReadBytes(3), 0, 3);
      if (str == "87a" || str == "89a")
      {
        this.m_reader.ReadBytes(4);
        byte num1 = this.m_reader.ReadByte();
        int num2 = 1 << ((int) num1 & 7) + 1;
        bool flag1 = (int) num1 >> 7 != 0;
        this.m_reader.ReadBytes(2);
        if (flag1)
          this.m_reader.ReadBytes(3 * num2);
        bool flag2 = true;
        while (flag2)
        {
          byte num3;
          try
          {
            num3 = this.m_reader.ReadByte();
          }
          catch
          {
            break;
          }
          switch (num3)
          {
            case 33:
              byte num4 = this.m_reader.ReadByte();
              byte length = this.m_reader.ReadByte();
              long position = this.m_reader.BaseStream.Position;
              switch (num4)
              {
                case 1:
                  if (length == (byte) 12)
                  {
                    this.m_reader.ReadBytes(12);
                    this.SkipBlocks();
                    break;
                  }
                  break;
                case 249:
                  if (length < (byte) 4)
                    length = (byte) 4;
                  this.m_reader.ReadBytes(4);
                  break;
                case 254:
                  for (byte count = length; count > (byte) 0; count = this.m_reader.ReadByte())
                    this.m_reader.ReadBytes((int) count);
                  break;
                case byte.MaxValue:
                  this.ReadApplicationExtensionBlock(length);
                  break;
              }
              long count1 = position + (long) length - this.m_reader.BaseStream.Position;
              if (count1 > 0L)
              {
                this.m_reader.ReadBytes((int) count1);
                continue;
              }
              continue;
            case 44:
              this.m_reader.ReadBytes(8);
              byte num5 = this.m_reader.ReadByte();
              if ((int) num5 >> 7 != 0)
                this.m_reader.ReadBytes(3 * (2 << ((int) num5 & 7)));
              int num6 = (int) this.m_reader.ReadByte();
              this.SkipBlocks();
              continue;
            default:
              flag2 = false;
              continue;
          }
        }
      }
    }
    return this.m_stream;
  }

  private void SkipBlocks()
  {
    while (true)
    {
      byte count = this.m_reader.ReadByte();
      if (count != (byte) 0)
        this.m_reader.ReadBytes((int) count);
      else
        break;
    }
  }

  private void ReadApplicationExtensionBlock(byte length)
  {
    if (length != (byte) 11)
      return;
    switch (Encoding.UTF8.GetString(this.m_reader.ReadBytes((int) length), 0, (int) length))
    {
      case "XMP DataXMP":
        MemoryStream memoryStream = new MemoryStream();
        byte[] buffer = new byte[257];
        while (true)
        {
          byte count = this.m_reader.ReadByte();
          if (count != (byte) 0)
          {
            buffer[0] = count;
            this.m_reader.BaseStream.Read(buffer, 1, (int) count);
            memoryStream.Write(buffer, 0, (int) count + 1);
          }
          else
            break;
        }
        byte[] array = memoryStream.ToArray();
        if (array == null)
          break;
        this.Stream.Write(array, 0, array.Length - 257);
        break;
      case "ICCRGBG1012":
        for (byte count = this.m_reader.ReadByte(); count > (byte) 0; count = this.m_reader.ReadByte())
          this.m_reader.ReadBytes((int) count);
        break;
      case "NETSCAPE2.0":
        this.m_reader.ReadBytes(5);
        break;
      default:
        this.SkipBlocks();
        break;
    }
  }
}
