using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;
using ServiceStack.OrmLite.SqlServer;
using System.Data;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using ServiceStack.DesignPatterns.Model;
using System.Data.SqlClient;

namespace ServiceStack.OrmLite
{
    public class Artist
    {
        [AutoIncrement]
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }

    public class AlbumResult
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public string Name { get; set; }
    }


    [TestFixture]
    public class ServiceStackOrmLiteTests
    {
        [Test]
        public void BasicSelect()
        {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString, SqlServerOrmLiteDialectProvider.Instance);
            using (IDbConnection conn = dbFactory.OpenDbConnection())
            using (IDbCommand cmd = conn.CreateCommand())
            {
                List<Artist> artists = cmd.Select<Artist>();
            }
        }

        [Test]
        public void BasicSelect2()
        {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString, SqlServerOrmLiteDialectProvider.Instance);
            var artists = dbFactory.Exec(cmd => cmd.Select<Artist>());
        }

        [Test]
        public void BasicPaging()
        {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString, SqlServerOrmLiteDialectProvider.Instance);
            SqlExpressionVisitor<Artist> ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Artist>();
            using (IDbConnection conn = dbFactory.OpenDbConnection())
            using (IDbCommand cmd = conn.CreateCommand())
            {
                ev.OrderBy(x => x.Name);
                ev.Where(x => x.Name.StartsWith("a"));
                ev.Limit(10, 10);
                List<Artist> artists = cmd.Select<Artist>(ev);
            }
        }

        [Test]
        public void BasicCRUD()
        {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString, SqlServerOrmLiteDialectProvider.Instance);
            using (IDbConnection conn = dbFactory.OpenDbConnection())
            {
                Artist acdc;
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    acdc = cmd.GetById<Artist>(1);
                }
                using (IDbCommand cmd = conn.CreateCommand())
                {

                    acdc.Name = "xxx";
                    cmd.Update<Artist>(acdc);
                }
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    acdc.Name = "AC/DC";
                    cmd.Update<Artist>(acdc);
                }
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    Artist a = new Artist { Name = "NewArtist" };
                    cmd.Insert<Artist>(a);
                }
            }

        }


        [Test]
        public void BasicSproc()
        {

            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString, SqlServerOrmLiteDialectProvider.Instance);
            var Artists = dbFactory.Exec(dbCmd =>
            {
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.Parameters.Add(new SqlParameter("@Artist", "AC/DC"));
                dbCmd.CommandText = "spGetAlbumsWithArtist";
                return dbCmd.ExecuteReader().ConvertToList<AlbumResult>();
            });
        }

        public class Address: IHasId<int>
        {
            [AutoIncrement,Alias("ShipperID")]
            public int Id { get; set; }

            [System.ComponentModel.DataAnnotations.DataType("NVarChar")] 
            public string Line1 { get; set; }

            [StringLength(100)]
            public string Line2 { get; set; }

            [StringLength(10), Required]
            public string ZipCode { get; set; }

            [StringLength(2), Required]
            public string State { get; set; }

            [StringLength(50), Required]
            public string City { get; set; }

            [StringLength(25), Required]
            public string Country { get; set; }
        }

        [Test]
        public void CodeFirst()
        {
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString, SqlServerOrmLiteDialectProvider.Instance);
            SqlExpressionVisitor<Artist> ev = OrmLiteConfig.DialectProvider.ExpressionVisitor<Artist>();
            using (IDbConnection conn = dbFactory.OpenDbConnection())
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.DropTable<Address>();
                cmd.CreateTable<Address>();
            }

        }
    }
}
