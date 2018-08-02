using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace StreamSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pipe = new Pipe();
            await WriteDataAsync(pipe.Writer);
            pipe.Writer.Complete();
            await ReadDataAsync(pipe.Reader);
        }

        private static async Task ReadDataAsync(PipeReader reader)
        {
            while (true)
            {
                // await some data being available
                ReadResult read = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = read.Buffer;
                // check whether we've reached the end
                // and processed everything
                if (buffer.IsEmpty && read.IsCompleted)
                    break; // exit loop

                // process what we received
                foreach (ReadOnlyMemory<byte> segment in buffer)
                {
                    string s = Encoding.UTF8.GetString(segment.Span);
                    Console.WriteLine(s);
                }
                // tell the pipe that we used everything
                reader.AdvanceTo(buffer.End);

            }
        }

        private static async Task WriteDataAsync(PipeWriter writer)
        {
            string hello = "Hello, Pipeline!";
            Memory<byte> memory = writer.GetMemory();
            int bytes = Encoding.UTF8.GetBytes(hello, memory.Span);
            writer.Advance(bytes);
            await writer.FlushAsync();
        }
    }
}
