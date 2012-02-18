using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;
using ServiceStack.OrmLite.SqlServer;

namespace ServiceStack.OrmLite
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }

    [TestFixture]
    public class ServiceStackOrmLiteTests
    {
        [Test]
        public void BasicSelect()
        {
            var dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString,SqlServerOrmLiteDialectProvider.Instance);
            var conn = dbFactory.OpenDbConnection();
            var cmd = conn.CreateCommand();
            var artists = cmd.Select<Artist>();
        }

        [Test]
        public void BasicPaging()
        {
            //Nope
        }
    }
}
