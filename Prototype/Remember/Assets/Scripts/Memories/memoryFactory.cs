using UnityEngine;
using System.Collections;

public enum ID
{
    spawnMemory,
}

public class memoryFactory : MonoBehaviour
{
	public static Memory makeMemory(ID id, Texture memories)
    {
        switch (id)
        {
            case ID.spawnMemory:
                return new Memory()
                {
                    Id = id,
                    Memories = Resources.Load<Texture>("Resources/Memories/Spawn"),
                    Sound = Resources.Load<AudioClip>("Resources/Sounds/SpawnSound")
                };                
        }
        return null;
    }
}
