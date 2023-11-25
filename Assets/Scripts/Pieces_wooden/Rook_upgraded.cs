using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook_upgraded : Shogiman {
    public override void Move(int x, int y, Vector3 tileCenter)
    {
        transform.DOMove(tileCenter, 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() => {
                BoardManager.Instance.CompleteMovement(this, x, y);
            });
    }
}
