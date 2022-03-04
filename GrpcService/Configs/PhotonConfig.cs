namespace PhotonRoomListGrpcService.Configs
{
    public class PhotonConfig
    {
        public const string Photon = "Photon";

        public bool TargetPhotonCloud { get; set; }

        public string AppId { get; set; }
        public string[] Region { get; set; }

        public string SpecificIP { get; set; }

        public bool ShowOnConsole { get; set; }
    }
}