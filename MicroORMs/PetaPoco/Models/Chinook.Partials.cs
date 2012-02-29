using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Chinook
{
    public partial class Album
    {
        [ResultColumn]
        public Artist Artist { get; set; }
    }

    public partial class Artist
    {
        [ResultColumn]
        public List<Album> Albums { get; set; }
    }
}
