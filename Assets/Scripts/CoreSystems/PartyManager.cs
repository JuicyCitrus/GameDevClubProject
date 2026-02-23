using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance { get; private set; }

    public List<EntityDetails> partyMembers = new List<EntityDetails>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddPartyMember(EntityDetails newMember)
    {
        if (!partyMembers.Contains(newMember))
        {
            partyMembers.Add(newMember);
        }
    }
}
