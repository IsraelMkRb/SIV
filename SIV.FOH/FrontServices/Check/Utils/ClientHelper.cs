using System;
using SIV.Entities;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Utils
{
    static class ClientHelper
    {
        public static SIVResponse Send(SIVRequest Request)
        {
            try
            {
             
                Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
                socketClient.Connect(host: Properties.Settings.Default.CtrlServer,
                                     port: Properties.Settings.Default.SrvPort);
                
                byte[] Request_InBufferForm = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Request));
                socketClient.Send(Request_InBufferForm);

                byte[] buffer = new byte[1024];
                int response_FromServer = socketClient.Receive(buffer);
                byte[] responseInBufferForm = new byte[response_FromServer];
                Array.Copy(sourceArray: buffer, destinationArray: responseInBufferForm, length: response_FromServer);
                string responseInString = Encoding.ASCII.GetString(responseInBufferForm);
                SIVResponse response = JsonConvert.DeserializeObject<SIVResponse>(responseInString);

                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();
                socketClient.Dispose();
                return response;
            }
            catch (Exception ex)
            {
                return new SIVResponse
                {
                    Code = CodeResponse.Error,
                    Data = ex.Message
                };
            }
        }
    }
}
