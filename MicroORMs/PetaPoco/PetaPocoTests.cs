using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;
using Chinook;

namespace PetaPoco
{
    [TestFixture]
    public class PetaPocoTests
    {
        [Test]
        public void BasicSelect()
        {
            var db = Chinook.ChinookDB.GetInstance();
            var artists = db.Fetch<Artist>("SELECT * FROM Artist");
        }

        [Test]
        public void BasicPaging()
        {
            var db = Chinook.ChinookDB.GetInstance();
            var artistsPage1 = db.Fetch<Artist>(1, 5, "SELECT ArtistId, Name FROM Artist Order By ArtistId" );
            var artistsPage2 = db.Fetch<Artist>(2, 5, "SELECT ArtistId, Name FROM Artist Order By ArtistId");
        }

        [Test]
        public void MultiMapping()
        {
            //Add ResultColumn to Album Partial
            var db = Chinook.ChinookDB.GetInstance();
            var query = db.Fetch<Album, Artist>("select * from album al inner join artist ar on al.ArtistId = ar.ArtistId");
        }

        [Test]
        public void MultiMappingWithRelator()
        {
            //Add ResultColumn to Album Partial
            var db = Chinook.ChinookDB.GetInstance();
            var query = db.FetchOneToMany<Artist, Album>(x => x.ArtistId, "select * from artist ar inner join album al on al.ArtistId = ar.ArtistId");
        }


        [Test]
        public void BasicCRUD()
        {
            var db = new PetaPoco.Database("Chinook");
            var newID = db.Insert(new Artist
            {
                Name = "Woot"
            });


            var acdcnew = db.Update(new Artist { ArtistId = 1, Name = "AC/DC Blah" });
            var acdc = db.Update(new Artist { ArtistId = 1, Name = "AC/DC" });

        }

        [Test]
        public void BasicSproc()
        {
            var db = Chinook.ChinookDB.GetInstance();

            var acdc = db.Query<Album, Artist>("exec spGetAlbumsWithArtist @artist", new { artist = "AC/DC" });

            var result = db.Query<Album, Artist>("exec spGetAlbumsWithArtist");


        }

    }
}
