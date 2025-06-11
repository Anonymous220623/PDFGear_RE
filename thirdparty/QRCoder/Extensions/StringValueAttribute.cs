// Decompiled with JetBrains decompiler
// Type: QRCoder.Extensions.StringValueAttribute
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;

#nullable disable
namespace QRCoder.Extensions;

public class StringValueAttribute : Attribute
{
  public string StringValue { get; protected set; }

  public StringValueAttribute(string value) => this.StringValue = value;
}
