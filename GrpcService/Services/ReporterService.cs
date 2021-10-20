using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotonRoomListGrpcService
{
    /// <summary>
    /// Reporter with gRPC protocol
    /// </summary>
    public class ReporterService : ReportPhotonRoom.ReportPhotonRoomBase
    {
        private readonly ILogger<ReporterService> _logger;
        public ReporterService(ILogger<ReporterService> logger)
        {
            _logger = logger;
        }

        public override Task<RoomListReply> RequestRoomList(RoomListRequest request, ServerCallContext context)
        {
            return Task.FromResult(new RoomListReply
            {
                //Message = "Hello " + request.Name
            });
        }
    }
}
