using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;
using Simple.Data;

namespace Simple.Data
{
    [TestFixture]
    public class SimpleDataTests
    {
        [Test]
        public void BasicSelect()
        {
            var db = Database.Open();
            var artists = db.Artist.All();
            foreach (var artist in artists) { };
        }

        [Test]
        public void BasicPaging()
        {
            var db = Database.Open();
            var artistsPage1 = db.Artist.All().Skip(0).Take(5);
            var artistsPage2 = db.Artist.All().Skip(5).Take(5);
            foreach (var artist in artistsPage1) { };
            foreach (var artist in artistsPage2) { };
        }
    }
}
