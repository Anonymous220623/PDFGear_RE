// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipSegmentedStream
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.IO;

#nullable disable
namespace Ionic.Zip;

internal class ZipSegmentedStream : Stream
{
  private ZipSegmentedStream.RwMode rwMode;
  private bool _exceptionPending;
  private string _baseName;
  private string _baseDir;
  private string _currentName;
  private string _currentTempName;
  private uint _currentDiskNumber;
  private uint _maxDiskNumber;
  private long _maxSegmentSize;
  private Stream _innerStream;

  private ZipSegmentedStream() => this._exceptionPending = false;

  public static ZipSegmentedStream ForReading(
    string name,
    uint initialDiskNumber,
    uint maxDiskNumber)
  {
    ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream();
    zipSegmentedStream.rwMode = ZipSegmentedStream.RwMode.ReadOnly;
    zipSegmentedStream.CurrentSegment = initialDiskNumber;
    zipSegmentedStream._maxDiskNumber = maxDiskNumber;
    zipSegmentedStream._baseName = name;
    zipSegmentedStream._SetReadStream();
    return zipSegmentedStream;
  }

  public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
  {
    return ZipSegmentedStream.ForWriting(name, (long) maxSegmentSize);
  }

  public static ZipSegmentedStream ForWriting(string name, long maxSegmentSize)
  {
    ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream()
    {
      rwMode = ZipSegmentedStream.RwMode.Write,
      CurrentSegment = 0,
      _baseName = name,
      _maxSegmentSize = maxSegmentSize,
      _baseDir = Path.GetDirectoryName(name)
    };
    if (zipSegmentedStream._baseDir == "")
      zipSegmentedStream._baseDir = ".";
    zipSegmentedStream._SetWriteStream(0U);
    return zipSegmentedStream;
  }

  public static Stream ForUpdate(string name, uint diskNumber)
  {
    return (Stream) File.Open($"{Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name))}.z{(ValueType) (uint) ((int) diskNumber + 1):D2}", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
  }

  public bool ContiguousWrite { get; set; }

  public uint CurrentSegment
  {
    get => this._currentDiskNumber;
    private set
    {
      this._currentDiskNumber = value;
      this._currentName = (string) null;
    }
  }

  public string CurrentName
  {
    get
    {
      if (this._currentName == null)
        this._currentName = this._NameForSegment(this.CurrentSegment);
      return this._currentName;
    }
  }

  public string CurrentTempName => this._currentTempName;

  private string _NameForSegment(uint diskNumber)
  {
    return $"{Path.Combine(Path.GetDirectoryName(this._baseName), Path.GetFileNameWithoutExtension(this._baseName))}.z{(ValueType) (uint) ((int) diskNumber + 1):D2}";
  }

  public uint ComputeSegment(int length)
  {
    return this._innerStream.Position + (long) length > this._maxSegmentSize ? this.CurrentSegment + 1U : this.CurrentSegment;
  }

  public override string ToString()
  {
    return $"{nameof (ZipSegmentedStream)}[{this.CurrentName}][{this.rwMode.ToString()}], pos=0x{this.Position:X})";
  }

  private void _SetReadStream()
  {
    if (this._innerStream != null)
      this._innerStream.Dispose();
    if ((int) this.CurrentSegment + 1 == (int) this._maxDiskNumber)
      this._currentName = this._baseName;
    this._innerStream = (Stream) File.OpenRead(this.CurrentName);
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (this.rwMode != ZipSegmentedStream.RwMode.ReadOnly)
    {
      this._exceptionPending = true;
      throw new InvalidOperationException("Stream Error: Cannot Read.");
    }
    int num1 = this._innerStream.Read(buffer, offset, count);
    int num2 = num1;
    while (num2 != count)
    {
      if (this._innerStream.Position != this._innerStream.Length)
      {
        this._exceptionPending = true;
        throw new ZipException($"Read error in file {this.CurrentName}");
      }
      if ((int) this.CurrentSegment + 1 == (int) this._maxDiskNumber)
        return num1;
      ++this.CurrentSegment;
      this._SetReadStream();
      offset += num2;
      count -= num2;
      num2 = this._innerStream.Read(buffer, offset, count);
      num1 += num2;
    }
    return num1;
  }

  private void _SetWriteStream(uint increment)
  {
    if (this._innerStream != null)
    {
      this._innerStream.Dispose();
      if (File.Exists(this.CurrentName))
        File.Delete(this.CurrentName);
      File.Move(this._currentTempName, this.CurrentName);
    }
    if (increment > 0U)
      this.CurrentSegment += increment;
    SharedUtilities.CreateAndOpenUniqueTempFile(this._baseDir, out this._innerStream, out this._currentTempName);
    if (this.CurrentSegment != 0U)
      return;
    this._innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    if (this.rwMode != ZipSegmentedStream.RwMode.Write)
    {
      this._exceptionPending = true;
      throw new InvalidOperationException("Stream Error: Cannot Write.");
    }
    if (this.ContiguousWrite)
    {
      if (this._innerStream.Position + (long) count > this._maxSegmentSize)
        this._SetWriteStream(1U);
    }
    else
    {
      while (this._innerStream.Position + (long) count > this._maxSegmentSize)
      {
        long num = this._maxSegmentSize - this._innerStream.Position;
        int count1 = num <= (long) buffer.Length ? (int) num : buffer.Length;
        this._innerStream.Write(buffer, offset, count1);
        this._SetWriteStream(1U);
        count -= count1;
        offset += count1;
      }
    }
    this._innerStream.Write(buffer, offset, count);
  }

  public long TruncateBackward(uint diskNumber, long offset)
  {
    if (this.rwMode != ZipSegmentedStream.RwMode.Write)
    {
      this._exceptionPending = true;
      throw new ZipException("bad state.");
    }
    if ((int) diskNumber == (int) this.CurrentSegment)
      return this._innerStream.Seek(offset, SeekOrigin.Begin);
    if (this._innerStream != null)
    {
      this._innerStream.Dispose();
      if (File.Exists(this._currentTempName))
        File.Delete(this._currentTempName);
    }
    for (uint diskNumber1 = this.CurrentSegment - 1U; diskNumber1 > diskNumber; --diskNumber1)
    {
      string path = this._NameForSegment(diskNumber1);
      if (File.Exists(path))
        File.Delete(path);
    }
    this.CurrentSegment = diskNumber;
    for (int index = 0; index < 3; ++index)
    {
      try
      {
        this._currentTempName = Path.Combine(Path.GetDirectoryName(this.CurrentName), SharedUtilities.InternalGetTempFileName());
        File.Move(this.CurrentName, this._currentTempName);
        break;
      }
      catch (IOException ex)
      {
        if (index == 2)
          throw;
      }
    }
    this._innerStream = (Stream) new FileStream(this._currentTempName, FileMode.Open);
    return this._innerStream.Seek(offset, SeekOrigin.Begin);
  }

  public override bool CanRead
  {
    get
    {
      return this.rwMode == ZipSegmentedStream.RwMode.ReadOnly && this._innerStream != null && this._innerStream.CanRead;
    }
  }

  public override bool CanSeek => this._innerStream != null && this._innerStream.CanSeek;

  public override bool CanWrite
  {
    get
    {
      return this.rwMode == ZipSegmentedStream.RwMode.Write && this._innerStream != null && this._innerStream.CanWrite;
    }
  }

  public override void Flush() => this._innerStream.Flush();

  public override long Length => this._innerStream.Length;

  public override long Position
  {
    get => this._innerStream.Position;
    set => this._innerStream.Position = value;
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    return this._innerStream.Seek(offset, origin);
  }

  public override void SetLength(long value)
  {
    if (this.rwMode != ZipSegmentedStream.RwMode.Write)
    {
      this._exceptionPending = true;
      throw new InvalidOperationException();
    }
    this._innerStream.SetLength(value);
  }

  protected override void Dispose(bool disposing)
  {
    try
    {
      if (this._innerStream == null)
        return;
      this._innerStream.Dispose();
      if (this.rwMode != ZipSegmentedStream.RwMode.Write)
        return;
      int num = this._exceptionPending ? 1 : 0;
    }
    finally
    {
      base.Dispose(disposing);
    }
  }

  private enum RwMode
  {
    None,
    ReadOnly,
    Write,
  }
}
