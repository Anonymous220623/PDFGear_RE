// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.BadPasswordException
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

#nullable disable
namespace Ionic.Zip;

[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
[Serializable]
public class BadPasswordException : ZipException
{
  public BadPasswordException()
  {
  }

  public BadPasswordException(string message)
    : base(message)
  {
  }

  public BadPasswordException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected BadPasswordException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
