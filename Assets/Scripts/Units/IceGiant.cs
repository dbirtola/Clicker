using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGiant : Enemy {

    float mptReduced = 0;

    protected override void Start()
    {
        base.Start();

        Player p = FindObjectOfType<Player>();
        mptReduced = p.GetComponent<PlayerStats>().GetTotalStatStruct().manaPerTap / 2;
        p.GetComponent<PlayerStats>().AddManaPerTap(-1 * mptReduced);
    }

    public override void CleanUp()
    {
        base.CleanUp();

        if(mptReduced == 0)
        {
            return;
        }
        Player p = FindObjectOfType<Player>();
        p.GetComponent<PlayerStats>().AddManaPerTap(mptReduced);
    }
}
