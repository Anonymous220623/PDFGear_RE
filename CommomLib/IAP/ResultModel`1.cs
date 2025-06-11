// Decompiled with JetBrains decompiler
// Type: CommomLib.IAP.ResultModel`1
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.IAP;

public class ResultModel<T>
{
  public bool Success { get; set; }

  public string Message { get; set; }

  public T Content { get; set; }
}
