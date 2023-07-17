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
        private Socket ClientRequest;

        private readonly string PathFlag = Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "isWorking");
        #region Main Events
        protected override void OnStart(string[] args)
        {
            try
            {
                if (!IsPortAvaible()) { throw new Exception("Puerto ocupado por otra aplicación. Favor de revisar."); }
                if (!File.Exists(PathFlag))
                {
                    FileStream fs = File.Create(PathFlag);
                    fs.Close();
                }

                Thread thread = new Thread(Loop);
                thread.Start();
            }
            catch (Exception Ex)
            {
                Logs.Write(Ex);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            if (File.Exists(PathFlag)) { File.Delete(PathFlag); }
        }
        #endregion
        #region Methods
        private bool IsPortAvaible()
        {
            try
            {
                var listener = new TcpListener(IPAddress.Loopback, Properties.Settings.Default.Port);
                listener.Start();
                listener.Stop();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }
        private void Loop()
        {
            try
            {
                Socket = new Socket(addressFamily: AddressFamily.InterNetwork,
                                socketType: SocketType.Stream,
                                protocolType: ProtocolType.Tcp);

                IPAddress iPAddress = IPAddress.Parse("127.0.0.1");

                Socket.Bind(new IPEndPoint(address: iPAddress,
                                           port: Properties.Settings.Default.Port));
                Socket.Listen(1);

                while (File.Exists(PathFlag))
                {
                    Logs.Write($"Escuchando en el puerto: {Properties.Settings.Default.Port}");
                    ClientRequest = Socket.Accept();
                    Logs.Write($"Cliente conectado: {ClientRequest.RemoteEndPoint}");

                    byte[] infoReceived = new byte[1024];
                    int bytesRead = ClientRequest.Receive(infoReceived);
                    string requestData = Encoding.UTF8.GetString(infoReceived, 0, bytesRead);

                    SIVRequest Request = JsonConvert.DeserializeObject<SIVRequest>(requestData);
                    if (Request is null)
                    {
                        throw new Exception("La peticion de la terminal tiene un formato incorrecta");
                    }
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

                    if (ClientRequest.Connected) ClientRequest.Shutdown(SocketShutdown.Both);
                     
                    ClientRequest.Close();
                    ClientRequest.Dispose();
                }
            }
            catch (Exception Ex)
            {
                Logs.Write(Ex);
                string responseError = JsonConvert.SerializeObject(new SIVResponse
                {
                    Code = CodeResponse.Error,
                    Data = Ex.Message
                });
                byte[] ResultInBufferForm = Encoding.UTF8.GetBytes(responseError);
                ClientRequest.Send(ResultInBufferForm);
                this.Stop();
            }
        }
        #endregion
    }
}
