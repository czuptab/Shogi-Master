using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
	public event Action<Vector2Int> OnSelect;
	public event Action<Vector2Int> OnPromote;

	private void Update()
	{
		CheckForMouseInput();
	}

	private void CheckForMouseInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2Int boardPosition = GetBoardPositionFromMouse();
			OnSelect?.Invoke(boardPosition);
		}

		if (Input.GetMouseButtonDown(1))
		{
			Vector2Int boardPosition = GetBoardPositionFromMouse();
			OnPromote?.Invoke(boardPosition);
		}
	}

	private Vector2Int GetBoardPositionFromMouse()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ShogiPlane")))
		{
			int x = Mathf.FloorToInt(hit.point.x);
			int y = Mathf.FloorToInt(hit.point.z);
			return new Vector2Int(x, y);
		}

		return new Vector2Int(-1, -1); // Invalid position
	}
}