using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;


public class Map : MonoBehaviour
{
  protected bool[,] m_obstruction_matrix;
  protected float m_floor_height;
  
  protected class Tile
  {
     public int guid;
     public string material;
     public string texture;
     public string type;

     public Tile()
     {
        guid = 0;
        material = "";
        texture = "";
        type = "";
     }
  };


   void Start()
  {
  }


  void Update()
  {
  }
  

  public bool IsObstructed(int x, int y)
  {
     if (x < 0 || y < 0)
        return false;

     if (x >= m_obstruction_matrix.GetLength(0) || y >= m_obstruction_matrix.GetLength(1))
        return false;

     return m_obstruction_matrix[y, x];
  }


  public void LoadMap(string map_name)
  {
     Dictionary<Int32, Tile> map_tiles = new Dictionary<Int32, Tile>();

     if (map_name == "sample")
        LoadSampleMap();
     else
     {

        TextAsset map_file = ResourceFactory<TextAsset>.Get("Maps/" + map_name);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(map_file.text);

        // Read the map tag
        XmlNodeList nodelist = xmlDoc.SelectNodes("map");
        foreach (XmlNode node in nodelist)
        {
           int map_width = Convert.ToInt32(node.Attributes.GetNamedItem("width").Value);
           int map_height = Convert.ToInt32(node.Attributes.GetNamedItem("height").Value);

           m_obstruction_matrix = new bool[map_width, map_height];
           for (int x = 0; x < map_width; x++)
              for (int y = 0; y < map_height; y++)
                 m_obstruction_matrix[x,y] = false;

           // Find all tilsets in the map tag
           XmlNodeList tileset_list = node.SelectNodes("tileset");
           foreach (XmlNode tileset_node in tileset_list)
           {
              Int32 tileset_guid = Convert.ToInt32(tileset_node.Attributes.GetNamedItem("firstgid").Value);

              // Find all individual tiles in the map tag
              XmlNodeList tile_list = tileset_node.SelectNodes("tile");
              foreach (XmlNode tile_node in tile_list)
              {
                 Tile tile = new Tile();
                 tile.guid = Convert.ToInt32(tile_node.Attributes.GetNamedItem("id").Value);

                 // Find all individual tiles in the map tag
                 XmlNodeList properties_list = tile_node.SelectNodes("properties");
                 foreach (XmlNode properties_node in properties_list)
                 {
                    XmlNodeList property_list = properties_node.SelectNodes("property");
                    foreach (XmlNode property_node in property_list)
                    {
                       string name = property_node.Attributes.GetNamedItem("name").Value;
                       string value = property_node.Attributes.GetNamedItem("value").Value;

                       if (name.ToLower() == "material")
                          tile.material = value;
                       else if (name.ToLower() == "texture")
                          tile.texture = value;
                       else if (name.ToLower() == "type")
                          tile.type = value.ToLower();
                    }
                 }

                 map_tiles.Add(tile.guid + tileset_guid, tile);
              }
           }

           m_floor_height = 0.000f;

           // Find all layer tags in the map tag
           XmlNodeList layer_list = node.SelectNodes("layer");
           foreach (XmlNode layer_node in layer_list)
           {
              int width = Convert.ToInt32(layer_node.Attributes.GetNamedItem("width").Value);
              int height = Convert.ToInt32(layer_node.Attributes.GetNamedItem("height").Value);
              string data = layer_node.SelectSingleNode("data").InnerText;

              Int32[,] map_matrix = new Int32[width, height];
              
              string[] lines = data.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
              int matrix_x = 0;
              int matrix_y = 0;
              foreach (string line in lines)
              {
                 
                 string[] tiles = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                 foreach (string tile in tiles)
                 {
                    map_matrix[matrix_x, matrix_y] = Convert.ToInt32(tile);
                    matrix_y++;
                 }
                 
                 matrix_y = 0;
                 matrix_x++;
              }

              string type = "unknown";

              // Find all individual tiles in the map tag
              XmlNodeList properties_list = layer_node.SelectNodes("properties");
              foreach (XmlNode properties_node in properties_list)
              {
                 XmlNodeList property_list = properties_node.SelectNodes("property");
                 foreach (XmlNode property_node in property_list)
                 {
                    string name = property_node.Attributes.GetNamedItem("name").Value;
                    string value = property_node.Attributes.GetNamedItem("value").Value;

                    if (name.ToLower() == "type")
                       type = value.ToLower();
                 }
              }


              // Create all map objects
              for (int x = 0; x < map_matrix.GetLength(0); x++)
              {
                 for (int y = 0; y < map_matrix.GetLength(1); y++)
                 {
                    if (map_tiles.ContainsKey(map_matrix[x, y]))
                    {
                       if (map_tiles[map_matrix[x, y]].material != "" || map_tiles[map_matrix[x, y]].texture != "")
                       {
                          if (type == "terrain")
                             CreateTerrain(new Vector2(x, y), map_tiles[map_matrix[x, y]].material, map_tiles[map_matrix[x, y]].texture);
                          else if (type == "walls")
                          {
                             CreateWall(new Vector2(x, y), map_tiles[map_matrix[x, y]].material, map_tiles[map_matrix[x, y]].texture);
                             m_obstruction_matrix[x, y] = true;
                          }
                          else if (type == "objects")
                          {
                             if (map_tiles[map_matrix[x, y]].type == "door")
                             {
                                CreateDoor(new Vector2(x, y), map_tiles[map_matrix[x, y]].material, map_tiles[map_matrix[x, y]].texture);
                             }
                             else
                                CreateBillboard(new Vector2(x, y), map_tiles[map_matrix[x, y]].material, map_tiles[map_matrix[x, y]].texture);
                          }
                       }
                    }
                 }
              }

              // If this was terrain we need to move up next terrain layer
              if (type == "terrain")
                 m_floor_height += 0.001f;
           }
        }
     }
  }


  private void LoadSampleMap()
  {/*
     m_background_matrix = new byte[20, 20]  {
        { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, },
        { 1,3,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,2,1, },
        { 1,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1, },
        { 1,7,7,7,7,1,1,1,7,7,7,7,1,1,1,1,1,7,7,1, },
        { 1,7,7,7,7,1,4,1,7,7,7,7,1,7,7,7,1,7,7,1, },
        { 1,7,7,7,7,7,7,7,7,7,7,7,1,7,7,7,1,7,7,1, },
        { 1,7,7,7,7,7,7,7,7,7,3,7,1,7,1,7,1,7,7,1, },
        { 1,2,7,7,7,1,4,1,7,7,7,7,1,7,1,7,1,7,7,1, },
        { 1,7,7,7,7,1,1,1,7,7,7,7,7,7,1,7,1,7,7,1, },
        { 1,7,7,7,7,1,1,1,2,7,7,7,1,1,1,7,1,7,7,1, },
        { 1,7,7,3,7,7,7,7,7,7,7,7,7,7,7,7,1,7,7,1, },
        { 1,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,7,7,1, },
        { 1,7,7,7,7,7,7,3,7,7,7,7,1,1,1,1,1,7,7,1, },
        { 1,7,7,7,7,7,7,7,7,7,7,7,1,1,1,1,1,7,7,1, },
        { 1,7,7,7,7,7,7,7,7,7,7,7,1,1,1,1,1,7,7,1, },
        { 1,7,7,1,1,1,1,7,7,7,7,7,1,1,1,1,1,7,7,1, },
        { 1,7,7,1,1,1,1,7,7,7,2,7,7,7,7,7,7,7,7,1, },
        { 1,7,7,1,1,1,1,7,7,7,7,7,7,7,7,7,7,2,7,1, },
        { 1,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1, },
        { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }};*/ 
  }
  

  public void CreateDoor(Vector2 pos, string material_name, string texture_name)
  {
     // Facing west
     GameObject billboard1 = GameObject.CreatePrimitive(PrimitiveType.Quad);
     billboard1.transform.position = new Vector3(pos.x, 0.5f, pos.y - 0.501f);
     billboard1.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
     
     if (material_name != "")
		billboard1.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		billboard1.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);

     // Facing east
     GameObject billboard2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
     billboard2.transform.position = new Vector3(pos.x, 0.5f, pos.y + 0.501f);
     billboard2.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
     billboard2.transform.Rotate(0, 180, 0);

     if (material_name != "")
		billboard2.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		billboard2.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);

     // Facing south
     GameObject billboard3 = GameObject.CreatePrimitive(PrimitiveType.Quad);
     billboard3.transform.position = new Vector3(pos.x + 0.501f, 0.5f, pos.y);
     billboard3.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
     billboard3.transform.Rotate(0, -90, 0);

     if (material_name != "")
		billboard3.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		billboard3.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);

     // Facing north
     GameObject billboard4 = GameObject.CreatePrimitive(PrimitiveType.Quad);
     billboard4.transform.position = new Vector3(pos.x - 0.501f, 0.5f, pos.y);
     billboard4.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
     billboard4.transform.Rotate(0, 90, 0);
     
     if (material_name != "")
		billboard4.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		billboard4.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);
  }


  public void CreateWall(Vector2 pos, string material_name, string texture_name)
  {
     GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
     cube.transform.position = new Vector3(pos.x, 0.5f, pos.y);
     cube.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

     if (material_name != "")
		cube.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		cube.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);
  }
  
  
  private void CreateTerrain(Vector2 pos, string material_name, string texture_name)
  {
     GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
     floor.transform.position = new Vector3(pos.x, m_floor_height, pos.y);
     floor.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

     if (material_name != "")
		floor.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		floor.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);
  }


  private void CreateBillboard(Vector2 pos, string material_name, string texture_name)
  {
     GameObject billboard = GameObject.CreatePrimitive(PrimitiveType.Quad);
     billboard.transform.position = new Vector3(pos.x, 0.5f, pos.y);
     billboard.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

     if (material_name != "")
		billboard.GetComponent<Renderer>().material = ResourceFactory<Material>.Get("Materials/" + material_name);
     if (texture_name != "")
		billboard.GetComponent<Renderer>().material.mainTexture = ResourceFactory<Texture2D>.Get("Textures/" + texture_name);

     billboard.AddComponent<FaceCamera>();
  }
}
