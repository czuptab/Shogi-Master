using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop_upgraded : Shogiman {
    public override void Move(int x, int y, Vector3 tileCenter, float movementDuration)
    {
        transform.DOMove(tileCenter, movementDuration).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                BoardManager.Instance.CompleteMovement(this, x, y);
            });
    }
}
