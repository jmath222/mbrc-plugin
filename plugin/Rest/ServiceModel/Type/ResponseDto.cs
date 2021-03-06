﻿using System.Runtime.Serialization;
using MusicBeePlugin.Rest.ServiceInterface;

namespace MusicBeePlugin.Rest.ServiceModel.Type
{
	[DataContract]
	public class ResponseBase
	{
	    [DataMember(Name = "code")]
	    public int Code { get; set; } = ApiCodes.Success;
	}

	[DataContract]
	public class LyricsResponse : ResponseBase
	{
		[DataMember(Name = "lyrics")]
		public string Lyrics { get; set; }
	}

	[DataContract]
	public class RatingResponse : ResponseBase
	{
		[DataMember(Name = "rating")]
		public float Rating { get; set; }
	}

	[DataContract]
	public class PositionResponse : ResponseBase
	{
		[DataMember(Name = "position")]
		public int Position { get; set; }

		[DataMember(Name = "duration")]
		public int Duration { get; set; }
	}
}
