using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;


namespace UAClient
{
    internal class Program
    {
        internal static Session? Session { get; set; }

        internal static Subscription? Subscription { get; set; }

        internal static SessionReconnectHandler? ReconnectHandler { get; set; }

        public static MonitoredItemNotificationEventHandler MonitoredItemNotification;



        static async Task Main(string[] args)
        {
            var app = new ApplicationInstance
            {
                ApplicationName = "UAClient",
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "TTClient"
            };

            // Load the application configuration.
            // 
            app.LoadApplicationConfiguration(false).Wait();

            var config = app.ApplicationConfiguration;
            config.CertificateValidator.CertificateValidation += CertificateValidator_CertificateValidation;

            // Updated to use the non-obsolete overload of CheckApplicationInstanceCertificates.  

            app.CheckApplicationInstanceCertificates(false, 12).Wait();

            //var serverUrl = "opc.tcp://10.35.16.18:4840";


            //callback creation
            MonitoredItemNotification = new MonitoredItemNotificationEventHandler(MonitoredItem_Notification);

            var serverUrl = "opc.tcp://FS-TS-P360:53530/OPCUA/SimulationServer";

            // Create the session.  
            var endpointDescription = CoreClientUtils.SelectEndpoint(config, serverUrl, false);
            var endpointConfiguration = EndpointConfiguration.Create(config);
            var endpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);

            ReconnectHandler = new SessionReconnectHandler(true, 10 * 1000);

            Session = await Opc.Ua.Client.Session.Create(
                config,
                endpoint,
                true,
                config.ApplicationName,
                60000,
                null,
                null);

            Session.KeepAlive += Session_KeepAlive;

            //Console.ReadKey();



            // With the corrected code:
            //var browseDescription = new BrowseDescription
            //{
            //    NodeId = new NodeId("ns=3;s=TEST"), //new NodeId(85), 
            //    BrowseDirection = BrowseDirection.Forward,
            //    IncludeSubtypes = true,
            //    NodeClassMask = (uint)NodeClass.Object | (uint)NodeClass.Variable,
            //    ResultMask = (uint)BrowseResultMask.All
            //};

            Browse("ns=3;i=1007");

            Monitor("ns=3;i=1001");


             Console.ReadKey();

            //Session.Dispose();

            while (false)
            {

            }



         }

        private static void Monitor(string nodeId)
        {
            if (Subscription == null)
            {
                Subscription = new Subscription(Session.DefaultSubscription);

                Subscription.PublishingEnabled = true;
                Subscription.PublishingInterval = 1000;
                Subscription.KeepAliveCount = 10;
                Subscription.LifetimeCount = 10;
                Subscription.MaxNotificationsPerPublish = 1000;
                Subscription.Priority = 100;

                Session.AddSubscription(Subscription);

                Subscription.Create();
            }

            MonitoredItem monitoredItem = new MonitoredItem(Subscription.DefaultItem);

            monitoredItem.StartNodeId = nodeId;
            monitoredItem.AttributeId = Attributes.Value;
            monitoredItem.DisplayName = $"TestItemWithId: {nodeId}";
            monitoredItem.MonitoringMode = MonitoringMode.Reporting;
            monitoredItem.SamplingInterval = 1000;
            monitoredItem.QueueSize = 0;
            monitoredItem.DiscardOldest = true;

            monitoredItem.Notification += MonitoredItemNotification;

            Subscription.AddItem(monitoredItem);
        }

        private static void MonitoredItem_Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Browse(string sourceId)
        {
            try
            {
                BrowseDescription nodeToBrowse1 = new BrowseDescription();

                nodeToBrowse1.NodeId = new NodeId(sourceId);
                nodeToBrowse1.BrowseDirection = BrowseDirection.Forward;
                nodeToBrowse1.ReferenceTypeId = ReferenceTypeIds.Aggregates;
                nodeToBrowse1.IncludeSubtypes = true;
                nodeToBrowse1.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable);
                nodeToBrowse1.ResultMask = (uint)BrowseResultMask.All;

                // find all nodes organized by the node.
                BrowseDescription nodeToBrowse2 = new BrowseDescription();

                nodeToBrowse2.NodeId = new NodeId(sourceId);
                nodeToBrowse2.BrowseDirection = BrowseDirection.Forward;
                nodeToBrowse2.ReferenceTypeId = ReferenceTypeIds.Organizes;
                nodeToBrowse2.IncludeSubtypes = true;
                nodeToBrowse2.NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable);
                nodeToBrowse2.ResultMask = (uint)BrowseResultMask.All;

                var x = Session.Browse(
                    null,
                    null,
                    0,
                    new BrowseDescriptionCollection { nodeToBrowse1, nodeToBrowse2 },
                    out var result,
                    out var dgInfos);


                var nodeToRead = new ReadValueId();
                nodeToRead.NodeId = new NodeId(sourceId); // (NodeId)result.First().References.First().NodeId;
                nodeToRead.AttributeId = 13;

                var y = Session.Read(null, 0, TimestampsToReturn.Neither,  new ReadValueIdCollection() { nodeToRead }, out var result2, out var dgInfos2);
            }
            catch (Exception)
            {

                
            }
        }

        private static void Session_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (ServiceResult.IsBad(e.Status))
            {
                ReconnectHandler?.BeginReconnect(session, 1000, Server_ReconnectComplete);
            }
        }

        private static void Server_ReconnectComplete(object? sender, EventArgs e)
        {
            if (ReconnectHandler.Session != null)
            {
                if (!ReferenceEquals(Session, ReconnectHandler.Session))
                {
                    var session = Session;
                    session.KeepAlive -= Session_KeepAlive;

                    Session = (Session?)ReconnectHandler.Session;
                    Session.KeepAlive += Session_KeepAlive;

                    Utils.SilentDispose(session);



                }
            }
        }

        private static void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            // Check if the certificate is trusted.
            e.Accept = true;
        }
    }
}
