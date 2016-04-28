using UnityEngine;
using System.Collections;

public enum memoryID
{
    spawnMemory,
    acquireLogs,
}

public class memoryFactory : MonoBehaviour
{
	public static Memory makeMemory(memoryID id)
    {
        switch (id)
        {
            case memoryID.spawnMemory:
                return new Memory()
                {
                    Id = id,
                    Memories = Resources.Load<Texture>("Memories/Spawn"),
                    Sound = Resources.Load<AudioClip>("Sounds/SpawnSound"),
                    Length = 5.0f
                };
            case memoryID.acquireLogs:
                return new Memory()
                {
                    Id = id,
                    Memories = Resources.Load<Texture>("Memories/AcquireLogs"),
                    Sound = Resources.Load<AudioClip>("Sounds/AcquireLogsSound"),
                    Length = 5.0f
                };       
        }
        return null;
    }
}
