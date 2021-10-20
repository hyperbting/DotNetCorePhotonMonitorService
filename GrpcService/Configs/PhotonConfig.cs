namespace PhotonRoomListGrpcService.Configs
{
    public class PhotonConfig
    {
        public const string Photon = "Photon";

        public string AppId { get; set; }
        public string[] Region { get; set; }

        public bool ShowOnConsole { get; set; }
    }
}