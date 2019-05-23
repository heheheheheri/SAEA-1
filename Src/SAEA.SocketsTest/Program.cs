using SAEA.FileSocket.Model;
using SAEA.Sockets.Core.Tcp;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAEA.SocketsTest
{
    class Program
    {
        private static void TestIocpClient()
        {
            bool Connected = false;

            IocpClientSocket iocpClient = new IocpClientSocket(new Context());
            iocpClient.OnReceive += IocpClient_OnReceive;
            iocpClient.ConnectAsync((state) =>
            {
                if (state == System.Net.Sockets.SocketError.Success)
                {
                    Connected = true;
                }
            });

            Task.Run(()=> {

                string msg = "Can you feel the love tonight?";
                int nCount = 0;
                while (true)
                {
                    if (Connected)
                    {
                        iocpClient.SendAsync(Encoding.Default.GetBytes(msg));
                    }

                    if (nCount++ > 100)
                    {
                        break;
                    }

                    Thread.Sleep(1000);
                }
            });

        }

        private static void IocpClient_OnReceive(byte[] data)
        {
            string msg = Encoding.Default.GetString(data);
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + ":" + msg);
        }

        static void Main(string[] args)
        {
            TestIocpClient();

            Console.ReadLine();
        }
    }
}
