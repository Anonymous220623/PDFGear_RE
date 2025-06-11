// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Relations
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression.Zip;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Relations : Part
{
  public Relations(ZipArchiveItem item)
    : base(item.DataStream)
  {
    this.m_name = item.ItemName;
  }

  internal Relations(Stream dataStream, string name)
    : base(dataStream)
  {
    this.m_name = name;
  }

  internal new Part Clone() => (Part) new Relations(this.m_dataStream, this.m_name);

  internal new void Close() => base.Close();
}
