using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
     
    private class PoolItem
    {
        public bool isActive; //해당 게임 오브젝트의 활성화/비활성화 정보
        public GameObject gameObject; // 실제로 있는 게임 오브젝트
    }

   // private int increaseCount = 5; //오브젝트 부족시 인스턴스로 추가 생성되는 오브젝트 갯수
    private int increaseCount = 1; //오브젝트 부족시 인스턴스로 추가 생성되는 오브젝트 갯수
    private int maxCount; //현재 리스트에 등록된 오브젝트 수
    private int activeCount; //현재 생성되어 있는 오브젝트 갯수 

    private GameObject poolObject; //풀링에서 관리하는 해당 오브젝트
    private List<PoolItem> poolItemList; //관리되는 모든 오브젝트에 대한 리스트

    public int MaxCount => maxCount; //외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 프로퍼티
    public int ActiveCount => activeCount;//외부에서 현재 활성화 되어 있는 오브젝트 개수 확인을 위한 프로퍼티

    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    
    //increaseCount 단위로 오브젝트를 생성한다
    public void InstantiateObjects()
    {
        //maxCount += increaseCount;
        maxCount += increaseCount;

        for(int i =0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    //현재 관리대상인 활성/비활성 모든 오브젝트를 삭제
    public void DestroyObjects()
    {
        if (poolItemList == null) { return; }

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    //PoolItemList에 저장된 오브젝트를 활성화해서 쓴다
    //현재 모든 오브젝트가 사용중이라면 InstantiateObject를 추가로 생성
    public GameObject ActivePoolItem()
    {
        if (poolItemList == null) { return null; }

        //현재 생성되어 관리되어지고 있는 모든 오브젝트의 개수들과, 현재 활성화 된 오브젝트 갯수를 비교
        //모든 오브젝트가 활성화 상태라면 새로운 오브젝트가 필요하다.

        if (maxCount == activeCount) { InstantiateObjects(); }

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }

        }
        return null;
    }

    //현재 사용이 완료된 오브젝트를 비활성화 상태로 설정
    public void DeactivePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) { return; }

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
                return;
            }        
        }
    }

    //게임에 사용중인 모든 오브젝트를 비활성화 상태로 설정
    public void DeactiveAllPoolItems()
    {
        if (poolItemList == null) { return; }

        int count = poolItemList.Count;

        for(int i =0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }
        activeCount = 0;
    } 
}
