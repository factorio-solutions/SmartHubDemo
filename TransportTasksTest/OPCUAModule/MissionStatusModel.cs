using Newtonsoft.Json;

namespace TransportTasksTest.OPCUAModule
{
    public class MissionStatusModel
    {
        [JsonProperty]
        public string OrgId { get; set; }

        [JsonProperty]
        public string MissionCode { get; set; }

        [JsonProperty]
        public string? ViewBoardType { get; set; }

        [JsonProperty]
        public string ContainerCode { get; set; }

        [JsonProperty]
        public string ContainerLocationCode { get; set; }

        [JsonProperty]
        public string ContainerOrientation { get; set; }

        [JsonProperty]
        public string CurrentPosition { get; set; }

        [JsonProperty]
        public string SlotCode { get; set; }

        [JsonProperty]
        public string MissionStatus { get; set; }

        [JsonProperty]
        public string RobotId { get; set; }

        [JsonProperty]
        public string RobotX { get; set; }

        [JsonProperty]
        public string RobotY { get; set; }

        [JsonProperty]
        public string Message { get; set; }

        [JsonProperty]
        public string MapCode { get; set; }

        [JsonProperty]
        public string FloorNumber { get; set; }

        [JsonProperty]
        public string? MissionData { get; set; }
    }
}
