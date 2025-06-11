// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.GAManager2
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

#nullable disable
namespace CommomLib.Commom;

public class GAManager2
{
  public static void SendEvent(string strCategory, string strAction, string strLabel, long value)
  {
    GAManager2.SendEvent(strCategory, strAction, strLabel, value, true);
  }

  public static void SendEvent(
    string strCategory,
    string strAction,
    string strLabel,
    long value,
    bool bSendGA4)
  {
    if (!bSendGA4)
      return;
    GA4Manager2.SendEvent(strCategory, strAction, strLabel, value);
  }
}
