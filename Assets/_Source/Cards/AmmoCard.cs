using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoCard", menuName = "2Gether/Cards/AmmoCard")]
public class AmmoCard : Card
{
    public GunType GunType;
    public int Amount;

    [SerializeField]
    public AudioClip UseSfx;

    public override void OnBeginUseCard(GameContext ctx)
    {
        GameManager.Instance.GetPlayerController().GetComponent<PlayerEquipment>()
            .AddAmmoToAmmoStorage(GunType, Amount);
        ctx.cardUi.DiscardCard(this);
    }

    public override void OnCardSubmitted(GameContext ctx)
    {
        AudioManager.Instance.PlaySFX(UseSfx);
    }

    public override void OnEndUseCard(GameContext ctx)
    {
    }


}
