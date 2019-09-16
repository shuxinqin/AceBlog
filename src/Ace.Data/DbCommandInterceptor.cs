using Chloe.Infrastructure.Interception;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Ace.Data
{
    class DbCommandInterceptor : IDbCommandInterceptor
    {
        public void ReaderExecuting(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext)
        {
#if DEBUG
            Debug.WriteLine(AppendDbCommandInfo(command));
            Console.WriteLine(command.CommandText);
#endif
        }
        public void ReaderExecuted(IDbCommand command, DbCommandInterceptionContext<IDataReader> interceptionContext)
        {
            if (interceptionContext.Exception == null)
                Console.WriteLine(interceptionContext.Result.FieldCount);
        }

        public void NonQueryExecuting(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {

#if DEBUG
            Debug.WriteLine(AppendDbCommandInfo(command));
            Console.WriteLine(command.CommandText);
#endif
        }
        public void NonQueryExecuted(IDbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if (interceptionContext.Exception == null)
                Console.WriteLine(interceptionContext.Result);
        }

        public void ScalarExecuting(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
#if DEBUG
            Debug.WriteLine(AppendDbCommandInfo(command));
            Console.WriteLine(command.CommandText);
#endif
        }
        public void ScalarExecuted(IDbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            if (interceptionContext.Exception == null)
                Console.WriteLine(interceptionContext.Result);
        }

        public static string AppendDbCommandInfo(IDbCommand command)
        {
            StringBuilder sb = new StringBuilder();

            foreach (IDbDataParameter param in command.Parameters)
            {
                if (param == null)
                    continue;

                object value = null;
                if (param.Value == null || param.Value == DBNull.Value)
                {
                    value = "NULL";
                }
                else
                {
                    value = param.Value;

                    if (param.DbType == DbType.String || param.DbType == DbType.AnsiString || param.DbType == DbType.DateTime)
                        value = "'" + value + "'";
                }

                sb.AppendFormat("{3} {0} {1} = {2};", Enum.GetName(typeof(DbType), param.DbType), param.ParameterName, value, Enum.GetName(typeof(ParameterDirection), param.Direction));
                sb.AppendLine();
            }

            sb.AppendLine(command.CommandText);

            return sb.ToString();
        }
    }

}
