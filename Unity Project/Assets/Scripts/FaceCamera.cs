using UnityEngine;
using System.Collections;


public class FaceCamera : MonoBehaviour
{
   
   public Camera m_Camera;
   public bool amActive =false;
   public bool autoInit =true;
   GameObject myContainer; 
   
   void Awake(){
      /*
      if (autoInit == true){
         m_Camera = Camera.main;
         amActive = true;
      }
      
      myContainer = new GameObject();
      myContainer.name = "GRP_"+transform.gameObject.name;
      myContainer.transform.position = transform.position;
      transform.parent = myContainer.transform;*/
   }
   
   
   void Update(){
      //if(amActive==true){
      //   myContainer.transform.LookAt(myContainer.transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
     // }
      transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
   }
}
