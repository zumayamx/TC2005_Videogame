using UnityEngine;
using UnityEngine.UI;

public class MagnifyingGlass : MonoBehaviour
{
    public Image centralImage; // The central UI Image to display the magnified texture
    private Sprite originalSprite; // The original sprite of the central image

    void Start()
    {
        if (centralImage != null)
        {
            originalSprite = centralImage.sprite;
            centralImage.enabled = false; // Start with the central image hidden
        }
        else
        {
            Debug.LogError("Central Image is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Vector2 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            Debug.Log("Right mouse button pressed. Casting ray...");

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit: " + hit.collider.name);

                MeshRenderer mr = hit.collider.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    Debug.Log("MeshRenderer found on: " + hit.collider.name);
                    Texture2D texture = mr.material.mainTexture as Texture2D;

                    if (texture != null)
                    {
                        centralImage.sprite = TextureToSprite(texture);
                        centralImage.enabled = true;
                    }
                    else
                    {
                        Debug.Log("No Texture2D found on: " + hit.collider.name);
                    }
                }
                else
                {
                    Debug.Log("No MeshRenderer found on: " + hit.collider.name);
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any objects.");
            }
        }

        if (Input.GetMouseButtonUp(1)) // Right-click release
        {
            Debug.Log("Right mouse button released.");
            if (centralImage != null)
            {
                centralImage.sprite = originalSprite;
                centralImage.enabled = false;
            }
            else
            {
                Debug.LogError("Central Image is not assigned.");
            }
        }
    }

    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
