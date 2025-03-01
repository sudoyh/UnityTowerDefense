﻿using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float           maxHP;          // 최대 체력
    private float           currentHP;      // 현재 체력
    private bool            isDie = false;  // 적이 사망 상태이면 isDie를 true로 설정
    private Enemy           enemy;
    private SpriteRenderer  spriteRenderer;
    
    public  float MaxHP => maxHP;
    public  float CurrentHP => currentHP;
    
    private void Awake()
    {
        currentHP       = maxHP;            // 현재 체력을 최대 체력과 같게 설정
        enemy           = GetComponent<Enemy>();
        spriteRenderer  = GetComponent<SpriteRenderer>();
    }
    
    public void TakeDamage(float damage)
    {
        // Tip. 적의 체력이 damage 만큼 감소해서 죽을 상황일 때 여러 타워의 공격을 동시에 받으면
        // enemy.OnDie() 함수가 여러 번 실행될 수 있다.
    
        // 현재 적의 상태가 사망 상태이면 아래 코드를 실행하지 않는다.
        if ( isDie == true ) return;
    
        // 현재 체력을 damage만큼 감소
        currentHP -= damage;
    
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");
    
        // 체력이 0이하 = 적 캐릭터 사망
        if ( currentHP <= 0 )
        {
            isDie = true;
            // 적 캐릭터 사망
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }
    
    private IEnumerator HitAlphaAnimation()
    {
        // 현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;
    
        // 적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;
    
        // 0.05초 동안 대기
        yield return new WaitForSeconds(0.05f);
    
        // 적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}


/*
 * File : EnemyHP.cs
 * Desc
 *	: 적 캐릭터의 체력
 *	
 * Functions
 *	: TakeDamage() - 체력 감소
 *	: HitAlphaAnimation() - 투명도를 100% -> 40% -> 100%로 설정
 */