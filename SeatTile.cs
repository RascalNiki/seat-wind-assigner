using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SeatTile : UdonSharpBehaviour
{
    public Transform tileTransform;
    public AudioSource revealSound;

    private Quaternion _frontRotation = Quaternion.Euler(-90f, 180f, 0f);
    private Quaternion _backRotation = Quaternion.Euler(90f, 0f, 0f);

    [UdonSynced]
    private bool _isRevealed = true;

    [UdonSynced]
    private Vector3 _syncedPosition;

    public override void Interact()
    {
        if (!Networking.IsOwner(gameObject))
            Networking.SetOwner(Networking.LocalPlayer, gameObject);

        if (!_isRevealed)
        {
            RevealTile();
            revealSound.Play();
            RequestSerialization();
        }
    }

    public void RevealTile()
    {
        _isRevealed = true;
        SetFace(true);
        RequestSerialization();
    }

    public void HideTile()
    {
        _isRevealed = false;
        SetFace(false);
        RequestSerialization();
    }

    public void SetPosition(Vector3 pos)
    {
        _syncedPosition = pos;
        transform.position = pos;
        RequestSerialization();
    }

    private void SetFace(bool showFront)
    {
        tileTransform.localRotation = showFront ? _frontRotation : _backRotation;
    }

    public override void OnDeserialization()
    {
        transform.position = _syncedPosition;
        SetFace(_isRevealed);
    }
}