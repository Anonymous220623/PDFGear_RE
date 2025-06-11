// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.ReferenceRecord
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Office;

internal abstract class ReferenceRecord
{
  internal abstract string Name { get; set; }

  internal abstract Encoding EncodingType { get; set; }

  internal abstract void ParseRecord(Stream dirData);

  internal abstract void SerializeRecord(Stream dirData);

  internal virtual ReferenceRecord Clone() => (ReferenceRecord) this.MemberwiseClone();
}
