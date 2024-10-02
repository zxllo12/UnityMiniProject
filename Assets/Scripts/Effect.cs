using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Collect(player);
        }
    }

    private void Collect(Player player)
    {
        switch (type)
        {
            case Type.Coin:
                Debug.Log("Coin");
                //GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                Debug.Log("Life");
                //GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:
                Debug.Log("Mushroom");
                //player.Grow();
                break;

            case Type.Starpower:
                Debug.Log("Star");
                //player.Starpower();
                break;
        }

        Destroy(gameObject);
    }
}
