using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class Torch : MonoBehaviour
{
    public int tilesBurnRange = 3;
    public float maxTorchLight = 100f;
    public float currentTorchLight;
    public float torchDecayRate = 1f;
    public Light2D torchLight;
    public AnimatedTile burningTile;
    public float burnDuration; 
    void Start()
    {
        currentTorchLight = maxTorchLight;
        UIController.instance.torchSlider.maxValue = maxTorchLight;
        UIController.instance.torchSlider.value = maxTorchLight;
    }

    // Update is called once per frame
    void Update()
    {
        currentTorchLight -= torchDecayRate * Time.deltaTime;
        currentTorchLight = Mathf.Clamp(currentTorchLight, 0, maxTorchLight);
        UIController.instance.torchSlider.value = currentTorchLight;
        torchLight.intensity = currentTorchLight / maxTorchLight;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            BurnTilesAhead();
        }
    }
    public void RefillTorch(float amount)
    {
        currentTorchLight += amount;
        if (currentTorchLight > maxTorchLight)
        {
            currentTorchLight = maxTorchLight;
        }
        UIController.instance.torchSlider.value = currentTorchLight;
    }
    public void UseTorch(float amount)
    {
        currentTorchLight -= amount;
        currentTorchLight = Mathf.Clamp(currentTorchLight, 0, maxTorchLight);
        UIController.instance.torchSlider.value = currentTorchLight / maxTorchLight;

        if (currentTorchLight <= 0)
        {
            currentTorchLight = 0;
        }
    }

    public void BurnTilesAhead()
    {
        if (currentTorchLight <= 0)
        {
            return;
        }

        Vector3 facingDirection = PlayerController.instance.lastMoveDirection;

        if (PlayerController.instance.currentRoom.roomBurningTile == null)
        {
            Debug.LogWarning("Tilemap component not found on Torch object.");
            return;
        }

        for (int i = 1; i <= tilesBurnRange; i++)
        {
            Vector3 worldPos = transform.position + facingDirection * i;
            Vector3Int cellPos = PlayerController.instance.currentRoom.roomBurningTile.WorldToCell(worldPos);

            PlayerController.instance.currentRoom.roomBurningTile.SetTile(cellPos, burningTile);
            ObjectPooler.instance.SpawnPooledObject(cellPos, Quaternion.identity);
            currentTorchLight = 0;
            // Start coroutine to reset the tile after 3 seconds
            StartCoroutine(ResetTileAfterDelay(PlayerController.instance.currentRoom.roomBurningTile, cellPos, burnDuration));
        }
    }

    private IEnumerator ResetTileAfterDelay(Tilemap tilemap, Vector3Int cellPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        tilemap.SetTile(cellPos, null);
    }
}
