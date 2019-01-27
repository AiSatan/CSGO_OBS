using CSStatsForNerf.Controllers;
using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatsForNerf.ConsoleApp
{
    class Program
    {
        private static Stopwatch _stopWatch;

        static async Task Main(string[] args)
        {
            var server = new AsyncServer();
            server.RunAsync();
            Console.ReadKey();

            // Console.WriteLine("Hello World!");
            //var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, 12069));

            //Console.WriteLine("Listening on port 12069");

            //listenSocket.Listen(120);

            //while (true)
            //{
            //    Console.WriteLine("NEW MSG!!!!!");
            //    var socket = await listenSocket.AcceptAsync();
            //    _stopWatch = Stopwatch.StartNew();
            //    _ = ProcessLinesAsync(socket);

            //    _stopWatch.Stop();
            //    Console.WriteLine(_stopWatch.ElapsedMilliseconds);
            //}
        }


        static Task ProcessLinesAsync(Socket socket)
        {
            var pipe = new Pipe();
            var fill = FillPipeAsync(socket, pipe.Writer);
            var read = ReadPipeAsync(pipe.Reader);
            return Task.WhenAll(fill, read);
        }

        static async Task FillPipeAsync(Socket socket, PipeWriter writer)
        {
            const int minimumBufferSize = 4096;

            while (true)
            {
                var memory = writer.GetMemory(minimumBufferSize);

                try
                {
                    var bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);

                    if (bytesRead == 0)
                    {
                        break;
                    }
                    writer.Advance(bytesRead);
                }
                catch (Exception)
                {
                    break;
                }

                var result = await writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }
            }

            writer.Complete();
        }

        static async Task ReadPipeAsync(PipeReader reader)
        {
            while (true)
            {
                var result = await reader.ReadAsync();
                if (result.Buffer.Length == 0)
                {
                    return;
                }
                var buffer = result.Buffer;
                //SequencePosition? position = null;

                //do
                //{
                //    position = buffer.PositionOf((byte)'\n');

                //    if (position != null)
                //    {
                //        var line = Encoding.UTF8.GetString(buffer.Slice(0, position.Value).ToArray());
                //        Console.WriteLine(line);

                //        // Skip the line + the \n character (basically position)
                //        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                //    }
                //}
                //while (position != null);

                reader.AdvanceTo(buffer.Start, buffer.End);

                if (result.IsCompleted)
                {
                    var line = Encoding.UTF8.GetString(result.Buffer.ToArray());
                    var jsonStart = line.IndexOf("{");
                    if (jsonStart >= 0)
                    {
                        line = line.Remove(0, jsonStart);
                        //Console.WriteLine(line);
                        var obj = JsonConvert.DeserializeObject<Model>(line);
                        lock (TwitchConnector._locker)
                        {
                            TwitchConnector.Execute(obj);
                        }
                    }
                    else
                    {
                        Console.WriteLine(line);
                    }
                    break;
                }
            }

            reader.Complete();
        }


        public class AsyncServer
        {
            public async Task RunAsync()
            {
                var listener = new HttpListener();

                listener.Prefixes.Add("http://localhost:12069/");
                listener.Prefixes.Add("http://127.0.0.1:12069/");

                listener.Start();

                while (true)
                {
                    var context = await listener.GetContextAsync();
                    Console.WriteLine("New");
                    _ = Task.Factory.StartNew(() => HandleRequest(context));
                    var context2 = await listener.GetContextAsync();
                    Console.WriteLine("New");
                    _ = Task.Factory.StartNew(() => HandleRequest(context2));
                }
            }

            private void HandleRequest(object state)
            {
                try
                {
                    var context = (HttpListenerContext)state;

                    context.Response.StatusCode = 200;
                    //context.Response.SendChunked = true;

                    using (var reader = new StreamReader(context.Request.InputStream))
                    {
                        var body = reader.ReadToEnd();
                        var obj = JsonConvert.DeserializeObject<Model>(body);
                        lock (TwitchConnector._locker)
                        {
                            TwitchConnector.Execute(obj);
                        }
                    }
                }
                catch (Exception)
                {
                    // Client disconnected or some other error - ignored for this example
                }
            }
        }
    }

}
