// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.BerDecodeException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Patagames.Activation;

[Serializable]
internal sealed class BerDecodeException : Exception, ISerializable
{
  private int m_position;

  public int Position => this.m_position;

  public override string Message
  {
    get
    {
      StringBuilder stringBuilder = new StringBuilder(base.Message);
      stringBuilder.AppendFormat(" (Position {0}){1}", (object) this.m_position, (object) Environment.NewLine);
      return stringBuilder.ToString();
    }
  }

  public BerDecodeException()
  {
  }

  public BerDecodeException(string message)
    : base(message)
  {
  }

  public BerDecodeException(string message, Exception ex)
    : base(message, ex)
  {
  }

  public BerDecodeException(string message, int position)
    : base(message)
  {
    this.m_position = position;
  }

  public BerDecodeException(string message, int position, Exception ex)
    : base(message, ex)
  {
    this.m_position = position;
  }

  private BerDecodeException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    this.m_position = info.GetInt32(nameof (Position));
  }

  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    base.GetObjectData(info, context);
    info.AddValue("Position", this.m_position);
  }
}
