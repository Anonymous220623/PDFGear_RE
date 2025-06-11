// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.StoreManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Commom;

public class StoreManager
{
  public static string YearlyProductID = "9P4MRQ6VJ37F";
  public static string LifetimeProductId = "9MV9RZZ21T7N";
  public static string YearlyPriceDefault = "$19.99";
  public static string LifetimePriceDefault = "$49.99";
  private readonly string[] storeIds = new string[2]
  {
    "9P4MRQ6VJ37F",
    "9MV9RZZ21T7N"
  };

  private StoreManager()
  {
  }

  public static StoreManager Instance { get; } = new StoreManager();

  public async Task<bool> PurchaseProduct(string storeID) => false;

  public void UpdateLicenseInfo()
  {
  }

  public bool IsPaidUser()
  {
    if (!ConfigManager.IsPaidExpired())
      return true;
    if (ConfigManager.GetIAPFlag() != 0U)
    {
      this.UpdateLicenseInfo();
      if (!ConfigManager.IsPaidExpired())
        return true;
    }
    return ConfigManager.GetIAPFlag() != 0U;
  }
}
