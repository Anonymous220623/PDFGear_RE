// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.XlsIO.LockShareViolationException
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.XlsIO;

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
