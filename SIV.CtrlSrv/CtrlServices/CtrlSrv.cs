using System;
using Newtonsoft.Json;
using System.Text;
using System.ServiceProcess;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using SIV.Entities;

namespace CtrlServices
{
    public partial class CtrlSrv : ServiceBase
    {
        public CtrlSrv()
        {
            InitializeComponent();
        }

        private Socket Socket;

        #region Main Events
        protected override void OnStart(string[] args)
        {

            try
            {
                if (!IsPortAvaible()) { throw new Exception("Puerto ocupado por otra aplicación. Favor de revisar."); }

                if (!File.Exists("isWorking"))
                {
                    FileStream fs = File.Create("isWorking");
                    fs.Close();
                }

                Data.Connection.SetConnectionString(connectionStringName: Properties.Settings.Default.bdcontext);

                Thread thread = new Thread(Loop);
                thread.Start();
            }
            catch (Exception)
            {
                this.Stop();
                throw;
            }
        }

        protected override void OnStop()
        {
            if (File.Exists("isWorking")) { File.Delete("isWorking"); }
        }
        #endregion
        private bool IsPortAvaible()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(hostname: "LocalHost", port: Properties.Settings.Default.Port);
                    return client.Connected;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Loop()
        {
            Socket = new Socket(addressFamily: AddressFamily.InterNetwork,
                                socketType: SocketType.Stream,
                                protocolType: ProtocolType.Tcp);

            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");

            Socket.Bind(new IPEndPoint(address: iPAddress,
                                       port: Properties.Settings.Default.Port));
            Socket.Listen(10);

            while (File.Exists("isWorking"))
            {
                Socket ClientRequest = Socket.Accept();
                Logs.Write($"Cliente conectado: {ClientRequest.RemoteEndPoint}");

                byte[] infoReceived = new byte[1024];
                int bytesRead = 0;
                string requestData = Encoding.UTF8.GetString(infoReceived, 0, bytesRead);

                SIVRequest Request = JsonConvert.DeserializeObject<SIVRequest>(requestData);

                string resultInJSONForm;

                if (Actions.keyValuePairs.ContainsKey(key: Request.Method))
                {
                    var InternalFunction = Actions.keyValuePairs[Request.Method];
                    var result = InternalFunction.Invoke(Request.Parameters);

                    resultInJSONForm = JsonConvert.SerializeObject(result);
                }
                else
                {
                    resultInJSONForm = JsonConvert.SerializeObject(new SIVResponse
                    {
                        Code = CodeResponse.Error,
                        Data = "La funcion especificada no existre dentro del ambito"
                    });
                }

                byte[] ResultInBufferForm = Encoding.UTF8.GetBytes(resultInJSONForm);
                ClientRequest.Send(ResultInBufferForm);

                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                ClientRequest.Dispose();
            }
        }
    }
}
