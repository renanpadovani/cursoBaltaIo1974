using System;
using BaltaStore.Shared;
using Microsoft.Data.SqlClient;


namespace BaltaStore.Infra.StoreContext.DataContexts
{
    public class BaltaDataContext : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public BaltaDataContext()
        {
            Connection = new SqlConnection(Settings.ConnectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            if (Connection.State != System.Data.ConnectionState.Closed)
                Connection.Close();
        }
    }
}
