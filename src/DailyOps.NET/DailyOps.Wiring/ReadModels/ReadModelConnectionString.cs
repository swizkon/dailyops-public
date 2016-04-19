using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Wiring.ReadModels
{
    public sealed class ReadModelConnectionString
    {
        private string connectionString;

        public ReadModelConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public static explicit operator ReadModelConnectionString(string s)
        {
            return new ReadModelConnectionString(s);
        }

        public static explicit operator string(ReadModelConnectionString obj)
        {
            return obj.connectionString;
        }
    }
}
