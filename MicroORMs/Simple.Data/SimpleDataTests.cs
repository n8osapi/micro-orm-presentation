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
            dynamic db = Database.Open();
            dynamic artists = db.Artist.All();
            foreach (dynamic artist in artists) { };

            Assert.That(artists.Count(), Is.GreaterThan(298));
        }

        [Test]
        public void BasicPaging()
        {
            dynamic db = Database.Open();
            dynamic artistsPage1 = db.Artist.All().Skip(0).Take(5);
            dynamic artistsPage2 = db.Artist.All().Skip(5).Take(5);

            foreach (dynamic artist in artistsPage1) { };
            foreach (dynamic artist in artistsPage2) { };

            var page1 = artistsPage1.ToList();

            Assert.That(artistsPage1.ToList().Count, Is.EqualTo(5));
            Assert.That(artistsPage2.ToList().Count, Is.EqualTo(5));
            Assert.That(artistsPage1.ToList()[0].ArtistId, Is.EqualTo(1));
            Assert.That(artistsPage2.ToList()[0].ArtistId, Is.EqualTo(6));
        }
    }
}
