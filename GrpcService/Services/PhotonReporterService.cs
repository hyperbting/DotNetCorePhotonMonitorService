using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService1
{
    public class PhotonReporterService : ReportPhotoRoom.ReportPhotoRoomBase
    {
        private readonly ILogger<PhotonReporterService> _logger;
        public PhotonReporterService(ILogger<PhotonReporterService> logger)
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
