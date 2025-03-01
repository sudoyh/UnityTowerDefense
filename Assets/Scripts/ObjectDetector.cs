﻿using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
	[SerializeField]
	private	TowerSpawner	towerSpawner;
	[SerializeField]
	private	TowerDataViewer	towerDataViewer;

	private	Camera			mainCamera;
	private	Ray				ray;
	private	RaycastHit		hit;
	private	Transform		hitTransform = null;			// 마우스 픽킹으로 선택한 오브젝트 임시 저장
	private	Transform		previousHitTransform = null;	// 마우스가 직전에 머물렀던 타일 정보 저장용

	private void Awake()
	{
		// "MainCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
		// GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 와 동일
		mainCamera = Camera.main;
	}

	private void Update()
	{
		// 마우스가 UI에 머물러 있을 때는 아래 코드가 실행되지 않도록 함
		if ( EventSystem.current.IsPointerOverGameObject() == true )
		{
			return;
		}

		// 타워 건설 버튼을 눌렀을 때
		if ( towerSpawner.IsOnTowerButton )
		{
			ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity) )
			{
				if ( hit.transform.CompareTag("Tile") )
				{
					// 현재 마우스가 위치하고 있는 타일 (색상 변경)
					if ( previousHitTransform == hit.transform )
					{
						hit.transform.GetComponent<Tile>().OnSelectedTile();
					}
					// 마우스가 TileWall에서 TileWall로 이동했을 때
					else
					{
						OnChangePreviousTileColor();
					}

					// 마우스가 위치하고 있던 hit.transform 정보를 previousHitTransform 변수에 저장해서
					// 마우스가 다른 타일을 선택하러 갔는지 판단하는데 사용
					previousHitTransform = hit.transform;
				}
			}
			else
			{
				// 마우스가 TileWall에서 TileWall이 아닌 빈 곳으로 이동했을 때
				OnChangePreviousTileColor();
			}
		}
		else
		{
			// 타워 건설을 취소했을 때
			OnChangePreviousTileColor();
		}

		// 마우스 왼쪽 버튼을 눌렀을 때
		if ( Input.GetMouseButtonDown(0) )
		{
			// 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
			// ray.origin : 광선의 시작위치(=카메라 위치)
			// ray.direction : 광선의 진행방향
			ray = mainCamera.ScreenPointToRay(Input.mousePosition);

			// 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
			// 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity) )
			{
				hitTransform = hit.transform;

				// 광선에 부딪힌 오브젝트의 태그가 "Tile"이면
				if ( hit.transform.CompareTag("Tile") )
				{
					// 타워를 생성하는 SpawnTower() 호출
					towerSpawner.SpawnTower(hit.transform);
					// 타일의 색상을 원래 색상으로 변경
					hit.transform.GetComponent<Tile>().OnColorReset();
				}
				// 타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 On
				else if ( hit.transform.CompareTag("Tower") )
				{
					towerDataViewer.OnPanel(hit.transform);
				}
			}
		}
		else if ( Input.GetMouseButtonUp(0) )
		{
			// 마우스를 눌렀을 때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
			if ( hitTransform == null || hitTransform.CompareTag("Tower") == false )
			{
				// 타워 정보 패널을 비활성화 한다
				towerDataViewer.OffPanel();
			}

			hitTransform = null;
		}
	}

	private void OnChangePreviousTileColor()
	{
		// 마우스가 바로 직전에 위치하고 있던 타일 (색상 리셋)
		if ( previousHitTransform != null )
		{
			previousHitTransform.GetComponent<Tile>().OnColorReset();
		}
	}
}


/*
 * File : ObjectDetector.cs
 * Desc
 *	: 마우스 클릭으로 오브젝트 선택
 *	
 */