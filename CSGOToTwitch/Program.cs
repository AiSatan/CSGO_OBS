using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSGOToTwitch
{
    public class Program
    {

        public static event Action OnKIll = delegate { };
        public static bool IsKilledAlready = false;
        private static Random rand = new Random();
        private static bool isRun = true;

        public enum Gifs
        {
            None = 0,
            Barney = 1,
            Crap = 2
        }

        static void Main(string[] args)
        {
            OnKIll += Program_OnKIll;
            //Task.Factory.StartNew(() =>
            //{
            //    while (isRun)
            //    {
            //        try
            //        {
            //            var rect = new Rectangle(1600, 0, 300, 200);
            //            var engine = GetEngine(rect);
            //            var allText = engine.GetUTF8Text();

            //            Console.WriteLine(allText);
            //            if (allText.ToUpper().Trim().LastIndexOf("AiSatan".ToUpper()) > 3)
            //            {
            //                Console.WriteLine(allText);
            //                OnKIll();
            //            }
            //            Thread.Sleep(100);
            //        }
            //        catch { }
            //    }
            //});

            //Console.WriteLine("Working...");
            //Console.ReadKey();
            //isRun = false;
            //Console.WriteLine("Stopped.");

            Task.Factory.StartNew(() =>
            {
                Server();
            });

            Console.ReadKey();
            isRun = false;
        }


        private static void Server()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 12069);
            // we set our IP address as server's address, and we also set the port: 9999

            server.Start();  // this will start the server

            while (isRun)   //we wait for a connection
            {
                TcpClient client = server.AcceptTcpClient();  //if a connection exists, the server will accept it

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                //byte[] hello = new byte[100];   //any message must be serialized (converted to byte array)
                //hello = Encoding.Default.GetBytes("hello world");  //conversion string => byte array

                //ns.Write(hello, 0, hello.Length);     //sending the message

                while (client.Connected && isRun)  //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024];     //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                    var res = Encoding.Default.GetString(msg).Trim('\0');
                    if (!string.IsNullOrWhiteSpace(res)) 
                    {
                        Console.Write(res); //now , we write the message as string
                    }
                }
                Console.WriteLine("Disconnected"); //now , we write the message as string
            }
        }


        private static void Program_OnKIll()
        {
            Task.Factory.StartNew(() =>
            {
                if (IsKilledAlready)
                {
                    return;
                }
                IsKilledAlready = true;
                File.WriteAllText("test.txt", rand.Next(0, 100) >= 50 ? Gifs.Barney.GifToString() : Gifs.Crap.GifToString());
                Thread.Sleep(3000);
                File.WriteAllText("test.txt", Gifs.None.GifToString());
                IsKilledAlready = false;
            });
        }

        protected static Tesseract GetEngine(Rectangle rectangle)
        {
            var bounds = new Size((int)rectangle.Width, (int)rectangle.Height);

            using (var bitmap = new Bitmap((int)rectangle.Width, (int)rectangle.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen((int)rectangle.X, (int)rectangle.Y, 0, 0, bounds);
                }

                var lang = "eng";

                var engine = new Tesseract(@"./tessdata", lang, OcrEngineMode.LstmOnly);

                using (var image = new Image<Bgr, byte>(bitmap))
                {
                    //136//199/229
                    var image2 = image.InRange(new Bgr(10, 10, 10), new Bgr(200, 200, 200));
                    //var path = Path.Combine(Environment.CurrentDirectory, i.ToString() + ".png");
                    //image2.Save(path);
                    engine.SetImage(image2);
                    engine.Recognize();
                }
                return engine;
            }
        }
    }
}
