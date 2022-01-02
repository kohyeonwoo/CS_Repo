using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRagdollSystem : MonoBehaviour
{
    [SerializeField]
    private Rigidbody spine;
    [SerializeField]
    private GameObject currentBody;
    [SerializeField]
    private GameObject deadBody;

    public void ChangeBody()
    {
        CopyCharacterTransformToRagdoll(currentBody.transform, deadBody.transform);

        currentBody.SetActive(false);
        deadBody.SetActive(true);

        spine.AddForce(new Vector3(0.0f, 0.0f, 150.0f), ForceMode.Impulse);
    }

    private void CopyCharacterTransformToRagdoll(Transform origin, Transform ragdoll)
    {
        for (int i = 0; i < origin.childCount; i++)
        {
            if (origin.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }
            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }
}
