using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.EventHubs;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Threading;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Linq.Expressions;

public class MonitoringClient : MonoBehaviour
{
    private List<EventData> eventData = new List<EventData>();

    private readonly static string s_eventHubsCompatibleEndpoint = "sb://iothub-ns-smartseety-1532430-27698b6c78.servicebus.windows.net/";
    private readonly static string s_eventHubsCompatiblePath = "smartseetyiothub";

    private readonly static string s_iotHubSasKey = "qR+zJ820H5LDW8uO5FR7j0EeXvGd1ZNBIZAYvkRDyRc=";
    private readonly static string s_iotHubSasKeyName = "iothubowner";
    private static EventHubClient s_eventHubClient;

    // Start is called before the first frame update
    async void Start()
    {
        var connectionString = new EventHubsConnectionStringBuilder(new Uri(s_eventHubsCompatibleEndpoint), s_eventHubsCompatiblePath, s_iotHubSasKeyName, s_iotHubSasKey);
        s_eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());
        
        var runtimeInfo = await s_eventHubClient.GetRuntimeInformationAsync();
        var d2cPartitions = runtimeInfo.PartitionIds;

        CancellationTokenSource cts = new CancellationTokenSource();

        var tasks = new List<Task>();
        foreach (string partition in d2cPartitions)
        {
            tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
        }
    }
    private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
    {
        var eventHubReceiver = s_eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.UtcNow));
        while (true)
        {
            if (ct.IsCancellationRequested) break;
            var events = await eventHubReceiver.ReceiveAsync(1);
            
            if (events == null) continue;

            eventData.AddRange(events);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Device originalDevice;
        Device copyDevice;
        List<EventData> newEventData = new List<EventData>(eventData);

        foreach (EventData eData in newEventData)
        {
            object deviceId;
            eData.SystemProperties.TryGetValue("iothub-connection-device-id", out deviceId);
            string deviceIdStr = deviceId.ToString();
            string data = Encoding.UTF8.GetString(eData.Body.Array);
            if (deviceIdStr.StartsWith("@") && ResourceLoader.devices.Where(d => d.name == deviceIdStr).Count() == 0)
            {
                string reference = "--" + string.Join("", deviceIdStr.Skip(1).TakeWhile(c => c != '-').ToArray()) + "--";
                Device referenceDevice = ResourceLoader.devices.Where(d => d.name == reference).FirstOrDefault();
                GameObject referenceGameObject = referenceDevice.gameObject;
                GameObject newGameObject = Instantiate<GameObject>(referenceGameObject, referenceGameObject.transform.parent);
                newGameObject.name = deviceIdStr;
                newGameObject.GetComponent<Device>().Start();
                foreach (Transform tr in newGameObject.GetComponentsInChildren<Transform>().Where(t => t.name != deviceIdStr && t.GetComponentInChildren<TextMesh>() != null).ToList())
                {
                    Destroy(tr.gameObject);
                }
                newGameObject.transform.localPosition += newGameObject.transform.forward * 10;
                ResourceLoader.devices.Add(newGameObject.GetComponent<Device>());
            }
            originalDevice = ResourceLoader.devices.Where(d => d.name == deviceIdStr).FirstOrDefault();
            copyDevice = JsonConvert.DeserializeObject<Device>(data.Replace("False", "false").Replace("True", "true"));

            originalDevice?.SetValues(copyDevice);

            eventData.Remove(eData);
        }

        eventData.Clear();
    }
}