using System;
using SIV.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using SIV.General;
using Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlServices
{
    class Actions
    {

        public static readonly Dictionary<string, Func<SIVParameters, SIVResponse>> keyValuePairs = new Dictionary<string, Func<SIVParameters, SIVResponse>>
        {
            { "Login", EmployeesMethods.Login},
            { "Login_FromTerminal", EmployeesMethods.Login_FromTerminal },
            { "SetUserBlockedByTerminal", EmployeesMethods.SetUserBlockedByTerminal }
        };
    }

    class EmployeesMethods
    {
        public static SIVResponse Login(SIVParameters Parameters)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            SIVResponse response = new SIVResponse();

            int? IdEmployee = (int)Connection.ExecuteScalar
            (
                query: "SELECT Emp_Id FROM general.Employees " +
                       "WHERE (Emp_User = @EmpUser AND Emp_Password = @EmpPassword)",
                commandType: CommandType.Text,
                parameters: (IDbDataParameter[])(new[]
                {
                    Connection.CreateParameter( ParameterName: "@EmpUser", Value: Parameters.EmployeeParameters.User ),
                    Connection.CreateParameter( ParameterName:"@EmpPassword", Value: SIV.EcriptionHelper.Decrypt(Parameters.EmployeeParameters.Password,rsa.ExportParameters(true)))
                })
            );

            if (IdEmployee.HasValue)
            {
                IDataReader reader = Connection.ExecuteReader
                    (
                        query: "SELECT * FROM general.vw_Employees " +
                               "WHERE Emp_Id = @EmpId",
                        commandType: CommandType.Text,
                        parameters: (IDbDataParameter[])(new[]
                        {
                            Connection.CreateParameter(ParameterName: "@EmpId",Value: IdEmployee)
                        })
                    );

                while (reader.Read())
                {
                    response.Code = CodeResponse.OK;
                    response.Data = SIV.General.ConvertHelper.ConvertDataReaderToObject<Employee>(reader);
                }
            }
            else
            {
                response.Code = CodeResponse.Error;
                response.Data = "Usuario no registrado en sistema";
            }

            rsa.Dispose();
            return response;
        }
        public static SIVResponse Login_FromTerminal(SIVParameters parameters)
        {
            SIVResponse response = new SIVResponse();
            IDataReader reader = Connection.ExecuteReader
                (
                    query: "SELECT ID FROM general.vw_Employees WHERE Password = @EmpPassword",
                    commandType: CommandType.Text,
                    parameters: (IDbDataParameter[])(new[]
                    {
                        Connection.CreateParameter(ParameterName: "@EmpPassword", Value: parameters.EmployeeParameters.Password)
                    })
                );

            if (reader.Read())
            {
                response.Code = CodeResponse.OK;
                response.Data = ConvertHelper.ConvertDataReaderToObject<Employee>(reader);
            }
            else
            {
                response.Code = CodeResponse.Error;
                response.Data = "No se encontrol usuario con ese registro";
            }

            reader.Close();
            return response;
        }
        public static SIVResponse SetUserBlockedByTerminal(SIVParameters parameters)
        {
            Connection.ExecuteNonQuery
                (
                    query: "UPDATE general.Employees set Emp_LoggedInTerminal = @TerminalSource where Emp_Id = @EmpId",
                    commandType: CommandType.Text,
                    parameters: new[]
                    {
                        Connection.CreateParameter("@EmpId",parameters.EmployeeParameters.ID),
                        Connection.CreateParameter("@TerminalSource",parameters.TerminalSource)
                    }
                );

            return new SIVResponse { Code = CodeResponse.OK, Data = "OK" };
        }
    }
}
