namespace PseudoCode.Core.Runtime;

public class PseudoFileStream : IDisposable
{
    public FileMode FileMode;
    public FileAccess FileAccess;
    public bool IsBinary;
    public FileStream FileStream;
    public BinaryReader BinaryReader;
    public BinaryWriter BinaryWriter;
    public StreamReader StreamReader;
    public StreamWriter StreamWriter;
    public string Path { get; set; }

    public PseudoFileStream(string path, FileMode fileMode, FileAccess fileAccess, bool isBinary)
    {
        FileMode = fileMode;
        FileAccess = fileAccess;
        IsBinary = isBinary;
        Path = path;
    }

    public void Open()
    {
        var stream = File.Open(Path, FileMode, FileAccess);
        if (IsBinary)
        {
            if (FileAccess.HasFlag(FileAccess.Read))
                BinaryReader = new BinaryReader(stream);
            if (FileAccess.HasFlag(FileAccess.Write))
                BinaryWriter = new BinaryWriter(stream);
        }
        else
        {
            if (FileAccess.HasFlag(FileAccess.Read))
                StreamReader = new StreamReader(stream);
            if (FileAccess.HasFlag(FileAccess.Write))
                StreamWriter = new StreamWriter(stream);
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
        BinaryReader?.Close();
        BinaryWriter?.Close();
    }

    public bool Eof()
    {
        return IsBinary ? BinaryReader.BaseStream.Position == BinaryReader.BaseStream.Length:
                StreamReader.EndOfStream;
    }


    public void Dispose()
    {
        FileStream?.Dispose();
        BinaryReader?.Dispose();
        BinaryWriter?.Dispose();
        StreamReader?.Dispose();
        StreamWriter?.Dispose();
    }
}