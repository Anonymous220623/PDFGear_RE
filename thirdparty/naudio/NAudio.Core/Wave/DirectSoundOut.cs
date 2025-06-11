// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.DirectSoundOut
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

#nullable disable
namespace NAudio.Wave;

public class DirectSoundOut : IWavePlayer, IDisposable
{
  private PlaybackState playbackState;
  private WaveFormat waveFormat;
  private int samplesTotalSize;
  private int samplesFrameSize;
  private int nextSamplesWriteIndex;
  private int desiredLatency;
  private Guid device;
  private byte[] samples;
  private IWaveProvider waveStream;
  private DirectSoundOut.IDirectSound directSound;
  private DirectSoundOut.IDirectSoundBuffer primarySoundBuffer;
  private DirectSoundOut.IDirectSoundBuffer secondaryBuffer;
  private EventWaitHandle frameEventWaitHandle1;
  private EventWaitHandle frameEventWaitHandle2;
  private EventWaitHandle endEventWaitHandle;
  private Thread notifyThread;
  private SynchronizationContext syncContext;
  private long bytesPlayed;
  private object m_LockObject = new object();
  private static List<DirectSoundDeviceInfo> devices;
  public static readonly Guid DSDEVID_DefaultPlayback = new Guid("DEF00000-9C6D-47ED-AAF1-4DDA8F2B5C03");
  public static readonly Guid DSDEVID_DefaultCapture = new Guid("DEF00001-9C6D-47ED-AAF1-4DDA8F2B5C03");
  public static readonly Guid DSDEVID_DefaultVoicePlayback = new Guid("DEF00002-9C6D-47ED-AAF1-4DDA8F2B5C03");
  public static readonly Guid DSDEVID_DefaultVoiceCapture = new Guid("DEF00003-9C6D-47ED-AAF1-4DDA8F2B5C03");

  public event EventHandler<StoppedEventArgs> PlaybackStopped;

  public static IEnumerable<DirectSoundDeviceInfo> Devices
  {
    get
    {
      DirectSoundOut.devices = new List<DirectSoundDeviceInfo>();
      DirectSoundOut.DirectSoundEnumerate(new DirectSoundOut.DSEnumCallback(DirectSoundOut.EnumCallback), IntPtr.Zero);
      return (IEnumerable<DirectSoundDeviceInfo>) DirectSoundOut.devices;
    }
  }

  private static bool EnumCallback(
    IntPtr lpGuid,
    IntPtr lpcstrDescription,
    IntPtr lpcstrModule,
    IntPtr lpContext)
  {
    DirectSoundDeviceInfo directSoundDeviceInfo = new DirectSoundDeviceInfo();
    if (lpGuid == IntPtr.Zero)
    {
      directSoundDeviceInfo.Guid = Guid.Empty;
    }
    else
    {
      byte[] numArray = new byte[16 /*0x10*/];
      Marshal.Copy(lpGuid, numArray, 0, 16 /*0x10*/);
      directSoundDeviceInfo.Guid = new Guid(numArray);
    }
    directSoundDeviceInfo.Description = Marshal.PtrToStringAnsi(lpcstrDescription);
    directSoundDeviceInfo.ModuleName = Marshal.PtrToStringAnsi(lpcstrModule);
    DirectSoundOut.devices.Add(directSoundDeviceInfo);
    return true;
  }

  public DirectSoundOut()
    : this(DirectSoundOut.DSDEVID_DefaultPlayback)
  {
  }

  public DirectSoundOut(Guid device)
    : this(device, 40)
  {
  }

  public DirectSoundOut(int latency)
    : this(DirectSoundOut.DSDEVID_DefaultPlayback, latency)
  {
  }

  public DirectSoundOut(Guid device, int latency)
  {
    if (device == Guid.Empty)
      device = DirectSoundOut.DSDEVID_DefaultPlayback;
    this.device = device;
    this.desiredLatency = latency;
    this.syncContext = SynchronizationContext.Current;
  }

  ~DirectSoundOut() => this.Dispose();

  public void Play()
  {
    if (this.playbackState == PlaybackState.Stopped)
    {
      this.notifyThread = new Thread(new ThreadStart(this.PlaybackThreadFunc));
      this.notifyThread.Priority = ThreadPriority.Normal;
      this.notifyThread.IsBackground = true;
      this.notifyThread.Start();
    }
    lock (this.m_LockObject)
      this.playbackState = PlaybackState.Playing;
  }

  public void Stop()
  {
    if (Monitor.TryEnter(this.m_LockObject, 50))
    {
      this.playbackState = PlaybackState.Stopped;
      Monitor.Exit(this.m_LockObject);
    }
    else
    {
      if (this.notifyThread == null)
        return;
      this.notifyThread.Abort();
      this.notifyThread = (Thread) null;
    }
  }

  public void Pause()
  {
    lock (this.m_LockObject)
      this.playbackState = PlaybackState.Paused;
  }

  public long GetPosition()
  {
    if (this.playbackState != PlaybackState.Stopped)
    {
      DirectSoundOut.IDirectSoundBuffer secondaryBuffer = this.secondaryBuffer;
      if (secondaryBuffer != null)
      {
        uint currentPlayCursor;
        secondaryBuffer.GetCurrentPosition(out currentPlayCursor, out uint _);
        return (long) currentPlayCursor + this.bytesPlayed;
      }
    }
    return 0;
  }

  public TimeSpan PlaybackPosition
  {
    get
    {
      return TimeSpan.FromMilliseconds((double) (this.GetPosition() / (long) (this.waveFormat.Channels * this.waveFormat.BitsPerSample / 8)) * 1000.0 / (double) this.waveFormat.SampleRate);
    }
  }

  public void Init(IWaveProvider waveProvider)
  {
    this.waveStream = waveProvider;
    this.waveFormat = waveProvider.WaveFormat;
  }

  private void InitializeDirectSound()
  {
    lock (this.m_LockObject)
    {
      DirectSoundOut.DirectSoundCreate(ref this.device, out this.directSound, IntPtr.Zero);
      if (this.directSound == null)
        return;
      this.directSound.SetCooperativeLevel(DirectSoundOut.GetDesktopWindow(), DirectSoundOut.DirectSoundCooperativeLevel.DSSCL_PRIORITY);
      DirectSoundOut.BufferDescription bufferDescription1 = new DirectSoundOut.BufferDescription();
      bufferDescription1.dwSize = Marshal.SizeOf<DirectSoundOut.BufferDescription>(bufferDescription1);
      bufferDescription1.dwBufferBytes = 0U;
      bufferDescription1.dwFlags = DirectSoundOut.DirectSoundBufferCaps.DSBCAPS_PRIMARYBUFFER;
      bufferDescription1.dwReserved = 0;
      bufferDescription1.lpwfxFormat = IntPtr.Zero;
      bufferDescription1.guidAlgo = Guid.Empty;
      object dsDSoundBuffer;
      this.directSound.CreateSoundBuffer(bufferDescription1, out dsDSoundBuffer, IntPtr.Zero);
      this.primarySoundBuffer = (DirectSoundOut.IDirectSoundBuffer) dsDSoundBuffer;
      this.primarySoundBuffer.Play(0U, 0U, DirectSoundOut.DirectSoundPlayFlags.DSBPLAY_LOOPING);
      this.samplesFrameSize = this.MsToBytes(this.desiredLatency);
      DirectSoundOut.BufferDescription bufferDescription2 = new DirectSoundOut.BufferDescription();
      bufferDescription2.dwSize = Marshal.SizeOf<DirectSoundOut.BufferDescription>(bufferDescription2);
      bufferDescription2.dwBufferBytes = (uint) (this.samplesFrameSize * 2);
      bufferDescription2.dwFlags = DirectSoundOut.DirectSoundBufferCaps.DSBCAPS_CTRLVOLUME | DirectSoundOut.DirectSoundBufferCaps.DSBCAPS_CTRLPOSITIONNOTIFY | DirectSoundOut.DirectSoundBufferCaps.DSBCAPS_STICKYFOCUS | DirectSoundOut.DirectSoundBufferCaps.DSBCAPS_GLOBALFOCUS | DirectSoundOut.DirectSoundBufferCaps.DSBCAPS_GETCURRENTPOSITION2;
      bufferDescription2.dwReserved = 0;
      GCHandle gcHandle = GCHandle.Alloc((object) this.waveFormat, GCHandleType.Pinned);
      bufferDescription2.lpwfxFormat = gcHandle.AddrOfPinnedObject();
      bufferDescription2.guidAlgo = Guid.Empty;
      this.directSound.CreateSoundBuffer(bufferDescription2, out dsDSoundBuffer, IntPtr.Zero);
      this.secondaryBuffer = (DirectSoundOut.IDirectSoundBuffer) dsDSoundBuffer;
      gcHandle.Free();
      DirectSoundOut.BufferCaps bufferCaps = new DirectSoundOut.BufferCaps();
      bufferCaps.dwSize = Marshal.SizeOf<DirectSoundOut.BufferCaps>(bufferCaps);
      this.secondaryBuffer.GetCaps(bufferCaps);
      this.nextSamplesWriteIndex = 0;
      this.samplesTotalSize = bufferCaps.dwBufferBytes;
      this.samples = new byte[this.samplesTotalSize];
      DirectSoundOut.IDirectSoundNotify directSoundNotify = (DirectSoundOut.IDirectSoundNotify) dsDSoundBuffer;
      this.frameEventWaitHandle1 = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.frameEventWaitHandle2 = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.endEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
      DirectSoundOut.DirectSoundBufferPositionNotify[] bufferPositionNotifyArray = new DirectSoundOut.DirectSoundBufferPositionNotify[3];
      bufferPositionNotifyArray[0] = new DirectSoundOut.DirectSoundBufferPositionNotify();
      bufferPositionNotifyArray[0].dwOffset = 0U;
      bufferPositionNotifyArray[0].hEventNotify = this.frameEventWaitHandle1.SafeWaitHandle.DangerousGetHandle();
      bufferPositionNotifyArray[1] = new DirectSoundOut.DirectSoundBufferPositionNotify();
      bufferPositionNotifyArray[1].dwOffset = (uint) this.samplesFrameSize;
      bufferPositionNotifyArray[1].hEventNotify = this.frameEventWaitHandle2.SafeWaitHandle.DangerousGetHandle();
      bufferPositionNotifyArray[2] = new DirectSoundOut.DirectSoundBufferPositionNotify();
      bufferPositionNotifyArray[2].dwOffset = uint.MaxValue;
      bufferPositionNotifyArray[2].hEventNotify = this.endEventWaitHandle.SafeWaitHandle.DangerousGetHandle();
      DirectSoundOut.DirectSoundBufferPositionNotify[] pcPositionNotifies = bufferPositionNotifyArray;
      directSoundNotify.SetNotificationPositions(3U, pcPositionNotifies);
    }
  }

  public PlaybackState PlaybackState => this.playbackState;

  public float Volume
  {
    get => 1f;
    set
    {
      if ((double) value != 1.0)
        throw new InvalidOperationException("Setting volume not supported on DirectSoundOut, adjust the volume on your WaveProvider instead");
    }
  }

  public WaveFormat OutputWaveFormat => this.waveFormat;

  public void Dispose()
  {
    this.Stop();
    GC.SuppressFinalize((object) this);
  }

  private bool IsBufferLost()
  {
    return (this.secondaryBuffer.GetStatus() & DirectSoundOut.DirectSoundBufferStatus.DSBSTATUS_BUFFERLOST) > (DirectSoundOut.DirectSoundBufferStatus) 0;
  }

  private int MsToBytes(int ms)
  {
    int num = ms * (this.waveFormat.AverageBytesPerSecond / 1000);
    return num - num % this.waveFormat.BlockAlign;
  }

  private void PlaybackThreadFunc()
  {
    bool flag1 = false;
    bool flag2 = false;
    this.bytesPlayed = 0L;
    Exception e = (Exception) null;
    try
    {
      this.InitializeDirectSound();
      int num1 = 1;
      if (this.PlaybackState == PlaybackState.Stopped)
      {
        this.secondaryBuffer.SetCurrentPosition(0U);
        this.nextSamplesWriteIndex = 0;
        num1 = this.Feed(this.samplesTotalSize);
      }
      if (num1 <= 0)
        return;
      lock (this.m_LockObject)
        this.playbackState = PlaybackState.Playing;
      this.secondaryBuffer.Play(0U, 0U, DirectSoundOut.DirectSoundPlayFlags.DSBPLAY_LOOPING);
      WaitHandle[] waitHandles = new WaitHandle[3]
      {
        (WaitHandle) this.frameEventWaitHandle1,
        (WaitHandle) this.frameEventWaitHandle2,
        (WaitHandle) this.endEventWaitHandle
      };
      bool flag3 = true;
      while (this.PlaybackState != 0 & flag3)
      {
        int num2 = WaitHandle.WaitAny(waitHandles, 3 * this.desiredLatency, false);
        switch (num2)
        {
          case 0:
            if (flag2)
            {
              this.bytesPlayed += (long) (this.samplesFrameSize * 2);
              break;
            }
            break;
          case 2:
            this.StopPlayback();
            flag1 = true;
            flag3 = false;
            continue;
          case 258:
            this.StopPlayback();
            flag1 = true;
            throw new Exception("DirectSound buffer timeout");
          default:
            flag2 = true;
            break;
        }
        this.nextSamplesWriteIndex = (num2 == 0 ? 1 : 0) * this.samplesFrameSize;
        if (this.Feed(this.samplesFrameSize) == 0)
        {
          this.StopPlayback();
          flag1 = true;
          flag3 = false;
        }
      }
    }
    catch (Exception ex)
    {
      e = ex;
    }
    finally
    {
      if (!flag1)
      {
        try
        {
          this.StopPlayback();
        }
        catch (Exception ex)
        {
          if (e == null)
            e = ex;
        }
      }
      lock (this.m_LockObject)
        this.playbackState = PlaybackState.Stopped;
      this.bytesPlayed = 0L;
      this.RaisePlaybackStopped(e);
    }
  }

  private void RaisePlaybackStopped(Exception e)
  {
    EventHandler<StoppedEventArgs> handler = this.PlaybackStopped;
    if (handler == null)
      return;
    if (this.syncContext == null)
      handler((object) this, new StoppedEventArgs(e));
    else
      this.syncContext.Post((SendOrPostCallback) (state => handler((object) this, new StoppedEventArgs(e))), (object) null);
  }

  private void StopPlayback()
  {
    lock (this.m_LockObject)
    {
      if (this.secondaryBuffer != null)
      {
        this.CleanUpSecondaryBuffer();
        this.secondaryBuffer.Stop();
        Marshal.ReleaseComObject((object) this.secondaryBuffer);
        this.secondaryBuffer = (DirectSoundOut.IDirectSoundBuffer) null;
      }
      if (this.primarySoundBuffer != null)
      {
        this.primarySoundBuffer.Stop();
        Marshal.ReleaseComObject((object) this.primarySoundBuffer);
        this.primarySoundBuffer = (DirectSoundOut.IDirectSoundBuffer) null;
      }
      if (this.directSound == null)
        return;
      Marshal.ReleaseComObject((object) this.directSound);
      this.directSound = (DirectSoundOut.IDirectSound) null;
    }
  }

  private void CleanUpSecondaryBuffer()
  {
    if (this.secondaryBuffer == null)
      return;
    byte[] source = new byte[this.samplesTotalSize];
    IntPtr audioPtr1;
    int audioBytes1;
    IntPtr audioPtr2;
    int audioBytes2;
    this.secondaryBuffer.Lock(0, (uint) this.samplesTotalSize, out audioPtr1, out audioBytes1, out audioPtr2, out audioBytes2, DirectSoundOut.DirectSoundBufferLockFlag.None);
    if (audioPtr1 != IntPtr.Zero)
    {
      Marshal.Copy(source, 0, audioPtr1, audioBytes1);
      if (audioPtr2 != IntPtr.Zero)
        Marshal.Copy(source, 0, audioPtr1, audioBytes1);
    }
    this.secondaryBuffer.Unlock(audioPtr1, audioBytes1, audioPtr2, audioBytes2);
  }

  private int Feed(int bytesToCopy)
  {
    int dwBytes = bytesToCopy;
    if (this.IsBufferLost())
      this.secondaryBuffer.Restore();
    if (this.playbackState == PlaybackState.Paused)
    {
      Array.Clear((Array) this.samples, 0, this.samples.Length);
    }
    else
    {
      dwBytes = this.waveStream.Read(this.samples, 0, bytesToCopy);
      if (dwBytes == 0)
      {
        Array.Clear((Array) this.samples, 0, this.samples.Length);
        return 0;
      }
    }
    IntPtr audioPtr1;
    int audioBytes1;
    IntPtr audioPtr2;
    int audioBytes2;
    this.secondaryBuffer.Lock(this.nextSamplesWriteIndex, (uint) dwBytes, out audioPtr1, out audioBytes1, out audioPtr2, out audioBytes2, DirectSoundOut.DirectSoundBufferLockFlag.None);
    if (audioPtr1 != IntPtr.Zero)
    {
      Marshal.Copy(this.samples, 0, audioPtr1, audioBytes1);
      if (audioPtr2 != IntPtr.Zero)
        Marshal.Copy(this.samples, 0, audioPtr1, audioBytes1);
    }
    this.secondaryBuffer.Unlock(audioPtr1, audioBytes1, audioPtr2, audioBytes2);
    return dwBytes;
  }

  [DllImport("dsound.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  private static extern void DirectSoundCreate(
    ref Guid GUID,
    [MarshalAs(UnmanagedType.Interface)] out DirectSoundOut.IDirectSound directSound,
    IntPtr pUnkOuter);

  [DllImport("dsound.dll", EntryPoint = "DirectSoundEnumerateA", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  private static extern void DirectSoundEnumerate(
    DirectSoundOut.DSEnumCallback lpDSEnumCallback,
    IntPtr lpContext);

  [DllImport("user32.dll")]
  private static extern IntPtr GetDesktopWindow();

  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  internal class BufferDescription
  {
    public int dwSize;
    [MarshalAs(UnmanagedType.U4)]
    public DirectSoundOut.DirectSoundBufferCaps dwFlags;
    public uint dwBufferBytes;
    public int dwReserved;
    public IntPtr lpwfxFormat;
    public Guid guidAlgo;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  internal class BufferCaps
  {
    public int dwSize;
    public int dwFlags;
    public int dwBufferBytes;
    public int dwUnlockTransferRate;
    public int dwPlayCpuOverhead;
  }

  internal enum DirectSoundCooperativeLevel : uint
  {
    DSSCL_NORMAL = 1,
    DSSCL_PRIORITY = 2,
    DSSCL_EXCLUSIVE = 3,
    DSSCL_WRITEPRIMARY = 4,
  }

  [Flags]
  internal enum DirectSoundPlayFlags : uint
  {
    DSBPLAY_LOOPING = 1,
    DSBPLAY_LOCHARDWARE = 2,
    DSBPLAY_LOCSOFTWARE = 4,
    DSBPLAY_TERMINATEBY_TIME = 8,
    DSBPLAY_TERMINATEBY_DISTANCE = 16, // 0x00000010
    DSBPLAY_TERMINATEBY_PRIORITY = 32, // 0x00000020
  }

  internal enum DirectSoundBufferLockFlag : uint
  {
    None,
    FromWriteCursor,
    EntireBuffer,
  }

  [Flags]
  internal enum DirectSoundBufferStatus : uint
  {
    DSBSTATUS_PLAYING = 1,
    DSBSTATUS_BUFFERLOST = 2,
    DSBSTATUS_LOOPING = 4,
    DSBSTATUS_LOCHARDWARE = 8,
    DSBSTATUS_LOCSOFTWARE = 16, // 0x00000010
    DSBSTATUS_TERMINATED = 32, // 0x00000020
  }

  [Flags]
  internal enum DirectSoundBufferCaps : uint
  {
    DSBCAPS_PRIMARYBUFFER = 1,
    DSBCAPS_STATIC = 2,
    DSBCAPS_LOCHARDWARE = 4,
    DSBCAPS_LOCSOFTWARE = 8,
    DSBCAPS_CTRL3D = 16, // 0x00000010
    DSBCAPS_CTRLFREQUENCY = 32, // 0x00000020
    DSBCAPS_CTRLPAN = 64, // 0x00000040
    DSBCAPS_CTRLVOLUME = 128, // 0x00000080
    DSBCAPS_CTRLPOSITIONNOTIFY = 256, // 0x00000100
    DSBCAPS_CTRLFX = 512, // 0x00000200
    DSBCAPS_STICKYFOCUS = 16384, // 0x00004000
    DSBCAPS_GLOBALFOCUS = 32768, // 0x00008000
    DSBCAPS_GETCURRENTPOSITION2 = 65536, // 0x00010000
    DSBCAPS_MUTE3DATMAXDISTANCE = 131072, // 0x00020000
    DSBCAPS_LOCDEFER = 262144, // 0x00040000
  }

  internal struct DirectSoundBufferPositionNotify
  {
    public uint dwOffset;
    public IntPtr hEventNotify;
  }

  [Guid("279AFA83-4981-11CE-A521-0020AF0BE560")]
  [SuppressUnmanagedCodeSecurity]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IDirectSound
  {
    void CreateSoundBuffer(
      [In] DirectSoundOut.BufferDescription desc,
      [MarshalAs(UnmanagedType.Interface)] out object dsDSoundBuffer,
      IntPtr pUnkOuter);

    void GetCaps(IntPtr caps);

    void DuplicateSoundBuffer(
      [MarshalAs(UnmanagedType.Interface), In] DirectSoundOut.IDirectSoundBuffer bufferOriginal,
      [MarshalAs(UnmanagedType.Interface), In] DirectSoundOut.IDirectSoundBuffer bufferDuplicate);

    void SetCooperativeLevel(IntPtr HWND, [MarshalAs(UnmanagedType.U4), In] DirectSoundOut.DirectSoundCooperativeLevel dwLevel);

    void Compact();

    void GetSpeakerConfig(IntPtr pdwSpeakerConfig);

    void SetSpeakerConfig(uint pdwSpeakerConfig);

    void Initialize([MarshalAs(UnmanagedType.LPStruct), In] Guid guid);
  }

  [Guid("279AFA85-4981-11CE-A521-0020AF0BE560")]
  [SuppressUnmanagedCodeSecurity]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IDirectSoundBuffer
  {
    void GetCaps([MarshalAs(UnmanagedType.LPStruct)] DirectSoundOut.BufferCaps pBufferCaps);

    void GetCurrentPosition(out uint currentPlayCursor, out uint currentWriteCursor);

    void GetFormat();

    [return: MarshalAs(UnmanagedType.I4)]
    int GetVolume();

    void GetPan(out uint pan);

    [return: MarshalAs(UnmanagedType.I4)]
    int GetFrequency();

    [return: MarshalAs(UnmanagedType.U4)]
    DirectSoundOut.DirectSoundBufferStatus GetStatus();

    void Initialize([MarshalAs(UnmanagedType.Interface), In] DirectSoundOut.IDirectSound directSound, [In] DirectSoundOut.BufferDescription desc);

    void Lock(
      int dwOffset,
      uint dwBytes,
      out IntPtr audioPtr1,
      out int audioBytes1,
      out IntPtr audioPtr2,
      out int audioBytes2,
      [MarshalAs(UnmanagedType.U4)] DirectSoundOut.DirectSoundBufferLockFlag dwFlags);

    void Play(uint dwReserved1, uint dwPriority, [MarshalAs(UnmanagedType.U4), In] DirectSoundOut.DirectSoundPlayFlags dwFlags);

    void SetCurrentPosition(uint dwNewPosition);

    void SetFormat([In] WaveFormat pcfxFormat);

    void SetVolume(int volume);

    void SetPan(uint pan);

    void SetFrequency(uint frequency);

    void Stop();

    void Unlock(IntPtr pvAudioPtr1, int dwAudioBytes1, IntPtr pvAudioPtr2, int dwAudioBytes2);

    void Restore();
  }

  [Guid("b0210783-89cd-11d0-af08-00a0c925cd16")]
  [SuppressUnmanagedCodeSecurity]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IDirectSoundNotify
  {
    void SetNotificationPositions(
      uint dwPositionNotifies,
      [MarshalAs(UnmanagedType.LPArray), In] DirectSoundOut.DirectSoundBufferPositionNotify[] pcPositionNotifies);
  }

  private delegate bool DSEnumCallback(
    IntPtr lpGuid,
    IntPtr lpcstrDescription,
    IntPtr lpcstrModule,
    IntPtr lpContext);
}
