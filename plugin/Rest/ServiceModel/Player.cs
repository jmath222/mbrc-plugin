﻿#region

using System.Runtime.Serialization;
using MusicBeePlugin.Rest.ServiceModel.Type;
using ServiceStack.ServiceHost;

#endregion

namespace MusicBeePlugin.Rest.ServiceModel
{
    [Route("/player/shuffle", "GET")]
    public class GetShuffleState : IReturn<StatusResponse>
    {
    }

    [Route("/player/shuffle", "PUT")]
    public class SetShuffleState : IReturn<SuccessStatusResponse>
    {
        public bool enabled { get; set; }
    }

    [Route("/player/shuffle/toggle", "PUT")]
    public class ToggleShuffleState : IReturn<SuccessStatusResponse> { }

    [Route("/player/scrobble", "GET")]
    public class GetScrobbleStatus : IReturn<StatusResponse>
    {
    }

    [Route("/player/scrobble", "PUT")]
    public class SetScrobbleStatus : IReturn<SuccessStatusResponse>
    {
        public bool enabled { get; set; }
    }

    [Route("/player/scrobble/toggle", "PUT")]
    public class ToggleScrobbleStatus : IReturn<SuccessStatusResponse> { }

    [Route("/player/repeat", "GET")]
    public class GetRepeatMode : IReturn<ValueResponse>
    {
    }

    [Route("/player/repeat", "PUT")]
    public class SetRepeatMode : IReturn<SuccessResponse>
    {
        public string mode { get; set; }
    }

    [Route("/player/mute", "GET")]
    public class GetMuteStatus : IReturn<StatusResponse>
    {
    }

    [Route("/player/mute", "PUT")]
    public class SetMuteStatus : IReturn<SuccessStatusResponse>
    {
        public bool enabled { get; set; }
    }

    [Route("/player/mute/toggle", "PUT")]
    public class ToggleMuteStatus : IReturn<SuccessStatusResponse> { }

    [Route("/player/volume", "GET")]
    public class GetVolume : IReturn<VolumeResponse>
    {
    }

    [Route("/player/volume", "PUT")]
    public class SetVolume : IReturn<SuccessResponse>
    {
        public int value { get; set; }
    }

    [Route("/player/autodj", "GET")]
    public class GetAutoDjStatus : IReturn<StatusResponse>
    {
    }

    [Route("/player/autodj", "PUT")]
    public class SetAutoDjStatus : IReturn<SuccessStatusResponse>
    {
        public bool enabled { get; set; }
    }

    [Route("/player/previous", "GET")]
    public class PlayPrevious : IReturn<SuccessResponse>
    {
    }

    [Route("/player/next", "GET")]
    public class PlayNext : IReturn<SuccessResponse>
    {
    }

    [Route("/player/play", "GET")]
    public class PlaybackStart : IReturn<SuccessResponse>
    {
    }

    [Route("/player/stop", "GET")]
    public class PlaybackStop : IReturn<SuccessResponse>
    {
    }

    [Route("/player/pause", "GET")]
    public class PlaybackPause : IReturn<SuccessResponse>
    {
    }

    [Route("/player/playpause", "PUT")]
    public class PlaybackPlayPause : IReturn<SuccessResponse> { }

    [Route("/player/status", "GET")]
    public class GetPlayerStatus : IReturn<PlayerStatus>
    {
    }

    [Route("/player/playstate", "GET")]
    public class GetPlayState : IReturn<ValueResponse>
    {
    }

    [Route("/player/changerepeat", "PUT")]
    public class ChangeRepeat : IReturn<ValueResponse> { }


    [DataContract]
    public class SuccessStatusResponse : SuccessResponse
    {
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }

    [DataContract]
    public class StatusResponse
    {
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }

    [DataContract]
    public class SuccessVolumeResponse : SuccessResponse
    {
        [DataMember(Name = "value")]
        public int Value { get; set; }
    }

    [DataContract]
    public class VolumeResponse
    {
        [DataMember(Name = "value")]
        public int Value { get; set; }
    }

    [DataContract]
    public class SuccessValueResponse : SuccessResponse
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class ValueResponse
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}