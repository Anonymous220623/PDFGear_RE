// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.AnnotationDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class AnnotationDescriptor
{
  internal const int DEF_LENGTH = 30;
  private string m_xstUsrInitl;
  private short m_ibst;
  private short m_ak;
  private short m_grfbmc;
  private int m_lTagBkmk;

  internal short IndexToGrpOwner
  {
    get => this.m_ibst;
    set => this.m_ibst = value;
  }

  internal string UserInitials
  {
    get => this.m_xstUsrInitl;
    set => this.m_xstUsrInitl = value;
  }

  internal short Ak
  {
    get => this.m_ak;
    set => this.m_ak = value;
  }

  internal short Grfbmc
  {
    get => this.m_grfbmc;
    set => this.m_grfbmc = value;
  }

  internal int TagBkmk
  {
    get => this.m_lTagBkmk;
    set => this.m_lTagBkmk = value;
  }

  internal AnnotationDescriptor(BinaryReader reader) => this.Read(reader);

  internal AnnotationDescriptor() => this.TagBkmk = -1;

  internal void Read(BinaryReader reader)
  {
    byte[] bytes = reader.ReadBytes(20);
    this.m_xstUsrInitl = Encoding.Unicode.GetString(bytes).Substring(1, (int) bytes[0]);
    this.m_ibst = reader.ReadInt16();
    this.m_ak = reader.ReadInt16();
    this.m_grfbmc = reader.ReadInt16();
    this.m_lTagBkmk = reader.ReadInt32();
  }

  internal void Write(BinaryWriter writer)
  {
    string empty = string.Empty;
    string s = this.m_xstUsrInitl.Length <= 9 ? ((char) this.m_xstUsrInitl.Length).ToString() + this.m_xstUsrInitl : '9'.ToString() + this.m_xstUsrInitl.Substring(0, 9);
    if (s.Length < 10)
    {
      int num = 0;
      for (int index = 10 - s.Length; num < index; ++num)
        s += (string) (object) char.MinValue;
    }
    byte[] bytes = Encoding.Unicode.GetBytes(s);
    writer.Write(bytes, 0, bytes.Length);
    writer.Write(this.m_ibst);
    writer.Write(this.m_ak);
    writer.Write(this.m_grfbmc);
    writer.Write(this.m_lTagBkmk);
  }
}
