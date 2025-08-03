using System;
using System.Collections;
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
        UIController.instance.torchSlider.fillAmount = 1f;
        PlayerController.instance.isNearlightSource = true;
    }
    void OnDisable()
    {
        PlayerController.instance.isNearlightSource = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentTorchLight > 0)
        {
            currentTorchLight -= torchDecayRate * Time.deltaTime;
            UIController.instance.torchAnimator.Play("torchLit");
            PlayerController.instance.isNearlightSource = true;
        }
        else
        {
            UIController.instance.torchAnimator.Play("torch-non-lit");
            gameObject.SetActive(false);
        }
       
        currentTorchLight = Mathf.Clamp(currentTorchLight, 0, maxTorchLight);
        UIController.instance.torchSlider.fillAmount = currentTorchLight / maxTorchLight;
        torchLight.intensity = currentTorchLight / maxTorchLight;

        if (Input.GetKeyDown(KeyCode.F))
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
        UIController.instance.torchSlider.fillAmount = currentTorchLight;
        UIController.instance.torchAnimator.Play("torchLit");
    }
    public void UseTorch(float amount)
    {
        currentTorchLight -= amount;
        currentTorchLight = Mathf.Clamp(currentTorchLight, 0, maxTorchLight);
        UIController.instance.torchSlider.fillAmount = currentTorchLight / maxTorchLight;

        if (currentTorchLight <= 0)
        {
            currentTorchLight = 0;
            UIController.instance.torchAnimator.Play("torch-non-lit");
        }
    }

    public void BurnTilesAhead()
    {
        if (currentTorchLight <= 0)
        {
            return;
        }
        
        PlayerController.instance.ResetVelocity();

        Vector3 facingDirection = PlayerController.instance.lastMoveDirection;

        if (PlayerController.instance.currentRoom.roomBurningTile == null)
        {
            Debug.LogWarning("Tilemap component not found on Torch object.");
            return;
        }

        UIController.instance.torchAnimator.Play("torch-non-lit");

        for (int i = 1; i <= tilesBurnRange; i++)
        {
            Vector3 worldPos = transform.position + facingDirection * i;
            Vector3Int cellPos = PlayerController.instance.currentRoom.roomBurningTile.WorldToCell(worldPos);
            Vector3 cellWorldCenter = PlayerController.instance.currentRoom.roomBurningTile.GetCellCenterWorld(cellPos);

            PlayerController.instance.currentRoom.roomBurningTile.SetTile(cellPos, burningTile);
            ObjectPooler.instance.SpawnPooledObject(cellWorldCenter, Quaternion.identity);
            // Start coroutine to reset the tile after 3 seconds
            GameManager.instance.ResetTile(PlayerController.instance.currentRoom.roomBurningTile, cellPos, burnDuration);
            currentTorchLight = 0;
        }
    }

    
}
