using PseudoCode.Core.Runtime.Instances;

namespace PseudoCode.Core.Runtime;

public class PseudoFileStream : IDisposable
{
    public FileAccess FileAccess;
    public FileMode FileMode;
    public FileStream FileStream;
    public bool IsBinary;
    public PseudoBinaryStream PseudoBinaryStream;
    public StreamReader StreamReader;
    public StreamWriter StreamWriter;

    public PseudoFileStream(string path, FileMode fileMode, FileAccess fileAccess, bool isBinary)
    {
        FileMode = fileMode;
        FileAccess = fileAccess;
        IsBinary = isBinary;
        Path = path;
    }

    public string Path { get; set; }


    public void Dispose()
    {
        FileStream?.Dispose();
        StreamReader?.Dispose();
        StreamWriter?.Dispose();
    }

    public void Open()
    {
        if (IsBinary)
        {
            PseudoBinaryStream = File.Exists(Path)
                ? PseudoBinaryStream.From(new MemoryStream(File.ReadAllBytes(Path)))
                : new PseudoBinaryStream();
        }
        else
        {
            FileStream = File.Open(Path, FileMode, FileAccess);
            if (FileAccess.HasFlag(FileAccess.Read))
                StreamReader = new StreamReader(FileStream);
            if (FileAccess.HasFlag(FileAccess.Write))
                StreamWriter = new StreamWriter(FileStream);
        }
    }

    public string ReadLine()
    {
        return StreamReader.ReadLine();
    }

    public void Write(string content)
    {
        StreamWriter.Write(content);
    }

    public void Close()
    {
        StreamReader?.Close();
        StreamWriter?.Close();
        if (!IsBinary) return;
        var memoryStream = new MemoryStream();
        PseudoBinaryStream?.Write(memoryStream);
        FileStream = File.Open(Path, FileMode.OpenOrCreate, FileAccess.Write);
        memoryStream.WriteTo(FileStream);
        memoryStream.Close();
        FileStream.Close();
    }

    public void Seek(int address)
    {
        PseudoBinaryStream.Seek(address);
    }

    public void Put(Instance i)
    {
        PseudoBinaryStream.Put(i);
    }

    public Instance Get()
    {
        return PseudoBinaryStream.Get();
    }

    public bool Eof()
    {
        return !IsBinary && StreamReader.EndOfStream;
    }
}