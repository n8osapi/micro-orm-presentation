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
    }
}
