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
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                conn.Open();
                var artistsPage1 = conn.Query<Artist>("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ArtistId) AS Row, * FROM Artist ) AS Paged  WHERE Row >= @Start AND Row <=@End", new { Start = 1, End = 5 });
                var artistsPage2 = conn.Query<Artist>("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ArtistId) AS Row, * FROM Artist ) AS Paged  WHERE Row >= @Start AND Row <=@End", new { Start = 6, End = 10 });
                Assert.That(artistsPage1.Count(), Is.EqualTo(5));
                Assert.That(artistsPage1.First().ArtistId, Is.EqualTo(1));
                Assert.That(artistsPage2.Count(), Is.EqualTo(5));
                Assert.That(artistsPage2.First().ArtistId, Is.EqualTo(6));
            }
            
        }
    }
}
