using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This templated class gives the ability to define a factory container
/// for other class instances.  You can register and retrieve those class
/// instances using this class.
///
/// In particular this is useful for extending the library without modifying
/// source code. For example you can register your own classes, races, monster,
/// and items using this class.
/// </summary>
public class ObjectFactory<KeyType, PrototypeType>
{
    private static Dictionary<KeyType, PrototypeType> _instances = new Dictionary<KeyType, PrototypeType>();


    /// <summary>
    /// This static function registers a class instance, pairing it to the ID
    /// passed.  Users can then get this class instance using simply by calling
    /// Clone and passing the ID that the instance was registered with.
    /// </summary>
    /// <param name="key">The unique ID of the object being registered.</param>
    /// <param name="obj">The class object to register.</param>
    public static void Register(KeyType key, PrototypeType obj)
    {
        if (!_instances.ContainsKey(key))
            _instances.Add(key, obj);
    }

  
    /// <summary>
    /// This static function unregisters a class instance using the ID passed.
    /// </summary>
    /// <param name="key">The unique ID of the object being registered.</param>
    /// <param name="obj">The class object to register.</param>
    public static void Unregister(KeyType key)
    {
       if (!_instances.ContainsKey(key))
          _instances.Remove(key);
    }

  
    /// <summary>
    /// This static function unregisters all class instances.
    /// </summary>
    public static void UnregisterAll()
    {
       _instances.Clear();
    }
  
  
    /// <summary>
    /// Clones a class instance.
    ///
    /// This static function clones a previously registered class instance.
    /// Class instances must first be registered before they can be retrieved.
    /// </summary>
    /// <param name="key">The unique ID of the object being retrieved.</param>
    /// <returns></returns>
    public static PrototypeType Clone(KeyType key)
    {
        PrototypeType obj = _instances[key];
        PrototypeType cloned_obj = (PrototypeType)obj.GetType().GetMethod("Clone").Invoke(obj, null);

        return cloned_obj;
    }
  
  
    /// <summary>
    /// Returns a class instance.
    ///
    /// This static function returns a previously registered class instance.
    /// Class instances must first be registered before they can be retrieved.
    /// </summary>
    /// <param name="key">The unique ID of the object being retrieved.</param>
    /// <returns></returns>
    public static PrototypeType Get(KeyType key)
    {
       if (_instances.ContainsKey(key))
          return _instances[key];

       throw new Exception("Unable to find key " + key);
    }
}
