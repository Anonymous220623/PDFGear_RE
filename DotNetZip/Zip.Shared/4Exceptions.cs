// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.BadStateException
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

#nullable disable
namespace Ionic.Zip;

[Guid("ebc25cf6-9120-4283-b972-0e5520d00007")]
[Serializable]
public class BadStateException : ZipException
{
  public BadStateException()
  {
  }

  public BadStateException(string message)
    : base(message)
  {
  }

  public BadStateException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected BadStateException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
