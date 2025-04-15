using System.Text;

namespace TransportTasksTest.AMRModule
{
    public static class AMRCommands
    {
        private static HttpClient Client { get; set; } = new HttpClient();

        public static async void CreateBox()
        {
            var url = "http://10.35.16.40:10870/interfaces/api/amr/containerIn";

            // Initialize variables for the payload  
            var container_code = "ap009";
            var container_model_code = "box01";
            var container_position = "test-test-1";
            var request_id = $"FS-Smart-Hub-Test{DateTime.Now.ToString()}";

            // JSON payload to send  
            var payload = new
            {
                containerCode = container_code,
                containerModelCode = container_model_code,
                enterOrientation = "0",
                isNew = "true",
                position = container_position,
                requestId = request_id
            };

            // Serialize payload to JSON  
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);

            // Send POST request  
            var response = await Client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);           
        }

        internal static async void DeleteBox()
        {
            var url = "http://10.35.16.40:10870/interfaces/api/amr/containerOut";

            // Initialize variables for the payload  
            var container_code = "ap009";
            var request_id = $"FS-Smart-Hub-Test{DateTime.Now.ToString()}";

            // JSON payload to send  
            var payload = new
            {
                containerCode = container_code,
                isDelete = true,
                requestId = request_id
            };

            // Serialize payload to JSON  
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);

            // Send POST request  
            var response = await Client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);
           
        }

        public static async Task FillUpBox()
        {
            var url = "http://10.35.16.40:10870/interfaces/api/amr/updateContainer";

            // Initialize variables for the payload  
            var container_code = "ap009";
            var request_id = $"FS-Smart-Hub-Test{DateTime.Now.ToString()}";

            // JSON payload to send  
            var payload = new
            {
                containerCode = container_code,
                emptyStatus = "EMPTY",
                requestId = request_id
            };

            // Serialize payload to JSON  
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);

            // Send POST request  
            var response = await Client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            Console.ReadKey();
        }

        public static async Task StartMission()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var request_id = $"FS-Smart-Hub-Test{DateTime.Now.ToString()}";
            var mission_code = $"M{timestamp}";

            // Target positions
            var idle_node = "test-test-5";
            var pick_node = "test-test-1";
            var place_node = "test-test-3";

            // Target API endpoint
            var url = "http://10.35.16.40:10870/interfaces/api/amr/submitMission";

            // JSON payload to send
            var payload = new
            {
                orgId = "Factorio Solutions",
                requestId = request_id,
                missionCode = mission_code,
                missionType = "RACK_MOVE",
                robotType = "LIFT",
                priority = 1,
                idleNode = idle_node,
                lockRobotAfterFinish = false,
                missionData = new[]
                {
                    new
                    {
                        sequence = 1,
                        position = pick_node,
                        type = "NODE_POINT",
                        putDown = false,
                        passStrategy = "AUTO",
                        waitingMillis = 0
                    },
                    new
                    {
                        sequence = 2,
                        position = place_node,
                        type = "NODE_POINT",
                        putDown = true,
                        passStrategy = "AUTO",
                        waitingMillis = 0
                    }
                }
            };

            // Serialize payload to JSON
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);

            // Send POST request
            var response = await Client.PostAsync(url, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            Console.ReadKey();
        }
    }
}
