using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net.Http;
//using Firebase.Database;
//using Firebase.Database.Query;
//using Firebase.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class InitilaizingDataBase : MonoBehaviour
{
    public static HttpClient client = new HttpClient();
    public static string DATABASE_URL = "https://smart-seety.firebaseio.com/";
    private string API_KEY = "AIzaSyDkARvlmhW0KnVdScei0TmlwMsafiqTxjo";
    //public static FirebaseClient firebaseClient;

    private static List<Device> devices;
    private bool init = true;

    // Start is called before the first frame update
    async void Start()
    {

    }

    // Update is called once per frame
    async void Update()
    {
        if (init)
        {
            devices = GameObject.FindObjectsOfType<Device>().ToList();
            var devicesData = devices.ToDictionary(device => device.gameObject.name, device => new { name = device.gameObject.name, sensors = device.sensors, status = device.GetStatus() });

            if (client != null)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(DATABASE_URL + "devices/.json", devicesData);

                init = false;
            }
        }
    }
}
