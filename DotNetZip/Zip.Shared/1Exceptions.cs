// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.BadReadException
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

#nullable disable
namespace Ionic.Zip;

[Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
[Serializable]
public class BadReadException : ZipException
{
  public BadReadException()
  {
  }

  public BadReadException(string message)
    : base(message)
  {
  }

  public BadReadException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected BadReadException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
