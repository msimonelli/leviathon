using System;
using System.Collections.Generic;
using UnityEngine;


public class ResourceFactory<PrototypeType> : GameObjectFactory<string, UnityEngine.Object>
{
  
  /// <summary>
  /// This static function registers a class instance, pairing it to the ID
  /// passed.  Users can then get this class instance using simply by calling
  /// Clone and passing the ID that the instance was registered with.
  /// </summary>
  /// <param name="key">The unique ID of the object being registered.</param>
  /// <param name="obj">The class object to register.</param>
  public static void Register(string key)
  {
     if (!_instances.ContainsKey(key))
        _instances.Add(key, Resources.Load(key));
  }
  
  
  /// <summary>
  /// This static function unregisters a class instance using the ID passed.
  /// </summary>
  /// <param name="key">The unique ID of the object being registered.</param>
  public new static void Unregister(string key)
  {
     if (!_instances.ContainsKey(key))
        _instances.Remove(key);
  }

  
  /// <summary>
  /// This static function unregisters a class instance using the ID passed.
  /// </summary>
  /// <param name="key">The unique ID of the object being registered.</param>
  public new static void UnregisterAll()
  {
     foreach(KeyValuePair<string, UnityEngine.Object> entry in _instances)
        Resources.UnloadAsset(entry.Value);
     
     _instances.Clear();
  }
  

  /// <summary>
  /// Clones a class instance.
  ///
  /// This static function returns a previously registered class instance.
  /// Class instances must first be registered before they can be retrieved.
  /// </summary>
  /// <param name="key">The unique ID of the object being retrieved.</param>
  /// <returns></returns>
  public new static PrototypeType Clone(string key)
  {
     throw new Exception("This function is not valid or this class");
  }


  /// <summary>
  /// Returns a class instance.
  ///
  /// This static function returns a previously registered class instance.
  /// Class instances must first be registered before they can be retrieved.
  /// </summary>
  /// <param name="key">The unique ID of the object being retrieved.</param>
  /// <returns></returns>
  public new static PrototypeType Get(string key)
  {
     // We dont' require pre-registering of resources, so let's verify that this
     // resource is actually registered now.
     Register(key);

     return (PrototypeType)Convert.ChangeType(_instances[key], typeof(PrototypeType));
  }
}
