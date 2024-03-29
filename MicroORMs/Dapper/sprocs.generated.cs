﻿//-----------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool at 02/27/2012 19:31:46
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ETNUG.Sample.MicroORMs
{
	public static class ChinookDB
	{
		
		public static IEnumerable<spGetAlbumsWithArtist> spGetAlbumsWithArtist(string Artist)
		{
			using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				conn.Open();
				return conn.Query<spGetAlbumsWithArtist>("spGetAlbumsWithArtist", new {Artist=Artist } , commandType: CommandType.StoredProcedure);
			}
		}
		
		public static IEnumerable<spGetAlbumsWithArtistsStartingWith> spGetAlbumsWithArtistsStartingWith(string StartsWith)
		{
			using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				conn.Open();
				return conn.Query<spGetAlbumsWithArtistsStartingWith>("spGetAlbumsWithArtistsStartingWith", new {StartsWith=StartsWith } , commandType: CommandType.StoredProcedure);
			}
		}
		
		public static IEnumerable<spGetArtistCounts> spGetArtistCounts()
		{
			using (IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
			{
				conn.Open();
				return conn.Query<spGetArtistCounts>("spGetArtistCounts", commandType: CommandType.StoredProcedure);
			}
		}
	}


    public partial class spGetAlbumsWithArtist
    {
        public System.Int32 AlbumId { get; set; }
        public System.String Title { get; set; }
        public System.Int32 ArtistId { get; set; }
        public System.Int32 ArtistId1 { get; set; }
        public System.String Name { get; set; }
    }

    public partial class spGetAlbumsWithArtistsStartingWith
    {
        public System.Int32 AlbumId { get; set; }
        public System.String Title { get; set; }
        public System.Int32 ArtistId { get; set; }
        public System.Int32 ArtistId1 { get; set; }
        public System.String Name { get; set; }
    }

    public partial class spGetArtistCounts
    {
        public System.String ArtistName { get; set; }
        public System.Int32 AlbumCount { get; set; }
    }
}
