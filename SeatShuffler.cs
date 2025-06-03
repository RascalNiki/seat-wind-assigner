using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SeatShuffler : UdonSharpBehaviour
{
    public SeatTile[] tiles;

    private Vector3[] _originalPositions;

    void Start()
    {
        if (!Networking.IsOwner(gameObject))
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

        _originalPositions = new Vector3[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            _originalPositions[i] = tiles[i].transform.position;
        }
    }

    public override void Interact()
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }

        ShuffleTiles();
        ResetTiles();
    }

    private void ResetTiles()
    {
        foreach (SeatTile tile in tiles)
        {
            Networking.SetOwner(Networking.LocalPlayer, tile.gameObject);
            tile.HideTile();
        }
    }

    private void ShuffleTiles()
    {
        for (int i = tiles.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            Vector3 tempPos = _originalPositions[i];
            _originalPositions[i] = _originalPositions[j];
            _originalPositions[j] = tempPos;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            Networking.SetOwner(Networking.LocalPlayer, tiles[i].gameObject);
            tiles[i].SetPosition(_originalPositions[i]);
        }
    }
}
