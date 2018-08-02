using System;
using System.IO;
using System.Text;

namespace StreamSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stream = new MemoryStream())
            {
                WriteData(stream);
                stream.Position = 0;
                ReadData(stream);
            }
        }

        private static void ReadData(Stream stream)
        {
            const int BufferSize = 512;
            int bytesRead;
            byte[] buffer = new byte[BufferSize];
            do
            {
                bytesRead = stream.Read(buffer, 0, BufferSize);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                }

            } while (bytesRead > 0);
        }

        private static void WriteData(Stream stream)
        {
            string hello = "Hello, Stream!";
            byte[] bytes = Encoding.UTF8.GetBytes(hello);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }
}
