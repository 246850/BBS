using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BBS.Framework.Extensions
{
    public static class DbParameterExtensions
    {
        public static IDbDataParameter CreateParameter(this IDbConnection connection, string name, object value)
        {
            using (var cmd = connection.CreateCommand())
            {
                var param = cmd.CreateParameter();
                param.ParameterName = name;
                param.Value = value;
                return param;
            }
        }

        public static List<IDbDataParameter> CreateParameter(this IDbConnection connection, object p)
        {
            var type = p.GetType();
            var props = type.GetProperties();
            using (var cmd = connection.CreateCommand())
            {
                return props.Select(x =>
                {
                    var param = cmd.CreateParameter();
                    param.ParameterName = x.Name;
                    param.Value = x.GetValue(p);
                    return param;
                }).ToList();
            }
        }
    }
}
