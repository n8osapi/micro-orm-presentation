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
using ETNUG.Sample.MicroORMs;

namespace Dapper
{
    [TestFixture]
    public class DapperTests
    {
        public class Artist
        {
            public int ArtistId { get; set; }
            public string Name { get; set; }

            public List<Album> Albums { get; set; }
        }

        public class Album
        {
            public int AlbumId { get; set; }
            public string Title { get; set; }
            public int ArtistId { get; set; }
        }

        public class AlbumResult
        {
            public int AlbumId { get; set; }
            public string Title { get; set; }
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

            //None Out of the box... but how do the other guys do it??? easy...
            var sql = "SELECT * FROM Artist";
            // SELECT ROW_NUMBER() OVER (ORDER BY ArtistId) AS Row, * FROM Artist
            var pagedsql = sql.Replace("SELECT *", "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ArtistId) AS Row, * ") + " ) AS Paged WHERE Row >= @Start AND Row <=@End";

            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                conn.Open();
                var artistsPage1 = conn.Query<Artist>(pagedsql, new { Start = 1, End = 5 });
                var artistsPage2 = conn.Query<Artist>(pagedsql, new { Start = 6, End = 10 });
                Assert.That(artistsPage1.Count(), Is.EqualTo(5));
                Assert.That(artistsPage1.First().ArtistId, Is.EqualTo(1));
                Assert.That(artistsPage2.Count(), Is.EqualTo(5));
                Assert.That(artistsPage2.First().ArtistId, Is.EqualTo(6));
            }

        }

        [Test]
        public void BasicCRUD()
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                conn.Open();
                var x = conn.Query<int>("INSERT INTO Artist (Name) VALUES (@Name); SELECT cast(@@Identity as int);", new { Name = "NewArtist" }).First();
                Assert.That(x, Is.GreaterThan(0));


                var artist = conn.Query<Artist>("INSERT INTO Artist (Name) VALUES (@Name); SELECT * from Artist WHERE ArtistId=SCOPE_IDENTITY();", new { Name = "NewArtistX" }).First();
                Assert.That(artist.ArtistId, Is.GreaterThan(0));
                Assert.That(artist.Name, Is.EqualTo("NewArtistX"));

            }
        }

        [Test]
        public void BasicSPROCs()
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                conn.Open();
                var albums = conn.Query<AlbumResult>("spGetAlbumsWithArtist", commandType: CommandType.StoredProcedure);
                Assert.That(albums.Count(), Is.EqualTo(347));


                var albumsFiltered = conn.Query<AlbumResult>("spGetAlbumsWithArtist", new { Artist = "AC/DC" }, commandType: CommandType.StoredProcedure);
                Assert.That(albumsFiltered.Count(), Is.EqualTo(2));

            }
        }

        [Test]
        public void ParameterizedSprocs()
        {
            using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@a", 11);
                p.Add("@b", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@c", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                conn.Execute("spFancyProc", p, commandType: CommandType.StoredProcedure);

                int b = p.Get<int>("@b");
                int c = p.Get<int>("@c");
            }
        }

#region "Perfect World Sprocs - Hidden From View Until 2/27/2012"







        [Test]
        public void PerfectWorldSprocs()
        {
            var acdc_albums = ChinookDB.spGetAlbumsWithArtist(Artist:"AC/DC");

            var all_albums = ChinookDB.spGetAlbumsWithArtist(null);

            var b_artist_albums = ChinookDB.spGetAlbumsWithArtistsStartingWith(StartsWith:"B");

            var artist_album_counts = ChinookDB.spGetArtistCounts();

            var results = ChinookDB.spGetAlbumsWithArtistsStartingWith(
        }

        
#endregion

    }
}
