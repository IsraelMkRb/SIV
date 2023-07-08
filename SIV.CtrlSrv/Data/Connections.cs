using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Data
{
    

    public static class Connection
    {
        private static string connectionString;

        public static bool ContainsColumn(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }
        public static IDataParameter CreateParameter(string ParameterName, object Value)
        {
            IDataParameter parameter = CreateConnection().CreateCommand().CreateParameter();

            parameter.ParameterName = ParameterName;
            parameter.Value = Value;

            return parameter;
        }

        public static void SetConnectionString(string connectionStringName)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString;
        }

        public static void ExecuteNonQuery(string query, CommandType commandType = CommandType.Text, params IDbDataParameter[] parameters)
        {
            using (IDbConnection connection = CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = CreateCommand(connection, query, commandType, parameters))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string query, CommandType commandType = CommandType.Text, params IDbDataParameter[] parameters)
        {
            using (IDbConnection connection = CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = CreateCommand(connection, query, commandType, parameters))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        public static IDataReader ExecuteReader(string query, CommandType commandType = CommandType.Text, params IDbDataParameter[] parameters)
        {
            IDbConnection connection = CreateConnection();
            connection.Open();
            IDbCommand command = CreateCommand(connection, query, commandType, parameters);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        private static IDbConnection CreateConnection()
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("La cadena de conexión no ha sido configurada.");

            return new SqlConnection(connectionString);
        }

        private static IDbCommand CreateCommand(IDbConnection connection, string query, CommandType commandType, IDbDataParameter[] parameters)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = commandType;

            if (parameters != null)
            {
                foreach (IDbDataParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
    }
}