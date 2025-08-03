using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("NPCS")]
    public NPC[] npcs;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public NPC FindNPC(string npcName)
    {
        foreach (NPC npc in npcs)
        {
            if (npc.characterData.characterName == npcName)
            {
                return npc;
            }
        }
        return null;
    }
    public void ResetTile(Tilemap tilemap, Vector3Int cellPos, float delay)
    {
        StartCoroutine(ResetTileAfterDelay(PlayerController.instance.currentRoom.roomBurningTile, cellPos, delay));
    }
    private IEnumerator ResetTileAfterDelay(Tilemap tilemap, Vector3Int cellPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        tilemap.SetTile(cellPos, null);
    }
}
