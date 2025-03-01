using UnityEngine;

public class PlayerGold : MonoBehaviour
{
	[SerializeField]
	private	int	currentGold = 1000;

	public int CurrentGold
	{
		set => currentGold = Mathf.Max(0, value+100);
		get => currentGold;
	}
}


/*
 * File : PlayerGold.cs
 * Desc
 *	: 플레이어의 소지 골드 정보
 *	
 */