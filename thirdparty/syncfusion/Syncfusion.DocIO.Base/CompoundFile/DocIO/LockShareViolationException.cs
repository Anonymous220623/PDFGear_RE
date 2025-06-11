// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.LockShareViolationException
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO;

[Serializable]
public class LockShareViolationException : ApplicationException
{
  private const string DEF_MESSAGE = "Access denied because another caller has the file open and locked.";
  private const string DEF_MESSAGE_CODE = "Access denied because another caller has the file open and locked. {0}.";

  public LockShareViolationException()
    : base("Access denied because another caller has the file open and locked.")
  {
  }

  public LockShareViolationException(string message)
    : base($"Access denied because another caller has the file open and locked. {message}.")
  {
  }

  public LockShareViolationException(string message, Exception innerException)
    : base(message, innerException)
  {
  }
}
