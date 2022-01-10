using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject CombatCube;
    public GameObject PlayerStats;
    public GameObject EnemyStats;

    // get two pawns, P1 attacker, P2 defender
    public GameObject Pawn1;
    public GameObject Pawn2;

    //no use now, get the tile buff, the buff should be like ((at_buff,df_buff),(at_buff,df_buff)), a list of two lists, the first list for attacker and the second for defender,
    //for example accros river can be ((-0.5,0.0),(0.0,0.3)), meaning the attack of the attacker will get 50% nerf and defense for defender will get 30% buff.
    // Tile should also include combat width, but we can do that later
    public GameObject Tile;

    // organization, attack, defense, health, speed
    private double og1, at1, df1, hp1, sp1, og2, at2, df2, hp2, sp2;
    private double nog1, nhp1, nog2, nhp2, atb1, atb2, dfb1, dfb2;




    //generator
    void Awake()
    {
        PlayerStats = GameObject.Find("Player Stats");
        EnemyStats = GameObject.Find("Enemy Stats");
        Getstats();
        DoShit();
    }



    public void DoShit()
    {
        ResolveCombat();
        UpdateNewStats();
        DisplayOnPanel();
    }





    //
    public void Getstats()
    {

        //Pawn needs to change to get this work, hard coded for testing RN
        og1 = og2 = 10;
        at1 = at2 = 2;
        df1 = df2 = 1;
        hp1 = hp2 = 10;


        //no need for speed right now
        sp1 = sp2 = 0;


        //buffs now set to 0
        atb1 = atb2 = dfb1 = dfb2 = 0;

        
    }



    //go read the rule book if u dont understand this part
    public void ResolveCombat()
    {

        nhp2 = hp2 - (at1 * (1 + atb1) - df2 * (1 + dfb2));

        nhp1 = hp1 - (at2 * (1 + atb2) + df2 * (1 + dfb2) * 0.3 - df1 * (1 + dfb1));

        nog2 = og2 - 2 * (at1 * (1 + atb1) - df2 * (1 + dfb2));

        nog1 = og2 - 2 * (at2 * (1 + atb2) + df2 * (1 + dfb2) * 0.3 - df1 * (1 + dfb1));

    }



    public void DisplayOnPanel()
    {
        PlayerStats.GetComponentInChildren<UnityEngine.UI.Text>().text = 
            string.Format("  Attack: {0} -> {0}\n  Defense: {1} -> {1} \n  Organization: {2} -> {3} \n  Health: {4} -> {5}",at1,df1,og1,nog1,hp1,nhp1);

        EnemyStats.GetComponentInChildren<UnityEngine.UI.Text>().text =
            string.Format("  Attack: {0} -> {0}\n  Defense: {1} -> {1} \n  Organization: {2} -> {3} \n  Health: {4} -> {5}", at2, df2, og2, nog2, hp2, nhp2);

    }


    //Currently does nothing
    public void UpdateNewStats()
    {
        
    }


}
