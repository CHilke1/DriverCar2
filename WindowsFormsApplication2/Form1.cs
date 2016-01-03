using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Json;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Device.Location;
using System.Diagnostics;
using System.IO;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        string API_Key = "AIzaSyCi1HBVbWSVvb1Z4ej8gyJ8fX_2ZBhJpjg";
        public Form1()
        {
            InitializeComponent();
            //https://maps.googleapis.com/maps/api/directions/json?origin=Toronto&destination=Montreal&key=YOUR_API_KEY      
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder URL = new StringBuilder("https://maps.googleapis.com/maps/api/directions/json?");
            //StringBuilder URL = new StringBuilder("https://maps.googleapis.com/maps/api/directions/xml?");
            string origin = textBox1.Text;
            string destination = textBox2.Text;
            URL.Append("origin=");
            URL.Append(origin);
            URL.Append("&destination=");
            URL.Append(destination);
            URL.Append("&key=");
            URL.Append(API_Key);
            WebClient downloader = new WebClient();
            string send = URL.ToString();
            var json = downloader.DownloadString(send);

            string carriageReturn = "\r\n";
            //WebClient client = new WebClient();
            //Stream stream = client.OpenRead(send);
            //StreamReader reader = new StreamReader(stream);
            StringBuilder t = new StringBuilder();
            JObject jObject = JObject.Parse(json);
            string summary = (string)jObject["routes"][0]["summary"];
            //MessageBox.Show(summary);
            label2.Text = summary;
            RootObject deserializedProduct = JsonConvert.DeserializeObject<RootObject>(json);
            IList<Route> routes = deserializedProduct.routes;
            foreach (Route r in routes)
            {
                IList<Leg> legs = r.legs;
                foreach (Leg l in legs)
                {
                    IList<Step> s = l.steps;
                    foreach (Step x in s)
                    {
                        StartLocation2 start_location = x.start_location;
                        EndLocation2 end_location = x.end_location;
                        Duration2 duration = x.duration;
                        Distance2 distance = x.distance;
                        string instructions = HttpUtility.HtmlDecode(x.html_instructions);                 
                        //MessageBox.Show(start_location.lat.ToString());
                        //MessageBox.Show(end_location.lat.ToString());
                        //MessageBox.Show(instructions);
                        t.Append(cleanDirections(instructions) + carriageReturn);
                        t.Append(distance.ToString() + carriageReturn);
                    }
                }
            }
            richTextBox1.Text = Convert.ToString(t);



            /*JArray legArray = (JArray)jObject["routes"][0]["legs"];
            JArray stepArray = (JArray)jObject["steps"];
            //var RootObjects = JsonConvert.DeserializeObject<List<RootObject>>(json);


            IList<Step> steps = legArray.ToObject<IList<Step>>();
            foreach (Step step in steps)
            {
                StartLocation2 start_location = step.start_location;
                EndLocation2 end_location = step.end_location;
                Duration2 duration = step.duration;
                Distance2 distance = step.distance;
                string instructions = HttpUtility.HtmlDecode(step.html_instructions);
                MessageBox.Show(start_location.lat.ToString());
                MessageBox.Show(end_location.lat.ToString());
                MessageBox.Show(instructions);
            }*/


            //string text = jObject["routes"][0]["legs"][0]["steps"][0]["duration"]["text"].ToString();

            //MessageBox.Show(text);

            /*RootObject root = new RootObject();
            root.routes = JsonConvert.DeserializeObject<List<Route>>(json);
            IList<Person> person = a.ToObject<IList<Person>>();
            JArray r = (JArray)jObject["routes"]["legs"];
            IList<Leg> routes = r.ToObject<IList<Leg>>();
            List<List<Step>> myDeserializedObjList = JsonConvert.DeserializeObject<List<List<Step>>>(json);*/
        }

        private string cleanDirections (string dirtext)
        {      
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
            string formattedDirection = rx.Replace(dirtext, "");
            return formattedDirection;           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GeoCoordinate g = new GeoCoordinate();
            double latitude = g.Latitude;
            double longitude = g.Longitude;
            //Geolocator g = new Geolocator();
            Address adrs = new Address();
            //adrs.Add = txtAddress.Text.ToString();
            adrs.Add = "425 E.Fremont Pl. Milwaukee, WI 53207";
            adrs.GeoCode();
            string myLatitude = adrs.Latitude;
            string myLongitude = adrs.Longitude;
        
            StringBuilder searchstring = new StringBuilder("https://www.google.com/maps/search/mechanics/@");
            //?q = Pizza & center = 37.759748,-122.427135

            searchstring.Append(latitude + ", ");
            searchstring.Append(longitude);
            searchstring.Append(",13z");
            Process myProcess = new Process();
            try
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = searchstring.ToString();
                myProcess.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Form2 frm = new Form2(searchstring.ToString());
            //frm.Show();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class GeocodedWaypoint
    {
        public string geocoder_status { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
        public bool? partial_match { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
        public override string ToString()
        {
            return text;
        }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class EndLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class StartLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Distance2
    {
        public string text { get; set; }
        public int value { get; set; }
        public override string ToString()
        {
            return text;
        }
    }

    public class Duration2
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class EndLocation2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Polyline
    {
        public string points { get; set; }
    }

    public class StartLocation2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Step
    {
        public Distance2 distance { get; set; }
        public Duration2 duration { get; set; }
        public EndLocation2 end_location { get; set; }
        public string html_instructions { get; set; }
        public Polyline polyline { get; set; }
        public StartLocation2 start_location { get; set; }
        public string travel_mode { get; set; }
        public string maneuver { get; set; }
    }

    public class Leg
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string end_address { get; set; }
        public EndLocation end_location { get; set; }
        public string start_address { get; set; }
        public StartLocation start_location { get; set; }
        public List<Step> steps { get; set; }
        public List<object> via_waypoint { get; set; }
    }

    public class OverviewPolyline
    {
        public string points { get; set; }
    }

    public class Route
    {
        public Bounds bounds { get; set; }
        public string copyrights { get; set; }
        public List<Leg> legs { get; set; }
        public OverviewPolyline overview_polyline { get; set; }
        public string summary { get; set; }
        public List<object> warnings { get; set; }
        public List<object> waypoint_order { get; set; }
    }

    public class RootObject
    {
        public List<GeocodedWaypoint> geocoded_waypoints { get; set; }
        public List<Route> routes { get; set; }
        public string status { get; set; }
    }
    public class Address
    {
        public Address()
        {
        }
        //Properties
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Add { get; set; }

        //The Geocoding here i.e getting the latt/long of address
        public void GeoCode()
        {
            //to Read the Stream
            StreamReader sr = null;

            //The Google Maps API Either return JSON or XML. We are using XML Here
            //Saving the url of the Google API 
            string url = String.Format("http://maps.googleapis.com/maps/api/geocode/xml?address=" +
            this.Add + "&sensor=false");

            //to Send the request to Web Client 
            WebClient wc = new WebClient();
            try
            {
                sr = new StreamReader(wc.OpenRead(url));
            }
            catch (Exception ex)
            {
                throw new Exception("The Error Occured" + ex.Message);
            }

            try
            {
                XmlTextReader xmlReader = new XmlTextReader(sr);
                bool latread = false;
                bool longread = false;

                while (xmlReader.Read())
                {
                    xmlReader.MoveToElement();
                    switch (xmlReader.Name)
                    {
                        case "lat":

                            if (!latread)
                            {
                                xmlReader.Read();
                                this.Latitude = xmlReader.Value.ToString();
                                latread = true;

                            }
                            break;
                        case "lng":
                            if (!longread)
                            {
                                xmlReader.Read();
                                this.Longitude = xmlReader.Value.ToString();
                                longread = true;
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An Error Occured" + ex.Message);
            }
        }
    }
}
