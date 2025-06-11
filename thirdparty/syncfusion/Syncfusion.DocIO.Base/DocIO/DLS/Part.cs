// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Part
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Part
{
  protected Stream m_dataStream;
  protected string m_name;

  internal Stream DataStream => this.m_dataStream;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public Part(Stream dataStream)
  {
    if (dataStream == null)
    {
      this.m_dataStream = (Stream) new MemoryStream();
    }
    else
    {
      byte[] buffer = new byte[dataStream.Length];
      dataStream.Position = 0L;
      dataStream.Read(buffer, 0, (int) dataStream.Length);
      this.m_dataStream = (Stream) new MemoryStream(buffer);
    }
  }

  internal Part Clone()
  {
    return new Part(this.m_dataStream)
    {
      Name = this.m_name
    };
  }

  internal void SetDataStream(Stream stream)
  {
    this.m_dataStream.Dispose();
    this.m_dataStream = stream;
  }

  internal void Close()
  {
    if (this.m_dataStream == null)
      return;
    this.m_dataStream.Close();
    this.m_dataStream = (Stream) null;
  }
}
