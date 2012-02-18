using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Dapper
{
    [TestFixture]
    public class DapperTests
    {
        public class Artist
        {
            public int ArtistId { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void BasicSelect()
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                conn.Open();
                var artists = conn.Query<Artist>("SELECT * FROM Artist");
                Assert.That(artists.Count(), Is.EqualTo(275));
            }

        }

        [Test]
        public void BasicPaging()
        {
            //Nope
        }
    }
}
