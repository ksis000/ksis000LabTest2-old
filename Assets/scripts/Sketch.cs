using UnityEngine;
using Pathfinding.Serialization.JsonFx; //make sure you include this using
using System.Globalization; //make sure to use this too for the point x, y, z

public class Sketch : MonoBehaviour
{
    public TextMesh nText = new TextMesh();
    public GameObject Sphere;
    // Put your URL here
    public string _WebsiteURL = "http://ksis000lab2.azurewebsites.net/tables/WaterPollutionReading?zumo-api-version=2.0.0";

    void Start()
    {
        //Reguest.GET can be called passing in your ODATA url as a string in the form:
        //http://{Your Site Name}.azurewebsites.net/tables/{Your Table Name}?zumo-api-version=2.0.0
        //The response produce is a JSON string
        string jsonResponse = Request.GET(_WebsiteURL);

        //Just in case something went wrong with the request we check the reponse and exit if there is no response.
        if (string.IsNullOrEmpty(jsonResponse))
        {
            return;
        }

        //We can now deserialize into an array of objects - in this case the class we created. The deserializer is smart enough to instantiate all the classes and populate the variables based on column name.
        WaterPollutionReading[] pollution = JsonReader.Deserialize<WaterPollutionReading[]>(jsonResponse);

        int totalRows = pollution.Length;
        Debug.Log("total row" + totalRows);


        //We can now loop through the array of objects and access each object individually
        foreach (WaterPollutionReading row in pollution)
        {


            float x = float.Parse(row.X, CultureInfo.InvariantCulture.NumberFormat);
            float y = float.Parse(row.Y, CultureInfo.InvariantCulture.NumberFormat);
            float z = float.Parse(row.Z, CultureInfo.InvariantCulture.NumberFormat);
            string location = row.Location;
            int id = int.Parse(row.ReadingID);
            //Color colour = Color.white;


            var newSphere = (GameObject)Instantiate(Sphere, new Vector3(x, y, z), Quaternion.identity);
            newSphere.name = id.ToString();
            var children = newSphere.GetComponentsInChildren<TextMesh>();
            newSphere.GetComponentInChildren<TextMesh>().text = location;
        }
    }
          

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Material white = Resources.Load("WhiteSmooth", typeof(Material)) as Material;
                string whiteName = white.ToString();

                string objName = hit.collider.gameObject.GetComponent<Renderer>().material.color.ToString();

                Debug.Log("Color " + objName);
                Debug.Log("res " + whiteName);
                Debug.Log("bool " + objName.Equals(whiteName));
                TextMesh x = GameObject.Find("x").GetComponent<TextMesh>();
                TextMesh y = GameObject.Find("y").GetComponent<TextMesh>();
                TextMesh z = GameObject.Find("z").GetComponent<TextMesh>();
                x.text = "X: " + hit.collider.gameObject.transform.position.x.ToString();
                y.text = "Y: " + hit.collider.gameObject.transform.position.y.ToString();
                z.text = "Z: " + hit.collider.gameObject.transform.position.z.ToString();


                if (hit.collider.gameObject.GetComponent<Renderer>().material.color == white.color)
                {
                    Material newMat = Resources.Load("TealSmooth", typeof(Material)) as Material;
                    hit.collider.gameObject.GetComponent<Renderer>().material = newMat;

                } else
                {

                    Material newMat = Resources.Load("WhiteSmooth", typeof(Material)) as Material;
                    hit.collider.gameObject.GetComponent<Renderer>().material = newMat;


                }

            }
        }
    }
}
