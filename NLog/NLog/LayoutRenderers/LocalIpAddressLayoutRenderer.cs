// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LocalIpAddressLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("local-ip")]
[ThreadAgnostic]
[ThreadSafe]
public class LocalIpAddressLayoutRenderer : LayoutRenderer
{
  private AddressFamily? _addressFamily;
  private readonly INetworkInterfaceRetriever _networkInterfaceRetriever;

  public AddressFamily AddressFamily
  {
    get => this._addressFamily ?? AddressFamily.InterNetwork;
    set => this._addressFamily = new AddressFamily?(value);
  }

  public LocalIpAddressLayoutRenderer()
  {
    this._networkInterfaceRetriever = (INetworkInterfaceRetriever) new NetworkInterfaceRetriever();
  }

  internal LocalIpAddressLayoutRenderer(
    INetworkInterfaceRetriever networkInterfaceRetriever)
  {
    this._networkInterfaceRetriever = networkInterfaceRetriever;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.LookupIpAddress());
  }

  private string LookupIpAddress()
  {
    int currentNetworkScore = 0;
    string empty = string.Empty;
    try
    {
      foreach (NetworkInterface networkInterface in this._networkInterfaceRetriever.AllNetworkInterfaces)
      {
        int networkInterfaceScore = LocalIpAddressLayoutRenderer.CalculateNetworkInterfaceScore(networkInterface);
        if (networkInterfaceScore != 0)
        {
          IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
          foreach (UnicastIPAddressInformation unicastAddress in ipProperties.UnicastAddresses)
          {
            int networkAddressScore = this.CalculateNetworkAddressScore(unicastAddress, ipProperties);
            if (networkAddressScore != 0 && this.CheckOptimalNetworkScore(unicastAddress, networkInterfaceScore + networkAddressScore, ref currentNetworkScore, ref empty))
              return empty;
          }
        }
      }
      return empty;
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Failed to lookup NetworkInterface.GetAllNetworkInterfaces()");
      return empty;
    }
  }

  private bool CheckOptimalNetworkScore(
    UnicastIPAddressInformation networkAddress,
    int networkScore,
    ref int currentNetworkScore,
    ref string currentIpAddress)
  {
    if (networkScore > currentNetworkScore)
    {
      currentIpAddress = networkAddress.Address.ToString();
      currentNetworkScore = networkScore;
      if (currentNetworkScore >= 30)
        return true;
    }
    return false;
  }

  private static int CalculateNetworkInterfaceScore(NetworkInterface networkInterface)
  {
    if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
      return 0;
    PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
    int num1;
    if (physicalAddress == null)
    {
      num1 = 0;
    }
    else
    {
      int? length = physicalAddress.ToString()?.Length;
      int num2 = 12;
      num1 = length.GetValueOrDefault() >= num2 & length.HasValue ? 1 : 0;
    }
    if (num1 == 0)
      return 0;
    int networkInterfaceScore = 1;
    if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || networkInterface.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet)
      ++networkInterfaceScore;
    if (networkInterface.OperationalStatus == OperationalStatus.Up)
      networkInterfaceScore += 9;
    return networkInterfaceScore;
  }

  private int CalculateNetworkAddressScore(
    UnicastIPAddressInformation networkAddress,
    IPInterfaceProperties ipProperties)
  {
    int ipAddressScore = this.CalculateIpAddressScore(networkAddress.Address);
    if (ipAddressScore == 0)
      return 0;
    GatewayIPAddressInformationCollection gatewayAddresses = ipProperties.GatewayAddresses;
    if (gatewayAddresses != null && gatewayAddresses.Count > 0)
    {
      foreach (GatewayIPAddressInformation addressInformation in gatewayAddresses)
      {
        if (!LocalIpAddressLayoutRenderer.IsLoopbackAddressValue(addressInformation.Address))
        {
          ipAddressScore += 3;
          break;
        }
      }
    }
    IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;
    if (dnsAddresses != null && dnsAddresses.Count > 0)
    {
      foreach (IPAddress ipAddress in dnsAddresses)
      {
        if (!LocalIpAddressLayoutRenderer.IsLoopbackAddressValue(ipAddress))
        {
          ++ipAddressScore;
          break;
        }
      }
    }
    return ipAddressScore;
  }

  private int CalculateIpAddressScore(IPAddress ipAddress)
  {
    if (ipAddress.AddressFamily != AddressFamily.InterNetwork && ipAddress.AddressFamily != AddressFamily.InterNetworkV6)
    {
      int addressFamily1 = (int) ipAddress.AddressFamily;
      AddressFamily? addressFamily2 = this._addressFamily;
      int valueOrDefault = (int) addressFamily2.GetValueOrDefault();
      if (!(addressFamily1 == valueOrDefault & addressFamily2.HasValue))
        return 0;
    }
    if (LocalIpAddressLayoutRenderer.IsLoopbackAddressValue(ipAddress))
      return 0;
    int num = 0;
    return !this._addressFamily.HasValue ? (ipAddress.AddressFamily != AddressFamily.InterNetwork ? num + 8 : num + 16 /*0x10*/) : (ipAddress.AddressFamily != this._addressFamily.Value ? num + 4 : num + 16 /*0x10*/);
  }

  private static bool IsLoopbackAddressValue(IPAddress ipAddress)
  {
    if (ipAddress == null || IPAddress.IsLoopback(ipAddress))
      return true;
    string str = ipAddress.ToString();
    return string.IsNullOrEmpty(str) || str == "127.0.0.1" || str == "0.0.0.0" || str == "::1" || str == "::";
  }
}
