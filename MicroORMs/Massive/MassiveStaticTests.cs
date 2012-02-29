using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;

namespace Massive
{

    public partial class Artists : DynamicModel
    {
        public Artists()
            : base("chinook", "Artist", "ArtistId")
        {
        }
    }

    [TestFixture]
    public class MassiveStaticTests
    {
        [Test]
        public void BasicSelect()
        {
            var table = new Artists();
            var artists = table.All(columns: "Name");
            var acdc = artists.Where(x => x.Name == "AC/DC");
            //var alanis = artists.Where(x => x.Name.StartsWith("Alanis"));
        }

        [Test]
        public void BasicPaging()
        {
            var table = new Artists();
            var artistsPage1 = table.Paged();
            //var artistsPage2 = table.Paged(orderBy: "name desc", currentPage: 2);
            var artistsPage2 = table.Paged(orderBy: "name desc", currentPage: 2);
            var artistsPage200 = table.Paged(currentPage: 200);
            foreach (dynamic item in artistsPage1.Items) { };
            foreach (dynamic item in artistsPage2.Items) { };
            foreach (dynamic item in artistsPage200.Items) { };

        }

        [Test]
        public void BasicInsert()
        {
            dynamic artists = new Artists();
            dynamic artist = artists.Insert(new { Name = "ArtistName" });

            Assert.That(artist.ID, Is.GreaterThan(0));
            Assert.AreEqual("ArtistName", artist.Name);
        }

        [Test]
        public void BasicUpdate()
        {
            var artists = new Artists();
            artists.Update( new { Name = "Not AC/DC" },1);
            artists.Update(new { Name = "AC/DC" }, 1);
        }

    }
}
