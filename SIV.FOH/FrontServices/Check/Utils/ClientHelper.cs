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
             
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(host: ConfigurationManager.AppSettings["CtrlServer"],
                                     port: Convert.ToInt32(ConfigurationManager.AppSettings["SrvPort"]));

                byte[] RequestInBufferForm = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Request));
                clientSocket.Send(RequestInBufferForm);

                byte[] buffer = new byte[1024];
                int responseServer = clientSocket.Receive(buffer);
                byte[] responseInBufferForm = new byte[responseServer];
                Array.Copy(buffer, responseInBufferForm, responseServer);
                string responseInString = Encoding.ASCII.GetString(responseInBufferForm);
                SIVResponse response = JsonConvert.DeserializeObject<SIVResponse>(responseInString);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

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
