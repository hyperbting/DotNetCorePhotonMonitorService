using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using PhotonRoomListGrpcService.Interfaces.IStorages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhotonRoomListGrpcService
{
    /// <summary>
    /// Reporter with gRPC protocol
    /// </summary>
    public class ReporterService : ReportPhotonRoom.ReportPhotonRoomBase
    {
        private readonly ILogger<ReporterService> _logger;
        private readonly IRoomList photonRoomListStorage;
        public ReporterService(ILogger<ReporterService> logger, IRoomList roomListStorage)
        {
            _logger = logger;

            photonRoomListStorage = roomListStorage;
        }

        public override Task<RegionSetReply> RequestRegionSet(RegionSetRequest request, ServerCallContext context)
        {
            // set target
            photonRoomListStorage.TargetPhotonRegion = request.Region.ToString();

            var reply =  new RegionSetReply() {
                Region = request.Region,
                LastUpdated = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(photonRoomListStorage.LastUpdated),
            };

            return Task.FromResult(reply);
        }

        public override Task<RoomListReply> RequestRoomList(RoomListRequest request, ServerCallContext context)
        {
            //photonRoomListStorage
            var pr = photonRoomListStorage.GetAllCachedRoom();

            var resp = new RoomListReply
            {
                Region = pr.Region,
                LastUpdated = pr.LastUpdate,
            };

            foreach (var uc in pr.uCounts)
            {
                resp.RoomUserCount.Add(uc);
            }

            return Task.FromResult(resp);
        }

        //public override Task RequestRoomListSubscription(RoomListRequest request, IServerStreamWriter<RoomListReply> responseStream, ServerCallContext context)
        //{
        //    return base.RequestRoomListSubscription(request, responseStream, context);
        //}

        //public override Task RequestRoomListUpdateSubscription(RoomListRequest request, IServerStreamWriter<RoomListReply> responseStream, ServerCallContext context)
        //{
        //    return base.RequestRoomListUpdateSubscription(request, responseStream, context);
        //}
    }

    [Serializable]
    public class RegionInGameUserCount
    {
        [Newtonsoft.Json.JsonProperty("region")]
        public PhotonRegion Region { get; set; }

        [Newtonsoft.Json.JsonProperty("region_string")]
        public string RegionString { get; set; }

        [Newtonsoft.Json.JsonProperty("ingame_user_count")]
        public InGameUserCount[] uCounts;

        [Newtonsoft.Json.JsonProperty("last_update")]
        public Google.Protobuf.WellKnownTypes.Timestamp LastUpdate { get; set; }

        [Newtonsoft.Json.JsonProperty("current_timestamp")]
        public Google.Protobuf.WellKnownTypes.Timestamp CurrentTimestamp { get; set; }

        public override string ToString()
        {
            RegionString = Region.ToString().ToLower();

            return Newtonsoft.Json.JsonConvert.SerializeObject(
                this, 
                Newtonsoft.Json.Formatting.Indented
            );
        }

        #region Serialize CurrentTimestamp
        public void SetLastUpdate(DateTimeOffset lastUpdate)
        {
            this.LastUpdate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(lastUpdate);
        }

        private bool serializeCurrentTimestamp = false;
        public void SetShouldSerializeCurrentTimestamp(bool shouldSerializeCurrent)
        {
            this.CurrentTimestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(DateTimeOffset.Now);
            serializeCurrentTimestamp = shouldSerializeCurrent;
        }

        public bool ShouldSerializeCurrentTimestamp()
        {
            return serializeCurrentTimestamp;
        }
        #endregion

        #region Serialize RegionString/ Region
        private bool serializeRegionStringInstead = false;
        public void SetShouldSerializeRegionStringInstead(bool showStringInstead)
        {
            serializeRegionStringInstead = showStringInstead;
        }

        public bool ShouldSerializeRegion()
        {
            return !serializeRegionStringInstead;
        }

        public bool ShouldSerializeRegionString()
        {
            return serializeRegionStringInstead;
        }
        #endregion
    }
}
