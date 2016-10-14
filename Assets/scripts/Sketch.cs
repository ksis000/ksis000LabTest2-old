using UnityEngine;
using Pathfinding.Serialization.JsonFx; //make sure you include this using
using System.Globalization;

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
        //totalRows= 5;
        //int totalDistance = 10;

        /*int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int e = 0;
        int f = 0;
        int g = 0;
        int h = 0;
        int i = 0;
        int j = 0;
        int k = 0;*/




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




            //Example of how to use the object
            //Debug.Log("The age is: " + cenotaph.dateofdeath);
            /*float perc = i / (float)totalCubes;
            i++;
            float x = perc * totalDistance;
            float y = 5.0f;
            float z = 0.0f;*/
            //GameObject newCube = (GameObject)Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);

            /*var children = newCube.GetComponentsInChildren<TextMesh>();
            foreach (TextMesh child in children)
            {
                if (child.tag == "second" && check == 0)
                {
                    child.text = cenotaph.cemetery;
                }
                newCube.GetComponent<myCubeScript>().setSize(.5f);
                newCube.GetComponent<myCubeScript>().rotateSpeed = -.25f;
                newCube.GetComponentInChildren<TextMesh>().text = cenotaph.dateofdeath;
                int age = int.Parse(cenotaph.ageatdeath);
                if (age <= 30)
                {
                    colour = Color.green;

                } else if (age >=31 && age <= 64)
                {
                    colour = Color.yellow;
                }
                else
                {
                    colour = Color.red;
                }

                newCube.GetComponent<Renderer>().material.color = colour;
            }*/
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
                Debug.Log("Color " + hit.collider.gameObject.GetComponent<Renderer>().material);
                Debug.Log("res " + white);
                Debug.Log("bool " + hit.collider.gameObject.GetComponent<Renderer>().materials[0].Equals(white));
                if (hit.collider.gameObject.GetComponent<Renderer>().materials[0].Equals(white))
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
