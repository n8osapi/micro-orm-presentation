using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;

namespace Massive
{



    [TestFixture]
    public class MassiveTests
    {
        [Test]
        public void BasicSelect()
        {
            dynamic db = Massive.DB.Current;
            var artists = db.Query("SELECT * FROM Artist");
        }

        [Test]
        public void BasicPaging()
        {
            dynamic artists = new DynamicModel("Chinook", "Artist", "ArtistId");
            dynamic artistsPage2 = artists.Paged(currentPage: 2, pageSize: 5);
            foreach( dynamic item in artistsPage2.Items) {};
        }
    }
}
